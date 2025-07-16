using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("TahunBuku", "Nop", "MasaPajak", "TahunPajak")]
[Table("T_PIUTANG_REKLAME")]
public partial class TPiutangReklame
{
    [Key]
    [Column("TAHUN_BUKU")]
    [Precision(4)]
    public byte TahunBuku { get; set; }

    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("MASA_PAJAK")]
    [Precision(2)]
    public byte MasaPajak { get; set; }

    [Key]
    [Column("TAHUN_PAJAK")]
    [Precision(4)]
    public byte TahunPajak { get; set; }

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaOp { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string AlamatOp { get; set; } = null!;

    [Column("KECAMATAN_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string KecamatanOp { get; set; } = null!;

    [Column("KELURAHAN_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string KelurahanOp { get; set; } = null!;

    [Column("WILAYAH_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string WilayahPajak { get; set; } = null!;

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("NPWPD")]
    [StringLength(20)]
    [Unicode(false)]
    public string Npwpd { get; set; } = null!;

    [Column("NAMA_WP")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaWp { get; set; } = null!;

    [Column("ALAMAT_WP")]
    [StringLength(150)]
    [Unicode(false)]
    public string AlamatWp { get; set; } = null!;

    [Column("PIUTANG", TypeName = "NUMBER(15,2)")]
    public decimal Piutang { get; set; }
}
