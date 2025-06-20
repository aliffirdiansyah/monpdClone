using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "Seq")]
[Table("OP_HIBURAN_KT_OTHER")]
public partial class OpHiburanKtOther
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }

    [Column("SARANA")]
    [StringLength(250)]
    [Unicode(false)]
    public string Sarana { get; set; } = null!;

    [Column("JUMLAH")]
    [Precision(10)]
    public int Jumlah { get; set; }

    [Column("KAPASITAS")]
    [Precision(10)]
    public int Kapasitas { get; set; }

    [Column("TARIF_WEEKDAY")]
    [Precision(10)]
    public int TarifWeekday { get; set; }

    [Column("TARIF_WEEKEND")]
    [Precision(10)]
    public int TarifWeekend { get; set; }

    [Column("TARIF_MEMBERSHIP")]
    [Precision(10)]
    public int TarifMembership { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpHiburanKtOthers")]
    public virtual OpHiburan NopNavigation { get; set; } = null!;
}
