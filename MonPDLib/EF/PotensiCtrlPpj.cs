using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "KdPajak")]
[Table("POTENSI_CTRL_PPJ")]
public partial class PotensiCtrlPpj
{
    [Key]
    [Column("NOP")]
    [StringLength(23)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("KD_PAJAK")]
    [Precision(1)]
    public bool KdPajak { get; set; }

    [Column("STATUS")]
    [Precision(1)]
    public bool Status { get; set; }

    [Column("JENIS")]
    [Precision(1)]
    public bool Jenis { get; set; }

    [Column("POTENSI_PAJAK_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal? PotensiPajakBulan { get; set; }

    [Column("POTENSI_PAJAK_TAHUN", TypeName = "NUMBER(18,2)")]
    public decimal? PotensiPajakTahun { get; set; }

    [Column("CREATED_AT")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    [Precision(6)]
    public DateTime? UpdatedAt { get; set; }
}
