using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbOpProfil
{
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("TGL_OP_BUKA", TypeName = "DATE")]
    public DateTime? TglOpBuka { get; set; }

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal? Kategori { get; set; }

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal? Status { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }
}
