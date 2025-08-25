using DocumentFormat.OpenXml.Drawing.Charts;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Collections.Generic;
using System.Web.Mvc;
using static MonPDReborn.Models.DataOP.ProfileOPVM;
using static MonPDReborn.Models.MonitoringGlobal.MonitoringTahunanVM.MonitoringTahunanViewModels;

namespace MonPDReborn.Models
{
    public class DashboardUPTBVM
    {
        public class Index
        {
            public ViewModel.DashboardUPTB Data { get; set; } = new ViewModel.DashboardUPTB();
            public ViewModel.DashboardUPTBChart ChartData { get; set; } = new ViewModel.DashboardUPTBChart();
            public string Em { get; set; } = string.Empty;
            public int wil { get; set; }
            public Index(int wilayah)
            {
                wil = wilayah;
                Data = Method.GetDashboardUPTBData(wilayah);
                //ChartData = Method.GetDashboardUPTBChartData();
            }
            public Index(string em)
            {
                Em = em;
            }
        }
        //public class SeriesPajakDaerah
        //{
        //    public List<ViewModel.SeriesPajakDaerah> Data { get; set; } = new List<ViewModel.SeriesPajakDaerah>();
        //    public SeriesPajakDaerah()
        //    {
        //        Data = Method.GetSeriesPajakDaerahData();
        //    }
        //}
        //public class JumlahObjekPajakTahunan
        //{
        //    public List<ViewModel.JumlahObjekPajakTahunan> Data { get; set; } = new List<ViewModel.JumlahObjekPajakTahunan>();
        //    public JumlahObjekPajakTahunan()
        //    {
        //        Data = Method.GetJumlahObjekPajakTahunanData();
        //    }
        //}
        //public class JumlahObjekPajakSeries
        //{
        //    public List<ViewModel.JumlahObjekPajakSeries> Data { get; set; } = new List<ViewModel.JumlahObjekPajakSeries>();
        //    public JumlahObjekPajakSeries()
        //    {
        //        Data = Method.GetJumlahObjekPajakSeriesData();
        //    }
        //}
        //public class PemasanganAlatRekamDetail
        //{
        //    public List<ViewModel.PemasanganAlatRekamDetail> Data { get; set; } = new List<ViewModel.PemasanganAlatRekamDetail>();
        //    public PemasanganAlatRekamDetail()
        //    {
        //        Data = Method.GetPemasanganAlatRekamDetailData();
        //    }
        //}
        //public class PemasanganAlatRekamSeries
        //{
        //    public List<ViewModel.PemasanganAlatRekamSeries> Data { get; set; } = new List<ViewModel.PemasanganAlatRekamSeries>();
        //    public PemasanganAlatRekamSeries()
        //    {
        //        Data = Method.GetPemasanganAlatRekamSeriesData();
        //    }
        //}
        //public class PemeriksaanPajak
        //{
        //    public List<ViewModel.PemeriksaanPajak> Data { get; set; } = new List<ViewModel.PemeriksaanPajak>();
        //    public PemeriksaanPajak()
        //    {
        //        Data = Method.GetPemeriksaanPajakData();
        //    }
        //}
        //public class PengedokanPajak
        //{
        //    public List<ViewModel.PengedokanPajak> Data { get; set; } = new List<ViewModel.PengedokanPajak>();
        //    public PengedokanPajak()
        //    {
        //        Data = Method.GetPengedokanPajakData();
        //    }
        //}
        //public class DataKontrolPotensiOp
        //{
        //    public List<ViewModel.DataKontrolPotensiOp> Data { get; set; } = new List<ViewModel.DataKontrolPotensiOp>();
        //    public DataKontrolPotensiOp()
        //    {
        //        Data = Method.GetDataKontrolPotensiOp();
        //    }
        //}
        
