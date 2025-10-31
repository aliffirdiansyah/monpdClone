using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Keyless]
[Table("T_CHECK_SSPD")]
public partial class TCheckSspd
{
    [Column("NOFORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Noformulir { get; set; }

    [Column("NOSSPD")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nosspd { get; set; }

    [Column("NOSKPD")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Noskpd { get; set; }

    [Column("TGLBAYAR", TypeName = "DATE")]
    public DateTime? Tglbayar { get; set; }

    [Column("PAJAK", TypeName = "NUMBER")]
    public decimal? Pajak { get; set; }

    [Column("TOTALBAYAR", TypeName = "NUMBER")]
    public decimal? Totalbayar { get; set; }
}
