using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("NoFormulir", "TglUpaya", "Seq")]
[Table("DB_MON_REKLAME_UPAYA")]
public partial class DbMonReklameUpaya
{
    [Key]
    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Key]
    [Column("TGL_UPAYA", TypeName = "DATE")]
    public DateTime TglUpaya { get; set; }

    [Column("UPAYA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Upaya { get; set; }

    [Column("KETERANGAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Petugas { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [InverseProperty("DbMonReklameUpaya")]
    public virtual DbMonReklameUpayaDok? DbMonReklameUpayaDok { get; set; }
}
