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

                // Ambil data pemeriksaan sesuai filter pajak & tahun
                var pemeriksaans = context.TPemeriksaans
                    .Where(x => x.PajakId == jenisPajak && x.TahunPajak == tahun)
                    .ToList(); // sudah difilter, baru di-list

                if (jenisPajak == 1) // Mamin / Restoran
                {
                    var dbMamin = context.DbOpRestos
                        .Where(o => o.Nop != null) // hindari null key
                        .GroupBy(x => x.Nop)
                        .Select(g => g.First())
                        .ToDictionary(g => g.Nop, g => g);

                    ret = pemeriksaans.Select(x =>
                    {
                        dbMamin.TryGetValue(x.Nop, out var mamin);
                        return new DataDetailPemeriksaan
                        {
                            pajakId = (int)EnumFactory.EPajak.MakananMinuman,
                            JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                            NOP = x.Nop ?? "-",
                            NamaOP = mamin != null ? (mamin.NamaOp ?? "-") : "NAMA OP TIDAK DIKETAHUI - " + (x.Ket ?? "-"),
                            Alamat = mamin?.AlamatOp ?? "-",
                            UPTB = "SURABAYA " + (mamin?.WilayahPajak ?? "-"),
                            NoSP = x.NoSp ?? "-",
                            TglST = x.TglSp,
                            Tahun = x.TahunPajak,
                            JumlahKB = x.JumlahKb ?? 0,
                            PokokBayar = x.Pokok,
                            Sanksi = x.Denda,
                            Keterangan = x.Ket ?? "-",
                            LHP = x.Lhp ?? "-",
                            TglLHP = x.TglLhp,
                            TglBayar = x.TglByr,
                            Tim = x.Petugas ?? "-"
                        };
                    }).ToList();
                }
                else if (jenisPajak == 3) // Hotel
                {
                    var dbHotel = context.DbOpHotels
                        .Where(o => o.Nop != null)
                        .GroupBy(x => x.Nop)
                        .Select(g => g.First())
                        .ToDictionary(g => g.Nop, g => g);

                    ret = pemeriksaans.Select(x =>
                    {
                        dbHotel.TryGetValue(x.Nop, out var hotel);
                        return new DataDetailPemeriksaan
                        {
                            pajakId = (int)EnumFactory.EPajak.JasaPerhotelan,
                            JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                            NOP = x.Nop ?? "-",
                            NamaOP = hotel != null ? (hotel.NamaOp ?? "-") : "NAMA OP TIDAK DIKETAHUI - " + (x.Ket ?? "-"),
                            Alamat = hotel?.AlamatOp ?? "-",
                            UPTB = "SURABAYA " + (hotel?.WilayahPajak ?? "-"),
                            NoSP = x.NoSp ?? "-",
                            TglST = x.TglSp,
                            Tahun = x.TahunPajak,
                            JumlahKB = x.JumlahKb ?? 0,
                            PokokBayar = x.Pokok,
                            Sanksi = x.Denda,
                            Keterangan = x.Ket ?? "-",
                            LHP = x.Lhp ?? "-",
                            TglLHP = x.TglLhp,
                            TglBayar = x.TglByr,
                            Tim = x.Petugas ?? "-"
                        };
                    }).ToList();
                }
                else if (jenisPajak == 4) // Parkir
                {
                    var dbParkir = context.DbOpParkirs
                        .Where(o => o.Nop != null)
                        .GroupBy(x => x.Nop)
                        .Select(g => g.First())
                        .ToDictionary(g => g.Nop, g => g);

                    ret = pemeriksaans.Select(x =>
                    {
                        dbParkir.TryGetValue(x.Nop, out var parkir);
                        return new DataDetailPemeriksaan
                        {
                            pajakId = (int)EnumFactory.EPajak.JasaParkir,
                            JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                            NOP = x.Nop ?? "-",
                            NamaOP = parkir != null ? (parkir.NamaOp ?? "-") : "NAMA OP TIDAK DIKETAHUI - " + (x.Ket ?? "-"),
                            Alamat = parkir?.AlamatOp ?? "-",
                            UPTB = "SURABAYA " + (parkir?.WilayahPajak ?? "-"),
                            NoSP = x.NoSp ?? "-",
                            TglST = x.TglSp,
                            Tahun = x.TahunPajak,
                            JumlahKB = x.JumlahKb ?? 0,
                            PokokBayar = x.Pokok,
                            Sanksi = x.Denda,
                            Keterangan = x.Ket ?? "-",
                            LHP = x.Lhp ?? "-",
                            TglLHP = x.TglLhp,
                            TglBayar = x.TglByr,
                            Tim = x.Petugas ?? "-"
                        };
                    }).ToList();
                }
                else if (jenisPajak == 5) // Hiburan
                {
                    var dbHiburan = context.DbOpHiburans
                        .Where(o => o.Nop != null)
                        .GroupBy(x => x.Nop)
                        .Select(g => g.First())
                        .ToDictionary(g => g.Nop, g => g);

                    ret = pemeriksaans.Select(x =>
                    {
                        dbHiburan.TryGetValue(x.Nop, out var hiburan);
                        return new DataDetailPemeriksaan
                        {
                            pajakId = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                            JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                            NOP = x.Nop ?? "-",
                            NamaOP = hiburan != null ? (hiburan.NamaOp ?? "-") : "NAMA OP TIDAK DIKETAHUI - " + (x.Ket ?? "-"),
                            Alamat = hiburan?.AlamatOp ?? "-",
                            UPTB = "SURABAYA " + (hiburan?.WilayahPajak ?? "-"),
                            NoSP = x.NoSp ?? "-",
                            TglST = x.TglSp,
                            Tahun = x.TahunPajak,
                            JumlahKB = x.JumlahKb ?? 0,
                            PokokBayar = x.Pokok,
                            Sanksi = x.Denda,
                            Keterangan = x.Ket ?? "-",
                            LHP = x.Lhp ?? "-",
                            TglLHP = x.TglLhp,
                            TglBayar = x.TglByr,
                            Tim = x.Petugas ?? "-"
                        };
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
            public int pajakId { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string UPTB { get; set; } = null!;
            public string NoSP { get; set; } = null!;
            public DateTime TglST { get; set; }
            public int Tahun { get; set; }
            public decimal PokokBayar { get; set; }
            public decimal Sanksi { get; set; }
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
