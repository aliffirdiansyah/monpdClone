using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using OfficeOpenXml;
using System.Globalization;

namespace MonPDReborn.Models.DataOP
{
    public class PotensiUploadVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public IFormFile Lampiran { get; set; } = null!;
            public Index()
            {

            }

        }

        public class  Method
        {
            public static void SimpanLampiranExcelParkir(IFormFile fileExcel)
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
                    var data = new DbPotensiParkir
                    {
                        Nop = sheet.Cells[row, 1].Text,
                        JenisTarif = TryInt(sheet.Cells[row, 2].Text),
                        SistemParkir = TryInt(sheet.Cells[row, 3].Text),
                        ToWd = TryDecimal(sheet.Cells[row, 4].Text),
                        ToWe = TryDecimal(sheet.Cells[row, 5].Text),
                        KapSepeda = TryInt(sheet.Cells[row, 6].Text),
                        TerparkirSepedaWd = TryInt(sheet.Cells[row, 7].Text),
                        TerparkirSepedaWe = TryInt(sheet.Cells[row, 8].Text),
                        TarifSepeda = TryDecimal(sheet.Cells[row, 9].Text),
                        OmzetSepeda = TryDecimal(sheet.Cells[row, 10].Text),
                        KapMotor = TryInt(sheet.Cells[row, 11].Text),
                        TerparkirMotorWd = TryInt(sheet.Cells[row, 12].Text),
                        TerparkirMotorWe = TryInt(sheet.Cells[row, 13].Text),
                        TarifMotor = TryDecimal(sheet.Cells[row, 14].Text),
                        OmzetMotor = TryDecimal(sheet.Cells[row, 15].Text),
                        KapMobil = TryInt(sheet.Cells[row, 16].Text),
                        TerparkirMobilWd = TryInt(sheet.Cells[row, 17].Text),
                        TerparkirMobilWe = TryInt(sheet.Cells[row, 18].Text),
                        TarifMobil = TryDecimal(sheet.Cells[row, 19].Text),
                        OmzetMobil = TryDecimal(sheet.Cells[row, 20].Text),
                        KapTrukMini = TryInt(sheet.Cells[row, 21].Text),
                        TerparkirTrukMiniWd = TryInt(sheet.Cells[row, 22].Text),
                        TerparkirTrukMiniWe = TryInt(sheet.Cells[row, 23].Text),
                        TarifTrukMini = TryDecimal(sheet.Cells[row, 24].Text),
                        OmzetTrukMini = TryDecimal(sheet.Cells[row, 25].Text),
                        KapTrukBus = TryInt(sheet.Cells[row, 26].Text),
                        TerparkirTrukBusWd = TryInt(sheet.Cells[row, 27].Text),
                        TerparkirTrukBusWe = TryInt(sheet.Cells[row, 28].Text),
                        TarifTrukBus = TryDecimal(sheet.Cells[row, 29].Text),
                        OmzetTrukBus = TryDecimal(sheet.Cells[row, 30].Text),
                        KapTrailer = TryInt(sheet.Cells[row, 31].Text),
                        TerparkirTrailerWd = TryInt(sheet.Cells[row, 32].Text),
                        TerparkirTrailerWe = TryInt(sheet.Cells[row, 33].Text),
                        TarifTrailer = TryDecimal(sheet.Cells[row, 34].Text),
                        OmzetTrailer = TryDecimal(sheet.Cells[row, 35].Text),
                        TotalOmzet = TryDecimal(sheet.Cells[row, 36].Text)
                    };

                    context.DbPotensiParkirs.Add(data);
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
            public byte[] lampiran { get; set; } = null!;
        }
    }
}
