using MonPDLib;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace MonPDReborn.Models.KontrolPBB
{
    public class AnalitikViewVM
    {
        public class Index
        {
            public Index()
            {
            }
        }
        public class ShowWilayah
        {
            public List<ViewModels.DetailWilayah> Data { get; set; } = new();
            public ShowWilayah(string wilayah, string kec) 
            { 
                Data = Methods.GetDetailWilayah(wilayah, kec);
            }
        }
        public class ShowSegmentasi
        {
            public List<ViewModels.Segmentasi> Data { get; set; } = new();
            public ShowSegmentasi(string wilayah, string kec)
            {
                Data = Methods.GetDataSegmentasi(wilayah, kec);
            }
        }
        public class ShowKontribusi
        {
            public List<ViewModels.KontribusiSegmen> Data { get; set; } = new();
            public ShowKontribusi(string wilayah, string kec)
            {
                Data = Methods.GetKontribusiSegmen(wilayah, kec);
            }
        }
        public class ShowTunggakan
        {
            public List<ViewModels.Tunggakan> Data { get; set; } = new();
            public ShowTunggakan(string wilayah, string kec)
            {
                Data = Methods.GetDataTunggakan(wilayah, kec);
            }
        }
        public class ViewModels
        {
            public class DetailWilayah
            {
                public string Wilayah { get; set; }
                public string NOP { get; set; } = null!;
                public string NamaWP { get; set; } = null!;
                public decimal Penetapan { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Pencapaian => Penetapan == 0 ? 0 : (Realisasi / Penetapan) * 100;
                public string Status { get; set; } = null!;
            }
            public class Segmentasi
            {
                public string KategoriWP { get; set; } = null!;
                public decimal JmlObjek { get; set; }
                public decimal Target { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Pencapaian => Target == 0 ? 0 : (Realisasi / Target) * 100;
                public decimal Kontribusi => Realisasi == 0 ? 0 : (Realisasi / Realisasi) * 100;
            }
            public class KontribusiSegmen
            {
                public string Segmen { get; set; } = null!;
                public decimal Ketetapan { get; set; }
                public decimal Persentase { get; set; }
            }
            public class Tunggakan
            {
                public string NamaWP { get; set; } = null!;
                public decimal Ketetapan { get; set; }
                public decimal Realisasi { get; set; }
                public decimal TunggakanWP => Ketetapan - Realisasi;
            }

            public class DashoboardKinerjaPetugas
            {
                public string Petugas { get; set; } = null!;
                public decimal Capaian { get; set; }
            }
        }
        public class uptbView
        {
            public string Value { get; set; } = null!;
            public string Text { get; set; } = null!;
        }
        public class kecamatanView
        {
            public string Value { get; set; } = null!;
            public string Text { get; set; } = null!;
        }
        public class Methods
        {
            public static List<ViewModels.DetailWilayah> GetDetailWilayah(string wilayah, string kec)
            {
                var context = DBClass.GetContext();
                var ret = new List<ViewModels.DetailWilayah>();
                var tahun = DateTime.Now.Year;

                ret = context.DbMonPbbSummaries
                    .Where(x => x.Wilayah == wilayah && x.Kecamatan == kec && x.TahunPajak == tahun)
                    .Select(x => new ViewModels.DetailWilayah()
                    {
                        NOP = x.Nop ?? "-",
                        NamaWP = x.NamaWp ?? "-",
                        Penetapan = x.Ketetapan ?? 0,
                        Realisasi = x.Realisasi ?? 0,
                        Status = x.StatusLunas ?? "-",
                        Wilayah = x.Wilayah ?? "-",
                    })
                    .ToList();

                return ret;
            }
            public static List<ViewModels.Segmentasi> GetDataSegmentasi(string wilayah, string kec)
            {
                var context = DBClass.GetContext();
                var tahun = DateTime.Now.Year;

                var data = context.DbMonPbbSummaries
                    .Where(x => x.Wilayah == wilayah && x.Kecamatan == kec && x.TahunPajak == tahun)
                    .AsQueryable();

                var segmentasi = new List<ViewModels.Segmentasi>();

                // WP Besar (> 50 juta)
                var besar = data.Where(x => x.Ketetapan > 50_000_000);
                segmentasi.Add(new ViewModels.Segmentasi
                {
                    KategoriWP = "WP Besar (>50 juta)",
                    JmlObjek = besar.Count(),
                    Target = besar.Sum(x => x.Ketetapan ?? 0),
                    Realisasi = besar.Sum(x => x.Realisasi ?? 0)
                });

                // WP Menengah (10 – 50 juta)
                var menengah = data.Where(x => x.Ketetapan >= 10_000_000 && x.Ketetapan <= 50_000_000);
                segmentasi.Add(new ViewModels.Segmentasi
                {
                    KategoriWP = "WP Menengah (10-50 juta)",
                    JmlObjek = menengah.Count(),
                    Target = menengah.Sum(x => x.Ketetapan ?? 0),
                    Realisasi = menengah.Sum(x => x.Realisasi ?? 0)
                });

                // WP Kecil (1 – 10 juta)
                var kecil = data.Where(x => x.Ketetapan >= 1_000_000 && x.Ketetapan < 10_000_000);
                segmentasi.Add(new ViewModels.Segmentasi
                {
                    KategoriWP = "WP Kecil (1-10 juta)",
                    JmlObjek = kecil.Count(),
                    Target = kecil.Sum(x => x.Ketetapan ?? 0),
                    Realisasi = kecil.Sum(x => x.Realisasi ?? 0)
                });

                // WP Mikro (< 1 juta)
                var mikro = data.Where(x => x.Ketetapan < 1_000_000);
                segmentasi.Add(new ViewModels.Segmentasi
                {
                    KategoriWP = "WP Mikro (<1 juta)",
                    JmlObjek = mikro.Count(),
                    Target = mikro.Sum(x => x.Ketetapan ?? 0),
                    Realisasi = mikro.Sum(x => x.Realisasi ?? 0)
                });

                return segmentasi;
            }
            public static List<ViewModels.KontribusiSegmen> GetKontribusiSegmen(string wilayah, string kec)
            {
                var context = DBClass.GetContext();
                var tahun = DateTime.Now.Year;

                var data = context.DbMonPbbSummaries
                    .Where(x => x.TahunPajak == tahun && x.Wilayah == wilayah && x.Kecamatan == kec)
                    .ToList();

                if (data.Count == 0)
                    return new List<ViewModels.KontribusiSegmen>();

                var totalSemua = data.Sum(x => x.Ketetapan ?? 0);

                var wpBesar = data.Where(x => (x.Ketetapan ?? 0) > 50_000_000).Sum(x => x.Ketetapan ?? 0);
                var wpMenengah = data.Where(x => (x.Ketetapan ?? 0) >= 10_000_000 && (x.Ketetapan ?? 0) <= 50_000_000).Sum(x => x.Ketetapan ?? 0);
                var wpKecil = data.Where(x => (x.Ketetapan ?? 0) < 10_000_000).Sum(x => x.Ketetapan ?? 0);

                var result = new List<ViewModels.KontribusiSegmen>
                {
                    new ViewModels.KontribusiSegmen
                    {
                        Segmen = "WP Besar (>50 juta)",
                        Ketetapan = wpBesar,
                        Persentase = totalSemua == 0 ? 0 : (wpBesar / totalSemua) * 100
                    },
                    new ViewModels.KontribusiSegmen
                    {
                        Segmen = "WP Menengah (10–50 juta)",
                        Ketetapan = wpMenengah,
                        Persentase = totalSemua == 0 ? 0 : (wpMenengah / totalSemua) * 100
                    },
                    new ViewModels.KontribusiSegmen
                    {
                        Segmen = "WP Kecil (<10 juta)",
                        Ketetapan = wpKecil,
                        Persentase = totalSemua == 0 ? 0 : (wpKecil / totalSemua) * 100
                    }
                };

                return result;
            }
            public static List<ViewModels.Tunggakan> GetDataTunggakan(string wilayah, string kec)
            {
                var context = DBClass.GetContext();
                var tahun = DateTime.Now.Year;

                var data = context.DbMonPbbSummaries
                    .Where(x => x.TahunPajak == tahun && x.Wilayah == wilayah && x.Kecamatan == kec)
                    .GroupBy(x => x.NamaWp)
                    .Select(g => new ViewModels.Tunggakan
                    {
                        NamaWP = g.Key,
                        Ketetapan = g.Sum(x => x.Ketetapan ?? 0),
                        Realisasi = g.Sum(x => x.Realisasi ?? 0)
                    })
                    .ToList();

                var top10 = data
                    .OrderByDescending(x => x.TunggakanWP)
                    .Take(10)
                    .ToList();

                return top10;
            }
        }
    }
}
