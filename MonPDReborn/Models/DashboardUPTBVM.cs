using DocumentFormat.OpenXml.Drawing.Charts;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Collections.Generic;
using System.Web.Mvc;
using static MonPDReborn.Models.DashboardVM.ViewModel;
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
        public class SeriesPajakDaerah
        {
            public List<ViewModel.SeriesPajakDaerah> Data { get; set; } = new List<ViewModel.SeriesPajakDaerah>();
            public SeriesPajakDaerah(int wilayah)
            {
                Data = Method.GetSeriesPajakDaerahData(wilayah);
            }
        }
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
                var dataTargetListrik = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik && x.Uptb == (decimal)wilayah && x.SubRincian == "2").Sum(x => x.Target);
                var dataTargetPbb = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.PBB && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                var dataTargetAbt = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.AirTanah && x.Uptb == (decimal)wilayah).Sum(x => x.Target);
                
                #region Method Get Jumlah OP
                var OpRestoAkhir = context.DbOpRestos.Where(x => x.WilayahPajak == wilayah.ToString()).AsQueryable();
                var OpHotelAkhir = context.DbOpHotels.Where(x => x.WilayahPajak == wilayah.ToString()).AsQueryable();
                var OpHiburanAkhir = context.DbOpHiburans.Where(x => x.WilayahPajak == wilayah.ToString()).AsQueryable();
                var OpParkirAkhir = context.DbOpParkirs.Where(x => x.WilayahPajak == wilayah.ToString()).AsQueryable();
                var OpListrikAkhir = context.DbOpListriks.Where(x => x.WilayahPajak == wilayah.ToString() && x.Sumber == 55).AsQueryable();
                var OpAbtAkhir = context.DbOpAbts.Where(x => x.WilayahPajak == wilayah.ToString()).AsQueryable();
                var OpPbbAkhir = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear && x.Uptb == wilayah).Select(x => x.Nop).Distinct().AsQueryable();


                #endregion
                // Realisasi
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpRestoAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpHotelAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpHiburanAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpParkirAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear && OpListrikAkhir.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == currentYear && x.TahunBuku == currentYear && OpPbbAkhir.Contains(x.Nop)).Sum(x => x.JumlahBayarPokok) ?? 0;
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
                    JumlahOpMamin = OpRestoAkhir.Where(x => x.PajakNama != "MAMIN" && x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).Count(),

                    TargetHotel = dataTargetHotel,
                    RealisasiHotel = dataRealisasiHotel,
                    PersentaseHotel = dataTargetHotel != 0 ? Math.Round((dataRealisasiHotel / dataTargetHotel) * 100, 2) : 0,
                    JumlahOpHotel = OpHotelAkhir.Where(x=> x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).Count(),

                    TargetHiburan = dataTargetHiburan,
                    RealisasiHiburan = dataRealisasiHiburan,
                    PersentaseHiburan = dataTargetHiburan != 0 ? Math.Round((dataRealisasiHiburan / dataTargetHiburan) * 100, 2) : 0,
                    JumlahOpHiburan = OpHiburanAkhir.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).Count(),

                    TargetParkir = dataTargetParkir,
                    RealisasiParkir = dataRealisasiParkir,
                    PersentaseParkir = dataTargetParkir != 0 ? Math.Round((dataRealisasiParkir / dataTargetParkir) * 100, 2) : 0,
                    JumlahOpParkir = OpParkirAkhir.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).Count(),

                    TargetListrik = dataTargetListrik,
                    RealisasiListrik = dataRealisasiListrik,
                    PersentaseListrik = dataTargetListrik != 0 ? Math.Round((dataRealisasiListrik / dataTargetListrik) * 100, 2) : 0,
                    JumlahOpListrik = OpListrikAkhir.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).Count(),

                    TargetPbb = dataTargetPbb,
                    RealisasiPbb = dataRealisasiPbb,
                    PersentasePbb = dataTargetPbb != 0 ? Math.Round((dataRealisasiPbb / dataTargetPbb) * 100, 2) : 0,
                    JumlahOpPbb = OpPbbAkhir.Count(),

                    TargetAbt = dataTargetAbt,
                    RealisasiAbt = dataRealisasiAbt,
                    PersentaseAbt = dataTargetAbt != 0 ? Math.Round((dataRealisasiAbt / dataTargetAbt) * 100, 2) : 0,
                    JumlahOpAbt = OpAbtAkhir.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear)).Count(),

                    
                };

                return result;
            }
            public static List<ViewModel.SeriesPajakDaerah> GetSeriesPajakDaerahData(int wilayah)
            {
                var result = new List<ViewModel.SeriesPajakDaerah>();
                var context = DBClass.GetContext();
                var year = DateTime.Now.Year;
                var yearLast = year - 6;

                //target
                var targetData = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku >= yearLast && x.TahunBuku <= year && x.Uptb == wilayah && x.PajakId != 7 && x.PajakId != 12 && x.PajakId != 20 && x.PajakId != 21 && (x.PajakId != 2 || x.SubRincian == "2"))
                    .GroupBy(x => new { x.PajakId, x.TahunBuku })
                    .Select(x => new { TahunBuku = x.Key.TahunBuku, PajakId = x.Key.PajakId, Target = x.Sum(q => q.Target) })
                    .AsEnumerable();

                //Wilayah
                var nopListSemuaAbt = context.DbOpAbts
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();

                var nopListSemuaResto = context.DbOpRestos
                    .Where(x =>x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();

                var nopListSemuaHotel = context.DbOpHotels
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();

                var nopListSemuaListrik = context.DbOpListriks
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && x.Sumber == 55)
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();

                var nopListSemuaParkir = context.DbOpParkirs
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();

                var nopListSemuaHiburan = context.DbOpHiburans
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();

                var nopListSemuaPbb = context.DbMonPbbs
                    .Where(x => x.TahunBuku == year && x.Uptb == wilayah)
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();


                //realisasi
                var dataRealisasiResto = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year && nopListSemuaResto.Contains(x.Nop))
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.MakananMinuman) })
                    .Select(x => new { TahunBuku = x.Key.Year, PajakId = x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiListrik = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year && nopListSemuaListrik.Contains(x.Nop))
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.TenagaListrik) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiHotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year && nopListSemuaHotel.Contains(x.Nop))
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaPerhotelan) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiParkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year && nopListSemuaParkir.Contains(x.Nop))
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaParkir) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiHiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year && nopListSemuaHiburan.Contains(x.Nop))
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaKesenianHiburan) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiAbt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year && nopListSemuaAbt.Contains(x.Nop))
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.AirTanah) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiPbb = Enumerable.Range(yearLast, year - yearLast + 1)
                    .Select(t => new
                    {
                        TahunBuku = t,
                        PajakId = (int)EnumFactory.EPajak.PBB,
                        Realisasi = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == t
                                        && x.TahunBuku == t
                                        && x.JumlahBayarPokok > 0)
                            .Sum(x => x.JumlahBayarPokok ?? 0)
                    })
                    .AsEnumerable();

                //isi realisasi data
                var dataRealisasi = new List<(int TahunBuku, int PajakId, decimal Realisasi)>();
                dataRealisasi.AddRange(dataRealisasiResto.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiListrik.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiHotel.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiParkir.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiHiburan.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiAbt.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiPbb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));

                var pajakList = context.MPajaks.Where(x => x.Id > 0 && x.Id != 7 && x.Id != 12 && x.Id != 20 && x.Id != 21).Select(x => x.Id).ToList();
                foreach (var pajakId in pajakList)
                {
                    int tahun1 = year - 6;
                    int tahun2 = year - 5;
                    int tahun3 = year - 4;
                    int tahun4 = year - 3;
                    int tahun5 = year - 2;
                    int tahun6 = year - 1;
                    int tahun7 = year;

                    decimal target1 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun1).FirstOrDefault()?.Target ?? 0;
                    decimal target2 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun2).FirstOrDefault()?.Target ?? 0;
                    decimal target3 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun3).FirstOrDefault()?.Target ?? 0;
                    decimal target4 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun4).FirstOrDefault()?.Target ?? 0;
                    decimal target5 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun5).FirstOrDefault()?.Target ?? 0;
                    decimal target6 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun6).FirstOrDefault()?.Target ?? 0;
                    decimal target7 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun7).FirstOrDefault()?.Target ?? 0;
                    decimal realisasi1 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun1).FirstOrDefault().Realisasi;
                    decimal realisasi2 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun2).FirstOrDefault().Realisasi;
                    decimal realisasi3 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun3).FirstOrDefault().Realisasi;
                    decimal realisasi4 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun4).FirstOrDefault().Realisasi;
                    decimal realisasi5 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun5).FirstOrDefault().Realisasi;
                    decimal realisasi6 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun6).FirstOrDefault().Realisasi;
                    decimal realisasi7 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun7).FirstOrDefault().Realisasi;
                    decimal persentase1 = target1 != 0 ? Math.Round(realisasi1 / target1 * 100, 2) : 0;
                    decimal persentase2 = target2 != 0 ? Math.Round(realisasi2 / target2 * 100, 2) : 0;
                    decimal persentase3 = target3 != 0 ? Math.Round(realisasi3 / target3 * 100, 2) : 0;
                    decimal persentase4 = target4 != 0 ? Math.Round(realisasi4 / target4 * 100, 2) : 0;
                    decimal persentase5 = target5 != 0 ? Math.Round(realisasi5 / target5 * 100, 2) : 0;
                    decimal persentase6 = target6 != 0 ? Math.Round(realisasi6 / target6 * 100, 2) : 0;
                    decimal persentase7 = target7 != 0 ? Math.Round(realisasi7 / target7 * 100, 2) : 0;



                    var res = new ViewModel.SeriesPajakDaerah();
                    res.JenisPajak = ((EnumFactory.EPajak)pajakId).GetDescription();
                    res.Target1 = target1;
                    res.Target2 = target2;
                    res.Target3 = target3;
                    res.Target4 = target4;
                    res.Target5 = target5;
                    res.Target6 = target6;
                    res.Target7 = target7;
                    res.Realisasi1 = realisasi1;
                    res.Realisasi2 = realisasi2;
                    res.Realisasi3 = realisasi3;
                    res.Realisasi4 = realisasi4;
                    res.Realisasi5 = realisasi5;
                    res.Realisasi6 = realisasi6;
                    res.Realisasi7 = realisasi7;
                    res.Persentase1 = persentase1;
                    res.Persentase2 = persentase2;
                    res.Persentase3 = persentase3;
                    res.Persentase4 = persentase4;
                    res.Persentase5 = persentase5;
                    res.Persentase6 = persentase6;
                    res.Persentase7 = persentase7;

                    result.Add(res);
                }


                return result;
            }
        }
    }
}
