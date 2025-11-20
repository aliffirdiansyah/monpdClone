using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbCtrlByrResto
{
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("NAMA_KATEGORI")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaKategori { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }

    [Column("NAMA_OP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(255)]
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
