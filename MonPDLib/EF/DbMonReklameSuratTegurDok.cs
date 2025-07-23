using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Klasifikasi", "TahunSurat", "Pajak", "KodeDokumen", "Bidang", "Agenda")]
[Table("DB_MON_REKLAME_SURAT_TEGUR_DOK")]
public partial class DbMonReklameSuratTegurDok
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

    [Column("ISI_FILE", TypeName = "BLOB")]
    public byte[] IsiFile { get; set; } = null!;
}