        //public class DataPiutang
        //{
        //    public List<ViewModel.DataPiutang> Data { get; set; } = new List<ViewModel.DataPiutang>();
        //    public DataPiutang()
        //    {
        //        Data = Method.GetDataPiutangData();
        //    }
        //}
        //public class DetailPiutang
        //{
        //    public List<ViewModel.DetailPiutang> Data { get; set; } = new List<ViewModel.DetailPiutang>();
        //    public DetailPiutang(EnumFactory.EPajak jenisPajak, int tahun)
        //    {
        //        Data = Method.GetDetailPiutangData(jenisPajak, tahun);
        //    }
        //}
        //public class DataMutasi
        //{
        //    public List<ViewModel.DataMutasi> Data { get; set; } = new List<ViewModel.DataMutasi>();
        //    public DataMutasi()
        //    {
        //        Data = Method.GetDataMutasiData();
        //    }
        //}

        //public class JumlahObjekPajak
        //{
        //    public List<ViewModel.JumlahObjekPajak> Data { get; set; } = new List<ViewModel.JumlahObjekPajak>();
        //    public JumlahObjekPajak()
        //    {
        //        Data = Method.GetJumlahObjekPajakData();
        //    }
        //}


        public class ViewModel
        {
            public class X
            {

            }
            public class DashboardUPTB
            {
                public decimal TotalTarget { get; set; }
                public decimal TotalRealisasi { get; set; }
                public decimal TotalPersentase { get; set; }
                public decimal TargetHotel { get; set; }
                public decimal RealisasiHotel { get; set; }
                public decimal PersentaseHotel { get; set; }
                public decimal TargetHiburan { get; set; }
                public decimal RealisasiHiburan { get; set; }
                public decimal PersentaseHiburan { get; set; }
                public decimal TargetParkir { get; set; }
                public decimal RealisasiParkir { get; set; }
                public decimal PersentaseParkir { get; set; }
                public decimal TargetMamin { get; set; }
                public decimal RealisasiMamin { get; set; }
                public decimal PersentaseMamin { get; set; }
                public decimal TargetListrik { get; set; }
                public decimal RealisasiListrik { get; set; }
                public decimal PersentaseListrik { get; set; }
                public decimal TargetAbt { get; set; }
                public decimal RealisasiAbt { get; set; }
                public decimal PersentaseAbt { get; set; }
                public decimal TargetPbb { get; set; }
                public decimal RealisasiPbb { get; set; }
                public decimal PersentasePbb { get; set; }
                public decimal TargetReklame { get; set; }
                public decimal RealisasiReklame { get; set; }
                public decimal PersentaseReklame { get; set; }
                public decimal TargetBphtb { get; set; }
                public decimal RealisasiBphtb { get; set; }
                public decimal PersentaseBphtb { get; set; }
                public decimal TargetOpsenPkb { get; set; }
                public decimal RealisasiOpsenPkb { get; set; }
                public decimal PersentaseOpsenPkb { get; set; }
                public decimal TargetOpsenBbnkb { get; set; }
                public decimal RealisasiOpsenBbnkb { get; set; }
                public decimal PersentaseOpsenBbnkb { get; set; }

