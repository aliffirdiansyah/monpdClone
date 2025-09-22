using MonPDLib;
using static MonPDReborn.Models.AktivitasOP.PemasanganAlatVM;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahUrgentVM
    {
        public class Index
        {
            public List<ViewModel.ShowSeriesSudutPandangRekening.Akun> SudutPandangRekening { get; set; } = Method.GetSudutPandangRekeningData();
            public Index()
            {
                SudutPandangRekening = Method.GetSudutPandangRekeningData();
            }
        }

        public class Method
        {
            public static List<ViewModel.ShowSeriesSudutPandangRekening.Akun> GetSudutPandangRekeningData()
            {
                var result = new List<ViewModel.ShowSeriesSudutPandangRekening.Akun>();

                int year1 = DateTime.Now.Year;
                int year2 = year1 - 1;
                int year3 = year1 - 2;
                int year4 = year1 - 3;
                int year5 = year1 - 4;
                int year6 = year1 - 5;
                int year7 = year1 - 6;

                var context = DBClass.GetContext();

                var groupRekening = context.DbPendapatanDaerahs
                    .Where(x => x.TahunBuku >= year7)
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


                var dataTahun1 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year1);
                var dataTahun2 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year2);
                var dataTahun3 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year3);
                var dataTahun4 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year4);
                var dataTahun5 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year5);
                var dataTahun6 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year6);
                var dataTahun7 = SeriesPendapatanDaerahVMLogic.Method.GetDataSudutPandangRekening(year7);

                int totalRekening = subrincianList.Count;

                int processed = 0;
                foreach (var akun in akunList)
                {
                    decimal target1 = dataTahun1.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi1 = dataTahun1.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase1 = target1 > 0 ? Math.Round((realisasi1 / target1) * 100, 2) : 0;

                    decimal target2 = dataTahun2.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi2 = dataTahun2.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase2 = target2 > 0 ? Math.Round((realisasi2 / target2) * 100, 2) : 0;

                    decimal target3 = dataTahun3.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi3 = dataTahun3.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase3 = target3 > 0 ? Math.Round((realisasi3 / target3) * 100, 2) : 0;

                    decimal target4 = dataTahun4.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi4 = dataTahun4.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase4 = target4 > 0 ? Math.Round((realisasi4 / target4) * 100, 2) : 0;

                    decimal target5 = dataTahun5.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi5 = dataTahun5.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase5 = target5 > 0 ? Math.Round((realisasi5 / target5) * 100, 2) : 0;

                    decimal target6 = dataTahun6.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi6 = dataTahun6.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase6 = target6 > 0 ? Math.Round((realisasi6 / target6) * 100, 2) : 0;

                    decimal target7 = dataTahun7.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Target);
                    decimal realisasi7 = dataTahun7.Where(x => x.Col.Kode == akun.Akun && x.Col.Nama == akun.NamaAkun).Sum(q => q.Col.Realisasi);
                    decimal persentase7 = target7 > 0 ? Math.Round((realisasi7 / target7) * 100, 2) : 0;

                    var resAkun = new ViewModel.ShowSeriesSudutPandangRekening.Akun();
                    resAkun.Col.Kode = akun.Akun;
                    resAkun.Col.Nama = $"{akun.Akun}-{akun.NamaAkun}";

                    resAkun.Col.Tahun1 = year1;
                    resAkun.Col.TargetTahun1 = target1;
                    resAkun.Col.RealisasiTahun1 = realisasi1;
                    resAkun.Col.PersentaseTahun1 = persentase1;

                    resAkun.Col.Tahun2 = year2;
                    resAkun.Col.TargetTahun2 = target2;
                    resAkun.Col.RealisasiTahun2 = realisasi2;
                    resAkun.Col.PersentaseTahun2 = persentase2;

                    resAkun.Col.Tahun3 = year3;
                    resAkun.Col.TargetTahun3 = target3;
                    resAkun.Col.RealisasiTahun3 = realisasi3;
                    resAkun.Col.PersentaseTahun3 = persentase3;

                    resAkun.Col.Tahun4 = year4;
                    resAkun.Col.TargetTahun4 = target4;
                    resAkun.Col.RealisasiTahun4 = realisasi4;
                    resAkun.Col.PersentaseTahun4 = persentase4;

                    resAkun.Col.Tahun5 = year5;
                    resAkun.Col.TargetTahun5 = target5;
                    resAkun.Col.RealisasiTahun5 = realisasi5;
                    resAkun.Col.PersentaseTahun5 = persentase5;

                    resAkun.Col.Tahun6 = year6;
                    resAkun.Col.TargetTahun6 = target6;
                    resAkun.Col.RealisasiTahun6 = realisasi6;
                    resAkun.Col.PersentaseTahun6 = persentase6;

                    resAkun.Col.Tahun7 = year7;
                    resAkun.Col.TargetTahun7 = target7;
                    resAkun.Col.RealisasiTahun7 = realisasi7;
                    resAkun.Col.PersentaseTahun7 = persentase7;

                    foreach (var kelompok in kelompokList.Where(x => x.Akun == akun.Akun).ToList())
                    {
                        decimal kelompoktarget1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase1 = kelompoktarget1 > 0 ? Math.Round((kelompokrealisasi1 / kelompoktarget1) * 100, 2) : 0;

                        decimal kelompoktarget2 = dataTahun2.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi2 = dataTahun2.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase2 = kelompoktarget2 > 0 ? Math.Round((kelompokrealisasi2 / kelompoktarget2) * 100, 2) : 0;

                        decimal kelompoktarget3 = dataTahun3.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi3 = dataTahun3.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase3 = kelompoktarget3 > 0 ? Math.Round((kelompokrealisasi3 / kelompoktarget3) * 100, 2) : 0;

                        decimal kelompoktarget4 = dataTahun4.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi4 = dataTahun4.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase4 = kelompoktarget4 > 0 ? Math.Round((kelompokrealisasi4 / kelompoktarget4) * 100, 2) : 0;

                        decimal kelompoktarget5 = dataTahun5.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi5 = dataTahun5.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase5 = kelompoktarget5 > 0 ? Math.Round((kelompokrealisasi5 / kelompoktarget5) * 100, 2) : 0;

                        decimal kelompoktarget6 = dataTahun6.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi6 = dataTahun6.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase6 = kelompoktarget6 > 0 ? Math.Round((kelompokrealisasi6 / kelompoktarget6) * 100, 2) : 0;

                        decimal kelompoktarget7 = dataTahun7.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Target);
                        decimal kelompokrealisasi7 = dataTahun7.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)).Sum(q => q.Col.Realisasi);
                        decimal kelompokpersentase7 = kelompoktarget7 > 0 ? Math.Round((kelompokrealisasi7 / kelompoktarget7) * 100, 2) : 0;


                        var resKelompok = new ViewModel.ShowSeriesSudutPandangRekening.Kelompok();

                        resKelompok.Col.Kode = kelompok.Kelompok;
                        resKelompok.Col.Nama = $"{kelompok.Kelompok}-{kelompok.NamaKelompok}";

                        resKelompok.Col.Tahun1 = year1;
                        resKelompok.Col.TargetTahun1 = kelompoktarget1;
                        resKelompok.Col.RealisasiTahun1 = kelompokrealisasi1;
                        resKelompok.Col.PersentaseTahun1 = kelompokpersentase1;

                        resKelompok.Col.Tahun2 = year2;
                        resKelompok.Col.TargetTahun2 = kelompoktarget2;
                        resKelompok.Col.RealisasiTahun2 = kelompokrealisasi2;
                        resKelompok.Col.PersentaseTahun2 = kelompokpersentase2;

                        resKelompok.Col.Tahun3 = year3;
                        resKelompok.Col.TargetTahun3 = kelompoktarget3;
                        resKelompok.Col.RealisasiTahun3 = kelompokrealisasi3;
                        resKelompok.Col.PersentaseTahun3 = kelompokpersentase3;

                        resKelompok.Col.Tahun4 = year4;
                        resKelompok.Col.TargetTahun4 = kelompoktarget4;
                        resKelompok.Col.RealisasiTahun4 = kelompokrealisasi4;
                        resKelompok.Col.PersentaseTahun4 = kelompokpersentase4;

                        resKelompok.Col.Tahun5 = year5;
                        resKelompok.Col.TargetTahun5 = kelompoktarget5;
                        resKelompok.Col.RealisasiTahun5 = kelompokrealisasi5;
                        resKelompok.Col.PersentaseTahun5 = kelompokpersentase5;

                        resKelompok.Col.Tahun6 = year6;
                        resKelompok.Col.TargetTahun6 = kelompoktarget6;
                        resKelompok.Col.RealisasiTahun6 = kelompokrealisasi6;
                        resKelompok.Col.PersentaseTahun6 = kelompokpersentase6;

                        resKelompok.Col.Tahun7 = year7;
                        resKelompok.Col.TargetTahun7 = kelompoktarget7;
                        resKelompok.Col.RealisasiTahun7 = kelompokrealisasi7;
                        resKelompok.Col.PersentaseTahun7 = kelompokpersentase7;

                        foreach (var jenis in jenisList.Where(x => x.Kelompok == kelompok.Kelompok).ToList())
                        {
                            decimal jenistarget1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi1 = dataTahun1.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase1 = jenistarget1 > 0 ? Math.Round((jenisrealisasi1 / jenistarget1) * 100, 2) : 0;

                            decimal jenistarget2 = dataTahun2.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi2 = dataTahun2.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase2 = jenistarget2 > 0 ? Math.Round((jenisrealisasi2 / jenistarget2) * 100, 2) : 0;

                            decimal jenistarget3 = dataTahun3.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi3 = dataTahun3.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase3 = jenistarget3 > 0 ? Math.Round((jenisrealisasi3 / jenistarget3) * 100, 2) : 0;

                            decimal jenistarget4 = dataTahun4.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi4 = dataTahun4.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase4 = jenistarget4 > 0 ? Math.Round((jenisrealisasi4 / jenistarget4) * 100, 2) : 0;

                            decimal jenistarget5 = dataTahun5.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi5 = dataTahun5.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase5 = jenistarget5 > 0 ? Math.Round((jenisrealisasi5 / jenistarget5) * 100, 2) : 0;

                            decimal jenistarget6 = dataTahun6.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi6 = dataTahun6.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase6 = jenistarget6 > 0 ? Math.Round((jenisrealisasi6 / jenistarget6) * 100, 2) : 0;

                            decimal jenistarget7 = dataTahun7.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Target);
                            decimal jenisrealisasi7 = dataTahun7.SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok).SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))).Sum(q => q.Col.Realisasi);
                            decimal jenispersentase7 = jenistarget7 > 0 ? Math.Round((jenisrealisasi7 / jenistarget7) * 100, 2) : 0;


                            var resJenis = new ViewModel.ShowSeriesSudutPandangRekening.Jenis();

                            resJenis.Col.Kode = jenis.Jenis;
                            resJenis.Col.Nama = $"{jenis.Jenis} - {jenis.NamaJenis}";

                            resJenis.Col.Tahun1 = year1;
                            resJenis.Col.TargetTahun1 = jenistarget1;
                            resJenis.Col.RealisasiTahun1 = jenisrealisasi1;
                            resJenis.Col.PersentaseTahun1 = jenispersentase1;

                            resJenis.Col.Tahun2 = year2;
                            resJenis.Col.TargetTahun2 = jenistarget2;
                            resJenis.Col.RealisasiTahun2 = jenisrealisasi2;
                            resJenis.Col.PersentaseTahun2 = jenispersentase2;

                            resJenis.Col.Tahun3 = year3;
                            resJenis.Col.TargetTahun3 = jenistarget3;
                            resJenis.Col.RealisasiTahun3 = jenisrealisasi3;
                            resJenis.Col.PersentaseTahun3 = jenispersentase3;

                            resJenis.Col.Tahun4 = year4;
                            resJenis.Col.TargetTahun4 = jenistarget4;
                            resJenis.Col.RealisasiTahun4 = jenisrealisasi4;
                            resJenis.Col.PersentaseTahun4 = jenispersentase4;

                            resJenis.Col.Tahun5 = year5;
                            resJenis.Col.TargetTahun5 = jenistarget5;
                            resJenis.Col.RealisasiTahun5 = jenisrealisasi5;
                            resJenis.Col.PersentaseTahun5 = jenispersentase5;

                            resJenis.Col.Tahun6 = year6;
                            resJenis.Col.TargetTahun6 = jenistarget6;
                            resJenis.Col.RealisasiTahun6 = jenisrealisasi6;
                            resJenis.Col.PersentaseTahun6 = jenispersentase6;

                            resJenis.Col.Tahun7 = year7;
                            resJenis.Col.TargetTahun7 = jenistarget7;
                            resJenis.Col.RealisasiTahun7 = jenisrealisasi7;
                            resJenis.Col.PersentaseTahun7 = jenispersentase7;

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

                                decimal objektarget2 = dataTahun2
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Target);
                                decimal objekrealisasi2 = dataTahun2
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Realisasi);
                                decimal objekpersentase2 = objektarget2 > 0 ? Math.Round((objekrealisasi2 / objektarget2) * 100, 2) : 0;

                                decimal objektarget3 = dataTahun3
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Target);
                                decimal objekrealisasi3 = dataTahun3
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Realisasi);
                                decimal objekpersentase3 = objektarget3 > 0 ? Math.Round((objekrealisasi3 / objektarget3) * 100, 2) : 0;

                                decimal objektarget4 = dataTahun4
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Target);
                                decimal objekrealisasi4 = dataTahun4
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Realisasi);
                                decimal objekpersentase4 = objektarget4 > 0 ? Math.Round((objekrealisasi4 / objektarget4) * 100, 2) : 0;

                                decimal objektarget5 = dataTahun5
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Target);
                                decimal objekrealisasi5 = dataTahun5
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Realisasi);
                                decimal objekpersentase5 = objektarget5 > 0 ? Math.Round((objekrealisasi5 / objektarget5) * 100, 2) : 0;

                                decimal objektarget6 = dataTahun6
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Target);
                                decimal objekrealisasi6 = dataTahun6
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Realisasi);
                                decimal objekpersentase6 = objektarget6 > 0 ? Math.Round((objekrealisasi6 / objektarget6) * 100, 2) : 0;

                                decimal objektarget7 = dataTahun7
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Target);
                                decimal objekrealisasi7 = dataTahun7
                                    .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                    .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                    .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek)))
                                    .Sum(q => q.Col.Realisasi);
                                decimal objekpersentase7 = objektarget7 > 0 ? Math.Round((objekrealisasi7 / objektarget7) * 100, 2) : 0;


                                var resObjek = new ViewModel.ShowSeriesSudutPandangRekening.Objek();

                                resObjek.Col.Kode = objek.Objek;
                                resObjek.Col.Nama = $"{objek.Objek} - {objek.NamaObjek}";

                                resObjek.Col.Tahun1 = year1;
                                resObjek.Col.TargetTahun1 = objektarget1;
                                resObjek.Col.RealisasiTahun1 = objekrealisasi1;
                                resObjek.Col.PersentaseTahun1 = objekpersentase1;

                                resObjek.Col.Tahun2 = year2;
                                resObjek.Col.TargetTahun2 = objektarget2;
                                resObjek.Col.RealisasiTahun2 = objekrealisasi2;
                                resObjek.Col.PersentaseTahun2 = objekpersentase2;

                                resObjek.Col.Tahun3 = year3;
                                resObjek.Col.TargetTahun3 = objektarget3;
                                resObjek.Col.RealisasiTahun3 = objekrealisasi3;
                                resObjek.Col.PersentaseTahun3 = objekpersentase3;

                                resObjek.Col.Tahun4 = year4;
                                resObjek.Col.TargetTahun4 = objektarget4;
                                resObjek.Col.RealisasiTahun4 = objekrealisasi4;
                                resObjek.Col.PersentaseTahun4 = objekpersentase4;

                                resObjek.Col.Tahun5 = year5;
                                resObjek.Col.TargetTahun5 = objektarget5;
                                resObjek.Col.RealisasiTahun5 = objekrealisasi5;
                                resObjek.Col.PersentaseTahun5 = objekpersentase5;

                                resObjek.Col.Tahun6 = year6;
                                resObjek.Col.TargetTahun6 = objektarget6;
                                resObjek.Col.RealisasiTahun6 = objekrealisasi6;
                                resObjek.Col.PersentaseTahun6 = objekpersentase6;

                                resObjek.Col.Tahun7 = year7;
                                resObjek.Col.TargetTahun7 = objektarget7;
                                resObjek.Col.RealisasiTahun7 = objekrealisasi7;
                                resObjek.Col.PersentaseTahun7 = objekpersentase7;

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

                                    decimal rinciantarget2 = dataTahun2
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Target);
                                    decimal rincianrealisasi2 = dataTahun2
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Realisasi);
                                    decimal rincianpersentase2 = rinciantarget2 > 0 ? Math.Round((rincianrealisasi2 / rinciantarget2) * 100, 2) : 0;

                                    decimal rinciantarget3 = dataTahun3
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Target);
                                    decimal rincianrealisasi3 = dataTahun3
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Realisasi);
                                    decimal rincianpersentase3 = rinciantarget3 > 0 ? Math.Round((rincianrealisasi3 / rinciantarget3) * 100, 2) : 0;

                                    decimal rinciantarget4 = dataTahun4
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Target);
                                    decimal rincianrealisasi4 = dataTahun4
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Realisasi);
                                    decimal rincianpersentase4 = rinciantarget4 > 0 ? Math.Round((rincianrealisasi4 / rinciantarget4) * 100, 2) : 0;

                                    decimal rinciantarget5 = dataTahun5
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Target);
                                    decimal rincianrealisasi5 = dataTahun5
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Realisasi);
                                    decimal rincianpersentase5 = rinciantarget5 > 0 ? Math.Round((rincianrealisasi5 / rinciantarget5) * 100, 2) : 0;

                                    decimal rinciantarget6 = dataTahun6
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Target);
                                    decimal rincianrealisasi6 = dataTahun6
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Realisasi);
                                    decimal rincianpersentase6 = rinciantarget6 > 0 ? Math.Round((rincianrealisasi6 / rinciantarget6) * 100, 2) : 0;

                                    decimal rinciantarget7 = dataTahun7
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Target);
                                    decimal rincianrealisasi7 = dataTahun7
                                        .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                        .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                        .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                        .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                        )
                                        .Sum(q => q.Col.Realisasi);
                                    decimal rincianpersentase7 = rinciantarget7 > 0 ? Math.Round((rincianrealisasi7 / rinciantarget7) * 100, 2) : 0;


                                    var resRincian = new ViewModel.ShowSeriesSudutPandangRekening.Rincian();

                                    resRincian.Col.Kode = rincian.Rincian;
                                    resRincian.Col.Nama = $"{rincian.Rincian} - {rincian.NamaRincian}";

                                    resRincian.Col.Tahun1 = year1;
                                    resRincian.Col.TargetTahun1 = rinciantarget1;
                                    resRincian.Col.RealisasiTahun1 = rincianrealisasi1;
                                    resRincian.Col.PersentaseTahun1 = rincianpersentase1;

                                    resRincian.Col.Tahun2 = year2;
                                    resRincian.Col.TargetTahun2 = rinciantarget2;
                                    resRincian.Col.RealisasiTahun2 = rincianrealisasi2;
                                    resRincian.Col.PersentaseTahun2 = rincianpersentase2;

                                    resRincian.Col.Tahun3 = year3;
                                    resRincian.Col.TargetTahun3 = rinciantarget3;
                                    resRincian.Col.RealisasiTahun3 = rincianrealisasi3;
                                    resRincian.Col.PersentaseTahun3 = rincianpersentase3;

                                    resRincian.Col.Tahun4 = year4;
                                    resRincian.Col.TargetTahun4 = rinciantarget4;
                                    resRincian.Col.RealisasiTahun4 = rincianrealisasi4;
                                    resRincian.Col.PersentaseTahun4 = rincianpersentase4;

                                    resRincian.Col.Tahun5 = year5;
                                    resRincian.Col.TargetTahun5 = rinciantarget5;
                                    resRincian.Col.RealisasiTahun5 = rincianrealisasi5;
                                    resRincian.Col.PersentaseTahun5 = rincianpersentase5;

                                    resRincian.Col.Tahun6 = year6;
                                    resRincian.Col.TargetTahun6 = rinciantarget6;
                                    resRincian.Col.RealisasiTahun6 = rincianrealisasi6;
                                    resRincian.Col.PersentaseTahun6 = rincianpersentase6;

                                    resRincian.Col.Tahun7 = year7;
                                    resRincian.Col.TargetTahun7 = rinciantarget7;
                                    resRincian.Col.RealisasiTahun7 = rincianrealisasi7;
                                    resRincian.Col.PersentaseTahun7 = rincianpersentase7;

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

                                        decimal subrinciantarget2 = dataTahun2
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Target);
                                        decimal subrincianrealisasi2 = dataTahun2
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Realisasi);
                                        decimal subrincianpersentase2 = subrinciantarget2 > 0 ? Math.Round((subrincianrealisasi2 / subrinciantarget2) * 100, 2) : 0;

                                        decimal subrinciantarget3 = dataTahun3
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Target);
                                        decimal subrincianrealisasi3 = dataTahun3
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Realisasi);
                                        decimal subrincianpersentase3 = subrinciantarget3 > 0 ? Math.Round((subrincianrealisasi3 / subrinciantarget3) * 100, 2) : 0;

                                        decimal subrinciantarget4 = dataTahun4
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Target);
                                        decimal subrincianrealisasi4 = dataTahun4
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Realisasi);
                                        decimal subrincianpersentase4 = subrinciantarget4 > 0 ? Math.Round((subrincianrealisasi4 / subrinciantarget4) * 100, 2) : 0;

                                        decimal subrinciantarget5 = dataTahun5
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Target);
                                        decimal subrincianrealisasi5 = dataTahun5
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Realisasi);
                                        decimal subrincianpersentase5 = subrinciantarget5 > 0 ? Math.Round((subrincianrealisasi5 / subrinciantarget5) * 100, 2) : 0;

                                        decimal subrinciantarget6 = dataTahun6
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Target);
                                        decimal subrincianrealisasi6 = dataTahun6
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Realisasi);
                                        decimal subrincianpersentase6 = subrinciantarget6 > 0 ? Math.Round((subrincianrealisasi6 / subrinciantarget6) * 100, 2) : 0;

                                        decimal subrinciantarget7 = dataTahun7
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Target);
                                        decimal subrincianrealisasi7 = dataTahun7
                                            .SelectMany(x => x.RekKelompoks.Where(w => w.Col.Kode == kelompok.Kelompok && w.Col.Nama == kelompok.NamaKelompok)
                                            .SelectMany(x => x.RekJeniss.Where(x => x.Col.Kode == jenis.Jenis && x.Col.Nama == jenis.NamaJenis))
                                            .SelectMany(x => x.RekObjeks.Where(x => x.Col.Kode == objek.Objek && x.Col.Nama == objek.NamaObjek))
                                            .SelectMany(x => x.RekRincians.Where(x => x.Col.Kode == rincian.Rincian && x.Col.Nama == rincian.NamaRincian))
                                            .SelectMany(x => x.RekSubRincians.Where(x => x.Col.Kode == subRincian.SubRincian && x.Col.Nama == subRincian.NamaSubRincian))
                                            )
                                            .Sum(q => q.Col.Realisasi);
                                        decimal subrincianpersentase7 = subrinciantarget7 > 0 ? Math.Round((subrincianrealisasi7 / subrinciantarget7) * 100, 2) : 0;


                                        var resSubRincian = new ViewModel.ShowSeriesSudutPandangRekening.SubRincian();

                                        resSubRincian.Col.Kode = subRincian.SubRincian;
                                        resSubRincian.Col.Nama = $"{subRincian.SubRincian} - {subRincian.NamaSubRincian}";

                                        resSubRincian.Col.Tahun1 = year1;
                                        resSubRincian.Col.TargetTahun1 = subrinciantarget1;
                                        resSubRincian.Col.RealisasiTahun1 = subrincianrealisasi1;
                                        resSubRincian.Col.PersentaseTahun1 = subrincianpersentase1;

                                        resSubRincian.Col.Tahun2 = year2;
                                        resSubRincian.Col.TargetTahun2 = subrinciantarget2;
                                        resSubRincian.Col.RealisasiTahun2 = subrincianrealisasi2;
                                        resSubRincian.Col.PersentaseTahun2 = subrincianpersentase2;

                                        resSubRincian.Col.Tahun3 = year3;
                                        resSubRincian.Col.TargetTahun3 = subrinciantarget3;
                                        resSubRincian.Col.RealisasiTahun3 = subrincianrealisasi3;
                                        resSubRincian.Col.PersentaseTahun3 = subrincianpersentase3;

                                        resSubRincian.Col.Tahun4 = year4;
                                        resSubRincian.Col.TargetTahun4 = subrinciantarget4;
                                        resSubRincian.Col.RealisasiTahun4 = subrincianrealisasi4;
                                        resSubRincian.Col.PersentaseTahun4 = subrincianpersentase4;

                                        resSubRincian.Col.Tahun5 = year5;
                                        resSubRincian.Col.TargetTahun5 = subrinciantarget5;
                                        resSubRincian.Col.RealisasiTahun5 = subrincianrealisasi5;
                                        resSubRincian.Col.PersentaseTahun5 = subrincianpersentase5;

                                        resSubRincian.Col.Tahun6 = year6;
                                        resSubRincian.Col.TargetTahun6 = subrinciantarget6;
                                        resSubRincian.Col.RealisasiTahun6 = subrincianrealisasi6;
                                        resSubRincian.Col.PersentaseTahun6 = subrincianpersentase6;

                                        resSubRincian.Col.Tahun7 = year7;
                                        resSubRincian.Col.TargetTahun7 = subrinciantarget7;
                                        resSubRincian.Col.RealisasiTahun7 = subrincianrealisasi7;
                                        resSubRincian.Col.PersentaseTahun7 = subrincianpersentase7;

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
        }


        public class ViewModel
        {
            public class SudutPandangRekening
            {
                public class Akun
                {
                    public FormatColumn.ColumnAA Col { get; set; } = new();
                    public List<Kelompok> RekKelompoks { get; set; } = new List<Kelompok>();
                }
                public class Kelompok
                {
                    public FormatColumn.ColumnAA Col { get; set; } = new();
                    public List<Jenis> RekJeniss { get; set; } = new List<Jenis>();
                }
                public class Jenis
                {
                    public FormatColumn.ColumnAA Col { get; set; } = new();
                    public List<Objek> RekObjeks { get; set; } = new List<Objek>();
                }
                public class Objek
                {
                    public FormatColumn.ColumnAA Col { get; set; } = new();
                    public List<Rincian> RekRincians { get; set; } = new List<Rincian>();
                }
                public class Rincian
                {
                    public FormatColumn.ColumnAA Col { get; set; } = new();
                    public List<SubRincian> RekSubRincians { get; set; } = new List<SubRincian>();
                }
                public class SubRincian
                {
                    public FormatColumn.ColumnAA Col { get; set; } = new();
                }
            }
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
        }

        public class FormatColumn
        {
            public class ColumnAA
            {
                public string Kode { get; set; } = "";
                public string Nama { get; set; } = "";
                public decimal Target { get; set; } = 0;
                public decimal Realisasi { get; set; } = 0;
                public decimal Persentase { get; set; } = 0;
            }
            public class ColumnA
            {
                public string Kode { get; set; } = "";
                public string Nama { get; set; } = "";

                public int Tahun1 { get; set; } //2025
                public decimal TargetTahun1 { get; set; }
                public decimal RealisasiTahun1 { get; set; }
                public decimal PersentaseTahun1 { get; set; }

                public int Tahun2 { get; set; } //2024
                public decimal TargetTahun2 { get; set; }
                public decimal RealisasiTahun2 { get; set; }
                public decimal PersentaseTahun2 { get; set; }


                public int Tahun3 { get; set; } //2023
                public decimal TargetTahun3 { get; set; }
                public decimal RealisasiTahun3 { get; set; }
                public decimal PersentaseTahun3 { get; set; }

                public int Tahun4 { get; set; } //2022
                public decimal TargetTahun4 { get; set; }
                public decimal RealisasiTahun4 { get; set; }
                public decimal PersentaseTahun4 { get; set; }

                public int Tahun5 { get; set; } //2021
                public decimal TargetTahun5 { get; set; }
                public decimal RealisasiTahun5 { get; set; }
                public decimal PersentaseTahun5 { get; set; }

                public int Tahun6 { get; set; } //2020
                public decimal TargetTahun6 { get; set; }
                public decimal RealisasiTahun6 { get; set; }
                public decimal PersentaseTahun6 { get; set; }

                public int Tahun7 { get; set; } //2019
                public decimal TargetTahun7 { get; set; }
                public decimal RealisasiTahun7 { get; set; }
                public decimal PersentaseTahun7 { get; set; }
            }
        }


    }
}
