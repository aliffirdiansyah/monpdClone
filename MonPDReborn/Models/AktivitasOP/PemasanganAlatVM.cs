using DevExpress.CodeParser;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PemasanganAlatVM
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
            public DashboardData Data { get; set; } = new();

            public Show()
            {
                SeriesPemasanganAlatList = Method.GetSeriesAlatRekam();

                Data.HotelTotal = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.JumlahOP);
                Data.RestoTotal = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.MakananMinuman).Sum(x => x.JumlahOP);
                Data.ParkirTotal = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaParkir).Sum(x => x.JumlahOP);
                Data.HiburanTotal = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.JumlahOP);

                Data.HotelTerpasang = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.TahunNow);
                Data.RestoTerpasang = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.MakananMinuman).Sum(x => x.TahunNow);
                Data.ParkirTerpasang = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaParkir).Sum(x => x.TahunNow);
                Data.HiburanTerpasang = SeriesPemasanganAlatList.Where(x => (EnumFactory.EPajak)x.EnumPajak == EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.TahunNow);
            }
        }

        public class Tahunan
        {
            public List<DataPemasanganAlat> DataPemasanganAlatList { get; set; } = new();
            public Tahunan()
            {
                DataPemasanganAlatList = Method.GetTahunanPemasanganAlatList();
            }
        }
        public class DetailOP
        {
            public List<SubDetailModal> PemasanganAlatDetailOP { get; set; } = new();
            public DetailOP(int jenisPajak, int kategori, int tahun, string status)
            {
                PemasanganAlatDetailOP = Method.GetSubDetailOPData((EnumFactory.EPajak)jenisPajak, kategori, tahun, status);
            }
        }
        public class Method
        {
            /*public static List<SeriesPemasanganAlat> GetSeriesPemasanganAlatList()
            {
                var ret = new List<SeriesPemasanganAlat>();
                var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;
                int tahunMulai = tahunSekarang - 4;

                ret = context.DbRekamAlatGabungs
                    .Where(x => x.Tahun >= tahunMulai && x.Tahun <= tahunSekarang)
                    .GroupBy(x => new { x.PajakId, x.Tahun })
                    .Select(g => new
                    {
                        g.Key.PajakId,
                        g.Key.Tahun,
                        JumlahOP = g.Count(),
                        Terpasang = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.IsTs == 1 || y.IsTb == 1 || y.IsSb == 1))),

                        BelumTerpasang = g.Select(x => x.Nop)
                        .Distinct()
                        .Count(nop => g.Any(y => y.Nop == nop &&
                                                (y.IsTs == 0 && y.IsTb == 0 && y.IsSb == 0)))
                    })
                    .AsNoTracking()
                    .ToList()
                    .GroupBy(x => x.PajakId)
                    .Select(g =>
                    {
                        var s = new SeriesPemasanganAlat
                        {
                            EnumPajak = (int)(EnumFactory.EPajak)g.Key,
                            JenisPajak = ((EnumFactory.EPajak)g.Key).GetDescription(),
                            JumlahOP = g.Sum(x => x.JumlahOP)
                        };

                        foreach (var row in g)
                        {
                            if (row.Tahun == tahunMulai)
                            {
                                s.Terpasang2021 = row.Terpasang;
                                s.BelumTerpasang2021 = row.BelumTerpasang;
                            }
                            else if (row.Tahun == tahunMulai + 1)
                            {
                                s.Terpasang2022 = row.Terpasang;
                                s.BelumTerpasang2022 = row.BelumTerpasang;
                            }
                            else if (row.Tahun == tahunMulai + 2)
                            {
                                s.Terpasang2023 = row.Terpasang;
                                s.BelumTerpasang2023 = row.BelumTerpasang;
                            }
                            else if (row.Tahun == tahunMulai + 3)
                            {
                                s.Terpasang2024 = row.Terpasang;
                                s.BelumTerpasang2024 = row.BelumTerpasang;
                            }
                            else if (row.Tahun == tahunMulai + 4)
                            {
                                s.Terpasang2025 = row.Terpasang;
                                s.BelumTerpasang2025 = row.BelumTerpasang;
                            }
                        }
                        return s;
                    })
                    .ToList();


                return ret;
            }*/
            /*public static List<DetailSeriesPemasanganAlat> GetDetailSeriesPemasanganAlatList(EnumFactory.EPajak jenisPajak)
            {
                using var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;
                int tahunMulai = tahunSekarang - 4;

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
                var query = context.DbRekamAlatGabungs
                    .Where(x => x.Tahun >= tahunMulai && x.Tahun <= tahunSekarang
                                && x.PajakId == (int)jenisPajak)
                    .GroupBy(x => new { x.KategoriId, x.Tahun })
                    .Select(g => new
                    {
                        g.Key.KategoriId,
                        g.Key.Tahun,
                        JumlahOP = g.Count(),
                        Terpasang = g.Select(x => x.Nop)
                                     .Distinct()
                                     .Count(nop => g.Any(y => y.Nop == nop &&
                                                  (y.IsTs == 1 || y.IsTb == 1 || y.IsSb == 1))),
                        BelumTerpasang = g.Select(x => x.Nop)
                                          .Distinct()
                                          .Count(nop => g.Any(y => y.Nop == nop &&
                                                  (y.IsTs == 0 && y.IsTb == 0 && y.IsSb == 0)))
                    })
                    .ToList();

                var result = new List<DetailSeriesPemasanganAlat>();

                foreach (var kategori in kategoriList)
                {
                    var s = new DetailSeriesPemasanganAlat
                    {
                        EnumPajak = (int)jenisPajak,
                        JenisPajak = jenisPajak.GetDescription(),
                        KategoriId = (int)kategori.Id,
                        KategoriNama = kategori.Nama
                    };

                    var dataKategori = query.Where(q => q.KategoriId == kategori.Id).ToList();

                    s.JumlahOP = dataKategori.Sum(d => d.JumlahOP);

                    foreach (var row in dataKategori)
                    {
                        if (row.Tahun == tahunMulai)
                        {
                            s.Terpasang2021 = row.Terpasang;
                            s.BelumTerpasang2021 = row.BelumTerpasang;
                        }
                        else if (row.Tahun == tahunMulai + 1)
                        {
                            s.Terpasang2022 = row.Terpasang;
                            s.BelumTerpasang2022 = row.BelumTerpasang;
                        }
                        else if (row.Tahun == tahunMulai + 2)
                        {
                            s.Terpasang2023 = row.Terpasang;
                            s.BelumTerpasang2023 = row.BelumTerpasang;
                        }
                        else if (row.Tahun == tahunMulai + 3)
                        {
                            s.Terpasang2024 = row.Terpasang;
                            s.BelumTerpasang2024 = row.BelumTerpasang;
                        }
                        else if (row.Tahun == tahunMulai + 4)
                        {
                            s.Terpasang2025 = row.Terpasang;
                            s.BelumTerpasang2025 = row.BelumTerpasang;
                        }
                    }

                    result.Add(s);
                }

                return result;
            }*/


            public static List<DataPemasanganAlat> GetTahunanPemasanganAlatList()
            {
                var ret = new List<DataPemasanganAlat>();
                var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;

                ret = context.DbRekamAlatGabungs
                    .Where(x => x.Tahun <= tahunSekarang)
                    .GroupBy(x => new { x.PajakId, x.Tahun })
                    .Select(g => new
                    {
                        g.Key.PajakId,
                        g.Key.Tahun,
                        JumlahOP = g.Count(),
                        TerpasangTS = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.IsTs == 1))),
                        TerpasangTB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.IsTb == 1))),
                        TerpasangSB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.IsSb == 1))),
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
            public static List<DetailDataPemasanganAlat> GetDetailTahunanPemasanganAlatList(EnumFactory.EPajak jenisPajak)
            {
                using var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;

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
                var query = context.DbRekamAlatGabungs
                    .Where(x => x.Tahun <= tahunSekarang
                                && x.PajakId == (int)jenisPajak)
                    .GroupBy(x => new { x.KategoriId })
                     .Select(g => new
                     {
                         g.Key.KategoriId,
                         JumlahOP = g.Count(),
                         TerpasangTS = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.IsTs == 1))),
                         TerpasangTB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.IsTb == 1))),
                         TerpasangSB = g.Select(x => x.Nop)
                         .Distinct()
                         .Count(nop => g.Any(y => y.Nop == nop &&
                                      (y.IsSb == 1))),
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
                    s.TerpasangTS = dataKategori.Sum(x => x.TerpasangTS);
                    s.TerpasangTB = dataKategori.Sum(x => x.TerpasangTB);
                    s.TerpasangSB = dataKategori.Sum(x => x.TerpasangSB);


                    result.Add(s);
                }

                return result;
            }

            public static List<SubDetailModal> GetSubDetailOPData(EnumFactory.EPajak jenisPajak, int kategori, int tahun, string status)
            {
                var ret = new List<SubDetailModal>();
                var context = DBClass.GetContext();
                if (status == "TerpasangTS")
                {
                    ret = context.DbRekamAlatGabungs
                    .Where(x => x.IsTs == 1 && x.Tahun <= tahun && x.KategoriId == kategori && x.PajakId == (int)jenisPajak)
                    .Select(x => new SubDetailModal()
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        Kategori = x.KategoriNama,
                        NamaOP = x.NamaOp,
                        NOP = x.Nop,
                        Alamat = x.AlamatOp,
                        TanggalPemasangan = x.TglTerpasang.HasValue ? x.TglTerpasang.Value : DateTime.MinValue,
                        JenisPasang = "Tax Surveillance"
                    })
                    .ToList();
                }
                else if (status == "TerpasangTB")
                {
                    ret = context.DbRekamAlatGabungs
                    .Where(x => x.IsTb == 1 && x.Tahun <= tahun && x.KategoriId == kategori && x.PajakId == (int)jenisPajak)
                    .Select(x => new SubDetailModal()
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        Kategori = x.KategoriNama,
                        NamaOP = x.NamaOp,
                        NOP = x.Nop,
                        Alamat = x.AlamatOp,
                        TanggalPemasangan = x.TglTerpasang.HasValue ? x.TglTerpasang.Value : DateTime.MinValue,
                        JenisPasang = "Tapping Box"
                    })
                    .ToList();
                }
                else if (status == "TerpasangSB")
                {
                    ret = context.DbRekamAlatGabungs
                    .Where(x => x.IsSb == 1 && x.Tahun <= tahun && x.KategoriId == kategori && x.PajakId == (int)jenisPajak)
                    .Select(x => new SubDetailModal()
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        Kategori = x.KategoriNama,
                        NamaOP = x.NamaOp,
                        NOP = x.Nop,
                        Alamat = x.AlamatOp,
                        TanggalPemasangan = x.TglTerpasang.HasValue ? x.TglTerpasang.Value : DateTime.MinValue,
                        JenisPasang = "Sinkron Box"
                    })
                    .ToList();
                }
                else if (status == "Terpasang")
                {
                    ret = context.DbRekamAlatGabungs
                    .Where(x => (x.IsTs == 1 || x.IsTb == 1 || x.IsSb == 1) && x.Tahun == tahun && x.KategoriId == kategori && x.PajakId == (int)jenisPajak)
                    .Select(x => new SubDetailModal()
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        Kategori = x.KategoriNama,
                        NamaOP = x.NamaOp,
                        NOP = x.Nop,
                        Alamat = x.AlamatOp,
                        TanggalPemasangan = x.TglTerpasang.HasValue ? x.TglTerpasang.Value : DateTime.MinValue,
                        JenisPasang = x.IsTs == 1 ? "Tax Surveillance"
                            : x.IsTb == 1 ? "Tapping Box"
                            : x.IsSb == 1 ? "Sinkron Box"
                            : "Belum Terpasang"
                    })
                    .ToList();
                }
                else if (status == "BelumTerpasang")
                {
                    ret = context.DbRekamAlatGabungs
                    .Where(x => (x.IsTs == 0 && x.IsTb == 0 && x.IsSb == 0) && x.Tahun <= tahun && x.KategoriId == kategori && x.PajakId == (int)jenisPajak)
                    .Select(x => new SubDetailModal()
                    {
                        JenisPajak = ((EnumFactory.EPajak)x.PajakId).GetDescription(),
                        Kategori = x.KategoriNama,
                        NamaOP = x.NamaOp,
                        NOP = x.Nop,
                        Alamat = x.AlamatOp,
                        TanggalPemasangan = x.TglTerpasang.HasValue ? x.TglTerpasang.Value : DateTime.MinValue,
                        JenisPasang = x.IsTs == 1 ? "Tax Surveillance"
                            : x.IsTb == 1 ? "Tapping Box"
                            : x.IsSb == 1 ? "Sinkron Box"
                            : "Belum Terpasang"
                    })
                    .ToList();
                }


                return ret;
            }

            public static List<SeriesPemasanganAlat> GetSeriesAlatRekam()
            {
                var ret = new List<SeriesPemasanganAlat>();
                var context = DBClass.GetContext();

                int tahunSekarang = DateTime.Now.Year;
                int tahunMulai = tahunSekarang - 4;

                ret = context.DbMonAlatRekams
                   .Where(x => x.Tahun >= tahunMulai && x.Tahun <= tahunSekarang)
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

            public static List<DetailSeriesPemasanganAlat> GetDetailSeriesAlatRekam(EnumFactory.EPajak jenisPajak)
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
                    .Where(x => x.Tahun >= tahunMulai && x.Tahun <= tahunSekarang
                                && x.PajakId == (int)jenisPajak)
                    .GroupBy(x => new { x.KategoriId, x.Tahun })
                    .Select(g => new
                    {
                        g.Key.KategoriId,
                        g.Key.Tahun,
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

                        TahunMines4 = dataKategori.Where(d => d.Tahun == tahunMulai)
                                                  .Sum(d => d.Terpasang5),
                        TahunMines3 = dataKategori.Where(d => d.Tahun == tahunMulai + 1)
                                                  .Sum(d => d.Terpasang4),
                        TahunMines2 = dataKategori.Where(d => d.Tahun == tahunMulai + 2)
                                                  .Sum(d => d.Terpasang3),
                        TahunMines1 = dataKategori.Where(d => d.Tahun == tahunMulai + 3)
                                                  .Sum(d => d.Terpasang2),
                        TahunNow = dataKategori.Where(d => d.Tahun == tahunMulai + 4)
                                                  .Sum(d => d.Terpasang1)
                    };
                }).ToList();



                return result;
            }
        }

        public class OPInfo
        {
            public string Nop { get; set; }
            public int JenisPajak { get; set; }
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

            public bool IsTerpasangTS { get; set; }
            public bool IsTerpasangTB { get; set; }
            public bool IsTerpasangSB { get; set; }

        }

    }
}
