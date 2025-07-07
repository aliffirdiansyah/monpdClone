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
                    new(){EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan, JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),Tahun = 2025, OPAwal = 60, OPTutup = 2, OPBaru = 18, OPAkhir = 76},
                    new(){EnumPajak = (int)EnumFactory.EPajak.MakananMinuman, JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),Tahun = 2025, OPAwal = 80, OPTutup = 3, OPBaru = 15, OPAkhir = 90},
                    new(){EnumPajak = (int)EnumFactory.EPajak.JasaParkir, JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(), Tahun = 2025, OPAwal = 25, OPTutup = 0, OPBaru = 4, OPAkhir = 28},
                    new(){EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan, JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),Tahun = 2024, OPAwal = 20, OPTutup = 2, OPBaru = 18, OPAkhir = 76},
                    new(){EnumPajak = (int)EnumFactory.EPajak.MakananMinuman, JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),Tahun = 2024, OPAwal = 10, OPTutup = 3, OPBaru = 15, OPAkhir = 90},
                    new(){EnumPajak = (int)EnumFactory.EPajak.JasaParkir, JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(), Tahun = 2024, OPAwal = 14, OPTutup = 0, OPBaru = 4, OPAkhir = 28},
                };
            }

            private static List<SeriesOP> GetAllSeriesData()
            {
                return new List<SeriesOP>
                {
                    new() {EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan, JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(), Tahun2021 = 50, Tahun2022 = 57, Tahun2023 = 52, Tahun2024 = 59, Tahun2025 = 60 },
                    new() {EnumPajak = (int)EnumFactory.EPajak.MakananMinuman, JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(), Tahun2021 = 70, Tahun2022 = 75, Tahun2023 = 80, Tahun2024 = 85, Tahun2025 = 90 },
                    new() {EnumPajak = (int)EnumFactory.EPajak.JasaParkir, JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(), Tahun2021 = 20, Tahun2022 = 22, Tahun2023 = 24, Tahun2024 = 26, Tahun2025 = 28 },
                };
            }

            public static List<RekapDetail> GetRekapDetailData(EnumFactory.EPajak JenisPajak, int tahun)
            {
                var ret = new List<RekapDetail>();
                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        ret = new List<RekapDetail>
                        {
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Tahun = tahun, Kategori = "Hotel Bintang Lima", OPAwal = 20, OPTutup = 1, OPBaru = 5, OPAkhir = 8 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Tahun = tahun, Kategori = "Hotel Bintang Empat", OPAwal = 15, OPTutup = 8, OPBaru = 4, OPAkhir = 6 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Tahun = tahun, Kategori = "Hotel Bintang Tiga", OPAwal = 25, OPTutup = 1, OPBaru = 5, OPAkhir = 4 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Tahun = tahun, Kategori = "Hotel Bintang Dua", OPAwal = 10, OPTutup = 2, OPBaru = 7, OPAkhir = 9 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Tahun = tahun, Kategori = "Hotel Bintang Satu", OPAwal = 30, OPTutup = 3, OPBaru = 7, OPAkhir = 10 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Tahun = tahun, Kategori = "Hotel Non Bintang", OPAwal = 40, OPTutup = 5, OPBaru = 8, OPAkhir = 15 },
                        };
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
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

                return ret; // ✅ Tambahkan ini agar semua jalur selalu mengembalikan nilai

            }

            public static List<SeriesDetail> GetSeriesDetailData(EnumFactory.EPajak JenisPajak)
            {
                var ret = new List<SeriesDetail>();
                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        ret = new List<SeriesDetail>
                        {
                            new() { EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Kategori = "Cafe", TahunMines4 = 12, TahunMines3 = 4, TahunMines2 = 6, TahunMines1 = 7, TahunNow = 9 },
                            new() { EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Kategori = "Resto", TahunMines4 = 18, TahunMines3 = 6, TahunMines2 = 9, TahunMines1 = 8, TahunNow = 10 },
                            new() { EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Kategori = "Warung", TahunMines4 = 25, TahunMines3 = 7, TahunMines2 = 10, TahunMines1 = 12, TahunNow = 15 },
                            new() { EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Kategori = "Resto", TahunMines4 = 20, TahunMines3 = 5, TahunMines2 = 8, TahunMines1 = 9, TahunNow = 11 },
                            new() { EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Kategori = "Kedai", TahunMines4 = 28, TahunMines3 = 8, TahunMines2 = 12, TahunMines1 = 13, TahunNow = 16 },
                            new() { EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Kategori = "Warung Makan", TahunMines4 = 35, TahunMines3 = 10, TahunMines2 = 15, TahunMines1 = 16, TahunNow = 20 },
                            new() { EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(), Kategori = "Kafe & Kedai", TahunMines4 = 22, TahunMines3 = 6, TahunMines2 = 11, TahunMines1 = 12, TahunNow = 14 },
                        };

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        ret = new List<SeriesDetail>
                        {
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(),  Kategori = "Hotel Bintang Lima", TahunMines4 = 50, TahunMines3 = 57, TahunMines2 = 52, TahunMines1 = 59, TahunNow = 60 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(),  Kategori = "Hotel Bintang Empat", TahunMines4 = 15, TahunMines3 = 5, TahunMines2 = 8, TahunMines1 = 4, TahunNow = 6 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(),  Kategori = "Hotel Bintang Tiga", TahunMines4 = 25, TahunMines3 = 0, TahunMines2 = 1, TahunMines1 = 5, TahunNow = 4 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(),  Kategori = "Hotel Bintang Dua", TahunMines4 = 10, TahunMines3 = 2, TahunMines2 = 2, TahunMines1 = 7, TahunNow = 9 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(),  Kategori = "Hotel Bintang Satu", TahunMines4 = 30, TahunMines3 = 4, TahunMines2 = 3, TahunMines1 = 7, TahunNow = 10 },
                            new() {EnumPajak = (int)JenisPajak, JenisPajak = JenisPajak.GetDescription(),  Kategori = "Hotel Non Bintang", TahunMines4 = 40, TahunMines3 = 8, TahunMines2 = 5, TahunMines1 = 8, TahunNow = 15 },
                        };
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
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

                return ret; // ✅ Tambahkan ini agar semua jalur selalu mengembalikan nilai

            }

            public static List<RekapMaster> GetRekapMasterData(EnumFactory.EPajak JenisPajak)
            {
                var ret = new List<RekapMaster>();
                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        ret = new List<RekapMaster>
                        {
                            new() {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 101,
                                Kategori_Nama = "Hotel Bintang Lima",
                                NOP = "32.71.040.001.123-4567.0",
                                NamaOP = "Hotel Mewah Raya",
                                Alamat = "Jl. Asia Afrika No. 1, Bandung",
                                JenisOP = "Bangunan Komersial",
                                Wilayah = "Kecamatan Sumur Bandung",
                                Status = "OPAwal"
                            },
                            new ()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 101,
                                Kategori_Nama = "Hotel Bintang Lima",
                                NOP = "32.71.040.001.234-5678.0",
                                NamaOP = "Hotel Elegan Sejahtera",
                                Alamat = "Jl. Merdeka No. 100, Bandung",
                                JenisOP = "Bangunan Komersial",
                                Wilayah = "Kecamatan Cicendo",
                                Status = "TutupSementara"
                            },
                            new ()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 102,
                                Kategori_Nama = "Hotel Bintang Empat",
                                NOP = "32.71.040.002.345-6789.0",
                                NamaOP = "Hotel Harmoni Indah",
                                Alamat = "Jl. Setiabudi No. 88, Bandung",
                                JenisOP = "Bangunan Komersial",
                                Wilayah = "Kecamatan Cidadap",
                                Status = "TutupPermanen"
                            },
                            new ()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 103,
                                Kategori_Nama = "Hotel Melati",
                                NOP = "32.71.040.003.456-7890.0",
                                NamaOP = "Hotel Bumi Asri",
                                Alamat = "Jl. Cihampelas No. 22, Bandung",
                                JenisOP = "Bangunan Komersial",
                                Wilayah = "Kecamatan Coblong",
                                Status = "OPBaru"
                            },
                            new ()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 103,
                                Kategori_Nama = "Hotel Melati",
                                NOP = "32.71.040.003.567-8901.0",
                                NamaOP = "Hotel Citra Lestari",
                                Alamat = "Jl. Pasteur No. 5, Bandung",
                                JenisOP = "Bangunan Komersial",
                                Wilayah = "Kecamatan Sukajadi",
                                Status = "Buka"
                            }
                        };
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
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

                return ret; // ✅ Tambahkan ini agar semua jalur selalu mengembalikan nilai

            }

            public static List<SeriesMaster> GetSeriesMasterData(EnumFactory.EPajak JenisPajak)
            {
                var ret = new List<SeriesMaster>();
                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        ret = new List<SeriesMaster>
                        {
                            new() {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 201,
                                Kategori_Nama = "Hotel Bintang Lima",
                                NOP = "32.71.050.001.100-2000.0",
                                NamaOP = "Grand Royal Hotel",
                                Alamat = "Jl. Braga No. 10, Bandung",
                                JenisOP = "Hotel Mewah",
                                Wilayah = "Kecamatan Sumur Bandung",
                            },
                            new()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 201,
                                Kategori_Nama = "Hotel Bintang Lima",
                                NOP = "32.71.050.001.101-2001.0",
                                NamaOP = "Majestic Palace Hotel",
                                Alamat = "Jl. Asia Afrika No. 25, Bandung",
                                JenisOP = "Hotel Mewah",
                                Wilayah = "Kecamatan Lengkong",
                            },
                            new()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 202,
                                Kategori_Nama = "Hotel Bintang Empat",
                                NOP = "32.71.050.002.102-2002.0",
                                NamaOP = "Dago Hills Hotel",
                                Alamat = "Jl. Dago No. 120, Bandung",
                                JenisOP = "Hotel Bisnis",
                                Wilayah = "Kecamatan Coblong",
                            },
                            new()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 203,
                                Kategori_Nama = "Hotel Bintang Tiga",
                                NOP = "32.71.050.003.103-2003.0",
                                NamaOP = "Hotel Bahagia",
                                Alamat = "Jl. Ciumbuleuit No. 88, Bandung",
                                JenisOP = "Hotel Budget",
                                Wilayah = "Kecamatan Cidadap",
                            },
                            new()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = 203,
                                Kategori_Nama = "Hotel Bintang Tiga",
                                NOP = "32.71.050.003.104-2004.0",
                                NamaOP = "Hotel Sunset View",
                                Alamat = "Jl. Setiabudi No. 55, Bandung",
                                JenisOP = "Hotel Budget",
                                Wilayah = "Kecamatan Sukasari",
                            }
                        };
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
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

                return ret; // ✅ Tambahkan ini agar semua jalur selalu mengembalikan nilai

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
            public int OPAwal { get; set; }
            public int OPTutup { get; set; }
            public int OPBaru { get; set; }
            public int OPAkhir { get; set; }

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
            public string Kategori { get; set; } = null!;
            public int Tahun { get; set; }
            public int OPAwal { get; set; }
            public int OPTutup { get; set; }
            public int OPBaru { get; set; }
            public int OPAkhir { get; set; }
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
            public string Status { get; set; } = null!;

        }

        public class SeriesDetail
        {
            public int EnumPajak { get; set; }

            public string JenisPajak { get; set; } = null!;
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
