using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "Tanggal", "Seq")]
[Table("DB_REKAM_PARKIR")]
public partial class DbRekamParkir
{
    [Key]
    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }

    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TANGGAL", TypeName = "DATE")]
    public DateTime Tanggal { get; set; }

    [Column("JENIS_BIAYA_PARKIR")]
    [StringLength(20)]
    [Unicode(false)]
    public string JenisBiayaParkir { get; set; } = null!;

    [Column("KAPASITAS_MOTOR", TypeName = "NUMBER")]
    public decimal KapasitasMotor { get; set; }

    [Column("KAPASITAS_MOBIL", TypeName = "NUMBER")]
    public decimal KapasitasMobil { get; set; }

    [Column("HASIL_JUMLAH_MOTOR", TypeName = "NUMBER")]
    public decimal HasilJumlahMotor { get; set; }

    [Column("HASIL_JUMLAH_MOBIL", TypeName = "NUMBER")]
    public decimal HasilJumlahMobil { get; set; }

    [Column("HASIL_JUMLAH_MOBIL_BOX", TypeName = "NUMBER")]
    public decimal HasilJumlahMobilBox { get; set; }

    [Column("HASIL_JUMLAH_TRUK", TypeName = "NUMBER")]
    public decimal HasilJumlahTruk { get; set; }

    [Column("HASIL_JUMLAH_TRAILER", TypeName = "NUMBER")]
    public decimal HasilJumlahTrailer { get; set; }

    [Column("EST_MOTOR_HARIAN", TypeName = "NUMBER")]
    public decimal EstMotorHarian { get; set; }

    [Column("EST_MOBIL_HARIAN", TypeName = "NUMBER")]
    public decimal EstMobilHarian { get; set; }

    [Column("EST_MOBIL_BOX_HARIAN", TypeName = "NUMBER")]
    public decimal EstMobilBoxHarian { get; set; }

    [Column("EST_TRUK_HARIAN", TypeName = "NUMBER")]
    public decimal EstTrukHarian { get; set; }

    [Column("EST_TRAILER_HARIAN", TypeName = "NUMBER")]
    public decimal EstTrailerHarian { get; set; }

    [Column("TARIF_MOTOR", TypeName = "NUMBER(12,2)")]
    public decimal TarifMotor { get; set; }

    [Column("TARIF_MOBIL", TypeName = "NUMBER(12,2)")]
    public decimal TarifMobil { get; set; }

    [Column("TARIF_MOBIL_BOX", TypeName = "NUMBER(12,2)")]
    public decimal TarifMobilBox { get; set; }

    [Column("TARIF_TRUK", TypeName = "NUMBER(12,2)")]
    public decimal TarifTruk { get; set; }

    [Column("TARIF_TRAILER", TypeName = "NUMBER(12,2)")]
    public decimal TarifTrailer { get; set; }

    [Column("OMZET_BULAN", TypeName = "NUMBER(15,2)")]
    public decimal OmzetBulan { get; set; }

    [Column("PAJAK_BULAN", TypeName = "NUMBER(15,2)")]
    public decimal PajakBulan { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal PajakId { get; set; }
}
