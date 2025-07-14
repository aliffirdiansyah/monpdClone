using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_HITUPTB")]
public partial class TempHituptb
{
    [Column("TAHUN")]
    [Precision(4)]
    public byte? Tahun { get; set; }

    [Column("PAJAK_ID")]
    [Precision(3)]
    public byte? PajakId { get; set; }

    [Column("NAMA_PAJAK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaPajak { get; set; }

    [Column("BULAN")]
    [Precision(2)]
    public byte? Bulan { get; set; }

    [Column("UPTB")]
    [Precision(2)]
    public byte? Uptb { get; set; }

    [Column("TARGET", TypeName = "NUMBER(20,2)")]
    public decimal? Target { get; set; }
}
