namespace MonPDReborn.Models.AktivitasOP
{
    public class PemasanganAlatVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
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
                return new List<DataPemasanganAlat>
                {
                    new DataPemasanganAlat{No = 1, JenisPajak = "Hotel",     Terpasang2021 = 10, BelumTerpasang2021 = 2, Terpasang2022 = 15, BelumTerpasang2022 = 3, Terpasang2023 = 18, BelumTerpasang2023 = 1, Terpasang2024 = 20, BelumTerpasang2024 = 0},
                    new DataPemasanganAlat{No = 2, JenisPajak = "Restoran",  Terpasang2021 = 9,  BelumTerpasang2021 = 3, Terpasang2022 = 14, BelumTerpasang2022 = 4, Terpasang2023 = 17, BelumTerpasang2023 = 2, Terpasang2024 = 19, BelumTerpasang2024 = 1},
                    new DataPemasanganAlat{No = 3, JenisPajak = "Parkir",    Terpasang2021 = 8,  BelumTerpasang2021 = 1, Terpasang2022 = 12, BelumTerpasang2022 = 2, Terpasang2023 = 14, BelumTerpasang2023 = 3, Terpasang2024 = 16, BelumTerpasang2024 = 2},
                    new DataPemasanganAlat{No = 4, JenisPajak = "PBB",       Terpasang2021 = 7,  BelumTerpasang2021 = 2, Terpasang2022 = 10, BelumTerpasang2022 = 3, Terpasang2023 = 13, BelumTerpasang2023 = 1, Terpasang2024 = 15, BelumTerpasang2024 = 1},
                    new DataPemasanganAlat{No = 5, JenisPajak = "Reklame",   Terpasang2021 = 6,  BelumTerpasang2021 = 1, Terpasang2022 = 9,  BelumTerpasang2022 = 2, Terpasang2023 = 11, BelumTerpasang2023 = 1, Terpasang2024 = 13, BelumTerpasang2024 = 0},
                    new DataPemasanganAlat{No = 6, JenisPajak = "PPJ",       Terpasang2021 = 5,  BelumTerpasang2021 = 0, Terpasang2022 = 8,  BelumTerpasang2022 = 1, Terpasang2023 = 10, BelumTerpasang2023 = 1, Terpasang2024 = 12, BelumTerpasang2024 = 1},
                };
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
                    new() { JenisPajak = "Parkir", Kategori = "Wilayah Barat",  JumlahOP = 4, TerpasangTS = 3, TerpasangTB = 0, TerpasangSB = 1 }
                };
            }

            public static List<SubDetailModal> GetSubDetailModalData()
            {
                return new List<SubDetailModal>
                {
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "The Westin Surabaya", Alamat = "Jalan Menuju Kemenangan", NOP = "350001004005006", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "Shangri-La Hotel", Alamat = "Jl. Mayjend Sungkono", NOP = "350001004005007", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "JW Marriott", Alamat = "Jl. Embong Malang", NOP = "350001004005008", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "Vasa Hotel", Alamat = "Jl. HR Muhammad", NOP = "350001004005009", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Lima", NamaOP = "DoubleTree by Hilton", Alamat = "Jl. Tunjungan", NOP = "350001004005010", Tahun = 2024 },

                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Hotel Santika Premiere", Alamat = "Jl. Raya Gubeng", NOP = "350001004005011", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Grand Darmo Suite", Alamat = "Jl. Progo", NOP = "350001004005012", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Swiss-Belinn Tunjungan", Alamat = "Jl. Tunjungan", NOP = "350001004005013", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Quest Hotel Surabaya", Alamat = "Jl. Ronggolawe", NOP = "350001004005014", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Empat", NamaOP = "Premier Place Hotel", Alamat = "Jl. Raya Juanda", NOP = "350001004005015", Tahun = 2024 },

                    new() { Kategori = "Hotel Bintang Tiga", NamaOP = "POP! Hotel Gubeng", Alamat = "Jl. Bangka", NOP = "350001004005016", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Tiga", NamaOP = "Ibis Styles Surabaya", Alamat = "Jl. Jemursari", NOP = "350001004005017", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Tiga", NamaOP = "Hotel 88 Embong Kenongo", Alamat = "Jl. Embong Kenongo", NOP = "350001004005018", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Tiga", NamaOP = "Favehotel Rungkut", Alamat = "Jl. Raya Kalirungkut", NOP = "350001004005019", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Tiga", NamaOP = "Yello Hotel Jemursari", Alamat = "Jl. Raya Jemursari", NOP = "350001004005020", Tahun = 2024 },

                    new() { Kategori = "Hotel Bintang Dua", NamaOP = "Hotel Bali Inn", Alamat = "Jl. Bali", NOP = "350001004005021", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Dua", NamaOP = "RedDoorz Near ITS", Alamat = "Jl. Teknik Kimia", NOP = "350001004005022", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Dua", NamaOP = "Hotel Nusantara", Alamat = "Jl. Diponegoro", NOP = "350001004005023", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Dua", NamaOP = "OYO 123", Alamat = "Jl. Raya Panjang Jiwo", NOP = "350001004005024", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Dua", NamaOP = "Hotel Royal Inn", Alamat = "Jl. Basuki Rahmat", NOP = "350001004005025", Tahun = 2024 },

                    new() { Kategori = "Hotel Bintang Satu", NamaOP = "Losmen Damai", Alamat = "Jl. Kalimantan", NOP = "350001004005026", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Satu", NamaOP = "Wisma Sari", Alamat = "Jl. Semolowaru", NOP = "350001004005027", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Satu", NamaOP = "Kost Harian Syariah", Alamat = "Jl. Nginden", NOP = "350001004005028", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Satu", NamaOP = "Pondok Jaya", Alamat = "Jl. Ketintang", NOP = "350001004005029", Tahun = 2024 },
                    new() { Kategori = "Hotel Bintang Satu", NamaOP = "Wisma Sakura", Alamat = "Jl. Dharmawangsa", NOP = "350001004005030", Tahun = 2024 },

                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Kost Eksklusif Merpati", Alamat = "Jl. Merpati", NOP = "350001004005031", Tahun = 2024 },
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Homestay Bunda", Alamat = "Jl. Manukan", NOP = "350001004005032", Tahun = 2024 },
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Wisma Rakyat", Alamat = "Jl. Pucang Anom", NOP = "350001004005033", Tahun = 2024 },
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Penginapan Asri", Alamat = "Jl. Simorejo", NOP = "350001004005034", Tahun = 2024 },
                    new() { Kategori = "Hotel Non Bintang", NamaOP = "Kost Harian Pagi Cerah", Alamat = "Jl. Raya Mulyosari", NOP = "350001004005035", Tahun = 2024 },

                };
            }

        }

        public class DataPemasanganAlat
        {
            public int No { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Terpasang2021 { get; set; }
            public int BelumTerpasang2021 { get; set; }
            public int Terpasang2022 { get; set; }
            public int BelumTerpasang2022 { get; set; }
            public int Terpasang2023 { get; set; }
            public int BelumTerpasang2023 { get; set; }
            public int Terpasang2024 { get; set; }
            public int BelumTerpasang2024 { get; set; }
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
            public int Tahun { get; set; }
            
        }

    }
}
