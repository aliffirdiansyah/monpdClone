using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nik", "Username", "KdAktifitas")]
[Table("M_PETUGAS_REKLAME")]
public partial class MPetugasReklame
{
    [Key]
    [Column("NIK")]
    [StringLength(16)]
    [Unicode(false)]
    public string Nik { get; set; } = null!;

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Key]
    [Column("USERNAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Key]
    [Column("KD_AKTIFITAS")]
    [StringLength(10)]
    [Unicode(false)]
    public string KdAktifitas { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }
}
