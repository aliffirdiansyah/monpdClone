using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[PrimaryKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "KodeOpd", "KodeSubOpd", "Bulan")]
[Table("T_TRANSAKSI_TEMP")]
public partial class TTransaksiTemp
{
    [Column("ID_TRANSAKSI", TypeName = "NUMBER")]
    public decimal IdTransaksi { get; set; }

    [Key]
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Key]
    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Key]
    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Kelompok { get; set; } = null!;

    [Key]
    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Key]
    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Key]
    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Key]
    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Key]
    [Column("KODE_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeOpd { get; set; } = null!;

    [Key]
    [Column("KODE_SUB_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeSubOpd { get; set; } = null!;

    [Key]
    [Column("BULAN")]
    [Precision(2)]
    public byte Bulan { get; set; }

    [Column("MASALAH")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? Masalah { get; set; }

    [Column("UPAYA")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? Upaya { get; set; }

    [Column("TND_LANJT")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? TndLanjt { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(13)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("UP_DATE", TypeName = "DATE")]
    public DateTime? UpDate { get; set; }

    [Column("UP_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string? UpBy { get; set; }

    [Column("ID_OPD")]
    [Precision(10)]
    public int IdOpd { get; set; }

    [ForeignKey("IdOpd")]
    [InverseProperty("TTransaksiTemps")]
    public virtual MOpd IdOpdNavigation { get; set; } = null!;
}
