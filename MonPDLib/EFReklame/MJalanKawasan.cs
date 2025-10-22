using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

[PrimaryKey("IdJalan", "KawasanId", "KelasJalanId")]
[Table("M_JALAN_KAWASAN")]
public partial class MJalanKawasan
{
    [Key]
    [Column("ID_JALAN", TypeName = "NUMBER")]
    public decimal IdJalan { get; set; }

    [Key]
    [Column("KAWASAN_ID", TypeName = "NUMBER")]
    public decimal KawasanId { get; set; }

    [Key]
    [Column("KELAS_JALAN_ID", TypeName = "NUMBER")]
    public decimal KelasJalanId { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("IdJalan")]
    [InverseProperty("MJalanKawasans")]
    public virtual DrftMJalan IdJalanNavigation { get; set; } = null!;

    [ForeignKey("KawasanId, KelasJalanId")]
    [InverseProperty("MJalanKawasans")]
    public virtual MKelasJalan K { get; set; } = null!;
}
