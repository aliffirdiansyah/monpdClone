using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbCtrlByrReklame
{
    [Column("NO_FORMULIR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("NAMA_KATEGORI")]
    [StringLength(10)]
    [Unicode(false)]
    public string? NamaKategori { get; set; }

    [Column("JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(5)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }

    [Column("NAMA_WP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaWp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(181)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal? Realisasi { get; set; }

    [Column("KETETAPAN", TypeName = "NUMBER")]
    public decimal? Ketetapan { get; set; }

    [Column("KETERANGAN")]
    [StringLength(21)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("STATUS_BAYAR", TypeName = "NUMBER")]
    public decimal? StatusBayar { get; set; }
}
