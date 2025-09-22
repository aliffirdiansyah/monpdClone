using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class MvDbOpResto
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

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }

    [Column("PAJAK_NAMA")]
    [StringLength(23)]
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

    [Column("ALAMAT_OP_NO")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AlamatOpNo { get; set; }

    [Column("ALAMAT_OP_RT")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AlamatOpRt { get; set; }

    [Column("ALAMAT_OP_RW")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AlamatOpRw { get; set; }

    [Column("TELP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Telp { get; set; }

    [Column("ALAMAT_OP_KD_LURAH")]
    [StringLength(3)]
    [Unicode(false)]
    public string? AlamatOpKdLurah { get; set; }

    [Column("ALAMAT_OP_KD_CAMAT")]
    [StringLength(3)]
    [Unicode(false)]
    public string? AlamatOpKdCamat { get; set; }

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("TGL_MULAI_BUKA_OP", TypeName = "DATE")]
    public DateTime? TglMulaiBukaOp { get; set; }

    [Column("METODE_PENJUALAN", TypeName = "NUMBER")]
    public decimal? MetodePenjualan { get; set; }

    [Column("METODE_PEMBAYARAN", TypeName = "NUMBER")]
    public decimal? MetodePembayaran { get; set; }

    [Column("JUMLAH_KARYAWAN", TypeName = "NUMBER")]
    public decimal? JumlahKaryawan { get; set; }

    [Column("JUMLAH_MEJA", TypeName = "NUMBER")]
    public decimal? JumlahMeja { get; set; }

    [Column("JUMLAH_KURSI", TypeName = "NUMBER")]
    public decimal? JumlahKursi { get; set; }

    [Column("KAPASITAS_RUANGAN_ORANG", TypeName = "NUMBER")]
    public decimal? KapasitasRuanganOrang { get; set; }

    [Column("MAKSIMAL_PRODUKSI_PORSI_HARI", TypeName = "NUMBER")]
    public decimal? MaksimalProduksiPorsiHari { get; set; }

    [Column("RATA_TERJUAL_PORSI_HARI", TypeName = "NUMBER")]
    public decimal? RataTerjualPorsiHari { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal? KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(38)]
    [Unicode(false)]
    public string? KategoriNama { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(3)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal? TahunBuku { get; set; }

    [Column("AKUN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Akun { get; set; }

    [Column("NAMA_AKUN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? NamaAkun { get; set; }

    [Column("JENIS")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Column("NAMA_JENIS")]
    [StringLength(1)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

    [Column("OBJEK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Objek { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? NamaObjek { get; set; }

    [Column("RINCIAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Rincian { get; set; }

    [Column("NAMA_RINCIAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? NamaRincian { get; set; }

    [Column("SUB_RINCIAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? SubRincian { get; set; }

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? NamaSubRincian { get; set; }

    [Column("KELOMPOK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Kelompok { get; set; }

    [Column("NAMA_KELOMPOK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }

    [Column("IS_TUTUP", TypeName = "NUMBER")]
    public decimal? IsTutup { get; set; }
}
