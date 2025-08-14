using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DATA_TPK_HOTEL")]
public partial class DataTpkHotel
{
    [Column("TAHUN")]
    [Precision(4)]
    public byte? Tahun { get; set; }

    [Column("BULAN")]
    [Precision(2)]
    public byte? Bulan { get; set; }

    [Column("HOTEL_BINTANG", TypeName = "NUMBER(5,2)")]
    public decimal? HotelBintang { get; set; }

    [Column("HOTEL_NON_BINTANG", TypeName = "NUMBER(5,2)")]
    public decimal? HotelNonBintang { get; set; }
}
