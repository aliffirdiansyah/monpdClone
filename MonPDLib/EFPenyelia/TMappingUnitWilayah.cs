using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Keyless]
[Table("T_MAPPING_UNIT_WILAYAH")]
public partial class TMappingUnitWilayah
{
    [Column("ID_UNIT", TypeName = "NUMBER")]
    public decimal IdUnit { get; set; }

    [Column("SEQ", TypeName = "NUMBER")]
    public decimal? Seq { get; set; }

    [Column("UPTD")]
    [StringLength(100)]
    [Unicode(false)]
    public string Uptd { get; set; } = null!;

    [Column("KECAMATAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string Kecamatan { get; set; } = null!;

    [Column("KELURAHAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string Kelurahan { get; set; } = null!;

    [Column("TGL_MULAI", TypeName = "DATE")]
    public DateTime? TglMulai { get; set; }

    [Column("TGL_AKHIR", TypeName = "DATE")]
    public DateTime? TglAkhir { get; set; }

    [Column("INSERT_DATE", TypeName = "DATE")]
    public DateTime? InsertDate { get; set; }
}
