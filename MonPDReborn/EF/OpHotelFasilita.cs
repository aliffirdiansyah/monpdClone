using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[PrimaryKey("Nop", "IdFasilitas")]
[Table("OP_HOTEL_FASILITAS")]
public partial class OpHotelFasilita
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("ID_FASILITAS")]
    [Precision(10)]
    public int IdFasilitas { get; set; }

    [Column("JUMLAH")]
    [Precision(10)]
    public int Jumlah { get; set; }

    [Column("NAMA_FASILITAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaFasilitas { get; set; }

    [Column("KAPASITAS")]
    [Precision(10)]
    public int Kapasitas { get; set; }

    [ForeignKey("IdFasilitas")]
    [InverseProperty("OpHotelFasilita")]
    public virtual MFasilita IdFasilitasNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpHotelFasilita")]
    public virtual OpHotel NopNavigation { get; set; } = null!;
}
