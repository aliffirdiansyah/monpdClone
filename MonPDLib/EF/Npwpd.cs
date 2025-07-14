using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("NPWPD")]
public partial class Npwpd
{
    [Key]
    [Column("NPWPD_NO")]
    [StringLength(100)]
    [Unicode(false)]
    public string NpwpdNo { get; set; } = null!;

    [Column("STATUS", TypeName = "NUMBER(38)")]
    public decimal Status { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("ALAMAT")]
    [StringLength(255)]
    [Unicode(false)]
    public string Alamat { get; set; } = null!;

    [Column("KOTA")]
    [StringLength(5)]
    [Unicode(false)]
    public string Kota { get; set; } = null!;

    [Column("KONTAK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Kontak { get; set; }

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("REF_BLN_PEL")]
    [Precision(10)]
    public int? RefBlnPel { get; set; }

    [Column("REF_THN_PEL")]
    [Precision(10)]
    public int? RefThnPel { get; set; }

    [Column("REF_SEQ_PEL")]
    [Precision(10)]
    public int? RefSeqPel { get; set; }

    [Column("REF_WF", TypeName = "NUMBER")]
    public decimal RefWf { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("PASSWORD")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Password { get; set; }

    [Column("LAST_ACT", TypeName = "DATE")]
    public DateTime? LastAct { get; set; }

    [Column("RESET_KEY")]
    [StringLength(10)]
    [Unicode(false)]
    public string? ResetKey { get; set; }

    [Column("HP")]
    [StringLength(15)]
    [Unicode(false)]
    public string Hp { get; set; } = null!;

    [Column("NPWPD_LAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string? NpwpdLama { get; set; }

    [Column("JENIS_WP", TypeName = "NUMBER(38)")]
    public decimal JenisWp { get; set; }

    [Column("ALAMAT_DOMISILI")]
    [StringLength(255)]
    [Unicode(false)]
    public string AlamatDomisili { get; set; } = null!;

    [Column("KODE_OTP")]
    [StringLength(10)]
    [Unicode(false)]
    public string? KodeOtp { get; set; }

    [Column("IS_VALID", TypeName = "NUMBER")]
    public decimal IsValid { get; set; }
}
