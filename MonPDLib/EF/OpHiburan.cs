using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_HIBURAN")]
public partial class OpHiburan
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("METODE_PENJUALAN")]
    [Precision(10)]
    public int MetodePenjualan { get; set; }

    [Column("METODE_PEMBAYARAN", TypeName = "NUMBER")]
    public decimal MetodePembayaran { get; set; }

    [Column("JUMLAH_KARYAWAN")]
    [Precision(10)]
    public int JumlahKaryawan { get; set; }

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal Kategori { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("ALAT_PENGAWASAN", TypeName = "NUMBER")]
    public decimal AlatPengawasan { get; set; }

    [Column("TGL_PASANG", TypeName = "DATE")]
    public DateTime? TglPasang { get; set; }

    [ForeignKey("Kategori")]
    [InverseProperty("OpHiburans")]
    public virtual MKategoriPajak KategoriNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpHiburan")]
    public virtual Op NopNavigation { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHiburanJadwal> OpHiburanJadwals { get; set; } = new List<OpHiburanJadwal>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHiburanKetetapan> OpHiburanKetetapans { get; set; } = new List<OpHiburanKetetapan>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHiburanKtFilm> OpHiburanKtFilms { get; set; } = new List<OpHiburanKtFilm>();

    [InverseProperty("NopNavigation")]
    public virtual OpHiburanKtGym? OpHiburanKtGym { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpHiburanKtOther> OpHiburanKtOthers { get; set; } = new List<OpHiburanKtOther>();
}
