using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("NoFormulir", "TglKirimEmail")]
[Table("DB_MON_REKLAME_EMAIL")]
public partial class DbMonReklameEmail
{
    [Key]
    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Key]
    [Column("TGL_KIRIM_EMAIL", TypeName = "DATE")]
    public DateTime TglKirimEmail { get; set; }

    [Column("STATUS_EMAIL")]
    [StringLength(20)]
    [Unicode(false)]
    public string? StatusEmail { get; set; }

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("KET_EMAIL")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? KetEmail { get; set; }
}
