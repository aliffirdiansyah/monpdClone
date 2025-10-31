using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Id", "Nop", "CctvId")]
[Table("T_OP_PARKIR_CCTV_DOK")]
public partial class TOpParkirCctvDok
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

    [Column("IMAGE_DATA", TypeName = "BLOB")]
    public byte[] ImageData { get; set; } = null!;
}
