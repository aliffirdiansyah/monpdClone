using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("T_OP_PARKIR_CCTV_REALTIME_DOK")]
public partial class TOpParkirCctvRealtimeDok
{
    [Key]
    [Column("ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [Column("IMAGE_DATA", TypeName = "BLOB")]
    public byte[] ImageData { get; set; } = null!;

    [ForeignKey("Id")]
    [InverseProperty("TOpParkirCctvRealtimeDok")]
    public virtual TOpParkirCctvRealtime IdNavigation { get; set; } = null!;
}
