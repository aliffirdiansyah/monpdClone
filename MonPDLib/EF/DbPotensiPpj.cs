using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DB_POTENSI_PPJ")]
public partial class DbPotensiPpj
{
    [Column("NOP")]
    [StringLength(20)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("NJTL_BULAN_MINUS_1", TypeName = "NUMBER(15,2)")]
    public decimal? NjtlBulanMinus1 { get; set; }

    [Column("NJTL_BULAN_MINUS_2", TypeName = "NUMBER(15,2)")]
    public decimal? NjtlBulanMinus2 { get; set; }

    [Column("NJTL_BULAN_MINUS_3", TypeName = "NUMBER(15,2)")]
    public decimal? NjtlBulanMinus3 { get; set; }

    [Column("NJTL_BULAN_MINUS_4", TypeName = "NUMBER(15,2)")]
    public decimal? NjtlBulanMinus4 { get; set; }

    [Column("NJTL_BULAN_MINUS_5", TypeName = "NUMBER(15,2)")]
    public decimal? NjtlBulanMinus5 { get; set; }

    [Column("NJTL_BULAN_MINUS_6", TypeName = "NUMBER(15,2)")]
    public decimal? NjtlBulanMinus6 { get; set; }

    [Column("RATA_RATA_NJTL_6_BULAN", TypeName = "NUMBER(15,2)")]
    public decimal? RataRataNjtl6Bulan { get; set; }

    [Column("JUMLAH_PAJAK", TypeName = "NUMBER(15,2)")]
    public decimal? JumlahPajak { get; set; }

    [Column("STATUS")]
    [Precision(1)]
    public bool? Status { get; set; }

    [Column("TAHUN_BUKU")]
    [Precision(4)]
    public byte? TahunBuku { get; set; }

    [Column("HIT_1BULAN", TypeName = "NUMBER(15,2)")]
    public decimal? Hit1bulan { get; set; }
}
