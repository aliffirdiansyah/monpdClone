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
        public class Method
        {
            public static List<MonitoringCCTV> GetMonitoringCCTVList(int uptbId)
            {
                var result = new List<MonitoringCCTV>();
                var context = DBClass.GetContext();
                result = context.MOpParkirCctvs
                .Include(x => x.MOpParkirCctvDets)
                    .ThenInclude(d => d.MOpParkirCctvLog)
                .Where(x => x.WilayahPajak == uptbId)
                .Select(x => new MonitoringCCTV
                {
                    Nop = x.Nop,
                    NamaOp = x.NamaOp,
                    AlamatOp = x.AlamatOp,
                    WilayahPajak = ((EnumFactory.EUPTB)x.WilayahPajak).GetDescription(),
                    UptbId = x.WilayahPajak,
                    TglTerpasang = x.MOpParkirCctvDets
                                        .OrderBy(d => d.TglPasang)
                                        .Select(d => d.TglPasang)
                                        .FirstOrDefault(),
                    Vendor = ((EnumFactory.EVendorParkirCCTV)x.Vendor).GetDescription(),

                    StatusAktif = x.MOpParkirCctvDets
                                        .Where(d => d.MOpParkirCctvLog != null)
                                        .OrderByDescending(d => d.MOpParkirCctvLog.TglAktif)
                                        .Select(d => d.MOpParkirCctvLog.Status)
                                        .FirstOrDefault() ?? "-",

                    TglTerakhirAktif = x.MOpParkirCctvDets
                                            .Where(d => d.MOpParkirCctvLog != null)
                                            .OrderByDescending(d => d.MOpParkirCctvLog.TglAktif)
                                            .Select(d => d.MOpParkirCctvLog.TglAktif)
                                            .FirstOrDefault()
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
            public static List<MonitoringCCTVLog> GetMonitoringCCTVLog(string nop)
            {
                var result = new List<MonitoringCCTVLog>();
                var context = DBClass.GetContext();
                result = context.TOpParkirCctvs
                .Where(l => l.Nop == nop && l.WaktuKeluar != null)
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
