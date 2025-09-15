using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbeMonPbbKecKel
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("PROP")]
    [StringLength(2)]
    [Unicode(false)]
    public string Prop { get; set; } = null!;

    [Column("DATI")]
    [StringLength(2)]
    [Unicode(false)]
    public string Dati { get; set; } = null!;

    [Column("KEC")]
    [StringLength(3)]
    [Unicode(false)]
    public string Kec { get; set; } = null!;

    [Column("KEL")]
    [StringLength(3)]
    [Unicode(false)]
    public string Kel { get; set; } = null!;

    [Column("BLOK")]
    [StringLength(3)]
    [Unicode(false)]
    public string Blok { get; set; } = null!;

    [Column("URUT")]
    [StringLength(4)]
    [Unicode(false)]
    public string Urut { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(1)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

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
    public string? AlamatOpNo { get; set; }

    [Column("ALAMAT_OP_RT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? AlamatOpRt { get; set; }

    [Column("ALAMAT_OP_RW")]
    [StringLength(100)]
    [Unicode(false)]
    public string? AlamatOpRw { get; set; }

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

    [Column("WP_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string WpNama { get; set; } = null!;

    [Column("WP_NPWP")]
    [StringLength(35)]
    [Unicode(false)]
    public string? WpNpwp { get; set; }

    [Column("TAHUN_PAJAK", TypeName = "NUMBER")]
    public decimal TahunPajak { get; set; }

    [Column("KETETAPAN", TypeName = "NUMBER")]
    public decimal Ketetapan { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("JENIS_BUKU", TypeName = "NUMBER")]
    public decimal? JenisBuku { get; set; }

    [Column("TGL_BAYAR", TypeName = "DATE")]
    public DateTime? TglBayar { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal? Realisasi { get; set; }

    [Column("TAHUN_BUKU_BAYAR", TypeName = "NUMBER")]
    public decimal? TahunBukuBayar { get; set; }

    [Column("KECAMATAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("KELURAHAN")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Kelurahan { get; set; }
}
