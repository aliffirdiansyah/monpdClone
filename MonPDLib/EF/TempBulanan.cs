using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_BULANAN")]
public partial class TempBulanan
{
    [Column("KODE_AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KodeAkun { get; set; }

    [Column("KODE_JENIS")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KodeJenis { get; set; }

    [Column("KODE_OBJEK")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KodeObjek { get; set; }

    [Column("TAHUN_BUKU")]
    [Precision(4)]
    public byte? TahunBuku { get; set; }

    [Column("KODE_RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KodeRincian { get; set; }

    [Column("OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Opd { get; set; }

    [Column("JAN", TypeName = "NUMBER(20)")]
    public decimal? Jan { get; set; }

    [Column("FEB", TypeName = "NUMBER(20)")]
    public decimal? Feb { get; set; }

    [Column("MAR", TypeName = "NUMBER(20)")]
    public decimal? Mar { get; set; }

    [Column("APR", TypeName = "NUMBER(20)")]
    public decimal? Apr { get; set; }

    [Column("MEI", TypeName = "NUMBER(20)")]
    public decimal? Mei { get; set; }

    [Column("JUN", TypeName = "NUMBER(20)")]
    public decimal? Jun { get; set; }

    [Column("JUL", TypeName = "NUMBER(20)")]
    public decimal? Jul { get; set; }

    [Column("AGT", TypeName = "NUMBER(20)")]
    public decimal? Agt { get; set; }

    [Column("SEP", TypeName = "NUMBER(20)")]
    public decimal? Sep { get; set; }

    [Column("OKT", TypeName = "NUMBER(20)")]
    public decimal? Okt { get; set; }

    [Column("NOV", TypeName = "NUMBER(20)")]
    public decimal? Nov { get; set; }

    [Column("DES", TypeName = "NUMBER(20)")]
    public decimal? Des { get; set; }
}
