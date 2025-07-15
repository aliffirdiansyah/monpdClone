using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "Tanggal", "Seq")]
[Table("DB_REKAM_RESTORAN")]
public partial class DbRekamRestoran
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TANGGAL", TypeName = "DATE")]
    public DateTime Tanggal { get; set; }

    [Column("JML_MEJA", TypeName = "NUMBER")]
    public decimal JmlMeja { get; set; }

    [Column("JML_KURSI", TypeName = "NUMBER")]
    public decimal JmlKursi { get; set; }

    [Column("JML_PENGUNJUNG", TypeName = "NUMBER")]
    public decimal JmlPengunjung { get; set; }

    [Column("BILL", TypeName = "NUMBER")]
    public decimal Bill { get; set; }

    [Column("RATA_PENGUNJUNG_HARI", TypeName = "NUMBER")]
    public decimal RataPengunjungHari { get; set; }

    [Column("RATA_BILL_PENGUNJUNG", TypeName = "NUMBER")]
    public decimal RataBillPengunjung { get; set; }

    [Column("OMSE_BULAN", TypeName = "NUMBER")]
    public decimal OmseBulan { get; set; }

    [Column("PAJAK_BULAN", TypeName = "NUMBER")]
    public decimal PajakBulan { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal PajakId { get; set; }

    [Key]
    [Column("SEQ", TypeName = "NUMBER")]
    public decimal Seq { get; set; }
}
