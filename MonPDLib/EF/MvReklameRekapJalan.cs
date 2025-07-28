using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvReklameRekapJalan
{
    [Column("ID_FLAG_PERMOHONAN", TypeName = "NUMBER")]
    public decimal? IdFlagPermohonan { get; set; }

    [Column("KELAS_JALAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KelasJalan { get; set; }

    [Column("NAMA_JALAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJalan { get; set; }

    [Column("JUMLAH_TOTAL", TypeName = "NUMBER")]
    public decimal? JumlahTotal { get; set; }

    [Column("AKTIF", TypeName = "NUMBER")]
    public decimal? Aktif { get; set; }

    [Column("EXPIRE", TypeName = "NUMBER")]
    public decimal? Expire { get; set; }

    [Column("PERPANJANGAN", TypeName = "NUMBER")]
    public decimal? Perpanjangan { get; set; }

    [Column("WAJIB_BONGKAR", TypeName = "NUMBER")]
    public decimal? WajibBongkar { get; set; }

    [Column("BONGKAR", TypeName = "NUMBER")]
    public decimal? Bongkar { get; set; }

    [Column("BLM_BONGKAR", TypeName = "NUMBER")]
    public decimal? BlmBongkar { get; set; }
}
