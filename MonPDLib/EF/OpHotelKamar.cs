using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "IdTipe")]
[Table("OP_HOTEL_KAMAR")]
public partial class OpHotelKamar
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("ID_TIPE")]
    [Precision(10)]
    public int IdTipe { get; set; }

    [Column("JUMLAH_KAMAR")]
    [Precision(10)]
    public int JumlahKamar { get; set; }

    [Column("TARIF")]
    [Precision(10)]
    public int Tarif { get; set; }

    [ForeignKey("IdTipe")]
    [InverseProperty("OpHotelKamars")]
    public virtual MTipekamarhotel IdTipeNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpHotelKamars")]
    public virtual OpHotel NopNavigation { get; set; } = null!;
}
