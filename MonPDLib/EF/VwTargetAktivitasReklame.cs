using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwTargetAktivitasReklame
{
    [Column("AKTIFITAS")]
    [StringLength(13)]
    [Unicode(false)]
    public string? Aktifitas { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }
}
