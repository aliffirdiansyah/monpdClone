using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("USER_API_VENDOR_CCTV")]
public partial class UserApiVendorCctv
{
    [Key]
    [Column("USERNAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("PASS")]
    [StringLength(150)]
    [Unicode(false)]
    public string Pass { get; set; } = null!;

    [Column("NAME")]
    [StringLength(150)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
