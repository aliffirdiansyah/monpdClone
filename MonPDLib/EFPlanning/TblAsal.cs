using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

[Table("TBL_ASAL")]
public partial class TblAsal
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("NILAI", TypeName = "NUMBER")]
    public decimal? Nilai { get; set; }

    [Column("TANGGAL_UPDATE", TypeName = "DATE")]
    public DateTime? TanggalUpdate { get; set; }
}
