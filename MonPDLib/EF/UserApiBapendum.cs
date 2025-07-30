using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("USER_API_BAPENDA")]
public partial class UserApiBapendum
{
    [Column("USERNAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("PASS")]
    [StringLength(150)]
    [Unicode(false)]
    public string Pass { get; set; } = null!;

    [Column("KODE_OPD")]
    [StringLength(30)]
    [Unicode(false)]
    public string KodeOpd { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
