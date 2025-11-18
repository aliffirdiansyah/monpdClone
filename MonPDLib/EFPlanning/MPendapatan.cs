using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[PrimaryKey("SubRincian", "KodeOpd", "KodeSubOpd")]
[Table("M_PENDAPATAN")]
public partial class MPendapatan
{
    [Column("AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Akun { get; set; }

    [Column("NAMA_AKUN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaAkun { get; set; }

    [Column("KELOMPOK")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Kelompok { get; set; }

    [Column("NAMA_KELOMPOK")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Column("NAMA_JENIS")]
    [StringLength(300)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

    [Column("OBJEK")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Objek { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(300)]
    [Unicode(false)]
    public string? NamaObjek { get; set; }

    [Column("RINCIAN")]
    [StringLength(40)]
    [Unicode(false)]
    public string? Rincian { get; set; }

    [Column("NAMA_RINCIAN")]
    [StringLength(300)]
    [Unicode(false)]
    public string? NamaRincian { get; set; }

    [Key]
    [Column("SUB_RINCIAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(400)]
    [Unicode(false)]
    public string? NamaSubRincian { get; set; }

    [Column("UPDATE_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UpdateBy { get; set; }

    [Column("UPDATE_DATE", TypeName = "DATE")]
    public DateTime? UpdateDate { get; set; }

    [Key]
    [Column("KODE_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeOpd { get; set; } = null!;

    [Column("NAMA_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaOpd { get; set; }

    [Key]
    [Column("KODE_SUB_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeSubOpd { get; set; } = null!;

    [Column("NAMA_SUB_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaSubOpd { get; set; }
}
