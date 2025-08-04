using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_MON_REKLAME_INS_JUMLAH")]
public partial class DbMonReklameInsJumlah
{
    [Key]
    [Column("NO_FORMULIR")]
    [StringLength(100)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Column("JUMLAH")]
    [Precision(10)]
    public int Jumlah { get; set; }
}
