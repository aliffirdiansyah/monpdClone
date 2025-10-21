using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("M_JENIS_REKLAME")]
public partial class MJenisReklame
{
    [Key]
    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int IdJenisReklame { get; set; }

    [Column("NAMA_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Column("KET")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("IS_BERJALAN")]
    [Precision(10)]
    public int? IsBerjalan { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("KATEGORI")]
    [Precision(10)]
    public int Kategori { get; set; }

    [Column("MODE_UKUR")]
    [Precision(2)]
    public byte? ModeUkur { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<MNilaiSatuanStrategi> MNilaiSatuanStrategis { get; set; } = new List<MNilaiSatuanStrategi>();

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<MNilaiStrategisDef> MNilaiStrategisDefs { get; set; } = new List<MNilaiStrategisDef>();

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<MNsrIn> MNsrIns { get; set; } = new List<MNsrIn>();

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<MNsrLua> MNsrLuas { get; set; } = new List<MNsrLua>();

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<MNsrTinggi> MNsrTinggis { get; set; } = new List<MNsrTinggi>();

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<TPermohonanIn> TPermohonanIns { get; set; } = new List<TPermohonanIn>();

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<TPermohonanInsNilai> TPermohonanInsNilais { get; set; } = new List<TPermohonanInsNilai>();

    [InverseProperty("IdJenisReklameNavigation")]
    public virtual ICollection<TPermohonanPrmn> TPermohonanPrmns { get; set; } = new List<TPermohonanPrmn>();
}
