using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("IdJenisReklame", "NilaiKetinggian", "TglAwalBerlaku")]
[Table("M_NSR_TINGGI")]
public partial class MNsrTinggi
{
    [Key]
    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int IdJenisReklame { get; set; }

    [Column("MASA_PAJAK")]
    [Precision(2)]
    public byte MasaPajak { get; set; }

    [Key]
    [Column("NILAI_KETINGGIAN", TypeName = "NUMBER(10,2)")]
    public decimal NilaiKetinggian { get; set; }

    [Column("KET")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Ket { get; set; }

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
    [InverseProperty("MNsrTinggis")]
    public virtual MJenisReklame IdJenisReklameNavigation { get; set; } = null!;
}
