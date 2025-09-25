using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_OP_PARKIR_CCTV")]
public partial class MOpParkirCctv
{
    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("NAMA_OP")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaOp { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(300)]
    [Unicode(false)]
    public string AlamatOp { get; set; } = null!;

    [Column("WILAYAH_PAJAK")]
    [Precision(10)]
    public int WilayahPajak { get; set; }

    [Column("VENDOR")]
    [Precision(10)]
    public int Vendor { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual ICollection<MOpParkirCctvDet> MOpParkirCctvDets { get; set; } = new List<MOpParkirCctvDet>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<MOpParkirCctvJasnitum> MOpParkirCctvJasnita { get; set; } = new List<MOpParkirCctvJasnitum>();
}
