using MonPDLib;
using static MonPDReborn.Models.AktivitasOP.PemasanganAlatVM;
using MonPDLib.General;
using static MonPDReborn.Models.DataOP.ProfilePembayaranOPVM;
using static MonPDReborn.Models.DashboardVM;
using MonPDLib.EF;
using static MonPDReborn.Models.DashboardVM.ViewModel;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileOPVM
    {
        public class Index
        {
            public Index()
            {

            }
        }

        public class ShowRekap
        {
            public List<RekapOP> DataRekapOPList { get; set; } = new();
            public Dashboard Data { get; set; } = new Dashboard();
            public RekapOPTotal SummaryData { get; set; } = new();

            public ShowRekap() { }

            public ShowRekap(int tahun)
            {
                DataRekapOPList = Method.GetDataRekapOPList(tahun);
                Data = Method.GetDashboardData();

                SummaryData.TotalOpAwal = DataRekapOPList.Sum(x => x.JmlOpAwal);
                SummaryData.TotalOpTutup = DataRekapOPList.Sum(x => x.JmlOpTutupPermanen);
                SummaryData.TotalOpBaru = DataRekapOPList.Sum(x => x.JmlOpBaru);
                SummaryData.TotalOpAkhir = DataRekapOPList.Sum(x => x.JmlOpAkhir);
            }
        }

        public class ShowSeries
        {
            public List<SeriesOP> DataSeriesOPList { get; set; } = new();
            public SeriesOPStatistik StatistikData { get; set; } = new();

            public ShowSeries() { }

            public ShowSeries(string keyword)
            {
                DataSeriesOPList = Method.GetDataSeriesOPList();

                // Hitung total untuk setiap tahun menggunakan LINQ
                int total2021 = DataSeriesOPList.Sum(x => x.Tahun2021);
                int total2022 = DataSeriesOPList.Sum(x => x.Tahun2022);
                int total2023 = DataSeriesOPList.Sum(x => x.Tahun2023);
                int total2024 = DataSeriesOPList.Sum(x => x.Tahun2024);
                int total2025 = DataSeriesOPList.Sum(x => x.Tahun2025);

                // Hitung selisih pertumbuhan tahunan
                var selisihPerTahun = new Dictionary<string, int>
                {
                    { "2021-2022", total2022 - total2021 },
                    { "2022-2023", total2023 - total2022 },
                    { "2023-2024", total2024 - total2023 },
                    { "2024-2025", total2025 - total2024 }
                };

                // Cari kenaikan tertinggi
                int kenaikanTertinggi = 0;
                string periodeTertinggi = "-";
                if (selisihPerTahun.Any())
                {
                    kenaikanTertinggi = selisihPerTahun.Values.Max();
                    periodeTertinggi = selisihPerTahun.FirstOrDefault(kv => kv.Value == kenaikanTertinggi).Key ?? "-";
                }

                // Masukkan semua hasil perhitungan ke dalam model StatistikData
                StatistikData = new SeriesOPStatistik
                {
                    TotalOpTahunIni = total2025,
                    PertumbuhanOp = total2025 - total2024,
                    KenaikanTertinggiOp = kenaikanTertinggi,
                    PeriodeKenaikanTertinggi = periodeTertinggi
                };
            }
        }
        public class Detail
        {
            public DataDetailOP DataDetail { get; set; } = new();
            public EnumFactory.EPajak Pajak { get; set; }
            public Detail()
            {

            }
            public Detail(string nop, EnumFactory.EPajak pajak)
            {
                Pajak = pajak;
                DataDetail = Method.GetDetailObjekPajak(nop, pajak);
            }
        }

        public class Method
        {
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    TotalPenghimbauan = 1274,
                    PenghimbauanAktif = 156,
                    PenghimbauanSelesai = 103,
                    TingkatKepatuhan = 11.7,
                };
            }

            public static List<RekapOP> GetDataRekapOPList(int tahun)
            {
                var context = DBClass.GetContext();

                #region Method
                var OpRestoTutup = context.DbOpRestos.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun);
                var OpRestoAwal = context.DbOpRestos.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1));
                var OpRestoBaru = context.DbOpRestos.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun);
                var OpRestoAkhir = context.DbOpRestos.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun));

                var OpHotelTutup = context.DbOpHotels.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun);
                var OpHotelAwal = context.DbOpHotels.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1));
                var OpHotelBaru = context.DbOpHotels.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun);
                var OpHotelAkhir = context.DbOpHotels.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun));

                var OpHiburanTutup = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun);
                var OpHiburanAwal = context.DbOpHiburans.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1));
                var OpHiburanBaru = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun);
                var OpHiburanAkhir = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun));

                var OpParkirTutup = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun);
                var OpParkirAwal = context.DbOpParkirs.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1));
                var OpParkirBaru = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun);
                var OpParkirAkhir = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun));

                var OpListrikTutup = context.DbOpListriks.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun);
                var OpListrikAwal = context.DbOpListriks.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1));
                var OpListrikBaru = context.DbOpListriks.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun);
                var OpListrikAkhir = context.DbOpListriks.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun));

                var OpAbtTutup = context.DbOpAbts.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun);
                var OpAbtAwal = context.DbOpAbts.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1));
                var OpAbtBaru = context.DbOpAbts.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun);
                var OpAbtAkhir = context.DbOpAbts.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun));

                var OpPbbTutup = context.DbOpPbbs.Count(x => x.TahunBuku == tahun /*&& x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun*/);
                var OpPbbAwal = context.DbOpPbbs.Count(x => x.TahunBuku == tahun - 1 /*&& (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1)*/);
                var OpPbbBaru = context.DbOpPbbs.Count(x => x.TahunBuku == tahun /*&& x.TglMulaiBukaOp.Year == tahun*/);
                var OpPbbAkhir = context.DbOpPbbs.Count(x => x.TahunBuku == tahun /*&& (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahu)n*/);

                var OpBphtbNow = context.DbMonBphtbs.Count(x => x.Tahun == tahun);
                var OpBphtbAwal = context.DbMonBphtbs.Count(x => x.Tahun == tahun - 1);

                var OpReklameTutup = context.DbOpReklames.Count(x => x.TahunBuku == tahun /*&& x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun*/);
                var OpReklameAwal = context.DbOpReklames.Count(x => x.TahunBuku == tahun - 1 /*&& (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1)*/);
                var OpReklameBaru = context.DbOpReklames.Count(x => x.TahunBuku == tahun /*&& x.TglMulaiBukaOp.Year == tahun*/);
                var OpReklameAkhir = context.DbOpReklames.Count(x => x.TahunBuku == tahun /*&& (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahu)n*/);

                var OpOpsenPkbNow = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == tahun);
                var OpOpsenPkbAwal = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == tahun - 1);

                var OpOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == tahun);
                var OpOpsenBbnkbAwal = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == tahun - 1);
                #endregion

                return new List<RekapOP>
                {
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                        JmlOpAwal = OpRestoAwal,
                        JmlOpTutupPermanen = OpRestoTutup,
                        JmlOpBaru = OpRestoBaru,
                        JmlOpAkhir = OpRestoAkhir,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                        JmlOpAwal = OpHotelAwal,
                        JmlOpTutupPermanen = OpHotelTutup,
                        JmlOpBaru = OpHotelBaru,
                        JmlOpAkhir = OpHotelAkhir,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                        JmlOpAwal = OpHiburanAwal,
                        JmlOpTutupPermanen = OpHiburanTutup,
                        JmlOpBaru = OpHiburanBaru,
                        JmlOpAkhir = OpHiburanAkhir,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                        JmlOpAwal = OpParkirAwal,
                        JmlOpTutupPermanen = OpParkirTutup,
                        JmlOpBaru = OpParkirBaru,
                        JmlOpAkhir = OpParkirAkhir,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                        JmlOpAwal = OpListrikAwal,
                        JmlOpTutupPermanen = OpListrikTutup,
                        JmlOpBaru = OpListrikBaru,
                        JmlOpAkhir = OpListrikAkhir,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.PBB,
                        JmlOpAwal = OpPbbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpPbbBaru,
                        JmlOpAkhir = OpPbbAkhir,
                        Tahun = tahun,
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                        JmlOpAwal = OpBphtbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpBphtbNow - 0,
                        JmlOpAkhir = OpBphtbAwal ,
                        Tahun = tahun- 0 + (OpBphtbNow - OpBphtbAwal)
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.Reklame,
                        JmlOpAwal = OpReklameAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpReklameBaru,
                        JmlOpAkhir = OpReklameAkhir,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                        JmlOpAwal = OpAbtAwal,
                        JmlOpTutupPermanen = OpAbtTutup,
                        JmlOpBaru = OpAbtBaru,
                        JmlOpAkhir = OpAbtAkhir,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                        JmlOpAwal = OpOpsenPkbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = 0,
                        JmlOpAkhir = OpOpsenPkbNow,
                        Tahun = tahun
                    },
                    new RekapOP
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                        JmlOpAwal = OpOpsenBbnkbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = 0,
                        JmlOpAkhir = OpOpsenBbnkbNow,
                        Tahun = tahun
                    },
                };
            }
            public static List<SeriesOP> GetDataSeriesOPList()
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var OpRestoNow = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpRestoMines1 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpRestoMines2 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpRestoMines3 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpRestoMines4 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpHotelNow = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpHotelMines1 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpHotelMines2 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpHotelMines3 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpHotelMines4 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpHiburanNow = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpHiburanMines1 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpHiburanMines2 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpHiburanMines3 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpHiburanMines4 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpParkirNow = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpParkirMines1 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpParkirMines2 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpParkirMines3 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpParkirMines4 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpListrikNow = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpListrikMines1 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpListrikMines2 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpListrikMines3 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpListrikMines4 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpAbtNow = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpAbtMines1 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpAbtMines2 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpAbtMines3 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpAbtMines4 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpPbbNow = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear);
                var OpPbbMines1 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 1);
                var OpPbbMines2 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 2);
                var OpPbbMines3 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 3);
                var OpPbbMines4 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 4);

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

                var result = new List<SeriesOP>();

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                    Tahun2021 = OpRestoMines4,
                    Tahun2022 = OpRestoMines3,
                    Tahun2023 = OpRestoMines2,
                    Tahun2024 = OpRestoMines1,
                    Tahun2025 = OpRestoNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                    Tahun2021 = OpHotelMines4,
                    Tahun2022 = OpHotelMines3,
                    Tahun2023 = OpHotelMines2,
                    Tahun2024 = OpHotelMines1,
                    Tahun2025 = OpHotelNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                    Tahun2021 = OpHiburanMines4,
                    Tahun2022 = OpHiburanMines3,
                    Tahun2023 = OpHiburanMines2,
                    Tahun2024 = OpHiburanMines1,
                    Tahun2025 = OpHiburanNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                    Tahun2021 = OpParkirMines4,
                    Tahun2022 = OpParkirMines3,
                    Tahun2023 = OpParkirMines2,
                    Tahun2024 = OpParkirMines1,
                    Tahun2025 = OpParkirNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                    Tahun2021 = OpListrikMines4,
                    Tahun2022 = OpListrikMines3,
                    Tahun2023 = OpListrikMines2,
                    Tahun2024 = OpListrikMines1,
                    Tahun2025 = OpListrikNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.PBB,
                    Tahun2021 = OpPbbMines4,
                    Tahun2022 = OpPbbMines3,
                    Tahun2023 = OpPbbMines2,
                    Tahun2024 = OpPbbMines1,
                    Tahun2025 = OpPbbNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                    Tahun2021 = OpBphtbMines4,
                    Tahun2022 = OpBphtbMines3,
                    Tahun2023 = OpBphtbMines2,
                    Tahun2024 = OpBphtbMines1,
                    Tahun2025 = OpBphtbNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.Reklame,
                    Tahun2021 = OpReklameMines4,
                    Tahun2022 = OpReklameMines3,
                    Tahun2023 = OpReklameMines2,
                    Tahun2024 = OpReklameMines1,
                    Tahun2025 = OpReklameNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                    Tahun2021 = OpAbtMines4,
                    Tahun2022 = OpAbtMines3,
                    Tahun2023 = OpAbtMines2,
                    Tahun2024 = OpAbtMines1,
                    Tahun2025 = OpAbtNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                    Tahun2021 = OpOpsenPkbMines4,
                    Tahun2022 = OpOpsenPkbMines3,
                    Tahun2023 = OpOpsenPkbMines2,
                    Tahun2024 = OpOpsenPkbMines1,
                    Tahun2025 = OpOpsenPkbNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                    Tahun2021 = OpOpsenBbnkbMines4,
                    Tahun2022 = OpOpsenBbnkbMines3,
                    Tahun2023 = OpOpsenBbnkbMines2,
                    Tahun2024 = OpOpsenBbnkbMines1,
                    Tahun2025 = OpOpsenBbnkbNow
                });

                return result;
            }

            #region REKAP DATA JUMLAH OP
            public static List<RekapDetail> GetRekapDetailData(EnumFactory.EPajak JenisPajak, int tahun)
            {
                var context = DBClass.GetContext();
                var ret = new List<RekapDetail>();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)JenisPajak)
                    .Select(x => new { x.Id, x.Nama })
                    .ToList();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        foreach (var kat in kategoriList)
                        {
                            var OpRestoTutup = context.DbOpRestos.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpRestoAwal = context.DbOpRestos.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpRestoBaru = context.DbOpRestos.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpRestoAkhir = context.DbOpRestos.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);


                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpRestoAwal,
                                JmlOpTutupPermanen = OpRestoTutup,
                                JmlOpBaru = OpRestoBaru,
                                JmlOpAkhir = OpRestoAkhir,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        foreach (var kat in kategoriList)
                        {
                            var OpListrikTutup = context.DbOpListriks.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpListrikAwal = context.DbOpListriks.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpListrikBaru = context.DbOpListriks.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpListrikAkhir = context.DbOpListriks.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpListrikAwal,
                                JmlOpTutupPermanen = OpListrikTutup,
                                JmlOpBaru = OpListrikBaru,
                                JmlOpAkhir = OpListrikAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        foreach (var kat in kategoriList)
                        {
                            var OpHotelTutup = context.DbOpHotels.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpHotelAwal = context.DbOpHotels.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpHotelBaru = context.DbOpHotels.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpHotelAkhir = context.DbOpHotels.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpHotelAwal,
                                JmlOpTutupPermanen = OpHotelTutup,
                                JmlOpBaru = OpHotelBaru,
                                JmlOpAkhir = OpHotelAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        foreach (var kat in kategoriList)
                        {
                            var OpParkirTutup = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpParkirAwal = context.DbOpParkirs.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpParkirBaru = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpParkirAkhir = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpParkirAwal,
                                JmlOpTutupPermanen = OpParkirTutup,
                                JmlOpBaru = OpParkirBaru,
                                JmlOpAkhir = OpParkirAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        foreach (var kat in kategoriList)
                        {
                            var OpHiburanTutup = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpHiburanAwal = context.DbOpHiburans.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpHiburanBaru = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpHiburanAkhir = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpHiburanAwal,
                                JmlOpTutupPermanen = OpHiburanTutup,
                                JmlOpBaru = OpHiburanBaru,
                                JmlOpAkhir = OpHiburanAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        foreach (var kat in kategoriList)
                        {
                            var OpAbtTutup = context.DbOpAbts.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpAbtAwal = context.DbOpAbts.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpAbtBaru = context.DbOpAbts.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpAbtAkhir = context.DbOpAbts.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpAbtAwal,
                                JmlOpTutupPermanen = OpAbtTutup,
                                JmlOpBaru = OpAbtBaru,
                                JmlOpAkhir = OpAbtAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        foreach (var kat in kategoriList)
                        {
                            var OpReklameTutup = context.DbOpReklames.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpReklameAwal = context.DbOpReklames.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpReklameBaru = context.DbOpReklames.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpReklameAkhir = context.DbOpReklames.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpReklameAwal,
                                JmlOpTutupPermanen = OpReklameTutup,
                                JmlOpBaru = OpReklameBaru,
                                JmlOpAkhir = OpReklameAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        foreach (var kat in kategoriList)
                        {
                            var OpPbbTutup = context.DbOpPbbs.Count(x => x.TahunBuku == tahun /*&& x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun*/ && x.KategoriId == kat.Id);
                            var OpPbbAwal = context.DbOpPbbs.Count(x => x.TahunBuku == tahun - 1 /*&& (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1)*/ && x.KategoriId == kat.Id);
                            var OpPbbBaru = context.DbOpPbbs.Count(x => x.TahunBuku == tahun /*&& x.TglMulaiBukaOp.Year == tahun */&& x.KategoriId == kat.Id);
                            var OpPbbAkhir = context.DbOpPbbs.Count(x => x.TahunBuku == tahun /*&& (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) */&& x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpPbbAwal,
                                JmlOpTutupPermanen = 0,
                                JmlOpBaru = OpPbbBaru,
                                JmlOpAkhir = OpPbbAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;

                return ret;

            }
            public static List<RekapMaster> GetRekapMasterData(EnumFactory.EPajak JenisPajak, int kategori, string status, int tahun)
            {
                var context = DBClass.GetContext();
                var ret = new List<RekapMaster>();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var OpRestoTutup = context.DbOpRestos.Where(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpRestoAwal = context.DbOpRestos.Where(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kategori).ToList();
                        var OpRestoBaru = context.DbOpRestos.Where(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpRestoAkhir = context.DbOpRestos.Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kategori).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpRestoAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpRestoTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpRestoBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpRestoAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var OpListrikTutup = context.DbOpListriks.Where(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpListrikAwal = context.DbOpListriks.Where(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kategori).ToList();
                        var OpListrikBaru = context.DbOpListriks.Where(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpListrikAkhir = context.DbOpListriks.Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kategori).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpListrikAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpListrikTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpListrikBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpListrikAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var OpHotelTutup = context.DbOpHotels.Where(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpHotelAwal = context.DbOpHotels.Where(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kategori).ToList();
                        var OpHotelBaru = context.DbOpHotels.Where(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpHotelAkhir = context.DbOpHotels.Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kategori).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpHotelAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpHotelTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpHotelBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpHotelAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var OpParkirTutup = context.DbOpParkirs.Where(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpParkirAwal = context.DbOpParkirs.Where(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kategori).ToList();
                        var OpParkirBaru = context.DbOpParkirs.Where(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpParkirAkhir = context.DbOpParkirs.Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kategori).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpParkirAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpParkirTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpParkirBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpParkirAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var OpHiburanTutup = context.DbOpHiburans.Where(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpHiburanAwal = context.DbOpHiburans.Where(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kategori).ToList();
                        var OpHiburanBaru = context.DbOpHiburans.Where(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpHiburanAkhir = context.DbOpHiburans.Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kategori).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpHiburanAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpHiburanTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpHiburanBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpHiburanAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var OpAbtTutup = context.DbOpAbts.Where(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpAbtAwal = context.DbOpAbts.Where(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kategori).ToList();
                        var OpAbtBaru = context.DbOpAbts.Where(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kategori).ToList();
                        var OpAbtAkhir = context.DbOpAbts.Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kategori).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpAbtAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpAbtTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpAbtBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpAbtAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var OpReklameTutup = context.DbOpReklames.Where(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun).ToList();
                        var OpReklameAwal = context.DbOpReklames.Where(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1)).ToList();
                        var OpReklameBaru = context.DbOpReklames.Where(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Value.Year == tahun).ToList();
                        var OpReklameAkhir = context.DbOpReklames.Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun)).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpReklameAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpReklameTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpReklameBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpReklameAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.PBB:

                        var OpPbbTutup = context.DbOpPbbs.Where(x => x.TahunBuku == tahun /*&& x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun*/).ToList();
                        var OpPbbAwal = context.DbOpPbbs.Where(x => x.TahunBuku == tahun - 1 /*&& x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1*/).ToList();
                        var OpPbbBaru = context.DbOpPbbs.Where(x => x.TahunBuku == tahun /*&& x.TglMulaiBukaOp.Year == tahun*/).ToList();
                        var OpPbbAkhir = context.DbOpPbbs.Where(x => x.TahunBuku == tahun /*&& x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun*/).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpPbbAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            //
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpPbbBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpPbbAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;

                return ret;
            }
            #endregion

            #region SERIES DATA JUMLAH OP
            public static List<SeriesDetail> GetSeriesDetailData(EnumFactory.EPajak JenisPajak)
            {
                var ret = new List<SeriesDetail>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)JenisPajak)
                    .Select(x => new { x.Id, x.Nama })
                    .ToList();
                var currentYear = DateTime.Now.Year;

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        foreach (var kat in kategoriList)
                        {
                            var OpRestoNow = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpRestoMines1 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpRestoMines2 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpRestoMines3 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpRestoMines4 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpRestoMines4,
                                TahunMines3 = OpRestoMines3,
                                TahunMines2 = OpRestoMines2,
                                TahunMines1 = OpRestoMines1,
                                TahunNow = OpRestoNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        foreach (var kat in kategoriList)
                        {
                            var OpListrikNow = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpListrikMines1 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpListrikMines2 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpListrikMines3 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpListrikMines4 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpListrikMines4,
                                TahunMines3 = OpListrikMines3,
                                TahunMines2 = OpListrikMines2,
                                TahunMines1 = OpListrikMines1,
                                TahunNow = OpListrikNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        foreach (var kat in kategoriList)
                        {
                            var OpHotelNow = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpHotelMines1 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpHotelMines2 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpHotelMines3 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpHotelMines4 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpHotelMines4,
                                TahunMines3 = OpHotelMines3,
                                TahunMines2 = OpHotelMines2,
                                TahunMines1 = OpHotelMines1,
                                TahunNow = OpHotelNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        foreach (var kat in kategoriList)
                        {
                            var OpParkirNow = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpParkirMines1 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpParkirMines2 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpParkirMines3 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpParkirMines4 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpParkirMines4,
                                TahunMines3 = OpParkirMines3,
                                TahunMines2 = OpParkirMines2,
                                TahunMines1 = OpParkirMines1,
                                TahunNow = OpParkirNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        foreach (var kat in kategoriList)
                        {
                            var OpHiburanNow = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpHiburanMines1 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpHiburanMines2 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpHiburanMines3 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpHiburanMines4 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpHiburanMines4,
                                TahunMines3 = OpHiburanMines3,
                                TahunMines2 = OpHiburanMines2,
                                TahunMines1 = OpHiburanMines1,
                                TahunNow = OpHiburanNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        foreach (var kat in kategoriList)
                        {
                            var OpAbtNow = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpAbtMines1 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpAbtMines2 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpAbtMines3 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpAbtMines4 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpAbtMines4,
                                TahunMines3 = OpAbtMines3,
                                TahunMines2 = OpAbtMines2,
                                TahunMines1 = OpAbtMines1,
                                TahunNow = OpAbtNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:

                        break;
                    case EnumFactory.EPajak.PBB:
                        foreach (var kat in kategoriList)
                        {
                            var OpPbbNow = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear && x.KategoriId == kat.Id);
                            var OpPbbMines1 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 1 && x.KategoriId == kat.Id);
                            var OpPbbMines2 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 2 && x.KategoriId == kat.Id);
                            var OpPbbMines3 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 3 && x.KategoriId == kat.Id);
                            var OpPbbMines4 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 4 && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpPbbMines4,
                                TahunMines3 = OpPbbMines3,
                                TahunMines2 = OpPbbMines2,
                                TahunMines1 = OpPbbMines1,
                                TahunNow = OpPbbNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        foreach (var kat in kategoriList)
                        {
                            var OpBphtbNow = context.DbMonBphtbs.Count(x => x.Tahun == currentYear);
                            var OpBphtbMines1 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 1);
                            var OpBphtbMines2 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 2);
                            var OpBphtbMines3 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 3);
                            var OpBphtbMines4 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 4);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpBphtbMines4,
                                TahunMines3 = OpBphtbMines3,
                                TahunMines2 = OpBphtbMines2,
                                TahunMines1 = OpBphtbMines1,
                                TahunNow = OpBphtbNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;

                return ret;

            }
            public static List<SeriesMaster> GetSeriesMasterData(EnumFactory.EPajak JenisPajak, int kategori, string tahunHuruf)
            {
                var ret = new List<SeriesMaster>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var OpRestoNow = context.DbOpRestos.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpRestoMines1 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpRestoMines2 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpRestoMines3 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpRestoMines4 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpRestoMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpRestoMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpRestoMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpRestoMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpRestoNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var OpListrikNow = context.DbOpListriks.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpListrikMines1 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpListrikMines2 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpListrikMines3 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpListrikMines4 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpListrikMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpListrikMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpListrikMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpListrikMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpListrikNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var OpHotelNow = context.DbOpHotels.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpHotelMines1 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpHotelMines2 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpHotelMines3 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpHotelMines4 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpHotelMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpHotelMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpHotelMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpHotelMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpHotelNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var OpParkirNow = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpParkirMines1 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpParkirMines2 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpParkirMines3 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpParkirMines4 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpParkirMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpParkirMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpParkirMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpParkirMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpParkirNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var OpHiburanNow = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines1 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines2 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines3 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines4 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpHiburanMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpHiburanMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpHiburanMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpHiburanMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpHiburanNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var OpAbtNow = context.DbOpAbts.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpAbtMines1 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpAbtMines2 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpAbtMines3 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpAbtMines4 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpAbtMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpAbtMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpAbtMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpAbtMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpAbtNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        var OpPbbNow = context.DbOpPbbs.Where(x => x.TahunBuku == currentYear && x.KategoriId == kategori).ToList();
                        var OpPbbMines1 = context.DbOpPbbs.Where(x => x.TahunBuku == currentYear - 1 && x.KategoriId == kategori).ToList();
                        var OpPbbMines2 = context.DbOpPbbs.Where(x => x.TahunBuku == currentYear - 2 && x.KategoriId == kategori).ToList();
                        var OpPbbMines3 = context.DbOpPbbs.Where(x => x.TahunBuku == currentYear - 3 && x.KategoriId == kategori).ToList();
                        var OpPbbMines4 = context.DbOpPbbs.Where(x => x.TahunBuku == currentYear - 4 && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpPbbMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpPbbMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpPbbMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpPbbMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpPbbNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var OpBphtbNow = context.DbMonBphtbs.Where(x => x.Tahun == currentYear).ToList();
                        var OpBphtbMines1 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 1).ToList();
                        var OpBphtbMines2 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 2).ToList();
                        var OpBphtbMines3 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 3).ToList();
                        var OpBphtbMines4 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 4).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpBphtbMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpBphtbMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpBphtbMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpBphtbMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpBphtbNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;

                return ret;

            }
            #endregion

            public static DataDetailOP GetDetailObjekPajak(string nop, EnumFactory.EPajak pajak)
            {
                var context = DBClass.GetContext();
                var ret = new DataDetailOP();
                switch (pajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var opResto = context.DbOpRestos.FirstOrDefault(x => x.Nop == nop);
                        if (opResto != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opResto.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opResto.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opResto.NpwpdNama;
                            ret.IdentitasPajak.NOP = opResto.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opResto.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opResto.AlamatOp;
                            ret.IdentitasPajak.Telepon = opResto.Telp;
                            ret.IdentitasPajak.TanggalBuka = opResto.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opResto.KategoriNama;
                            //isi data resto
                            ret.RestoRow.PendapatanRow = new DetailResto.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };
                            ret.RestoRow.SaranaRestoPendukungRow = new DetailResto.SaranaPendukung
                            {
                                JumlahKaryawan = (int)opResto.JumlahKaryawan,
                                MetodePembayaran = opResto.MetodePembayaran,
                                MetodePenjualan = opResto.MetodePenjualan
                            };
                        }
                        break;

                    case EnumFactory.EPajak.TenagaListrik:
                        var opListrik = context.DbOpListriks.FirstOrDefault(x => x.Nop == nop);
                        break;

                    case EnumFactory.EPajak.JasaPerhotelan:
                        var opHotel = context.DbOpHotels.FirstOrDefault(x => x.Nop == nop);
                        if (opHotel != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opHotel.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opHotel.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opHotel.NpwpdNama;
                            ret.IdentitasPajak.NOP = opHotel.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opHotel.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opHotel.AlamatOp;
                            ret.IdentitasPajak.Telepon = opHotel.Telp;
                            ret.IdentitasPajak.TanggalBuka = opHotel.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opHotel.KategoriNama;
                            //isi data hotel
                            ret.HotelRow.PendapatanRow = new DetailHotel.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };
                            ret.HotelRow.SaranaHotelPendukungRow = new DetailHotel.SaranaPendukung
                            {
                                JumlahKaryawan = (int)opHotel.JumlahKaryawan,
                                MetodePembayaran = opHotel.MetodePembayaran,
                                MetodePenjualan = opHotel.MetodePenjualan
                            };
                            //ret.HotelRow.BanquetHotelDetailList = context.DbOpBanquets
                            //    .Where(x => x.Nop == nop)
                            //    .Select(x => new DetailHotel.DetailBanquet
                            //    {
                            //        Nama = x.NamaBanquet,
                            //        Jumlah = (int)x.JumlahBanquet,
                            //        JenisBanquet = (int)x.JenisBanquet,
                            //        Kapasitas = (int)x.KapasitasBanquet,
                            //        HargaSewa = (int)x.HargaSewaBanquet,
                            //        HargaPaket = (int)x.HargaPaketBanquet,
                            //        Okupansi = (int)x.OkupansiBanquet
                            //    }).ToList();
                        }
                        break;

                    case EnumFactory.EPajak.JasaParkir:
                        var opParkir = context.DbOpParkirs.FirstOrDefault(x => x.Nop == nop);
                        if (opParkir != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opParkir.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opParkir.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opParkir.NpwpdNama;
                            ret.IdentitasPajak.NOP = opParkir.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opParkir.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opParkir.AlamatOp;
                            ret.IdentitasPajak.Telepon = opParkir.Telp;
                            ret.IdentitasPajak.TanggalBuka = opParkir.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opParkir.KategoriNama;
                        }
                        break;

                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var opHiburan = context.DbOpHiburans.FirstOrDefault(x => x.Nop == nop);
                        if (opHiburan != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opHiburan.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opHiburan.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opHiburan.NpwpdNama;
                            ret.IdentitasPajak.NOP = opHiburan.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opHiburan.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opHiburan.AlamatOp;
                            ret.IdentitasPajak.Telepon = opHiburan.Telp;
                            ret.IdentitasPajak.TanggalBuka = opHiburan.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opHiburan.KategoriNama;
                        }
                        break;

                    case EnumFactory.EPajak.AirTanah:
                        var opAbt = context.DbOpAbts.FirstOrDefault(x => x.Nop == nop);
                        if (opAbt != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opAbt.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opAbt.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opAbt.NpwpdNama;
                            ret.IdentitasPajak.NOP = opAbt.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opAbt.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opAbt.AlamatOp;
                            ret.IdentitasPajak.Telepon = opAbt.Telp;
                            ret.IdentitasPajak.TanggalBuka = opAbt.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opAbt.KategoriNama;

                            ret.AbtRow.PendapatanRow = new DetailAbt.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };

                            ret.AbtRow.SaranaAbtPendukungRow = new DetailAbt.SaranaPendukung
                            {
                                KelompokNama = opAbt.NamaKelompok,
                            };

                        }
                        break;

                    case EnumFactory.EPajak.Reklame:
                        var opReklame = context.DbOpReklames.FirstOrDefault(x => x.Nop == nop);
                        break;

                    case EnumFactory.EPajak.PBB:
                        var opPbb = context.DbOpPbbs.FirstOrDefault(x => x.Nop == nop);
                        break;

                    case EnumFactory.EPajak.BPHTB:
                        // var opBphtb = context.DbOpBphtbs.FirstOrDefault(x => x.Nop == nop);
                        break;

                    case EnumFactory.EPajak.OpsenPkb:
                        // var opOpsenPkb = context.DbOpOpsenPkb.FirstOrDefault(x => x.Nop == nop);
                        break;

                    case EnumFactory.EPajak.OpsenBbnkb:
                        // var opOpsenBbnkb = context.DbOpOpsenBbnkb.FirstOrDefault(x => x.Nop == nop);
                        break;

                    default:
                        break;
                }

                return ret;
            }
        }

        public class RekapOP
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }
            public int JmlOpAwal { get; set; }
            public int JmlOpTutupPermanen { get; set; }
            public int JmlOpBaru { get; set; }
            public int JmlOpAkhir { get; set; }
        }
        public class RekapOPTotal
        {
            public int TotalOpAwal { get; set; }
            public int TotalOpTutup { get; set; }
            public int TotalOpBaru { get; set; }
            public int TotalOpAkhir { get; set; }
        }

        public class SeriesOP
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun2021 { get; set; }
            public int Tahun2022 { get; set; }
            public int Tahun2023 { get; set; }
            public int Tahun2024 { get; set; }
            public int Tahun2025 { get; set; }
        }

        // Di dalam ProfileOPVM.cs
        public class SeriesOPStatistik
        {
            public int TotalOpTahunIni { get; set; }
            public int PertumbuhanOp { get; set; }
            public int KenaikanTertinggiOp { get; set; }
            public string PeriodeKenaikanTertinggi { get; set; } = "-";
        }

        public class Dashboard
        {
            public int TotalPenghimbauan { get; set; }
            public double PenghimbauanAktif { get; set; }
            public double PenghimbauanSelesai { get; set; }
            public double TingkatKepatuhan { get; set; }

        }

        public class DataDetailOP
        {
            public IdentitasObjekPajak IdentitasPajak { get; set; } = new();
            public DataPerizinan Perizinan { get; set; } = new();
            public DetailHotel HotelRow { get; set; } = new();
            public DetailResto RestoRow { get; set; } = new();
            public DetailAbt AbtRow { get; set; } = new();

        }

        public class RekapDetail
        {
            public int EnumPajak { get; set; }

            public string JenisPajak { get; set; } = null!;
            public int KategoriId { get; set; }
            public string Kategori { get; set; } = null!;
            public int Tahun { get; set; }
            public int JmlOpAwal { get; set; }
            public int JmlOpTutupPermanen { get; set; }
            public int JmlOpBaru { get; set; }
            public int JmlOpAkhir { get; set; }
        }

        public class RekapMaster
        {
            public int EnumPajak { get; set; }
            public int Kategori_Id { get; set; }
            public string Kategori_Nama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOP { get; set; } = null!;
            public string Wilayah { get; set; } = null!;

        }

        public class SeriesDetail
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int KategoriId { get; set; }
            public string Kategori { get; set; } = null!;
            public int TahunMines4 { get; set; }
            public int TahunMines3 { get; set; }
            public int TahunMines2 { get; set; }
            public int TahunMines1 { get; set; }
            public int TahunNow { get; set; }
        }

        public class SeriesMaster
        {
            public int EnumPajak { get; set; }
            public int Kategori_Id { get; set; }
            public string Kategori_Nama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOP { get; set; } = null!;
            public string Wilayah { get; set; } = null!;

        }

        public class IdentitasObjekPajak
        {
            public string NpwpdNo { get; set; }
            public string NamaNpwpd { get; set; }
            public string NamaObjekPajak { get; set; }
            public string AlamatLengkap { get; set; }
            public string WilayahPajak { get; set; }
            public string NOP { get; set; }
            public string Telepon { get; set; }
            public DateTime TanggalBuka { get; set; }
            public EnumFactory.EPajak EnumPajak { get; set; }
            public string JenisPajak { get; set; }
            public string KategoriPajak { get; set; }
        }
        public class DataPerizinan
        {
            public string NomorIMB { get; set; }
            public DateTime TanggalIMB { get; set; }
            public string NomorSITU_NIB { get; set; }
            public string NomorIzinOperasional { get; set; }
        }
        //DETAIL OP HOTEL
        public class DetailHotel
        {
            public Pendapatan PendapatanRow { get; set; } = new();
            public SaranaPendukung SaranaHotelPendukungRow { get; set; } = new();
            public List<DetailBanquet> BanquetHotelDetailList { get; set; } = new();
            public List<DetailFasilitas> FasilitasHotelDetailList { get; set; } = new();
            public List<DetailKamar> KamarHotelDetailList { get; set; } = new();

            public class Pendapatan
            {
                public string Okupansi { get; set; }
                public decimal RataTarifKamar { get; set; }
                public decimal PendapatanKotor { get; set; }
                public string JumlahTransaksi { get; set; }
            }
            public class SaranaPendukung
            {
                public int JumlahKaryawan { get; set; }
                public string MetodePembayaran { get; set; }
                public string MetodePenjualan { get; set; }
            }
            public class DetailBanquet
            {
                public string Nama { get; set; }
                public int Jumlah { get; set; }
                public int JenisBanquet { get; set; }
                public int Kapasitas { get; set; }
                public int HargaSewa { get; set; }
                public int HargaPaket { get; set; }
                public int Okupansi { get; set; }
            }
            public class DetailFasilitas
            {
                public string Nama { get; set; }
                public int Jumlah { get; set; }
                public int Kapasitas { get; set; }
            }
            public class DetailKamar
            {
                public string Kamar { get; set; }
                public int Jumlah { get; set; }
                public int Tarif { get; set; }
            }
        }
        //DETAIL OP RESTO
        public class DetailResto
        {
            public Pendapatan PendapatanRow { get; set; } = new();
            public SaranaPendukung SaranaRestoPendukungRow { get; set; } = new();
            public List<DetailOperasional> OperasionalRestoDetailList { get; set; } = new();
            public List<DetailFasilitas> FasilitasRestoDetailList { get; set; } = new();
            public DetailKapasitas KapasitasRestoDetailRow { get; set; } = new();

            public class Pendapatan
            {

            }
            public class SaranaPendukung
            {
                public int JumlahKaryawan { get; set; }
                public string MetodePembayaran { get; set; }
                public string MetodePenjualan { get; set; }
            }
            public class DetailOperasional
            {
                public string Hari { get; set; }
                public DateTime JamBuka { get; set; }
                public DateTime JamTutup { get; set; }

            }
            public class DetailFasilitas
            {
                public string Nama { get; set; }
                public int Jumlah { get; set; }
                public int Kapasitas { get; set; }
            }
            public class DetailKapasitas
            {
                public int JumlahKursi { get; set; }
                public int JumlahMeja { get; set; }
                public int KapasitasRuangan { get; set; }
            }
        }
        //DETAIL ABT
        public class DetailAbt
        {
            public Pendapatan PendapatanRow { get; set; } = new();
            public SaranaPendukung SaranaAbtPendukungRow { get; set; } = new();

            public class Pendapatan
            {

            }
            public class SaranaPendukung
            {
                public string KelompokNama { get; set; }
            }
        }
    }
}
