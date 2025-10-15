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
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> GetSudutPandangRekeningJenisObjekOpdData(int year)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>();
                var context = DBClass.GetContext();

                // Ambil data dari DbPendapatanDaerahs
                var query = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == year)
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
                        Target = x.Sum(y => y.Target),
                        Realisasi = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // Group berdasarkan OPD
                var groupByOpd = query
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        Target = x.Sum(q => q.Target),
                        Realisasi = x.Sum(q => q.Realisasi),
                        SubRincians = x.ToList()
                    })
                    .ToList();

                decimal totalTargetSemuaOpd = 0;
                decimal totalRealisasiSemuaOpd = 0;

                foreach (var opd in groupByOpd)
                {
                    var resOpd = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                    resOpd.Col.Kode = opd.KodeOpd;
                    resOpd.Col.Nama = opd.NamaOpd;
                    resOpd.Col.Target = opd.Target;
                    resOpd.Col.Realisasi = opd.Realisasi;
                    resOpd.Col.Persentase = opd.Target > 0 ? Math.Round((opd.Realisasi / opd.Target) * 100, 2) : 0;

                    totalTargetSemuaOpd += opd.Target;
                    totalRealisasiSemuaOpd += opd.Realisasi;

                    // Isi sub rincian di dalam OPD
                    foreach (var sub in opd.SubRincians)
                    {
                        var resSub = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                        resSub.Col.Kode = sub.SubRincian;
                        resSub.Col.Nama = sub.NamaSubRincian;
                        resSub.Col.Target = sub.Target;
                        resSub.Col.Realisasi = sub.Realisasi;
                        resSub.Col.Persentase = sub.Target > 0 ? Math.Round((sub.Realisasi / sub.Target) * 100, 2) : 0;

                        resOpd.RekSubRincians.Add(resSub);
                    }

                    result.Add(resOpd);
                }

                var totalOpd = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                totalOpd.Col.Kode = "";
                totalOpd.Col.Nama = "TOTAL SEMUA OPD";
                totalOpd.Col.Target = totalTargetSemuaOpd;
                totalOpd.Col.Realisasi = totalRealisasiSemuaOpd;
                totalOpd.Col.Persentase = totalTargetSemuaOpd > 0
                    ? Math.Round((totalRealisasiSemuaOpd / totalTargetSemuaOpd) * 100, 2)
                    : 0;

                result.Add(totalOpd);

                return result;
            }
        }
    }
}
