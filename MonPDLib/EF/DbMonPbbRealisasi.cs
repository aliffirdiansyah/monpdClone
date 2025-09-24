using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("TahunBuku", "Prop", "Dati", "Kec", "Kel", "Blok", "Urut", "Jenis", "TahunPajak")]
[Table("DB_MON_PBB_REALISASI")]
public partial class DbMonPbbRealisasi
{
    [Key]
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Key]
    [Column("PROP")]
    [StringLength(2)]
    [Unicode(false)]
    public string Prop { get; set; } = null!;

    [Key]
    [Column("DATI")]
    [StringLength(2)]
    [Unicode(false)]
    public string Dati { get; set; } = null!;

    [Key]
    [Column("KEC")]
    [StringLength(3)]
    [Unicode(false)]
    public string Kec { get; set; } = null!;

    [Key]
    [Column("KEL")]
    [StringLength(3)]
    [Unicode(false)]
    public string Kel { get; set; } = null!;

    [Key]
    [Column("BLOK")]
    [StringLength(3)]
    [Unicode(false)]
    public string Blok { get; set; } = null!;

    [Key]
    [Column("URUT")]
    [StringLength(4)]
    [Unicode(false)]
    public string Urut { get; set; } = null!;

    [Key]
    [Column("JENIS")]
    [StringLength(1)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Key]
    [Column("TAHUN_PAJAK", TypeName = "NUMBER")]
    public decimal TahunPajak { get; set; }

    [Column("TGL_BAYAR", TypeName = "DATE")]
    public DateTime TglBayar { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal Realisasi { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
