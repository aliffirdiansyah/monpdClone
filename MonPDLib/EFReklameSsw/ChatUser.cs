using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("CHAT_USERS")]
public partial class ChatUser
{
    [Key]
    [Column("NIK")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nik { get; set; } = null!;

    [Column("PASSWORD")]
    [StringLength(500)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("LAST_ACT", TypeName = "DATE")]
    public DateTime? LastAct { get; set; }

    [Column("RESET_KEY")]
    [StringLength(6)]
    [Unicode(false)]
    public string? ResetKey { get; set; }

    [Column("AKTIF", TypeName = "NUMBER(38)")]
    public decimal Aktif { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(50)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [InverseProperty("NikNavigation")]
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
}
