using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EFPenyelia;
using MonPDLib.General;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class PenyeliaVM
    {
        private static PenyeliaContext _context = DBClass.GetPenyeliaContext();
        public class Index
        {
            public int SelectedTahun { get; set; } = DateTime.Now.Year;
            public int SelectedBulan { get; set; } = DateTime.Now.Month;
            public string SelectedBidang { get; set; } = null!;
            public List<SelectListItem> BulanList { get; set; } = new();
            public List<SelectListItem> TahunList { get; set; } = new();
            public Index()
            {
                SelectedBulan = DateTime.Now.Month;
                for (int i = 1; i <= 12; i++)
                {
                    var namaBulan = new DateTime(1, i, 1).ToString("MMMM", new CultureInfo("id-ID"));
                    BulanList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = namaBulan
                    });
                }
                for (int i = 2025; i >= 2021; i--)
                {
                    TahunList.Add(new SelectListItem
                    {
                        Value = i.ToString(),
                        Text = i.ToString()
                    });
                }
            }
        }
        public class Show
        {
            public List<ViewModels.Penyelia> Data { get; set; } = new();
            public Show(int tahun, int bulan, string bidang)
            {
                Data = Methods.GetDataPenyelia(tahun, bulan, bidang);
            }

        }
        public class Detail
        {
            public List<ViewModels.DetailPenyelia> Data { get; set; } = new();
            public Detail(int tahun, int bulan, string nip)
            {
                Data = Methods.GetDetailPenyelia(tahun, bulan, nip);
            }
        }
        public class DetailPajak
        {
            public List<ViewModels.DetailNop> Data { get; set; } = new();
            public DetailPajak(int tahun, int bulan, string nip, int pajakId)
            {
                Data = Methods.GetDataNop(tahun, bulan, nip, pajakId);
            }
        }
    
        public class ViewModels
        {
            public class Penyelia
            {
                public string Nip { get; set; } = null!;
                public string Nama { get; set; } = null!;
                public decimal JmlNOP { get; set; }
                public decimal Capaian { get; set; }
                public int Tahun { get; set; }
                public int Bulan { get; set; }
            }
            public class DetailPenyelia
            {
                public string Nip { get; set; } = null!;
                public string JenisPajak { get; set; } = null!;
                public int pajakId { get; set; }
                public int SudahBayar { get; set; }
                public decimal JmlNOP { get; set; }
                public decimal Capaian { get; set; }
                public int Tahun { get; set; }
                public int Bulan { get; set; }
            }
            public class DetailNop
            {
                public string Nip { get; set; } = null!;
                public int pajakId { get; set; }
                public string Nop { get; set; } = null!;
                public string Alamat { get; set; } = null!;
                public string NamaOP { get; set; } = null!;
                public string Upaya { get; set; } = null!;
                public decimal Capaian { get; set; }
            }
            public class BidangView
            {
                public string Value { get; set; } = null!;
                public string Text { get; set; } = null!;
            }
        }

        public class Methods
        {
            public static List<ViewModels.Penyelia> GetDataPenyelia(int tahun, int bulan, string bidang)
            {
                var MonPdContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.Penyelia>();

                // Ambil semua NOP yang sudah bayar di bulan & tahun tertentu
                var nopSudahBayar = new HashSet<string>(
                    MonPdContext.DbMonRestos
                        .Where(x => x.NominalPokokBayar.HasValue &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month == bulan)
                        .Select(x => x.Nop)
                        .Concat(MonPdContext.DbMonHotels
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonHiburans
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonParkirs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonPpjs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonAbts
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Distinct()
                        .ToList()
                );

                // Ambil aktivitas, tapi tidak lagi dipakai untuk menentukan capaian
                var aktivitasPerNop = context.TAktifitasPegawais
                    .GroupBy(a => a.Nop)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Hitung capaian per pegawai
                ret = context.MPegawaiBarus
                    .Where(p => p.UnitKerja == bidang)
                    .AsEnumerable()
                    .Select(p =>
                    {
                        var nopsPegawai = context.MPegawaiOpDets
                            .Where(op => op.Nip == p.NipNik)
                            .Select(op => op.Nop)
                            .ToList();

                        var totalNOP = nopsPegawai.Count;
                        if (totalNOP == 0)
                            return new ViewModels.Penyelia
                            {
                                Nip = p.NipNik ?? "-",
                                Nama = p.Nama,
                                JmlNOP = 0,
                                Capaian = 0
                            };

                        // 🔹 Capaian hanya dihitung untuk NOP yang sudah bayar
                        var totalCapaian = nopsPegawai.Count(nop => nopSudahBayar.Contains(nop));

                        var capaianPersen = (decimal)totalCapaian / totalNOP;

                        return new ViewModels.Penyelia
                        {
                            Nip = p.NipNik ?? "-",
                            Nama = p.Nama,
                            JmlNOP = totalNOP,
                            Tahun = tahun,
                            Bulan = bulan,
                            Capaian = Math.Round(capaianPersen, 2)
                        };
                    })
                    .ToList();

                return ret;

            }
            public static List<ViewModels.DetailPenyelia> GetDetailPenyelia(int tahun, int bulan, string nip)
            {
                var MonPdContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.DetailPenyelia>();

                // Ambil semua NOP yang sudah bayar
                var nopSudahBayar = new HashSet<string>(
                    MonPdContext.DbMonRestos
                        .Where(x => x.NominalPokokBayar.HasValue &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month == bulan)
                        .Select(x => x.Nop)
                        .Concat(MonPdContext.DbMonHotels
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonHiburans
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonParkirs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonPpjs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonAbts
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Distinct()
                );

                // Ambil semua NOP pegawai
                var pegawaiOps = context.MPegawaiOpDets
                    .Where(x => x.Nip == nip)
                    .Include(x => x.NopNavigation)
                    .ToList();

                // Kelompokkan berdasarkan jenis pajak
                var grupPerPajak = pegawaiOps
                    .GroupBy(x => x.NopNavigation.PajakId)
                    .ToList();

                foreach (var grup in grupPerPajak)
                {
                    var pajakId = (int)grup.Key;
                    var namaPajak = ((EnumFactory.EPajak)pajakId).GetDescription();

                    var totalNOP = grup.Count();
                    var sudahBayar = grup.Count(x => nopSudahBayar.Contains(x.Nop));
                    var capaian = totalNOP > 0 ? Math.Round((decimal)sudahBayar / totalNOP, 2) : 0;

                    ret.Add(new ViewModels.DetailPenyelia
                    {
                        Nip = nip,
                        pajakId = pajakId,
                        JenisPajak = namaPajak,
                        JmlNOP = totalNOP,
                        SudahBayar = sudahBayar,
                        Capaian = capaian,
                        Tahun = tahun,
                        Bulan = bulan
                    });
                }

                return ret;
            }
            public static List<ViewModels.DetailNop> GetDataNop(int tahun, int bulan, string nip, int pajakId)
            {
                var MonPdContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.DetailNop>();

                // 🔹 1. Kumpulkan semua NOP yang sudah bayar dari semua jenis pajak
                var nopSudahBayar = new HashSet<string>(
                    MonPdContext.DbMonRestos
                        .Where(x => x.NominalPokokBayar.HasValue &&
                                    x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    x.TglBayarPokok.Value.Month == bulan)
                        .Select(x => x.Nop)
                        .Concat(MonPdContext.DbMonHotels
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonHiburans
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonParkirs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonPpjs
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Concat(MonPdContext.DbMonAbts
                            .Where(x => x.NominalPokokBayar.HasValue &&
                                        x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        x.TglBayarPokok.Value.Month == bulan)
                            .Select(x => x.Nop))
                        .Distinct()
                );

                // 🔹 2. Ambil semua objek pajak milik pegawai untuk jenis pajak tertentu
                var dataPegawai = context.MPegawaiOpDets
                    .Where(x => x.Nip == nip && x.NopNavigation.PajakId == pajakId)
                    .Include(x => x.NopNavigation)
                    .Select(x => new
                    {
                        x.Nip,
                        x.Nop,
                        x.NopNavigation.NamaOp,
                        x.NopNavigation.AlamatOp
                    })
                    .ToList();

                // 🔹 3. Ambil data Upaya dari tabel monitoring (contoh pakai DbMonUpayas)
                var dataUpaya = context.TAktifitasPegawais
                    .Where(x => x.TanggalAktifitas.Value.Year == tahun && x.TanggalAktifitas.Value.Month == bulan && x.Nip == nip)
                    .Select(x => new { x.Nop, x.Aktifitas })
                    .ToList();

                // 🔹 4. Mapping hasil ke ViewModel
                foreach (var item in dataPegawai)
                {
                    bool sudahBayar = nopSudahBayar.Contains(item.Nop);

                    var upayaList = dataUpaya
                        .Where(u => u.Nop == item.Nop)
                        .Select(u => u.Aktifitas)
                        .Distinct()
                        .ToList();

                    string upayaText = "-";
                    if (upayaList.Any())
                    {
                        upayaText = $"{upayaList.Count}x: " + string.Join(", ", upayaList);
                    }

                    // capaian 100% kalau sudah bayar, 0 kalau belum
                    decimal capaian = sudahBayar ? 10 : 0m;

                    ret.Add(new ViewModels.DetailNop
                    {
                        Nip = item.Nip,
                        pajakId = pajakId,
                        Nop = item.Nop,
                        NamaOP = item.NamaOp,
                        Alamat = item.AlamatOp,
                        Upaya = upayaText,
                        Capaian = capaian
                    });
                }

                return ret;
            }

        }
    }
}
