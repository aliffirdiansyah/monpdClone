using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "KdPajak")]
[Table("POTENSI_CTRL_PARKIR")]
public partial class PotensiCtrlParkir
{
    [Key]
    [Column("NOP")]
    [StringLength(23)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("KD_PAJAK")]
    [Precision(10)]
    public int KdPajak { get; set; }

    [Column("STATUS")]
    [Precision(10)]
    public int Status { get; set; }

    [Column("JENIS")]
    [Precision(10)]
    public int Jenis { get; set; }

    [Column("TO_WD", TypeName = "NUMBER(5,2)")]
    public decimal ToWd { get; set; }

    [Column("TO_WE", TypeName = "NUMBER(5,2)")]
    public decimal ToWe { get; set; }

    [Column("KAP_SEPEDA")]
    [Precision(10)]
    public int KapSepeda { get; set; }

    [Column("TERPARKIR_SEPEDA_WD", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirSepedaWd { get; set; }

    [Column("TERPARKIR_SEPEDA_WE", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirSepedaWe { get; set; }

    [Column("TARIF_SEPEDA", TypeName = "NUMBER(10,2)")]
    public decimal TarifSepeda { get; set; }

    [Column("OMZET_SEPEDA", TypeName = "NUMBER(18,2)")]
    public decimal OmzetSepeda { get; set; }

    [Column("KAP_MOTOR")]
    [Precision(10)]
    public int KapMotor { get; set; }

    [Column("TERPARKIR_MOTOR_WD", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirMotorWd { get; set; }

    [Column("TERPARKIR_MOTOR_WE", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirMotorWe { get; set; }

    [Column("TARIF_MOTOR", TypeName = "NUMBER(10,2)")]
    public decimal TarifMotor { get; set; }

    [Column("OMZET_MOTOR", TypeName = "NUMBER(18,2)")]
    public decimal OmzetMotor { get; set; }

    [Column("KAP_MOBIL")]
    [Precision(10)]
    public int KapMobil { get; set; }

    [Column("TERPARKIR_MOBIL_WD", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirMobilWd { get; set; }

    [Column("TERPARKIR_MOBIL_WE", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirMobilWe { get; set; }

    [Column("TARIF_MOBIL", TypeName = "NUMBER(10,2)")]
    public decimal TarifMobil { get; set; }

    [Column("OMZET_MOBIL", TypeName = "NUMBER(18,2)")]
    public decimal OmzetMobil { get; set; }

    [Column("KAP_TRUK_MINI")]
    [Precision(10)]
    public int KapTrukMini { get; set; }

    [Column("TERPARKIR_TRUK_MINI_WD", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirTrukMiniWd { get; set; }

    [Column("TERPARKIR_TRUK_MINI_WE", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirTrukMiniWe { get; set; }

    [Column("TARIF_TRUK_MINI", TypeName = "NUMBER(10,2)")]
    public decimal TarifTrukMini { get; set; }

    [Column("OMZET_TRUK_MINI", TypeName = "NUMBER(18,2)")]
    public decimal OmzetTrukMini { get; set; }

    [Column("KAP_TRUK_BUS")]
    [Precision(10)]
    public int KapTrukBus { get; set; }

    [Column("TERPARKIR_TRUK_BUS_WD", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirTrukBusWd { get; set; }

    [Column("TERPARKIR_TRUK_BUS_WE", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirTrukBusWe { get; set; }

    [Column("TARIF_TRUK_BUS", TypeName = "NUMBER(10,2)")]
    public decimal TarifTrukBus { get; set; }

    [Column("OMZET_TRUK_BUS", TypeName = "NUMBER(18,2)")]
    public decimal OmzetTrukBus { get; set; }

    [Column("KAP_TRAILER")]
    [Precision(10)]
    public int KapTrailer { get; set; }

    [Column("TERPARKIR_TRAILER_WD", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirTrailerWd { get; set; }

    [Column("TERPARKIR_TRAILER_WE", TypeName = "NUMBER(10,2)")]
    public decimal TerparkirTrailerWe { get; set; }

    [Column("TARIF_TRAILER", TypeName = "NUMBER(10,2)")]
    public decimal TarifTrailer { get; set; }

    [Column("OMZET_TRAILER", TypeName = "NUMBER(18,2)")]
    public decimal OmzetTrailer { get; set; }

    [Column("TOTAL_OMZET", TypeName = "NUMBER(18,2)")]
    public decimal TotalOmzet { get; set; }

    [Column("POTENSI_PAJAK_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal PotensiPajakBulan { get; set; }

    [Column("POTENSI_PAJAK_TAHUN", TypeName = "NUMBER(18,2)")]
    public decimal PotensiPajakTahun { get; set; }

    [Column("CREATED_AT")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    [Precision(6)]
    public DateTime? UpdatedAt { get; set; }
}
