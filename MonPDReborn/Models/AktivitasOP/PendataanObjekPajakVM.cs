using MonPDLib;
using MonPDLib.General;
using System.Linq;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PendataanObjekPajakVM
    {
        public class Index
        {
            public string Keyword { get; set; } = string.Empty;
            public Dashboard Data { get; set; } = new Dashboard();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }

        public class Show
        {
            public List<DataPendataan> DataPendataanList { get; set; } = new();
            public Show()
            {
                DataPendataanList = Method.GetDataPendataanList();
            }
        }

        public class Detail
        {
            public List<DataDetailPendataan> DataDetailList { get; set; } = new();

            public Detail() { }

            public Detail(EnumFactory.EPajak jenisPajak, int tahun)
            {
                DataDetailList = Method.GetDetailByJenisPajakAndTahun(jenisPajak, tahun);
            }
        }

        public class SubDetail
        {
            public List<SubDetailRestoran> DataRestoranList { get; set; } = new();
            public List<SubDetailParkir> DataParkirList { get; set; } = new();
            public SubDetail() { }
            public SubDetail(EnumFactory.EPajak jenisPajak, string nop)
            {
                if (jenisPajak == EnumFactory.EPajak.MakananMinuman)
                {
                    DataRestoranList = Method.GetSubDetailRestoran(jenisPajak, nop);
                }
                else if (jenisPajak == EnumFactory.EPajak.JasaParkir)
                {
                    DataParkirList = Method.GetSubDetailParkir(jenisPajak, nop);
                }
            }
        }

        public class DataPendataan
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string Nop { get; set; } = string.Empty;
            public string JenisPajak { get; set; } = string.Empty;
            public decimal JumlahOp { get; set; }
            public decimal Potensi { get; set; }
            public decimal TotalRealisasi { get; set; }
            public decimal Selisih { get; set; }
        }

        public class DataDetailPendataan
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = string.Empty;
            public string NOP { get; set; } = string.Empty;
            public string ObjekPajak { get; set; } = string.Empty;
            public string Alamat { get; set; } = string.Empty;
            public int Omzet { get; set; }
            public int PajakBulanan { get; set; }
            public int AvgRealisasi { get; set; }
            public int Selisih { get; set; }
        }

        public class Dashboard
        {
            public decimal TotalPengedokan { get; set; }
            public decimal TotalRealisasi { get; set; }

            public int JumlahObjek { get; set; }

            public decimal Ratarata =>
                JumlahObjek > 0 ? TotalRealisasi / JumlahObjek : 0;
        }


        public static class Method
        {
            public static List<DataPendataan> GetDataPendataanList()
            {
                var ret = new List<DataPendataan>();
                var context = DBClass.GetContext();

                var restoDokNop = context.DbRekamRestorans.GroupBy(x => new { Nop = x.Nop }).Select(x => x.Key.Nop).ToList();
                var restoRealisasi = context.DbMonRestos.Where(x => restoDokNop.Contains(x.Nop)).Sum(x => x.NominalPokokBayar);
                var restoDok = context.DbRekamRestorans.GroupBy(x => new { Nop = x.Nop })
                    .Select(x => new DataPendataan
                    {
                        Tahun = DateTime.Now.Year,
                        EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                        Nop = x.Key.Nop,
                        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                        JumlahOp = x.Count(),
                        Potensi = x.Sum(x => x.PajakBulan),
                        TotalRealisasi = restoRealisasi ?? 0,
                        Selisih = x.Sum(x => x.PajakBulan) - (restoRealisasi ?? 0)
                    })
                    .ToList();

                ret.AddRange(restoDok);

                var parkirDokNop = context.DbRekamParkirs.GroupBy(x => new { Nop = x.Nop }).Select(x => x.Key.Nop).ToList();
                var parkirRealisasi = context.DbMonRestos.Where(x => parkirDokNop.Contains(x.Nop)).Sum(x => x.NominalPokokBayar);
                var parkirDok = context.DbRekamParkirs.GroupBy(x => new { Nop = x.Nop })
                    .Select(x => new DataPendataan
                    {
                        Tahun = DateTime.Now.Year,
                        EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                        Nop = x.Key.Nop,
                        JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                        JumlahOp = x.Count(),
                        Potensi = x.Sum(x => x.PajakBulan),
                        TotalRealisasi = parkirRealisasi ?? 0,
                        Selisih = x.Sum(x => x.PajakBulan) - (parkirRealisasi ?? 0)
                    })
                    .ToList();

                ret.AddRange(parkirDok);

                return ret;
            }

            public static List<DataDetailPendataan> GetDetailByJenisPajakAndTahun(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var ret = new List<DataDetailPendataan>();

                return ret;
            }
            public static List<SubDetailRestoran> GetSubDetailRestoran(EnumFactory.EPajak jenisPajak, string nop)
            {
                var ret = new List<SubDetailRestoran>();

                return ret;
            }
            public static List<SubDetailParkir> GetSubDetailParkir(EnumFactory.EPajak jenisPajak, string nop)
            {
                var ret = new List<SubDetailParkir>();

                return ret;
            }
            private static List<SubDetailParkir> GetSubDetailParkir()
            {
                return new List<SubDetailParkir>
                {
                    new SubDetailParkir
                    {
                        Tahun = 2023,
                        JenisPajak = "PBJT Parkir",
                        NOP = "32.76.020.002.456-7890.0",
                        ObjekPajak = "Parkir Mall ABC",
                        Alamat = "Jl. Merdeka No.1",
                        Hari = DateTime.Today,
                        Tgl = DateTime.Today,
                        JenisBiaya = "Flat",
                        KapasitasMotor = 200,
                        KapasitasMobil = 100,
                        JmlMotor = 150,
                        JmlMobil = 80,
                        JmlMobilBox = 5,
                        JmlTruk = 3,
                        JmlTrailer = 1,
                        EstMotor = 160,
                        EstMobil = 85,
                        EstMobilBox = 6,
                        EstTruk = 4,
                        EstTrailer = 1,
                        TarifMotor = 2000,
                        TarifMobil = 5000,
                        TarifMobilBox = 7000,
                        TarifTruk = 10000,
                        TarifTrailer = 15000
                    },
                };
            }
            private static List<SubDetailRestoran> GetSubDetailRestoran()
            {
                return new List<SubDetailRestoran>
                {
                    new SubDetailRestoran
                    {
                        Tahun = 2025,
                        JenisPajak = "PBJT Makanan & Minuman",
                        NOP = "32.71.111.001.234-5",
                        ObjekPajak = "Restoran Sederhana",
                        Alamat = "Jl. Merdeka No. 123, Bandung",
                        Hari = new DateTime(2025, 7, 1),
                        Tgl = new DateTime(2025, 7, 1),
                        JmlMeja = 20,
                        JmlKursi = 80,
                        JmlPengunjung = 250,
                        Bill = 12500000m,
                        RataPengunjung = 50,
                        RataBill = 50000m
                    },
                };
            }
            private static List<DataPendataan> GetAllData()
            {
                return new List<DataPendataan>
                {
                    new DataPendataan
                    {
                        Tahun = 2023,
                        JenisPajak = "PBJT Makanan & Minuman",
                        JumlahOp = 120,
                        Potensi = 500000000,
                        TotalRealisasi = 450000000,
                        Selisih = 50000000
                    },
                    new DataPendataan
                    {
                        Tahun = 2023,
                        JenisPajak = "PBJT Parkir",
                        JumlahOp = 80,
                        Potensi = 300_000_000,
                        TotalRealisasi = 275_000_000,
                        Selisih = 25_000_000
                    }
                };
            }

            private static List<DataDetailPendataan> GetAllDetail()
            {
                return new List<DataDetailPendataan>
                {
                    new DataDetailPendataan
                    {
                        Tahun = 2023,
                        JenisPajak = "PBJT Makanan & Minuman",
                        NOP = "32.76.010.001.123-4567.0",
                        ObjekPajak = "Restoran Sederhana",
                        Alamat = "Jl. Merdeka No. 10",
                        Omzet = 120_000_000,
                        PajakBulanan = 12_000_000,
                        AvgRealisasi = 10_500_000,
                        Selisih = 1_500_000
                    },
                    new DataDetailPendataan
                    {
                        Tahun = 2023,
                        JenisPajak = "PBJT Makanan & Minuman",
                        NOP = "32.76.010.001.234-5678.0",
                        ObjekPajak = "Warung Makan Enak",
                        Alamat = "Jl. Asia Afrika No. 25",
                        Omzet = 90_000_000,
                        PajakBulanan = 9_000_000,
                        AvgRealisasi = 8_250_000,
                        Selisih = 750_000
                    },
                    new DataDetailPendataan
                    {
                        Tahun = 2023,
                        JenisPajak = "PBJT Parkir",
                        NOP = "32.76.020.002.345-6789.0",
                        ObjekPajak = "Parkir Mall Kota",
                        Alamat = "Jl. Braga No. 50",
                        Omzet = 80_000_000,
                        PajakBulanan = 8_000_000,
                        AvgRealisasi = 7_200_000,
                        Selisih = 800_000
                    },
                    new DataDetailPendataan
                    {
                        Tahun = 2023,
                        JenisPajak = "PBJT Parkir",
                        NOP = "32.76.020.002.456-7890.0",
                        ObjekPajak = "Parkir Pasar Baru",
                        Alamat = "Jl. Otto Iskandardinata No. 12",
                        Omzet = 60_000_000,
                        PajakBulanan = 6_000_000,
                        AvgRealisasi = 5_500_000,
                        Selisih = 500_000
                    }
                };
            }
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    TotalPengedokan = 50,
                    JumlahObjek = 100,
                    TotalRealisasi = 800000000
                };
            }
        }

        public class SubDetailRestoran
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string ObjekPajak { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public DateTime Hari { get; set; }
            public DateTime Tgl { get; set; }
            public int JmlMeja { get; set; }
            public int JmlKursi { get; set; }
            public int JmlPengunjung { get; set; }
            public decimal Bill { get; set; }
            public int RataPengunjung { get; set; }
            public decimal RataBill { get; set; }
        }

        public class SubDetailParkir
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string ObjekPajak { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public DateTime Hari { get; set; }
            public DateTime Tgl { get; set; }
            public string JenisBiaya { get; set; } = null!;
            public int KapasitasMotor { get; set; }
            public int KapasitasMobil { get; set; }
            public int JmlMotor { get; set; }
            public int JmlMobil { get; set; }
            public int JmlMobilBox { get; set; }
            public int JmlTruk { get; set; }
            public int JmlTrailer { get; set; }
            public int EstMotor { get; set; }
            public int EstMobil { get; set; }
            public int EstMobilBox { get; set; }
            public int EstTruk { get; set; }
            public int EstTrailer { get; set; }
            public decimal TarifMotor { get; set; }
            public decimal TarifMobil { get; set; }
            public decimal TarifMobilBox { get; set; }
            public decimal TarifTruk { get; set; }
            public decimal TarifTrailer { get; set; }
        }
    }
}