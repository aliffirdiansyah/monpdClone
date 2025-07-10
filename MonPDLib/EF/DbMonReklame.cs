using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_MON_REKLAME")]
public partial class DbMonReklame
{
    [Key]
    [Column("NO_FORMULIR")]
    [StringLength(20)]
    [Unicode(false)]
    public string NoFormulir { get; set; } = null!;

    [Column("NO_PERUSAHAAN", TypeName = "NUMBER")]
    public decimal? NoPerusahaan { get; set; }

    [Column("NAMA_PERUSAHAAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPerusahaan { get; set; }

    [Column("ALAMAT_PERUSAHAAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatPerusahaan { get; set; }

    [Column("NO_ALAMAT_PERUSAHAAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoAlamatPerusahaan { get; set; }

    [Column("BLOK_ALAMAT_PERUSAHAAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? BlokAlamatPerusahaan { get; set; }

    [Column("ALAMATPER")]
    [StringLength(467)]
    [Unicode(false)]
    public string? Alamatper { get; set; }

    [Column("TELP_PERUSAHAAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? TelpPerusahaan { get; set; }

    [Column("CLIENTNAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Clientnama { get; set; }

    [Column("CLIENTALAMAT")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Clientalamat { get; set; }

    [Column("JABATAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Jabatan { get; set; }

    [Column("KODE_JENIS")]
    [StringLength(2)]
    [Unicode(false)]
    public string? KodeJenis { get; set; }

    [Column("NM_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NmJenis { get; set; }

    [Column("NO_WP")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoWp { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("ALAMAT")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Alamat { get; set; }

    [Column("NO_ALAMAT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoAlamat { get; set; }

    [Column("BLOK_ALAMAT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? BlokAlamat { get; set; }

    [Column("ALAMATWP")]
    [StringLength(468)]
    [Unicode(false)]
    public string? Alamatwp { get; set; }

    [Column("JENIS_PERMOHONAN")]
    [StringLength(7)]
    [Unicode(false)]
    public string? JenisPermohonan { get; set; }

    [Column("TGL_PERMOHONAN", TypeName = "DATE")]
    public DateTime? TglPermohonan { get; set; }

    [Column("TGL_MULAI_BERLAKU", TypeName = "DATE")]
    public DateTime? TglMulaiBerlaku { get; set; }

    [Column("TGL_AKHIR_BERLAKU", TypeName = "DATE")]
    public DateTime? TglAkhirBerlaku { get; set; }

    [Column("NAMA_JALAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJalan { get; set; }

    [Column("NO_JALAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoJalan { get; set; }

    [Column("BLOK_JALAN")]
    [StringLength(50)]
    [Unicode(false)]
    public string? BlokJalan { get; set; }

    [Column("ALAMATREKLAME")]
    [StringLength(181)]
    [Unicode(false)]
    public string? Alamatreklame { get; set; }

    [Column("DETIL_LOKASI")]
    [StringLength(200)]
    [Unicode(false)]
    public string? DetilLokasi { get; set; }

    [Column("KECAMATAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("JENIS_PRODUK")]
    [StringLength(29)]
    [Unicode(false)]
    public string? JenisProduk { get; set; }

    [Column("LETAK_REKLAME")]
    [StringLength(26)]
    [Unicode(false)]
    public string? LetakReklame { get; set; }

    [Column("STATUS_TANAH")]
    [StringLength(50)]
    [Unicode(false)]
    public string? StatusTanah { get; set; }

    [Column("FLAG_PERMOHONAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FlagPermohonan { get; set; }

    [Column("STATUSPROSES")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Statusproses { get; set; }

    [Column("FLAG_SIMPATIK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? FlagSimpatik { get; set; }

    [Column("KODE_OBYEK")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KodeObyek { get; set; }

    [Column("PANJANG", TypeName = "NUMBER")]
    public decimal? Panjang { get; set; }

    [Column("LEBAR", TypeName = "NUMBER")]
    public decimal? Lebar { get; set; }

    [Column("LUAS", TypeName = "NUMBER")]
    public decimal? Luas { get; set; }

    [Column("LUASDISKON", TypeName = "NUMBER")]
    public decimal? Luasdiskon { get; set; }

    [Column("SISI", TypeName = "NUMBER")]
    public decimal? Sisi { get; set; }

    [Column("KETINGGIAN", TypeName = "NUMBER")]
    public decimal? Ketinggian { get; set; }

    [Column("ISI_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? IsiReklame { get; set; }

    [Column("PERMOHONAN_BARU")]
    [StringLength(12)]
    [Unicode(false)]
    public string? PermohonanBaru { get; set; }

    [Column("NO_FORMULIR_LAMA")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulirLama { get; set; }

    [Column("SUDUT_PANDANG", TypeName = "NUMBER")]
    public decimal? SudutPandang { get; set; }

    [Column("NILAIPAJAK", TypeName = "NUMBER(38)")]
    public decimal? Nilaipajak { get; set; }

    [Column("NILAIJAMBONG", TypeName = "NUMBER(38)")]
    public decimal? Nilaijambong { get; set; }

    [Column("KELAS_JALAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KelasJalan { get; set; }

    [Column("NO_TELP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoTelp { get; set; }

    [Column("TIMETRANS", TypeName = "TIMESTAMP(6) WITH LOCAL TIME ZONE")]
    public DateTime? Timetrans { get; set; }

    [Column("NPWPD")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Npwpd { get; set; }

    [Column("FLAGTUNG")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Flagtung { get; set; }

    [Column("STATUSCABUT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Statuscabut { get; set; }

    [Column("NOR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nor { get; set; }

    [Column("KODE_LOKASI")]
    [StringLength(2)]
    [Unicode(false)]
    public string? KodeLokasi { get; set; }

    [Column("NAMA_PENEMPATAN")]
    [StringLength(60)]
    [Unicode(false)]
    public string? NamaPenempatan { get; set; }

    [Column("NO_FORMULIR_AWAL")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoFormulirAwal { get; set; }

    [Column("KETPERSIL")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Ketpersil { get; set; }

    [Column("PER_PENANGGUNGJAWAB")]
    [StringLength(255)]
    [Unicode(false)]
    public string? PerPenanggungjawab { get; set; }

    [Column("ALAMATPER_PENANGGUNGJAWAB")]
    [StringLength(467)]
    [Unicode(false)]
    public string? AlamatperPenanggungjawab { get; set; }

    [Column("NPWPD_PENANGGUNGJAWAB")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NpwpdPenanggungjawab { get; set; }

    [Column("POTENSI")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Potensi { get; set; }

    [Column("FLAGMALL")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Flagmall { get; set; }

    [Column("FLAGJEDA")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Flagjeda { get; set; }

    [Column("FLAGBRANDED")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Flagbranded { get; set; }

    [Column("NLPR")]
    [StringLength(5)]
    [Unicode(false)]
    public string? Nlpr { get; set; }

    [Column("USERNAME")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Username { get; set; }

    [Column("JENIS_WP")]
    [StringLength(10)]
    [Unicode(false)]
    public string? JenisWp { get; set; }

    [Column("TGL_CETAK_PER", TypeName = "DATE")]
    public DateTime? TglCetakPer { get; set; }

    [Column("STATUS_A_WP", TypeName = "NUMBER")]
    public decimal? StatusAWp { get; set; }

    [Column("STATUS_A_PER", TypeName = "NUMBER")]
    public decimal? StatusAPer { get; set; }

    [Column("NMKELURAHAN")]
    [StringLength(25)]
    [Unicode(false)]
    public string? Nmkelurahan { get; set; }

    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nop { get; set; }

    [Column("UNIT_KERJA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UnitKerja { get; set; }

    [Column("UNIT_BERKAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UnitBerkas { get; set; }

    [Column("STATUS_VER", TypeName = "NUMBER")]
    public decimal? StatusVer { get; set; }

    [Column("TGL_VER", TypeName = "DATE")]
    public DateTime? TglVer { get; set; }

    [Column("USER_VER")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UserVer { get; set; }

    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }

    [Column("ID_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? IdKetetapan { get; set; }

    [Column("TGLPENETAPAN", TypeName = "DATE")]
    public DateTime? Tglpenetapan { get; set; }

    [Column("TAHUN_PAJAK")]
    [StringLength(4)]
    [Unicode(false)]
    public string? TahunPajak { get; set; }

    [Column("BULAN_PAJAK")]
    [StringLength(2)]
    [Unicode(false)]
    public string? BulanPajak { get; set; }

    [Column("PAJAK_POKOK", TypeName = "NUMBER")]
    public decimal? PajakPokok { get; set; }

    [Column("JNS_KETETAPAN")]
    [StringLength(16)]
    [Unicode(false)]
    public string? JnsKetetapan { get; set; }

    [Column("TGL_JTEMPO_SKPD", TypeName = "DATE")]
    public DateTime? TglJtempoSkpd { get; set; }

    [Column("AKUN")]
    [StringLength(20)]
    [Unicode(false)]
    public string Akun { get; set; } = null!;

    [Column("NAMA_AKUN")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaAkun { get; set; } = null!;

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

    [Column("TAHUN_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal TahunPajakKetetapan { get; set; }

    [Column("MASA_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal MasaPajakKetetapan { get; set; }

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

    [Column("KELOMPOK_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KelompokKetetapan { get; set; }

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

    [Column("KELOMPOK_POKOK_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KelompokPokokBayar { get; set; }

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

    [Column("KELOMPOK_SANKSI_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KelompokSanksiBayar { get; set; }

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

    [Column("NOMINAL_JAMBONG_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalJambongBayar { get; set; }

    [Column("AKUN_JAMBONG_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? AkunJambongBayar { get; set; }

    [Column("KELOMPOK_JAMBONG_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? KelompokJambongBayar { get; set; }

    [Column("JENIS_JAMBONG_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? JenisJambongBayar { get; set; }

    [Column("OBJEK_JAMBONG_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ObjekJambongBayar { get; set; }

    [Column("RINCIAN_JAMBONG_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? RincianJambongBayar { get; set; }

    [Column("SUB_RINCIAN_JAMBONG_BAYAR")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincianJambongBayar { get; set; }

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

    [Column("NO_KETETAPAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string NoKetetapan { get; set; } = null!;
}
