using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CctvRealtimeWs.JasnitaModels
{
    public class RekapJasnita2
    {
        public string Id { get; set; } = null!;
        public string Nop { get; set; } = null!;
        public string CctvId { get; set; } = null!;
        public int VendorId { get; set; }
        public int JenisKend { get; set; }
        public string? PlatNo { get; set; }
        public DateTime WaktuMasuk { get; set; }
        public byte[] ImageData { get; set; }
    }
}
