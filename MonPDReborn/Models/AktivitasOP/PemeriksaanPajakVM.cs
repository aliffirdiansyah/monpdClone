using MonPDLib;
using MonPDLib.General;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PemeriksaanPajakVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Dashboard dashboard { get; set; } = new();

            public Index()
            {
                dashboard = Method.GetDashboardAllPajak();
            }
        }

        public class Show
        {
            public List<DataPemeriksaan> DataPemeriksaanList { get; set; } = new();
            
            public Show()
            {
                DataPemeriksaanList = Method.GetPemeriksaanList();
                
            }
        }

        // ======= DETAIL ==========
        public class Detail
        {
            public List<DataDetailPemeriksaan> DataDetailList { get; set; } = new();

            public Detail() { }

            public Detail(int jenisPajak, int tahun)
            {
                DataDetailList = Method.GetDetailPemeriksaanList(jenisPajak, tahun);
            }
        }

        // ======= METHOD ==========
        public class Method
        {
            public static List<DataPemeriksaan> GetPemeriksaanList()
            {
                var ret = new List<DataPemeriksaan>();
                var context = DBClass.GetContext();

                var query = context.TPemeriksaans.AsQueryable();

                foreach (var item in query.Select(x => x.PajakId).Distinct().ToList())
                {
                    var col = new DataPemeriksaan();
                    col.JenisPajak = ((EnumFactory.EPajak)item).GetDescription();

                    col.JumlahOPMines2 = query.Count(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year - 2);
                    col.JumlahOPMines1 = query.Count(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year - 1);
                    col.JumlahOPNow = query.Count(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year);

                    col.PokokMines2 = query.Where(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year - 2).Sum(x => x.Pokok);
                    col.SanksiMines2 = query.Where(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year - 2).Sum(x => x.Denda);

                    col.PokokMines1 = query.Where(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year - 1).Sum(x => x.Pokok);
                    col.SanksiMines1 = query.Where(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year - 1).Sum(x => x.Denda);

                    col.PokokNow = query.Where(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year).Sum(x => x.Pokok);
                    col.SanksiNow = query.Where(x => x.PajakId == item && x.TahunPajak == DateTime.Now.Year).Sum(x => x.Denda);

                    ret.Add(col);
                }

                return ret;

            }

            public static List<DataDetailPemeriksaan> GetDetailPemeriksaanList(int jenisPajak, int tahun)
            {
                var ret = new List<DataDetailPemeriksaan>();
                var context = DBClass.GetContext();

                var pemeriksaans = context.TPemeriksaans
                    .Where(x => x.PajakId == jenisPajak && x.TahunPajak == tahun)
                    .ToList();
                if (jenisPajak == 1)
                {
                    var dbMamin = context.DbOpRestos
                        .GroupBy(x => x.Nop)
                        .ToDictionary(g => g.Key, g => g.First());

                    ret = pemeriksaans.Select(x => new DataDetailPemeriksaan
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        NOP = x.Nop ?? "-",
                        WajibPajak = dbMamin.ContainsKey(x.Nop) ? dbMamin[x.Nop].NpwpdNama ?? "-" : "-",
                        Alamat = dbMamin.ContainsKey(x.Nop) ? dbMamin[x.Nop].AlamatOp ?? "-" : "-",
                        UPTB = dbMamin.ContainsKey(x.Nop) ? dbMamin[x.Nop].WilayahPajak ?? "-" : "-",
                        NoSP = x.NoSp ?? "-",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = x.JumlahKb ?? 0,
                        Keterangan = x.Ket ?? "-",
                        LHP = x.Lhp ??  "-",
                        TglLHP = x.TglLhp,
                        TglBayar = x.TglByr,
                        Tim = x.Petugas ?? "-"
                    }).ToList();
                }
                else if (jenisPajak == 3) // Hotel
                {
                    // ✅ Hindari duplikat key NOP
                    var dbHotel = context.DbOpHotels
                        .GroupBy(x => x.Nop)
                        .ToDictionary(g => g.Key, g => g.First());

                    ret = pemeriksaans.Select(x => new DataDetailPemeriksaan
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        NOP = x.Nop ?? "-",
                        WajibPajak = dbHotel.ContainsKey(x.Nop) ? dbHotel[x.Nop].NpwpdNama ?? "-" : "-",
                        Alamat = dbHotel.ContainsKey(x.Nop) ? dbHotel[x.Nop].AlamatOp ?? "-" : "-",
                        UPTB = dbHotel.ContainsKey(x.Nop) ? dbHotel[x.Nop].WilayahPajak ?? "-" : "-",
                        NoSP = x.NoSp ?? "-",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = x.JumlahKb ?? 0,
                        Keterangan = x.Ket ?? "-",
                        LHP = x.Lhp ?? "-",
                        TglLHP = x.TglLhp,
                        TglBayar = x.TglByr,
                        Tim = x.Petugas ?? "-"
                    }).ToList();
                }
                else if (jenisPajak == 4)
                {
                    var dbParkir = context.DbOpParkirs
                        .GroupBy(x => x.Nop)
                        .ToDictionary(g => g.Key, g => g.First());

                    ret = pemeriksaans.Select(x => new DataDetailPemeriksaan
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        NOP = x.Nop ?? "-",
                        WajibPajak = dbParkir.ContainsKey(x.Nop) ? dbParkir[x.Nop].NpwpdNama ?? "-" : "-",
                        Alamat = dbParkir.ContainsKey(x.Nop) ? dbParkir[x.Nop].AlamatOp ?? "-" : "-",
                        UPTB = dbParkir.ContainsKey(x.Nop) ? dbParkir[x.Nop].WilayahPajak ?? "-" : "-",
                        NoSP = x.NoSp ?? "-",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = x.JumlahKb ?? 0,
                        Keterangan = x.Ket ?? "-",
                        LHP = x.Lhp ?? "-",
                        TglLHP = x.TglLhp,
                        TglBayar = x.TglByr,
                        Tim = x.Petugas ?? "-"
                    }).ToList();
                }
                else if (jenisPajak == 5)
                {
                    var dbHiburan = context.DbOpHiburans
                        .GroupBy(x => x.Nop)
                        .ToDictionary(g => g.Key, g => g.First());

                    ret = pemeriksaans.Select(x => new DataDetailPemeriksaan
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        NOP = x.Nop ?? "-",
                        WajibPajak = dbHiburan.ContainsKey(x.Nop) ? dbHiburan[x.Nop].NpwpdNama ?? "-" : "-",
                        Alamat = dbHiburan.ContainsKey(x.Nop) ? dbHiburan[x.Nop].AlamatOp ?? "-" : "-",
                        UPTB = dbHiburan.ContainsKey(x.Nop) ? dbHiburan[x.Nop].WilayahPajak ?? "-" : "-",
                        NoSP = x.NoSp ?? "-",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = x.JumlahKb ?? 0,
                        Keterangan = x.Ket ?? "-",
                        LHP = x.Lhp ?? "-",
                        TglLHP = x.TglLhp,
                        TglBayar = x.TglByr,
                        Tim = x.Petugas ?? "-"
                    }).ToList();
                }
                    
                    return ret;
            }

            public static Dashboard GetDashboardAllPajak()
            {
                var context = DBClass.GetContext();
                int tahunIni = DateTime.Now.Year;

                var data = context.TPemeriksaans
                    .Where(x => x.TahunPajak == tahunIni)
                    .ToList();

                var totalOP = data.Count;
                var totalKurangBayar = data.Sum(x => x.JumlahKb ?? 0);
                var rataKurangBayar = totalOP > 0 ? totalKurangBayar / totalOP : 0;

                return new Dashboard
                {
                    TotalOP = totalOP,
                    TotalKurangBayar = totalKurangBayar,
                    RataKurangBayar = rataKurangBayar
                };
            }

        }

        // ======= ENTITY UTAMA ==========
        public class DataPemeriksaan
        {
            public string JenisPajak { get; set; } = null!;
            public int JumlahOPMines2 { get; set; }
            public int JumlahOPMines1 { get; set; }
            public int JumlahOPNow { get; set; }

            public decimal PokokMines2 { get; set; }
            public decimal SanksiMines2 { get; set; }
            public decimal TotalMines2 => PokokMines2 + SanksiMines2;

            public decimal PokokMines1 { get; set; }
            public decimal SanksiMines1 { get; set; }
            public decimal TotalMines1 => PokokMines1 + SanksiMines1;

            public decimal PokokNow { get; set; }
            public decimal SanksiNow { get; set; }
            public decimal TotalNow => PokokNow + SanksiNow;
        }

        // ======= ENTITY DETAIL ==========
        public class DataDetailPemeriksaan
        {
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string WajibPajak { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string UPTB { get; set; } = null!;
            public string NoSP { get; set; } = null!;
            public DateTime TglST { get; set; }
            public int Tahun { get; set; }
            public decimal JumlahKB { get; set; }
            public string Keterangan { get; set; } = null!;
            public string LHP { get; set; } = null!;
            public DateTime? TglLHP { get; set; }
            public DateTime? TglBayar { get; set; }
            public string Tim { get; set; } = null!;

            public string TglLHPDisplay => TglLHP.HasValue ? TglLHP.Value.ToString("dd/MM/yyyy") : "-";
            public string TglBayarDisplay => TglBayar.HasValue ? TglBayar.Value.ToString("dd/MM/yyyy") : "-";
        }

        public class Dashboard
        {
            public decimal TotalOP { get; set; }
            public decimal RataKurangBayar { get; set; }
            public decimal TotalKurangBayar { get; set; }
        }
    }

    
}
