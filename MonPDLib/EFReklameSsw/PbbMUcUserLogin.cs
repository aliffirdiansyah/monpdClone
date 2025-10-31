using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Keyless]
[Table("PBB_M_UC_USER_LOGIN")]
public partial class PbbMUcUserLogin
{
    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Column("PASSWORD")]
    [StringLength(250)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("USER_ROLE")]
    [Precision(10)]
    public int UserRole { get; set; }

    [Column("LAST_ACT", TypeName = "DATE")]
    public DateTime? LastAct { get; set; }

    [Column("RESET_KEY")]
    [StringLength(10)]
    [Unicode(false)]
    public string? ResetKey { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(45)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
