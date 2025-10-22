using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[Table("M_KAWASAN")]
public partial class MKawasan
{
    [Key]
    [Column("KAWASAN_ID")]
    [Precision(10)]
    public int KawasanId { get; set; }

    [Column("NAMA_KAWASAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaKawasan { get; set; } = null!;
}
