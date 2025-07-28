using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvReklameRekapLiar
{
    [Column("NAMA_JALAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJalan { get; set; }

    [Column("KELAS_JALAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KelasJalan { get; set; }

    [Column("JENIS")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Jenis { get; set; }

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

    [Column("AGT", TypeName = "NUMBER")]
    public decimal? Agt { get; set; }

    [Column("SEP", TypeName = "NUMBER")]
    public decimal? Sep { get; set; }

    [Column("OKT", TypeName = "NUMBER")]
    public decimal? Okt { get; set; }

    [Column("NOV", TypeName = "NUMBER")]
    public decimal? Nov { get; set; }

    [Column("DES", TypeName = "NUMBER")]
    public decimal? Des { get; set; }

    [Column("TOTAL", TypeName = "NUMBER")]
    public decimal? Total { get; set; }
}
