using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("P_TB")]
public partial class PTb
{
    [Key]
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("TIPE")]
    [Precision(10)]
    public int? Tipe { get; set; }

    [Column("AKTIVE")]
    [Precision(10)]
    public int? Aktive { get; set; }

    [Column("KET")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Ket { get; set; }
}
