using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_PERPANJANGAN")]
public partial class TempPerpanjangan
{
    [Column("NOFORM_S")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoformS { get; set; }

    [Column("IS_PERPANJANGAN")]
    [Precision(1)]
    public bool? IsPerpanjangan { get; set; }
}
