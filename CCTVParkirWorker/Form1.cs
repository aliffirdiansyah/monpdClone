using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Drawing;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CCTVParkirWorker
{
    public partial class Form1 : Form
    {
        ApiWorker _ApiWorker = new ApiWorker();
        private List<ParkirView> _parkirList;
        private List<ParkirView> _parkirListTelkom;

        public int _INTERVAL_API;
        public string _URL;
        public string _USER;
        public string _PASS;
        public string _USER_TELKOM;
        public string _PASS_TELKOM;
        public string _TOKEN_TELKOM;
        public string _TOKEN;
        public int _INTERVAL_DAY;

        private Dictionary<int, CancellationTokenSource> _taskTokens = new Dictionary<int, CancellationTokenSource>();
        private CancellationTokenSource _telkomLogTokenSource;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView3.AllowUserToAddRows = false;

            _parkirList = GetOpParkirCctv();
            foreach (var item in _parkirList)
            {
                var idx = dataGridView1.Rows.Add(
                    item.No,
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

                var idxLog = dataGridView2.Rows.Add(
                    item.No,
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

            _parkirListTelkom = GetOpParkirCctvTelkom();
            foreach (var item in _parkirListTelkom)
            {
                var idx = dataGridView3.Rows.Add(
                    item.No,
                    item.Id,
                    item.NOP, //NOP
                    item.Nama, //Nama
                    item.Alamat, //Alamat
                    item.Uptb, //CCTVId
                    item.CCTVId, //CCTVId
                    item.Mode, //Mode
                     "", //LastConnected
                     "Start", //Action
                     "Idle", //Status
                     "", //ErrMessage
                     "" //Log
                );
            }

            _INTERVAL_API = 45;
            _URL = "http://202.146.133.26/grpc";

            _USER = "bapendasby";
            _PASS = "surabaya2025!!";

            _USER_TELKOM = "pemkot_surabaya_va";
            _PASS_TELKOM = "P3mk0tSuR4b4Ya";

            _INTERVAL_DAY = 90;

            dataGridView1.CellClick += DataGridView1_CellClick;
            btnStartAll.Click += BtnStartAll_Click;
            btnStopAll.Click += BtnStopAll_Click;

            dataGridView2.CellClick += DataGridView2_CellClick;
            btnStartAllLogJasnita.Click += BtnStartLogAll_Click;
            btnStopAllLogJasnita.Click += BtnStopLogAll_Click;

            dataGridView3.CellClick += DataGridView3_CellClick;
            btnStartAllTelkom.Click += BtnStartTelkomAll_Click;
            btnStopAllLogJasnita.Click += BtnStopTelkomAll_Click;

            btnStartAllLogTelkom.Click += BtnStartTelkomLogAll_Click;
            btnStopAllLogTelkom.Click += BtnStopTelkomLogAll_Click;
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

                    var context = DBClass.GetContext();
                    await context.Database.CloseConnectionAsync();
                }
            }
        }
        private async void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView2.Columns[e.ColumnIndex].Name == "ActionLog")
            {
                var row = dataGridView2.Rows[e.RowIndex];
                var btnCell = (DataGridViewButtonCell)row.Cells["ActionLog"];

                if (btnCell.Value.ToString() == "Start")
                {
                    btnCell.Value = "Stop";
                    row.Cells["ErrorLog"].Value = "";
                    row.Cells["StatusLog"].Value = "";
                    row.Cells["StatusLog"].Style.BackColor = Color.Green;

                    // mulai task
                    var cts = new CancellationTokenSource();
                    _taskTokens[e.RowIndex] = cts;

                    string id = row.Cells["IdLog"].Value?.ToString() ?? "";
                    var parkir = GetParkirById(id);

                    if (parkir == null)
                    {
                        row.Cells["ErrorLog"].Value = "Data parkir tidak ditemukan";
                        btnCell.Value = "Start";
                        row.Cells["StatusLog"].Value = "";
                        row.Cells["StatusLog"].Style.BackColor = Color.Red;
                        _taskTokens.Remove(e.RowIndex);
                        return;
                    }

                    // Jalankan task di thread background
                    _ = Task.Run(async () =>
                    {
                        await RunTaskCctvLog(parkir, cts.Token, row);
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
                    row.Cells["StatusLog"].Value = "";
                    row.Cells["StatusLog"].Style.BackColor = Color.Red;

                    var context = DBClass.GetContext();
                    await context.Database.CloseConnectionAsync();
                }
            }
        }
        private async void DataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView3.Columns[e.ColumnIndex].Name == "ActionTelkom")
            {
                var row = dataGridView3.Rows[e.RowIndex];
                var btnCell = (DataGridViewButtonCell)row.Cells["ActionTelkom"];

                if (btnCell.Value.ToString() == "Start")
                {
                    btnCell.Value = "Stop";
                    row.Cells["ErrorTelkom"].Value = "";
                    row.Cells["StatusTelkom"].Value = "";
                    row.Cells["StatusTelkom"].Style.BackColor = Color.Green;

                    // mulai task
                    var cts = new CancellationTokenSource();
                    _taskTokens[e.RowIndex] = cts;

                    string id = row.Cells["IdTelkom"].Value?.ToString() ?? "";
                    var parkir = GetParkirTelkomById(id);

                    if (parkir == null)
                    {
                        row.Cells["ErrorTelkom"].Value = "Data parkir tidak ditemukan";
                        btnCell.Value = "Start";
                        row.Cells["StatusTelkom"].Value = "";
                        row.Cells["StatusTelkom"].Style.BackColor = Color.Red;
                        _taskTokens.Remove(e.RowIndex);
                        return;
                    }

                    // Jalankan task di thread background
                    _ = Task.Run(async () =>
                    {
                        await RunTaskCctvTelkom(parkir, cts.Token, row);
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
                    row.Cells["StatusTelkom"].Value = "";
                    row.Cells["StatusTelkom"].Style.BackColor = Color.Red;

                    var context = DBClass.GetContext();
                    await context.Database.CloseConnectionAsync();
                }
            }
        }


        //DATAGRIDVIEW 1 - PARKIR CCTV
        private async Task RunTaskCctv(ParkirView op, CancellationToken token, DataGridViewRow row)
        {
            try
            {
                while (!token.IsCancellationRequested) // loop terus
                {
                    UpdateLog(row, "Fetching...");

                    var tglAwal = DateTime.Now.AddDays(-1 * _INTERVAL_DAY);
                    var tglAkhir = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

                    var dataCctvParkir = await GetDataParkirCctv(row, op, tglAwal, tglAkhir, token);
                    await InsertToDbCctv(op, dataCctvParkir, row, token);

                    row.Cells["LastConnected"].Value = DateTime.Now.ToString();
                    UpdateLog(row, "Inserted");

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

                UpdateLog(row, $"{op.AccessPoint} Error: " + ex.Message);

                return;
            }
        }
        public async Task<List<EventAll.EventAllResponse>> GetDataParkirCctv(DataGridViewRow row, ParkirView op, DateTime tglAwal, DateTime tglAkhir, CancellationToken token)
        {
            var result = new List<EventAll.EventAllResponse>();

            using (var client = new HttpClient())
            {
                await GenerateToken(row);

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN);
                client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                int totalData = 0;
                int limit = 20;
                int offset = 0;
                bool hasMore = true;
                int attempt = 0;
                const int maxRetry = 5;

                while (hasMore)
                {
                    try
                    {
                        // 🔹 cek cancel sebelum request
                        token.ThrowIfCancellationRequested();

                        var resEvent = new EventAll.EventAllResponse();

                        do
                        {
                            // 🔹 cek cancel di setiap iterasi retry
                            token.ThrowIfCancellationRequested();

                            var body = new
                            {
                                method = "axxonsoft.bl.events.EventHistoryService.ReadEvents",
                                data = new
                                {
                                    range = new
                                    {
                                        begin_time = tglAwal.ToString("yyyyMMdd'T'HHmmss.'000'"),
                                        end_time = tglAkhir.ToString("yyyyMMdd'T'HHmmss.'999'")
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
                                    limit = limit,
                                    offset = offset,
                                    descending = true
                                }
                            };

                            var jsonBody = JsonSerializer.Serialize(body);
                            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                            //UpdateLog(row, $"Getting Data {tglAwal} s/d {tglAkhir}");

                            // 🔹 kirim request dengan token
                            HttpResponseMessage response = await client.PostAsync(_URL, content, token);

                            if (response.IsSuccessStatusCode)
                            {
                                UpdateLog(row, "Data Received, Processing...");

                                var rawResponse = await response.Content.ReadAsStringAsync(token);
                                var apiResponse = ConvertSseOutputJson(rawResponse);
                                var res = JsonSerializer.Deserialize<EventAll.EventAllResponse>(apiResponse);

                                if (res == null)
                                    throw new Exception("Response dari API tidak valid");

                                resEvent = res;
                                break;
                            }
                            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                            {
                                UpdateLog(row, "Token kadaluarsa, refresh token dan ulangi...");
                                await GenerateToken(row);
                            }
                            else
                            {
                                UpdateLog(row, $"Error: {response.StatusCode}");
                            }

                            attempt++;
                            if (attempt >= maxRetry)
                            {
                                UpdateLog(row, $"Gagal setelah {maxRetry} percobaan pada offset {offset}");
                                break;
                            }

                        } while (attempt < maxRetry);

                        // 🔹 setelah selesai mencoba, cek cancel
                        token.ThrowIfCancellationRequested();

                        if (resEvent == null || resEvent.Items == null || resEvent.Items.Count == 0)
                        {
                            UpdateLog(row, $"Tidak ada data event di offset {offset}");
                            hasMore = false;
                        }
                        else
                        {
                            UpdateLog(row, $"Dapat {resEvent.Items.Count} event/items di offset {offset}");
                            result.Add(resEvent);

                            totalData += resEvent.Items.Count;
                            if (resEvent.Items.Count < limit)
                            {
                                UpdateLog(row, $"Data sudah habis, total {totalData} event.");
                                hasMore = false;
                            }
                            else
                            {
                                offset += limit;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateLog(row, $"Error: {ex.Message}");
                    }
                }
            }

            return result;
        }
        public async Task InsertToDbCctv(ParkirView op, List<EventAll.EventAllResponse> dataList, DataGridViewRow row, CancellationToken token)
        {
            await using var context = DBClass.GetContext();

            var toInsert = new List<TOpParkirCctv>();

            UpdateLog(row, $"Processing...");

            var allIds = dataList
                .SelectMany(d => d.Items.Select(i => i.Body.Guid))
                .Distinct()
                .ToList();

            var existingIds = await context.TOpParkirCctvs
                .Where(x => allIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(token);

            var existingSet = new HashSet<string>(existingIds);

            foreach (var data in dataList)
            {
                int index = 0;
                int jmlData = data.Items.Count;

                foreach (var item in data.Items)
                {
                    if (item.Body?.Details?.Count > 0)
                    {
                        foreach (var detail in item.Body.Details)
                        {
                            token.ThrowIfCancellationRequested();

                            var ar = detail.AutoRecognitionResult;
                            if (ar == null)
                                continue;

                            var id = item.Body.Guid;

                            bool dataExist = existingSet.Contains(id);
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
                                    WaktuMasuk = ParseFlexibleDate(ar.TimeBegin),
                                    WaktuKeluar = ParseFlexibleDate(ar.TimeBegin),
                                    JenisKend = (int)GetJenisKendaraan(ar.VehicleClass),
                                    PlatNo = ar.Hypotheses?.FirstOrDefault()?.PlateFull ?? "",
                                    Direction = (int)GetDirection(ar.Direction),
                                    Log = $"ID:{id},DIR:{ar.Direction},CLASS:{ar.VehicleClass ?? "-"},BRAND:{ar.VehicleBrand ?? "-"},MODEL:{ar.VehicleModel ?? "-"}",
                                    Vendor = (int)(EnumFactory.EVendorParkirCCTV.Jasnita)
                                };

                                toInsert.Add(insert);
                            }
                        }
                    }

                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    UpdateLog(row, $"({persen:F2}%)");
                }
            }

            try
            {
                await context.Database.OpenConnectionAsync(token);
                if (toInsert.Count > 0)
                {
                    toInsert = toInsert
                       .GroupBy(x => new { x.Id, x.Nop, x.CctvId })
                       .Select(g => g.First())
                       .ToList();


                    UpdateLog(row, $"Inserting {toInsert.Count} record(s).");
                    await context.TOpParkirCctvs.AddRangeAsync(toInsert, token);
                    await context.SaveChangesAsync(token);
                    await context.Database.CloseConnectionAsync();
                    UpdateLog(row, $"Inserted {toInsert.Count} record(s).");
                }
                else
                {
                    UpdateLog(row, "No new data to insert.");
                }
            }
            catch (Exception ex)
            {
                UpdateLog(row, $"Error Insert Db: {ex.Message}");
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
            // simpan semua sekaligus

        }


        //DATAGRIDVIEW 2 - LOG JASNITA
        private async Task RunTaskCctvLog(ParkirView op, CancellationToken token, DataGridViewRow row)
        {
            try
            {
                while (!token.IsCancellationRequested) // loop terus
                {
                    UpdateLogLog(row, "Fetching...");

                    var tglAwal = DateTime.Now.AddDays(-1 * _INTERVAL_DAY);
                    var tglAkhir = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

                    var data = await GetDataParkirCctvLog(row, op, tglAwal, tglAkhir, token);
                    await InsertToDbCctvLog(op, data, row, token);

                    row.Cells["LastConnectedLog"].Value = DateTime.Now.ToString();
                    UpdateLogLog(row, "Inserted");

                    if (token.IsCancellationRequested)
                        break;

                    var nextRun = DateTime.Now.AddMinutes(_INTERVAL_API);
                    UpdateLogLog(row, $"[DONE] Next Run: {nextRun:dd/MM/yyyy HH:mm:ss}");
                    await Task.Delay(TimeSpan.FromMinutes(_INTERVAL_API), token);
                }
            }
            catch (TaskCanceledException)
            {
                UpdateLogLog(row, "Dihentikan manual ❌");
            }
            catch (Exception ex)
            {
                // ✅ Log error di UI
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        row.Cells["ErrorLog"].Value = ex.Message;
                        row.Cells["StatusLog"].Style.BackColor = Color.Red;
                        row.Cells["StatusLog"].Value = "Error";
                        var btnCell = (DataGridViewButtonCell)row.Cells["ActionLog"];
                        btnCell.Value = "Start";
                    }));
                }
                else
                {
                    row.Cells["ErrorLog"].Value = ex.Message;
                    row.Cells["StatusLog"].Style.BackColor = Color.Red;
                    row.Cells["StatusLog"].Value = "Error";
                    var btnCell = (DataGridViewButtonCell)row.Cells["ActionLog"];
                    btnCell.Value = "Start";
                }

                UpdateLogLog(row, $"{op.AccessPoint} Error: " + ex.Message);

                return;
            }
        }
        public async Task<List<CameraStatus.CameraStatusResponse>> GetDataParkirCctvLog(DataGridViewRow row, ParkirView op, DateTime tglAwal, DateTime tglAkhir, CancellationToken token)
        {
            var result = new List<CameraStatus.CameraStatusResponse>();

            using (var client = new HttpClient())
            {
                await GenerateToken(row);

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _TOKEN);
                client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                int totalData = 0;
                int limit = 50;
                int offset = 0;
                bool hasMore = true;
                int attempt = 0;
                const int maxRetry = 5;

                while (hasMore)
                {
                    try
                    {
                        // 🔹 cek cancel sebelum request
                        token.ThrowIfCancellationRequested();

                        var resEvent = new CameraStatus.CameraStatusResponse();

                        do
                        {
                            // 🔹 cek cancel di setiap iterasi retry
                            token.ThrowIfCancellationRequested();

                            var body = new
                            {
                                method = "axxonsoft.bl.events.EventHistoryService.ReadEvents",
                                data = new
                                {
                                    range = new
                                    {
                                        begin_time = tglAwal.ToString("yyyyMMdd'T'HHmmss.'000'"),
                                        end_time = tglAkhir.ToString("yyyyMMdd'T'HHmmss.'999'")
                                    },
                                    filters = new
                                    {
                                        filters = new[]
                                        {
                                new {
                                    type = "ET_IpDeviceStateChangedEvent",
                                    subjects = op.AccessPoint
                                }
                            }
                                    },
                                    limit = limit,
                                    offset = offset,
                                    descending = true
                                }
                            };

                            var jsonBody = JsonSerializer.Serialize(body);
                            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                            //UpdateLog(row, $"Getting Data {tglAwal} s/d {tglAkhir}");

                            // 🔹 kirim request dengan token
                            HttpResponseMessage response = await client.PostAsync(_URL, content, token);

                            if (response.IsSuccessStatusCode)
                            {
                                UpdateLogLog(row, "Data Received, Processing...");

                                var rawResponse = await response.Content.ReadAsStringAsync(token);
                                var apiResponse = ConvertSseOutputJson(rawResponse);
                                var res = JsonSerializer.Deserialize<CameraStatus.CameraStatusResponse>(apiResponse);

                                if (res == null)
                                    throw new Exception("Response dari API tidak valid");

                                resEvent = res;
                                break;
                            }
                            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                            {
                                UpdateLogLog(row, "Token kadaluarsa, refresh token dan ulangi...");
                                await GenerateToken(row);
                            }
                            else
                            {
                                UpdateLogLog(row, $"Error: {response.StatusCode}");
                            }

                            attempt++;
                            if (attempt >= maxRetry)
                            {
                                UpdateLogLog(row, $"Gagal setelah {maxRetry} percobaan pada offset {offset}");
                                break;
                            }

                        } while (attempt < maxRetry);

                        // 🔹 setelah selesai mencoba, cek cancel
                        token.ThrowIfCancellationRequested();

                        if (resEvent == null || resEvent.Items == null || resEvent.Items.Count == 0)
                        {
                            UpdateLogLog(row, $"Tidak ada data event di offset {offset}");
                            hasMore = false;
                        }
                        else
                        {
                            UpdateLogLog(row, $"Dapat {resEvent.Items.Count} event/items di offset {offset}");
                            result.Add(resEvent);

                            totalData += resEvent.Items.Count;
                            if (resEvent.Items.Count < limit)
                            {
                                UpdateLogLog(row, $"Data sudah habis, total {totalData} event.");
                                hasMore = false;
                            }
                            else
                            {
                                offset += limit;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UpdateLogLog(row, $"Error: {ex.Message}");
                    }
                }
            }

            return result;
        }
        public async Task InsertToDbCctvLog(ParkirView op, List<CameraStatus.CameraStatusResponse> dataCameraStatusList, DataGridViewRow row, CancellationToken token)
        {
            var context = DBClass.GetContext();

            var rekapResult = new List<CameraStatusRekap>();
            var dataList = dataCameraStatusList.SelectMany(x => x.Items).ToList();

            foreach (var item in dataList)
            {
                var res = new CameraStatusRekap();

                res.Guid = item.Body.Guid;
                res.AccessPoint = item.Subjects[0];
                res.Localization = item.Localization.Text;
                res.StateAsli = item.Body.State;
                res.StateEnum = GetStatusCctv(item.Body.State);
                res.State = GetStatusCctv(item.Body.State).GetDescription();
                res.Tanggal = ParseFlexibleDate(item.Body.Timestamp);

                rekapResult.Add(res);
            }

            UpdateLogLog(row, "Inserting Detail");
            await InsertToDbCctvLogDetail(op, rekapResult, token);
            UpdateLogLog(row, "Done Detail");

            if(rekapResult.Count > 0)
            {
                var dataPalingTerakhir = rekapResult
                    .OrderByDescending(x => x.Tanggal)
                    .First();

                DateTime dataTanggalTerakhirAktif = rekapResult
                    .Where(x => x.StateEnum == EnumFactory.EStatusCCTV.Aktif).OrderByDescending(x => x.Tanggal)
                    .First().Tanggal;

                DateTime dataTanggalTerakhirNonAktif = rekapResult
                    .Where(x => x.StateEnum == EnumFactory.EStatusCCTV.NonAktif).OrderByDescending(x => x.Tanggal)
                    .First().Tanggal;

                var insert = new MOpParkirCctvJasnitaLog();
                insert.Nop = op.NOP;
                insert.CctvId = op.CCTVId;
                insert.TglTerakhirAktif = dataTanggalTerakhirAktif;
                insert.TglTerakhirDown = dataTanggalTerakhirNonAktif;
                insert.Status = dataPalingTerakhir.State;

                var checkWaktuMasukTransaksiTerakhir = context.TOpParkirCctvs
                    .Where(x => x.Nop == op.NOP && x.CctvId == op.CCTVId)
                    .OrderByDescending(x => x.WaktuMasuk)
                    .FirstOrDefault();

                if(checkWaktuMasukTransaksiTerakhir != null)
                {
                    if(checkWaktuMasukTransaksiTerakhir.WaktuMasuk > dataTanggalTerakhirAktif)
                    {
                        insert.TglTerakhirAktif = checkWaktuMasukTransaksiTerakhir.WaktuMasuk;
                        insert.Status = EnumFactory.EStatusCCTV.Aktif.GetDescription();
                    }
                }

                try
                {
                    await context.Database.OpenConnectionAsync(token);

                    UpdateLogLog(row, "Inserting...");
                    if (dataPalingTerakhir != null)
                    {
                        // ✅ Perhatikan "await" di sini
                        var existing = await context.MOpParkirCctvJasnitaLogs
                            .FirstOrDefaultAsync(x => x.Nop == op.NOP && x.CctvId == op.CCTVId, token);

                        if (existing != null)
                        {
                            // update record yang sudah ada
                            existing.TglTerakhirAktif = insert.TglTerakhirAktif;
                            existing.TglTerakhirDown = insert.TglTerakhirDown;
                            existing.Status = insert.Status;
                        }
                        else
                        {
                            // tambah record baru
                            await context.MOpParkirCctvJasnitaLogs.AddAsync(insert, token);
                        }

                        UpdateLogLog(row, "Saving changes...");
                        await context.SaveChangesAsync(token);
                    }
                    await context.Database.CloseConnectionAsync();
                }
                catch (Exception ex)
                {
                    UpdateLogLog(row, $"Error Insert Db: {ex.Message}");
                }
                finally
                {
                    await context.Database.CloseConnectionAsync();
                }
            }
        }
        public async Task InsertToDbCctvLogDetail(ParkirView op, List<CameraStatusRekap> dataList, CancellationToken token)
        {
            var context = DBClass.GetContext();

            var allIds = dataList
                .Select(x => x.Guid)
                .Distinct()
                .ToList();

            var existingKeys = await context.MOpParkirCctvJasnitaLogDs
                .Where(x => allIds.Contains(x.Guid) && x.Nop == op.NOP)
                .Select(x => new { x.Guid, x.Nop })
                .ToListAsync(token);

            var existingSet = new HashSet<(string Guid, string Nop)>(
                existingKeys.Select(k => (k.Guid, k.Nop))
            );

            var insertData = new List<MOpParkirCctvJasnitaLogD>();
            foreach (var item in dataList)
            {
                bool dataExist = existingSet.Contains((item.Guid, op.NOP));
                if (!dataExist)
                {
                    var insert = new MOpParkirCctvJasnitaLogD
                    {
                        Guid = item.Guid,
                        Nop = op.NOP,
                        CctvId = op.CCTVId,
                        TglEvent = item.Tanggal,
                        Event = item.StateAsli,
                        IsOn = item.StateAsli.ToUpper() == "IPDS_SIGNAL_RESTORED" ? 1 : 0
                    };

                    insertData.Add(insert);
                }
            }

            try
            {
                await context.Database.OpenConnectionAsync(token);

                if (insertData.Count > 0)
                {
                    await context.MOpParkirCctvJasnitaLogDs.AddRangeAsync(insertData, token);
                    await context.SaveChangesAsync(token);
                }
            }
            catch (Exception ex)
            {
                // log error
                Console.WriteLine($"Insert error: {ex.Message}");
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }


        //DATAGRIDVIEW 3 - PARKIR CCTV TELKOM
        private async Task RunTaskCctvTelkom(ParkirView op, CancellationToken token, DataGridViewRow row)
        {
            try
            {
                while (!token.IsCancellationRequested) // loop terus
                {
                    UpdateLogTelkom(row, "Fetching...");

                    var tglAwal = DateTime.Now.AddDays(-1 * _INTERVAL_DAY);
                    var tglAkhir = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

                    var dataCctvParkir = await GetDataParkirCctvTelkom(row, op, tglAwal, tglAkhir, token);
                    await InsertToDbCctvTelkom(op, dataCctvParkir, row, token);

                    row.Cells["LastConnectedTelkom"].Value = DateTime.Now.ToString();
                    UpdateLogTelkom(row, "Inserted");

                    if (token.IsCancellationRequested)
                        break;

                    var nextRun = DateTime.Now.AddMinutes(_INTERVAL_API);
                    UpdateLogTelkom(row, $"[DONE] Next Run: {nextRun:dd/MM/yyyy HH:mm:ss}");
                    await Task.Delay(TimeSpan.FromMinutes(_INTERVAL_API), token);
                }
            }
            catch (TaskCanceledException)
            {
                UpdateLogTelkom(row, "Dihentikan manual ❌");
            }
            catch (Exception ex)
            {
                // ✅ Log error di UI
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        row.Cells["ErrorTelkom"].Value = ex.Message;
                        row.Cells["StatusTelkom"].Style.BackColor = Color.Red;
                        row.Cells["StatusTelkom"].Value = "Error";
                        var btnCell = (DataGridViewButtonCell)row.Cells["ActionTelkom"];
                        btnCell.Value = "Start";
                    }));
                }
                else
                {
                    row.Cells["ErrorTelkom"].Value = ex.Message;
                    row.Cells["StatusTelkom"].Style.BackColor = Color.Red;
                    row.Cells["StatusTelkom"].Value = "Error";
                    var btnCell = (DataGridViewButtonCell)row.Cells["ActionTelkom"];
                    btnCell.Value = "Start";
                }

                UpdateLogTelkom(row, $"{op.AccessPoint} Error: " + ex.Message);

                return;
            }
        }
        public async Task<List<TelkomEvent.Result>> GetDataParkirCctvTelkom(DataGridViewRow row, ParkirView op, DateTime tglAwal, DateTime tglAkhir, CancellationToken token)
        {
            await GenerateTokenTelkom(row);
            var results = new List<TelkomEvent.Result>();

            using (var client = new HttpClient())
            {
                try
                {
                    if (string.IsNullOrEmpty(_TOKEN_TELKOM))
                    {
                        UpdateLogTelkom(row, "Token Telkom belum dibuat. Silakan login dulu.");
                        return results;
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_TELKOM);

                    // Loop dari tanggal awal sampai akhir
                    for (var tgl = tglAwal; tgl <= tglAkhir; tgl = tgl.AddDays(1))
                    {
                        if (token.IsCancellationRequested)
                            break;

                        string dateStr = tgl.ToString("yyyy-MM-dd");
                        string url = $"https://bigvision.id/api/analytics/license-plate-recognition/data-tables?date={dateStr}&id_camera={op.CCTVId}";

                        var response = await client.GetAsync(url, token);

                        // Jika status bukan sukses, lanjut ke tanggal berikutnya
                        if (!response.IsSuccessStatusCode)
                        {
                            UpdateLogTelkom(row, $"Gagal ambil data Telkom untuk {dateStr}: {response.StatusCode}");
                            continue;
                        }

                        var json = await response.Content.ReadAsStringAsync(token);

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var data = JsonSerializer.Deserialize<TelkomEvent.TelkomEventResponse>(json, options);

                        if (data?.Result != null && data.Result.Count > 0)
                        {
                            results.AddRange(data.Result);
                            UpdateLogTelkom(row, $"✅ {dateStr} → {data.Result.Count} data");
                        }
                        else
                        {
                            UpdateLogTelkom(row, $"ℹ️ {dateStr} → tidak ada data");
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                    UpdateLogTelkom(row, "Dihentikan manual ❌");
                }
                catch (Exception ex)
                {
                    // ✅ Log error di UI
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            row.Cells["ErrorTelkom"].Value = ex.Message;
                            row.Cells["StatusTelkom"].Style.BackColor = Color.Red;
                            row.Cells["StatusTelkom"].Value = "Error";
                            var btnCell = (DataGridViewButtonCell)row.Cells["ActionTelkom"];
                            btnCell.Value = "Start";
                        }));
                    }
                    else
                    {
                        row.Cells["ErrorTelkom"].Value = ex.Message;
                        row.Cells["StatusTelkom"].Style.BackColor = Color.Red;
                        row.Cells["StatusTelkom"].Value = "Error";
                        var btnCell = (DataGridViewButtonCell)row.Cells["ActionTelkom"];
                        btnCell.Value = "Start";
                    }

                    UpdateLogTelkom(row, $"{op.NOP} {op.CCTVId} Error: " + ex.Message);
                }
            }

            return results;
        }
        public async Task InsertToDbCctvTelkom(ParkirView op, List<TelkomEvent.Result> dataList, DataGridViewRow row, CancellationToken token)
        {
            UpdateLogTelkom(row, "inserting...");
            var context = DBClass.GetContext();
            var toInsert = new List<TOpParkirCctv>();

            var allIds = dataList
               .Select(x => x.Id.ToString())
               .Distinct()
               .ToList();

            var existingIds = await context.TOpParkirCctvs
                .Where(x => allIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(token);

            var existingSet = new HashSet<string>(existingIds);

            int seq = 1;
            int totalData = dataList.Count; // bukan toInsert.Count, karena toInsert diisi di dalam loop

            foreach (var item in dataList)
            {
                bool isIdExist = existingSet.Contains(item.Id.ToString());
                if (!isIdExist)
                {
                    var insert = new TOpParkirCctv
                    {
                        Id = item.Id.ToString(),
                        Nop = op.NOP,
                        CctvId = op.CCTVId,
                        NamaOp = op.Nama,
                        AlamatOp = op.Alamat,
                        WilayahPajak = op.Uptb,
                        WaktuMasuk = ParseFlexibleDate(item.Timestamp),
                        JenisKend = (int)GetJenisKendaraan(item.TipeKendaraan),
                        PlatNo = item.PlatNomor.ToUpper() == "UNRECOGNIZED" ? null : item.PlatNomor.ToUpper(),
                        WaktuKeluar = ParseFlexibleDate(item.Timestamp),
                        Direction = (int)EnumFactory.CctvParkirDirection.Incoming,
                        Log = "",
                        ImageUrl = item.Image,
                        Vendor = (int)(EnumFactory.EVendorParkirCCTV.Telkom)
                    };

                    toInsert.Add(insert);
                }

                // Hitung progres dalam persen
                double progress = ((double)seq / totalData) * 100;

                // Bulatkan dua angka di belakang koma
                string progressText = progress.ToString("0.00");

                // Tampilkan log seperti "Progress: 45.67%"
                UpdateLogTelkom(row, $"Proses data {seq}/{totalData} ({progressText}%)");

                seq++;
            }

            try
            {
                await context.Database.OpenConnectionAsync(token);
                if (toInsert.Count > 0)
                {
                    UpdateLogTelkom(row, $"Inserting {toInsert.Count} record(s).");
                    await context.TOpParkirCctvs.AddRangeAsync(toInsert, token);
                    await context.SaveChangesAsync(token);
                    await context.Database.CloseConnectionAsync();
                    UpdateLogTelkom(row, $"Inserted {toInsert.Count} record(s).");
                }
                else
                {
                    UpdateLogTelkom(row, "No new data to insert.");
                }
            }
            catch (Exception ex)
            {
                UpdateLogTelkom(row, $"Error Insert Db: {ex.Message}");
            }
            finally
            {
                await context.Database.CloseConnectionAsync();
            }
        }


        //DATAGRIDVIEW 4 - PARKIR CCTV TELKOM LOG
        private async Task ProcessTelkomLogAsync(CancellationToken token)
        {
            UpdateLogTelkomLog("🔄 Mulai proses Telkom Log...");

            try
            {
                // Pastikan token Telkom valid
                if (string.IsNullOrEmpty(_TOKEN_TELKOM))
                {
                    UpdateLogTelkomLog("🔐 Token Telkom belum ada, mencoba generate...");
                    await GenerateTokenTelkom();
                    if (string.IsNullOrEmpty(_TOKEN_TELKOM))
                    {
                        UpdateLogTelkomLog("❌ Gagal generate token Telkom.");
                        return;
                    }
                    UpdateLogTelkomLog("✅ Token Telkom berhasil dibuat.");
                }

                // Ambil list analytics dari API Telkom
                var results = new List<TelkomEventCameraStatus.Result>();
                string url = "https://bigvision.id/api/setup-project/list-analytics/55";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _TOKEN_TELKOM);

                UpdateLogTelkomLog("🌐 Mengambil data dari Telkom API...");

                var response = await client.GetAsync(url, token);
                if (!response.IsSuccessStatusCode)
                {
                    UpdateLogTelkomLog($"❌ Gagal ambil data Telkom: {response.StatusCode}");
                    return;
                }

                string json = await response.Content.ReadAsStringAsync(token);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<TelkomEventCameraStatus.TelkomEventCameraStatusResponse>(json, options);

                if (data?.Result == null || data.Result.Count == 0)
                {
                    UpdateLogTelkomLog("ℹ️ Tidak ada data yang diterima dari Telkom.");
                    return;
                }

                results.AddRange(data.Result.Where(x => x.AnalyticsUsecase == "lpr").ToList());
                UpdateLogTelkomLog($"✅ Diterima {results.Count} data dari Telkom API.");

                // Ambil seluruh data OP dari DB
                var dataOp = GetOpParkirCctvTelkom();
                if (dataOp == null || dataOp.Count == 0)
                {
                    UpdateLogTelkomLog("❌ Tidak ada data OP Telkom yang ditemukan di database.");
                    return;
                }

                UpdateLogTelkomLog($"📦 Memproses {dataOp.Count} data OP Telkom...");

                await using var context = DBClass.GetContext();
                int processed = 0;

                try
                {
                    foreach (var dataResponse in results)
                    {
                        if (token.IsCancellationRequested)
                        {
                            UpdateLogTelkomLog("⏹️ Dihentikan manual.");
                            return;
                        }

                        var op = dataOp.FirstOrDefault(x => x.CCTVId == dataResponse.IdAnalytics.ToString());
                        if (op == null) continue;

                        processed++;

                        try
                        {
                            var lastDateScan = await context.TOpParkirCctvs
                                .Where(x => x.Nop == op.NOP && x.CctvId == op.CCTVId)
                                .MaxAsync(q => (DateTime?)q.WaktuMasuk, token);

                            if (lastDateScan == null)
                            {
                                lastDateScan = DateTime.MinValue;
                            }

                            string lastStatus = dataResponse.Status?.ToUpper() == "ACTIVE" ? "AKTIF" : "NON AKTIF";
                            DateTime dateStatusAktif = ParseFlexibleDate(
                                string.IsNullOrEmpty(dataResponse.StatusUpdate)
                                    ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                    : dataResponse.StatusUpdate
                            );
                            DateTime? dateStatusDown = lastStatus == "NON AKTIF" ? dateStatusAktif : null;

                            if (lastDateScan > dateStatusAktif)
                            {
                                dateStatusAktif = lastDateScan.Value;
                                lastStatus = "AKTIF";
                            }

                            var existing = await context.MOpParkirCctvTelkomLogs
                                .FirstOrDefaultAsync(x => x.Nop == op.NOP && x.CctvId == op.CCTVId, token);

                            if (existing != null)
                            {
                                existing.TglTerakhirAktif = dateStatusAktif;
                                existing.TglTerakhirDown = dateStatusDown;
                                existing.Status = lastStatus;
                            }
                            else
                            {
                                await context.MOpParkirCctvTelkomLogs.AddAsync(new MOpParkirCctvTelkomLog
                                {
                                    Nop = op.NOP,
                                    CctvId = op.CCTVId,
                                    TglTerakhirAktif = dateStatusAktif,
                                    TglTerakhirDown = dateStatusDown,
                                    Status = lastStatus
                                }, token);
                            }

                            // log setiap proses, tanpa batasan kelipatan 10
                            UpdateLogTelkomLog($"🧾 Diproses {processed}/{results.Count} CCTV...");
                        }
                        catch (Exception exInner)
                        {
                            UpdateLogTelkomLog($"⚠️ Gagal proses CCTV {op.NOP} ({op.CCTVId}): {exInner.Message}");
                        }
                    }

                    // Simpan semua perubahan terakhir
                    await context.SaveChangesAsync(token);
                }
                catch (Exception ex)
                {
                    UpdateLogTelkomLog($"err : {ex.Message}");
                }
                finally 
                {
                    await context.Database.CloseConnectionAsync();
                }

                UpdateLogTelkomLog($"✅ Semua {processed} data TelkomLog selesai diproses.");
            }
            catch (TaskCanceledException)
            {
                UpdateLogTelkomLog("⏹️ Proses dihentikan manual (TaskCanceled).");
            }
            catch (Exception ex)
            {
                UpdateLogTelkomLog($"❌ Terjadi error: {ex.Message}");
            }
        }


        // EVENT
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

        private async void BtnStartLogAll_Click(object sender, EventArgs e)
        {
            // Disable tombol start, enable tombol stop
            btnStartAllLogJasnita.Enabled = false;
            btnStopAllLogJasnita.Enabled = true;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["ActionLog"] is DataGridViewButtonCell btnCell)
                {
                    string actionText = btnCell.Value?.ToString() ?? "";

                    if (actionText == "Start")
                    {
                        int rowIndex = row.Index;
                        int colIndex = dataGridView2.Columns["ActionLog"].Index;

                        await Task.Run(() =>
                        {
                            DataGridView2_CellClick(
                                dataGridView2,
                                new DataGridViewCellEventArgs(colIndex, rowIndex)
                            );
                        });

                        // kasih jeda sedikit biar tidak semua nembak API bersamaan
                        await Task.Delay(300);
                    }
                }
            }
        }
        private void BtnStopLogAll_Click(object sender, EventArgs e)
        {
            // Enable kembali tombol start, disable tombol stop
            btnStartAllLogJasnita.Enabled = true;
            btnStopAllLogJasnita.Enabled = false;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["ActionLog"] is DataGridViewButtonCell btnCell)
                {
                    string actionText = btnCell.Value?.ToString() ?? "";

                    if (actionText == "Stop")
                    {
                        int rowIndex = row.Index;
                        int colIndex = dataGridView2.Columns["ActionLog"].Index;

                        DataGridView2_CellClick(
                            dataGridView2,
                            new DataGridViewCellEventArgs(colIndex, rowIndex)
                        );
                    }
                }
            }
        }

        private async void BtnStartTelkomAll_Click(object sender, EventArgs e)
        {
            // Disable tombol start, enable tombol stop
            btnStartAllTelkom.Enabled = false;
            btnStopAllTelkom.Enabled = true;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["ActionTelkom"] is DataGridViewButtonCell btnCell)
                {
                    string actionText = btnCell.Value?.ToString() ?? "";

                    if (actionText == "Start")
                    {
                        int rowIndex = row.Index;
                        int colIndex = dataGridView3.Columns["ActionTelkom"].Index;

                        await Task.Run(() =>
                        {
                            DataGridView3_CellClick(
                                dataGridView3,
                                new DataGridViewCellEventArgs(colIndex, rowIndex)
                            );
                        });

                        // kasih jeda sedikit biar tidak semua nembak API bersamaan
                        await Task.Delay(300);
                    }
                }
            }
        }
        private void BtnStopTelkomAll_Click(object sender, EventArgs e)
        {
            // Enable kembali tombol start, disable tombol stop
            btnStartAllTelkom.Enabled = true;
            btnStopAllTelkom.Enabled = false;

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["ActionTelkom"] is DataGridViewButtonCell btnCell)
                {
                    string actionText = btnCell.Value?.ToString() ?? "";

                    if (actionText == "Stop")
                    {
                        int rowIndex = row.Index;
                        int colIndex = dataGridView3.Columns["ActionTelkom"].Index;

                        DataGridView3_CellClick(
                            dataGridView3,
                            new DataGridViewCellEventArgs(colIndex, rowIndex)
                        );
                    }
                }
            }
        }


        private async void BtnStartTelkomLogAll_Click(object sender, EventArgs e)
        {
            LogTelkomLog.Clear();

            _telkomLogTokenSource = new CancellationTokenSource();
            var token = _telkomLogTokenSource.Token;
            panelStatusTelkomLog.BackColor = Color.LimeGreen; // 🟢 Menandakan sedang berjalan

            UpdateLogTelkomLog("🚀 Memulai proses log Telkom otomatis tiap 15 menit...");

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var startTime = DateTime.Now;
                    UpdateLogTelkomLog($"⏰ Mulai proses TelkomLog pada {startTime:dd/MM/yyyy HH:mm:ss}");

                    try
                    {
                        await ProcessTelkomLogAsync(token);
                        UpdateLogTelkomLog($"✅ Selesai proses TelkomLog ({DateTime.Now:HH:mm:ss})");
                    }
                    catch (OperationCanceledException)
                    {
                        UpdateLogTelkomLog("🛑 Proses TelkomLog dibatalkan oleh pengguna.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        UpdateLogTelkomLog($"⚠️ Error saat proses TelkomLog: {ex.Message}");
                    }

                    if (token.IsCancellationRequested)
                        break;

                    // Jadwalkan proses berikutnya
                    var nextRun = DateTime.Now.AddMinutes(15);
                    UpdateLogTelkomLog($"⏳ [DONE] Next Run: {nextRun:dd/MM/yyyy HH:mm:ss}");

                    await Task.Delay(TimeSpan.FromMinutes(15), token);
                }
            }
            catch (TaskCanceledException)
            {
                UpdateLogTelkomLog("🛑 Proses loop TelkomLog dihentikan manual (TaskCanceled).");
            }
            catch (Exception ex)
            {
                UpdateLogTelkomLog($"❌ Terjadi error di loop utama: {ex.Message}");
            }
            finally
            {
                panelStatusTelkomLog.BackColor = Color.Red; // 🔴 Menandakan berhenti
                _telkomLogTokenSource?.Dispose();
                _telkomLogTokenSource = null;
                UpdateLogTelkomLog("🟢 Loop TelkomLog berhenti total.");
            }
        }
        private void BtnStopTelkomLogAll_Click(object sender, EventArgs e)
        {
            if (_telkomLogTokenSource != null && !_telkomLogTokenSource.IsCancellationRequested)
            {
                _telkomLogTokenSource.Cancel();
                panelStatusTelkomLog.BackColor = Color.Red; // 🔴 berhenti
                UpdateLogTelkomLog("⏹️ Permintaan pembatalan dikirim...");
            }
            else
            {
                panelStatusTelkomLog.BackColor = Color.Red; // 🔴 berhenti
                UpdateLogTelkomLog("ℹ️ Tidak ada proses yang sedang berjalan.");
            }
        }



        public List<ParkirView> GetOpParkirCctvTelkom()
        {
            var result = new List<ParkirView>();

            var _cont = DBClass.GetContext();
            var pl = _cont.MOpParkirCctvs
                .Include(x => x.MOpParkirCctvTelkoms)
                .Where(x => x.Vendor == (int)EnumFactory.EVendorParkirCCTV.Telkom)
                .OrderBy(x => x.NamaOp)
                .ToList();


            int no = 1;
            foreach (var item in pl)
            {
                foreach (var det in item.MOpParkirCctvTelkoms)
                {
                    result.Add(new ParkirView()
                    {
                        No = no,
                        Id = item.Nop + "-" + det.CctvId.ToString(),
                        NOP = item.Nop,
                        Nama = item.NamaOp,
                        Alamat = item.AlamatOp,
                        Uptb = item.WilayahPajak,
                        CCTVId = !string.IsNullOrEmpty(det.CctvId) ? det.CctvId : "",
                        Mode = det.CctvMode == 1 ? "IN" : det.CctvMode == 2 ? "OUT" : "HYBRID",
                        Status = "Idle",
                        LastConnected = null,
                        Err = null
                    });

                    no++;
                }
            }

            return result.OrderBy(x => x.Nama).ToList();
        }
        public List<ParkirView> GetOpParkirCctv()
        {
            var result = new List<ParkirView>();

            var _cont = DBClass.GetContext();
            var pl = _cont.MOpParkirCctvs
                .Include(x => x.MOpParkirCctvJasnita)
                .Where(x => x.Vendor == (int)EnumFactory.EVendorParkirCCTV.Jasnita)
                .OrderBy(x => x.NamaOp)
                .ToList();


            int no = 1;
            foreach (var item in pl)
            {
                foreach (var det in item.MOpParkirCctvJasnita)
                {
                    result.Add(new ParkirView()
                    {
                        No = no,
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

                    no++;
                }
            }

            return result.OrderBy(x => x.Nama).ToList();
        }
        public ParkirView? GetParkirById(string id)
        {
            return _parkirList.FirstOrDefault(q => q.Id == id);
        }
        public ParkirView? GetParkirTelkomById(string id)
        {
            return _parkirListTelkom.FirstOrDefault(q => q.Id == id);
        }
        private async Task GenerateToken(DataGridViewRow row)
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

                    _TOKEN = obj?["token_value"]?.ToString() ?? ""; // ambil token
                }
                else
                {
                    throw new Exception("Gagal ambil token. Status: " + response.StatusCode);
                }
            }
        }
        private async Task GenerateTokenTelkom()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // URL endpoint login
                    var url = "https://stage1.bigvision.id/api/user/va/login";

                    // Body request (ubah sesuai kredensial sebenarnya)
                    var requestBody = new
                    {
                        username = _USER_TELKOM,
                        password = _PASS_TELKOM
                    };

                    // Serialize body ke JSON
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Kirim POST request
                    var response = await client.PostAsync(url, content);

                    // Pastikan respons sukses
                    response.EnsureSuccessStatusCode();

                    // Baca hasil JSON
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Parse JSON
                    using var doc = JsonDocument.Parse(responseString);
                    var root = doc.RootElement;

                    // Ambil access_token
                    var token = root.GetProperty("result").GetProperty("access_token").GetString();

                    // Simpan ke field global
                    _TOKEN_TELKOM = token ?? "";
                }
                catch (HttpRequestException ex)
                {
                    UpdateLogTelkomLog($"Gagal menghubungi server Telkom: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    UpdateLogTelkomLog($"Gagal parsing respon login Telkom: {ex.Message}");
                }
                catch (Exception ex)
                {
                    UpdateLogTelkomLog($"Error tidak terduga: {ex.Message}");
                }
            }
        }
        private async Task GenerateTokenTelkom(DataGridViewRow row)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // URL endpoint login
                    var url = "https://stage1.bigvision.id/api/user/va/login";

                    // Body request (ubah sesuai kredensial sebenarnya)
                    var requestBody = new
                    {
                        username = _USER_TELKOM,
                        password = _PASS_TELKOM
                    };

                    // Serialize body ke JSON
                    var json = JsonSerializer.Serialize(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Kirim POST request
                    var response = await client.PostAsync(url, content);

                    // Pastikan respons sukses
                    response.EnsureSuccessStatusCode();

                    // Baca hasil JSON
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Parse JSON
                    using var doc = JsonDocument.Parse(responseString);
                    var root = doc.RootElement;

                    // Ambil access_token
                    var token = root.GetProperty("result").GetProperty("access_token").GetString();

                    // Simpan ke field global
                    _TOKEN_TELKOM = token ?? "";
                }
                catch (HttpRequestException ex)
                {
                    UpdateLogTelkom(row, $"Gagal menghubungi server Telkom: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    UpdateLogTelkom(row, $"Gagal parsing respon login Telkom: {ex.Message}");
                }
                catch (Exception ex)
                {
                    UpdateLogTelkom(row, $"Error tidak terduga: {ex.Message}");
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
                case "BUS":
                    return EnumFactory.EJenisKendParkirCCTV.Bus;
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
        public EnumFactory.EStatusCCTV GetStatusCctv(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "IPDS_SIGNAL_RESTORED":
                    return EnumFactory.EStatusCCTV.Aktif;
                default:
                    return EnumFactory.EStatusCCTV.NonAktif;
            }
        }
        public static DateTime ParseFlexibleDate(string timeStr)
        {
            if (string.IsNullOrWhiteSpace(timeStr))
            {
                throw new ArgumentException("timeStr tidak boleh kosong");
            }

            var formats = new[]
            {
        "yyyyMMdd'T'HHmmss.ffffff",  // 20250924T083638.867000
        "yyyyMMdd'T'HHmmss.fff",     // 20250924T083638.867
        "yyyyMMdd'T'HHmmss",         // ✅ 20250925T073926 (kasus kamu)
        "yyyy-MM-dd'T'HH:mm:ss.ffffff",
        "yyyy-MM-dd'T'HH:mm:ss.fff",
        "yyyy-MM-dd'T'HH:mm:ss",
        "yyyyMMddHHmmss",            // 20250925073926 (tanpa 'T')
        "yyyy-MM-dd HH:mm:ss",       // 2025-09-25 07:39:26
        "yyyy/MM/dd HH:mm:ss",       // 2025/09/25 07:39:26
        "yyyyMMdd'T'HHmmssZ",        // 20250925T073926Z
        "yyyy-MM-dd'T'HH:mm:ssZ",    // 2025-09-25T07:39:26Z
        "yyyy-MM-dd'T'HH:mm:ssK"     // 2025-09-25T07:39:26+07:00
    };

            foreach (var fmt in formats)
            {
                if (DateTime.TryParseExact(
                    timeStr,
                    fmt,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal,
                    out DateTime result))
                {
                    return result;
                }
            }

            throw new Exception(timeStr + " tidak sesuai format yang dikenali.");
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
        private void UpdateLogLog(DataGridViewRow row, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateLogLog(row, message)));
                return;
            }

            row.Cells["LogLog"].Value = message; // ✅ replace, bukan append
        }
        private void UpdateLogTelkom(DataGridViewRow row, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateLogTelkom(row, message)));
                return;
            }

            row.Cells["LogTelkom"].Value = message; // ✅ replace, bukan append
        }
        private void UpdateLogTelkomLog(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(UpdateLogTelkomLog), message);
                return;
            }

            LogTelkomLog.AppendText($"[{DateTime.Now.ToString("dd-MMM-yyy HH:mm:ss")}] {message}{Environment.NewLine}");
        }
    }
}
