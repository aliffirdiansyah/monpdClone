using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DATAPBB_SATUPETA")]
public partial class DatapbbSatupetum
{
    [Column("ID_PERSIL")]
    [StringLength(20)]
    [Unicode(false)]
    public string IdPersil { get; set; } = null!;

    [Column("ID_PBB", TypeName = "NUMBER")]
    public decimal IdPbb { get; set; }

    [Column("NOP")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("NAMA_WP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaWp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(120)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("BLOK_KAV_NO")]
    [StringLength(30)]
    [Unicode(false)]
    public string? BlokKavNo { get; set; }

    [Column("RT_OP")]
    [StringLength(3)]
    [Unicode(false)]
    public string? RtOp { get; set; }

    [Column("RW_OP")]
    [StringLength(2)]
    [Unicode(false)]
    public string? RwOp { get; set; }

    [Column("KECAMATAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("KELURAHAN")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Kelurahan { get; set; }

    [Column("LUAS_BUMI", TypeName = "NUMBER")]
    public decimal? LuasBumi { get; set; }

    [Column("LUAS_BANGUNAN", TypeName = "NUMBER")]
    public decimal? LuasBangunan { get; set; }

    [Column("NJOP_BUMI", TypeName = "NUMBER")]
    public decimal? NjopBumi { get; set; }

    [Column("NJOP_BANGUNAN", TypeName = "NUMBER")]
    public decimal? NjopBangunan { get; set; }

    [Column("NO_IMB")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoImb { get; set; }

    [Column("ID_WP")]
    [StringLength(40)]
    [Unicode(false)]
    public string? IdWp { get; set; }

    [Column("STATUS_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? StatusBayar { get; set; }

    [Column("PERUNTUKAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Peruntukan { get; set; }
}
