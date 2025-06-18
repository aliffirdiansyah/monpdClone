using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[PrimaryKey("Nop", "IdHari")]
[Table("OP_LISTRIK_JADWAL")]
public partial class OpListrikJadwal
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

    [Column("JAM_MULAI", TypeName = "DATE")]
    public DateTime JamMulai { get; set; }

    [Column("JAM_SELESAI", TypeName = "DATE")]
    public DateTime JamSelesai { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpListrikJadwals")]
    public virtual OpListrik NopNavigation { get; set; } = null!;
}
