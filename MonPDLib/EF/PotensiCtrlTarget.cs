using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "KdPajak", "Tahun")]
[Table("POTENSI_CTRL_TARGET")]
public partial class PotensiCtrlTarget
{
    [Key]
    [Column("NOP")]
    [StringLength(23)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("KD_PAJAK")]
    [Precision(10)]
    public int KdPajak { get; set; }

    [Key]
    [Column("TAHUN")]
    [Precision(10)]
    public int Tahun { get; set; }

    [Column("TARGET", TypeName = "NUMBER(18,2)")]
    public decimal Target { get; set; }

    [Column("CREATED_AT")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }
}
