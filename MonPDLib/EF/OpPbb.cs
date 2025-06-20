using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_PBB")]
public partial class OpPbb
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal Kategori { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("ALAMAT_OP_NO")]
    [StringLength(30)]
    [Unicode(false)]
    public string? AlamatOpNo { get; set; }

    [Column("ALAMAT_OP_RT")]
    [StringLength(4)]
    [Unicode(false)]
    public string? AlamatOpRt { get; set; }

    [Column("ALAMAT_OP_RW")]
    [StringLength(4)]
    [Unicode(false)]
    public string? AlamatOpRw { get; set; }

    [Column("ALAMAT_KD_CAMAT")]
    [StringLength(3)]
    [Unicode(false)]
    public string? AlamatKdCamat { get; set; }

    [Column("ALAMAT_KD_LURAH")]
    [StringLength(3)]
    [Unicode(false)]
    public string? AlamatKdLurah { get; set; }

    [Column("LUAS_TANAH")]
    [Precision(10)]
    public int? LuasTanah { get; set; }

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
    public string? WpNama { get; set; }

    [Column("WP_NPWP")]
    [StringLength(35)]
    [Unicode(false)]
    public string? WpNpwp { get; set; }

    [Column("WP_RT")]
    [StringLength(4)]
    [Unicode(false)]
    public string? WpRt { get; set; }

    [Column("WP_RW")]
    [StringLength(4)]
    [Unicode(false)]
    public string? WpRw { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal? Status { get; set; }

    [ForeignKey("AlamatKdCamat, AlamatKdLurah")]
    [InverseProperty("OpPbbs")]
    public virtual MWilayah? AlamatKd { get; set; }

    [ForeignKey("Kategori")]
    [InverseProperty("OpPbbs")]
    public virtual MKategoriPajak KategoriNavigation { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpPbbKetetapan> OpPbbKetetapans { get; set; } = new List<OpPbbKetetapan>();
}
