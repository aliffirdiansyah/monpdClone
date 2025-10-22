using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPDLib.Lib
{
    internal class SettingReklame
    {
        public decimal PERSEN_PAJAK { get; set; }
        public decimal PERSEN_ROKOK { get; set; }

        public decimal JAMBONG_SAMPAI_8M { get; set; }
        public decimal JAMBONG_DIATAS_8M { get; set; }
        public decimal TAMBAH_KETINGGIAN { get; set; }
        public decimal NILAI_KETINGGIAN { get; set; }
        public decimal MINIM_DPP_9_SELEBARAN { get; set; }
        public decimal MINIM_DPP_10_STIKER { get; set; }
        public decimal MINIM_DPP_15_PERAGAAN { get; set; }
        public SettingReklame()
        {
            var a = DBClass.GetReklameContext().Settings.ToList();
            foreach (var b in a)
            {
                if (b.Properti == "PERSEN_PAJAK_REKLAME")
                {
                    PERSEN_PAJAK = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "PERSEN_ROKOK_REKLAME")
                {
                    PERSEN_ROKOK = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "JAMBONG_SAMPAI_8M")
                {
                    JAMBONG_SAMPAI_8M = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "JAMBONG_DIATAS_8M")
                {
                    JAMBONG_DIATAS_8M = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "TAMBAH_KETINGGIAN_REKLAME")
                {
                    TAMBAH_KETINGGIAN = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "NILAI_KETINGGIAN_REKLAME")
                {
                    NILAI_KETINGGIAN = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "MINIM_DPP_9_SELEBARAN")
                {
                    MINIM_DPP_9_SELEBARAN = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "MINIM_DPP_10_STIKER")
                {
                    MINIM_DPP_10_STIKER = Convert.ToDecimal(b.Nilai);
                }
                else if (b.Properti == "MINIM_DPP_15_PERAGAAN")
                {
                    MINIM_DPP_15_PERAGAAN = Convert.ToDecimal(b.Nilai);
                }
            }
        }
    }
}
