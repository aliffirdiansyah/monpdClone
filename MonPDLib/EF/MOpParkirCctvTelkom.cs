using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "CctvId")]
[Table("M_OP_PARKIR_CCTV_TELKOM")]
public partial class MOpParkirCctvTelkom
{
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

    [Column("CCTV_MODE")]
    [Precision(3)]
    public byte CctvMode { get; set; }

    [Column("TGL_PASANG", TypeName = "DATE")]
    public DateTime TglPasang { get; set; }

    [InverseProperty("MOpParkirCctvTelkom")]
    public virtual MOpParkirCctvTelkomLog? MOpParkirCctvTelkomLog { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("MOpParkirCctvTelkoms")]
    public virtual MOpParkirCctv NopNavigation { get; set; } = null!;
}
