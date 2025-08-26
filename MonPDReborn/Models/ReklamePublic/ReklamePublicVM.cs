using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using System.Globalization;
using static MonPDReborn.Models.StrukPBJT.StrukPBJTVM;

namespace MonPDReborn.Models.ReklamePublic
{
    public class ReklamePublicVM
    {
        public class Index
        {
            public string selectedUpaya { get; set; }
            public string lokasiUpaya { get; set; }
            public int Number1 { get; set; }
            public int Number2 { get; set; }
            public int CaptchaAnswer { get; set; }
            public string? RecaptchaToken { get; set; }

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
            public string? RecaptchaToken { get; set; }
            public string NamaJalan { get; set; }
            public List<ReklameJalan> Data { get; set; } = new List<ReklameJalan>();
            public Show(string namaJalan, string detailLokasi)
            {
                NamaJalan = namaJalan;
                Data = Method.GetReklameJalanList(namaJalan, detailLokasi);
            }
        }

        public class Method
        {
            public static List<ReklameJalan> GetReklameJalanList(string namaJalan, string detailLokasi)
            {
                using var context = DBClass.GetContext();
                var hariIni = DateTime.Today;

                var query = context.MvReklameSummaries
                    .Where(x => x.TglMulaiBerlaku.HasValue &&
                                x.TglAkhirBerlaku.HasValue &&
                                x.TglAkhirBerlaku.Value.Date >= hariIni);

                // Validasi: minimal salah satu harus diisi
                if (string.IsNullOrWhiteSpace(namaJalan) && string.IsNullOrWhiteSpace(detailLokasi))
                {
                    throw new Exception("Nama jalan atau detail lokasi harus diisi!");
                }

                // Filter nama jalan
                if (!string.IsNullOrWhiteSpace(namaJalan))
                {
                    query = query.Where(x =>
                        x.NamaJalan != null &&
                        x.NamaJalan.ToLower().Contains(namaJalan.ToLower())
                    );
                }

                // Filter detail lokasi (pakai nama depan sebelum koma)
                if (!string.IsNullOrWhiteSpace(detailLokasi))
                {
                    query = query.Where(x =>
                        x.DetailLokasi != null &&
                        (
                            (x.DetailLokasi.Contains(",")
                                ? x.DetailLokasi.Substring(0, x.DetailLokasi.IndexOf(","))
                                : x.DetailLokasi
                            ).ToLower().Contains(detailLokasi.ToLower())
                        )
                    );
                }

                var result = query
                    .Select(x => new ReklameJalan
                    {
                        JenisReklame = x.FlagPermohonan ?? "-",
                        Kategori = x.NmJenis ?? "-",
                        Jumlah = x.Jumlah ?? 0,
                        Jalan = x.NamaJalan ?? "-",
                        Alamat = x.Alamatreklame ?? "-",
                        // tampilkan lengkap, bukan nama depan
                        DetailLokasi = x.DetailLokasi ?? "-",
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
            public string DetailLokasi { get; set; }
            public string Kategori { get; set; }
            public string Status { get; set; }
            public DateTime tglMulai { get; set; }
            public DateTime tglAkhir { get; set; }
            public string TanggalTayang => string.Concat(tglMulai.ToString("dd MMM yyyy", new CultureInfo("id-ID")), " - ", tglAkhir.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
            public decimal Jumlah { get; set; }
        }
    }
}
