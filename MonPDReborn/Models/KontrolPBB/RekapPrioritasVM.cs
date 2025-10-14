using Microsoft.AspNetCore.Mvc;
using MonPDLib;

namespace MonPDReborn.Models.KontrolPBB
{
    public class RekapPrioritasVM
    {
        public class Index
        {
            public Index()
            {

            }
        }
        public class Show
        {
            public List<ViewModels.Prioritas> Data { get; set; } = new();
            public Show()
            {
                Data = Methods.GetPrioritas();
            }
        }
        public class ViewModels
        {
            public class Prioritas
            {
                public string NOP { get; set; } = null!;
                public string NamaWP { get; set; } = null!;
                public decimal Ketetapan { get; set; }
                public decimal Realisasi { get; set; }
                public decimal Tunggakan { get; set; }
                public decimal Tahun { get; set; }
                public string PrioritasKet { get;set; } = null!;
                public string Petugas { get; set; } = null!;
                public string StatusTagih { get;set; } = null!;
            }
        }
        public class Methods
        {
            public static List<ViewModels.Prioritas> GetPrioritas()
            {
                var context = DBClass.GetContext();
                var tahun = DateTime.Now.Year;

                var ret = context.DbMonPbbSummaries
                    .Where(x => x.TahunPajak == tahun)
                    .GroupBy(x => new { x.Nop, x.NamaWp, x.Petugas, x.Upaya }) // grouping per NOP dan info WP
                    .Select(g => new ViewModels.Prioritas()
                    {
                        NOP = g.Key.Nop ?? "-",
                        NamaWP = g.Key.NamaWp ?? "-",
                        Ketetapan = g.Sum(x => x.Ketetapan ?? 0),
                        Realisasi = g.Sum(x => x.Realisasi ?? 0),
                        Tahun = tahun,
                        Petugas = g.Key.Petugas ?? "-",
                        StatusTagih = g.Key.Upaya ?? "-",
                        Tunggakan = g.Sum(x => (x.Ketetapan ?? 0) - (x.Realisasi ?? 0))
                    })
                    .ToList();

                // Tentukan prioritas berdasarkan total tunggakan
                ret.ForEach(x =>
                {
                    if (x.Tunggakan >= 100_000_000)
                        x.PrioritasKet = "Tinggi";
                    else if (x.Tunggakan >= 25_000_000)
                        x.PrioritasKet = "Sedang";
                    else
                        x.PrioritasKet = "Rendah";
                });

                // Ambil 100 NOP dengan tunggakan tertinggi
                ret = ret
                    .OrderByDescending(x => x.Tunggakan)
                    .Take(100)
                    .ToList();

                return ret;
            }

        }
    }
}
