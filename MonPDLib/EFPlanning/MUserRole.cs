using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[Table("M_USER_ROLE")]
public partial class MUserRole
{
    [Key]
    [Column("ROLE_ID", TypeName = "NUMBER")]
    public decimal RoleId { get; set; }

    [Column("ROLE_NAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    [Column("KET")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("STATUS")]
    [Precision(10)]
    public int? Status { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<MUserLogin> MUserLogins { get; set; } = new List<MUserLogin>();
}
