using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PendataanObjekPajakUploadVM
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
            public static void UploadPendataanResto(IFormFile fileExcel, int tahun)
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


                    var existingData = context.DbRekamRestorans
                        .FirstOrDefault(x => x.Tanggal.Year == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbRekamRestorans.Remove(existingData);
                    }

                    var data = new DbRekamRestoran
                    {
                        Tanggal = DateTime.TryParse(sheet.Cells[row, 1].Text, out var tgl) ? tgl : new DateTime(tahun, 1, 1),
                        Nop = sheet.Cells[row, 2].Text,

                        JmlMeja = TryDecimal(sheet.Cells[row, 3].Text) ?? 0,
                        JmlKursi = TryDecimal(sheet.Cells[row, 4].Text) ?? 0,
                        JmlPengunjung = TryDecimal(sheet.Cells[row, 5].Text) ?? 0,
                        Bill = TryDecimal(sheet.Cells[row, 6].Text) ?? 0,
                        RataPengunjungHari = TryDecimal(sheet.Cells[row, 7].Text) ?? 0,
                        RataBillPengunjung = TryDecimal(sheet.Cells[row, 8].Text) ?? 0,
                        OmseBulan = TryDecimal(sheet.Cells[row, 9].Text) ?? 0,
                        PajakBulan = TryDecimal(sheet.Cells[row, 10].Text) ?? 0,
                        PajakId = TryDecimal(sheet.Cells[row, 11].Text) ?? 0,
                        Seq = TryDecimal(sheet.Cells[row, 12].Text) ?? 0,
                    };
                    context.DbRekamRestorans.Add(data);
                }

                context.SaveChanges();
            }
            public static void UploadPendataanParkir(IFormFile fileExcel, int tahun)
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


                    var existingData = context.DbRekamParkirs
                        .FirstOrDefault(x => x.Tanggal.Year == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbRekamParkirs.Remove(existingData);
                    }

                    var data = new DbRekamParkir
                    {
                        Seq = TryDecimal(sheet.Cells[row, 1].Text) ?? 0,
                        Nop = sheet.Cells[row, 2].Text,
                        Tanggal = DateTime.TryParse(sheet.Cells[row, 3].Text, out var tgl) ? tgl : new DateTime(tahun, 1, 1),

                        JenisBiayaParkir = sheet.Cells[row, 4].Text ?? "",

                        KapasitasMotor = TryDecimal(sheet.Cells[row, 5].Text) ?? 0,
                        KapasitasMobil = TryDecimal(sheet.Cells[row, 6].Text) ?? 0,

                        HasilJumlahMotor = TryDecimal(sheet.Cells[row, 7].Text) ?? 0,
                        HasilJumlahMobil = TryDecimal(sheet.Cells[row, 8].Text) ?? 0,
                        HasilJumlahMobilBox = TryDecimal(sheet.Cells[row, 9].Text) ?? 0,
                        HasilJumlahTruk = TryDecimal(sheet.Cells[row, 10].Text) ?? 0,
                        HasilJumlahTrailer = TryDecimal(sheet.Cells[row, 11].Text) ?? 0,

                        EstMotorHarian = TryDecimal(sheet.Cells[row, 12].Text) ?? 0,
                        EstMobilHarian = TryDecimal(sheet.Cells[row, 13].Text) ?? 0,
                        EstMobilBoxHarian = TryDecimal(sheet.Cells[row, 14].Text) ?? 0,
                        EstTrukHarian = TryDecimal(sheet.Cells[row, 15].Text) ?? 0,
                        EstTrailerHarian = TryDecimal(sheet.Cells[row, 16].Text) ?? 0,

                        TarifMotor = TryDecimal(sheet.Cells[row, 17].Text) ?? 0,
                        TarifMobil = TryDecimal(sheet.Cells[row, 18].Text) ?? 0,
                        TarifMobilBox = TryDecimal(sheet.Cells[row, 19].Text) ?? 0,
                        TarifTruk = TryDecimal(sheet.Cells[row, 20].Text) ?? 0,
                        TarifTrailer = TryDecimal(sheet.Cells[row, 21].Text) ?? 0,

                        OmzetBulan = TryDecimal(sheet.Cells[row, 22].Text) ?? 0,
                        PajakBulan = TryDecimal(sheet.Cells[row, 23].Text) ?? 0,
                        PajakId = TryDecimal(sheet.Cells[row, 24].Text) ?? 0,
                    };
                    context.DbRekamParkirs.Add(data);
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
