using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Keyless]
public partial class DbMonReklameAktivitasDet
{
    [Column("TAHUN", TypeName = "NUMBER")]
    public decimal? Tahun { get; set; }

    [Column("BULAN", TypeName = "NUMBER")]
    public decimal? Bulan { get; set; }

    [Column("NOR")]
    [StringLength(12)]
    [Unicode(false)]
    public string? Nor { get; set; }

    [Column("NO_FORMULIR")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NoFormulir { get; set; }

    [Column("TANGGAL", TypeName = "DATE")]
    public DateTime? Tanggal { get; set; }

    [Column("AKTIFITAS")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Aktifitas { get; set; }

    [Column("PETUGAS")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Petugas { get; set; }

    [Column("NAMA_PERUSAHAAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? NamaPerusahaan { get; set; }

    [Column("NAMA")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nama { get; set; }

    [Column("ALAMAT_PERUSAHAAN")]
    [StringLength(255)]
    [Unicode(false)]
    public string? AlamatPerusahaan { get; set; }

    [Column("ALAMATREKLAME")]
    [StringLength(181)]
    [Unicode(false)]
    public string? Alamatreklame { get; set; }

    [Column("ISI_REKLAME")]
    [StringLength(255)]
    [Unicode(false)]
    public string? IsiReklame { get; set; }

    [Column("POKOK_PAJAK_KETETAPAN", TypeName = "NUMBER")]
    public decimal? PokokPajakKetetapan { get; set; }

    [Column("NOMINAL_POKOK_BAYAR", TypeName = "NUMBER")]
    public decimal? NominalPokokBayar { get; set; }
}
