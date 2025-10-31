using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("T_PERSETUJUAN_TEKNIS")]
public partial class TPersetujuanTekni
{
    [Key]
    [Column("NO_KETETAPAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NoKetetapan { get; set; } = null!;

    [Column("NO_PERTEK")]
    [StringLength(150)]
    [Unicode(false)]
    public string NoPertek { get; set; } = null!;

    [Column("TGL_PERTEK", TypeName = "DATE")]
    public DateTime? TglPertek { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }

    [Column("NO_SSPD")]
    [StringLength(150)]
    [Unicode(false)]
    public string NoSspd { get; set; } = null!;
}
