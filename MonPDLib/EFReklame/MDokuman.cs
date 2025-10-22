using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("M_DOKUMEN")]
public partial class MDokuman
{
    [Key]
    [Column("ID_DOKUMEN")]
    [Precision(10)]
    public int IdDokumen { get; set; }

    [Column("NAMA_DOKUMEN")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaDokumen { get; set; } = null!;

    [Column("AKTIF")]
    [Precision(2)]
    public byte Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("IdDokumenNavigation")]
    public virtual MDokumenIn? MDokumenIn { get; set; }
}
