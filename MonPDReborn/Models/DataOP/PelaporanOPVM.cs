using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System.Globalization;

namespace MonPDReborn.Models.DataOP
{
    public class PelaporanOPVM
    {
        // Tidak ada filter
        public class Index
        {
        }

        // Tampilkan semua data hasil pelaporan
        public class Show
        {
            public List<HasilPelaporan> DaftarHasil { get; set; } = new();

            public Show()
            {
                DaftarHasil = Method.GetPalporanList();
            }

        }

        public class Detail
        {
            public List<StatusPelaporanBulanan> DaftarRealisasi { get; set; } = new();
            public int TotalNilai { get; set; }
            public Detail(string nop)
            {
                var currentYear = DateTime.Now.Year;
                var data = Method.GetDetailListByNOP(nop, currentYear);

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
            public static List<HasilPelaporan> GetPalporanList()
            {
                var ret = new List<HasilPelaporan>();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                var dataTerlaporResto = context.DbMonRestos
                    .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                    .GroupBy(x => x.Nop)
                    .Select(g => new
                    {
                        Nop = g.Key,
                        Count = g.Count()
                    })
                    .ToDictionary(x => x.Nop, x => x.Count);
                var laporResto = context.DbMonRestos
                    .Where(x => x.TglKetetapan.HasValue && x.TglKetetapan.Value.Year == currentYear)
                    .GroupBy(x => x.Nop)
                    .Select(g => g.First()) // ambil salah satu data per NOP untuk ditampilkan
                    .Select(x => new HasilPelaporan
                    {
                        
                        NOP = x.Nop,
                        Nama = x.NamaOp,
                        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                        Wilayah = "",
                        Status = "", // isi sesuai kebutuhan
                        PajakTerlapor = dataTerlaporResto.ContainsKey(x.Nop) ? dataTerlaporResto[x.Nop] : 0,
                        MasaBelumLapor = 12 - (dataTerlaporResto.ContainsKey(x.Nop) ? dataTerlaporResto[x.Nop] : 0),
                        PajakSeharusnya = 12,
                        Alamat = x.AlamatOp
                    }).ToList();
                return ret;
            }

            public static List<RealisasiBulanan> GetDetailListByNOP(string nop, int tahun)
            {
                var ret = new List<RealisasiBulanan>();
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
            public string Tahun { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime? TanggalLapor { get; set; }
            public int Nilai { get; set; } // <--- tambahkan properti ini
            public int TotaNilai { get; set; }

        }

        // Model tampilan detail bulanan auto-generate
        public class StatusPelaporanBulanan
        {
            public int Id { get; set; }
            public string Bulan { get; set; } = null!;
            public string Status { get; set; } = null!;
            public int Nilai { get; set; }
            public int TotalNilai { get; set; }
            public string? TanggalLapor { get; set; }
        }

    }
}
