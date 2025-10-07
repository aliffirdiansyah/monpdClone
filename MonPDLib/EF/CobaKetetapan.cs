using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class CobaKetetapan
{
    [Column("TAHUN_PAJAK", TypeName = "NUMBER")]
    public decimal? TahunPajak { get; set; }

    [Column("NOP")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("POKOK_PAJAK", TypeName = "NUMBER")]
    public decimal? PokokPajak { get; set; }

    [Column("TANGGAL_KETETAPAN", TypeName = "DATE")]
    public DateTime? TanggalKetetapan { get; set; }
}
