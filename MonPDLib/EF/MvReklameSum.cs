using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvReklameSum
{
    [Column("ID_FLAG_PERMOHONAN", TypeName = "NUMBER")]
    public decimal? IdFlagPermohonan { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("TOTAL_SKPD", TypeName = "NUMBER")]
    public decimal? TotalSkpd { get; set; }

    [Column("TOTAL_NILAI", TypeName = "NUMBER")]
    public decimal? TotalNilai { get; set; }

    [Column("SKPD_BELUM_BAYAR", TypeName = "NUMBER")]
    public decimal? SkpdBelumBayar { get; set; }

    [Column("NILAI_BELUM_BAYAR", TypeName = "NUMBER")]
    public decimal? NilaiBelumBayar { get; set; }

    [Column("SKPD_PERPANJANG", TypeName = "NUMBER")]
    public decimal? SkpdPerpanjang { get; set; }

    [Column("NILAI_PERPANJANG", TypeName = "NUMBER")]
    public decimal? NilaiPerpanjang { get; set; }

    [Column("SKPD_BLM_PERPANJANG", TypeName = "NUMBER")]
    public decimal? SkpdBlmPerpanjang { get; set; }

    [Column("NILAI_BLM_PERPANJANG", TypeName = "NUMBER")]
    public decimal? NilaiBlmPerpanjang { get; set; }

    [Column("SKPD_BARU", TypeName = "NUMBER")]
    public decimal? SkpdBaru { get; set; }

    [Column("NILAI_BARU", TypeName = "NUMBER")]
    public decimal? NilaiBaru { get; set; }

    [Column("SKPD_BARU_BELUM_BAYAR", TypeName = "NUMBER")]
    public decimal? SkpdBaruBelumBayar { get; set; }

    [Column("NILAI_BARU_BELUM_BAYAR", TypeName = "NUMBER")]
    public decimal? NilaiBaruBelumBayar { get; set; }
}
