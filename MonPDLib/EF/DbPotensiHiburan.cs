using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "TahunBuku")]
[Table("DB_POTENSI_HIBURAN")]
public partial class DbPotensiHiburan
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("JUMLAH_STUDIO")]
    [Precision(10)]
    public int? JumlahStudio { get; set; }

    [Column("KAP_KURSI_STUDIO")]
    [Precision(10)]
    public int? KapKursiStudio { get; set; }

    [Column("KAP_PENGUNJUNG")]
    [Precision(10)]
    public int? KapPengunjung { get; set; }

    [Column("HTM_WD", TypeName = "NUMBER(12,2)")]
    public decimal? HtmWd { get; set; }

    [Column("HTM_WE", TypeName = "NUMBER(12,2)")]
    public decimal? HtmWe { get; set; }

    [Column("HARGA_MEMBER_BULAN", TypeName = "NUMBER(12,2)")]
    public decimal? HargaMemberBulan { get; set; }

    [Column("TO_WD", TypeName = "NUMBER(12,2)")]
    public decimal? ToWd { get; set; }

    [Column("TO_WE", TypeName = "NUMBER(12,2)")]
    public decimal? ToWe { get; set; }

    [Column("AVG_VIS_WD", TypeName = "NUMBER(10,2)")]
    public decimal? AvgVisWd { get; set; }

    [Column("AVG_VIS_WE", TypeName = "NUMBER(10,2)")]
    public decimal? AvgVisWe { get; set; }

    [Column("AVG_MEMBER_BULAN", TypeName = "NUMBER(10,2)")]
    public decimal? AvgMemberBulan { get; set; }

    [Column("OMZET_BULAN", TypeName = "NUMBER(18,2)")]
    public decimal? OmzetBulan { get; set; }

    [Column("CREATED_AT", TypeName = "DATE")]
    public DateTime? CreatedAt { get; set; }

    [Column("UPDATED_AT", TypeName = "DATE")]
    public DateTime? UpdatedAt { get; set; }

    [Key]
    [Column("TAHUN_BUKU")]
    [Precision(10)]
    public int TahunBuku { get; set; }
}
