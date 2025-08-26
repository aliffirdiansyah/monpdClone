using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using System.Globalization;

namespace MonPDReborn.Models.PengawasanReklame
{
    public class PencarianReklameVM
    {
        public class Index
        {
            public string selectedUpaya { get; set; }
            public string lokasiUpaya { get; set; }
            public int Number1 { get; set; }
            public int Number2 { get; set; }
            public int CaptchaAnswer { get; set; }

            public Index()
            {
                var random = new Random();
                int number1 = random.Next(1, 10);
                int number2 = random.Next(1, 10);
                int captchaAnswer = number1 + number2;

                Number1 = number1;
                Number2 = number2;
                CaptchaAnswer = captchaAnswer;
            }
        }
        public class Show
        {
            public List<ReklameJalan> Data { get; set; } = new List<ReklameJalan>();
            public Show(string namaJalan, string detailLokasi, string status, int page = 1, int pageSize = 200)
            {;
                Data = Method.GetReklameJalanList(namaJalan, detailLokasi, status, page, pageSize);
            }
        }
        public class Method
        {
            public static List<ReklameJalan> GetReklameJalanList(string namaJalan, string detailLokasi, string status, int page = 1, int pageSize = 200)
            {
                using var context = DBClass.GetContext();
                var hariIni = DateTime.Today;

                // Query dasar dengan AsNoTracking (lebih cepat untuk read-only)
                var query = context.MvReklameSummaries.AsNoTracking().AsQueryable();

                // Filter berdasarkan status
                if (!string.IsNullOrEmpty(status) && status.ToLower() != "semua")
                {
                    if (status.ToLower() == "aktif")
                    {
                        query = query.Where(x =>
                            x.TglMulaiBerlaku.HasValue &&
                            x.TglAkhirBerlaku.HasValue &&
                            x.TglMulaiBerlaku <= hariIni &&
                            x.TglAkhirBerlaku >= hariIni
                        );
                    }
                    else if (status.ToLower() == "expired")
                    {
                        query = query.Where(x =>
                            x.TglAkhirBerlaku.HasValue &&
                            x.TglAkhirBerlaku < hariIni   // tanpa .Date → index kepakai
                        );
                    }
                }

                // Filter nama jalan / detail lokasi
                if (string.IsNullOrWhiteSpace(namaJalan) && string.IsNullOrWhiteSpace(detailLokasi))
                    throw new Exception("Nama jalan atau detail lokasi harus diisi!");

                if (!string.IsNullOrWhiteSpace(namaJalan))
                {
                    query = query.Where(x =>
                        x.NamaJalan != null &&
                        x.NamaJalan.ToLower().Contains(namaJalan.ToLower())
                    );
                }

                if (!string.IsNullOrWhiteSpace(detailLokasi))
                {
                    query = query.Where(x =>
                        x.DetailLokasi != null &&
                        (
                            // ambil nama depan sebelum koma, kalau tidak ada koma ambil semua
                            (x.DetailLokasi.Contains(",")
                                ? x.DetailLokasi.Substring(0, x.DetailLokasi.IndexOf(","))
                                : x.DetailLokasi
                            ).ToLower().Contains(detailLokasi.ToLower())
                        )
                    );
                }

                // Paging (supaya tidak load ribuan data sekaligus)
                query = query
                    .OrderBy(x => x.TglAkhirBerlaku) // urutkan supaya paging konsisten
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                // Proyeksi hasil
                var result = query
                    .Select(x => new ReklameJalan
                    {
                        JenisReklame = x.FlagPermohonan ?? "-",
                        Kategori = x.NmJenis ?? "-",
                        Jumlah = x.Jumlah ?? 0,
                        Jalan = x.NamaJalan ?? "-",
                        Alamat = x.Alamatreklame ?? "-",
                        DetailLokasi = x.DetailLokasi ?? "-",
                        Letak = x.LetakReklame ?? "-",
                        IsiReklame = x.IsiReklame ?? "-",
                        tglMulai = x.TglMulaiBerlaku ?? DateTime.MinValue,
                        tglAkhir = x.TglAkhirBerlaku ?? DateTime.MinValue
                    })
                    .ToList();

                return result;
            }






        }

        public class namaJalanView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }

        public class lokasiReklameView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }

        public class ReklameJalan
        {

            public string Jalan { get; set; }
            public string Alamat { get; set; }
            public string JenisReklame { get; set; }
            public string IsiReklame { get; set; }
            public string Kategori { get; set; }
            public string Status { get; set; }
            public string DetailLokasi { get; set; }
            public string Letak { get; set; }
            public DateTime tglMulai { get; set; }
            public DateTime tglAkhir { get; set; }
            public string TanggalTayang => string.Concat(tglMulai.ToString("dd MMM yyyy", new CultureInfo("id-ID")), " - ", tglAkhir.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
            public decimal Jumlah { get; set; }
        }
    }
}
