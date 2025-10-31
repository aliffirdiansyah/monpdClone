using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_UPTB_STRUKTUR")]
public partial class MUptbStruktur
{
    [Key]
    [Column("UPTB", TypeName = "NUMBER")]
    public decimal Uptb { get; set; }

    [Column("STRUKTUR_ORGANISASI", TypeName = "BLOB")]
    public byte[] StrukturOrganisasi { get; set; } = null!;

    [ForeignKey("Uptb")]
    [InverseProperty("MUptbStruktur")]
    public virtual MUptb UptbNavigation { get; set; } = null!;
}
