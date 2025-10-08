using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CctvRealtimeWs
{
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
}
