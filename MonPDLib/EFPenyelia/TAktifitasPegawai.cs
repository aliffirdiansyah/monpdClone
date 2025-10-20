using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[PrimaryKey("Nip", "Nop", "IdAktifitas", "Seq")]
[Table("T_AKTIFITAS_PEGAWAI")]
public partial class TAktifitasPegawai
{
    [Key]
    [Column("NIP")]
    [StringLength(50)]
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

    [Column("PENANGGUNG_JAWAB")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PenanggungJawab { get; set; }

    [Column("NO_TELP")]
    [StringLength(20)]
    [Unicode(false)]
    public string? NoTelp { get; set; }

    [Column("TANGGAL_AKTIFITAS", TypeName = "DATE")]
    public DateTime? TanggalAktifitas { get; set; }

    [Column("KET")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [Column("LATITUDE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Latitude { get; set; }

    [Column("LONGITUDE")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Longitude { get; set; }

    [Key]
    [Column("SEQ")]
    [Precision(10)]
    public int Seq { get; set; }

    [Column("AKTIFITAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Aktifitas { get; set; }

    [ForeignKey("IdAktifitas")]
    [InverseProperty("TAktifitasPegawais")]
    public virtual MAktifita IdAktifitasNavigation { get; set; } = null!;

    [ForeignKey("Nip")]
    [InverseProperty("TAktifitasPegawais")]
    public virtual MPegawai NipNavigation { get; set; } = null!;

    [ForeignKey("Nop")]
    [InverseProperty("TAktifitasPegawais")]
    public virtual MObjekPajak NopNavigation { get; set; } = null!;

    [InverseProperty("TAktifitasPegawai")]
    public virtual TAktifitasPegawaiFile? TAktifitasPegawaiFile { get; set; }
}
