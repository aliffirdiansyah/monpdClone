using DocumentFormat.OpenXml.Presentation;
using MonPDLib;
using MonPDLib.General;

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
            public Index(string keyword)
            {
                Keyword = keyword;
            }
        }
        public class Show
        {
            public List<DataPencarianOp> DataPencarianOpList { get; set; } = new();
            public Show()
            {

            }
            public Show(string keyword)
            {
                DataPencarianOpList = Method.GetDataPencarianOpList(keyword);
            }
        }
        public class Detail
        {
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();
            public List<RealisasiBulanan> DataRealisasiBulananMines1List { get; set; } = new();
            public Detail()
            {

            }
            public Detail(string nop, EnumFactory.EPajak pajak)
            {
                DataRealisasiBulananList = Method.GetRealisasiDetail(nop, pajak);
            }
        }
        public class Method
        {
            public static List<DataPencarianOp> GetDataPencarianOpList(string keyword)
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    throw new ArgumentException("Keyword harus diisi");
                }
                if (keyword.Length < 3)
                {
                    throw new ArgumentException("Keyword harus diisi minimal 3");
                }
                var context = DBClass.GetContext();
                var ret = new List<DataPencarianOp>();
                var dataResto = context.DbOpRestos.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()) && x.TahunBuku == DateTime.Now.Year)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? "",
                            EnumPajak = (int)EnumFactory.EPajak.MakananMinuman
                        }
                    ).ToList();
                ret.AddRange(dataResto);

                var dataHotel = context.DbOpHotels.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()) && x.TahunBuku == DateTime.Now.Year)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? "",
                            EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan
                        }
                    ).ToList();
                ret.AddRange(dataHotel);

                var dataHiburan = context.DbOpHiburans.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()) && x.TahunBuku == DateTime.Now.Year)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? "",
                            EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan
                        }
                    ).ToList();
                ret.AddRange(dataHiburan);

                var dataParkir = context.DbOpParkirs.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()) && x.TahunBuku == DateTime.Now.Year)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? "",
                            EnumPajak = (int)EnumFactory.EPajak.JasaParkir
                        }
                    ).ToList();
                ret.AddRange(dataParkir);

                var dataListrik = context.DbOpListriks.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()) && x.TahunBuku == DateTime.Now.Year)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama??"",
                            JenisPenarikan = "",
                            StatusNOP = x.IsTutup == 1 ? "Tutup" : "Buka",
                            Wilayah = x.WilayahPajak ?? "",
                            EnumPajak = (int)EnumFactory.EPajak.TenagaListrik
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
                            KategoriOp = "Reklame",
                            JenisPenarikan = "",
                            StatusNOP = "-",
                            Wilayah = "-",
                            EnumPajak = (int)EnumFactory.EPajak.Reklame
                        }
                    ).ToList();
                ret.AddRange(dataReklame);
                

                var dataAbt = context.DbOpAbts.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()) && x.TahunBuku == DateTime.Now.Year)).Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = x.PajakNama,
                            KategoriOp = x.KategoriNama ?? "",
                            JenisPenarikan = "",
                            StatusNOP = "-",
                            Wilayah = "-",
                            EnumPajak = (int)EnumFactory.EPajak.AirTanah
                        }
                    ).ToList();
                ret.AddRange(dataAbt);


                return ret;
            }

            public static List<RealisasiBulanan> GetRealisasiDetail(string nop, EnumFactory.EPajak pajak)
            {
                var context = DBClass.GetContext();
                var ret = new List<RealisasiBulanan>();

                if (string.IsNullOrWhiteSpace(nop))
                {
                    throw new ArgumentException("NOP harus diisi");
                }

                if (pajak == EnumFactory.EPajak.MakananMinuman)
                {
                    //ret = context.DbMonRestos
                    //    .Where(x => x.Nop == nop)
                    //    .Select(x => new RealisasiBulanan
                    //    {
                    //        NOP = x.Nop,
                    //        Bulan = x.MasaPajakKetetapan,
                    //        Nominal = x.Nominal
                    //    }).ToList();
                }


                return ret;
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
            public int EnumPajak { get; set; }
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Bulan { get; set; } = null!;
            public decimal Nominal { get; set; }
        }
    }
}
