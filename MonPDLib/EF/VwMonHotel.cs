using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwMonHotel
{
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("TAHUN_KETETAPAN", TypeName = "NUMBER")]
    public decimal? TahunKetetapan { get; set; }

    [Column("BULAN_KETETAPAN", TypeName = "NUMBER")]
    public decimal? BulanKetetapan { get; set; }

    [Column("JUMLAH_TRANSAKSI", TypeName = "NUMBER")]
    public decimal? JumlahTransaksi { get; set; }

    [Column("TANGGAL_BAYAR", TypeName = "NUMBER")]
    public decimal? TanggalBayar { get; set; }

    [Column("BULAN_BAYAR", TypeName = "NUMBER")]
    public decimal? BulanBayar { get; set; }

    [Column("TAHUN_BAYAR", TypeName = "NUMBER")]
    public decimal? TahunBayar { get; set; }

    [Column("KETETAPAN", TypeName = "NUMBER")]
    public decimal? Ketetapan { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal? Realisasi { get; set; }
}
