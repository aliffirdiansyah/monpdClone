using DevExpress.CodeParser;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System.Web.Mvc;

namespace MonPDReborn.Models.CCTVParkir
{
    public class MonitoringCCTVVM
    {
        public class Index
        {
            public List<SelectListItem> JenisUptbList { get; set; } = new();
            public int SelectedUPTB { get; set; }
            public Index()
            {
                JenisUptbList = Enum.GetValues(typeof(EnumFactory.EUPTB))
                    .Cast<EnumFactory.EUPTB>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
            }
        }
        public class Show
        {
            public List<MonitoringCCTV> MonitoringCCTVList { get; set; } = new();
            public decimal TotalOp => MonitoringCCTVList.Count;
            public decimal TotalCCTV => MonitoringCCTVList.Count(x => x.StatusAktif == "TERPASANG");
            public decimal TotalBelumTerpasang => MonitoringCCTVList.Count(x => x.StatusAktif == "BELUM TERPASANG");
            //public decimal TotalCCTV => MonitoringCCTVList.Count;
            //public decimal TotalAktif => MonitoringCCTVList.Count(c => c.StatusAktif.ToUpper() == "AKTIF");
            //public decimal TotalNonAktif => MonitoringCCTVList.Count(c => c.StatusAktif.ToUpper() == "NON AKTIF");
            //public decimal TotalBelumTerpasang => MonitoringCCTVList.Count(c => c.StatusAktif.ToUpper() == "BELUM TERPASANG");
            public Show(int uptb)
            {
                MonitoringCCTVList = Method.GetMonitoringCCTVList(uptb);
            }
        }
        public class Detail
        {
            public List<MonitoringCCTVDet> MonitoringCCTVDetList { get; set; } = new();
            public Detail(string nop, int vendor)
            {
                MonitoringCCTVDetList = Method.GetMonitoringCCTVDetList(nop,(EnumFactory.EVendorParkirCCTV)vendor);
            }
        }
        public class Kapasitas
        {
            //tidak dipakai lagi
            //public List<MonitoringCCTVKapasitas> MonitoringCCTVKapasitasList { get; set; } = new();

            // Data Bulanan
            public List<MonitoringCCTVBulanan> RekapBulanan { get; set; } = new();
            public string Nop { get; set; } = "";
            public string NamaOP { get; set; }
            public string AlamatOP { get; set; }
            public string Vendor { get; set; }


            public Kapasitas(string nop, int vendorId)
            {
                Nop = nop;
                NamaOP = "Dummy OP untuk " + nop;
                AlamatOP = "Alamat Dummy";
                Vendor = "Vendor Dummy";

                try
                {
                    // Kalau DB ready
                    RekapBulanan = Method.GetMonitoringCCTVBulanan(nop, vendorId);

                    // Kalau kosong (DB ada tapi data ga ada), isi dummy juga
                    if (RekapBulanan == null || !RekapBulanan.Any())
                    {
                        RekapBulanan = GetDummyData();
                    }
                }
                catch
                {
                    // Kalau DB error → isi dummy
                    RekapBulanan = GetDummyData();
                }

                //tidak dipakai 
                //MonitoringCCTVKapasitasList = Method.GetMonitoringCCTVKapasitas(nop, vendorId);

                //ambil bulanan
                //RekapBulanan = Method.GetMonitoringCCTVBulanan(nop, vendorId);
            }

            private List<MonitoringCCTVBulanan> GetDummyData()
            {
                return new List<MonitoringCCTVBulanan>
        {
            new MonitoringCCTVBulanan
            {
                Nop = Nop,
                Tahun = DateTime.Now.Year,
                Bulan = 1,
                Motor = 120,
                Mobil = 40,
                Unknown = 5,
                Omset = 3000000,
                EstimasiPajak = 750000,
                TahunIni = 165,
                TahunKemarin = 140,
                Potensi = 4000000
            },
                new MonitoringCCTVBulanan
                {
                    Nop = Nop,
                    Tahun = DateTime.Now.Year,
                    Bulan = 2,
                    Motor = 80,
                    Mobil = 60,
                    Unknown = 8,
                    Omset = 2500000,
                    EstimasiPajak = 625000,
                    TahunIni = 148,
                    TahunKemarin = 120,
                    Potensi = 4000000
                }
            };
            }
        }





