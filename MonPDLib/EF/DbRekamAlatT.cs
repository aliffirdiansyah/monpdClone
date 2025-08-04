using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_REKAM_ALAT_TS")]
public partial class DbRekamAlatT
{
    [Column("KONDISI")]
    [StringLength(100)]
    [Unicode(false)]
    public string Kondisi { get; set; } = null!;

    [Column("KODEREKENING")]
    [StringLength(100)]
    [Unicode(false)]
    public string Koderekening { get; set; } = null!;

    [Key]
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("NAMAOP")]
    [StringLength(200)]
    [Unicode(false)]
    public string Namaop { get; set; } = null!;

    [Column("ALAMAT")]
    [StringLength(300)]
    [Unicode(false)]
    public string Alamat { get; set; } = null!;

    [Column("CREATE_DATE", TypeName = "DATE")]
    public DateTime? CreateDate { get; set; }

    [Column("NAMAREKENING")]
    [StringLength(100)]
    [Unicode(false)]
    public string Namarekening { get; set; } = null!;

    [Column("JENISUSAHA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Jenisusaha { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("LOCK_SPTPD")]
    [StringLength(50)]
    [Unicode(false)]
    public string LockSptpd { get; set; } = null!;

    [Column("OPEN_TS")]
    [StringLength(50)]
    [Unicode(false)]
    public string OpenTs { get; set; } = null!;

    [Column("TERPASANG")]
    [Precision(10)]
    public int Terpasang { get; set; }

    [Column("TERAKHIR_AKTIF", TypeName = "DATE")]
    public DateTime? TerakhirAktif { get; set; }

    [Column("HARI_INI", TypeName = "DATE")]
    public DateTime? HariIni { get; set; }
}
