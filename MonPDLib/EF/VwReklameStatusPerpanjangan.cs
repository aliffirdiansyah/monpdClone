using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwReklameStatusPerpanjangan
{
    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("FORM")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Form { get; set; }

    [Column("STATUS_PERPANJANGAN", TypeName = "NUMBER")]
    public decimal? StatusPerpanjangan { get; set; }
}
