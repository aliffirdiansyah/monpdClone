using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("OP_PARKIR")]
public partial class OpParkir
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("METODE_PEMBAYARAN")]
    [Precision(10)]
    public int MetodePembayaran { get; set; }

    [Column("JUMLAH_KARYAWAN")]
    [Precision(10)]
    public int JumlahKaryawan { get; set; }

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal Kategori { get; set; }

    [Column("DIKELOLA", TypeName = "NUMBER")]
    public decimal Dikelola { get; set; }

    [Column("PUNGUT_TARIF")]
    [Precision(10)]
    public int PungutTarif { get; set; }

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
    [InverseProperty("OpParkirs")]
    public virtual MKategoriPajak KategoriNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpParkir")]
    public virtual Op NopNavigation { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpParkirJadwal> OpParkirJadwals { get; set; } = new List<OpParkirJadwal>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpParkirKetetapan> OpParkirKetetapans { get; set; } = new List<OpParkirKetetapan>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpParkirTarif> OpParkirTarifs { get; set; } = new List<OpParkirTarif>();
}
