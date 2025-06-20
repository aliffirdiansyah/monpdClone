using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_JENIS_KENDARAAN")]
public partial class MJenisKendaraan
{
    [Key]
    [Column("ID")]
    [Precision(10)]
    public int Id { get; set; }

    [Column("NAMA")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("KET")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(45)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("TARIF_PERWALI")]
    [Precision(10)]
    public int TarifPerwali { get; set; }

    [InverseProperty("IdJenisKendaraanNavigation")]
    public virtual ICollection<OpParkirTarif> OpParkirTarifs { get; set; } = new List<OpParkirTarif>();
}
