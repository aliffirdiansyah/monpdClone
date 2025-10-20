using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("SpptProp", "SpptKota", "SpptKec", "SpptKel", "SpptUrutblk", "SpptUrutop", "SpptTanda", "TahunPbb")]
[Table("DATA_TUNG_PBBBACK")]
public partial class DataTungPbbback
{
    [Column("KETDATA")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Ketdata { get; set; }

    [Key]
    [Column("SPPT_PROP")]
    [StringLength(2)]
    [Unicode(false)]
    public string SpptProp { get; set; } = null!;

    [Key]
    [Column("SPPT_KOTA")]
    [StringLength(2)]
    [Unicode(false)]
    public string SpptKota { get; set; } = null!;

    [Key]
    [Column("SPPT_KEC")]
    [StringLength(3)]
    [Unicode(false)]
    public string SpptKec { get; set; } = null!;

    [Key]
    [Column("SPPT_KEL")]
    [StringLength(3)]
    [Unicode(false)]
    public string SpptKel { get; set; } = null!;

    [Key]
    [Column("SPPT_URUTBLK")]
    [StringLength(3)]
    [Unicode(false)]
    public string SpptUrutblk { get; set; } = null!;

    [Key]
    [Column("SPPT_URUTOP")]
    [StringLength(4)]
    [Unicode(false)]
    public string SpptUrutop { get; set; } = null!;

    [Key]
    [Column("SPPT_TANDA")]
    [StringLength(1)]
    [Unicode(false)]
    public string SpptTanda { get; set; } = null!;

    [Key]
    [Column("TAHUN_PBB")]
    [StringLength(4)]
    [Unicode(false)]
    public string TahunPbb { get; set; } = null!;

    [Column("SALDO_AWAL", TypeName = "NUMBER")]
    public decimal? SaldoAwal { get; set; }

    [Column("PEMUTAKHIRANDATA", TypeName = "NUMBER")]
    public decimal? Pemutakhirandata { get; set; }

    [Column("KOMPENSASI", TypeName = "NUMBER")]
    public decimal? Kompensasi { get; set; }

    [Column("PENCAIRANBG", TypeName = "NUMBER")]
    public decimal? Pencairanbg { get; set; }

    [Column("PEMBATALANVETERAN", TypeName = "NUMBER")]
    public decimal? Pembatalanveteran { get; set; }

    [Column("PENGURANGANPOKOK", TypeName = "NUMBER")]
    public decimal? Penguranganpokok { get; set; }

    [Column("KONFIRMASI", TypeName = "NUMBER")]
    public decimal? Konfirmasi { get; set; }

    [Column("PEMBATALAN", TypeName = "NUMBER")]
    public decimal? Pembatalan { get; set; }

    [Column("KOREKSITAMBAH", TypeName = "NUMBER")]
    public decimal? Koreksitambah { get; set; }

    [Column("BAYAR", TypeName = "NUMBER")]
    public decimal? Bayar { get; set; }

    [Column("SISA_TUNGGAKAN", TypeName = "NUMBER")]
    public decimal? SisaTunggakan { get; set; }

    [Column("LEBIHBAYAR", TypeName = "NUMBER")]
    public decimal? Lebihbayar { get; set; }

    [Column("TGL_INSERT", TypeName = "DATE")]
    public DateTime? TglInsert { get; set; }

    [Column("ISHAPUSBUKU")]
    [StringLength(4)]
    [Unicode(false)]
    public string? Ishapusbuku { get; set; }

    [Column("TAHUN_BUKU")]
    [StringLength(4)]
    [Unicode(false)]
    public string? TahunBuku { get; set; }
}
