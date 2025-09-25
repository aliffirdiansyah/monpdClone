using DevExpress.CodeParser;
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
            public Show(int uptb)
            {
                MonitoringCCTVList = Method.GetMonitoringCCTVList(uptb);
            }
        }
        public class Detail
        {
            public List<MonitoringCCTVDet> MonitoringCCTVDetList { get; set; } = new();
            public Detail(string nop)
            {
                MonitoringCCTVDetList = Method.GetMonitoringCCTVDetList(nop);
            }
        }
        public class Kapasitas
        {
            public List<MonitoringCCTVKapasitas> MonitoringCCTVKapasitasList { get; set; } = new();
            public Kapasitas(string nop)
            {
                MonitoringCCTVKapasitasList = Method.GetMonitoringCCTVKapasitas(nop);
            }
        }
        public class Log
        {
            public List<MonitoringCCTVLog> MonitoringCCTVLogList { get; set; } = new();
            public Log(string nop, int jenisKend)
            {
                MonitoringCCTVLogList = Method.GetMonitoringCCTVLog(nop, jenisKend);
            }
        }

        public class Method
        {
            public static List<MonitoringCCTV> GetMonitoringCCTVList(int uptbId)
            {
                var result = new List<MonitoringCCTV>();
                var context = DBClass.GetContext();

                var qwe = (
                    from c in context.MOpParkirCctvs
                    join d in context.MOpParkirCctvDets on c.Nop equals d.Nop into dets
                    from d in dets.DefaultIfEmpty()
                    join l in context.MOpParkirCctvLogs
                        on new { d.Nop, d.CctvId } equals new { l.Nop, l.CctvId } into logs
                    from l in logs.DefaultIfEmpty()
                    where c.WilayahPajak == uptbId
                    group new { c, d, l } by new
                    {
                        c.Nop,
                        c.NamaOp,
                        c.AlamatOp,
                        c.WilayahPajak,
                        c.Vendor
                    }
                    into g
                    select new
                    {
                        g.Key.Nop,
                        g.Key.NamaOp,
                        g.Key.AlamatOp,
                        g.Key.WilayahPajak,
                        g.Key.Vendor,
                        Dets = g.Select(x => x.d).Where(d => d != null),
                        Logs = g.Select(x => x.l).Where(l => l != null)
                    }
                )
                .ToList();

                result = (
                    from c in context.MOpParkirCctvs
                    join d in context.MOpParkirCctvDets on c.Nop equals d.Nop into dets
                    from d in dets.DefaultIfEmpty()
                    join l in context.MOpParkirCctvLogs
                        on new { d.Nop, d.CctvId } equals new { l.Nop, l.CctvId } into logs
                    from l in logs.DefaultIfEmpty()
                    where c.WilayahPajak == uptbId
                    group new { c, d, l } by new
                    {
                        c.Nop,
                        c.NamaOp,
                        c.AlamatOp,
                        c.WilayahPajak,
                        c.Vendor
                    }
                    into g
                    select new
                    {
                        g.Key.Nop,
                        g.Key.NamaOp,
                        g.Key.AlamatOp,
                        g.Key.WilayahPajak,
                        g.Key.Vendor,
                        Dets = g.Select(x => x.d).Where(d => d != null),
                        Logs = g.Select(x => x.l).Where(l => l != null)
                    }
                )
                .AsEnumerable()
                .Select(g =>
                {
                    var tglTerpasang = g.Dets
                        .Select(d => (DateTime)d.TglPasang)
                        .DefaultIfEmpty(DateTime.Now)
                        .Min();

                    var lastLog = g.Logs
                        .OrderByDescending(l => l.TglAktif)
                        .FirstOrDefault();

                    return new MonitoringCCTV
                    {
                        Nop = g.Nop,
                        NamaOp = g.NamaOp,
                        AlamatOp = g.AlamatOp,
                        WilayahPajak = ((EnumFactory.EUPTB)g.WilayahPajak).GetDescription(),
                        UptbId = g.WilayahPajak,
                        TglTerpasang = tglTerpasang,
                        Vendor = ((EnumFactory.EVendorParkirCCTV)g.Vendor).GetDescription(),
                        StatusAktif = lastLog?.Status ?? "-",
                        TglTerakhirAktif = lastLog?.TglAktif ?? DateTime.MinValue
                    };
                })
                .ToList();

                return result;
            }
            public static List<MonitoringCCTVDet> GetMonitoringCCTVDetList(string nop)
            {
                var result = new List<MonitoringCCTVDet>();
                var context = DBClass.GetContext();

                result = context.MOpParkirCctvLogs
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

                return result;
            }
            public static List<MonitoringCCTVKapasitas> GetMonitoringCCTVKapasitas(string nop)
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
            public static List<MonitoringCCTVLog> GetMonitoringCCTVLog(string nop, int jenisKend)
            {
                var result = new List<MonitoringCCTVLog>();
                var context = DBClass.GetContext();
                result = context.TOpParkirCctvs
                .Where(l => l.Nop == nop && l.JenisKend == jenisKend)
                .Select(l => new MonitoringCCTVLog
                {
                    Nop = l.Nop,
                    JenisKendaraan = l.JenisKend,
                    JenisKendaraanNama = ((EnumFactory.EJenisKendParkirCCTV)l.JenisKend).GetDescription(),
                    WaktuIn = l.WaktuMasuk,
                    WaktuOut = l.WaktuKeluar,
                    PlatNo = l.PlatNo
                })
                .ToList();
                return result;
            }
        }


        public class MonitoringCCTV
        {
            public string Nop { get; set; } = null!;
            public string NamaOp { get; set; } = null!;
            public string AlamatOp { get; set; } = null!;
            public string WilayahPajak { get; set; } = null!;
            public int UptbId { get; set; }
            public string StatusAktif { get; set; } = null!;
            public DateTime TglTerpasang { get; set; } = DateTime.MinValue;
            public string Vendor { get; set; } = null!;
            public DateTime TglTerakhirAktif { get; set; } = DateTime.MinValue;
        }
        public class MonitoringCCTVDet
        {
            public string Nop { get; set; } = null!;
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
