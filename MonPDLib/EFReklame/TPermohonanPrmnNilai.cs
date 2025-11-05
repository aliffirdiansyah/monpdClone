using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel", "Seq")]
[Table("T_PERMOHONAN_PRMN_NILAI")]
public partial class TPermohonanPrmnNilai
{
    [Key]
    [Column("TAHUN_PEL")]
    [Precision(10)]
    public int TahunPel { get; set; }

    [Key]
    [Column("BULAN_PEL")]
    [Precision(10)]
    public int BulanPel { get; set; }

    [Key]
    [Column("SEQ_PEL")]
    [Precision(10)]
    public int SeqPel { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

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

    [Column("PANJANG")]
    [Precision(10)]
    public int Panjang { get; set; }

    [Column("LEBAR")]
    [Precision(10)]
    public int Lebar { get; set; }

    [Column("TINGGI")]
    [Precision(10)]
    public int Tinggi { get; set; }

    [Column("SUDUT_PANDANG")]
    [Precision(10)]
    public int SudutPandang { get; set; }

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

    [Column("STATUS_TERPASANG")]
    [Precision(10)]
    public int StatusTerpasang { get; set; }

    [Column("EST_TGL_PASANG", TypeName = "DATE")]
    public DateTime? EstTglPasang { get; set; }

    [Column("AKTUAL_PASANG", TypeName = "DATE")]
    public DateTime? AktualPasang { get; set; }

    [ForeignKey("IdJenisReklame")]
    [InverseProperty("TPermohonanPrmnNilais")]
    public virtual MJenisReklame IdJenisReklameNavigation { get; set; } = null!;

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq")]
    [InverseProperty("TPermohonanPrmnNilai")]
    public virtual TPermohonanPrmn TPermohonanPrmn { get; set; } = null!;

    [InverseProperty("TPermohonanPrmnNilai")]
    public virtual ICollection<TPermohonanPrmnNilaiAct> TPermohonanPrmnNilaiActs { get; set; } = new List<TPermohonanPrmnNilaiAct>();
}
