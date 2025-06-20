using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_HOTEL")]
public partial class OpHotel
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal Kategori { get; set; }

    [Column("METODE_PENJUALAN")]
    [Precision(10)]
    public int MetodePenjualan { get; set; }

    [Column("METODE_PEMBAYARAN", TypeName = "NUMBER")]
    public decimal MetodePembayaran { get; set; }

    [Column("JUMLAH_KARYAWAN")]
    [Precision(10)]
    public int JumlahKaryawan { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("ALAT_PENGAWASAN", TypeName = "NUMBER")]
    public decimal AlatPengawasan { get; set; }

    [Column("TGL_PASANG", TypeName = "DATE")]
    public DateTime? TglPasang { get; set; }

    [ForeignKey("Kategori")]
    [InverseProperty("OpHotels")]
    public virtual MKategoriPajak KategoriNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpHotel")]
    public virtual Op NopNavigation { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHotelBanquet> OpHotelBanquets { get; set; } = new List<OpHotelBanquet>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHotelFasilita> OpHotelFasilita { get; set; } = new List<OpHotelFasilita>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHotelKamar> OpHotelKamars { get; set; } = new List<OpHotelKamar>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHotelKetetapan> OpHotelKetetapans { get; set; } = new List<OpHotelKetetapan>();

    [InverseProperty("NopNavigation")]
    public virtual OpHotelKost? OpHotelKost { get; set; }
}
