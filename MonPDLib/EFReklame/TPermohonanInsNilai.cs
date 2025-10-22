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
    [Precision(4)]
    public byte TahunPel { get; set; }

    [Key]
    [Column("BULAN_PEL")]
    [Precision(2)]
    public byte BulanPel { get; set; }

    [Key]
    [Column("SEQ_PEL")]
    [Precision(10)]
    public int SeqPel { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Column("LETA_KREKLAME")]
    [Precision(2)]
    public byte? LetaKreklame { get; set; }

    [Column("STATUS_TANAH")]
    [Precision(2)]
    public byte? StatusTanah { get; set; }

    [Column("LOK_PENYELENGGARAAN")]
    [StringLength(300)]
    [Unicode(false)]
    public string? LokPenyelenggaraan { get; set; }

    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int? IdJenisReklame { get; set; }

    [Column("JENIS_PRODUK")]
    [Precision(2)]
    public byte? JenisProduk { get; set; }

    [Column("PANJANG")]
    [Precision(10)]
    public int? Panjang { get; set; }

    [Column("LEBAR")]
    [Precision(10)]
    public int? Lebar { get; set; }

    [Column("JUMLAH_SATUAN")]
    [Precision(10)]
    public int? JumlahSatuan { get; set; }

    [Column("JUMLAH_PERULANGAN")]
    [Precision(10)]
    public int? JumlahPerulangan { get; set; }

    [Column("JUMLAH_LAYAR")]
    [Precision(10)]
    public int? JumlahLayar { get; set; }

    [Column("TGL_MULAI_BERLAKU", TypeName = "DATE")]
    public DateTime? TglMulaiBerlaku { get; set; }

    [Column("TGL_SELESAI_BERLAKU", TypeName = "DATE")]
    public DateTime? TglSelesaiBerlaku { get; set; }

    [Column("MATERI_REKLAME")]
    [StringLength(200)]
    [Unicode(false)]
    public string? MateriReklame { get; set; }

    [ForeignKey("IdJenisReklame")]
    [InverseProperty("TPermohonanInsNilais")]
    public virtual MJenisReklame? IdJenisReklameNavigation { get; set; }

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq")]
    [InverseProperty("TPermohonanInsNilai")]
    public virtual TPermohonanIn TPermohonanIn { get; set; } = null!;

    [InverseProperty("TPermohonanInsNilai")]
    public virtual ICollection<TPermohonanInsNilaiAct> TPermohonanInsNilaiActs { get; set; } = new List<TPermohonanInsNilaiAct>();
}
