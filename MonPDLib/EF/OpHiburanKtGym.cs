using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_HIBURAN_KT_GYM")]
public partial class OpHiburanKtGym
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KAPASITAS_HARI")]
    [Precision(10)]
    public int KapasitasHari { get; set; }

    [Column("TARIF_INSIDENTIL")]
    [Precision(10)]
    public int TarifInsidentil { get; set; }

    [Column("TARIF_MEMBERSHIP")]
    [Precision(10)]
    public int TarifMembership { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpHiburanKtGym")]
    public virtual OpHiburan NopNavigation { get; set; } = null!;
}
