using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbAkunTargetObjekReklame
{
    [Column("NO_FORMULIR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("NOR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nor { get; set; }

    [Column("FLAG_PERMOHONAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FlagPermohonan { get; set; }

    [Column("BULAN_BUKU", TypeName = "NUMBER")]
    public decimal? BulanBuku { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("TARGET_BULAN", TypeName = "NUMBER")]
    public decimal? TargetBulan { get; set; }
}
