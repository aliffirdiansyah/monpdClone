using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwDataOpParkir
{
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("LAST_BUKA", TypeName = "DATE")]
    public DateTime? LastBuka { get; set; }

    [Column("LAST_TUTUP", TypeName = "DATE")]
    public DateTime? LastTutup { get; set; }

    [Column("NAMA_OP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("NAMA_KATEGORI")]
    [StringLength(38)]
    [Unicode(false)]
    public string? NamaKategori { get; set; }

    [Column("WILAYAH")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Wilayah { get; set; }

    [Column("JUMLAH", TypeName = "NUMBER")]
    public decimal? Jumlah { get; set; }
}
