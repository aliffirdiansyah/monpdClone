using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[PrimaryKey("NomorDaftar", "Seq", "FotoId", "Tahun")]
[Table("T_PERIZINAN_REKLAME_DET_FOTO")]
public partial class TPerizinanReklameDetFoto
{
    [Key]
    [Column("NOMOR_DAFTAR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NomorDaftar { get; set; } = null!;

    [Key]
    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }

    [Key]
    [Column("FOTO_ID", TypeName = "NUMBER")]
    public decimal FotoId { get; set; }

    [Column("FOTO", TypeName = "BLOB")]
    public byte[] Foto { get; set; } = null!;

    [Column("URL")]
    [Unicode(false)]
    public string? Url { get; set; }

    [Key]
    [Column("TAHUN")]
    [Precision(10)]
    public int Tahun { get; set; }

    [ForeignKey("NomorDaftar, Seq, Tahun")]
    [InverseProperty("TPerizinanReklameDetFotos")]
    public virtual TPerizinanReklameDet TPerizinanReklameDet { get; set; } = null!;
}
