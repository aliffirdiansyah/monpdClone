using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbOpPhr
{
    [Column("ID_SSPD")]
    [StringLength(108)]
    [Unicode(false)]
    public string? IdSspd { get; set; }

    [Column("OP_ID")]
    [StringLength(100)]
    [Unicode(false)]
    public string? OpId { get; set; }

    [Column("THN_PAJAK")]
    [StringLength(40)]
    [Unicode(false)]
    public string? ThnPajak { get; set; }

    [Column("BLN_PAJAK", TypeName = "NUMBER")]
    public decimal? BlnPajak { get; set; }

    [Column("NM_BULAN")]
    [StringLength(9)]
    [Unicode(false)]
    public string? NmBulan { get; set; }

    [Column("JML_POKOK", TypeName = "NUMBER")]
    public decimal? JmlPokok { get; set; }

    [Column("JML_DENDA", TypeName = "NUMBER")]
    public decimal? JmlDenda { get; set; }

    [Column("JML_TOTAL", TypeName = "NUMBER")]
    public decimal? JmlTotal { get; set; }

    [Column("JML_RESTI", TypeName = "NUMBER")]
    public decimal? JmlResti { get; set; }

    [Column("JML_TOTAL_RESTI", TypeName = "NUMBER")]
    public decimal? JmlTotalResti { get; set; }

    [Column("TGL_SETOR", TypeName = "DATE")]
    public DateTime? TglSetor { get; set; }

    [Column("TGL_RESTI", TypeName = "DATE")]
    public DateTime? TglResti { get; set; }

    [Column("CARA_BAYAR")]
    [StringLength(44)]
    [Unicode(false)]
    public string? CaraBayar { get; set; }

    [Column("DIBAYAR")]
    [StringLength(5)]
    [Unicode(false)]
    public string? Dibayar { get; set; }

    [Column("KWITIR", TypeName = "NUMBER")]
    public decimal? Kwitir { get; set; }

    [Column("PORPORASI", TypeName = "NUMBER")]
    public decimal? Porporasi { get; set; }

    [Column("HITUNG_SSPD")]
    [StringLength(7)]
    [Unicode(false)]
    public string? HitungSspd { get; set; }

    [Column("PP_AWAL")]
    [StringLength(10)]
    [Unicode(false)]
    public string? PpAwal { get; set; }

    [Column("PP_AKHIR")]
    [StringLength(10)]
    [Unicode(false)]
    public string? PpAkhir { get; set; }

    [Column("DASAR_SETOR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? DasarSetor { get; set; }

    [Column("REF_DASAR")]
    [StringLength(150)]
    [Unicode(false)]
    public string? RefDasar { get; set; }

    [Column("NM_OP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NmOp { get; set; }

    [Column("ALMT_OP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlmtOp { get; set; }

    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("NPWPD")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Npwpd { get; set; }

    [Column("WIL_PJK_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? WilPjkId { get; set; }

    [Column("NM_WIL_PJK")]
    [StringLength(10)]
    [Unicode(false)]
    public string? NmWilPjk { get; set; }

    [Column("WIL_PJK8_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? WilPjk8Id { get; set; }

    [Column("NM_WIL_PJK8")]
    [StringLength(10)]
    [Unicode(false)]
    public string? NmWilPjk8 { get; set; }

    [Column("PJK_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? PjkId { get; set; }

    [Column("NM_PJK")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NmPjk { get; set; }

    [Column("WIL_BAYAR_ID")]
    [StringLength(37)]
    [Unicode(false)]
    public string? WilBayarId { get; set; }

    [Column("NM_WIL_BAYAR")]
    [StringLength(37)]
    [Unicode(false)]
    public string? NmWilBayar { get; set; }

    [Column("AYAT_ID")]
    [StringLength(30)]
    [Unicode(false)]
    public string? AyatId { get; set; }

    [Column("NM_AYAT")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NmAyat { get; set; }

    [Column("KAT_PJK")]
    [Unicode(false)]
    public string? KatPjk { get; set; }

    [Column("MP_AWAL", TypeName = "DATE")]
    public DateTime? MpAwal { get; set; }

    [Column("MP_AKHIR", TypeName = "DATE")]
    public DateTime? MpAkhir { get; set; }

    [Column("STR_SETOR")]
    [StringLength(10)]
    [Unicode(false)]
    public string? StrSetor { get; set; }

    [Column("THN_SETOR", TypeName = "NUMBER")]
    public decimal? ThnSetor { get; set; }

    [Column("BLN_SETOR", TypeName = "NUMBER")]
    public decimal? BlnSetor { get; set; }

    [Column("TGL_SETORAN", TypeName = "NUMBER")]
    public decimal? TglSetoran { get; set; }

    [Column("JML_MURNI", TypeName = "NUMBER")]
    public decimal? JmlMurni { get; set; }

    [Column("JML_TUNGGAK", TypeName = "NUMBER")]
    public decimal? JmlTunggak { get; set; }

    [Column("VALID_KH", TypeName = "NUMBER")]
    public decimal? ValidKh { get; set; }

    [Column("TGL_VALIDASI", TypeName = "DATE")]
    public DateTime? TglValidasi { get; set; }

    [Column("KEC_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? KecId { get; set; }

    [Column("NM_KECAMATAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NmKecamatan { get; set; }

    [Column("KEL_ID")]
    [StringLength(5)]
    [Unicode(false)]
    public string? KelId { get; set; }

    [Column("NM_KELURAHAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NmKelurahan { get; set; }

    [Column("SYNC_SAH")]
    [StringLength(40)]
    [Unicode(false)]
    public string? SyncSah { get; set; }

    [Column("ESPTPD_ID")]
    [StringLength(18)]
    [Unicode(false)]
    public string? EsptpdId { get; set; }

    [Column("BILL_KD")]
    [StringLength(20)]
    [Unicode(false)]
    public string? BillKd { get; set; }

    [Column("SYNC_SBYTAX")]
    [StringLength(100)]
    [Unicode(false)]
    public string? SyncSbytax { get; set; }

    [Column("NM_LOKASI")]
    [StringLength(37)]
    [Unicode(false)]
    public string? NmLokasi { get; set; }

    [Column("NM_WP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NmWp { get; set; }

    [Column("ALMT_WP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlmtWp { get; set; }
}
