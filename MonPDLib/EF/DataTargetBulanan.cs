using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DATA_TARGET_BULANAN")]
public partial class DataTargetBulanan
{
    [Column("KODE_SUB_RINCIAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? KodeSubRincian { get; set; }

    [Column("RINCIAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Rincian { get; set; }

    [Column("JAN", TypeName = "NUMBER")]
    public decimal? Jan { get; set; }

    [Column("FEB", TypeName = "NUMBER")]
    public decimal? Feb { get; set; }

    [Column("MAR", TypeName = "NUMBER")]
    public decimal? Mar { get; set; }

    [Column("APR", TypeName = "NUMBER")]
    public decimal? Apr { get; set; }

    [Column("MEI", TypeName = "NUMBER")]
    public decimal? Mei { get; set; }

    [Column("JUN", TypeName = "NUMBER")]
    public decimal? Jun { get; set; }

    [Column("JUL", TypeName = "NUMBER")]
    public decimal? Jul { get; set; }

    [Column("AGU", TypeName = "NUMBER")]
    public decimal? Agu { get; set; }

    [Column("SEP", TypeName = "NUMBER")]
    public decimal? Sep { get; set; }

    [Column("OKT", TypeName = "NUMBER")]
    public decimal? Okt { get; set; }

    [Column("NOV", TypeName = "NUMBER")]
    public decimal? Nov { get; set; }

    [Column("DES", TypeName = "NUMBER")]
    public decimal? Des { get; set; }

    [Column("TOTAL_TAHUN", TypeName = "NUMBER")]
    public decimal? TotalTahun { get; set; }
}
