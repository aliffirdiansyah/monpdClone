using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("SudutPandang", "TglAwalBerlaku")]
[Table("M_NILAI_STRATEGIS_SPANDANG")]
public partial class MNilaiStrategisSpandang
{
    [Key]
    [Column("SUDUT_PANDANG")]
    [Precision(10)]
    public int SudutPandang { get; set; }

    [Column("IS_DLM_RUANG")]
    [Precision(10)]
    public int IsDlmRuang { get; set; }

    [Column("BOBOT", TypeName = "NUMBER(10,2)")]
    public decimal Bobot { get; set; }

    [Column("SKOR")]
    [Precision(10)]
    public int Skor { get; set; }

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
}
