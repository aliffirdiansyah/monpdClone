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
                    new MonitoringCCTV { Nop = "3171010001", NamaOp = "Parkir Plaza Surabaya", AlamatOp = "Jl. Pemuda No. 1", WilayahPajak = "Genteng", UptbId = uptbId, StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Now.AddMinutes(-5) },
                    new MonitoringCCTV { Nop = "3171010002", NamaOp = "Parkir Tunjungan Plaza", AlamatOp = "Jl. Basuki Rahmat No. 8", WilayahPajak = "Tegalsari", UptbId = uptbId, StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Now.AddMinutes(-10) },
                    new MonitoringCCTV { Nop = "3171010003", NamaOp = "Parkir Galaxy Mall", AlamatOp = "Jl. Dharmahusada Indah Timur", WilayahPajak = "Mulyorejo", UptbId = uptbId, StatusAktif = "Non-Aktif", TglTerakhirAktif = DateTime.Now.AddHours(-2) },
                    new MonitoringCCTV { Nop = "3171010004", NamaOp = "Parkir Royal Plaza", AlamatOp = "Jl. Ahmad Yani No. 16", WilayahPajak = "Wonokromo", UptbId = uptbId, StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Now },
                    new MonitoringCCTV { Nop = "3171010005", NamaOp = "Parkir Pakuwon Mall", AlamatOp = "Jl. Puncak Indah Lontar", WilayahPajak = "Sambikerep", UptbId = uptbId, StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Now.AddMinutes(-30) },
                    new MonitoringCCTV { Nop = "3171010006", NamaOp = "Parkir City of Tomorrow", AlamatOp = "Jl. Ahmad Yani No. 288", WilayahPajak = "Jambangan", UptbId = uptbId, StatusAktif = "Non-Aktif", TglTerakhirAktif = DateTime.Now.AddHours(-1) },
                    new MonitoringCCTV { Nop = "3171010007", NamaOp = "Parkir Lenmarc", AlamatOp = "Jl. Bukit Darmo Golf", WilayahPajak = "Sukomanunggal", UptbId = uptbId, StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Now },
                    new MonitoringCCTV { Nop = "3171010008", NamaOp = "Parkir BG Junction", AlamatOp = "Jl. Bubutan No. 1", WilayahPajak = "Bubutan", UptbId = uptbId, StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Now.AddMinutes(-45) },
                    new MonitoringCCTV { Nop = "3171010009", NamaOp = "Parkir Ciputra World", AlamatOp = "Jl. Mayjen Sungkono No. 87", WilayahPajak = "Sawahan", UptbId = uptbId, StatusAktif = "Non-Aktif", TglTerakhirAktif = DateTime.Now.AddHours(-5) },
                    new MonitoringCCTV { Nop = "3171010010", NamaOp = "Parkir Grand City", AlamatOp = "Jl. Walikota Mustajab No. 1", WilayahPajak = "Genteng", UptbId = uptbId, StatusAktif = "Aktif", TglTerakhirAktif = DateTime.Now }
                };
                return result;
            }

            public static List<MonitoringCCTVDet> GetMonitoringCCTVDetList(string nop)
            {
                return new List<MonitoringCCTVDet>
                {
                    new MonitoringCCTVDet { Nop = "3171010001", TglAktif = DateTime.Today.AddDays(-10), TglDown = DateTime.Today.AddDays(-9), StatusAktif = "Aktif", DownTime = 2 },
                    new MonitoringCCTVDet { Nop = "3171010002", TglAktif = DateTime.Today.AddDays(-8), TglDown = DateTime.Today.AddDays(-7), StatusAktif = "Down", DownTime = 5 },
                    new MonitoringCCTVDet { Nop = "3171010003", TglAktif = DateTime.Today.AddDays(-7), TglDown = DateTime.Today.AddDays(-6), StatusAktif = "Aktif", DownTime = 1 },
                    new MonitoringCCTVDet { Nop = "3171010004", TglAktif = DateTime.Today.AddDays(-6), TglDown = DateTime.Today.AddDays(-5), StatusAktif = "Aktif", DownTime = 0 },
                    new MonitoringCCTVDet { Nop = "3171010005", TglAktif = DateTime.Today.AddDays(-5), TglDown = DateTime.Today.AddDays(-4), StatusAktif = "Down", DownTime = 3 },
                    new MonitoringCCTVDet { Nop = "3171010006", TglAktif = DateTime.Today.AddDays(-4), TglDown = DateTime.Today.AddDays(-3), StatusAktif = "Aktif", DownTime = 0 },
                    new MonitoringCCTVDet { Nop = "3171010007", TglAktif = DateTime.Today.AddDays(-3), TglDown = DateTime.Today.AddDays(-2), StatusAktif = "Down", DownTime = 7 },
                    new MonitoringCCTVDet { Nop = "3171010008", TglAktif = DateTime.Today.AddDays(-2), TglDown = DateTime.Today.AddDays(-1), StatusAktif = "Aktif", DownTime = 0 },
                    new MonitoringCCTVDet { Nop = "3171010009", TglAktif = DateTime.Today.AddDays(-1), TglDown = DateTime.Today, StatusAktif = "Aktif", DownTime = 0 },
                    new MonitoringCCTVDet { Nop = "3171010010", TglAktif = DateTime.Today, TglDown = DateTime.Today.AddDays(1), StatusAktif = "Aktif", DownTime = 0 }
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
                    TarifTrailer = 25000m }
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
            public DateTime TglTerakhirAktif { get; set; }
        }
        public class MonitoringCCTVDet
        {
            public string Nop { get; set; } = null!;
            public DateTime TglAktif { get; set; }
            public DateTime TglDown { get; set; }
            public string StatusAktif { get; set; } = null!;
            public int DownTime { get; set; }
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
        }

    }
    
}
