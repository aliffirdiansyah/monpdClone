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
                List<MonitoringCCTV> result = new()
                {
                    new MonitoringCCTV { Nop = "3171010001", NamaOp = "Parkir Plaza Surabaya", AlamatOp = "Jl. Pemuda No. 1", WilayahPajak = "Genteng", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50) },
                    new MonitoringCCTV { Nop = "3171010002", NamaOp = "Parkir Tunjungan Plaza", AlamatOp = "Jl. Basuki Rahmat No. 8", WilayahPajak = "Tegalsari", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50) },
                    new MonitoringCCTV { Nop = "3171010003", NamaOp = "Parkir Galaxy Mall", AlamatOp = "Jl. Dharmahusada Indah Timur", WilayahPajak = "Mulyorejo", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Non-Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50) },
                    new MonitoringCCTV { Nop = "3171010004", NamaOp = "Parkir Royal Plaza", AlamatOp = "Jl. Ahmad Yani No. 16", WilayahPajak = "Wonokromo", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50) },
                    new MonitoringCCTV {Nop = "3171010005", NamaOp = "Parkir Pakuwon Mall", AlamatOp = "Jl. Puncak Indah Lontar", WilayahPajak = "Sambikerep", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50)},
                    new MonitoringCCTV {Nop = "3171010006", NamaOp = "Parkir City of Tomorrow", AlamatOp = "Jl. Ahmad Yani No. 288", WilayahPajak = "Jambangan", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Non-Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50)},
                    new MonitoringCCTV {Nop = "3171010007", NamaOp = "Parkir Lenmarc", AlamatOp = "Jl. Bukit Darmo Golf", WilayahPajak = "Sukomanunggal", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50)},
                    new MonitoringCCTV {Nop = "3171010008", NamaOp = "Parkir BG Junction", AlamatOp = "Jl. Bubutan No. 1", WilayahPajak = "Bubutan", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50)},
                    new MonitoringCCTV {Nop = "3171010009", NamaOp = "Parkir Ciputra World", AlamatOp = "Jl. Mayjen Sungkono No. 87", WilayahPajak = "Sawahan", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Non-Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50)},
                    new MonitoringCCTV {Nop = "3171010010", NamaOp = "Parkir Grand City", AlamatOp = "Jl. Walikota Mustajab No. 1", WilayahPajak = "Genteng", UptbId = uptbId, TglTerpasang = DateTime.Now.AddMinutes(-5), Vendor = "Jasmita", StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50)}
                };
                return result;
            }

            public static List<MonitoringCCTVDet> GetMonitoringCCTVDetList(string nop)
            {
                return new List<MonitoringCCTVDet>
                {
                    new MonitoringCCTVDet { Nop = "3171010001", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-10).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-9).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Aktif"},
                    new MonitoringCCTVDet { Nop = "3171010002", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-8).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-7).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Down"},
                    new MonitoringCCTVDet { Nop = "3171010003", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-7).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-6).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Aktif"},
                    new MonitoringCCTVDet { Nop = "3171010004", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-6).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-5).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Aktif"},
                    new MonitoringCCTVDet { Nop = "3171010005", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-5).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-4).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Down"},
                    new MonitoringCCTVDet { Nop = "3171010006", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-4).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-3).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Aktif"},
                    new MonitoringCCTVDet { Nop = "3171010007", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-3).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-2).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Down"},
                    new MonitoringCCTVDet { Nop = "3171010008", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-2).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-1).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Aktif"},
                    new MonitoringCCTVDet { Nop = "3171010009", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-1).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(-1).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Aktif"},
                    new MonitoringCCTVDet { Nop = "3171010010", Tgl = DateTime.Today.AddDays(-10), TglAktif = DateTime.Today.AddDays(-1).AddHours(8).AddMinutes(30).AddSeconds(15), TglDown = DateTime.Today.AddDays(1).AddHours(17).AddMinutes(45).AddSeconds(50), StatusAktif = "Aktif"}
                };
            }

            public static List<MonitoringCCTVKapasitas> GetMonitoringCCTVKapasitas(string nop)
            {
                return new List<MonitoringCCTVKapasitas>
                {
                    new MonitoringCCTVKapasitas {
                    Nop = "3171010001",
                    Tanggal = DateTime.Today,
                    KapasitasSepeda = 50,
                    TarifSepeda = 2000m,
                    KapasitasMotor = 200,
                    TarifMotor = 5000m,
                    KapasitasMobil = 100,
                    TarifMobil = 10000m,
                    KapasitasTrukMini = 30,
                    TarifTrukMini = 15000m,
                    KapasitasTrukBus = 20,
                    TarifTrukBus = 20000m,
                    KapasitasTrailer = 10,
                    TarifTrailer = 25000m,
                    EstPajak = 2500000000}
                };

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
            public DateTime TglTerpasang { get; set; }
            public string Vendor { get; set; } = null!;
            public DateTime TglTerakhirAktif { get; set; }
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

            public string DownTime => $"{Hari} Hari {Jam} Jam {Menit} Menit";
        }
        public class MonitoringCCTVKapasitas
        {
            public string Nop { get; set; } = null!;
            public DateTime Tanggal { get; set; }
            // Data kendaraan & tarif
            public int KapasitasSepeda { get; set; }
            public decimal TarifSepeda { get; set; }
            public int KapasitasMotor { get; set; }
            public decimal TarifMotor { get; set; }
            public int KapasitasMobil { get; set; }
            public decimal TarifMobil { get; set; }
            public int KapasitasTrukMini { get; set; }
            public decimal TarifTrukMini { get; set; }
            public int KapasitasTrukBus { get; set; }
            public decimal TarifTrukBus { get; set; }
            public int KapasitasTrailer { get; set; }
            public decimal TarifTrailer { get; set; }
            public decimal EstPajak { get; set; }
        }

    }
    
}
