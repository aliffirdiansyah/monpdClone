using DevExpress.XtraRichEdit.Import.Html;
using MonPDLib;
using MonPDLib.General;
using static MonPDReborn.Models.DataOP.ProfilePembayaranOPVM;
using System.Globalization;

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

                nop = nop.Replace(".", "").Replace("-", "").Trim();

                EnumFactory.EPajak jenisPajak = Utility.GetJenisPajakFromNop(nop);

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
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
        }
    }
}
