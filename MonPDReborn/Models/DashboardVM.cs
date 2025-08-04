using DocumentFormat.OpenXml.Drawing.Charts;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Web.Mvc;
using static MonPDReborn.Models.DataOP.ProfileOPVM;
using static MonPDReborn.Models.MonitoringGlobal.MonitoringTahunanVM.MonitoringTahunanViewModels;

namespace MonPDReborn.Models
{
    public class DashboardVM
    {
        public class Index
        {
            public ViewModel.Dashboard Data { get; set; } = new ViewModel.Dashboard();
            public ViewModel.DashboardChart ChartData { get; set; } = new ViewModel.DashboardChart();
            public string Em { get; set; } = string.Empty;
            public Index()
            {
                Data = Method.GetDashboardData();
                ChartData = Method.GetDashboardChartData();
            }
            public Index(string em)
            {
                Em = em;
            }
        }
        public class SeriesPajakDaerah
        {
            public List<ViewModel.SeriesPajakDaerah> Data { get; set; } = new List<ViewModel.SeriesPajakDaerah>();
            public SeriesPajakDaerah()
            {
                Data = Method.GetSeriesPajakDaerahData();
            }
        }
        public class JumlahObjekPajakTahunan
        {
            public List<ViewModel.JumlahObjekPajakTahunan> Data { get; set; } = new List<ViewModel.JumlahObjekPajakTahunan>();
            public JumlahObjekPajakTahunan()
            {
                Data = Method.GetJumlahObjekPajakTahunanData();
            }
        }
        public class JumlahObjekPajakSeries
        {
            public List<ViewModel.JumlahObjekPajakSeries> Data { get; set; } = new List<ViewModel.JumlahObjekPajakSeries>();
            public JumlahObjekPajakSeries()
            {
                Data = Method.GetJumlahObjekPajakSeriesData();
            }
        }
        public class PemasanganAlatRekamDetail
        {
            public List<ViewModel.PemasanganAlatRekamDetail> Data { get; set; } = new List<ViewModel.PemasanganAlatRekamDetail>();
            public PemasanganAlatRekamDetail()
            {
                Data = Method.GetPemasanganAlatRekamDetailData();
            }
        }
        public class PemasanganAlatRekamSeries
        {
            public List<ViewModel.PemasanganAlatRekamSeries> Data { get; set; } = new List<ViewModel.PemasanganAlatRekamSeries>();
            public PemasanganAlatRekamSeries()
            {
                Data = Method.GetPemasanganAlatRekamSeriesData();
            }
        }
        public class PemeriksaanPajak
        {
            public List<ViewModel.PemeriksaanPajak> Data { get; set; } = new List<ViewModel.PemeriksaanPajak>();
            public PemeriksaanPajak()
            {
                Data = Method.GetPemeriksaanPajakData();
            }
        }
        public class PengedokanPajak
        {
            public List<ViewModel.PengedokanPajak> Data { get; set; } = new List<ViewModel.PengedokanPajak>();
            public PengedokanPajak()
            {
                Data = Method.GetPengedokanPajakData();
            }
        }
        public class DataKontrolPotensiOp
        {
            public List<ViewModel.DataKontrolPotensiOp> Data { get; set; } = new List<ViewModel.DataKontrolPotensiOp>();
            public DataKontrolPotensiOp()
            {
                Data = Method.GetDataKontrolPotensiOp();
            }
        }
        public class DataPiutang
        {
            public List<ViewModel.DataPiutang> Data { get; set; } = new List<ViewModel.DataPiutang>();
            public DataPiutang()
            {
                Data = Method.GetDataPiutangData();
            }
        }
        public class DetailPiutang
        {
            public List<ViewModel.DetailPiutang> Data { get; set; } = new List<ViewModel.DetailPiutang>();
            public DetailPiutang(EnumFactory.EPajak jenisPajak, int tahun)
            {
                Data = Method.GetDetailPiutangData(jenisPajak, tahun);
            }
        }
        public class DataMutasi
        {
            public List<ViewModel.DataMutasi> Data { get; set; } = new List<ViewModel.DataMutasi>();
            public DataMutasi()
            {
                Data = Method.GetDataMutasiData();
            }
        }

        public class JumlahObjekPajak
        {
            public List<ViewModel.JumlahObjekPajak> Data { get; set; } = new List<ViewModel.JumlahObjekPajak>();
            public JumlahObjekPajak()
            {
                Data = Method.GetJumlahObjekPajakData();
            }
        }


        public class ViewModel
        {
            public class X
            {

            }
            public class Dashboard
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
            public class DashboardChart
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
            public static ViewModel.Dashboard GetDashboardData()
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;
                // Target
                var dataTargetMamin = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var dataTargetHotel = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var dataTargetHiburan = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var dataTargetParkir = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var dataTargetListrik = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var dataTargetReklame = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var dataTargetPbb = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                var dataTargetBphtb = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var dataTargetAbt = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var dataTargetOpsenPkb = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var dataTargetOpsenBbnkb = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);

