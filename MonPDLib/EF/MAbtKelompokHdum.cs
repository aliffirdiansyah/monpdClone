using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Id", "PemakaianBatasMinim")]
[Table("M_ABT_KELOMPOK_HDA")]
public partial class MAbtKelompokHdum
{
    [Key]
    [Column("ID")]
    [Precision(10)]
    public int Id { get; set; }

    [Key]
    [Column("PEMAKAIAN_BATAS_MINIM")]
    [Precision(10)]
    public int PemakaianBatasMinim { get; set; }

    [Column("PEMAKAIAN_BATAS_MAKS")]
    [Precision(10)]
    public int PemakaianBatasMaks { get; set; }

    [Column("HARGA")]
    [Precision(10)]
    public int Harga { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("MAbtKelompokHda")]
    public virtual MAbtKelompok IdNavigation { get; set; } = null!;
}
