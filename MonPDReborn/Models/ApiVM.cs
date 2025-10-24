using DevExpress.XtraRichEdit.Import.Html;
using MonPDLib;
using MonPDLib.General;
using static MonPDReborn.Models.DataOP.ProfilePembayaranOPVM;
using System.Globalization;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using static MonPDLib.Helper;

namespace MonPDReborn.Models
{
    public class ApiVM
    {
        public class Method
        {
            public static RealisasiPajak GetRealisasiPajak(string nop)
            {
                var result = new RealisasiPajak();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;


                EnumFactory.EPajak jenisPajak = Utility.GetJenisPajakFromNop(nop);
                nop = nop.Replace(".", "").Replace("-", "").Trim();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var queryRestoLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.MakananMinuman && x.FkNop == nop).FirstOrDefault();

                        var dataRestoNow = context.DbMonRestos.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var dataRestoBack = context.DbMonRestos.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear - 1 && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();

                        var op = context.DbOpRestos.FirstOrDefault(x => x.Nop == nop);

                        result.NOP = Utility.GetFormattedNOP(nop);
                        result.NAMA_OP = op?.NamaOp ?? "";
                        result.ALAMAT_OP = op?.AlamatOp ?? "";
                        result.NPWPD = op?.Npwpd ?? "";
                        result.NAMA_WAJIB_PAJAK = op?.NpwpdNama ?? "";
                        result.ALAMAT_WAJIB_PAJAK = op?.NpwpdAlamat ?? "";

                        result.Longitude = queryRestoLocation?.Longitude ?? "-";
                        result.Latitude = queryRestoLocation?.Latitude ?? "-";

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restNow = dataRestoNow
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear)
                                .ToList();

                            decimal realisasiNow = restNow.Sum(q => q.Realisasi) ?? 0;

