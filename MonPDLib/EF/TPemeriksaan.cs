using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "TahunPajak", "MasaPajak", "Seq")]
[Table("T_PEMERIKSAAN")]
public partial class TPemeriksaan
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TAHUN_PAJAK")]
    [Precision(10)]
    public int TahunPajak { get; set; }

    [Key]
    [Column("MASA_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string MasaPajak { get; set; } = null!;

    [Column("NO_SP")]
    [StringLength(100)]
    [Unicode(false)]
    public string NoSp { get; set; } = null!;

    [Column("TGL_SP", TypeName = "DATE")]
    public DateTime TglSp { get; set; }

    [Column("POKOK", TypeName = "NUMBER")]
    public decimal Pokok { get; set; }

    [Column("DENDA", TypeName = "NUMBER")]
    public decimal Denda { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string Petugas { get; set; } = null!;

    [Column("KET")]
    [StringLength(250)]
    [Unicode(false)]
    public string Ket { get; set; } = null!;

    [Column("PAJAK_ID", TypeName = "NUMBER(38)")]
    public decimal PajakId { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(2)]
    public byte Seq { get; set; }

    [Column("JUMLAH_KB", TypeName = "NUMBER(15,2)")]
    public decimal? JumlahKb { get; set; }

    [Column("LHP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Lhp { get; set; }

    [Column("TGL_LHP", TypeName = "DATE")]
    public DateTime? TglLhp { get; set; }

    [Column("TGL_BYR", TypeName = "DATE")]
    public DateTime? TglByr { get; set; }
}
