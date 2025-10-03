using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCTVParkirWorker
{
    public class CameraStatusRekap
    {
        public string Guid { get; set; }
        public string AccessPoint { get; set; }
        public string Localization { get; set; }
        public string StateAsli { get; set; }
        public string State { get; set; }
        public EnumFactory.EStatusCCTV StateEnum { get; set; }
        public DateTime Tanggal { get; set; }
    }
}
