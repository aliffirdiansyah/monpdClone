using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwNpwpdEmail
{
    [Column("NPWPD_LAMA")]
    [StringLength(50)]
    [Unicode(false)]
    public string NpwpdLama { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Email { get; set; }
}
