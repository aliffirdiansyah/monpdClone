using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("T_OP_PARKIR_LOCATION")]
public partial class TOpParkirLocation
{
    [Column("FK_NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? FkNop { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("ALAMAT")]
    [StringLength(255)]
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
}
