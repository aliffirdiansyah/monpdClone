using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("IdJenisReklame", "TglAwalBerlaku")]
[Table("M_NSR_INS_JAMBONG")]
public partial class MNsrInsJambong
{
    [Key]
    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int IdJenisReklame { get; set; }

    [Column("NILAI", TypeName = "NUMBER(10,2)")]
    public decimal Nilai { get; set; }

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

    [ForeignKey("IdJenisReklame, TglAwalBerlaku")]
    [InverseProperty("MNsrInsJambong")]
    public virtual MNsrIn MNsrIn { get; set; } = null!;
}
