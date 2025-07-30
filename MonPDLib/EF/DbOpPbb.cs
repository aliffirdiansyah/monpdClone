using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_OP_PBB")]
public partial class DbOpPbb
{
    [Key]
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string KategoriNama { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(100)]
    [Unicode(false)]
    public string AlamatOp { get; set; } = null!;

    [Column("ALAMAT_OP_NO")]
    [StringLength(100)]
    [Unicode(false)]
    public string AlamatOpNo { get; set; } = null!;

    [Column("ALAMAT_OP_RT")]
    [StringLength(100)]
    [Unicode(false)]
    public string AlamatOpRt { get; set; } = null!;

    [Column("ALAMAT_OP_RW")]
    [StringLength(100)]
    [Unicode(false)]
    public string AlamatOpRw { get; set; } = null!;

    [Column("ALAMAT_KD_CAMAT")]
    [StringLength(100)]
    [Unicode(false)]
    public string AlamatKdCamat { get; set; } = null!;

    [Column("ALAMAT_KD_LURAH")]
    [StringLength(100)]
    [Unicode(false)]
    public string AlamatKdLurah { get; set; } = null!;

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("LUAS_TANAH", TypeName = "NUMBER")]
    public decimal LuasTanah { get; set; }

    [Column("ALAMAT_WP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? AlamatWp { get; set; }

    [Column("ALAMAT_WP_NO")]
    [StringLength(30)]
    [Unicode(false)]
    public string? AlamatWpNo { get; set; }

    [Column("ALAMAT_WP_KEL")]
    [StringLength(50)]
    [Unicode(false)]
    public string? AlamatWpKel { get; set; }

    [Column("ALAMAT_WP_KOTA")]
    [StringLength(50)]
    [Unicode(false)]
    public string? AlamatWpKota { get; set; }

    [Column("WP_KTP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? WpKtp { get; set; }

    [Column("WP_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string WpNama { get; set; } = null!;

    [Column("WP_NPWP")]
    [StringLength(35)]
    [Unicode(false)]
    public string? WpNpwp { get; set; }

    [Column("WP_RT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? WpRt { get; set; }

    [Column("WP_RW")]
    [StringLength(100)]
    [Unicode(false)]
    public string? WpRw { get; set; }

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal Status { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
