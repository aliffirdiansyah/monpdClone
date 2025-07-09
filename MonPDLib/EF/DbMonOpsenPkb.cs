using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_MON_OPSEN_PKB")]
public partial class DbMonOpsenPkb
{
    [Key]
    [Column("ID_SSPD")]
    [StringLength(150)]
    [Unicode(false)]
    public string IdSspd { get; set; } = null!;

    [Column("TGL_SSPD", TypeName = "DATE")]
    public DateTime TglSspd { get; set; }

    [Column("SSPD_TGL_ENTRY", TypeName = "DATE")]
    public DateTime SspdTglEntry { get; set; }

    [Column("ID_AYAT_PAJAK")]
    [StringLength(17)]
    [Unicode(false)]
    public string IdAyatPajak { get; set; } = null!;

    [Column("BULAN_PAJAK_SSPD")]
    [Precision(10)]
    public int BulanPajakSspd { get; set; }

    [Column("TAHUN_PAJAK_SSPD")]
    [Precision(10)]
    public int TahunPajakSspd { get; set; }

    [Column("JML_POKOK", TypeName = "NUMBER(18,2)")]
    public decimal JmlPokok { get; set; }

    [Column("JML_DENDA", TypeName = "NUMBER(18,2)")]
    public decimal? JmlDenda { get; set; }

    [Column("REFF_DASAR_SETORAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ReffDasarSetoran { get; set; }

    [Column("TEMPAT_BAYAR")]
    [StringLength(255)]
    [Unicode(false)]
    public string? TempatBayar { get; set; }

    [Column("SETORAN_BERDASARKAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? SetoranBerdasarkan { get; set; }

    [Column("REKON_DATE", TypeName = "DATE")]
    public DateTime? RekonDate { get; set; }

    [Column("REKON_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? RekonBy { get; set; }

    [Column("DASAR_SETORAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? DasarSetoran { get; set; }

    [Column("NAMA_JENIS_PAJAK")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaJenisPajak { get; set; }

    [Column("DESCRIPTION")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("SAMSAT_ASAL")]
    [StringLength(150)]
    [Unicode(false)]
    public string? SamsatAsal { get; set; }

    [Column("JENIS_BAYAR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? JenisBayar { get; set; }
}
