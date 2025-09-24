using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using static CobaAPICctv.ViewModel;
using static CobaAPICctv.ViewModel.EventAll;

namespace CobaAPICctv
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public string TOKEN_AUTH = "";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
                await GenerateToken();

                var cameraList = await GetCameraList();

                Console.WriteLine($"[INFO] Kamera ditemukan: {cameraList?.Items?.Count ?? 0}");
                Console.WriteLine($"\n");

                if(cameraList == null || cameraList.Items.Count == 0)
                {
                    Console.WriteLine($"[ERROR] Tidak ada kamera ditemukan, hentikan proses...");
                    return;
                }

                var getEventList = await GetEventList(cameraList);

                var rekapResults = await SetRekap(getEventList);

                var x = "";
            }
        }

        public async Task GenerateToken()
        {
            string url = "http://202.146.133.26/grpc";
            using (var client = new HttpClient())
            {
                string user = "bapendasby";
                string pass = "surabaya2025!!";

                // Basic Auth
                var byteArray = Encoding.ASCII.GetBytes($"{user}:{pass}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                // Headers
                client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");

                var body = new
                {
                    method = "axxonsoft.bl.auth.AuthenticationService.AuthenticateEx",
                    data = new
                    {
                        user_name = "bapendasby",
                        password = "surabaya2025!!"
                    }
                };

                var jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var obj = JsonObject.Parse(result);

                    TOKEN_AUTH = obj?["token_value"]?.ToString() ?? ""; // ambil token
                }
                else
                {
                    throw new Exception("Gagal ambil token. Status: " + response.StatusCode);
                }
            }
        }
        public async Task<ViewModel.CameraList.CameraListResponse?> GetCameraList()
        {
            int attempt = 0;
            const int maxRetry = 5;

            var result = new ViewModel.CameraList.CameraListResponse();
            do
            {

                string url = "http://202.146.133.26/grpc";
                using (var client = new HttpClient())
                {
                    // Bearer Token Auth
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TOKEN_AUTH);

                    // Headers
                    client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");
                    var body = new
                    {
                        method = "axxonsoft.bl.domain.DomainService.ListCameras",
                        data = new
                        {
                            view = "VIEW_MODE_FULL"
                        }
                    };

                    var jsonBody = JsonSerializer.Serialize(body);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var rawResponse = await response.Content.ReadAsStringAsync();
                        var apiResponse = Method.ConvertSseOutputJson(rawResponse);
                        var res = JsonSerializer.Deserialize<ViewModel.CameraList.CameraListResponse>(apiResponse);
                        if (res != null)
                        {
                            result = res;
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        Console.WriteLine($"[DEBUG] [GET_CAMERA_LIST] Token mungkin sudah kadaluarsa, refresh token dan ulangi...");

                        // Refresh token hanya sekali (di attempt pertama gagal 403)
                        await GenerateToken();
                    }
                    else
                    {
                        throw new Exception("Gagal ambil daftar kamera. Status: " + response.StatusCode);
                    }
                }

                attempt++;

            }
            while (attempt < maxRetry);

            return result;
        }
        public async Task<List<ViewModel.EventAll.EventAllResponse>> GetEventList(ViewModel.CameraList.CameraListResponse cameraList)
        {
            var allEvents = new List<ViewModel.EventAll.EventAllResponse>();
            var accessPoints = cameraList?.Items.Take(1);
            int seq = 1;

            var tgl_awal = DateTime.Now.AddDays(-1).Date;
            var tgl_akhir = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
            if (accessPoints != null)
            {
                foreach (var item in accessPoints)
                {
                    Console.WriteLine($"");
                    Console.WriteLine($"[INFO] [{DateTime.Now}] {seq}. AccessPoint: {item.AccessPoint} {item.DisplayName}, Date Range : {tgl_awal} - {tgl_akhir}");

                    int totalData = 0;
                    int limit = 20;
                    int offset = 0;
                    bool hasMore = true;

                    while (hasMore)
                    {
                        try
                        {
                            var getEvent = await GetEventAll(
                            limit,
                            offset,
                            item.AccessPoint ?? "",
                            tgl_awal,
                            tgl_akhir
                        );

                            if (getEvent == null)
                            {
                                Console.WriteLine($"");
                                Console.Write($"\r[WARN] Tidak ada data event di offset {offset}");
                                hasMore = false;
                            }
                            else
                            {
                                Console.Write($"\r[INFO] Dapat {getEvent.Items.Count} event di offset {offset}");
                                allEvents.Add(getEvent);



                                totalData += getEvent.Items.Count;
                                if (getEvent.Items.Count < limit)
                                {
                                    Console.WriteLine($"");
                                    Console.Write($"[INFO] [{DateTime.Now}] Data sudah habis, data didapat : {totalData}.");
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
                            Console.WriteLine($"");
                            Console.WriteLine($"\r[ERROR] Gagal ambil event di offset {offset}, AccessPoint: {item.AccessPoint}-{item.DisplayName}. Error: {ex.Message}");
                            string x = "";
                        }
                    }

                    // kasih jeda 5 detik setelah selesai proses 1 AccessPoint
                    Console.WriteLine("");
                    Console.WriteLine($"[INFO] Delay 5 detik sebelum lanjut ke AccessPoint berikutnya...");
                    await Task.Delay(5000);
                    seq++;
                }
            }


            Console.WriteLine($"[DONE] Total event terkumpul: {allEvents.Count}");

            return allEvents;
        }

        public async Task<List<ViewModel.RekapResult>> SetRekap(List<ViewModel.EventAll.EventAllResponse> eventList)
        {
            var result = new List<ViewModel.RekapResult>();

            var dataCctvResult = eventList.SelectMany(q => q.Items).ToList();

            foreach (var item in dataCctvResult)
            {

                if(item.Body.Details.Count > 0)
                {

                    foreach (var detail in item.Body.Details)
                    {

                        var ar = detail.AutoRecognitionResult;
                        if (ar != null)
                        {
                            var res = new ViewModel.RekapResult();
                            res.EventType = "AutoRecognitionResult";
                            res.Name = item.Body.OriginExt.FriendlyName;
                            res.Direction = ar.Direction;
                            res.TimeBegin = ar.TimeBegin;
                            res.TimeEnd = ar.TimeEnd;
                            if(ar.Hypotheses != null && ar.Hypotheses.Count > 0)
                            {
                                foreach (var w in ar.Hypotheses)
                                {
                                    res.HypothesisOcrQuality = w.OcrQuality;
                                    res.HypothesisPlateFull = w.PlateFull;
                                    res.HypothesisPlateRectangleX = w.PlateRectangle.X;
                                    res.HypothesisPlateRectangleY = w.PlateRectangle.Y;
                                    res.HypothesisPlateRectangleW = w.PlateRectangle.W;
                                    res.HypothesisPlateRectangleH = w.PlateRectangle.H;
                                    res.HypothesisPlateRectangleIndex = w.PlateRectangle.Index;
                                    res.HypothesisTimeBest = w.TimeBest;
                                    res.HypothesisCountry = w.Country;
                                    res.HypothesisPlateState = w.PlateState;
                                }
                            }
                            res.VehicleClass = ar.VehicleClass;
                            res.VehicleColor = ar.VehicleColor;
                            res.VehicleBrand = ar.VehicleBrand;
                            res.VehicleModel = ar.VehicleModel;
                            res.HeadlightsStatus = ar.HeadlightsStatus;
                            res.VehicleSpeed = ar.VehicleSpeed;
                            res.VehicleSpeedKmph = ar.VehicleSpeedKmph;
                            res.PlateType = ar.PlateType;
                            result.Add(res);
                        }

                        var arx = detail.AutoRecognitionResultEx;
                        if (arx != null)
                        {
                            var res = new ViewModel.RekapResult();
                            res.EventType = "AutoRecognitionResultEx";
                            res.Name = item.Body.OriginExt.FriendlyName;
                            res.Direction = arx.Direction.Value;
                            res.TimeBegin = arx.TimeBegin.ToString();
                            res.TimeEnd = arx.TimeEnd.ToString();
                            if (arx.Hypotheses != null && arx.Hypotheses.Count > 0)
                            {
                                foreach (var w in arx.Hypotheses)
                                {
                                    res.HypothesisOcrQuality = w.OcrQuality;
                                    res.HypothesisPlateFull = w.PlateFull;
                                    res.HypothesisPlateRectangleX = w.PlateRectangle.X;
                                    res.HypothesisPlateRectangleY = w.PlateRectangle.Y;
                                    res.HypothesisPlateRectangleW = w.PlateRectangle.W;
                                    res.HypothesisPlateRectangleH = w.PlateRectangle.H;
                                    res.HypothesisPlateRectangleIndex = w.PlateRectangle.Index;
                                    res.HypothesisTimeBest = w.TimeBest;
                                    res.HypothesisCountry = w.Country;
                                    res.HypothesisPlateState = w.PlateState;
                                } 
                            }

                            
                            res.VehicleClass = arx.VehicleClass?.Value ?? "";
                            res.VehicleColor = arx.VehicleColor?.Value ?? "";
                            res.VehicleBrand = arx.VehicleBrand?.Value ?? "";
                            res.VehicleModel = arx.VehicleModel?.Value ?? "";
                            res.HeadlightsStatus = "";
                            res.VehicleSpeed = 0;
                            res.VehicleSpeedKmph = 0;
                            res.PlateType = arx.PlateType?.Value ?? "";
                            result.Add(res);
                        }
                    }
                }
            }
            
            return result;
        }
        public async Task<ViewModel.EventAll.EventAllResponse?> GetEventAll(int limit, int offset, string access_point, DateTime tgl_awal, DateTime tgl_akhir)
        {

            int attempt = 0;
            const int maxRetry = 5;
            var result = new ViewModel.EventAll.EventAllResponse();

            do
            {
                string url = "http://202.146.133.26/grpc";
                using (var client = new HttpClient())
                {

                    // Bearer Token Auth
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TOKEN_AUTH);

                    // Headers
                    client.DefaultRequestHeaders.Add("User-Agent", "insomnia/9.3.3");
                    var body = new
                    {
                        method = "axxonsoft.bl.events.EventHistoryService.ReadEvents",
                        data = new
                        {
                            range = new
                            {
                                begin_time = tgl_awal.ToString("yyyyMMdd'T'HHmmss.'000'"),
                                end_time = tgl_akhir.ToString("yyyyMMdd'T'HHmmss.'999'")
                            },
                            filters = new
                            {
                                filters = new[]
                                {
                                new {
                                    type = "ET_DetectorEvent",
                                    subjects = access_point
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
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var rawResponse = await response.Content.ReadAsStringAsync();
                        var apiResponse = Method.ConvertSseOutputJson(rawResponse);
                        var res = JsonSerializer.Deserialize<ViewModel.EventAll.EventAllResponse>(apiResponse);
                        if (res != null)
                        {
                            result = res;
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        Console.WriteLine($"[DEBUG] [GET_CAMERA_LIST] Token mungkin sudah kadaluarsa, refresh token dan ulangi...");

                        // Refresh token hanya sekali (di attempt pertama gagal 403)
                        await GenerateToken();
                    }
                    else
                    {
                        throw new Exception($"Gagal event kamera. Status: {response.StatusCode}, Message: {response.Content}");
                    }
                }

            }
            while (attempt < maxRetry);
            return result;
        }
    }

    public class Method
    {
        public static string ConvertSseOutputJson(string output)
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


    public class ViewModel
    {
        public class CameraList
        {
            public class AlternativeView
            {
                [JsonPropertyName("alternative_camera_name")]
                public string AlternativeCameraName { get; set; }

                [JsonPropertyName("second_alternative_camera_name")]
                public string SecondAlternativeCameraName { get; set; }
            }

            public class Archive
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("incomplete")]
                public bool Incomplete { get; set; }

                [JsonPropertyName("display_name")]
                public string DisplayName { get; set; }

                [JsonPropertyName("display_id")]
                public string DisplayId { get; set; }

                [JsonPropertyName("is_embedded")]
                public bool IsEmbedded { get; set; }

                [JsonPropertyName("archive_access")]
                public string ArchiveAccess { get; set; }

                [JsonPropertyName("bindings")]
                public List<object> Bindings { get; set; }

                [JsonPropertyName("is_activated")]
                public bool IsActivated { get; set; }

                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }
            }

            public class ArchiveBinding
            {
                [JsonPropertyName("name")]
                public string Name { get; set; }

                [JsonPropertyName("storage")]
                public string Storage { get; set; }

                [JsonPropertyName("archive")]
                public Archive Archive { get; set; }

                [JsonPropertyName("is_default")]
                public bool IsDefault { get; set; }

                [JsonPropertyName("is_replica")]
                public bool IsReplica { get; set; }

                [JsonPropertyName("is_permanent")]
                public bool IsPermanent { get; set; }

                [JsonPropertyName("has_live_sources")]
                public bool HasLiveSources { get; set; }

                [JsonPropertyName("has_replica_sources")]
                public bool HasReplicaSources { get; set; }

                [JsonPropertyName("sources")]
                public List<Source> Sources { get; set; }
            }

            public class Center
            {
                [JsonPropertyName("x")]
                public int X { get; set; }

                [JsonPropertyName("y")]
                public int Y { get; set; }
            }

            public class Circle
            {
                [JsonPropertyName("center")]
                public Center Center { get; set; }

                [JsonPropertyName("radius")]
                public int Radius { get; set; }
            }

            public class Detector
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("display_name")]
                public string DisplayName { get; set; }

                [JsonPropertyName("display_id")]
                public string DisplayId { get; set; }

                [JsonPropertyName("parent_detector")]
                public string ParentDetector { get; set; }

                [JsonPropertyName("type")]
                public string Type { get; set; }

                [JsonPropertyName("type_name")]
                public string TypeName { get; set; }

                [JsonPropertyName("is_activated")]
                public bool IsActivated { get; set; }

                [JsonPropertyName("groups")]
                public List<string> Groups { get; set; }

                [JsonPropertyName("scene_descriptions")]
                public List<SceneDescription> SceneDescriptions { get; set; }

                [JsonPropertyName("events")]
                public List<Event> Events { get; set; }

                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }

                [JsonPropertyName("is_realtime_recognition_enabled")]
                public bool IsRealtimeRecognitionEnabled { get; set; }

                [JsonPropertyName("is_recording_objects_tracking_enabled")]
                public bool IsRecordingObjectsTrackingEnabled { get; set; }
            }

            public class Event
            {
                [JsonPropertyName("id")]
                public string Id { get; set; }

                [JsonPropertyName("name")]
                public string Name { get; set; }

                [JsonPropertyName("event_type")]
                public string EventType { get; set; }
            }

            public class FisheyeCircles
            {
                [JsonPropertyName("circle")]
                public List<Circle> Circle { get; set; }
            }

            public class Item
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("incomplete")]
                public bool Incomplete { get; set; }

                [JsonPropertyName("display_name")]
                public string DisplayName { get; set; }

                [JsonPropertyName("display_id")]
                public string DisplayId { get; set; }

                [JsonPropertyName("ip_address")]
                public string IpAddress { get; set; }

                [JsonPropertyName("camera_access")]
                public string CameraAccess { get; set; }

                [JsonPropertyName("vendor")]
                public string Vendor { get; set; }

                [JsonPropertyName("model")]
                public string Model { get; set; }

                [JsonPropertyName("firmware")]
                public string Firmware { get; set; }

                [JsonPropertyName("comment")]
                public string Comment { get; set; }

                [JsonPropertyName("armed")]
                public bool Armed { get; set; }

                [JsonPropertyName("geo_location_latitude")]
                public string GeoLocationLatitude { get; set; }

                [JsonPropertyName("geo_location_longitude")]
                public string GeoLocationLongitude { get; set; }

                [JsonPropertyName("geo_location_azimuth")]
                public string GeoLocationAzimuth { get; set; }

                [JsonPropertyName("breaks_unused_connections")]
                public bool BreaksUnusedConnections { get; set; }

                [JsonPropertyName("serial_number")]
                public string SerialNumber { get; set; }

                [JsonPropertyName("video_streams")]
                public List<VideoStream> VideoStreams { get; set; }

                [JsonPropertyName("microphones")]
                public List<object> Microphones { get; set; }

                [JsonPropertyName("ptzs")]
                public List<object> Ptzs { get; set; }

                [JsonPropertyName("archive_bindings")]
                public List<ArchiveBinding> ArchiveBindings { get; set; }

                [JsonPropertyName("ray")]
                public List<object> Ray { get; set; }

                [JsonPropertyName("relay")]
                public List<object> Relay { get; set; }

                [JsonPropertyName("detectors")]
                public List<Detector> Detectors { get; set; }

                [JsonPropertyName("offline_detectors")]
                public List<object> OfflineDetectors { get; set; }

                [JsonPropertyName("group_ids")]
                public List<string> GroupIds { get; set; }

                [JsonPropertyName("is_activated")]
                public bool IsActivated { get; set; }

                [JsonPropertyName("text_sources")]
                public List<object> TextSources { get; set; }

                [JsonPropertyName("speakers")]
                public List<object> Speakers { get; set; }

                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }

                [JsonPropertyName("panomorph")]
                public Panomorph Panomorph { get; set; }

                [JsonPropertyName("video_buffer_size")]
                public int VideoBufferSize { get; set; }

                [JsonPropertyName("video_buffer_enabled")]
                public bool VideoBufferEnabled { get; set; }

                [JsonPropertyName("alternative_view")]
                public AlternativeView AlternativeView { get; set; }
            }

            public class Panomorph
            {
                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }

                [JsonPropertyName("fit_to_frame")]
                public bool FitToFrame { get; set; }

                [JsonPropertyName("camera_position")]
                public int CameraPosition { get; set; }

                [JsonPropertyName("view_type")]
                public int ViewType { get; set; }

                [JsonPropertyName("camera_lens")]
                public string CameraLens { get; set; }

                [JsonPropertyName("fisheye_circles")]
                public FisheyeCircles FisheyeCircles { get; set; }
            }

            public class CameraListResponse
            {
                [JsonPropertyName("items")]
                public List<Item> Items { get; set; }

                [JsonPropertyName("next_page_token")]
                public string NextPageToken { get; set; }

                [JsonPropertyName("search_meta_data")]
                public List<SearchMetaDatum> SearchMetaData { get; set; }
            }

            public class SceneDescription
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("mimetype")]
                public string Mimetype { get; set; }
            }

            public class SearchMetaDatum
            {
                [JsonPropertyName("score")]
                public int Score { get; set; }

                [JsonPropertyName("matches")]
                public List<object> Matches { get; set; }
            }

            public class Source
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("storage")]
                public string Storage { get; set; }

                [JsonPropertyName("binding")]
                public string Binding { get; set; }

                [JsonPropertyName("media_source")]
                public string MediaSource { get; set; }

                [JsonPropertyName("origin")]
                public string Origin { get; set; }

                [JsonPropertyName("mimetype")]
                public string Mimetype { get; set; }

                [JsonPropertyName("origin_storage")]
                public string OriginStorage { get; set; }

                [JsonPropertyName("origin_storage_source")]
                public string OriginStorageSource { get; set; }

                [JsonPropertyName("prerecording")]
                public int Prerecording { get; set; }
            }

            public class VideoStream
            {
                [JsonPropertyName("stream_acess_point")]
                public string StreamAcessPoint { get; set; }

                [JsonPropertyName("decoder_acess_point")]
                public string DecoderAcessPoint { get; set; }

                [JsonPropertyName("enabled")]
                public bool Enabled { get; set; }

                [JsonPropertyName("display_name")]
                public string DisplayName { get; set; }

                [JsonPropertyName("display_id")]
                public string DisplayId { get; set; }

                [JsonPropertyName("fps")]
                public int Fps { get; set; }

                [JsonPropertyName("is_activated")]
                public bool IsActivated { get; set; }
            }
        }

        public class EventAll
        {
            public class AutoRecognitionResult
            {
                [JsonPropertyName("direction")]
                public string Direction { get; set; }

                [JsonPropertyName("time_begin")]
                public string TimeBegin { get; set; }

                [JsonPropertyName("time_end")]
                public string TimeEnd { get; set; }

                [JsonPropertyName("hypotheses")]
                public List<Hypothesis> Hypotheses { get; set; }

                [JsonPropertyName("vehicle_class")]
                [JsonConverter(typeof(FlexibleStringConverter))]
                public string VehicleClass { get; set; }

                [JsonPropertyName("vehicle_color")]
                public string VehicleColor { get; set; }

                [JsonPropertyName("vehicle_brand")]
                public string VehicleBrand { get; set; }

                [JsonPropertyName("vehicle_model")]
                public string VehicleModel { get; set; }

                [JsonPropertyName("headlights_status")]
                public string HeadlightsStatus { get; set; }

                [JsonPropertyName("vehicle_speed")]
                public int VehicleSpeed { get; set; }

                [JsonPropertyName("vehicle_speed_kmph")]
                public int VehicleSpeedKmph { get; set; }

                [JsonPropertyName("plate_type")]
                public string PlateType { get; set; }
            }

            public class AutoRecognitionResultEx
            {
                [JsonPropertyName("hypotheses")]
                public List<Hypothesis> Hypotheses { get; set; }

                [JsonPropertyName("direction")]
                public Direction Direction { get; set; }

                [JsonPropertyName("vehicle_class")]
                public VehicleClass VehicleClass { get; set; }

                [JsonPropertyName("vehicle_color")]
                public VehicleColor VehicleColor { get; set; }

                [JsonPropertyName("vehicle_brand")]
                public VehicleBrand VehicleBrand { get; set; }

                [JsonPropertyName("vehicle_model")]
                public VehicleModel VehicleModel { get; set; }

                [JsonPropertyName("time_end")]
                public DateTime TimeEnd { get; set; }

                [JsonPropertyName("time_begin")]
                public DateTime TimeBegin { get; set; }

                [JsonPropertyName("plate_type")]
                public PlateType PlateType { get; set; }
            }

            public class Body
            {
                [JsonPropertyName("@type")]
                public string Type { get; set; }

                [JsonPropertyName("guid")]
                public string Guid { get; set; }

                [JsonPropertyName("timestamp")]
                public string Timestamp { get; set; }

                [JsonPropertyName("state")]
                public string State { get; set; }

                [JsonPropertyName("origin_deprecated")]
                public string OriginDeprecated { get; set; }

                [JsonPropertyName("origin_ext")]
                public OriginExt OriginExt { get; set; }

                [JsonPropertyName("offline_analytics_source")]
                public string OfflineAnalyticsSource { get; set; }

                [JsonPropertyName("detector_deprecated")]
                public string DetectorDeprecated { get; set; }

                [JsonPropertyName("detector_ext")]
                public DetectorExt DetectorExt { get; set; }

                [JsonPropertyName("node_info")]
                public NodeInfo NodeInfo { get; set; }

                [JsonPropertyName("event_type")]
                public string EventType { get; set; }

                [JsonPropertyName("multi_phase_id")]
                public string MultiPhaseId { get; set; }

                [JsonPropertyName("detectors_group")]
                public List<string> DetectorsGroup { get; set; }

                [JsonPropertyName("details")]
                public List<Detail> Details { get; set; }

                [JsonPropertyName("data")]
                public Data Data { get; set; }
            }

            public class Data
            {
                [JsonPropertyName("phase")]
                public int Phase { get; set; }

                [JsonPropertyName("TimeEnd")]
                public string TimeEnd { get; set; }

                [JsonPropertyName("TrackId")]
                public int TrackId { get; set; }

                [JsonPropertyName("ObjectId")]
                public int ObjectId { get; set; }

                [JsonPropertyName("TimeBest")]
                public string TimeBest { get; set; }

                [JsonPropertyName("Direction")]
                public int Direction { get; set; }

                [JsonPropertyName("Rectangle")]
                public List<double> Rectangle { get; set; }

                [JsonPropertyName("TimeBegin")]
                public string TimeBegin { get; set; }

                [JsonPropertyName("origin_id")]
                public string OriginId { get; set; }

                [JsonPropertyName("Hypotheses")]
                public List<Hypothesis> Hypotheses { get; set; }

                [JsonPropertyName("rectangles")]
                public List<List<double>> Rectangles { get; set; }

                [JsonPropertyName("VehicleBrand")]
                public string VehicleBrand { get; set; }

                [JsonPropertyName("VehicleClass")]
                public int VehicleClass { get; set; }

                [JsonPropertyName("VehicleColor")]
                public string VehicleColor { get; set; }

                [JsonPropertyName("VehicleModel")]
                public string VehicleModel { get; set; }

                [JsonPropertyName("detector_type")]
                public string DetectorType { get; set; }

                [JsonPropertyName("DetectorsGroup")]
                public List<string> DetectorsGroup { get; set; }

                [JsonPropertyName("PlateType")]
                public int? PlateType { get; set; }
            }

            public class Detail
            {
                [JsonPropertyName("rectangle")]
                public Rectangle Rectangle { get; set; }

                [JsonPropertyName("auto_recognition_result_ex")]
                public AutoRecognitionResultEx AutoRecognitionResultEx { get; set; }

                [JsonPropertyName("auto_recognition_result")]
                public AutoRecognitionResult AutoRecognitionResult { get; set; }
            }

            public class DetectorExt
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("friendly_name")]
                public string FriendlyName { get; set; }

                [JsonPropertyName("group")]
                public string Group { get; set; }
            }

            public class Direction
            {
                [JsonPropertyName("value")]
                public string Value { get; set; }
            }

            public class Hypothesis
            {
                [JsonPropertyName("ocr_quality")]
                public int OcrQuality { get; set; }

                [JsonPropertyName("plate_full")]
                public string PlateFull { get; set; }

                [JsonPropertyName("plate_rectangle")]
                public PlateRectangle PlateRectangle { get; set; }

                [JsonPropertyName("time_best")]
                public string TimeBest { get; set; }

                [JsonPropertyName("country")]
                public string Country { get; set; }

                [JsonPropertyName("plate_state")]
                public string PlateState { get; set; }
            }

            public class Hypothesis3
            {
                [JsonPropertyName("TimeBest")]
                public string TimeBest { get; set; }

                [JsonPropertyName("PlateFull")]
                public string PlateFull { get; set; }

                [JsonPropertyName("OCRQuality")]
                public int OCRQuality { get; set; }

                [JsonPropertyName("PlateCountry")]
                public string PlateCountry { get; set; }

                [JsonPropertyName("PlateRectangle")]
                public List<double> PlateRectangle { get; set; }
            }

            public class Item
            {
                [JsonPropertyName("event_type")]
                public string EventType { get; set; }

                [JsonPropertyName("subject")]
                public string Subject { get; set; }

                [JsonPropertyName("event_name")]
                public string EventName { get; set; }

                [JsonPropertyName("body")]
                public Body Body { get; set; }

                [JsonPropertyName("subjects")]
                public List<string> Subjects { get; set; }

                [JsonPropertyName("external")]
                public bool External { get; set; }

                [JsonPropertyName("localization")]
                public Localization Localization { get; set; }

                [JsonPropertyName("required_permissions")]
                public RequiredPermissions RequiredPermissions { get; set; }
            }

            public class Localization
            {
                [JsonPropertyName("text")]
                public string Text { get; set; }
            }

            public class NodeInfo
            {
                [JsonPropertyName("name")]
                public string Name { get; set; }

                [JsonPropertyName("friendly_name")]
                public string FriendlyName { get; set; }
            }

            public class OriginExt
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("friendly_name")]
                public string FriendlyName { get; set; }

                [JsonPropertyName("group")]
                public string Group { get; set; }
            }

            public class PlateRectangle
            {
                [JsonPropertyName("x")]
                public double X { get; set; }

                [JsonPropertyName("y")]
                public double Y { get; set; }

                [JsonPropertyName("w")]
                public double W { get; set; }

                [JsonPropertyName("h")]
                public double H { get; set; }

                [JsonPropertyName("index")]
                public int Index { get; set; }
            }

            public class PlateType
            {
                [JsonPropertyName("value")]
                public string Value { get; set; }
            }

            public class Rectangle
            {
                [JsonPropertyName("x")]
                public double X { get; set; }

                [JsonPropertyName("y")]
                public double Y { get; set; }

                [JsonPropertyName("w")]
                public double W { get; set; }

                [JsonPropertyName("h")]
                public double H { get; set; }

                [JsonPropertyName("index")]
                public int Index { get; set; }
            }

            public class RequiredObjectPermission
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("camera_access")]
                public string CameraAccess { get; set; }
            }

            public class RequiredPermissions
            {
                [JsonPropertyName("required_object_permissions")]
                public List<RequiredObjectPermission> RequiredObjectPermissions { get; set; }
            }

            public class EventAllResponse
            {
                [JsonPropertyName("items")]
                public List<Item> Items { get; set; }

                [JsonPropertyName("unreachable_subjects")]
                public List<object> UnreachableSubjects { get; set; }
            }

            public class VehicleBrand
            {
                [JsonPropertyName("value")]
                public string Value { get; set; }
            }

            public class VehicleClass
            {
                [JsonPropertyName("value")]
                [JsonConverter(typeof(FlexibleStringConverter))]
                public string Value { get; set; }
            }

            public class VehicleColor
            {
                [JsonPropertyName("value")]
                public string Value { get; set; }
            }

            public class VehicleModel
            {
                [JsonPropertyName("value")]
                public string Value { get; set; }
            }


        }


        public class RekapResult
        {
            public string EventType { get; set; }
            public string Name { get; set; }
            public string Direction { get; set; }
            public string TimeBegin { get; set; }
            public string TimeEnd { get; set; }
            public int HypothesisOcrQuality { get; set; }
            public string HypothesisPlateFull { get; set; }
            public double HypothesisPlateRectangleX { get; set; }
            public double HypothesisPlateRectangleY { get; set; }
            public double HypothesisPlateRectangleW { get; set; }
            public double HypothesisPlateRectangleH { get; set; }
            public int HypothesisPlateRectangleIndex { get; set; }
            public string HypothesisTimeBest { get; set; }
            public string HypothesisCountry { get; set; }
            public string HypothesisPlateState { get; set; }
            [JsonConverter(typeof(FlexibleStringConverter))]
            public string VehicleClass { get; set; }
            public string VehicleColor { get; set; }
            public string VehicleBrand { get; set; }
            public string VehicleModel { get; set; }
            public string HeadlightsStatus { get; set; }
            public int VehicleSpeed { get; set; }
            public int VehicleSpeedKmph { get; set; }
            public string PlateType { get; set; }
        }
    }
}
