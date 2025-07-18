using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("NoBukti", "TgBukti", "KdRinci", "KdKegiatan", "IdSubkegiatan", "KdOrganisasi", "Kode", "NoDokumen")]
[Table("T_SERIES_PENDAPATAN")]
public partial class TSeriesPendapatan
{
    [Key]
    [Column("NO_BUKTI")]
    [StringLength(150)]
    [Unicode(false)]
    public string NoBukti { get; set; } = null!;

    [Key]
    [Column("TG_BUKTI", TypeName = "DATE")]
    public DateTime TgBukti { get; set; }

    [Key]
    [Column("KD_RINCI")]
    [StringLength(150)]
    [Unicode(false)]
    public string KdRinci { get; set; } = null!;

    [Key]
    [Column("KD_KEGIATAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string KdKegiatan { get; set; } = null!;

    [Key]
    [Column("ID_SUBKEGIATAN", TypeName = "NUMBER")]
    public decimal IdSubkegiatan { get; set; }

    [Key]
    [Column("KD_ORGANISASI")]
    [StringLength(150)]
    [Unicode(false)]
    public string KdOrganisasi { get; set; } = null!;

    [Key]
    [Column("KODE")]
    [StringLength(10)]
    [Unicode(false)]
    public string Kode { get; set; } = null!;

    [Key]
    [Column("NO_DOKUMEN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NoDokumen { get; set; } = null!;

    [Column("URAIAN", TypeName = "CLOB")]
    public string? Uraian { get; set; }

    [Column("JUMLAH", TypeName = "NUMBER(30,2)")]
    public decimal? Jumlah { get; set; }

    [Column("TAHUN", TypeName = "NUMBER(38)")]
    public decimal? Tahun { get; set; }

    [Column("INSERT_DATE", TypeName = "DATE")]
    public DateTime? InsertDate { get; set; }

    [Column("INSERT_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string? InsertBy { get; set; }
}
