using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EFPenyelia;
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
            public Detail()
            {

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
            }
            public class DetailPenyelia
            {
                public string Nip { get; set; } = null!;
                public string JenisPajak { get; set; } = null!;
                public int pajakId { get; set; }
                public decimal JmlNOP { get; set; }
                public decimal Capaian { get; set; }
            }
            public class DetailNop
            {
                public string Nip { get; set; } = null!;
                public int pajakId { get; set; }
                public string Nop { get; set; } = null!;
                public string Alamat { get; set; } = null!;
                public string NamaOP { get; set; } = null!;
                public decimal JmlUpaya { get; set; }
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

                var aktivitasPerNop = context.TAktifitasPegawais
                    .GroupBy(a => a.Nop)
                    .ToDictionary(g => g.Key, g => g.Count());

                // === HITUNG CAPAIAN PER PEGAWAI ===
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

                        var totalCapaian = nopsPegawai.Count(nop =>
                            nopSudahBayar.Contains(nop) ||
                            (aktivitasPerNop.ContainsKey(nop) && aktivitasPerNop[nop] >= 4)
                        );

                        var capaianPersen = (decimal)totalCapaian / totalNOP * 100;

                        return new ViewModels.Penyelia
                        {
                            Nip = p.NipNik ?? "-",
                            Nama = p.Nama,
                            JmlNOP = totalNOP,
                            Capaian = Math.Round(capaianPersen, 2)
                        };
                    })
                    .ToList();

                return ret;

            }
        }
    }
}
