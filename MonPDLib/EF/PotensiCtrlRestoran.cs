using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "KdPajak")]
[Table("POTENSI_CTRL_RESTORAN")]
public partial class PotensiCtrlRestoran
{
    [Key]
    [Column("NOP")]
    [StringLength(23)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("KD_PAJAK")]
    [Precision(1)]
    public bool KdPajak { get; set; }

    [Required]
    [Column("STATUS")]
    [Precision(1)]
    public bool? Status { get; set; }

    [Column("JENIS")]
    [Precision(2)]
    public byte Jenis { get; set; }

    [Column("KAP_KURSI")]
    [Precision(10)]
    public int KapKursi { get; set; }

    [Column("KAP_TENANT_CATERING")]
    [Precision(10)]
    public int KapTenantCatering { get; set; }

    [Column("AVG_BILL_ORG", TypeName = "NUMBER(10,2)")]
    public decimal AvgBillOrg { get; set; }

    [Column("TURNOVER_WD", TypeName = "NUMBER(10,2)")]
    public decimal TurnoverWd { get; set; }

    [Column("TURNOVER_WE", TypeName = "NUMBER(10,2)")]
    public decimal TurnoverWe { get; set; }

    [Column("AVG_VIS_WD")]
    [Precision(10)]
    public int AvgVisWd { get; set; }

    [Column("AVG_VIS_WE")]
    [Precision(10)]
    public int AvgVisWe { get; set; }

    [Column("AVG_TENAT_CAT_WD")]
    [Precision(10)]
    public int AvgTenatCatWd { get; set; }

    [Column("AVG_TENAT_CAT_WE")]
    [Precision(10)]
    public int AvgTenatCatWe { get; set; }

    [Column("OMZET_DINEIN", TypeName = "NUMBER(18,2)")]
    public decimal OmzetDinein { get; set; }

    [Column("OMZET_TENAT_CATERING", TypeName = "NUMBER(18,2)")]
    public decimal OmzetTenatCatering { get; set; }

    [Column("OMZET_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal OmzetBulan { get; set; }

    [Column("POTENSI_PAJAK_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal PotensiPajakBulan { get; set; }

    [Column("POTENSI_PAJAK_TAHUN", TypeName = "NUMBER(18,2)")]
    public decimal PotensiPajakTahun { get; set; }

    [Column("CREATED_AT")]
    [Precision(6)]
    public DateTime CreatedAt { get; set; }

    [Column("UPDATED_AT")]
    [Precision(6)]
    public DateTime? UpdatedAt { get; set; }
}
