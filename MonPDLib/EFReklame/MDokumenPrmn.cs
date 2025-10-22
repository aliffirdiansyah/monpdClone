using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("M_DOKUMEN_PRMN")]
public partial class MDokumenPrmn
{
    [Key]
    [Column("ID_DOKUMEN")]
    [Precision(10)]
    public int IdDokumen { get; set; }

    [Column("IS_MANDATORY")]
    [Precision(2)]
    public byte IsMandatory { get; set; }

    [ForeignKey("IdDokumen")]
    [InverseProperty("MDokumenPrmn")]
    public virtual MDokuman IdDokumenNavigation { get; set; } = null!;
}
