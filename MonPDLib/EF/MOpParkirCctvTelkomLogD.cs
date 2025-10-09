using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_OP_PARKIR_CCTV_TELKOM_LOG_D")]
public partial class MOpParkirCctvTelkomLogD
{
    [Key]
    [Column("GUID")]
    [StringLength(250)]
    [Unicode(false)]
    public string Guid { get; set; } = null!;

    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("CCTV_ID")]
    [StringLength(150)]
    [Unicode(false)]
    public string CctvId { get; set; } = null!;

    [Column("TGL_EVENT", TypeName = "DATE")]
    public DateTime TglEvent { get; set; }

    [Column("EVENT")]
    [StringLength(250)]
    [Unicode(false)]
    public string Event { get; set; } = null!;

    [Column("IS_ON", TypeName = "NUMBER")]
    public decimal IsOn { get; set; }
}
