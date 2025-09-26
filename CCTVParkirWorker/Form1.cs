using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CCTVParkirWorker
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();
        ApiWorker _ApiWorker = new ApiWorker();

        private int _INTERVAL_API;
        private string _URL;
        private string _USER;
        private string _PASS;
        private int _INTERVAL_DAY;

        private Dictionary<int, CancellationTokenSource> _taskTokens = new Dictionary<int, CancellationTokenSource>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var parkirViews = GetDataParkirCctv();

            foreach (var item in parkirViews)
            {
                var idx = dataGridView1.Rows.Add(
                    item.Id,
                    item.NOP, //NOP
                    item.Nama, //Nama
                    item.Alamat, //Alamat
                    item.CCTVId, //CCTVId
                    item.AccessPoint, //AccessPoint
                    item.Mode, //Mode
                     "", //LastConnected
                     "Start", //Action
                     "Idle", //Status
                     "", //ErrMessage
                     "" //Log
                );
            }



            //_ApiWorker = new ApiWorker("http://202.146.133.26/grpc", "bapendasby", "surabaya2025!!",5,30);
            //_ApiWorker.StartWorkers(parkirViews);
        }

        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Action")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var btnCell = (DataGridViewButtonCell)row.Cells["Action"];

                if (btnCell.Value.ToString() == "Start")
                {
                    //ketika start
                    btnCell.Value = "Stop";
                    row.Cells["Error"].Value = "";
                    row.Cells["Status"].Value = "";
                    row.Cells["Status"].Style.BackColor = Color.Green;

                    // mulai task
                    var cts = new CancellationTokenSource();
                    _taskTokens[e.RowIndex] = cts;

                    //int id = Convert.ToInt32(row.Cells["Id"].Value);
                    //string recipient = row.Cells["Recipient"].Value.ToString();
                    //string subject = row.Cells["Subject"].Value.ToString();

                    string id = row.Cells["Id"].Value?.ToString() ?? "";

                    var parkir = GetParkirById(id);
                    if(parkir == null)
                    {
                        row.Cells["Error"].Value = "Data parkir tidak ditemukan";
                        btnCell.Value = "Start";
                        row.Cells["Status"].Value = "";
                        row.Cells["Status"].Style.BackColor = Color.Red;
                        _taskTokens.Remove(e.RowIndex);
                        return;
                    }

                    await RunTaskCctv(parkir, 3000, cts.Token, row);

                    // ketika stop
                    btnCell.Value = "Start";
                    row.Cells["Status"].Value = "";
                    row.Cells["Status"].Style.BackColor = Color.Red; 
                    _taskTokens.Remove(e.RowIndex);
                }
                else // Stop ditekan
                {
                    if (_taskTokens.ContainsKey(e.RowIndex))
                    {
                        _taskTokens[e.RowIndex].Cancel();
                        _taskTokens.Remove(e.RowIndex);
                    }

                    btnCell.Value = "Start";
                    row.Cells["Status"].Value = "";
                    row.Cells["Status"].Style.BackColor = Color.Red;
                }
            }
        }

        private async Task RunTaskCctv(ParkirView op, int intervalMs, CancellationToken token, DataGridViewRow row)
        {
            try
            {
                while (!token.IsCancellationRequested) // loop terus
                {
                    UpdateLog(row, "Fetching...");
                    // logic

                    _INTERVAL_API = 5;
                    _URL = "http://202.146.133.26/grpc";
                    _USER = "bapendasby";
                    _PASS = "surabaya2025!!";
                    _INTERVAL_DAY = 30;

                    var bearer = GenerateToken();
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
                            var apiResponse = ConvertSseOutputJson(rawResponse);
                            var res = JsonSerializer.Deserialize<EventAll.EventAllResponse>(apiResponse);
                            // proses insert to db


                            UpdateLog(row, "Inserted");

                        }
                        else
                        {
                            UpdateLog(row, $"Error: {response.StatusCode}, Message: {response.Content}");
                        }
                    }

                    if (token.IsCancellationRequested)
                    break;

                    
                    UpdateLog(row, "Finished");
                    await Task.Delay(TimeSpan.FromMinutes(_INTERVAL_API), token);
                }
            }
            catch (TaskCanceledException)
            {
                UpdateLog(row, "Dihentikan manual ❌");
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        row.Cells["Error"].Value = ex.Message;
                    }));
                }
                else
                {
                    row.Cells["Error"].Value = ex.Message;
                }

                UpdateLog(row, "Error: " + ex.Message);
            }
        }

        private void UpdateLog(DataGridViewRow row, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateLog(row, message)));
                return;
            }

            row.Cells["Log"].Value = message; // ✅ replace, bukan append
        }

        public List<ParkirView> GetDataParkirCctv()
        {
            var result = new List<ParkirView>();

            var _cont = DBClass.GetContext();
            var pl = _cont.MOpParkirCctvs
                .Include(x => x.MOpParkirCctvJasnita)
                .Where(x => x.Vendor == 1)
                .ToList();
            foreach (var item in pl)
            {
                foreach (var det in item.MOpParkirCctvJasnita)
                {
                    result.Add(new ParkirView()
                    {
                        Id = item.Nop + "-" + det.CctvId.ToString(),
                        NOP = item.Nop,
                        Nama = item.NamaOp,
                        Alamat = item.AlamatOp,
                        Uptb = item.WilayahPajak,
                        CCTVId = !string.IsNullOrEmpty(det.CctvId) ? det.CctvId : "",
                        AccessPoint = det.AccessPoint,
                        Mode = det.CctvMode == 1 ? "IN" : det.CctvMode == 2 ? "OUT" : "HYBRID",
                        Status = "Idle",
                        LastConnected = null,
                        Err = null
                    });
                }
            }

            return result;
        }

        public ParkirView? GetParkirById(string id)
        {
            var result = new ParkirView();

            result = GetDataParkirCctv().FirstOrDefault(q => q.Id == id);

            return result;
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

        public async Task InsertToDb(ParkirView op, EventAll.EventAllResponse data, CancellationToken token)
        {
            var context = DBClass.GetContext();
            foreach (var item in data.Items)
            {
                if(item.Body.Details.Count > 0)
                {
                    foreach (var detail in item.Body.Details)
                    {
                        var ar = detail.AutoRecognitionResult;
                        if (ar != null)
                        {

                            var id = item.Body.Guid;

                            var dataExist = context.TOpParkirCctvs.Any(x => x.Id == id);
                            if (!dataExist)
                            {
                                var insert = new TOpParkirCctv();

                                insert.Id = id;
                                insert.Nop = op.NOP;
                                insert.CctvId = op.CCTVId;
                                insert.NamaOp = op.Nama;
                                insert.AlamatOp = op.Alamat;
                                insert.WilayahPajak = op.Uptb;
                                insert.WaktuMasuk = DateTime.ParseExact(ar.TimeBegin,"yyyyMMdd'T'HHmmss.ffffff",System.Globalization.CultureInfo.InvariantCulture);
                                insert.JenisKend = (int)GetJenisKendaraan(ar.VehicleClass);
                                insert.PlatNo = ar.Hypotheses != null && ar.Hypotheses.Count > 0 ? ar.Hypotheses[0].PlateFull : "";
                                insert.WaktuKeluar = insert.WaktuMasuk;


                                context.TOpParkirCctvs.Add(insert);
                            }

                            //var res = new ViewModel.RekapResult();
                            //res.EventType = "AutoRecognitionResult";
                            //res.Name = item.Body.OriginExt.FriendlyName;
                            //res.Direction = ar.Direction;
                            //res.TimeBegin = ar.TimeBegin;
                            //res.TimeEnd = ar.TimeEnd;
                            //if (ar.Hypotheses != null && ar.Hypotheses.Count > 0)
                            //{
                            //    foreach (var w in ar.Hypotheses)
                            //    {
                            //        res.HypothesisOcrQuality = w.OcrQuality;
                            //        res.HypothesisPlateFull = w.PlateFull;
                            //        res.HypothesisPlateRectangleX = w.PlateRectangle.X;
                            //        res.HypothesisPlateRectangleY = w.PlateRectangle.Y;
                            //        res.HypothesisPlateRectangleW = w.PlateRectangle.W;
                            //        res.HypothesisPlateRectangleH = w.PlateRectangle.H;
                            //        res.HypothesisPlateRectangleIndex = w.PlateRectangle.Index;
                            //        res.HypothesisTimeBest = w.TimeBest;
                            //        res.HypothesisCountry = w.Country;
                            //        res.HypothesisPlateState = w.PlateState;
                            //    }
                            //}
                            //res.VehicleClass = ar.VehicleClass;
                            //res.VehicleColor = ar.VehicleColor;
                            //res.VehicleBrand = ar.VehicleBrand;
                            //res.VehicleModel = ar.VehicleModel;
                            //res.HeadlightsStatus = ar.HeadlightsStatus;
                            //res.VehicleSpeed = ar.VehicleSpeed;
                            //res.VehicleSpeedKmph = ar.VehicleSpeedKmph;
                            //res.PlateType = ar.PlateType;
                            //result.Add(res);
                        }
                    }
                }
            }   

            context.SaveChanges();
        }


        public EnumFactory.EJenisKendParkirCCTV GetJenisKendaraan(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "CAR":
                    return EnumFactory.EJenisKendParkirCCTV.Mobil;
                    break;
                case "MOTORCYCLE":
                    return EnumFactory.EJenisKendParkirCCTV.Motor;
                    break;
                case "TRUCK":
                    return EnumFactory.EJenisKendParkirCCTV.Truck;
                    break;
                default:
                    return EnumFactory.EJenisKendParkirCCTV.Unknown;
                    break;
            }
        }
    }
}
