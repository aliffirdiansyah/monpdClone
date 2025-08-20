using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DataPbb
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal? Uptb { get; set; }

    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }

    [Column("KETETAPAN", TypeName = "NUMBER(38)")]
    public decimal? Ketetapan { get; set; }

    [Column("REALISASI", TypeName = "NUMBER(38)")]
    public decimal? Realisasi { get; set; }

    [Column("ALAMAT_LENGKAP")]
    [StringLength(205)]
    [Unicode(false)]
    public string? AlamatLengkap { get; set; }
}
