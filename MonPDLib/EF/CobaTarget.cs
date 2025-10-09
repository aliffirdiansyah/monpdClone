using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class CobaTarget
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("NAMA_AKUN")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaAkun { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("NAMA_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("NAMA_OBJEK")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("NAMA_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaRincian { get; set; } = null!;

    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaSubRincian { get; set; } = null!;

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal PajakId { get; set; }

    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Kelompok { get; set; } = null!;

    [Column("NAMA_KELOMPOK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }
}
