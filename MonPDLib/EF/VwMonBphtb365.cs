using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwMonBphtb365
{
    [Column("TANGGAL_LABEL")]
    [StringLength(6)]
    [Unicode(false)]
    public string? TanggalLabel { get; set; }

    [Column("HARI", TypeName = "NUMBER")]
    public decimal? Hari { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("REALISASI_TMIN3", TypeName = "NUMBER")]
    public decimal? RealisasiTmin3 { get; set; }

    [Column("REALISASI_TMIN2", TypeName = "NUMBER")]
    public decimal? RealisasiTmin2 { get; set; }

    [Column("REALISASI_TMIN1", TypeName = "NUMBER")]
    public decimal? RealisasiTmin1 { get; set; }

    [Column("REALISASI_TMIN0", TypeName = "NUMBER")]
    public decimal? RealisasiTmin0 { get; set; }

    [Column("AVG_3T", TypeName = "NUMBER")]
    public decimal? Avg3t { get; set; }

    [Column("SUM_AVG_3T", TypeName = "NUMBER")]
    public decimal? SumAvg3t { get; set; }

    [Column("SUM_TMIN3", TypeName = "NUMBER")]
    public decimal? SumTmin3 { get; set; }

    [Column("SUM_TMIN2", TypeName = "NUMBER")]
    public decimal? SumTmin2 { get; set; }

    [Column("SUM_TMIN1", TypeName = "NUMBER")]
    public decimal? SumTmin1 { get; set; }

    [Column("KOEF", TypeName = "NUMBER")]
    public decimal? Koef { get; set; }

    [Column("AVGT", TypeName = "NUMBER")]
    public decimal? Avgt { get; set; }

    [Column("MAXT", TypeName = "NUMBER")]
    public decimal? Maxt { get; set; }

    [Column("MINT", TypeName = "NUMBER")]
    public decimal? Mint { get; set; }

    [Column("PREDIKSI_A", TypeName = "NUMBER")]
    public decimal? PrediksiA { get; set; }

    [Column("PROYEKSI_FINAL", TypeName = "NUMBER")]
    public decimal? ProyeksiFinal { get; set; }

    [Column("SELISIH_AVGT", TypeName = "NUMBER")]
    public decimal? SelisihAvgt { get; set; }

    [Column("SISA_PRE", TypeName = "NUMBER")]
    public decimal? SisaPre { get; set; }

    [Column("FIX_PREDIKSI", TypeName = "NUMBER")]
    public decimal? FixPrediksi { get; set; }

    [Column("SUM_FIX_PREDIKSI_BULANAN", TypeName = "NUMBER")]
    public decimal? SumFixPrediksiBulanan { get; set; }

    [Column("ID_PAJAK", TypeName = "NUMBER")]
    public decimal? IdPajak { get; set; }

    [Column("JENIS_PAJAK")]
    [StringLength(5)]
    [Unicode(false)]
    public string? JenisPajak { get; set; }
}
