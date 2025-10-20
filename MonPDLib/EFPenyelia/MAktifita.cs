using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Table("M_AKTIFITAS")]
public partial class MAktifita
{
    [Key]
    [Column("ID_AKTIFITAS")]
    [Precision(18)]
    public long IdAktifitas { get; set; }

    [Column("NAMA_AKTIFITAS")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaAktifitas { get; set; }

    [Column("KET")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [InverseProperty("IdAktifitasNavigation")]
    public virtual ICollection<MKeterangan> MKeterangans { get; set; } = new List<MKeterangan>();

    [InverseProperty("IdAktifitasNavigation")]
    public virtual ICollection<TAktifitasPegawai> TAktifitasPegawais { get; set; } = new List<TAktifitasPegawai>();
}
