using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class CobaOp
{
    [Column("WILAYAH", TypeName = "NUMBER")]
    public decimal? Wilayah { get; set; }

    [Column("KECAMATAN")]
    [StringLength(6)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("KELURAHAN")]
    [StringLength(14)]
    [Unicode(false)]
    public string? Kelurahan { get; set; }

    [Column("NOP")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("KATAGORI")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Katagori { get; set; }

    [Column("NAMA_WP")]
    [StringLength(12)]
    [Unicode(false)]
    public string? NamaWp { get; set; }

    [Column("ALAMAT_WP")]
    [StringLength(60)]
    [Unicode(false)]
    public string? AlamatWp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(34)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("L_BUMI", TypeName = "NUMBER")]
    public decimal? LBumi { get; set; }

    [Column("L_BANGUNAN", TypeName = "NUMBER")]
    public decimal? LBangunan { get; set; }

    [Column("L_BUMI_BERSAMA", TypeName = "NUMBER")]
    public decimal? LBumiBersama { get; set; }

    [Column("L_BANGUNAN_BERSAMA", TypeName = "NUMBER")]
    public decimal? LBangunanBersama { get; set; }

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal? Status { get; set; }
}
