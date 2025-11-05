using DevExpress.PivotGrid.OLAP;
using MonPDLib;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PerbandinganPendapatanDaerahVM
    {
        public class Index
        {
            public int SelectedBulan { get; set; } = DateTime.Now.Month;
            public List<SelectListItem> BulanList { get; set; } = new();
            public Index()
            {
                SelectedBulan = DateTime.Now.Month;
                for (int i = 1; i <= 12; i++)
                {
                    var namaBulan = new DateTime(1, i, 1).ToString("MMMM", new CultureInfo("id-ID"));
                    BulanList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = namaBulan
                    });
                }
            }
        }
        public class Show
        {
            public List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> Data { get; set; } = new();
            public Show(int bulan)
            {
                Data = Method.GetSudutPandangRekeningJenisObjekOpdData(bulan);
            }
        }
        public class Akumulasi
        {
            public List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> Data { get; set; } = new();
            public Akumulasi(int bulan)
            {
                Data = Method.GetSudutPandangRekeningJenisObjekOpdDataAkumulasi(bulan);
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
                    public string Bulan { get; set; } = "";
                    public decimal Target { get; set; } = 0;
                    public decimal RealisasiNow { get; set; } = 0;
                    public decimal RealisasiMinSatu { get; set; } = 0;
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
        public class Method
        {
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> GetSudutPandangRekeningJenisObjekOpdData(int month)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>();
                var context = DBClass.GetContext();

                var yearNow = DateTime.Now.Year;
                var yearMines = yearNow - 1;

                // Ambil data dua tahun (tahun ini & tahun sebelumnya)
                var query = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == yearNow || x.TahunBuku == yearMines && x.Bulan == month);
;
                // Group by untuk menghitung Target & Realisasi per tahun
                var rawData = query
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.TahunBuku,
                        x.Bulan
                    })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        g.Key.TahunBuku,
                        g.Key.Bulan,
                        Target = g.Sum(y => y.Target),
                        Realisasi = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // Sekarang gabungkan data tahun ini & tahun sebelumnya per OPD/SubRincian
                var combined = rawData
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
                        Target = g.Where(y => y.TahunBuku == yearNow && y.Bulan == month).Sum(y => y.Target),
                        RealisasiNow = g.Where(y => y.TahunBuku == yearNow && y.Bulan == month).Sum(y => y.Realisasi),
                        RealisasiMinSatu = g.Where(y => y.TahunBuku == yearMines && y.Bulan == month).Sum(y => y.Realisasi)
                    })
                    .ToList();

                // Group lagi berdasarkan OPD untuk membuat struktur ViewModel
                var groupByOpd = combined
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        Target = g.Sum(q => q.Target),
                        RealisasiNow = g.Sum(q => q.RealisasiNow),
                        RealisasiMinSatu = g.Sum(q => q.RealisasiMinSatu),
                        SubRincians = g.OrderBy(x => x.SubRincian).ToList()
                    })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();

                decimal totalTarget = 0;
                decimal totalNow = 0;
                decimal totalPrev = 0;

                foreach (var opd in groupByOpd)
                {
                    var opdVm = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                    opdVm.Col.Kode = opd.KodeOpd;
                    opdVm.Col.Bulan = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM", new CultureInfo("id-ID"));
                    opdVm.Col.Nama = opd.NamaOpd;
                    opdVm.Col.Target = opd.Target;
                    opdVm.Col.RealisasiNow = opd.RealisasiNow;
                    opdVm.Col.RealisasiMinSatu = opd.RealisasiMinSatu;
                    opdVm.Col.Persentase = opd.Target > 0
                        ? Math.Round((opd.RealisasiNow / opd.Target) * 100, 2)
                        : 0;

                    totalTarget += opd.Target;
                    totalNow += opd.RealisasiNow;
                    totalPrev += opd.RealisasiMinSatu;

                    // Tambahkan daftar SubRincian
                    foreach (var sub in opd.SubRincians)
                    {
                        var subVm = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                        subVm.Col.Kode = sub.SubRincian;
                        subVm.Col.Nama = sub.NamaSubRincian;
                        subVm.Col.Target = sub.Target;
                        subVm.Col.RealisasiNow = sub.RealisasiNow;
                        subVm.Col.RealisasiMinSatu = sub.RealisasiMinSatu;
                        subVm.Col.Persentase = sub.Target > 0
                            ? Math.Round((sub.RealisasiNow / sub.Target) * 100, 2)
                            : 0;

                        opdVm.RekSubRincians.Add(subVm);
                    }
                    

                    result.Add(opdVm);
                }

                // Tambahkan baris total di akhir
                var total = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                total.Col.Kode = "";
                total.Col.Nama = "TOTAL SEMUA OPD";
                total.Col.Target = totalTarget;
                total.Col.RealisasiNow = totalNow;
                total.Col.RealisasiMinSatu = totalPrev;
                total.Col.Persentase = totalTarget > 0
                    ? Math.Round((totalNow / totalTarget) * 100, 2)
                    : 0;

                result.Add(total);

                return result;
            }
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> GetSudutPandangRekeningJenisObjekOpdDataAkumulasi(int month)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>();
                var context = DBClass.GetContext();

                var yearNow = DateTime.Now.Year;
                var yearMines = yearNow - 1;

                // 🔹 Ambil data dua tahun (tahun ini & tahun sebelumnya)
                //    dan akumulasi dari bulan 1 sampai bulan yang dipilih
                var query = context.DbPendapatanDaerahs
                    .Where(x =>
                        (x.TahunBuku == yearNow && x.Bulan <= month) ||
                        (x.TahunBuku == yearMines && x.Bulan <= month))
                    .ToList();

                // 🔹 Group by untuk menghitung Target & Realisasi per tahun
                var rawData = query
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.TahunBuku
                    })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        g.Key.TahunBuku,
                        Target = g.Sum(y => y.Target),
                        Realisasi = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // 🔹 Gabungkan data tahun ini dan tahun sebelumnya per OPD/SubRincian
                var combined = rawData
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
                        Target = g.Where(y => y.TahunBuku == yearNow).Sum(y => y.Target),
                        RealisasiNow = g.Where(y => y.TahunBuku == yearNow).Sum(y => y.Realisasi),
                        RealisasiMinSatu = g.Where(y => y.TahunBuku == yearMines).Sum(y => y.Realisasi)
                    })
                    .ToList();

                // 🔹 Group by OPD untuk bentuk ViewModel
                var groupByOpd = combined
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        Target = g.Sum(q => q.Target),
                        RealisasiNow = g.Sum(q => q.RealisasiNow),
                        RealisasiMinSatu = g.Sum(q => q.RealisasiMinSatu),
                        SubRincians = g.OrderBy(x => x.SubRincian).ToList()
                    })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();

                decimal totalTarget = 0;
                decimal totalNow = 0;
                decimal totalPrev = 0;

                foreach (var opd in groupByOpd)
                {
                    var opdVm = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                    opdVm.Col.Kode = opd.KodeOpd;
                    opdVm.Col.Nama = opd.NamaOpd;
                    opdVm.Col.Target = opd.Target;
                    opdVm.Col.RealisasiNow = opd.RealisasiNow;
                    opdVm.Col.RealisasiMinSatu = opd.RealisasiMinSatu;
                    opdVm.Col.Persentase = opd.Target > 0
                        ? Math.Round((opd.RealisasiNow / opd.Target) * 100, 2)
                        : 0;

                    totalTarget += opd.Target;
                    totalNow += opd.RealisasiNow;
                    totalPrev += opd.RealisasiMinSatu;

                    // 🔹 Tambahkan daftar SubRincian
                    foreach (var sub in opd.SubRincians)
                    {
                        var subVm = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                        subVm.Col.Kode = sub.SubRincian;
                        subVm.Col.Nama = sub.NamaSubRincian;
                        subVm.Col.Target = sub.Target;
                        subVm.Col.RealisasiNow = sub.RealisasiNow;
                        subVm.Col.RealisasiMinSatu = sub.RealisasiMinSatu;
                        subVm.Col.Persentase = sub.Target > 0
                            ? Math.Round((sub.RealisasiNow / sub.Target) * 100, 2)
                            : 0;

                        opdVm.RekSubRincians.Add(subVm);
                    }

                    result.Add(opdVm);
                }

                // 🔹 Tambahkan total akhir
                var total = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                total.Col.Kode = "";
                total.Col.Nama = $"TOTAL SEMUA OPD (JANUARI - {new DateTime(1, month, 1):MMMM})";
                total.Col.Target = totalTarget;
                total.Col.RealisasiNow = totalNow;
                total.Col.RealisasiMinSatu = totalPrev;
                total.Col.Persentase = totalTarget > 0
                    ? Math.Round((totalNow / totalTarget) * 100, 2)
                    : 0;

                result.Add(total);

                return result;
            }

        }
    }
}
