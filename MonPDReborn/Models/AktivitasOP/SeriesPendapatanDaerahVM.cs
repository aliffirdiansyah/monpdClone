using DevExpress.XtraReports.Wizards.Templates;
using Microsoft.AspNetCore.Components.Web;
using MonPDLib;
using MonPDLib.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using static MonPDReborn.Models.AktivitasOP.PemasanganAlatVM;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahVM
    {
        public class Index
        {

        }

        public class Show
        {
            public Show(int tahunAkhir)
            {
                
            }
        }
        public class Method
        {
            public static List<ViewModel.ShowSeriesSudutPandangRekening.Akun> GetSudutPandangRekening()
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
                    .ToList();

                var kelompokList = groupRekening
                    .GroupBy(x => new { x.Akun, x.Kelompok, x.NamaKelompok })
                    .Select(x => new { x.Key.Akun, x.Key.Kelompok, x.Key.NamaKelompok })
                    .ToList();

                var jenisList = groupRekening
                    .GroupBy(x => new { x.Kelompok, x.Jenis, x.NamaJenis })
                    .Select(x => new { x.Key.Kelompok, x.Key.Jenis, x.Key.NamaJenis })
                    .ToList();

                var objekList = groupRekening
                    .GroupBy(x => new { x.Jenis, x.Objek, x.NamaObjek })
                    .Select(x => new { x.Key.Jenis, x.Key.Objek, x.Key.NamaObjek })
                    .ToList();

                var rincianList = groupRekening
                    .GroupBy(x => new { x.Objek, x.Rincian, x.NamaRincian })
                    .Select(x => new { x.Key.Objek, x.Key.Rincian, x.Key.NamaRincian })
                    .ToList();

                var subrincianList = groupRekening
                    .GroupBy(x => new { x.Rincian, x.SubRincian, x.NamaSubRincian })
                    .Select(x => new { x.Key.Rincian, x.Key.SubRincian, x.Key.NamaSubRincian })
                    .ToList();

                var dataTahun1 = SeriesPendapatanDaerahVMMethod.Method.GetDataSudutPandangRekening(year1);
                var dataTahun2 = SeriesPendapatanDaerahVMMethod.Method.GetDataSudutPandangRekening(year2);
                var dataTahun3 = SeriesPendapatanDaerahVMMethod.Method.GetDataSudutPandangRekening(year3);
                var dataTahun4 = SeriesPendapatanDaerahVMMethod.Method.GetDataSudutPandangRekening(year4);
                var dataTahun5 = SeriesPendapatanDaerahVMMethod.Method.GetDataSudutPandangRekening(year5);
                var dataTahun6 = SeriesPendapatanDaerahVMMethod.Method.GetDataSudutPandangRekening(year6);
                var dataTahun7 = SeriesPendapatanDaerahVMMethod.Method.GetDataSudutPandangRekening(year7);

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
                    resAkun.Col.Nama = akun.NamaAkun;

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
                        resKelompok.Col.Nama = kelompok.NamaKelompok;

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
                            resJenis.Col.Nama = jenis.NamaJenis;

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

            }

            public class ShowSeriesSudutPandangRekeningKelompokObjekOpd
            {

            }

            public class CustomSeriesSudutPandang
            {
                public int TahunBuku { get; set; }
                public List<SeriesPendapatanDaerahVMMethod.ViewModel.SudutPandangRekening.Akun> Data { get; set; } = new();
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