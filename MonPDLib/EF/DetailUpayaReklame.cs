using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DETAIL_UPAYA_REKLAME")]
public partial class DetailUpayaReklame
{
    [Column("NOFORM_S")]
    [StringLength(50)]
    [Unicode(false)]
    public string NoformS { get; set; } = null!;

    [Column("TGL_UPAYA", TypeName = "DATE")]
    public DateTime? TglUpaya { get; set; }

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

    [Column("SEQ")]
    [Precision(10)]
    public int? Seq { get; set; }
}
