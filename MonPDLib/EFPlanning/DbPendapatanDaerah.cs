using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[Keyless]
[Table("DB_PENDAPATAN_DAERAH")]
[Index("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "KodeOpd", "KodeSubOpd", "Bulan", Name = "UK_PENDAPATAN_9KODE", IsUnique = true)]
public partial class DbPendapatanDaerah
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("NAMA_AKUN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaAkun { get; set; } = null!;

    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Kelompok { get; set; } = null!;

    [Column("NAMA_KELOMPOK")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaKelompok { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("NAMA_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("NAMA_OBJEK")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("NAMA_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaRincian { get; set; } = null!;

    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaSubRincian { get; set; } = null!;

    [Column("KODE_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeOpd { get; set; } = null!;

    [Column("NAMA_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaOpd { get; set; } = null!;

    [Column("KODE_SUB_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeSubOpd { get; set; } = null!;

    [Column("NAMA_SUB_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaSubOpd { get; set; } = null!;

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal? Realisasi { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(13)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
