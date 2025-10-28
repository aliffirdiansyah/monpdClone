using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbMonJambong
{
    [Column("FLAG_PERMOHONAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FlagPermohonan { get; set; }

    [Column("NO_FORMULIR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("NAMA_PERUSAHAAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPerusahaan { get; set; }

    [Column("ISI_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? IsiReklame { get; set; }

    [Column("ALAMATREKLAME")]
    [StringLength(181)]
    [Unicode(false)]
    public string? Alamatreklame { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("JAMBONG", TypeName = "NUMBER")]
    public decimal? Jambong { get; set; }
}
