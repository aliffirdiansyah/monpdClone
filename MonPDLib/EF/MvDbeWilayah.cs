using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbeWilayah
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal? Uptb { get; set; }

    [Column("TARGET", TypeName = "NUMBER")]
    public decimal? Target { get; set; }

    [Column("KECAMATAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("KELURAHAN")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Kelurahan { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }

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

    [Column("TARGETALL", TypeName = "NUMBER")]
    public decimal? Targetall { get; set; }
}
