using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "TahunPajakKetetapan", "MasaPajakKetetapan", "SeqPajakKetetapan")]
[Table("DB_MON_ABT")]
public partial class DbMonAbt
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

    [Column("IS_TUTUP", TypeName = "NUMBER")]
    public decimal IsTutup { get; set; }

    [Column("PERUNTUKAN_ID", TypeName = "NUMBER")]
    public decimal PeruntukanId { get; set; }

    [Column("PERUNTUKAN_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string PeruntukanNama { get; set; } = null!;

    [Column("KATEGORI_ID", TypeName = "NUMBER")]
    public decimal KategoriId { get; set; }

    [Column("KATEGORI_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string KategoriNama { get; set; } = null!;

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

    [Key]
    [Column("TAHUN_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal TahunPajakKetetapan { get; set; }

    [Key]
    [Column("MASA_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal MasaPajakKetetapan { get; set; }

    [Key]
    [Column("SEQ_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal SeqPajakKetetapan { get; set; }

    [Column("KATEGORI_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KategoriKetetapan { get; set; }

    [Column("TGL_KETETAPAN", TypeName = "DATE")]
    public DateTime? TglKetetapan { get; set; }

    [Column("TGL_JATUH_TEMPO_BAYAR", TypeName = "DATE")]
    public DateTime? TglJatuhTempoBayar { get; set; }

    [Column("IS_LUNAS_KETETAPAN", TypeName = "NUMBER")]
    public decimal? IsLunasKetetapan { get; set; }

    [Column("TGL_LUNAS_KETETAPAN", TypeName = "DATE")]
    public DateTime? TglLunasKetetapan { get; set; }

    [Column("POKOK_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? PokokPajakKetetapan { get; set; }

    [Column("PENGURANG_POKOK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? PengurangPokokKetetapan { get; set; }

    [Column("AKUN_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? AkunKetetapan { get; set; }

    [Column("JENIS_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? JenisKetetapan { get; set; }

    [Column("OBJEK_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ObjekKetetapan { get; set; }

    [Column("RINCIAN_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? RincianKetetapan { get; set; }

    [Column("SUB_RINCIAN_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincianKetetapan { get; set; }

    [Column("TGL_BAYAR_POKOK", TypeName = "DATE")]
    public DateTime? TglBayarPokok { get; set; }

    [Column("NOMINAL_POKOK_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalPokokBayar { get; set; }

    [Column("AKUN_POKOK_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? AkunPokokBayar { get; set; }

    [Column("JENIS_POKOK_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? JenisPokokBayar { get; set; }

    [Column("OBJEK_POKOK_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ObjekPokokBayar { get; set; }

    [Column("RINCIAN_POKOK_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? RincianPokokBayar { get; set; }

    [Column("SUB_RINCIAN_POKOK_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincianPokokBayar { get; set; }

    [Column("TGL_BAYAR_SANKSI", TypeName = "DATE")]
    public DateTime? TglBayarSanksi { get; set; }

    [Column("NOMINAL_SANKSI_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalSanksiBayar { get; set; }

    [Column("AKUN_SANKSI_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? AkunSanksiBayar { get; set; }

    [Column("JENIS_SANKSI_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? JenisSanksiBayar { get; set; }

    [Column("OBJEK_SANKSI_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ObjekSanksiBayar { get; set; }

    [Column("RINCIAN_SANKSI_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? RincianSanksiBayar { get; set; }

    [Column("SUB_RINCIAN_SANKSI_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincianSanksiBayar { get; set; }

    [Column("TGL_BAYAR_SANKSI_KENAIKAN", TypeName = "DATE")]
    public DateTime? TglBayarSanksiKenaikan { get; set; }

    [Column("NOMINAL_SANKSI_KENAIKAN_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalSanksiKenaikanBayar { get; set; }

    [Column("AKUN_KENAIKAN_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? AkunKenaikanBayar { get; set; }

    [Column("JENIS_KENAIKAN_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? JenisKenaikanBayar { get; set; }

    [Column("OBJEK_KENAIKAN_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ObjekKenaikanBayar { get; set; }

    [Column("RINCIAN_KENAIKAN_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? RincianKenaikanBayar { get; set; }

    [Column("SUB_RINCIAN_KENAIKAN_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincianKenaikanBayar { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("UPD_DATE", TypeName = "DATE")]
    public DateTime UpdDate { get; set; }

    [Column("UPD_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string UpdBy { get; set; } = null!;

    [Column("KELOMPOK")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Kelompok { get; set; }

    [Column("KELOMPOK_NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? KelompokNama { get; set; }

    [Column("KELOMPOK_KETETAPAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KelompokKetetapan { get; set; }

    [Column("KELOMPOK_BAYAR")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KelompokBayar { get; set; }

    [Column("KELOMPOK_SANKSI_BAYAR")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KelompokSanksiBayar { get; set; }

    [Column("KELOMPOK_KENAIKAN_BAYAR")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KelompokKenaikanBayar { get; set; }
}
