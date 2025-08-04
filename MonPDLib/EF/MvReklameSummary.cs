using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvReklameSummary
{
    [Column("NO_FORMULIR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("ID_FLAG_PERMOHONAN", TypeName = "NUMBER")]
    public decimal? IdFlagPermohonan { get; set; }

    [Column("FLAG_PERMOHONAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FlagPermohonan { get; set; }

    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("NAMA_PERUSAHAAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPerusahaan { get; set; }

    [Column("TELP_PERUSAHAAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TelpPerusahaan { get; set; }

    [Column("ALAMATREKLAME")]
    [StringLength(181)]
    [Unicode(false)]
    public string? Alamatreklame { get; set; }

    [Column("NM_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NmJenis { get; set; }

    [Column("PANJANG", TypeName = "NUMBER")]
    public decimal? Panjang { get; set; }

    [Column("LEBAR", TypeName = "NUMBER")]
    public decimal? Lebar { get; set; }

    [Column("LUAS", TypeName = "NUMBER")]
    public decimal? Luas { get; set; }

    [Column("KETINGGIAN", TypeName = "NUMBER")]
    public decimal? Ketinggian { get; set; }

    [Column("ISI_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? IsiReklame { get; set; }

    [Column("TAHUN_PAJAK")]
    [StringLength(4)]
    [Unicode(false)]
    public string? TahunPajak { get; set; }

    [Column("TGL_MULAI_BERLAKU", TypeName = "DATE")]
    public DateTime? TglMulaiBerlaku { get; set; }

    [Column("TGL_AKHIR_BERLAKU", TypeName = "DATE")]
    public DateTime? TglAkhirBerlaku { get; set; }

    [Column("PAJAK_POKOK", TypeName = "NUMBER")]
    public decimal? PajakPokok { get; set; }

    [Column("TGL_BAYAR_POKOK", TypeName = "DATE")]
    public DateTime? TglBayarPokok { get; set; }

    [Column("NOMINAL_POKOK_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalPokokBayar { get; set; }

    [Column("SILANG", TypeName = "NUMBER")]
    public decimal? Silang { get; set; }

    [Column("BONGKAR", TypeName = "NUMBER")]
    public decimal? Bongkar { get; set; }

    [Column("BANTIP", TypeName = "NUMBER")]
    public decimal? Bantip { get; set; }

    [Column("KELAS_JALAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KelasJalan { get; set; }

    [Column("NAMA_JALAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJalan { get; set; }

    [Column("NO_FORMULIR_A")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulirA { get; set; }

    [Column("NO_FORMULIR_LAMA_A")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulirLamaA { get; set; }

    [Column("ID_FLAG_PERMOHONAN_A", TypeName = "NUMBER")]
    public decimal? IdFlagPermohonanA { get; set; }

    [Column("FLAG_PERMOHONAN_A")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FlagPermohonanA { get; set; }

    [Column("TAHUN_A", TypeName = "NUMBER")]
    public decimal? TahunA { get; set; }

    [Column("BULAN_A", TypeName = "NUMBER")]
    public decimal? BulanA { get; set; }

    [Column("NAMA_A")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaA { get; set; }

    [Column("NAMA_PERUSAHAAN_A")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPerusahaanA { get; set; }

    [Column("TELP_PERUSAHAAN_A")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TelpPerusahaanA { get; set; }

    [Column("ALAMATREKLAME_A")]
    [StringLength(181)]
    [Unicode(false)]
    public string? AlamatreklameA { get; set; }

    [Column("NM_JENIS_A")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NmJenisA { get; set; }

    [Column("PANJANG_A", TypeName = "NUMBER")]
    public decimal? PanjangA { get; set; }

    [Column("LEBAR_A", TypeName = "NUMBER")]
    public decimal? LebarA { get; set; }

    [Column("LUAS_A", TypeName = "NUMBER")]
    public decimal? LuasA { get; set; }

    [Column("KETINGGIAN_A", TypeName = "NUMBER")]
    public decimal? KetinggianA { get; set; }

    [Column("ISI_REKLAME_A")]
    [StringLength(255)]
    [Unicode(false)]
    public string? IsiReklameA { get; set; }

    [Column("TAHUN_PAJAK_A")]
    [StringLength(4)]
    [Unicode(false)]
    public string? TahunPajakA { get; set; }

    [Column("TGL_MULAI_BERLAKU_A", TypeName = "DATE")]
    public DateTime? TglMulaiBerlakuA { get; set; }

    [Column("TGL_AKHIR_BERLAKU_A", TypeName = "DATE")]
    public DateTime? TglAkhirBerlakuA { get; set; }

    [Column("TGL_KETETAPAN_A", TypeName = "DATE")]
    public DateTime? TglKetetapanA { get; set; }

    [Column("PAJAK_POKOK_A", TypeName = "NUMBER")]
    public decimal? PajakPokokA { get; set; }

    [Column("TGL_BAYAR_POKOK_A", TypeName = "DATE")]
    public DateTime? TglBayarPokokA { get; set; }

    [Column("NOMINAL_POKOK_BAYAR_A", TypeName = "NUMBER")]
    public decimal? NominalPokokBayarA { get; set; }

    [Column("SILANG_A", TypeName = "NUMBER")]
    public decimal? SilangA { get; set; }

    [Column("BONGKAR_A", TypeName = "NUMBER")]
    public decimal? BongkarA { get; set; }

    [Column("BANTIP_A", TypeName = "NUMBER")]
    public decimal? BantipA { get; set; }

    [Column("KELAS_JALAN_A")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KelasJalanA { get; set; }

    [Column("NAMA_JALAN_A")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJalanA { get; set; }

    [Column("IS_PERPANJANGAN", TypeName = "NUMBER")]
    public decimal? IsPerpanjangan { get; set; }

    [Column("JUMLAH")]
    [Precision(10)]
    public int? Jumlah { get; set; }
}
