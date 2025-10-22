using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("TahunPel", "BulanPel", "SeqPel", "Seq")]
[Table("T_PERMOHONAN_INS_PENELITIAN")]
public partial class TPermohonanInsPenelitian
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

    [Column("ID_JALAN")]
    [Precision(10)]
    public int IdJalan { get; set; }

    [Column("LATITUDE")]
    [StringLength(50)]
    [Unicode(false)]
    public string Latitude { get; set; } = null!;

    [Column("LONGITUDE")]
    [StringLength(50)]
    [Unicode(false)]
    public string Longitude { get; set; } = null!;

    [Column("KD_CAMAT")]
    [StringLength(3)]
    [Unicode(false)]
    public string KdCamat { get; set; } = null!;

    [Column("KD_LURAH")]
    [StringLength(3)]
    [Unicode(false)]
    public string KdLurah { get; set; } = null!;

    [Column("TGL_PENELITIAN", TypeName = "DATE")]
    public DateTime TglPenelitian { get; set; }

    [ForeignKey("IdJalan")]
    [InverseProperty("TPermohonanInsPenelitians")]
    public virtual MJalan IdJalanNavigation { get; set; } = null!;

    [ForeignKey("KdCamat, KdLurah")]
    [InverseProperty("TPermohonanInsPenelitians")]
    public virtual MWilayah Kd { get; set; } = null!;

    [ForeignKey("TahunPel, BulanPel, SeqPel, Seq")]
    [InverseProperty("TPermohonanInsPenelitian")]
    public virtual TPermohonanIn TPermohonanIn { get; set; } = null!;
}
