using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("P_SB")]
public partial class PSb
{
    [Key]
    [Column("ID_OP", TypeName = "NUMBER")]
    public decimal IdOp { get; set; }

    [Column("FK_NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? FkNop { get; set; }

    [Column("STATUS")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column("TGL_PASANG", TypeName = "DATE")]
    public DateTime? TglPasang { get; set; }
}
