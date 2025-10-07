using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("FkNop", "PajakId")]
[Table("DB_OP_LOCATION")]
public partial class DbOpLocation
{
    [Key]
    [Column("FK_NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string FkNop { get; set; } = null!;

    [Column("NAMA")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("ALAMAT")]
    [StringLength(350)]
    [Unicode(false)]
    public string? Alamat { get; set; }

    [Column("LATITUDE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Latitude { get; set; }

    [Column("LONGITUDE")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Longitude { get; set; }

    [Key]
    [Column("PAJAK_ID", TypeName = "NUMBER(38)")]
    public decimal PajakId { get; set; }
}
