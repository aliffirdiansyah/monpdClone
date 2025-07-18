using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvSeriesTargetP
{
    [Column("URAIAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Uraian { get; set; }

    [Column("JUMLAH", TypeName = "NUMBER")]
    public decimal? Jumlah { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("SEQ", TypeName = "NUMBER")]
    public decimal? Seq { get; set; }
}
