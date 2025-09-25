using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCTVParkirWorker
{
    public class ParkirView
    {
        public string Id { get; set; }
        public string NOP { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public int CCTVId { get; set; }
        public string AccessPoint { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public DateTime? LastConnected { get; set; }
        public string? Err { get; set; }
    }
}
