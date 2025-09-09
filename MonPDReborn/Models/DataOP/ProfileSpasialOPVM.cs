using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Text.Json;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileSpasialOPVM
    {
        public class Index
        {
            public string MapDataJSON { get; set; } = "";
            public List<UPTBViewModel> Uptbs { get; set; } = new List<UPTBViewModel>();
            public Index()
            {
                var data = Method.GetDataOpAll();
                MapDataJSON = JsonSerializer.Serialize(data);
                Uptbs = new List<UPTBViewModel>
                {
                    new UPTBViewModel { Id = "1", Nama = "UPTB 1" },
                    new UPTBViewModel { Id = "2", Nama = "UPTB 2" },
                    new UPTBViewModel { Id = "3", Nama = "UPTB 3" },
                    new UPTBViewModel { Id = "4", Nama = "UPTB 4" },
                    new UPTBViewModel { Id = "5", Nama = "UPTB 5" }
                };
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
            public static List<DataRealisasiOp> GetDataRealisasiOpList(string keyword)
            {
                var allData = GetAllData();

                if (string.IsNullOrWhiteSpace(keyword))
                    return allData;

                return allData
                    .Where(d => d.Nama != null && d.Nama.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            public static List<RealisasiBulanan> GetDetailByNOP(string nop)
            {
                var allDetail = GetAllDetail();
                return allDetail.Where(x => x.NOP == nop).ToList();
            }

            // Internal dummy data
            private static List<DataRealisasiOp> GetAllData()
            {
                return new List<DataRealisasiOp>
                {
                    new() { No = 1, Wilayah = "01", NOP = "35.78.170.005.902.00066", StatusNOP = "Buka", Nama = "MC. DONALDS", Alamat = "RAJAWALI NO.47", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TS" },
                    new() { No = 2, Wilayah = "01", NOP = "35.78.100.002.902.00172", StatusNOP = "Buka", Nama = "MC. DONALDS KIOS", Alamat = "BUBUTAN 1-7 (BG JUNCTION LT.GL DAN LT.LL)", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TS" },
                    new() { No = 3, Wilayah = "01", NOP = "35.78.160.001.902.05140", StatusNOP = "Tutup Permanen", Nama = "MC. DONALDS", Alamat = "MALL PASAR ATUM", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TIDAK TERPASANG" },
                    new() { No = 4, Wilayah = "01", NOP = "35.78.170.005.902.01044", StatusNOP = "Tutup Permanen", Nama = "MC. DONALDS", Alamat = "JL. TAMAN JAYENGRONO (JMP)", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TIDAK TERPASANG" },
                    new() { No = 5, Wilayah = "02", NOP = "35.78.050.005.902.00124", StatusNOP = "Buka", Nama = "MC. DONALDS", Alamat = "DR. IR. H. SOEKARNO NO.218", JenisOp = "RESTORAN (RESTORAN)", JenisPenarikan = "TS" }
                };
            }

            private static List<RealisasiBulanan> GetAllDetail()
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

            public static List<MAPViewModel> GetDataOpAll()
            {
                var result = new List<MAPViewModel>();

                var context = DBClass.GetContext();

                
                var queryHotelLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).ToList();
                foreach (var item in queryHotelLocation)
                {
                    var op = context.DbOpHotels.FirstOrDefault(x => x.Nop == item.FkNop);
                    var res = new MAPViewModel();

                    res.FK_NOP = op?.Nop ?? string.Empty;
                    res.KATEGORI_STATUS = op?.KategoriNama ?? "-";
                    res.NAMA_OP = op?.NamaOp ?? "-";
                    res.ALAMAT_OP = op?.AlamatOp ?? "-";
                    res.UPTB = op?.WilayahPajak ?? "-";
                    res.LATITUDE = item?.Latitude ?? string.Empty;
                    res.LONGITUDE = item?.Longitude ?? string.Empty;
                    res.FK_PAJAK_DAERAH = ((int)EnumFactory.EPajak.JasaPerhotelan).ToString();
                    res.COLOR_MARKER = "blue";

                    result.Add(res);
                }

                var queryRestoLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).ToList();
                foreach (var item in queryRestoLocation)
                {
                    var op = context.DbOpRestos.FirstOrDefault(x => x.Nop == item.FkNop);
                    var res = new MAPViewModel();

                    res.FK_NOP = op?.Nop ?? string.Empty;
                    res.KATEGORI_STATUS = op?.KategoriNama ?? "-";
                    res.NAMA_OP = op?.NamaOp ?? "-";
                    res.ALAMAT_OP = op?.AlamatOp ?? "-";
                    res.UPTB = op?.WilayahPajak ?? "-";
                    res.LATITUDE = item?.Latitude ?? string.Empty;
                    res.LONGITUDE = item?.Longitude ?? string.Empty;
                    res.FK_PAJAK_DAERAH = ((int)EnumFactory.EPajak.MakananMinuman).ToString();
                    res.COLOR_MARKER = "red";

                    result.Add(res);
                }

                var queryHiburanLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).ToList();
                foreach (var item in queryHiburanLocation)
                {
                    var op = context.DbOpHiburans.FirstOrDefault(x => x.Nop == item.FkNop);
                    var res = new MAPViewModel();

                    res.FK_NOP = op?.Nop ?? string.Empty;
                    res.KATEGORI_STATUS = op?.KategoriNama ?? "-";
                    res.NAMA_OP = op?.NamaOp ?? "-";
                    res.ALAMAT_OP = op?.AlamatOp ?? "-";
                    res.UPTB = op?.WilayahPajak ?? "-";
                    res.LATITUDE = item?.Latitude ?? string.Empty;
                    res.LONGITUDE = item?.Longitude ?? string.Empty;
                    res.FK_PAJAK_DAERAH = ((int)EnumFactory.EPajak.JasaKesenianHiburan).ToString();
                    res.COLOR_MARKER = "green";

                    result.Add(res);
                }

                var queryPpjLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).ToList();
                foreach (var item in queryPpjLocation)
                {
                    var op = context.DbOpListriks.FirstOrDefault(x => x.Nop == item.FkNop);
                    var res = new MAPViewModel();

                    res.FK_NOP = op?.Nop ?? string.Empty;
                    res.KATEGORI_STATUS = op?.KategoriNama ?? "-";
                    res.NAMA_OP = op?.NamaOp ?? "-";
                    res.ALAMAT_OP = op?.AlamatOp ?? "-";
                    res.UPTB = op?.WilayahPajak ?? "-";
                    res.LATITUDE = item?.Latitude ?? string.Empty;
                    res.LONGITUDE = item?.Longitude ?? string.Empty;
                    res.FK_PAJAK_DAERAH = ((int)EnumFactory.EPajak.TenagaListrik).ToString();
                    res.COLOR_MARKER = "purple";

                    result.Add(res);
                }

                var queryParkirLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaParkir).ToList();
                foreach (var item in queryParkirLocation)
                {
                    var op = context.DbOpParkirs.FirstOrDefault(x => x.Nop == item.FkNop);
                    var res = new MAPViewModel();

                    res.FK_NOP = op?.Nop ?? string.Empty;
                    res.KATEGORI_STATUS = op?.KategoriNama ?? "-";
                    res.NAMA_OP = op?.NamaOp ?? "-";
                    res.ALAMAT_OP = op?.AlamatOp ?? "-";
                    res.UPTB = op?.WilayahPajak ?? "-";
                    res.LATITUDE = item?.Latitude ?? string.Empty;
                    res.LONGITUDE = item?.Longitude ?? string.Empty;
                    res.FK_PAJAK_DAERAH = ((int)EnumFactory.EPajak.JasaParkir).ToString();
                    res.COLOR_MARKER = "brown";

                    result.Add(res);
                }


                return result;
            }
        }
        public class DataRealisasiOp
        {
            public int No { get; set; }
            public string Wilayah { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string StatusNOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOp { get; set; } = null!;
            public string JenisPenarikan { get; set; } = null!;
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public string Bulan { get; set; } = null!;
            public decimal Nominal { get; set; }
        }

        public class UPTBViewModel
        {
            public string Id { get; set; }
            public string Nama { get; set; }
        }
        public class MAPViewModel
        {
            public string FK_NOP { get; set; } = string.Empty;
            public string KATEGORI_STATUS { get; set; } = string.Empty;
            public string NAMA_OP { get; set; } = string.Empty;
            public string ALAMAT_OP { get; set; } = string.Empty;
            public string UPTB { get; set; } = string.Empty;
            public string FK_PAJAK_DAERAH { get; set; } = string.Empty;
            public string FK_KECAMATAN { get; set; } = string.Empty;
            public string FK_KELURAHAN { get; set; } = string.Empty;
            public string LATITUDE { get; set; } = string.Empty;
            public string LONGITUDE { get; set; } = string.Empty;
            public string COLOR_MARKER { get; set; } = string.Empty;
        }
    }
}
