using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "KdPajak")]
[Table("POTENSI_CTRL_HIBURAN")]
public partial class PotensiCtrlHiburan
{
    [Key]
    [Column("NOP")]
    [StringLength(23)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("KD_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string KdPajak { get; set; } = null!;

    [Required]
    [Column("STATUS")]
    [Precision(1)]
    public bool? Status { get; set; }

    [Column("JENIS")]
    [Precision(2)]
    public byte Jenis { get; set; }

    [Column("KAP_KURSI_STUDIO")]
    [Precision(10)]
    public int KapKursiStudio { get; set; }

    [Column("KAP_KURSI_STUDIO_TERJUAL_HARI")]
    [Precision(10)]
    public int KapKursiStudioTerjualHari { get; set; }

    [Column("KAP_PENGUNJUNG")]
    [Precision(10)]
    public int KapPengunjung { get; set; }

    [Column("HTM_WD", TypeName = "NUMBER(10,2)")]
    public decimal HtmWd { get; set; }

    [Column("HTM_WE", TypeName = "NUMBER(10,2)")]
    public decimal HtmWe { get; set; }

    [Column("HARGA_MEMBER_BULAN", TypeName = "NUMBER(10,2)")]
    public decimal HargaMemberBulan { get; set; }

    [Column("TO_WD", TypeName = "NUMBER(10,2)")]
    public decimal ToWd { get; set; }

    [Column("TO_WE", TypeName = "NUMBER(10,2)")]
    public decimal ToWe { get; set; }

    [Column("AVG_VIS_WD", TypeName = "NUMBER(10,2)")]
    public decimal AvgVisWd { get; set; }

    [Column("AVG_VIS_WE", TypeName = "NUMBER(10,2)")]
    public decimal AvgVisWe { get; set; }

    [Column("AVG_MEMBER_BULAN", TypeName = "NUMBER(10,2)")]
    public decimal AvgMemberBulan { get; set; }

    [Column("OMZET_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal OmzetBulan { get; set; }

    [Column("POTENSI_PAJAK_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal PotensiPajakBulan { get; set; }

    [Column("POTENSI_PAJAK_TAHUN", TypeName = "NUMBER(18,2)")]
    public decimal PotensiPajakTahun { get; set; }

    [Column("CREATED_AT")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    [Precision(6)]
    public DateTime? UpdatedAt { get; set; }
}
