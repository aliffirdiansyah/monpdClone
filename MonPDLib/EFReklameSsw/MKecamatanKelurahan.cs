using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[PrimaryKey("Uptb", "KdCamat", "KdLurah")]
[Table("M_KECAMATAN_KELURAHAN")]
public partial class MKecamatanKelurahan
{
    [Key]
    [Column("UPTB", TypeName = "NUMBER(38)")]
    public decimal Uptb { get; set; }

    [Key]
    [Column("KD_CAMAT")]
    [StringLength(30)]
    [Unicode(false)]
    public string KdCamat { get; set; } = null!;

    [Key]
    [Column("KD_LURAH")]
    [StringLength(500)]
    [Unicode(false)]
    public string KdLurah { get; set; } = null!;

    [Column("NAMA_KECAMATAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaKecamatan { get; set; } = null!;

    [Column("NAMA_KELURAHAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaKelurahan { get; set; } = null!;

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("MKecamatanKelurahan")]
    public virtual ICollection<TJadwalMobling> TJadwalMoblings { get; set; } = new List<TJadwalMobling>();
}
