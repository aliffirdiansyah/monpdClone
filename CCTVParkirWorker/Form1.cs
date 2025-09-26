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
        ApiWorker _ApiWorker = new ApiWorker();
        private List<ParkirView> _parkirList;

        public int _INTERVAL_API;
        public string _URL;
        public string _USER;
        public string _PASS;
        public int _INTERVAL_DAY;

        private Dictionary<int, CancellationTokenSource> _taskTokens = new Dictionary<int, CancellationTokenSource>();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;

            _parkirList = GetDataParkirCctv();

            foreach (var item in _parkirList)
            {
                var idx = dataGridView1.Rows.Add(
                    item.Id,
                    item.NOP, //NOP
                    item.Nama, //Nama
                    item.Alamat, //Alamat
                    item.Uptb, //CCTVId
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

            _INTERVAL_API = 15;
            _URL = "http://202.146.133.26/grpc";
            _USER = "bapendasby";
            _PASS = "surabaya2025!!";
            _INTERVAL_DAY = 90;

            dataGridView1.CellClick += DataGridView1_CellClick;
            btnStartAll.Click += BtnStartAll_Click;
            btnStopAll.Click += BtnStopAll_Click;
        }
        private async void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Action")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var btnCell = (DataGridViewButtonCell)row.Cells["Action"];

                if (btnCell.Value.ToString() == "Start")
                {
                    btnCell.Value = "Stop";
                    row.Cells["Error"].Value = "";
                    row.Cells["Status"].Value = "";
                    row.Cells["Status"].Style.BackColor = Color.Green;

                    // mulai task
                    var cts = new CancellationTokenSource();
                    _taskTokens[e.RowIndex] = cts;

                    string id = row.Cells["Id"].Value?.ToString() ?? "";
                    var parkir = GetParkirById(id);

                    if (parkir == null)
                    {
                        row.Cells["Error"].Value = "Data parkir tidak ditemukan";
                        btnCell.Value = "Start";
                        row.Cells["Status"].Value = "";
                        row.Cells["Status"].Style.BackColor = Color.Red;
                        _taskTokens.Remove(e.RowIndex);
                        return;
                    }

                    // Jalankan task di thread background
                    _ = Task.Run(async () =>
                    {
                        await RunTaskCctv(parkir, cts.Token, row);
                    });
                }
                else // tombol Stop ditekan
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
        private async Task RunTaskCctv(ParkirView op, CancellationToken token, DataGridViewRow row)
        {
            try
            {
                while (!token.IsCancellationRequested) // loop terus
                {
                    UpdateLog(row, "Fetching...");
                    // logic
                    var bearer = await GenerateToken(row);
                    using (var client = new HttpClient())
                    {
                        // Bearer Token Auth
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearer);

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
                                limit = 1000,
                                offset = 0,
                                descending = true
                            }
                        };

                        var jsonBody = JsonSerializer.Serialize(body);
                        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                        UpdateLog(row, $"Getting Data {tglBackDate} s/d {DateTime.Now}");
                        HttpResponseMessage response = await client.PostAsync(_URL, content);

                        if (response.IsSuccessStatusCode)
                        {
                            UpdateLog(row, "Data Received, Processing...");

                            var rawResponse = await response.Content.ReadAsStringAsync();
                            var apiResponse = ConvertSseOutputJson(rawResponse);
                            var res = JsonSerializer.Deserialize<EventAll.EventAllResponse>(apiResponse);
                            if(res == null)
                            {
                                throw new Exception("Response dari API tidak valid");
                            }

                            await InsertToDb(op, res, row, token);

                            row.Cells["LastConnected"].Value = DateTime.Now.ToString();
                            UpdateLog(row, "Inserted");
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            UpdateLog(row,$"Token kadaluarsa, refresh token dan ulangi...");

                            // Refresh token hanya sekali (di attempt pertama gagal 403)
                            await GenerateToken(row);
                        }
                        else
                        {
                            UpdateLog(row, $"Error: {response.StatusCode}, Message: {response.Content}");
                        }
                    }

                    if (token.IsCancellationRequested)
                    break;


                    var nextRun = DateTime.Now.AddMinutes(_INTERVAL_API);
                    UpdateLog(row, $"[DONE] Next Run: {nextRun:dd/MM/yyyy HH:mm:ss}");
                    await Task.Delay(TimeSpan.FromMinutes(_INTERVAL_API), token);
                }
            }
            catch (TaskCanceledException)
            {
                UpdateLog(row, "Dihentikan manual ❌");
            }
            catch (Exception ex)
            {
                // ✅ Log error di UI
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        row.Cells["Error"].Value = ex.Message;
                        row.Cells["Status"].Style.BackColor = Color.Red;
                        row.Cells["Status"].Value = "Error";
                        var btnCell = (DataGridViewButtonCell)row.Cells["Action"];
                        btnCell.Value = "Start";
                    }));
                }
                else
                {
                    row.Cells["Error"].Value = ex.Message;
                    row.Cells["Status"].Style.BackColor = Color.Red;
                    row.Cells["Status"].Value = "Error";
                    var btnCell = (DataGridViewButtonCell)row.Cells["Action"];
                    btnCell.Value = "Start";
                }

                UpdateLog(row, "Error: " + ex.Message);

                return;
            }
        }
        public async Task InsertToDb(ParkirView op, EventAll.EventAllResponse data, DataGridViewRow row, CancellationToken token)
        {
            await using var context = DBClass.GetContext();

            int index = 0;
            var jmlData = data.Items.Count;
            foreach (var item in data.Items)
            {
                if (item.Body?.Details?.Count > 0)
                {

                    UpdateLog(row, $"Total Data {jmlData}");

                    foreach (var detail in item.Body.Details)
                    {
                        token.ThrowIfCancellationRequested();

                        var ar = detail.AutoRecognitionResult;
                        if (ar == null)
                            continue;

                        var id = item.Body.Guid;

                        // gunakan AnyAsync
                        bool dataExist = await context.TOpParkirCctvs
                            .AnyAsync(x => x.Id == id, token);

                        if (!dataExist)
                        {
                            var insert = new TOpParkirCctv
                            {
                                Id = id,
                                Nop = op.NOP,
                                CctvId = op.CCTVId,
                                NamaOp = op.Nama,
                                AlamatOp = op.Alamat,
                                WilayahPajak = op.Uptb,
                                WaktuMasuk = DateTime.ParseExact(
                                    ar.TimeBegin,
                                    "yyyyMMdd'T'HHmmss.ffffff",
                                    System.Globalization.CultureInfo.InvariantCulture
                                ),
                                JenisKend = (int)GetJenisKendaraan(ar.VehicleClass),
                                PlatNo = ar.Hypotheses != null && ar.Hypotheses.Count > 0
                                    ? ar.Hypotheses[0].PlateFull
                                    : "",
                                WaktuKeluar = DateTime.ParseExact(
                                    ar.TimeBegin,
                                    "yyyyMMdd'T'HHmmss.ffffff",
                                    System.Globalization.CultureInfo.InvariantCulture
                                ),
                                Direction = (int)GetDirection(ar.Direction),
                                Log = $"ID:{id},DIRECTION:{ar.Direction},VEHICLE_CLASS:{ar.VehicleClass ?? "-"},VEHICLE_BRAND:{ar.VehicleBrand ?? "-"},VEHICLE_MODEL:{ar.VehicleModel ?? "-"}"
                            };

                            // tambahkan async
                            await context.TOpParkirCctvs.AddAsync(insert, token);
                            await context.SaveChangesAsync(token);
                        }
                    }
                }

                index++;
                double persen = ((double)index / jmlData) * 100;
                UpdateLog(row, $"({persen:F2}%)");
            }
        }
        private async void BtnStartAll_Click(object sender, EventArgs e)
        {
            // Disable tombol start, enable tombol stop
            btnStartAll.Enabled = false;
            btnStopAll.Enabled = true;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Action"] is DataGridViewButtonCell btnCell)
                {
                    string actionText = btnCell.Value?.ToString() ?? "";

                    if (actionText == "Start")
                    {
                        int rowIndex = row.Index;
                        int colIndex = dataGridView1.Columns["Action"].Index;

                        await Task.Run(() =>
                        {
                            DataGridView1_CellClick(
                                dataGridView1,
                                new DataGridViewCellEventArgs(colIndex, rowIndex)
                            );
                        });

                        // kasih jeda sedikit biar tidak semua nembak API bersamaan
                        await Task.Delay(300);
                    }
                }
            }
        }
        private void BtnStopAll_Click(object sender, EventArgs e)
        {
            // Enable kembali tombol start, disable tombol stop
            btnStartAll.Enabled = true;
            btnStopAll.Enabled = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Action"] is DataGridViewButtonCell btnCell)
                {
                    string actionText = btnCell.Value?.ToString() ?? "";

                    if (actionText == "Stop")
                    {
                        int rowIndex = row.Index;
                        int colIndex = dataGridView1.Columns["Action"].Index;

                        DataGridView1_CellClick(
                            dataGridView1,
                            new DataGridViewCellEventArgs(colIndex, rowIndex)
                        );
                    }
                }
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
            return _parkirList.FirstOrDefault(q => q.Id == id);
        }
        private async Task<string> GenerateToken(DataGridViewRow row)
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

                UpdateLog(row, "Token Received");
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
        public EnumFactory.EJenisKendParkirCCTV GetJenisKendaraan(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "CAR":
                    return EnumFactory.EJenisKendParkirCCTV.Mobil;
                case "MOTORCYCLE":
                    return EnumFactory.EJenisKendParkirCCTV.Motor;
                case "TRUCK":
                    return EnumFactory.EJenisKendParkirCCTV.Truck;
                default:
                    return EnumFactory.EJenisKendParkirCCTV.Unknown;
            }
        }
        public EnumFactory.CctvParkirDirection GetDirection(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "INCOMING":
                    return EnumFactory.CctvParkirDirection.Incoming;
                case "OUTGOING":
                    return EnumFactory.CctvParkirDirection.Outgoing;
                default:
                    return EnumFactory.CctvParkirDirection.Unknown;
            }
        }
    }
}
