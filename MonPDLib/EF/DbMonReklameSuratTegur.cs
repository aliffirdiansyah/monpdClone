using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Klasifikasi", "TahunSurat", "Pajak", "KodeDokumen", "Bidang", "Agenda")]
[Table("DB_MON_REKLAME_SURAT_TEGUR")]
public partial class DbMonReklameSuratTegur
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

    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("NAMA_PERUSAHAAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaPerusahaan { get; set; } = null!;

    [Column("TGL_AKHIR_BERLAKU", TypeName = "DATE")]
    public DateTime? TglAkhirBerlaku { get; set; }

    [Column("TGL_JT_TEMPO", TypeName = "DATE")]
    public DateTime? TglJtTempo { get; set; }

    [Column("FLAG_PERMOHONAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string FlagPermohonan { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("JUMLAH_NILAI", TypeName = "NUMBER(18,2)")]
    public decimal JumlahNilai { get; set; }

    [Column("ISI_REKLAME")]
    [StringLength(500)]
    [Unicode(false)]
    public string IsiReklame { get; set; } = null!;

    [Column("ALAMATREKLAME")]
    [StringLength(500)]
    [Unicode(false)]
    public string Alamatreklame { get; set; } = null!;

    [Column("PANJANG", TypeName = "NUMBER(10,2)")]
    public decimal Panjang { get; set; }

    [Column("LEBAR", TypeName = "NUMBER(10,2)")]
    public decimal Lebar { get; set; }

    [Column("LS", TypeName = "NUMBER(10,2)")]
    public decimal Ls { get; set; }

    [Column("KETINGGIAN", TypeName = "NUMBER(10,2)")]
    public decimal Ketinggian { get; set; }

    [Column("NAMA_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Column("TAHUN_PAJAK", TypeName = "NUMBER(38)")]
    public decimal TahunPajak { get; set; }

    [Column("MASA1", TypeName = "DATE")]
    public DateTime? Masa1 { get; set; }

    [Column("MASA2", TypeName = "DATE")]
    public DateTime? Masa2 { get; set; }

    [Column("PAJAKLB", TypeName = "NUMBER(18,2)")]
    public decimal Pajaklb { get; set; }
}
