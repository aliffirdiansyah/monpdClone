using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Keyless]
[Table("BOBOT_PEGAWAI")]
public partial class BobotPegawai
{
    [Column("ID_PEGAWAI")]
    [StringLength(100)]
    [Unicode(false)]
    public string? IdPegawai { get; set; }

    [Column("NAMA_PEGAWAI")]
    [StringLength(100)]
    [Unicode(false)]
    public string NamaPegawai { get; set; } = null!;

    [Column("ID_WILAYAH", TypeName = "NUMBER")]
    public decimal IdWilayah { get; set; }

    [Column("USIA")]
    [Precision(2)]
    public byte? Usia { get; set; }

    [Column("PENDIDIKAN")]
    [Precision(2)]
    public byte? Pendidikan { get; set; }

    [Column("TUPOKSI")]
    [Precision(2)]
    public byte? Tupoksi { get; set; }

    [Column("TOTAL_BOBOT_PEGAWAI")]
    [Precision(3)]
    public byte? TotalBobotPegawai { get; set; }

    [Column("SEQ")]
    [Precision(3)]
    public byte? Seq { get; set; }

    [Column("TAHUN")]
    [Precision(4)]
    public byte? Tahun { get; set; }

    [Column("UNIT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Unit { get; set; }
}
