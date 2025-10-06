using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "CctvId")]
[Table("M_OP_PARKIR_CCTV_JASNITA")]
public partial class MOpParkirCctvJasnitum
{
    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("CCTV_MODE")]
    [Precision(3)]
    public byte CctvMode { get; set; }

    [Column("TGL_PASANG", TypeName = "DATE")]
    public DateTime TglPasang { get; set; }

    [Column("ACCESS_POINT")]
    [StringLength(250)]
    [Unicode(false)]
    public string AccessPoint { get; set; } = null!;

    [Column("DISPLAY_NAME")]
    [StringLength(250)]
    [Unicode(false)]
    public string DisplayName { get; set; } = null!;

    [Column("DISPLAY_ID")]
    [StringLength(250)]
    [Unicode(false)]
    public string DisplayId { get; set; } = null!;

    [Column("IP_ADDRESS")]
    [StringLength(250)]
    [Unicode(false)]
    public string IpAddress { get; set; } = null!;

    [Key]
    [Column("CCTV_ID")]
    [StringLength(150)]
    [Unicode(false)]
    public string CctvId { get; set; } = null!;

    [Column("RTSP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Rtsp { get; set; }

    [InverseProperty("MOpParkirCctvJasnitum")]
    public virtual MOpParkirCctvJasnitaLog? MOpParkirCctvJasnitaLog { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("MOpParkirCctvJasnita")]
    public virtual MOpParkirCctv NopNavigation { get; set; } = null!;
}
