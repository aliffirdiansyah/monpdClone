using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbePbjtRekap
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("NAMA")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("JML_OP", TypeName = "NUMBER")]
    public decimal? JmlOp { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal? Realisasi { get; set; }

    [Column("PENCAPAIAN", TypeName = "NUMBER")]
    public decimal? Pencapaian { get; set; }
}
