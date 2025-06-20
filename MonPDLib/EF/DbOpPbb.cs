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
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string KategoriNama { get; set; } = null!;

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

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal Status { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("NAMA_AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string NamaAkun { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(10)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("NAMA_JENIS")]
    [StringLength(10)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Column("OBJEK")]
    [StringLength(10)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("NAMA_OBJEK")]
    [StringLength(10)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Column("RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("NAMA_RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string NamaRincian { get; set; } = null!;

    [Column("SUB_RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string NamaSubRincian { get; set; } = null!;
}
