using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("SuratKlasifikasi", "SuratAgenda", "SuratDokumen", "SuratBidang", "SuratPajak", "SuratOpd", "SuratTahun")]
[Table("T_SKPD_PRMN_PENETAPAN")]
public partial class TSkpdPrmnPenetapan
{
    [Key]
    [Column("SURAT_KLASIFIKASI")]
    [StringLength(10)]
    [Unicode(false)]
    public string SuratKlasifikasi { get; set; } = null!;

    [Key]
    [Column("SURAT_AGENDA")]
    [StringLength(10)]
    [Unicode(false)]
    public string SuratAgenda { get; set; } = null!;

    [Key]
    [Column("SURAT_DOKUMEN")]
    [StringLength(10)]
    [Unicode(false)]
    public string SuratDokumen { get; set; } = null!;

    [Key]
    [Column("SURAT_BIDANG")]
    [StringLength(10)]
    [Unicode(false)]
    public string SuratBidang { get; set; } = null!;

    [Key]
    [Column("SURAT_PAJAK")]
    [StringLength(10)]
    [Unicode(false)]
    public string SuratPajak { get; set; } = null!;

    [Key]
    [Column("SURAT_OPD")]
    [StringLength(10)]
    [Unicode(false)]
    public string SuratOpd { get; set; } = null!;

    [Key]
    [Column("SURAT_TAHUN")]
    [Precision(10)]
    public int SuratTahun { get; set; }

    [Column("TAHUN")]
    [Precision(10)]
    public int? Tahun { get; set; }

    [Column("MASAPAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Masapajak { get; set; }

    [Column("SEQ")]
    [Precision(10)]
    public int? Seq { get; set; }

    [Column("NIK_PEJABAT")]
    [StringLength(30)]
    [Unicode(false)]
    public string? NikPejabat { get; set; }

    [Column("NAMA_PEJABAT")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaPejabat { get; set; }

    [Column("GOLONGAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Golongan { get; set; }

    [Column("JABATAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Jabatan { get; set; }

    [Column("TGL_PENETAPAN", TypeName = "DATE")]
    public DateTime? TglPenetapan { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("SuratKlasifikasi, SuratAgenda, SuratDokumen, SuratBidang, SuratPajak, SuratOpd, SuratTahun")]
    [InverseProperty("TSkpdPrmnPenetapan")]
    public virtual TSkpdPrmn Surat { get; set; } = null!;
}
