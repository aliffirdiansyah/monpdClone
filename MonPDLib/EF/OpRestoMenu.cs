using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "IdSeqMenu")]
[Table("OP_RESTO_MENU")]
public partial class OpRestoMenu
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("ID_SEQ_MENU")]
    [Precision(10)]
    public int IdSeqMenu { get; set; }

    [Column("IS_MAKANAN")]
    [Precision(10)]
    public int IsMakanan { get; set; }

    [Column("KET_MENU")]
    [StringLength(100)]
    [Unicode(false)]
    public string KetMenu { get; set; } = null!;

    [Column("HARGA_TERTINGGI")]
    [Precision(10)]
    public int HargaTertinggi { get; set; }

    [Column("HARGA_TERENDAH")]
    [Precision(10)]
    public int HargaTerendah { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpRestoMenus")]
    public virtual OpResto NopNavigation { get; set; } = null!;
}
