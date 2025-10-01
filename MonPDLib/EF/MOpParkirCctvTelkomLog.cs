using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "CctvId")]
[Table("M_OP_PARKIR_CCTV_TELKOM_LOG")]
public partial class MOpParkirCctvTelkomLog
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

    [Column("TGL_TERAKHIR_AKTIF", TypeName = "DATE")]
    public DateTime TglTerakhirAktif { get; set; }

    [Column("TGL_TERAKHIR_DOWN", TypeName = "DATE")]
    public DateTime? TglTerakhirDown { get; set; }

    [Column("STATUS")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey("Nop, CctvId")]
    [InverseProperty("MOpParkirCctvTelkomLog")]
    public virtual MOpParkirCctvTelkom MOpParkirCctvTelkom { get; set; } = null!;
}
