using MonPDLib;
using static MonPDReborn.Models.AktivitasOP.PemasanganAlatVM;
using MonPDLib.General;
using static MonPDReborn.Models.DataOP.ProfilePembayaranOPVM;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileOPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
        }

        public class ShowRekap
        {
            public List<RekapOP> DataRekapOPList { get; set; } = new();

            public ShowRekap() { }

            public ShowRekap(string keyword, int tahun)
            {
                DataRekapOPList = Method.GetDataRekapOPList(keyword, tahun);
            }
        }



        public class ShowSeries
        {
            public List<SeriesOP> DataSeriesOPList { get; set; } = new();

            public ShowSeries() { }

            public ShowSeries(string keyword)
            {
                DataSeriesOPList = Method.GetDataSeriesOPList(keyword);
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
            public static List<RekapOP> GetDataRekapOPList(string keyword, int tahun)
            {
                var allData = GetAllData()
                    .Where(d => d.Tahun == tahun) // ✅ FILTER tahun
                    .ToList();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    allData = allData
                        .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                return allData;
            }





            public static List<SeriesOP> GetDataSeriesOPList(string keyword)
            {
                var allData = GetAllSeriesData();
                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<RekapOP> GetAllData()
            {
                return new List<RekapOP>
                {
                    new(){JenisPajak = "Pajak Hotel",Tahun = 2025, OPAwal = 60, TutupSementara = 0, TutupPermanen = 2, OPBaru = 18, Buka = 76},
                    new(){JenisPajak = "Pajak Restoran",Tahun = 2025, OPAwal = 80, TutupSementara = 2, TutupPermanen = 3, OPBaru = 15, Buka = 90},
                    new(){JenisPajak = "Pajak Parkir", Tahun = 2025, OPAwal = 25, TutupSementara = 1, TutupPermanen = 0, OPBaru = 4, Buka = 28},
                    new(){JenisPajak = "Pajak Hotel",Tahun = 2024, OPAwal = 20, TutupSementara = 0, TutupPermanen = 2, OPBaru = 18, Buka = 76},
                    new(){JenisPajak = "Pajak Restoran",Tahun = 2024, OPAwal = 10, TutupSementara = 2, TutupPermanen = 3, OPBaru = 15, Buka = 90},
                    new(){JenisPajak = "Pajak Parkir", Tahun = 2024, OPAwal = 14, TutupSementara = 1, TutupPermanen = 0, OPBaru = 4, Buka = 28},
                };
            }

            private static List<SeriesOP> GetAllSeriesData()
            {
                return new List<SeriesOP>
                {
                    new() { JenisPajak = "Pajak Hotel", Tahun2021 = 50, Tahun2022 = 57, Tahun2023 = 52, Tahun2024 = 59, Tahun2025 = 60 },
                    new() { JenisPajak = "Pajak Restoran", Tahun2021 = 70, Tahun2022 = 75, Tahun2023 = 80, Tahun2024 = 85, Tahun2025 = 90 },
                    new() { JenisPajak = "Pajak Parkir", Tahun2021 = 20, Tahun2022 = 22, Tahun2023 = 24, Tahun2024 = 26, Tahun2025 = 28 },
                };
            }

            //get Data OP
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
                            //ret.RestoRow.OperasionalRestoDetailList = context.DbOpRestoOperasionals
                            //    .Where(x => x.Nop == nop)
                            //    .Select(x => new DetailResto.DetailOperasional
                            //    {
                            //        //isi data operasional resto jika ada
                            //    }).ToList();
            public static List<RekapDetail> GetRekapDetailData(int JenisPajak, int Tahun)
            {
                return new List<RekapDetail>
                {
                    new RekapDetail { JenisPajak = "Pajak Hotel",Tahun = 2025, Kategori = "Hotel Bintang Lima", OPAwal = 20, TutupSementara = 0, TutupPermanen = 1, OPBaru = 5, Buka = 8 },
                    new RekapDetail { JenisPajak = "Pajak Hotel",Tahun = 2025, Kategori = "Hotel Bintang Empat", OPAwal = 15, TutupSementara = 5, TutupPermanen = 8, OPBaru = 4, Buka = 6 },
                    new RekapDetail { JenisPajak = "Pajak Hotel",Tahun = 2025, Kategori = "Hotel Bintang Tiga", OPAwal = 25, TutupSementara = 0, TutupPermanen = 1, OPBaru = 5, Buka = 4 },
                    new RekapDetail { JenisPajak = "Pajak Hotel",Tahun = 2025, Kategori = "Hotel Bintang Dua", OPAwal = 10, TutupSementara = 2, TutupPermanen = 2, OPBaru = 7, Buka = 9 },
                    new RekapDetail { JenisPajak = "Pajak Hotel",Tahun = 2025, Kategori = "Hotel Bintang Satu", OPAwal = 30, TutupSementara = 4, TutupPermanen = 3, OPBaru = 7, Buka = 10 },
                    new RekapDetail { JenisPajak = "Pajak Hotel",Tahun = 2025, Kategori = "Hotel Non Bintang", OPAwal = 40, TutupSementara = 8, TutupPermanen = 5, OPBaru = 8, Buka = 15 },

                };
            }
            /*  public static List<DetailPemasanganAlat> GetDetailData(string jenisPajak)
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


                  };
              }*/

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
                        break;

                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var opHiburan = context.DbOpHiburans.FirstOrDefault(x => x.Nop == nop);
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
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }             // ✅ Tambahan field tahun

            public int OPAwal { get; set; }
            public int TutupSementara { get; set; }
            public int TutupPermanen { get; set; }
            public int OPBaru { get; set; }
            public int Buka { get; set; }

        }

        public class SeriesOP
        {
            public string JenisPajak { get; set; } = null!;
            public int Tahun2021 { get; set; }
            public int Tahun2022 { get; set; }
            public int Tahun2023 { get; set; }
            public int Tahun2024 { get; set; }
            public int Tahun2025 { get; set; }
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
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public int Tahun { get; set; }
            public int OPAwal { get; set; }
            public int TutupSementara { get; set; }
            public int TutupPermanen { get; set; }
            public int OPBaru { get; set; }
            public int Buka { get; set; }
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
                public string Hari {  get; set; }
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
                public int JumlahKursi {  get; set; }
                public int JumlahMeja {  get; set; }
                public int KapasitasRuangan {  get; set; }
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
