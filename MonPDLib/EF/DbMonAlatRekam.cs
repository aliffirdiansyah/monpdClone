using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbMonAlatRekam
{
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("UPTB", TypeName = "NUMBER")]
    public decimal? Uptb { get; set; }

    [Column("TMIN0", TypeName = "NUMBER")]
    public decimal? Tmin0 { get; set; }

    [Column("TMIN1", TypeName = "NUMBER")]
    public decimal? Tmin1 { get; set; }

    [Column("TMIN2", TypeName = "NUMBER")]
    public decimal? Tmin2 { get; set; }

    [Column("TMIN3", TypeName = "NUMBER")]
    public decimal? Tmin3 { get; set; }

    [Column("TMIN4", TypeName = "NUMBER")]
    public decimal? Tmin4 { get; set; }

    [Column("TMIN5", TypeName = "NUMBER")]
    public decimal? Tmin5 { get; set; }

    [Column("TMIN6", TypeName = "NUMBER")]
    public decimal? Tmin6 { get; set; }

    [Column("TMIN7", TypeName = "NUMBER")]
    public decimal? Tmin7 { get; set; }

    [Column("STATUS_TERPASANG", TypeName = "NUMBER")]
    public decimal? StatusTerpasang { get; set; }

    [Column("TGL_TERPASANG", TypeName = "DATE")]
    public DateTime? TglTerpasang { get; set; }

    [Column("STATUS_KUNCI", TypeName = "NUMBER")]
    public decimal? StatusKunci { get; set; }

    [Column("JENIS_ALAT")]
    [StringLength(2)]
    [Unicode(false)]
    public string? JenisAlat { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("TGL", TypeName = "NUMBER")]
    public decimal? Tgl { get; set; }

    [Column("BLN", TypeName = "NUMBER")]
    public decimal? Bln { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("JAM")]
    [StringLength(8)]
    [Unicode(false)]
    public string? Jam { get; set; }

    [Column("STATUS_ONLINE", TypeName = "NUMBER")]
    public decimal? StatusOnline { get; set; }

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }
}
