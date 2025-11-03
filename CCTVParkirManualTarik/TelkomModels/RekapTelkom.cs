using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCTVParkirManualTarik.TelkomModels
{
    public class RekapTelkom
    {
        public string Id { get; set; } = null!;
        public string Nop { get; set; } = null!;
        public string CctvId { get; set; } = null!;
        public string? NamaOp { get; set; }
        public string? AlamatOp { get; set; }
        public int? WilayahPajak { get; set; }
        public DateTime WaktuMasuk { get; set; }
        public int JenisKend { get; set; }
        public string? PlatNo { get; set; }
        public DateTime? WaktuKeluar { get; set; }
        public int Direction { get; set; }
        public string? Log { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Vendor { get; set; }
    }
}
