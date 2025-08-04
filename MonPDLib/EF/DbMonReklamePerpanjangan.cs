using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_MON_REKLAME_PERPANJANGAN")]
public partial class DbMonReklamePerpanjangan
{
    [Key]
    [Column("NO_FORMULIR")]
    [StringLength(100)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Column("IS_PERPANJANGAN")]
    [Precision(1)]
    public bool IsPerpanjangan { get; set; }
}
