using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "TahunBuku")]
[Table("DB_POTENSI_RESTO")]
public partial class DbPotensiResto
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KAP_KURSI")]
    [Precision(10)]
    public int? KapKursi { get; set; }

    [Column("KAP_TENANT_CATERING")]
    [Precision(10)]
    public int? KapTenantCatering { get; set; }

    [Column("AVG_BILL_ORG", TypeName = "NUMBER(12,2)")]
    public decimal? AvgBillOrg { get; set; }

    [Column("TURNOVER_WD", TypeName = "NUMBER(12,2)")]
    public decimal? TurnoverWd { get; set; }

    [Column("TURNOVER_WE", TypeName = "NUMBER(12,2)")]
    public decimal? TurnoverWe { get; set; }

    [Column("AVG_VIS_WD", TypeName = "NUMBER(10,2)")]
    public decimal? AvgVisWd { get; set; }

    [Column("AVG_VIS_WE", TypeName = "NUMBER(10,2)")]
    public decimal? AvgVisWe { get; set; }

    [Column("AVG_TENAT_CAT_WD", TypeName = "NUMBER(10,2)")]
    public decimal? AvgTenatCatWd { get; set; }

    [Column("AVG_TENAT_CAT_WE", TypeName = "NUMBER(10,2)")]
    public decimal? AvgTenatCatWe { get; set; }

    [Column("OMZET", TypeName = "NUMBER(18,2)")]
    public decimal? Omzet { get; set; }

    [Column("CREATED_AT", TypeName = "DATE")]
    public DateTime? CreatedAt { get; set; }

    [Column("UPDATED_AT", TypeName = "DATE")]
    public DateTime? UpdatedAt { get; set; }

    [Key]
    [Column("TAHUN_BUKU")]
    [Precision(10)]
    public int TahunBuku { get; set; }

    [Column("POTENSI_PAJAK_TAHUN", TypeName = "NUMBER(30)")]
    public decimal? PotensiPajakTahun { get; set; }
}
