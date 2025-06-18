using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("OP_LISTRIK_SUMBER_LAIN")]
public partial class OpListrikSumberLain
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("ASAL_SUMBER")]
    [StringLength(250)]
    [Unicode(false)]
    public string AsalSumber { get; set; } = null!;

    [Column("DAYA_GENSET_KVA")]
    [Precision(10)]
    public int DayaGensetKva { get; set; }

    [Column("AVG_PENGGUNAAN_MNT_HARI")]
    [Precision(10)]
    public int AvgPenggunaanMntHari { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpListrikSumberLain")]
    public virtual OpListrik NopNavigation { get; set; } = null!;
}
