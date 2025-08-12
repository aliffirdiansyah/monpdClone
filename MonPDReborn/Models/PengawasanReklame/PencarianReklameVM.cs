using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using System.Globalization;

namespace MonPDReborn.Models.PengawasanReklame
{
    public class PencarianReklameVM
    {
        public class Index
        {
            public string selectedUpaya { get; set; }
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
            public Show(string namaJalan)
            {;
                Data = Method.GetReklameJalanList(namaJalan);
            }
        }
        public class Method
        {
            public static List<ReklameJalan> GetReklameJalanList(string namaJalan)
            {
                var context = DBClass.GetContext();
                var hariIni = DateTime.Today;

                var query = context.MvReklameSummaries
                    .Where(x => x.TglMulaiBerlaku.HasValue && x.TglAkhirBerlaku.Value.Date >= hariIni);

                if (!string.IsNullOrWhiteSpace(namaJalan))
                {
                    query = query.Where(x => x.NamaJalan != null &&
                                             x.NamaJalan.ToLower().Contains(namaJalan.ToLower()));
                }

                var result = query
                    .Select(x => new ReklameJalan
                    {
                        JenisReklame = x.FlagPermohonan ?? "-",
                        Kategori = x.NmJenis ?? "-",
                        Jumlah = x.Jumlah ?? 0,
                        Jalan = x.NamaJalan ?? "-",
                        Alamat = x.Alamatreklame ?? "-",
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

        public class ReklameJalan
        {

            public string Jalan { get; set; }
            public string Alamat { get; set; }
            public string JenisReklame { get; set; }
            public string IsiReklame { get; set; }
            public string Kategori { get; set; }
            public string Status { get; set; }
            public DateTime tglMulai { get; set; }
            public DateTime tglAkhir { get; set; }
            public string TanggalTayang => string.Concat(tglMulai.ToString("dd MMM yyyy", new CultureInfo("id-ID")), " - ", tglAkhir.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
            public decimal Jumlah { get; set; }
        }
    }
}
