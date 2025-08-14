using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DATA_TARGET_PAJAK")]
public partial class DataTargetPajak
{
    [Column("JENIS")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Column("TAHUN", TypeName = "NUMBER(38)")]
    public decimal? Tahun { get; set; }

    [Column("TARGET", TypeName = "NUMBER(18,2)")]
    public decimal? Target { get; set; }
}
