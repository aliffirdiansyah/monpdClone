using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("T_BERITA_GAMBAR")]
public partial class TBeritaGambar
{
    [Key]
    [Column("ID_BERITA", TypeName = "NUMBER(38)")]
    public decimal IdBerita { get; set; }

    [Column("GAMBAR", TypeName = "BLOB")]
    public byte[] Gambar { get; set; } = null!;

    [ForeignKey("IdBerita")]
    [InverseProperty("TBeritaGambar")]
    public virtual TBeritum IdBeritaNavigation { get; set; } = null!;
}
