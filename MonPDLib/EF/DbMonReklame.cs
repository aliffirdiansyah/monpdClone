using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("DB_MON_REKLAME")]
public partial class DbMonReklame
{
    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("ID_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? IdKetetapan { get; set; }

    [Column("TGLPENETAPAN", TypeName = "DATE")]
    public DateTime? Tglpenetapan { get; set; }

    [Column("TAHUN_PAJAK", TypeName = "NUMBER")]
    public decimal? TahunPajak { get; set; }

    [Column("BULAN_PAJAK", TypeName = "NUMBER")]
    public decimal? BulanPajak { get; set; }

    [Column("PAJAK_POKOK", TypeName = "NUMBER")]
    public decimal? PajakPokok { get; set; }

    [Column("JNS_KETETAPAN", TypeName = "NUMBER")]
    public decimal? JnsKetetapan { get; set; }

    [Column("TGL_JTEMPO_SKPD", TypeName = "DATE")]
    public DateTime? TglJtempoSkpd { get; set; }

    [Column("AKUN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? Akun { get; set; }

    [Column("NAMA_AKUN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaAkun { get; set; }

    [Column("KELOMPOK")]
    [StringLength(3)]
    [Unicode(false)]
    public string? Kelompok { get; set; }

    [Column("NAMA_KELOMPOK")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaKelompok { get; set; }

    [Column("JENIS")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Jenis { get; set; }

    [Column("NAMA_JENIS")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaJenis { get; set; }

    [Column("OBJEK")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Objek { get; set; }

    [Column("NAMA_OBJEK")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaObjek { get; set; }

    [Column("RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Rincian { get; set; }

    [Column("NAMA_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaRincian { get; set; }

    [Column("SUB_RINCIAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? SubRincian { get; set; }

    [Column("NAMA_SUB_RINCIAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? NamaSubRincian { get; set; }

    [Column("TAHUN_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? TahunPajakKetetapan { get; set; }

    [Column("MASA_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? MasaPajakKetetapan { get; set; }

    [Column("SEQ_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? SeqPajakKetetapan { get; set; }

    [Column("KATEGORI_KETETAPAN")]
    [StringLength(16)]
    [Unicode(false)]
    public string? KategoriKetetapan { get; set; }

    [Column("TGL_KETETAPAN", TypeName = "DATE")]
    public DateTime? TglKetetapan { get; set; }

    [Column("IS_LUNAS_KETETAPAN", TypeName = "NUMBER")]
    public decimal? IsLunasKetetapan { get; set; }

    [Column("TGL_LUNAS_KETETAPAN", TypeName = "DATE")]
    public DateTime? TglLunasKetetapan { get; set; }

    [Column("POKOK_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? PokokPajakKetetapan { get; set; }

    [Column("PENGURANG_POKOK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? PengurangPokokKetetapan { get; set; }

    [Column("AKUN_KETETAPAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AkunKetetapan { get; set; }

    [Column("KELOMPOK_KETETAPAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KelompokKetetapan { get; set; }

    [Column("JENIS_KETETAPAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? JenisKetetapan { get; set; }

    [Column("OBJEK_KETETAPAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? ObjekKetetapan { get; set; }

    [Column("RINCIAN_KETETAPAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? RincianKetetapan { get; set; }

    [Column("SUB_RINCIAN_KETETAPAN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? SubRincianKetetapan { get; set; }

    [Column("TGL_BAYAR_POKOK", TypeName = "DATE")]
    public DateTime? TglBayarPokok { get; set; }

    [Column("NOMINAL_POKOK_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalPokokBayar { get; set; }

    [Column("TGL_BAYAR_SANKSI", TypeName = "DATE")]
    public DateTime? TglBayarSanksi { get; set; }

    [Column("NOMINAL_SANKSI_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalSanksiBayar { get; set; }

    [Column("AKUN_SANKSI_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AkunSanksiBayar { get; set; }

    [Column("KELOMPOK_SANKSI_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KelompokSanksiBayar { get; set; }

    [Column("JENIS_SANKSI_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? JenisSanksiBayar { get; set; }

    [Column("OBJEK_SANKSI_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? ObjekSanksiBayar { get; set; }

    [Column("RINCIAN_SANKSI_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? RincianSanksiBayar { get; set; }

    [Column("SUB_RINCIAN_SANKSI_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? SubRincianSanksiBayar { get; set; }

    [Column("TGL_BAYAR_SANKSI_KENAIKAN", TypeName = "DATE")]
    public DateTime? TglBayarSanksiKenaikan { get; set; }

    [Column("NOMINAL_JAMBONG_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalJambongBayar { get; set; }

    [Column("AKUN_JAMBONG_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? AkunJambongBayar { get; set; }

    [Column("KELOMPOK_JAMBONG_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? KelompokJambongBayar { get; set; }

    [Column("JENIS_JAMBONG_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? JenisJambongBayar { get; set; }

    [Column("OBJEK_JAMBONG_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? ObjekJambongBayar { get; set; }

    [Column("RINCIAN_JAMBONG_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? RincianJambongBayar { get; set; }

    [Column("SUB_RINCIAN_JAMBONG_BAYAR")]
    [StringLength(1)]
    [Unicode(false)]
    public string? SubRincianJambongBayar { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime? InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(3)]
    [Unicode(false)]
    public string? InsBy { get; set; }

    [Column("UPD_DATE", TypeName = "DATE")]
    public DateTime? UpdDate { get; set; }

    [Column("UPD_BY")]
    [StringLength(3)]
    [Unicode(false)]
    public string? UpdBy { get; set; }

    [Column("NO_KETETAPAN")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoKetetapan { get; set; }

    [Column("SEQ", TypeName = "NUMBER")]
    public decimal? Seq { get; set; }
}
