using System.Text.Json.Serialization;

namespace CctvRealtimeWs.TelkomModels
{
    public class TelkomEvent
    {
        public class Result
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("id_camera")]
            public int IdCamera { get; set; }

            [JsonPropertyName("daydate")]
            public string Daydate { get; set; }

            [JsonPropertyName("timestamp")]
            public string Timestamp { get; set; }

            [JsonPropertyName("image")]
            public string Image { get; set; }

            [JsonPropertyName("tipe_kendaraan")]
            public string TipeKendaraan { get; set; }

            [JsonPropertyName("plat_nomor")]
            public string PlatNomor { get; set; }
        }

        public class TelkomEventResponse
        {
            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("result")]
            public List<Result> Result { get; set; }

            [JsonPropertyName("success")]
            public bool Success { get; set; }
        }
    }
}
