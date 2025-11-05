using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class VwDbMonResto
{
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("NPWPD")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Npwpd { get; set; }

    [Column("NPWPD_NAMA")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NpwpdNama { get; set; }

    [Column("NPWPD_ALAMAT")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NpwpdAlamat { get; set; }

    [Column("PAJAK_ID")]
    [StringLength(1)]
    [Unicode(false)]
    public string? PajakId { get; set; }

    [Column("PAJAK_NAMA")]
    [Unicode(false)]
    public string? PajakNama { get; set; }

    [Column("NAMA_OP")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaOp { get; set; }

    [Column("ALAMAT_OP")]
    [StringLength(255)]
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

    [Column("IS_TUTUP", TypeName = "NUMBER")]
    public decimal? IsTutup { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("AKUN_1")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Akun1 { get; set; }

    [Column("NAMA_AKUN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? NamaAkun { get; set; }

    [Column("JENIS_1")]
    [StringLength(6)]
    [Unicode(false)]
    public string? Jenis1 { get; set; }

    [Column("NAMA_JENIS")]
    [StringLength(12)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

    [Column("OBJEK_1")]
    [StringLength(9)]
    [Unicode(false)]
    public string? Objek1 { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(37)]
    [Unicode(false)]
    public string? NamaObjek { get; set; }

    [Column("RINCIAN_1")]
    [StringLength(12)]
    [Unicode(false)]
    public string? Rincian1 { get; set; }

    [Column("NAMA_RINCIAN")]
    [StringLength(64)]
    [Unicode(false)]
    public string? NamaRincian { get; set; }

    [Column("SUB_RINCIAN_1")]
    [StringLength(17)]
    [Unicode(false)]
    public string? SubRincian1 { get; set; }

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(13)]
    [Unicode(false)]
    public string? NamaSubRincian { get; set; }

    [Column("TAHUN_PAJAK_KETETAPAN")]
    [StringLength(40)]
    [Unicode(false)]
    public string? TahunPajakKetetapan { get; set; }

    [Column("MASA_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? MasaPajakKetetapan { get; set; }

    [Column("SEQ_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? SeqPajakKetetapan { get; set; }

    [Column("KATEGORI_KETETAPAN", TypeName = "NUMBER")]
    public decimal? KategoriKetetapan { get; set; }

    [Column("TGL_KETETAPAN", TypeName = "DATE")]
    public DateTime? TglKetetapan { get; set; }

    [Column("TGL_JATUH_TEMPO_BAYAR", TypeName = "DATE")]
    public DateTime? TglJatuhTempoBayar { get; set; }

    [Column("POKOK_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? PokokPajakKetetapan { get; set; }

    [Column("PENGURANG_POKOK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? PengurangPokokKetetapan { get; set; }

    [Column("AKUN_KETETAPAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AkunKetetapan { get; set; }

    [Column("JENIS_KETETAPAN")]
    [StringLength(6)]
    [Unicode(false)]
    public string? JenisKetetapan { get; set; }

    [Column("OBJEK_KETETAPAN")]
    [StringLength(9)]
    [Unicode(false)]
    public string? ObjekKetetapan { get; set; }

    [Column("RINCIAN_KETETAPAN")]
    [StringLength(12)]
    [Unicode(false)]
    public string? RincianKetetapan { get; set; }

    [Column("SUB_RINCIAN_KETETAPAN")]
    [StringLength(17)]
    [Unicode(false)]
    public string? SubRincianKetetapan { get; set; }

    [Column("TGL_BAYAR_POKOK", TypeName = "DATE")]
    public DateTime? TglBayarPokok { get; set; }

    [Column("NOMINAL_POKOK_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalPokokBayar { get; set; }

    [Column("AKUN_POKOK_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AkunPokokBayar { get; set; }

    [Column("JENIS_POKOK_BAYAR")]
    [StringLength(6)]
    [Unicode(false)]
    public string? JenisPokokBayar { get; set; }

    [Column("OBJEK_POKOK_BAYAR")]
    [StringLength(9)]
    [Unicode(false)]
    public string? ObjekPokokBayar { get; set; }

    [Column("RINCIAN_POKOK_BAYAR")]
    [StringLength(12)]
    [Unicode(false)]
    public string? RincianPokokBayar { get; set; }

    [Column("SUB_RINCIAN_POKOK_BAYAR")]
    [StringLength(17)]
    [Unicode(false)]
    public string? SubRincianPokokBayar { get; set; }

    [Column("TGL_BAYAR_SANKSI", TypeName = "DATE")]
    public DateTime? TglBayarSanksi { get; set; }

    [Column("NOMINAL_SANKSI_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalSanksiBayar { get; set; }

    [Column("AKUN_SANKSI_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AkunSanksiBayar { get; set; }

    [Column("JENIS_SANKSI_BAYAR")]
    [StringLength(6)]
    [Unicode(false)]
    public string? JenisSanksiBayar { get; set; }

    [Column("OBJEK_SANKSI_BAYAR")]
    [StringLength(9)]
    [Unicode(false)]
    public string? ObjekSanksiBayar { get; set; }

    [Column("RINCIAN_SANKSI_BAYAR")]
    [StringLength(11)]
    [Unicode(false)]
    public string? RincianSanksiBayar { get; set; }

    [Column("SUB_RINCIAN_SANKSI_BAYAR")]
    [StringLength(13)]
    [Unicode(false)]
    public string? SubRincianSanksiBayar { get; set; }

    [Column("TGL_BAYAR_SANKSI_KENAIKAN", TypeName = "DATE")]
    public DateTime? TglBayarSanksiKenaikan { get; set; }

    [Column("NOMINAL_SANKSI_KENAIKAN_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalSanksiKenaikanBayar { get; set; }

    [Column("AKUN_KENAIKAN_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AkunKenaikanBayar { get; set; }

    [Column("JENIS_KENAIKAN_BAYAR")]
    [StringLength(6)]
    [Unicode(false)]
    public string? JenisKenaikanBayar { get; set; }

    [Column("OBJEK_KENAIKAN_BAYAR")]
    [StringLength(9)]
    [Unicode(false)]
    public string? ObjekKenaikanBayar { get; set; }

    [Column("RINCIAN_KENAIKAN_BAYAR")]
    [StringLength(11)]
    [Unicode(false)]
    public string? RincianKenaikanBayar { get; set; }

    [Column("SUB_RINCIAN_KENAIKAN_BAYAR")]
    [StringLength(13)]
    [Unicode(false)]
    public string? SubRincianKenaikanBayar { get; set; }

    [Column("KELOMPOK_POKOK_BAYAR")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KelompokPokokBayar { get; set; }

    [Column("KELOMPOK_SANKSI_BAYAR")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KelompokSanksiBayar { get; set; }

    [Column("KELOMPOK_KENAIKAN_BAYAR")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KelompokKenaikanBayar { get; set; }

    [Column("KELOMPOK_KETETAPAN")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KelompokKetetapan { get; set; }

    [Column("KELOMPOK")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Kelompok { get; set; }

    [Column("NAMA_KELOMPOK")]
    [StringLength(28)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }
}
