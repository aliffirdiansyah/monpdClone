using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_OP_REKLAME")]
public partial class DbOpReklame
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

    [Column("NAMA_JENIS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

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
}
