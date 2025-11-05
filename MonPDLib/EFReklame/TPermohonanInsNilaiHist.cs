using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel", "Seq", "ActId", "WfId", "ActSeq", "SeqHistory")]
[Table("T_PERMOHONAN_INS_NILAI_HIST")]
public partial class TPermohonanInsNilaiHist
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

    [Key]
    [Column("SEQ_HISTORY")]
    [Precision(10)]
    public int SeqHistory { get; set; }

    [Column("JENIS_PROSES")]
    [Precision(10)]
    public int? JenisProses { get; set; }

    [Column("KET")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("PROSES_DATE", TypeName = "DATE")]
    public DateTime? ProsesDate { get; set; }

    [Column("PROSES_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ProsesBy { get; set; }

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq, ActId, WfId, ActSeq")]
    [InverseProperty("TPermohonanInsNilaiHists")]
    public virtual TPermohonanInsNilaiAct TPermohonanInsNilaiAct { get; set; } = null!;
}
