using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("KawasanId", "KelasJalanId")]
[Table("M_KELAS_JALAN")]
public partial class MKelasJalan
{
    [Key]
    [Column("KAWASAN_ID", TypeName = "NUMBER")]
    public decimal KawasanId { get; set; }

    [Key]
    [Column("KELAS_JALAN_ID", TypeName = "NUMBER")]
    public decimal KelasJalanId { get; set; }

    [Column("NAMA_KELAS_JALAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaKelasJalan { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [InverseProperty("K")]
    public virtual ICollection<MJalanKawasan> MJalanKawasans { get; set; } = new List<MJalanKawasan>();
}
