using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "Seq")]
[Table("OP_LISTRIK_SUMBER_SENDIRI")]
public partial class OpListrikSumberSendiri
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Column("NAMA_GENSET")]
    [StringLength(30)]
    [Unicode(false)]
    public string NamaGenset { get; set; } = null!;

    [Column("ADA_METERAN")]
    [Precision(10)]
    public int AdaMeteran { get; set; }

    [Column("JUMLAH")]
    [Precision(10)]
    public int Jumlah { get; set; }

    [Column("DAYA_GENSET_KVA")]
    [Precision(10)]
    public int DayaGensetKva { get; set; }

    [Column("AVG_PENGGUNAAN_MNT_HARI")]
    [Precision(10)]
    public int AvgPenggunaanMntHari { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpListrikSumberSendiris")]
    public virtual OpListrik NopNavigation { get; set; } = null!;
}
