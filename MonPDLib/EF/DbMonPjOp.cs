using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nip", "Nop")]
[Table("DB_MON_PJ_OP")]
public partial class DbMonPjOp
{
    [Key]
    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("PAJAK_ID")]
    [Precision(10)]
    public int PajakId { get; set; }

    [Column("KATEGORI_ID")]
    [Precision(10)]
    public int KategoriId { get; set; }

    [ForeignKey("Nip")]
    [InverseProperty("DbMonPjOps")]
    public virtual MPegawai NipNavigation { get; set; } = null!;
}
