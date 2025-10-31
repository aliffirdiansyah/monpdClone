using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_UPTB_PERAN")]
public partial class MUptbPeran
{
    [Key]
    [Column("ID", TypeName = "NUMBER(38)")]
    public decimal Id { get; set; }

    [Column("UPTB", TypeName = "NUMBER(38)")]
    public decimal Uptb { get; set; }

    [Column("PERAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string Peran { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("Uptb")]
    [InverseProperty("MUptbPerans")]
    public virtual MUptb UptbNavigation { get; set; } = null!;
}
