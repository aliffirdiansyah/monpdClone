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
            public string Bulan { get; set; } = null!;
            public Show(int bulan)
            {
                Data = Method.GetSudutPandangRekeningJenisObjekOpdData(bulan);
                Bulan = new DateTime(DateTime.Now.Year, bulan, 1).ToString("MMMM", new CultureInfo("id-ID"));
            }
        }
        public class Akumulasi
        {
            public List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd> Data { get; set; } = new();
            public string Bulan { get;set; } = null!;
            public Akumulasi(int bulan)
            {
                Data = Method.GetSudutPandangRekeningJenisObjekOpdDataAkumulasi(bulan);
                Bulan = new DateTime(DateTime.Now.Year, bulan, 1).ToString("MMMM", new CultureInfo("id-ID"));
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
                    public decimal Selisih { get; set; } = 0;
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
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>
     GetSudutPandangRekeningJenisObjekOpdData(int month)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd>();
                var context = DBClass.GetContext();
                var planningContext = DBClass.GetEPlanningContext();

                var yearNow = DateTime.Now.Year;
                var yearMin1 = yearNow - 1;

                // ============================
                // 1. TARGET & REALISASI AKUNTANSI (DbPendapatanDaerah)
                // ============================
                var akpQuery = context.DbPendapatanDaerahs
                    .Where(x => (x.TahunBuku == yearNow || x.TahunBuku == yearMin1)
                             && x.Bulan == month)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.TahunBuku
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        x.Key.TahunBuku,
                        Target = x.Sum(y => y.Target),
                        Realisasi = x.Sum(y => y.Realisasi)
                    })
                    .ToList();


                // ============================
                // 2. REALISASI MANUAL (TInputManuals)
                // ============================
                var realisasiManualQuery = planningContext.TInputManuals
                    .Where(x => x.Tanggal.Year == yearNow && x.Tanggal.Month == month)
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
                        RealisasiManual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();


                // ============================
                // 3. MERGE (AKUNTANSI + MANUAL)
                // ============================
                var merged =
                    from a in akpQuery
                    join m in realisasiManualQuery
                        on new { a.KodeOpd, a.SubRincian }
                        equals new { m.KodeOpd, m.SubRincian }
                        into gj
                    from m in gj.DefaultIfEmpty()
                    select new
                    {
                        a.KodeOpd,
                        a.NamaOpd,
                        a.SubRincian,
                        a.NamaSubRincian,
                        a.TahunBuku,

                        Target = a.Target,
                        RealisasiAccrual = a.Realisasi + (m?.RealisasiManual ?? 0)
                    };


                // ============================
                // 4. COMBINE: Tahun Ini & Tahun Lalu
                // ============================
                var combined = merged
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
                        RealisasiNow = g.Where(y => y.TahunBuku == yearNow).Sum(y => y.RealisasiAccrual),
                        RealisasiMinSatu = g.Where(y => y.TahunBuku == yearMin1).Sum(y => y.RealisasiAccrual)
                    })
                    .ToList();


                // ============================
                // 5. GROUP OPD
                // ============================
                var groupByOpd = combined
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        Target = g.Sum(y => y.Target),
                        RealisasiNow = g.Sum(y => y.RealisasiNow),
                        RealisasiMinSatu = g.Sum(y => y.RealisasiMinSatu),
                        Subs = g.ToList()
                    })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();


                // ============================
                // 6. MAPPING VIEWMODEL
                // ============================
                decimal totalTarget = 0, totalNow = 0, totalPrev = 0;

                foreach (var opd in groupByOpd)
                {
                    var vm = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                    vm.Col.Kode = opd.KodeOpd;
                    vm.Col.Nama = opd.NamaOpd;
                    vm.Col.Target = opd.Target;
                    vm.Col.RealisasiNow = opd.RealisasiNow;
                    vm.Col.RealisasiMinSatu = opd.RealisasiMinSatu;
                    vm.Col.Selisih = opd.RealisasiNow - opd.RealisasiMinSatu;
                    vm.Col.Persentase = opd.Target > 0
                        ? Math.Round((opd.RealisasiNow / opd.Target) * 100, 2)
                        : 0;

                    totalTarget += opd.Target;
                    totalNow += opd.RealisasiNow;
                    totalPrev += opd.RealisasiMinSatu;

                    foreach (var s in opd.Subs)
                    {
                        var subVm = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                        subVm.Col.Kode = s.SubRincian;
                        subVm.Col.Nama = s.NamaSubRincian;
                        subVm.Col.Target = s.Target;
                        subVm.Col.RealisasiNow = s.RealisasiNow;
                        subVm.Col.RealisasiMinSatu = s.RealisasiMinSatu;
                        subVm.Col.Selisih = s.RealisasiNow - s.RealisasiMinSatu;
                        subVm.Col.Persentase = s.Target > 0
                            ? Math.Round((s.RealisasiNow / s.Target) * 100, 2)
                            : 0;

                        vm.RekSubRincians.Add(subVm);
                    }

                    result.Add(vm);
                }

                // ============================
                // 7. TOTAL
                // ============================
                var totalRow = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                totalRow.Col.Kode = "";
                totalRow.Col.Nama = "TOTAL SEMUA OPD";
                totalRow.Col.Target = totalTarget;
                totalRow.Col.RealisasiNow = totalNow;
                totalRow.Col.RealisasiMinSatu = totalPrev;
                totalRow.Col.Selisih = totalNow - totalPrev;
                totalRow.Col.Persentase = totalTarget > 0
                    ? Math.Round((totalNow / totalTarget) * 100, 2)
                    : 0;

                result.Insert(0, totalRow);

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
                    opdVm.Col.Bulan = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM", new CultureInfo("id-ID"));
                    opdVm.Col.Nama = opd.NamaOpd;
                    opdVm.Col.Target = opd.Target;
                    opdVm.Col.RealisasiNow = opd.RealisasiNow;
                    opdVm.Col.RealisasiMinSatu = opd.RealisasiMinSatu;
                    opdVm.Col.Selisih = opd.RealisasiNow - opd.RealisasiMinSatu;
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
                        subVm.Col.Selisih = sub.RealisasiNow - sub.RealisasiMinSatu;
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
                total.Col.Selisih = totalNow - totalPrev;
                total.Col.Persentase = totalTarget > 0
                    ? Math.Round((totalNow / totalTarget) * 100, 2)
                    : 0;

                result.Insert(0, total);

                return result;
            }

        }
    }
}
