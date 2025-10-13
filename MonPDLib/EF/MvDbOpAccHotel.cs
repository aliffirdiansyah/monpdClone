using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbOpAccHotel
{
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("TAHUN_MIN1", TypeName = "NUMBER")]
    public decimal? TahunMin1 { get; set; }

    [Column("TAHUN_INI", TypeName = "NUMBER")]
    public decimal? TahunIni { get; set; }

    [Column("TAHUN_INI_SAMPAI_SYSDATE", TypeName = "NUMBER")]
    public decimal? TahunIniSampaiSysdate { get; set; }
}
