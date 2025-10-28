using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_TARGET_UPTB_19")]
public partial class TempTargetUptb19
{
    [Column("JENIS_PAJAK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? JenisPajak { get; set; }

    [Column("UPTB")]
    [Precision(5)]
    public short? Uptb { get; set; }

    [Column("BULAN")]
    [Precision(2)]
    public byte? Bulan { get; set; }

    [Column("TGL")]
    [Precision(2)]
    public byte? Tgl { get; set; }

    [Column("TARGET_1", TypeName = "NUMBER(18,2)")]
    public decimal? Target1 { get; set; }

    [Column("TARGET_2", TypeName = "NUMBER(18,2)")]
    public decimal? Target2 { get; set; }

    [Column("TARGET_3", TypeName = "NUMBER(18,2)")]
    public decimal? Target3 { get; set; }
}
