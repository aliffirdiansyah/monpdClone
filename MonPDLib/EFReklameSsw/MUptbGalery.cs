using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_UPTB_GALERY")]
public partial class MUptbGalery
{
    [Key]
    [Column("ID", TypeName = "NUMBER(38)")]
    public decimal Id { get; set; }

    [Column("UPTB", TypeName = "NUMBER(38)")]
    public decimal Uptb { get; set; }

    [Column("GAMBAR", TypeName = "BLOB")]
    public byte[] Gambar { get; set; } = null!;

    [Column("DESKRIPSI")]
    [StringLength(500)]
    [Unicode(false)]
    public string Deskripsi { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("Uptb")]
    [InverseProperty("MUptbGaleries")]
    public virtual MUptb UptbNavigation { get; set; } = null!;
}
