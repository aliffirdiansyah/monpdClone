using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel")]
[Table("T_PERMOHONAN")]
public partial class TPermohonan
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

    [Column("NPWPD_NO")]
    [StringLength(20)]
    [Unicode(false)]
    public string NpwpdNo { get; set; } = null!;

    [Column("TGL_PERMOHONAN", TypeName = "DATE")]
    public DateTime TglPermohonan { get; set; }

    [Column("STATUS_PERMOHONAN")]
    [Precision(10)]
    public int StatusPermohonan { get; set; }

    [Column("STATUS_PROSES")]
    [Precision(10)]
    public int StatusProses { get; set; }

    [Column("KAT_PENYELENGGARAAN")]
    [Precision(10)]
    public int KatPenyelenggaraan { get; set; }

    [Column("KD_CAMAT")]
    [StringLength(3)]
    [Unicode(false)]
    public string KdCamat { get; set; } = null!;

    [Column("KD_LURAH")]
    [StringLength(3)]
    [Unicode(false)]
    public string KdLurah { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("KdCamat, KdLurah")]
    [InverseProperty("TPermohonans")]
    public virtual MWilayah Kd { get; set; } = null!;

    [InverseProperty("TPermohonan")]
    public virtual ICollection<TPermohonanFile> TPermohonanFiles { get; set; } = new List<TPermohonanFile>();

    [InverseProperty("TPermohonan")]
    public virtual ICollection<TPermohonanIn> TPermohonanIns { get; set; } = new List<TPermohonanIn>();

    [InverseProperty("TPermohonan")]
    public virtual ICollection<TPermohonanPrmn> TPermohonanPrmns { get; set; } = new List<TPermohonanPrmn>();
}
