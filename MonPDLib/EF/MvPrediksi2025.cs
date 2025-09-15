using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvPrediksi2025
{
    [Column("TANGGAL", TypeName = "DATE")]
    public DateTime? Tanggal { get; set; }

    [Column("HARI", TypeName = "NUMBER")]
    public decimal? Hari { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("ID_PAJAK", TypeName = "NUMBER")]
    public decimal? IdPajak { get; set; }

    [Column("JENIS_PAJAK")]
    [StringLength(27)]
    [Unicode(false)]
    public string? JenisPajak { get; set; }

    [Column("REALISASI_2025", TypeName = "NUMBER")]
    public decimal? Realisasi2025 { get; set; }

    [Column("PREDIKSI_2025", TypeName = "NUMBER")]
    public decimal? Prediksi2025 { get; set; }
}
