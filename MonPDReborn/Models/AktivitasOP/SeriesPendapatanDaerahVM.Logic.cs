using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahVMLogic
    {
        public class Method
        {
            public static List<ViewModel.TahunRow> GetTahunList()
            {
                var result = new List<ViewModel.TahunRow>();
                var context = DBClass.GetContext();

                result = context.DbPendapatanDaerahs
                    .Select(x => new ViewModel.TahunRow { Tahun = Convert.ToInt32(x.TahunBuku) })
                    .Distinct()
                    .OrderByDescending(x => x.Tahun)
                    .ToList();

                return result;
            }
            public static List<ViewModel.SudutPandangRekening.Akun> GetDataSudutPandangRekening(int tahunBuku)
            {
                var result = new List<ViewModel.SudutPandangRekening.Akun>();

                var context = DBClass.GetContext();

                var query = context.DbPendapatanDaerahs.Where(x => x.TahunBuku == tahunBuku)
                    .GroupBy(x => new
                    {
                        x.Akun,
                        x.NamaAkun,
                        x.Kelompok,
                        x.NamaKelompok,
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek,
                        x.Rincian,
                        x.NamaRincian,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(x => new
                    {
                        x.Key.Akun,
                        x.Key.NamaAkun,
                        x.Key.Kelompok,
                        x.Key.NamaKelompok,
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        x.Key.Objek,
                        x.Key.NamaObjek,
                        x.Key.Rincian,
                        x.Key.NamaRincian,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        Target = x.Sum(y => y.Target),
                        Realisasi = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                var groupByAkun = query
                    .GroupBy(x => new { x.Akun, x.NamaAkun })
                    .Select(x => new { x.Key.Akun, x.Key.NamaAkun, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                    .ToList();

                foreach (var akun in groupByAkun)
                {
                    var resAkun = new ViewModel.SudutPandangRekening.Akun();
                    resAkun.Col.Kode = akun.Akun;
                    resAkun.Col.Nama = akun.NamaAkun;
                    resAkun.Col.Target = akun.Target;
                    resAkun.Col.Realisasi = akun.Realisasi;
                    resAkun.Col.Persentase = akun.Target > 0 ? Math.Round((akun.Realisasi / akun.Target) * 100, 2) : 0;

                    var groupByKelompok = query
                        .Where(x => x.Akun == akun.Akun)
                        .GroupBy(x => new { x.Kelompok, x.NamaKelompok })
                        .Select(x => new { x.Key.Kelompok, x.Key.NamaKelompok, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                        .ToList();

                    foreach (var kelompok in groupByKelompok)
                    {
                        var resKelompok = new ViewModel.SudutPandangRekening.Kelompok();
                        resKelompok.Col.Kode = kelompok.Kelompok;
                        resKelompok.Col.Nama = kelompok.NamaKelompok;
                        resKelompok.Col.Target = kelompok.Target;
                        resKelompok.Col.Realisasi = kelompok.Realisasi;
                        resKelompok.Col.Persentase = kelompok.Target > 0 ? Math.Round((kelompok.Realisasi / kelompok.Target) * 100, 2) : 0;

                        var groupByJenis = query
                            .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok)
                            .GroupBy(x => new { x.Jenis, x.NamaJenis })
                            .Select(x => new { x.Key.Jenis, x.Key.NamaJenis, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                            .ToList();

                        foreach (var jenis in groupByJenis)
                        {
                            var resJenis = new ViewModel.SudutPandangRekening.Jenis();
                            resJenis.Col.Kode = jenis.Jenis;
                            resJenis.Col.Nama = jenis.NamaJenis;
                            resJenis.Col.Target = jenis.Target;
                            resJenis.Col.Realisasi = jenis.Realisasi;
                            resJenis.Col.Persentase = jenis.Target > 0 ? Math.Round((jenis.Realisasi / jenis.Target) * 100, 2) : 0;

                            var groupByObjek = query
                                .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok && x.Jenis == jenis.Jenis)
                                .GroupBy(x => new { x.Objek, x.NamaObjek })
                                .Select(x => new { x.Key.Objek, x.Key.NamaObjek, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                                .ToList();

                            foreach (var objek in groupByObjek)
                            {
                                var resObjek = new ViewModel.SudutPandangRekening.Objek();
                                resObjek.Col.Kode = objek.Objek;
                                resObjek.Col.Nama = objek.NamaObjek;
                                resObjek.Col.Target = objek.Target;
                                resObjek.Col.Realisasi = objek.Realisasi;
                                resObjek.Col.Persentase = objek.Target > 0 ? Math.Round((objek.Realisasi / objek.Target) * 100, 2) : 0;

                                var groupByRincian = query
                                    .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok && x.Jenis == jenis.Jenis && x.Objek == objek.Objek)
                                    .GroupBy(x => new { x.Rincian, x.NamaRincian })
                                    .Select(x => new { x.Key.Rincian, x.Key.NamaRincian, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                                    .ToList();

                                foreach (var rincian in groupByRincian)
                                {
                                    var resRincian = new ViewModel.SudutPandangRekening.Rincian();
                                    resRincian.Col.Kode = rincian.Rincian;
                                    resRincian.Col.Nama = rincian.NamaRincian;
                                    resRincian.Col.Target = rincian.Target;
                                    resRincian.Col.Realisasi = rincian.Realisasi;
                                    resRincian.Col.Persentase = rincian.Target > 0 ? Math.Round((rincian.Realisasi / rincian.Target) * 100, 2) : 0;

                                    var groupBySubrincian = query
                                        .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok && x.Jenis == jenis.Jenis && x.Objek == objek.Objek && x.Rincian == rincian.Rincian)
                                        .GroupBy(x => new { x.SubRincian, x.NamaSubRincian })
                                        .Select(x => new { x.Key.SubRincian, x.Key.NamaSubRincian, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                                        .ToList();

                                    foreach (var subrincian in groupBySubrincian)
                                    {
                                        var resSubRincian = new ViewModel.SudutPandangRekening.SubRincian();
                                        resSubRincian.Col.Kode = subrincian.SubRincian;
                                        resSubRincian.Col.Nama = subrincian.NamaSubRincian;
                                        resSubRincian.Col.Target = subrincian.Target;
                                        resSubRincian.Col.Realisasi = subrincian.Realisasi;
                                        resSubRincian.Col.Persentase = subrincian.Target > 0 ? Math.Round((subrincian.Realisasi / subrincian.Target) * 100, 2) : 0;

                                        resRincian.RekSubRincians.Add(resSubRincian);
                                    }
                                    resObjek.RekRincians.Add(resRincian);
                                }
                                resJenis.RekObjeks.Add(resObjek);
                            }
                            resKelompok.RekJeniss.Add(resJenis);
                        }
                        resAkun.RekKelompoks.Add(resKelompok);
                    }
                    result.Add(resAkun);
                }

                return result;
            }
            public static List<ViewModel.SudutPandangOpd.Opd> GetDataSudutPandangOpd(int tahunBuku)
            {
                var result = new List<ViewModel.SudutPandangOpd.Opd>();

                var context = DBClass.GetContext();

                var query = context.DbPendapatanDaerahs.Where(x => x.TahunBuku == tahunBuku)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.KodeSubOpd,
                        x.NamaSubOpd
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.KodeSubOpd,
                        x.Key.NamaSubOpd,
                        Target = x.Sum(y => y.Target),
                        Realisasi = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                var groupByOpd = query
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(x => new { x.Key.KodeOpd, x.Key.NamaOpd, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                    .ToList();

                foreach (var opd in groupByOpd)
                {
                    var resOpd = new ViewModel.SudutPandangOpd.Opd();
                    resOpd.Col.Kode = opd.KodeOpd;
                    resOpd.Col.Nama = opd.NamaOpd;
                    resOpd.Col.Target = opd.Target;
                    resOpd.Col.Realisasi = opd.Realisasi;
                    resOpd.Col.Persentase = opd.Target > 0 ? Math.Round((opd.Realisasi / opd.Target) * 100, 2) : 0;

                    var groupBySubOpd = query
                        .Where(x => x.KodeOpd == opd.KodeOpd)
                        .GroupBy(x => new { x.KodeSubOpd, x.NamaSubOpd })
                        .Select(x => new { x.Key.KodeSubOpd, x.Key.NamaSubOpd, Target = x.Sum(q => q.Target), Realisasi = x.Sum(q => q.Realisasi) })
                        .ToList();

                    foreach (var subopd in groupBySubOpd)
                    {
                        var resSubOpd = new ViewModel.SudutPandangOpd.SubOpd();
                        resSubOpd.Col.Kode = subopd.KodeSubOpd;
                        resSubOpd.Col.Nama = subopd.NamaSubOpd;
                        resSubOpd.Col.Target = subopd.Target;
                        resSubOpd.Col.Realisasi = subopd.Realisasi;
                        resSubOpd.Col.Persentase = subopd.Target > 0 ? Math.Round((subopd.Realisasi / subopd.Target) * 100, 2) : 0;

                        resOpd.RekSubOpds.Add(resSubOpd);
                    }
                    result.Add(resOpd);
                }

                return result;
            }
            public static List<ViewModel.SudutPandangRekeningJenisObjekOpd.Jenis> GetDataSudutPandangRekeningJenisObjekOpd(int tahunBuku)
            {
                var result = new List<ViewModel.SudutPandangRekeningJenisObjekOpd.Jenis>();

                var context = DBClass.GetContext();

                var query = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == tahunBuku)
                    .GroupBy(x => new
                    {
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek,
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(x => new
                    {
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        x.Key.Objek,
                        x.Key.NamaObjek,
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        Target = x.Sum(y => y.Target),
                        Realisasi = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                var groupByJenis = query
                    .GroupBy(x => new { x.Jenis, x.NamaJenis })
                    .Select(x => new
                    {
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        Target = x.Sum(q => q.Target),
                        Realisasi = x.Sum(q => q.Realisasi)
                    })
                    .ToList();

                foreach (var jenis in groupByJenis)
                {
                    var resJenis = new ViewModel.SudutPandangRekeningJenisObjekOpd.Jenis();
                    resJenis.Col.Kode = jenis.Jenis;
                    resJenis.Col.Nama = jenis.NamaJenis;
                    resJenis.Col.Target = jenis.Target;
                    resJenis.Col.Realisasi = jenis.Realisasi;
                    resJenis.Col.Persentase = jenis.Target > 0 ? Math.Round((jenis.Realisasi / jenis.Target) * 100, 2) : 0;

                    // Grouping level Obyek
                    var groupByObjek = query
                        .Where(x => x.Jenis == jenis.Jenis)
                        .GroupBy(x => new { x.Objek, x.NamaObjek })
                        .Select(x => new
                        {
                            x.Key.Objek,
                            x.Key.NamaObjek,
                            Target = x.Sum(q => q.Target),
                            Realisasi = x.Sum(q => q.Realisasi)
                        })
                        .ToList();

                    foreach (var objek in groupByObjek)
                    {
                        var resObjek = new ViewModel.SudutPandangRekeningJenisObjekOpd.Obyek();
                        resObjek.Col.Kode = objek.Objek;
                        resObjek.Col.Nama = objek.NamaObjek;
                        resObjek.Col.Target = objek.Target;
                        resObjek.Col.Realisasi = objek.Realisasi;
                        resObjek.Col.Persentase = objek.Target > 0 ? Math.Round((objek.Realisasi / objek.Target) * 100, 2) : 0;

                        // Grouping level OPD
                        var groupByOpd = query
                            .Where(x => x.Jenis == jenis.Jenis && x.Objek == objek.Objek)
                            .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                            .Select(x => new
                            {
                                x.Key.KodeOpd,
                                x.Key.NamaOpd,
                                Target = x.Sum(q => q.Target),
                                Realisasi = x.Sum(q => q.Realisasi)
                            })
                            .ToList();

                        foreach (var opd in groupByOpd)
                        {
                            var resOpd = new ViewModel.SudutPandangRekeningJenisObjekOpd.Opd();
                            resOpd.Col.Kode = opd.KodeOpd;
                            resOpd.Col.Nama = opd.NamaOpd;
                            resOpd.Col.Target = opd.Target;
                            resOpd.Col.Realisasi = opd.Realisasi;
                            resOpd.Col.Persentase = opd.Target > 0 ? Math.Round((opd.Realisasi / opd.Target) * 100, 2) : 0;

                            // Grouping level Sub OPD
                            var groupBySubRincian = query
                                .Where(x => x.Jenis == jenis.Jenis && x.Objek == objek.Objek && x.KodeOpd == opd.KodeOpd)
                                .GroupBy(x => new { x.SubRincian, x.NamaSubRincian })
                                .Select(x => new
                                {
                                    x.Key.SubRincian,
                                    x.Key.NamaSubRincian,
                                    Target = x.Sum(q => q.Target),
                                    Realisasi = x.Sum(q => q.Realisasi)
                                })
                                .ToList();

                            foreach (var subrincian in groupBySubRincian)
                            {
                                var resSubRincian = new ViewModel.SudutPandangRekeningJenisObjekOpd.SubRincian();
                                resSubRincian.Col.Kode = subrincian.SubRincian;
                                resSubRincian.Col.Nama = subrincian.NamaSubRincian;
                                resSubRincian.Col.Target = subrincian.Target;
                                resSubRincian.Col.Realisasi = subrincian.Realisasi;
                                resSubRincian.Col.Persentase = subrincian.Target > 0 ? Math.Round((subrincian.Realisasi / subrincian.Target) * 100, 2) : 0;

                                resOpd.RekSubRincians.Add(resSubRincian);
                            }

                            resObjek.RekOpds.Add(resOpd);
                        }

                        resJenis.RekObyeks.Add(resObjek);
                    }

                    result.Add(resJenis);
                }

                return result;
            }

        }
    }

    public class ViewModel
    {
        public class SudutPandangRekening
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
        public class SudutPandangOpd
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
        public class SudutPandangRekeningJenisObjekOpd
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
    }
}