using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("T_REKLAME_SKPD")]
public partial class TReklameSkpd
{
    [Key]
    [Column("NO_KETETAPAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NoKetetapan { get; set; } = null!;

    [Column("FILE_KETETAPAN", TypeName = "BLOB")]
    public byte[]? FileKetetapan { get; set; }

    [Column("TGL_KETETAPAN", TypeName = "DATE")]
    public DateTime? TglKetetapan { get; set; }

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal? Status { get; set; }

    [Column("LINK_DOWN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? LinkDown { get; set; }
}
