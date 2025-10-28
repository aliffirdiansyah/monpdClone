using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CctvRealtimeWs.JasnitaModels
{
    public class EventV2
    {
        public class Event
        {
            [JsonPropertyName("alertState")]
            public string AlertState { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("multiPhaseSyncId")]
            public string MultiPhaseSyncId { get; set; }

            [JsonPropertyName("origin")]
            public string Origin { get; set; }

            [JsonPropertyName("rectangles")]
            public List<Rectangle> Rectangles { get; set; }

            [JsonPropertyName("source")]
            public string Source { get; set; }

            [JsonPropertyName("timestamp")]
            public string Timestamp { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }

        public class Rectangle
        {
            [JsonPropertyName("bottom")]
            public double Bottom { get; set; }

            [JsonPropertyName("index")]
            public int Index { get; set; }

            [JsonPropertyName("left")]
            public double Left { get; set; }

            [JsonPropertyName("right")]
            public double Right { get; set; }

            [JsonPropertyName("top")]
            public double Top { get; set; }
        }

        public class EventV2Response
        {
            [JsonPropertyName("events")]
            public List<Event> Events { get; set; }

            [JsonPropertyName("more")]
            public bool More { get; set; }
        }
    }
}
