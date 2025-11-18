using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[PrimaryKey("IdOpd", "TahunBuku", "KodeOpd", "SubRincian", "Seq")]
[Table("T_TRANSAKSI")]
public partial class TTransaksi
{
    [Key]
    [Column("ID_OPD")]
    [Precision(10)]
    public int IdOpd { get; set; }

    [Key]
    [Column("TAHUN_BUKU")]
    [Precision(10)]
    public int TahunBuku { get; set; }

    [Key]
    [Column("KODE_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeOpd { get; set; } = null!;

    [Key]
    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("BULAN")]
    [Precision(10)]
    public int? Bulan { get; set; }

    [Column("UPAYA")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? Upaya { get; set; }

    [Column("MASALAH")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? Masalah { get; set; }

    [Column("TND_LANJT")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? TndLanjt { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("UP_DATE", TypeName = "DATE")]
    public DateTime? UpDate { get; set; }

    [Column("UP_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UpBy { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [ForeignKey("IdOpd")]
    [InverseProperty("TTransaksis")]
    public virtual MOpd IdOpdNavigation { get; set; } = null!;
}
