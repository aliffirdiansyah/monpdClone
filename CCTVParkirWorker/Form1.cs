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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        public string _TOKEN;
        public int _INTERVAL_DAY;

        private Dictionary<int, CancellationTokenSource> _taskTokens = new Dictionary<int, CancellationTokenSource>();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;

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

            _INTERVAL_API = 45;
            _URL = "http://202.146.133.26/grpc";
            _USER = "bapendasby";
            _PASS = "surabaya2025!!";
            _INTERVAL_DAY = 90;

            dataGridView1.CellClick += DataGridView1_CellClick;
            btnStartAll.Click += BtnStartAll_Click;
            btnStopAll.Click += BtnStopAll_Click;

            dataGridView2.CellClick += DataGridView2_CellClick;
            btnStartAllLogJasnita.Click += BtnStartLogAll_Click;
            btnStopAllLogJasnita.Click += BtnStopLogAll_Click;
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

                            bool dataExist = await context.TOpParkirCctvs.AnyAsync(x => x.Id == id, token);

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
                                    Log = $"ID:{id},DIR:{ar.Direction},CLASS:{ar.VehicleClass ?? "-"},BRAND:{ar.VehicleBrand ?? "-"},MODEL:{ar.VehicleModel ?? "-"}"
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

                res.AccessPoint = item.Subjects[0];
                res.Localization = item.Localization.Text;
                res.State = GetStatusCctv(item.Body.State).GetDescription();
                res.Tanggal = ParseFlexibleDate(item.Body.Timestamp);

                rekapResult.Add(res);
            }

            //do logic here
            var dataLog = rekapResult
                .GroupBy(x => new { x.AccessPoint })
                .Select(g =>
                {
                    //nop
                    string nop = op.NOP;

                    //cctv id
                    string cctvId = op.CCTVId;

                    // ambil event terakhir (tanggal terbaru)
                    var lastEvent = g.OrderByDescending(x => x.Tanggal).FirstOrDefault();

                    // ambil waktu terakhir aktif & terakhir nonaktif
                    var lastAktif = g.Where(x => x.State == "AKTIF")
                                     .OrderByDescending(x => x.Tanggal)
                                     .FirstOrDefault();

                    var lastNonAktif = g.Where(x => x.State == "NON AKTIF")
                                        .OrderByDescending(x => x.Tanggal)
                                        .FirstOrDefault();

                    // bikin objek hasilnya
                    return new MOpParkirCctvJasnitaLog
                    {
                        Nop = nop, // sesuaikan kalau NOP beda field
                        CctvId = cctvId,
                        TglTerakhirAktif = lastAktif?.Tanggal ?? DateTime.MinValue,
                        TglTerakhirDown = lastNonAktif?.Tanggal ?? DateTime.MinValue,
                        Status = lastEvent?.State ?? "NON AKTIF"
                    };
                })
                .FirstOrDefault();

            if (dataLog != null)
            {
                var existing = context.MOpParkirCctvJasnitaLogs.FirstOrDefault(x => x.Nop == op.NOP && x.CctvId == op.CCTVId);
                if (existing != null)
                {
                    // Update record yang sudah ada
                    existing.TglTerakhirAktif = dataLog.TglTerakhirAktif;
                    existing.TglTerakhirDown = dataLog.TglTerakhirDown;
                    existing.Status = dataLog.Status;
                }
                else
                {
                    // Tambah record baru
                    context.MOpParkirCctvJasnitaLogs.Add(dataLog);
                }
            }

            UpdateLogLog(row, "Inserting...");
            context.SaveChanges();
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
            btnStartAll.Enabled = false;
            btnStopAll.Enabled = true;

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
            btnStartAll.Enabled = true;
            btnStopAll.Enabled = false;

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




        public List<ParkirView> GetOpParkirCctv()
        {
            var result = new List<ParkirView>();

            var _cont = DBClass.GetContext();
            var pl = _cont.MOpParkirCctvs
                .Include(x => x.MOpParkirCctvJasnita)
                .Where(x => x.Vendor == 1)
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
    }
}
