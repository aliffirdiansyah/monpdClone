using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbCtrlByrPbb
{
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("NAMA_KATEGORI")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaKategori { get; set; }

    [Column("WILAYAH_PAJAK", TypeName = "NUMBER")]
    public decimal WilayahPajak { get; set; }

    [Column("NAMA_WP")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaWp { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(222)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("REALISASI", TypeName = "NUMBER(38)")]
    public decimal? Realisasi { get; set; }

    [Column("KETETAPAN", TypeName = "NUMBER(38)")]
    public decimal? Ketetapan { get; set; }

    [Column("KETERANGAN")]
    [StringLength(21)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("STATUS_BAYAR", TypeName = "NUMBER")]
    public decimal? StatusBayar { get; set; }
}
