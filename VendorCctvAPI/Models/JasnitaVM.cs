using MonPDLib;
using MonPDLib.EF;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace VendorCctvAPI.Models
{
    public class JasnitaVM
    {
        public class Method
        {
            public static void SendJasnitaLog(List<ViewModel.JasnitaEvent> eventList)
            {
                var context = DBClass.GetContext();

                foreach (var item in eventList)
                {
                    if (!DateTime.TryParseExact(item.TimeStamp, "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        throw new ArgumentException($"Format tanggal salah untuk Access Point {item.AccessPoint}. Gunakan format 'yyyy-MM-dd HH:mm:ss'.");
                    }

                    // Contoh validasi access point di DB
                    var data = context.MOpParkirCctvJasnita
                        .FirstOrDefault(x => x.AccessPoint.Trim() == item.AccessPoint.Trim());

                    if (data == null)
                    {
                        throw new ArgumentException($"Access Point '{item.AccessPoint}' tidak ditemukan di database.");
                    }

                    //// Contoh: simpan ke tabel log
                    //var log = new TOpParkirCctvLog
                    //{
                    //    AccessPoint = item.Access_Point.Trim(),
                    //    TanggalEvent = parsedDate,
                    //    EventState = item.Event_State,
                    //    IsOn = item.IsOn
                    //};

                    //context.TOpParkirCctvLogs.Add(log);
                }

                //context.SaveChanges();
            }
        }

        public class ViewModel
        {
            public class JasnitaEvent
            {
                [JsonPropertyName("access_point")]
                public string AccessPoint { get; set; }

                [JsonPropertyName("timestamp")]
                public string TimeStamp { get; set; }
                [JsonPropertyName("description_state")]
                public string DescriptionState { get; set; }

                [JsonPropertyName("is_on")]
                public bool IsOn { get; set; }
            }
        }
    }
}
