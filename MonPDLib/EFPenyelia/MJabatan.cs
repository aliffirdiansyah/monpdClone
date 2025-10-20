using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Table("M_JABATAN")]
public partial class MJabatan
{
    [Key]
    [Column("ID")]
    [Precision(10)]
    public int Id { get; set; }

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

    [InverseProperty("Jabatan")]
    public virtual ICollection<MPegawai> MPegawais { get; set; } = new List<MPegawai>();
}
