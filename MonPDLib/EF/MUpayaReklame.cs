using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_UPAYA_REKLAME")]
public partial class MUpayaReklame
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("UPAYA")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Upaya { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [InverseProperty("IdUpayaNavigation")]
    public virtual ICollection<MTindakanReklame> MTindakanReklames { get; set; } = new List<MTindakanReklame>();
}
