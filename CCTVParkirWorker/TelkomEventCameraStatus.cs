using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CCTVParkirWorker
{
    public class TelkomEventCameraStatus
    {
        public class Result
        {
            [JsonPropertyName("id_analytics")]
            public int IdAnalytics { get; set; }

            [JsonPropertyName("id_cctv")]
            public int IdCctv { get; set; }

            [JsonPropertyName("camera_name")]
            public string CameraName { get; set; }

            [JsonPropertyName("analytics_usecase")]
            public string AnalyticsUsecase { get; set; }

            [JsonPropertyName("rtsp")]
            public string Rtsp { get; set; }

            [JsonPropertyName("roi_type")]
            public string RoiType { get; set; }

            [JsonPropertyName("path")]
            public string Path { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonPropertyName("status_update")]
            public string StatusUpdate { get; set; }
        }

        public class TelkomEventCameraStatusResponse
        {
            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("result")]
            public List<Result> Result { get; set; }
        }
    }
}
