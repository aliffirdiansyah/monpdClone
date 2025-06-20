using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_MON_BPHTB")]
public partial class DbMonBphtb
{
    [Key]
    [Column("IDSSPD")]
    [StringLength(50)]
    [Unicode(false)]
    public string Idsspd { get; set; } = null!;

    [Column("TGL_BAYAR", TypeName = "DATE")]
    public DateTime? TglBayar { get; set; }

    [Column("TGL_DATA", TypeName = "DATE")]
    public DateTime? TglData { get; set; }

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

    [Column("SPPT_NOP")]
    [StringLength(25)]
    [Unicode(false)]
    public string? SpptNop { get; set; }

    [Column("NAMA_WP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaWp { get; set; }

    [Column("ALAMAT")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Alamat { get; set; }

    [Column("MASA", TypeName = "NUMBER")]
    public decimal? Masa { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("POKOK", TypeName = "NUMBER")]
    public decimal? Pokok { get; set; }

    [Column("SANKSI", TypeName = "NUMBER")]
    public decimal? Sanksi { get; set; }

    [Column("NOMORDASARSETOR")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Nomordasarsetor { get; set; }

    [Column("TEMPATBAYAR")]
    [StringLength(15)]
    [Unicode(false)]
    public string? Tempatbayar { get; set; }

    [Column("REFSETORAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Refsetoran { get; set; }

    [Column("REKON_DATE", TypeName = "DATE")]
    public DateTime? RekonDate { get; set; }

    [Column("REKON_BY")]
    [StringLength(3)]
    [Unicode(false)]
    public string? RekonBy { get; set; }

    [Column("KD_PEROLEHAN")]
    [StringLength(5)]
    [Unicode(false)]
    public string? KdPerolehan { get; set; }

    [Column("KD_BYR", TypeName = "NUMBER")]
    public decimal? KdByr { get; set; }

    [Column("KODE_NOTARIS")]
    [StringLength(25)]
    [Unicode(false)]
    public string? KodeNotaris { get; set; }

    [Column("KD_PELAYANAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? KdPelayanan { get; set; }

    [Column("PEROLEHAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Perolehan { get; set; }

    [Column("KD_CAMAT")]
    [StringLength(5)]
    [Unicode(false)]
    public string? KdCamat { get; set; }

    [Column("KD_LURAH")]
    [StringLength(5)]
    [Unicode(false)]
    public string? KdLurah { get; set; }
}
