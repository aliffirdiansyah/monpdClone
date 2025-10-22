using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel", "Seq", "ActId", "WfId", "ActSeq")]
[Table("T_PERMOHONAN_INS_NILAI_ACT")]
public partial class TPermohonanInsNilaiAct
{
    [Key]
    [Column("TAHUN_PEL")]
    [Precision(4)]
    public byte TahunPel { get; set; }

    [Key]
    [Column("BULAN_PEL")]
    [Precision(2)]
    public byte BulanPel { get; set; }

    [Key]
    [Column("SEQ_PEL")]
    [Precision(10)]
    public int SeqPel { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Key]
    [Column("ACT_ID")]
    [Precision(10)]
    public int ActId { get; set; }

    [Key]
    [Column("WF_ID")]
    [Precision(10)]
    public int WfId { get; set; }

    [Key]
    [Column("ACT_SEQ")]
    [Precision(10)]
    public int ActSeq { get; set; }

    [Column("STATUS")]
    [Precision(2)]
    public byte Status { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("ActId")]
    [InverseProperty("TPermohonanInsNilaiActs")]
    public virtual MWfActivity Act { get; set; } = null!;

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq")]
    [InverseProperty("TPermohonanInsNilaiActs")]
    public virtual TPermohonanInsNilai TPermohonanInsNilai { get; set; } = null!;

    [ForeignKey("WfId")]
    [InverseProperty("TPermohonanInsNilaiActs")]
    public virtual MWfWorkflow Wf { get; set; } = null!;
}
