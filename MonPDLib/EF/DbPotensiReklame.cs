using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbPotensiReklame
{
    [Column("NOR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nor { get; set; }

    [Column("FLAG_PERMOHONAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FlagPermohonan { get; set; }

    [Column("NSR4", TypeName = "NUMBER")]
    public decimal? Nsr4 { get; set; }

    [Column("NSR3", TypeName = "NUMBER")]
    public decimal? Nsr3 { get; set; }

    [Column("NSR2", TypeName = "NUMBER")]
    public decimal? Nsr2 { get; set; }

    [Column("NSR1", TypeName = "NUMBER")]
    public decimal? Nsr1 { get; set; }

    [Column("NSR0", TypeName = "NUMBER")]
    public decimal? Nsr0 { get; set; }

    [Column("RATA2_NSR", TypeName = "NUMBER")]
    public decimal? Rata2Nsr { get; set; }

    [Column("RATA2_PAJAK", TypeName = "NUMBER")]
    public decimal? Rata2Pajak { get; set; }

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal? Status { get; set; }
}