                //JumlahOP 
                public decimal JumlahOpHotel { get; set; }
                public decimal JumlahOpHiburan { get; set; }
                public decimal JumlahOpParkir { get; set; }
                public decimal JumlahOpMamin { get; set; }
                public decimal JumlahOpListrik { get; set; }
                public decimal JumlahOpAbt { get; set; }
                public decimal JumlahOpPbb { get; set; }
                public decimal JumlahOpReklame { get; set; }
                public decimal JumlahOpBphtb { get; set; }
                public decimal JumlahOpOpsenPkb { get; set; }
                public decimal JumlahOpOpsenBbnkb { get; set; }
                public decimal TotalJumlahOp { get; set; }

            }
            public class DashboardUPTBChart
            {
                public decimal Target1 { get; set; }
                public decimal Target2 { get; set; }
                public decimal Target3 { get; set; }
                public decimal Target4 { get; set; }
                public decimal Target5 { get; set; }
                public decimal Target6 { get; set; }
                public decimal Target7 { get; set; }
                public decimal Target8 { get; set; }
                public decimal Target9 { get; set; }
                public decimal Target10 { get; set; }
                public decimal Target11 { get; set; }
                public decimal Target12 { get; set; }
                public decimal Realisasi1 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Realisasi3 { get; set; }
                public decimal Realisasi4 { get; set; }
                public decimal Realisasi5 { get; set; }
                public decimal Realisasi6 { get; set; }
                public decimal Realisasi7 { get; set; }
                public decimal Realisasi8 { get; set; }
                public decimal Realisasi9 { get; set; }
                public decimal Realisasi10 { get; set; }
                public decimal Realisasi11 { get; set; }
                public decimal Realisasi12 { get; set; }
            }
            public class SeriesPajakDaerah
            {
                public string JenisPajak { get; set; } = "";
                public decimal Target1 { get; set; }
                public decimal Target2 { get; set; }
                public decimal Target3 { get; set; }
                public decimal Target4 { get; set; }
                public decimal Target5 { get; set; }
                public decimal Target6 { get; set; }
                public decimal Target7 { get; set; }
                public decimal Realisasi1 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Realisasi3 { get; set; }
                public decimal Realisasi4 { get; set; }
                public decimal Realisasi5 { get; set; }
                public decimal Realisasi6 { get; set; }
                public decimal Realisasi7 { get; set; }
                public decimal Persentase1 { get; set; }
                public decimal Persentase2 { get; set; }
                public decimal Persentase3 { get; set; }
                public decimal Persentase4 { get; set; }
                public decimal Persentase5 { get; set; }
                public decimal Persentase6 { get; set; }
                public decimal Persentase7 { get; set; }
            }
            public class JumlahObjekPajakTahunan
            {
                public string JenisPajak { get; set; } = "";
                public decimal JmlOpAwal { get; set; }
                public decimal JmlOpTutupSementara { get; set; }
                public decimal JmlOpTutupPermanen { get; set; }
                public decimal JmlOpBaru { get; set; }
                public decimal JmlOpAkhir { get; set; }
            }
            public class JumlahObjekPajakSeries
            {
                public string JenisPajak { get; set; }
                public decimal Jumlah1 { get; set; }
                public decimal Jumlah2 { get; set; }
                public decimal Jumlah3 { get; set; }
                public decimal Jumlah4 { get; set; }
                public decimal Jumlah5 { get; set; }
            }

            public class JumlahObjekPajak
            {
                public int EnumPajak { get; set; }
                public string JenisPajak { get; set; }
                public decimal Tahun1_Awal { get; set; }
                public decimal Tahun1_Tutup { get; set; }
                public decimal Tahun1_Baru { get; set; }
                public decimal Tahun1_Akhir { get; set; }

                public decimal Tahun2_Awal { get; set; }
                public decimal Tahun2_Tutup { get; set; }
                public decimal Tahun2_Baru { get; set; }
                public decimal Tahun2_Akhir { get; set; }

                public decimal Tahun3_Awal { get; set; }
                public decimal Tahun3_Tutup { get; set; }
                public decimal Tahun3_Baru { get; set; }
                public decimal Tahun3_Akhir { get; set; }

                public decimal Tahun4_Awal { get; set; }
                public decimal Tahun4_Tutup { get; set; }
                public decimal Tahun4_Baru { get; set; }
                public decimal Tahun4_Akhir { get; set; }

