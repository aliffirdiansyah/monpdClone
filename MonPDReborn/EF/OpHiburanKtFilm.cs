using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[PrimaryKey("Nop", "Seq")]
[Table("OP_HIBURAN_KT_FILM")]
public partial class OpHiburanKtFilm
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }

    [Column("NAMA_STUDIO")]
    [StringLength(130)]
    [Unicode(false)]
    public string NamaStudio { get; set; } = null!;

    [Column("JUMLAH_KURSI")]
    [Precision(10)]
    public int JumlahKursi { get; set; }

    [Column("TARIF_WEEKDAY")]
    [Precision(10)]
    public int TarifWeekday { get; set; }

    [Column("TARIF_WEEKEND")]
    [Precision(10)]
    public int TarifWeekend { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpHiburanKtFilms")]
    public virtual OpHiburan NopNavigation { get; set; } = null!;
}
