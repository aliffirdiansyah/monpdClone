using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "KdPajak")]
[Table("POTENSI_CTRL_HOTEL")]
public partial class PotensiCtrlHotel
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

    [Column("TOTAL_ROOM")]
    [Precision(5)]
    public short TotalRoom { get; set; }

    [Column("OKUPANSI_RATE_ROOM", TypeName = "NUMBER(5,2)")]
    public decimal OkupansiRateRoom { get; set; }

    [Column("AVG_ROOM_PRICE", TypeName = "NUMBER(10,2)")]
    public decimal AvgRoomPrice { get; set; }

    [Column("AVG_ROOM_SOLD", TypeName = "NUMBER(10,2)")]
    public decimal AvgRoomSold { get; set; }

    [Column("ROOM_OMZET", TypeName = "NUMBER(15,2)")]
    public decimal RoomOmzet { get; set; }

    [Column("ROOM_TAX_BULAN", TypeName = "NUMBER(15,2)")]
    public decimal RoomTaxBulan { get; set; }

    [Column("MAX_PAX_BANQUET")]
    [Precision(5)]
    public short MaxPaxBanquet { get; set; }

    [Column("OKUPANSI_RATE_BANQUET", TypeName = "NUMBER(5,2)")]
    public decimal OkupansiRateBanquet { get; set; }

    [Column("AVG_PAX_BANQUET", TypeName = "NUMBER(10,2)")]
    public decimal AvgPaxBanquet { get; set; }

    [Column("AVG_BANQUET_PRICE", TypeName = "NUMBER(12,2)")]
    public decimal AvgBanquetPrice { get; set; }

    [Column("BANQUET_OMZET", TypeName = "NUMBER(15,2)")]
    public decimal BanquetOmzet { get; set; }

    [Column("BANQUET_TAX_BULAN", TypeName = "NUMBER(15,2)")]
    public decimal BanquetTaxBulan { get; set; }

    [Column("POTENSI_PAJAK_BULAN", TypeName = "NUMBER(15,2)")]
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
