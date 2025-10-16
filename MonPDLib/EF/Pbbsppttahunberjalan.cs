using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("SpptProp", "SpptKota", "SpptKec", "SpptKel", "SpptUrutblk", "SpptUrutop", "SpptTanda")]
[Table("PBBSPPTTAHUNBERJALAN")]
public partial class Pbbsppttahunberjalan
{
    [Key]
    [Column("SPPT_PROP")]
    [StringLength(2)]
    [Unicode(false)]
    public string SpptProp { get; set; } = null!;

    [Key]
    [Column("SPPT_KOTA")]
    [StringLength(2)]
    [Unicode(false)]
    public string SpptKota { get; set; } = null!;

    [Key]
    [Column("SPPT_KEC")]
    [StringLength(3)]
    [Unicode(false)]
    public string SpptKec { get; set; } = null!;

    [Key]
    [Column("SPPT_KEL")]
    [StringLength(3)]
    [Unicode(false)]
    public string SpptKel { get; set; } = null!;

    [Key]
    [Column("SPPT_URUTBLK")]
    [StringLength(3)]
    [Unicode(false)]
    public string SpptUrutblk { get; set; } = null!;

    [Key]
    [Column("SPPT_URUTOP")]
    [StringLength(4)]
    [Unicode(false)]
    public string SpptUrutop { get; set; } = null!;

    [Key]
    [Column("SPPT_TANDA")]
    [StringLength(1)]
    [Unicode(false)]
    public string SpptTanda { get; set; } = null!;

    [Column("TAHUN_PBB")]
    [StringLength(4)]
    [Unicode(false)]
    public string TahunPbb { get; set; } = null!;

