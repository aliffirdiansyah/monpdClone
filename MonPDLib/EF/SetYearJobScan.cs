using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("SET_YEAR_JOB_SCAN")]
public partial class SetYearJobScan
{
    [Key]
    [Column("ID_PAJAK")]
    [Precision(10)]
    public int IdPajak { get; set; }

    [Column("YEAR_BEFORE", TypeName = "NUMBER")]
    public decimal YearBefore { get; set; }

    [ForeignKey("IdPajak")]
    [InverseProperty("SetYearJobScan")]
    public virtual MPajak IdPajakNavigation { get; set; } = null!;
}
