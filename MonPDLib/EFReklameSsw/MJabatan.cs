using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_JABATAN")]
public partial class MJabatan
{
    [Key]
    [Column("ID", TypeName = "NUMBER(38)")]
    public decimal Id { get; set; }

    [Column("NAMA")]
    [StringLength(500)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("DESKRIPSI")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Deskripsi { get; set; }

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("JabatanNavigation")]
    public virtual ICollection<MPegawai> MPegawais { get; set; } = new List<MPegawai>();
}
