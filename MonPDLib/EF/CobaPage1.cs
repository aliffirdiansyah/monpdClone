using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class CobaPage1
{
    [Column("KOMPONEN")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Komponen { get; set; }

    [Column("JUMLAH_OBJEK", TypeName = "NUMBER")]
    public decimal? JumlahObjek { get; set; }

    [Column("TARGET_TAHUNAN", TypeName = "NUMBER")]
    public decimal? TargetTahunan { get; set; }

    [Column("TOTAL_KETETAPAN", TypeName = "NUMBER")]
    public decimal? TotalKetetapan { get; set; }

    [Column("TOTAL_REALISASI", TypeName = "NUMBER")]
    public decimal? TotalRealisasi { get; set; }

    [Column("SELISIH", TypeName = "NUMBER")]
    public decimal? Selisih { get; set; }

    [Column("CAPAIAN_PERSEN", TypeName = "NUMBER")]
    public decimal? CapaianPersen { get; set; }

    [Column("PREDIKSI", TypeName = "NUMBER")]
    public decimal? Prediksi { get; set; }

    [Column("PERSEN_PREDIKSI", TypeName = "NUMBER")]
    public decimal? PersenPrediksi { get; set; }

    [Column("PERSEN_TARGET", TypeName = "NUMBER")]
    public decimal? PersenTarget { get; set; }
}
