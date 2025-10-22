using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("IdFile", "TahunPel", "BulanPel", "SeqPel")]
[Table("T_PERMOHONAN_FILE")]
public partial class TPermohonanFile
{
    [Key]
    [Column("ID_FILE")]
    [Precision(10)]
    public int IdFile { get; set; }

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

    [Column("IS_MANDATORY")]
    [Precision(10)]
    public int? IsMandatory { get; set; }

    [Column("ISI_FILE", TypeName = "BLOB")]
    public byte[]? IsiFile { get; set; }

    [ForeignKey("TahunPel, BulanPel, SeqPel")]
    [InverseProperty("TPermohonanFiles")]
    public virtual TPermohonan TPermohonan { get; set; } = null!;
}
