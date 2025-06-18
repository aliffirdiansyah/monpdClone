using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("T_TEGURAN_SPTPD")]
public partial class TTeguranSptpd
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

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

    [Column("TGL_UPAYA", TypeName = "DATE")]
    public DateTime TglUpaya { get; set; }

    [Column("KATEGORI_UPAYA_ID", TypeName = "NUMBER")]
    public decimal KategoriUpayaId { get; set; }

    [Column("KETERANGAN")]
    [StringLength(2000)]
    [Unicode(false)]
    public string Keterangan { get; set; } = null!;

    [Column("JANJI_LAPOR", TypeName = "DATE")]
    public DateTime? JanjiLapor { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;
}
