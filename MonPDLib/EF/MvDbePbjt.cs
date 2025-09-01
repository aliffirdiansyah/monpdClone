using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbePbjt
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal Bulan { get; set; }

    [Column("PAJAK_ID")]
    [Precision(10)]
    public int PajakId { get; set; }

    [Column("NAMA")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("TGL_MULAI_BUKA_OP", TypeName = "DATE")]
    public DateTime? TglMulaiBukaOp { get; set; }

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("TAHUN_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? TahunPajakKetetapan { get; set; }

    [Column("MASA_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? MasaPajakKetetapan { get; set; }

    [Column("BULAN_BAYAR", TypeName = "NUMBER")]
    public decimal? BulanBayar { get; set; }

    [Column("TAHUN_BAYAR", TypeName = "NUMBER")]
    public decimal? TahunBayar { get; set; }

    [Column("NOMINAL_POKOK_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalPokokBayar { get; set; }

    [Column("TGL_REALISASI", TypeName = "DATE")]
    public DateTime? TglRealisasi { get; set; }
}
