using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "TahunBuku")]
[Table("DB_POTENSI_HOTEL")]
public partial class DbPotensiHotel
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("TOTAL_ROOM")]
    [Precision(10)]
    public int? TotalRoom { get; set; }

    [Column("AVG_ROOM_PRICE", TypeName = "NUMBER(12,2)")]
    public decimal? AvgRoomPrice { get; set; }

    [Column("OKUPANSI_RATE_ROOM", TypeName = "NUMBER(10,2)")]
    public decimal? OkupansiRateRoom { get; set; }

    [Column("AVG_ROOM_SOLD", TypeName = "NUMBER(10,2)")]
    public decimal? AvgRoomSold { get; set; }

    [Column("ROOM_OMZET", TypeName = "NUMBER(18,2)")]
    public decimal? RoomOmzet { get; set; }

    [Column("MAX_PAX_BANQUET")]
    [Precision(10)]
    public int? MaxPaxBanquet { get; set; }

    [Column("AVG_BANQUET_PRICE", TypeName = "NUMBER(12,2)")]
    public decimal? AvgBanquetPrice { get; set; }

    [Column("OKUPANSI_RATE_BANQUET", TypeName = "NUMBER(10,2)")]
    public decimal? OkupansiRateBanquet { get; set; }

    [Column("AVG_PAX_BANQUET_SOLD", TypeName = "NUMBER(10,2)")]
    public decimal? AvgPaxBanquetSold { get; set; }

    [Column("BANQUET_OMZET", TypeName = "NUMBER(18,2)")]
    public decimal? BanquetOmzet { get; set; }

    [Column("CREATED_AT", TypeName = "DATE")]
    public DateTime? CreatedAt { get; set; }

    [Column("UPDATED_AT", TypeName = "DATE")]
    public DateTime? UpdatedAt { get; set; }

    [Key]
    [Column("TAHUN_BUKU")]
    [Precision(10)]
    public int TahunBuku { get; set; }

    [Column("POTENSI_PAJAK_TAHUN", TypeName = "NUMBER(30)")]
    public decimal? PotensiPajakTahun { get; set; }

    [Column("AVG_ROOM_PRICE_KOS", TypeName = "NUMBER(12,2)")]
    public decimal? AvgRoomPriceKos { get; set; }
}
