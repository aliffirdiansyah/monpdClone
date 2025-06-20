using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_RESTO")]
public partial class OpResto
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal Kategori { get; set; }

    [Column("JUMLAH_KARYAWAN")]
    [Precision(10)]
    public int JumlahKaryawan { get; set; }

    [Column("METODE_PENJUALAN")]
    [Precision(10)]
    public int MetodePenjualan { get; set; }

    [Column("METODE_PEMBAYARAN")]
    [Precision(10)]
    public int MetodePembayaran { get; set; }

    [Column("JUMLAH_MEJA")]
    [Precision(10)]
    public int JumlahMeja { get; set; }

    [Column("JUMLAH_KURSI")]
    [Precision(10)]
    public int JumlahKursi { get; set; }

    [Column("KAPASITAS_RUANGAN_ORANG")]
    [Precision(10)]
    public int KapasitasRuanganOrang { get; set; }

    [Column("MAKSIMAL_PRODUKSI_PORSI_HARI")]
    [Precision(10)]
    public int MaksimalProduksiPorsiHari { get; set; }

    [Column("RATA_TERJUAL_PORSI_HARI")]
    [Precision(10)]
    public int RataTerjualPorsiHari { get; set; }

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
    [InverseProperty("OpRestos")]
    public virtual MKategoriPajak KategoriNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpResto")]
    public virtual Op NopNavigation { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpRestoFasilita> OpRestoFasilita { get; set; } = new List<OpRestoFasilita>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpRestoJadwal> OpRestoJadwals { get; set; } = new List<OpRestoJadwal>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpRestoKetetapan> OpRestoKetetapans { get; set; } = new List<OpRestoKetetapan>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpRestoMenu> OpRestoMenus { get; set; } = new List<OpRestoMenu>();
}
