using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_UPTB_MISI")]
public partial class MUptbMisi
{
    [Key]
    [Column("ID", TypeName = "NUMBER(38)")]
    public decimal Id { get; set; }

    [Column("UPTB", TypeName = "NUMBER(38)")]
    public decimal Uptb { get; set; }

    [Column("MISI")]
    [StringLength(500)]
    [Unicode(false)]
    public string Misi { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("Uptb")]
    [InverseProperty("MUptbMisis")]
    public virtual MUptb UptbNavigation { get; set; } = null!;
}
