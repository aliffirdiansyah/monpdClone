using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwRekapUpaya
{
    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("SURVEY", TypeName = "NUMBER")]
    public decimal? Survey { get; set; }

    [Column("PEMBERITAHUAN", TypeName = "NUMBER")]
    public decimal? Pemberitahuan { get; set; }

    [Column("SILANG", TypeName = "NUMBER")]
    public decimal? Silang { get; set; }

    [Column("BONGKAR", TypeName = "NUMBER")]
    public decimal? Bongkar { get; set; }

    [Column("BANTIP", TypeName = "NUMBER")]
    public decimal? Bantip { get; set; }

    [Column("JML_KETETAPAN", TypeName = "NUMBER")]
    public decimal? JmlKetetapan { get; set; }

    [Column("NOMINAL_KETETAPAN", TypeName = "NUMBER")]
    public decimal? NominalKetetapan { get; set; }
}
