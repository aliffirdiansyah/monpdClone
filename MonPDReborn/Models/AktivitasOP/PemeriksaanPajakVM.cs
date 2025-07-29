using MonPDLib;
using MonPDLib.General;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PemeriksaanPajakVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;

            public int TotalOpDiperiksa { get; set; }
            public decimal RataRataKurangBayar { get; set; }
            public decimal TotalKurangBayar { get; set; }
            public Index() { }
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
                        JenisPajak = x.PajakId.ToString(),
                        NOP = x.Nop ?? "",
                        WajibPajak = dbMamin.ContainsKey(x.Nop) ? dbMamin[x.Nop].NpwpdNama ?? "" : "",
                        Alamat = dbMamin.ContainsKey(x.Nop) ? dbMamin[x.Nop].AlamatOp ?? "" : "",
                        UPTB = dbMamin.ContainsKey(x.Nop) ? dbMamin[x.Nop].WilayahPajak ?? "" : "",
                        NoSP = x.NoSp ?? "",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = 0,
                        Keterangan = x.Ket ?? "",
                        LHP = "-",
                        TglLHP = DateTime.MinValue,
                        TglBayar = DateTime.MinValue,
                        Tim = x.Petugas ?? ""
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
                        JenisPajak = x.PajakId.ToString(),
                        NOP = x.Nop ?? "",
                        WajibPajak = dbHotel.ContainsKey(x.Nop) ? dbHotel[x.Nop].NpwpdNama ?? "" : "",
                        Alamat = dbHotel.ContainsKey(x.Nop) ? dbHotel[x.Nop].AlamatOp ?? "" : "",
                        UPTB = dbHotel.ContainsKey(x.Nop) ? dbHotel[x.Nop].WilayahPajak ?? "" : "",
                        NoSP = x.NoSp ?? "",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = 0,
                        Keterangan = x.Ket ?? "",
                        LHP = "-",
                        TglLHP = DateTime.MinValue,
                        TglBayar = DateTime.MinValue,
                        Tim = x.Petugas ?? ""
                    }).ToList();
                }
                else if (jenisPajak == 4)
                {
                    var dbParkir = context.DbOpParkirs
                        .GroupBy(x => x.Nop)
                        .ToDictionary(g => g.Key, g => g.First());

                    ret = pemeriksaans.Select(x => new DataDetailPemeriksaan
                    {
                        JenisPajak = x.PajakId.ToString(),
                        NOP = x.Nop ?? "",
                        WajibPajak = dbParkir.ContainsKey(x.Nop) ? dbParkir[x.Nop].NpwpdNama ?? "" : "",
                        Alamat = dbParkir.ContainsKey(x.Nop) ? dbParkir[x.Nop].AlamatOp ?? "" : "",
                        UPTB = dbParkir.ContainsKey(x.Nop) ? dbParkir[x.Nop].WilayahPajak ?? "" : "",
                        NoSP = x.NoSp ?? "",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = 0,
                        Keterangan = x.Ket ?? "",
                        LHP = "-",
                        TglLHP = DateTime.MinValue,
                        TglBayar = DateTime.MinValue,
                        Tim = x.Petugas ?? ""
                    }).ToList();
                }
                else if (jenisPajak == 5)
                {
                    var dbHiburan = context.DbOpHiburans
                        .GroupBy(x => x.Nop)
                        .ToDictionary(g => g.Key, g => g.First());

                    ret = pemeriksaans.Select(x => new DataDetailPemeriksaan
                    {
                        JenisPajak = x.PajakId.ToString(),
                        NOP = x.Nop ?? "",
                        WajibPajak = dbHiburan.ContainsKey(x.Nop) ? dbHiburan[x.Nop].NpwpdNama ?? "" : "",
                        Alamat = dbHiburan.ContainsKey(x.Nop) ? dbHiburan[x.Nop].AlamatOp ?? "" : "",
                        UPTB = dbHiburan.ContainsKey(x.Nop) ? dbHiburan[x.Nop].WilayahPajak ?? "" : "",
                        NoSP = x.NoSp ?? "",
                        TglST = x.TglSp,
                        Tahun = x.TahunPajak,
                        JumlahKB = 0,
                        Keterangan = x.Ket ?? "",
                        LHP = "-",
                        TglLHP = DateTime.MinValue,
                        TglBayar = DateTime.MinValue,
                        Tim = x.Petugas ?? ""
                    }).ToList();
                }
                    /*else if (jenisPajak == 2) // Restoran
                    {
                        var dbResto = context.DbOpRestorans.ToDictionary(x => x.Nop, x => x);

                        ret = pemeriksaans.Select(x => new DataDetailPemeriksaan
                        {
                            JenisPajak = x.JenisPajak.ToString(),
                            NOP = x.Nop ?? "",
                            WajibPajak = dbResto.ContainsKey(x.Nop) ? dbResto[x.Nop].NamaWp ?? "" : "",
                            Alamat = dbResto.ContainsKey(x.Nop) ? dbResto[x.Nop].Alamat ?? "" : "",
                            UPTB = dbResto.ContainsKey(x.Nop) ? dbResto[x.Nop].Uptb ?? "" : "",
                            NoSP = x.NoSP ?? "",
                            TglST = x.TglST ?? DateTime.MinValue,
                            Tahun = x.Tahun,
                            JumlahKB = x.JumlahKB ?? 0,
                            Keterangan = x.Keterangan ?? "",
                            LHP = x.NoLHP ?? "",
                            TglLHP = x.TglLHP ?? DateTime.MinValue,
                            TglBayar = x.TglBayar ?? DateTime.MinValue,
                            Tim = x.NamaTim ?? ""
                        }).ToList();
                    }*/

                    // Tambahkan else if untuk jenis pajak lain (3 = Hiburan, 4 = Parkir, dll)

                    return ret;
            }



            //var all = GetAllDetail();

            //    // Jika tidak ada filter, kembalikan list kosong agar tidak semua data tampil di awal
            //    if (string.IsNullOrWhiteSpace(jenisPajak) || tahun == 0)
            //        return new List<DataDetailPemeriksaan>();

            //    // Filter berdasarkan Jenis Pajak DAN Tahun
            //    return all
            //        .Where(x => x.JenisPajak.Equals(jenisPajak, StringComparison.OrdinalIgnoreCase) && x.Tahun == tahun)
            //        .ToList();
        }

        private static List<DataDetailPemeriksaan> GetAllDetail()
        {
            return new List<DataDetailPemeriksaan>
            {
                new()
                {
                    JenisPajak = "Pajak Hotel", // ✅ cocok dengan "Pajak Hotel"
                    NOP = "35.78.170.005.902.00066",
                    WajibPajak = "Hotel Bintang",
                    Alamat = "Jl. Rajawali No. 47",
                    UPTB = "UPTB Timur",
                    NoSP = "SP-001/2025",
                    TglST = new DateTime(2025, 1, 10),
                    Tahun = 2023,
                    JumlahKB = 15000000,
                    Keterangan = "Kurang Bayar 2023",
                    LHP = "LHP-2025-01",
                    TglLHP = new DateTime(2025, 1, 20),
                    TglBayar = new DateTime(2025, 2, 5),
                    Tim = "Tim A"
                },
                new()
                {
                    JenisPajak = "Pajak Hotel", // ✅ baris kedua juga hotel
                    NOP = "35.78.170.005.902.00067",
                    WajibPajak = "Hotel Nusantara",
                    Alamat = "Jl. Merdeka No. 12",
                    UPTB = "UPTB Selatan",
                    NoSP = "SP-003/2025",
                    TglST = new DateTime(2025, 3, 1),
                    Tahun = 2024,
                    JumlahKB = 18000000,
                    Keterangan = "Kurang Bayar 2024",
                    LHP = "LHP-2025-03",
                    TglLHP = new DateTime(2025, 3, 15),
                    TglBayar = new DateTime(2025, 3, 20),
                    Tim = "Tim C"
                },
                new()
                {
                    JenisPajak = "Pajak Restoran", // ✅ cocok dengan "Pajak Restoran"
                    NOP = "35.78.100.002.902.00172",
                    WajibPajak = "Restoran Nikmat",
                    Alamat = "Jl. Bubutan No. 1-7",
                    UPTB = "UPTB Pusat",
                    NoSP = "SP-002/2025",
                    TglST = new DateTime(2025, 2, 15),
                    Tahun = 2024,
                    JumlahKB = 12000000,
                    Keterangan = "Kurang Bayar 2024",
                    LHP = "LHP-2025-02",
                    TglLHP = new DateTime(2025, 2, 25),
                    TglBayar = new DateTime(2025, 3, 10),
                    Tim = "Tim B"
                }
            };
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
            public DateTime TglLHP { get; set; }
            public DateTime TglBayar { get; set; }
            public string Tim { get; set; } = null!;
        }
    }

    
}
