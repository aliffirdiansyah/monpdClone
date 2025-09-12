using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("M_PETUGAS_REKLAME")]
[Index("Username", Name = "SYS_C0034201", IsUnique = true)]
public partial class MPetugasReklame
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("USERNAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("KD_AKTIFITAS")]
    [StringLength(10)]
    [Unicode(false)]
    public string KdAktifitas { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }
}
