using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[PrimaryKey("Nop", "IdJenisKendaraan")]
[Table("OP_PARKIR_TARIF")]
public partial class OpParkirTarif
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("ID_JENIS_KENDARAAN")]
    [Precision(10)]
    public int IdJenisKendaraan { get; set; }

    [Column("KAPASITAS_MAKS")]
    [Precision(10)]
    public int KapasitasMaks { get; set; }

    [Column("TARIF_AWAL")]
    [Precision(10)]
    public int TarifAwal { get; set; }

    [Column("TARIF_PROGRESIF")]
    [Precision(10)]
    public int TarifProgresif { get; set; }

    [Column("TARIF_MEMBER")]
    [Precision(10)]
    public int TarifMember { get; set; }

    [Column("TARIF_VALET")]
    [Precision(10)]
    public int TarifValet { get; set; }

    [ForeignKey("IdJenisKendaraan")]
    [InverseProperty("OpParkirTarifs")]
    public virtual MJenisKendaraan IdJenisKendaraanNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpParkirTarifs")]
    public virtual OpParkir NopNavigation { get; set; } = null!;
}
