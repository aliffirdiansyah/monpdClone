using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("T_BERITA_ISI")]
public partial class TBeritaIsi
{
    [Key]
    [Column("ID_BERITA", TypeName = "NUMBER(38)")]
    public decimal IdBerita { get; set; }

    [Column("ISI_HTML", TypeName = "BLOB")]
    public byte[] IsiHtml { get; set; } = null!;

    [ForeignKey("IdBerita")]
    [InverseProperty("TBeritaIsi")]
    public virtual TBeritum IdBeritaNavigation { get; set; } = null!;
}
