using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Keyless]
[Table("BOBOT_NOP")]
public partial class BobotNop
{
    [Column("ID_NOP", TypeName = "NUMBER")]
    public decimal? IdNop { get; set; }

    [Column("KODE_NOP")]
    [StringLength(20)]
    [Unicode(false)]
    public string KodeNop { get; set; } = null!;

    [Column("ID_WILAYAH", TypeName = "NUMBER")]
    public decimal IdWilayah { get; set; }

    [Column("NOMINAL_KETETAPAN")]
    [Precision(2)]
    public byte? NominalKetetapan { get; set; }

    [Column("MASA_NUNGGAK")]
    [Precision(2)]
    public byte? MasaNunggak { get; set; }

    [Column("SK_KURANG_BAYAR")]
    [Precision(2)]
    public byte? SkKurangBayar { get; set; }

    [Column("TOTAL_BOBOT_NOP")]
    [Precision(3)]
    public byte? TotalBobotNop { get; set; }

    [Column("SEQ")]
    [Precision(3)]
    public byte? Seq { get; set; }

    [Column("TAHUN")]
    [Precision(4)]
    public byte? Tahun { get; set; }

    [Column("PAJAK_ID")]
    [Precision(4)]
    public byte? PajakId { get; set; }

    [Column("NM_KECAMATAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NmKecamatan { get; set; }

    [Column("NM_KELURAHAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NmKelurahan { get; set; }
}
