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

                var restoDict = context.DbOpRestos
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .ToDictionary(g => g.Key, g => g.First());

                var listrikDict = context.DbOpListriks
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .ToDictionary(g => g.Key, g => g.First());

                var hotelDict = context.DbOpHotels
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .ToDictionary(g => g.Key, g => g.First());

                var parkirDict = context.DbOpParkirs
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .ToDictionary(g => g.Key, g => g.First());

                var hiburanDict = context.DbOpHiburans
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .ToDictionary(g => g.Key, g => g.First());

                var abtDict = context.DbOpAbts
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .ToDictionary(g => g.Key, g => g.First());

                var wilayahDict = context.MWilayahs
                        .AsEnumerable()
                        .GroupBy(x => x.KdKecamatan + x.KdKelurahan)
                        .ToDictionary(g => g.Key, g => g.First());

                // 🟢 Kumpulkan semua NOP dari Excel
                var listNop = new List<string>();
                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                    listNop.Add(sheet.Cells[row, 1].Text.Replace(".", "").Trim());

                // 🟢 Hapus data lama dalam 1 batch
                var existing = context.DbOpLocations.Where(x => listNop.Contains(x.FkNop)).ToList();
                if (existing.Any())
                {
                    context.DbOpLocations.RemoveRange(existing);
                    context.SaveChanges();
                }

                // 🟢 Siapkan list data baru
                var listDataBaru = new ConcurrentBag<DbOpLocation>();
                int totalProcessed = 0;
                int progressBarWidth = 40;

                // 🧠 Gunakan Parallel.For untuk mempercepat
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

                    string latitude = "";
                    string longitude = "";

                    try
                    {
                        string nama = namaOp?.Trim() ?? "";
                        string alamat = alamatOp?.Trim() ?? "";
                        string kelurahans = kelurahan?.Trim() ?? "";
                        string kecamatans = kecamatan?.Trim() ?? "";

                        // 🔹 Coba ambil dari Google Geocoding API
                        (latitude, longitude) = GetKoordinatDariAlamat(nama,alamat,kelurahans,kecamatans);
                    }
                    catch
                    {
                        // Kalau gagal ambil dari Google, pakai nilai dari Excel
                        latitude = sheet.Cells[row, 2].Text.Trim() ?? "";
                        longitude = sheet.Cells[row, 3].Text.Trim() ?? "";
                    }

                    


                    listDataBaru.Add(new DbOpLocation
                    {
                        FkNop = nop,
                        Nama = namaOp,
                        Alamat = alamatOp,
                        Latitude = latitude,
                        Longitude = longitude,
                        PajakId = (decimal)TryDecimal(sheet.Cells[row, 4].Text)
                    });

                    // 🔵 Update progress bar (thread-safe)
                    int done = Interlocked.Increment(ref totalProcessed);
                    double progress = (double)done / totalRows;
                    int filledBars = (int)(progress * progressBarWidth);
                    string bar = "[" + new string('#', filledBars) + new string('-', progressBarWidth - filledBars) + "]";
                    Console.Write($"\r{bar} {done}/{totalRows} ({progress:P0})");
                });

                // 🟢 Simpan ke database sekaligus
                Console.WriteLine("\n============================================================");
                Console.WriteLine("Menyimpan perubahan ke database...");
                context.DbOpLocations.AddRange(listDataBaru);
                context.SaveChanges();
                Console.WriteLine("Proses upload spasial selesai ✅");
            }
            //Fungsi ambil koordinat dari OpenStreetMap(Nominatim)
            private static (string lat, string lon) GetKoordinatDariAlamat(
                string namaOp,
                string alamatOp,
                string kelurahan = "",
                string kecamatan = "")
            {
                // 🟢 Validasi dasar
                if (string.IsNullOrWhiteSpace(namaOp) && string.IsNullOrWhiteSpace(alamatOp))
                    throw new Exception("Nama dan alamat kosong.");

                // 🔄 Rapikan format alamat
                alamatOp = alamatOp.Replace("Jl.", "Jalan ").Replace("JL.", "Jalan ");
                alamatOp = alamatOp.Replace("PBI", "Pondok Benowo Indah");

                // 🧩 Susun urutan query dari paling lengkap ke paling sederhana
                var queryList = new List<string>
                {
                    $"{namaOp}, {alamatOp}, {kelurahan}, {kecamatan}, Surabaya, Indonesia",
                    $"{namaOp}, {alamatOp}, Surabaya, Indonesia",
                    $"{alamatOp}, Surabaya, Indonesia",
                    $"{namaOp}, Surabaya, Indonesia"
                };

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "MonPD/1.0");

                foreach (var query in queryList)
                {
                    try
                    {
                        string url = $"https://nominatim.openstreetmap.org/search?" +
                                     $"format=json&countrycodes=id&limit=1&q={Uri.EscapeDataString(query)}";

                        var response = client.GetStringAsync(url).Result;
                        var json = JArray.Parse(response);

                        if (json.Count > 0)
                        {
                            string lat = (string)json[0]["lat"];
                            string lon = (string)json[0]["lon"];
                            return (lat, lon);
                        }
                    }
                    catch
                    {
                        // lanjut ke query berikutnya
                    }
                }

                // ❌ Jika semua gagal
                throw new Exception("Koordinat tidak ditemukan dari semua variasi query.");
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
