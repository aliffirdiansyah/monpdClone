using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_UPTB_EVENT_GAMBAR")]
public partial class MUptbEventGambar
{
    [Key]
    [Column("ID_EVENT", TypeName = "NUMBER(38)")]
    public decimal IdEvent { get; set; }

    [Column("GAMBAR", TypeName = "BLOB")]
    public byte[] Gambar { get; set; } = null!;

    [Column("DESKRIPSI")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Deskripsi { get; set; }

    [ForeignKey("IdEvent")]
    [InverseProperty("MUptbEventGambar")]
    public virtual MUptbEvent IdEventNavigation { get; set; } = null!;
}
