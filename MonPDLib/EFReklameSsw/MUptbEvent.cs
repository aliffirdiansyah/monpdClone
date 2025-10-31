using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_UPTB_EVENT")]
public partial class MUptbEvent
{
    [Key]
    [Column("ID", TypeName = "NUMBER(38)")]
    public decimal Id { get; set; }

    [Column("UPTB", TypeName = "NUMBER(38)")]
    public decimal Uptb { get; set; }

    [Column("TANGGAL_AWAL", TypeName = "DATE")]
    public DateTime TanggalAwal { get; set; }

    [Column("TANGGAL_SAMPAI", TypeName = "DATE")]
    public DateTime TanggalSampai { get; set; }

    [Column("NAMA")]
    [StringLength(500)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("DESKRIPSI")]
    [StringLength(1000)]
    [Unicode(false)]
    public string Deskripsi { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("IdEventNavigation")]
    public virtual MUptbEventGambar? MUptbEventGambar { get; set; }

    [ForeignKey("Uptb")]
    [InverseProperty("MUptbEvents")]
    public virtual MUptb UptbNavigation { get; set; } = null!;
}
