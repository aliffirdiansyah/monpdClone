using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EFPenyelia;
using MonPDLib.General;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class PADSummaryVM
    {
        public class Index
        {
            public int SelectedTahun { get; set; } = DateTime.Now.Year;
            public int SelectedBulan { get; set; } = DateTime.Now.Month;
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
            public List<ViewModels.PADShow> Data { get; set; } = new();
            public Show(int tahun, int bulan)
            {
                Data = Methods.GetDataPAD(tahun, bulan);
            }
        }
        public class Kategori
        {
            public List<ViewModels.PADKategori> Data { get; set; } = new();
            public Kategori(int tahun, int bulan, EnumFactory.EPajak pajakId)
            {
                Data = Methods.GetDataKategori(tahun, bulan, pajakId);
            }
        }
        public class DetailOPbuka
        {
            public List<ViewModels.DetailOP> Data { get; set; } = new();
            public DetailOPbuka(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId)
            {
                Data = Methods.GetDetailOPBuka(tahun, bulan, pajakId, kategoriId);
            }
        }
        public class DetailUpaya
        {
            public List<ViewModels.DetailUpaya> Data { get; set; } = new();
            public DetailUpaya(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId, int upaya)
            {
                Data = Methods.GetDetailUpaya(tahun, bulan, pajakId, kategoriId, upaya);
            }
        }
        public class DetailTotalOPKategori
        {
            public List<ViewModels.TotalOPKategori> Data { get; set; } = new();
            public DetailTotalOPKategori(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId)
            {
                Data = Methods.GetOPTotalKategori(tahun, bulan, pajakId, kategoriId);
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
            public class PADShow
            {
                public int PajakID { get; set; }
                public string JenisPajak { get; set; } = null!;
                public decimal OpBayar { get; set; }
                public decimal OpBlmBayar => OPbuka - OpBayar;
                public decimal SudahBayar { get; set; }
                public decimal BelumBayar { get; set; }
                public int Tahun { get; set; }
                public int Bulan { get; set; }
                public decimal OPbuka { get; set; }
                public decimal Himbauan { get; set; }
                public decimal Teguran { get; set; }
                public decimal Silang { get; set; }
                public decimal Penutupan { get; set; }
            }
            public class PADKategori
            {
                public int PajakID { get; set; }
                public int KategoriID { get; set; }
                public string JenisPajak { get; set; } = null!;
                public string Kategori { get; set; } = null!;
                public int OPbuka { get; set; }
                public int OpBayar { get; set; }
                public int OpBlmBayar => OPbuka - OpBayar;
                public decimal? SudahBayar { get; set; }
                public int Tahun { get; set; }
                public int Bulan { get; set; }
                public int Himbauan { get; set; }
                public int Teguran { get; set; }
                public int Silang { get; set; }
                public int Penutupan { get; set; }
            }
            public class TotalOPKategori
            {
                public string Nop { get; set; } = null!;
                public string FormattedNOP => Utility.GetFormattedNOP(Nop);
                public string NamaOP { get; set; } = null!;
                public string Alamat { get; set; } = null!;
                public string Wilayah { get; set; } = null!;
            }

            public class DetailOP
            {
                public string Nop { get; set; } = null!;
                public string FormattedNOP => Utility.GetFormattedNOP(Nop);
                public string NamaOP { get; set; } = null!;
                public string Alamat { get; set; } = null!;
                public decimal? SudahBayar { get; set; }
                public DateTime TglBayar { get; set; }
            }
            public class DetailUpaya
            {
                public string Nop { get; set; } = null!;
                public string FormattedNOP => Utility.GetFormattedNOP(Nop);
                public string NamaOP { get; set; } = null!;
                public string Alamat { get; set; } = null!;
                public DateTime TglUpaya { get; set; }
            }
        }
        
        private static PenyeliaContext _context = DBClass.GetPenyeliaContext();
        public class Methods
        {
            public static List<ViewModels.PADShow> GetDataPAD(int tahun, int bulan)
            {
                var MonPDContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.PADShow>();

                var nopList = context.MObjekPajaks
                    .Select(x => new { x.Nop, x.PajakId })
                    .ToList();

                var nopSet = new HashSet<string>(nopList.Select(n => n.Nop));

                var dataRealisasi = new List<(int PajakId, int Bulan, int SudahBayar, decimal NominalBayar)>();

                // Hotel
                dataRealisasi.AddRange(
                    MonPDContext.DbMonHotels
                        .Where(x => x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    nopSet.Contains(x.Nop))
                        .GroupBy(x => x.TglBayarPokok.Value.Month)
                        .Select(g => new
                        {
                            PajakId = (int)EnumFactory.EPajak.JasaPerhotelan,
                            Bulan = g.Key,
                            SudahBayar = g.Where(x => x.NominalPokokBayar.HasValue && x.NominalPokokBayar.Value > 0 && x.TglBayarPokok.Value.Month == bulan)
                                            .Select(x => x.Nop)
                                             .Distinct()
                                             .Count(),
                            NominalBayar = g.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0,
                        })
                        .AsEnumerable()
                        .Select(x => (x.PajakId, x.Bulan, x.SudahBayar, x.NominalBayar))
                );

                // Mamin
                dataRealisasi.AddRange(
                    MonPDContext.DbMonRestos
                        .Where(x => x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun && x.PajakNama != "MAMIN" &&
                                    nopSet.Contains(x.Nop))
                        .GroupBy(x => x.TglBayarPokok.Value.Month)
                        .Select(g => new
                        {
                            PajakId = (int)EnumFactory.EPajak.MakananMinuman,
                            Bulan = g.Key,
                            SudahBayar = g.Where(x => x.TglBayarPokok.Value.Month == bulan && x.NominalPokokBayar > 0)
                                             .Select(x => x.Nop)
                                             .Distinct()
                                             .Count(),
                            NominalBayar = g.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0,
                        })
                        .AsEnumerable()
                        .Select(x => (x.PajakId, x.Bulan, x.SudahBayar, x.NominalBayar))
                );

                // Hiburan
                dataRealisasi.AddRange(
                    MonPDContext.DbMonHiburans
                        .Where(x => x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    nopSet.Contains(x.Nop))
                        .GroupBy(x => x.TglBayarPokok.Value.Month)
                        .Select(g => new
                        {
                            PajakId = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                            Bulan = g.Key,
                            SudahBayar = g.Where(x => x.NominalPokokBayar.HasValue && x.NominalPokokBayar.Value > 0 && x.TglBayarPokok.Value.Month == bulan)
                                            .Select(x => x.Nop)
                                             .Distinct()
                                             .Count(),
                            NominalBayar = g.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0,
                        })
                        .AsEnumerable()
                        .Select(x => (x.PajakId, x.Bulan, x.SudahBayar, x.NominalBayar))
                );

                // Parkir
                dataRealisasi.AddRange(
                    MonPDContext.DbMonParkirs
                        .Where(x => x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    nopSet.Contains(x.Nop))
                        .GroupBy(x => x.TglBayarPokok.Value.Month)
                        .Select(g => new
                        {
                            PajakId = (int)EnumFactory.EPajak.JasaParkir,
                            Bulan = g.Key,
                            SudahBayar = g.Where(x => x.NominalPokokBayar.HasValue && x.NominalPokokBayar.Value > 0 && x.TglBayarPokok.Value.Month == bulan)
                                            .Select(x => x.Nop)
                                            .Distinct()
                                            .Count(),
                            NominalBayar = g.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0,
                        })
                        .AsEnumerable()
                        .Select(x => (x.PajakId, x.Bulan, x.SudahBayar, x.NominalBayar))
                );

                // listrik
                dataRealisasi.AddRange(
                    MonPDContext.DbMonPpjs
                        .Where(x => x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    nopSet.Contains(x.Nop))
                        .GroupBy(x => x.TglBayarPokok.Value.Month)
                        .Select(g => new
                        {
                            PajakId = (int)EnumFactory.EPajak.TenagaListrik,
                            Bulan = g.Key,
                            SudahBayar = g.Where(x => x.NominalPokokBayar.HasValue && x.NominalPokokBayar.Value > 0 && x.TglBayarPokok.Value.Month == bulan)
                                            .Select(x => x.Nop)
                                            .Distinct()
                                            .Count(),
                            NominalBayar = g.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0,
                        })
                        .AsEnumerable()
                        .Select(x => (x.PajakId, x.Bulan, x.SudahBayar, x.NominalBayar))
                );

                // abt
                dataRealisasi.AddRange(
                    MonPDContext.DbMonAbts
                        .Where(x => x.TglBayarPokok.HasValue &&
                                    x.TglBayarPokok.Value.Year == tahun &&
                                    nopSet.Contains(x.Nop))
                        .GroupBy(x => x.TglBayarPokok.Value.Month)
                        .Select(g => new
                        {
                            PajakId = (int)EnumFactory.EPajak.AirTanah,
                            Bulan = g.Key,
                            SudahBayar = g.Where(x => x.NominalPokokBayar.HasValue && x.NominalPokokBayar.Value > 0 && x.TglBayarPokok.Value.Month == bulan)
                                            .Select(x => x.Nop)
                                            .Distinct()
                                            .Count(),
                            NominalBayar = g.Sum(x => (decimal?)x.NominalPokokBayar) ?? 0,
                        })
                        .AsEnumerable()
                        .Select(x => (x.PajakId, x.Bulan, x.SudahBayar, x.NominalBayar))
                );


                // --- Kelompokkan per jenis pajak ---
                var realisasiGrouped = dataRealisasi
                    .GroupBy(x => x.PajakId)
                    .Select(g => new
                    {
                        PajakID = g.Key,
                        OpBayar = g.Sum(x => x.SudahBayar),
                        SudahBayar = g.Where(x => x.Bulan == bulan).Sum(x => x.NominalBayar),
                    })
                    .ToList();

                // --- Data aktifitas (himbauan, teguran, silang) ---
                var upaya = context.TAktifitasPegawais
                    .Where(x => x.TanggalAktifitas.HasValue &&
                                x.TanggalAktifitas.Value.Year == tahun &&
                                x.TanggalAktifitas.Value.Month == bulan)
                    .GroupBy(x => x.Nop)
                    .Select(g => new
                    {
                        Nop = g.Key,
                        Himbauan = g.Count(x => x.Aktifitas == "HIMBAUAN"),
                        Teguran = g.Count(x => x.Aktifitas == "TEGURAN"),
                        Silang = g.Count(x => x.Aktifitas == "SILANG"),
                        Penutupan = g.Count(x => x.Aktifitas == "PENUTUPAN")
                    })
                    .ToList();

                // --- Data OP buka per jenis pajak ---
                var groupedNop = nopList
                    .GroupBy(x => x.PajakId)
                    .Select(g => new
                    {
                        PajakID = g.Key,
                        JenisPajak = ((EnumFactory.EPajak)g.Key).GetDescription(),
                        OPbuka = g.Count()
                    })
                    .ToList();

                // --- Gabungkan semua data ---
                var retData = (from n in groupedNop
                               join r in realisasiGrouped on n.PajakID equals r.PajakID into nr
                               from r in nr.DefaultIfEmpty()
                               select new ViewModels.PADShow
                               {
                                   PajakID = (int)n.PajakID,
                                   JenisPajak = n.JenisPajak,
                                   Tahun = tahun,
                                   Bulan = bulan,
                                   OPbuka = n.OPbuka,
                                   OpBayar = r?.OpBayar ?? 0,
                                   SudahBayar = r?.SudahBayar ?? 0,
                                   Himbauan = upaya.Where(x => nopList.Any(nl => nl.Nop == x.Nop && nl.PajakId == n.PajakID)).Sum(x => x.Himbauan),
                                   Teguran = upaya.Where(x => nopList.Any(nl => nl.Nop == x.Nop && nl.PajakId == n.PajakID)).Sum(x => x.Teguran),
                                   Silang = upaya.Where(x => nopList.Any(nl => nl.Nop == x.Nop && nl.PajakId == n.PajakID)).Sum(x => x.Silang),
                                   Penutupan = upaya.Where(x => nopList.Any(nl => nl.Nop == x.Nop && nl.PajakId == n.PajakID)).Sum(x => x.Penutupan)
                               })
                               .OrderBy(x => x.PajakID)
                               .ToList();

                return retData;
            }
            public static List<ViewModels.PADKategori> GetDataKategori(int tahun, int bulan, EnumFactory.EPajak pajakId)
            {
                var MonPDContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.PADKategori>();

                var nopList = context.MObjekPajaks
                    .Select(x => new { x.Nop, x.PajakId, x.KategoriPajak })
                    .ToList();

                var kategoriList = MonPDContext.MKategoriPajaks
                    .Where(x => x.PajakId == (int)pajakId)
                    .OrderBy(x => x.Urutan)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                switch (pajakId)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dbRestos = MonPDContext.DbMonRestos
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun && x.PajakNama != "MAMIN" &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opRestos = MonPDContext.DbOpRestos
                            .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.PajakNama != "MAMIN" && nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.MakananMinuman
                            })
                            .ToList();

                        var resto = (from r in dbRestos
                                     join o in opRestos on r.Nop equals o.Nop
                                     join n in context.MObjekPajaks on r.Nop equals n.Nop
                                     group new { r, o, n } by new
                                     {
                                         o.PajakId,
                                         KategoriId = n.KategoriPajak,
                                         Bulan = r.TglBayarPokok.Value.Month
                                     } into g
                                     select new
                                     {
                                         PajakId = g.Key.PajakId,
                                         KategoriId = g.Key.KategoriId,
                                         Bulan = g.Key.Bulan,
                                         TotalNominal = g.Sum(x => x.r.NominalPokokBayar),
                                         JumlahOp = g.Where(x => x.r.TglBayarPokok.Value.Month == bulan && x.r.NominalPokokBayar > 0)
                                             .Select(x => x.r.Nop)
                                             .Distinct()
                                             .Count()

                                     })
                                    .ToList();


                        var realisasiGrouped = resto
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, x.KategoriId })
                            .Select(g => new
                            {
                                PajakId = g.Key.PajakId,
                                KategoriId = g.Key.KategoriId,
                                OpBayar = g.Sum(x => x.JumlahOp),
                                SudahBayar = g.Where(x => x.Bulan == bulan).Sum(x => x.TotalNominal)
                            })
                            .ToList();

                        var upayaResto = context.TAktifitasPegawais
                            .Where(x => x.TanggalAktifitas.HasValue &&
                                        x.TanggalAktifitas.Value.Year == tahun &&
                                        x.TanggalAktifitas.Value.Month == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                Himbauan = g.Count(x => x.Aktifitas == "HIMBAUAN"),
                                Teguran = g.Count(x => x.Aktifitas == "TEGURAN"),
                                Silang = g.Count(x => x.Aktifitas == "SILANG"),
                                Penutupan = g.Count(x => x.Aktifitas == "PENUTUPAN")
                            })
                            .ToList();

                        var groupedNop = nopList
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, Kategori = x.KategoriPajak })
                            .Select(g => new
                            {
                                PajakID = g.Key.PajakId,
                                KategoriId = (int)g.Key.Kategori,
                                JenisPajak = ((EnumFactory.EPajak)g.Key.PajakId).GetDescription(),
                                OPbuka = g.Count(),
                                NopList = g.Select(x => x.Nop).ToList()
                            })
                            .ToList();

                        var retData = groupedNop.Select(n =>
                        {
                            var r = realisasiGrouped
                                .FirstOrDefault(x => x.PajakId == n.PajakID && x.KategoriId == n.KategoriId);

                            var upayaData = upayaResto
                                .Where(u => n.NopList.Contains(u.Nop))
                                .ToList();

                            return new ViewModels.PADKategori
                            {
                                PajakID = (int)EnumFactory.EPajak.MakananMinuman,
                                JenisPajak = n.JenisPajak,
                                KategoriID = (int)MonPDContext.MKategoriPajaks
                                    .Where(k => k.Id == n.KategoriId)
                                    .Select(k => k.Id)
                                    .FirstOrDefault(),
                                Kategori = MonPDContext.MKategoriPajaks
                                    .Where(k => k.Id == n.KategoriId)
                                    .Select(k => k.Nama)
                                    .FirstOrDefault() ?? "Tanpa Kategori",
                                Tahun = tahun,
                                Bulan = bulan,
                                OPbuka = n.OPbuka,
                                OpBayar = r?.OpBayar ?? 0,
                                SudahBayar = r?.SudahBayar ?? 0,
                                Himbauan = upayaData.Sum(x => x.Himbauan),
                                Teguran = upayaData.Sum(x => x.Teguran),
                                Silang = upayaData.Sum(x => x.Silang),
                                Penutupan = upayaData.Sum(x => x.Penutupan)
                            };
                        })
                        .OrderBy(x => x.KategoriID)
                        .ToList();

                        var hasil = retData
                            .Where(x => kategoriList.Any(k => k.Id == x.KategoriID))
                            .OrderBy(x => kategoriList.FindIndex(k => k.Id == x.KategoriID))
                            .ToList();

                        ret.AddRange(hasil);

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dbListrik = MonPDContext.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opListrik = MonPDContext.DbOpListriks
                            .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.TenagaListrik
                            })
                            .ToList();

                        var listrik = (from r in dbListrik
                                       join o in opListrik on r.Nop equals o.Nop
                                       join n in context.MObjekPajaks on r.Nop equals n.Nop
                                       group new { r, o, n } by new
                                       {
                                           o.PajakId,
                                           KategoriId = n.KategoriPajak,
                                           Bulan = r.TglBayarPokok.Value.Month
                                       } into g
                                       select new
                                       {
                                           PajakId = g.Key.PajakId,
                                           KategoriId = g.Key.KategoriId,
                                           Bulan = g.Key.Bulan,
                                           TotalNominal = g.Sum(x => x.r.NominalPokokBayar),
                                           JumlahOp = g.Where(x => x.r.TglBayarPokok.Value.Month == bulan && x.r.NominalPokokBayar > 0)
                                               .Select(x => x.r.Nop)
                                               .Distinct()
                                               .Count()

                                       })
                                    .ToList();


                        var realisasiListrik = listrik
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, x.KategoriId })
                            .Select(g => new
                            {
                                PajakId = g.Key.PajakId,
                                KategoriId = g.Key.KategoriId,
                                OpBayar = g.Sum(x => x.JumlahOp),
                                SudahBayar = g.Where(x => x.Bulan == bulan).Sum(x => x.TotalNominal)
                            })
                            .ToList();

                        var upayaListrik = context.TAktifitasPegawais
                            .Where(x => x.TanggalAktifitas.HasValue &&
                                        x.TanggalAktifitas.Value.Year == tahun &&
                                        x.TanggalAktifitas.Value.Month == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                Himbauan = g.Count(x => x.Aktifitas == "HIMBAUAN"),
                                Teguran = g.Count(x => x.Aktifitas == "TEGURAN"),
                                Silang = g.Count(x => x.Aktifitas == "SILANG"),
                                Penutupan = g.Count(x => x.Aktifitas == "PENUTUPAN")
                            })
                            .ToList();

                        var groupedNopListrik = nopList
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, Kategori = x.KategoriPajak })
                            .Select(g => new
                            {
                                PajakID = g.Key.PajakId,
                                KategoriId = (int)g.Key.Kategori,
                                JenisPajak = ((EnumFactory.EPajak)g.Key.PajakId).GetDescription(),
                                OPbuka = g.Count(),
                                NopList = g.Select(x => x.Nop).ToList()
                            })
                            .ToList();

                        var retListrik = groupedNopListrik.Select(n =>
                        {
                            var r = realisasiListrik
                                .FirstOrDefault(x => x.PajakId == n.PajakID && x.KategoriId == n.KategoriId);

                            var upayaData = upayaListrik
                                .Where(u => n.NopList.Contains(u.Nop))
                                .ToList();

                            return new ViewModels.PADKategori
                            {
                                PajakID = (int)n.PajakID,
                                JenisPajak = n.JenisPajak,
                                KategoriID = n.KategoriId,
                                Kategori = MonPDContext.MKategoriPajaks
                                    .Where(k => k.Id == n.KategoriId)
                                    .Select(k => k.Nama)
                                    .FirstOrDefault() ?? "Tanpa Kategori",
                                Tahun = tahun,
                                Bulan = bulan,
                                OPbuka = n.OPbuka,
                                OpBayar = r?.OpBayar ?? 0,
                                SudahBayar = r?.SudahBayar ?? 0,
                                Himbauan = upayaData.Sum(x => x.Himbauan),
                                Teguran = upayaData.Sum(x => x.Teguran),
                                Silang = upayaData.Sum(x => x.Silang),
                                Penutupan = upayaData.Sum(x => x.Penutupan)
                            };
                        })
                        .OrderBy(x => x.KategoriID)
                        .ToList();

                        var hasilListrik = retListrik
                            .Where(x => kategoriList.Any(k => k.Id == x.KategoriID))
                            .OrderBy(x => kategoriList.FindIndex(k => k.Id == x.KategoriID))
                            .ToList();

                        ret.AddRange(hasilListrik);

                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dbHotel = MonPDContext.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opHotel = MonPDContext.DbOpHotels
                            .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.JasaPerhotelan
                            })
                            .ToList();

                        var hotel = (from r in dbHotel
                                     join o in opHotel on r.Nop equals o.Nop
                                     join n in context.MObjekPajaks on r.Nop equals n.Nop
                                     group new { r, o, n } by new
                                     {
                                         o.PajakId,
                                         KategoriId = n.KategoriPajak,
                                         Bulan = r.TglBayarPokok.Value.Month
                                     } into g
                                     select new
                                     {
                                         PajakId = g.Key.PajakId,
                                         KategoriId = g.Key.KategoriId,
                                         Bulan = g.Key.Bulan,
                                         TotalNominal = g.Sum(x => x.r.NominalPokokBayar),
                                         JumlahOp = g.Where(x => x.r.TglBayarPokok.Value.Month == bulan && x.r.NominalPokokBayar > 0)
                                             .Select(x => x.r.Nop)
                                             .Distinct()
                                             .Count()

                                     })
                                    .ToList();


                        var realisasiHotel = hotel
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, x.KategoriId })
                            .Select(g => new
                            {
                                PajakId = g.Key.PajakId,
                                KategoriId = g.Key.KategoriId,
                                OpBayar = g.Sum(x => x.JumlahOp),
                                SudahBayar = g.Where(x => x.Bulan == bulan).Sum(x => x.TotalNominal)
                            })
                            .ToList();

                        var upayaHotel = context.TAktifitasPegawais
                            .Where(x => x.TanggalAktifitas.HasValue &&
                                        x.TanggalAktifitas.Value.Year == tahun &&
                                        x.TanggalAktifitas.Value.Month == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                Himbauan = g.Count(x => x.Aktifitas == "HIMBAUAN"),
                                Teguran = g.Count(x => x.Aktifitas == "TEGURAN"),
                                Silang = g.Count(x => x.Aktifitas == "SILANG"),
                                Penutupan = g.Count(x => x.Aktifitas == "PENUTUPAN")
                            })
                            .ToList();

                        var groupedNopHotel = nopList
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, Kategori = x.KategoriPajak })
                            .Select(g => new
                            {
                                PajakID = g.Key.PajakId,
                                KategoriId = (int)g.Key.Kategori,
                                JenisPajak = ((EnumFactory.EPajak)g.Key.PajakId).GetDescription(),
                                OPbuka = g.Count(),
                                NopList = g.Select(x => x.Nop).ToList()
                            })
                            .ToList();

                        var retHotel = groupedNopHotel.Select(n =>
                        {
                            var r = realisasiHotel
                                .FirstOrDefault(x => x.PajakId == n.PajakID && x.KategoriId == n.KategoriId);

                            var upayaData = upayaHotel
                                .Where(u => n.NopList.Contains(u.Nop))
                                .ToList();

                            return new ViewModels.PADKategori
                            {
                                PajakID = (int)n.PajakID,
                                JenisPajak = n.JenisPajak,
                                KategoriID = n.KategoriId,
                                Kategori = MonPDContext.MKategoriPajaks
                                    .Where(k => k.Id == n.KategoriId)
                                    .Select(k => k.Nama)
                                    .FirstOrDefault() ?? "Tanpa Kategori",
                                Tahun = tahun,
                                Bulan = bulan,
                                OPbuka = n.OPbuka,
                                OpBayar = r?.OpBayar ?? 0,
                                SudahBayar = r?.SudahBayar ?? 0,
                                Himbauan = upayaData.Sum(x => x.Himbauan),
                                Teguran = upayaData.Sum(x => x.Teguran),
                                Silang = upayaData.Sum(x => x.Silang),
                                Penutupan = upayaData.Sum(x => x.Penutupan)
                            };
                        })
                        .OrderBy(x => x.KategoriID)
                        .ToList();

                        var hasilHotel = retHotel
                            .Where(x => kategoriList.Any(k => k.Id == x.KategoriID))
                            .OrderBy(x => kategoriList.FindIndex(k => k.Id == x.KategoriID))
                            .ToList();

                        ret.AddRange(hasilHotel);

                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dbParkir = MonPDContext.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opParkir = MonPDContext.DbOpParkirs
                            .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.JasaParkir
                            })
                            .ToList();

                        var parkir = (from r in dbParkir
                                      join o in opParkir on r.Nop equals o.Nop
                                      join n in context.MObjekPajaks on r.Nop equals n.Nop
                                      group new { r, o, n } by new
                                      {
                                          o.PajakId,
                                          KategoriId = n.KategoriPajak,
                                          Bulan = r.TglBayarPokok.Value.Month
                                      } into g
                                      select new
                                      {
                                          PajakId = g.Key.PajakId,
                                          KategoriId = g.Key.KategoriId,
                                          Bulan = g.Key.Bulan,
                                          TotalNominal = g.Sum(x => x.r.NominalPokokBayar),
                                          JumlahOp = g.Where(x => x.r.TglBayarPokok.Value.Month == bulan && x.r.NominalPokokBayar > 0)
                                              .Select(x => x.r.Nop)
                                              .Distinct()
                                              .Count()

                                      })
                                    .ToList();


                        var realisasiParkir = parkir
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, x.KategoriId })
                            .Select(g => new
                            {
                                PajakId = g.Key.PajakId,
                                KategoriId = g.Key.KategoriId,
                                OpBayar = g.Sum(x => x.JumlahOp),
                                SudahBayar = g.Where(x => x.Bulan == bulan).Sum(x => x.TotalNominal)
                            })
                            .ToList();

                        var upayaParkir = context.TAktifitasPegawais
                            .Where(x => x.TanggalAktifitas.HasValue &&
                                        x.TanggalAktifitas.Value.Year == tahun &&
                                        x.TanggalAktifitas.Value.Month == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                Himbauan = g.Count(x => x.Aktifitas == "HIMBAUAN"),
                                Teguran = g.Count(x => x.Aktifitas == "TEGURAN"),
                                Silang = g.Count(x => x.Aktifitas == "SILANG"),
                                Penutupan = g.Count(x => x.Aktifitas == "PENUTUPAN")
                            })
                            .ToList();

                        var groupedNopParkir = nopList
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, Kategori = x.KategoriPajak })
                            .Select(g => new
                            {
                                PajakID = g.Key.PajakId,
                                KategoriId = (int)g.Key.Kategori,
                                JenisPajak = ((EnumFactory.EPajak)g.Key.PajakId).GetDescription(),
                                OPbuka = g.Count(),
                                NopList = g.Select(x => x.Nop).ToList()
                            })
                            .ToList();

                        var retParkir = groupedNopParkir.Select(n =>
                        {
                            var r = realisasiParkir
                                .FirstOrDefault(x => x.PajakId == n.PajakID && x.KategoriId == n.KategoriId);

                            var upayaData = upayaParkir
                                .Where(u => n.NopList.Contains(u.Nop))
                                .ToList();

                            return new ViewModels.PADKategori
                            {
                                PajakID = (int)n.PajakID,
                                JenisPajak = n.JenisPajak,
                                KategoriID = n.KategoriId,
                                Kategori = MonPDContext.MKategoriPajaks
                                    .Where(k => k.Id == n.KategoriId)
                                    .Select(k => k.Nama)
                                    .FirstOrDefault() ?? "Tanpa Kategori",
                                Tahun = tahun,
                                Bulan = bulan,
                                OPbuka = n.OPbuka,
                                OpBayar = r?.OpBayar ?? 0,
                                SudahBayar = r?.SudahBayar ?? 0,
                                Himbauan = upayaData.Sum(x => x.Himbauan),
                                Teguran = upayaData.Sum(x => x.Teguran),
                                Silang = upayaData.Sum(x => x.Silang),
                                Penutupan = upayaData.Sum(x => x.Penutupan)
                            };
                        })
                        .OrderBy(x => x.KategoriID)
                        .ToList();

                        var hasilParkir = retParkir
                            .Where(x => kategoriList.Any(k => k.Id == x.KategoriID))
                            .OrderBy(x => kategoriList.FindIndex(k => k.Id == x.KategoriID))
                            .ToList();

                        ret.AddRange(hasilParkir);

                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dbHiburan = MonPDContext.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opHiburan = MonPDContext.DbOpHiburans
                            .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.JasaKesenianHiburan
                            })
                            .ToList();

                        var hiburan = (from r in dbHiburan
                                       join o in opHiburan on r.Nop equals o.Nop
                                       join n in context.MObjekPajaks on r.Nop equals n.Nop
                                       group new { r, o, n } by new
                                       {
                                           o.PajakId,
                                           KategoriId = n.KategoriPajak,
                                           Bulan = r.TglBayarPokok.Value.Month
                                       } into g
                                       select new
                                       {
                                           PajakId = g.Key.PajakId,
                                           KategoriId = g.Key.KategoriId,
                                           Bulan = g.Key.Bulan,
                                           TotalNominal = g.Sum(x => x.r.NominalPokokBayar),
                                           JumlahOp = g.Where(x => x.r.TglBayarPokok.Value.Month == bulan && x.r.NominalPokokBayar > 0)
                                               .Select(x => x.r.Nop)
                                               .Distinct()
                                               .Count()

                                       })
                                    .ToList();


                        var realisasiHiburan = hiburan
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, x.KategoriId })
                            .Select(g => new
                            {
                                PajakId = g.Key.PajakId,
                                KategoriId = g.Key.KategoriId,
                                OpBayar = g.Sum(x => x.JumlahOp),
                                SudahBayar = g.Where(x => x.Bulan == bulan).Sum(x => x.TotalNominal)
                            })
                            .ToList();

                        var upayaHiburan = context.TAktifitasPegawais
                            .Where(x => x.TanggalAktifitas.HasValue &&
                                        x.TanggalAktifitas.Value.Year == tahun &&
                                        x.TanggalAktifitas.Value.Month == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                Himbauan = g.Count(x => x.Aktifitas == "HIMBAUAN"),
                                Teguran = g.Count(x => x.Aktifitas == "TEGURAN"),
                                Silang = g.Count(x => x.Aktifitas == "SILANG"),
                                Penutupan = g.Count(x => x.Aktifitas == "PENUTUPAN")
                            })
                            .ToList();

                        var groupedNopHiburan = nopList
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, Kategori = x.KategoriPajak })
                            .Select(g => new
                            {
                                PajakID = g.Key.PajakId,
                                KategoriId = (int)g.Key.Kategori,
                                JenisPajak = ((EnumFactory.EPajak)g.Key.PajakId).GetDescription(),
                                OPbuka = g.Count(),
                                NopList = g.Select(x => x.Nop).ToList()
                            })
                            .ToList();

                        var retHiburan = groupedNopHiburan.Select(n =>
                        {
                            var r = realisasiHiburan
                                .FirstOrDefault(x => x.PajakId == n.PajakID && x.KategoriId == n.KategoriId);

                            var upayaData = upayaHiburan
                                .Where(u => n.NopList.Contains(u.Nop))
                                .ToList();

                            return new ViewModels.PADKategori
                            {
                                PajakID = (int)n.PajakID,
                                JenisPajak = n.JenisPajak,
                                KategoriID = n.KategoriId,
                                Kategori = MonPDContext.MKategoriPajaks
                                    .Where(k => k.Id == n.KategoriId)
                                    .Select(k => k.Nama)
                                    .FirstOrDefault() ?? "Tanpa Kategori",
                                Tahun = tahun,
                                Bulan = bulan,
                                OPbuka = n.OPbuka,
                                OpBayar = r?.OpBayar ?? 0,
                                SudahBayar = r?.SudahBayar ?? 0,
                                Himbauan = upayaData.Sum(x => x.Himbauan),
                                Teguran = upayaData.Sum(x => x.Teguran),
                                Silang = upayaData.Sum(x => x.Silang),
                                Penutupan = upayaData.Sum(x => x.Penutupan)
                            };
                        })
                        .OrderBy(x => x.KategoriID)
                        .ToList();

                        var hasilHiburan = retHiburan
                            .Where(x => kategoriList.Any(k => k.Id == x.KategoriID))
                            .OrderBy(x => kategoriList.FindIndex(k => k.Id == x.KategoriID))
                            .ToList();

                        ret.AddRange(hasilHiburan);

                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dbABT = MonPDContext.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opABT = MonPDContext.DbOpAbts
                            .Where(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.AirTanah
                            })
                            .ToList();

                        var abt = (from r in dbABT
                                   join o in opABT on r.Nop equals o.Nop
                                   join n in context.MObjekPajaks on r.Nop equals n.Nop
                                   group new { r, o, n } by new
                                   {
                                       o.PajakId,
                                       KategoriId = n.KategoriPajak,
                                       Bulan = r.TglBayarPokok.Value.Month
                                   } into g
                                   select new
                                   {
                                       PajakId = g.Key.PajakId,
                                       KategoriId = g.Key.KategoriId,
                                       Bulan = g.Key.Bulan,
                                       TotalNominal = g.Sum(x => x.r.NominalPokokBayar),
                                       JumlahOp = g.Where(x => x.r.TglBayarPokok.Value.Month == bulan && x.r.NominalPokokBayar > 0)
                                           .Select(x => x.r.Nop)
                                           .Distinct()
                                           .Count()

                                   })
                                    .ToList();


                        var realisasiABT = abt
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, x.KategoriId })
                            .Select(g => new
                            {
                                PajakId = g.Key.PajakId,
                                KategoriId = g.Key.KategoriId,
                                OpBayar = g.Sum(x => x.JumlahOp),
                                SudahBayar = g.Where(x => x.Bulan == bulan).Sum(x => x.TotalNominal)
                            })
                            .ToList();

                        var upayaABT = context.TAktifitasPegawais
                            .Where(x => x.TanggalAktifitas.HasValue &&
                                        x.TanggalAktifitas.Value.Year == tahun &&
                                        x.TanggalAktifitas.Value.Month == bulan)
                            .GroupBy(x => x.Nop)
                            .Select(g => new
                            {
                                Nop = g.Key,
                                Himbauan = g.Count(x => x.Aktifitas == "HIMBAUAN"),
                                Teguran = g.Count(x => x.Aktifitas == "TEGURAN"),
                                Silang = g.Count(x => x.Aktifitas == "SILANG"),
                                Penutupan = g.Count(x => x.Aktifitas == "PENUTUPAN")
                            })
                            .ToList();

                        var groupedNopABT = nopList
                            .Where(x => x.PajakId == (int)pajakId)
                            .GroupBy(x => new { x.PajakId, Kategori = x.KategoriPajak })
                            .Select(g => new
                            {
                                PajakID = g.Key.PajakId,
                                KategoriId = (int)g.Key.Kategori,
                                JenisPajak = ((EnumFactory.EPajak)g.Key.PajakId).GetDescription(),
                                OPbuka = g.Count(),
                                NopList = g.Select(x => x.Nop).ToList()
                            })
                            .ToList();

                        var retABT = groupedNopABT.Select(n =>
                        {
                            var r = realisasiABT
                                .FirstOrDefault(x => x.PajakId == n.PajakID && x.KategoriId == n.KategoriId);

                            var upayaData = upayaABT
                                .Where(u => n.NopList.Contains(u.Nop))
                                .ToList();

                            return new ViewModels.PADKategori
                            {
                                PajakID = (int)n.PajakID,
                                JenisPajak = n.JenisPajak,
                                KategoriID = n.KategoriId,
                                Kategori = MonPDContext.MKategoriPajaks
                                    .Where(k => k.Id == n.KategoriId)
                                    .Select(k => k.Nama)
                                    .FirstOrDefault() ?? "Tanpa Kategori",
                                Tahun = tahun,
                                Bulan = bulan,
                                OPbuka = n.OPbuka,
                                OpBayar = r?.OpBayar ?? 0,
                                SudahBayar = r?.SudahBayar ?? 0,
                                Himbauan = upayaData.Sum(x => x.Himbauan),
                                Teguran = upayaData.Sum(x => x.Teguran),
                                Silang = upayaData.Sum(x => x.Silang),
                                Penutupan = upayaData.Sum(x => x.Penutupan)
                            };
                        })
                        .OrderBy(x => x.KategoriID)
                        .ToList();

                        var hasilABT = retABT
                            .Where(x => kategoriList.Any(k => k.Id == x.KategoriID))
                            .OrderBy(x => kategoriList.FindIndex(k => k.Id == x.KategoriID))
                            .ToList();

                        ret.AddRange(hasilABT);

                        break;
                }

                return ret;
            }
            public static List<ViewModels.DetailOP> GetDetailOPBuka(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId)
            {
                var MonPDContext = DBClass.GetContext();
                var context = _context;
                var ret = new List<ViewModels.DetailOP>();

                var nopList = context.MObjekPajaks
                    .Where(x => x.KategoriPajak == kategoriId)
                    .Select(x => new { x.Nop, x.PajakId, x.KategoriPajak })
                    .ToList();

                var kategoriList = MonPDContext.MKategoriPajaks
                    .Where(x => x.PajakId == (int)pajakId)
                    .OrderBy(x => x.Urutan)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                switch (pajakId)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dbRestos = MonPDContext.DbMonRestos
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun && x.PajakNama != "MAMIN" &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opRestos = MonPDContext.DbOpRestos
                            .Where(x => x.TahunBuku == tahun &&
                                        (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) &&
                                        x.PajakNama != "MAMIN" &&
                                        x.KategoriId == kategoriId &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.MakananMinuman
                            })
                            .ToList();

                        // 1) Pastikan restos sudah benar dan ter-materialisasi
                        var restosList = (from r in dbRestos
                                          join o in opRestos on r.Nop equals o.Nop
                                          join n in context.MObjekPajaks on r.Nop equals n.Nop
                                          where o.KategoriId == kategoriId
                                            && r.TglBayarPokok.HasValue
                                            && r.TglBayarPokok.Value.Month == bulan
                                            && r.NominalPokokBayar > 0
                                          select new
                                          {
                                              r.Nop,
                                              n.NamaOp,
                                              n.AlamatOp,
                                              NominalPokokBayar = r.NominalPokokBayar,
                                              r.TglBayarPokok
                                          })
                                         .GroupBy(x => new { x.Nop, x.NamaOp, x.AlamatOp })
                                         .Select(g => new
                                         {
                                             Nop = g.Key.Nop,
                                             NamaOp = g.Key.NamaOp,
                                             AlamatOp = g.Key.AlamatOp,
                                             NominalPokokBayar = g.Sum(x => x.NominalPokokBayar),
                                             TglBayarPokok = g.Max(x => x.TglBayarPokok)
                                         })
                                         .OrderBy(x => x.Nop)
                                         .ToList();

                        var countRestos = restosList.Count;

                        var detailList = restosList
                            .Select(x => new ViewModels.DetailOP
                            {
                                Nop = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                SudahBayar = x.NominalPokokBayar,
                                TglBayar = x.TglBayarPokok ?? DateTime.MinValue
                            })
                            .ToList();

                        ret = detailList;

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dbListrik = MonPDContext.DbMonPpjs
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opListrik = MonPDContext.DbOpListriks
                            .Where(x => x.TahunBuku == tahun &&
                                        (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) &&
                                        x.KategoriId == kategoriId &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.TenagaListrik
                            })
                            .ToList();

                        var listrikList = (from r in dbListrik
                                           join o in opListrik on r.Nop equals o.Nop
                                           join n in context.MObjekPajaks on r.Nop equals n.Nop
                                           where o.KategoriId == kategoriId
                                             && r.TglBayarPokok.HasValue
                                             && r.TglBayarPokok.Value.Month == bulan
                                             && r.NominalPokokBayar > 0
                                           select new
                                           {
                                               r.Nop,
                                               n.NamaOp,
                                               n.AlamatOp,
                                               NominalPokokBayar = r.NominalPokokBayar,
                                               r.TglBayarPokok
                                           })
                                         .GroupBy(x => new { x.Nop, x.NamaOp, x.AlamatOp })
                                         .Select(g => new
                                         {
                                             Nop = g.Key.Nop,
                                             NamaOp = g.Key.NamaOp,
                                             AlamatOp = g.Key.AlamatOp,
                                             NominalPokokBayar = g.Sum(x => x.NominalPokokBayar),
                                             TglBayarPokok = g.Max(x => x.TglBayarPokok)
                                         })
                                         .OrderBy(x => x.Nop)
                                         .ToList();

                        var countListrik = listrikList.Count;

                        var hasilListrik = listrikList
                            .Select(x => new ViewModels.DetailOP
                            {
                                Nop = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                SudahBayar = x.NominalPokokBayar,
                                TglBayar = x.TglBayarPokok ?? DateTime.MinValue
                            })
                            .ToList();

                        ret = hasilListrik;
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dbHotel = MonPDContext.DbMonHotels
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opHotel = MonPDContext.DbOpHotels
                            .Where(x => x.TahunBuku == tahun &&
                                        (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) &&
                                        x.KategoriId == kategoriId &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.JasaPerhotelan
                            })
                            .ToList();

                        var hotelList = (from r in dbHotel
                                         join o in opHotel on r.Nop equals o.Nop
                                         join n in context.MObjekPajaks on r.Nop equals n.Nop
                                         where o.KategoriId == kategoriId
                                           && r.TglBayarPokok.HasValue
                                           && r.TglBayarPokok.Value.Month == bulan
                                           && r.NominalPokokBayar > 0
                                         select new
                                         {
                                             r.Nop,
                                             n.NamaOp,
                                             n.AlamatOp,
                                             NominalPokokBayar = r.NominalPokokBayar,
                                             r.TglBayarPokok
                                         })
                                         .GroupBy(x => new { x.Nop, x.NamaOp, x.AlamatOp })
                                         .Select(g => new
                                         {
                                             Nop = g.Key.Nop,
                                             NamaOp = g.Key.NamaOp,
                                             AlamatOp = g.Key.AlamatOp,
                                             NominalPokokBayar = g.Sum(x => x.NominalPokokBayar),
                                             TglBayarPokok = g.Max(x => x.TglBayarPokok)
                                         })
                                         .OrderBy(x => x.Nop)
                                         .ToList();

                        var countHotel = hotelList.Count;

                        var hasilHotel = hotelList
                            .Select(x => new ViewModels.DetailOP
                            {
                                Nop = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                SudahBayar = x.NominalPokokBayar,
                                TglBayar = x.TglBayarPokok ?? DateTime.MinValue
                            })
                            .ToList();

                        ret = hasilHotel;
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dbParkir = MonPDContext.DbMonParkirs
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opParkir = MonPDContext.DbOpParkirs
                            .Where(x => x.TahunBuku == tahun &&
                                        (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) &&
                                        x.KategoriId == kategoriId &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.JasaParkir
                            })
                            .ToList();

                        var parkirList = (from r in dbParkir
                                          join o in opParkir on r.Nop equals o.Nop
                                          join n in context.MObjekPajaks on r.Nop equals n.Nop
                                          where o.KategoriId == kategoriId
                                            && r.TglBayarPokok.HasValue
                                            && r.TglBayarPokok.Value.Month == bulan
                                            && r.NominalPokokBayar > 0
                                          select new
                                          {
                                              r.Nop,
                                              n.NamaOp,
                                              n.AlamatOp,
                                              NominalPokokBayar = r.NominalPokokBayar,
                                              r.TglBayarPokok
                                          })
                                         .GroupBy(x => new { x.Nop, x.NamaOp, x.AlamatOp })
                                         .Select(g => new
                                         {
                                             Nop = g.Key.Nop,
                                             NamaOp = g.Key.NamaOp,
                                             AlamatOp = g.Key.AlamatOp,
                                             NominalPokokBayar = g.Sum(x => x.NominalPokokBayar),
                                             TglBayarPokok = g.Max(x => x.TglBayarPokok)
                                         })
                                         .OrderBy(x => x.Nop)
                                         .ToList();

                        var countParkir = parkirList.Count;

                        var hasilParkir = parkirList
                            .Select(x => new ViewModels.DetailOP
                            {
                                Nop = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                SudahBayar = x.NominalPokokBayar,
                                TglBayar = x.TglBayarPokok ?? DateTime.MinValue
                            })
                            .ToList();

                        ret = hasilParkir;
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dbHiburan = MonPDContext.DbMonHiburans
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opHiburan = MonPDContext.DbOpHiburans
                            .Where(x => x.TahunBuku == tahun &&
                                        (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) &&
                                        x.KategoriId == kategoriId &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.JasaKesenianHiburan
                            })
                            .ToList();

                        var hiburanList = (from r in dbHiburan
                                           join o in opHiburan on r.Nop equals o.Nop
                                           join n in context.MObjekPajaks on r.Nop equals n.Nop
                                           where o.KategoriId == kategoriId
                                             && r.TglBayarPokok.HasValue
                                             && r.TglBayarPokok.Value.Month == bulan
                                             && r.NominalPokokBayar > 0
                                           select new
                                           {
                                               r.Nop,
                                               n.NamaOp,
                                               n.AlamatOp,
                                               NominalPokokBayar = r.NominalPokokBayar,
                                               r.TglBayarPokok
                                           })
                                         .GroupBy(x => new { x.Nop, x.NamaOp, x.AlamatOp })
                                         .Select(g => new
                                         {
                                             Nop = g.Key.Nop,
                                             NamaOp = g.Key.NamaOp,
                                             AlamatOp = g.Key.AlamatOp,
                                             NominalPokokBayar = g.Sum(x => x.NominalPokokBayar),
                                             TglBayarPokok = g.Max(x => x.TglBayarPokok)
                                         })
                                         .OrderBy(x => x.Nop)
                                         .ToList();

                        var countHiburan = hiburanList.Count;

                        var hasilHiburan = hiburanList
                            .Select(x => new ViewModels.DetailOP
                            {
                                Nop = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                SudahBayar = x.NominalPokokBayar,
                                TglBayar = x.TglBayarPokok ?? DateTime.MinValue
                            })
                            .ToList();

                        ret = hasilHiburan;
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dbABT = MonPDContext.DbMonAbts
                            .Where(x => x.TglBayarPokok.HasValue &&
                                        x.TglBayarPokok.Value.Year == tahun &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                NominalPokokBayar = x.NominalPokokBayar ?? 0,
                                x.TglBayarPokok
                            })
                            .AsEnumerable()
                            .ToList();

                        var opABT = MonPDContext.DbOpAbts
                            .Where(x => x.TahunBuku == tahun &&
                                        (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) &&
                                        x.KategoriId == kategoriId &&
                                        nopList.Select(n => n.Nop).Contains(x.Nop))
                            .Select(x => new
                            {
                                x.Nop,
                                x.KategoriId,
                                PajakId = (int)EnumFactory.EPajak.AirTanah
                            })
                            .ToList();

                        var abtList = (from r in dbABT
                                       join o in opABT on r.Nop equals o.Nop
                                       join n in context.MObjekPajaks on r.Nop equals n.Nop
                                       where o.KategoriId == kategoriId
                                         && r.TglBayarPokok.HasValue
                                         && r.TglBayarPokok.Value.Month == bulan
                                         && r.NominalPokokBayar > 0
                                       select new
                                       {
                                           r.Nop,
                                           n.NamaOp,
                                           n.AlamatOp,
                                           NominalPokokBayar = r.NominalPokokBayar,
                                           r.TglBayarPokok
                                       })
                                         .GroupBy(x => new { x.Nop, x.NamaOp, x.AlamatOp })
                                         .Select(g => new
                                         {
                                             Nop = g.Key.Nop,
                                             NamaOp = g.Key.NamaOp,
                                             AlamatOp = g.Key.AlamatOp,
                                             NominalPokokBayar = g.Sum(x => x.NominalPokokBayar),
                                             TglBayarPokok = g.Max(x => x.TglBayarPokok)
                                         })
                                         .OrderBy(x => x.Nop)
                                         .ToList();

                        var countABT = abtList.Count;

                        var hasilABT = abtList
                            .Select(x => new ViewModels.DetailOP
                            {
                                Nop = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                SudahBayar = x.NominalPokokBayar,
                                TglBayar = x.TglBayarPokok ?? DateTime.MinValue
                            })
                            .ToList();

                        ret = hasilABT;
                        break;
                }

                return ret;
            }
            public static List<ViewModels.DetailUpaya> GetDetailUpaya(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId, int upaya)
            {
                var context = _context;
                var ret = new List<ViewModels.DetailUpaya>();

                ret = context.TAktifitasPegawais
                    .Where(x => x.TanggalAktifitas.HasValue &&
                                x.TanggalAktifitas.Value.Year == tahun &&
                                x.TanggalAktifitas.Value.Month == bulan &&
                                x.IdAktifitas == upaya &&
                                context.MObjekPajaks
                                    .Where(o => o.KategoriPajak == kategoriId && o.PajakId == (int)pajakId)
                                    .Select(o => o.Nop)
                                    .Contains(x.Nop))
                    .Include(x => x.NopNavigation)
                    .Select(x => new ViewModels.DetailUpaya
                    {
                        Nop = x.Nop,
                        NamaOP = x.NopNavigation.NamaOp,
                        Alamat = x.NopNavigation.AlamatOp,
                        TglUpaya = x.TanggalAktifitas ?? DateTime.MinValue,
                    })
                    .OrderBy(x => x.Nop)
                    .ToList();
                return ret;
            }
            public static List<ViewModels.TotalOPKategori> GetOPTotalKategori(int tahun, int bulan, EnumFactory.EPajak pajakId, int kategoriId)
            {
                var context = _context;
                var ret = new List<ViewModels.TotalOPKategori>();

                ret = context.MObjekPajaks
                    .Where(x => x.PajakId == (int)pajakId && x.KategoriPajak == kategoriId)
                    .Select(x => new ViewModels.TotalOPKategori
                    {
                        Nop = x.Nop,
                        NamaOP = x.NamaOp,
                        Alamat = x.AlamatOp,
                        Wilayah = "UPTB " + x.WilayahPajak
                    })
                    .ToList();

                return ret;
            }
        }
    }
}
