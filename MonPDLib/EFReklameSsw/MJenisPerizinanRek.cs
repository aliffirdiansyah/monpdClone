using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

[Table("M_JENIS_PERIZINAN_REK")]
public partial class MJenisPerizinanRek
{
    [Key]
    [Column("ID")]
    [StringLength(15)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [Column("NAMA_PERIZINAN")]
    [StringLength(500)]
    [Unicode(false)]
    public string? NamaPerizinan { get; set; }

    [Column("KET")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Ket { get; set; }

    [InverseProperty("KdPerizinanNavigation")]
    public virtual ICollection<TPerizinanReklame> TPerizinanReklames { get; set; } = new List<TPerizinanReklame>();
}
