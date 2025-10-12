using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("MinKetinggian", "MaxKetinggian", "TglAwalBerlaku")]
[Table("M_NILAI_STRATEGIS_TINGGI")]
public partial class MNilaiStrategisTinggi
{
    [Key]
    [Column("MIN_KETINGGIAN", TypeName = "NUMBER(10,2)")]
    public decimal MinKetinggian { get; set; }

    [Key]
    [Column("MAX_KETINGGIAN", TypeName = "NUMBER(10,2)")]
    public decimal MaxKetinggian { get; set; }

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
