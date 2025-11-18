using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwMonReklameUpaya
{
    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("UPAYA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Upaya { get; set; }

    [Column("KETERANGAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("NOR")]
    [StringLength(12)]
    [Unicode(false)]
    public string Nor { get; set; } = null!;

    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("TGL_SKPD", TypeName = "DATE")]
    public DateTime? TglSkpd { get; set; }

    [Column("KETETAPAN", TypeName = "NUMBER")]
    public decimal? Ketetapan { get; set; }

    [Column("TOTAL_BAYAR", TypeName = "NUMBER")]
    public decimal? TotalBayar { get; set; }
}
