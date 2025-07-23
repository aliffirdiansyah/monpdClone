using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_PIUTANG")]
public partial class TempPiutang
{
    [Column("FK_PAJAK_DAERAH")]
    [StringLength(100)]
    [Unicode(false)]
    public string FkPajakDaerah { get; set; } = null!;

    [Column("FK_NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string FkNop { get; set; } = null!;

    [Column("MASA_PAJAK")]
    [StringLength(50)]
    [Unicode(false)]
    public string MasaPajak { get; set; } = null!;

    [Column("TAHUN_PAJAK")]
    [StringLength(50)]
    [Unicode(false)]
    public string TahunPajak { get; set; } = null!;

    [Column("PIUTANG", TypeName = "NUMBER(15,2)")]
    public decimal? Piutang { get; set; }

    [Column("TAHUN_CATAT")]
    [Precision(4)]
    public byte TahunCatat { get; set; }
}
