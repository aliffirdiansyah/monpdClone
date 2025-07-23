using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Agenda", "Bidang", "Klasifikasi", "KodeDokumen", "Pajak", "TahunSurat")]
[Table("DB_MON_REKLAME_SURAT")]
public partial class DbMonReklameSurat
{
    [Key]
    [Column("KLASIFIKASI")]
    [StringLength(20)]
    [Unicode(false)]
    public string Klasifikasi { get; set; } = null!;

    [Key]
    [Column("TAHUN_SURAT", TypeName = "NUMBER")]
    public decimal TahunSurat { get; set; }

    [Key]
    [Column("PAJAK", TypeName = "NUMBER")]
    public decimal Pajak { get; set; }

    [Key]
    [Column("KODE_DOKUMEN")]
    [StringLength(5)]
    [Unicode(false)]
    public string KodeDokumen { get; set; } = null!;

    [Key]
    [Column("BIDANG")]
    [StringLength(5)]
    [Unicode(false)]
    public string Bidang { get; set; } = null!;

    [Key]
    [Column("AGENDA", TypeName = "NUMBER")]
    public decimal Agenda { get; set; }

    [Column("NO_SURAT")]
    [StringLength(60)]
    [Unicode(false)]
    public string NoSurat { get; set; } = null!;

    [Column("TGL_SURAT", TypeName = "DATE")]
    public DateTime TglSurat { get; set; }

    [Column("NAMA")]
    [StringLength(250)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("ALAMAT")]
    [StringLength(250)]
    [Unicode(false)]
    public string Alamat { get; set; } = null!;

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal Status { get; set; }

    [Column("REFF_BATAL")]
    [StringLength(250)]
    [Unicode(false)]
    public string? ReffBatal { get; set; }

    [Column("KETERANGAN")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Column("NAMA_PEJABAT")]
    [StringLength(250)]
    [Unicode(false)]
    public string NamaPejabat { get; set; } = null!;

    [Column("GOLONGAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string Golongan { get; set; } = null!;

    [Column("JABATAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string Jabatan { get; set; } = null!;

    [Column("TAG_PENCARIAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string TagPencarian { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
