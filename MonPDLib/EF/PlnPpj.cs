using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("PLN_PPJ")]
public partial class PlnPpj
{
    [Column("IDPEL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Idpel { get; set; }

    [Column("TARIF")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Tarif { get; set; }

    [Column("DAYA", TypeName = "NUMBER")]
    public decimal? Daya { get; set; }

    [Column("SUMBER")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Sumber { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("PEMAKAIAN_KWH", TypeName = "NUMBER")]
    public decimal? PemakaianKwh { get; set; }

    [Column("TAGIHAN", TypeName = "NUMBER")]
    public decimal? Tagihan { get; set; }

    [Column("GOLONGAN_PEL_PLN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? GolonganPelPln { get; set; }

    [Column("GOLONGAN_PEL_PLN_DESKRIPSI")]
    [StringLength(100)]
    [Unicode(false)]
    public string? GolonganPelPlnDeskripsi { get; set; }

    [Column("CREATED_AT")]
    [Precision(6)]
    public DateTime? CreatedAt { get; set; }
}
