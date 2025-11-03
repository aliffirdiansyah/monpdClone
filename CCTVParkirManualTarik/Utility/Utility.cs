using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCTVParkirManualTarik.Utility
{
    public static class Utility
    {

        #region Utility
        public static EnumFactory.EJenisKendParkirCCTV GetJenisKendaraan(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "CAR":
                    return EnumFactory.EJenisKendParkirCCTV.Mobil;
                case "MOTORCYCLE":
                    return EnumFactory.EJenisKendParkirCCTV.Motor;
                case "TRUCK":
                    return EnumFactory.EJenisKendParkirCCTV.Truck;
                case "BUS":
                    return EnumFactory.EJenisKendParkirCCTV.Bus;
                default:
                    return EnumFactory.EJenisKendParkirCCTV.Unknown;
            }
        }
        public static EnumFactory.CctvParkirDirection GetDirection(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "INCOMING":
                    return EnumFactory.CctvParkirDirection.Incoming;
                case "OUTGOING":
                    return EnumFactory.CctvParkirDirection.Outgoing;
                default:
                    return EnumFactory.CctvParkirDirection.Unknown;
            }
        }
        public static EnumFactory.EStatusCCTV GetStatusCctv(string input)
        {
            input = input.ToUpper().Trim();
            switch (input)
            {
                case "IPDS_SIGNAL_RESTORED":
                    return EnumFactory.EStatusCCTV.Aktif;
                default:
                    return EnumFactory.EStatusCCTV.NonAktif;
            }
        }
        public static DateTime ParseFlexibleDate(string timeStr)
        {
            if (string.IsNullOrWhiteSpace(timeStr))
            {
                throw new ArgumentException("timeStr tidak boleh kosong");
            }

            var formats = new[]
            {
        "yyyyMMdd'T'HHmmss.ffffff",  // 20250924T083638.867000
        "yyyyMMdd'T'HHmmss.fff",     // 20250924T083638.867
        "yyyyMMdd'T'HHmmss",         // ✅ 20250925T073926 (kasus kamu)
        "yyyy-MM-dd'T'HH:mm:ss.ffffff",
        "yyyy-MM-dd'T'HH:mm:ss.fff",
        "yyyy-MM-dd'T'HH:mm:ss",
        "yyyyMMddHHmmss",            // 20250925073926 (tanpa 'T')
        "yyyy-MM-dd HH:mm:ss",       // 2025-09-25 07:39:26
        "yyyy/MM/dd HH:mm:ss",       // 2025/09/25 07:39:26
        "yyyyMMdd'T'HHmmssZ",        // 20250925T073926Z
        "yyyy-MM-dd'T'HH:mm:ssZ",    // 2025-09-25T07:39:26Z
        "yyyy-MM-dd'T'HH:mm:ssK"     // 2025-09-25T07:39:26+07:00
    };

            foreach (var fmt in formats)
            {
                if (DateTime.TryParseExact(
                    timeStr,
                    fmt,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AdjustToUniversal,
                    out DateTime result))
                {
                    return result;
                }
            }

            throw new Exception(timeStr + " tidak sesuai format yang dikenali.");
        }
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
        public static string ConvertSseOutputJson2(string output)
        {
            try
            {
                var jsonObjects = output.Split('\n')
                    .Where(l => l.StartsWith("data:"))
                    .Select(l => l.Substring("data:".Length).Trim())
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Where(l => !l.Contains("\"items\":[]"))
                    .ToList();

                if (jsonObjects.Count == 0)
                    return "[]";

                var jsonArray = "[" + string.Join(",", jsonObjects) + "]";
                return jsonArray;
            }
            catch (Exception)
            {
                return output;
            }
        }
        public static string GetFullExceptionMessage(Exception ex)
        {
            var sb = new StringBuilder();
            int level = 0;

            while (ex != null)
            {
                // Pilih warna berdasarkan level
                var color = level switch
                {
                    0 => ConsoleColor.Red,
                    1 => ConsoleColor.Yellow,
                    2 => ConsoleColor.Cyan,
                    _ => ConsoleColor.Gray
                };

                // Tulis ke console langsung (lebih interaktif)
                Console.ForegroundColor = color;
                Console.WriteLine($"[Level {level}] {ex.GetType().Name}: {ex.Message}");
                Console.ResetColor();

                // Simpan juga di string builder kalau mau di-log ke file
                sb.AppendLine($"[Level {level}] {ex.GetType().Name}: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    sb.AppendLine(ex.StackTrace);
                }

                ex = ex.InnerException;
                level++;
            }

            return sb.ToString();
        }
        // Konversi dari UTC (+0) ke WIB (+7)
        public static DateTime ConvertUtcToWib(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(utcTime, DateTimeKind.Utc),
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
            );
        }

        // Konversi dari WIB (+7) ke UTC (+0)
        public static DateTime ConvertWibToUtc(DateTime wibTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(
                DateTime.SpecifyKind(wibTime, DateTimeKind.Local)
            );
        }
        #endregion
    }
}
