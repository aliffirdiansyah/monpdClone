using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbAkunTargetObjekHiburan
{
    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("AVG_NILAI", TypeName = "NUMBER")]
    public decimal? AvgNilai { get; set; }

    [Column("TOTAL_AVG_NILAI", TypeName = "NUMBER")]
    public decimal? TotalAvgNilai { get; set; }

    [Column("KOEFISIEN", TypeName = "NUMBER")]
    public decimal? Koefisien { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("TARGET_TAHUN", TypeName = "NUMBER")]
    public decimal? TargetTahun { get; set; }
}
