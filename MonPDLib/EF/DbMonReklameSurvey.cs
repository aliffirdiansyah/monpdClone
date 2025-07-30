using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_MON_REKLAME_SURVEY")]
public partial class DbMonReklameSurvey
{
    [Column("NOR")]
    [StringLength(12)]
    [Unicode(false)]
    public string? Nor { get; set; }

    [Column("IDTRANS_SUVEY")]
    [StringLength(13)]
    [Unicode(false)]
    public string IdtransSuvey { get; set; } = null!;

    [Column("SURVEYOR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Surveyor { get; set; }

    [Column("TGL_SURVEY", TypeName = "DATE")]
    public DateTime? TglSurvey { get; set; }

    [Column("VERIFIKATOR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Verifikator { get; set; }

    [Column("TGL_VERIF", TypeName = "DATE")]
    public DateTime? TglVerif { get; set; }

    [Column("NILAI_VERIF", TypeName = "NUMBER")]
    public decimal? NilaiVerif { get; set; }

    [Column("IS_BARU", TypeName = "NUMBER")]
    public decimal? IsBaru { get; set; }

    [Column("IS_TUTUP", TypeName = "NUMBER")]
    public decimal? IsTutup { get; set; }

    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("KODE_OBYEK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KodeObyek { get; set; }

    [Column("NO_SKPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoSkpd { get; set; }

    [Column("TGL_SKPD", TypeName = "DATE")]
    public DateTime? TglSkpd { get; set; }

    [Column("NILAI_PAJAK", TypeName = "NUMBER")]
    public decimal? NilaiPajak { get; set; }

    [Key]
    [Column("SEQ", TypeName = "NUMBER(38)")]
    public decimal Seq { get; set; }
}
