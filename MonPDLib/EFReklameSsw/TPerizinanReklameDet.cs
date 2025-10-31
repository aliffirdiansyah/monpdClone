using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[PrimaryKey("NomorDaftar", "Seq", "Tahun")]
[Table("T_PERIZINAN_REKLAME_DET")]
public partial class TPerizinanReklameDet
{
    [Key]
    [Column("NOMOR_DAFTAR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NomorDaftar { get; set; } = null!;

    [Key]
    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }

    [Column("JENIS_REKLAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string? JenisReklame { get; set; }

    [Column("NAMA_JALAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaJalan { get; set; }

    [Column("LOKASI_PENYELENGGARAAN_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? LokasiPenyelenggaraanReklame { get; set; }

    [Column("DETAIL_PENYELENGGARAAN_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? DetailPenyelenggaraanReklame { get; set; }

    [Column("KECAMATAN_REKLAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string? KecamatanReklame { get; set; }

    [Column("KELURAHAN_REKLAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string? KelurahanReklame { get; set; }

    [Column("JENIS_PRODUK")]
    [StringLength(50)]
    [Unicode(false)]
    public string? JenisProduk { get; set; }

    [Column("PENEMPATAN_REKLAME")]
    [StringLength(50)]
    [Unicode(false)]
    public string? PenempatanReklame { get; set; }

    [Column("STATUS_TANAH")]
    [StringLength(50)]
    [Unicode(false)]
    public string? StatusTanah { get; set; }

    [Column("FILE_UPLOAD_GAMBAR")]
    [StringLength(255)]
    [Unicode(false)]
    public string? FileUploadGambar { get; set; }

    [Column("PERSETUJUAN_PEMILIK_LAHAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PersetujuanPemilikLahan { get; set; }

    [Column("SKETSA_TITIK_LOKASI")]
    [StringLength(255)]
    [Unicode(false)]
    public string? SketsaTitikLokasi { get; set; }

    [Column("DESAIN_TIPOLOGI_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? DesainTipologiReklame { get; set; }

    [Column("FOTO_LOKASI_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? FotoLokasiReklame { get; set; }

    [Column("FOTOKOPI_SIPR")]
    [StringLength(255)]
    [Unicode(false)]
    public string? FotokopiSipr { get; set; }

    [Column("PERJANJIAN_SEWA")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PerjanjianSewa { get; set; }

    [Column("TANGGAL_MULAI", TypeName = "DATE")]
    public DateTime? TanggalMulai { get; set; }

    [Column("TANGGAL_SELESAI", TypeName = "DATE")]
    public DateTime? TanggalSelesai { get; set; }

    [Column("SUDUT_PANDANG")]
    [StringLength(50)]
    [Unicode(false)]
    public string? SudutPandang { get; set; }

    [Column("PANJANG", TypeName = "NUMBER(18,4)")]
    public decimal? Panjang { get; set; }

    [Column("LEBAR", TypeName = "NUMBER(18,4)")]
    public decimal? Lebar { get; set; }

    [Column("TINGGI", TypeName = "NUMBER(18,4)")]
    public decimal? Tinggi { get; set; }

    [Column("LUAS", TypeName = "NUMBER(18,4)")]
    public decimal? Luas { get; set; }

    [Column("JUMLAH", TypeName = "NUMBER(38)")]
    public decimal? Jumlah { get; set; }

    [Column("ISI_TEKS")]
    [StringLength(500)]
    [Unicode(false)]
    public string? IsiTeks { get; set; }

    [Column("JUMLAH_LAYAR", TypeName = "NUMBER(38)")]
    public decimal? JumlahLayar { get; set; }

    [Column("LAMA_DURASI", TypeName = "NUMBER(38)")]
    public decimal? LamaDurasi { get; set; }

    [Column("STATUS", TypeName = "NUMBER(38)")]
    public decimal? Status { get; set; }

    [Column("NO_KETETAPAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NoKetetapan { get; set; }

    [Column("KODE_VA")]
    [StringLength(75)]
    [Unicode(false)]
    public string? KodeVa { get; set; }

    [Column("NILAI_VA", TypeName = "NUMBER(15,2)")]
    public decimal? NilaiVa { get; set; }

    [Column("EXP_VA", TypeName = "DATE")]
    public DateTime? ExpVa { get; set; }

    [Key]
    [Column("TAHUN")]
    [Precision(10)]
    public int Tahun { get; set; }

    [ForeignKey("NomorDaftar, Tahun")]
    [InverseProperty("TPerizinanReklameDets")]
    public virtual TPerizinanReklame TPerizinanReklame { get; set; } = null!;

    [InverseProperty("TPerizinanReklameDet")]
    public virtual ICollection<TPerizinanReklameDetFoto> TPerizinanReklameDetFotos { get; set; } = new List<TPerizinanReklameDetFoto>();
}
