using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class TPendapatanDaerah
{
    [Column("SEQ", TypeName = "NUMBER")]
    public decimal? Seq { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("URAIAN_REALISASI")]
    [StringLength(50)]
    [Unicode(false)]
    public string? UraianRealisasi { get; set; }

    [Column("JUMLAH_TARGET", TypeName = "NUMBER")]
    public decimal? JumlahTarget { get; set; }

    [Column("JUMLAH_REALISASI", TypeName = "NUMBER")]
    public decimal? JumlahRealisasi { get; set; }
}
