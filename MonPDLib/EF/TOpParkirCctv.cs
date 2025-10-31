using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Id", "Nop", "CctvId")]
[Table("T_OP_PARKIR_CCTV")]
public partial class TOpParkirCctv
{
    [Key]
    [Column("ID")]
    [StringLength(250)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("CCTV_ID")]
    [StringLength(150)]
    [Unicode(false)]
    public string CctvId { get; set; } = null!;

    [Column("NAMA_OP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(300)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("WILAYAH_PAJAK")]
    [Precision(10)]
    public int? WilayahPajak { get; set; }

    [Column("WAKTU_MASUK", TypeName = "DATE")]
    public DateTime WaktuMasuk { get; set; }

    [Column("JENIS_KEND")]
    [Precision(10)]
    public int JenisKend { get; set; }

    [Column("PLAT_NO")]
    [StringLength(120)]
    [Unicode(false)]
    public string? PlatNo { get; set; }

    [Column("WAKTU_KELUAR", TypeName = "DATE")]
    public DateTime? WaktuKeluar { get; set; }

    [Column("DIRECTION")]
    [Precision(10)]
    public int Direction { get; set; }

    [Column("LOG")]
    [StringLength(300)]
    [Unicode(false)]
    public string? Log { get; set; }

    [Column("IMAGE_URL")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [Column("VENDOR", TypeName = "NUMBER")]
    public decimal Vendor { get; set; }

    [InverseProperty("TOpParkirCctv")]
    public virtual TOpParkirCctvDok? TOpParkirCctvDok { get; set; }
}
