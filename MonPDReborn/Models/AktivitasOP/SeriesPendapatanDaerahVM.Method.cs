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

                var context = DBClass.GetContext();

                var groupRekening = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == year)
                    .GroupBy(x => new {
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
                    .Select(x => new {
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
                    })
                    .ToList();

                var akunList = groupRekening
                    .GroupBy(x => new { x.Akun, x.NamaAkun })
                    .Select(x => new { x.Key.Akun, x.Key.NamaAkun })
                    .OrderBy(x => x.Akun)
                    .ToList();

                var kelompokList = groupRekening
                    .GroupBy(x => new { x.Akun, x.Kelompok, x.NamaKelompok })
                    .Select(x => new { x.Key.Akun, x.Key.Kelompok, x.Key.NamaKelompok })
                    .OrderBy(x => x.Kelompok)
                    .ToList();

                var jenisList = groupRekening
                    .GroupBy(x => new { x.Kelompok, x.Jenis, x.NamaJenis })
                    .Select(x => new { x.Key.Kelompok, x.Key.Jenis, x.Key.NamaJenis })
                    .OrderBy(x => x.Jenis)
                    .ToList();

                var objekList = groupRekening
                    .GroupBy(x => new { x.Jenis, x.Objek, x.NamaObjek })
                    .Select(x => new { x.Key.Jenis, x.Key.Objek, x.Key.NamaObjek })
                    .OrderBy(x => x.Objek)
                    .ToList();

                var rincianList = groupRekening
                    .GroupBy(x => new { x.Objek, x.Rincian, x.NamaRincian })
                    .Select(x => new { x.Key.Objek, x.Key.Rincian, x.Key.NamaRincian })
                    .OrderBy(x => x.Rincian)
                    .ToList();

                var subrincianList = groupRekening
                    .GroupBy(x => new { x.Rincian, x.SubRincian, x.NamaSubRincian })
                    .Select(x => new { x.Key.Rincian, x.Key.SubRincian, x.Key.NamaSubRincian })
                    .OrderBy(x => x.SubRincian)
                    .ToList();


                var dataTahun1 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year);

                int totalRekening = subrincianList.Count;

                int processed = 0;
                foreach (var akun in akunList)
                {
                    decimal target1 = dataTahun1.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi1 = dataTahun1.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase1 = target1 > 0 ? Math.Round((realisasi1 / target1) * 100, 2) : 0;

                    var resAkun = new ViewModel.ShowSeriesSudutPandangRekening.Akun();
                    resAkun.Col.Kode = akun.Akun;
                    resAkun.Col.Nama = $"{akun.Akun}-{akun.NamaAkun}";

                    resAkun.Col.Tahun1 = year;
                    resAkun.Col.TargetTahun1 = target1;
                    resAkun.Col.RealisasiTahun1 = realisasi1;
                    resAkun.Col.PersentaseTahun1 = persentase1;

                    foreach (var kelompok in kelompokList.Where(x => x.Akun == akun.Akun).ToList())
                    {
                        decimal kelompoktarget1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase1 = kelompoktarget1 > 0 ? Math.Round((kelompokrealisasi1 / kelompoktarget1) * 100, 2) : 0;


                        var resKelompok = new ViewModel.ShowSeriesSudutPandangRekening.Kelompok();

                        resKelompok.Col.Kode = kelompok.Kelompok;
                        resKelompok.Col.Nama = $"{kelompok.Kelompok}-{kelompok.NamaKelompok}";

                        resKelompok.Col.Tahun1 = year;
                        resKelompok.Col.TargetTahun1 = kelompoktarget1;
                        resKelompok.Col.RealisasiTahun1 = kelompokrealisasi1;
                        resKelompok.Col.PersentaseTahun1 = kelompokpersentase1;


                        foreach (var jenis in jenisList.Where(x => x.Kelompok == kelompok.Kelompok).ToList())
                        {
                            decimal jenistarget1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase1 = jenistarget1 > 0 ? Math.Round((jenisrealisasi1 / jenistarget1) * 100, 2) : 0;

                            var resJenis = new ViewModel.ShowSeriesSudutPandangRekening.Jenis();

                            resJenis.Col.Kode = jenis.Jenis;
                            resJenis.Col.Nama = $"{jenis.Jenis} - {jenis.NamaJenis}";

                            resJenis.Col.Tahun1 = year;
                            resJenis.Col.TargetTahun1 = jenistarget1;
                            resJenis.Col.RealisasiTahun1 = jenisrealisasi1;
                            resJenis.Col.PersentaseTahun1 = jenispersentase1;

                            foreach (var objek in objekList.Where(x => x.Jenis == jenis.Jenis).ToList())
                            {
                                decimal objektarget1 = dataTahun1
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Target);
                                decimal objekrealisasi1 = dataTahun1
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Realisasi);
                                decimal objekpersentase1 = objektarget1 > 0 ? Math.Round((objekrealisasi1 / objektarget1) * 100, 2) : 0;

                                var resObjek = new ViewModel.ShowSeriesSudutPandangRekening.Objek();

                                resObjek.Col.Kode = objek.Objek;
                                resObjek.Col.Nama = $"{objek.Objek} - {objek.NamaObjek}";

                                resObjek.Col.Tahun1 = year;
                                resObjek.Col.TargetTahun1 = objektarget1;
                                resObjek.Col.RealisasiTahun1 = objekrealisasi1;
                                resObjek.Col.PersentaseTahun1 = objekpersentase1;

                                foreach (var rincian in rincianList.Where(x => x.Objek == objek.Objek).ToList())
                                {
                                    decimal rinciantarget1 = dataTahun1
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Target);
                                    decimal rincianrealisasi1 = dataTahun1
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Realisasi);
                                    decimal rincianpersentase1 = rinciantarget1 > 0 ? Math.Round((rincianrealisasi1 / rinciantarget1) * 100, 2) : 0;


                                    var resRincian = new ViewModel.ShowSeriesSudutPandangRekening.Rincian();

                                    resRincian.Col.Kode = rincian.Rincian;
                                    resRincian.Col.Nama = $"{rincian.Rincian} - {rincian.NamaRincian}";

                                    resRincian.Col.Tahun1 = year;
                                    resRincian.Col.TargetTahun1 = rinciantarget1;
                                    resRincian.Col.RealisasiTahun1 = rincianrealisasi1;
                                    resRincian.Col.PersentaseTahun1 = rincianpersentase1;

                                    foreach (var subRincian in subrincianList.Where(x => x.Rincian == rincian.Rincian).ToList())
                                    {
                                        decimal subrinciantarget1 = dataTahun1
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Target);
                                        decimal subrincianrealisasi1 = dataTahun1
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Realisasi);
                                        decimal subrincianpersentase1 = subrinciantarget1 > 0 ? Math.Round((subrincianrealisasi1 / subrinciantarget1) * 100, 2) : 0;



                                        var resSubRincian = new ViewModel.ShowSeriesSudutPandangRekening.SubRincian();

                                        resSubRincian.Col.Kode = subRincian.SubRincian;
                                        resSubRincian.Col.Nama = $"{subRincian.SubRincian} - {subRincian.NamaSubRincian}";

                                        resSubRincian.Col.Tahun1 = year;
                                        resSubRincian.Col.TargetTahun1 = subrinciantarget1;
                                        resSubRincian.Col.RealisasiTahun1 = subrincianrealisasi1;
                                        resSubRincian.Col.PersentaseTahun1 = subrincianpersentase1;

                                        resRincian.RekSubRincians.Add(resSubRincian);
                                        processed++;
                                        double percent = (processed / (double)totalRekening) * 100;

                                        Console.Write($"\rProgress: {percent:F2}% ({processed}/{totalRekening})");
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
            public static List<ViewModel.ShowSeriesSudutPandangOpd.Opd> GetSudutPandangOpdData(int year)
            {
                var result = new List<ViewModel.ShowSeriesSudutPandangOpd.Opd>();

                var context = DBClass.GetContext();

                var groupOpd = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == year)
                    .GroupBy(x => new {
                        x.KodeOpd,
                        x.NamaOpd,
                        x.KodeSubOpd,
                        x.NamaSubOpd,
                    })
                    .Select(x => new {
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.KodeSubOpd,
                        x.Key.NamaSubOpd,
                    })
                    .ToList();

                var opdList = groupOpd
                    .GroupBy(x => new { x.KodeOpd, x.NamaOpd })
                    .Select(x => new { x.Key.KodeOpd, x.Key.NamaOpd })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();

                var subOpdList = groupOpd
                    .GroupBy(x => new { x.KodeOpd, x.KodeSubOpd, x.NamaSubOpd })
                    .Select(x => new { x.Key.KodeOpd, x.Key.KodeSubOpd, x.Key.NamaSubOpd })
                    .ToList();

                var dataTahun1 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangOpd(year);

                int totalData = subOpdList.Count;
                int processed = 0;

                foreach (var opd in opdList)
                {
                    decimal target1 = dataTahun1.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd).Sum(q => q.Col.Target);
                    decimal realisasi1 = dataTahun1.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd).Sum(q => q.Col.Realisasi);
                    decimal persentase1 = target1 > 0 ? Math.Round((realisasi1 / target1) * 100, 2) : 0;

                    var resOpd = new ViewModel.ShowSeriesSudutPandangOpd.Opd();
                    resOpd.Col.Kode = opd.KodeOpd;
                    resOpd.Col.Nama = opd.NamaOpd;

                    resOpd.Col.Tahun1 = year;
                    resOpd.Col.TargetTahun1 = target1;
                    resOpd.Col.RealisasiTahun1 = realisasi1;
                    resOpd.Col.PersentaseTahun1 = persentase1;

                    foreach (var subOpd in subOpdList.Where(x => x.KodeOpd == opd.KodeOpd).ToList())
                    {
                        decimal subOpdtarget1 = dataTahun1.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd)
                            .SelectMany(x => x.RekSubOpds.Where(x => x.Col.Kode == subOpd.KodeSubOpd && x.Col.Nama == subOpd.NamaSubOpd))
                            .Sum(q => q.Col.Target);
                        decimal subOpdrealisasi1 = dataTahun1.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd)
                            .SelectMany(x => x.RekSubOpds.Where(x => x.Col.Kode == subOpd.KodeSubOpd && x.Col.Nama == subOpd.NamaSubOpd))
                            .Sum(q => q.Col.Realisasi);
                        decimal subOpdpersentase1 = subOpdtarget1 > 0 ? Math.Round((subOpdrealisasi1 / subOpdtarget1) * 100, 2) : 0;

                        var resSubOpd = new ViewModel.ShowSeriesSudutPandangOpd.SubOpd();
                        resSubOpd.Col.Kode = subOpd.KodeSubOpd == "-" ? opd.KodeOpd : subOpd.KodeSubOpd;
                        resSubOpd.Col.Nama = subOpd.NamaSubOpd == "-" ? opd.NamaOpd : subOpd.NamaSubOpd;

                        resSubOpd.Col.Tahun1 = year;
                        resSubOpd.Col.TargetTahun1 = subOpdtarget1;
                        resSubOpd.Col.RealisasiTahun1 = subOpdrealisasi1;
                        resSubOpd.Col.PersentaseTahun1 = subOpdpersentase1;

                        resOpd.RekSubOpds.Add(resSubOpd);

                        processed++;
                        double percent = (processed / (double)totalData) * 100;

                        Console.Write($"\rProgress: {percent:F2}% ({processed}/{totalData})");
                    }

                    result.Add(resOpd);
                }

                return result;
            }
            public static List<ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis> GetSudutPandangRekeningJenisObjekOpdData(int year)
            {
                var result = new List<ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis>();


                var context = DBClass.GetContext();

                var groupData = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku == year)
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
                    .Select(x => new {
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        x.Key.Objek,
                        x.Key.NamaObjek,
                        x.Key.KodeOpd,
                        x.Key.NamaOpd,
                        x.Key.SubRincian,
                        x.Key.NamaSubRincian
                    })
                    .ToList();

                var jenisList = groupData
                    .GroupBy(x => new { x.Jenis, x.NamaJenis })
                    .Select(x => new { x.Key.Jenis, x.Key.NamaJenis })
                    .OrderBy(x => x.Jenis)
                    .ToList();

                var objekList = groupData
                    .GroupBy(x => new { x.Jenis, x.Objek, x.NamaObjek })
                    .Select(x => new { x.Key.Jenis, x.Key.Objek, x.Key.NamaObjek })
                    .OrderBy(x => x.Objek)
                    .ToList();

                var opdList = groupData
                    .GroupBy(x => new { x.Objek, x.KodeOpd, x.NamaOpd })
                    .Select(x => new { x.Key.Objek, x.Key.KodeOpd, x.Key.NamaOpd })
                    .OrderBy(x => x.KodeOpd)
                    .ToList();

                var subRincianList = groupData
                    .GroupBy(x => new { x.Objek, x.KodeOpd, x.SubRincian, x.NamaSubRincian })
                    .Select(x => new { x.Key.Objek, x.Key.KodeOpd, x.Key.SubRincian, x.Key.NamaSubRincian })
                    .OrderBy(x => x.SubRincian)
                    .ToList();

                var dataTahun1 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekeningJenisObjekOpd(year);

                int totalData = subRincianList.Count;
                int processed = 0;

                foreach (var jenis in jenisList)
                {
                    decimal jenistarget1 = dataTahun1.Where(w => w.Col.Kode == jenis.Jenis && w.Col.Nama == jenis.NamaJenis).Sum(q => q.Col.Target);
                    decimal jenisrealisasi1 = dataTahun1.Where(w => w.Col.Kode == jenis.Jenis && w.Col.Nama == jenis.NamaJenis).Sum(q => q.Col.Realisasi);
                    decimal jenispersentase1 = jenistarget1 > 0 ? Math.Round((jenisrealisasi1 / jenistarget1) * 100, 2) : 0;

                    var resJenis = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis();

                    resJenis.Col.Kode = jenis.Jenis;
                    resJenis.Col.Nama = jenis.NamaJenis;

                    resJenis.Col.Tahun1 = year;
                    resJenis.Col.TargetTahun1 = jenistarget1;
                    resJenis.Col.RealisasiTahun1 = jenisrealisasi1;
                    resJenis.Col.PersentaseTahun1 = jenispersentase1;

                    foreach (var objek in objekList.Where(x => x.Jenis == jenis.Jenis).ToList())
                    {
                        decimal objektarget1 = dataTahun1
                            .SelectMany(x => x.RekObyeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                            .Sum(q => q.Col.Target);
                        decimal objekrealisasi1 = dataTahun1
                            .SelectMany(x => x.RekObyeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                            .Sum(q => q.Col.Realisasi);
                        decimal objekpersentase1 = objektarget1 > 0 ? Math.Round((objekrealisasi1 / objektarget1) * 100, 2) : 0;

                        var resObjek = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Obyek();

                        resObjek.Col.Kode = objek.Objek;
                        resObjek.Col.Nama = objek.NamaObjek;

                        resObjek.Col.Tahun1 = year;
                        resObjek.Col.TargetTahun1 = objektarget1;
                        resObjek.Col.RealisasiTahun1 = objekrealisasi1;
                        resObjek.Col.PersentaseTahun1 = objekpersentase1;

                        foreach (var opd in opdList.Where(x => x.Objek == objek.Objek).ToList())
                        {
                            decimal target1 = dataTahun1
                                .SelectMany(x => x.RekObyeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                .SelectMany(x => x.RekOpds.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd))
                                .Sum(q => q.Col.Target);
                            decimal realisasi1 = dataTahun1
                                .SelectMany(x => x.RekObyeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                .SelectMany(x => x.RekOpds.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd))
                                .Sum(q => q.Col.Realisasi);
                            decimal persentase1 = target1 > 0 ? Math.Round((realisasi1 / target1) * 100, 2) : 0;

                            var resOpd = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Opd();
                            resOpd.Col.Kode = opd.KodeOpd;
                            resOpd.Col.Nama = opd.NamaOpd;

                            resOpd.Col.Tahun1 = year;
                            resOpd.Col.TargetTahun1 = target1;
                            resOpd.Col.RealisasiTahun1 = realisasi1;
                            resOpd.Col.PersentaseTahun1 = persentase1;

                            var subRincianList2 = subRincianList.Where(x => x.Objek == objek.Objek && x.KodeOpd == opd.KodeOpd).ToList();
                            foreach (var subRincian in subRincianList2)
                            {
                                decimal subRinciantarget1 = dataTahun1
                                    .SelectMany(x => x.RekObyeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                    .SelectMany(x => x.RekOpds.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd))
                                    .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                    .Sum(q => q.Col.Target);
                                decimal subRincianrealisasi1 = dataTahun1
                                    .SelectMany(x => x.RekObyeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                    .SelectMany(x => x.RekOpds.Where(x => x.Col.Kode == opd.KodeOpd && x.Col.Nama == opd.NamaOpd))
                                    .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                    .Sum(q => q.Col.Realisasi);
                                decimal subRincianpersentase1 = subRinciantarget1 > 0 ? Math.Round((subRincianrealisasi1 / subRinciantarget1) * 100, 2) : 0;

                                var resSubOpd = new ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.SubRincian();
                                resSubOpd.Col.Kode = subRincian.SubRincian;
                                resSubOpd.Col.Nama = $"{subRincian.SubRincian} - {subRincian.NamaSubRincian}";

                                resSubOpd.Col.Tahun1 = year;
                                resSubOpd.Col.TargetTahun1 = subRinciantarget1;
                                resSubOpd.Col.RealisasiTahun1 = subRincianrealisasi1;
                                resSubOpd.Col.PersentaseTahun1 = subRincianpersentase1;

                                resOpd.RekSubRincians.Add(resSubOpd);

                                processed++;
                                double percent = (processed / (double)totalData) * 100;

                                Console.Write($"\rProgress: {percent:F2}% ({processed}/{totalData})");
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
