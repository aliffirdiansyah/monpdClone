using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("NoformS", "TglUpaya", "Seq")]
[Table("DB_MON_REKLAME_UPAYA_DOK")]
public partial class DbMonReklameUpayaDok
{
    [Key]
    [Column("NOFORM_S")]
    [StringLength(50)]
    [Unicode(false)]
    public string NoformS { get; set; } = null!;

    [Key]
    [Column("TGL_UPAYA", TypeName = "DATE")]
    public DateTime TglUpaya { get; set; }

    [Column("GAMBAR", TypeName = "BLOB")]
    public byte[] Gambar { get; set; } = null!;

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [ForeignKey("NoformS, TglUpaya, Seq")]
    [InverseProperty("DbMonReklameUpayaDok")]
    public virtual DbMonReklameUpaya DbMonReklameUpaya { get; set; } = null!;
}
