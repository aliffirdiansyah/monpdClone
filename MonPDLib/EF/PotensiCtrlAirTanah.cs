using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("POTENSI_CTRL_AIR_TANAH")]
public partial class PotensiCtrlAirTanah
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KELOMPOK")]
    [Precision(1)]
    public bool? Kelompok { get; set; }

    [Column("VOL_PENGGUNAAN")]
    [Precision(10)]
    public int? VolPenggunaan { get; set; }

    [Column("VOL_R1")]
    [Precision(10)]
    public int? VolR1 { get; set; }

    [Column("VOL_R2")]
    [Precision(10)]
    public int? VolR2 { get; set; }

    [Column("VOL_R3")]
    [Precision(10)]
    public int? VolR3 { get; set; }

    [Column("VOL_R4")]
    [Precision(10)]
    public int? VolR4 { get; set; }

    [Column("VOL_R5")]
    [Precision(10)]
    public int? VolR5 { get; set; }

    [Column("HDA_R1", TypeName = "NUMBER(10,2)")]
    public decimal? HdaR1 { get; set; }

    [Column("HDA_R2", TypeName = "NUMBER(10,2)")]
    public decimal? HdaR2 { get; set; }

    [Column("HDA_R3", TypeName = "NUMBER(10,2)")]
    public decimal? HdaR3 { get; set; }

    [Column("HDA_R4", TypeName = "NUMBER(10,2)")]
    public decimal? HdaR4 { get; set; }

    [Column("HDA_R5", TypeName = "NUMBER(10,2)")]
    public decimal? HdaR5 { get; set; }

    [Column("NPA_R1", TypeName = "NUMBER(15,2)")]
    public decimal? NpaR1 { get; set; }

    [Column("NPA_R2", TypeName = "NUMBER(15,2)")]
    public decimal? NpaR2 { get; set; }

    [Column("NPA_R3", TypeName = "NUMBER(15,2)")]
    public decimal? NpaR3 { get; set; }

    [Column("NPA_R4", TypeName = "NUMBER(15,2)")]
    public decimal? NpaR4 { get; set; }

    [Column("NPA_R5", TypeName = "NUMBER(15,2)")]
    public decimal? NpaR5 { get; set; }

    [Column("TOTAL_NPA", TypeName = "NUMBER(15,2)")]
    public decimal? TotalNpa { get; set; }

    [Column("PAJAK_AIR_TANAH", TypeName = "NUMBER(15,2)")]
    public decimal? PajakAirTanah { get; set; }
}
