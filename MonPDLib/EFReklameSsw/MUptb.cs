using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_UPTB")]
public partial class MUptb
{
    [Key]
    [Column("UPTB", TypeName = "NUMBER(38)")]
    public decimal Uptb { get; set; }

    [Column("NAMA")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("ALAMAT")]
    [StringLength(500)]
    [Unicode(false)]
    public string Alamat { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("TELP")]
    [StringLength(250)]
    [Unicode(false)]
    public string Telp { get; set; } = null!;

    [Column("DESKRIPSI")]
    [StringLength(500)]
    [Unicode(false)]
    public string Deskripsi { get; set; } = null!;

    [Column("LONG_LAT")]
    [StringLength(500)]
    [Unicode(false)]
    public string LongLat { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("BidangNavigation")]
    public virtual ICollection<MPegawai> MPegawais { get; set; } = new List<MPegawai>();

    [InverseProperty("UptbNavigation")]
    public virtual ICollection<MUptbEvent> MUptbEvents { get; set; } = new List<MUptbEvent>();

    [InverseProperty("UptbNavigation")]
    public virtual ICollection<MUptbFungsi> MUptbFungsis { get; set; } = new List<MUptbFungsi>();

    [InverseProperty("UptbNavigation")]
    public virtual ICollection<MUptbGalery> MUptbGaleries { get; set; } = new List<MUptbGalery>();

    [InverseProperty("UptbNavigation")]
    public virtual ICollection<MUptbMisi> MUptbMisis { get; set; } = new List<MUptbMisi>();

    [InverseProperty("UptbNavigation")]
    public virtual ICollection<MUptbMitra> MUptbMitras { get; set; } = new List<MUptbMitra>();

    [InverseProperty("UptbNavigation")]
    public virtual ICollection<MUptbPeran> MUptbPerans { get; set; } = new List<MUptbPeran>();

    [InverseProperty("UptbNavigation")]
    public virtual MUptbStruktur? MUptbStruktur { get; set; }

    [InverseProperty("UptbNavigation")]
    public virtual ICollection<MUptbVisi> MUptbVisis { get; set; } = new List<MUptbVisi>();
}
