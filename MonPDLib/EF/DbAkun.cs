using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian")]
[Table("DB_AKUN")]
public partial class DbAkun
{
    [Key]
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Key]
    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("NAMA_AKUN")]
    [StringLength(500)]
    [Unicode(false)]
    public string NamaAkun { get; set; } = null!;

    [Key]
    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Kelompok { get; set; } = null!;

    [Column("NAMA_KELOMPOK")]
    [StringLength(500)]
    [Unicode(false)]
    public string NamaKelompok { get; set; } = null!;

    [Key]
    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("NAMA_JENIS")]
    [StringLength(500)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Key]
    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("NAMA_OBJEK")]
    [StringLength(500)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Key]
    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("NAMA_RINCIAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string NamaRincian { get; set; } = null!;

    [Key]
    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string NamaSubRincian { get; set; } = null!;

    [ForeignKey("TahunBuku, Akun, Kelompok, Jenis, Objek, Rincian, SubRincian")]
    [InverseProperty("DbAkunsNavigation")]
    public virtual ICollection<MKategoriPajak> KategoriKenaikans { get; set; } = new List<MKategoriPajak>();

    [ForeignKey("TahunBuku, Akun, Kelompok, Jenis, Objek, Rincian, SubRincian")]
    [InverseProperty("DbAkuns1")]
    public virtual ICollection<MKategoriPajak> KategoriSanksis { get; set; } = new List<MKategoriPajak>();

    [ForeignKey("TahunBuku, Akun, Kelompok, Jenis, Objek, Rincian, SubRincian")]
    [InverseProperty("DbAkuns")]
    public virtual ICollection<MKategoriPajak> Kategoris { get; set; } = new List<MKategoriPajak>();
}
