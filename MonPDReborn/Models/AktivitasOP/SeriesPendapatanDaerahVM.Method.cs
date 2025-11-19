using MonPDLib;
using System.Diagnostics;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahVMMethod
    {
        public class Method
        {
            public static List<ViewModel.TahunRow> GetTahunList()
            {
                return SeriesPendapatanDaerahVMLogic.Method.GetTahunList().Select(x => new ViewModel.TahunRow { Tahun = x.Tahun }).ToList();
            }
            public static List<ViewModel.ShowSeriesSudutPandangRekening.Akun> GetSudutPandangRekeningData(int year)
            {
                var result = new List<ViewModel.ShowSeriesSudutPandangRekening.Akun>();

                // Ambil data dari Dashboard (sudah berisi Target & Realisasi yang benar)
                var data = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year);

                foreach (var akun in data)
                {
                    var vmAkun = new ViewModel.ShowSeriesSudutPandangRekening.Akun();
                    vmAkun.Col.Kode = akun.Col.Kode;
                    vmAkun.Col.Nama = akun.Col.Nama;

                    vmAkun.Col.Tahun1 = year;
                    vmAkun.Col.TargetTahun1 = akun.Col.Target;
                    vmAkun.Col.RealisasiTahun1 = akun.Col.Realisasi;
                    vmAkun.Col.PersentaseTahun1 = akun.Col.Persentase;

                    foreach (var kelompok in akun.RekKelompoks)
                    {
                        var vmKelompok = new ViewModel.ShowSeriesSudutPandangRekening.Kelompok();
                        vmKelompok.Col.Kode = kelompok.Col.Kode;
                        vmKelompok.Col.Nama = kelompok.Col.Nama;

                        vmKelompok.Col.Tahun1 = year;
                        vmKelompok.Col.TargetTahun1 = kelompok.Col.Target;
                        vmKelompok.Col.RealisasiTahun1 = kelompok.Col.Realisasi;
                        vmKelompok.Col.PersentaseTahun1 = kelompok.Col.Persentase;

                        foreach (var jenis in kelompok.RekJeniss)
                        {
                            var vmJenis = new ViewModel.ShowSeriesSudutPandangRekening.Jenis();
                            vmJenis.Col.Kode = jenis.Col.Kode;
                            vmJenis.Col.Nama = jenis.Col.Nama;

                            vmJenis.Col.Tahun1 = year;
                            vmJenis.Col.TargetTahun1 = jenis.Col.Target;
                            vmJenis.Col.RealisasiTahun1 = jenis.Col.Realisasi;
                            vmJenis.Col.PersentaseTahun1 = jenis.Col.Persentase;

                            foreach (var objek in jenis.RekObjeks)
                            {
                                var vmObjek = new ViewModel.ShowSeriesSudutPandangRekening.Objek();
                                vmObjek.Col.Kode = objek.Col.Kode;
                                vmObjek.Col.Nama = objek.Col.Nama;

                                vmObjek.Col.Tahun1 = year;
                                vmObjek.Col.TargetTahun1 = objek.Col.Target;
                                vmObjek.Col.RealisasiTahun1 = objek.Col.Realisasi;
                                vmObjek.Col.PersentaseTahun1 = objek.Col.Persentase;

                                foreach (var rincian in objek.RekRincians)
                                {
                                    var vmRincian = new ViewModel.ShowSeriesSudutPandangRekening.Rincian();
                                    vmRincian.Col.Kode = rincian.Col.Kode;
                                    vmRincian.Col.Nama = rincian.Col.Nama;

                                    vmRincian.Col.Tahun1 = year;
                                    vmRincian.Col.TargetTahun1 = rincian.Col.Target;
                                    vmRincian.Col.RealisasiTahun1 = rincian.Col.Realisasi;
                                    vmRincian.Col.PersentaseTahun1 = rincian.Col.Persentase;

                                    foreach (var sub in rincian.RekSubRincians)
                                    {
                                        var vmSub = new ViewModel.ShowSeriesSudutPandangRekening.SubRincian();
                                        vmSub.Col.Kode = sub.Col.Kode;
                                        vmSub.Col.Nama = sub.Col.Nama;

                                        vmSub.Col.Tahun1 = year;
                                        vmSub.Col.TargetTahun1 = sub.Col.Target;
                                        vmSub.Col.RealisasiTahun1 = sub.Col.Realisasi;
                                        vmSub.Col.PersentaseTahun1 = sub.Col.Persentase;

                                        vmRincian.RekSubRincians.Add(vmSub);
                                    }

                                    vmObjek.RekRincians.Add(vmRincian);
                                }

                                vmJenis.RekObjeks.Add(vmObjek);
                            }

                            vmKelompok.RekJeniss.Add(vmJenis);
                        }

                        vmAkun.RekKelompoks.Add(vmKelompok);
                    }

                    result.Add(vmAkun);
                }

                return result;
            }

            public static List<ViewModel.ShowSeriesSudutPandangOpd.Opd> GetSudutPandangOpdData(int year)
            {
                var result = new List<ViewModel.ShowSeriesSudutPandangOpd.Opd>();

                var context = DBClass.GetContext();
                DateTime cutOff = DateTime.Now.Date;

                // --- TARGET SETAHUN ---
                var targetData = context.DbPendapatanDaerahHarians
                    .Where(x => x.TahunBuku == year)
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd, x.KodeSubOpd, x.NamaSubOpd })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.KodeSubOpd,
                        g.Key.NamaSubOpd,
                        Target = g.Sum(x => x.Target)
                    })
                    .ToList();

                // --- REALISASI S.D CUT OFF ---
                var realisasiData = context.DbPendapatanDaerahHarians
                    .Where(x => x.TahunBuku == year && x.Tanggal <= cutOff)
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd, x.KodeSubOpd, x.NamaSubOpd })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.KodeSubOpd,
                        g.Key.NamaSubOpd,
                        Realisasi = g.Sum(x => x.Realisasi)
                    })
                    .ToList();

                // --- MERGE TARGET & REALISASI ---
                var merged =
                    from t in targetData
                    join r in realisasiData
                        on new { t.KodeOpd, t.KodeSubOpd } equals new { r.KodeOpd, r.KodeSubOpd }
                        into gj
                    from r in gj.DefaultIfEmpty()
                    select new
                    {
                        t.KodeOpd,
                        t.NamaOpd,
                        t.KodeSubOpd,
                        t.NamaSubOpd,
                        Target = t.Target,
                        Realisasi = r?.Realisasi ?? 0
                    };

                // --- LIST OPD UNIK ---
                var opdList = merged
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(x => new { x.Key.KodeOpd, x.Key.NamaOpd })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();

                // --- LIST SUB OPD PER OPD ---
                var subOpdList = merged
                    .GroupBy(x => new { x.KodeOpd, x.KodeSubOpd, x.NamaSubOpd })
                    .Select(x => new { x.Key.KodeOpd, x.Key.KodeSubOpd, x.Key.NamaSubOpd })
                    .ToList();

                // --- BUILD VIEWMODEL ---
                foreach (var opd in opdList)
                {
                    var opdTarget = merged.Where(x => x.KodeOpd == opd.KodeOpd).Sum(x => x.Target);
                    var opdRealisasi = merged.Where(x => x.KodeOpd == opd.KodeOpd).Sum(x => x.Realisasi);

                    var resOpd = new ViewModel.ShowSeriesSudutPandangOpd.Opd();
                    resOpd.Col.Kode = opd.KodeOpd;
                    resOpd.Col.Nama = opd.NamaOpd;
                    resOpd.Col.Tahun1 = year;
                    resOpd.Col.TargetTahun1 = opdTarget;
                    resOpd.Col.RealisasiTahun1 = opdRealisasi;
                    resOpd.Col.PersentaseTahun1 = opdTarget > 0 ? Math.Round((opdRealisasi / opdTarget) * 100, 2) : 0;

                    foreach (var subOpd in subOpdList.Where(x => x.KodeOpd == opd.KodeOpd))
                    {
                        var subTarget = merged.Where(x =>
                                x.KodeOpd == opd.KodeOpd &&
                                x.KodeSubOpd == subOpd.KodeSubOpd)
                            .Sum(x => x.Target);

                        var subRealisasi = merged.Where(x =>
                                x.KodeOpd == opd.KodeOpd &&
                                x.KodeSubOpd == subOpd.KodeSubOpd)
                            .Sum(x => x.Realisasi);

                        var resSub = new ViewModel.ShowSeriesSudutPandangOpd.SubOpd();
                        resSub.Col.Kode = subOpd.KodeSubOpd == "-" ? opd.KodeOpd : subOpd.KodeSubOpd;
                        resSub.Col.Nama = subOpd.NamaSubOpd == "-" ? opd.NamaOpd : subOpd.NamaSubOpd;

                        resSub.Col.Tahun1 = year;
                        resSub.Col.TargetTahun1 = subTarget;
                        resSub.Col.RealisasiTahun1 = subRealisasi;
                        resSub.Col.PersentaseTahun1 = subTarget > 0 ? Math.Round((subRealisasi / subTarget) * 100, 2) : 0;

                        resOpd.RekSubOpds.Add(resSub);
                    }

                    result.Add(resOpd);
                }

                return result;
            }

            public static List<ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis>GetSudutPandangRekeningJenisObjekOpdData(int year)
            {
                var result = new List<ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis>();

                // Data sudah lengkap + sudah dijumlahkan dari logic utama
                var data = SeriesPendapatanDaerahVMLogic.Method
                            .GetDataSudutPandangRekeningJenisObjekOpd(year);

                foreach (var jenis in data)
                {
                    var resJenis = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis();
                    resJenis.Col.Kode = jenis.Col.Kode;
                    resJenis.Col.Nama = jenis.Col.Nama;

                    resJenis.Col.Tahun1 = year;
                    resJenis.Col.TargetTahun1 = jenis.Col.Target;
                    resJenis.Col.RealisasiTahun1 = jenis.Col.Realisasi;
                    resJenis.Col.PersentaseTahun1 =
                        jenis.Col.Target > 0 ? Math.Round((jenis.Col.Realisasi / jenis.Col.Target) * 100, 2) : 0;

                    // ========== LEVEL OBJEK ==========
                    foreach (var objek in jenis.RekObyeks)
                    {
                        var resObjek = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Obyek();
                        resObjek.Col.Kode = objek.Col.Kode;
                        resObjek.Col.Nama = objek.Col.Nama;

                        resObjek.Col.Tahun1 = year;
                        resObjek.Col.TargetTahun1 = objek.Col.Target;
                        resObjek.Col.RealisasiTahun1 = objek.Col.Realisasi;
                        resObjek.Col.PersentaseTahun1 =
                            objek.Col.Target > 0 ? Math.Round((objek.Col.Realisasi / objek.Col.Target) * 100, 2) : 0;

                        // ========== LEVEL OPD ==========
                        foreach (var opd in objek.RekOpds)
                        {
                            var resOpd = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                            resOpd.Col.Kode = opd.Col.Kode;
                            resOpd.Col.Nama = opd.Col.Nama;

                            resOpd.Col.Tahun1 = year;
                            resOpd.Col.TargetTahun1 = opd.Col.Target;
                            resOpd.Col.RealisasiTahun1 = opd.Col.Realisasi;
                            resOpd.Col.PersentaseTahun1 =
                                opd.Col.Target > 0 ? Math.Round((opd.Col.Realisasi / opd.Col.Target) * 100, 2) : 0;

                            // ========== LEVEL SUB RINCIAN ==========
                            foreach (var sub in opd.RekSubRincians)
                            {
                                var resSub = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                                resSub.Col.Kode = sub.Col.Kode;
                                resSub.Col.Nama = sub.Col.Nama;

                                resSub.Col.Tahun1 = year;
                                resSub.Col.TargetTahun1 = sub.Col.Target;
                                resSub.Col.RealisasiTahun1 = sub.Col.Realisasi;
                                resSub.Col.PersentaseTahun1 =
                                    sub.Col.Target > 0 ? Math.Round((sub.Col.Realisasi / sub.Col.Target) * 100, 2) : 0;

                                resOpd.RekSubRincians.Add(resSub);
                            }

                            resObjek.RekOpds.Add(resOpd);
                        }

                        resJenis.RekObyeks.Add(resObjek);
                    }

                    result.Add(resJenis);
                }

                return result;
            }

            public static ViewModel.ReportTrOpdRinci GetReportTrOpdRinci(int year, int bulan)
            {
                var result = new ViewModel.ReportTrOpdRinci();
                result.Data = SeriesPendapatanDaerahVMLogic.Method.GetDataReportTrOpdRinci(year, bulan);
                return result;
            }
            public static ViewModel.ReportTRopdWarna GetReportTRopdWarna(int year, int bulan)
            {
                var result = new ViewModel.ReportTRopdWarna();
                result.Data = SeriesPendapatanDaerahVMLogic.Method.GetDataReportTRopdWarna(year, bulan);
                return result;
            }
        }

        public class ViewModel
        {
            public class ShowSeriesSudutPandangRekening
            {
                public class Akun
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Kelompok> RekKelompoks { get; set; } = new List<Kelompok>();
                }
                public class Kelompok
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Jenis> RekJeniss { get; set; } = new List<Jenis>();
                }
                public class Jenis
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Objek> RekObjeks { get; set; } = new List<Objek>();
                }
                public class Objek
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Rincian> RekRincians { get; set; } = new List<Rincian>();
                }
                public class Rincian
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<SubRincian> RekSubRincians { get; set; } = new List<SubRincian>();
                }
                public class SubRincian
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                }
            }
            public class ShowSeriesSudutPandangOpd
            {
                public class Opd
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<SubOpd> RekSubOpds { get; set; } = new List<SubOpd>();
                }
                public class SubOpd
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                }
            }
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
            public class TahunRow
            {
                public int Tahun { get; set; }
            }
            public class ReportTrOpdRinci
            {
                public List<SeriesPendapatanDaerahVMLogic.ViewModel.ReportTrOpdRinci.Opd> Data { get; set; } = new();
            }
            public class ReportTRopdWarna
            {
                public List<SeriesPendapatanDaerahVMLogic.ViewModel.ReportTRopdWarna.Opd> Data { get; set; } = new();
            }
        }

        public class FormatColumn
        {
            public class ColumnA
            {
                public string Kode { get; set; } = "";
                public string Nama { get; set; } = "";

                public int Tahun1 { get; set; } //2025
                public decimal TargetTahun1 { get; set; }
                public decimal RealisasiTahun1 { get; set; }
                public decimal PersentaseTahun1 { get; set; }
            }
        }
    }
}
