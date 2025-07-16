using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("TEMP_PERIKSA")]
public partial class TempPeriksa
{
    [Column("NOP")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("WAJIB_PAJAK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? WajibPajak { get; set; }

    [Column("ALAMAT")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Alamat { get; set; }

    [Column("UPTB")]
    [Precision(2)]
    public byte? Uptb { get; set; }

    [Column("NO_SP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoSp { get; set; }

    [Column("TGL_ST", TypeName = "DATE")]
    public DateTime? TglSt { get; set; }

    [Column("MASA_PAJAK")]
    [StringLength(50)]
    [Unicode(false)]
    public string? MasaPajak { get; set; }

    [Column("POKOK", TypeName = "NUMBER(15,2)")]
    public decimal? Pokok { get; set; }

    [Column("DENDA", TypeName = "NUMBER(15,2)")]
    public decimal? Denda { get; set; }

    [Column("JUMLAH_KB", TypeName = "NUMBER(15,2)")]
    public decimal? JumlahKb { get; set; }

    [Column("KET")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("LHP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Lhp { get; set; }

    [Column("TGL_LHP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TglLhp { get; set; }

    [Column("TGL_BYR", TypeName = "DATE")]
    public DateTime? TglByr { get; set; }

    [Column("TIM")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Tim { get; set; }

    [Column("JENIS_PAJAK")]
    [Precision(2)]
    public byte? JenisPajak { get; set; }

    [Column("TAHUN_PAJAK")]
    [Precision(4)]
    public byte? TahunPajak { get; set; }

    [Column("DASAR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Dasar { get; set; }
}
