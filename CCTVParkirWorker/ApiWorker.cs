using MonPDLib.EF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CCTVParkirWorker
{
    public class ApiWorker
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _workers = new();

        private int _INTERVAL_API;
        private string _URL;
        private string _USER;
        private string _PASS;
        private int _INTERVAL_DAY;
        public ApiWorker()
        {

        }
        public ApiWorker(string url, string user, string pass, int intervalAPI, int intervalDay)
        {
            _URL = url;
            _USER = user;
            _PASS = pass;
            _INTERVAL_API = intervalAPI;
            _INTERVAL_DAY = intervalDay;
        }
        public void StartWorkers(IEnumerable<ParkirView> data)
        {
            foreach (var op in data)
            {
                var cts = new CancellationTokenSource();
                _workers[op.Id] = cts;

                Task.Run(() => RunWorkerAsync(op, cts.Token));
            }
        }

        private async Task RunWorkerAsync(ParkirView op, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    op.Status = "Fetching...";

                    // ambil bearer
                    var bearer = GenerateToken();
                    // jupuk data api                    
                    using (var client = new HttpClient())
                    {

                        // Bearer Token Auth
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearer.Result);

                        // Headers
                        client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");
                        var tglBackDate = DateTime.Now.AddDays(-1 * _INTERVAL_DAY);
                        tglBackDate = new DateTime(tglBackDate.Year, tglBackDate.Month, tglBackDate.Day, 0, 0, 0);
                        var body = new
                        {
                            method = "axxonsoft.bl.events.EventHistoryService.ReadEvents",
                            data = new
                            {
                                range = new
                                {
                                    begin_time = tglBackDate.ToString("yyyyMMdd'T'HHmmss.'000'"),
                                    end_time = DateTime.Now.ToString("yyyyMMdd'T'HHmmss.'999'")
                                },
                                filters = new
                                {
                                    filters = new[]
                                    {
                                        new {
                                                type = "ET_DetectorEvent",
                                                subjects = op.AccessPoint
                                            }
                                    }
                                },                                
                                offset = 0,
                                descending = true
                            }
                        };

                        var jsonBody = JsonSerializer.Serialize(body);
                        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(_URL, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var rawResponse = await response.Content.ReadAsStringAsync();
                            var apiResponse = ApiWorker.ConvertSseOutputJson(rawResponse);
                            var res = JsonSerializer.Deserialize<EventAll.EventAllResponse>(apiResponse);
                            // proses insert to db
                            op.Status = "OK";
                            
                        }                        
                        else
                        {                            
                            op.Status = $"Error: {response.StatusCode}, Message: {response.Content}";
                        }
                    }
                    // end
                   
                }
                catch (Exception ex)
                {
                    op.Status = $"Error: {ex.Message}";
                }

                op.LastConnected = DateTime.Now;

                // tunggu sesuai interval
                await Task.Delay(TimeSpan.FromMinutes(_INTERVAL_API), token);
            }
        }

        public void StopAll()
        {
            foreach (var kvp in _workers)
                kvp.Value.Cancel();
            _workers.Clear();
        }

        private async Task<string> GenerateToken()
        {
            using (var client = new HttpClient())
            {
                // Basic Auth
                var byteArray = Encoding.ASCII.GetBytes($"{_USER}:{_PASS}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Headers
                client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                var body = new
                {
                    method = "axxonsoft.bl.auth.AuthenticationService.AuthenticateEx",
                    data = new
                    {
                        user_name = _USER,
                        password = _PASS
                    }
                };

                var jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_URL, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var obj = JsonObject.Parse(result);

                    return obj?["token_value"]?.ToString() ?? ""; // ambil token
                }
                else
                {
                    throw new Exception("Gagal ambil token. Status: " + response.StatusCode);
                }
            }
        }

        private static string ConvertSseOutputJson(string output)
        {
            try
            {
                var lines = output.Split('\n')
                            .Where(l => l.StartsWith("data:"))
                            .Select(l => l.Substring("data:".Length).Trim())
                            .ToList();
                var dtResponse = lines[0];

                return dtResponse;
            }
            catch (Exception ex)
            {
                return output;
            }
        }
    }
}
