using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[PrimaryKey("Nik", "Seq")]
[Table("CHAT_MESSAGE")]
public partial class ChatMessage
{
    [Key]
    [Column("NIK")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nik { get; set; } = null!;

    [Column("USERNAME")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Username { get; set; }

    [Key]
    [Column("SEQ", TypeName = "NUMBER(38)")]
    public decimal Seq { get; set; }

    [Column("MESSAGE")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? Message { get; set; }

    [Column("BLOB_FILE", TypeName = "BLOB")]
    public byte[]? BlobFile { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [ForeignKey("Nik")]
    [InverseProperty("ChatMessages")]
    public virtual ChatUser NikNavigation { get; set; } = null!;

    [ForeignKey("Username")]
    [InverseProperty("ChatMessages")]
    public virtual MUcUserLogin? UsernameNavigation { get; set; }
}
