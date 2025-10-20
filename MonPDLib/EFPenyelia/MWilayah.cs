using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[PrimaryKey("KdKecamatan", "KdKelurahan")]
[Table("M_WILAYAH")]
public partial class MWilayah
{
    [Column("UPTD")]
    [StringLength(1)]
    [Unicode(false)]
    public string Uptd { get; set; } = null!;

    [Key]
    [Column("KD_KECAMATAN")]
    [StringLength(3)]
    [Unicode(false)]
    public string KdKecamatan { get; set; } = null!;

    [Column("NM_KECAMATAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string NmKecamatan { get; set; } = null!;

    [Key]
    [Column("KD_KELURAHAN")]
    [StringLength(3)]
    [Unicode(false)]
    public string KdKelurahan { get; set; } = null!;

    [Column("NM_KELURAHAN")]
    [StringLength(25)]
    [Unicode(false)]
    public string NmKelurahan { get; set; } = null!;

    [Column("KELURAHAN_LAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? KelurahanLama { get; set; }

    [InverseProperty("Ke")]
    public virtual ICollection<MPegawai> MPegawais { get; set; } = new List<MPegawai>();
}
