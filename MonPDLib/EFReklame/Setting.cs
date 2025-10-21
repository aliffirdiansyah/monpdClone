using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("SETTING")]
public partial class Setting
{
    [Key]
    [Column("PROPERTI")]
    [StringLength(150)]
    [Unicode(false)]
    public string Properti { get; set; } = null!;

    [Column("NILAI")]
    [StringLength(200)]
    [Unicode(false)]
    public string Nilai { get; set; } = null!;
}
