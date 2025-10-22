using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("SuratKlasifikasi", "SuratAgenda", "SuratDokumen", "SuratBidang", "SuratPajak", "SuratOpd", "SuratTahun")]
[Table("T_SKPD_PRMN")]
public partial class TSkpdPrmn
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

    [Column("NPWPD")]
    [StringLength(30)]
    [Unicode(false)]
    public string Npwpd { get; set; } = null!;

    [Column("NAMA_WP")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaWp { get; set; } = null!;

    [Column("LETAK_REKLAME")]
    [Precision(10)]
    public int LetakReklame { get; set; }

    [Column("STATUS_TANAH")]
    [Precision(10)]
    public int StatusTanah { get; set; }

    [Column("LOK_PENYELENGGARAAN")]
    [StringLength(300)]
    [Unicode(false)]
    public string LokPenyelenggaraan { get; set; } = null!;

    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int IdJenisReklame { get; set; }

    [Column("JENIS_PRODUK")]
    [Precision(10)]
    public int JenisProduk { get; set; }

    [Column("PANJANG", TypeName = "NUMBER(10,2)")]
    public decimal Panjang { get; set; }

    [Column("LEBAR", TypeName = "NUMBER(10,2)")]
    public decimal Lebar { get; set; }

    [Column("TINGGI")]
    [Precision(10)]
    public int Tinggi { get; set; }

    [Column("KET_SISI")]
    [StringLength(200)]
    [Unicode(false)]
    public string KetSisi { get; set; } = null!;

    [Column("TGL_MULAI_BERLAKU", TypeName = "DATE")]
    public DateTime TglMulaiBerlaku { get; set; }

    [Column("TGL_SELESAI_BERLAKU", TypeName = "DATE")]
    public DateTime TglSelesaiBerlaku { get; set; }

    [Column("MATERI_REKLAME")]
    [StringLength(200)]
    [Unicode(false)]
    public string MateriReklame { get; set; } = null!;

    [Column("MASA_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string MasaPajak { get; set; } = null!;

    [Column("NILAI_NJOP", TypeName = "NUMBER(15,2)")]
    public decimal NilaiNjop { get; set; }

    [Column("TARIF_PAJAK", TypeName = "NUMBER(5,2)")]
    public decimal TarifPajak { get; set; }

    [Column("TARIF_ROKOK", TypeName = "NUMBER(5,2)")]
    public decimal TarifRokok { get; set; }

    [Column("NILAI_JAMBONG", TypeName = "NUMBER(15,2)")]
    public decimal NilaiJambong { get; set; }

    [Column("REFF_TAHUN_PEL")]
    [Precision(10)]
    public int? ReffTahunPel { get; set; }

    [Column("REFF_BULAN_PEL")]
    [Precision(10)]
    public int? ReffBulanPel { get; set; }

    [Column("REFF_SEQ_PEL")]
    [Precision(10)]
    public int? ReffSeqPel { get; set; }

    [Column("REFF_SEQ")]
    [Precision(10)]
    public int? ReffSeq { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("BATAL_STATUS")]
    [Precision(10)]
    public int BatalStatus { get; set; }

    [Column("BATAL_DATE", TypeName = "DATE")]
    public DateTime? BatalDate { get; set; }

    [Column("BATAL_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? BatalBy { get; set; }

    [Column("BATAL_KET")]
    [StringLength(255)]
    [Unicode(false)]
    public string? BatalKet { get; set; }

    [ForeignKey("IdJenisReklame")]
    [InverseProperty("TSkpdPrmns")]
    public virtual MJenisReklame IdJenisReklameNavigation { get; set; } = null!;

    [InverseProperty("Surat")]
    public virtual TSkpdPrmnPenetapan? TSkpdPrmnPenetapan { get; set; }
}
