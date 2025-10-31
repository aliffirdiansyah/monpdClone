using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_GAMBAR_DEPAN")]
public partial class MGambarDepan
{
    [Key]
    [Column("ID")]
    [StringLength(1)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [Column("GAMBAR", TypeName = "BLOB")]
    public byte[] Gambar { get; set; } = null!;

    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }
}