                // Realisasi
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == currentYear).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);
                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);

                // Total keseluruhan
                decimal TotalTarget = dataTargetMamin + dataTargetHotel + dataTargetHiburan + dataTargetParkir + dataTargetListrik + dataTargetReklame
                                    + dataTargetPbb + dataTargetBphtb + dataTargetAbt + dataTargetOpsenPkb + dataTargetOpsenBbnkb;

                decimal TotalRealisasi = dataRealisasiMamin + dataRealisasiHotel + dataRealisasiHiburan + dataRealisasiParkir + dataRealisasiListrik + dataRealisasiReklame
                                       + dataRealisasiPbb + dataRealisasiBphtb + dataRealisasiAbt + dataRealisasiOpsenPkb + dataRealisasiOpsenBbnkb;

                decimal TotalPersentase = TotalTarget != 0 ? (TotalRealisasi / TotalTarget) * 100 : 0;

                #region Method Get Jumlah OP
                var OpRestoAkhir = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpHotelAkhir = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpHiburanAkhir = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpParkirAkhir = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpListrikAkhir = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpAbtAkhir = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpPbbAkhir = context.DbOpPbbs.Count();
                var OpReklameAkhir = context.DbOpReklames.Count(x => x.TahunBuku == currentYear);

                #endregion

                // Hasil akhir ViewModel
                var result = new ViewModel.Dashboard
                {
                    TotalTarget = TotalTarget,
                    TotalRealisasi = TotalRealisasi,
                    TotalPersentase = Math.Round(TotalPersentase, 2),
                    TotalJumlahOp = 0,

                    TargetMamin = dataTargetMamin,
                    RealisasiMamin = dataRealisasiMamin,
                    PersentaseMamin = dataTargetMamin != 0 ? Math.Round((dataRealisasiMamin / dataTargetMamin) * 100, 2) : 0,
                    JumlahOpMamin = OpRestoAkhir,

                    TargetHotel = dataTargetHotel,
                    RealisasiHotel = dataRealisasiHotel,
                    PersentaseHotel = dataTargetHotel != 0 ? Math.Round((dataRealisasiHotel / dataTargetHotel) * 100, 2) : 0,
                    JumlahOpHotel = OpHotelAkhir,

                    TargetHiburan = dataTargetHiburan,
                    RealisasiHiburan = dataRealisasiHiburan,
                    PersentaseHiburan = dataTargetHiburan != 0 ? Math.Round((dataRealisasiHiburan / dataTargetHiburan) * 100, 2) : 0,
                    JumlahOpHiburan = OpHiburanAkhir,

                    TargetParkir = dataTargetParkir,
                    RealisasiParkir = dataRealisasiParkir,
                    PersentaseParkir = dataTargetParkir != 0 ? Math.Round((dataRealisasiParkir / dataTargetParkir) * 100, 2) : 0,
                    JumlahOpParkir = OpParkirAkhir,

                    TargetListrik = dataTargetListrik,
                    RealisasiListrik = dataRealisasiListrik,
                    PersentaseListrik = dataTargetListrik != 0 ? Math.Round((dataRealisasiListrik / dataTargetListrik) * 100, 2) : 0,
                    JumlahOpListrik = OpListrikAkhir,

                    TargetPbb = dataTargetPbb,
                    RealisasiPbb = dataRealisasiPbb,
                    PersentasePbb = dataTargetPbb != 0 ? Math.Round((dataRealisasiPbb / dataTargetPbb) * 100, 2) : 0,
                    JumlahOpPbb = OpPbbAkhir,

                    TargetBphtb = dataTargetBphtb,
                    RealisasiBphtb = dataRealisasiBphtb,
                    PersentaseBphtb = dataTargetBphtb != 0 ? Math.Round((dataRealisasiBphtb / dataTargetBphtb) * 100, 2) : 0,
                    JumlahOpBphtb = OpPbbAkhir,

                    TargetReklame = dataTargetReklame,
                    RealisasiReklame = dataRealisasiReklame,
                    PersentaseReklame = dataTargetReklame != 0 ? Math.Round((dataRealisasiReklame / dataTargetReklame) * 100, 2) : 0,
                    JumlahOpReklame = OpReklameAkhir,

                    TargetAbt = dataTargetAbt,
                    RealisasiAbt = dataRealisasiAbt,
                    PersentaseAbt = dataTargetAbt != 0 ? Math.Round((dataRealisasiAbt / dataTargetAbt) * 100, 2) : 0,
                    JumlahOpAbt = OpAbtAkhir,

                    TargetOpsenPkb = dataTargetOpsenPkb,
                    RealisasiOpsenPkb = dataRealisasiOpsenPkb,
                    PersentaseOpsenPkb = dataTargetOpsenPkb != 0 ? Math.Round((dataRealisasiOpsenPkb / dataTargetOpsenPkb) * 100, 2) : 0,
                    JumlahOpOpsenPkb = 0,

                    TargetOpsenBbnkb = dataTargetOpsenBbnkb,
                    RealisasiOpsenBbnkb = dataRealisasiOpsenBbnkb,
                    PersentaseOpsenBbnkb = dataTargetOpsenBbnkb != 0 ? Math.Round((dataRealisasiOpsenBbnkb / dataTargetOpsenBbnkb) * 100, 2) : 0,
                    JumlahOpOpsenBbnkb = 0
                };

                return result;
            }
            public static ViewModel.DashboardChart GetDashboardChartData()
            {
                var context = DBClass.GetContext();
                int currentYear = DateTime.Now.Year;
                decimal monthlyTarget = 100_000_000;

                var monthlyRealisasi = new decimal[12];

                // Mamin
                var resto = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Hotel
                var hotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Hiburan
                var hiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Parkir
                var parkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Listrik (PPJ)
                var ppj = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // PBB
                var pbb = context.DbMonPbbs
                    .Where(x => x.TglBayar.HasValue && x.TglBayar.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayar.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.JumlahBayarPokok ?? 0) });

                // BPHTB
                var bphtb = context.DbMonBphtbs
                    .Where(x => x.TglBayar.HasValue && x.TglBayar.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayar.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Pokok ?? 0) });

                // ABT
                var abt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Opsen PKB
                var opsenPkb = context.DbMonOpsenPkbs
                    .Where(x => x.TglSspd.Year == currentYear)
                    .GroupBy(x => x.TglSspd.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.JmlPokok) });

                // Opsen BBNKB
                var opsenBbnkb = context.DbMonOpsenBbnkbs
                    .Where(x => x.TglSspd.Year == currentYear)
                    .GroupBy(x => x.TglSspd.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.JmlPokok) });

                // Gabungkan semua data pajak ke monthlyRealisasi
                void Tambah(IEnumerable<dynamic> data)
                {
                    foreach (var d in data)
                    {
                        int idx = d.Month - 1;
                        if (idx >= 0 && idx < 12)
                            monthlyRealisasi[idx] += d.Total;
                    }
                }

                Tambah(resto);
                Tambah(hotel);
                Tambah(hiburan);
                Tambah(parkir);
                Tambah(ppj);
                Tambah(pbb);
                Tambah(bphtb);
                Tambah(abt);
                Tambah(opsenPkb);
                Tambah(opsenBbnkb);
                // Tidak ada data reklame, jadi tidak dimasukkan

                // Bangun ViewModel
                var result = new ViewModel.DashboardChart();
                for (int i = 0; i < 12; i++)
                {
                    typeof(ViewModel.DashboardChart).GetProperty($"Target{i + 1}")?.SetValue(result, monthlyTarget);
                    typeof(ViewModel.DashboardChart).GetProperty($"Realisasi{i + 1}")?.SetValue(result, monthlyRealisasi[i]);
                }

                return result;
            }
            public static List<ViewModel.SeriesPajakDaerah> GetSeriesPajakDaerahData()
            {

                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                List<ViewModel.SeriesPajakDaerah> result = new();

                #region Now
                var targetRestoNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoNow = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelNow = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanNow = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirNow = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikNow = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                var realisasiPbbNow = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == currentYear).Sum(x => x.JumlahBayarPokok) ?? 0;

                var targetBphtbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var realisasiBphtbNow = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear).Sum(x => x.Pokok) ?? 0;

                var targetReklameNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameNow = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtNow = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var realisasiOpsenPkbNow = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                var realisasiOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);
                #endregion

                #region Mines1
                var targetRestoMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines1 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                var realisasiPbbMines1 = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == currentYear - 1).Sum(x => x.JumlahBayarPokok) ?? 0;

                var targetBphtbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var realisasiBphtbMines1 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 1).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines1 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var realisasiOpsenPkbMines1 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                var realisasiOpsenBbnkbMines1 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);
                #endregion

                #region Mines2
                var targetRestoMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines2 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                var realisasiPbbMines2 = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == currentYear - 2).Sum(x => x.JumlahBayarPokok) ?? 0;

                var targetBphtbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var realisasiBphtbMines2 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 2).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines2 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var realisasiOpsenPkbMines2 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                var realisasiOpsenBbnkbMines2 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);
                #endregion

                #region Mines3
                var targetRestoMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines3 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines3 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines3 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines3 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines3 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                //var realisasiPbbMines3 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var realisasiBphtbMines3 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 3).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines3 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines3 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var realisasiOpsenPkbMines3 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 3).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines3 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 3 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                var realisasiOpsenBbnkbMines3 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 3).Sum(x => x.JmlPokok);
                #endregion

                #region Mines4
                var targetRestoMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines4 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines4 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines4 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines4 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines4 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                //var realisasiPbbMines4 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var realisasiBphtbMines4 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 4).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines4 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines4 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var realisasiOpsenPkbMines4 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 4).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines4 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 4 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                var realisasiOpsenBbnkbMines4 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 4).Sum(x => x.JmlPokok);
                #endregion

                #region Mines5
                var targetRestoMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines5 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines5 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines5 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines5 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines5 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                //var realisasiPbbMines5 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var realisasiBphtbMines5 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 5).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines5 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines5 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 5).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var realisasiOpsenPkbMines5 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 5).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines5 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 5 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                var realisasiOpsenBbnkbMines5 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 5).Sum(x => x.JmlPokok);
                #endregion

                #region Mines6
                var targetRestoMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines6 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines6 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines6 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines6 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines6 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                //var realisasiPbbMines6 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var realisasiBphtbMines6 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 6).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines6 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines6 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 6).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var realisasiOpsenPkbMines6 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 6).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines6 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 6 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                var realisasiOpsenBbnkbMines6 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 6).Sum(x => x.JmlPokok);
                #endregion

                #region Ngisi Data
                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    Target7 = targetRestoNow,
                    Realisasi7 = realisasiRestoNow,
                    Persentase7 = targetRestoNow != 0 ? Math.Round(realisasiRestoNow / targetRestoNow * 100, 2) : 0,
                    Target6 = targetRestoMines1,
                    Realisasi6 = realisasiRestoMines1,
                    Persentase6 = targetRestoMines1 != 0 ? Math.Round(realisasiRestoMines1 / targetRestoMines1 * 100, 2) : 0,
                    Target5 = targetRestoMines2,
                    Realisasi5 = realisasiRestoMines2,
                    Persentase5 = targetRestoMines2 != 0 ? Math.Round(realisasiRestoMines2 / targetRestoMines2 * 100, 2) : 0,
                    Target4 = targetRestoMines3,
                    Realisasi4 = realisasiRestoMines3,
                    Persentase4 = targetRestoMines3 != 0 ? Math.Round(realisasiRestoMines3 / targetRestoMines3 * 100, 2) : 0,
                    Target3 = targetRestoMines4,
                    Realisasi3 = realisasiRestoMines4,
                    Persentase3 = targetRestoMines4 != 0 ? Math.Round(realisasiRestoMines4 / targetRestoMines4 * 100, 2) : 0,
                    Target2 = targetRestoMines5,
                    Realisasi2 = realisasiRestoMines5,
                    Persentase2 = targetRestoMines5 != 0 ? Math.Round(realisasiRestoMines5 / targetRestoMines5 * 100, 2) : 0,
                    Target1 = targetRestoMines6,
                    Realisasi1 = realisasiRestoMines6,
                    Persentase1 = targetRestoMines6 != 0 ? Math.Round(realisasiRestoMines6 / targetRestoMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    Target7 = targetListrikNow,
                    Realisasi7 = realisasiListrikNow,
                    Persentase7 = targetListrikNow != 0 ? Math.Round(realisasiListrikNow / targetListrikNow * 100, 2) : 0,
                    Target6 = targetListrikMines1,
                    Realisasi6 = realisasiListrikMines1,
                    Persentase6 = targetListrikMines1 != 0 ? Math.Round(realisasiListrikMines1 / targetListrikMines1 * 100, 2) : 0,
                    Target5 = targetListrikMines2,
                    Realisasi5 = realisasiListrikMines2,
                    Persentase5 = targetListrikMines2 != 0 ? Math.Round(realisasiListrikMines2 / targetListrikMines2 * 100, 2) : 0,
                    Target4 = targetListrikMines3,
                    Realisasi4 = realisasiListrikMines3,
                    Persentase4 = targetListrikMines3 != 0 ? Math.Round(realisasiListrikMines3 / targetListrikMines3 * 100, 2) : 0,
                    Target3 = targetListrikMines4,
                    Realisasi3 = realisasiListrikMines4,
                    Persentase3 = targetListrikMines4 != 0 ? Math.Round(realisasiListrikMines4 / targetListrikMines4 * 100, 2) : 0,
                    Target2 = targetListrikMines5,
                    Realisasi2 = realisasiListrikMines5,
                    Persentase2 = targetListrikMines5 != 0 ? Math.Round(realisasiListrikMines5 / targetListrikMines5 * 100, 2) : 0,
                    Target1 = targetListrikMines6,
                    Realisasi1 = realisasiListrikMines6,
                    Persentase1 = targetListrikMines6 != 0 ? Math.Round(realisasiListrikMines6 / targetListrikMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    Target7 = targetHotelNow,
                    Realisasi7 = realisasiHotelNow,
                    Persentase7 = targetHotelNow != 0 ? Math.Round(realisasiHotelNow / targetHotelNow * 100, 2) : 0,
                    Target6 = targetHotelMines1,
                    Realisasi6 = realisasiHotelMines1,
                    Persentase6 = targetHotelMines1 != 0 ? Math.Round(realisasiHotelMines1 / targetHotelMines1 * 100, 2) : 0,
                    Target5 = targetHotelMines2,
                    Realisasi5 = realisasiHotelMines2,
                    Persentase5 = targetHotelMines2 != 0 ? Math.Round(realisasiHotelMines2 / targetHotelMines2 * 100, 2) : 0,
                    Target4 = targetHotelMines3,
                    Realisasi4 = realisasiHotelMines3,
                    Persentase4 = targetHotelMines3 != 0 ? Math.Round(realisasiHotelMines3 / targetHotelMines3 * 100, 2) : 0,
                    Target3 = targetHotelMines4,
                    Realisasi3 = realisasiHotelMines4,
                    Persentase3 = targetHotelMines4 != 0 ? Math.Round(realisasiHotelMines4 / targetHotelMines4 * 100, 2) : 0,
                    Target2 = targetHotelMines5,
                    Realisasi2 = realisasiHotelMines5,
                    Persentase2 = targetHotelMines5 != 0 ? Math.Round(realisasiHotelMines5 / targetHotelMines5 * 100, 2) : 0,
                    Target1 = targetHotelMines6,
                    Realisasi1 = realisasiHotelMines6,
                    Persentase1 = targetHotelMines6 != 0 ? Math.Round(realisasiHotelMines6 / targetHotelMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    Target7 = targetParkirNow,
                    Realisasi7 = realisasiParkirNow,
                    Persentase7 = targetParkirNow != 0 ? Math.Round(realisasiParkirNow / targetParkirNow * 100, 2) : 0,
                    Target6 = targetParkirMines1,
                    Realisasi6 = realisasiParkirMines1,
                    Persentase6 = targetParkirMines1 != 0 ? Math.Round(realisasiParkirMines1 / targetParkirMines1 * 100, 2) : 0,
                    Target5 = targetParkirMines2,
                    Realisasi5 = realisasiParkirMines2,
                    Persentase5 = targetParkirMines2 != 0 ? Math.Round(realisasiParkirMines2 / targetParkirMines2 * 100, 2) : 0,
                    Target4 = targetParkirMines3,
                    Realisasi4 = realisasiParkirMines3,
                    Persentase4 = targetParkirMines3 != 0 ? Math.Round(realisasiParkirMines3 / targetParkirMines3 * 100, 2) : 0,
                    Target3 = targetParkirMines4,
                    Realisasi3 = realisasiParkirMines4,
                    Persentase3 = targetParkirMines4 != 0 ? Math.Round(realisasiParkirMines4 / targetParkirMines4 * 100, 2) : 0,
                    Target2 = targetParkirMines5,
                    Realisasi2 = realisasiParkirMines5,
                    Persentase2 = targetParkirMines5 != 0 ? Math.Round(realisasiParkirMines5 / targetParkirMines5 * 100, 2) : 0,
                    Target1 = targetParkirMines6,
                    Realisasi1 = realisasiParkirMines6,
                    Persentase1 = targetParkirMines6 != 0 ? Math.Round(realisasiParkirMines6 / targetParkirMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    Target7 = targetHiburanNow,
                    Realisasi7 = realisasiHiburanNow,
                    Persentase7 = targetHiburanNow != 0 ? Math.Round(realisasiHiburanNow / targetHiburanNow * 100, 2) : 0,
                    Target6 = targetHiburanMines1,
                    Realisasi6 = realisasiHiburanMines1,
                    Persentase6 = targetHiburanMines1 != 0 ? Math.Round(realisasiHiburanMines1 / targetHiburanMines1 * 100, 2) : 0,
                    Target5 = targetHiburanMines2,
                    Realisasi5 = realisasiHiburanMines2,
                    Persentase5 = targetHiburanMines2 != 0 ? Math.Round(realisasiHiburanMines2 / targetHiburanMines2 * 100, 2) : 0,
                    Target4 = targetHiburanMines3,
                    Realisasi4 = realisasiHiburanMines3,
                    Persentase4 = targetHiburanMines3 != 0 ? Math.Round(realisasiHiburanMines3 / targetHiburanMines3 * 100, 2) : 0,
                    Target3 = targetHiburanMines4,
                    Realisasi3 = realisasiHiburanMines4,
                    Persentase3 = targetHiburanMines4 != 0 ? Math.Round(realisasiHiburanMines4 / targetHiburanMines4 * 100, 2) : 0,
                    Target2 = targetHiburanMines5,
                    Realisasi2 = realisasiHiburanMines5,
                    Persentase2 = targetHiburanMines5 != 0 ? Math.Round(realisasiHiburanMines5 / targetHiburanMines5 * 100, 2) : 0,
                    Target1 = targetHiburanMines6,
                    Realisasi1 = realisasiHiburanMines6,
                    Persentase1 = targetHiburanMines6 != 0 ? Math.Round(realisasiHiburanMines6 / targetHiburanMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    Target7 = targetAbtNow,
                    Realisasi7 = realisasiAbtNow,
                    Persentase7 = targetAbtNow != 0 ? Math.Round(realisasiAbtNow / targetAbtNow * 100, 2) : 0,
                    Target6 = targetAbtMines1,
                    Realisasi6 = realisasiAbtMines1,
                    Persentase6 = targetAbtMines1 != 0 ? Math.Round(realisasiAbtMines1 / targetAbtMines1 * 100, 2) : 0,
                    Target5 = targetAbtMines2,
                    Realisasi5 = realisasiAbtMines2,
                    Persentase5 = targetAbtMines2 != 0 ? Math.Round(realisasiAbtMines2 / targetAbtMines2 * 100, 2) : 0,
                    Target4 = targetAbtMines3,
                    Realisasi4 = realisasiAbtMines3,
                    Persentase4 = targetAbtMines3 != 0 ? Math.Round(realisasiAbtMines3 / targetAbtMines3 * 100, 2) : 0,
                    Target3 = targetAbtMines4,
                    Realisasi3 = realisasiAbtMines4,
                    Persentase3 = targetAbtMines4 != 0 ? Math.Round(realisasiAbtMines4 / targetAbtMines4 * 100, 2) : 0,
                    Target2 = targetAbtMines5,
                    Realisasi2 = realisasiAbtMines5,
                    Persentase2 = targetAbtMines5 != 0 ? Math.Round(realisasiAbtMines5 / targetAbtMines5 * 100, 2) : 0,
                    Target1 = targetAbtMines6,
                    Realisasi1 = realisasiAbtMines6,
                    Persentase1 = targetAbtMines6 != 0 ? Math.Round(realisasiAbtMines6 / targetAbtMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    Target7 = targetReklameNow,
                    Realisasi7 = realisasiReklameNow,
                    Persentase7 = targetReklameNow != 0 ? Math.Round(realisasiReklameNow / targetReklameNow * 100, 2) : 0,
                    Target6 = targetReklameMines1,
                    Realisasi6 = realisasiReklameMines1,
                    Persentase6 = targetReklameMines1 != 0 ? Math.Round(realisasiReklameMines1 / targetReklameMines1 * 100, 2) : 0,
                    Target5 = targetReklameMines2,
                    Realisasi5 = realisasiReklameMines2,
                    Persentase5 = targetReklameMines2 != 0 ? Math.Round(realisasiReklameMines2 / targetReklameMines2 * 100, 2) : 0,
                    Target4 = targetReklameMines3,
                    Realisasi4 = realisasiReklameMines3,
                    Persentase4 = targetReklameMines3 != 0 ? Math.Round(realisasiReklameMines3 / targetReklameMines3 * 100, 2) : 0,
                    Target3 = targetReklameMines4,
                    Realisasi3 = realisasiReklameMines4,
                    Persentase3 = targetReklameMines4 != 0 ? Math.Round(realisasiReklameMines4 / targetReklameMines4 * 100, 2) : 0,
                    Target2 = targetReklameMines5,
                    Realisasi2 = realisasiReklameMines5,
                    Persentase2 = targetReklameMines5 != 0 ? Math.Round(realisasiReklameMines5 / targetReklameMines5 * 100, 2) : 0,
                    Target1 = targetReklameMines6,
                    Realisasi1 = realisasiReklameMines6,
                    Persentase1 = targetReklameMines6 != 0 ? Math.Round(realisasiReklameMines6 / targetReklameMines6 * 100, 2) : 0,
                });


                //result.Add(new ViewModel.SeriesPajakDaerah
                //{
                //    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                //    Target7 = targetPbbNow,
                //    Realisasi7 = realisasiPbbNow,
                //    Persentase7 = targetPbbNow != 0 ? Math.Round(realisasiPbbNow / targetPbbNow * 100, 2) : 0,
                //    Target6 = targetPbbMines1,
                //    Realisasi6 = realisasiPbbMines1,
                //    Persentase6 = targetPbbMines1 != 0 ? Math.Round(realisasiPbbMines1 / targetPbbMines1 * 100, 2) : 0,
                //    Target5 = targetPbbMines2,
                //    Realisasi5 = realisasiPbbMines2,
                //    Persentase5 = targetPbbMines2 != 0 ? Math.Round(realisasiPbbMines2 / targetPbbMines2 * 100, 2) : 0,
                //    Target4 = targetPbbMines3,
                //    Realisasi4 = realisasiPbbMines3,
                //    Persentase4 = targetPbbMines3 != 0 ? Math.Round(realisasiPbbMines3 / targetPbbMines3 * 100, 2) : 0,
                //    Target3 = targetPbbMines4,
                //    Realisasi3 = realisasiPbbMines4,
                //    Persentase3 = targetPbbMines4 != 0 ? Math.Round(realisasiPbbMines4 / targetPbbMines4 * 100, 2) : 0,
                //    Target2 = targetPbbMines5,
                //    Realisasi2 = realisasiPbbMines5,
                //    Persentase2 = targetPbbMines5 != 0 ? Math.Round(realisasiPbbMines5 / targetPbbMines5 * 100, 2) : 0,
                //    Target1 = targetPbbMines6,
                //    Realisasi1 = realisasiPbbMines6,
                //    Persentase1 = targetPbbMines6 != 0 ? Math.Round(realisasiPbbMines6 / targetPbbMines6 * 100, 2) : 0,
                //});

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    Target7 = targetBphtbNow,
                    Realisasi7 = realisasiBphtbNow,
                    Persentase7 = targetBphtbNow != 0 ? Math.Round(realisasiBphtbNow / targetBphtbNow * 100, 2) : 0,
                    Target6 = targetBphtbMines1,
                    Realisasi6 = realisasiBphtbMines1,
                    Persentase6 = targetBphtbMines1 != 0 ? Math.Round(realisasiBphtbMines1 / targetBphtbMines1 * 100, 2) : 0,
                    Target5 = targetBphtbMines2,
                    Realisasi5 = realisasiBphtbMines2,
                    Persentase5 = targetBphtbMines2 != 0 ? Math.Round(realisasiBphtbMines2 / targetBphtbMines2 * 100, 2) : 0,
                    Target4 = targetBphtbMines3,
                    Realisasi4 = realisasiBphtbMines3,
                    Persentase4 = targetBphtbMines3 != 0 ? Math.Round(realisasiBphtbMines3 / targetBphtbMines3 * 100, 2) : 0,
                    Target3 = targetBphtbMines4,
                    Realisasi3 = realisasiBphtbMines4,
                    Persentase3 = targetBphtbMines4 != 0 ? Math.Round(realisasiBphtbMines4 / targetBphtbMines4 * 100, 2) : 0,
                    Target2 = targetBphtbMines5,
                    Realisasi2 = realisasiBphtbMines5,
                    Persentase2 = targetBphtbMines5 != 0 ? Math.Round(realisasiBphtbMines5 / targetBphtbMines5 * 100, 2) : 0,
                    Target1 = targetBphtbMines6,
                    Realisasi1 = realisasiBphtbMines6,
                    Persentase1 = targetBphtbMines6 != 0 ? Math.Round(realisasiBphtbMines6 / targetBphtbMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    Target7 = targetOpsenPkbNow,
                    Realisasi7 = realisasiOpsenPkbNow,
                    Persentase7 = targetOpsenPkbNow != 0 ? Math.Round(realisasiOpsenPkbNow / targetOpsenPkbNow * 100, 2) : 0,
                    Target6 = targetOpsenPkbMines1,
                    Realisasi6 = realisasiOpsenPkbMines1,
                    Persentase6 = targetOpsenPkbMines1 != 0 ? Math.Round(realisasiOpsenPkbMines1 / targetOpsenPkbMines1 * 100, 2) : 0,
                    Target5 = targetOpsenPkbMines2,
                    Realisasi5 = realisasiOpsenPkbMines2,
                    Persentase5 = targetOpsenPkbMines2 != 0 ? Math.Round(realisasiOpsenPkbMines2 / targetOpsenPkbMines2 * 100, 2) : 0,
                    Target4 = targetOpsenPkbMines3,
                    Realisasi4 = realisasiOpsenPkbMines3,
                    Persentase4 = targetOpsenPkbMines3 != 0 ? Math.Round(realisasiOpsenPkbMines3 / targetOpsenPkbMines3 * 100, 2) : 0,
                    Target3 = targetOpsenPkbMines4,
                    Realisasi3 = realisasiOpsenPkbMines4,
                    Persentase3 = targetOpsenPkbMines4 != 0 ? Math.Round(realisasiOpsenPkbMines4 / targetOpsenPkbMines4 * 100, 2) : 0,
                    Target2 = targetOpsenPkbMines5,
                    Realisasi2 = realisasiOpsenPkbMines5,
                    Persentase2 = targetOpsenPkbMines5 != 0 ? Math.Round(realisasiOpsenPkbMines5 / targetOpsenPkbMines5 * 100, 2) : 0,
                    Target1 = targetOpsenPkbMines6,
                    Realisasi1 = realisasiOpsenPkbMines6,
                    Persentase1 = targetOpsenPkbMines6 != 0 ? Math.Round(realisasiOpsenPkbMines6 / targetOpsenPkbMines6 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    Target7 = targetOpsenBbnkbNow,
                    Realisasi7 = realisasiOpsenBbnkbNow,
                    Persentase7 = targetOpsenBbnkbNow != 0 ? Math.Round(realisasiOpsenBbnkbNow / targetOpsenBbnkbNow * 100, 2) : 0,
                    Target6 = targetOpsenBbnkbMines1,
                    Realisasi6 = realisasiOpsenBbnkbMines1,
                    Persentase6 = targetOpsenBbnkbMines1 != 0 ? Math.Round(realisasiOpsenBbnkbMines1 / targetOpsenBbnkbMines1 * 100, 2) : 0,
                    Target5 = targetOpsenBbnkbMines2,
                    Realisasi5 = realisasiOpsenBbnkbMines2,
                    Persentase5 = targetOpsenBbnkbMines2 != 0 ? Math.Round(realisasiOpsenBbnkbMines2 / targetOpsenBbnkbMines2 * 100, 2) : 0,
                    Target4 = targetOpsenBbnkbMines3,
                    Realisasi4 = realisasiOpsenBbnkbMines3,
                    Persentase4 = targetOpsenBbnkbMines3 != 0 ? Math.Round(realisasiOpsenBbnkbMines3 / targetOpsenBbnkbMines3 * 100, 2) : 0,
                    Target3 = targetOpsenBbnkbMines4,
                    Realisasi3 = realisasiOpsenBbnkbMines4,
                    Persentase3 = targetOpsenBbnkbMines4 != 0 ? Math.Round(realisasiOpsenBbnkbMines4 / targetOpsenBbnkbMines4 * 100, 2) : 0,
                    Target2 = targetOpsenBbnkbMines5,
                    Realisasi2 = realisasiOpsenBbnkbMines5,
                    Persentase2 = targetOpsenBbnkbMines5 != 0 ? Math.Round(realisasiOpsenBbnkbMines5 / targetOpsenBbnkbMines5 * 100, 2) : 0,
                    Target1 = targetOpsenBbnkbMines6,
                    Realisasi1 = realisasiOpsenBbnkbMines6,
                    Persentase1 = targetOpsenBbnkbMines6 != 0 ? Math.Round(realisasiOpsenBbnkbMines6 / targetOpsenBbnkbMines6 * 100, 2) : 0,
                });
                #endregion

                return result;
            }
            public static List<ViewModel.JumlahObjekPajakTahunan> GetJumlahObjekPajakTahunanData()
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                #region Method
                var OpRestoTutup = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpRestoAwal = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpRestoBaru = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && x.TglMulaiBukaOp.Year == currentYear);
                var OpRestoAkhir = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);

                var OpHotelTutup = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpHotelAwal = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpHotelBaru = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && x.TglMulaiBukaOp.Year == currentYear);
                var OpHotelAkhir = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);

                var OpHiburanTutup = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpHiburanAwal = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpHiburanBaru = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && x.TglMulaiBukaOp.Year == currentYear);
                var OpHiburanAkhir = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);

                var OpParkirTutup = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpParkirAwal = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpParkirBaru = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && x.TglMulaiBukaOp.Year == currentYear);
                var OpParkirAkhir = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);

                var OpListrikTutup = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpListrikAwal = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpListrikBaru = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && x.TglMulaiBukaOp.Year == currentYear);
                var OpListrikAkhir = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);

                var OpAbtTutup = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpAbtAwal = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpAbtBaru = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && x.TglMulaiBukaOp.Year == currentYear);
                var OpAbtAkhir = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);

                //var OpPbbTutup = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear /*&& x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear*/);
                //var OpPbbAwal = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 1 /*&& x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1*/);
                //var OpPbbBaru = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear /*&& x.TglMulaiBukaOp.Year == currentYear*/);
                //var OpPbbAkhir = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear /*&& x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear*/);

                var OpBphtbNow = context.DbMonBphtbs.Count(x => x.Tahun == currentYear);
                var OpBphtbAwal = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 1);

                var OpReklameTutup = context.DbOpReklames.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpReklameAwal = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpReklameBaru = context.DbOpReklames.Count(x => x.TahunBuku == currentYear && x.TglMulaiBukaOp.Value.Year == currentYear);
                var OpReklameAkhir = context.DbOpReklames.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);

                var OpOpsenPkbNow = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear);
                var OpOpsenPkbAwal = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 1);

                var OpOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear);
                var OpOpsenBbnkbAwal = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 1);
                #endregion

                return new List<ViewModel.JumlahObjekPajakTahunan>
                {
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                        JmlOpAwal = OpRestoAwal,
                        JmlOpTutupPermanen = OpRestoTutup,
                        JmlOpBaru = OpRestoBaru,
                        JmlOpAkhir = OpRestoAkhir
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                        JmlOpAwal = OpListrikAwal,
                        JmlOpTutupPermanen = OpListrikTutup,
                        JmlOpBaru = OpListrikBaru,
                        JmlOpAkhir = OpListrikAkhir
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                        JmlOpAwal = OpHotelAwal,
                        JmlOpTutupPermanen = OpHotelTutup,
                        JmlOpBaru = OpHotelBaru,
                        JmlOpAkhir = OpHotelAkhir
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                        JmlOpAwal = OpParkirAwal,
                        JmlOpTutupPermanen = OpParkirTutup,
                        JmlOpBaru = OpParkirBaru,
                        JmlOpAkhir = OpParkirAkhir
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                        JmlOpAwal = OpHiburanAwal,
                        JmlOpTutupPermanen = OpHiburanTutup,
                        JmlOpBaru = OpHiburanBaru,
                        JmlOpAkhir = OpHiburanAkhir
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                        JmlOpAwal = OpAbtAwal,
                        JmlOpTutupPermanen = OpAbtTutup,
                        JmlOpBaru = OpAbtBaru,
                        JmlOpAkhir = OpAbtAkhir
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                        JmlOpAwal = OpReklameAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpReklameBaru,
                        JmlOpAkhir = OpReklameAkhir
                    },
                    //new ViewModel.JumlahObjekPajakTahunan
                    //{
                    //    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    //    JmlOpAwal = OpPbbAwal,
                    //    JmlOpTutupPermanen = 0,
                    //    JmlOpBaru = OpPbbBaru,
                    //    JmlOpAkhir = OpPbbAkhir,
                    //},
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                        JmlOpAwal = OpBphtbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpBphtbNow - 0,
                        JmlOpAkhir = OpBphtbAwal - 0 + (OpBphtbNow - OpBphtbAwal)
                    },                    
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                        JmlOpAwal = OpOpsenPkbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = 0,
                        JmlOpAkhir = OpOpsenPkbNow
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                        JmlOpAwal = OpOpsenBbnkbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = 0,
                        JmlOpAkhir = OpOpsenBbnkbNow
                    },
                };
            }
            public static List<ViewModel.JumlahObjekPajakSeries> GetJumlahObjekPajakSeriesData()
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var OpRestoNow = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);
                var OpRestoMines1 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpRestoMines2 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 2 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2);
                var OpRestoMines3 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 3 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3);
                var OpRestoMines4 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 4 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4);

                var OpHotelNow = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);
                var OpHotelMines1 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpHotelMines2 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 2 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2);
                var OpHotelMines3 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 3 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3);
                var OpHotelMines4 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 4 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4);

                var OpHiburanNow = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);
                var OpHiburanMines1 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpHiburanMines2 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 2 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2);
                var OpHiburanMines3 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 3 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3);
                var OpHiburanMines4 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 4 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4);

                var OpParkirNow = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);
                var OpParkirMines1 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpParkirMines2 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 2 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2);
                var OpParkirMines3 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 3 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3);
                var OpParkirMines4 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 4 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4);

                var OpListrikNow = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);
                var OpListrikMines1 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpListrikMines2 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 2 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2);
                var OpListrikMines3 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 3 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3);
                var OpListrikMines4 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 4 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4);

                var OpAbtNow = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear);
                var OpAbtMines1 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1);
                var OpAbtMines2 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 2 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2);
                var OpAbtMines3 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 3 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3);
                var OpAbtMines4 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 4 && x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4);

                //var OpPbbNow = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear);
                //var OpPbbMines1 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 1);
                //var OpPbbMines2 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 2);
                //var OpPbbMines3 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 3);
                //var OpPbbMines4 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 4);

                var OpBphtbNow = context.DbMonBphtbs.Count(x => x.Tahun == currentYear);
                var OpBphtbMines1 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 1);
                var OpBphtbMines2 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 2);
                var OpBphtbMines3 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 3);
                var OpBphtbMines4 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 4);

                var OpReklameNow = context.DbOpReklames.Count(x => x.TahunBuku == currentYear);
                var OpReklameMines1 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 1);
                var OpReklameMines2 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 2);
                var OpReklameMines3 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 3);
                var OpReklameMines4 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 4);

                var OpOpsenPkbNow = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear);
                var OpOpsenPkbMines1 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 1);
                var OpOpsenPkbMines2 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 2);
                var OpOpsenPkbMines3 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 3);
                var OpOpsenPkbMines4 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 4);

                var OpOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear);
                var OpOpsenBbnkbMines1 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 1);
                var OpOpsenBbnkbMines2 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 2);
                var OpOpsenBbnkbMines3 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 3);
                var OpOpsenBbnkbMines4 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 4);

                var result = new List<ViewModel.JumlahObjekPajakSeries>();

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    Jumlah1 = OpRestoMines4,
                    Jumlah2 = OpRestoMines3,
                    Jumlah3 = OpRestoMines2,
                    Jumlah4 = OpRestoMines1,
                    Jumlah5 = OpRestoNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    Jumlah1 = OpListrikMines4,
                    Jumlah2 = OpListrikMines3,
                    Jumlah3 = OpListrikMines2,
                    Jumlah4 = OpListrikMines1,
                    Jumlah5 = OpListrikNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    Jumlah1 = OpHotelMines4,
                    Jumlah2 = OpHotelMines3,
                    Jumlah3 = OpHotelMines2,
                    Jumlah4 = OpHotelMines1,
                    Jumlah5 = OpHotelNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    Jumlah1 = OpParkirMines4,
                    Jumlah2 = OpParkirMines3,
                    Jumlah3 = OpParkirMines2,
                    Jumlah4 = OpParkirMines1,
                    Jumlah5 = OpParkirNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    Jumlah1 = OpHiburanMines4,
                    Jumlah2 = OpHiburanMines3,
                    Jumlah3 = OpHiburanMines2,
                    Jumlah4 = OpHiburanMines1,
                    Jumlah5 = OpHiburanNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    Jumlah1 = OpAbtMines4,
                    Jumlah2 = OpAbtMines3,
                    Jumlah3 = OpAbtMines2,
                    Jumlah4 = OpAbtMines1,
                    Jumlah5 = OpAbtNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    Jumlah1 = OpReklameMines4,
                    Jumlah2 = OpReklameMines3,
                    Jumlah3 = OpReklameMines2,
                    Jumlah4 = OpReklameMines1,
                    Jumlah5 = OpReklameNow
                });

                //result.Add(new ViewModel.JumlahObjekPajakSeries()
                //{
                //    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                //    Jumlah1 = OpPbbMines4,
                //    Jumlah2 = OpPbbMines3,
                //    Jumlah3 = OpPbbMines2,
                //    Jumlah4 = OpPbbMines1,
                //    Jumlah5 = OpPbbNow
                //});

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    Jumlah1 = OpBphtbMines4,
                    Jumlah2 = OpBphtbMines3,
                    Jumlah3 = OpBphtbMines2,
                    Jumlah4 = OpBphtbMines1,
                    Jumlah5 = OpBphtbNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    Jumlah1 = OpOpsenPkbMines4,
                    Jumlah2 = OpOpsenPkbMines3,
                    Jumlah3 = OpOpsenPkbMines2,
                    Jumlah4 = OpOpsenPkbMines1,
                    Jumlah5 = OpOpsenPkbNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    Jumlah1 = OpOpsenBbnkbMines4,
                    Jumlah2 = OpOpsenBbnkbMines3,
                    Jumlah3 = OpOpsenBbnkbMines2,
                    Jumlah4 = OpOpsenBbnkbMines1,
                    Jumlah5 = OpOpsenBbnkbNow
                });

                return result;
            }
            public static List<ViewModel.JumlahObjekPajak> GetJumlahObjekPajakData()
            {
                var context = DBClass.GetContext();

                var result = new List<ViewModel.JumlahObjekPajak>();

                var tahunList = Enumerable.Range(DateTime.Now.Year - 4, 5).Reverse().ToArray();
                var pajakList = MonPDLib.General.Extension.ToEnumList<EnumFactory.EPajak>();

                foreach (var pajak in pajakList)
                {
                    if ((EnumFactory.EPajak)pajak.Value == EnumFactory.EPajak.Semua)
                        continue;

                    var item = new ViewModel.JumlahObjekPajak
                    {
                        EnumPajak = (int)pajak.Value,
                        JenisPajak = pajak.Description,
                    };

                    for (int i = 0; i < tahunList.Length; i++)
                    {
                        var year = tahunList[i];
                        var yearBefore = year - 1;

                        int awal = 0, tutup = 0, baru = 0, akhir = 0;

                        switch ((EnumFactory.EPajak)pajak.Value)
                        {
                            case EnumFactory.EPajak.MakananMinuman:
                                awal = context.DbOpRestos.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpRestos.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpRestos.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpRestos.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.JasaPerhotelan:
                                awal = context.DbOpHotels.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpHotels.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpHotels.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpHotels.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.JasaKesenianHiburan:
                                awal = context.DbOpHiburans.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpHiburans.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpHiburans.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpHiburans.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.JasaParkir:
                                awal = context.DbOpParkirs.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpParkirs.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpParkirs.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpParkirs.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.TenagaListrik:
                                awal = context.DbOpListriks.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpListriks.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpListriks.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpListriks.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.PBB:
                                //awal = context.DbOpPbbs.Count(x => x.TahunBuku == yearBefore/* && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore)*/);
                                //tutup = context.DbOpPbbs.Count(x => x.TahunBuku == year /*&& x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year*/);
                                //baru = context.DbOpPbbs.Count(x => x.TahunBuku == year /*&& x.TglMulaiBukaOp.Year == year*/);
                                //akhir = context.DbOpPbbs.Count(x => x.TahunBuku == year /*&& (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year)*/);
                                break;

                            case EnumFactory.EPajak.AirTanah:
                                awal = context.DbOpAbts.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpAbts.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpAbts.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpAbts.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.BPHTB:
                                //awal = context.DbMonBphtbs.Count(x => x.Tahun == yearBefore);
                                //tutup = 0; // karena tidak ada data tutup di BPHTB
                                //baru = context.DbMonBphtbs.Count(x => x.Tahun == year);
                                //akhir = awal + baru;

                                awal = context.DbMonBphtbs.Count(x => x.Tahun == yearBefore && x.TglBayar.HasValue && x.TglBayar.Value.Year == yearBefore);
                                tutup = 0;
                                baru = context.DbMonBphtbs.Count(x => x.Tahun == year && x.TglBayar.HasValue && x.TglBayar.Value.Year == year);
                                akhir = awal + baru;
                                break;

                            case EnumFactory.EPajak.Reklame:
                                awal = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year - 1 &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > (year - 1))
                                );

                                tutup = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year &&
                                        x.TglOpTutup.HasValue &&
                                        x.TglOpTutup.Value.Year == year
                                );

                                baru = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year &&
                                        x.TglMulaiBukaOp.HasValue &&
                                        x.TglMulaiBukaOp.Value.Year == year
                                );

                                akhir = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year)
                                );
                                break;

                            case EnumFactory.EPajak.OpsenPkb:
                                awal = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == yearBefore);
                                tutup = 0; // Tidak ada data `TglOpTutup` di contoh pola sebelumnya
                                baru = 0; // Tidak ada data `TglMulaiBukaOp` di contoh pola sebelumnya
                                akhir = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == year);
                                break;

                            case EnumFactory.EPajak.OpsenBbnkb:
                                awal = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == yearBefore);
                                tutup = 0; // tidak ada kolom TglOpTutup di pola
                                baru = 0;  // tidak ada kolom TglMulaiBukaOp di pola
                                akhir = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == year);
                                break;
                        }

                        switch (i)
                        {
                            case 0:
                                item.Tahun1_Awal = awal;
                                item.Tahun1_Tutup = tutup;
                                item.Tahun1_Baru = baru;
                                item.Tahun1_Akhir = akhir;
                                break;
                            case 1:
                                item.Tahun2_Awal = awal;
                                item.Tahun2_Tutup = tutup;
                                item.Tahun2_Baru = baru;
                                item.Tahun2_Akhir = akhir;
                                break;
                            case 2:
                                item.Tahun3_Awal = awal;
                                item.Tahun3_Tutup = tutup;
                                item.Tahun3_Baru = baru;
                                item.Tahun3_Akhir = akhir;
                                break;
                            case 3:
                                item.Tahun4_Awal = awal;
                                item.Tahun4_Tutup = tutup;
                                item.Tahun4_Baru = baru;
                                item.Tahun4_Akhir = akhir;
                                break;
                            case 4:
                                item.Tahun5_Awal = awal;
                                item.Tahun5_Tutup = tutup;
                                item.Tahun5_Baru = baru;
                                item.Tahun5_Akhir = akhir;
                                break;
                        }
                    }

                    result.Add(item);
                }

                return result;
            }


            public static List<ViewModel.PemasanganAlatRekamSeries> GetPemasanganAlatRekamSeriesData()
            {
                var result = new List<ViewModel.PemasanganAlatRekamSeries>();
                result.Add(new ViewModel.PemasanganAlatRekamSeries()
                {
                    JenisPajak = "Hotel",
                    Jumlah1 = 950,
                    Jumlah2 = 980,
                    Jumlah3 = 1025,
                    Jumlah4 = 1080,
                    Jumlah5 = 1150,
                });
                result.Add(new ViewModel.PemasanganAlatRekamSeries()
                {
                    JenisPajak = "Resto",
                    Jumlah1 = 951,
                    Jumlah2 = 982,
                    Jumlah3 = 1035,
                    Jumlah4 = 1040,
                    Jumlah5 = 1120,
                });

                return result;
            }
            public static List<ViewModel.PemasanganAlatRekamDetail> GetPemasanganAlatRekamDetailData()
            {
                return new List<ViewModel.PemasanganAlatRekamDetail>
                {
                    new ViewModel.PemasanganAlatRekamDetail
                    {
                        JenisPajak = "Hotel",
                        JmlOp = 100,
                        JmlTerpasangTS = 30,    // Terpasang SPT
                        JmlTerpasangTB = 20,    // Terpasang Biller
                        JmlTerpasangSB = 10,    // Terpasang SPT + Biller
                        TotalTerpasang = 60,    // Total dari atas
                        TotalBelumTerpasang = 40
                    },
                    new ViewModel.PemasanganAlatRekamDetail
                    {
                        JenisPajak = "Resto",
                        JmlOp = 150,
                        JmlTerpasangTS = 50,
                        JmlTerpasangTB = 30,
                        JmlTerpasangSB = 20,
                        TotalTerpasang = 100,
                        TotalBelumTerpasang = 50
                    },
                    new ViewModel.PemasanganAlatRekamDetail
                    {
                        JenisPajak = "Parkir",
                        JmlOp = 200,
                        JmlTerpasangTS = 70,
                        JmlTerpasangTB = 50,
                        JmlTerpasangSB = 30,
                        TotalTerpasang = 150,
                        TotalBelumTerpasang = 50
                    }
                };
            }
            public static List<ViewModel.PengedokanPajak> GetPengedokanPajakData()
            {
                return new List<ViewModel.PengedokanPajak>
                {
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Hotel",
                        JmlOp = 120,
                        PotensiHasilPengedokan = 500_000_000,
                        TotalRealisasi = 450_000_000,
                        Selisih = 50_000_000
                    },
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Restoran",
                        JmlOp = 200,
                        PotensiHasilPengedokan = 800_000_000,
                        TotalRealisasi = 750_000_000,
                        Selisih = 50_000_000
                    },
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Hiburan",
                        JmlOp = 75,
                        PotensiHasilPengedokan = 300_000_000,
                        TotalRealisasi = 250_000_000,
                        Selisih = 50_000_000
                    },
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Parkir",
                        JmlOp = 50,
                        PotensiHasilPengedokan = 150_000_000,
                        TotalRealisasi = 140_000_000,
                        Selisih = 10_000_000
                    }
                };
            }
            public static List<ViewModel.PemeriksaanPajak> GetPemeriksaanPajakData()
            {
                return new List<ViewModel.PemeriksaanPajak>()
                {
                    new ViewModel.PemeriksaanPajak()
                    {
                        JenisPajak = "Restoran",
                        OpPeriksa1 = 10,
                        OpPeriksa2 = 12,
                        OpPeriksa3 = 15,
                        Pokok1 = 10000000,
                        Pokok2 = 12000000,
                        Pokok3 = 15000000,
                        Sanksi1 = 1000000,
                        Sanksi2 = 1200000,
                        Sanksi3 = 1500000,
                        Total1 = 11000000,
                        Total2 = 13200000,
                        Total3 = 16500000
                    },
                    new ViewModel.PemeriksaanPajak()
                    {
                        JenisPajak = "Hotel",
                        OpPeriksa1 = 8,
                        OpPeriksa2 = 9,
                        OpPeriksa3 = 11,
                        Pokok1 = 8000000,
                        Pokok2 = 9000000,
                        Pokok3 = 11000000,
                        Sanksi1 = 800000,
                        Sanksi2 = 900000,
                        Sanksi3 = 1100000,
                        Total1 = 8800000,
                        Total2 = 9900000,
                        Total3 = 12100000
                    },
                    new ViewModel.PemeriksaanPajak()
                    {
                        JenisPajak = "Hiburan",
                        OpPeriksa1 = 5,
                        OpPeriksa2 = 6,
                        OpPeriksa3 = 7,
                        Pokok1 = 6000000,
                        Pokok2 = 7000000,
                        Pokok3 = 7500000,
                        Sanksi1 = 600000,
                        Sanksi2 = 700000,
                        Sanksi3 = 750000,
                        Total1 = 6600000,
                        Total2 = 7700000,
                        Total3 = 8250000
                    }
                };
            }

            public static List<ViewModel.DataKontrolPotensiOp> GetDataKontrolPotensiOp()
            {
                return new List<ViewModel.DataKontrolPotensiOp>
                {
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Restoran",
                        Target1 = 100000000,
                        Realisasi1 = 85000000,
                        Selisih1 = 15000000,
                        Persentase1 = 85,
                        Target2 = 120000000,
                        Realisasi2 = 90000000,
                        Selisih2 = 30000000,
                        Persentase2 = 75
                    },
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Hotel",
                        Target1 = 200000000,
                        Realisasi1 = 180000000,
                        Selisih1 = 20000000,
                        Persentase1 = 90,
                        Target2 = 210000000,
                        Realisasi2 = 195000000,
                        Selisih2 = 15000000,
                        Persentase2 = 92.86M
                    },
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Parkir",
                        Target1 = 50000000,
                        Realisasi1 = 40000000,
                        Selisih1 = 10000000,
                        Persentase1 = 80,
                        Target2 = 60000000,
                        Realisasi2 = 50000000,
                        Selisih2 = 10000000,
                        Persentase2 = 83.33M
                    }
                };
            }
            public static List<ViewModel.DataPiutang> GetDataPiutangData()
            {
                var ret = new List<ViewModel.DataPiutang>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;


                var dataPiutangResto = context.TPiutangRestos
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangResto);

                var dataPiutangListrik = context.TPiutangListriks
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangListrik);

                var dataPiutangHotel = context.TPiutangHotels
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangHotel);

                var dataPiutangParkir = context.TPiutangParkirs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangParkir);

                var dataPiutangHiburan = context.TPiutangHiburans
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangHiburan);

                var dataPiutangAbt = context.TPiutangAbts
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangAbt);

                var dataPiutangReklame = context.TPiutangReklames
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.Reklame,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangReklame);

                var dataPiutangPbb = context.TPiutangPbbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.PBB,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangPbb);

                var dataPiutangBphtb = context.TPiutangBphtbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangBphtb);

                var dataPiutangOpsenPkb = context.TPiutangOpsenPkbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangOpsenPkb);

                var dataPiutangOpsenBbnkb = context.TPiutangOpsenBbnkbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangOpsenBbnkb);

                return ret;

            }
            public static List<ViewModel.DetailPiutang> GetDetailPiutangData(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var ret = new List<ViewModel.DetailPiutang>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataPiutangResto = context.TPiutangRestos
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangResto);
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataPiutangListrik = context.TPiutangListriks
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangListrik);
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataPiutangHotel = context.TPiutangHotels
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangHotel);
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataPiutangParkir = context.TPiutangParkirs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangParkir);
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataPiutangHiburan = context.TPiutangHiburans
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangHiburan);
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataPiutangAbt = context.TPiutangAbts
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangAbt);
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var dataPiutangReklame = context.TPiutangReklames
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangReklame);
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataPiutangPbb = context.TPiutangPbbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangPbb);
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var dataPiutangBphtb = context.TPiutangBphtbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangBphtb);
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        var dataPiutangOpsenPkb = context.TPiutangOpsenPkbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangOpsenPkb);
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        var dataPiutangOpsenBbnkb = context.TPiutangOpsenBbnkbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangOpsenBbnkb);
                        break;
                    default:
                        break;
                }

                return ret;
            }
            public static List<ViewModel.DataMutasi> GetDataMutasiData()
            {
                var ret = new List<ViewModel.DataMutasi>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var MutasiData = context.TMutasiPiutangs
                    .AsEnumerable()
                    .GroupBy(x => new { x.Mutasi, x.Urutan })
                    .OrderBy(g => g.Key.Urutan)
                    .ToList();

                ret = MutasiData.Select(g => new ViewModel.DataMutasi
                {
                    Keterangan = g.Key.Mutasi,
                    NominalMutasi1 = g.Where(x => int.Parse(x.TahunBuku) <= currentYear - 4).Sum(x => x.Nilai),
                    NominalMutasi2 = g.Where(x => int.Parse(x.TahunBuku) == currentYear - 3).Sum(x => x.Nilai),
                    NominalMutasi3 = g.Where(x => int.Parse(x.TahunBuku) == currentYear - 2).Sum(x => x.Nilai),
                    NominalMutasi4 = g.Where(x => int.Parse(x.TahunBuku) == currentYear - 1).Sum(x => x.Nilai),
                    NominalMutasi5 = g.Where(x => int.Parse(x.TahunBuku) == currentYear).Sum(x => x.Nilai),
                }).ToList();

                return ret;

            }

        }
    }
}
