using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_TINDAKAN_REKLAME")]
public partial class MTindakanReklame
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("ID_UPAYA", TypeName = "NUMBER")]
    public decimal? IdUpaya { get; set; }

    [Column("TINDAKAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Tindakan { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [ForeignKey("IdUpaya")]
    [InverseProperty("MTindakanReklames")]
    public virtual MUpayaReklame? IdUpayaNavigation { get; set; }
}
