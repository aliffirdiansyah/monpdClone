using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[PrimaryKey("Nip", "Nop", "IdAktifitas", "Seq")]
[Table("T_AKTIFITAS_PEGAWAI_FILE")]
public partial class TAktifitasPegawaiFile
{
    [Key]
    [Column("NIP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nip { get; set; } = null!;

    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("ID_AKTIFITAS")]
    [Precision(18)]
    public long IdAktifitas { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Column("FILE_DATA", TypeName = "BLOB")]
    public byte[] FileData { get; set; } = null!;

    [Column("FOTO_FILE", TypeName = "BLOB")]
    public byte[] FotoFile { get; set; } = null!;

    [ForeignKey("Nip, Nop, IdAktifitas, Seq")]
    [InverseProperty("TAktifitasPegawaiFile")]
    public virtual TAktifitasPegawai TAktifitasPegawai { get; set; } = null!;
}
