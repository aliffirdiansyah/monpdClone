using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Table("M_USER_LOGIN")]
public partial class MUserLogin
{
    [Key]
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

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("MUserLogins")]
    public virtual MUserRole? Role { get; set; }

    [ForeignKey("Username")]
    [InverseProperty("MUserLogin")]
    public virtual MPegawai UsernameNavigation { get; set; } = null!;
}
