using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PemeriksaanPajakUploadVM
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
            public static void UploadPemeriksaanPajak(IFormFile fileExcel, int tahun)
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


                    var existingData = context.TPemeriksaans
                        .FirstOrDefault(x => x.TahunPajak == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.TPemeriksaans.Remove(existingData);
                    }

                    var data = new TPemeriksaan
                    {
                        Nop = nop,
                        TahunPajak = tahun,
                        MasaPajak = "Tahunan",
                        Seq = (byte)(row - 1),

                        NoSp = sheet.Cells[row, 12].Text ?? string.Empty,
                        TglSp = DateTime.TryParse(sheet.Cells[row, 13].Text, out var tglSp) ? tglSp : DateTime.Now,

                        Pokok = TryDecimal(sheet.Cells[row, 14].Text) ?? 0,
                        Denda = TryDecimal(sheet.Cells[row, 15].Text) ?? 0,

                        Petugas = sheet.Cells[row, 16].Text ?? string.Empty,
                        Ket = sheet.Cells[row, 17].Text ?? string.Empty,

                        PajakId = TryDecimal(sheet.Cells[row, 18].Text) ?? 0,

                        JumlahKb = TryDecimal(sheet.Cells[row, 19].Text),
                        Lhp = sheet.Cells[row, 20].Text,
                        TglLhp = DateTime.TryParse(sheet.Cells[row, 21].Text, out var tglLhp) ? tglLhp : null,
                        TglByr = DateTime.TryParse(sheet.Cells[row, 22].Text, out var tglByr) ? tglByr : null
                    };
                    context.TPemeriksaans.Add(data);
                }

                context.SaveChanges();
            }
            private static int? TryInt(string value)
            {
                return int.TryParse(value, out var result) ? result : null;
            }

            private static decimal? TryDecimal(string value)
            {
                return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                    ? result : null;
            }
        }

        public class Upload
        {
            public byte[] Data { get; set; }
        }
    }
}
