using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_PEGAWAI_FOTO")]
public partial class MPegawaiFoto
{
    [Key]
    [Column("NIP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Column("FOTO_PEGAWAI", TypeName = "BLOB")]
    public byte[] FotoPegawai { get; set; } = null!;

    [ForeignKey("Nip")]
    [InverseProperty("MPegawaiFoto")]
    public virtual MPegawai NipNavigation { get; set; } = null!;
}
