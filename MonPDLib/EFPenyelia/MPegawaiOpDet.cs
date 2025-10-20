using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[PrimaryKey("Nip", "Nop", "TglAssign")]
[Table("M_PEGAWAI_OP_DET")]
public partial class MPegawaiOpDet
{
    [Key]
    [Column("NIP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TGL_ASSIGN", TypeName = "DATE")]
    public DateTime TglAssign { get; set; }

    [Column("TGL_END", TypeName = "DATE")]
    public DateTime? TglEnd { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(30)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("Nip")]
    [InverseProperty("MPegawaiOpDets")]
    public virtual MPegawai NipNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("MPegawaiOpDets")]
    public virtual MObjekPajak NopNavigation { get; set; } = null!;
}
