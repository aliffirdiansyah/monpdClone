using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_REKAM_ALAT_TBSB")]
public partial class DbRekamAlatTbsb
{
    [Key]
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("PAJAK_ID")]
    [Precision(10)]
    public int PajakId { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(200)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Column("ALAMAT_OBJEK")]
    [StringLength(200)]
    [Unicode(false)]
    public string AlamatObjek { get; set; } = null!;

    [Column("TGL_TERPASANG", TypeName = "DATE")]
    public DateTime TglTerpasang { get; set; }

    [Column("KETERANGAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string Keterangan { get; set; } = null!;
}
