using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "IdHari")]
[Table("OP_RESTO_JADWAL")]
public partial class OpRestoJadwal
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

    [Column("JAM_BUKA")]
    [Precision(6)]
    public DateTime JamBuka { get; set; }

    [Column("JAM_TUTUP")]
    [Precision(6)]
    public DateTime JamTutup { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpRestoJadwals")]
    public virtual OpResto NopNavigation { get; set; } = null!;
}
