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
    [Precision(10)]
    public int Id { get; set; }

    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("CCTV_ID")]
    [Precision(10)]
    public int CctvId { get; set; }

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

    [Column("WAKTU", TypeName = "DATE")]
    public DateTime Waktu { get; set; }

    [Column("JENIS_KEND")]
    [Precision(2)]
    public byte? JenisKend { get; set; }

    [Column("PLAT_NO")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PlatNo { get; set; }

    [ForeignKey("Nop, CctvId")]
    [InverseProperty("TOpParkirCctvs")]
    public virtual MOpParkirCctvDet MOpParkirCctvDet { get; set; } = null!;
}
