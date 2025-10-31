using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("T_JADWAL_MOBLING")]
public partial class TJadwalMobling
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("KD_CAMAT")]
    [StringLength(30)]
    [Unicode(false)]
    public string KdCamat { get; set; } = null!;

    [Column("KD_LURAH")]
    [StringLength(500)]
    [Unicode(false)]
    public string KdLurah { get; set; } = null!;

    [Column("KECAMATAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string Kecamatan { get; set; } = null!;

    [Column("KELURAHAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string Kelurahan { get; set; } = null!;

    [Column("LOKASI")]
    [StringLength(150)]
    [Unicode(false)]
    public string Lokasi { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("TGL_MOBLING", TypeName = "DATE")]
    public DateTime TglMobling { get; set; }

    [ForeignKey("Uptb, KdCamat, KdLurah")]
    [InverseProperty("TJadwalMoblings")]
    public virtual MKecamatanKelurahan MKecamatanKelurahan { get; set; } = null!;
}
