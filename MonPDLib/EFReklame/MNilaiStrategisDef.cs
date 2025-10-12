using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("IdJenisReklame", "TglAwalBerlaku")]
[Table("M_NILAI_STRATEGIS_DEF")]
public partial class MNilaiStrategisDef
{
    [Key]
    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int IdJenisReklame { get; set; }

    [Column("LOKASI")]
    [Precision(10)]
    public int Lokasi { get; set; }

    [Column("SPANDANG")]
    [Precision(10)]
    public int Spandang { get; set; }

    [Column("KETINGGIAN")]
    [Precision(10)]
    public int Ketinggian { get; set; }

    [Key]
    [Column("TGL_AWAL_BERLAKU", TypeName = "DATE")]
    public DateTime TglAwalBerlaku { get; set; }

    [Column("TGL_AKHIR_BERLAKU", TypeName = "DATE")]
    public DateTime? TglAkhirBerlaku { get; set; }

    [Column("KETERANGAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [ForeignKey("IdJenisReklame")]
    [InverseProperty("MNilaiStrategisDefs")]
    public virtual MJenisReklame IdJenisReklameNavigation { get; set; } = null!;
}
