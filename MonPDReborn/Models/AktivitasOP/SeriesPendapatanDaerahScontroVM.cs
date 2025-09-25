using Microsoft.EntityFrameworkCore;
using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahScontroVM
    {
        public class Index
        {
            public Index()
            {
                
            }
        }

        public class Show
        {
            public ViewModel.Scontro Data { get; set; } = new ViewModel.Scontro();
            public Show()
            {
                Data = Method.GetDataScontro();
            }
        }

        public class Method
        {
            public static ViewModel.Scontro GetDataScontro()
            {
                var result = new ViewModel.Scontro();
                var context = DBClass.GetContext();
                var query = context.DbPendapatanDaerahs.Where(x => x.TahunBuku >= 2019).ToList();

                //Tahun1
                #region Tahun1
                {
                    Console.WriteLine("[GET] Data Tahun 1");
                    var tahun = (DateTime.Now.Year) - 6;
                    result.FormatScontro1.TargetPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1", "4.2", "4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.01.12", "4.1.1.01.16" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.01.12", "4.1.1.01.16" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.02.01", "4.1.1.02.05" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.02.01", "4.1.1.02.05" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.03.01", "4.1.1.03.20", "4.1.1.03.21" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.03.01", "4.1.1.03.20", "4.1.1.03.21" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.04.01", "4.1.1.04.11", "4.1.1.04.12" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.04.01", "4.1.1.04.11", "4.1.1.04.12" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.05.01", "4.1.1.05.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.05.01", "4.1.1.05.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.07.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.07.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.08.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.08.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.12.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.12.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetBphtb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.13.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiBphtb = query.Where(predicate: x => x.TahunBuku == tahun && (new[] { "4.1.1.13.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.01", "4.2.1.01.03", "4.2.1.01.04", "4.2.1.02.02", "4.2.1.02.05", "4.2.1.02.07", "4.2.1.02.08", "4.2.1.02.09", "4.2.1.02.10", "4.2.2.01.01", "4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.01", "4.2.1.01.03", "4.2.1.01.04", "4.2.1.02.02", "4.2.1.02.05", "4.2.1.02.07", "4.2.1.02.08", "4.2.1.02.09", "4.2.1.02.10", "4.2.2.01.01", "4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.01", "4.2.1.01.03", "4.2.1.01.04", "4.2.1.02.02", "4.2.1.02.05", "4.2.1.02.07", "4.2.1.02.08", "4.2.1.02.09", "4.2.1.02.10" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.01", "4.2.1.01.03", "4.2.1.01.04", "4.2.1.02.02", "4.2.1.02.05", "4.2.1.02.07", "4.2.1.02.08", "4.2.1.02.09", "4.2.1.02.10" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetAlokasiKhususNonFisik = 0;
                    result.FormatScontro1.RealisasiAlokasiKhususNonFisik = 0;

                    result.FormatScontro1.TargetInsentifFiskal = 0;
                    result.FormatScontro1.RealisasiInsentifFiskal = 0;

                    result.FormatScontro1.TargetPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.3.01.01", "4.3.3.01.03", "4.3.3.01.05", "4.3.3.01.07", "4.3.3.01.08", "4.3.1.06.01", "4.3.7.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.3.01.01", "4.3.3.01.03", "4.3.3.01.05", "4.3.3.01.07", "4.3.3.01.08", "4.3.1.06.01", "4.3.7.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.5.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro1.RealisasiBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.5.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro1.TargetLainLainPendapatanSah = 0;
                    result.FormatScontro1.RealisasiLainLainPendapatanSah = 0;

                    result.FormatScontro1.TargetLainLainSesuaiPerundangan = 0;
                    result.FormatScontro1.RealisasiLainLainSesuaiPerundangan = 0;

                    result.FormatScontro1.TargetPenerimaanPembiayaan = 0;
                    result.FormatScontro1.RealisasiPenerimaanPembiayaan = 0;

                    result.FormatScontro1.TargetPenerimaanPembiayaanDaerah = 0;
                    result.FormatScontro1.RealisasiPenerimaanPembiayaanDaerah = 0;

                    result.FormatScontro1.TargetSilpaTahunSebelumnya = 0;
                    result.FormatScontro1.RealisasiSilpaTahunSebelumnya = 0;

                    result.FormatScontro1.TargetPenerimaanKembaliPemberianPinjaman = 0;
                    result.FormatScontro1.RealisasiPenerimaanKembaliPemberianPinjaman = 0;
                    Console.WriteLine("[DONE] Data Tahun 1");

                }
                #endregion

                #region Tahun2
                {
                    Console.WriteLine("[GET] Data Tahun 2");
                    var tahun = (DateTime.Now.Year) - 5;
                    result.FormatScontro2.TargetPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1", "4.2", "4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.01.12", "4.1.1.01.16" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.01.12", "4.1.1.01.16" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.02.01", "4.1.1.02.05" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.02.01", "4.1.1.02.05" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.03.01", "4.1.1.03.20", "4.1.1.03.21" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.03.01", "4.1.1.03.20", "4.1.1.03.21" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.04.01", "4.1.1.04.11" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.04.01", "4.1.1.04.11" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.05.01", "4.1.1.05.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.05.01", "4.1.1.05.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.07.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.07.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.08.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.08.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.12.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.12.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetBphtb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.1.13.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiBphtb = query.Where(predicate: x => x.TahunBuku == tahun && (new[] { "4.1.1.13.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.03", "4.2.1.01.04", "4.2.1.02.02", "4.2.1.02.07", "4.2.1.02.08", "4.2.1.02.09", "4.2.1.02.10", "4.2.1.02.12", "4.2.2.01.01", "4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.03","4.2.1.01.04","4.2.1.02.02","4.2.1.02.07","4.2.1.02.08","4.2.1.02.09","4.2.1.02.10","4.2.1.02.12","4.2.2.01.01","4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.03", "4.2.1.01.04", "4.2.1.02.02", "4.2.1.02.07", "4.2.1.02.08", "4.2.1.02.09", "4.2.1.02.10", "4.2.1.02.12" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1.01.03","4.2.1.01.04","4.2.1.02.02","4.2.1.02.07","4.2.1.02.08","4.2.1.02.09","4.2.1.02.10","4.2.1.02.12" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.3.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetAlokasiKhususNonFisik = 0;
                    result.FormatScontro2.RealisasiAlokasiKhususNonFisik = 0;

                    result.FormatScontro2.TargetInsentifFiskal = 0;
                    result.FormatScontro2.RealisasiInsentifFiskal = 0;

                    result.FormatScontro2.TargetPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.1.01.01", "4.3.1.03.01", "4.3.1.04.01", "4.3.1.06.01", "4.3.3.01.01", "4.3.3.01.03", "4.3.3.01.05", "4.3.3.01.07", "4.3.3.01.08", "4.3.7.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.1.01.01","4.3.1.03.01","4.3.1.04.01","4.3.1.06.01","4.3.3.01.01","4.3.3.01.03","4.3.3.01.05","4.3.3.01.07","4.3.3.01.08","4.3.7.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.5.01.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro2.RealisasiBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3.5.01.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro2.TargetLainLainPendapatanSah = 0;
                    result.FormatScontro2.RealisasiLainLainPendapatanSah = 0;

                    result.FormatScontro2.TargetLainLainSesuaiPerundangan = 0;
                    result.FormatScontro2.RealisasiLainLainSesuaiPerundangan = 0;

                    result.FormatScontro2.TargetPenerimaanPembiayaan = 0;
                    result.FormatScontro2.RealisasiPenerimaanPembiayaan = 0;

                    result.FormatScontro2.TargetPenerimaanPembiayaanDaerah = 0;
                    result.FormatScontro2.RealisasiPenerimaanPembiayaanDaerah = 0;

                    result.FormatScontro2.TargetSilpaTahunSebelumnya = 0;
                    result.FormatScontro2.RealisasiSilpaTahunSebelumnya = 0;

                    result.FormatScontro2.TargetPenerimaanKembaliPemberianPinjaman = 0;
                    result.FormatScontro2.RealisasiPenerimaanKembaliPemberianPinjaman = 0;
                    Console.WriteLine("[DONE] Data Tahun 2");
                }
                #endregion

                #region Tahun3
                {
                    Console.WriteLine("[GET] Data Tahun 3");
                    var tahun = (DateTime.Now.Year) - 4;
                    result.FormatScontro3.TargetPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01", "4.1.01.06.08" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01","4.1.01.06.08" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01", "4.1.01.07.07" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01","4.1.01.07.07" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01", "4.1.01.08.02", "4.1.01.08.05" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01","4.1.01.08.02","4.1.01.08.05" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01", "4.1.01.09.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01","4.1.01.09.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01", "4.1.01.10.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01","4.1.01.10.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetBphtb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01", "4.1.01.16.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiBphtb = query.Where(predicate: x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01","4.1.01.16.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0013", "4.2.01.01.02.0001", "4.2.01.01.03.0002", "4.2.01.01.03.0003", "4.2.01.01.03.0006", "4.2.01.01.03.0013", "4.2.01.01.03.0015", "4.2.01.01.03.0016", "4.2.01.01.03.0025", "4.2.01.01.03.0034", "4.2.01.01.03.0042", "4.2.01.01.03.0055", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0013", "4.2.01.01.04.0014", "4.2.01.01.04.0016", "4.2.01.01.04.0017", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001","4.2.01.01.01.0002","4.2.01.01.01.0003","4.2.01.01.01.0004","4.2.01.01.01.0005","4.2.01.01.01.0006","4.2.01.01.01.0007","4.2.01.01.01.0009","4.2.01.01.01.0010","4.2.01.01.01.0013","4.2.01.01.02.0001","4.2.01.01.03.0002","4.2.01.01.03.0003","4.2.01.01.03.0006","4.2.01.01.03.0013","4.2.01.01.03.0015","4.2.01.01.03.0016","4.2.01.01.03.0025","4.2.01.01.03.0034","4.2.01.01.03.0042","4.2.01.01.03.0055","4.2.01.01.04.0004","4.2.01.01.04.0005","4.2.01.01.04.0007","4.2.01.01.04.0008","4.2.01.01.04.0009","4.2.01.01.04.0011","4.2.01.01.04.0013","4.2.01.01.04.0014","4.2.01.01.04.0016","4.2.01.01.04.0017","4.2.01.01.04.0019","4.2.01.01.04.0020","4.2.01.01.04.0021","4.2.01.01.04.0022","4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001","4.2.01.01.01.0002","4.2.01.01.01.0003","4.2.01.01.01.0004","4.2.01.01.01.0005","4.2.01.01.01.0006","4.2.01.01.01.0007","4.2.01.01.01.0009","4.2.01.01.01.0010","4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0002", "4.2.01.01.03.0003", "4.2.01.01.03.0006", "4.2.01.01.03.0013", "4.2.01.01.03.0015", "4.2.01.01.03.0016", "4.2.01.01.03.0025", "4.2.01.01.03.0034", "4.2.01.01.03.0042", "4.2.01.01.03.0055" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0002","4.2.01.01.03.0003","4.2.01.01.03.0006","4.2.01.01.03.0013","4.2.01.01.03.0015","4.2.01.01.03.0016","4.2.01.01.03.0025","4.2.01.01.03.0034","4.2.01.01.03.0042","4.2.01.01.03.0055" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0013", "4.2.01.01.04.0014", "4.2.01.01.04.0016", "4.2.01.01.04.0017", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0004","4.2.01.01.04.0005","4.2.01.01.04.0007","4.2.01.01.04.0008","4.2.01.01.04.0009","4.2.01.01.04.0011","4.2.01.01.04.0013","4.2.01.01.04.0014","4.2.01.01.04.0016","4.2.01.01.04.0017","4.2.01.01.04.0019","4.2.01.01.04.0020","4.2.01.01.04.0021","4.2.01.01.04.0022","4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001", "4.2.02.01.01.0002", "4.2.02.01.01.0003", "4.2.02.01.01.0004", "4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001","4.2.02.01.01.0002","4.2.02.01.01.0003","4.2.02.01.01.0004","4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro3.RealisasiLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro3.TargetLainLainSesuaiPerundangan = 0;
                    result.FormatScontro3.RealisasiLainLainSesuaiPerundangan = 0;

                    result.FormatScontro3.TargetPenerimaanPembiayaan = 0;
                    result.FormatScontro3.RealisasiPenerimaanPembiayaan = 0;

                    result.FormatScontro3.TargetPenerimaanPembiayaanDaerah = 0;
                    result.FormatScontro3.RealisasiPenerimaanPembiayaanDaerah = 0;

                    result.FormatScontro3.TargetSilpaTahunSebelumnya = 0;
                    result.FormatScontro3.RealisasiSilpaTahunSebelumnya = 0;

                    result.FormatScontro3.TargetPenerimaanKembaliPemberianPinjaman = 0;
                    result.FormatScontro3.RealisasiPenerimaanKembaliPemberianPinjaman = 0;
                    Console.WriteLine("[DONE] Data Tahun 3");
                }
                #endregion

                #region Tahun4
                {
                    Console.WriteLine("[GET] Data Tahun 4");
                    var tahun = (DateTime.Now.Year) - 3;
                    result.FormatScontro4.TargetPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01", "4.1.01.06.08" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01", "4.1.01.06.08" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01", "4.1.01.07.07" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01", "4.1.01.07.07" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01", "4.1.01.08.02", "4.1.01.08.05" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01", "4.1.01.08.02", "4.1.01.08.05" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01", "4.1.01.09.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01", "4.1.01.09.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01", "4.1.01.10.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01", "4.1.01.10.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetBphtb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01", "4.1.01.16.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiBphtb = query.Where(predicate: x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01", "4.1.01.16.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0013", "4.2.01.01.02.0001", "4.2.01.01.03.0002", "4.2.01.01.03.0015", "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0025", "4.2.01.01.03.0026", "4.2.01.01.03.0034", "4.2.01.01.03.0037", "4.2.01.01.03.0040", "4.2.01.01.04.0001", "4.2.01.01.04.0003", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0016", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001","4.2.01.01.01.0002","4.2.01.01.01.0003","4.2.01.01.01.0004","4.2.01.01.01.0005","4.2.01.01.01.0006","4.2.01.01.01.0007","4.2.01.01.01.0009","4.2.01.01.01.0010","4.2.01.01.01.0013","4.2.01.01.02.0001","4.2.01.01.03.0002","4.2.01.01.03.0015","4.2.01.01.03.0016","4.2.01.01.03.0018","4.2.01.01.03.0025","4.2.01.01.03.0026","4.2.01.01.03.0034","4.2.01.01.03.0037","4.2.01.01.03.0040","4.2.01.01.04.0001","4.2.01.01.04.0003","4.2.01.01.04.0004","4.2.01.01.04.0005","4.2.01.01.04.0007","4.2.01.01.04.0008","4.2.01.01.04.0009","4.2.01.01.04.0011","4.2.01.01.04.0016","4.2.01.01.04.0019","4.2.01.01.04.0020","4.2.01.01.04.0021","4.2.01.01.04.0022","4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001","4.2.01.01.01.0002","4.2.01.01.01.0003","4.2.01.01.01.0004","4.2.01.01.01.0005","4.2.01.01.01.0006","4.2.01.01.01.0007","4.2.01.01.01.0009","4.2.01.01.01.0010","4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0002", "4.2.01.01.03.0015", "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0025", "4.2.01.01.03.0026", "4.2.01.01.03.0034", "4.2.01.01.03.0037", "4.2.01.01.03.0040" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0002","4.2.01.01.03.0015","4.2.01.01.03.0016","4.2.01.01.03.0018","4.2.01.01.03.0025","4.2.01.01.03.0026","4.2.01.01.03.0034","4.2.01.01.03.0037","4.2.01.01.03.0040" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0001", "4.2.01.01.04.0003", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0016", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0001","4.2.01.01.04.0003","4.2.01.01.04.0004","4.2.01.01.04.0005","4.2.01.01.04.0007","4.2.01.01.04.0008","4.2.01.01.04.0009","4.2.01.01.04.0011","4.2.01.01.04.0016","4.2.01.01.04.0019","4.2.01.01.04.0020","4.2.01.01.04.0021","4.2.01.01.04.0022","4.2.01.01.04.0023" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001", "4.2.02.01.01.0002", "4.2.02.01.01.0003", "4.2.02.01.01.0004", "4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001", "4.2.02.01.01.0002", "4.2.02.01.01.0003", "4.2.02.01.01.0004", "4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro4.RealisasiLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro4.TargetLainLainSesuaiPerundangan = 0;
                    result.FormatScontro4.RealisasiLainLainSesuaiPerundangan = 0;

                    result.FormatScontro4.TargetPenerimaanPembiayaan = 0;
                    result.FormatScontro4.RealisasiPenerimaanPembiayaan = 0;

                    result.FormatScontro4.TargetPenerimaanPembiayaanDaerah = 0;
                    result.FormatScontro4.RealisasiPenerimaanPembiayaanDaerah = 0;

                    result.FormatScontro4.TargetSilpaTahunSebelumnya = 0;
                    result.FormatScontro4.RealisasiSilpaTahunSebelumnya = 0;

                    result.FormatScontro4.TargetPenerimaanKembaliPemberianPinjaman = 0;
                    result.FormatScontro4.RealisasiPenerimaanKembaliPemberianPinjaman = 0;
                    Console.WriteLine("[DONE] Data Tahun 4");
                }
                #endregion

                #region Tahun5
                {
                    Console.WriteLine("[GET] Data Tahun 5");
                    var tahun = (DateTime.Now.Year) - 2;
                    result.FormatScontro5.TargetPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01", "4.1.01.06.08" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01", "4.1.01.06.08" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01", "4.1.01.07.07" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01", "4.1.01.07.07" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01", "4.1.01.08.02", "4.1.01.08.05" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01", "4.1.01.08.02", "4.1.01.08.05" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01", "4.1.01.09.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01", "4.1.01.09.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01", "4.1.01.10.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01", "4.1.01.10.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetBphtb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01", "4.1.01.16.02" }).Contains(x.Objek)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiBphtb = query.Where(predicate: x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01", "4.1.01.16.02" }).Contains(x.Objek)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0008", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0012", "4.2.01.01.01.0013", "4.2.01.01.02.0001", "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0042", "4.2.01.01.03.0055", "4.2.01.01.04.0001", "4.2.01.01.04.0003", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0012", "4.2.01.01.04.0014", "4.2.01.01.04.0016", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023", "4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0008", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0012", "4.2.01.01.01.0013", "4.2.01.01.02.0001", "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0042", "4.2.01.01.03.0055", "4.2.01.01.04.0001", "4.2.01.01.04.0003", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0012", "4.2.01.01.04.0014", "4.2.01.01.04.0016", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023", "4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0008", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0012", "4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001","4.2.01.01.01.0002","4.2.01.01.01.0003","4.2.01.01.01.0004","4.2.01.01.01.0005","4.2.01.01.01.0006","4.2.01.01.01.0007","4.2.01.01.01.0008","4.2.01.01.01.0009","4.2.01.01.01.0010","4.2.01.01.01.0012","4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0042", "4.2.01.01.03.0055" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0016","4.2.01.01.03.0018","4.2.01.01.03.0042","4.2.01.01.03.0055" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0001", "4.2.01.01.04.0003", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0012", "4.2.01.01.04.0014", "4.2.01.01.04.0016", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023", "4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0001","4.2.01.01.04.0003","4.2.01.01.04.0004","4.2.01.01.04.0005","4.2.01.01.04.0007","4.2.01.01.04.0008","4.2.01.01.04.0009","4.2.01.01.04.0011","4.2.01.01.04.0012","4.2.01.01.04.0014","4.2.01.01.04.0016","4.2.01.01.04.0019","4.2.01.01.04.0020","4.2.01.01.04.0021","4.2.01.01.04.0022","4.2.01.01.04.0023","4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001","4.2.02.01.01.0002","4.2.02.01.01.0003","4.2.02.01.01.0004","4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001","4.2.02.01.01.0002","4.2.02.01.01.0003","4.2.02.01.01.0004","4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro5.RealisasiLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro5.TargetLainLainSesuaiPerundangan = 0;
                    result.FormatScontro5.RealisasiLainLainSesuaiPerundangan = 0;

                    result.FormatScontro5.TargetPenerimaanPembiayaan = 0;
                    result.FormatScontro5.RealisasiPenerimaanPembiayaan = 0;

                    result.FormatScontro5.TargetPenerimaanPembiayaanDaerah = 0;
                    result.FormatScontro5.RealisasiPenerimaanPembiayaanDaerah = 0;

                    result.FormatScontro5.TargetSilpaTahunSebelumnya = 0;
                    result.FormatScontro5.RealisasiSilpaTahunSebelumnya = 0;

                    result.FormatScontro5.TargetPenerimaanKembaliPemberianPinjaman = 0;
                    result.FormatScontro5.RealisasiPenerimaanKembaliPemberianPinjaman = 0;
                    Console.WriteLine("[DONE] Data Tahun 5");
                }
                #endregion

                #region Tahun6
                {
                    Console.WriteLine("[GET] Data Tahun 6");
                    var tahun = (DateTime.Now.Year) - 1;
                    result.FormatScontro6.TargetPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01.0001", "4.1.01.19.03.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.06.01.0001","4.1.01.19.03.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01.0001", "4.1.01.07.07.0001", "4.1.01.19.01.0001","4.1.01.19.01.0002" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.07.01.0001","4.1.01.07.07.0001","4.1.01.19.01.0001","4.1.01.19.01.0002" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01.0001", "4.1.01.08.02.0001", "4.1.01.08.05.0001", "4.1.01.19.05.0001", "4.1.01.19.05.0002", "4.1.01.19.05.0012" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.08.01.0001","4.1.01.08.02.0001","4.1.01.08.05.0001","4.1.01.19.05.0001","4.1.01.19.05.0002","4.1.01.19.05.0012" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01.0001","4.1.01.09.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01.0001","4.1.01.09.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01.0001", "4.1.01.10.02.0001","4.1.01.19.02.0001","4.1.01.19.02.0002" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.10.01.0001","4.1.01.10.02.0001","4.1.01.19.02.0001","4.1.01.19.02.0002" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01.0001","4.1.01.19.04.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.11.01.0001","4.1.01.19.04.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetBphtb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01.0001", "4.1.01.16.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiBphtb = query.Where(predicate: x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01.0001","4.1.01.16.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0008", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0012", "4.2.01.01.01.0013", "4.2.01.01.02.0001", "4.2.01.01.02.0002", "4.2.01.01.02.0004", "4.2.01.01.02.0005", "4.2.01.01.02.0006", "4.2.01.01.03.0001", "4.2.01.01.03.0002", "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0042", "4.2.01.01.03.0055", "4.2.01.01.03.0060", "4.2.01.01.04.0001", "4.2.01.01.04.0003", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0012", "4.2.01.01.04.0014", "4.2.01.01.04.0016", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023", "4.2.01.01.04.0026", "4.2.01.01.04.0027", "4.2.01.01.04.0028", "4.2.01.01.04.0029", "4.2.01.01.04.0030", "4.2.01.01.04.0031", "4.2.01.01.04.0033", "4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001","4.2.01.01.01.0002","4.2.01.01.01.0003","4.2.01.01.01.0004","4.2.01.01.01.0005","4.2.01.01.01.0006","4.2.01.01.01.0007","4.2.01.01.01.0008","4.2.01.01.01.0009","4.2.01.01.01.0010","4.2.01.01.01.0012","4.2.01.01.01.0013","4.2.01.01.02.0001","4.2.01.01.02.0002","4.2.01.01.02.0004","4.2.01.01.02.0005","4.2.01.01.02.0006","4.2.01.01.03.0001","4.2.01.01.03.0002","4.2.01.01.03.0016","4.2.01.01.03.0018","4.2.01.01.03.0042","4.2.01.01.03.0055","4.2.01.01.03.0060","4.2.01.01.04.0001","4.2.01.01.04.0003","4.2.01.01.04.0004","4.2.01.01.04.0005","4.2.01.01.04.0007","4.2.01.01.04.0008","4.2.01.01.04.0009","4.2.01.01.04.0011","4.2.01.01.04.0012","4.2.01.01.04.0014","4.2.01.01.04.0016","4.2.01.01.04.0019","4.2.01.01.04.0020","4.2.01.01.04.0021","4.2.01.01.04.0022","4.2.01.01.04.0023","4.2.01.01.04.0026","4.2.01.01.04.0027","4.2.01.01.04.0028","4.2.01.01.04.0029","4.2.01.01.04.0030","4.2.01.01.04.0031","4.2.01.01.04.0033","4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001", "4.2.01.01.01.0002", "4.2.01.01.01.0003", "4.2.01.01.01.0004", "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0007", "4.2.01.01.01.0008", "4.2.01.01.01.0009", "4.2.01.01.01.0010", "4.2.01.01.01.0012", "4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0001","4.2.01.01.01.0002","4.2.01.01.01.0003","4.2.01.01.01.0004","4.2.01.01.01.0005","4.2.01.01.01.0006","4.2.01.01.01.0007","4.2.01.01.01.0008","4.2.01.01.01.0009","4.2.01.01.01.0010","4.2.01.01.01.0012","4.2.01.01.01.0013" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001","4.2.01.01.02.0002","4.2.01.01.02.0004","4.2.01.01.02.0005","4.2.01.01.02.0006" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001","4.2.01.01.02.0002","4.2.01.01.02.0004","4.2.01.01.02.0005","4.2.01.01.02.0006" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0001", "4.2.01.01.03.0002", "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0042", "4.2.01.01.03.0055", "4.2.01.01.03.0060" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0001","4.2.01.01.03.0002","4.2.01.01.03.0016","4.2.01.01.03.0018","4.2.01.01.03.0042","4.2.01.01.03.0055","4.2.01.01.03.0060" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0001", "4.2.01.01.04.0003", "4.2.01.01.04.0004", "4.2.01.01.04.0005", "4.2.01.01.04.0007", "4.2.01.01.04.0008", "4.2.01.01.04.0009", "4.2.01.01.04.0011", "4.2.01.01.04.0012", "4.2.01.01.04.0014", "4.2.01.01.04.0016", "4.2.01.01.04.0019", "4.2.01.01.04.0020", "4.2.01.01.04.0021", "4.2.01.01.04.0022", "4.2.01.01.04.0023", "4.2.01.01.04.0026", "4.2.01.01.04.0027", "4.2.01.01.04.0028", "4.2.01.01.04.0029", "4.2.01.01.04.0030", "4.2.01.01.04.0031", "4.2.01.01.04.0033", "4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.04.0001","4.2.01.01.04.0003","4.2.01.01.04.0004","4.2.01.01.04.0005","4.2.01.01.04.0007","4.2.01.01.04.0008","4.2.01.01.04.0009","4.2.01.01.04.0011","4.2.01.01.04.0012","4.2.01.01.04.0014","4.2.01.01.04.0016","4.2.01.01.04.0019","4.2.01.01.04.0020","4.2.01.01.04.0021","4.2.01.01.04.0022","4.2.01.01.04.0023","4.2.01.01.04.0026","4.2.01.01.04.0027","4.2.01.01.04.0028","4.2.01.01.04.0029","4.2.01.01.04.0030","4.2.01.01.04.0031","4.2.01.01.04.0033","4.2.01.01.04.0035" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001","4.2.01.06.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001","4.2.01.06.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001","4.2.02.01.01.0002","4.2.02.01.01.0003","4.2.02.01.01.0004","4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001","4.2.02.01.01.0002","4.2.02.01.01.0003","4.2.02.01.01.0004","4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro6.RealisasiLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro6.TargetLainLainSesuaiPerundangan = 0;
                    result.FormatScontro6.RealisasiLainLainSesuaiPerundangan = 0;

                    result.FormatScontro6.TargetPenerimaanPembiayaan = 0;
                    result.FormatScontro6.RealisasiPenerimaanPembiayaan = 0;

                    result.FormatScontro6.TargetPenerimaanPembiayaanDaerah = 0;
                    result.FormatScontro6.RealisasiPenerimaanPembiayaanDaerah = 0;

                    result.FormatScontro6.TargetSilpaTahunSebelumnya = 0;
                    result.FormatScontro6.RealisasiSilpaTahunSebelumnya = 0;

                    result.FormatScontro6.TargetPenerimaanKembaliPemberianPinjaman = 0;
                    result.FormatScontro6.RealisasiPenerimaanKembaliPemberianPinjaman = 0;
                    Console.WriteLine("[DONE] Data Tahun 6");
                }
                #endregion

                #region Tahun7
                {
                    Console.WriteLine("[GET] Data Tahun 7");
                    var tahun = (DateTime.Now.Year);
                    result.FormatScontro7.TargetPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPendapatan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1","4.2","4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPendapatanAsliDaerah = query.Where(x => x.TahunBuku == tahun && x.Kelompok == "4.1").Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPajakDaerah = query.Where(x => x.TahunBuku == tahun && x.Jenis == "4.1.1").Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.03.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPajakHotel = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.03.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.01.0001", "4.1.01.19.01.0002" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPajakRestoran = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.01.0001", "4.1.01.19.01.0002" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.05.0001", "4.1.01.19.05.0002", "4.1.01.19.05.0012" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPajakHiburan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.05.0001", "4.1.01.19.05.0002", "4.1.01.19.05.0012" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01.0001","4.1.01.09.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPajakReklame = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.09.01.0001","4.1.01.09.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.02.0001", "4.1.01.19.02.0002" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPajakPeneranganJalan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.02.0001", "4.1.01.19.02.0002" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.04.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPajakParkir = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.19.04.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiAbt = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.12.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPbb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.15.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetBphtb = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01.0001", "4.1.01.16.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiBphtb = query.Where(predicate: x => x.TahunBuku == tahun && (new[] { "4.1.01.16.01.0001","4.1.01.16.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiRetribusiDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiHpkd = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.3" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiLainlainPad = query.Where(x => x.TahunBuku == tahun && (new[] { "4.1.4" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPendapatanTransfer = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiTransferPemerintahPusat = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.1" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0009", "4.2.01.01.01.0013", "4.2.01.01.02.0001", "4.2.01.07.01.0001", "4.2.01.07.01.0002", "4.2.01.07.01.0003", "4.2.01.07.01.0004", "4.2.01.07.02.0001", "4.2.01.07.02.0002", "4.2.01.07.02.0003", "4.2.01.07.02.0005", "4.2.01.07.02.0006", "4.2.01.07.02.0009", "4.2.01.08.01.0001", "4.2.01.08.02.0001", "4.2.01.08.02.0003", "4.2.01.08.02.0004", "4.2.01.08.02.0005", "4.2.01.09.01.0060", "4.2.01.09.02.0004", "4.2.01.09.02.0005", "4.2.01.09.02.0009", "4.2.01.09.02.0016", "4.2.01.09.02.0021", "4.2.01.09.02.0022", "4.2.01.09.02.0023", "4.2.01.09.02.0026", "4.2.01.09.02.0027", "4.2.01.09.02.0028", "4.2.01.09.02.0029", "4.2.01.09.02.0030", "4.2.01.09.02.0031", "4.2.01.09.02.0033", "4.2.01.09.02.0035" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiDanaPerimbangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0009", "4.2.01.01.01.0013", "4.2.01.01.02.0001", "4.2.01.07.01.0001", "4.2.01.07.01.0002", "4.2.01.07.01.0003", "4.2.01.07.01.0004", "4.2.01.07.02.0001", "4.2.01.07.02.0002", "4.2.01.07.02.0003", "4.2.01.07.02.0005", "4.2.01.07.02.0006", "4.2.01.07.02.0009", "4.2.01.08.01.0001", "4.2.01.08.02.0001", "4.2.01.08.02.0003", "4.2.01.08.02.0004", "4.2.01.08.02.0005", "4.2.01.09.01.0060", "4.2.01.09.02.0004", "4.2.01.09.02.0005", "4.2.01.09.02.0009", "4.2.01.09.02.0016", "4.2.01.09.02.0021", "4.2.01.09.02.0022", "4.2.01.09.02.0023", "4.2.01.09.02.0026", "4.2.01.09.02.0027", "4.2.01.09.02.0028", "4.2.01.09.02.0029", "4.2.01.09.02.0030", "4.2.01.09.02.0031", "4.2.01.09.02.0033", "4.2.01.09.02.0035" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0009", "4.2.01.01.01.0013", "4.2.01.07.01.0001", "4.2.01.07.01.0002", "4.2.01.07.01.0003", "4.2.01.07.01.0004", "4.2.01.07.02.0001", "4.2.01.07.02.0002", "4.2.01.07.02.0003", "4.2.01.07.02.0005", "4.2.01.07.02.0006", "4.2.01.07.02.0009" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiDanaBagiHasil = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.01.0005", "4.2.01.01.01.0006", "4.2.01.01.01.0009", "4.2.01.01.01.0013", "4.2.01.07.01.0001", "4.2.01.07.01.0002", "4.2.01.07.01.0003", "4.2.01.07.01.0004", "4.2.01.07.02.0001", "4.2.01.07.02.0002", "4.2.01.07.02.0003", "4.2.01.07.02.0005", "4.2.01.07.02.0006", "4.2.01.07.02.0009" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001","4.2.01.08.01.0001","4.2.01.08.02.0001","4.2.01.08.02.0003","4.2.01.08.02.0004","4.2.01.08.02.0005" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiDanaAlokasiUmum = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.02.0001","4.2.01.08.01.0001","4.2.01.08.02.0001","4.2.01.08.02.0003","4.2.01.08.02.0004","4.2.01.08.02.0005" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0042", "4.2.01.01.03.0055", "4.2.01.09.01.0060" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiAlokasiKhususFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.01.03.0016", "4.2.01.01.03.0018", "4.2.01.01.03.0042", "4.2.01.01.03.0055", "4.2.01.09.01.0060" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.09.02.0004", "4.2.01.09.02.0005", "4.2.01.09.02.0009", "4.2.01.09.02.0016", "4.2.01.09.02.0021", "4.2.01.09.02.0022", "4.2.01.09.02.0023", "4.2.01.09.02.0026", "4.2.01.09.02.0027", "4.2.01.09.02.0028", "4.2.01.09.02.0029", "4.2.01.09.02.0030", "4.2.01.09.02.0031", "4.2.01.09.02.0033", "4.2.01.09.02.0035" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiAlokasiKhususNonFisik = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.09.02.0004", "4.2.01.09.02.0005", "4.2.01.09.02.0009", "4.2.01.09.02.0016", "4.2.01.09.02.0021", "4.2.01.09.02.0022", "4.2.01.09.02.0023", "4.2.01.09.02.0026", "4.2.01.09.02.0027", "4.2.01.09.02.0028", "4.2.01.09.02.0029", "4.2.01.09.02.0030", "4.2.01.09.02.0031", "4.2.01.09.02.0033", "4.2.01.09.02.0035" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001","4.2.01.06.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiInsentifFiskal = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.01.02.01.0001","4.2.01.06.01.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiPendapatanTransferAntarDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.2" }).Contains(x.Jenis)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001","4.2.02.01.01.0002","4.2.02.01.01.0003","4.2.02.01.01.0004","4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiBagiHasilDaerah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.01.01.0001","4.2.02.01.01.0002","4.2.02.01.01.0003","4.2.02.01.01.0004","4.2.02.01.01.0005" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiBantuanKeuangan = query.Where(x => x.TahunBuku == tahun && (new[] { "4.2.02.02.02.0001" }).Contains(x.SubRincian)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Target);
                    result.FormatScontro7.RealisasiLainLainPendapatanSah = query.Where(x => x.TahunBuku == tahun && (new[] { "4.3" }).Contains(x.Kelompok)).Sum(x => x.Realisasi);

                    result.FormatScontro7.TargetLainLainSesuaiPerundangan = 0;
                    result.FormatScontro7.RealisasiLainLainSesuaiPerundangan = 0;

                    result.FormatScontro7.TargetPenerimaanPembiayaan = 0;
                    result.FormatScontro7.RealisasiPenerimaanPembiayaan = 0;

                    result.FormatScontro7.TargetPenerimaanPembiayaanDaerah = 0;
                    result.FormatScontro7.RealisasiPenerimaanPembiayaanDaerah = 0;

                    result.FormatScontro7.TargetSilpaTahunSebelumnya = 0;
                    result.FormatScontro7.RealisasiSilpaTahunSebelumnya = 0;

                    result.FormatScontro7.TargetPenerimaanKembaliPemberianPinjaman = 0;
                    result.FormatScontro7.RealisasiPenerimaanKembaliPemberianPinjaman = 0;
                    Console.WriteLine("[DONE] Data Tahun 7");
                }
                #endregion
                return result;
            }
        }

        public class ViewModel
        {
            public class Scontro
            {
                public FormatColScontro FormatScontro1 { get; set; } = new FormatColScontro();
                public FormatColScontro FormatScontro2 { get; set; } = new FormatColScontro();
                public FormatColScontro FormatScontro3 { get; set; } = new FormatColScontro();
                public FormatColScontro FormatScontro4 { get; set; } = new FormatColScontro();
                public FormatColScontro FormatScontro5 { get; set; } = new FormatColScontro();
                public FormatColScontro FormatScontro6 { get; set; } = new FormatColScontro();
                public FormatColScontro FormatScontro7 { get; set; } = new FormatColScontro();
            }
            public class FormatColScontro
            {
                public decimal TargetPendapatan { get; set; }
                public decimal RealisasiPendapatan { get; set; }
                public decimal PersentasePendapatan => Math.Round(TargetPendapatan > 0 ? (RealisasiPendapatan / TargetPendapatan) * 100 : 0, 2);
                public decimal TargetPendapatanAsliDaerah { get; set; }
                public decimal RealisasiPendapatanAsliDaerah { get; set; }
                public decimal PersentasePendapatanAsliDaerah => Math.Round(TargetPendapatanAsliDaerah > 0 ? (RealisasiPendapatanAsliDaerah / TargetPendapatanAsliDaerah) * 100 : 0, 2);
                public decimal TargetPajakDaerah { get; set; }
                public decimal RealisasiPajakDaerah { get; set; }
                public decimal PersentasePajakDaerah => Math.Round(TargetPajakDaerah > 0 ? (RealisasiPajakDaerah / TargetPajakDaerah) * 100 : 0, 2);
                public decimal TargetPajakHotel { get; set; }
                public decimal RealisasiPajakHotel { get; set; }
                public decimal PersentasePajakHotel => Math.Round(TargetPajakHotel > 0 ? (RealisasiPajakHotel / TargetPajakHotel) * 100 : 0, 2);
                public decimal TargetPajakRestoran { get; set; }
                public decimal RealisasiPajakRestoran { get; set; }
                public decimal PersentasePajakRestoran => Math.Round(TargetPajakRestoran > 0 ? (RealisasiPajakRestoran / TargetPajakRestoran) * 100 : 0, 2);
                public decimal TargetPajakHiburan { get; set; }
                public decimal RealisasiPajakHiburan { get; set; }
                public decimal PersentasePajakHiburan => Math.Round(TargetPajakHiburan > 0 ? (RealisasiPajakHiburan / TargetPajakHiburan) * 100 : 0, 2);
                public decimal TargetPajakReklame { get; set; }
                public decimal RealisasiPajakReklame { get; set; }
                public decimal PersentasePajakReklame => Math.Round(TargetPajakReklame > 0 ? (RealisasiPajakReklame / TargetPajakReklame) * 100 : 0, 2);
                public decimal TargetPajakPeneranganJalan { get; set; }
                public decimal RealisasiPajakPeneranganJalan { get; set; }
                public decimal PersentasePajakPeneranganJalan => Math.Round(TargetPajakPeneranganJalan > 0 ? (RealisasiPajakPeneranganJalan / TargetPajakPeneranganJalan) * 100 : 0, 2);
                public decimal TargetPajakParkir { get; set; }
                public decimal RealisasiPajakParkir { get; set; }
                public decimal PersentasePajakParkir => Math.Round(TargetPajakParkir > 0 ? (RealisasiPajakParkir / TargetPajakParkir) * 100 : 0, 2);
                public decimal TargetAbt { get; set; }
                public decimal RealisasiAbt { get; set; }
                public decimal PersentaseAbt => Math.Round(TargetAbt > 0 ? (RealisasiAbt / TargetAbt) * 100 : 0, 2);
                public decimal TargetPbb { get; set; }
                public decimal RealisasiPbb { get; set; }
                public decimal PersentasePbb => Math.Round(TargetPbb > 0 ? (RealisasiPbb / TargetPbb) * 100 : 0, 2);
                public decimal TargetBphtb { get; set; }
                public decimal RealisasiBphtb { get; set; }
                public decimal PersentaseBphtb => Math.Round(TargetBphtb > 0 ? (RealisasiBphtb / TargetBphtb) * 100 : 0, 2);
                public decimal TargetRetribusiDaerah { get; set; }
                public decimal RealisasiRetribusiDaerah { get; set; }
                public decimal PersentaseRetribusiDaerah => Math.Round(TargetRetribusiDaerah > 0 ? (RealisasiRetribusiDaerah / TargetRetribusiDaerah) * 100 : 0, 2);
                public decimal TargetHpkd { get; set; }
                public decimal RealisasiHpkd { get; set; }
                public decimal PersentaseHpkd => Math.Round(TargetHpkd > 0 ? (RealisasiHpkd / TargetHpkd) * 100 : 0, 2);
                public decimal TargetLainlainPad { get; set; }
                public decimal RealisasiLainlainPad { get; set; }
                public decimal PersentaseLainlainPad => Math.Round(TargetLainlainPad > 0 ? (RealisasiLainlainPad / TargetLainlainPad) * 100 : 0, 2);
                public decimal TargetPendapatanTransfer { get; set; }
                public decimal RealisasiPendapatanTransfer { get; set; }
                public decimal PersentasePendapatanTransfer => Math.Round(TargetPendapatanTransfer > 0 ? (RealisasiPendapatanTransfer / TargetPendapatanTransfer) * 100 : 0, 2);
                public decimal TargetTransferPemerintahPusat { get; set; }
                public decimal RealisasiTransferPemerintahPusat { get; set; }
                public decimal PersentaseTransferPemerintahPusat => Math.Round(TargetTransferPemerintahPusat > 0 ? (RealisasiTransferPemerintahPusat / TargetTransferPemerintahPusat) * 100 : 0, 2);
                public decimal TargetDanaPerimbangan { get; set; }
                public decimal RealisasiDanaPerimbangan { get; set; }
                public decimal PersentaseDanaPerimbangan => Math.Round(TargetDanaPerimbangan > 0 ? (RealisasiDanaPerimbangan / TargetDanaPerimbangan) * 100 : 0, 2);
                public decimal TargetDanaBagiHasil { get; set; }
                public decimal RealisasiDanaBagiHasil { get; set; }
                public decimal PersentaseDanaBagiHasil => Math.Round(TargetDanaBagiHasil > 0 ? (RealisasiDanaBagiHasil / TargetDanaBagiHasil) * 100 : 0, 2);
                public decimal TargetDanaAlokasiUmum { get; set; }
                public decimal RealisasiDanaAlokasiUmum { get; set; }
                public decimal PersentaseDanaAlokasiUmum => Math.Round(TargetDanaAlokasiUmum > 0 ? (RealisasiDanaAlokasiUmum / TargetDanaAlokasiUmum) * 100 : 0, 2);
                public decimal TargetAlokasiKhususFisik { get; set; }
                public decimal RealisasiAlokasiKhususFisik { get; set; }
                public decimal PersentaseAlokasiKhususFisik => Math.Round(TargetAlokasiKhususFisik > 0 ? (RealisasiAlokasiKhususFisik / TargetAlokasiKhususFisik) * 100 : 0, 2);
                public decimal TargetAlokasiKhususNonFisik { get; set; }
                public decimal RealisasiAlokasiKhususNonFisik { get; set; }
                public decimal PersentaseAlokasiKhususNonFisik => Math.Round(TargetAlokasiKhususNonFisik > 0 ? (RealisasiAlokasiKhususNonFisik / TargetAlokasiKhususNonFisik) * 100 : 0, 2);
                public decimal TargetInsentifFiskal { get; set; }
                public decimal RealisasiInsentifFiskal { get; set; }
                public decimal PersentaseInsentifFiskal => Math.Round(TargetInsentifFiskal > 0 ? (RealisasiInsentifFiskal / TargetInsentifFiskal) * 100 : 0, 2);
                public decimal TargetPendapatanTransferAntarDaerah { get; set; }
                public decimal RealisasiPendapatanTransferAntarDaerah { get; set; }
                public decimal PersentasePendapatanTransferAntarDaerah => Math.Round(TargetPendapatanTransferAntarDaerah > 0 ? (RealisasiPendapatanTransferAntarDaerah / TargetPendapatanTransferAntarDaerah) * 100 : 0, 2);
                public decimal TargetBagiHasilDaerah { get; set; }
                public decimal RealisasiBagiHasilDaerah { get; set; }
                public decimal PersentaseBagiHasilDaerah => Math.Round(TargetBagiHasilDaerah > 0 ? (RealisasiBagiHasilDaerah / TargetBagiHasilDaerah) * 100 : 0, 2);
                public decimal TargetBantuanKeuangan { get; set; }
                public decimal RealisasiBantuanKeuangan { get; set; }
                public decimal PersentaseBantuanKeuangan => Math.Round(TargetBantuanKeuangan > 0 ? (RealisasiBantuanKeuangan / TargetBantuanKeuangan) * 100 : 0, 2);
                public decimal TargetLainLainPendapatanSah { get; set; }
                public decimal RealisasiLainLainPendapatanSah { get; set; }
                public decimal PersentaseLainLainPendapatanSah => Math.Round(TargetLainLainPendapatanSah > 0 ? (RealisasiLainLainPendapatanSah / TargetLainLainPendapatanSah) * 100 : 0, 2);
                public decimal TargetLainLainSesuaiPerundangan { get; set; }
                public decimal RealisasiLainLainSesuaiPerundangan { get; set; }
                public decimal PersentaseLainLainSesuaiPerundangan => Math.Round(TargetLainLainSesuaiPerundangan > 0 ? (RealisasiLainLainSesuaiPerundangan / TargetLainLainSesuaiPerundangan) * 100 : 0, 2);
                public decimal TargetPenerimaanPembiayaan { get; set; }
                public decimal RealisasiPenerimaanPembiayaan { get; set; }
                public decimal PersentasePenerimaanPembiayaan => Math.Round(TargetPenerimaanPembiayaan > 0 ? (RealisasiPenerimaanPembiayaan / TargetPenerimaanPembiayaan) * 100 : 0, 2);
                public decimal TargetPenerimaanPembiayaanDaerah { get; set; }
                public decimal RealisasiPenerimaanPembiayaanDaerah { get; set; }
                public decimal PersentasePenerimaanPembiayaanDaerah => Math.Round(TargetPenerimaanPembiayaanDaerah > 0 ? (RealisasiPenerimaanPembiayaanDaerah / TargetPenerimaanPembiayaanDaerah) * 100 : 0, 2);
                public decimal TargetSilpaTahunSebelumnya { get; set; }
                public decimal RealisasiSilpaTahunSebelumnya { get; set; }
                public decimal PersentaseSilpaTahunSebelumnya => Math.Round(TargetSilpaTahunSebelumnya > 0 ? (RealisasiSilpaTahunSebelumnya / TargetSilpaTahunSebelumnya) * 100 : 0, 2);
                public decimal TargetPenerimaanKembaliPemberianPinjaman { get; set; }
                public decimal RealisasiPenerimaanKembaliPemberianPinjaman { get; set; }
                public decimal PersentasePenerimaanKembaliPemberianPinjaman => Math.Round(TargetPenerimaanKembaliPemberianPinjaman > 0 ? (RealisasiPenerimaanKembaliPemberianPinjaman / TargetPenerimaanKembaliPemberianPinjaman) * 100 : 0, 2);
                public decimal TargetJumlahTotalPenerimaan => TargetPendapatanAsliDaerah + TargetPendapatanTransfer + TargetLainLainPendapatanSah + TargetPenerimaanPembiayaan;
                public decimal RealisasiJumlahTotalPenerimaan => RealisasiPendapatanAsliDaerah + RealisasiPendapatanTransfer + RealisasiLainLainPendapatanSah + RealisasiPenerimaanPembiayaan;
                public decimal PersentaseJumlahTotalPenerimaan => Math.Round(TargetJumlahTotalPenerimaan > 0 ? (RealisasiJumlahTotalPenerimaan / TargetJumlahTotalPenerimaan) * 100 : 0, 2);
            }
        }
    }
}
