using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_PEGAWAI")]
public partial class MPegawai
{
    [Key]
    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [InverseProperty("NipNavigation")]
    public virtual ICollection<DbMonPjOp> DbMonPjOps { get; set; } = new List<DbMonPjOp>();
}
