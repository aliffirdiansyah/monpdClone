using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using OfficeOpenXml;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MonPDReborn.Models.DataOP
{
    public class UploadDataVM
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
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
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
                        PotensiPajakTahun = TryDecimal(sheet.Cells[row, 12].Text),
                        AvgRoomPriceKos = TryDecimal(sheet.Cells[row, 13].Text),
                    };

                    context.DbPotensiHotels.Add(dataHotel);
                }

                context.SaveChanges();
            }

            public static void SimpanLampiranExcelParkir(IFormFile fileExcel, int tahun)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var nop = sheet.Cells[row, 1].Text;

                    var existingData = context.DbPotensiParkirs
                        .FirstOrDefault(x => x.TahunBuku == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbPotensiParkirs.Remove(existingData);
                    }

                    var data = new DbPotensiParkir
                    {
                        TahunBuku = tahun,
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
                        TotalOmzet = TryDecimal(sheet.Cells[row, 36].Text),
                        PotensiPajakTahun = TryDecimal(sheet.Cells[row, 37].Text),
                    };

                    context.DbPotensiParkirs.Add(data);
                }

                context.SaveChanges();
            }

            public static void SimpanLampiranExcelResto(IFormFile fileExcel, int tahun)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var nop = sheet.Cells[row, 1].Text;

                    var existingData = context.DbPotensiRestos
                        .FirstOrDefault(x => x.TahunBuku == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbPotensiRestos.Remove(existingData);
                    }

                    var data = new DbPotensiResto
                    {
                        TahunBuku = tahun,
                        Nop = sheet.Cells[row, 1].Text,
                        KapKursi = TryInt(sheet.Cells[row, 2].Text),
                        KapTenantCatering = TryInt(sheet.Cells[row, 3].Text),
                        AvgBillOrg = TryDecimal(sheet.Cells[row, 4].Text),
                        TurnoverWd = TryDecimal(sheet.Cells[row, 5].Text),
                        TurnoverWe = TryDecimal(sheet.Cells[row, 6].Text),
                        AvgVisWd = TryDecimal(sheet.Cells[row, 7].Text),
                        AvgVisWe = TryDecimal(sheet.Cells[row, 8].Text),
                        AvgTenatCatWd = TryDecimal(sheet.Cells[row, 9].Text),
                        AvgTenatCatWe = TryDecimal(sheet.Cells[row, 10].Text),
                        Omzet = TryDecimal(sheet.Cells[row, 11].Text),
                        PotensiPajakTahun = TryDecimal(sheet.Cells[row, 12].Text),
                    };

                    context.DbPotensiRestos.Add(data);
                }
                context.SaveChanges();
            }
            public static void SimpanLampiranExcelHiburan(IFormFile fileExcel, int tahun)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var nop = sheet.Cells[row, 1].Text;

                    var existingData = context.DbPotensiHiburans
                        .FirstOrDefault(x => x.TahunBuku == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbPotensiHiburans.Remove(existingData);
                    }

                    var data = new DbPotensiHiburan
                    {
                        TahunBuku = tahun,
                        Nop = sheet.Cells[row, 1].Text,
                        JumlahStudio = TryInt(sheet.Cells[row, 2].Text),
                        KapKursiStudio = TryInt(sheet.Cells[row, 3].Text),
                        KapPengunjung = TryInt(sheet.Cells[row, 4].Text),
                        HtmWd = TryDecimal(sheet.Cells[row, 5].Text),
                        HtmWe = TryDecimal(sheet.Cells[row, 6].Text),
                        HargaMemberBulan = TryDecimal(sheet.Cells[row, 7].Text),
                        ToWd = TryDecimal(sheet.Cells[row, 8].Text),
                        ToWe = TryDecimal(sheet.Cells[row, 9].Text),
                        AvgVisWd = TryDecimal(sheet.Cells[row, 10].Text),
                        AvgVisWe = TryDecimal(sheet.Cells[row, 11].Text),
                        AvgMemberBulan = TryDecimal(sheet.Cells[row, 12].Text),
                        OmzetBulan = TryDecimal(sheet.Cells[row, 13].Text),
                        PotensiPajakTahun = TryDecimal(sheet.Cells[row, 14].Text),
                    };

                    context.DbPotensiHiburans.Add(data);
                }

                context.SaveChanges();
            }
            public static void UploadPemeriksaanPajak(IFormFile fileExcel, int tahun)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                // counter per NOP
                var seqCounter = new Dictionary<string, int>();

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var nop = sheet.Cells[row, 1].Text?.Trim();
                    if (string.IsNullOrEmpty(nop)) continue;

                    // Hitung seq
                    if (!seqCounter.ContainsKey(nop))
                        seqCounter[nop] = 1;
                    else
                        seqCounter[nop]++;

                    // Hapus data lama jika ada (untuk tahun dan NOP ini)
                    var existingData = context.TPemeriksaans
                        .FirstOrDefault(x => x.TahunPajak == tahun && x.Nop == nop && x.Seq == seqCounter[nop]);

                    if (existingData != null)
                        context.TPemeriksaans.Remove(existingData);

                    var data = new TPemeriksaan
                    {
                        Nop = nop,
                        TahunPajak = tahun,
                        Seq = (byte)seqCounter[nop],

                        TglSp = DateTime.TryParseExact(
                            sheet.Cells[row, 2].Text,
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var tgl
                        ) ? tgl : new DateTime(tahun, 1, 1),

                        NoSp = sheet.Cells[row, 3].Text ?? string.Empty,
                        MasaPajak = sheet.Cells[row, 4].Text ?? string.Empty,
                        Pokok = TryDecimal(sheet.Cells[row, 5].Text) ?? 0,
                        Denda = TryDecimal(sheet.Cells[row, 6].Text) ?? 0,
                        Petugas = sheet.Cells[row, 7].Text ?? string.Empty,
                        Ket = sheet.Cells[row, 8].Text ?? string.Empty,
                        PajakId = TryDecimal(sheet.Cells[row, 9].Text) ?? 0,

                        JumlahKb = TryDecimal(sheet.Cells[row, 10].Text),
                        Lhp = sheet.Cells[row, 11].Text,
                        TglLhp = DateTime.TryParseExact(
                            sheet.Cells[row, 12].Text,
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var tglLhp
                        ) ? tglLhp : new DateTime(tahun, 1, 1),
                        TglByr = DateTime.TryParseExact(
                            sheet.Cells[row, 13].Text,
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var tglByr
                        ) ? tglByr : new DateTime(tahun, 1, 1),
                    };

                    context.TPemeriksaans.Add(data);
                }

                context.SaveChanges();
            }
            public static void UploadPendataanResto(IFormFile fileExcel, int tahun)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                // Counter per NOP
                var nopCounter = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var nop = sheet.Cells[row, 1].Text?.Trim();
                    if (string.IsNullOrEmpty(nop)) continue;

                    // Hitung seq per NOP
                    if (!nopCounter.ContainsKey(nop))
                        nopCounter[nop] = 1;
                    else
                        nopCounter[nop]++;

                    var existingData = context.DbRekamRestorans
                        .FirstOrDefault(x => x.Tanggal.Year == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbRekamRestorans.Remove(existingData);
                    }

                    var data = new DbRekamRestoran
                    {
                        Nop = nop,
                        Tanggal = DateTime.TryParseExact(
                            sheet.Cells[row, 2].Text,
                            "MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var tglBulanHari
                        ) ? new DateTime(tahun, tglBulanHari.Month, tglBulanHari.Day)
                          : new DateTime(tahun, 1, 1),

                        JmlMeja = TryDecimal(sheet.Cells[row, 3].Text) ?? 0,
                        JmlKursi = TryDecimal(sheet.Cells[row, 4].Text) ?? 0,
                        JmlPengunjung = TryDecimal(sheet.Cells[row, 5].Text) ?? 0,
                        Bill = TryDecimal(sheet.Cells[row, 6].Text) ?? 0,
                        RataPengunjungHari = TryDecimal(sheet.Cells[row, 7].Text) ?? 0,
                        RataBillPengunjung = TryDecimal(sheet.Cells[row, 8].Text) ?? 0,
                        OmseBulan = TryDecimal(sheet.Cells[row, 9].Text) ?? 0,
                        PajakBulan = TryDecimal(sheet.Cells[row, 10].Text) ?? 0,
                        PajakId = 1,

                        Seq = nopCounter[nop], // urut per NOP
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
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using var context = DBClass.GetContext();

                // Counter per NOP
                var nopCounter = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                {
                    var nop = sheet.Cells[row, 1].Text?.Trim();
                    if (string.IsNullOrEmpty(nop)) continue;

                    // Hitung seq per NOP
                    if (!nopCounter.ContainsKey(nop))
                        nopCounter[nop] = 1;
                    else
                        nopCounter[nop]++;

                    var existingData = context.DbRekamParkirs
                        .FirstOrDefault(x => x.Tanggal.Year == tahun && x.Nop == nop);

                    if (existingData != null)
                    {
                        context.DbRekamParkirs.Remove(existingData);
                    }

                    var data = new DbRekamParkir
                    {
                        Nop = nop,

                        Tanggal = DateTime.TryParseExact(
                            sheet.Cells[row, 2].Text,
                            "MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var tglBulanHari
                        ) ? new DateTime(tahun, tglBulanHari.Month, tglBulanHari.Day)
                          : new DateTime(tahun, 1, 1),

                        JenisBiayaParkir = sheet.Cells[row, 3].Text ?? string.Empty,

                        // Kapasitas
                        KapasitasMotor = TryDecimal(sheet.Cells[row, 4].Text) ?? 0,
                        KapasitasMobil = TryDecimal(sheet.Cells[row, 5].Text) ?? 0,

                        // Hasil Jumlah
                        HasilJumlahMotor = TryDecimal(sheet.Cells[row, 6].Text) ?? 0,
                        HasilJumlahMobil = TryDecimal(sheet.Cells[row, 7].Text) ?? 0,
                        HasilJumlahMobilBox = TryDecimal(sheet.Cells[row, 8].Text) ?? 0,
                        HasilJumlahTruk = TryDecimal(sheet.Cells[row, 9].Text) ?? 0,
                        HasilJumlahTrailer = TryDecimal(sheet.Cells[row, 10].Text) ?? 0,

                        // Estimasi Harian
                        EstMotorHarian = TryDecimal(sheet.Cells[row, 11].Text) ?? 0,
                        EstMobilHarian = TryDecimal(sheet.Cells[row, 12].Text) ?? 0,
                        EstMobilBoxHarian = TryDecimal(sheet.Cells[row, 13].Text) ?? 0,
                        EstTrukHarian = TryDecimal(sheet.Cells[row, 14].Text) ?? 0,
                        EstTrailerHarian = TryDecimal(sheet.Cells[row, 15].Text) ?? 0,

                        // Tarif
                        TarifMotor = TryDecimal(sheet.Cells[row, 16].Text) ?? 0,
                        TarifMobil = TryDecimal(sheet.Cells[row, 17].Text) ?? 0,
                        TarifMobilBox = TryDecimal(sheet.Cells[row, 18].Text) ?? 0,
                        TarifTruk = TryDecimal(sheet.Cells[row, 19].Text) ?? 0,
                        TarifTrailer = TryDecimal(sheet.Cells[row, 20].Text) ?? 0,

                        // Omzet & Pajak
                        OmzetBulan = TryDecimal(sheet.Cells[row, 21].Text) ?? 0,
                        PajakBulan = TryDecimal(sheet.Cells[row, 22].Text) ?? 0,
                        PajakId = 4,

                        // Sequence urut per NOP
                        Seq = nopCounter[nop],
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
            public byte[] lampiran { get; set; } = null!;
        }
    }
}
