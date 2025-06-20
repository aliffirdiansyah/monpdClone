using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_FASILITAS")]
public partial class MFasilita
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

    [InverseProperty("IdFasilitasNavigation")]
    public virtual ICollection<OpHotelFasilita> OpHotelFasilita { get; set; } = new List<OpHotelFasilita>();

    [InverseProperty("IdFasilitasNavigation")]
    public virtual ICollection<OpRestoFasilita> OpRestoFasilita { get; set; } = new List<OpRestoFasilita>();
}
