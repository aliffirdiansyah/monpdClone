using DevExpress.CodeParser;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PemasanganAlatUPTBVM
    {
        public class Index
        {
            public Index()
            {
                //Data = Method.GetDashboardData();
            }
        }

        public class Show
        {
            public List<SeriesPemasanganAlat> SeriesPemasanganAlatList { get; set; } = new();

            public Show(int wilayah)
            {
                SeriesPemasanganAlatList = Method.GetSeriesAlatRekam(wilayah);

                
            }
        }

        public class Tahunan
        {
            public List<DataPemasanganAlat> DataPemasanganAlatList { get; set; } = new();
            public DashboardData Data { get; set; } = new();

            public Tahunan(int wilayah)
            {
                DataPemasanganAlatList = Method.GetTahunBerjalan(wilayah);

                Data.HotelTotal = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.JumlahOP);
                Data.RestoTotal = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.MakananMinuman).Sum(x => x.JumlahOP);
                Data.ParkirTotal = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaParkir).Sum(x => x.JumlahOP);
                Data.HiburanTotal = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.JumlahOP);

                Data.HotelTerpasang = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.TotalTerpasang);
                Data.RestoTerpasang = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.MakananMinuman).Sum(x => x.TotalTerpasang);
                Data.ParkirTerpasang = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaParkir).Sum(x => x.TotalTerpasang);
                Data.HiburanTerpasang = DataPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.TotalTerpasang);
            }
        }
        public class DetailOP
        {
            public List<SubDetailModal> PemasanganAlatDetailOP { get; set; } = new();
            public DetailOP(int jenisPajak, int kategori, int tahun, int wilayah)
            {
                PemasanganAlatDetailOP = Method.GetDetailModal((EnumFactory.EPajak)jenisPajak, kategori, tahun, wilayah);
            }
        }
        public class DetailTahun
        {
            public List<SubDetailDataModal> PemasanganAlatDetailTahun { get; set; } = new();
            public DetailTahun(int jenisPajak, int kategori, int status, int wilayah)
            {
                PemasanganAlatDetailTahun = Method.GetTahunBerjalanModal((EnumFactory.EPajak)jenisPajak, kategori, status, wilayah);
            }
        }
        public class Method
        {
            public static List<SeriesPemasanganAlat> GetSeriesAlatRekam(int wilayah)
            {
                var ret = new List<SeriesPemasanganAlat>();
                var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;
                int tahunMulai = tahunSekarang - 4;

                ret = context.DbMonAlatRekams
                   .Where(x => x.TglTerpasang.HasValue && x.TglTerpasang.Value.Year >= tahunMulai && x.Uptb == wilayah)
                   .GroupBy(x => x.PajakId)
                   .Select(g => new SeriesPemasanganAlat
                   {
                       EnumPajak = (int)(EnumFactory.EPajak)g.Key,
                       JenisPajak = ((EnumFactory.EPajak)g.Key).GetDescription(),
                       JumlahOP = g.Select(x => x.Nop).Distinct().Count(),

                       TahunMines4 = g.Where(x => x.Tmin4 == 1)
                                      .Select(x => x.Nop).Distinct().Count(),

                       TahunMines3 = g.Where(x => x.Tmin3 == 1)
                                      .Select(x => x.Nop).Distinct().Count(),

                       TahunMines2 = g.Where(x => x.Tmin2 == 1)
                                      .Select(x => x.Nop).Distinct().Count(),

                       TahunMines1 = g.Where(x => x.Tmin1 == 1)
                                      .Select(x => x.Nop).Distinct().Count(),

                       TahunNow = g.Where(x => x.Tmin0 == 1)
                                   .Select(x => x.Nop).Distinct().Count()
                   })
                   .AsNoTracking()
                   .ToList();


                return ret;
            }

            public static List<DetailSeriesPemasanganAlat> GetDetailSeriesAlatRekam(EnumFactory.EPajak jenisPajak, int wilayah)
            {
                var ret = new List<DetailSeriesPemasanganAlat>();
                using var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;
                int tahunMulai = tahunSekarang - 4;

                // Ambil daftar kategori
                // Ambil daftar kategori sesuai jenis pajak
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak)
                    .OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                // Ambil data pemasangan alat, group by KategoriId & Tahun
                var query = context.DbMonAlatRekams
                    .Where(x => x.TglTerpasang.HasValue && x.TglTerpasang.Value.Year >= tahunMulai && x.TglTerpasang.Value.Year <= tahunSekarang
                                && x.PajakId == (int)jenisPajak && x.Uptb == wilayah)
                    .GroupBy(x => new { x.KategoriId, x.TglTerpasang.Value.Year })
                    .Select(g => new
                    {
                        g.Key.KategoriId,
                        g.Key.Year,
                        JumlahOP = g.Select(x => x.Nop).Distinct().Count(),

                        // Hitung hanya NOP unik yang punya TminX = 1
                        Terpasang1 = g.Select(x => x.Nop).Distinct()
                                      .Count(nop => g.Any(y => y.Nop == nop && y.Tmin0 == 1)),
                        Terpasang2 = g.Select(x => x.Nop).Distinct()
                                      .Count(nop => g.Any(y => y.Nop == nop && y.Tmin1 == 1)),
                        Terpasang3 = g.Select(x => x.Nop).Distinct()
                                      .Count(nop => g.Any(y => y.Nop == nop && y.Tmin2 == 1)),
                        Terpasang4 = g.Select(x => x.Nop).Distinct()
                                      .Count(nop => g.Any(y => y.Nop == nop && y.Tmin3 == 1)),
                        Terpasang5 = g.Select(x => x.Nop).Distinct()
                                      .Count(nop => g.Any(y => y.Nop == nop && y.Tmin4 == 1)),
                    })
                    .ToList();

                // Bentuk hasil akhir
                var result = kategoriList.Select(kategori =>
                {
                    var dataKategori = query.Where(q => q.KategoriId == kategori.Id).ToList();

                    return new DetailSeriesPemasanganAlat
                    {
                        EnumPajak = (int)jenisPajak,
                        JenisPajak = jenisPajak.GetDescription(),
                        KategoriId = (int)kategori.Id,
                        KategoriNama = kategori.Nama,

                        JumlahOP = dataKategori.Sum(d => d.JumlahOP),

                        TahunMines4 = dataKategori.Sum(d => d.Terpasang5),
                        TahunMines3 = dataKategori.Sum(d => d.Terpasang4),
                        TahunMines2 = dataKategori.Sum(d => d.Terpasang3),
                        TahunMines1 = dataKategori.Sum(d => d.Terpasang2),
                        TahunNow = dataKategori.Sum(d => d.Terpasang1)
                    };
                }).ToList();



                return result;
            }

            public static List<SubDetailModal> GetDetailModal(EnumFactory.EPajak jenisPajak, int kategori, int tahun, int wilayah)
            {
                using var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;
                int offset = tahunSekarang - tahun; // hitung selisih tahun

                var query = context.DbMonAlatRekams
                    .Where(x => x.PajakId == (int)jenisPajak
                                && x.KategoriId == kategori
                                && x.TglTerpasang.HasValue
                                && x.TglTerpasang.Value.Year == tahun
                                && x.Uptb == wilayah);

                switch (offset)
                {
                    case 0: // tahun sekarang
                        query = query.Where(x => x.Tmin0 == 1);
                        break;
                    case 1:
                        query = query.Where(x => x.Tmin1 == 1);
                        break;
                    case 2:
                        query = query.Where(x => x.Tmin2 == 1);
                        break;
                    case 3:
                        query = query.Where(x => x.Tmin3 == 1);
                        break;
                    case 4:
                        query = query.Where(x => x.Tmin4 == 1);
                        break;
                    default:
                        query = query.Where(x => false); // tahun di luar range
                        break;
                }

                var rows = query
                    .AsNoTracking()
                    .ToList();


                var latestPerNop = rows
                    .GroupBy(r => r.Nop)
                    .Distinct()
                    .Select(g => g.OrderByDescending(r => r.TglTerpasang).First())
                    .ToList();

                var result = latestPerNop.Select(x =>
                {
                    // aman parsing jam
                    var jam = string.IsNullOrWhiteSpace(x.Jam) ? "00:00:00" : x.Jam;
                    if (!TimeSpan.TryParse(jam, out var ts)) ts = TimeSpan.Zero;

                    string terakhirAktif = (x.Tahun > 0 && x.Bln > 0 && x.Tgl > 0)
                        ? new DateTime((int)x.Tahun, (int)x.Bln, (int)x.Tgl).Add(ts).ToString("dd/MM/yyyy HH:mm:ss")
                        : "Tidak Ada Data";

                    return new SubDetailModal
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        Kategori = x.KategoriNama,
                        NamaOP = x.NamaOp,
                        NOP = x.Nop,
                        Alamat = x.AlamatOp,
                        TanggalPemasangan = x.TglTerpasang ?? DateTime.MinValue,
                        JenisPasang = x.JenisAlat,
                        StatusKunci = x.StatusKunci == 1 ? "Terkunci" : "Tidak Terkunci",
                        StatusOnline = x.StatusOnline == 1 ? "Online" : "Offline",
                        TerakhirAktif = terakhirAktif
                    };
                }).ToList();

                return result;
            }

            public static List<DataPemasanganAlat> GetTahunBerjalan(int wilayah)
            {
                var ret = new List<DataPemasanganAlat>();
                var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;
                int tahunAwal = tahunSekarang - 7;

                ret = context.DbMonAlatRekams
                    .Where(x => x.TglTerpasang.HasValue && x.TglTerpasang.Value.Year >= tahunAwal && x.TglTerpasang.Value.Year <= tahunSekarang && x.Uptb == wilayah)
                    .GroupBy(x => new { x.PajakId, x.Tahun })
                    .Select(g => new
                    {
                        g.Key.PajakId,
                        g.Key.Tahun,
                        JumlahOP = g.Count(),
                        TerpasangTS = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.JenisAlat == "TS"))),
                        TerpasangTB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.JenisAlat == "TB"))),
                        TerpasangSB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.JenisAlat == "SB"))),
                    })
                    .AsNoTracking()
                    .ToList()
                    .GroupBy(x => x.PajakId)
                    .Select(g =>
                    {
                        var s = new DataPemasanganAlat
                        {
                            EnumPajak = (int)(EnumFactory.EPajak)g.Key,
                            JenisPajak = ((EnumFactory.EPajak)g.Key).GetDescription(),
                            JumlahOP = g.Sum(x => x.JumlahOP)
                        };
                        s.TerpasangTS = g.Sum(x => x.TerpasangTS);
                        s.TerpasangTB = g.Sum(x => x.TerpasangTB);
                        s.TerpasangSB = g.Sum(x => x.TerpasangSB);

                        return s;
                    })
                    .ToList();


                return ret;
            }
            public static List<DetailDataPemasanganAlat> GetDetailTahunBerjalan(EnumFactory.EPajak jenisPajak, int wilayah)
            {
                using var context = DBClass.GetContext();
                int tahunSekarang = DateTime.Now.Year;
                int tahunAwal = tahunSekarang - 7;
                // Ambil daftar kategori
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                // Ambil data pemasangan alat
                var query = context.DbMonAlatRekams
                    .Where(x => x.TglTerpasang.HasValue && x.TglTerpasang.Value.Year >= tahunAwal && x.TglTerpasang.Value.Year <= tahunSekarang
                                && x.PajakId == (int)jenisPajak && x.Uptb == wilayah)
                    .GroupBy(x => new { x.KategoriId })
                     .Select(g => new
                     {
                         g.Key.KategoriId,
                         JumlahOP = g.Count(),
                         TerpasangTS = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.JenisAlat == "TS"))),
                         TerpasangTB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.JenisAlat == "TB"))),
                         TerpasangSB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.JenisAlat == "SB"))),
                     })
                    .ToList();
                var result = new List<DetailDataPemasanganAlat>();
                foreach (var kategori in kategoriList)
                {
                    var s = new DetailDataPemasanganAlat
                    {
                        EnumPajak = (int)jenisPajak,
                        JenisPajak = jenisPajak.GetDescription(),
                        KategoriId = (int)kategori.Id,
                        KategoriNama = kategori.Nama
                    };
                    var dataKategori = query.Where(q => q.KategoriId == kategori.Id).ToList();
                    s.JumlahOP = dataKategori.Sum(d => d.JumlahOP);
                    s.TerpasangTS = dataKategori.Sum(d => d.TerpasangTS);
                    s.TerpasangTB = dataKategori.Sum(d => d.TerpasangTB);
                    s.TerpasangSB = dataKategori.Sum(d => d.TerpasangSB);

                    result.Add(s);
                }

                return result;
            }

            public static List<SubDetailDataModal> GetTahunBerjalanModal(EnumFactory.EPajak jenisPajak, int kategori, int status, int wilayah)
            {
                using var context = DBClass.GetContext();
                int tahunSekarang = DateTime.Now.Year;
                int tahunAwal = tahunSekarang - 7;

                var query = context.DbMonAlatRekams
                    .Where(x => x.PajakId == (int)jenisPajak
                                && x.KategoriId == kategori
                                && x.TglTerpasang.HasValue
                                && x.TglTerpasang.Value.Year >= tahunAwal && x.TglTerpasang.Value.Year <= tahunSekarang
                                && x.Uptb == wilayah);
                switch (status)
                {
                    case 1:
                        query = query.Where(x => x.JenisAlat == "TS");
                        break;
                    case 2:
                        query = query.Where(x => x.JenisAlat == "TB");
                        break;
                    case 3:
                        query = query.Where(x => x.JenisAlat == "SB");
                        break;
                    case 0:
                        query = query.Where(x => x.JenisAlat == "TS" || x.JenisAlat == "TB" || x.JenisAlat == "SB");
                        break;
                    default:
                        query = query.Where(x => false); // status di luar range
                        break;
                }
                var rows = query
                    .AsNoTracking()
                    .ToList();
                var latestPerNop = rows
                    .GroupBy(r => r.Nop)
                    .Distinct()
                    .Select(g => g.OrderByDescending(r => r.TglTerpasang).First())
                    .ToList();
                var result = latestPerNop.Select(x =>
                {
                    // aman parsing jam
                    var jam = string.IsNullOrWhiteSpace(x.Jam) ? "00:00:00" : x.Jam;
                    if (!TimeSpan.TryParse(jam, out var ts)) ts = TimeSpan.Zero;
                    string terakhirAktif = (x.Tahun > 0 && x.Bln > 0 && x.Tgl > 0)
                        ? new DateTime((int)x.Tahun, (int)x.Bln, (int)x.Tgl).Add(ts).ToString("dd/MM/yyyy HH:mm:ss")
                        : "Tidak Ada Data";
                    return new SubDetailDataModal
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        Kategori = x.KategoriNama,
                        NamaOP = x.NamaOp,
                        NOP = x.Nop,
                        Alamat = x.AlamatOp,
                        TanggalPemasangan = x.TglTerpasang ?? DateTime.MinValue,
                        JenisPasang = x.JenisAlat,
                        StatusKunci = x.StatusKunci == 1 ? "Terkunci" : "Tidak Terkunci",
                        StatusOnline = x.StatusOnline == 1 ? "Online" : "Offline",
                        TerakhirAktif = terakhirAktif
                    };
                }).ToList();

                return result;
            }
        }
        public class DashboardData
        {
            public int HotelTerpasang { get; set; }
            public int HotelTotal { get; set; }
            public int RestoTerpasang { get; set; }
            public int RestoTotal { get; set; }
            public int HiburanTerpasang { get; set; }
            public int HiburanTotal { get; set; }
            public int ParkirTerpasang { get; set; }
            public int ParkirTotal { get; set; }
        }
        public class SeriesPemasanganAlat
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int JumlahOP { get; set; }
            public int TahunMines4 { get; set; }
            public int BelumTahunMines4 { get; set; }
            public int TahunMines3 { get; set; }
            public int BelumTahunMines3 { get; set; }
            public int TahunMines2 { get; set; }
            public int BelumTahunMines2 { get; set; }
            public int TahunMines1 { get; set; }
            public int BelumTahunMines1 { get; set; }
            public int TahunNow { get; set; }
            public int BelumTahunNow { get; set; }
            public int TotalTerpasang => TahunMines4 + TahunMines3 + TahunMines2 + TahunMines1 + TahunNow;
        }
        public class DetailSeriesPemasanganAlat
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string KategoriNama { get; set; } = null!;
            public int KategoriId { get; set; }
            public int JumlahOP { get; set; }
            public int TahunMines4 { get; set; }
            public int BelumTahunMines4 { get; set; }
            public int TahunMines3 { get; set; }
            public int BelumTahunMines3 { get; set; }
            public int TahunMines2 { get; set; }
            public int BelumTahunMines2 { get; set; }
            public int TahunMines1 { get; set; }
            public int BelumTahunMines1 { get; set; }
            public int TahunNow { get; set; }
            public int BelumTahunNow { get; set; }
            public int TotalTerpasang => TahunMines4 + TahunMines3 + TahunMines2 + TahunMines1 + TahunNow;
            public int TotalBelumTerpasang => JumlahOP - TotalTerpasang;
        }

        public class DataPemasanganAlat
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int JumlahOP { get; set; }
            public int TerpasangTS { get; set; }
            public int TerpasangTB { get; set; }
            public int TerpasangSB { get; set; }
            public int TotalTerpasang => TerpasangTS + TerpasangTB + TerpasangSB;
            public int BelumTerpasang => JumlahOP - TotalTerpasang;
        }

        public class DetailDataPemasanganAlat
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string KategoriNama { get; set; } = null!;
            public int KategoriId { get; set; }
            public int JumlahOP { get; set; }
            public int TerpasangTS { get; set; }
            public int TerpasangTB { get; set; }
            public int TerpasangSB { get; set; }
            public int TotalTerpasang => TerpasangTS + TerpasangTB + TerpasangSB;
            public int BelumTerpasang => JumlahOP - TotalTerpasang;
        }

        public class SubDetailModal
        {
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string Alamat { get; set; } = null!;
            public DateTime TanggalPemasangan { get; set; }
            public string JenisPasang { get; set; } = null!;
            public string StatusKunci { get; set; } = null!;
            public string JenisAlat { get; set; } = null!;
            public string StatusOnline { get; set; } = null!;
            public string TerakhirAktif { get; set; } = null!;
            public decimal status { get; set; }


            public bool IsTerpasangTS { get; set; }
            public bool IsTerpasangTB { get; set; }
            public bool IsTerpasangSB { get; set; }

        }

        public class SubDetailDataModal
        {
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string Alamat { get; set; } = null!;
            public DateTime TanggalPemasangan { get; set; }
            public string JenisPasang { get; set; } = null!;
            public string StatusKunci { get; set; } = null!;
            public string JenisAlat { get; set; } = null!;
            public string StatusOnline { get; set; } = null!;
            public string TerakhirAktif { get; set; } = null!;
            public decimal status { get; set; }


            public bool IsTerpasangTS { get; set; }
            public bool IsTerpasangTB { get; set; }
            public bool IsTerpasangSB { get; set; }

        }
    }
}
