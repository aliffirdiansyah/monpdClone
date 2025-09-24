using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "CctvId")]
[Table("M_OP_PARKIR_CCTV_DET")]
public partial class MOpParkirCctvDet
{
    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("CCTV_ID")]
    [Precision(10)]
    public int CctvId { get; set; }

    [Column("CCTV_MODE")]
    [Precision(3)]
    public byte CctvMode { get; set; }

    [Column("TGL_PASANG", TypeName = "DATE")]
    public DateTime TglPasang { get; set; }

    [InverseProperty("MOpParkirCctvDet")]
    public virtual MOpParkirCctvLog? MOpParkirCctvLog { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("MOpParkirCctvDets")]
    public virtual MOpParkirCctv NopNavigation { get; set; } = null!;

    [InverseProperty("MOpParkirCctvDet")]
    public virtual ICollection<TOpParkirCctv> TOpParkirCctvs { get; set; } = new List<TOpParkirCctv>();
}
