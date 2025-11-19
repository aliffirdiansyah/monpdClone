using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwTManualPbp
{
    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Akun { get; set; }

    [Column("NAMA_AKUN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaAkun { get; set; }

    [Column("KELOMPOK")]
    [StringLength(20)]
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
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Objek { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaObjek { get; set; }

    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Rincian { get; set; }

    [Column("NAMA_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaRincian { get; set; }

    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincian { get; set; }

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaSubRincian { get; set; }

    [Column("KODE_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KodeOpd { get; set; }

    [Column("NAMA_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaOpd { get; set; }

    [Column("KODE_SUB_OPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KodeSubOpd { get; set; }

    [Column("NAMA_SUB_OPD")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaSubOpd { get; set; }
}
