using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Mutasi", "TahunBuku")]
[Table("T_MUTASI_PIUTANG")]
public partial class TMutasiPiutang
{
    [Key]
    [Column("MUTASI")]
    [StringLength(100)]
    [Unicode(false)]
    public string Mutasi { get; set; } = null!;

    [Key]
    [Column("TAHUN_BUKU")]
    [StringLength(4)]
    [Unicode(false)]
    public string TahunBuku { get; set; } = null!;

    [Column("STATUS")]
    [Precision(1)]
    public bool Status { get; set; }

    [Column("NILAI", TypeName = "NUMBER(20,2)")]
    public decimal Nilai { get; set; }
}
