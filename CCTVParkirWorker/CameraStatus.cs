using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CCTVParkirWorker
{
    public class CameraStatus
    {
        public class Body
        {
            [JsonPropertyName("@type")]
            public string Type { get; set; }

            [JsonPropertyName("object_id_deprecated")]
            public string ObjectIdDeprecated { get; set; }

            [JsonPropertyName("timestamp")]
            public string Timestamp { get; set; }

            [JsonPropertyName("state")]
            public string State { get; set; }

            [JsonPropertyName("guid")]
            public string Guid { get; set; }

            [JsonPropertyName("object_id_ext")]
            public ObjectIdExt ObjectIdExt { get; set; }

            [JsonPropertyName("node_info")]
            public NodeInfo NodeInfo { get; set; }

            [JsonPropertyName("prev_timestamp")]
            public string PrevTimestamp { get; set; }

            [JsonPropertyName("extra")]
            public string Extra { get; set; }
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

        public class ObjectIdExt
        {
            [JsonPropertyName("access_point")]
            public string AccessPoint { get; set; }

            [JsonPropertyName("friendly_name")]
            public string FriendlyName { get; set; }

            [JsonPropertyName("group")]
            public string Group { get; set; }
        }

        public class CameraStatusResponse
        {
            [JsonPropertyName("items")]
            public List<Item> Items { get; set; }

            [JsonPropertyName("unreachable_subjects")]
            public List<object> UnreachableSubjects { get; set; }
        }
    }
}
