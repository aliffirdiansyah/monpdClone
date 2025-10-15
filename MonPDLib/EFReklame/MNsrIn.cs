using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("IdJenisReklame", "TglAwalBerlaku")]
[Table("M_NSR_INS")]
public partial class MNsrIn
{
    [Key]
    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int IdJenisReklame { get; set; }

    [Column("MASA_PAJAK")]
    [Precision(2)]
    public byte MasaPajak { get; set; }

    [Column("SATUAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Satuan { get; set; }

    [Column("NILAI_NJOP", TypeName = "NUMBER(18,2)")]
    public decimal? NilaiNjop { get; set; }

    [Column("KET")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("INDIKATOR", TypeName = "NUMBER(10,2)")]
    public decimal? Indikator { get; set; }

    [Key]
    [Column("TGL_AWAL_BERLAKU", TypeName = "DATE")]
    public DateTime TglAwalBerlaku { get; set; }

    [Column("TGL_AKHIR_BERLAKU", TypeName = "DATE")]
    public DateTime? TglAkhirBerlaku { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [ForeignKey("IdJenisReklame")]
    [InverseProperty("MNsrIns")]
    public virtual MJenisReklame IdJenisReklameNavigation { get; set; } = null!;
}
