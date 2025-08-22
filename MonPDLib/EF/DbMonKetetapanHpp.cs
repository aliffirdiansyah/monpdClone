using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

[PrimaryKey("Nop", "TahunPajak", "MasaPajak", "SeqPajak", "JenisKetetapan", "TahunBuku")]
[Table("DB_MON_KETETAPAN_HPP")]
public partial class DbMonKetetapanHpp
{
    [Column("ID_KETETAPAN")]
    [StringLength(150)]
    [Unicode(false)]
    public string IdKetetapan { get; set; } = null!;

    [Key]
    [Column("NOP")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nop { get; set; } = null!;

    [Key]
    [Column("TAHUN_PAJAK", TypeName = "NUMBER")]
    public decimal TahunPajak { get; set; }

    [Key]
    [Column("MASA_PAJAK", TypeName = "NUMBER")]
    public decimal MasaPajak { get; set; }

    [Key]
    [Column("SEQ_PAJAK", TypeName = "NUMBER")]
    public decimal SeqPajak { get; set; }

    [Column("TGL_SPTPD", TypeName = "DATE")]
    public DateTime TglSptpd { get; set; }

    [Column("KETETAPAN_TOTAL", TypeName = "NUMBER")]
    public decimal KetetapanTotal { get; set; }

    [Column("TGL_JATUH_TEMPO", TypeName = "DATE")]
    public DateTime TglJatuhTempo { get; set; }

    [Key]
    [Column("JENIS_KETETAPAN")]
    [StringLength(30)]
    [Unicode(false)]
    public string JenisKetetapan { get; set; } = null!;

    [Column("PAJAK_ID", TypeName = "NUMBER")]
    public decimal PajakId { get; set; }

    [Column("NAMA_PAJAK_DAERAH")]
    [StringLength(150)]
    [Unicode(false)]
    public string NamaPajakDaerah { get; set; } = null!;

    [Key]
    [Column("TAHUN_BUKU", TypeName = "NUMBER")]
    public decimal TahunBuku { get; set; }
}
