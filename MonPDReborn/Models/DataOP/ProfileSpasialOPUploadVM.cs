using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                    var nop = sheet.Cells[row, 1].Text;

                    var existingData = context.DbOpLocations
                        .FirstOrDefault(x => x.FkNop == nop);

                    if (existingData != null)
                    {
                        context.DbOpLocations.Remove(existingData);
                    }

                    var data = new DbOpLocation
                    {
                        FkNop = sheet.Cells[row, 1].Text, 
                        Nama = sheet.Cells[row, 37].Text ?? "",         
                        Alamat = sheet.Cells[row, 38].Text ?? "",       
                        Latitude = sheet.Cells[row, 39].Text ?? "",     
                        Longitude = sheet.Cells[row, 40].Text ?? "",   
                        PajakId = TryDecimal(sheet.Cells[row, 41].Text) 
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
