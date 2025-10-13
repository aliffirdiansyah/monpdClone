using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonPDLib.General;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using static MonPDLib.Helper;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileSpasialOPUploadVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public IFormFile FileExcel { get; set; } = null!;
            public int Tahun { get; set; }

            public List<SelectListItem>? TahunList { get; set; } // untuk dropdown
            public Index()
            {

            }

        }
        public class Method
        {
            public static void UploadSpasial(IFormFile fileExcel)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets[0];
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                int totalRows = sheet.Dimension.End.Row - 1;
                Console.WriteLine($"Memulai upload spasial... Total baris: {totalRows}");
                Console.WriteLine("============================================================");

                // 🔹 Dictionary cache dari database
                var restoDict = context.DbOpRestos.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var listrikDict = context.DbOpListriks.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var hotelDict = context.DbOpHotels.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var parkirDict = context.DbOpParkirs.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var hiburanDict = context.DbOpHiburans.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var abtDict = context.DbOpAbts.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var wilayahDict = context.MWilayahs.AsEnumerable().GroupBy(x => x.KdKecamatan + x.KdKelurahan).ToDictionary(g => g.Key, g => g.First());

                // 🟢 Kumpulkan semua NOP dari Excel
                var listNop = new List<string>();
                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                    listNop.Add(sheet.Cells[row, 1].Text.Replace(".", "").Trim());

                // 🧹 Hapus data lama
                var existing = context.DbOpLocations.Where(x => listNop.Contains(x.FkNop)).ToList();
                if (existing.Any())
                {
                    context.DbOpLocations.RemoveRange(existing);
                    context.SaveChanges();
                }

                // 🧩 Siapkan list data baru
                var listDataBaru = new ConcurrentBag<DbOpLocation>();
                int totalProcessed = 0;
                int progressBarWidth = 40;

                Parallel.For(2, sheet.Dimension.End.Row + 1, new ParallelOptions { MaxDegreeOfParallelism = 5 }, row =>
                {
                    var nop = sheet.Cells[row, 1].Text.Replace(".", "");
                    var pajakId = (EnumFactory.EPajak)TryDecimal(sheet.Cells[row, 4].Text);

                    string namaOp = "-";
                    string alamatOp = "-";
                    string kelurahan = "-";
                    string kecamatan = "-";

                    switch (pajakId)
                    {
                        case EnumFactory.EPajak.MakananMinuman:
                            if (restoDict.TryGetValue(nop, out var resto))
                            {
                                namaOp = resto.NamaOp;
                                alamatOp = resto.AlamatOp;
                                var key = (resto.AlamatOpKdCamat ?? "") + (resto.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case EnumFactory.EPajak.TenagaListrik:
                            if (listrikDict.TryGetValue(nop, out var l))
                            {
                                namaOp = l.NamaOp;
                                alamatOp = l.AlamatOp;
                                var key = (l.AlamatOpKdCamat ?? "") + (l.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case EnumFactory.EPajak.JasaPerhotelan:
                            if (hotelDict.TryGetValue(nop, out var h))
                            {
                                namaOp = h.NamaOp;
                                alamatOp = h.AlamatOp;
                                var key = (h.AlamatOpKdCamat ?? "") + (h.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case EnumFactory.EPajak.JasaParkir:
                            if (parkirDict.TryGetValue(nop, out var p))
                            {
                                namaOp = p.NamaOp;
                                alamatOp = p.AlamatOp;
                                var key = (p.AlamatOpKdCamat ?? "") + (p.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case EnumFactory.EPajak.JasaKesenianHiburan:
                            if (hiburanDict.TryGetValue(nop, out var hi))
                            {
                                namaOp = hi.NamaOp;
                                alamatOp = hi.AlamatOp;
                                var key = (hi.AlamatOpKdCamat ?? "") + (hi.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case EnumFactory.EPajak.AirTanah:
                            if (abtDict.TryGetValue(nop, out var a))
                            {
                                namaOp = a.NamaOp;
                                alamatOp = a.AlamatOp;
                                var key = (a.AlamatOpKdCamat ?? "") + (a.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                    }

                    // 🧩 Ambil lat lon bawaan Excel
                    string latExcel = sheet.Cells[row, 2].Text.Trim();
                    string lonExcel = sheet.Cells[row, 3].Text.Trim();

                    // ⏩ Upload cepat, lat/lon sementara dari Excel
                    listDataBaru.Add(new DbOpLocation
                    {
                        FkNop = nop,
                        Nama = namaOp,
                        Alamat = alamatOp,
                        Latitude = latExcel,
                        Longitude = lonExcel,
                        PajakId = (decimal)pajakId
                    });

                    // 🔵 Progress
                    int done = Interlocked.Increment(ref totalProcessed);
                    double progress = (double)done / totalRows;
                    int filledBars = (int)(progress * progressBarWidth);
                    string bar = "[" + new string('#', filledBars) + new string('-', progressBarWidth - filledBars) + "]";
                    Console.Write($"\r{bar} {done}/{totalRows} ({progress:P0})");
                });

                // 🟢 Simpan semua ke DB
                Console.WriteLine("\nMenyimpan ke database...");
                context.DbOpLocations.AddRange(listDataBaru);
                context.SaveChanges();
                Console.WriteLine("Upload selesai ✅");

                // 🚀 Jalankan geocoding di background (cek semua data)
                Task.Run(() => ProsesGeocodingAsync());
            }
            public static async Task ProsesGeocodingAsync()
            {
                Console.WriteLine("🚀 Memulai proses geocoding lat/lon di background...");
                using var context = DBClass.GetContext();
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "MonPD/1.0");

                var startTime = DateTime.Now;

                // 🔹 Dictionary cache dari database
                var restoDict = context.DbOpRestos.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var listrikDict = context.DbOpListriks.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var hotelDict = context.DbOpHotels.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var parkirDict = context.DbOpParkirs.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var hiburanDict = context.DbOpHiburans.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var abtDict = context.DbOpAbts.AsEnumerable().GroupBy(x => x.Nop).ToDictionary(g => g.Key, g => g.First());
                var wilayahDict = context.MWilayahs.AsEnumerable().GroupBy(x => x.KdKecamatan + x.KdKelurahan).ToDictionary(g => g.Key, g => g.First());

                var semuaData = context.DbOpLocations.ToList();
                int totalData = semuaData.Count;
                int count = 0;

                Console.WriteLine($"📦 Total data yang akan diproses: {totalData:N0}");

                foreach (var item in semuaData)
                {
                    string? namaOp = null, alamatOp = null, kelurahan = null, kecamatan = null;
                    string nop = item.FkNop ?? "";

                    // 🧩 Deteksi jenis pajak dari item
                    switch (item.PajakId)
                    {
                        case (decimal)EnumFactory.EPajak.MakananMinuman:
                            if (restoDict.TryGetValue(nop, out var resto))
                            {
                                namaOp = resto.NamaOp;
                                alamatOp = resto.AlamatOp;
                                var key = (resto.AlamatOpKdCamat ?? "") + (resto.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case (decimal)EnumFactory.EPajak.TenagaListrik:
                            if (listrikDict.TryGetValue(nop, out var l))
                            {
                                namaOp = l.NamaOp;
                                alamatOp = l.AlamatOp;
                                var key = (l.AlamatOpKdCamat ?? "") + (l.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case (decimal)EnumFactory.EPajak.JasaPerhotelan:
                            if (hotelDict.TryGetValue(nop, out var h))
                            {
                                namaOp = h.NamaOp;
                                alamatOp = h.AlamatOp;
                                var key = (h.AlamatOpKdCamat ?? "") + (h.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case (decimal)EnumFactory.EPajak.JasaParkir:
                            if (parkirDict.TryGetValue(nop, out var p))
                            {
                                namaOp = p.NamaOp;
                                alamatOp = p.AlamatOp;
                                var key = (p.AlamatOpKdCamat ?? "") + (p.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case (decimal)EnumFactory.EPajak.JasaKesenianHiburan:
                            if (hiburanDict.TryGetValue(nop, out var hi))
                            {
                                namaOp = hi.NamaOp;
                                alamatOp = hi.AlamatOp;
                                var key = (hi.AlamatOpKdCamat ?? "") + (hi.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                        case (decimal)EnumFactory.EPajak.AirTanah:
                            if (abtDict.TryGetValue(nop, out var a))
                            {
                                namaOp = a.NamaOp;
                                alamatOp = a.AlamatOp;
                                var key = (a.AlamatOpKdCamat ?? "") + (a.AlamatOpKdLurah ?? "");
                                if (wilayahDict.TryGetValue(key, out var w))
                                {
                                    kelurahan = w.NmKelurahan;
                                    kecamatan = w.NmKecamatan;
                                }
                            }
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(namaOp) || string.IsNullOrWhiteSpace(alamatOp))
                        continue;

                    await Task.Delay(1000); // throttle biar gak diblokir OSM

                    try
                    {
                        var (lat, lon) = await GetKoordinatDariAlamatAsync(client, namaOp, alamatOp, kelurahan, kecamatan);

                        if (!string.IsNullOrWhiteSpace(lat) && !string.IsNullOrWhiteSpace(lon))
                        {
                            item.Latitude = lat;
                            item.Longitude = lon;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ❌ Gagal geocoding {namaOp}: {ex.Message}");
                    }

                    count++;

                    // 🔹 Progress reporting setiap 100 data
                    if (count % 100 == 0 || count == totalData)
                    {
                        double percent = (double)count / totalData * 100;
                        TimeSpan elapsed = DateTime.Now - startTime;
                        double avgPerItem = elapsed.TotalSeconds / count;
                        double remainingSeconds = (totalData - count) * avgPerItem;
                        var eta = DateTime.Now.AddSeconds(remainingSeconds);

                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Progress: {count:N0}/{totalData:N0} ({percent:F2}%)  ⏱️ ETA: {eta:HH:mm:ss}");

                        await context.SaveChangesAsync();
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Geocoding selesai. Total waktu: {(DateTime.Now - startTime):hh\\:mm\\:ss}");
            }

            private static async Task<(string lat, string lon)> GetKoordinatDariAlamatAsync(
                HttpClient client,
                string namaOp,
                string alamatOp,
                string? kelurahan,
                string? kecamatan)
            {
                var queryList = new List<string>
                {
                    $"{namaOp}, {alamatOp}, {kelurahan}, {kecamatan}, Surabaya, Indonesia",
                    $"{namaOp}, {alamatOp}, {kecamatan}, Surabaya, Indonesia",
                    $"{namaOp}, {alamatOp}, Surabaya, Indonesia",
                    $"{alamatOp}, {kelurahan}, {kecamatan}, Surabaya, Indonesia",
                    $"{alamatOp}, {kecamatan}, Surabaya, Indonesia",
                    $"{alamatOp}, Surabaya, Indonesia",
                    $"{namaOp}, Surabaya, Indonesia"
                };

                foreach (var query in queryList)
                {
                    var url = $"https://nominatim.openstreetmap.org/search?format=json&limit=1&q={Uri.EscapeDataString(query)}";

                    try
                    {
                        var response = await client.GetStringAsync(url);
                        var json = JArray.Parse(response);

                        if (json.Count > 0)
                        {
                            var lat = (string)json[0]["lat"];
                            var lon = (string)json[0]["lon"];
                            if (!string.IsNullOrWhiteSpace(lat) && !string.IsNullOrWhiteSpace(lon))
                                return (lat, lon);
                        }
                    }
                    catch
                    {
                        // lanjut ke query berikutnya jika error
                    }
                }

                return ("", "");
            }

            //private static (string lat, string lon) GetKoordinatDariAlamat(string alamat)
            //{
            //    if (string.IsNullOrWhiteSpace(alamat))
            //        throw new Exception("Alamat kosong.");

            //    string apiKey = "AIzaSyAOVYRIgupAurZup5y1PRh8Ismb1A3lLao"; // 🔑 ganti dengan API key milikmu
            //    string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(alamat)}&key={apiKey}";

            //    using (var client = new HttpClient())
            //    {
            //        var response = client.GetStringAsync(url).Result;
            //        var json = JObject.Parse(response);

            //        string status = (string)json["status"] ?? "ERROR";
            //        if (status == "OK")
            //        {
            //            var result = json["results"]?[0]?["geometry"]?["location"];
            //            if (result != null)
            //            {
            //                string lat = result["lat"]?.ToString() ?? "";
            //                string lon = result["lng"]?.ToString() ?? "";
            //                return (lat, lon);
            //            }
            //        }
            //        else if (status == "ZERO_RESULTS")
            //        {
            //            throw new Exception("Alamat tidak ditemukan di Google Maps.");
            //        }
            //        else if (status == "OVER_QUERY_LIMIT")
            //        {
            //            throw new Exception("Kuota API Google Geocoding habis atau dibatasi.");
            //        }
            //        else
            //        {
            //            throw new Exception($"Gagal ambil koordinat (Status: {status})");
            //        }

            //        throw new Exception("Koordinat tidak ditemukan.");
            //    }
            //}
            private static decimal? TryDecimal(string value)
            {
                return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                    ? result : null;
            }
        }

        public class Upload
        {
            public byte[] FileExcel { get; set; } = null!;
        }
    }
}
