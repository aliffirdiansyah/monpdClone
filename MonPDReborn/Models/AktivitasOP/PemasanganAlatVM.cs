namespace MonPDReborn.Models.AktivitasOP
{
    public class PemasanganAlatVM
    {
        public class Index
        {
            public DashboardData Data { get; set; } = new();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }

        public class Show
        {
            public List<DataPemasanganAlat> DataPemasanganAlatList { get; set; } = new();

            public Show() { }

            public Show(string keyword)
            {
                DataPemasanganAlatList = Method.GetDataPemasanganAlatList(keyword);
            }
        }

        public class Detail
        {
            public List<DetailPemasanganAlat> DetailPemasanganAlatList { get; set; } = new();

            public Detail() { }

            public Detail(string jenisPajak)
            {
                DetailPemasanganAlatList = Method.GetDetailData(jenisPajak);
            }
        }

        public class Method
        {
            public static DashboardData GetDashboardData()
            {
                var allData = GetAllData(); // Ambil data master
                var dashboard = new DashboardData
                {
                    HotelTerpasang = allData.FirstOrDefault(x => x.JenisPajak == "Hotel")?.Terpasang2024 ?? 0,
                    HotelTotal = allData.FirstOrDefault(x => x.JenisPajak == "Hotel")?.JumlahOP ?? 0,
                    RestoTerpasang = allData.FirstOrDefault(x => x.JenisPajak == "Restoran")?.Terpasang2024 ?? 0,
                    RestoTotal = allData.FirstOrDefault(x => x.JenisPajak == "Restoran")?.JumlahOP ?? 0,
                    HiburanTerpasang = allData.FirstOrDefault(x => x.JenisPajak == "Hiburan")?.Terpasang2024 ?? 0,
                    HiburanTotal = allData.FirstOrDefault(x => x.JenisPajak == "Hiburan")?.JumlahOP ?? 0,
                    ParkirTerpasang = allData.FirstOrDefault(x => x.JenisPajak == "Parkir")?.Terpasang2024 ?? 0,
                    ParkirTotal = allData.FirstOrDefault(x => x.JenisPajak == "Parkir")?.JumlahOP ?? 0
                };
                return dashboard;
            }
            public static List<DataPemasanganAlat> GetDataPemasanganAlatList(string keyword)
            {
                var allData = GetAllData();
                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<DataPemasanganAlat> GetAllData()
            {
                // 1. Ambil semua data detail kategori sebagai sumber utama
                var semuaKategori = GetSubDetailData();

                // 2. Kelompokkan data detail berdasarkan JenisPajak (Hotel, Restoran, dll)
                var groupedData = semuaKategori.GroupBy(x => x.JenisPajak);

                // 3. Buat list untuk hasil akhir (data master)
                var masterList = new List<DataPemasanganAlat>();
                int masterId = 1;

                foreach (var group in groupedData)
                {
                    // Buat list anak/detail untuk setiap grup
                    var detailItems = group.Select((item, index) => new DataPemasanganAlat
                    {
                        No = (masterId * 100) + index + 1, // Buat ID unik untuk anak
                        JenisPajak = item.Kategori, // Anak menampilkan Kategori
                        JumlahOP = item.JumlahOP,
                        Terpasang2023 = item.TerpasangTS + item.TerpasangTB + item.TerpasangSB,
                        BelumTerpasang2023 = item.JumlahOP - (item.TerpasangTS + item.TerpasangTB + item.TerpasangSB),
                        // Tambahkan data dummy untuk tahun lain jika perlu
                        Terpasang2021 = item.JumlahOP - 5,
                        BelumTerpasang2021 = 5,
                        Terpasang2022 = item.JumlahOP - 3,
                        BelumTerpasang2022 = 3,
                        Terpasang2024 = item.JumlahOP - 1,
                        BelumTerpasang2024 = 1
                    }).ToList();

                    // Buat baris master/induk
                    var masterItem = new DataPemasanganAlat
                    {
                        No = masterId++,
                        JenisPajak = group.Key, // Induk menampilkan JenisPajak
                        JumlahOP = detailItems.Sum(x => x.JumlahOP),
                        Terpasang2021 = detailItems.Sum(x => x.Terpasang2021),
                        BelumTerpasang2021 = detailItems.Sum(x => x.BelumTerpasang2021),
                        Terpasang2022 = detailItems.Sum(x => x.Terpasang2022),
                        BelumTerpasang2022 = detailItems.Sum(x => x.BelumTerpasang2022),
                        Terpasang2023 = detailItems.Sum(x => x.Terpasang2023),
                        BelumTerpasang2023 = detailItems.Sum(x => x.BelumTerpasang2023),
                        Terpasang2024 = detailItems.Sum(x => x.Terpasang2024),
                        BelumTerpasang2024 = detailItems.Sum(x => x.BelumTerpasang2024),
                        DetailItems = detailItems
                    };
                    masterList.Add(masterItem);
                }

                return masterList;
            }

            public static List<DetailPemasanganAlat> GetDetailData(string jenisPajak)
            {
                return new List<DetailPemasanganAlat>
                {
                    new() { JenisPajak = "Hotel",    JumlahOP = 10, TerpasangTS = 7, TerpasangTB = 2, TerpasangSB = 1 },
                    new() { JenisPajak = "Restoran", JumlahOP = 8,  TerpasangTS = 5, TerpasangTB = 2, TerpasangSB = 1 },
                    new() { JenisPajak = "Parkir",   JumlahOP = 6,  TerpasangTS = 4, TerpasangTB = 1, TerpasangSB = 1 }
                };
            }
            public static List<SubDetailKategori> GetSubDetailData()
            {
                return new List<SubDetailKategori>
                {
                    new() { JenisPajak = "Hotel", Kategori = "Hotel Bintang Lima",  JumlahOP = 4, TerpasangTS = 3, TerpasangTB = 1, TerpasangSB = 0 },
                    new() { JenisPajak = "Hotel", Kategori = "Hotel Bintang Empat",  JumlahOP = 6, TerpasangTS = 4, TerpasangTB = 1, TerpasangSB = 1 },
                    new() { JenisPajak = "Hotel", Kategori = "Hotel Bintang Tiga",  JumlahOP = 6, TerpasangTS = 4, TerpasangTB = 1, TerpasangSB = 1 },
                    new() { JenisPajak = "Hotel", Kategori = "Hotel Bintang Dua",  JumlahOP = 6, TerpasangTS = 4, TerpasangTB = 1, TerpasangSB = 1 },
                    new() { JenisPajak = "Hotel", Kategori = "Hotel Bintang Satu",  JumlahOP = 6, TerpasangTS = 4, TerpasangTB = 1, TerpasangSB = 1 },
                    new() { JenisPajak = "Hotel", Kategori = "Hotel Non Bintang",  JumlahOP = 6, TerpasangTS = 4, TerpasangTB = 1, TerpasangSB = 1 },
                    new() { JenisPajak = "Restoran", Kategori = "Wilayah Timur",  JumlahOP = 5, TerpasangTS = 3, TerpasangTB = 1, TerpasangSB = 1 },
                    new() { JenisPajak = "Restoran", Kategori = "Wilayah Barat",  JumlahOP = 3, TerpasangTS = 2, TerpasangTB = 1, TerpasangSB = 0 },
                    new() { JenisPajak = "Parkir", Kategori = "Wilayah Timur",  JumlahOP = 2, TerpasangTS = 1, TerpasangTB = 1, TerpasangSB = 0 },
                    new() { JenisPajak = "Parkir", Kategori = "Wilayah Barat",  JumlahOP = 4, TerpasangTS = 3, TerpasangTB = 0, TerpasangSB = 1 },
                    new() { JenisPajak = "Hiburan", Kategori = "Bioskop", JumlahOP = 8, TerpasangTS = 8, TerpasangTB = 0, TerpasangSB = 0 },
                    new() { JenisPajak = "Hiburan", Kategori = "Karaoke", JumlahOP = 12, TerpasangTS = 10, TerpasangTB = 2, TerpasangSB = 0 },
                    new() { JenisPajak = "Hiburan", Kategori = "Billiard", JumlahOP = 15, TerpasangTS = 10, TerpasangTB = 3, TerpasangSB = 2 },
                    new() { JenisPajak = "Hiburan", Kategori = "Panti Pijat", JumlahOP = 20, TerpasangTS = 15, TerpasangTB = 5, TerpasangSB = 0 }
                };
            }

            public static List<SubDetailModal> GetSubDetailModalData()
            {
                return new List<SubDetailModal>
                {
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "The Westin Surabaya", Alamat = "Jalan Menuju Kemenangan", NOP = "350001004005006", TanggalPemasangan = new DateTime(2025, 1, 15), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "Raffles Jakarta", Alamat = "Jl. Prof. Dr. Satrio", NOP = "317501000000001", TanggalPemasangan = new DateTime(2025, 1, 20), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "The Ritz-Carlton Bali", Alamat = "Jl. Raya Nusa Dua", NOP = "510901000000002", TanggalPemasangan = new DateTime(2025, 1, 25), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = true, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "Four Seasons Jakarta", Alamat = "Jl. Gatot Subroto", NOP = "317501000000003", TanggalPemasangan = new DateTime(2025, 1, 28), JenisPajak = "Hotel" , IsTerpasangTS =true, IsTerpasangTB = true, IsTerpasangSB = false},

                    // Bintang Empat (6 data)
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Grand Mercure Bandung", Alamat = "Jl. Braga No. 99", NOP = "327301000000001", TanggalPemasangan = new DateTime(2025, 2, 10), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Novotel Solo", Alamat = "Jl. Slamet Riyadi", NOP = "337401000000002", TanggalPemasangan = new DateTime(2025, 2, 12), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Mercure Bali Legian", Alamat = "Jl. Legian", NOP = "510901000000003", TanggalPemasangan = new DateTime(2025, 2, 14), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Swiss-Belhotel Pondok Indah", Alamat = "Jakarta Selatan", NOP = "317502000000004", TanggalPemasangan = new DateTime(2025, 2, 16), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Holiday Inn Bandung", Alamat = "Jl. Ir. H. Juanda", NOP = "327302000000005", TanggalPemasangan = new DateTime(2025, 2, 18), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = true, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Hotel Santika Premiere Yogyakarta", Alamat = "Jl. Jend. Sudirman", NOP = "347101000000006", TanggalPemasangan = new DateTime(2025, 2, 20), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = false, IsTerpasangSB = true },

                    // Bintang Tiga (6 data)
                    new() { Kategori = "Hotel Bintang Tiga", NamaOP = "Hotel Santika Malang", Alamat = "Jl. Letjen Sutoyo No. 79", NOP = "357901000000007", TanggalPemasangan = new DateTime(2025, 3, 5), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Bintang Tiga", NamaOP = "Whiz Hotel Cikini", Alamat = "Jl. Cikini Raya", NOP = "317503000000008", TanggalPemasangan = new DateTime(2025, 3, 7), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() {Kategori = "Hotel Bintang Tiga", NamaOP = "POP! Hotel Tebet", Alamat = "Jl. Prof. Dr. Soepomo", NOP = "317504000000009", TanggalPemasangan = new DateTime(2025, 3, 9), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Tiga", NamaOP = "Hotel Amaris Surabaya", Alamat = "Jl. Embong Malang", NOP = "357802000000010", TanggalPemasangan = new DateTime(2025, 3, 11), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Tiga", NamaOP = "Neo Malioboro Yogyakarta", Alamat = "Jl. Pasar Kembang", NOP = "347102000000011", TanggalPemasangan = new DateTime(2025, 3, 13), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = true, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Tiga", NamaOP = "Favehotel Braga", Alamat = "Jl. Braga", NOP = "327303000000012", TanggalPemasangan = new DateTime(2025, 3, 15), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = false, IsTerpasangSB = true},

                    // Bintang Dua (6 data)
                    new() {Kategori = "Hotel Bintang Dua", NamaOP = "Red Planet Jakarta", Alamat = "Jl. Pecenongan", NOP = "317505000000013", TanggalPemasangan = new DateTime(2025, 4, 1), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Dua", NamaOP = "Hotel Griyadi", Alamat = "Jl. Cikini", NOP = "317506000000014", TanggalPemasangan = new DateTime(2025, 4, 3), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Dua", NamaOP = "Hotel Nusa Indah", Alamat = "Jl. Juanda", NOP = "317507000000015", TanggalPemasangan = new DateTime(2025, 4, 5), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Dua", NamaOP = "Hotel New Idola", Alamat = "Jl. Pramuka", NOP = "317508000000016", TanggalPemasangan = new DateTime(2025, 4, 7), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Dua", NamaOP = "Hotel Cempaka", Alamat = "Jl. Cempaka Putih", NOP = "317509000000017", TanggalPemasangan = new DateTime(2025, 4, 9), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = true, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Dua", NamaOP = "Hotel Menteng 1", Alamat = "Jl. Matraman", NOP = "317510000000018", TanggalPemasangan = new DateTime(2025, 4, 11), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = false, IsTerpasangSB = true},

                    // Bintang Satu (6 data)
                    new() {Kategori = "Hotel Bintang Satu", NamaOP = "Hotel Alia", Alamat = "Jl. Cikini Raya", NOP = "317511000000019", TanggalPemasangan = new DateTime(2025, 4, 15), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Satu", NamaOP = "Hotel Mustika", Alamat = "Jl. Kebon Sirih", NOP = "317512000000020", TanggalPemasangan = new DateTime(2025, 4, 17), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() { Kategori = "Hotel Bintang Satu", NamaOP = "Hotel Monas", Alamat = "Jl. Merdeka", NOP = "317513000000021", TanggalPemasangan = new DateTime(2025, 4, 19), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() {Kategori = "Hotel Bintang Satu", NamaOP = "Hotel Rahayu", Alamat = "Jl. Pahlawan", NOP = "317514000000022", TanggalPemasangan = new DateTime(2025, 4, 21), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Satu", NamaOP = "Hotel Mawar", Alamat = "Jl. Mawar", NOP = "317515000000023", TanggalPemasangan = new DateTime(2025, 4, 23), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = true, IsTerpasangSB = false},
                    new() {Kategori = "Hotel Bintang Satu", NamaOP = "Hotel Seruni", Alamat = "Jl. Bogor", NOP = "327304000000024", TanggalPemasangan = new DateTime(2025, 4, 25), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = false, IsTerpasangSB = true},

                    // Non Bintang (6 data)
                    new() {Kategori = "Hotel Non Bintang", NamaOP = "Wisma Sederhana", Alamat = "Jl. Kebon Jeruk No. 88", NOP = "317101000000005", TanggalPemasangan = new DateTime(2025, 4, 28), JenisPajak = "Hotel", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Penginapan Amanah", Alamat = "Jl. Menteng Raya", NOP = "317102000000026", TanggalPemasangan = new DateTime(2025, 4, 29), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Losmen Maju", Alamat = "Jl. Jatinegara", NOP = "317103000000027", TanggalPemasangan = new DateTime(2025, 5, 1), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Kost Harian Damai", Alamat = "Jl. Fatmawati", NOP = "317104000000028", TanggalPemasangan = new DateTime(2025, 5, 3), JenisPajak = "Hotel", IsTerpasangTS =true, IsTerpasangTB = false, IsTerpasangSB = false },
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Griya Tamu Cempaka", Alamat = "Jl. Cempaka", NOP = "317105000000029", TanggalPemasangan = new DateTime(2025, 5, 5), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = true, IsTerpasangSB = false},
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Homestay Mawar", Alamat = "Jl. Karang Tengah", NOP = "317106000000030", TanggalPemasangan = new DateTime(2025, 5, 7), JenisPajak = "Hotel", IsTerpasangTS =false, IsTerpasangTB = false, IsTerpasangSB = true },

                    new() { Kategori = "Bioskop", NamaOP = "XXI Tunjungan Plaza", Alamat = "Jl. Basuki Rahmat", NOP = "350101001001001", TanggalPemasangan = new DateTime(2023, 5, 10), JenisPajak = "Hiburan", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                    new() { Kategori = "Bioskop", NamaOP = "CGV Marvell City", Alamat = "Jl. Ngagel", NOP = "350101001002002", TanggalPemasangan = new DateTime(2023, 6, 15), JenisPajak = "Hiburan", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
        
                    // Data untuk Kategori Karaoke
                    new() { Kategori = "Karaoke", NamaOP = "NAV Karaoke", Alamat = "Jl. Mayjend Sungkono", NOP = "350202002003003", TanggalPemasangan = new DateTime(2024, 1, 20), JenisPajak = "Hiburan", IsTerpasangTS = true, IsTerpasangTB = true, IsTerpasangSB = false},
                    new() { Kategori = "Karaoke", NamaOP = "Happy Puppy", Alamat = "Jl. HR Muhammad", NOP = "350202002004004", TanggalPemasangan = new DateTime(2024, 2, 25), JenisPajak = "Hiburan", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},

                    // Data untuk Kategori Billiard
                    new() { Kategori = "Billiard", NamaOP = "Strike Billiard", Alamat = "Jl. Tidar", NOP = "350303003005005", TanggalPemasangan = new DateTime(2023, 8, 8), JenisPajak = "Hiburan", IsTerpasangTS = true, IsTerpasangTB = true, IsTerpasangSB = true},
        
                    // Data untuk Kategori Panti Pijat
                    new() { Kategori = "Panti Pijat", NamaOP = "Kokuo Reflexology", Alamat = "Jl. Tunjungan", NOP = "350404004006006", TanggalPemasangan = new DateTime(2024, 3, 3), JenisPajak = "Hiburan", IsTerpasangTS = true, IsTerpasangTB = false, IsTerpasangSB = false},
                };
            }

        }

        public class DashboardData
        {
            public int HotelTerpasang { get; set; }
            public int HotelTotal { get; set; }
            public int RestoTerpasang { get; set; }
            public int RestoTotal { get; set; }
            public int HiburanTerpasang { get; set; }
            public int HiburanTotal { get; set; }
            public int ParkirTerpasang { get; set; }
            public int ParkirTotal { get; set; }
        }
        public class DataPemasanganAlat
        {
            public int No { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int JumlahOP { get; set; }
            public int Terpasang2021 { get; set; }
            public int BelumTerpasang2021 { get; set; }
            public int Terpasang2022 { get; set; }
            public int BelumTerpasang2022 { get; set; }
            public int Terpasang2023 { get; set; }
            public int BelumTerpasang2023 { get; set; }
            public int Terpasang2024 { get; set; }
            public int BelumTerpasang2024 { get; set; }

            public List<DataPemasanganAlat>? DetailItems { get; set; }
        }

        public class DetailPemasanganAlat
        {
            public string JenisPajak { get; set; } = null!;
            public int JumlahOP { get; set; }
            public int TerpasangTS { get; set; }
            public int TerpasangTB { get; set; }
            public int TerpasangSB { get; set; }
        }

        public class SubDetailKategori
        {
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public int JumlahOP { get; set; }
            public int TerpasangTS { get; set; }
            public int TerpasangTB { get; set; }
            public int TerpasangSB { get; set; }
        }

        public class SubDetailModal
        {
            public string Kategori { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public DateTime TanggalPemasangan { get; set; }
            public string JenisPajak { get; set; } = null!;

            public bool IsTerpasangTS { get; set; }
            public bool IsTerpasangTB { get; set; }
            public bool IsTerpasangSB { get; set; }

        }

    }
}
