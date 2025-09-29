using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nor", "TanggalSkSilang", "TanggalBongkar", "TanggalBantib")]
[Table("DB_MON_REKLAME_LIAR")]
public partial class DbMonReklameLiar
{
    [Key]
    [Column("NOR")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nor { get; set; } = null!;

    [Column("NO_FORM")]
    [StringLength(30)]
    [Unicode(false)]
    public string? NoForm { get; set; }

    [Column("SK_SILANG")]
    [StringLength(5)]
    [Unicode(false)]
    public string? SkSilang { get; set; }

    [Column("NOMOR_SK_SILANG")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NomorSkSilang { get; set; }

    [Key]
    [Column("TANGGAL_SK_SILANG", TypeName = "DATE")]
    public DateTime TanggalSkSilang { get; set; }

    [Column("SK_BONGKAR")]
    [StringLength(5)]
    [Unicode(false)]
    public string? SkBongkar { get; set; }

    [Column("NOMOR_SK_BONGKAR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NomorSkBongkar { get; set; }

    [Column("TANGGAL_SK_BONGKAR", TypeName = "DATE")]
    public DateTime? TanggalSkBongkar { get; set; }

    [Column("BANTIB")]
    [StringLength(5)]
    [Unicode(false)]
    public string? Bantib { get; set; }

    [Column("NOMOR_SURAT_BANTIB")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NomorSuratBantib { get; set; }

    [Key]
    [Column("TANGGAL_BANTIB", TypeName = "DATE")]
    public DateTime TanggalBantib { get; set; }

    [Column("KELAS_JALAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KelasJalan { get; set; }

    [Column("LOKASI")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Lokasi { get; set; }

    [Column("ALAMAT_PENEMPATAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? AlamatPenempatan { get; set; }

    [Column("JENIS")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Key]
    [Column("TANGGAL_BONGKAR", TypeName = "DATE")]
    public DateTime TanggalBongkar { get; set; }

    [Column("TANGGAL_BAYAR", TypeName = "DATE")]
    public DateTime? TanggalBayar { get; set; }
}
