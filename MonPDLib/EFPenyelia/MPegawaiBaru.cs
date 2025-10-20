using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

[Table("M_PEGAWAI_BARU")]
public partial class MPegawaiBaru
{
    [Key]
    [Column("ID", TypeName = "NUMBER")]
    public decimal Id { get; set; }

    [Column("NAMA")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nama { get; set; } = null!;

    [Column("NIP_NIK")]
    [StringLength(30)]
    [Unicode(false)]
    public string? NipNik { get; set; }

    [Column("STATUS_PEGAWAI")]
    [StringLength(20)]
    [Unicode(false)]
    public string? StatusPegawai { get; set; }

    [Column("UNIT_KERJA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UnitKerja { get; set; }

    [Column("PANGKAT_KELAS_JABATAN")]
    [StringLength(100)]
    [Unicode(false)]
    public string? PangkatKelasJabatan { get; set; }

    [Column("JABATAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string? Jabatan { get; set; }

    [Column("JENIS_KELAMIN")]
    [StringLength(1)]
    [Unicode(false)]
    public string? JenisKelamin { get; set; }

    [Column("JENJANG_TERAKHIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? JenjangTerakhir { get; set; }

    [Column("KET")]
    [Precision(1)]
    public bool? Ket { get; set; }
}
