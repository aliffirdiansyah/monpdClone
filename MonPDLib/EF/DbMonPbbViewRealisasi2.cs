using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbMonPbbViewRealisasi2
{
    [Column("TAHUN_PAJAK", TypeName = "NUMBER")]
    public decimal TahunPajak { get; set; }

    [Column("NOP")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string KategoriNama { get; set; } = null!;

    [Column("NAMA_WP")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaWp { get; set; } = null!;

    [Column("ALAMAT_WP")]
    [StringLength(244)]
    [Unicode(false)]
    public string? AlamatWp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(205)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("RT")]
    [StringLength(100)]
    [Unicode(false)]
    public string Rt { get; set; } = null!;

    [Column("RW")]
    [StringLength(100)]
    [Unicode(false)]
    public string Rw { get; set; } = null!;

    [Column("KD_CAMAT")]
    [StringLength(100)]
    [Unicode(false)]
    public string KdCamat { get; set; } = null!;

    [Column("KD_LURAH")]
    [StringLength(100)]
    [Unicode(false)]
    public string KdLurah { get; set; } = null!;

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("NILAI_KETETAPAN", TypeName = "NUMBER(38)")]
    public decimal? NilaiKetetapan { get; set; }

    [Column("NILAI_REALISASI", TypeName = "NUMBER(38)")]
    public decimal? NilaiRealisasi { get; set; }

    [Column("TANGGAL_REALISASI", TypeName = "DATE")]
    public DateTime? TanggalRealisasi { get; set; }

    [Column("STATUS_PELUNASAN")]
    [StringLength(22)]
    [Unicode(false)]
    public string? StatusPelunasan { get; set; }

    [Column("CREATE_DATE", TypeName = "DATE")]
    public DateTime CreateDate { get; set; }

    [Column("CREATE_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string CreateBy { get; set; } = null!;
}
