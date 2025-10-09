using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class CobaRealisasi
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("NOP")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("POKOK_PAJAK", TypeName = "NUMBER")]
    public decimal? PokokPajak { get; set; }

    [Column("TANGGAL_BAYAR", TypeName = "DATE")]
    public DateTime? TanggalBayar { get; set; }

    [Column("BAYAR")]
    [StringLength(6)]
    [Unicode(false)]
    public string? Bayar { get; set; }
}
