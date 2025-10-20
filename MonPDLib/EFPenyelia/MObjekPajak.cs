using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Table("M_OBJEK_PAJAK")]
public partial class MObjekPajak
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
    [StringLength(250)]
    [Unicode(false)]
    public string NpwpdNama { get; set; } = null!;

    [Column("NPWPD_ALAMAT")]
    [StringLength(250)]
    [Unicode(false)]
    public string NpwpdAlamat { get; set; } = null!;

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal PajakId { get; set; }

    [Column("KATEGORI_PAJAK", TypeName = "NUMBER")]
    public decimal? KategoriPajak { get; set; }

    [Column("NAMA_OP")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaOp { get; set; } = null!;

    [Column("ALAMAT_OP")]
    [StringLength(250)]
    [Unicode(false)]
    public string AlamatOp { get; set; } = null!;

    [Column("TGL_OP_TUTUP", TypeName = "DATE")]
    public DateTime? TglOpTutup { get; set; }

    [Column("TGL_MULAI_BUKA_OP", TypeName = "DATE")]
    public DateTime TglMulaiBukaOp { get; set; }

    [Column("KEL")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Kel { get; set; }

    [Column("KEC")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Kec { get; set; }

    [Column("WILAYAH_PAJAK")]
    [StringLength(100)]
    [Unicode(false)]
    public string? WilayahPajak { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(30)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<MPegawaiOpDet> MPegawaiOpDets { get; set; } = new List<MPegawaiOpDet>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<TAktifitasPegawai> TAktifitasPegawais { get; set; } = new List<TAktifitasPegawai>();
}
