using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "Tahun", "Masapajak", "Seq")]
[Table("OP_HOTEL_KETETAPAN")]
public partial class OpHotelKetetapan
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TAHUN")]
    [Precision(10)]
    public int Tahun { get; set; }

    [Key]
    [Column("MASAPAJAK")]
    [Precision(10)]
    public int Masapajak { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Column("JENIS_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string JenisKetetapan { get; set; } = null!;

    [Column("NPWPD")]
    [StringLength(100)]
    [Unicode(false)]
    public string Npwpd { get; set; } = null!;

    [Column("TGL_KETETAPAN", TypeName = "DATE")]
    public DateTime TglKetetapan { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string KategoriNama { get; set; } = null!;

    [Column("TGL_JATUH_TEMPO_BAYAR", TypeName = "DATE")]
    public DateTime TglJatuhTempoBayar { get; set; }

    [Column("IS_LUNAS", TypeName = "NUMBER")]
    public decimal IsLunas { get; set; }

    [Column("TGL_LUNAS", TypeName = "DATE")]
    public DateTime? TglLunas { get; set; }

    [Column("POKOK", TypeName = "NUMBER")]
    public decimal Pokok { get; set; }

    [Column("PENGURANG_POKOK", TypeName = "NUMBER")]
    public decimal PengurangPokok { get; set; }

    [Column("AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(10)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("OBJEK")]
    [StringLength(10)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("SUB_RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("KategoriId")]
    [InverseProperty("OpHotelKetetapans")]
    public virtual MKategoriPajak Kategori { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpHotelKetetapans")]
    public virtual OpHotel NopNavigation { get; set; } = null!;

    [InverseProperty("OpHotelKetetapan")]
    public virtual ICollection<OpHotelKetetapanSspd> OpHotelKetetapanSspds { get; set; } = new List<OpHotelKetetapanSspd>();
}
