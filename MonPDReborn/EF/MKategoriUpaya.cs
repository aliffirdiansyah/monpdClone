using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("M_KATEGORI_UPAYA")]
public partial class MKategoriUpaya
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("KET")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(45)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