                public decimal Tahun5_Awal { get; set; }
                public decimal Tahun5_Tutup { get; set; }
                public decimal Tahun5_Baru { get; set; }
                public decimal Tahun5_Akhir { get; set; }
            }
            public class PemasanganAlatRekamSeries
            {
                public string JenisPajak { get; set; }
                public decimal Jumlah1 { get; set; }
                public decimal Jumlah2 { get; set; }
                public decimal Jumlah3 { get; set; }
                public decimal Jumlah4 { get; set; }
                public decimal Jumlah5 { get; set; }
            }
            public class PemasanganAlatRekamDetail
            {
                public string JenisPajak { get; set; } = "";
                public int JmlOp { get; set; }
                public int JmlTerpasangTS { get; set; }
                public int JmlTerpasangTB { get; set; }
                public int JmlTerpasangSB { get; set; }
                public int TotalTerpasang { get; set; }
                public int TotalBelumTerpasang { get; set; }
            }
            public class PemeriksaanPajak
            {
                public string JenisPajak { get; set; } = "";
                public decimal OpPeriksa1 { get; set; }
                public decimal OpPeriksa2 { get; set; }
                public decimal OpPeriksa3 { get; set; }
                public decimal Pokok1 { get; set; }
                public decimal Pokok2 { get; set; }
                public decimal Pokok3 { get; set; }
                public decimal Sanksi1 { get; set; }
                public decimal Sanksi2 { get; set; }
                public decimal Sanksi3 { get; set; }
                public decimal Total1 { get; set; }
                public decimal Total2 { get; set; }
                public decimal Total3 { get; set; }
            }
            public class PengedokanPajak
            {
                public string JenisPajak { get; set; } = "";
                public decimal JmlOp { get; set; }
                public decimal PotensiHasilPengedokan { get; set; }
                public decimal TotalRealisasi { get; set; }
                public decimal Selisih { get; set; }
            }
            public class DataKontrolPotensiOp
            {
                public string JenisPajak { get; set; } = "";
                public decimal Target1 { get; set; }
                public decimal Realisasi1 { get; set; }
                public decimal Selisih1 { get; set; }
                public decimal Persentase1 { get; set; }
                public decimal Target2 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Selisih2 { get; set; }
                public decimal Persentase2 { get; set; }
            }
            public class DataPiutang
            {
                public string JenisPajak { get; set; } = "";
                public int EnumPajak { get; set; }
                public decimal NominalPiutang1 { get; set; }
                public decimal NominalPiutang2 { get; set; }
                public decimal NominalPiutang3 { get; set; }
                public decimal NominalPiutang4 { get; set; }
            }
            public class DetailPiutang
            {
                public string Nop { get; set; } = "";
                public string NamaOp { get; set; } = "";
                public string AlamatOp { get; set; } = "";
                public string WilayahPajak { get; set; } = "";
                public string JenisPajak { get; set; }
                public int EnumPajak { get; set; }
                public int MasaPajak { get; set; }
                public int TahunPajak { get; set; }
                public decimal NominalPiutang { get; set; }
            }
            public class DataMutasi
            {
                public string Keterangan { get; set; } = "";
                public decimal NominalMutasi1 { get; set; }
                public decimal NominalMutasi2 { get; set; }
                public decimal NominalMutasi3 { get; set; }
                public decimal NominalMutasi4 { get; set; }
                public decimal NominalMutasi5 { get; set; }
            }
        }

        public class Method
        {
            public static ViewModel.DashboardUPTB GetDashboardUPTBData(int wilayah)
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;
                // Target
                var dataTargetMamin = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                var dataTargetHotel = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                var dataTargetHiburan = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                var dataTargetParkir = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaParkir && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                var dataTargetListrik = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                var dataTargetPbb = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.PBB && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                var dataTargetAbt = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.AirTanah && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                
                #region Method Get Jumlah OP
                var OpRestoAkhir = context.DbOpRestos.Where(x => x.TahunBuku == currentYear && x.PajakNama != "MAMIN" && x.WilayahPajak == wilayah.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).AsQueryable();
                var OpHotelAkhir = context.DbOpHotels.Where(x => x.TahunBuku == currentYear && x.WilayahPajak == wilayah.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).AsQueryable();
                var OpHiburanAkhir = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear && x.WilayahPajak == wilayah.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).AsQueryable();
                var OpParkirAkhir = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear && x.WilayahPajak == wilayah.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).AsQueryable();
                var OpListrikAkhir = context.DbOpListriks.Where(x => x.TahunBuku == currentYear && x.WilayahPajak == wilayah.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).AsQueryable();
                var OpAbtAkhir = context.DbOpAbts.Where(x => x.TahunBuku == currentYear && x.WilayahPajak == wilayah.ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpPbbAkhir = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear && x.Uptb == wilayah).Select(x => x.Nop).Distinct().AsQueryable();


