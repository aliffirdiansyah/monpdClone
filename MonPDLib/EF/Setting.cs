using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("SETTING")]
public partial class Setting
{
    [Key]
    [Column("PROPERTI")]
    [StringLength(100)]
    [Unicode(false)]
    public string Properti { get; set; } = null!;

    [Column("VALUE", TypeName = "CLOB")]
    public string? Value { get; set; }
}
