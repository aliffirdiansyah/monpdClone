using DevExpress.XtraRichEdit.Import.Html;
using MonPDLib;
using MonPDLib.General;
using static MonPDReborn.Models.DataOP.ProfilePembayaranOPVM;
using System.Globalization;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using static MonPDLib.Helper;
using DevExpress.DataAccess.Wizard.Native;
using static MonPDLib.General.EnumFactory;

namespace MonPDReborn.Models
{
    public class ApiVM
    {
        public class KartuData
        {
            public List<KartuDataData> KartuDataList { get; set; } = new();
            public KartuData(string connectionString, string idop, int tahun1, int tahun2, int jenisPajak)
            {
                KartuDataList = Method.GetKartuDataData(connectionString, idop, tahun1, tahun2, jenisPajak);
            }
        }
        public class TS
        {
            public List<TSData> TSList { get; set; } = new List<TSData>();
            public TS(string connectionString, string nop, int tahun, string masa)
            {
                TSList = Method.GetTSData(connectionString, nop, tahun, masa);
            }
        }
        public class TB
        {
            public List<TBData> TBList { get; set; } = new List<TBData>();
            public TB(string connectionString, string nop, int tahun, string masa)
            {
                TBList = Method.GetTBData(connectionString, nop, tahun, masa);
            }
        }
        public class SB
        {
            public List<SBData> SBList { get; set; } = new List<SBData>();
            public SB(string connectionString, string nop, int tahun, string masa)
            {
                SBList = Method.GetSBData(connectionString, nop, tahun, masa);
            }
        }
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

            public static List<KartuDataData> GetKartuDataData(string connectionString, string idop, int tahun1, int tahun2, int jenisPajak)
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    EPajak pajak = (EPajak)jenisPajak;
                    string Query = "";

