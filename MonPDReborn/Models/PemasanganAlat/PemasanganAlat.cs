namespace MonPDReborn.Models.PemasanganAlat
{
    public class PemasanganAlat
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;

            public Index()
            {

            }
        }

        public class Show
        {
            public List<DataPemasanganAlat> DataPemasanganAlatList { get; set; } = new();
            public Show()
            {
            }
            public Show(string keyword)
            {
                DataPemasanganAlatList = Method.GetDataPemasanganAlatList(keyword);
            }
        }

        public class Detail
        {
            public List<DetailPemasanganAlat> DataDetailPemasanganAlatList { get; set; } = new();
            public Detail()
            {
            }
            public Detail(string JenisPajak)
            {
                DataDetailPemasanganAlatList = Method.GetDetailByKeyword(JenisPajak);
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
            public static List<DetailPemasanganAlat> GetDetailByKeyword(string keyword)
            {
                var allDetail = GetAllDetail();
                return allDetail
                    .Where(x => x.JenisPajak.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            // Internal dummy data
            private static List<DataPemasanganAlat> GetAllData()
            {
                return new List<DataPemasanganAlat>
                {
                    new DataPemasanganAlat{No = 1, JenisPajak = "Hotel", Terpasang2021 = 10, BelumTerpasang2021 = 2, Terpasang2022 = 15, BelumTerpasang2022 = 3, Terpasang2023 = 18, BelumTerpasang2023 = 1, Terpasang2024 = 20, BelumTerpasang2024 = 0},
                    new DataPemasanganAlat{No = 2, JenisPajak = "Restoraan", Terpasang2021 = 10, BelumTerpasang2021 = 2, Terpasang2022 = 15, BelumTerpasang2022 = 3, Terpasang2023 = 18, BelumTerpasang2023 = 1, Terpasang2024 = 20, BelumTerpasang2024 = 0},
                    new DataPemasanganAlat{No = 3, JenisPajak = "Parkir", Terpasang2021 = 10, BelumTerpasang2021 = 2, Terpasang2022 = 15, BelumTerpasang2022 = 3, Terpasang2023 = 18, BelumTerpasang2023 = 1, Terpasang2024 = 20, BelumTerpasang2024 = 0},
                    new DataPemasanganAlat{No = 4, JenisPajak = "PBB", Terpasang2021 = 10, BelumTerpasang2021 = 2, Terpasang2022 = 15, BelumTerpasang2022 = 3, Terpasang2023 = 18, BelumTerpasang2023 = 1, Terpasang2024 = 20, BelumTerpasang2024 = 0},
                    new DataPemasanganAlat{No = 5, JenisPajak = "Reklame", Terpasang2021 = 10, BelumTerpasang2021 = 2, Terpasang2022 = 15, BelumTerpasang2022 = 3, Terpasang2023 = 18, BelumTerpasang2023 = 1, Terpasang2024 = 20, BelumTerpasang2024 = 0},
                    new DataPemasanganAlat{No = 6, JenisPajak = "PPJ", Terpasang2021 = 10, BelumTerpasang2021 = 2, Terpasang2022 = 15, BelumTerpasang2022 = 3, Terpasang2023 = 18, BelumTerpasang2023 = 1, Terpasang2024 = 20, BelumTerpasang2024 = 0},
                };
            }
            private static List<DetailPemasanganAlat> GetAllDetail()
            {
                return new List<DetailPemasanganAlat>
                {
                    new DetailPemasanganAlat
                    {
                        JenisPajak = "HOTEL",
                        JumlahOP = 10,
                        TerpasangTS = 2,
                        TerpasangTB = 3,
                        TerpasangSB = 1,
                        DetailKategori = new List<KategoriDetail>
                        {
                            new KategoriDetail { Kategori = "Hotel Bintang 5", JumlahOP = 3, TerpasangTS = 1, TerpasangTB = 1, TerpasangSB = 0 },
                            new KategoriDetail { Kategori = "Hotel Bintang 3", JumlahOP = 5, TerpasangTS = 1, TerpasangTB = 2, TerpasangSB = 1 },
                        }
                    },
                    new DetailPemasanganAlat { JenisPajak = "RESTORAN", JumlahOP = 8, TerpasangTS = 1, TerpasangTB = 1, TerpasangSB = 1 },
                    new DetailPemasanganAlat { JenisPajak = "HIBURAN NON BIOSKOP", JumlahOP = 5, TerpasangTS = 0, TerpasangTB = 2, TerpasangSB = 0 },
                    new DetailPemasanganAlat { JenisPajak = "HIBURAN BIOSKOP", JumlahOP = 3, TerpasangTS = 0, TerpasangTB = 0, TerpasangSB = 0 },
                    new DetailPemasanganAlat { JenisPajak = "PARKIR", JumlahOP = 7, TerpasangTS = 2, TerpasangTB = 1, TerpasangSB = 1 }
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
            public int TotalTerpasang => TerpasangTS + TerpasangTB + TerpasangSB;

            public List<KategoriDetail> DetailKategori { get; set; } = new();
        }

        public class KategoriDetail
        {
            public string Kategori { get; set; } = null!;
            public int JumlahOP { get; set; }
            public int TerpasangTS { get; set; }
            public int TerpasangTB { get; set; }
            public int TerpasangSB { get; set; }
            public int TotalTerpasang => TerpasangTS + TerpasangTB + TerpasangSB;
        }

    }
}