using MonPDLib;

namespace MonPDReborn.Models.DataOP
{
    public class PencarianOPVM
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
            public List<DataRealisasiOp> DataRealisasiOpList { get; set; } = new();
            public Show()
            {

            }
            public Show(string keyword)
            {
                DataRealisasiOpList = Method.GetDataRealisasiOpList(keyword);
            }
        }
        public class Detail
        {
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();
            public Detail()
            {

            }
            public Detail(string nop)
            {
                DataRealisasiBulananList = Method.GetDetailByNOP(nop);
            }
        }
        public class Method
        {
            public static List<DataPencarianOp> GetDataRealisasiOpList(string keyword)
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    new ArgumentException("Keyword harus diisi");
                }
                if (keyword.Length <= 3)
                {
                    new ArgumentException("Keyword harus diisi minimal 3 ");
                }
                var ret = GetDataPencarian(keyword);


                return ret;
            }

            public static List<RealisasiBulanan> GetDetailByNOP(string nop)
            {
                var ret = GetRealisasiDetail(nop);
                return ret;
            }

            // Internal dummy data
            private static List<DataPencarianOp> GetDataPencarian(string keyword)
            {
                var context = DBClass.GetContext();
                var ret = new List<DataPencarianOp>();
                var dataResto = context.DbOpRestos.Where(x => x.Nop == keyword || x.NamaOp.Contains(keyword)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? ""
                        }
                    ).ToList();
                ret.AddRange(dataResto);

                var dataHotel = context.DbOpHotels.Where(x => x.Nop == keyword || x.NamaOp.Contains(keyword)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? ""
                        }
                    ).ToList();
                ret.AddRange(dataHotel);

                var dataHiburan = context.DbOpHiburans.Where(x => x.Nop == keyword || x.NamaOp.Contains(keyword)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? ""
                        }
                    ).ToList();
                ret.AddRange(dataHiburan);

                var dataParkir = context.DbOpParkirs.Where(x => x.Nop == keyword || x.NamaOp.Contains(keyword)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? ""
                        }
                    ).ToList();
                ret.AddRange(dataParkir);

                var dataListrik = context.DbOpListriks.Where(x => x.Nop == keyword || x.NamaOp.Contains(keyword)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = "PPJ",
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? ""
                        }
                    ).ToList();
                ret.AddRange(dataListrik);

                var dataReklame = context.DbOpReklames.Where(x => x.Nop == keyword || x.Nama.Contains(keyword)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.Nama,
                            Alamat = x.Alamat,
                            JenisOp = x.NamaJenis,
                            JenisPenarikan = "",
                            StatusNOP = "-",
                            Wilayah = "-"
                        }
                    ).ToList();
                ret.AddRange(dataReklame);


                return ret;
            }

            private static List<RealisasiBulanan> GetRealisasiDetail(string nop)
            {
                return new List<RealisasiBulanan>
                {
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Jan", Nominal = 186020436 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Feb", Nominal = 152000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Mar", Nominal = 173000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Apr", Nominal = 165000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Mei", Nominal = 178000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Jun", Nominal = 181000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Jul", Nominal = 190000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Agt", Nominal = 200000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Sep", Nominal = 210000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Okt", Nominal = 220000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Nov", Nominal = 230000000 },
                    new() { NOP = "35.78.170.005.902.00066", Bulan = "Des", Nominal = 240000000 },

                    new() { NOP = "35.78.100.002.902.00172", Bulan = "Jan", Nominal = 30222959 },
                    new() { NOP = "35.78.100.002.902.00172", Bulan = "Feb", Nominal = 25000000 },
                    new() { NOP = "35.78.100.002.902.00172", Bulan = "Mar", Nominal = 27000000 },

                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Jan", Nominal = 134483411 },
                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Feb", Nominal = 140000000 },
                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Mar", Nominal = 150000000 },
                    new() { NOP = "35.78.050.005.902.00124", Bulan = "Des", Nominal = 155000000 }
                };
            }
        }
        public class DataPencarianOp
        {
            //public int No { get; set; }
            public string Wilayah { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string StatusNOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOp { get; set; } = null!;
            public string KategoriOp { get; set; } = null!;
            public string JenisPenarikan { get; set; } = null!;
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Bulan { get; set; } = null!;
            public decimal Nominal { get; set; }
        }
    }
}
