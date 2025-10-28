using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_TARGET_UPTB")]
public partial class TempTargetUptb
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

    [Column("TANGGAL_TARGET")]
    [Precision(2)]
    public byte? TanggalTarget { get; set; }

    [Column("TARGET_HARIAN", TypeName = "NUMBER(18,2)")]
    public decimal? TargetHarian { get; set; }
}
