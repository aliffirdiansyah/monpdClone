using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDReborn.EF;

[Table("OP")]
public partial class Op
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

    [Column("PAJAK_ID")]
    [Precision(10)]
    public int PajakId { get; set; }

    [Column("NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("ALAMAT")]
    [StringLength(250)]
    [Unicode(false)]
    public string Alamat { get; set; } = null!;

    [Column("ALAMAT_NO")]
    [StringLength(50)]
    [Unicode(false)]
    public string AlamatNo { get; set; } = null!;

    [Column("RT")]
    [StringLength(5)]
    [Unicode(false)]
    public string Rt { get; set; } = null!;

    [Column("RW")]
    [StringLength(5)]
    [Unicode(false)]
    public string Rw { get; set; } = null!;

    [Column("TELP")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Telp { get; set; }

    [Column("KD_LURAH")]
    [StringLength(5)]
    [Unicode(false)]
    public string KdLurah { get; set; } = null!;

    [Column("KD_CAMAT")]
    [StringLength(5)]
    [Unicode(false)]
    public string KdCamat { get; set; } = null!;

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("TGL_MULAI_BUKA_OP", TypeName = "DATE")]
    public DateTime TglMulaiBukaOp { get; set; }

    [Column("AKTIF")]
    [Precision(10)]
    public int Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [ForeignKey("KdCamat, KdLurah")]
    [InverseProperty("Ops")]
    public virtual MWilayah Kd { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual OpAbt? OpAbt { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual OpHiburan? OpHiburan { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual OpHotel? OpHotel { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual OpListrik? OpListrik { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual OpParkir? OpParkir { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual OpResto? OpResto { get; set; }

    [ForeignKey("PajakId")]
    [InverseProperty("Ops")]
    public virtual MPajak Pajak { get; set; } = null!;
}
