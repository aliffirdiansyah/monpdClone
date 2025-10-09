using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class CobaPetuga
{
    [Column("NOP")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("PETUGAS")]
    [StringLength(6)]
    [Unicode(false)]
    public string? Petugas { get; set; }

    [Column("TGL_AWAL", TypeName = "DATE")]
    public DateTime? TglAwal { get; set; }

    [Column("TGL_AKHIR", TypeName = "DATE")]
    public DateTime? TglAkhir { get; set; }

    [Column("UPAYA")]
    [StringLength(13)]
    [Unicode(false)]
    public string? Upaya { get; set; }
}
