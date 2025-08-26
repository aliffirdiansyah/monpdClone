using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "Tahun", "Masapajak", "Seq")]
[Table("TMP_OBJEK_PAJAK_SKPD_ABT")]
public partial class TmpObjekPajakSkpdAbt
{
    [Key]
    [Column("NOP")]
    [StringLength(20)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TAHUN")]
    [Precision(4)]
    public byte Tahun { get; set; }

    [Key]
    [Column("MASAPAJAK")]
    [Precision(2)]
    public byte Masapajak { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(4)]
    public byte Seq { get; set; }

    [Column("KELOMPOK_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? KelompokNama { get; set; }

    [Column("VOL_PENGGUNAAN_AIR", TypeName = "NUMBER(18,2)")]
    public decimal? VolPenggunaanAir { get; set; }
}
