using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("M_WF_ACTIVITY")]
public partial class MWfActivity
{
    [Key]
    [Column("ID")]
    [Precision(10)]
    public int Id { get; set; }

    [Column("NAMA")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("SEND_MAIL_STATE")]
    [Precision(1)]
    public bool SendMailState { get; set; }

    [Column("SEND_WA_STATE")]
    [Precision(1)]
    public bool SendWaState { get; set; }

    [Column("KET")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(45)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("ALLOW_REJECT")]
    [Precision(10)]
    public int AllowReject { get; set; }

    [Column("KD_SURVEY", TypeName = "NUMBER")]
    public decimal KdSurvey { get; set; }

    [Column("ALLOW_EDIT", TypeName = "NUMBER")]
    public decimal AllowEdit { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<MWfWorkflowActivity> MWfWorkflowActivities { get; set; } = new List<MWfWorkflowActivity>();

    [InverseProperty("Act")]
    public virtual ICollection<TPermohonanInsNilaiAct> TPermohonanInsNilaiActs { get; set; } = new List<TPermohonanInsNilaiAct>();
}
