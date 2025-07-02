using MonPDLib;
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

            public ShowRekap(string keyword)
            {
                DataRekapOPList = Method.GetDataRekapOPList(keyword);
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
            public Detail()
            {

            }
            public Detail(string nop, EnumFactory.EPajak pajak)
            {
                DataDetail = Method.GetDetailObjekPajak(nop, pajak);
            }
        }

        public class Method
        {
            public static List<RekapOP> GetDataRekapOPList(string keyword)
            {
                var allData = GetAllData();
                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.JenisPajak != null && d.JenisPajak.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
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
                    new(){JenisPajak = "Pajak Hotel", OPAwal = 60, TutupSementara = 0, TutupPermanen = 2, OPBaru = 18, Buka = 76},
                    new(){JenisPajak = "Pajak Restoran", OPAwal = 80, TutupSementara = 2, TutupPermanen = 3, OPBaru = 15, Buka = 90},
                    new(){JenisPajak = "Pajak Parkir", OPAwal = 25, TutupSementara = 1, TutupPermanen = 0, OPBaru = 4, Buka = 28},
                };
            }

            private static List<SeriesOP> GetAllSeriesData()
            {
                return new List<SeriesOP>
                {
                    new() { JenisPajak = "PAJAK HOTEL", Tahun2021 = 50, Tahun2022 = 57, Tahun2023 = 52, Tahun2024 = 59, Tahun2025 = 60 },
                    new() { JenisPajak = "PAJAK RESTORAN", Tahun2021 = 70, Tahun2022 = 75, Tahun2023 = 80, Tahun2024 = 85, Tahun2025 = 90 },
                    new() { JenisPajak = "PAJAK PARKIR", Tahun2021 = 20, Tahun2022 = 22, Tahun2023 = 24, Tahun2024 = 26, Tahun2025 = 28 },
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

            }
            public class DetailFasilitas
            {
                public string Nama { get; set; }
                public int Jumlah { get; set; }
                public int Kapasitas { get; set; }
            }
            public class DetailKapasitas
            {

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