        public class MonitoringCCTVBulanan
        {
            public string Nop { get; set; } = null!;
            public int Tahun { get; set; }
            public int Bulan { get; set; }

            // Untuk tampilan
            public string NamaBulan => new DateTime(Tahun, Bulan, 1).ToString("MMMM");
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);

            // Agregat per bulan
            public int Motor { get; set; }
            public int Mobil { get; set; }
            public int Unknown { get; set; }
            public decimal Omset { get; set; }
            public decimal EstimasiPajak { get; set; }
            public decimal TahunKemarin { get; set; }
            public decimal TahunIni { get; set; }
            public decimal Potensi { get; set; }

        }

        public class MonitoringCCTVHarian
        {
            public string Nop { get; set; } = null!;
            public DateTime Tanggal { get; set; }


            // Untuk tampilan
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);

            // Agregat per bulan
            public int Motor { get; set; }
            public int Mobil { get; set; }
            public int Unknown { get; set; }
            public decimal Omset { get; set; }
            public decimal EstimasiPajak { get; set; }
            public decimal TahunKemarin { get; set; }
            public decimal TahunIni { get; set; }
            public decimal Potensi { get; set; }

        }


        public class DataKapasitasParkir
        {
            public List<MonitoringCCTVKapasitas> MonitoringCCTVKapasitasList { get; set; } = new();
            public DataKapasitasParkir(string nop, DateTime tglAwal, DateTime tglAkhir)
            {
                MonitoringCCTVKapasitasList = Method.GetMonitoringCCTVKapasitas(nop, tglAwal, tglAkhir);
            }
        }
        public class Log
        {
            public List<MonitoringCCTVLog> MonitoringCCTVLogList { get; set; } = new();
            public Log(string nop, int jenisKend, DateTime tanggalAwal, DateTime tanggalAkhir)
            {
                MonitoringCCTVLogList = Method.GetMonitoringCCTVLog(nop, jenisKend, tanggalAwal, tanggalAkhir);
            }
        }

        public class Method
        {
            public static List<MonitoringCCTV> GetMonitoringCCTVList(int uptbId)
            {
                var result = new List<MonitoringCCTV>();
                var context = DBClass.GetContext();
                if (uptbId == 0)
                {
                    result = (
                        from c in context.MOpParkirCctvs
                        join d1 in context.MOpParkirCctvJasnita on c.Nop equals d1.Nop into jasnitaDets
                        from d1 in jasnitaDets.DefaultIfEmpty()
                        join d2 in context.MOpParkirCctvTelkoms on c.Nop equals d2.Nop into telkomDets
                        from d2 in telkomDets.DefaultIfEmpty()
                        join l1 in context.MOpParkirCctvJasnitaLogs
                            on new { Nop = (string?)d1.Nop, CctvId = d1.CctvId }
                            equals new { Nop = (string?)l1.Nop, CctvId = l1.CctvId }
                            into logsJasnita
                        from l1 in logsJasnita.DefaultIfEmpty()
                        join l2 in context.MOpParkirCctvTelkomLogs
                            on new { Nop = (string?)d2.Nop, CctvId = d2.CctvId }
                            equals new { Nop = (string?)l2.Nop, CctvId = l2.CctvId }
                            into logsTelkom
                        from l2 in logsTelkom.DefaultIfEmpty()
                        group new { c, d1, d2, l1, l2 } by new
                        {
                            c.Nop,
                            c.NamaOp,
                            c.AlamatOp,
                            c.WilayahPajak,
                            c.Vendor,
                            c.IsPasang
                        }
                        into g
                        select new
                        {
                            g.Key.Nop,
                            g.Key.NamaOp,
                            g.Key.AlamatOp,
                            g.Key.WilayahPajak,
                            g.Key.Vendor,
                            g.Key.IsPasang,
                            D1 = g.Select(x => x.d1),
                            D2 = g.Select(x => x.d2),
                            L1 = g.Select(x => x.l1),
                            L2 = g.Select(x => x.l2)
                        }
                    )
                    .AsEnumerable()
                    .Select(g =>
                    {
                        var dets = g.D1.Where(x => x != null).Cast<object>()
                            .Concat(g.D2.Where(x => x != null));

                        var logs = g.L1.Where(x => x != null).Cast<object>()
                            .Concat(g.L2.Where(x => x != null));

                        var tglTerpasang = dets
                            .Select(d => (DateTime?)((dynamic)d).TglPasang)
                            .Min();

                        var lastLog = logs
                            .OrderByDescending(l => ((dynamic)l).TglTerakhirAktif)
                            .FirstOrDefault();

                        return new MonitoringCCTV
                        {
                            Nop = g.Nop,
                            NamaOp = g.NamaOp,
                            AlamatOp = g.AlamatOp,
                            WilayahPajak = ((EnumFactory.EUPTB)g.WilayahPajak).GetDescription(),
                            UptbId = g.WilayahPajak,
                            TglTerpasang = tglTerpasang,
                            VedorId = g.Vendor,
                            Vendor = ((EnumFactory.EVendorParkirCCTV)g.Vendor).GetDescription(),
                            //StatusAktif = lastLog != null 
                            //    ? ((dynamic)lastLog).Status 
                            //    : (tglTerpasang.HasValue)
                            //        ? "NON AKTIF"
                            //        : "BELUM TERPASANG",
                            StatusAktif = g.IsPasang == 1 ? "TERPASANG" : "BELUM TERPASANG",
                            TglTerakhirAktif = lastLog != null ? ((dynamic)lastLog).TglTerakhirAktif : null
                        };
                    })
                    .ToList();
                }
                else
                {
                    result = (
                        from c in context.MOpParkirCctvs
                        join d1 in context.MOpParkirCctvJasnita on c.Nop equals d1.Nop into jasnitaDets
                        from d1 in jasnitaDets.DefaultIfEmpty()
                        join d2 in context.MOpParkirCctvTelkoms on c.Nop equals d2.Nop into telkomDets
                        from d2 in telkomDets.DefaultIfEmpty()
                        join l1 in context.MOpParkirCctvJasnitaLogs
                            on new { Nop = (string?)d1.Nop, CctvId = d1.CctvId }
                            equals new { Nop = (string?)l1.Nop, CctvId = l1.CctvId }
                            into logsJasnita
                        from l1 in logsJasnita.DefaultIfEmpty()
                        join l2 in context.MOpParkirCctvTelkomLogs
                            on new { Nop = (string?)d2.Nop, CctvId = d2.CctvId }
                            equals new { Nop = (string?)l2.Nop, CctvId = l2.CctvId }
                            into logsTelkom
                        from l2 in logsTelkom.DefaultIfEmpty()
                        where c.WilayahPajak == uptbId
                        group new { c, d1, d2, l1, l2 } by new
                        {
                            c.Nop,
                            c.NamaOp,
                            c.AlamatOp,
                            c.WilayahPajak,
                            c.Vendor,
                            c.IsPasang
                        }
                        into g
                        select new
                        {
                            g.Key.Nop,
                            g.Key.NamaOp,
                            g.Key.AlamatOp,
                            g.Key.WilayahPajak,
                            g.Key.Vendor,
                            g.Key.IsPasang,
                            D1 = g.Select(x => x.d1),
                            D2 = g.Select(x => x.d2),
                            L1 = g.Select(x => x.l1),
                            L2 = g.Select(x => x.l2)
                        }
                    )
                    .AsEnumerable()
                    .Select(g =>
                    {
                        var dets = g.D1.Where(x => x != null).Cast<object>()
                            .Concat(g.D2.Where(x => x != null));

                        var logs = g.L1.Where(x => x != null).Cast<object>()
                            .Concat(g.L2.Where(x => x != null));

                        var tglTerpasang = dets
                            .Select(d => (DateTime?)((dynamic)d).TglPasang)
                            .Min();

                        var lastLog = logs
                            .OrderByDescending(l => ((dynamic)l).TglTerakhirAktif)
                            .FirstOrDefault();

                        return new MonitoringCCTV
                        {
                            Nop = g.Nop,
                            NamaOp = g.NamaOp,
                            AlamatOp = g.AlamatOp,
                            WilayahPajak = ((EnumFactory.EUPTB)g.WilayahPajak).GetDescription(),
                            UptbId = g.WilayahPajak,
                            TglTerpasang = tglTerpasang,
                            VedorId = g.Vendor,
                            Vendor = ((EnumFactory.EVendorParkirCCTV)g.Vendor).GetDescription(),
                            //StatusAktif = lastLog != null
                            //    ? ((dynamic)lastLog).Status
                            //    : (tglTerpasang.HasValue)
                            //        ? "NON AKTIF"
                            //        : "BELUM TERPASANG",
                            StatusAktif = g.IsPasang == 1 ? "TERPASANG" : "BELUM TERPASANG",
                            TglTerakhirAktif = lastLog != null ? ((dynamic)lastLog).TglTerakhirAktif : null
                        };
                    })
                    .ToList();
                }
                return result;
            }
            public static List<MonitoringCCTVDet> GetMonitoringCCTVDetList(string nop, EnumFactory.EVendorParkirCCTV vendorid)
            {
                var result = new List<MonitoringCCTVDet>();
                var context = DBClass.GetContext();

                switch (vendorid)
                {
                    case EnumFactory.EVendorParkirCCTV.Jasnita:
                        result = context.MOpParkirCctvJasnitaLogs
                        .Where(l => l.Nop == nop)
                        .Select(l => new MonitoringCCTVDet
                        {
                            Nop = l.Nop,
                            Tgl = l.TglTerakhirAktif,
                            TglAktif = l.TglTerakhirAktif,
                            TglDown = l.TglTerakhirDown.Value,
                            StatusAktif = l.Status
                        })
                        .ToList();
                        break;
                    case EnumFactory.EVendorParkirCCTV.Telkom:
                        result = context.MOpParkirCctvTelkomLogs
                        .Where(l => l.Nop == nop)
                        .Select(l => new MonitoringCCTVDet
                        {
                            Nop = l.Nop,
                            Tgl = l.TglAktif,
                            TglAktif = l.TglAktif,
                            TglDown = l.TglDown.Value,
                            StatusAktif = l.Status
                        })
                        .ToList();
                        break;
                    default:
                        break;
                }
                

                return result;
            }
            public static List<MonitoringCCTVKapasitas> GetMonitoringCCTVKapasitas(string nop, int vendorId)
            {
                var result = new List<MonitoringCCTVKapasitas>();
                var context = DBClass.GetContext();

                var kapasitas = context.DbPotensiParkirs
                    .Where(p => p.Nop == nop && p.TahunBuku == DateTime.Now.Year + 1)
                    .AsQueryable();

                var data = context.TOpParkirCctvs
                    .Where(l => l.Nop == nop && l.WaktuMasuk.Date == DateTime.Now.Date)
                    .GroupBy(l => l.JenisKend)
                    .Select(g => new
                    {
                        JenisKendaraan = g.Key,
                        JumlahKendaraanTerparkir = g.Count()
                    })
                    .ToList();
                result = data.Select(d => new MonitoringCCTVKapasitas
                {
                    Nop = nop,
                    Tanggal = DateTime.Today,
                    JenisKendaraan = d.JenisKendaraan,
                    JumlahKendaraanTerparkir = d.JumlahKendaraanTerparkir,
                    Kapasitas = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMotor ?? 0 : 0) :
                                d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMobil ?? 0 : 0) :
                                d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapTrukBus ?? 0 : 0) : 0,

                    Tarif = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMotor ?? 0 : 0) :
                            d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMobil ?? 0 : 0) :
                            d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifTrukBus ?? 0 : 0) : 0,
                }).ToList();
                return result;
            }
            public static List<MonitoringCCTVKapasitas> GetMonitoringCCTVKapasitas(string nop, DateTime tglAwal, DateTime tglAkhir)
            {
                var result = new List<MonitoringCCTVKapasitas>();
                var context = DBClass.GetContext();

                var kapasitas = context.DbPotensiParkirs
                    .Where(p => p.Nop == nop && p.TahunBuku == DateTime.Now.Year + 1)
                    .AsQueryable();

                var data = context.TOpParkirCctvs
                    .Where(l => l.Nop == nop && l.WaktuMasuk >= tglAwal && l.WaktuMasuk <= tglAkhir)
                    .GroupBy(l => l.JenisKend)
                    .Select(g => new
                    {
                        JenisKendaraan = g.Key,
                        JumlahKendaraanTerparkir = g.Count()
                    })
                    .ToList();
                result = data.Select(d => new MonitoringCCTVKapasitas
                {
                    Nop = nop,
                    JenisKendaraan = d.JenisKendaraan,
                    JumlahKendaraanTerparkir = d.JumlahKendaraanTerparkir,
                    Kapasitas = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMotor ?? 0 : 0) :
                                d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMobil ?? 0 : 0) :
                                d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapTrukBus ?? 0 : 0) : 0,

                    Tarif = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMotor ?? 0 : 0) :
                            d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMobil ?? 0 : 0) :
                            d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifTrukBus ?? 0 : 0) : 0,
                }).ToList();
                return result;
            }
            public static List<MonitoringCCTVLog> GetMonitoringCCTVLog(string nop, int jenisKend, DateTime tglAwal, DateTime tglAkhir)
            {
                var result = new List<MonitoringCCTVLog>();
                var context = DBClass.GetContext();
                result = context.TOpParkirCctvs
                .Where(l => l.Nop == nop && l.JenisKend == jenisKend && l.WaktuMasuk >= tglAwal && l.WaktuMasuk <= tglAkhir)
                .Select(l => new MonitoringCCTVLog
                {
                    Nop = l.Nop,
                    JenisKendaraan = l.JenisKend,
                    JenisKendaraanNama = ((EnumFactory.EJenisKendParkirCCTV)l.JenisKend).GetDescription(),
                    WaktuIn = l.WaktuMasuk,
                    WaktuOut = l.WaktuKeluar,
                    PlatNo = l.PlatNo ?? "-"
                })
                .ToList();
                return result;
            }

            public static List<MonitoringCCTVBulanan> GetMonitoringCCTVBulanan(string nop, int vendorId)
            {
                /************************************
                 *  AWAS BISA DISESUAIKAN KEBUTUHAN LOGICNYA
                 * 
                 */
                var context = DBClass.GetContext();

                var kapasitas = context.DbPotensiParkirs
                    .FirstOrDefault(p => p.Nop == nop && p.TahunBuku == DateTime.Now.Year + 1);

                // data tahun ini
                var queryTahunIni = context.TOpParkirCctvs
                    .Where(l => l.Nop == nop && l.WaktuMasuk.Year == DateTime.Now.Year);

                // data tahun kemarin
                // data tahun kemarin, langsung ToList() biar jadi LINQ to Objects
                var queryTahunKemarin = context.TOpParkirCctvs
                    .Where(l => l.Nop == nop && l.WaktuMasuk.Year == DateTime.Now.Year - 1)
                    .GroupBy(l => l.WaktuMasuk.Month)
                    .Select(g => new { Bulan = g.Key, Total = g.Count() })
                    .ToList(); // <-- penting!


                var data = queryTahunIni
                    .GroupBy(l => new { l.WaktuMasuk.Year, l.WaktuMasuk.Month })
                    .Select(g => new MonitoringCCTVBulanan
                    {
                        Nop = nop,
                        Tahun = g.Key.Year,
                        Bulan = g.Key.Month,

                        Motor = g.Count(x => x.JenisKend == 1),
                        Mobil = g.Count(x => x.JenisKend == 2),
                        Unknown = g.Count(x => x.JenisKend != 1 && x.JenisKend != 2),

                        Omset = g.Sum(x => x.JenisKend == 1 ? (kapasitas.TarifMotor ?? 0) :
                                           x.JenisKend == 2 ? (kapasitas.TarifMobil ?? 0) :
                                           x.JenisKend == 3 ? (kapasitas.TarifTrukBus ?? 0) : 0),

                        EstimasiPajak = g.Sum(x => x.JenisKend == 1 ? (kapasitas.TarifMotor ?? 0) :
                                                   x.JenisKend == 2 ? (kapasitas.TarifMobil ?? 0) :
                                                   x.JenisKend == 3 ? (kapasitas.TarifTrukBus ?? 0) : 0) * 0.25m,

                        TahunIni = g.Count(),
                        TahunKemarin = queryTahunKemarin
                                        .Where(x => x.Bulan == g.Key.Month)
                                        .Select(x => (int?)x.Total) // cast ke nullable
                                        .FirstOrDefault() ?? 0,


                        Potensi = (kapasitas.TarifMotor ?? 0) * (kapasitas.KapMotor ?? 0) +
                                  (kapasitas.TarifMobil ?? 0) * (kapasitas.KapMobil ?? 0) +
                                  (kapasitas.TarifTrukBus ?? 0) * (kapasitas.KapTrukBus ?? 0)
                    })
                    .ToList();

                return data;
            }


            public static List<MonitoringCCTVHarian> GetMonitoringCCTVHarian(string nop, int vendorId, int tahun, int bulan)
            {
                /************************************
                 *  AWAS BISA DISESUAIKAN KEBUTUHAN LOGICNYA
                 * 
                 */

                var context = DBClass.GetContext();

                var kapasitas = context.DbPotensiParkirs
                    .Where(p => p.Nop == nop && p.TahunBuku == tahun)
                    .FirstOrDefault();

                //var data = context.TOpParkirCctvs
                //    .Where(l => l.Nop == nop && l.WaktuMasuk.Year == tahun && l.WaktuMasuk.Month == bulan)
                //    .GroupBy(l => new { l.WaktuMasuk.Date, l.JenisKend })
                //    .Select(g => new MonitoringCCTVKapasitas
                //    {
                //        Nop = nop,
                //        Tanggal = g.Key.Date,
                //        JenisKendaraan = g.Key.JenisKend,
                //        JumlahKendaraanTerparkir = g.Count(),
                //        Kapasitas = g.Key.JenisKend == 1 ? (kapasitas?.KapMotor ?? 0) :
                //                    g.Key.JenisKend == 2 ? (kapasitas?.KapMobil ?? 0) :
                //                    g.Key.JenisKend == 3 ? (kapasitas?.KapTrukBus ?? 0) : 0,
                //        Tarif = g.Key.JenisKend == 1 ? (kapasitas?.TarifMotor ?? 0) :
                //                g.Key.JenisKend == 2 ? (kapasitas?.TarifMobil ?? 0) :
                //                g.Key.JenisKend == 3 ? (kapasitas?.TarifTrukBus ?? 0) : 0
                //    })
                //    .ToList();

                //dikosongin dulu
                return null;
            }


        }


        public class MonitoringCCTV
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public string NamaOp { get; set; } = null!;
            public string AlamatOp { get; set; } = null!;
            public string WilayahPajak { get; set; } = null!;
            public int UptbId { get; set; }
            public string StatusAktif { get; set; } = null!;
            public DateTime? TglTerpasang { get; set; } = DateTime.MinValue;
            public int VedorId { get; set; }
            public string Vendor { get; set; } = null!;
            public DateTime? TglTerakhirAktif { get; set; } = DateTime.MinValue;
        }
        public class MonitoringCCTVDet
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public DateTime Tgl { get; set; }
            public DateTime TglAktif { get; set; }
            public DateTime TglDown { get; set; }
            public string StatusAktif { get; set; } = null!;

            public int Hari => (TglDown - TglAktif).Days;
            public int Jam => (TglDown - TglAktif).Hours;
            public int Menit => (TglDown - TglAktif).Minutes;

            public string DownTime
            {
                get
                {
                    var parts = new List<string>();

                    if (Hari > 0) parts.Add($"{Hari} Hari");
                    if (Jam > 0) parts.Add($"{Jam} Jam");
                    if (Menit > 0) parts.Add($"{Menit} Menit");

                    // Kalau semua 0 (misal TglDown == TglAktif)
                    if (parts.Count == 0)
                        return "0 Menit";

                    return string.Join(" ", parts);
                }
            }
        }
        public class MonitoringCCTVKapasitas
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public DateTime Tanggal { get; set; }
            public int JenisKendaraan { get; set; }
            public string JenisKendaraanNama => ((EnumFactory.EJenisKendParkirCCTV)JenisKendaraan).GetDescription();
            public int JumlahKendaraanTerparkir { get; set; }
            public decimal Tarif { get; set; }
            public int Kapasitas { get; set; }
        }
        public class MonitoringCCTVLog
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public int JenisKendaraan { get; set; }
            public string JenisKendaraanNama { get; set; } = null!;
            public DateTime WaktuIn { get; set; }
            public DateTime? WaktuOut { get; set; }   // nullable
            public string? PlatNo { get; set; }

            private TimeSpan LamaTerparkir => (WaktuOut ?? DateTime.Now) - WaktuIn;

            public int Hari => LamaTerparkir.Days;
            public int Jam => LamaTerparkir.Hours;
            public int Menit => LamaTerparkir.Minutes;

            public string Terparkir
            {
                get
                {
                    var parts = new List<string>();

                    if (Hari > 0) parts.Add($"{Hari} Hari");
                    if (Jam > 0) parts.Add($"{Jam} Jam");
                    if (Menit > 0) parts.Add($"{Menit} Menit");

                    if (parts.Count == 0)
                        return "0 Menit";

                    return string.Join(" ", parts);
                }
            }

            public bool MasihTerparkir => WaktuOut == null;
        }
    }

}
