using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace VendorCctvAPI.Models
{
    public class TelkomVM
    {
        public class Method
        {
            public static async Task SendTelkomLogAsync(List<ViewModel.TelkomEvent> eventList)
            {
                using var context = DBClass.GetContext();

                var distinctId = eventList
                    .Select(x => x.IdAnalytics)
                    .Distinct()
                    .ToList();

                var dataTelkom = await context.MOpParkirCctvTelkoms
                    .Where(x => distinctId.Contains(x.CctvId))
                    .ToListAsync();

                var insertData = new List<MOpParkirCctvTelkomLogD>();

                foreach (var item in eventList)
                {
                    if (!DateTime.TryParseExact(
                        item.TimeStamp,
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime parsedDate))
                    {
                        throw new ArgumentException($"Format tanggal salah untuk Access Point {item.IdAnalytics}. Gunakan format 'yyyy-MM-dd HH:mm:ss'.");
                    }

                    // Validasi access point
                    var data = dataTelkom.FirstOrDefault(x => x.CctvId.Trim() == item.IdAnalytics.Trim());
                    if (data == null)
                    {
                        throw new ArgumentException($"Access Point '{item.IdAnalytics}' tidak ditemukan di database.");
                    }

                    // Simpan log detail
                    string id = $"{item.IdAnalytics}-{parsedDate:yyyyMMddHHmmss}";
                    insertData.Add(new MOpParkirCctvTelkomLogD
                    {
                        Guid = id,
                        Nop = data.Nop,
                        CctvId = data.CctvId,
                        TglEvent = parsedDate,
                        Event = item.DescriptionState,
                        IsOn = item.IsOn ? 1 : 0
                    });
                }

                // Insert log detail (bulk insert)
                await context.MOpParkirCctvTelkomLogDs.AddRangeAsync(insertData);

                // Hitung status terakhir per CCTV
                var dataLastStatus = insertData
                    .GroupBy(x => new { x.Nop, x.CctvId })
                    .Select(g => new
                    {
                        Nop = g.Key.Nop,
                        CctvId = g.Key.CctvId,
                        LastOn = g
                            .Where(x => x.IsOn == 1)
                            .OrderByDescending(x => x.TglEvent)
                            .FirstOrDefault(),
                        LastOff = g
                            .Where(x => x.IsOn == 0)
                            .OrderByDescending(x => x.TglEvent)
                            .FirstOrDefault()
                    })
                    .ToList();

                foreach (var item in dataLastStatus)
                {
                    var data = dataTelkom.FirstOrDefault(x => x.Nop.Trim() == item.Nop.Trim() && x.CctvId == item.CctvId);
                    if (data == null)
                    {
                        throw new ArgumentException($"Nop '{item.Nop}' tidak ditemukan di database.");
                    }

                    var oldData = context.MOpParkirCctvTelkomLogs.FirstOrDefault(x => x.Nop == item.Nop && x.CctvId == item.CctvId);

                    DateTime? tanggalTerakhirAktif = item.LastOn?.TglEvent;
                    DateTime? tanggalTerakhirNonAktif = item.LastOff?.TglEvent;

                    if (!tanggalTerakhirAktif.HasValue && !tanggalTerakhirNonAktif.HasValue)
                    {
                        throw new ArgumentException($"Nop '{item.Nop}' aktif dan non aktif harus ada");
                    }

                    string status = "NON AKTIF";

                    if (!tanggalTerakhirAktif.HasValue && !tanggalTerakhirNonAktif.HasValue)
                    {
                        throw new ArgumentException($"Nop '{item.Nop}' aktif dan non aktif harus ada");
                    }
                    else if (!tanggalTerakhirAktif.HasValue)
                    {
                        status = "NON AKTIF";
                    }
                    else if (!tanggalTerakhirNonAktif.HasValue)
                    {
                        status = "AKTIF";
                    }
                    else
                    {
                        // Ambil yang terbaru
                        status = (tanggalTerakhirAktif.Value > tanggalTerakhirNonAktif.Value) ? "AKTIF" : "NON AKTIF";
                    }


                    if (oldData != null)
                    {
                        oldData.TglTerakhirAktif = tanggalTerakhirAktif ?? oldData.TglTerakhirAktif;
                        oldData.TglTerakhirDown = tanggalTerakhirNonAktif ?? oldData.TglTerakhirDown;
                        oldData.Status = status;
                    }
                    else
                    {
                        var insert = new MOpParkirCctvTelkomLog();

                        insert.Nop = item.Nop;
                        insert.CctvId = item.CctvId;
                        insert.TglTerakhirAktif = tanggalTerakhirAktif ?? DateTime.Now.Date;
                        insert.TglTerakhirDown = tanggalTerakhirNonAktif ?? DateTime.Now.Date;
                        insert.Status = status;


                        await context.MOpParkirCctvTelkomLogs.AddAsync(insert);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
        public class ViewModel
        {
            public class TelkomEvent
            {
                [JsonPropertyName("id_analytics")]
                [SwaggerSchema(Description = "ID kamera.")]
                public string IdAnalytics { get; set; }

                [JsonPropertyName("timestamp")]
                [SwaggerSchema(Description = "Waktu event, format: yyyy-MM-dd HH:mm:ss")]
                public string TimeStamp { get; set; }

                [JsonPropertyName("description_state")]
                [SwaggerSchema(Description = "State Event")]
                public string DescriptionState { get; set; }

                [JsonPropertyName("is_on")]
                [SwaggerSchema(Description = "if restored = true else false")]
                public bool IsOn { get; set; }
            }
        }
    }
}
