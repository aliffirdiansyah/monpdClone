using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("T_BERITA")]
public partial class TBeritum
{
    [Key]
    [Column("ID", TypeName = "NUMBER(38)")]
    public decimal Id { get; set; }

    [Column("TGL_BERITA", TypeName = "DATE")]
    public DateTime TglBerita { get; set; }

    [Column("JUDUL")]
    [StringLength(500)]
    [Unicode(false)]
    public string Judul { get; set; } = null!;

    [Column("SINOPSIS")]
    [StringLength(2000)]
    [Unicode(false)]
    public string Sinopsis { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("TAG")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? Tag { get; set; }

    [InverseProperty("IdBeritaNavigation")]
    public virtual TBeritaGambar? TBeritaGambar { get; set; }

    [InverseProperty("IdBeritaNavigation")]
    public virtual TBeritaIsi? TBeritaIsi { get; set; }
}
