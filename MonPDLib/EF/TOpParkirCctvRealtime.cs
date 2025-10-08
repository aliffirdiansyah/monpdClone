using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("T_OP_PARKIR_CCTV_REALTIME")]
public partial class TOpParkirCctvRealtime
{
    [Key]
    [Column("ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("CCTV_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string CctvId { get; set; } = null!;

    [Column("VENDOR_ID")]
    [Precision(10)]
    public int VendorId { get; set; }

    [Column("JENIS_KEND")]
    [Precision(10)]
    public int JenisKend { get; set; }

    [Column("PLAT_NO")]
    [StringLength(20)]
    [Unicode(false)]
    public string? PlatNo { get; set; }

    [Column("WAKTU_MASUK", TypeName = "DATE")]
    public DateTime WaktuMasuk { get; set; }

    [Column("IMAGE_URL")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }
}
