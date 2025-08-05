using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "TahunBuku")]
[Table("DB_POTENSI_PARKIR")]
public partial class DbPotensiParkir
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("JENIS_TARIF")]
    [Precision(10)]
    public int? JenisTarif { get; set; }

    [Column("SISTEM_PARKIR")]
    [Precision(10)]
    public int? SistemParkir { get; set; }

    [Column("TO_WD", TypeName = "NUMBER(12,2)")]
    public decimal? ToWd { get; set; }

    [Column("TO_WE", TypeName = "NUMBER(12,2)")]
    public decimal? ToWe { get; set; }

    [Column("KAP_SEPEDA")]
    [Precision(10)]
    public int? KapSepeda { get; set; }

    [Column("TERPARKIR_SEPEDA_WD")]
    [Precision(10)]
    public int? TerparkirSepedaWd { get; set; }

    [Column("TERPARKIR_SEPEDA_WE")]
    [Precision(10)]
    public int? TerparkirSepedaWe { get; set; }

    [Column("TARIF_SEPEDA", TypeName = "NUMBER(12,2)")]
    public decimal? TarifSepeda { get; set; }

    [Column("OMZET_SEPEDA", TypeName = "NUMBER(18,2)")]
    public decimal? OmzetSepeda { get; set; }

    [Column("KAP_MOTOR")]
    [Precision(10)]
    public int? KapMotor { get; set; }

    [Column("TERPARKIR_MOTOR_WD")]
    [Precision(10)]
    public int? TerparkirMotorWd { get; set; }

    [Column("TERPARKIR_MOTOR_WE")]
    [Precision(10)]
    public int? TerparkirMotorWe { get; set; }

    [Column("TARIF_MOTOR", TypeName = "NUMBER(12,2)")]
    public decimal? TarifMotor { get; set; }

    [Column("OMZET_MOTOR", TypeName = "NUMBER(18,2)")]
    public decimal? OmzetMotor { get; set; }

    [Column("KAP_MOBIL")]
    [Precision(10)]
    public int? KapMobil { get; set; }

    [Column("TERPARKIR_MOBIL_WD")]
    [Precision(10)]
    public int? TerparkirMobilWd { get; set; }

    [Column("TERPARKIR_MOBIL_WE")]
    [Precision(10)]
    public int? TerparkirMobilWe { get; set; }

    [Column("TARIF_MOBIL", TypeName = "NUMBER(12,2)")]
    public decimal? TarifMobil { get; set; }

    [Column("OMZET_MOBIL", TypeName = "NUMBER(18,2)")]
    public decimal? OmzetMobil { get; set; }

    [Column("KAP_TRUK_MINI")]
    [Precision(10)]
    public int? KapTrukMini { get; set; }

    [Column("TERPARKIR_TRUK_MINI_WD")]
    [Precision(10)]
    public int? TerparkirTrukMiniWd { get; set; }

    [Column("TERPARKIR_TRUK_MINI_WE")]
    [Precision(10)]
    public int? TerparkirTrukMiniWe { get; set; }

    [Column("TARIF_TRUK_MINI", TypeName = "NUMBER(12,2)")]
    public decimal? TarifTrukMini { get; set; }

    [Column("OMZET_TRUK_MINI", TypeName = "NUMBER(18,2)")]
    public decimal? OmzetTrukMini { get; set; }

    [Column("KAP_TRUK_BUS")]
    [Precision(10)]
    public int? KapTrukBus { get; set; }

    [Column("TERPARKIR_TRUK_BUS_WD")]
    [Precision(10)]
    public int? TerparkirTrukBusWd { get; set; }

    [Column("TERPARKIR_TRUK_BUS_WE")]
    [Precision(10)]
    public int? TerparkirTrukBusWe { get; set; }

    [Column("TARIF_TRUK_BUS", TypeName = "NUMBER(12,2)")]
    public decimal? TarifTrukBus { get; set; }

    [Column("OMZET_TRUK_BUS", TypeName = "NUMBER(18,2)")]
    public decimal? OmzetTrukBus { get; set; }

    [Column("KAP_TRAILER")]
    [Precision(10)]
    public int? KapTrailer { get; set; }

    [Column("TERPARKIR_TRAILER_WD")]
    [Precision(10)]
    public int? TerparkirTrailerWd { get; set; }

    [Column("TERPARKIR_TRAILER_WE")]
    [Precision(10)]
    public int? TerparkirTrailerWe { get; set; }

    [Column("TARIF_TRAILER", TypeName = "NUMBER(12,2)")]
    public decimal? TarifTrailer { get; set; }

    [Column("OMZET_TRAILER", TypeName = "NUMBER(18,2)")]
    public decimal? OmzetTrailer { get; set; }

    [Column("TOTAL_OMZET", TypeName = "NUMBER(20,2)")]
    public decimal? TotalOmzet { get; set; }

    [Column("CREATED_AT", TypeName = "DATE")]
    public DateTime? CreatedAt { get; set; }

    [Column("UPDATED_AT", TypeName = "DATE")]
    public DateTime? UpdatedAt { get; set; }

    [Key]
    [Column("TAHUN_BUKU")]
    [Precision(10)]
    public int TahunBuku { get; set; }
}
