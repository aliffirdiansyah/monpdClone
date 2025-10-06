using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonPDLib.General;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileSpasialOPUploadVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public IFormFile FileExcel { get; set; } = null!;
            public int Tahun { get; set; }

            public List<SelectListItem>? TahunList { get; set; } // untuk dropdown
            public Index()
            {

            }

        }
        public class Method
        {
            public static void UploadSpasial(IFormFile fileExcel)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets[0]; // Sheet1
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var nop = sheet.Cells[row, 1].Text.Replace(".", "");
                    var pajakId = (EnumFactory.EPajak)TryDecimal(sheet.Cells[row, 4].Text);

                    string namaOp = "-";
                    string alamatOp = "-";

                    switch (pajakId)
                    {
                        case EnumFactory.EPajak.Semua:
                            break;
                        case EnumFactory.EPajak.MakananMinuman:
                            var opResto = context.DbOpRestos.Where(x => x.Nop == nop).FirstOrDefault();
                            namaOp = opResto?.NamaOp ?? "-";
                            alamatOp = opResto?.AlamatOp ?? "-";
                            break;
                        case EnumFactory.EPajak.TenagaListrik:
                            var opListrik = context.DbOpListriks.Where(x => x.Nop == nop).FirstOrDefault();
                            namaOp = opListrik?.NamaOp ?? "-";
                            alamatOp = opListrik?.AlamatOp ?? "-";
                            break;
                        case EnumFactory.EPajak.JasaPerhotelan:
                            var opHotel = context.DbOpHotels.Where(x => x.Nop == nop).FirstOrDefault();
                            namaOp = opHotel?.NamaOp ?? "-";
                            alamatOp = opHotel?.AlamatOp ?? "-";
                            break;
                        case EnumFactory.EPajak.JasaParkir:
                            var opParkir = context.DbOpParkirs.Where(x => x.Nop == nop).FirstOrDefault();
                            namaOp = opParkir?.NamaOp ?? "-";
                            alamatOp = opParkir?.AlamatOp ?? "-";
                            break;
                        case EnumFactory.EPajak.JasaKesenianHiburan:
                            var opHiburan = context.DbOpHiburans.Where(x => x.Nop == nop).FirstOrDefault();
                            namaOp = opHiburan?.NamaOp ?? "-";
                            alamatOp = opHiburan?.AlamatOp ?? "-";
                            break;
                        case EnumFactory.EPajak.AirTanah:
                            var opAbt = context.DbOpAbts.Where(x => x.Nop == nop).FirstOrDefault();
                            namaOp = opAbt?.NamaOp ?? "-";
                            alamatOp = opAbt?.AlamatOp ?? "-";
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
                    

                    var existingData = context.DbOpLocations
                        .FirstOrDefault(x => x.FkNop == nop);

                    if (existingData != null)
                    {
                        //context.DbOpLocations.Remove(existingData);
                        continue;
                    }

                    var data = new DbOpLocation
                    {
                        FkNop = nop, 
                        Nama = namaOp,
                        Alamat = alamatOp,       
                        Latitude = sheet.Cells[row, 2].Text ?? "",     
                        Longitude = sheet.Cells[row, 3].Text ?? "",   
                        PajakId = (decimal)TryDecimal(sheet.Cells[row, 4].Text)
                    };

                    context.DbOpLocations.Add(data);
                }

                context.SaveChanges();
            }
            private static decimal? TryDecimal(string value)
            {
                return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                    ? result : null;
            }
        }

        public class Upload
        {
            public byte[] FileExcel { get; set; } = null!;  
        }
    }
}
