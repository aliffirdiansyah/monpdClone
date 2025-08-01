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
            public static void SimpanLampiranExcelHotel(IFormFile fileExcel, int tahun)
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


                    var existingData = context.DbPotensiHotels
                        .FirstOrDefault(x => x.TahunBuku == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbPotensiHotels.Remove(existingData);
                    }

                    var dataHotel = new DbPotensiHotel
                    {
                        TahunBuku = tahun,
                        Nop = sheet.Cells[row, 1].Text,
                        TotalRoom = TryInt(sheet.Cells[row, 2].Text),
                        AvgRoomPrice = TryDecimal(sheet.Cells[row, 3].Text),
                        OkupansiRateRoom = TryDecimal(sheet.Cells[row, 4].Text),
                        AvgRoomSold = TryDecimal(sheet.Cells[row, 5].Text),
                        RoomOmzet = TryDecimal(sheet.Cells[row, 6].Text),
                        MaxPaxBanquet = TryInt(sheet.Cells[row, 7].Text),
                        AvgBanquetPrice = TryDecimal(sheet.Cells[row, 8].Text),
                        OkupansiRateBanquet = TryDecimal(sheet.Cells[row, 9].Text),
                        AvgPaxBanquetSold = TryDecimal(sheet.Cells[row, 10].Text),
                        BanquetOmzet = TryDecimal(sheet.Cells[row, 11].Text),
                    };

                    context.DbPotensiHotels.Add(dataHotel);
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
