using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "Tgl", "Bulan", "Uptb")]
[Table("DB_AKUN_TARGET_BULAN_UPTB")]
public partial class DbAkunTargetBulanUptb
{
    [Key]
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Key]
    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Key]
    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Kelompok { get; set; } = null!;

    [Key]
    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Key]
    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Key]
    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Key]
    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Key]
    [Column("TGL", TypeName = "NUMBER")]
    public decimal Tgl { get; set; }

    [Key]
    [Column("BULAN", TypeName = "NUMBER")]
    public decimal Bulan { get; set; }

    [Key]
    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal Target { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER(38)")]
    public decimal? PajakId { get; set; }

    [ForeignKey("TahunBuku, Akun, Kelompok, Jenis, Objek, Rincian, SubRincian, Tgl, Bulan")]
    [InverseProperty("DbAkunTargetBulanUptbs")]
    public virtual DbAkunTargetBulan DbAkunTargetBulan { get; set; } = null!;
}
