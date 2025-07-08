using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "Tgl", "Bulan")]
[Table("DB_AKUN_TARGET_BULAN")]
public partial class DbAkunTargetBulan
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

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal Target { get; set; }

    [ForeignKey("TahunBuku, Akun, Kelompok, Jenis, Objek, Rincian, SubRincian")]
    [InverseProperty("DbAkunTargetBulans")]
    public virtual DbAkunTarget DbAkunTarget { get; set; } = null!;

    [InverseProperty("DbAkunTargetBulan")]
    public virtual ICollection<DbAkunTargetBulanUptb> DbAkunTargetBulanUptbs { get; set; } = new List<DbAkunTargetBulanUptb>();
}
