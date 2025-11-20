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
                var planningContext = DBClass.GetEPlanningContext();

                // Cutoff: akhir tahun agar sama dengan series-per-tahun behaviour
                DateTime TglCutOff = new DateTime(tahunBuku, 12, 31);

                // 1) Target tahunan (AkpTahun) dari tabel harian
                var akpTahunQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.TahunBuku == tahunBuku)
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
                    .Select(g => new
                    {
                        g.Key.Akun,
                        g.Key.NamaAkun,
                        g.Key.Kelompok,
                        g.Key.NamaKelompok,
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.Rincian,
                        g.Key.NamaRincian,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        AkpTahun = g.Sum(y => y.Target)
                    })
                    .ToList();

                // 2) Realisasi pada hari cutoff (harian)
                var realisasiHariQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.Tanggal.Date == TglCutOff.Date
                        && !(x.Kelompok == "4.2" && x.TahunBuku == TglCutOff.Year)
                        && !(x.Jenis == "4.1.03" && x.TahunBuku == TglCutOff.Year))
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
                    .Select(g => new
                    {
                        g.Key.Akun,
                        g.Key.NamaAkun,
                        g.Key.Kelompok,
                        g.Key.NamaKelompok,
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.Rincian,
                        g.Key.NamaRincian,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        RealisasiHariAccrual = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // 3) Realisasi s/d cutoff (harian)
                var realisasiSdQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.Tanggal.Date <= TglCutOff.Date
                        && !(x.Kelompok == "4.2" && x.TahunBuku == TglCutOff.Year)
                        && !(x.Jenis == "4.1.03" && x.TahunBuku == TglCutOff.Year))
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
                    .Select(g => new
                    {
                        g.Key.Akun,
                        g.Key.NamaAkun,
                        g.Key.Kelompok,
                        g.Key.NamaKelompok,
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.Rincian,
                        g.Key.NamaRincian,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        RealisasiSDHariAccrual = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // 4) Manual input hari ini dari e-planning
                var realisasiInputHari = planningContext.TInputManuals
                    .Where(x => x.Tanggal.Date == TglCutOff.Date)
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
                    .Select(g => new
                    {
                        g.Key.Akun,
                        g.Key.NamaAkun,
                        g.Key.Kelompok,
                        g.Key.NamaKelompok,
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.Rincian,
                        g.Key.NamaRincian,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        RealisasiHariAccrual = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // 5) Manual input s/d hari cutoff
                var realisasiInputSd = planningContext.TInputManuals
                    .Where(x => x.Tanggal.Date <= TglCutOff.Date)
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
                    .Select(g => new
                    {
                        g.Key.Akun,
                        g.Key.NamaAkun,
                        g.Key.Kelompok,
                        g.Key.NamaKelompok,
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.Rincian,
                        g.Key.NamaRincian,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        RealisasiSDHariAccrual = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // 6) Merge semua (left joins)
                var merged = (
                    from a in akpTahunQuery

                    join b in realisasiHariQuery
                        on new
                        {
                            a.Akun,
                            a.Kelompok,
                            a.Jenis,
                            a.Objek,
                            a.Rincian,
                            a.SubRincian
                        }
                        equals new
                        {
                            b.Akun,
                            b.Kelompok,
                            b.Jenis,
                            b.Objek,
                            b.Rincian,
                            b.SubRincian
                        } into gj1
                    from b in gj1.DefaultIfEmpty()

                    join c in realisasiSdQuery
                        on new
                        {
                            a.Akun,
                            a.Kelompok,
                            a.Jenis,
                            a.Objek,
                            a.Rincian,
                            a.SubRincian
                        }
                        equals new
                        {
                            c.Akun,
                            c.Kelompok,
                            c.Jenis,
                            c.Objek,
                            c.Rincian,
                            c.SubRincian
                        } into gj2
                    from c in gj2.DefaultIfEmpty()

                    join d in realisasiInputHari
                        on new
                        {
                            a.Akun,
                            a.Kelompok,
                            a.Jenis,
                            a.Objek,
                            a.Rincian,
                            a.SubRincian
                        }
                        equals new
                        {
                            d.Akun,
                            d.Kelompok,
                            d.Jenis,
                            d.Objek,
                            d.Rincian,
                            d.SubRincian
                        } into gj3
                    from d in gj3.DefaultIfEmpty()

                    join e in realisasiInputSd
                        on new
                        {
                            a.Akun,
                            a.Kelompok,
                            a.Jenis,
                            a.Objek,
                            a.Rincian,
                            a.SubRincian
                        }
                        equals new
                        {
                            e.Akun,
                            e.Kelompok,
                            e.Jenis,
                            e.Objek,
                            e.Rincian,
                            e.SubRincian
                        } into gj4
                    from e in gj4.DefaultIfEmpty()

                    select new
                    {
                        a.Akun,
                        a.NamaAkun,
                        a.Kelompok,
                        a.NamaKelompok,
                        a.Jenis,
                        a.NamaJenis,
                        a.Objek,
                        a.NamaObjek,
                        a.Rincian,
                        a.NamaRincian,
                        a.SubRincian,
                        a.NamaSubRincian,

                        AkpTahun = a.AkpTahun,

                        RealisasiHariAccrual = (b?.RealisasiHariAccrual ?? 0) + (d?.RealisasiHariAccrual ?? 0),

                        RealisasiSDHariAccrual = (c?.RealisasiSDHariAccrual ?? 0) + (e?.RealisasiSDHariAccrual ?? 0)
                    }
                ).ToList();

                // 7) Grouping dan mapping ke ViewModel (hierarki akun -> kelompok -> jenis -> objek -> rincian -> subrincian)
                // Hitung totals menggunakan AkpTahun dan RealisasiSDHariAccrual so Series will match Dashboard
                var groupByAkun = merged
                    .GroupBy(x => new { x.Akun, x.NamaAkun })
                    .Select(g => new
                    {
                        g.Key.Akun,
                        g.Key.NamaAkun,
                        Target = g.Sum(z => z.AkpTahun),
                        Realisasi = g.Sum(z => z.RealisasiSDHariAccrual)
                    })
                    .ToList();

                foreach (var akun in groupByAkun)
                {
                    var resAkun = new ViewModel.SudutPandangRekening.Akun();
                    resAkun.Col.Kode = akun.Akun;
                    resAkun.Col.Nama = akun.NamaAkun;
                    resAkun.Col.Target = akun.Target;
                    resAkun.Col.Realisasi = akun.Realisasi;
                    resAkun.Col.Persentase = akun.Target > 0 ? Math.Round((akun.Realisasi / akun.Target) * 100, 2) : 0;

                    var groupByKelompok = merged
                        .Where(x => x.Akun == akun.Akun)
                        .GroupBy(x => new { x.Kelompok, x.NamaKelompok })
                        .Select(g => new
                        {
                            g.Key.Kelompok,
                            g.Key.NamaKelompok,
                            Target = g.Sum(z => z.AkpTahun),
                            Realisasi = g.Sum(z => z.RealisasiSDHariAccrual)
                        })
                        .ToList();

                    foreach (var kelompok in groupByKelompok)
                    {
                        var resKelompok = new ViewModel.SudutPandangRekening.Kelompok();
                        resKelompok.Col.Kode = kelompok.Kelompok;
                        resKelompok.Col.Nama = kelompok.NamaKelompok;
                        resKelompok.Col.Target = kelompok.Target;
                        resKelompok.Col.Realisasi = kelompok.Realisasi;
                        resKelompok.Col.Persentase = kelompok.Target > 0 ? Math.Round((kelompok.Realisasi / kelompok.Target) * 100, 2) : 0;

                        var groupByJenis = merged
                            .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok)
                            .GroupBy(x => new { x.Jenis, x.NamaJenis })
                            .Select(g => new
                            {
                                g.Key.Jenis,
                                g.Key.NamaJenis,
                                Target = g.Sum(z => z.AkpTahun),
                                Realisasi = g.Sum(z => z.RealisasiSDHariAccrual)
                            })
                            .ToList();

                        foreach (var jenis in groupByJenis)
                        {
                            var resJenis = new ViewModel.SudutPandangRekening.Jenis();
                            resJenis.Col.Kode = jenis.Jenis;
                            resJenis.Col.Nama = jenis.NamaJenis;
                            resJenis.Col.Target = jenis.Target;
                            resJenis.Col.Realisasi = jenis.Realisasi;
                            resJenis.Col.Persentase = jenis.Target > 0 ? Math.Round((jenis.Realisasi / jenis.Target) * 100, 2) : 0;

                            var groupByObjek = merged
                                .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok && x.Jenis == jenis.Jenis)
                                .GroupBy(x => new { x.Objek, x.NamaObjek })
                                .Select(g => new
                                {
                                    g.Key.Objek,
                                    g.Key.NamaObjek,
                                    Target = g.Sum(z => z.AkpTahun),
                                    Realisasi = g.Sum(z => z.RealisasiSDHariAccrual)
                                })
                                .ToList();

                            foreach (var objek in groupByObjek)
                            {
                                var resObjek = new ViewModel.SudutPandangRekening.Objek();
                                resObjek.Col.Kode = objek.Objek;
                                resObjek.Col.Nama = objek.NamaObjek;
                                resObjek.Col.Target = objek.Target;
                                resObjek.Col.Realisasi = objek.Realisasi;
                                resObjek.Col.Persentase = objek.Target > 0 ? Math.Round((objek.Realisasi / objek.Target) * 100, 2) : 0;

                                var groupByRincian = merged
                                    .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok && x.Jenis == jenis.Jenis && x.Objek == objek.Objek)
                                    .GroupBy(x => new { x.Rincian, x.NamaRincian })
                                    .Select(g => new
                                    {
                                        g.Key.Rincian,
                                        g.Key.NamaRincian,
                                        Target = g.Sum(z => z.AkpTahun),
                                        Realisasi = g.Sum(z => z.RealisasiSDHariAccrual)
                                    })
                                    .ToList();

                                foreach (var rincian in groupByRincian)
                                {
                                    var resRincian = new ViewModel.SudutPandangRekening.Rincian();
                                    resRincian.Col.Kode = rincian.Rincian;
                                    resRincian.Col.Nama = rincian.NamaRincian;
                                    resRincian.Col.Target = rincian.Target;
                                    resRincian.Col.Realisasi = rincian.Realisasi;
                                    resRincian.Col.Persentase = rincian.Target > 0 ? Math.Round((rincian.Realisasi / rincian.Target) * 100, 2) : 0;

                                    var groupBySubrincian = merged
                                        .Where(x => x.Akun == akun.Akun && x.Kelompok == kelompok.Kelompok && x.Jenis == jenis.Jenis && x.Objek == objek.Objek && x.Rincian == rincian.Rincian)
                                        .GroupBy(x => new { x.SubRincian, x.NamaSubRincian })
                                        .Select(g => new
                                        {
                                            g.Key.SubRincian,
                                            g.Key.NamaSubRincian,
                                            Target = g.Sum(z => z.AkpTahun),
                                            Realisasi = g.Sum(z => z.RealisasiSDHariAccrual)
                                        })
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

                DateTime today = DateTime.Now.Date;

                // === TARGET TAHUNAN ===
                var targetTahunan = context.DbPendapatanDaerahHarians
                    .Where(x => x.TahunBuku == tahunBuku)
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

                // === REALISASI s.d HARI INI ===
                var realisasiSd = context.DbPendapatanDaerahHarians
                    .Where(x => x.TahunBuku == tahunBuku && x.Tanggal <= today)
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

                // === MERGE ===
                var merged = (
                    from t in targetTahunan
                    join r in realisasiSd
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
                    }
                ).ToList();

                // === GROUP OPD ===
                var opdList = merged
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(g => new
                    {
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        Target = g.Sum(x => x.Target),
                        Realisasi = g.Sum(x => x.Realisasi),
                        SubOpds = g.ToList()
                    })
                    .OrderBy(o => o.KodeOpd)
                    .ToList();

                // === BUILD VIEWMODEL ===
                foreach (var opd in opdList)
                {
                    var resOpd = new ViewModel.SudutPandangOpd.Opd();
                    resOpd.Col.Kode = opd.KodeOpd;
                    resOpd.Col.Nama = opd.NamaOpd;
                    resOpd.Col.Target = opd.Target;
                    resOpd.Col.Realisasi = opd.Realisasi;
                    resOpd.Col.Persentase = opd.Target > 0 ? Math.Round((opd.Realisasi / opd.Target) * 100, 2) : 0;

                    foreach (var sub in opd.SubOpds)
                    {
                        var resSub = new ViewModel.SudutPandangOpd.SubOpd();
                        resSub.Col.Kode = string.IsNullOrWhiteSpace(sub.KodeSubOpd) || sub.KodeSubOpd == "-"
                                         ? opd.KodeOpd
                                         : sub.KodeSubOpd;

                        resSub.Col.Nama = string.IsNullOrWhiteSpace(sub.NamaSubOpd) || sub.NamaSubOpd == "-"
                                         ? opd.NamaOpd
                                         : sub.NamaSubOpd;

                        resSub.Col.Target = sub.Target;
                        resSub.Col.Realisasi = sub.Realisasi;
                        resSub.Col.Persentase = sub.Target > 0 ? Math.Round((sub.Realisasi / sub.Target) * 100, 2) : 0;

                        resOpd.RekSubOpds.Add(resSub);
                    }

                    result.Add(resOpd);
                }

                return result;
            }

            public static List<ViewModel.SudutPandangRekeningJenisObjekOpd.Jenis>GetDataSudutPandangRekeningJenisObjekOpd(int tahunBuku)
            {
                var result = new List<ViewModel.SudutPandangRekeningJenisObjekOpd.Jenis>();

                var context = DBClass.GetContext();
                var planning = DBClass.GetEPlanningContext();

                DateTime today = DateTime.Now.Date;

                // =========================
                // 1) TARGET (TAHUNAN)
                // =========================
                var targetQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.TahunBuku == tahunBuku)
                    .GroupBy(x => new {
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek,
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(g => new {
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        Target = g.Sum(y => y.Target)
                    })
                    .ToList();

                // =========================
                // 2) REALISASI S/D HARI INI
                // =========================
                var realisasiSd = context.DbPendapatanDaerahHarians
                    .Where(x => x.TahunBuku == tahunBuku && x.Tanggal <= today)
                    .GroupBy(x => new {
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek,
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(g => new {
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        Realisasi = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // =========================
                // 3) REALISASI INPUT MANUAL (tambahan)
                // =========================
                var realisasiManual = planning.TInputManuals
                    .Where(x => x.Tanggal <= today)
                    .GroupBy(x => new {
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek,
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian
                    })
                    .Select(g => new {
                        g.Key.Jenis,
                        g.Key.NamaJenis,
                        g.Key.Objek,
                        g.Key.NamaObjek,
                        g.Key.KodeOpd,
                        g.Key.NamaOpd,
                        g.Key.SubRincian,
                        g.Key.NamaSubRincian,
                        RealManual = g.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // =========================
                // 4) MERGE DATA
                // =========================
                var merged = (
                   from t in targetQuery
                   join r in realisasiSd
                      on new { t.Jenis, t.Objek, t.KodeOpd, t.SubRincian }
                      equals new { r.Jenis, r.Objek, r.KodeOpd, r.SubRincian }
                      into gj
                   from r in gj.DefaultIfEmpty()

                   join m in realisasiManual
                      on new { t.Jenis, t.Objek, t.KodeOpd, t.SubRincian }
                      equals new { m.Jenis, m.Objek, m.KodeOpd, m.SubRincian }
                      into gj2
                   from m in gj2.DefaultIfEmpty()

                   select new
                   {
                       t.Jenis,
                       t.NamaJenis,
                       t.Objek,
                       t.NamaObjek,
                       t.KodeOpd,
                       t.NamaOpd,
                       t.SubRincian,
                       t.NamaSubRincian,
                       Target = t.Target,
                       Realisasi = (r?.Realisasi ?? 0) + (m?.RealManual ?? 0)
                   }
                ).ToList();

                // =========================
                // 5) BUILD HIERARKI
                // =========================

                var groupJenis = merged
                    .GroupBy(x => new { x.Jenis, x.NamaJenis })
                    .ToList();

                foreach (var jenis in groupJenis)
                {
                    var vmJenis = new ViewModel.SudutPandangRekeningJenisObjekOpd.Jenis();
                    vmJenis.Col.Kode = jenis.Key.Jenis;
                    vmJenis.Col.Nama = jenis.Key.NamaJenis;
                    vmJenis.Col.Target = jenis.Sum(x => x.Target);
                    vmJenis.Col.Realisasi = jenis.Sum(x => x.Realisasi);
                    vmJenis.Col.Persentase =
                        vmJenis.Col.Target > 0 ? Math.Round(vmJenis.Col.Realisasi / vmJenis.Col.Target * 100, 2) : 0;

                    // OBJEK -----------------------
                    var groupObjek = jenis.GroupBy(x => new { x.Objek, x.NamaObjek });

                    foreach (var objek in groupObjek)
                    {
                        var vmObjek = new ViewModel.SudutPandangRekeningJenisObjekOpd.Obyek();
                        vmObjek.Col.Kode = objek.Key.Objek;
                        vmObjek.Col.Nama = objek.Key.NamaObjek;
                        vmObjek.Col.Target = objek.Sum(x => x.Target);
                        vmObjek.Col.Realisasi = objek.Sum(x => x.Realisasi);
                        vmObjek.Col.Persentase =
                            vmObjek.Col.Target > 0 ? Math.Round(vmObjek.Col.Realisasi / vmObjek.Col.Target * 100, 2) : 0;

                        // OPD ------------------------
                        var groupOpd = objek.GroupBy(x => new { x.KodeOpd, x.NamaOpd });

                        foreach (var opd in groupOpd)
                        {
                            var vmOpd = new ViewModel.SudutPandangRekeningJenisObjekOpd.Opd();
                            vmOpd.Col.Kode = opd.Key.KodeOpd;
                            vmOpd.Col.Nama = opd.Key.NamaOpd;
                            vmOpd.Col.Target = opd.Sum(x => x.Target);
                            vmOpd.Col.Realisasi = opd.Sum(x => x.Realisasi);
                            vmOpd.Col.Persentase =
                                vmOpd.Col.Target > 0 ? Math.Round(vmOpd.Col.Realisasi / vmOpd.Col.Target * 100, 2) : 0;

                            // SUB RINCIAN -----------------------
                            foreach (var sr in opd)
                            {
                                var vmSub = new ViewModel.SudutPandangRekeningJenisObjekOpd.SubRincian();
                                vmSub.Col.Kode = sr.SubRincian;
                                vmSub.Col.Nama = sr.NamaSubRincian;
                                vmSub.Col.Target = sr.Target;
                                vmSub.Col.Realisasi = sr.Realisasi;
                                vmSub.Col.Persentase =
                                    sr.Target > 0 ? Math.Round(sr.Realisasi / sr.Target * 100, 2) : 0;

                                vmOpd.RekSubRincians.Add(vmSub);
                            }

                            vmObjek.RekOpds.Add(vmOpd);
                        }

                        vmJenis.RekObyeks.Add(vmObjek);
                    }

                    result.Add(vmJenis);
                }

                return result;
            }


            public static List<ViewModel.ReportTrOpdRinci.Opd> GetDataReportTrOpdRinci(int tahunBuku, int bulan)
            {
                var result = new List<ViewModel.ReportTrOpdRinci.Opd>();
                var context = DBClass.GetContext();

                var tanggal = DateTime.Now;
                if (tahunBuku < tanggal.Year && bulan < tanggal.Month)
                {
                    tanggal = new DateTime(tahunBuku, bulan, 31);
                }

                var query = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == tahunBuku)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.SubRincian,
                        x.NamaSubRincian,
                        x.Bulan
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian,
                        x.Key.Bulan,
                        Target = x.Sum(y => y.Target),
                        Realisasi = x.Sum(y => y.Realisasi)
                    })
                    .OrderBy(x => x.KodeOpd)
                    .ThenBy(x => x.SubRincian)
                    .ToList();

                var opdList = query.GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(x => new { x.Key.KodeOpd, x.Key.NamaOpd })
                    .ToList();

                foreach (var opd in opdList)
                {
                    string nama = opd.NamaOpd;
                    decimal targetTotal = query.Where(x => x.KodeOpd == opd.KodeOpd).Sum(x => x.Target);
                    decimal targetSampaiDengan = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan <= tanggal.Month).Sum(x => x.Target);
                    decimal realisasiSampaiDengan = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan <= tanggal.Month).Sum(x => x.Realisasi);
                    decimal targetBulanIni = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan == tanggal.Month).Sum(x => x.Target);
                    decimal realisasiBulanIni = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan == tanggal.Month).Sum(x => x.Realisasi);

                    var res = new ViewModel.ReportTrOpdRinci.Opd();
                    res.Col.Nama = nama;
                    res.Col.TargetTotal = targetTotal;
                    res.Col.TargetSampaiDengan = targetSampaiDengan;
                    res.Col.RealisasiSampaiDengan = realisasiSampaiDengan;
                    res.Col.TargetBulanIni = targetBulanIni;
                    res.Col.RealisasiBulanIni = realisasiBulanIni;


                    var subrincianList = query
                        .Where(x => x.KodeOpd == opd.KodeOpd)
                        .GroupBy(x => new { x.SubRincian, x.NamaSubRincian })
                        .Select(x => new { x.Key.SubRincian, x.Key.NamaSubRincian })
                        .ToList();
                    foreach (var subrincian in subrincianList)
                    {
                        string namaSubrincian = subrincian.NamaSubRincian;
                        decimal targetTotalSubrincian = query.Where(x => x.KodeOpd == opd.KodeOpd && x.SubRincian == subrincian.SubRincian).Sum(x => x.Target);
                        decimal targetSampaiDenganSubrincian = query.Where(x => x.KodeOpd == opd.KodeOpd && x.SubRincian == subrincian.SubRincian && x.Bulan <= tanggal.Month).Sum(x => x.Target);
                        decimal realisasiSampaiDenganSubrincian = query.Where(x => x.KodeOpd == opd.KodeOpd && x.SubRincian == subrincian.SubRincian && x.Bulan <= tanggal.Month).Sum(x => x.Realisasi);
                        decimal targetBulanIniSubrincian = query.Where(x => x.KodeOpd == opd.KodeOpd && x.SubRincian == subrincian.SubRincian && x.Bulan == tanggal.Month).Sum(x => x.Target);
                        decimal realisasiBulanIniSubrincian = query.Where(x => x.KodeOpd == opd.KodeOpd && x.SubRincian == subrincian.SubRincian && x.Bulan == tanggal.Month).Sum(x => x.Realisasi);

                        var re = new ViewModel.ReportTrOpdRinci.Subrincian();
                        re.Col.Nama = namaSubrincian;
                        re.Col.TargetTotal = targetTotalSubrincian;
                        re.Col.TargetSampaiDengan = targetSampaiDenganSubrincian;
                        re.Col.RealisasiSampaiDengan = realisasiSampaiDenganSubrincian;
                        re.Col.TargetBulanIni = targetBulanIniSubrincian;
                        re.Col.RealisasiBulanIni = realisasiBulanIniSubrincian;

                        res.SubrincianList.Add(re);
                    }

                    result.Add(res);
                }

                return result;
            }
            public static List<ViewModel.ReportTRopdWarna.Opd> GetDataReportTRopdWarna(int tahunBuku, int bulan)
            {
                var result = new List<ViewModel.ReportTRopdWarna.Opd>();
                var context = DBClass.GetContext();

                var tanggal = DateTime.Now;
                if (tahunBuku < tanggal.Year && bulan < tanggal.Month)
                {
                    tanggal = new DateTime(tahunBuku, bulan, 31);
                }

                var query = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == tahunBuku)
                    .GroupBy(x => new
                    {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.Bulan
                    })
                    .Select(x => new
                    {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.Bulan,
                        Target = x.Sum(y => y.Target),
                        Realisasi = x.Sum(y => y.Realisasi)
                    })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();

                var opdList = query.GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(x => new { x.Key.KodeOpd, x.Key.NamaOpd })
                    .ToList();

                foreach (var opd in opdList)
                {
                    string nama = opd.NamaOpd;
                    decimal targetTotal = query.Where(x => x.KodeOpd == opd.KodeOpd).Sum(x => x.Target);
                    decimal targetSampaiDengan = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan <= tanggal.Month).Sum(x => x.Target);
                    decimal realisasiSampaiDengan = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan <= tanggal.Month).Sum(x => x.Realisasi);
                    decimal targetBulanIni = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan == tanggal.Month).Sum(x => x.Target);
                    decimal realisasiBulanIni = query.Where(x => x.KodeOpd == opd.KodeOpd && x.Bulan == tanggal.Month).Sum(x => x.Realisasi);

                    var res = new ViewModel.ReportTRopdWarna.Opd();
                    res.Col.Nama = nama;
                    res.Col.TargetTotal = targetTotal;
                    res.Col.TargetSampaiDengan = targetSampaiDengan;
                    res.Col.RealisasiSampaiDengan = realisasiSampaiDengan;
                    res.Col.TargetBulanIni = targetBulanIni;
                    res.Col.RealisasiBulanIni = realisasiBulanIni;

                    if (res.Col.PersentaseTotal >= 0 && res.Col.PersentaseTotal < 70)
                    {
                        res.Col.Status = 0; // Merah
                    }
                    else if (res.Col.PersentaseTotal >= 70 && res.Col.PersentaseTotal < 75)
                    {
                        res.Col.Status = 1; // Kuning
                    }
                    else if (res.Col.PersentaseTotal >= 75)
                    {
                        res.Col.Status = 2; // Hijau
                    }

                    result.Add(res);
                }

                return result;
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

            public class ReportTrOpdRinci
            {
                public class Opd
                {
                    public FormatColumn.ReportColumnA Col { get; set; } = new();
                    public List<Subrincian> SubrincianList { get; set; } = new List<Subrincian>();
                }
                public class Subrincian
                {
                    public FormatColumn.ReportColumnA Col { get; set; } = new();
                }
            }

            public class ReportTRopdWarna
            {
                public class Opd
                {
                    public FormatColumn.ReportColumnB Col { get; set; } = new();
                }
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

}