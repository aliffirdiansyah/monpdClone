using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("T_UPAYA_REKLAME")]
public partial class TUpayaReklame
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Column("TGL_UPAYA", TypeName = "DATE")]
    public DateTime TglUpaya { get; set; }

    [Column("ID_UPAYA", TypeName = "NUMBER")]
    public decimal IdUpaya { get; set; }

    [Column("UPAYA")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Upaya { get; set; }

    [Column("ID_TINDAKAN", TypeName = "NUMBER")]
    public decimal IdTindakan { get; set; }

    [Column("TINDAKAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Tindakan { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Petugas { get; set; }
}
