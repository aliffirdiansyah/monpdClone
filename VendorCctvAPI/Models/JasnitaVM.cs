using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.Json.Serialization;

namespace VendorCctvAPI.Models
{
    public class JasnitaVM
    {
        public class Method
        {
            public static async Task SendJasnitaLogAsync(List<ViewModel.JasnitaEvent> eventList)
            {
                using var context = DBClass.GetContext();

                var distinctAccessPoint = eventList
                    .Select(x => x.AccessPoint)
                    .Distinct()
                    .ToList();

                var dataJasnita = await context.MOpParkirCctvJasnita
                    .Where(x => distinctAccessPoint.Contains(x.AccessPoint))
                    .ToListAsync();

                var insertData = new List<MOpParkirCctvJasnitaLogD>();

                foreach (var item in eventList)
                {
                    if (!DateTime.TryParseExact(
                        item.TimeStamp,
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime parsedDate))
                    {
                        throw new ArgumentException($"Format tanggal salah untuk Access Point {item.AccessPoint}. Gunakan format 'yyyy-MM-dd HH:mm:ss'.");
                    }

                    // Validasi access point
                    var data = dataJasnita.FirstOrDefault(x => x.AccessPoint.Trim() == item.AccessPoint.Trim());
                    if (data == null)
                    {
                        throw new ArgumentException($"Access Point '{item.AccessPoint}' tidak ditemukan di database.");
                    }

                    // Simpan log detail
                    string id = $"{item.AccessPoint}-{parsedDate:yyyyMMddHHmmss}";
                    insertData.Add(new MOpParkirCctvJasnitaLogD
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
                await context.MOpParkirCctvJasnitaLogDs.AddRangeAsync(insertData);

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
                    var data = dataJasnita.FirstOrDefault(x => x.Nop.Trim() == item.Nop.Trim() && x.CctvId == item.CctvId);
                    if (data == null)
                    {
                        throw new ArgumentException($"Nop '{item.Nop}' tidak ditemukan di database.");
                    }

                    string status = "NON AKTIF";

                    if (item.LastOn != null && item.LastOff != null)
                    {
                        status = item.LastOn.TglEvent > item.LastOff.TglEvent ? "AKTIF" : "NON AKTIF";
                    }
                    else if (item.LastOn != null)
                    {
                        status = "AKTIF";
                    }

                    var oldData = await context.MOpParkirCctvJasnitaLogs
                        .FirstOrDefaultAsync(x => x.Nop == item.Nop && x.CctvId == item.CctvId);

                    if (oldData != null)
                    {
                        oldData.TglTerakhirAktif = item.LastOn?.TglEvent ?? DateTime.Now;
                        oldData.TglTerakhirDown = item.LastOff?.TglEvent ?? DateTime.Now;
                        oldData.Status = status;
                    }
                    else
                    {
                        await context.MOpParkirCctvJasnitaLogs.AddAsync(new MOpParkirCctvJasnitaLog
                        {
                            Nop = item.Nop,
                            CctvId = item.CctvId,
                            TglTerakhirAktif = item.LastOn?.TglEvent ?? DateTime.Now,
                            TglTerakhirDown = item.LastOff?.TglEvent ?? DateTime.Now,
                            Status = status
                        });
                    }
                }

                await context.SaveChangesAsync();
            }

        }

        public class ViewModel
        {
            public class JasnitaEvent
            {
                [JsonPropertyName("access_point")]
                [SwaggerSchema(Description = "ID kamera.")]
                public string AccessPoint { get; set; }

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
