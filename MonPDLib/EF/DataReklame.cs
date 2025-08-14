using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DATA_REKLAME")]
public partial class DataReklame
{
    [Key]
    [Column("NOR")]
    [StringLength(20)]
    [Unicode(false)]
    public string Nor { get; set; } = null!;

    [Column("FLAG_PERMOHONAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? FlagPermohonan { get; set; }

    [Column("NSR4", TypeName = "NUMBER(15,2)")]
    public decimal? Nsr4 { get; set; }

    [Column("NSR3", TypeName = "NUMBER(15,2)")]
    public decimal? Nsr3 { get; set; }

    [Column("NSR2", TypeName = "NUMBER(15,2)")]
    public decimal? Nsr2 { get; set; }

    [Column("NSR1", TypeName = "NUMBER(15,2)")]
    public decimal? Nsr1 { get; set; }

    [Column("NSR0", TypeName = "NUMBER(15,2)")]
    public decimal? Nsr0 { get; set; }

    [Column("RATA2_NSR", TypeName = "NUMBER(15,2)")]
    public decimal? Rata2Nsr { get; set; }

    [Column("RATA2_PAJAK", TypeName = "NUMBER(15,2)")]
    public decimal? Rata2Pajak { get; set; }

    [Column("STATUS", TypeName = "NUMBER(38)")]
    public decimal? Status { get; set; }
}
