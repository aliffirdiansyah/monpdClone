using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("T_PENUNGGUAN_SPTPD")]
public partial class TPenungguanSptpd
{
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("TAHUN_PAJAK")]
    [Precision(10)]
    public int TahunPajak { get; set; }

    [Column("MASA_PAJAK")]
    [Precision(10)]
    public int MasaPajak { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string Petugas { get; set; } = null!;

    [Column("KETERANGAN")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Column("TGL_MULAI", TypeName = "DATE")]
    public DateTime TglMulai { get; set; }

    [Column("TGL_SELESAI", TypeName = "DATE")]
    public DateTime TglSelesai { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [InverseProperty("IdNavigation")]
    public virtual ICollection<TPenungguanSptpdMamin> TPenungguanSptpdMamins { get; set; } = new List<TPenungguanSptpdMamin>();

    [InverseProperty("IdNavigation")]
    public virtual ICollection<TPenungguanSptpdParkir> TPenungguanSptpdParkirs { get; set; } = new List<TPenungguanSptpdParkir>();
}
