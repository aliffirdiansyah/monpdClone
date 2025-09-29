using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwHariFrom2025
{
    [Column("TANGGAL", TypeName = "DATE")]
    public DateTime? Tanggal { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("HARI", TypeName = "NUMBER")]
    public decimal? Hari { get; set; }

    [Column("HARI_NAMA")]
    [StringLength(6)]
    [Unicode(false)]
    public string? HariNama { get; set; }
}
