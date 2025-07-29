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

            public Detail(string jenisPajak, int tahun)
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

            public static List<DataDetailPemeriksaan> GetDetailPemeriksaanList(string jenisPajak, int tahun)
            {
                var ret = new List<DataDetailPemeriksaan>();
                var context = DBClass.GetContext();

                var query = context.TPemeriksaans.AsQueryable();
                    
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
