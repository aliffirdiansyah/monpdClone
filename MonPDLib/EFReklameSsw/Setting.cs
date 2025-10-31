using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("SETTING")]
public partial class Setting
{
    [Key]
    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("NILAI")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? Nilai { get; set; }
}
