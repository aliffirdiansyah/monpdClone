using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using static MonPDReborn.Models.StrukPBJT.StrukPBJTVM;

namespace MonPDReborn.Models.ReklamePublic
{
    public class ReklamePublicVM
    {
        public class Index
        {
            public int selectedUpaya { get; set; }
            public Index()
            {
                
            }
        }
        public class Show
        {
            public List<ReklameJalan> Data { get; set; } = new List<ReklameJalan>();
            public Show(string namaJalan)
            {
                Data = Method.GetReklameJalanList(namaJalan);
            }
        }

        public class Method
        {
            /*public static List<ReklameJalan> GetReklameJalanList(string namaJalan)
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
                    .AsEnumerable() 
                    .GroupBy(x => new
                    {
                        JenisReklame = x.FlagPermohonan ?? "-",
                        Alamat = x.Alamatreklame ?? "-",
                        Kategori = x.NmJenis ?? "-"
                    })
                    .Select(g =>
                    {
                        var first = g.FirstOrDefault();
                        return new ReklameJalan
                        {
                            JenisReklame = g.Key.JenisReklame,
                            Kategori = g.Key.Kategori,
                            Jumlah = g.Sum(x => x.Jumlah ?? 0),
                            Jalan = first?.NamaJalan ?? "-",
                            Alamat = g.Key.Alamat,
                            IsiReklame = first?.IsiReklame ?? "-",
                            tglMulai = first?.TglMulaiBerlaku ?? DateTime.MinValue,
                            tglAkhir = first?.TglAkhirBerlaku ?? DateTime.MinValue
                        };
                    })
                    .ToList();

                return result;
            }*/

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
            public int Jumlah { get; set; }
        }
    }
}
