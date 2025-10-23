using System;
using System.Collections.Generic;
using System.Globalization;
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
                if (b.Properti == "PERSEN_PAJAK")
                {
                    PERSEN_PAJAK = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "PERSEN_ROKOK")
                {
                    PERSEN_ROKOK = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "JAMBONG_SAMPAI_8M")
                {
                    JAMBONG_SAMPAI_8M = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "JAMBONG_DIATAS_8M")
                {
                    JAMBONG_DIATAS_8M = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "TAMBAH_KETINGGIAN")
                {
                    TAMBAH_KETINGGIAN = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "NILAI_KETINGGIAN")
                {
                    NILAI_KETINGGIAN = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "MINIM_DPP_9_SELEBARAN")
                {
                    MINIM_DPP_9_SELEBARAN = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "MINIM_DPP_10_STIKER")
                {
                    MINIM_DPP_10_STIKER = SafeToDecimal(b.Nilai);
                }
                else if (b.Properti == "MINIM_DPP_15_PERAGAAN")
                {
                    MINIM_DPP_15_PERAGAAN = SafeToDecimal(b.Nilai);
                }
            }
        }
        private static decimal SafeToDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            value = value.Trim();

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;

            if (decimal.TryParse(value, NumberStyles.Any, new CultureInfo("id-ID"), out result))
                return result;

            return 0m;
        }
    }
}
