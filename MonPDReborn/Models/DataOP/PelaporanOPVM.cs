using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class PelaporanOPVM
    {

        // Tidak ada filter
        public class Index
        {
            public int SelectedPajak { get; set; }
            public List<SelectListItem> JenisPajakList { get; set; }
            public Index()
            {
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Where(x => x != EnumFactory.EPajak.PBB && x != EnumFactory.EPajak.OpsenBbnkb && x != EnumFactory.EPajak.OpsenPkb && x != EnumFactory.EPajak.BPHTB && x != EnumFactory.EPajak.Reklame)
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
            }

        }

        // Tampilkan semua data hasil pelaporan
        public class Show
        {
            public List<HasilPelaporan> DaftarHasil { get; set; } = new();

            public Show(EnumFactory.EPajak JenisPajak)
            {
                DaftarHasil = Method.GetPalporanList(JenisPajak);
            }

        }

        public class Detail
        {
            public List<StatusPelaporanBulanan> DaftarRealisasi { get; set; } = new();
            public decimal TotalNilai { get; set; }
            public Detail(EnumFactory.EPajak jenisPajak, string nop)
            {
                var currentYear = DateTime.Now.Year;
                var data = Method.GetDetailListByNOP(jenisPajak, nop, currentYear);

                DaftarRealisasi = Enumerable.Range(1, 12).Select((bulanKe, index) =>
                {
                    var dataBulanIni = data.FirstOrDefault(x => x.BulanKe == bulanKe);
                    return new StatusPelaporanBulanan
                    {
                        Id = bulanKe,
                        Bulan = new DateTime(1, bulanKe, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Status = dataBulanIni?.Status ?? "Belum Lapor",
                        TanggalLapor = dataBulanIni?.TanggalLapor?.ToString("dd MMMM yyyy", new CultureInfo("id-ID")) ?? "-",
                        Nilai = dataBulanIni?.Nilai ?? 0
                    };
                }).ToList();

                // Hitung total nilai dari data yang ada
                TotalNilai = DaftarRealisasi.Sum(d => d.Nilai);
            }
        }
        public static class Method
        {
            public static List<HasilPelaporan> GetPalporanList(EnumFactory.EPajak JenisPajak)
            {
                var ret = new List<HasilPelaporan>();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataTerlaporResto = context.DbMonRestos
                        .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                        .GroupBy(x => x.Nop)
                        .Select(g => new
                        {
                            Nop = g.Key,
                            Count = g.Count()
                        })
                        .ToList();

                        var getWilayahResto = context.DbOpRestos
                            .Select(x => new
                            {
                                Nop = x.Nop,
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                        var laporResto = context.DbMonRestos
                            .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                            .GroupBy(x => x.Nop)
                            .Select(g => g.First())
                            .ToList()
                            .Select(x => new HasilPelaporan
                            {
                                NOP = x.Nop,
                                Nama = x.NamaOp,
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Wilayah = getWilayahResto.Where(y => y.Nop == x.Nop).Select(y => y.Wilayah).FirstOrDefault() ?? "",
                                Status = "",
                                PajakTerlapor = dataTerlaporResto.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0,
                                MasaBelumLapor = 12 - (dataTerlaporResto.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0),
                                PajakSeharusnya = 12,
                                Alamat = x.AlamatOp
                            }).ToList();

                        ret.AddRange(laporResto);
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataTerlaporListrik = context.DbMonPpjs
                        .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                        .GroupBy(x => x.Nop)
                        .Select(g => new
                        {
                            Nop = g.Key,
                            Count = g.Count()
                        })
                        .ToList();

                        var getWilayahListrik = context.DbOpListriks
                            .Select(x => new
                            {
                                Nop = x.Nop,
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                        var laporListrik = context.DbMonPpjs
                            .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                            .GroupBy(x => x.Nop)
                            .Select(g => g.First())
                            .ToList()
                            .Select(x => new HasilPelaporan
                            {
                                NOP = x.Nop,
                                Nama = x.NamaOp,

                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Wilayah = getWilayahListrik.Where(y => y.Nop == x.Nop).Select(y => y.Wilayah).FirstOrDefault() ?? "",
                                Status = "",
                                PajakTerlapor = dataTerlaporListrik.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0,
                                MasaBelumLapor = 12 - (dataTerlaporListrik.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0),
                                PajakSeharusnya = 12,
                                Alamat = x.AlamatOp
                            }).ToList();

                        ret.AddRange(laporListrik);
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataTerlaporHotel = context.DbMonHotels
                        .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                        .GroupBy(x => x.Nop)
                        .Select(g => new
                        {
                            Nop = g.Key,
                            Count = g.Count()
                        })
                        .ToList();

                        var getWilayahHotel = context.DbOpHotels
                            .Select(x => new
                            {
                                Nop = x.Nop,
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                        var laporHotel = context.DbMonHotels
                            .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                            .GroupBy(x => x.Nop)
                            .Select(g => g.First())
                            .ToList()
                            .Select(x => new HasilPelaporan
                            {
                                NOP = x.Nop,
                                Nama = x.NamaOp,
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Wilayah = getWilayahHotel.Where(y => y.Nop == x.Nop).Select(y => y.Wilayah).FirstOrDefault() ?? "",
                                Status = "",
                                PajakTerlapor = dataTerlaporHotel.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0,
                                MasaBelumLapor = 12 - (dataTerlaporHotel.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0),
                                PajakSeharusnya = 12,
                                Alamat = x.AlamatOp
                            }).ToList();

                        ret.AddRange(laporHotel);
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataTerlaporParkir = context.DbMonParkirs
                        .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                        .GroupBy(x => x.Nop)
                        .Select(g => new
                        {
                            Nop = g.Key,
                            Count = g.Count()
                        })
                        .ToList();

                        var getWilayahParkir = context.DbOpParkirs
                            .Select(x => new
                            {
                                Nop = x.Nop,
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                        var laporParkir = context.DbMonParkirs
                            .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                            .GroupBy(x => x.Nop)
                            .Select(g => g.First())
                            .ToList()
                            .Select(x => new HasilPelaporan
                            {
                                NOP = x.Nop,
                                Nama = x.NamaOp,
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Wilayah = getWilayahParkir.Where(y => y.Nop == x.Nop).Select(y => y.Wilayah).FirstOrDefault() ?? "",
                                Status = "",
                                PajakTerlapor = dataTerlaporParkir.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0,
                                MasaBelumLapor = 12 - (dataTerlaporParkir.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0),
                                PajakSeharusnya = 12,
                                Alamat = x.AlamatOp
                            }).ToList();

                        ret.AddRange(laporParkir);
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataTerlaporHiburan = context.DbMonHiburans
                        .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                        .GroupBy(x => x.Nop)
                        .Select(g => new
                        {
                            Nop = g.Key,
                            Count = g.Count()
                        })
                        .ToList();

                        var getWilayahHiburan = context.DbOpHiburans
                            .Select(x => new
                            {
                                Nop = x.Nop,
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                        var laporHiburan = context.DbMonHiburans
                            .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                            .GroupBy(x => x.Nop)
                            .Select(g => g.First())
                            .ToList()
                            .Select(x => new HasilPelaporan
                            {
                                NOP = x.Nop,
                                Nama = x.NamaOp,
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Wilayah = getWilayahHiburan.Where(y => y.Nop == x.Nop).Select(y => y.Wilayah).FirstOrDefault() ?? "",
                                Status = "",
                                PajakTerlapor = dataTerlaporHiburan.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0,
                                MasaBelumLapor = 12 - (dataTerlaporHiburan.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0),
                                PajakSeharusnya = 12,
                                Alamat = x.AlamatOp
                            }).ToList();

                        ret.AddRange(laporHiburan);
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataTerlaporAbt = context.DbMonAbts
                        .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                        .GroupBy(x => x.Nop)
                        .Select(g => new
                        {
                            Nop = g.Key,
                            Count = g.Count()
                        })
                        .ToList();

                        var getWilayahAbt = context.DbOpAbts
                            .Select(x => new
                            {
                                Nop = x.Nop,
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                        var laporAbt = context.DbMonAbts
                            .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                            .GroupBy(x => x.Nop)
                            .Select(g => g.First())
                            .ToList()
                            .Select(x => new HasilPelaporan
                            {
                                NOP = x.Nop,
                                Nama = x.NamaOp,
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Wilayah = getWilayahAbt.Where(y => y.Nop == x.Nop).Select(y => y.Wilayah).FirstOrDefault() ?? "",
                                Status = "",
                                PajakTerlapor = dataTerlaporAbt.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0,
                                MasaBelumLapor = 12 - (dataTerlaporAbt.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0),
                                PajakSeharusnya = 12,
                                Alamat = x.AlamatOp
                            }).ToList();

                        ret.AddRange(laporAbt);
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataTerlaporPbb = context.DbMonPbbs
                        .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                        .GroupBy(x => x.Nop)
                        .Select(g => new
                        {
                            Nop = g.Key,
                            Count = g.Count()
                        })
                        .ToList();

                        var getWilayahPbb = context.DbOpPbbs
                            .Select(x => new
                            {
                                Nop = x.Nop,
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                        var laporPbb = context.DbMonPbbs
                            .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                            .GroupBy(x => x.Nop)
                            .Select(g => g.First())
                            .ToList()
                            .Select(x => new HasilPelaporan
                            {
                                NOP = x.Nop,
                                Nama = x.NamaOp,
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Wilayah = getWilayahPbb.Where(y => y.Nop == x.Nop).Select(y => y.Wilayah).FirstOrDefault() ?? "",
                                Status = "",
                                PajakTerlapor = dataTerlaporPbb.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0,
                                MasaBelumLapor = 12 - (dataTerlaporPbb.FirstOrDefault(y => y.Nop == x.Nop)?.Count ?? 0),
                                PajakSeharusnya = 12,
                                Alamat = x.AlamatOp
                            }).ToList();

                        ret.AddRange(laporPbb);
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }


                return ret;
            }

            public static List<RealisasiBulanan> GetDetailListByNOP(EnumFactory.EPajak JenisPajak, string nop, int tahun)
            {
                var ret = new List<RealisasiBulanan>();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataResto = context.DbMonRestos
                            .Where(x => x.Nop == nop && x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, BulanKe = x.TglKetetapan.Value.Month, Tahun = x.TahunBuku })
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = g.Key.Nop,
                                BulanKe = g.Key.BulanKe,
                                Tahun = (int)g.Key.Tahun,
                                Status = "Sudah Lapor",
                                TanggalLapor = g.Max(x => x.TglKetetapan),
                                Nilai = g.Sum(x => x.PokokPajakKetetapan.Value)
                            }).ToList();
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrik = context.DbMonPpjs
                            .Where(x => x.Nop == nop && x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, BulanKe = x.TglKetetapan.Value.Month, Tahun = x.TahunBuku })
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = g.Key.Nop,
                                BulanKe = g.Key.BulanKe,
                                Tahun = (int)g.Key.Tahun,
                                Status = "Sudah Lapor",
                                TanggalLapor = g.Max(x => x.TglKetetapan),
                                Nilai = g.Sum(x => x.PokokPajakKetetapan.Value)
                            }).ToList();
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotel = context.DbMonHotels
                            .Where(x => x.Nop == nop && x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, BulanKe = x.TglKetetapan.Value.Month, Tahun = x.TahunBuku })
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = g.Key.Nop,
                                BulanKe = g.Key.BulanKe,
                                Tahun = (int)g.Key.Tahun,
                                Status = "Sudah Lapor",
                                TanggalLapor = g.Max(x => x.TglKetetapan),
                                Nilai = g.Sum(x => x.PokokPajakKetetapan.Value)
                            }).ToList();
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkir = context.DbMonParkirs
                            .Where(x => x.Nop == nop && x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, BulanKe = x.TglKetetapan.Value.Month, Tahun = x.TahunBuku })
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = g.Key.Nop,
                                BulanKe = g.Key.BulanKe,
                                Tahun = (int)g.Key.Tahun,
                                Status = "Sudah Lapor",
                                TanggalLapor = g.Max(x => x.TglKetetapan),
                                Nilai = g.Sum(x => x.PokokPajakKetetapan.Value)
                            }).ToList();
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburan = context.DbMonHiburans
                            .Where(x => x.Nop == nop && x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, BulanKe = x.TglKetetapan.Value.Month, Tahun = x.TahunBuku })
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = g.Key.Nop,
                                BulanKe = g.Key.BulanKe,
                                Tahun = (int)g.Key.Tahun,
                                Status = "Sudah Lapor",
                                TanggalLapor = g.Max(x => x.TglKetetapan),
                                Nilai = g.Sum(x => x.PokokPajakKetetapan.Value)
                            }).ToList();
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbt = context.DbMonAbts
                            .Where(x => x.Nop == nop && x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, BulanKe = x.TglKetetapan.Value.Month, Tahun = x.TahunBuku })
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = g.Key.Nop,
                                BulanKe = g.Key.BulanKe,
                                Tahun = (int)g.Key.Tahun,
                                Status = "Sudah Lapor",
                                TanggalLapor = g.Max(x => x.TglKetetapan),
                                Nilai = g.Sum(x => x.PokokPajakKetetapan.Value)
                            }).ToList();
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }

                //return new List<RealisasiBulanan>
                //{
                //    new() { NOP = "35.78.001.001.902.00001", BulanKe = 1, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 1, 15), Nilai = 45000000 },
                //    new() { NOP = "35.78.001.001.902.00001", BulanKe = 3, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 3, 18), Nilai = 45000000 },
                //    new() { NOP = "35.78.001.001.902.00001", BulanKe = 6, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 6, 10), Nilai = 45000000 },
                //    new() { NOP = "35.78.001.001.902.00004", BulanKe = 1, Tahun = "2025", Status ="Sudah Lapor",TanggalLapor = new DateTime(2025, 1, 20), Nilai = 45000000 },
                //};

                return ret;
            }
        }
        public class HasilPelaporan
        {
            public int No { get; set; }
            public string NOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string Wilayah { get; set; } = null!;
            public string Status { get; set; } = null!;
            public int MasaBelumLapor { get; set; }
            public int PajakSeharusnya { get; set; }
            public int PajakTerlapor { get; set; }
            public string Alamat { get; set; } = null!;
        }

        // Model realisasi pelaporan dari dummy (disederhanakan)
        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public int BulanKe { get; set; } // 1 = Januari, ..., 12 = Desember
            public int Tahun { get; set; }
            public string Status { get; set; } = null!;
            public DateTime? TanggalLapor { get; set; }
            public decimal Nilai { get; set; } // <--- tambahkan properti ini
            public decimal TotaNilai { get; set; }

        }

        // Model tampilan detail bulanan auto-generate
        public class StatusPelaporanBulanan
        {
            public int Id { get; set; }
            public string Bulan { get; set; } = null!;
            public string Status { get; set; } = null!;
            public decimal Nilai { get; set; }
            public decimal TotalNilai { get; set; }
            public string? TanggalLapor { get; set; }
        }

    }
}
