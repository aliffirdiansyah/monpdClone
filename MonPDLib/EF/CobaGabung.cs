using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class CobaGabung
{
    [Column("NOP")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("TAHUN_PAJAK", TypeName = "NUMBER")]
    public decimal? TahunPajak { get; set; }

    [Column("KETETAPAN", TypeName = "NUMBER")]
    public decimal? Ketetapan { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("TANGGAL_BAYAR", TypeName = "DATE")]
    public DateTime? TanggalBayar { get; set; }

    [Column("REALISASI", TypeName = "NUMBER")]
    public decimal? Realisasi { get; set; }

    [Column("STATUS_LUNAS")]
    [StringLength(11)]
    [Unicode(false)]
    public string? StatusLunas { get; set; }

    [Column("BAYAR")]
    [StringLength(6)]
    [Unicode(false)]
    public string? Bayar { get; set; }

    [Column("KATAGORI")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Katagori { get; set; }

    [Column("WILAYAH")]
    [StringLength(45)]
    [Unicode(false)]
    public string? Wilayah { get; set; }

    [Column("KECAMATAN")]
    [StringLength(9)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("KELURAHAN")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Kelurahan { get; set; }

    [Column("NPWPD")]
    [StringLength(13)]
    [Unicode(false)]
    public string? Npwpd { get; set; }

    [Column("NAMA_WP")]
    [StringLength(12)]
    [Unicode(false)]
    public string? NamaWp { get; set; }

    [Column("ALAMAT_WP")]
    [StringLength(72)]
    [Unicode(false)]
    public string? AlamatWp { get; set; }

    [Column("KONTAK")]
    [StringLength(9)]
    [Unicode(false)]
    public string? Kontak { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(34)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("L_BUMI", TypeName = "NUMBER")]
    public decimal? LBumi { get; set; }

    [Column("L_BANGUNAN", TypeName = "NUMBER")]
    public decimal? LBangunan { get; set; }

    [Column("L_BUMI_BERSAMA", TypeName = "NUMBER")]
    public decimal? LBumiBersama { get; set; }

    [Column("L_BANGUNAN_BERSAMA", TypeName = "NUMBER")]
    public decimal? LBangunanBersama { get; set; }

    [Column("SEGMENT_WP")]
    [StringLength(15)]
    [Unicode(false)]
    public string? SegmentWp { get; set; }

    [Column("PETUGAS")]
    [StringLength(16)]
    [Unicode(false)]
    public string? Petugas { get; set; }

    [Column("UPAYA")]
    [StringLength(13)]
    [Unicode(false)]
    public string? Upaya { get; set; }
}
