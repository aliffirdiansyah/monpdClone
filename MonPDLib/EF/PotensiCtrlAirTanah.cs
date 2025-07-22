using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "KdPajak")]
[Table("POTENSI_CTRL_AIR_TANAH")]
public partial class PotensiCtrlAirTanah
{
    [Key]
    [Column("NOP")]
    [StringLength(23)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("KD_PAJAK")]
    [Precision(10)]
    public int KdPajak { get; set; }

    [Column("STATUS")]
    [Precision(10)]
    public int Status { get; set; }

    [Column("JENIS")]
    [Precision(10)]
    public int Jenis { get; set; }

    [Column("VOL_PENGGUNAAN", TypeName = "NUMBER(10,2)")]
    public decimal VolPenggunaan { get; set; }

    [Column("POTENSI_PAJAK_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal? PotensiPajakBulan { get; set; }

    [Column("POTENSI_PAJAK_TAHUN", TypeName = "NUMBER(18,2)")]
    public decimal? PotensiPajakTahun { get; set; }

    [Column("CREATED_AT")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    [Precision(6)]
    public DateTime? UpdatedAt { get; set; }
}
