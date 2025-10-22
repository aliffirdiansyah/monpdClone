using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel", "Seq")]
[Table("T_PERMOHONAN_INS_NILAI")]
public partial class TPermohonanInsNilai
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

    [Column("PANJANG", TypeName = "NUMBER(10,2)")]
    public decimal Panjang { get; set; }

    [Column("LEBAR", TypeName = "NUMBER(10,2)")]
    public decimal Lebar { get; set; }

    [Column("JUMLAH_SATUAN", TypeName = "NUMBER(10,2)")]
    public decimal JumlahSatuan { get; set; }

    [Column("JUMLAH_PERULANGAN", TypeName = "NUMBER(10,2)")]
    public decimal JumlahPerulangan { get; set; }

    [Column("JUMLAH_LAYAR", TypeName = "NUMBER(10,2)")]
    public decimal JumlahLayar { get; set; }

    [Column("TGL_MULAI_BERLAKU", TypeName = "DATE")]
    public DateTime TglMulaiBerlaku { get; set; }

    [Column("TGL_SELESAI_BERLAKU", TypeName = "DATE")]
    public DateTime TglSelesaiBerlaku { get; set; }

    [Column("MATERI_REKLAME")]
    [StringLength(200)]
    [Unicode(false)]
    public string MateriReklame { get; set; } = null!;

    [ForeignKey("IdJenisReklame")]
    [InverseProperty("TPermohonanInsNilais")]
    public virtual MJenisReklame IdJenisReklameNavigation { get; set; } = null!;

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq")]
    [InverseProperty("TPermohonanInsNilai")]
    public virtual TPermohonanIn TPermohonanIn { get; set; } = null!;

    [InverseProperty("TPermohonanInsNilai")]
    public virtual ICollection<TPermohonanInsNilaiAct> TPermohonanInsNilaiActs { get; set; } = new List<TPermohonanInsNilaiAct>();
}
