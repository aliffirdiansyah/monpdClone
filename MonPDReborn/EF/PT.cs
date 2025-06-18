using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("P_TS")]
public partial class PT
{
    [Column("KONDISI")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Kondisi { get; set; }

    [Column("KODEREKENING")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Koderekening { get; set; }

    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("NAMAOP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Namaop { get; set; }

    [Column("ALAMAT")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Alamat { get; set; }

    [Column("CREATE_DATE", TypeName = "DATE")]
    public DateTime? CreateDate { get; set; }

    [Column("NAMAREKENING")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Namarekening { get; set; }

    [Column("JENISUSAHA")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Jenisusaha { get; set; }

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Column("LOCK_SPTPD")]
    [StringLength(5)]
    [Unicode(false)]
    public string? LockSptpd { get; set; }

    [Column("OPEN_TS")]
    [StringLength(5)]
    [Unicode(false)]
    public string? OpenTs { get; set; }

    [Column("TERPASANG")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Terpasang { get; set; }

    [Column("TERAKHIR_AKTIF")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TerakhirAktif { get; set; }

    [Column("HARI_INI")]
    [StringLength(20)]
    [Unicode(false)]
    public string? HariIni { get; set; }
}
