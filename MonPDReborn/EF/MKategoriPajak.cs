using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("M_KATEGORI_PAJAK")]
public partial class MKategoriPajak
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("PAJAK_ID")]
    [Precision(10)]
    public int PajakId { get; set; }

    [Column("NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("KET")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("MODE_KAP_TAR", TypeName = "NUMBER")]
    public decimal ModeKapTar { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(45)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("Kategori")]
    public virtual ICollection<OpAbtKetetapan> OpAbtKetetapans { get; set; } = new List<OpAbtKetetapan>();

    [InverseProperty("KategoriNavigation")]
    public virtual ICollection<OpAbt> OpAbts { get; set; } = new List<OpAbt>();

    [InverseProperty("Kategori")]
    public virtual ICollection<OpHiburanKetetapan> OpHiburanKetetapans { get; set; } = new List<OpHiburanKetetapan>();

    [InverseProperty("KategoriNavigation")]
    public virtual ICollection<OpHiburan> OpHiburans { get; set; } = new List<OpHiburan>();

    [InverseProperty("Kategori")]
    public virtual ICollection<OpHotelKetetapan> OpHotelKetetapans { get; set; } = new List<OpHotelKetetapan>();

    [InverseProperty("KategoriNavigation")]
    public virtual ICollection<OpHotel> OpHotels { get; set; } = new List<OpHotel>();

    [InverseProperty("Kategori")]
    public virtual ICollection<OpListrikKetetapan> OpListrikKetetapans { get; set; } = new List<OpListrikKetetapan>();

    [InverseProperty("KategoriNavigation")]
    public virtual ICollection<OpParkir> OpParkirs { get; set; } = new List<OpParkir>();

    [InverseProperty("Kategori")]
    public virtual ICollection<OpPbbKetetapan> OpPbbKetetapans { get; set; } = new List<OpPbbKetetapan>();

    [InverseProperty("KategoriNavigation")]
    public virtual ICollection<OpPbb> OpPbbs { get; set; } = new List<OpPbb>();

    [InverseProperty("Kategori")]
    public virtual ICollection<OpRestoKetetapan> OpRestoKetetapans { get; set; } = new List<OpRestoKetetapan>();

    [InverseProperty("KategoriNavigation")]
    public virtual ICollection<OpResto> OpRestos { get; set; } = new List<OpResto>();

    [ForeignKey("PajakId")]
    [InverseProperty("MKategoriPajaks")]
    public virtual MPajak Pajak { get; set; } = null!;
}
