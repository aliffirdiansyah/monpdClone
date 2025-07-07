using static MonPDReborn.Models.AktivitasOP.PemasanganAlatVM;
using static MonPDReborn.Models.EvaluasiTarget.KontrolPembayaranVM.Method;

namespace MonPDReborn.Models.EvaluasiTarget
{
    public class KontrolPembayaranVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }
        }

        //public class ShowHotel
        //{
        //    public int Tahun { get; set; }

        //    public List<Hotel> DataHotelList { get; set; } = new();

        //    public ShowHotel()
        //    {
        //        DataHotelList = Method.GetDataHotelList("", 2025);
        //    }
        //    public ShowHotel(string keyword, int tahun)
        //    {
        //        DataHotelList = Method.GetDataHotelList(keyword, tahun);
        //    }
        //}

        //public class ShowRestoran
        //{
        //    public int Tahun { get; set; }

        //    public List<Restoran> DataRestoranList { get; set; } = new();

        //    public ShowRestoran()
        //    {
        //        DataRestoranList = Method.GetDataRestoranList("", 2025);
        //    }
        //    public ShowRestoran(string keyword, int tahun)
        //    {
        //        DataRestoranList = Method.GetDataRestoranList(keyword, tahun);
        //    }
        //}

        public class Show
        {
            public int Tahun { get; set; }

            public List<KontrolPembayaran> DataKontrolPembayaranList { get; set; } = new();

            public Show()
            {
                Tahun = 2025;
                DataKontrolPembayaranList = Method.GetDataKontolPembayaranList("", Tahun);
            }

            public Show(string JenisPajak, int tahun)
            {
                Tahun = tahun;
                DataKontrolPembayaranList = Method.GetDataKontolPembayaranList(JenisPajak, tahun);
            }
        }


        public class Detail
        {
            public Detail()
            {

            }
            public Detail(string nop)
            {
            }
        }
        public class Method
        {
            public static List<KontrolPembayaran> GetDataKontolPembayaranList(string JenisPajak, int tahun)
            {
                var allData = GetAllData();

                return allData
                    .Where(d =>
                        (string.IsNullOrWhiteSpace(JenisPajak) ||
                         (d.JenisPajak != null && d.JenisPajak.Contains(JenisPajak, StringComparison.OrdinalIgnoreCase)))
                        && d.Tahun == tahun)
                    .ToList();
            }

            private static List<KontrolPembayaran> GetAllData()
            {
                return new List<KontrolPembayaran>
                {
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG LIMA",  Tahun = 2025, OPbuka1 = 12, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG EMPAT", Tahun = 2025, OPbuka1 = 20, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG TIGA",  Tahun = 2025, OPbuka1 = 30, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG DUA",   Tahun = 2025, OPbuka1 = 40, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG SATU",  Tahun = 2025, OPbuka1 = 50, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL NON BINTANG",   Tahun = 2025, OPbuka1 = 60, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },

                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG LIMA",  Tahun = 2024, OPbuka1 = 10, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG EMPAT", Tahun = 2024, OPbuka1 = 10, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG TIGA",  Tahun = 2024, OPbuka1 = 10, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG DUA",   Tahun = 2024, OPbuka1 = 10, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL BINTANG SATU",  Tahun = 2024, OPbuka1 = 10, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "HOTEL",Kategori = "HOTEL NON BINTANG",   Tahun = 2024, OPbuka1 = 10, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },

                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "RESTORAN",     Tahun = 2025, OPbuka1 = 3, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "KEDAI/DEPOT",  Tahun = 2025, OPbuka1 = 3, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "RUMAH MAKAN",  Tahun = 2025, OPbuka1 = 3, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "CAFE",         Tahun = 2025, OPbuka1 = 3, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "FAST FOOD",    Tahun = 2025, OPbuka1 = 3, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "CATERING",     Tahun = 2025, OPbuka1 = 3, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },

                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "RESTORAN",     Tahun = 2024, OPbuka1 = 8, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "KEDAI/DEPOT",  Tahun = 2024, OPbuka1 = 8, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "RUMAH MAKAN",  Tahun = 2024, OPbuka1 = 8, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "CAFE",         Tahun = 2024, OPbuka1 = 8, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "FAST FOOD",    Tahun = 2024, OPbuka1 = 8, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
                    new KontrolPembayaran {JenisPajak = "RESTORAN",Kategori = "CATERING",     Tahun = 2024, OPbuka1 = 8, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },
                };
            }

            //public static List<Hotel> GetDataHotelList(string keyword, int tahun)
            //{
            //    var allData = GetAllData();

            //    return allData
            //        .Where(d =>
            //            (string.IsNullOrWhiteSpace(keyword) ||
            //             (d.Kategori != null && d.Kategori.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            //            && d.Tahun == tahun)
            //        .ToList();
            //}
            //private static List<Hotel> GetAllData()
            //{
            //    return new List<Hotel>
            //    {
            //        new Hotel {Kategori = "HOTEL BINTANG LIMA",  Tahun = 2025, OPbuka1 = 12, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
            //        new Hotel {Kategori = "HOTEL BINTANG EMPAT", Tahun = 2025, OPbuka1 = 20, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
            //        new Hotel {Kategori = "HOTEL BINTANG TIGA",  Tahun = 2025, OPbuka1 = 30, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
            //        new Hotel {Kategori = "HOTEL BINTANG DUA",   Tahun = 2025, OPbuka1 = 40, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
            //        new Hotel {Kategori = "HOTEL BINTANG SATU",  Tahun = 2025, OPbuka1 = 50, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
            //        new Hotel {Kategori = "HOTEL NON BINTANG",   Tahun = 2025, OPbuka1 = 60, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },

            //        new Hotel {Kategori = "HOTEL BINTANG LIMA",  Tahun = 2024, OPbuka1 = 10, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
            //        new Hotel {Kategori = "HOTEL BINTANG EMPAT", Tahun = 2024, OPbuka1 = 10, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
            //        new Hotel {Kategori = "HOTEL BINTANG TIGA",  Tahun = 2024, OPbuka1 = 10, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
            //        new Hotel {Kategori = "HOTEL BINTANG DUA",   Tahun = 2024, OPbuka1 = 10, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
            //        new Hotel {Kategori = "HOTEL BINTANG SATU",  Tahun = 2024, OPbuka1 = 10, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
            //        new Hotel {Kategori = "HOTEL NON BINTANG",   Tahun = 2024, OPbuka1 = 10, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },
            //    };
            //}

            //public static List<Restoran> GetDataRestoranList(string keyword, int tahun)
            //{
            //    var allData = GetRestoranAllData();

            //    return allData
            //        .Where(d =>
            //            (string.IsNullOrWhiteSpace(keyword) ||
            //             (d.Kategori != null && d.Kategori.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            //            && d.Tahun == tahun)
            //        .ToList();
            //}
            //private static List<Restoran> GetRestoranAllData()
            //{
            //    return new List<Restoran>
            //    {
            //        new Restoran {Kategori = "RESTORAN",     Tahun = 2025, OPbuka1 = 3, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
            //        new Restoran {Kategori = "KEDAI/DEPOT",  Tahun = 2025, OPbuka1 = 3, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
            //        new Restoran {Kategori = "RUMAH MAKAN",  Tahun = 2025, OPbuka1 = 3, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
            //        new Restoran {Kategori = "CAFE",         Tahun = 2025, OPbuka1 = 3, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
            //        new Restoran {Kategori = "FAST FOOD",    Tahun = 2025, OPbuka1 = 3, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
            //        new Restoran {Kategori = "CATERING",     Tahun = 2025, OPbuka1 = 3, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },

            //        new Restoran {Kategori = "RESTORAN",     Tahun = 2024, OPbuka1 = 8, Byr1 = 10, Nts1 = 1, Blm1 = 1, OPbuka2 = 11, Byr2 = 9, Nts2 = 1, Blm2 = 1, OPbuka3 = 13, Byr3 = 11, Nts3 = 1, Blm3 = 1, OPbuka4 = 12, Byr4 = 10, Nts4 = 1, Blm4 = 1, OPbuka5 = 12, Byr5 = 10, Nts5 = 1, Blm5 = 1, OPbuka6 = 12, Byr6 = 10, Nts6 = 1, Blm6 = 1, OPbuka7 = 12, Byr7 = 10, Nts7 = 1, Blm7 = 1, OPbuka8 = 12, Byr8 = 10, Nts8 = 1, Blm8 = 1, OPbuka9 = 12, Byr9 = 10, Nts9 = 1, Blm9 = 1, OPbuka10 = 12, Byr10 = 10, Nts10 = 1, Blm10 = 1, OPbuka11 = 12, Byr11 = 10, Nts11 = 1, Blm11 = 1, OPbuka12 = 12, Byr12 = 10, Nts12 = 1, Blm12 = 1 },
            //        new Restoran {Kategori = "KEDAI/DEPOT",  Tahun = 2024, OPbuka1 = 8, Byr1 = 18, Nts1 = 1, Blm1 = 1, OPbuka2 = 19, Byr2 = 17, Nts2 = 1, Blm2 = 1, OPbuka3 = 21, Byr3 = 19, Nts3 = 1, Blm3 = 1, OPbuka4 = 20, Byr4 = 18, Nts4 = 1, Blm4 = 1, OPbuka5 = 20, Byr5 = 18, Nts5 = 1, Blm5 = 1, OPbuka6 = 20, Byr6 = 18, Nts6 = 1, Blm6 = 1, OPbuka7 = 20, Byr7 = 18, Nts7 = 1, Blm7 = 1, OPbuka8 = 20, Byr8 = 18, Nts8 = 1, Blm8 = 1, OPbuka9 = 20, Byr9 = 18, Nts9 = 1, Blm9 = 1, OPbuka10 = 20, Byr10 = 18, Nts10 = 1, Blm10 = 1, OPbuka11 = 20, Byr11 = 18, Nts11 = 1, Blm11 = 1, OPbuka12 = 20, Byr12 = 18, Nts12 = 1, Blm12 = 1 },
            //        new Restoran {Kategori = "RUMAH MAKAN",  Tahun = 2024, OPbuka1 = 8, Byr1 = 27, Nts1 = 2, Blm1 = 1, OPbuka2 = 29, Byr2 = 26, Nts2 = 2, Blm2 = 1, OPbuka3 = 31, Byr3 = 28, Nts3 = 2, Blm3 = 1, OPbuka4 = 30, Byr4 = 27, Nts4 = 2, Blm4 = 1, OPbuka5 = 30, Byr5 = 27, Nts5 = 2, Blm5 = 1, OPbuka6 = 30, Byr6 = 27, Nts6 = 2, Blm6 = 1, OPbuka7 = 30, Byr7 = 27, Nts7 = 2, Blm7 = 1, OPbuka8 = 30, Byr8 = 27, Nts8 = 2, Blm8 = 1, OPbuka9 = 30, Byr9 = 27, Nts9 = 2, Blm9 = 1, OPbuka10 = 30, Byr10 = 27, Nts10 = 2, Blm10 = 1, OPbuka11 = 30, Byr11 = 27, Nts11 = 2, Blm11 = 1, OPbuka12 = 30, Byr12 = 27, Nts12 = 2, Blm12 = 1 },
            //        new Restoran {Kategori = "CAFE",         Tahun = 2024, OPbuka1 = 8, Byr1 = 36, Nts1 = 2, Blm1 = 2, OPbuka2 = 39, Byr2 = 35, Nts2 = 2, Blm2 = 2, OPbuka3 = 41, Byr3 = 37, Nts3 = 2, Blm3 = 2, OPbuka4 = 40, Byr4 = 36, Nts4 = 2, Blm4 = 2, OPbuka5 = 40, Byr5 = 36, Nts5 = 2, Blm5 = 2, OPbuka6 = 40, Byr6 = 36, Nts6 = 2, Blm6 = 2, OPbuka7 = 40, Byr7 = 36, Nts7 = 2, Blm7 = 2, OPbuka8 = 40, Byr8 = 36, Nts8 = 2, Blm8 = 2, OPbuka9 = 40, Byr9 = 36, Nts9 = 2, Blm9 = 2, OPbuka10 = 40, Byr10 = 36, Nts10 = 2, Blm10 = 2, OPbuka11 = 40, Byr11 = 36, Nts11 = 2, Blm11 = 2, OPbuka12 = 40, Byr12 = 36, Nts12 = 2, Blm12 = 2 },
            //        new Restoran {Kategori = "FAST FOOD",    Tahun = 2024, OPbuka1 = 8, Byr1 = 45, Nts1 = 3, Blm1 = 2, OPbuka2 = 49, Byr2 = 44, Nts2 = 3, Blm2 = 2, OPbuka3 = 51, Byr3 = 46, Nts3 = 3, Blm3 = 2, OPbuka4 = 50, Byr4 = 45, Nts4 = 3, Blm4 = 2, OPbuka5 = 50, Byr5 = 45, Nts5 = 3, Blm5 = 2, OPbuka6 = 50, Byr6 = 45, Nts6 = 3, Blm6 = 2, OPbuka7 = 50, Byr7 = 45, Nts7 = 3, Blm7 = 2, OPbuka8 = 50, Byr8 = 45, Nts8 = 3, Blm8 = 2, OPbuka9 = 50, Byr9 = 45, Nts9 = 3, Blm9 = 2, OPbuka10 = 50, Byr10 = 45, Nts10 = 3, Blm10 = 2, OPbuka11 = 50, Byr11 = 45, Nts11 = 3, Blm11 = 2, OPbuka12 = 50, Byr12 = 45, Nts12 = 3, Blm12 = 2 },
            //        new Restoran {Kategori = "CATERING",     Tahun = 2024, OPbuka1 = 8, Byr1 = 54, Nts1 = 4, Blm1 = 2, OPbuka2 = 59, Byr2 = 53, Nts2 = 4, Blm2 = 2, OPbuka3 = 61, Byr3 = 55, Nts3 = 4, Blm3 = 2, OPbuka4 = 60, Byr4 = 54, Nts4 = 4, Blm4 = 2, OPbuka5 = 60, Byr5 = 54, Nts5 = 4, Blm5 = 2, OPbuka6 = 60, Byr6 = 54, Nts6 = 4, Blm6 = 2, OPbuka7 = 60, Byr7 = 54, Nts7 = 4, Blm7 = 2, OPbuka8 = 60, Byr8 = 54, Nts8 = 4, Blm8 = 2, OPbuka9 = 60, Byr9 = 54, Nts9 = 4, Blm9 = 2, OPbuka10 = 60, Byr10 = 54, Nts10 = 4, Blm10 = 2, OPbuka11 = 60, Byr11 = 54, Nts11 = 4, Blm11 = 2, OPbuka12 = 60, Byr12 = 54, Nts12 = 4, Blm12 = 2 },
            //    };
            //}

        }

        public class KontrolPembayaran
        {
            public string Kategori { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }

            public int OPbuka1 { get; set; }
            public int Byr1 { get; set; }
            public int Nts1 { get; set; }
            public int Blm1 { get; set; }

            public int OPbuka2 { get; set; }
            public int Byr2 { get; set; }
            public int Nts2 { get; set; }
            public int Blm2 { get; set; }

            public int OPbuka3 { get; set; }
            public int Byr3 { get; set; }
            public int Nts3 { get; set; }
            public int Blm3 { get; set; }

            public int OPbuka4 { get; set; }
            public int Byr4 { get; set; }
            public int Nts4 { get; set; }
            public int Blm4 { get; set; }

            public int OPbuka5 { get; set; }
            public int Byr5 { get; set; }
            public int Nts5 { get; set; }
            public int Blm5 { get; set; }

            public int OPbuka6 { get; set; }
            public int Byr6 { get; set; }
            public int Nts6 { get; set; }
            public int Blm6 { get; set; }

            public int OPbuka7 { get; set; }
            public int Byr7 { get; set; }
            public int Nts7 { get; set; }
            public int Blm7 { get; set; }

            public int OPbuka8 { get; set; }
            public int Byr8 { get; set; }
            public int Nts8 { get; set; }
            public int Blm8 { get; set; }

            public int OPbuka9 { get; set; }
            public int Byr9 { get; set; }
            public int Nts9 { get; set; }
            public int Blm9 { get; set; }

            public int OPbuka10 { get; set; }
            public int Byr10 { get; set; }
            public int Nts10 { get; set; }
            public int Blm10 { get; set; }

            public int OPbuka11 { get; set; }
            public int Byr11 { get; set; }
            public int Nts11 { get; set; }
            public int Blm11 { get; set; }

            public int OPbuka12 { get; set; }
            public int Byr12 { get; set; }
            public int Nts12 { get; set; }
            public int Blm12 { get; set; }
        }

        public class Restoran
        {
            public string Kategori { get; set; } = null!;
            public int Tahun { get; set; }

            public int OPbuka1 { get; set; }
            public int Byr1 { get; set; }
            public int Nts1 { get; set; }
            public int Blm1 { get; set; }

            public int OPbuka2 { get; set; }
            public int Byr2 { get; set; }
            public int Nts2 { get; set; }
            public int Blm2 { get; set; }

            public int OPbuka3 { get; set; }
            public int Byr3 { get; set; }
            public int Nts3 { get; set; }
            public int Blm3 { get; set; }

            public int OPbuka4 { get; set; }
            public int Byr4 { get; set; }
            public int Nts4 { get; set; }
            public int Blm4 { get; set; }

            public int OPbuka5 { get; set; }
            public int Byr5 { get; set; }
            public int Nts5 { get; set; }
            public int Blm5 { get; set; }

            public int OPbuka6 { get; set; }
            public int Byr6 { get; set; }
            public int Nts6 { get; set; }
            public int Blm6 { get; set; }

            public int OPbuka7 { get; set; }
            public int Byr7 { get; set; }
            public int Nts7 { get; set; }
            public int Blm7 { get; set; }

            public int OPbuka8 { get; set; }
            public int Byr8 { get; set; }
            public int Nts8 { get; set; }
            public int Blm8 { get; set; }

            public int OPbuka9 { get; set; }
            public int Byr9 { get; set; }
            public int Nts9 { get; set; }
            public int Blm9 { get; set; }

            public int OPbuka10 { get; set; }
            public int Byr10 { get; set; }
            public int Nts10 { get; set; }
            public int Blm10 { get; set; }

            public int OPbuka11 { get; set; }
            public int Byr11 { get; set; }
            public int Nts11 { get; set; }
            public int Blm11 { get; set; }

            public int OPbuka12 { get; set; }
            public int Byr12 { get; set; }
            public int Nts12 { get; set; }
            public int Blm12 { get; set; }
        }





    }
}
