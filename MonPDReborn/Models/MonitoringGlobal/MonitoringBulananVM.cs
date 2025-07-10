using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.General;

namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringBulananVM
    {
        public class Index
        {
            public int SelectedJenisPajakId { get; set; }  // Untuk binding pilihan user
            public List<dynamic> JenisPajakList { get; set; } = new();
            public Index()
            {
                JenisPajakList = GetJenisPajakList();
            }
            private List<dynamic> GetJenisPajakList()
            {
                return new List<dynamic>
                {
                    new { Value = 0, Text = "Semua Pajak" },
                    new { Value = 1, Text = "PBJT - HOTEL" },
                    new { Value = 2, Text = "PBJT - RESTORAN" },
                    new { Value = 3, Text = "PBJT - HIBURAN" },
                    new { Value = 4, Text = "REKLAME" },
                    new { Value = 5, Text = "PBJT - PPJ" },
                    new { Value = 7, Text = "PBJT - PARKIR" },
                    new { Value = 8, Text = "AIR TANAH" },
                    new { Value = 12, Text = "PBB P2" },
                    new { Value = 13, Text = "BPHTB" },
                    new { Value = 20, Text = "OPSEN PKB" },
                    new { Value = 21, Text = "OPSEN BBNKB" },
                };
            }
        }
        public class Show
        {
            public List<MonitoringBulananViewModels.BulananPajak> BulananPajakList { get; set; } = new();
            public Show(EnumFactory.EPajak jenisPajak, int tahun)
            {
                BulananPajakList = Method.GetBulananPajak(jenisPajak, tahun);
            }
        }
        public class Method
        {
            public static List<MonitoringBulananViewModels.BulananPajak> GetBulananPajak(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var ret = new List<MonitoringBulananViewModels.BulananPajak>();
                var context = DBClass.GetContext();



                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataTargetWilayahResto = context.DbAkunTargetBulanUptbs
                                .Where(x => x.TahunBuku == tahun && x.PajakId == (decimal)jenisPajak)
                                .GroupBy(x => new { x.Bulan })
                                .Select(g => new
                                {
                                    Bulan = g.Key.Bulan,
                                    TotalTarget = g.Sum(x => x.Target)
                                })
                                .ToList();

                        var realisasiRestoPerBulan = context.DbMonRestos
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => new { TglBayarPokok = x.TglBayarPokok.Value.Month })
                            .Select(g => new {
                                Bulan = g.Key.TglBayarPokok, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var realisasiListrikPerBulan = context.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var realisasiHotelPerBulan = context.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var realisasiParkirPerBulan = context.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var realisasiHiburanPerBulan = context.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var realisasiAbtPerBulan = context.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var realisasiReklamePerBulan = context.DbMonReklames
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.PBB:
                        var realisasiPbbPerBulan = context.DbMonPbbs
                            .Where(x => x.TglBayarPokok.HasValue
                                        && x.TglBayarPokok.Value.Year == tahun)
                            .GroupBy(x => x.TglBayarPokok.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.NominalPokokBayar) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var realisasiBphtbPerBulan = context.DbMonBphtbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == tahun)
                            .GroupBy(x => x.TglBayar.Value.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.Pokok) ?? 0
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        var realisasiOpsenPkbPerBulan = context.DbMonOpsenPkbs
                            .Where(x => x.TglSspd.Year == tahun)
                            .GroupBy(x => x.TglSspd.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.JmlPokok)
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        var realisasiOpsenBbnkbPerBulan = context.DbMonOpsenBbnkbs
                            .Where(x => x.TglSspd.Year == tahun)
                            .GroupBy(x => x.TglSspd.Month)
                            .Select(g => new {
                                Bulan = g.Key, // 1 - 12
                                Realisasi = g.Sum(x => x.JmlPokok)
                            })
                            .OrderBy(x => x.Bulan)
                            .ToList();
                        break;
                    default:
                        break;
                }
                

                return ret;
            }
        }
        public class MonitoringBulananViewModels
        {
            public class BulananPajak
            {
                public string JenisPajak { get; set; } = null!;
                public int Tahun { get; set; } 
                public int Bulan { get; set; } 
                public string BulanNama { get; set; } = null!;
                public long AkpTarget { get; set; }
                public long Realisasi { get; set; }
                public double Pencapaian { get; set; }
                public double Selisih => Realisasi - AkpTarget;
            }
        }
    }
}
