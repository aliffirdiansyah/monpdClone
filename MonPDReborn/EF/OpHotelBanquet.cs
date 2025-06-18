using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[PrimaryKey("Nop", "Seq")]
[Table("OP_HOTEL_BANQUET")]
public partial class OpHotelBanquet
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Column("NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("JUMLAH")]
    [Precision(10)]
    public int Jumlah { get; set; }

    [Column("JENIS_BANQUET")]
    [Precision(10)]
    public int JenisBanquet { get; set; }

    [Column("KAPASITAS")]
    [Precision(10)]
    public int Kapasitas { get; set; }

    [Column("HARGA_SEWA")]
    [Precision(10)]
    public int HargaSewa { get; set; }

    [Column("HARGA_PAKET")]
    [Precision(10)]
    public int HargaPaket { get; set; }

    [Column("OKUPANSI_RATA", TypeName = "NUMBER(10,2)")]
    public decimal OkupansiRata { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpHotelBanquets")]
    public virtual OpHotel NopNavigation { get; set; } = null!;
}
