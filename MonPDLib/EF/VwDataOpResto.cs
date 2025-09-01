using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwDataOpResto
{
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("LAST_BUKA", TypeName = "DATE")]
    public DateTime? LastBuka { get; set; }

    [Column("LAST_TUTUP", TypeName = "DATE")]
    public DateTime? LastTutup { get; set; }

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("NAMA_KATEGORI")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaKategori { get; set; }

    [Column("PAJAK_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PajakNama { get; set; }

    [Column("WILAYAH")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Wilayah { get; set; }

    [Column("JUMLAH", TypeName = "NUMBER")]
    public decimal? Jumlah { get; set; }
}
