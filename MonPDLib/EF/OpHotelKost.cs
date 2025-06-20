using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_HOTEL_KOST")]
public partial class OpHotelKost
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("JML_KMR_MND_DLM")]
    [Precision(10)]
    public int JmlKmrMndDlm { get; set; }

    [Column("TRF_HARI_KMR_MND_DLM")]
    [Precision(10)]
    public int TrfHariKmrMndDlm { get; set; }

    [Column("TRF_BULAN_KMR_MND_DLM")]
    [Precision(10)]
    public int TrfBulanKmrMndDlm { get; set; }

    [Column("JML_KMR_MND_LUAR")]
    [Precision(10)]
    public int JmlKmrMndLuar { get; set; }

    [Column("TRF_HARI_KMR_MND_LUAR")]
    [Precision(10)]
    public int TrfHariKmrMndLuar { get; set; }

    [Column("TRF_BULAN_KMR_MND_LUAR")]
    [Precision(10)]
    public int TrfBulanKmrMndLuar { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpHotelKost")]
    public virtual OpHotel NopNavigation { get; set; } = null!;
}
