using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbePbbBulanUptb
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal Bulan { get; set; }

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("SPPT_MURNI", TypeName = "NUMBER")]
    public decimal? SpptMurni { get; set; }

    [Column("KETETAPAN_MURNI", TypeName = "NUMBER")]
    public decimal? KetetapanMurni { get; set; }

    [Column("SPPT_TUNGGAKAN", TypeName = "NUMBER")]
    public decimal? SpptTunggakan { get; set; }

    [Column("KETETAPAN_TUNGGAKAN", TypeName = "NUMBER")]
    public decimal? KetetapanTunggakan { get; set; }

    [Column("REALISASI_MURNI", TypeName = "NUMBER")]
    public decimal? RealisasiMurni { get; set; }

    [Column("REALISASI_TUNGGAKAN", TypeName = "NUMBER")]
    public decimal? RealisasiTunggakan { get; set; }

    [Column("REALISASI_SPPT_MURNI", TypeName = "NUMBER")]
    public decimal? RealisasiSpptMurni { get; set; }

    [Column("REALISASI_SPPT_TUNGGAKAN", TypeName = "NUMBER")]
    public decimal? RealisasiSpptTunggakan { get; set; }
}
