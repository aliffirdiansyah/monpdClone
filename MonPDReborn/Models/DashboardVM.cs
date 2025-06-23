namespace MonPDReborn.Models
{
    public class DashboardVM
    {
        public class Index
        {
            public ViewModel.Dashboard Data { get; set; } = new ViewModel.Dashboard();
            public ViewModel.DashboardChart ChartData { get; set; } = new ViewModel.DashboardChart();
            public Index()
            {
                Data = Method.GetDashboardData();
                ChartData = Method.GetDashboardChartData();
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
        public class DataMutasi
        {
            public List<ViewModel.DataMutasi> Data { get; set; } = new List<ViewModel.DataMutasi>();
            public DataMutasi()
            {
                Data = Method.GetDataMutasiData();
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
                public decimal Realisasi1 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Realisasi3 { get; set; }
                public decimal Realisasi4 { get; set; }
                public decimal Realisasi5 { get; set; }
                public decimal Persentase1 { get; set; }
                public decimal Persentase2 { get; set; }
                public decimal Persentase3 { get; set; }
                public decimal Persentase4 { get; set; }
                public decimal Persentase5 { get; set; }
            }
            public class JumlahObjekPajakTahunan
            {
                public string JenisPajak { get; set; } = "";
                public decimal JmlOpAwal { get; set; }
                public decimal JmlOpTutupSementara { get; set; }
                public decimal JmlOpTutupPermanen { get; set; }
                public decimal JmlOpBaru { get; set; }
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
            public class PemasanganAlatRekamSeries
            {
                public int Tahun { get; set; }
                public int JmlTerpasang { get; set; }
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
                public List<PemeriksaanPajakDetail> Details { get; set; } = new List<PemeriksaanPajakDetail>();
            }
            public class PemeriksaanPajakDetail
            {
                public int Tahun { get; set; }
                public decimal JumlahOpDiperiksa { get; set; }
                public decimal JumlahPokok { get; set; }
                public decimal JumlahSanksi { get; set; }
                public decimal JumlahTotal { get; set; }
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
                public int Tahun { get; set; }
                public decimal Target { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Selisih { get; set; }
                public decimal Persentase { get; set; }
            }
            public class DataPiutang
            {
                public string JenisPajak { get; set; } = "";
                public int Tahun { get; set; }
                public decimal NominalPiutang { get; set; }
            }
            public class DataMutasi
            {
                public string Keterangan { get; set; } = "";
                public int Tahun { get; set; }
                public decimal NominalMutasi { get; set; }
            }
        }

        public class Method
        {
            public static ViewModel.Dashboard GetDashboardData()
            {
                var result = new ViewModel.Dashboard
                {
                    TotalTarget = 1000000000,
                    TotalRealisasi = 850000000,
                    TotalPersentase = 85,

                    TargetHotel = 200000000,
                    RealisasiHotel = 180000000,
                    PersentaseHotel = 90,

                    TargetHiburan = 150000000,
                    RealisasiHiburan = 120000000,
                    PersentaseHiburan = 80,

                    TargetParkir = 50000000,
                    RealisasiParkir = 40000000,
                    PersentaseParkir = 80,

                    TargetMamin = 250000000,
                    RealisasiMamin = 230000000,
                    PersentaseMamin = 92,

                    TargetListrik = 50000000,
                    RealisasiListrik = 45000000,
                    PersentaseListrik = 90,

                    TargetAbt = 30000000,
                    RealisasiAbt = 25000000,
                    PersentaseAbt = 83.33m,

                    TargetPbb = 100000000,
                    RealisasiPbb = 85000000,
                    PersentasePbb = 85,

                    TargetReklame = 40000000,
                    RealisasiReklame = 30000000,
                    PersentaseReklame = 75,

                    TargetBphtb = 50000000,
                    RealisasiBphtb = 47000000,
                    PersentaseBphtb = 94,

                    TargetOpsenPkb = 30000000,
                    RealisasiOpsenPkb = 28000000,
                    PersentaseOpsenPkb = 93.33m,

                    TargetOpsenBbnkb = 20000000,
                    RealisasiOpsenBbnkb = 18000000,
                    PersentaseOpsenBbnkb = 90
                };

                return result;
            }
            public static ViewModel.DashboardChart GetDashboardChartData()
            {
                var result = new ViewModel.DashboardChart
                {
                    Target1 = 100000000,
                    Target2 = 100000000,
                    Target3 = 100000000,
                    Target4 = 100000000,
                    Target5 = 100000000,
                    Target6 = 100000000,
                    Target7 = 100000000,
                    Target8 = 100000000,
                    Target9 = 100000000,
                    Target10 = 100000000,
                    Target11 = 100000000,
                    Target12 = 100000000,

                    Realisasi1 = 85000000,
                    Realisasi2 = 82000000,
                    Realisasi3 = 90000000,
                    Realisasi4 = 88000000,
                    Realisasi5 = 80000000,
                    Realisasi6 = 95000000,
                    Realisasi7 = 91000000,
                    Realisasi8 = 87000000,
                    Realisasi9 = 83000000,
                    Realisasi10 = 89000000,
                    Realisasi11 = 92000000,
                    Realisasi12 = 90000000
                };

                return result;
            }
            public static List<ViewModel.SeriesPajakDaerah> GetSeriesPajakDaerahData()
            {
                return new List<ViewModel.SeriesPajakDaerah>
                {
                    new ViewModel.SeriesPajakDaerah
                    {
                        JenisPajak = "Pajak Restoran",
                        Target1 = 80000000,
                        Target2 = 100000000,
                        Target3 = 110000000,
                        Target4 = 130000000,
                        Target5 = 150000000,
                        Realisasi1 = 70000000,
                        Realisasi2 = 90000000,
                        Realisasi3 = 100000000,
                        Realisasi4 = 110000000,
                        Realisasi5 = 120000000,
                        Persentase1 = Math.Round(70000000m / 80000000m * 100, 2),
                        Persentase2 = Math.Round(90000000m / 100000000m * 100, 2),
                        Persentase3 = Math.Round(100000000m / 110000000m * 100, 2),
                        Persentase4 = Math.Round(110000000m / 130000000m * 100, 2),
                        Persentase5 = Math.Round(120000000m / 150000000m * 10, 2)
                    },
                    new ViewModel.SeriesPajakDaerah
                    {
                        JenisPajak = "Pajak Hotel",
                        Target1 = 80000000,
                        Target2 = 100000000,
                        Target3 = 110000000,
                        Target4 = 130000000,
                        Target5 = 150000000,
                        Realisasi1 = 70000000,
                        Realisasi2 = 90000000,
                        Realisasi3 = 100000000,
                        Realisasi4 = 110000000,
                        Realisasi5 = 120000000,
                        Persentase1 = Math.Round(70000000m / 80000000m * 100, 2),
                        Persentase2 = Math.Round(90000000m / 100000000m * 100, 2),
                        Persentase3 = Math.Round(100000000m / 110000000m * 100, 2),
                        Persentase4 = Math.Round(110000000m / 130000000m * 100, 2),
                        Persentase5 = Math.Round(120000000m / 150000000m * 100, 2)
                    },
                };
            }
            public static List<ViewModel.JumlahObjekPajakTahunan> GetJumlahObjekPajakTahunanData()
            {
                return new List<ViewModel.JumlahObjekPajakTahunan>
                {
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Hotel",
                        JmlOpAwal = 120,
                        JmlOpTutupSementara = 5,
                        JmlOpTutupPermanen = 3,
                        JmlOpBaru = 10
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Restoran",
                        JmlOpAwal = 250,
                        JmlOpTutupSementara = 12,
                        JmlOpTutupPermanen = 8,
                        JmlOpBaru = 25
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Parkir",
                        JmlOpAwal = 80,
                        JmlOpTutupSementara = 2,
                        JmlOpTutupPermanen = 1,
                        JmlOpBaru = 5
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Hiburan",
                        JmlOpAwal = 60,
                        JmlOpTutupSementara = 3,
                        JmlOpTutupPermanen = 2,
                        JmlOpBaru = 7
                    }
                    // Tambahkan jenis pajak lain jika diperlukan
                };
            }
            public static List<ViewModel.JumlahObjekPajakSeries> GetJumlahObjekPajakSeriesData()
            {
                var result = new List<ViewModel.JumlahObjekPajakSeries>();
                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = "Hotel",
                    Jumlah1 = 950,
                    Jumlah2 = 980,
                    Jumlah3 = 1025,
                    Jumlah4 = 1080,
                    Jumlah5 = 1150,
                });
                result.Add(new ViewModel.JumlahObjekPajakSeries()
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
            public static List<ViewModel.PemasanganAlatRekamSeries> GetPemasanganAlatRekamSeriesData()
            {
                return new List<ViewModel.PemasanganAlatRekamSeries>
                {
                    new ViewModel.PemasanganAlatRekamSeries
                    {
                        Tahun = 2020,
                        JmlTerpasang = 120
                    },
                    new ViewModel.PemasanganAlatRekamSeries
                    {
                        Tahun = 2021,
                        JmlTerpasang = 150
                    },
                    new ViewModel.PemasanganAlatRekamSeries
                    {
                        Tahun = 2022,
                        JmlTerpasang = 180
                    },
                    new ViewModel.PemasanganAlatRekamSeries
                    {
                        Tahun = 2023,
                        JmlTerpasang = 210
                    },
                    new ViewModel.PemasanganAlatRekamSeries
                    {
                        Tahun = 2024,
                        JmlTerpasang = 250
                    }
                };
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
                return new List<ViewModel.PemeriksaanPajak>
                {
                    new ViewModel.PemeriksaanPajak
                    {
                        JenisPajak = "Hotel",
                        Details = new List<ViewModel.PemeriksaanPajakDetail>
                        {
                            new ViewModel.PemeriksaanPajakDetail
                            {
                                Tahun = 2022,
                                JumlahOpDiperiksa = 15,
                                JumlahPokok = 200_000_000,
                                JumlahSanksi = 20_000_000,
                                JumlahTotal = 220_000_000
                            },
                            new ViewModel.PemeriksaanPajakDetail
                            {
                                Tahun = 2023,
                                JumlahOpDiperiksa = 18,
                                JumlahPokok = 240_000_000,
                                JumlahSanksi = 25_000_000,
                                JumlahTotal = 265_000_000
                            }
                        }
                    },
                    new ViewModel.PemeriksaanPajak
                    {
                        JenisPajak = "Restoran",
                        Details = new List<ViewModel.PemeriksaanPajakDetail>
                        {
                            new ViewModel.PemeriksaanPajakDetail
                            {
                                Tahun = 2022,
                                JumlahOpDiperiksa = 20,
                                JumlahPokok = 300_000_000,
                                JumlahSanksi = 30_000_000,
                                JumlahTotal = 330_000_000
                            },
                            new ViewModel.PemeriksaanPajakDetail
                            {
                                Tahun = 2023,
                                JumlahOpDiperiksa = 22,
                                JumlahPokok = 350_000_000,
                                JumlahSanksi = 40_000_000,
                                JumlahTotal = 390_000_000
                            }
                        }
                    },
                    new ViewModel.PemeriksaanPajak
                    {
                        JenisPajak = "Parkir",
                        Details = new List<ViewModel.PemeriksaanPajakDetail>
                        {
                            new ViewModel.PemeriksaanPajakDetail
                            {
                                Tahun = 2023,
                                JumlahOpDiperiksa = 10,
                                JumlahPokok = 100_000_000,
                                JumlahSanksi = 10_000_000,
                                JumlahTotal = 110_000_000
                            }
                        }
                    }
                };
            }
            public static List<ViewModel.DataKontrolPotensiOp> GetDataKontrolPotensiOp()
            {
                return new List<ViewModel.DataKontrolPotensiOp>
                {
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Hotel",
                        Tahun = 2024,
                        Target = 500_000_000,
                        Realisasi = 450_000_000,
                        Selisih = 50_000_000,
                        Persentase = 90
                    },
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Restoran",
                        Tahun = 2024,
                        Target = 800_000_000,
                        Realisasi = 760_000_000,
                        Selisih = 40_000_000,
                        Persentase = 95
                    },
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Hiburan",
                        Tahun = 2024,
                        Target = 300_000_000,
                        Realisasi = 240_000_000,
                        Selisih = 60_000_000,
                        Persentase = 80
                    },
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Parkir",
                        Tahun = 2024,
                        Target = 200_000_000,
                        Realisasi = 180_000_000,
                        Selisih = 20_000_000,
                        Persentase = 90
                    }
                };
            }
            public static List<ViewModel.DataPiutang> GetDataPiutangData()
            {
                return new List<ViewModel.DataPiutang>
                {
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Hotel",
                        Tahun = 2022,
                        NominalPiutang = 75_000_000
                    },
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Hotel",
                        Tahun = 2023,
                        NominalPiutang = 60_000_000
                    },
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Restoran",
                        Tahun = 2022,
                        NominalPiutang = 120_000_000
                    },
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Restoran",
                        Tahun = 2023,
                        NominalPiutang = 100_000_000
                    },
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Parkir",
                        Tahun = 2023,
                        NominalPiutang = 30_000_000
                    },
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Hiburan",
                        Tahun = 2022,
                        NominalPiutang = 45_000_000
                    }
                };
            }
            public static List<ViewModel.DataMutasi> GetDataMutasiData()
            {
                return new List<ViewModel.DataMutasi>
                {
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Penambahan OP Baru",
                        Tahun = 2022,
                        NominalMutasi = 150_000_000
                    },
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Penghapusan OP Tutup Permanen",
                        Tahun = 2022,
                        NominalMutasi = -50_000_000
                    },
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Perubahan Tarif Pajak",
                        Tahun = 2023,
                        NominalMutasi = 80_000_000
                    },
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Pemutakhiran Data",
                        Tahun = 2023,
                        NominalMutasi = 30_000_000
                    },
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Koreksi Kesalahan Data",
                        Tahun = 2024,
                        NominalMutasi = -20_000_000
                    },
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Penambahan OP Baru",
                        Tahun = 2024,
                        NominalMutasi = 100_000_000
                    }
                };
            }
        }
    }
}
