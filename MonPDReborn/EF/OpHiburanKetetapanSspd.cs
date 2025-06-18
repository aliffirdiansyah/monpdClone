using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[PrimaryKey("Nop", "Tahun", "Masapajak", "Seq", "IdSspd")]
[Table("OP_HIBURAN_KETETAPAN_SSPD")]
public partial class OpHiburanKetetapanSspd
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TAHUN")]
    [Precision(10)]
    public int Tahun { get; set; }

    [Key]
    [Column("MASAPAJAK")]
    [Precision(10)]
    public int Masapajak { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Key]
    [Column("ID_SSPD")]
    [StringLength(100)]
    [Unicode(false)]
    public string IdSspd { get; set; } = null!;

    [Column("TGL_BAYAR", TypeName = "DATE")]
    public DateTime TglBayar { get; set; }

    [Column("NAMA_JENIS_PEMBAYARAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaJenisPembayaran { get; set; } = null!;

    [Column("NOMINAL_POKOK")]
    [Precision(10)]
    public int NominalPokok { get; set; }

    [Column("NOMINAL_SANKSI_BAYAR")]
    [Precision(10)]
    public int NominalSanksiBayar { get; set; }

    [Column("NOMINAL_KENAIKAN")]
    [Precision(10)]
    public int NominalKenaikan { get; set; }

    [Column("AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(10)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("OBJEK")]
    [StringLength(10)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("SUB_RINCIAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(20)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("Nop, Tahun, Masapajak, Seq")]
    [InverseProperty("OpHiburanKetetapanSspds")]
    public virtual OpHiburanKetetapan OpHiburanKetetapan { get; set; } = null!;
}
