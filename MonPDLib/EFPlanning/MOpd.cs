using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[Table("M_OPD")]
public partial class MOpd
{
    [Key]
    [Column("ID_OPD")]
    [Precision(10)]
    public int IdOpd { get; set; }

    [Column("NAMA_OPD")]
    [StringLength(250)]
    [Unicode(false)]
    public string NamaOpd { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [InverseProperty("IdOpdNavigation")]
    public virtual ICollection<MUserLogin> MUserLogins { get; set; } = new List<MUserLogin>();

    [InverseProperty("IdOpdNavigation")]
    public virtual ICollection<TTransaksiTemp> TTransaksiTemps { get; set; } = new List<TTransaksiTemp>();

    [InverseProperty("IdOpdNavigation")]
    public virtual ICollection<TTransaksi> TTransaksis { get; set; } = new List<TTransaksi>();
}
