using Microsoft.EntityFrameworkCore;
using MonPDLib.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MonPDLib
{
    public class Helper
    {
        public class NpwpdPhr
        {
            public string NPWPD_NO { get; set; }
            public int JENIS_WP { get; set; }
            public int STATUS { get; set; }
            public string NAMA { get; set; }
            public string ALAMAT { get; set; }
            public string ALAMAT_DOMISILI { get; set; }
            public string KOTA { get; set; }
            public string HP { get; set; }
            public string KONTAK { get; set; }
            public string EMAIL { get; set; }
            public int REF_BLN_PEL { get; set; }
            public int REF_THN_PEL { get; set; }
            public int REF_SEQ_PEL { get; set; }
            public string NPWPD_LAMA { get; set; }
            public int REF_WF { get; set; }
            public string INS_BY { get; set; }
            public string RESET_KEY { get; set; }
        }
        public class OpSkpdSspdReklame
        {
            public string? NO_FORMULIR { get; set; }
            public string? ID_KETETAPAN { get; set; }
            public DateTime? TGLPENETAPAN { get; set; }
            public int? TAHUN_PAJAK { get; set; }
            public int? BULAN_PAJAK { get; set; }
            public decimal? PAJAK_POKOK { get; set; }
            public int? JNS_KETETAPAN { get; set; }
            public DateTime? TGL_JTEMPO_SKPD { get; set; }
            public string? AKUN { get; set; }
            public string? NAMA_AKUN { get; set; }
            public string? KELOMPOK { get; set; }
            public string? NAMA_KELOMPOK { get; set; }
            public string? JENIS { get; set; }
            public string? NAMA_JENIS { get; set; }
            public string? OBJEK { get; set; }
            public string? NAMA_OBJEK { get; set; }
            public string? RINCIAN { get; set; }
            public string? NAMA_RINCIAN { get; set; }
            public string? SUB_RINCIAN { get; set; }
            public string? NAMA_SUB_RINCIAN { get; set; }
            public int? TAHUN_PAJAK_KETETAPAN { get; set; }
            public int? MASA_PAJAK_KETETAPAN { get; set; }
            public int? SEQ_PAJAK_KETETAPAN { get; set; }
            public string? KATEGORI_KETETAPAN { get; set; }
            public DateTime? TGL_KETETAPAN { get; set; }
            public DateTime? TGL_JATUH_TEMPO_BAYAR { get; set; }
            public int? IS_LUNAS_KETETAPAN { get; set; }
            public DateTime? TGL_LUNAS_KETETAPAN { get; set; }
            public decimal? POKOK_PAJAK_KETETAPAN { get; set; }
            public decimal? PENGURANG_POKOK_KETETAPAN { get; set; }
            public string? AKUN_KETETAPAN { get; set; }
            public string? KELOMPOK_KETETAPAN { get; set; }
            public string? JENIS_KETETAPAN { get; set; }
            public string? OBJEK_KETETAPAN { get; set; }
            public string? RINCIAN_KETETAPAN { get; set; }
            public string? SUB_RINCIAN_KETETAPAN { get; set; }
            public DateTime? TGL_BAYAR_POKOK { get; set; }
            public decimal? NOMINAL_POKOK_BAYAR { get; set; }
            public DateTime? TGL_BAYAR_SANKSI { get; set; }
            public decimal? NOMINAL_SANKSI_BAYAR { get; set; }
            public string? AKUN_SANKSI_BAYAR { get; set; }
            public string? KELOMPOK_SANKSI_BAYAR { get; set; }
            public string? JENIS_SANKSI_BAYAR { get; set; }
            public string? OBJEK_SANKSI_BAYAR { get; set; }
            public string? RINCIAN_SANKSI_BAYAR { get; set; }
            public string? SUB_RINCIAN_SANKSI_BAYAR { get; set; }
            public DateTime? TGL_BAYAR_SANKSI_KENAIKAN { get; set; }
            public decimal? NOMINAL_JAMBONG_BAYAR { get; set; }
            public string? AKUN_JAMBONG_BAYAR { get; set; }
            public string? KELOMPOK_JAMBONG_BAYAR { get; set; }
            public string? JENIS_JAMBONG_BAYAR { get; set; }
            public string? OBJEK_JAMBONG_BAYAR { get; set; }
            public string? RINCIAN_JAMBONG_BAYAR { get; set; }
            public string? SUB_RINCIAN_JAMBONG_BAYAR { get; set; }
            public DateTime? INS_DATE { get; set; }
            public string? INS_BY { get; set; }
            public DateTime? UPD_DATE { get; set; }
            public string? UPD_BY { get; set; }
            public string? NO_KETETAPAN { get; set; }
            public int SEQ { get; set; }
        }
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
        public class OPSkpdResto
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
        public class OPSkpdParkir
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
        public class OPSkpdPbb
        {
            public string NOP { get; set; }
            public DateTime TGL_JATUH_TEMPO_BAYAR { get; set; }
            public DateTime TGL_KETETAPAN { get; set; }
            public int TAHUN { get; set; }
            public int MASAPAJAK { get; set; }
            public int SEQ { get; set; }
            public int JENIS_KETETAPAN { get; set; }
            public decimal NILAI_PENGURANG { get; set; }
            public decimal POKOK { get; set; }
            public DateTime? TRANSACTION_DATE { get; set; }
            public decimal? NOMINAL_POKOK { get; set; }
            public decimal? NOMINAL_SANKSI { get; set; }
            public decimal? NOMINAL_ADMINISTRASI { get; set; }
            public decimal? NOMINAL_LAINNYA { get; set; }
            public decimal? PENGURANG_POKOK { get; set; }
            public decimal? PENGURANG_SANSKSI { get; set; }
            public int SEQ_KETETAPAN { get; set; }
        }
        public class OpOpsenSkpdPkb
        {
            public string ID_SSPD { get; set; } = null!;
            public DateTime TGL_SSPD { get; set; }
            public DateTime SSPD_TGL_ENTRY { get; set; }
            public string ID_AYAT_PAJAK { get; set; } = null!;
            public int BULAN_PAJAK_SSPD { get; set; }
            public int TAHUN_PAJAK_SSPD { get; set; }
            public decimal JML_POKOK { get; set; }
            public decimal? JML_DENDA { get; set; }
            public string? REFF_DASAR_SETORAN { get; set; }
            public string? TEMPAT_BAYAR { get; set; }
            public string? SETORAN_BERDASARKAN { get; set; }
            public DateTime? REKON_DATE { get; set; }
            public string? REKON_BY { get; set; }
            public string? DASAR_SETORAN { get; set; }
            public string? NAMA_JENIS_PAJAK { get; set; }
            public string? DESCRIPTION { get; set; }
            public string? SAMSAT_ASAL { get; set; }
            public string? JENIS_BAYAR { get; set; }
        }
        public class OpOpsenSkpdBbnkb
        {
            public string ID_SSPD { get; set; } = null!;
            public DateTime TGL_SSPD { get; set; }
            public DateTime SSPD_TGL_ENTRY { get; set; }
            public string ID_AYAT_PAJAK { get; set; } = null!;
            public int BULAN_PAJAK_SSPD { get; set; }
            public int TAHUN_PAJAK_SSPD { get; set; }
            public decimal JML_POKOK { get; set; }
            public decimal? JML_DENDA { get; set; }
            public string? REFF_DASAR_SETORAN { get; set; }
            public string? TEMPAT_BAYAR { get; set; }
            public string? SETORAN_BERDASARKAN { get; set; }
            public DateTime? REKON_DATE { get; set; }
            public string? REKON_BY { get; set; }
            public string? DASAR_SETORAN { get; set; }
            public string? NAMA_JENIS_PAJAK { get; set; }
            public string? DESCRIPTION { get; set; }
            public string? SAMSAT_ASAL { get; set; }
            public string? JENIS_BAYAR { get; set; }
        }
        public class OpSkpdBphtb
        {
            public string IDSSPD { get; set; }
            public DateTime TGL_BAYAR { get; set; }
            public DateTime TGL_DATA { get; set; }
            public string AKUN { get; set; }
            public string NAMA_AKUN { get; set; }
            public string JENIS { get; set; }
            public string NAMA_JENIS { get; set; }
            public string OBJEK { get; set; }
            public string NAMA_OBJEK { get; set; }
            public string RINCIAN { get; set; }
            public string NAMA_RINCIAN { get; set; }
            public string SUB_RINCIAN { get; set; }
            public string NAMA_SUB_RINCIAN { get; set; }
            public string SPPT_NOP { get; set; }
            public string NAMA_WP { get; set; }
            public string ALAMAT { get; set; }
            public int MASA { get; set; }
            public int TAHUN { get; set; }
            public decimal POKOK { get; set; }
            public decimal SANKSI { get; set; }
            public string NOMORDASARSETOR { get; set; }
            public string TEMPATBAYAR { get; set; }
            public string REFSETORAN { get; set; }
            public DateTime REKON_DATE { get; set; }
            public string REKON_BY { get; set; }
            public string KD_PEROLEHAN { get; set; }
            public int KD_BYR { get; set; }
            public string KODE_NOTARIS { get; set; }
            public string KD_PELAYANAN { get; set; }
            public string PEROLEHAN { get; set; }
            public string KD_CAMAT { get; set; }
            public string KD_LURAH { get; set; }
            public string KELOMPOK { get; set; }
            public string NAMA_KELOMPOK { get; set; }
        }
        public class OPSkpdHiburan
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
        public class OPSkpdListrik
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
            public string NO_KETETAPAN { get; set; }
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

        public class OPPbb
        {
            public string NOP { get; set; }
            public int KATEGORI_ID { get; set; }
            public string KATEGORI_NAMA { get; set; }
            public string ALAMAT_OP { get; set; }
            public string ALAMAT_OP_NO { get; set; }
            public string ALAMAT_OP_RT { get; set; }
            public string ALAMAT_OP_RW { get; set; }
            public string ALAMAT_KD_CAMAT { get; set; }
            public string ALAMAT_KD_LURAH { get; set; }
            public int UPTB { get; set; }
            public int LUAS_TANAH { get; set; }
            public string ALAMAT_WP { get; set; }
            public string ALAMAT_WP_NO { get; set; }
            public string ALAMAT_WP_KEL { get; set; }
            public string ALAMAT_WP_KOTA { get; set; }
            public string WP_KTP { get; set; }
            public string WP_NAMA { get; set; }
            public string WP_NPWP { get; set; }
            public string WP_RT { get; set; }
            public string WP_RW { get; set; }
            public int STATUS { get; set; }
            public DateTime INS_DATE { get; set; }
            public string INS_BY { get; set; }
        }

        public class KetetapanPbb
        {
            public string NOP { get; set; } = null!;            
            public int TAHUN_BUKU { get; set; }            
            public int KATEGORI_ID { get; set; }            
            public string KATEGORI_NAMA { get; set; } = null!;            
            public string ALAMAT_OP { get; set; } = null!;            
            public string ALAMAT_OP_NO { get; set; } = null!;            
            public string ALAMAT_OP_RT { get; set; } = null!;            
            public string ALAMAT_OP_RW { get; set; } = null!;            
            public string ALAMAT_KD_CAMAT { get; set; } = null!;            
            public string ALAMAT_KD_LURAH { get; set; } = null!;            
            public int UPTB { get; set; }            
            public string ALAMAT_WP { get; set; }            

            public string ALAMAT_WP_NO { get; set; }            
            public string ALAMAT_WP_KEL { get; set; }            
            public string ALAMAT_WP_KOTA { get; set; }            

            public string WP_NAMA { get; set; } = null!;            
            public string WP_NPWP { get; set; }            
            public int TAHUN_PAJAK { get; set; }            

            public decimal POKOK_PAJAK { get; set; }            
            public string KATEGORI_OP { get; set; }            
            public string PERUNTUKAN { get; set; }            

            public int IS_LUNAS { get; set; }            
            public DateTime? TGL_BAYAR { get; set; }            
            public decimal JUMLAH_BAYAR_POKOK { get; set; }

            public decimal JUMLAH_BAYAR_SANKSI { get; set; }
            public DateTime INS_DATE { get; set; }            
            public string INS_BY { get; set; } = null!;
        }


        public class KetetapanPbbAsync
        {
            public string NOP { get; set; } = null!;
            public decimal TAHUN_BUKU { get; set; }
            public decimal KATEGORI_ID { get; set; }

            public string KATEGORI_NAMA { get; set; } = null!;
            public string ALAMAT_OP { get; set; } = null!;
            public string ALAMAT_OP_NO { get; set; } = null!;

            public string ALAMAT_OP_RT { get; set; } = null!;
            public string ALAMAT_OP_RW { get; set; } = null!;
            public string ALAMAT_KD_CAMAT { get; set; } = null!;

            public string ALAMAT_KD_LURAH { get; set; } = null!;
            public decimal UPTB { get; set; }
            public string ALAMAT_WP { get; set; }

            public string ALAMAT_WP_NO { get; set; }
            public string ALAMAT_WP_KEL { get; set; }
            public string ALAMAT_WP_KOTA { get; set; }

            public string WP_NAMA { get; set; } = null!;
            public string WP_NPWP { get; set; }
            public decimal TAHUN_PAJAK { get; set; }

            public decimal POKOK_PAJAK { get; set; }
            public string KATEGORI_OP { get; set; }
            public string PERUNTUKAN { get; set; }

            public decimal IS_LUNAS { get; set; }
            public DateTime? TGL_BAYAR { get; set; }
            public decimal JUMLAH_BAYAR_POKOK { get; set; }

            public decimal JUMLAH_BAYAR_SANKSI { get; set; }
            public decimal POKOK_BANK { get; set; }

            public decimal DENDA_BANK { get; set; }

            public DateTime INS_DATE { get; set; }
            public string INS_BY { get; set; } = null!;
        }
        public class RealisasiPbb
        {
            public string NOP { get; set; } = null!;
            public int TAHUN_PAJAK { get; set; }
            public decimal POKOK { get; set; }
            public decimal SANKSI { get; set; }            
        }

        public class KetetapanHPP
        {
            public string NOP { get; set; } = null!;
            public decimal TAHUN_PAJAK  { get; set; }
            public decimal MASA_PAJAK { get; set; }
            public int SEQ { get; set; }
            public int JENIS_KETETAPAN { get; set; }
            public DateTime TGL_KETETAPAN { get; set; } 
            public DateTime TGL_JATUH_TEMPO_BAYAR { get; set; }
            public int NILAI_P0ENGURANG { get; set; }
            public decimal POKOK { get; set; }
            
        }
        public class SSPDHPP
        {
            public string NOP { get; set; } = null!;
            public decimal MASA_PAJAK { get; set; }
            public decimal TAHUN_PAJAK { get; set; }
            public decimal SEQ { get; set; }
            public DateTime JATUH_TEMPO { get; set; }
            public decimal NOMINAL_POKOK { get; set; }
            public decimal NOMINAL_SANKSI { get; set; }
            public DateTime TRANSACTION_DATE { get; set; }
        }

        public class SSPDABT
        {
            public string NOP { get; set; } = null!;
            public decimal MASA_PAJAK { get; set; }
            public decimal TAHUN_PAJAK { get; set; }
            public decimal SEQ { get; set; }
            public DateTime JATUH_TEMPO { get; set; }
            public decimal NOMINAL_POKOK { get; set; }
            public decimal NOMINAL_SANKSI { get; set; }
            public DateTime TRANSACTION_DATE { get; set; }
            public string JENIS_PERUNTUKAN { get; set; } = null!;
        }
        public class SSPDPBB
        {
            public string NOP { get; set; } = null!;            
            public decimal TAHUN_PAJAK { get; set; }                        
            public decimal POKOK { get; set; }
            public string? LUNAS { get; set; }
            public DateTime? TGL_BAYAR { get; set; }
            public decimal BAYAR_POKOK { get; set; }
            public decimal BAYAR_SANKSI { get; set; }
            public int ID_PERUNTUKAN { get; set; }
            public string PERUNTUKAN { get; set; }=null!;
        }

        public class KETETAPANPBJT
        {
            public string ID_KETETAPAN { get; set; } = null!;
            public string? NOP { get; set; } = null!;
            public decimal TAHUN_PAJAK { get; set; }
            public decimal MASA_PAJAK { get; set; }
            public decimal SEQ { get; set; }
            public DateTime TGL_SPTPD { get; set; }
            public decimal KETETAPAN_TOTAL { get; set; }
            public DateTime TGL_JATUH_TEMPO { get; set; }            
            public string JENIS_KETETAPAN { get; set; } = null!;
            public int PAJAK_ID { get; set; }
            public string NAMA_PAJAK_DAERAH { get; set; } = null!;

        }

        public class KETETAPANHR
        {
            public string ID_KETETAPAN { get; set; } = null!;
            public string? NOP { get; set; } = null!;
            public decimal TAHUN_PAJAK { get; set; }
            public decimal MASA_PAJAK { get; set; }
            public decimal SEQ { get; set; }
            public DateTime? TGL_SPTPD { get; set; }
            public decimal KETETAPAN_TOTAL { get; set; }
            public DateTime? TGL_JATUH_TEMPO { get; set; }
            public string JENIS_KETETAPAN { get; set; } = null!;
            public int PAJAK_ID { get; set; }
            public string NAMA_PAJAK_DAERAH { get; set; } = null!;

        }
    }
}
