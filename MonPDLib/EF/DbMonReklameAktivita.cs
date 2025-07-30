using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbMonReklameAktivita
{
    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("AKTIFITAS")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Aktifitas { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Petugas { get; set; }

    [Column("TERLAKSANA", TypeName = "NUMBER")]
    public decimal? Terlaksana { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("SELISIH", TypeName = "NUMBER")]
    public decimal? Selisih { get; set; }

    [Column("STATUS")]
    [StringLength(14)]
    [Unicode(false)]
    public string? Status { get; set; }
}
