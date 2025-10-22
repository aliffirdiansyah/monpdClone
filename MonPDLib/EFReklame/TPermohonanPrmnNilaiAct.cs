using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel", "Seq", "ActId", "WfId", "ActSeq")]
[Table("T_PERMOHONAN_PRMN_NILAI_ACT")]
public partial class TPermohonanPrmnNilaiAct
{
    [Key]
    [Column("TAHUN_PEL")]
    [Precision(10)]
    public int TahunPel { get; set; }

    [Key]
    [Column("BULAN_PEL")]
    [Precision(10)]
    public int BulanPel { get; set; }

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
    [Precision(10)]
    public int Status { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("ActId")]
    [InverseProperty("TPermohonanPrmnNilaiActs")]
    public virtual MWfActivity Act { get; set; } = null!;

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq")]
    [InverseProperty("TPermohonanPrmnNilaiActs")]
    public virtual TPermohonanPrmnNilai TPermohonanPrmnNilai { get; set; } = null!;

    [InverseProperty("TPermohonanPrmnNilaiAct")]
    public virtual ICollection<TPermohonanPrmnNilaiHist> TPermohonanPrmnNilaiHists { get; set; } = new List<TPermohonanPrmnNilaiHist>();

    [ForeignKey("WfId")]
    [InverseProperty("TPermohonanPrmnNilaiActs")]
    public virtual MWfWorkflow Wf { get; set; } = null!;
}
