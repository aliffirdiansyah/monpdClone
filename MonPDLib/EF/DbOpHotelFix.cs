using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbOpHotelFix
{
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("NPWPD")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Npwpd { get; set; }

    [Column("NPWPD_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NpwpdNama { get; set; }

    [Column("NPWPD_ALAMAT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NpwpdAlamat { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("PAJAK_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PajakNama { get; set; }

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string? AlamatOp { get; set; }

    [Column("ALAMAT_OP_KD_LURAH")]
    [StringLength(5)]
    [Unicode(false)]
    public string? AlamatOpKdLurah { get; set; }

    [Column("ALAMAT_OP_KD_CAMAT")]
    [StringLength(5)]
    [Unicode(false)]
    public string? AlamatOpKdCamat { get; set; }

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("TGL_MULAI_BUKA_OP", TypeName = "DATE")]
    public DateTime? TglMulaiBukaOp { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }

    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Akun { get; set; }

    [Column("NAMA_AKUN")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaAkun { get; set; }

    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Kelompok { get; set; }

    [Column("NAMA_KELOMPOK")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Column("NAMA_JENIS")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Objek { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaObjek { get; set; }

    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Rincian { get; set; }

    [Column("NAMA_RINCIAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaRincian { get; set; }

    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincian { get; set; }

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaSubRincian { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }

    [Column("IS_TUTUP", TypeName = "NUMBER")]
    public decimal? IsTutup { get; set; }
}
