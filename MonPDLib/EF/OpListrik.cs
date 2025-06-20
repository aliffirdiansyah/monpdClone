using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[Table("OP_LISTRIK")]
public partial class OpListrik
{
    [Key]
    [Column("NOP")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Column("SUMBER")]
    [Precision(10)]
    public int Sumber { get; set; }

    [Column("PERUNTUKAN")]
    [Precision(10)]
    public int Peruntukan { get; set; }

    [Column("JUMLAH_KARYAWAN", TypeName = "NUMBER")]
    public decimal JumlahKaryawan { get; set; }

    [Column("INS_DATE", TypeName = "DATE")]
    public DateTime InsDate { get; set; }

    [Column("INS_BY")]
    [StringLength(100)]
    [Unicode(false)]
    public string InsBy { get; set; } = null!;

    [Column("ALAT_PENGAWASAN", TypeName = "NUMBER")]
    public decimal AlatPengawasan { get; set; }

    [Column("TGL_PASANG", TypeName = "DATE")]
    public DateTime? TglPasang { get; set; }

    [ForeignKey("Nop")]
    [InverseProperty("OpListrik")]
    public virtual Op NopNavigation { get; set; } = null!;

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpListrikJadwal> OpListrikJadwals { get; set; } = new List<OpListrikJadwal>();

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpListrikKetetapan> OpListrikKetetapans { get; set; } = new List<OpListrikKetetapan>();

    [InverseProperty("NopNavigation")]
    public virtual OpListrikSumberLain? OpListrikSumberLain { get; set; }

    [InverseProperty("NopNavigation")]
    public virtual ICollection<OpListrikSumberSendiri> OpListrikSumberSendiris { get; set; } = new List<OpListrikSumberSendiri>();
}
