using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "Id")]
[Table("OP_ABT_KAPASITAS")]
public partial class OpAbtKapasita
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("ID")]
    [Precision(10)]
    public int Id { get; set; }

    [Column("MERK_JENIS_POMPA")]
    [StringLength(250)]
    [Unicode(false)]
    public string MerkJenisPompa { get; set; } = null!;

    [Column("JUMLAH", TypeName = "NUMBER")]
    public decimal Jumlah { get; set; }

    [Column("DAYA_POMPA", TypeName = "NUMBER")]
    public decimal DayaPompa { get; set; }

    [Column("RATA_PENGGUNAAN_MENIT", TypeName = "NUMBER")]
    public decimal RataPenggunaanMenit { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpAbtKapasita")]
    public virtual OpAbt NopNavigation { get; set; } = null!;
}
