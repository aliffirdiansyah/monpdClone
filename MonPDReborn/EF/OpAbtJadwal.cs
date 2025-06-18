using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[PrimaryKey("Nop", "IdHari")]
[Table("OP_ABT_JADWAL")]
public partial class OpAbtJadwal
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("ID_HARI")]
    [Precision(10)]
    public int IdHari { get; set; }

    [Column("JAM_BUKA", TypeName = "DATE")]
    public DateTime JamBuka { get; set; }

    [Column("JAM_TUTUP", TypeName = "DATE")]
    public DateTime JamTutup { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpAbtJadwals")]
    public virtual OpAbt NopNavigation { get; set; } = null!;
}
