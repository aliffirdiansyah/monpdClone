using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DataPpj
{
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("NJTL_BULAN_MINUS_1", TypeName = "NUMBER")]
    public decimal? NjtlBulanMinus1 { get; set; }

    [Column("NJTL_BULAN_MINUS_2", TypeName = "NUMBER")]
    public decimal? NjtlBulanMinus2 { get; set; }

    [Column("NJTL_BULAN_MINUS_3", TypeName = "NUMBER")]
    public decimal? NjtlBulanMinus3 { get; set; }

    [Column("NJTL_BULAN_MINUS_4", TypeName = "NUMBER")]
    public decimal? NjtlBulanMinus4 { get; set; }

    [Column("NJTL_BULAN_MINUS_5", TypeName = "NUMBER")]
    public decimal? NjtlBulanMinus5 { get; set; }

    [Column("NJTL_BULAN_MINUS_6", TypeName = "NUMBER")]
    public decimal? NjtlBulanMinus6 { get; set; }

    [Column("RATA_RATA_NJTL_6_BULAN", TypeName = "NUMBER")]
    public decimal? RataRataNjtl6Bulan { get; set; }

    [Column("JUMLAH_PAJAK", TypeName = "NUMBER")]
    public decimal? JumlahPajak { get; set; }

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal? Status { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }
}
