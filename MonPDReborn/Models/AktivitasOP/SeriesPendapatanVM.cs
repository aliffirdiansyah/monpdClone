using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanVM
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
            public Show(int year)
            {
                Data = Methods.GetSudutPandangRekeningJenisObjekOpdData(year);
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
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>
    GetSudutPandangRekeningJenisObjekOpdData(int year)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>();

                var context = DBClass.GetContext();
                var planning = DBClass.GetEPlanningContext();

                DateTime today = DateTime.Now.Date;

                // =========================
                // 1) TARGET
                // =========================
                var targetList = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == year)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        Target = g.Sum(y => y.Target)
                    })
                    .ToList();


                // =========================
                // 2) REALISASI OTOMATIS
                // =========================
                var realisasiDb = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == year)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        Realisasi = g.Sum(y => y.Realisasi)
                    })
                    .ToList();


                // =========================
                // 3) REALISASI MANUAL
                // =========================
                var realManual = planning.TInputManuals
                    .Where(x => x.Tanggal <= today)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        RealManual = g.Sum(y => y.Realisasi)
                    })
                    .ToList();


                // =========================
                // 4) MERGE TARGET + REALISASI DB + MANUAL
                // =========================
                var merged = (
                    from t in targetList
                    join r in realisasiDb
                        on new { t.KodeOpd, t.SubRincian }
                        equals new { r.KodeOpd, r.SubRincian }
                        into jr
                    from r in jr.DefaultIfEmpty()

                    join m in realManual
                        on new { t.KodeOpd, t.SubRincian }
                        equals new { m.KodeOpd, m.SubRincian }
                        into jm
                    from m in jm.DefaultIfEmpty()

                    select new
                    {
                        t.KodeOpd,
                        t.NamaOpd,
                        t.SubRincian,
                        t.NamaSubRincian,
                        Target = t.Target,
                        Realisasi = (r?.Realisasi ?? 0) + (m?.RealManual ?? 0)
                    }
                ).ToList();


                // =========================
                // 5) GROUP BERDASARKAN OPD
                // =========================
                var groupOpd = merged
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        Target = g.Sum(y => y.Target),
                        Realisasi = g.Sum(y => y.Realisasi),
                        SubRincians = g.ToList()
                    })
                    .ToList();


                decimal totalTarget = 0;
                decimal totalRealisasi = 0;

                foreach (var opd in groupOpd)
                {
                    var vmOpd = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                    vmOpd.Col.Kode = opd.KodeOpd;
                    vmOpd.Col.Nama = opd.NamaOpd;
                    vmOpd.Col.Target = opd.Target;
                    vmOpd.Col.Realisasi = opd.Realisasi;
                    vmOpd.Col.Persentase = opd.Target > 0 ? Math.Round((opd.Realisasi / opd.Target) * 100, 2) : 0;

                    totalTarget += opd.Target;
                    totalRealisasi += opd.Realisasi;

                    // Sub Rincian
                    foreach (var sub in opd.SubRincians)
                    {
                        var vmSub = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                        vmSub.Col.Kode = sub.SubRincian;
                        vmSub.Col.Nama = sub.NamaSubRincian;
                        vmSub.Col.Target = sub.Target;
                        vmSub.Col.Realisasi = sub.Realisasi;
                        vmSub.Col.Persentase =
                            sub.Target > 0 ? Math.Round((sub.Realisasi / sub.Target) * 100, 2) : 0;

                        vmOpd.RekSubRincians.Add(vmSub);
                    }

                    result.Add(vmOpd);
                }

                // TOTAL BARIS
                var totalRow = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                totalRow.Col.Kode = "";
                totalRow.Col.Nama = "TOTAL SEMUA OPD";
                totalRow.Col.Target = totalTarget;
                totalRow.Col.Realisasi = totalRealisasi;
                totalRow.Col.Persentase =
                    totalTarget > 0 ? Math.Round((totalRealisasi / totalTarget) * 100, 2) : 0;

                result.Insert(0, totalRow);

                return result;
            }

        }
    }
}