                #endregion
                // Realisasi
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpRestoAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpHotelAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpHiburanAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpParkirAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpListrikAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == currentYear && OpPbbAkhir.Contains(x.Nop)).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpAbtAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                
                // Total keseluruhan
                decimal TotalTarget = dataTargetMamin + dataTargetHotel + dataTargetHiburan + dataTargetParkir + dataTargetListrik 
                                    + dataTargetPbb + dataTargetAbt;

                decimal TotalRealisasi = dataRealisasiMamin + dataRealisasiHotel + dataRealisasiHiburan + dataRealisasiParkir + dataRealisasiListrik
                                       + dataRealisasiPbb + dataRealisasiAbt;

                decimal TotalPersentase = TotalTarget != 0 ? (TotalRealisasi / TotalTarget) * 100 : 0;

                

                // Hasil akhir ViewModel
                var result = new ViewModel.DashboardUPTB
                {
                    TotalTarget = TotalTarget,
                    TotalRealisasi = TotalRealisasi,
                    TotalPersentase = Math.Round(TotalPersentase, 2),
                    TotalJumlahOp = 0,

                    TargetMamin = dataTargetMamin,
                    RealisasiMamin = dataRealisasiMamin,
                    PersentaseMamin = dataTargetMamin != 0 ? Math.Round((dataRealisasiMamin / dataTargetMamin) * 100, 2) : 0,
                    JumlahOpMamin = OpRestoAkhir.Count(),

                    TargetHotel = dataTargetHotel,
                    RealisasiHotel = dataRealisasiHotel,
                    PersentaseHotel = dataTargetHotel != 0 ? Math.Round((dataRealisasiHotel / dataTargetHotel) * 100, 2) : 0,
                    JumlahOpHotel = OpHotelAkhir.Count(),

                    TargetHiburan = dataTargetHiburan,
                    RealisasiHiburan = dataRealisasiHiburan,
                    PersentaseHiburan = dataTargetHiburan != 0 ? Math.Round((dataRealisasiHiburan / dataTargetHiburan) * 100, 2) : 0,
                    JumlahOpHiburan = OpHiburanAkhir.Count(),

                    TargetParkir = dataTargetParkir,
                    RealisasiParkir = dataRealisasiParkir,
                    PersentaseParkir = dataTargetParkir != 0 ? Math.Round((dataRealisasiParkir / dataTargetParkir) * 100, 2) : 0,
                    JumlahOpParkir = OpParkirAkhir.Count(),

                    TargetListrik = dataTargetListrik,
                    RealisasiListrik = dataRealisasiListrik,
                    PersentaseListrik = dataTargetListrik != 0 ? Math.Round((dataRealisasiListrik / dataTargetListrik) * 100, 2) : 0,
                    JumlahOpListrik = OpListrikAkhir.Count(),

                    TargetPbb = dataTargetPbb,
                    RealisasiPbb = dataRealisasiPbb,
                    PersentasePbb = dataTargetPbb != 0 ? Math.Round((dataRealisasiPbb / dataTargetPbb) * 100, 2) : 0,
                    JumlahOpPbb = OpPbbAkhir.Count(),

                    TargetAbt = dataTargetAbt,
                    RealisasiAbt = dataRealisasiAbt,
                    PersentaseAbt = dataTargetAbt != 0 ? Math.Round((dataRealisasiAbt / dataTargetAbt) * 100, 2) : 0,
                    JumlahOpAbt = OpAbtAkhir.Count(),

                    
                };

                return result;
            }
        }
    }
}
