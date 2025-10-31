using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DB_AKUN_TARGET_BULAN_UPTB")]
public partial class DbAkunTargetBulanUptb
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Kelompok { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("TGL", TypeName = "NUMBER")]
    public decimal Tgl { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal Bulan { get; set; }

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal Target { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER(38)")]
    public decimal? PajakId { get; set; }

    [ForeignKey("TahunBuku, Akun, Kelompok, Jenis, Objek, Rincian, SubRincian, Tgl, Bulan")]
    public virtual DbAkunTargetBulan DbAkunTargetBulan { get; set; } = null!;
}
