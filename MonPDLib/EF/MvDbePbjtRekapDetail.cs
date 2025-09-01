using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbePbjtRekapDetail
{
    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal Bulan { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("JML_OP", TypeName = "NUMBER")]
    public decimal? JmlOp { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal? Realisasi { get; set; }

    [Column("PENCAPAIAN", TypeName = "NUMBER")]
    public decimal? Pencapaian { get; set; }
}
