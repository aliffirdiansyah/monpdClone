using MonPDLib;
using MonPDLib.EFPlanning;

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
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>GetSudutPandangRekeningJenisObjekOpdData(DateTime TglCutOff)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>();
                var context = DBClass.GetContext();
                var planningContext = DBClass.GetEPlanningContext();


                // =======================
                // 1. Target Tahunan
                // =======================
                var akpTahunQuery = context.DbPendapatanDaerahHarians
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.Kelompok,
                        x.NamaKelompok,
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        x.Key.Kelompok,
                        x.Key.NamaKelompok,
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        x.Key.Objek,
                        x.Key.NamaObjek,
                        AkpTahun = x.Sum(y => y.Target)
                    })
                    .ToList();


                // =======================
                // 2. Realisasi Hari Ini (accrual harian)
                // =======================
                var realisasiHariQuery = context.DbPendapatanDaerahHarians
                    .Where(x =>
                        x.Tanggal.Date == TglCutOff.Date &&
                        !(x.Kelompok == "4.2" && x.TahunBuku == TglCutOff.Year) &&
                        !(x.Jenis == "4.1.03" && x.TahunBuku == TglCutOff.Year)
                    )
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.Kelompok,
                        x.Jenis,
                        x.Objek
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        x.Key.Kelompok,
                        x.Key.Jenis,
                        x.Key.Objek,
                        RealisasiHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();


                // =======================
                // 3. Realisasi s/d Hari Ini (accrual harian)
                // =======================
                var realisasiSdQuery = context.DbPendapatanDaerahHarians
                    .Where(x =>
                        x.Tanggal.Date <= TglCutOff.Date &&
                        !(x.Kelompok == "4.2" && x.TahunBuku == TglCutOff.Year) &&
                        !(x.Jenis == "4.1.03" && x.TahunBuku == TglCutOff.Year)
                    )
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.Kelompok,
                        x.Jenis,
                        x.Objek
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        x.Key.Kelompok,
                        x.Key.Jenis,
                        x.Key.Objek,
                        RealisasiSDHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();


                // =======================
                // 4. Manual Input (Hari Ini)
                // =======================
                var realisasiInputHari = planningContext.TInputManuals
                    .Where(x => x.Tanggal.Date == TglCutOff.Date)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.Kelompok,
                        x.Jenis,
                        x.Objek
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        x.Key.Kelompok,
                        x.Key.Jenis,
                        x.Key.Objek,
                        RealisasiHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();



                // =======================
                // 5. Manual Input (s/d Hari Ini)
                // =======================
                var realisasiInput = planningContext.TInputManuals
                    .Where(x => x.Tanggal.Date <= TglCutOff.Date)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.Kelompok,
                        x.Jenis,
                        x.Objek
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        x.Key.Kelompok,
                        x.Key.Jenis,
                        x.Key.Objek,
                        RealisasiSDHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();



                // =======================
                // 6. MERGE DATA (sama dengan dashboard)
                // =======================
                var merged =
                    (from a in akpTahunQuery

                     join b in realisasiHariQuery
                         on new { a.KodeOpd, a.SubRincian }
                         equals new { b.KodeOpd, b.SubRincian }
                         into gj1
                     from b in gj1.DefaultIfEmpty()

                     join c in realisasiSdQuery
                         on new { a.KodeOpd, a.SubRincian }
                         equals new { c.KodeOpd, c.SubRincian }
                         into gj2
                     from c in gj2.DefaultIfEmpty()

                     join d in realisasiInputHari
                         on new { a.KodeOpd, a.SubRincian }
                         equals new { d.KodeOpd, d.SubRincian }
                         into gj3
                     from d in gj3.DefaultIfEmpty()

                     join e in realisasiInput
                         on new { a.KodeOpd, a.SubRincian }
                         equals new { e.KodeOpd, e.SubRincian }
                         into gj4
                     from e in gj4.DefaultIfEmpty()

                     select new
                     {
                         a.KodeOpd,
                         a.NamaOpd,
                         a.SubRincian,
                         a.NamaSubRincian,

                         AkpTahun = a.AkpTahun,

                         RealisasiHariAccrual =
                             (b?.RealisasiHariAccrual ?? 0) +
                             (d?.RealisasiHariAccrual ?? 0),

                         RealisasiSDHariAccrual =
                             (c?.RealisasiSDHariAccrual ?? 0) +
                             (e?.RealisasiSDHariAccrual ?? 0)
                     })
                     .ToList();


                // =======================
                // 7. GROUPING OPD
                // =======================
                var groupByOpd = merged
                    .GroupBy(k => new { k.KodeOpd, k.NamaOpd })
                    .Select(opd => new
                    {
                        opd.Key.KodeOpd,
                        opd.Key.NamaOpd,
                        AkpTahun = opd.Sum(z => z.AkpTahun),
                        RealisasiHariAccrual = opd.Sum(z => z.RealisasiHariAccrual),
                        RealisasiSDHariAccrual = opd.Sum(z => z.RealisasiSDHariAccrual),

                        SubList = opd.Select(x => new
                        {
                            x.SubRincian,
                            x.NamaSubRincian,
                            x.AkpTahun,
                            x.RealisasiHariAccrual,
                            x.RealisasiSDHariAccrual
                        }).ToList()
                    })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();


                // =======================
                // 8. MAPPING VIEWMODEL
                // =======================
                foreach (var opd in groupByOpd)
                {
                    var o = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                    o.Col.Kode = opd.KodeOpd;
                    o.Col.Nama = opd.NamaOpd;
                    o.Col.AkpTahun = opd.AkpTahun;
                    o.Col.RealisasiHariAccrual = opd.RealisasiHariAccrual;
                    o.Col.RealisasiSDHariAccrual = opd.RealisasiSDHariAccrual;
                    o.Col.PersenAccrual = opd.AkpTahun > 0
                        ? Math.Round((opd.RealisasiSDHariAccrual / opd.AkpTahun) * 100, 2)
                        : 0;

                    foreach (var s in opd.SubList)
                    {
                        var sub = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                        sub.Col.Kode = s.SubRincian;
                        sub.Col.Nama = s.NamaSubRincian;
                        sub.Col.AkpTahun = s.AkpTahun;
                        sub.Col.RealisasiHariAccrual = s.RealisasiHariAccrual;
                        sub.Col.RealisasiSDHariAccrual = s.RealisasiSDHariAccrual;
                        sub.Col.PersenAccrual = s.AkpTahun > 0
                            ? Math.Round((s.RealisasiSDHariAccrual / s.AkpTahun) * 100, 2)
                            : 0;

                        o.RekSubRincians.Add(sub);
                    }

                    result.Add(o);
                }


                // =======================
                // 9. TOTAL SEMUA OPD
                // =======================
                var total = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                total.Col.Kode = "";
                total.Col.Nama = "TOTAL SEMUA OPD";
                total.Col.AkpTahun = result.Sum(x => x.Col.AkpTahun);
                total.Col.RealisasiHariAccrual = result.Sum(x => x.Col.RealisasiHariAccrual);
                total.Col.RealisasiSDHariAccrual = result.Sum(x => x.Col.RealisasiSDHariAccrual);
                total.Col.PersenAccrual = total.Col.AkpTahun > 0
                    ? Math.Round((total.Col.RealisasiSDHariAccrual / total.Col.AkpTahun) * 100, 2)
                    : 0;

                result.Insert(0, total);

                return result;
            }



        }
    }
}
