using DevExtreme.AspNet.Mvc.Builders;
using MonPDLib.General;
using System.Web.Mvc;

namespace MonPDReborn.Models.CCTVParkir
{
    public class VendorParkirVM
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
            public List<VendorParkir> VendorParkirList { get; set; } = new();
            public Show()
            {
                
            }
            public Show(int uptb)
            {
                VendorParkirList = Method.GetVendorParkirList(uptb);
            }
        }
        public class Method
        {
            public static List<VendorParkir> GetVendorParkirList(int uptb)
            {
                List<VendorParkir> result = new()
                {
                    new VendorParkir { Nop = "NOP001", NamaOp = "Parkir Plaza Surabaya", AlamatOp = "Jl. Pemuda No. 1", WilayahPajak = "Genteng", UptbId = uptb, Vendor = "Jasnita" },
                    new VendorParkir { Nop = "NOP002", NamaOp = "Parkir Tunjungan Plaza", AlamatOp = "Jl. Basuki Rahmat No. 8", WilayahPajak = "Tegalsari", UptbId = uptb, Vendor = "Telkom" },
                    new VendorParkir { Nop = "NOP003", NamaOp = "Parkir Galaxy Mall", AlamatOp = "Jl. Dharmahusada Indah Timur", WilayahPajak = "Mulyorejo", UptbId = uptb, Vendor = "Jasnita" },
                    new VendorParkir { Nop = "NOP004", NamaOp = "Parkir Royal Plaza", AlamatOp = "Jl. Ahmad Yani No. 16", WilayahPajak = "Wonokromo", UptbId = uptb, Vendor = "Telkom" },
                    new VendorParkir { Nop = "NOP005", NamaOp = "Parkir Pakuwon Mall", AlamatOp = "Jl. Puncak Indah Lontar", WilayahPajak = "Sambikerep", UptbId = uptb, Vendor = "Jasnita" },
                    new VendorParkir { Nop = "NOP006", NamaOp = "Parkir City of Tomorrow", AlamatOp = "Jl. Ahmad Yani No. 288", WilayahPajak = "Jambangan", UptbId = uptb, Vendor = "Telkom" },
                    new VendorParkir { Nop = "NOP007", NamaOp = "Parkir Lenmarc", AlamatOp = "Jl. Bukit Darmo Golf", WilayahPajak = "Suko Manunggal", UptbId = uptb, Vendor = "Jasnita" },
                    new VendorParkir { Nop = "NOP008", NamaOp = "Parkir BG Junction", AlamatOp = "Jl. Bubutan No. 1", WilayahPajak = "Bubutan", UptbId = uptb, Vendor = "Telkom" },
                    new VendorParkir { Nop = "NOP009", NamaOp = "Parkir Ciputra World", AlamatOp = "Jl. Mayjen Sungkono No. 87", WilayahPajak = "Sawahan", UptbId = uptb, Vendor = "Jasnita" },
                    new VendorParkir { Nop = "NOP010", NamaOp = "Parkir Grand City", AlamatOp = "Jl. Walikota Mustajab No. 1", WilayahPajak = "Genteng", UptbId = uptb, Vendor = "Telkom" }
                };
                return result;
            }
        }
        public class VendorParkir
        {
            public string Nop { get; set; } = null!;
            public string NamaOp { get; set; } = null!;
            public string AlamatOp { get; set; } = null!;
            public string WilayahPajak { get; set; } = null!;
            public int UptbId { get; set; }
            public string Vendor { get; set; } = null!;
        }
    }
    
}
