using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_OP_RESTO")]
public partial class DbOpResto
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("NPWPD")]
    [StringLength(100)]
    [Unicode(false)]
    public string Npwpd { get; set; } = null!;

    [Column("NPWPD_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string NpwpdNama { get; set; } = null!;

    [Column("NPWPD_ALAMAT")]
    [StringLength(100)]
    [Unicode(false)]
    public string NpwpdAlamat { get; set; } = null!;

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal PajakId { get; set; }

    [Column("PAJAK_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string PajakNama { get; set; } = null!;

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaOp { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string AlamatOp { get; set; } = null!;

    [Column("ALAMAT_OP_NO")]
    [StringLength(50)]
    [Unicode(false)]
    public string AlamatOpNo { get; set; } = null!;

    [Column("ALAMAT_OP_RT")]
    [StringLength(5)]
    [Unicode(false)]
    public string AlamatOpRt { get; set; } = null!;

    [Column("ALAMAT_OP_RW")]
    [StringLength(5)]
    [Unicode(false)]
    public string AlamatOpRw { get; set; } = null!;

    [Column("TELP")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Telp { get; set; }

    [Column("ALAMAT_OP_KD_LURAH")]
    [StringLength(5)]
    [Unicode(false)]
    public string AlamatOpKdLurah { get; set; } = null!;

    [Column("ALAMAT_OP_KD_CAMAT")]
    [StringLength(5)]
    [Unicode(false)]
    public string AlamatOpKdCamat { get; set; } = null!;

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("TGL_MULAI_BUKA_OP", TypeName = "DATE")]
    public DateTime TglMulaiBukaOp { get; set; }

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string KategoriNama { get; set; } = null!;

    [Column("METODE_PEMBAYARAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string MetodePembayaran { get; set; } = null!;

    [Column("METODE_PENJUALAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string MetodePenjualan { get; set; } = null!;

    [Column("JUMLAH_KARYAWAN", TypeName = "NUMBER")]
    public decimal JumlahKaryawan { get; set; }

    [Column("JUMLAH_MEJA", TypeName = "NUMBER")]
    public decimal JumlahMeja { get; set; }

    [Column("JUMLAH_KURSI", TypeName = "NUMBER")]
    public decimal JumlahKursi { get; set; }

    [Column("KAPASITAS_RUANGAN_ORANG", TypeName = "NUMBER")]
    public decimal KapasitasRuanganOrang { get; set; }

    [Column("MAKSIMAL_PRODUKSI_PORSI_HARI", TypeName = "NUMBER")]
    public decimal MaksimalProduksiPorsiHari { get; set; }

    [Column("RATA_TERJUAL_PORSI_HARI", TypeName = "NUMBER")]
    public decimal RataTerjualPorsiHari { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(30)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("NAMA_AKUN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaAkun { get; set; } = null!;

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string Jenis { get; set; } = null!;

    [Column("NAMA_JENIS")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaJenis { get; set; } = null!;

    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string Objek { get; set; } = null!;

    [Column("NAMA_OBJEK")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaObjek { get; set; } = null!;

    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Rincian { get; set; } = null!;

    [Column("NAMA_RINCIAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaRincian { get; set; } = null!;

    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string SubRincian { get; set; } = null!;

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaSubRincian { get; set; } = null!;

    [Column("KELOMPOK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Kelompok { get; set; }

    [Column("NAMA_KELOMPOK")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }

    [Column("IS_TUTUP")]
    [Precision(2)]
    public byte? IsTutup { get; set; }
}
