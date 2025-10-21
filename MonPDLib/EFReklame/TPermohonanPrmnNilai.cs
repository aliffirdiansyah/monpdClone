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

    [Column("LETAK_REKLAME")]
    [Precision(2)]
    public byte? LetakReklame { get; set; }

    [Column("STATUS_TANAH")]
    [Precision(2)]
    public byte? StatusTanah { get; set; }

    [Column("LOK_PENYELENGGARAAN")]
    [StringLength(300)]
    [Unicode(false)]
    public string? LokPenyelenggaraan { get; set; }

    [Column("DET_PENYELENGGARAAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string? DetPenyelenggaraan { get; set; }

    [Column("ID_JENIS_REKLAME")]
    [Precision(10)]
    public int? IdJenisReklame { get; set; }

    [Column("SUDUT_PANDANG")]
    [Precision(10)]
    public int? SudutPandang { get; set; }

    [Column("JENIS_PRODUK")]
    [Precision(2)]
    public byte? JenisProduk { get; set; }

    [Column("PANJANG")]
    [Precision(10)]
    public int? Panjang { get; set; }

    [Column("LEBAR")]
    [Precision(10)]
    public int? Lebar { get; set; }

    [Column("TINGGI")]
    [Precision(10)]
    public int? Tinggi { get; set; }

    [Column("TGL_MULAI_BERLAKU", TypeName = "DATE")]
    public DateTime? TglMulaiBerlaku { get; set; }

    [Column("TGL_SELESAI_BERLAKU", TypeName = "DATE")]
    public DateTime? TglSelesaiBerlaku { get; set; }

    [Column("MATERI_REKLAME")]
    [StringLength(200)]
    [Unicode(false)]
    public string? MateriReklame { get; set; }
}
