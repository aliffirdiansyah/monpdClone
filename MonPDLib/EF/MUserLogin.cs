using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("M_USER_LOGIN")]
public partial class MUserLogin
{
    [Column("USERNAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("PASSWORD")]
    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("ROLE_ID", TypeName = "NUMBER")]
    public decimal? RoleId { get; set; }

    [Column("ROLE_NAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    [Column("INSERT_DATE", TypeName = "DATE")]
    public DateTime? InsertDate { get; set; }

    [Column("INSERT_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsertBy { get; set; } = null!;

    [Column("STATUS")]
    [Precision(1)]
    public bool? Status { get; set; }
}
