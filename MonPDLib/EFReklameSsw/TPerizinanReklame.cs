using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[PrimaryKey("NomorDaftar", "Tahun")]
[Table("T_PERIZINAN_REKLAME")]
public partial class TPerizinanReklame
{
    [Key]
    [Column("NOMOR_DAFTAR")]
    [StringLength(50)]
    [Unicode(false)]
    public string NomorDaftar { get; set; } = null!;

    [Column("PILIH_SUB_PERIZINAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PilihSubPerizinan { get; set; }

    [Column("PEMOHON_SEBAGAI")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PemohonSebagai { get; set; }

    [Column("NIK_PEMOHON")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NikPemohon { get; set; }

    [Column("NAMA_PEMOHON")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPemohon { get; set; }

    [Column("PROVINSI")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Provinsi { get; set; }

    [Column("KABUPATEN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Kabupaten { get; set; }

    [Column("KECAMATAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("KELURAHAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Kelurahan { get; set; }

    [Column("ALAMAT_PEMOHON")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatPemohon { get; set; }

    [Column("ALAMAT_DOMISILI_PEMOHON")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatDomisiliPemohon { get; set; }

    [Column("NO_HP_PEMOHON")]
    [StringLength(15)]
    [Unicode(false)]
    public string? NoHpPemohon { get; set; }

    [Column("PEKERJAAN_PEMOHON")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PekerjaanPemohon { get; set; }

    [Column("JABATAN_PEMOHON")]
    [StringLength(255)]
    [Unicode(false)]
    public string? JabatanPemohon { get; set; }

    [Column("NAMA_PERUSAHAAN_PEMOHON")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPerusahaanPemohon { get; set; }

    [Column("ALAMAT_PERUSAHAAN_PEMOHON")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatPerusahaanPemohon { get; set; }

    [Column("NO_TELP_PERUSAHAAN_PEMOHON")]
    [StringLength(15)]
    [Unicode(false)]
    public string? NoTelpPerusahaanPemohon { get; set; }

    [Column("NPWPD_PERUSAHAAN_PEMOHON")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NpwpdPerusahaanPemohon { get; set; }

    [Column("NAMA_PEMILIK")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPemilik { get; set; }

    [Column("ALAMAT_PEMILIK")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatPemilik { get; set; }

    [Column("NO_TELP_PEMILIK")]
    [StringLength(15)]
    [Unicode(false)]
    public string? NoTelpPemilik { get; set; }

    [Column("NAMA_PERUSAHAAN_PEMILIK")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPerusahaanPemilik { get; set; }

    [Column("ALAMAT_PERUSAHAAN_PEMILIK")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatPerusahaanPemilik { get; set; }

    [Column("NO_TELP_PERUSAHAAN_PEMILIK")]
    [StringLength(15)]
    [Unicode(false)]
    public string? NoTelpPerusahaanPemilik { get; set; }

    [Column("KD_PERIZINAN")]
    [StringLength(15)]
    [Unicode(false)]
    public string? KdPerizinan { get; set; }

    [Column("TGL_DAFTAR", TypeName = "DATE")]
    public DateTime? TglDaftar { get; set; }

    [Column("STATUS_KIRIM", TypeName = "NUMBER(38)")]
    public decimal? StatusKirim { get; set; }

    [Key]
    [Column("TAHUN")]
    [Precision(10)]
    public int Tahun { get; set; }

    [ForeignKey("KdPerizinan")]
    [InverseProperty("TPerizinanReklames")]
    public virtual MJenisPerizinanRek? KdPerizinanNavigation { get; set; }

    [InverseProperty("TPerizinanReklame")]
    public virtual ICollection<TPerizinanReklameBatal> TPerizinanReklameBatals { get; set; } = new List<TPerizinanReklameBatal>();

    [InverseProperty("TPerizinanReklame")]
    public virtual ICollection<TPerizinanReklameDet> TPerizinanReklameDets { get; set; } = new List<TPerizinanReklameDet>();
}
