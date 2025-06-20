using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Id", "Nop", "TahunPajak", "MasaPajak", "TglPenungguan", "JamMulai")]
[Table("T_PENUNGGUAN_SPTPD_MAMIN")]
public partial class TPenungguanSptpdMamin
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TAHUN_PAJAK")]
    [Precision(10)]
    public int TahunPajak { get; set; }

    [Key]
    [Column("MASA_PAJAK")]
    [Precision(10)]
    public int MasaPajak { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string Petugas { get; set; } = null!;

    [Key]
    [Column("TGL_PENUNGGUAN", TypeName = "DATE")]
    public DateTime TglPenungguan { get; set; }

    [Key]
    [Column("JAM_MULAI", TypeName = "DATE")]
    public DateTime JamMulai { get; set; }

    [Column("JAM_SELESAI", TypeName = "DATE")]
    public DateTime JamSelesai { get; set; }

    [Column("JUMLAH_PENGUNJUNG", TypeName = "NUMBER")]
    public decimal JumlahPengunjung { get; set; }

    [Column("OMSET_PENJUALAN", TypeName = "NUMBER")]
    public decimal OmsetPenjualan { get; set; }

    [Column("JUMLAH_MEJA", TypeName = "NUMBER")]
    public decimal JumlahMeja { get; set; }

    [Column("JUMLAH_KURSI", TypeName = "NUMBER")]
    public decimal JumlahKursi { get; set; }

    [Column("KETERANGAN")]
    [StringLength(1000)]
    [Unicode(false)]
    public string Keterangan { get; set; } = null!;

    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("TPenungguanSptpdMamins")]
    public virtual TPenungguanSptpd IdNavigation { get; set; } = null!;
}