    [Column("KODE_KOTA_LOP")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KodeKotaLop { get; set; }

    [Column("KODE_CAMAT_LOP")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KodeCamatLop { get; set; }

    [Column("KODE_LURAH_LOP")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KodeLurahLop { get; set; }

    [Column("JALAN_LOP")]
    [StringLength(120)]
    [Unicode(false)]
    public string? JalanLop { get; set; }

    [Column("BLOK_LOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string? BlokLop { get; set; }

    [Column("JALAN_NO_LOP")]
    [StringLength(15)]
    [Unicode(false)]
    public string? JalanNoLop { get; set; }

    [Column("RT_LOP")]
    [StringLength(3)]
    [Unicode(false)]
    public string? RtLop { get; set; }

    [Column("RW_LOP")]
    [StringLength(2)]
    [Unicode(false)]
    public string? RwLop { get; set; }

    [Column("NO_WP", TypeName = "NUMBER")]
    public decimal? NoWp { get; set; }

    [Column("NAMA_WP")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NamaWp { get; set; }

    [Column("JALAN_WP")]
    [StringLength(120)]
    [Unicode(false)]
    public string? JalanWp { get; set; }

    [Column("BLOK_WP")]
    [StringLength(15)]
    [Unicode(false)]
    public string? BlokWp { get; set; }

    [Column("JALAN_NO_WP")]
    [StringLength(15)]
    [Unicode(false)]
    public string? JalanNoWp { get; set; }

    [Column("RT_WP")]
    [StringLength(3)]
    [Unicode(false)]
    public string? RtWp { get; set; }

    [Column("RW_WP")]
    [StringLength(2)]
    [Unicode(false)]
    public string? RwWp { get; set; }

    [Column("KEL_WP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? KelWp { get; set; }

    [Column("KOTA_WP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? KotaWp { get; set; }

    [Column("KD_POS_WP_SPPT")]
    [StringLength(5)]
    [Unicode(false)]
    public string? KdPosWpSppt { get; set; }

    [Column("NO_PERSIL_SPPT")]
    [StringLength(5)]
    [Unicode(false)]
    public string? NoPersilSppt { get; set; }

    [Column("NPWP")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Npwp { get; set; }

    [Column("OP_BUMI")]
    [StringLength(10)]
    [Unicode(false)]
    public string? OpBumi { get; set; }

    [Column("LUAS_BUMI", TypeName = "NUMBER")]
    public decimal? LuasBumi { get; set; }

    [Column("KELAS_BUMI")]
    [StringLength(4)]
    [Unicode(false)]
    public string? KelasBumi { get; set; }

    [Column("THN_AWAL_KLS_TANAH")]
    [StringLength(4)]
    [Unicode(false)]
    public string? ThnAwalKlsTanah { get; set; }

    [Column("NJOP_BUMI", TypeName = "NUMBER")]
    public decimal? NjopBumi { get; set; }

    [Column("TOTAL_NJOP_BUMI", TypeName = "NUMBER")]
    public decimal? TotalNjopBumi { get; set; }

    [Column("OP_BANGUNAN")]
    [StringLength(10)]
    [Unicode(false)]
    public string? OpBangunan { get; set; }

    [Column("LUAS_BANGUNAN", TypeName = "NUMBER")]
    public decimal? LuasBangunan { get; set; }

    [Column("KELAS_BANGUN")]
    [StringLength(4)]
    [Unicode(false)]
    public string? KelasBangun { get; set; }

    [Column("THN_AWAL_KLS_BNG")]
    [StringLength(4)]
    [Unicode(false)]
    public string? ThnAwalKlsBng { get; set; }

    [Column("NJOP_BANGUNAN", TypeName = "NUMBER")]
    public decimal? NjopBangunan { get; set; }

    [Column("TOTAL_NJOP_BANGUNAN", TypeName = "NUMBER")]
    public decimal? TotalNjopBangunan { get; set; }

    [Column("LUAS_BUMIB", TypeName = "NUMBER")]
    public decimal? LuasBumib { get; set; }

    [Column("KELAS_BUMIB")]
    [StringLength(4)]
    [Unicode(false)]
    public string? KelasBumib { get; set; }

    [Column("NJOP_BUMIB", TypeName = "NUMBER")]
    public decimal? NjopBumib { get; set; }

    [Column("TOTAL_NJOP_BUMIB", TypeName = "NUMBER")]
    public decimal? TotalNjopBumib { get; set; }

    [Column("LUAS_BANGUNANB", TypeName = "NUMBER")]
    public decimal? LuasBangunanb { get; set; }

    [Column("KELAS_BANGUNANB")]
    [StringLength(4)]
    [Unicode(false)]
    public string? KelasBangunanb { get; set; }

    [Column("NJOP_BANGUNANB", TypeName = "NUMBER")]
    public decimal? NjopBangunanb { get; set; }

    [Column("TOTAL_NJOP_BANGUNANB", TypeName = "NUMBER")]
    public decimal? TotalNjopBangunanb { get; set; }

    [Column("NJOP_DASAR_PBB", TypeName = "NUMBER")]
    public decimal? NjopDasarPbb { get; set; }

    [Column("NJOPTKP", TypeName = "NUMBER")]
    public decimal? Njoptkp { get; set; }

    [Column("NJOP_HIT_PBB", TypeName = "NUMBER")]
    public decimal? NjopHitPbb { get; set; }

    [Column("NJKP_PERSEN", TypeName = "NUMBER")]
    public decimal? NjkpPersen { get; set; }

    [Column("NJKP_RP", TypeName = "NUMBER")]
    public decimal? NjkpRp { get; set; }

    [Column("PBB_HUT_PERSEN", TypeName = "NUMBER")]
    public decimal? PbbHutPersen { get; set; }

    [Column("PBB_HUT_RUPIAH", TypeName = "NUMBER")]
    public decimal? PbbHutRupiah { get; set; }

    [Column("TGL_JATUH_TEMPO_SPPT", TypeName = "DATE")]
    public DateTime? TglJatuhTempoSppt { get; set; }

    [Column("AR_PERSEN", TypeName = "NUMBER")]
    public decimal? ArPersen { get; set; }

    [Column("AR_RUPIAH", TypeName = "NUMBER")]
    public decimal? ArRupiah { get; set; }

    [Column("PBB_HUT_RUPIAHLAMA", TypeName = "NUMBER")]
    public decimal? PbbHutRupiahlama { get; set; }

    [Column("KD_CAMAT")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KdCamat { get; set; }

    [Column("KD_LURAH")]
    [StringLength(3)]
    [Unicode(false)]
    public string? KdLurah { get; set; }

    [Column("KECAMATAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Kecamatan { get; set; }

    [Column("KELURAHAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Kelurahan { get; set; }

    [Column("BAYARBERJALAN", TypeName = "NUMBER")]
    public decimal? Bayarberjalan { get; set; }

    [Column("ADJ", TypeName = "NUMBER")]
    public decimal? Adj { get; set; }

    [Column("SISA_TUNGBERJALAN", TypeName = "NUMBER")]
    public decimal? SisaTungberjalan { get; set; }

    [Column("LEBIHBAYAR", TypeName = "NUMBER")]
    public decimal? Lebihbayar { get; set; }

    [Column("KD_JPB")]
    [StringLength(2)]
    [Unicode(false)]
    public string? KdJpb { get; set; }

    [Column("STATUS_BANGUNAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? StatusBangunan { get; set; }

    [Column("KATEGORITUNGGAKAN")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Kategoritunggakan { get; set; }

    [Column("TGL_INSERT", TypeName = "DATE")]
    public DateTime? TglInsert { get; set; }
}
