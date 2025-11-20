using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[PrimaryKey("Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "KodeOpd", "KodeSubOpd", "Seq", "Tanggal")]
[Table("T_INPUT_MANUAL")]
public partial class TInputManual
{
    [Key]
    [Column("AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("NAMA_AKUN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaAkun { get; set; } = null!;

    [Key]
    [Column("KELOMPOK")]
    [StringLength(10)]
    [Unicode(false)]
    public string Kelompok { get; set; } = null!;

    [Column("NAMA_KELOMPOK")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaKelompok { get; set; } = null!;

    [Key]
    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("NAMA_JENIS")]
    [StringLength(300)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Key]
    [Column("OBJEK")]
    [StringLength(30)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("NAMA_OBJEK")]
    [StringLength(300)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Key]
    [Column("RINCIAN")]
    [StringLength(40)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("NAMA_RINCIAN")]
    [StringLength(300)]
    [Unicode(false)]
    public string NamaRincian { get; set; } = null!;

    [Key]
    [Column("SUB_RINCIAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(400)]
    [Unicode(false)]
    public string NamaSubRincian { get; set; } = null!;

    [Key]
    [Column("KODE_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeOpd { get; set; } = null!;

    [Column("NAMA_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaOpd { get; set; } = null!;

    [Key]
    [Column("KODE_SUB_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeSubOpd { get; set; } = null!;

    [Column("NAMA_SUB_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaSubOpd { get; set; } = null!;

    [Key]
    [Column("TANGGAL", TypeName = "DATE")]
    public DateTime Tanggal { get; set; }

    [Column("REALISASI", TypeName = "NUMBER(18,2)")]
    public decimal? Realisasi { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }
}
