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

    [Column("NPWPD_NO")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NpwpdNo { get; set; }

    [Column("TGL_PERMOHONAN", TypeName = "DATE")]
    public DateTime? TglPermohonan { get; set; }

    [Column("JENIS_PERMOHONAN")]
    [Precision(2)]
    public byte JenisPermohonan { get; set; }

    [Column("STATUS_PERMOHONAN")]
    [Precision(2)]
    public byte StatusPermohonan { get; set; }

    [Column("STATUS_PROSES")]
    [Precision(2)]
    public byte StatusProses { get; set; }

    [Column("KAT_PENYELENGGARAAN")]
    [Precision(9)]
    public int? KatPenyelenggaraan { get; set; }

    [Column("KD_CAMAT")]
    [Precision(10)]
    public int? KdCamat { get; set; }

    [Column("KD_LURAH")]
    [Precision(10)]
    public int? KdLurah { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("TPermohonan")]
    public virtual ICollection<TPermohonanFile> TPermohonanFiles { get; set; } = new List<TPermohonanFile>();

    [InverseProperty("TPermohonan")]
    public virtual ICollection<TPermohonanIn> TPermohonanIns { get; set; } = new List<TPermohonanIn>();
}
