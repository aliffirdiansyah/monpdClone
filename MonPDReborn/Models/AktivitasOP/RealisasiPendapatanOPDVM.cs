using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class RealisasiPendapatanOPDVM
    {
        public class Index
        {
            public Index()
            {

            }
        }
        public class Show
        {
            public List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> Data { get; set; } = new();
            public DateTime Tgl { get; set; }
            public Show(DateTime TglCutOff)
            {
                Data = Methods.GetSudutPandangRekeningJenisObjekOpdData(TglCutOff);
                Tgl = TglCutOff;
            }
        }
        public class ViewModels
        {
            public class ShowSeriesSudutPandangRekeningJenisObjekOpd
            {
                public class Jenis
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Obyek> RekObyeks { get; set; } = new List<Obyek>();
                }
                public class Kelompok
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Jenis> RekJenis { get; set; } = new();
                }
                public class Obyek
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Opd> RekOpds { get; set; } = new List<Opd>();
                }
                public class Opd
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<SubRincian> RekSubRincians { get; set; } = new List<SubRincian>();
                }
                public class SubRincian
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                }
            }
            public class FormatColumn
            {
                public class ColumnA
                {
                    public string Kode { get; set; } = "";
                    public string Nama { get; set; } = "";
                    public decimal Target { get; set; } = 0;
                    public decimal Realisasi { get; set; } = 0;
                    public decimal Persentase { get; set; } = 0;
                    public decimal AkpTahun { get; set; } = 0;
                    public decimal RealisasiHariAccrual { get; set; } = 0;
                    public decimal RealisasiSDHariAccrual { get; set; } = 0;
                    public decimal PersenAccrual { get; set; } = 0;
                }

                public class ReportColumnA
                {
                    public string Nama { get; set; } = "";
                    public decimal TargetTotal { get; set; } = 0;
                    public decimal TargetSampaiDengan { get; set; } = 0;
                    public decimal RealisasiSampaiDengan { get; set; } = 0;
                    public decimal SelisihSampaiDengan => RealisasiSampaiDengan - TargetSampaiDengan;
                    public decimal PersentaseSampaiDengan => TargetSampaiDengan > 0
                        ? Math.Round((RealisasiSampaiDengan / TargetSampaiDengan) * 100, 2)
                        : 0;
                    public decimal TargetBulanIni { get; set; } = 0;
                    public decimal RealisasiBulanIni { get; set; } = 0;
                    public decimal SelisihBulanIni => RealisasiBulanIni - TargetBulanIni;
                    public decimal PersentaseBulanIni => TargetBulanIni > 0
                       ? Math.Round((RealisasiBulanIni / TargetBulanIni) * 100, 2)
                       : 0;
                    public decimal PersentaseTotal => TargetTotal > 0
                       ? Math.Round((RealisasiSampaiDengan / TargetTotal) * 100, 2)
                       : 0;
                }

                public class ReportColumnB
                {
                    public string Nama { get; set; } = "";
                    public decimal TargetTotal { get; set; } = 0;
                    public decimal TargetSampaiDengan { get; set; } = 0;
                    public decimal RealisasiSampaiDengan { get; set; } = 0;
                    public decimal SelisihSampaiDengan => RealisasiSampaiDengan - TargetSampaiDengan;
                    public decimal PersentaseSampaiDengan => TargetSampaiDengan > 0
                        ? Math.Round((RealisasiSampaiDengan / TargetSampaiDengan) * 100, 2)
                        : 0;
                    public decimal TargetBulanIni { get; set; } = 0;
                    public decimal RealisasiBulanIni { get; set; } = 0;
                    public decimal SelisihBulanIni => RealisasiBulanIni - TargetBulanIni;
                    public decimal PersentaseBulanIni => TargetBulanIni > 0
                       ? Math.Round((RealisasiBulanIni / TargetBulanIni) * 100, 2)
                       : 0;
                    public decimal PersentaseTotal => TargetTotal > 0
                       ? Math.Round((RealisasiSampaiDengan / TargetTotal) * 100, 2)
                       : 0;
                    public int Status { get; set; } = 0;
                }
            }
        }
        public class Methods
        {
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> GetSudutPandangRekeningJenisObjekOpdData(DateTime TglCutOff)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>();
                var context = DBClass.GetContext();

                // === 1️⃣ Target Tahunan ===
                var akpTahunQuery = context.DbPendapatanDaerahHarians
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        AkpTahun = x.Sum(y => y.Target)
                    })
                    .ToList();

                // === 2️⃣ Realisasi Hari Ini ===
                var realisasiHariQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.Tanggal.Date == TglCutOff.Date && !(x.Kelompok == "4.2" && x.TahunBuku == TglCutOff.Year) && !(x.Jenis == "4.1.03" && x.TahunBuku == TglCutOff.Year))
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        RealisasiHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // === 3️⃣ Realisasi s/d Hari Ini ===
                var realisasiSdQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.Tanggal.Date <= TglCutOff.Date && !(x.Kelompok == "4.2" && x.TahunBuku == TglCutOff.Year) && !(x.Jenis == "4.1.03" && x.TahunBuku == TglCutOff.Year))
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        RealisasiSDHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // === 4️⃣ Gabungkan Semua Data ===
                var merged = (from a in akpTahunQuery
                              join b in realisasiHariQuery
                                  on new { a.KodeOpd, a.SubRincian } equals new { b.KodeOpd, b.SubRincian } into gj1
                              from b in gj1.DefaultIfEmpty()
                              join c in realisasiSdQuery
                                  on new { a.KodeOpd, a.SubRincian } equals new { c.KodeOpd, c.SubRincian } into gj2
                              from c in gj2.DefaultIfEmpty()
                              select new
                              {
                                  a.KodeOpd,
                                  a.NamaOpd,
                                  a.SubRincian,
                                  a.NamaSubRincian,
                                  AkpTahun = a.AkpTahun,
                                  RealisasiHariAccrual = b?.RealisasiHariAccrual ?? 0,
                                  RealisasiSDHariAccrual = c?.RealisasiSDHariAccrual ?? 0
                              })
                              .ToList();

                // === 5️⃣ Group per OPD ===
                var groupByOpd = merged
                    .GroupBy(opd => new { opd.KodeOpd, opd.NamaOpd })
                    .Select(opd => new
                    {
                        opd.Key.KodeOpd,
                        opd.Key.NamaOpd,
                        SubRincianList = opd
                            .Select(sub => new
                            {
                                sub.SubRincian,
                                sub.NamaSubRincian,
                                sub.AkpTahun,
                                sub.RealisasiHariAccrual,
                                sub.RealisasiSDHariAccrual
                            })
                            .OrderBy(x => x.SubRincian)
                            .ToList(),
                        AkpTahun = opd.Sum(z => z.AkpTahun),
                        RealisasiHariAccrual = opd.Sum(z => z.RealisasiHariAccrual),
                        RealisasiSDHariAccrual = opd.Sum(z => z.RealisasiSDHariAccrual)
                    })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();

                // === 6️⃣ Mapping ke ViewModel ===
                foreach (var opd in groupByOpd)
                {
                    var resOpd = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                    resOpd.Col.Kode = opd.KodeOpd;
                    resOpd.Col.Nama = opd.NamaOpd;
                    resOpd.Col.AkpTahun = opd.AkpTahun;
                    resOpd.Col.RealisasiHariAccrual = opd.RealisasiHariAccrual;
                    resOpd.Col.RealisasiSDHariAccrual = opd.RealisasiSDHariAccrual;
                    resOpd.Col.PersenAccrual = opd.AkpTahun > 0 ? Math.Round((opd.RealisasiSDHariAccrual / opd.AkpTahun) * 100, 2) : 0;

                    foreach (var sub in opd.SubRincianList)
                    {
                        var resSub = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                        resSub.Col.Kode = sub.SubRincian;
                        resSub.Col.Nama = sub.NamaSubRincian;
                        resSub.Col.AkpTahun = sub.AkpTahun;
                        resSub.Col.RealisasiHariAccrual = sub.RealisasiHariAccrual;
                        resSub.Col.RealisasiSDHariAccrual = sub.RealisasiSDHariAccrual;
                        resSub.Col.PersenAccrual = sub.AkpTahun > 0 ? Math.Round((sub.RealisasiSDHariAccrual / sub.AkpTahun) * 100, 2) : 0;

                        resOpd.RekSubRincians.Add(resSub);
                    }

                    result.Add(resOpd);
                }

                // === 7️⃣ Tambahkan TOTAL SEMUA OPD ===
                var totalTargetSemuaOpd = result.Sum(x => x.Col.AkpTahun);
                var totalRealisasiHariSemuaOpd = result.Sum(x => x.Col.RealisasiHariAccrual);
                var totalRealisasiSdSemuaOpd = result.Sum(x => x.Col.RealisasiSDHariAccrual);

                var totalOpd = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                totalOpd.Col.Kode = "";
                totalOpd.Col.Nama = "TOTAL SEMUA OPD";
                totalOpd.Col.AkpTahun = totalTargetSemuaOpd;
                totalOpd.Col.RealisasiHariAccrual = totalRealisasiHariSemuaOpd;
                totalOpd.Col.RealisasiSDHariAccrual = totalRealisasiSdSemuaOpd;
                totalOpd.Col.PersenAccrual = totalTargetSemuaOpd > 0
                    ? Math.Round((totalRealisasiSdSemuaOpd / totalTargetSemuaOpd) * 100, 2)
                    : 0;

                result.Insert(0, totalOpd);

                return result;
            }


        }
    }
}