                    switch (pajak)
                    {
                        case EPajak.MakananMinuman:
                            Query = @"
                            SELECT
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    NOP, MIN(NO_SPTPD) AS NO_SPTPD,
                                    TAHUN, MASAPAJAK, SUM(NVL(PAJAK_TERUTANG, 0)) AS PAJAK_TERUTANG,
                                    MIN(KETERANGAN) AS KETERANGAN,
                                    MIN(TANGGALENTRY) AS TANGGALENTRY, MIN(PENGENTRY) AS PENGENTRY,
                                    MIN(NPWPD) AS NPWPD, MIN(TANGGALJATUHTEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MASAPAJAKAWAL) AS MASAPAJAKAWAL, MIN(MASAPAJAKAKHIR) AS MASAPAJAKAKHIR,
                                    MIN(RUMUS_PROSEN) AS RUMUS_PROSEN, SUM(NVL(OMSET, 0)) AS OMSET
                                FROM T_KETETAPAN_RESTORAN
                                GROUP BY NOP, TAHUN, MASAPAJAK                            
                            ) A
                            JOIN T_OP_RESTORAN B ON A.NOP = B.ID_OP AND B.ID_OP=:IDOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_RESTORAN GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD)
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                            break;
                        case EPajak.TenagaListrik:
                            Query = @"
                            SELECT
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_PPJ
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_PPJ B ON A.NOP = B.FK_NOP AND B.FK_NOP=:IDOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_PPJ GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                            break;
                        case EPajak.JasaPerhotelan:
                            Query = @"
                            SELECT
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT
                                    NOP, MIN(NO_SPTPD) AS NO_SPTPD, TAHUN, MASAPAJAK,
                                    SUM(NVL(PAJAK_TERUTANG, 0)) AS PAJAK_TERUTANG, MIN(KETERANGAN) AS KETERANGAN,
                                    MIN(TANGGALENTRY) AS TANGGALENTRY, MIN(PENGENTRY) AS PENGENTRY,
                                    MIN(NPWPD) AS NPWPD, MIN(TANGGALJATUHTEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MASAPAJAKAWAL) AS MASAPAJAKAWAL, MIN(MASAPAJAKAKHIR) AS MASAPAJAKAKHIR,
                                    MIN(RUMUS_PROSEN) AS RUMUS_PROSEN, SUM(NVL(OMSET, 0)) AS OMSET
                                FROM T_KETETAPAN_HOTEL
                                GROUP BY NOP, TAHUN, MASAPAJAK
                            ) A
                            JOIN T_OP_HOTEL B ON A.NOP = B.ID_OP AND B.ID_OP = :IDOP
                            LEFT JOIN (
                                SELECT
                                    MAX(TGL_SSPD) AS TGL_SSPD, FK_NOP, TAHUN_PAJAK_SSPD,
                                    BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK, SUM(JML_DENDA) AS JML_DENDA
                                FROM T_SSPD_HOTEL
                                GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP = C.FK_NOP AND A.MASAPAJAK = C.BULAN_PAJAK_SSPD AND A.TAHUN = C.TAHUN_PAJAK_SSPD)
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                            break;
                        case EPajak.JasaParkir:
                            Query = @"
                            SELECT
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_PARKIR
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_PARKIR B ON A.NOP = B.FK_NOP AND B.FK_NOP=:IDOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_PARKIR GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                            break;
                        case EPajak.JasaKesenianHiburan:
                            Query = @"
                            SELECT
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_HIBURAN
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_HIBURAN B ON A.NOP = B.FK_NOP AND B.FK_NOP=:IDOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_HIBURAN GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                            break;
                        case EPajak.AirTanah:
                            Query = @"
                              SELECT
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'SKPD ABT' AS Sistem,
                                'SKPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                CASE
                                WHEN A.TAHUN > 2024 OR (A.TAHUN = 2024 AND A.MASAPAJAK > 9) THEN 'SYNC SurabayaTax'
	                                ELSE 'SIMPADA'
	                            END AS LokasiSetoran,
	                            CASE
	                                WHEN A.TAHUN > 2024 OR (A.TAHUN = 2024 AND A.MASAPAJAK > 9) THEN 'SYNC SurabayaTax'
	                                ELSE 'SIMPADA'
	                            END AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    NOP AS NOP, MIN(NOMOR_SKPD) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, MASA_PAJAK AS MASAPAJAK, SUM(NVL(JUMLAH_PAJAK, 0)) AS PAJAK_TERUTANG,
                                    '' AS KETERANGAN,
                                    MIN(TGL_PENETAPAN) AS TANGGALENTRY, MIN(TGL_PENETAPAN) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_PENETAPAN) AS TANGGALJATUHTEMPO,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_ABT
                                GROUP BY NOP, TAHUN_PAJAK, MASA_PAJAK                    
                            ) A
                            JOIN T_OP_ABT B ON A.NOP = B.FK_NOP AND B.FK_NOP=:IDOP
                            LEFT JOIN (
                                SELECT MAX(TANGGAL) AS TGL_SSPD,  NOP, TAHUN, BULAN, SUM(SKPD) AS JML_POKOK,
	                            SUM(DENDA) AS JML_DENDA 
	                            FROM T_SSPD_ABT GROUP BY NOP, TAHUN, BULAN
                            ) C ON (A.NOP=C.NOP AND A.MASAPAJAK=C.BULAN AND A.TAHUN=C.TAHUN) 
                            WHERE A.TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                            ";
                            break;
                        default:
                            break;
                    }

                    var param = new
                    {
                        IDOP = idop,
                        TAHUN1 = tahun1,
                        TAHUN2 = tahun2
                    };

                    var data = conn.Query<KartuDataData>(Query, param).ToList();

                    return data;
                }
            }

            //PENAGIHAN DATA
            public static List<PenagihanData> GetPenagihanData(string connectionString, int tahun1, int tahun2, int jenisPajak)
            {
                List<PenagihanData> ret = new List<PenagihanData>();

                string Query = @"";
                EPajak pajak = (EPajak)jenisPajak;
                switch (pajak)
                {
                    case EPajak.MakananMinuman:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    NOP, MIN(NO_SPTPD) AS NO_SPTPD,
                                    TAHUN, MASAPAJAK, SUM(NVL(PAJAK_TERUTANG, 0)) AS PAJAK_TERUTANG,
                                    MIN(KETERANGAN) AS KETERANGAN,
                                    MIN(TANGGALENTRY) AS TANGGALENTRY, MIN(PENGENTRY) AS PENGENTRY,
                                    MIN(NPWPD) AS NPWPD, MIN(TANGGALJATUHTEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MASAPAJAKAWAL) AS MASAPAJAKAWAL, MIN(MASAPAJAKAKHIR) AS MASAPAJAKAKHIR,
                                    MIN(RUMUS_PROSEN) AS RUMUS_PROSEN, SUM(NVL(OMSET, 0)) AS OMSET
                                FROM T_KETETAPAN_RESTORAN
                                GROUP BY NOP, TAHUN, MASAPAJAK                            
                            ) A
                            JOIN T_OP_RESTORAN B ON A.NOP = B.ID_OP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_RESTORAN GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD)
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.TenagaListrik:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_PPJ
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_PPJ B ON A.NOP = B.FK_NOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_PPJ GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.JasaPerhotelan:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT
                                    NOP, MIN(NO_SPTPD) AS NO_SPTPD, TAHUN, MASAPAJAK,
                                    SUM(NVL(PAJAK_TERUTANG, 0)) AS PAJAK_TERUTANG, MIN(KETERANGAN) AS KETERANGAN,
                                    MIN(TANGGALENTRY) AS TANGGALENTRY, MIN(PENGENTRY) AS PENGENTRY,
                                    MIN(NPWPD) AS NPWPD, MIN(TANGGALJATUHTEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MASAPAJAKAWAL) AS MASAPAJAKAWAL, MIN(MASAPAJAKAKHIR) AS MASAPAJAKAKHIR,
                                    MIN(RUMUS_PROSEN) AS RUMUS_PROSEN, SUM(NVL(OMSET, 0)) AS OMSET
                                FROM T_KETETAPAN_HOTEL
                                GROUP BY NOP, TAHUN, MASAPAJAK
                            ) A
                            JOIN T_OP_HOTEL B ON A.NOP = B.ID_OP
                            LEFT JOIN (
                                SELECT
                                    MAX(TGL_SSPD) AS TGL_SSPD, FK_NOP, TAHUN_PAJAK_SSPD,
                                    BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK, SUM(JML_DENDA) AS JML_DENDA
                                FROM T_SSPD_HOTEL
                                GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP = C.FK_NOP AND A.MASAPAJAK = C.BULAN_PAJAK_SSPD AND A.TAHUN = C.TAHUN_PAJAK_SSPD)
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.JasaParkir:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_PARKIR
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_PARKIR B ON A.NOP = B.FK_NOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_PARKIR GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.JasaKesenianHiburan:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_HIBURAN
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_HIBURAN B ON A.NOP = B.FK_NOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_HIBURAN GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    default:
                        break;
                }

                using (var conn = new OracleConnection(connectionString))
                {

                    var param = new
                    {
                        TAHUN1 = tahun1,
                        TAHUN2 = tahun2
                    };

                    ret = conn.Query<PenagihanData>(Query, param).ToList();
                }

                return ret;
            }
            public static List<PenagihanData> GetPenagihanData(string connectionString, int tahun1, int tahun2, int jenisPajak, string nop)
            {
                List<PenagihanData> ret = new List<PenagihanData>();
                string Query = @"";
                EPajak pajak = (EPajak)jenisPajak;
                switch (pajak)
                {
                    case EPajak.MakananMinuman:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    NOP, MIN(NO_SPTPD) AS NO_SPTPD,
                                    TAHUN, MASAPAJAK, SUM(NVL(PAJAK_TERUTANG, 0)) AS PAJAK_TERUTANG,
                                    MIN(KETERANGAN) AS KETERANGAN,
                                    MIN(TANGGALENTRY) AS TANGGALENTRY, MIN(PENGENTRY) AS PENGENTRY,
                                    MIN(NPWPD) AS NPWPD, MIN(TANGGALJATUHTEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MASAPAJAKAWAL) AS MASAPAJAKAWAL, MIN(MASAPAJAKAKHIR) AS MASAPAJAKAKHIR,
                                    MIN(RUMUS_PROSEN) AS RUMUS_PROSEN, SUM(NVL(OMSET, 0)) AS OMSET
                                FROM T_KETETAPAN_RESTORAN
                                GROUP BY NOP, TAHUN, MASAPAJAK                            
                            ) A
                            JOIN T_OP_RESTORAN B ON A.NOP = B.ID_OP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_RESTORAN GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD)
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' AND NOP = :NOP ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.TenagaListrik:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_PPJ
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_PPJ B ON A.NOP = B.FK_NOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_PPJ GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' AND NOP = :NOP ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.JasaPerhotelan:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT
                                    NOP, MIN(NO_SPTPD) AS NO_SPTPD, TAHUN, MASAPAJAK,
                                    SUM(NVL(PAJAK_TERUTANG, 0)) AS PAJAK_TERUTANG, MIN(KETERANGAN) AS KETERANGAN,
                                    MIN(TANGGALENTRY) AS TANGGALENTRY, MIN(PENGENTRY) AS PENGENTRY,
                                    MIN(NPWPD) AS NPWPD, MIN(TANGGALJATUHTEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MASAPAJAKAWAL) AS MASAPAJAKAWAL, MIN(MASAPAJAKAKHIR) AS MASAPAJAKAKHIR,
                                    MIN(RUMUS_PROSEN) AS RUMUS_PROSEN, SUM(NVL(OMSET, 0)) AS OMSET
                                FROM T_KETETAPAN_HOTEL
                                GROUP BY NOP, TAHUN, MASAPAJAK
                            ) A
                            JOIN T_OP_HOTEL B ON A.NOP = B.ID_OP
                            LEFT JOIN (
                                SELECT
                                    MAX(TGL_SSPD) AS TGL_SSPD, FK_NOP, TAHUN_PAJAK_SSPD,
                                    BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK, SUM(JML_DENDA) AS JML_DENDA
                                FROM T_SSPD_HOTEL
                                GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP = C.FK_NOP AND A.MASAPAJAK = C.BULAN_PAJAK_SSPD AND A.TAHUN = C.TAHUN_PAJAK_SSPD)
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' AND NOP = :NOP ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.JasaParkir:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_PARKIR
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_PARKIR B ON A.NOP = B.FK_NOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_PARKIR GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' AND NOP = :NOP ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    case EPajak.JasaKesenianHiburan:
                        Query = @"
                            SELECT
                                A.NOP,
                                A.TAHUN,
                                CASE
                                    WHEN A.MASAPAJAK = 1 THEN 'JANUARI'
                                    WHEN A.MASAPAJAK = 2 THEN 'FEBRUARI'
                                    WHEN A.MASAPAJAK = 3 THEN 'MARET'
                                    WHEN A.MASAPAJAK = 4 THEN 'APRIL'
                                    WHEN A.MASAPAJAK = 5 THEN 'MEI'
                                    WHEN A.MASAPAJAK = 6 THEN 'JUNI'
                                    WHEN A.MASAPAJAK = 7 THEN 'JULI'
                                    WHEN A.MASAPAJAK = 8 THEN 'AGUSTUS'
                                    WHEN A.MASAPAJAK = 9 THEN 'SEPTEMBER'
                                    WHEN A.MASAPAJAK = 10 THEN 'OKTOBER'
                                    WHEN A.MASAPAJAK = 11 THEN 'NOVEMBER'
                                    WHEN A.MASAPAJAK = 12 THEN 'DESEMBER'
                                END AS Bulan,
                                'MPS' AS Sistem,
                                'SPTPD' AS Surat,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanPokok,
                                0 AS KetetapanSanksiSK,
                                NVL(A.PAJAK_TERUTANG, -1) AS KetetapanTotal, -- PajakTerutang = KetetapanTotal
                                NVL(C.JML_POKOK, 0) AS SetoranPokok,
                                NVL(C.JML_DENDA, 0) AS SetoranDenda,
                                (NVL(C.JML_POKOK, 0) + NVL(C.JML_DENDA, 0)) AS SetoranTotal,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanPokok,
                                0 AS TunggakanSanksiSK,
                                0 AS TunggakanPersen,
                                0 AS TunggakanDenda,
                                CASE
                                    WHEN NVL(C.JML_POKOK, 0) > 0 THEN 0
                                    ELSE NVL(A.PAJAK_TERUTANG, -1)
                                END AS TunggakanTotal,
                                C.TGL_SSPD AS TglSetoran,
                                'SIMPADA' AS LokasiSetoran,
                                'SIMPADA' AS OperatorSetoran,
                                0 AS Restitusi,
                                NVL(C.JML_POKOK, 0) AS RestitusiTotal
                            FROM (
                                SELECT 
                                    FK_NOP AS NOP, MIN(ID_KETETAPAN) AS NO_SPTPD,
                                    TAHUN_PAJAK AS TAHUN, BULAN_PAJAK AS MASAPAJAK, SUM(NVL(KETETAPAN_TOTAL, 0)) AS PAJAK_TERUTANG,
                                    MIN(CATATAN) AS KETERANGAN,
                                    MIN(TGL_SPTPD_DISETOR) AS TANGGALENTRY, MIN(TGL_SPTPD_DISETOR) AS PENGENTRY,
                                    '' AS NPWPD, MIN(TGL_JATUH_TEMPO) AS TANGGALJATUHTEMPO,
                                    MIN(MP_AWAL) AS MASAPAJAKAWAL, MIN(MP_AKHIR) AS MASAPAJAKAKHIR,
                                    10 AS RUMUS_PROSEN,  0 AS OMSET
                                FROM T_KETETAPAN_HIBURAN
                                GROUP BY FK_NOP, TAHUN_PAJAK, BULAN_PAJAK                    
                            ) A
                            JOIN T_OP_HIBURAN B ON A.NOP = B.FK_NOP
                            LEFT JOIN (
                                SELECT MAX(TGL_SSPD) AS TGL_SSPD,  FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD, SUM(JML_POKOK) AS JML_POKOK,
                                SUM(JML_DENDA) AS JML_DENDA 
                                FROM T_SSPD_HIBURAN GROUP BY FK_NOP, TAHUN_PAJAK_SSPD, BULAN_PAJAK_SSPD
                            ) C ON (A.NOP=C.FK_NOP AND A.MASAPAJAK=C.BULAN_PAJAK_SSPD AND A.TAHUN=C.TAHUN_PAJAK_SSPD) 
                            WHERE TAHUN BETWEEN :TAHUN1 AND :TAHUN2
                              AND B.KATEGORI_PAJAK NOT LIKE 'OBJEK TESTING' AND NOP = :NOP ORDER BY A.TAHUN,A.MASAPAJAK
                        ";
                        break;
                    default:
                        break;
                }

                using (var conn = new OracleConnection(connectionString))
                {
                    var param = new
                    {
                        TAHUN1 = tahun1,
                        TAHUN2 = tahun2,
                        NOP = nop
                    };

                    ret = conn.Query<PenagihanData>(Query, param).ToList();
                }

                return ret;
            }

        }

        //PENAGIHAN
        public class PenagihanData
        {
            public string NOP { get; set; }
            public int Tahun { get; set; }
            public string Bulan { get; set; } = string.Empty;
            public string Sistem { get; set; } = string.Empty;
            public string Surat { get; set; } = string.Empty;
            public decimal KetetapanPokok { get; set; }
            public decimal KetetapanSanksiSK { get; set; }
            public decimal KetetapanTotal { get; set; }
            public decimal SetoranPokok { get; set; }
            public decimal SetoranDenda { get; set; }
            public decimal SetoranTotal { get; set; }
            public decimal TunggakanPokok { get; set; }
            public decimal TunggakanSanksiSK { get; set; }
            public decimal TunggakanPersen { get; set; }
            public decimal TunggakanDenda { get; set; }
            public decimal TunggakanTotal { get; set; }
            public string TglSetoran { get; set; } = string.Empty;
            public string LokasiSetoran { get; set; } = string.Empty;
            public string OperatorSetoran { get; set; } = string.Empty;
            public decimal Restitusi { get; set; }
            public decimal RestitusiTotal { get; set; }
        }
        //KARTU DATA
        public class KartuDataData
        {
            public int Tahun { get; set; }
            public string Bulan { get; set; } = string.Empty;
            public string Sistem { get; set; } = string.Empty;
            public string Surat { get; set; } = string.Empty;
            public decimal KetetapanPokok { get; set; }
            public decimal KetetapanSanksiSK { get; set; }
            public decimal KetetapanTotal { get; set; }
            public decimal SetoranPokok { get; set; }
            public decimal SetoranDenda { get; set; }
            public decimal SetoranTotal { get; set; }
            public decimal TunggakanPokok { get; set; }
            public decimal TunggakanSanksiSK { get; set; }
            public decimal TunggakanPersen { get; set; }
            public decimal TunggakanDenda { get; set; }
            public decimal TunggakanTotal { get; set; }
            public string TglSetoran { get; set; } = string.Empty;
            public string LokasiSetoran { get; set; } = string.Empty;
            public string OperatorSetoran { get; set; } = string.Empty;
            public decimal Restitusi { get; set; }
            public decimal RestitusiTotal { get; set; }
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
