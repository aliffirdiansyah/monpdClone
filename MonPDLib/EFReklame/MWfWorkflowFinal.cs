using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("WorkflowId", "Nip", "TglMulai")]
[Table("M_WF_WORKFLOW_FINAL")]
public partial class MWfWorkflowFinal
{
    [Key]
    [Column("WORKFLOW_ID")]
    [Precision(10)]
    public int WorkflowId { get; set; }

    [Key]
    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Key]
    [Column("TGL_MULAI", TypeName = "DATE")]
    public DateTime TglMulai { get; set; }

    [Column("TGL_SELESAI", TypeName = "DATE")]
    public DateTime TglSelesai { get; set; }

    [Column("IS_PLT")]
    [Precision(10)]
    public int IsPlt { get; set; }

    [Column("STATUS_BLOCK")]
    [Precision(10)]
    public int StatusBlock { get; set; }

    [Column("TERMINATE")]
    [Precision(10)]
    public int Terminate { get; set; }

    [Column("TGL_TERMINATE", TypeName = "DATE")]
    public DateTime? TglTerminate { get; set; }

    [Column("MODE_KEPUTUSAN", TypeName = "NUMBER")]
    public decimal ModeKeputusan { get; set; }

    [ForeignKey("WorkflowId")]
    [InverseProperty("MWfWorkflowFinals")]
    public virtual MWfWorkflow Workflow { get; set; } = null!;
}