                            switch (bln)
                            {
                                case 1: result.TAHUN_INI_1 = realisasiNow; break;
                                case 2: result.TAHUN_INI_2 = realisasiNow; break;
                                case 3: result.TAHUN_INI_3 = realisasiNow; break;
                                case 4: result.TAHUN_INI_4 = realisasiNow; break;
                                case 5: result.TAHUN_INI_5 = realisasiNow; break;
                                case 6: result.TAHUN_INI_6 = realisasiNow; break;
                                case 7: result.TAHUN_INI_7 = realisasiNow; break;
                                case 8: result.TAHUN_INI_8 = realisasiNow; break;
                                case 9: result.TAHUN_INI_9 = realisasiNow; break;
                                case 10: result.TAHUN_INI_10 = realisasiNow; break;
                                case 11: result.TAHUN_INI_11 = realisasiNow; break;
                                case 12: result.TAHUN_INI_12 = realisasiNow; break;
                            }
                        }

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restBack = dataRestoBack
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear - 1)
                                .ToList();

                            decimal realisasiBack = restBack.Sum(q => q.Realisasi) ?? 0;

                            switch (bln)
                            {
                                case 1: result.TAHUN_LALU_1 = realisasiBack; break;
                                case 2: result.TAHUN_LALU_2 = realisasiBack; break;
                                case 3: result.TAHUN_LALU_3 = realisasiBack; break;
                                case 4: result.TAHUN_LALU_4 = realisasiBack; break;
                                case 5: result.TAHUN_LALU_5 = realisasiBack; break;
                                case 6: result.TAHUN_LALU_6 = realisasiBack; break;
                                case 7: result.TAHUN_LALU_7 = realisasiBack; break;
                                case 8: result.TAHUN_LALU_8 = realisasiBack; break;
                                case 9: result.TAHUN_LALU_9 = realisasiBack; break;
                                case 10: result.TAHUN_LALU_10 = realisasiBack; break;
                                case 11: result.TAHUN_LALU_11 = realisasiBack; break;
                                case 12: result.TAHUN_LALU_12 = realisasiBack; break;
                            }
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var queryPpjLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.TenagaListrik && x.FkNop == nop).FirstOrDefault();

                        var dataListrikNow = context.DbMonPpjs.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var dataListrikBack = context.DbMonPpjs.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear - 1 && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var opListrik = context.DbOpListriks.FirstOrDefault(x => x.Nop == nop);
                        result.NOP = Utility.GetFormattedNOP(nop);
                        result.NAMA_OP = opListrik?.NamaOp ?? "";
                        result.ALAMAT_OP = opListrik?.AlamatOp ?? "";
                        result.NPWPD = opListrik?.Npwpd ?? "";
                        result.NAMA_WAJIB_PAJAK = opListrik?.NpwpdNama ?? "";
                        result.ALAMAT_WAJIB_PAJAK = opListrik?.NpwpdAlamat ?? "";

                        result.Longitude = queryPpjLocation?.Longitude ?? "-";
                        result.Latitude = queryPpjLocation?.Latitude ?? "-";

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restNow = dataListrikNow
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear)
                                .ToList();
                            decimal realisasiNow = restNow.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_INI_1 = realisasiNow; break;
                                case 2: result.TAHUN_INI_2 = realisasiNow; break;
                                case 3: result.TAHUN_INI_3 = realisasiNow; break;
                                case 4: result.TAHUN_INI_4 = realisasiNow; break;
                                case 5: result.TAHUN_INI_5 = realisasiNow; break;
                                case 6: result.TAHUN_INI_6 = realisasiNow; break;
                                case 7: result.TAHUN_INI_7 = realisasiNow; break;
                                case 8: result.TAHUN_INI_8 = realisasiNow; break;
                                case 9: result.TAHUN_INI_9 = realisasiNow; break;
                                case 10: result.TAHUN_INI_10 = realisasiNow; break;
                                case 11: result.TAHUN_INI_11 = realisasiNow; break;
                                case 12: result.TAHUN_INI_12 = realisasiNow; break;
                            }
                        }
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restBack = dataListrikBack
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear - 1)
                                .ToList();
                            decimal realisasiBack = restBack.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_LALU_1 = realisasiBack; break;
                                case 2: result.TAHUN_LALU_2 = realisasiBack; break;
                                case 3: result.TAHUN_LALU_3 = realisasiBack; break;
                                case 4: result.TAHUN_LALU_4 = realisasiBack; break;
                                case 5: result.TAHUN_LALU_5 = realisasiBack; break;
                                case 6: result.TAHUN_LALU_6 = realisasiBack; break;
                                case 7: result.TAHUN_LALU_7 = realisasiBack; break;
                                case 8: result.TAHUN_LALU_8 = realisasiBack; break;
                                case 9: result.TAHUN_LALU_9 = realisasiBack; break;
                                case 10: result.TAHUN_LALU_10 = realisasiBack; break;
                                case 11: result.TAHUN_LALU_11 = realisasiBack; break;
                                case 12: result.TAHUN_LALU_12 = realisasiBack; break;
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var queryHotelLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan && x.FkNop == nop).FirstOrDefault();

                        var dataHotelNow = context.DbMonHotels.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var dataHotelBack = context.DbMonHotels.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear - 1 && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var opHotel = context.DbOpHotels.FirstOrDefault(x => x.Nop == nop);
                        result.NOP = Utility.GetFormattedNOP(nop);
                        result.NAMA_OP = opHotel?.NamaOp ?? "";
                        result.ALAMAT_OP = opHotel?.AlamatOp ?? "";
                        result.NPWPD = opHotel?.Npwpd ?? "";
                        result.NAMA_WAJIB_PAJAK = opHotel?.NpwpdNama ?? "";
                        result.ALAMAT_WAJIB_PAJAK = opHotel?.NpwpdAlamat ?? "";

                        result.Longitude = queryHotelLocation?.Longitude ?? "-";
                        result.Latitude = queryHotelLocation?.Latitude ?? "-";

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restNow = dataHotelNow
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear)
                                .ToList();
                            decimal realisasiNow = restNow.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_INI_1 = realisasiNow; break;
                                case 2: result.TAHUN_INI_2 = realisasiNow; break;
                                case 3: result.TAHUN_INI_3 = realisasiNow; break;
                                case 4: result.TAHUN_INI_4 = realisasiNow; break;
                                case 5: result.TAHUN_INI_5 = realisasiNow; break;
                                case 6: result.TAHUN_INI_6 = realisasiNow; break;
                                case 7: result.TAHUN_INI_7 = realisasiNow; break;
                                case 8: result.TAHUN_INI_8 = realisasiNow; break;
                                case 9: result.TAHUN_INI_9 = realisasiNow; break;
                                case 10: result.TAHUN_INI_10 = realisasiNow; break;
                                case 11: result.TAHUN_INI_11 = realisasiNow; break;
                                case 12: result.TAHUN_INI_12 = realisasiNow; break;
                            }
                        }
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restBack = dataHotelBack
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear - 1)
                                .ToList();
                            decimal realisasiBack = restBack.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_LALU_1 = realisasiBack; break;
                                case 2: result.TAHUN_LALU_2 = realisasiBack; break;
                                case 3: result.TAHUN_LALU_3 = realisasiBack; break;
                                case 4: result.TAHUN_LALU_4 = realisasiBack; break;
                                case 5: result.TAHUN_LALU_5 = realisasiBack; break;
                                case 6: result.TAHUN_LALU_6 = realisasiBack; break;
                                case 7: result.TAHUN_LALU_7 = realisasiBack; break;
                                case 8: result.TAHUN_LALU_8 = realisasiBack; break;
                                case 9: result.TAHUN_LALU_9 = realisasiBack; break;
                                case 10: result.TAHUN_LALU_10 = realisasiBack; break;
                                case 11: result.TAHUN_LALU_11 = realisasiBack; break;
                                case 12: result.TAHUN_LALU_12 = realisasiBack; break;
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var queryParkirLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaParkir && x.FkNop == nop).FirstOrDefault();

                        var dataParkirNow = context.DbMonParkirs.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var dataParkirBack = context.DbMonParkirs.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear - 1 && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var opParkir = context.DbOpParkirs.FirstOrDefault(x => x.Nop == nop);
                        result.NOP = Utility.GetFormattedNOP(nop);
                        result.NAMA_OP = opParkir?.NamaOp ?? "";
                        result.ALAMAT_OP = opParkir?.AlamatOp ?? "";
                        result.NPWPD = opParkir?.Npwpd ?? "";
                        result.NAMA_WAJIB_PAJAK = opParkir?.NpwpdNama ?? "";
                        result.ALAMAT_WAJIB_PAJAK = opParkir?.NpwpdAlamat ?? "";

                        result.Longitude = queryParkirLocation?.Longitude ?? "-";
                        result.Latitude = queryParkirLocation?.Latitude ?? "-";

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restNow = dataParkirNow
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear)
                                .ToList();
                            decimal realisasiNow = restNow.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_INI_1 = realisasiNow; break;
                                case 2: result.TAHUN_INI_2 = realisasiNow; break;
                                case 3: result.TAHUN_INI_3 = realisasiNow; break;
                                case 4: result.TAHUN_INI_4 = realisasiNow; break;
                                case 5: result.TAHUN_INI_5 = realisasiNow; break;
                                case 6: result.TAHUN_INI_6 = realisasiNow; break;
                                case 7: result.TAHUN_INI_7 = realisasiNow; break;
                                case 8: result.TAHUN_INI_8 = realisasiNow; break;
                                case 9: result.TAHUN_INI_9 = realisasiNow; break;
                                case 10: result.TAHUN_INI_10 = realisasiNow; break;
                                case 11: result.TAHUN_INI_11 = realisasiNow; break;
                                case 12: result.TAHUN_INI_12 = realisasiNow; break;
                            }
                        }
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restBack = dataParkirBack
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear - 1)
                                .ToList();
                            decimal realisasiBack = restBack.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_LALU_1 = realisasiBack; break;
                                case 2: result.TAHUN_LALU_2 = realisasiBack; break;
                                case 3: result.TAHUN_LALU_3 = realisasiBack; break;
                                case 4: result.TAHUN_LALU_4 = realisasiBack; break;
                                case 5: result.TAHUN_LALU_5 = realisasiBack; break;
                                case 6: result.TAHUN_LALU_6 = realisasiBack; break;
                                case 7: result.TAHUN_LALU_7 = realisasiBack; break;
                                case 8: result.TAHUN_LALU_8 = realisasiBack; break;
                                case 9: result.TAHUN_LALU_9 = realisasiBack; break;
                                case 10: result.TAHUN_LALU_10 = realisasiBack; break;
                                case 11: result.TAHUN_LALU_11 = realisasiBack; break;
                                case 12: result.TAHUN_LALU_12 = realisasiBack; break;
                            }
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var queryHiburanLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan && x.FkNop == nop).FirstOrDefault();

                        var dataHiburanNow = context.DbMonHiburans.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var dataHiburanBack = context.DbMonHiburans.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear - 1 && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var opHiburan = context.DbOpHiburans.FirstOrDefault(x => x.Nop == nop);
                        result.NOP = Utility.GetFormattedNOP(nop);
                        result.NAMA_OP = opHiburan?.NamaOp ?? "";
                        result.ALAMAT_OP = opHiburan?.AlamatOp ?? "";
                        result.NPWPD = opHiburan?.Npwpd ?? "";
                        result.NAMA_WAJIB_PAJAK = opHiburan?.NpwpdNama ?? "";
                        result.ALAMAT_WAJIB_PAJAK = opHiburan?.NpwpdAlamat ?? "";

                        result.Longitude = queryHiburanLocation?.Longitude ?? "-";
                        result.Latitude = queryHiburanLocation?.Latitude ?? "-";

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restNow = dataHiburanNow
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear)
                                .ToList();
                            decimal realisasiNow = restNow.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_INI_1 = realisasiNow; break;
                                case 2: result.TAHUN_INI_2 = realisasiNow; break;
                                case 3: result.TAHUN_INI_3 = realisasiNow; break;
                                case 4: result.TAHUN_INI_4 = realisasiNow; break;
                                case 5: result.TAHUN_INI_5 = realisasiNow; break;
                                case 6: result.TAHUN_INI_6 = realisasiNow; break;
                                case 7: result.TAHUN_INI_7 = realisasiNow; break;
                                case 8: result.TAHUN_INI_8 = realisasiNow; break;
                                case 9: result.TAHUN_INI_9 = realisasiNow; break;
                                case 10: result.TAHUN_INI_10 = realisasiNow; break;
                                case 11: result.TAHUN_INI_11 = realisasiNow; break;
                                case 12: result.TAHUN_INI_12 = realisasiNow; break;
                            }
                        }
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restBack = dataHiburanBack
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear - 1)
                                .ToList();
                            decimal realisasiBack = restBack.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_LALU_1 = realisasiBack; break;
                                case 2: result.TAHUN_LALU_2 = realisasiBack; break;
                                case 3: result.TAHUN_LALU_3 = realisasiBack; break;
                                case 4: result.TAHUN_LALU_4 = realisasiBack; break;
                                case 5: result.TAHUN_LALU_5 = realisasiBack; break;
                                case 6: result.TAHUN_LALU_6 = realisasiBack; break;
                                case 7: result.TAHUN_LALU_7 = realisasiBack; break;
                                case 8: result.TAHUN_LALU_8 = realisasiBack; break;
                                case 9: result.TAHUN_LALU_9 = realisasiBack; break;
                                case 10: result.TAHUN_LALU_10 = realisasiBack; break;
                                case 11: result.TAHUN_LALU_11 = realisasiBack; break;
                                case 12: result.TAHUN_LALU_12 = realisasiBack; break;
                            }
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var queryAbtLocation = context.DbOpLocations.Where(x => x.PajakId == (int)EnumFactory.EPajak.AirTanah && x.FkNop == nop).FirstOrDefault();

                        var dataAbtNow = context.DbMonAbts.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var dataAbtBack = context.DbMonAbts.Where(x => x.Nop == nop && x.TahunPajakKetetapan == currentYear - 1 && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value,
                                TahunPajak = x.TahunPajakKetetapan,
                                MasaPajak = x.MasaPajakKetetapan
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    x.Key.TahunPajak,
                                    x.Key.MasaPajak,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        var opAbt = context.DbOpAbts.FirstOrDefault(x => x.Nop == nop);
                        result.NOP = Utility.GetFormattedNOP(nop);
                        result.NAMA_OP = opAbt?.NamaOp ?? "";
                        result.ALAMAT_OP = opAbt?.AlamatOp ?? "";
                        result.NPWPD = opAbt?.Npwpd ?? "";
                        result.NAMA_WAJIB_PAJAK = opAbt?.NpwpdNama ?? "";
                        result.ALAMAT_WAJIB_PAJAK = opAbt?.NpwpdAlamat ?? "";

                        result.Longitude = queryAbtLocation?.Longitude ?? "-";
                        result.Latitude = queryAbtLocation?.Latitude ?? "-";

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restNow = dataAbtNow
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear)
                                .ToList();
                            decimal realisasiNow = restNow.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_INI_1 = realisasiNow; break;
                                case 2: result.TAHUN_INI_2 = realisasiNow; break;
                                case 3: result.TAHUN_INI_3 = realisasiNow; break;
                                case 4: result.TAHUN_INI_4 = realisasiNow; break;
                                case 5: result.TAHUN_INI_5 = realisasiNow; break;
                                case 6: result.TAHUN_INI_6 = realisasiNow; break;
                                case 7: result.TAHUN_INI_7 = realisasiNow; break;
                                case 8: result.TAHUN_INI_8 = realisasiNow; break;
                                case 9: result.TAHUN_INI_9 = realisasiNow; break;
                                case 10: result.TAHUN_INI_10 = realisasiNow; break;
                                case 11: result.TAHUN_INI_11 = realisasiNow; break;
                                case 12: result.TAHUN_INI_12 = realisasiNow; break;
                            }
                        }
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restBack = dataAbtBack
                                .Where(x => x.MasaPajak == bln && x.TahunPajak == currentYear - 1)
                                .ToList();
                            decimal realisasiBack = restBack.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_LALU_1 = realisasiBack; break;
                                case 2: result.TAHUN_LALU_2 = realisasiBack; break;
                                case 3: result.TAHUN_LALU_3 = realisasiBack; break;
                                case 4: result.TAHUN_LALU_4 = realisasiBack; break;
                                case 5: result.TAHUN_LALU_5 = realisasiBack; break;
                                case 6: result.TAHUN_LALU_6 = realisasiBack; break;
                                case 7: result.TAHUN_LALU_7 = realisasiBack; break;
                                case 8: result.TAHUN_LALU_8 = realisasiBack; break;
                                case 9: result.TAHUN_LALU_9 = realisasiBack; break;
                                case 10: result.TAHUN_LALU_10 = realisasiBack; break;
                                case 11: result.TAHUN_LALU_11 = realisasiBack; break;
                                case 12: result.TAHUN_LALU_12 = realisasiBack; break;
                            }
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataPbbNow = context.DbMonPbbs.Where(x => x.Nop == nop && x.TahunBuku == currentYear && x.TglBayar.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayar = x.TglBayar.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayar,
                                    Realisasi = x.Sum(q => q.JumlahBayarPokok)
                                })
                            .ToList();
                        var dataPbbBack = context.DbMonPbbs.Where(x => x.Nop == nop && x.TahunBuku == currentYear - 1 && x.TglBayar.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayar = x.TglBayar.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayar,
                                    Realisasi = x.Sum(q => q.JumlahBayarPokok)
                                })
                            .ToList();
                        var opPbb = context.DbOpPbbs.FirstOrDefault(x => x.Nop == nop);
                        result.NOP = Utility.GetFormattedNOP(nop);
                        result.NAMA_OP = opPbb?.WpNama ?? "";
                        result.ALAMAT_OP = opPbb?.AlamatOp ?? "";
                        result.NPWPD = opPbb?.WpNpwp ?? opPbb.WpKtp ?? "";
                        result.NAMA_WAJIB_PAJAK = opPbb?.WpNama ?? "";
                        result.ALAMAT_WAJIB_PAJAK = opPbb?.AlamatWp ?? "";

                        result.Longitude = "-";
                        result.Latitude = "-";

                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restNow = dataPbbNow
                                .Where(x => x.TglBayar.Month == bln)
                                .ToList();
                            decimal realisasiNow = restNow.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_INI_1 = realisasiNow; break;
                                case 2: result.TAHUN_INI_2 = realisasiNow; break;
                                case 3: result.TAHUN_INI_3 = realisasiNow; break;
                                case 4: result.TAHUN_INI_4 = realisasiNow; break;
                                case 5: result.TAHUN_INI_5 = realisasiNow; break;
                                case 6: result.TAHUN_INI_6 = realisasiNow; break;
                                case 7: result.TAHUN_INI_7 = realisasiNow; break;
                                case 8: result.TAHUN_INI_8 = realisasiNow; break;
                                case 9: result.TAHUN_INI_9 = realisasiNow; break;
                                case 10: result.TAHUN_INI_10 = realisasiNow; break;
                                case 11: result.TAHUN_INI_11 = realisasiNow; break;
                                case 12: result.TAHUN_INI_12 = realisasiNow; break;
                            }
                        }
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var restBack = dataPbbBack
                                .Where(x => x.TglBayar.Month == bln)
                                .ToList();
                            decimal realisasiBack = restBack.Sum(q => q.Realisasi) ?? 0;
                            switch (bln)
                            {
                                case 1: result.TAHUN_LALU_1 = realisasiBack; break;
                                case 2: result.TAHUN_LALU_2 = realisasiBack; break;
                                case 3: result.TAHUN_LALU_3 = realisasiBack; break;
                                case 4: result.TAHUN_LALU_4 = realisasiBack; break;
                                case 5: result.TAHUN_LALU_5 = realisasiBack; break;
                                case 6: result.TAHUN_LALU_6 = realisasiBack; break;
                                case 7: result.TAHUN_LALU_7 = realisasiBack; break;
                                case 8: result.TAHUN_LALU_8 = realisasiBack; break;
                                case 9: result.TAHUN_LALU_9 = realisasiBack; break;
                                case 10: result.TAHUN_LALU_10 = realisasiBack; break;
                                case 11: result.TAHUN_LALU_11 = realisasiBack; break;
                                case 12: result.TAHUN_LALU_12 = realisasiBack; break;
                            }
                        }
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

                return result;
            }
            public static List<TSData> GetTSData(string connectionString, string nop, int tahun, string masa)
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            nop,
                            TO_CHAR(tanggal, 'yyyy-MM-dd') AS tanggal,
                            pajak_terutang,
                            masapajak,
                            tahun,
                            is_generate
                        FROM rekap_perekaman_all@LIHATSURVEILLANCE
                        WHERE  
                            NOP = :nop 
                            AND TAHUN = :tahun 
                            AND MASAPAJAK = :masa
                        ORDER BY tanggal
                    ";

                    return connection.Query<TSData>(query, new { nop, tahun, masa }).ToList();
                }
            }

            public static List<TBData> GetTBData(string connectionString, string nop, int tahun, string masa)
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT * 
                        FROM VW_TB_DATA 
                        WHERE  
                            NOP = :nop 
                            AND TAHUN = :tahun
                            AND MASAPAJAK = :masa
                        ORDER BY TANGGAL
                    ";

                    return connection.Query<TBData>(query, new { nop, tahun, masa }).ToList();
                }
            }

            public static List<SBData> GetSBData(string connectionString, string nop, int tahun, string masa)
            {
                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT *
                        FROM VW_SB_DATA 
                        WHERE  
                            NOP = :nop 
                            AND TAHUN = :tahun 
                            AND MASAPAJAK = :masa
                        ORDER BY TANGGAL
                    ";

                    return connection.Query<SBData>(query, new { nop, tahun, masa }).ToList();
                }
            }

            //public static List<DailyReportView> DailyReportVM(DateTime tglserver, DateTime TglCutOff, bool ww)
            //{
            //    var ret = new List<DailyReportView>();
                
            //    //PPJ            
            //    ret.Add(new DailyReportView
            //    {
            //        Nomor = (int)EnumFactory.EPajak.PPJ,
            //        JenisPajak = "PPJ",
            //        Target = (double)new AkunJenisObjek("4.1", "01", "10", tglserver.Year).GetTotalTarget(),
            //        Realisasi = (double)SSPDMPS.GetRekapitulasiSSPDMPS(EnumFactory.EPajakMPS.PPJ, tglserver.Year).Sum(m => m.Pokok),
            //        Daily = (double)SSPDMPS.GetTotalRealisasiPokok(EnumFactory.EPajakMPS.PPJ, tglserver, tglserver),
            //        SampaiDengan = (double)SSPDMPS.GetTotalRealisasiPokok(EnumFactory.EPajakMPS.PPJ, TglCutOff, tglserver),
            //        BulanIni = (double)SSPDMPS.GetTotalRealisasiPokokBulanIni(EnumFactory.EPajakMPS.PPJ, TglCutOff, tglserver),
            //    });

            //    TotPersen = 100;

            //    if (lstDailyReport.Sum(m => m.Target) > 0)
            //        TotPersen = Math.Round((lstDailyReport.Sum(m => m.SampaiDengan) / lstDailyReport.Sum(m => m.Target)) * 100, 2, MidpointRounding.AwayFromZero);
            //}
        }
        /// DAILY REPORT VIEW
        public class DailyReportView
        {
            public string KodeSubRincian { get; set; }
            public int Nomor { get; set; }
            public string JenisPajak { get; set; }
            public double Target { get; set; }
            public double Realisasi { get; set; }
            public double Persen
            {
                get
                {
                    double prs = 100;
                    if (Target > 0)
                        prs = Math.Round((Realisasi / Target) * 100, 2, MidpointRounding.AwayFromZero);

                    return prs;
                }
            }
            public double Daily { get; set; }
            public double SampaiDengan { get; set; }
            public double BulanIni { get; set; }
        }
        ///TARIKAN DATA TS TB SB
        public class TSData
        {
            public string NOP { get; set; }
            public string TANGGAL { get; set; }
            public decimal PAJAK_TERUTANG { get; set; }
            public string MASAPAJAK { get; set; }
            public int TAHUN { get; set; }
            public int IS_GENERATE { get; set; }
        }
        public class TBData
        {
            public string NOP { get; set; }
            public string TANGGAL { get; set; }
            public decimal JUMLAH_SETOR { get; set; }
            public string MASAPAJAK { get; set; }
            public int TAHUN { get; set; }
        }
        public class SBData
        {
            public string NOP { get; set; }
            public string TANGGAL { get; set; }
            public decimal PAJAK_TERUTANG { get; set; }
            public string MASAPAJAK { get; set; }
            public int TAHUN { get; set; }
        }

        public class RealisasiPajak
        {
            public string NOP { get; set; } = string.Empty;
            public string NAMA_OP { get; set; } = string.Empty;
            public string ALAMAT_OP { get; set; } = string.Empty;
            public string NPWPD { get; set; } = string.Empty;
            public string NAMA_WAJIB_PAJAK { get; set; } = string.Empty;
            public string ALAMAT_WAJIB_PAJAK { get; set; } = string.Empty;
            public decimal TAHUN_INI_1 { get; set; } = 0;
            public decimal TAHUN_INI_2 { get; set; } = 0;
            public decimal TAHUN_INI_3 { get; set; } = 0;
            public decimal TAHUN_INI_4 { get; set; } = 0;
            public decimal TAHUN_INI_5 { get; set; } = 0;
            public decimal TAHUN_INI_6 { get; set; } = 0;
            public decimal TAHUN_INI_7 { get; set; } = 0;
            public decimal TAHUN_INI_8 { get; set; } = 0;
            public decimal TAHUN_INI_9 { get; set; } = 0;
            public decimal TAHUN_INI_10 { get; set; } = 0;
            public decimal TAHUN_INI_11 { get; set; } = 0;
            public decimal TAHUN_INI_12 { get; set; } = 0;

            public decimal TAHUN_LALU_1 { get; set; } = 0;
            public decimal TAHUN_LALU_2 { get; set; } = 0;
            public decimal TAHUN_LALU_3 { get; set; } = 0;
            public decimal TAHUN_LALU_4 { get; set; } = 0;
            public decimal TAHUN_LALU_5 { get; set; } = 0;
            public decimal TAHUN_LALU_6 { get; set; } = 0;
            public decimal TAHUN_LALU_7 { get; set; } = 0;
            public decimal TAHUN_LALU_8 { get; set; } = 0;
            public decimal TAHUN_LALU_9 { get; set; } = 0;
            public decimal TAHUN_LALU_10 { get; set; } = 0;
            public decimal TAHUN_LALU_11 { get; set; } = 0;
            public decimal TAHUN_LALU_12 { get; set; } = 0;

            public string Latitude { get; set; } = string.Empty;
            public string Longitude { get; set; } = string.Empty;
        }
    }
}
