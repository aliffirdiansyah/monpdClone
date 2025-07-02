using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPDLib
{
    public class Helper
    {
        public class DbAkun
        {
            public string Akun { get; set; } = null!;
            public string NamaAkun { get; set; } = null!;
            public string Kelompok { get; set; } = null!;
            public string NamaKelompok { get; set; } = null!;
            public string Jenis { get; set; } = null!;
            public string NamaJenis { get; set; } = null!;
            public string Objek { get; set; } = null!;
            public string NamaObjek { get; set; } = null!;
            public string Rincian { get; set; } = null!;
            public string NamaRincian { get; set; } = null!;
            public string SubRincian { get; set; } = null!;
            public string NamaSubRincian { get; set; } = null!;
        }
        public class OPSkpdHotel
        {
            public string NOP { get; set; }
            public int TAHUN { get; set; }
            public int MASAPAJAK { get; set; }
            public int SEQ { get; set; }
            public int JENIS_KETETAPAN { get; set; }
            public DateTime TGL_KETETAPAN { get; set; }
            public DateTime? TGL_JATUH_TEMPO_BAYAR { get; set; }
            public decimal NILAI_PENGURANG { get; set; }
            public decimal POKOK { get; set; }
        }
        public class OPSkpdAbt
        {
            public string NOP { get; set; }
            public int TAHUN { get; set; }
            public int MASAPAJAK { get; set; }
            public int SEQ { get; set; }
            public int JENIS_KETETAPAN { get; set; }
            public string NPWPD { get; set; }
            public int? AKUN { get; set; }
            public int? AKUN_JENIS { get; set; }
            public int? AKUN_JENIS_OBJEK { get; set; }
            public int? AKUN_JENIS_OBJEK_RINCIAN { get; set; }
            public int? AKUN_JENIS_OBJEK_RINCIAN_SUB { get; set; }
            public DateTime TGL_KETETAPAN { get; set; }
            public decimal POKOK { get; set; }
            public decimal? SANKSI_TERLAMBAT_LAPOR { get; set; }
            public decimal SANKSI_ADMINISTRASI { get; set; }
            public decimal PROSEN_TARIF_PAJAK { get; set; }
            public decimal PROSEN_SANKSI_TELAT_BAYAR { get; set; }
            public DateTime TGL_JATUH_TEMPO_BAYAR { get; set; }
            public DateTime? TGL_JATUH_TEMPO_LAPOR { get; set; }
            public int JATUH_TEMPO_LAPOR_MODE { get; set; }
            public int JATUH_TEMPO_BAYAR { get; set; }
            public int JATUH_TEMPO_BAYAR_MODE { get; set; }
            public int KELOMPOK_ID { get; set; }
            public string KELOMPOK_NAMA { get; set; }
            public decimal VOL_PENGGUNAAN_AIR { get; set; }
            public int STATUS_BATAL { get; set; }
            public string? BATAL_KET { get; set; }
            public DateTime? BATAL_DATE { get; set; }
            public string? BATAL_BY { get; set; }
            public string? BATAL_REF { get; set; }
            public DateTime INS_DATE { get; set; }
            public string INS_BY { get; set; }
            public int PERUNTUKAN { get; set; }
            public int NILAI_PENGURANG { get; set; }
            public int JENIS_PENGURANG { get; set; }
            public string? REFF_PENGURANG { get; set; }
        }

        public class SSPDPbjt
        {
            public string NOP { get; set; }
            public int TAHUN_PAJAK { get; set; }
            public int BULAN_PAJAK { get; set; }
            public DateTime TRANSACTION_DATE { get; set; }
            public decimal NOMINAL_POKOK { get; set; }
            public decimal NOMINAL_SANKSI { get; set; }
            public decimal NOMINAL_ADMINISTRASI { get; set; }
            public decimal NOMINAL_LAINNYA { get; set; }
            public decimal PENGURANG_POKOK { get; set; }
            public decimal PENGURANG_SANSKSI { get; set; }
            public decimal SEQ_KETETAPAN { get; set; }
        }
        public class SSPD
        {
            public string ID_SSPD { get; set; } = string.Empty;
            public string KODE_BILL { get; set; } = string.Empty;
            public string NO_KETETAPAN { get; set; } = string.Empty;
            public decimal JENIS_PEMBAYARAN { get; set; }
            public decimal JENIS_PAJAK { get; set; }
            public decimal JENIS_KETETAPAN { get; set; }
            public DateTime JATUH_TEMPO { get; set; }
            public string NOP { get; set; } = string.Empty;
            public decimal MASA { get; set; }
            public decimal TAHUN { get; set; }
            public decimal NOMINAL_POKOK { get; set; }
            public decimal NOMINAL_SANKSI { get; set; } = 0;
            public decimal NOMINAL_ADMINISTRASI { get; set; } = 0;
            public decimal NOMINAL_LAINYA { get; set; } = 0;
            public decimal PENGURANG_POKOK { get; set; } = 0;
            public decimal PENGURANG_SANKSI { get; set; } = 0;
            public string? REFF_PENGURANG_POKOK { get; set; }
            public string? REFF_PENGURANG_SANKSI { get; set; }
            public string AKUN_POKOK { get; set; } = string.Empty;
            public string AKUN_SANKSI { get; set; } = string.Empty;
            public string? AKUN_ADMINISTRASI { get; set; }
            public string? AKUN_LAINNYA { get; set; }
            public string? AKUN_PENGURANG_POKOK { get; set; }
            public string? AKUN_PENGURANG_SANKSI { get; set; }
            public string INVOICE_NUMBER { get; set; } = string.Empty;
            public DateTime TRANSACTION_DATE { get; set; }
            public string? NO_NTPD { get; set; }
            public decimal STATUS_NTPD { get; set; } = 0;
            public DateTime? REKON_DATE { get; set; }
            public string? REKON_BY { get; set; }
            public string? REKON_REFF { get; set; }
            public decimal SEQ_KETETAPAN { get; set; }
            public DateTime INS_DATE { get; set; } = DateTime.Now;
        }
    }
}
