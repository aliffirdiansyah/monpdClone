using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("WorkflowId", "ActivityId")]
[Table("M_WF_WORKFLOW_ACTIVITY")]
public partial class MWfWorkflowActivity
{
    [Key]
    [Column("WORKFLOW_ID")]
    [Precision(10)]
    public int WorkflowId { get; set; }

    [Key]
    [Column("ACTIVITY_ID")]
    [Precision(10)]
    public int ActivityId { get; set; }

    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("MWfWorkflowActivities")]
    public virtual MWfActivity Activity { get; set; } = null!;

    [ForeignKey("WorkflowId")]
    [InverseProperty("MWfWorkflowActivities")]
    public virtual MWfWorkflow Workflow { get; set; } = null!;
}
