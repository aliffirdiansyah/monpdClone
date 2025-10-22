using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel", "Seq", "SeqFoto")]
[Table("T_PERMOHONAN_INS_FOTO")]
public partial class TPermohonanInsFoto
{
    [Key]
    [Column("TAHUN_PEL")]
    [Precision(10)]
    public int TahunPel { get; set; }

    [Key]
    [Column("BULAN_PEL")]
    [Precision(10)]
    public int BulanPel { get; set; }

    [Key]
    [Column("SEQ_PEL")]
    [Precision(10)]
    public int SeqPel { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Key]
    [Column("SEQ_FOTO")]
    [Precision(10)]
    public int SeqFoto { get; set; }

    [Column("FILE_DATA", TypeName = "BLOB")]
    public byte[] FileData { get; set; } = null!;

    [Column("KET")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq")]
    [InverseProperty("TPermohonanInsFotos")]
    public virtual TPermohonanIn TPermohonanIn { get; set; } = null!;
}
