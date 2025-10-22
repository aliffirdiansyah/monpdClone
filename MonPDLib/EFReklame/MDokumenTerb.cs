using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("M_DOKUMEN_TERB")]
public partial class MDokumenTerb
{
    [Key]
    [Column("ID_DOKUMEN")]
    [Precision(10)]
    public int IdDokumen { get; set; }

    [Column("IS_MANDATORY")]
    [Precision(2)]
    public byte IsMandatory { get; set; }

    [ForeignKey("IdDokumen")]
    [InverseProperty("MDokumenTerb")]
    public virtual MDokuman IdDokumenNavigation { get; set; } = null!;
}
