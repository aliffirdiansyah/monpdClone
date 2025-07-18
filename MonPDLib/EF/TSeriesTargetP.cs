using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("TahunBuku", "KelompokRek", "JenisRek", "ObyekRek", "RincianRek", "SubrincianRek")]
[Table("T_SERIES_TARGET_P")]
public partial class TSeriesTargetP
{
    [Key]
    [Column("TAHUN_BUKU")]
    [Precision(4)]
    public byte TahunBuku { get; set; }

    [Key]
    [Column("KELOMPOK_REK")]
    [StringLength(10)]
    [Unicode(false)]
    public string KelompokRek { get; set; } = null!;

    [Column("NAMA_KELOMPOK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }

    [Key]
    [Column("JENIS_REK")]
    [StringLength(15)]
    [Unicode(false)]
    public string JenisRek { get; set; } = null!;

    [Column("NAMA_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

    [Key]
    [Column("OBYEK_REK")]
    [StringLength(20)]
    [Unicode(false)]
    public string ObyekRek { get; set; } = null!;

    [Column("NAMA_OBYEK")]
    [StringLength(300)]
    [Unicode(false)]
    public string? NamaObyek { get; set; }

    [Key]
    [Column("RINCIAN_REK")]
    [StringLength(25)]
    [Unicode(false)]
    public string RincianRek { get; set; } = null!;

    [Column("NAMA_RINCIAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaRincian { get; set; }

    [Key]
    [Column("SUBRINCIAN_REK")]
    [StringLength(30)]
    [Unicode(false)]
    public string SubrincianRek { get; set; } = null!;

    [Column("NAMA_SUBRINCIAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaSubrincian { get; set; }

    [Column("TOTAL_TARGET", TypeName = "NUMBER(20,2)")]
    public decimal? TotalTarget { get; set; }
}
