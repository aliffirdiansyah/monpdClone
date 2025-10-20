using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Table("M_KETERANGAN")]
public partial class MKeterangan
{
    [Key]
    [Column("ID")]
    [Precision(18)]
    public long Id { get; set; }

    [Column("ID_AKTIFITAS")]
    [Precision(18)]
    public long? IdAktifitas { get; set; }

    [Column("KETERANGAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [ForeignKey("IdAktifitas")]
    [InverseProperty("MKeterangans")]
    public virtual MAktifita? IdAktifitasNavigation { get; set; }
}
