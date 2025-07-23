using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("NoFormulir", "TglUpaya", "Seq")]
[Table("DB_MON_REKLAME_UPAYA_DOK")]
public partial class DbMonReklameUpayaDok
{
    [Key]
    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Key]
    [Column("TGL_UPAYA", TypeName = "DATE")]
    public DateTime TglUpaya { get; set; }

    [Column("GAMBAR", TypeName = "BLOB")]
    public byte[] Gambar { get; set; } = null!;

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }
}
