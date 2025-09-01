using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("DB_MUTASI_REKENING")]
public partial class DbMutasiRekening
{
    [Key]
    [Column("TRANSACTION_CODE")]
    [StringLength(250)]
    [Unicode(false)]
    public string TransactionCode { get; set; } = null!;

    [Column("TANGGAL_TRANSAKSI", TypeName = "DATE")]
    public DateTime TanggalTransaksi { get; set; }

    [Column("DESCRIPTION")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Description { get; set; }

    [Column("AMOUNT", TypeName = "NUMBER(18,2)")]
    public decimal? Amount { get; set; }

    [Column("FLAG")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Flag { get; set; }

    [Column("CCY")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Ccy { get; set; }

    [Column("REFFNO")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Reffno { get; set; }

    [Column("REKENING_BANK")]
    [StringLength(50)]
    [Unicode(false)]
    public string? RekeningBank { get; set; }

    [Column("REKENING_BANK_NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? RekeningBankNama { get; set; }
}
