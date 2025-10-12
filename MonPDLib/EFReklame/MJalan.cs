using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("M_JALAN")]
public partial class MJalan
{
    [Column("KELAS_JALAN")]
    [Precision(10)]
    public int? KelasJalan { get; set; }

    [Key]
    [Column("ID_JALAN")]
    [Precision(10)]
    public int IdJalan { get; set; }

    [Column("NAMA_JALAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaJalan { get; set; } = null!;

    [Column("KET")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? InsBy { get; set; }
}
