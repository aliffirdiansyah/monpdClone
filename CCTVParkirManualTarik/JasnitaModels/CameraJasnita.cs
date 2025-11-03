using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CCTVParkirManualTarik.JasnitaModels
{
    public class CameraJasnita
    {
        public class Archive
        {
            [JsonPropertyName("accessPoint")]
            public string AccessPoint { get; set; }

            [JsonPropertyName("default")]
            public bool Default { get; set; }

            [JsonPropertyName("incomplete")]
            public bool Incomplete { get; set; }

            [JsonPropertyName("isEmbedded")]
            public bool IsEmbedded { get; set; }

            [JsonPropertyName("storage")]
            public string Storage { get; set; }

            [JsonPropertyName("storageDisplayName")]
            public string StorageDisplayName { get; set; }
        }

        public class AudioStream
        {
            [JsonPropertyName("accessPoint")]
            public string AccessPoint { get; set; }

            [JsonPropertyName("enabled")]
            public bool Enabled { get; set; }

            [JsonPropertyName("isActivated")]
            public bool IsActivated { get; set; }
        }

        public class Camera
        {
            [JsonPropertyName("accessPoint")]
            public string AccessPoint { get; set; }

            [JsonPropertyName("archives")]
            public List<Archive> Archives { get; set; }

            [JsonPropertyName("arm_state")]
            public string ArmState { get; set; }

            [JsonPropertyName("audioStreams")]
            public List<AudioStream> AudioStreams { get; set; }

            [JsonPropertyName("azimuth")]
            public string Azimuth { get; set; }

            [JsonPropertyName("camera_access")]
            public string CameraAccess { get; set; }

            [JsonPropertyName("comment")]
            public string Comment { get; set; }

            [JsonPropertyName("detectors")]
            public List<Detector> Detectors { get; set; }

            [JsonPropertyName("displayId")]
            public string DisplayId { get; set; }

            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }

            [JsonPropertyName("enabled")]
            public bool Enabled { get; set; }

            [JsonPropertyName("groups")]
            public List<string> Groups { get; set; }

            [JsonPropertyName("ipAddress")]
            public string IpAddress { get; set; }

            [JsonPropertyName("isActivated")]
            public bool IsActivated { get; set; }

            [JsonPropertyName("latitude")]
            public string Latitude { get; set; }

            [JsonPropertyName("longitude")]
            public string Longitude { get; set; }

            [JsonPropertyName("model")]
            public string Model { get; set; }

            [JsonPropertyName("offlineDetectors")]
            public List<object> OfflineDetectors { get; set; }

            [JsonPropertyName("panomorph")]
            public bool Panomorph { get; set; }

            [JsonPropertyName("ptzs")]
            public List<object> Ptzs { get; set; }

            [JsonPropertyName("rays")]
            public List<object> Rays { get; set; }

            [JsonPropertyName("speakers")]
            public List<object> Speakers { get; set; }

            [JsonPropertyName("textSources")]
            public List<object> TextSources { get; set; }

            [JsonPropertyName("vendor")]
            public string Vendor { get; set; }

            [JsonPropertyName("videoStreams")]
            public List<VideoStream> VideoStreams { get; set; }
        }

        public class Detector
        {
            [JsonPropertyName("accessPoint")]
            public string AccessPoint { get; set; }

            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }

            [JsonPropertyName("events")]
            public List<string> Events { get; set; }

            [JsonPropertyName("isActivated")]
            public bool IsActivated { get; set; }

            [JsonPropertyName("parentDetector")]
            public string ParentDetector { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }

        public class CameraJasnitaResponse
        {
            [JsonPropertyName("cameras")]
            public List<Camera> Cameras { get; set; }

            [JsonPropertyName("search_meta_data")]
            public List<SearchMetaDatum> SearchMetaData { get; set; }
        }

        public class SearchMetaDatum
        {
            [JsonPropertyName("matches")]
            public List<object> Matches { get; set; }

            [JsonPropertyName("score")]
            public int Score { get; set; }
        }

        public class VideoStream
        {
            [JsonPropertyName("accessPoint")]
            public string AccessPoint { get; set; }

            [JsonPropertyName("enabled")]
            public bool Enabled { get; set; }
        }


    }
}
