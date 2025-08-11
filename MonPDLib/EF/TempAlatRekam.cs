using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_ALAT_REKAM")]
public partial class TempAlatRekam
{
    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("TGL_TERPASANG", TypeName = "DATE")]
    public DateTime? TglTerpasang { get; set; }

    [Column("KET")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Ket { get; set; }
}
