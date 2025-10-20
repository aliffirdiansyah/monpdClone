using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Keyless]
[Table("BOBOT_PEGAWAI_HASIL")]
public partial class BobotPegawaiHasil
{
    [Column("ID_PEGAWAI")]
    [StringLength(100)]
    [Unicode(false)]
    public string? IdPegawai { get; set; }

    [Column("UNIT")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Unit { get; set; }

    [Column("TOTAL_BOBOT_PEGAWAI")]
    [Precision(3)]
    public byte? TotalBobotPegawai { get; set; }

    [Column("TOTAL_SEMUA_PEGAWAI", TypeName = "NUMBER")]
    public decimal? TotalSemuaPegawai { get; set; }

    [Column("KOEF", TypeName = "NUMBER")]
    public decimal? Koef { get; set; }

    [Column("BOBOT_NOP", TypeName = "NUMBER")]
    public decimal? BobotNop { get; set; }

    [Column("BEBAN_BOBOT_PEGAWAI", TypeName = "NUMBER")]
    public decimal? BebanBobotPegawai { get; set; }

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal? PajakId { get; set; }
}
