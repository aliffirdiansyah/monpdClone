using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
[Table("PBBREALISASI_BERJALANBACK")]
public partial class PbbrealisasiBerjalanback
{
    [Column("T_KEC_KD")]
    [StringLength(3)]
    [Unicode(false)]
    public string? TKecKd { get; set; }

    [Column("T_KEL_KD")]
    [StringLength(3)]
    [Unicode(false)]
    public string? TKelKd { get; set; }

    [Column("D_NOP_BLK")]
    [StringLength(3)]
    [Unicode(false)]
    public string? DNopBlk { get; set; }

    [Column("D_NOP_URUT")]
    [StringLength(4)]
    [Unicode(false)]
    public string? DNopUrut { get; set; }

    [Column("D_NOP_JNS")]
    [StringLength(1)]
    [Unicode(false)]
    public string? DNopJns { get; set; }

    [Column("D_PJK_THN")]
    [StringLength(4)]
    [Unicode(false)]
    public string? DPjkThn { get; set; }

    [Column("NO_DOKUMEN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? NoDokumen { get; set; }

    [Column("D_PJK_PBB", TypeName = "NUMBER")]
    public decimal? DPjkPbb { get; set; }

    [Column("DENDA", TypeName = "NUMBER")]
    public decimal? Denda { get; set; }

    [Column("D_PJK_JMBYR", TypeName = "NUMBER")]
    public decimal? DPjkJmbyr { get; set; }

    [Column("TGL_BYR", TypeName = "DATE")]
    public DateTime? TglByr { get; set; }

    [Column("TEMPATBAYAR")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Tempatbayar { get; set; }

    [Column("TGL_CREATE", TypeName = "DATE")]
    public DateTime? TglCreate { get; set; }

    [Column("D_MODI_USER")]
    [StringLength(100)]
    [Unicode(false)]
    public string? DModiUser { get; set; }

    [Column("TAHUN_BUKU")]
    [StringLength(4)]
    [Unicode(false)]
    public string? TahunBuku { get; set; }
}
