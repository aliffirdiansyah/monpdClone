using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("PajakId", "Nop", "Bulan", "Tahun")]
[Table("DB_MON_UPAYA_PAD")]
public partial class DbMonUpayaPad
{
    [Key]
    [Column("PAJAK_ID")]
    [Precision(15)]
    public long PajakId { get; set; }

    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("BULAN")]
    [Precision(15)]
    public long Bulan { get; set; }

    [Key]
    [Column("TAHUN")]
    [Precision(15)]
    public long Tahun { get; set; }

    [Column("IS_HIMBAUAN")]
    [Precision(15)]
    public long IsHimbauan { get; set; }

    [Column("IS_PENYILANGAN")]
    [Precision(15)]
    public long IsPenyilangan { get; set; }

    [Column("IS_TEGURAN")]
    [Precision(15)]
    public long IsTeguran { get; set; }

    [Column("IS_PANGGILAN")]
    [Precision(15)]
    public long IsPanggilan { get; set; }

    [Column("IS_KEJAKSAAN")]
    [Precision(15)]
    public long IsKejaksaan { get; set; }

    [Column("IS_ANGSURAN")]
    [Precision(15)]
    public long IsAngsuran { get; set; }

    [Column("IS_BANTIP")]
    [Precision(15)]
    public long IsBantip { get; set; }

    [Column("IS_PEMBONGKARAN")]
    [Precision(15)]
    public long IsPembongkaran { get; set; }

    [Column("IS_RENCANA_TS")]
    [Precision(15)]
    public long IsRencanaTs { get; set; }

    [Column("IS_TS")]
    [Precision(15)]
    public long IsTs { get; set; }

    [Column("CREATE_DATE", TypeName = "DATE")]
    public DateTime? CreateDate { get; set; }

    [Column("CREATE_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string? CreateBy { get; set; }

    [Column("MODI_DATE", TypeName = "DATE")]
    public DateTime? ModiDate { get; set; }

    [Column("MODI_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ModiBy { get; set; }
}
