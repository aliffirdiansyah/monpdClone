using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class ItungAvgPerNop
{
    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("NOP")]
    [StringLength(43)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("AVG_NILAI", TypeName = "NUMBER")]
    public decimal? AvgNilai { get; set; }
}
