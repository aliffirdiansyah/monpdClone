using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_PEGAWAI")]
public partial class MPegawai
{
    [Key]
    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("BIDANG", TypeName = "NUMBER")]
    public decimal Bidang { get; set; }

    [Column("JABATAN", TypeName = "NUMBER")]
    public decimal Jabatan { get; set; }

    [Column("TINGKAT", TypeName = "NUMBER")]
    public decimal Tingkat { get; set; }

    [Column("KET")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("AKTIF", TypeName = "NUMBER")]
    public decimal Aktif { get; set; }

    [ForeignKey("Bidang")]
    [InverseProperty("MPegawais")]
    public virtual MUptb BidangNavigation { get; set; } = null!;

    [ForeignKey("Jabatan")]
    [InverseProperty("MPegawais")]
    public virtual MJabatan JabatanNavigation { get; set; } = null!;

    [InverseProperty("NipNavigation")]
    public virtual MPegawaiFoto? MPegawaiFoto { get; set; }
}
