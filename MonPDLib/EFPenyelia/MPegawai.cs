using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Table("M_PEGAWAI")]
public partial class MPegawai
{
    [Key]
    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Column("BIDANG_ID")]
    [Precision(10)]
    public int BidangId { get; set; }

    [Column("JABATAN_ID")]
    [Precision(10)]
    public int JabatanId { get; set; }

    [Column("GOLONGAN_ID")]
    [Precision(10)]
    public int GolonganId { get; set; }

    [Column("NAMA")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("ALAMAT")]
    [StringLength(150)]
    [Unicode(false)]
    public string Alamat { get; set; } = null!;

    [Column("HP")]
    [StringLength(45)]
    [Unicode(false)]
    public string Hp { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [Column("TGL_LAHIR", TypeName = "DATE")]
    public DateTime TglLahir { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(45)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("KEL")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Kel { get; set; }

    [Column("KEC")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Kec { get; set; }

    [ForeignKey("BidangId")]
    [InverseProperty("MPegawais")]
    public virtual MBidang Bidang { get; set; } = null!;

    [ForeignKey("GolonganId")]
    [InverseProperty("MPegawais")]
    public virtual MGolongan Golongan { get; set; } = null!;

    [ForeignKey("JabatanId")]
    [InverseProperty("MPegawais")]
    public virtual MJabatan Jabatan { get; set; } = null!;

    [ForeignKey("Kec, Kel")]
    [InverseProperty("MPegawais")]
    public virtual MWilayah? Ke { get; set; }

    [InverseProperty("NipNavigation")]
    public virtual ICollection<MPegawaiOpDet> MPegawaiOpDets { get; set; } = new List<MPegawaiOpDet>();

    [InverseProperty("UsernameNavigation")]
    public virtual MUserLogin? MUserLogin { get; set; }

    [InverseProperty("NipNavigation")]
    public virtual ICollection<TAktifitasPegawai> TAktifitasPegawais { get; set; } = new List<TAktifitasPegawai>();
}
