using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("PEN_TEGURAN_BAYAR")]
public partial class PenTeguranBayar
{
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaOp { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string AlamatOp { get; set; } = null!;

    [Column("TAHUN_PAJAK")]
    [Precision(10)]
    public int TahunPajak { get; set; }

    [Column("MASA_PAJAK")]
    [Precision(10)]
    public int MasaPajak { get; set; }

    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string Petugas { get; set; } = null!;

    [Column("JENIS_KETETAPAN", TypeName = "NUMBER(38)")]
    public decimal JenisKetetapan { get; set; }

    [Column("TGL_TEGURAN", TypeName = "DATE")]
    public DateTime TglTeguran { get; set; }

    [Column("NOMINAL_POKOK")]
    [Precision(10)]
    public int NominalPokok { get; set; }

    [Column("NOMINAL_BUNGA")]
    [Precision(10)]
    public int NominalBunga { get; set; }

    [Column("NOMINAL_DENDA_ADM")]
    [Precision(10)]
    public int NominalDendaAdm { get; set; }

    [Column("STATUS_KIRIM", TypeName = "NUMBER")]
    public decimal StatusKirim { get; set; }

    [Column("JANJI_BAYAR", TypeName = "DATE")]
    public DateTime? JanjiBayar { get; set; }

    [Column("PENERIMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Penerima { get; set; }

    [Column("KETERANGAN")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Keterangan { get; set; }

    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }
}
