using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("SET_LAST_RUN")]
public partial class SetLastRun
{
    [Key]
    [Column("JOB")]
    [StringLength(30)]
    [Unicode(false)]
    public string Job { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }
}
