using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class ItungTargetHiburan
{
    [Column("NOP")]
    [StringLength(43)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("STATUS_OP", TypeName = "NUMBER")]
    public decimal? StatusOp { get; set; }

    [Column("TAHUN_DATA", TypeName = "NUMBER")]
    public decimal? TahunData { get; set; }

    [Column("MASA_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? MasaPajakKetetapan { get; set; }

    [Column("NOMINAL_POKOK_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalPokokBayar { get; set; }

    [Column("RN", TypeName = "NUMBER")]
    public decimal? Rn { get; set; }
}
