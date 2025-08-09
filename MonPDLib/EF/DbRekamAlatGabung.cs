using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbRekamAlatGabung
{
    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string KategoriNama { get; set; } = null!;

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaOp { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string AlamatOp { get; set; } = null!;

    [Column("IS_TS", TypeName = "NUMBER")]
    public decimal? IsTs { get; set; }

    [Column("IS_TB", TypeName = "NUMBER")]
    public decimal? IsTb { get; set; }

    [Column("IS_SB", TypeName = "NUMBER")]
    public decimal? IsSb { get; set; }

    [Column("TGL_TERPASANG", TypeName = "DATE")]
    public DateTime? TglTerpasang { get; set; }
}
