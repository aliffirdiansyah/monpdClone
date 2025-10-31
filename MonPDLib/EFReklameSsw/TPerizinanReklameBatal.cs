using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[PrimaryKey("NomorDaftar", "Tahun", "Seq")]
[Table("T_PERIZINAN_REKLAME_BATAL")]
public partial class TPerizinanReklameBatal
{
    [Key]
    [Column("NOMOR_DAFTAR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NomorDaftar { get; set; } = null!;

    [Column("STATUS", TypeName = "NUMBER")]
    public decimal Status { get; set; }

    [Column("KETERANGAN")]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Key]
    [Column("TAHUN")]
    [Precision(10)]
    public int Tahun { get; set; }

    [Column("TGL_BATAL", TypeName = "DATE")]
    public DateTime TglBatal { get; set; }

    [Key]
    [Column("SEQ", TypeName = "NUMBER(38)")]
    public decimal Seq { get; set; }

    [ForeignKey("NomorDaftar, Tahun")]
    [InverseProperty("TPerizinanReklameBatals")]
    public virtual TPerizinanReklame TPerizinanReklame { get; set; } = null!;
}
