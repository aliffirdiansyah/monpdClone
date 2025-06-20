using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_ABT")]
public partial class OpAbt
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("PERUNTUKAN")]
    [Precision(10)]
    public int Peruntukan { get; set; }

    [Column("KATEGORI", TypeName = "NUMBER")]
    public decimal Kategori { get; set; }

    [Column("IS_METERAN_AIR", TypeName = "NUMBER")]
    public decimal IsMeteranAir { get; set; }

    [Column("JUMLAH_KARYAWAN", TypeName = "NUMBER")]
    public decimal JumlahKaryawan { get; set; }

    [ForeignKey("Kategori")]
    [InverseProperty("OpAbts")]
    public virtual MKategoriPajak KategoriNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("OpAbt")]
    public virtual Op NopNavigation { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpAbtJadwal> OpAbtJadwals { get; set; } = new List<OpAbtJadwal>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpAbtKapasita> OpAbtKapasita { get; set; } = new List<OpAbtKapasita>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpAbtKetetapan> OpAbtKetetapans { get; set; } = new List<OpAbtKetetapan>();
}
