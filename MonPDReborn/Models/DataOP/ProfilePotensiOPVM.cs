using MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM;

namespace MonPDReborn.Models.DataOP
{
    public class ProfilePotensiOPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Dashboard Data { get; set; } = new();
            public Index()
            {
                Data = Method.GetDashboardData();

            }
        }

        public class ShowRekap
        {
            public List<RekapPotensi> DataRekapPotensi { get; set; } = new();
            public ShowRekap() { }
            public ShowRekap(string jenisPajak)
            {

                DataRekapPotensi = Method.GetRekapPotensiList(jenisPajak);
            }
        }

        public class ShowDetail
        {
            public List<DetailPotensi> DataDetailPotensi { get; set; } = new();
            public string JenisPajak { get; set; } = string.Empty;

            public ShowDetail() { }
            public ShowDetail(string jenisPajak)
            {
                JenisPajak = jenisPajak;
                DataDetailPotensi = Method.GetDetailPotensiList(jenisPajak);
            }
        }

        public class ShowData
        {
            public List<DataPotensi> DataPotensiList { get; set; } = new();
            public string JenisPajak { get; set; } = string.Empty;
            public string Kategori { get; set; } = string.Empty;
            public ShowData() { }
            public ShowData(string jenisPajak, string kategori)
            {
                JenisPajak = jenisPajak;
                Kategori = kategori;
                DataPotensiList = Method.GetDataPotensiList(jenisPajak, kategori);
            }
        }
        public class Detail
        {
            public string NOP { get; set; } = string.Empty;
            public string JenisPajak { get; set; } = string.Empty;
            public string? Kategori { get; set; }
            
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();

            public Detail() { }

            public Detail(string nop, string jenisPajak, string? kategori = null)
            {
                NOP = nop;
                JenisPajak = jenisPajak;
                Kategori = kategori;

                DataRealisasiBulananList = Method.GetDetailByNOP(nop, jenisPajak, kategori);
            }
        }


        public class Method
        {
            public static Dashboard GetDashboardData()
            {
                decimal potensi = 125000000;
                decimal realisasi = 110000000;
                int totalOp = 500;
                int realisasiOp = 450;

                var dashboardData = new Dashboard
                {
                    Potensi = potensi,
                    RealisasiTotal = realisasi,
                    Capaian = (potensi > 0) ? Math.Round((realisasi / potensi) * 100, 0) : 0,
                    TotalOP = totalOp,
                    RealisasiOP = realisasiOp,
                    CapaianOP = $"{realisasiOp} dari {totalOp} OP"
                };

                return dashboardData;
            }

            public static List<DataPotensi> GetDataPotensiList(string jenisPajak, string kategori)
            {
                var allData = GetAllData();

                return allData
                    .Where(d =>
                        (string.IsNullOrWhiteSpace(jenisPajak) || 
                         (!string.IsNullOrEmpty(d.JenisPajak) && d.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase)))
                        &&
                        (string.IsNullOrWhiteSpace(kategori) || 
                         (!string.IsNullOrEmpty(d.Kategori) && d.Kategori.Equals(kategori, StringComparison.OrdinalIgnoreCase)))
                    )
                    .ToList();
            }


            public static List<RealisasiBulanan> GetDetailByNOP(string nop, string jenisPajak, string? kategori = null)
            {
                var dataPotensi = GetAllData()
                    .FirstOrDefault(d => d.NOP == nop && d.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase));

                if (dataPotensi != null)
                {
                    var match = AllRealisasiBulanan
                        .Where(r => r.NOP == nop);

                    // jika kategori ada, filter juga berdasarkan kategori
                    if (!string.IsNullOrWhiteSpace(kategori))
                    {
                        match = match.Where(r => r.Kategori != null && r.Kategori.Equals(kategori, StringComparison.OrdinalIgnoreCase));
                    }

                    var result = match.ToList();

                    if (result.Any()) return result;
                }

                // fallback
                return new List<RealisasiBulanan>
                {
                    new()
                    {
                        NOP = nop,
                        NamaWP = "Data tidak ditemukan",
                        Alamat = "-",
                        Kapasitas = 0,
                        Perhari = 0,
                        Perbulan = 0,
                        Pertahun = 0
                    }
                };
                        }

            // Internal dummy data
            private static List<DataPotensi> GetAllData()
            {
                return new List<DataPotensi>
                {
                     new() {JenisPajak = "Hiburan", NOP = "32.76.050.123", NamaWP = "PT ABC Sejahtera", Alamat = "Jl. Merdeka", Kategori = "Massage", MasaPajak = "Januari", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hotel", NOP = "32.76.050.124", NamaWP = "CV Mitra Usaha", Alamat = "Jl. Sudirman", Kategori = "Bintang 5", MasaPajak = "Februari", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Restoran", NOP = "32.76.050.125", NamaWP = "PT Maju Mundur", Alamat = "Jl. Gatot Subroto", Kategori = "Restoran", MasaPajak = "Maret", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hiburan", NOP = "32.76.050.126", NamaWP = "Toko Sumber Rejeki", Alamat = "Jl. Gajah Mada", Kategori = "Gym", MasaPajak = "April", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hiburan", NOP = "32.76.050.127", NamaWP = "PT Sinar Terang", Alamat = "Jl. Imam Bonjol", Kategori = "Bioskop", MasaPajak = "Mei", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Parkir", NOP = "32.76.050.128", NamaWP = "Bengkel Jaya Motor", Alamat = "Jl. Pahlawan", Kategori = "Parkir", MasaPajak = "Juni", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "PBB", NOP = "32.76.050.129", NamaWP = "Apotek Sehat Sentosa", Alamat = "Jl. Kalimantan", Kategori = "PBB", MasaPajak = "Juli", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Restoran", NOP = "32.76.050.130", NamaWP = "Kafe Kopi Nusantara", Alamat = "Jl. Sumatra", Kategori = "Restoran", MasaPajak = "Agustus", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Parkir", NOP = "32.76.050.131", NamaWP = "PT Transport Abadi", Alamat = "Jl. Diponegoro", Kategori = "Parkir", MasaPajak = "September", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000},
                     new() {JenisPajak = "Hotel", NOP = "32.76.050.132", NamaWP = "Mall Surya", Alamat = "Jl. Basuki Rahmat", Kategori = "Bintang 4", MasaPajak = "Oktober", Target1 = 6_000_000, Realisasi1 = 5_000_000, Target2 = 7_000_000, Realisasi2 = 6_200_000, Target3 = 8_000_000, Realisasi3 = 7_800_000, TotalPotensi = 18_000_000}
                };
            }

            private static List<RealisasiBulanan> AllRealisasiBulanan = new List<RealisasiBulanan>
            {
                new()
                {
                    NOP = "32.76.050.200",
                    NamaWP = "URBAN ATHLETES",
                    Alamat = "JL. RAYA TIAD A THE CENTRAL MALL - GUNAWANGSA TIADJI, SURABAYA",
                    Kapasitas = 150,
                    Perhari = 120,
                    Perbulan = 3600,
                    Pertahun = 43200
                },
                new()
                {
                    NOP = "32.76.050.127",
                    NamaWP = "PT Sinar Terang",
                    Alamat = "Jl. Imam Bonjol",
                    Kapasitas = 100,
                    Perhari = 90,
                    Perbulan = 2700,
                    Pertahun = 32400
                },
                new()
                {
                    NOP = "32.76.050.128",
                    NamaWP = "Bengkel Jaya Motor",
                    Alamat = "Jl. Pahlawan",
                    Kapasitas = 80,
                    Perhari = 70,
                    Perbulan = 2100,
                    Pertahun = 25200
                },
                // tambahkan sebanyak yang kamu perlukan
            };

            public static List<RekapPotensi> GetRekapPotensiList(string jenisPajak)
            {
                var allData = GetRekapPotensi();

                if (string.IsNullOrWhiteSpace(jenisPajak))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(jenisPajak, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<RekapPotensi> GetRekapPotensi()
            {
                return new List<RekapPotensi>
                {
                     new RekapPotensi
                    {
                        JenisPajak = "Hotel",
                        Target1 = 10_000_000,
                        Realisasi1 = 8_500_000,
                        Target2 = 12_000_000,
                        Realisasi2 = 11_000_000,
                        Target3 = 15_000_000,
                        Realisasi3 = 14_500_000,
                        TotalPotensi = 16_500_000
                    },
                    new RekapPotensi
                    {
                        JenisPajak = "Restoran",
                        Target1 = 8_000_000,
                        Realisasi1 = 6_750_000,
                        Target2 = 9_000_000,
                        Realisasi2 = 8_250_000,
                        Target3 = 10_000_000,
                        Realisasi3 = 9_200_000,
                        TotalPotensi = 16_500_000
                    },
                    new RekapPotensi
                    {
                        JenisPajak = "Hiburan",
                        Target1 = 5_000_000,
                        Realisasi1 = 4_000_000,
                        Target2 = 6_000_000,
                        Realisasi2 = 5_200_000,
                        Target3 = 7_000_000,
                        Realisasi3 = 6_800_000,
                        TotalPotensi = 16_500_000
                    },
                    new RekapPotensi
                    {
                        JenisPajak = "Parkir",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_500_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_000_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_750_000,
                        TotalPotensi = 16_500_000
                    }
                };
            }

            public static List<DetailPotensi> GetDetailPotensiList(string jenisPajak)
            {
                var allData = GetDetailPotensi();

                if (string.IsNullOrWhiteSpace(jenisPajak))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(jenisPajak, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<DetailPotensi> GetDetailPotensi()
            {
                return new List<DetailPotensi>
                {
                    new DetailPotensi
                    {
                        JenisPajak = "Hotel",
                        Kategori = "Bintang 5",
                        Target1 = 6_000_000,
                        Realisasi1 = 5_000_000,
                        Target2 = 7_000_000,
                        Realisasi2 = 6_200_000,
                        Target3 = 8_000_000,
                        Realisasi3 = 7_800_000,
                        TotalPotensi = 18_000_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Hotel",
                        Kategori = "Bintang 3",
                        Target1 = 4_000_000,
                        Realisasi1 = 3_500_000,
                        Target2 = 5_000_000,
                        Realisasi2 = 4_800_000,
                        Target3 = 6_000_000,
                        Realisasi3 = 5_900_000,
                        TotalPotensi = 12_000_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Restoran",
                        Kategori = "Franchise",
                        Target1 = 5_000_000,
                        Realisasi1 = 4_200_000,
                        Target2 = 5_500_000,
                        Realisasi2 = 5_000_000,
                        Target3 = 6_000_000,
                        Realisasi3 = 5_700_000,
                        TotalPotensi = 16_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Restoran",
                        Kategori = "Mandiri",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_550_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_250_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_800_000,
                        TotalPotensi = 10_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Hiburan",
                        Kategori = "Bioskop",
                        Target1 = 2_000_000,
                        Realisasi1 = 1_750_000,
                        Target2 = 2_500_000,
                        Realisasi2 = 2_200_000,
                        Target3 = 3_000_000,
                        Realisasi3 = 2_850_000,
                        TotalPotensi = 7_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Hiburan",
                        Kategori = "Arena",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_300_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_000_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_750_000,
                        TotalPotensi = 10_500_000
                    },
                    new DetailPotensi
                    {
                        JenisPajak = "Parkir",
                        Kategori = "Parkir",
                        Target1 = 3_000_000,
                        Realisasi1 = 2_300_000,
                        Target2 = 3_500_000,
                        Realisasi2 = 3_000_000,
                        Target3 = 4_000_000,
                        Realisasi3 = 3_750_000,
                        TotalPotensi = 10_500_000
                    }
                };
            }

            public static InfoDasar GetInfoDasar(string nop)
            {
                // Simulasi mengambil data dari DB berdasarkan NOP
                return new InfoDasar
                {
                    NOP = nop,
                    NamaWP = "GALAXY MALL PARKIR",
                    Alamat = "JL. DHARMAHUSADA INDAH TIMUR",
                    KapasitasMotor = 1500,
                    KapasitasMobil = 1000
                };
            }

            // Method untuk data dummy Kapasitas & Tarif
            public static KapasitasTarif GetKapasitasTarif(string nop)
            {
                return new KapasitasTarif
                {
                    KapasitasMotor = 1500,
                    TerpakaiMotorHariKerja = 900,
                    TerpakaiMotorAkhirPekan = 1350,
                    TarifMotor = 3000,
                    KapasitasMobil = 1000,
                    TerpakaiMobilHariKerja = 800,
                    TerpakaiMobilAkhirPekan = 950,
                    TarifMobil = 10000
                };
            }

            public static InfoDasar GetInfoDasarHotel(string nop)
            {
                // Simulasi ambil data dari DB
                return new InfoDasar
                {
                    NOP = nop,
                    NamaWP = "HOTEL JW MARRIOTT",
                    Alamat = "Jl. Embong Malang 85-89, Surabaya",
                };
            }

            public static InfoKamar GetInfoKamar(string nop)
            {
                return new InfoKamar
                {
                    JumlahKamar = 400,
                    TarifRataRata = 1500000,
                    TingkatHunian = 85 // artinya 85%
                };
            }

            public static InfoBanquet GetInfoBanquet(string nop)
            {
                return new InfoBanquet
                {
                    KapasitasMaksimum = 1500,
                    TingkatOkupansi = 60, // artinya 60%
                    TarifRataRata = 250000,
                    HariEventPerBulan = 10
                };
            }

            public static InfoDasar GetInfoDasarBioskop(string nop)
            {
                return new InfoDasar
                {
                    NOP = nop,
                    NamaWP = "XXI CITO",
                    Alamat = "Jl. A. Yani No. 288, Surabaya",
                    KapasitasKursiStudio = 850 // Properti baru untuk InfoDasar
                };
            }

            public static InfoBioskop GetInfoBioskop(string nop)
            {
                return new InfoBioskop
                {
                    KapasitasKursi = 850,
                    KursiTerjualPerHari = 600,
                    TurnoverHariKerja = 2.5m,
                    TurnoverAkhirPekan = 4.0m,
                    HargaTiketHariKerja = 40000,
                    HargaTiketAkhirPekan = 55000
                };
            }
        }

        public class Dashboard
        {
            public decimal Potensi { get; set; }
            public decimal RealisasiTotal { get; set; }
            public decimal Capaian { get; set; }
            public int TotalOP { get; set; }
            public int RealisasiOP { get; set; }
            public string CapaianOP { get; set; } = "";
        }

        public class DataPotensi
        {
            public string NOP { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public string MasaPajak { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public int Kapasitas { get; set; }
            public int Perhari { get; set; }
            public int Perbulan { get; set; }
            public int Pertahun { get; set; }
        }

        public class RekapPotensi
        {
            public string JenisPajak { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }

        public class DetailPotensi
        {
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }

        public class DetailParkir
        {
            public InfoDasar DataDasar { get; set; } = new();
            public KapasitasTarif DataKapasitas { get; set; } = new();
            public PotensiPajak DataPotensi { get; set; } = new();

            public DetailParkir() { }

            // Konstruktor untuk mengambil semua data berdasarkan NOP
            public DetailParkir(string nop)
            {
                // Panggil method untuk mengambil semua data dummy
                DataDasar = Method.GetInfoDasar(nop);
                DataKapasitas = Method.GetKapasitasTarif(nop);
                DataPotensi = new PotensiPajak(DataKapasitas); // Potensi dihitung dari kapasitas
            }
        }

        // Class untuk data di kartu paling atas
        public class InfoDasar
        {
            public string NamaWP { get; set; } = "";
            public string Alamat { get; set; } = "";
            public string NOP { get; set; } = "";
            public decimal TotalKapasitas => KapasitasMotor + KapasitasMobil;
            public decimal KapasitasMotor { get; set; }
            public decimal KapasitasMobil { get; set; }

            public decimal PersenKapasitasMotor => TotalKapasitas > 0 ? (KapasitasMotor / TotalKapasitas) * 100 : 0;

            // Menghitung persentase kapasitas mobil dari total
            public decimal PersenKapasitasMobil => TotalKapasitas > 0 ? (KapasitasMobil / TotalKapasitas) * 100 : 0;

            // Hotel
            public int? JumlahKamar { get; set; }
            public int? KapasitasBanquet { get; set; }

            // Bioskop
            public int? KapasitasKursiStudio { get; set; }
        }

        // Class untuk data di kartu kapasitas dan tarif
        public class KapasitasTarif
        {
            public int KapasitasMotor { get; set; }
            public int TerpakaiMotorHariKerja { get; set; }
            public int TerpakaiMotorAkhirPekan { get; set; }
            public decimal TarifMotor { get; set; }

            public int KapasitasMobil { get; set; }
            public int TerpakaiMobilHariKerja { get; set; }
            public int TerpakaiMobilAkhirPekan { get; set; }
            public decimal TarifMobil { get; set; }

            // Properti kalkulasi untuk persentase pemakaian
            public decimal PersenMotorHariKerja => KapasitasMotor > 0 ? ((decimal)TerpakaiMotorHariKerja / KapasitasMotor) * 100 : 0;
            public decimal PersenMotorAkhirPekan => KapasitasMotor > 0 ? ((decimal)TerpakaiMotorAkhirPekan / KapasitasMotor) * 100 : 0;
            public decimal PersenMobilHariKerja => KapasitasMobil > 0 ? ((decimal)TerpakaiMobilHariKerja / KapasitasMobil) * 100 : 0;
            public decimal PersenMobilAkhirPekan => KapasitasMobil > 0 ? ((decimal)TerpakaiMobilAkhirPekan / KapasitasMobil) * 100 : 0;

            public int TotalTerparkirHariKerja => TerpakaiMotorHariKerja + TerpakaiMobilHariKerja;
            public int TotalTerparkirAkhirPekan => TerpakaiMotorAkhirPekan + TerpakaiMobilAkhirPekan;
        }

        // Class untuk data di kartu perhitungan potensi
        public class PotensiPajak
        {
            public decimal OmzetMotor { get; }
            public decimal OmzetMobil { get; }
            public decimal TotalOmzetBulanan { get; }
            public decimal PotensiBulanan { get; }
            public decimal PotensiTahunan { get; }
            public const decimal TarifPajak = 0.10m; // 10%

            // Konstruktor untuk menghitung semua potensi
            public PotensiPajak() { }
            public PotensiPajak(KapasitasTarif dataKapasitas)
            {
                decimal omzetMotorHariKerja = dataKapasitas.TerpakaiMotorHariKerja * dataKapasitas.TarifMotor * 22;
                decimal omzetMotorAkhirPekan = dataKapasitas.TerpakaiMotorAkhirPekan * dataKapasitas.TarifMotor * 8;
                OmzetMotor = omzetMotorHariKerja + omzetMotorAkhirPekan;

                decimal omzetMobilHariKerja = dataKapasitas.TerpakaiMobilHariKerja * dataKapasitas.TarifMobil * 22;
                decimal omzetMobilAkhirPekan = dataKapasitas.TerpakaiMobilAkhirPekan * dataKapasitas.TarifMobil * 8;
                OmzetMobil = omzetMobilHariKerja + omzetMobilAkhirPekan;

                TotalOmzetBulanan = OmzetMotor + OmzetMobil;
                PotensiBulanan = TotalOmzetBulanan * TarifPajak;
                PotensiTahunan = PotensiBulanan * 12;
            }
        }

        // Class ini akan menjadi @model untuk halaman DetailHotel.cshtml
        public class DetailHotel
        {
            public InfoDasar DataDasar { get; set; } = new();
            public InfoKamar DataKamar { get; set; } = new();
            public InfoBanquet DataBanquet { get; set; } = new();
            public PotensiPajakHotel DataPotensi { get; set; } = new();

            public DetailHotel() { }
            public DetailHotel(string nop)
            {
                DataDasar = Method.GetInfoDasarHotel(nop);
                DataKamar = Method.GetInfoKamar(nop);
                DataBanquet = Method.GetInfoBanquet(nop);
                DataPotensi = new PotensiPajakHotel(DataKamar, DataBanquet);
            }
        }

        // Class untuk info kamar
        public class InfoKamar
        {
            public int JumlahKamar { get; set; }
            public decimal TarifRataRata { get; set; }
            public decimal TingkatHunian { get; set; } // Dalam persen (misal: 85 untuk 85%)
            public int RataKamarTerjual => (int)(JumlahKamar * (TingkatHunian / 100));
        }

        // Class untuk info banquet
        public class InfoBanquet
        {
            public int KapasitasMaksimum { get; set; }
            public decimal TingkatOkupansi { get; set; } // Dalam persen
            public decimal TarifRataRata { get; set; }
            public int HariEventPerBulan { get; set; }
            public int RataTamuBanquet => (int)(KapasitasMaksimum * (TingkatOkupansi / 100));
        }

        // Class untuk kalkulasi potensi pajak hotel
        public class PotensiPajakHotel
        {
            public decimal OmzetKamarBulanan { get; }
            public decimal PajakKamarBulanan { get; }
            public decimal OmzetBanquetBulanan { get; }
            public decimal PajakBanquetBulanan { get; }
            public decimal TotalPotensiBulanan { get; }
            public decimal TotalPotensiTahunan { get; }
            public const decimal TarifPajak = 0.10m; // 10%

            public decimal PersenPotensiKamar => TotalPotensiBulanan > 0 ? (PajakKamarBulanan / TotalPotensiBulanan) * 100 : 0;
            public decimal PersenPotensiBanquet => TotalPotensiBulanan > 0 ? (PajakBanquetBulanan / TotalPotensiBulanan) * 100 : 0;

            public PotensiPajakHotel() { }
            public PotensiPajakHotel(InfoKamar kamar, InfoBanquet banquet)
            {
                OmzetKamarBulanan = kamar.TarifRataRata * kamar.RataKamarTerjual * 30;
                PajakKamarBulanan = OmzetKamarBulanan * TarifPajak;

                OmzetBanquetBulanan = banquet.TarifRataRata * banquet.RataTamuBanquet * banquet.HariEventPerBulan;
                PajakBanquetBulanan = OmzetBanquetBulanan * TarifPajak;

                TotalPotensiBulanan = PajakKamarBulanan + PajakBanquetBulanan;
                TotalPotensiTahunan = TotalPotensiBulanan * 12;
            }
        }

        // Class ini akan menjadi @model untuk halaman DetailBioskop.cshtml
        public class DetailBioskop
        {
            public InfoDasar DataDasar { get; set; } = new();
            public InfoBioskop DataBioskop { get; set; } = new();
            public PotensiPajakBioskop DataPotensi { get; set; } = new();

            public DetailBioskop() { }
            public DetailBioskop(string nop)
            {
                DataDasar = Method.GetInfoDasarBioskop(nop);
                DataBioskop = Method.GetInfoBioskop(nop);
                DataPotensi = new PotensiPajakBioskop(DataBioskop);
            }
        }

        // Class untuk info spesifik bioskop
        public class InfoBioskop
        {
            public int KapasitasKursi { get; set; }
            public int KursiTerjualPerHari { get; set; }
            public decimal TurnoverHariKerja { get; set; }
            public decimal TurnoverAkhirPekan { get; set; }
            public decimal HargaTiketHariKerja { get; set; }
            public decimal HargaTiketAkhirPekan { get; set; }

            // Properti kalkulasi
            public int RataKunjunganHariKerja => (int)(KursiTerjualPerHari * TurnoverHariKerja);
            public int RataKunjunganAkhirPekan => (int)(KursiTerjualPerHari * TurnoverAkhirPekan);
        }

        // Class untuk kalkulasi potensi pajak bioskop
        public class PotensiPajakBioskop
        {
            public decimal OmzetHariKerja { get; }
            public decimal OmzetAkhirPekan { get; }
            public decimal TotalOmzetBulanan { get; }
            public decimal PotensiBulanan { get; }
            public decimal PotensiTahunan { get; }
            public const decimal TarifPajak = 0.10m; // 10%

            public PotensiPajakBioskop() { }
            public PotensiPajakBioskop(InfoBioskop dataBioskop)
            {
                OmzetHariKerja = dataBioskop.HargaTiketHariKerja * dataBioskop.RataKunjunganHariKerja * 22;
                OmzetAkhirPekan = dataBioskop.HargaTiketAkhirPekan * dataBioskop.RataKunjunganAkhirPekan * 8;
                TotalOmzetBulanan = OmzetHariKerja + OmzetAkhirPekan;
                PotensiBulanan = TotalOmzetBulanan * TarifPajak;
                PotensiTahunan = PotensiBulanan * 12;
            }
        }


    }
}
