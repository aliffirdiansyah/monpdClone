using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DB_POTENSI_REKLAME")]
[Index("Nor", "TahunBuku", "Hit1bulan", Name = "DB_POTENSI_REKLAME_UNIQUE", IsUnique = true)]
public partial class DbPotensiReklame
{
    [Column("NOR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nor { get; set; }

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

    [Column("STATUS")]
    [Precision(1)]
    public bool? Status { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER(20)")]
    public decimal? TahunBuku { get; set; }

    [Column("HIT_1BULAN", TypeName = "NUMBER(20)")]
    public decimal? Hit1bulan { get; set; }
}
