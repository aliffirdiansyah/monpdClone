using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwMonHotel365
{
    [Column("TANGGAL")]
    [StringLength(6)]
    [Unicode(false)]
    public string? Tanggal { get; set; }

    [Column("HARI", TypeName = "NUMBER")]
    public decimal? Hari { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("NOMINAL_2022", TypeName = "NUMBER")]
    public decimal? Nominal2022 { get; set; }

    [Column("NOMINAL_2023", TypeName = "NUMBER")]
    public decimal? Nominal2023 { get; set; }

    [Column("NOMINAL_2024", TypeName = "NUMBER")]
    public decimal? Nominal2024 { get; set; }

    [Column("AVG_3TAHUN", TypeName = "NUMBER(18,2)")]
    public decimal? Avg3tahun { get; set; }

    [Column("MAX_NOMINAL", TypeName = "NUMBER")]
    public decimal? MaxNominal { get; set; }

    [Column("MIN_NOMINAL", TypeName = "NUMBER")]
    public decimal? MinNominal { get; set; }

    [Column("REALISASI_2025", TypeName = "NUMBER")]
    public decimal? Realisasi2025 { get; set; }

    [Column("KOEFISIEN_NILAI", TypeName = "NUMBER")]
    public decimal? KoefisienNilai { get; set; }

    [Column("ID_PAJAK", TypeName = "NUMBER")]
    public decimal? IdPajak { get; set; }

    [Column("JENIS_PAJAK")]
    [StringLength(21)]
    [Unicode(false)]
    public string? JenisPajak { get; set; }
}
