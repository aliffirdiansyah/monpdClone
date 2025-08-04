using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DB_REKAM_ALAT_TBSB")]
public partial class DbRekamAlatTbsb
{
    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("PAJAK_ID")]
    [Precision(10)]
    public int PajakId { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Column("ALAMAT_OBJEK")]
    [StringLength(100)]
    [Unicode(false)]
    public string AlamatObjek { get; set; } = null!;

    [Column("KETERANGAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string Keterangan { get; set; } = null!;
}
