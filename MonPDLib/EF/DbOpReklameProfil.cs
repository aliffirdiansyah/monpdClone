using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbOpReklameProfil
{
    [Column("NO_FORMULIR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("TGL_MULAI", TypeName = "DATE")]
    public DateTime? TglMulai { get; set; }

    [Column("TGL_AKHIR", TypeName = "DATE")]
    public DateTime? TglAkhir { get; set; }

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal? Kategori { get; set; }

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal? Status { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }
}
