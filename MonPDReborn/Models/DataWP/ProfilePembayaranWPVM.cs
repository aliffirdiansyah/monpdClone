using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Collections.Generic;
using static MonPDReborn.Models.DataWP.ProfileWPVM;

namespace MonPDReborn.Models.DataWP
{
    public class ProfilePembayaranWPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Dashboard Data { get; set; } = new Dashboard();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }
        public class Show
        {
            public List<PembayaranWP> DataPembayaranWPList { get; set; } = new();
            public Show()
            {
                DataPembayaranWPList = Method.GetDataPembayaranWPList();
            }
        }
        public class Detail
        {
            public ProfilWP DataWPDetailRow { get; set; } = new();

            public Detail() { }

            public Detail(string NPWPD)
            {
                DataWPDetailRow = Method.GetDetailWP(NPWPD);
            }

        }
        public class Method
        {
            public static List<PembayaranWP> GetDataPembayaranWPList()
            {
                var ret = new List<PembayaranWP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var DataResto = context.DbMonRestos
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new PembayaranWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            NamaWP = first?.NpwpdNama ?? "-",
                            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                            Status = (first?.IsTutup ?? 0) == 1 ? "Tidak Aktif" : "Aktif"
                        };
                    })
                    .ToList();
                var DataListrik = context.DbMonPpjs
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new PembayaranWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            NamaWP = first?.NpwpdNama ?? "-",
                            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                            Status = (first?.IsTutup ?? 0) == 1 ? "Tidak Aktif" : "Aktif"
                        };
                    })
                    .ToList();
                var DataHotel = context.DbMonHotels
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new PembayaranWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            NamaWP = first?.NpwpdNama ?? "-",
                            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                            Status = (first?.IsTutup ?? 0) == 1 ? "Tidak Aktif" : "Aktif"
                        };
                    })
                    .ToList();
                var DataParkir = context.DbMonParkirs
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new PembayaranWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            NamaWP = first?.NpwpdNama ?? "-",
                            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                            Status = (first?.IsTutup ?? 0) == 1 ? "Tidak Aktif" : "Aktif"
                        };
                    })
                    .ToList();
                var DataHiburan = context.DbMonHiburans
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new PembayaranWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            NamaWP = first?.NpwpdNama ?? "-",
                            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                            Status = (first?.IsTutup ?? 0) == 1 ? "Tidak Aktif" : "Aktif"
                        };
                    })
                    .ToList();
                var DataAbt = context.DbMonAbts
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new PembayaranWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            NamaWP = first?.NpwpdNama ?? "-",
                            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                            Status = (first?.IsTutup ?? 0) == 1 ? "Tidak Aktif" : "Aktif"
                        };
                    })
                    .ToList();
                var DataReklame = context.DbMonReklames
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new PembayaranWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            NamaWP = first?.NamaPerusahaan ?? "-",
                            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                            Status = "Aktif"
                        };
                    })
                    .ToList();

                //var DataPbb = context.DbMonPbbs
                //    .GroupBy(r => new { r.Npwpd })
                //    .ToList()
                //    .Select(wp =>
                //    {
                //        var first = wp.FirstOrDefault();
                //        return new PembayaranWP
                //        {
                //            NPWPD = wp.Key.Npwpd,
                //            NamaWP = first?.NpwpdNama ?? "-",
                //            Ketetapan = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.PokokPajakKetetapan) ?? 0,
                //            Terbayar = wp.Where(x => x.TahunBuku == currentYear).Sum(x => x.NominalPokokBayar) ?? 0,
                //            Status = (first?.IsTutup ?? 0) == 1 ? "Tidak Aktif" : "Aktif"
                //        };
                //    })
                //    .ToList();

                ret.AddRange(DataResto);
                ret.AddRange(DataListrik);
                ret.AddRange(DataHotel);
                ret.AddRange(DataParkir);
                ret.AddRange(DataHiburan);
                ret.AddRange(DataAbt);
                ret.AddRange(DataReklame);
                //ret.AddRange(DataPbb);

                return ret;
            }
            public static ProfilWP GetDetailWP(string npwpd)
            {
                var ret = new ProfilWP();
                var context = DBClass.GetContext();

                var wp = context.Npwpds
                    .FirstOrDefault(w => w.NpwpdNo == npwpd);

                if (wp != null)
                {
                    ret.NPWPD = wp.NpwpdNo;
                    ret.NamaWP = wp.Nama;
                    ret.JenisSubjek = wp.JenisWp == 0 ? "Orang Pribadi" : wp.JenisWp == 1 ? "Badan Usaha" : "Tidak Diketahui";
                    ret.NIB = wp.JenisWp == 0 ? "-" : wp.JenisWp == 1 ? wp.NpwpdNo : "-";
                    ret.Status = wp.Status == 1 ? "Tidak Aktif" : "Aktif";
                    ret.TanggalDaftar = wp.InsDate;
                    ret.AlamatDom = wp.AlamatDomisili;
                    ret.AlamatUsaha = wp.Alamat;
                    ret.Kontak = wp.Kontak ?? wp.Hp;
                    ret.Email = wp.Email ?? "-";
                }

                var DataResto = context.DbMonRestos
                    .Where(x => x.Npwpd == npwpd)
                    .Select(wp => new DetailPembayaranWP
                    {
                        NPWPD = wp.Npwpd,
                        JenisPajak = ((EnumFactory.EPajak)wp.PajakId).GetDescription(),
                        TahunBuku = (int)wp.TahunBuku,
                        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                        Terbayar = wp.NominalPokokBayar ?? 0,
                        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                    })
                    .ToList();

                var DataListrik = context.DbMonPpjs
                    .Where(x => x.Npwpd == npwpd)
                    .Select(wp => new DetailPembayaranWP
                    {
                        NPWPD = wp.Npwpd,
                        JenisPajak = ((EnumFactory.EPajak)wp.PajakId).GetDescription(),
                        TahunBuku = (int)wp.TahunBuku,
                        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                        Terbayar = wp.NominalPokokBayar ?? 0,
                        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                    })
                    .ToList();

                var DataHotel = context.DbMonHotels
                    .Where(x => x.Npwpd == npwpd)
                    .Select(wp => new DetailPembayaranWP
                    {
                        NPWPD = wp.Npwpd,
                        JenisPajak = ((EnumFactory.EPajak)wp.PajakId).GetDescription(),
                        TahunBuku = (int)wp.TahunBuku,
                        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                        Terbayar = wp.NominalPokokBayar ?? 0,
                        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                    })
                    .ToList();

                var DataParkir = context.DbMonParkirs
                    .Where(x => x.Npwpd == npwpd)
                    .Select(wp => new DetailPembayaranWP
                    {
                        NPWPD = wp.Npwpd,
                        JenisPajak = ((EnumFactory.EPajak)wp.PajakId).GetDescription(),
                        TahunBuku = (int)wp.TahunBuku,
                        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                        Terbayar = wp.NominalPokokBayar ?? 0,
                        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                    })
                    .ToList();

                var DataHiburan = context.DbMonHiburans
                    .Where(x => x.Npwpd == npwpd)
                    .Select(wp => new DetailPembayaranWP
                    {
                        NPWPD = wp.Npwpd,
                        JenisPajak = ((EnumFactory.EPajak)wp.PajakId).GetDescription(),
                        TahunBuku = (int)wp.TahunBuku,
                        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                        Terbayar = wp.NominalPokokBayar ?? 0,
                        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                    })
                    .ToList();

                var DataAbt = context.DbMonAbts
                    .Where(x => x.Npwpd == npwpd)
                    .Select(wp => new DetailPembayaranWP
                    {
                        NPWPD = wp.Npwpd,
                        JenisPajak = ((EnumFactory.EPajak)wp.PajakId).GetDescription(),
                        TahunBuku = (int)wp.TahunBuku,
                        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                        Terbayar = wp.NominalPokokBayar ?? 0,
                        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                    })
                    .ToList();

                var DataReklame = context.DbMonReklames
                    .Where(x => x.Npwpd == npwpd)
                    .Select(wp => new DetailPembayaranWP
                    {
                        NPWPD = wp.Npwpd,
                        JenisPajak = ((EnumFactory.EPajak.Reklame)).GetDescription(),
                        TahunBuku = (int)wp.TahunBuku,
                        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                        Terbayar = wp.NominalPokokBayar ?? 0,
                        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                    })
                    .ToList();


                //var DataPbb = context.DbMonPbbs
                //    .Where(x => x.Npwpd == npwpd)
                //    .Select(wp => new DetailPembayaranWP
                //    {
                //        NPWPD = wp.Npwpd,
                //        JenisPajak = ((EnumFactory.EPajak)wp.PajakId).GetDescription(),
                //        TahunBuku = (int)wp.TahunBuku,
                //        Ketetapan = wp.PokokPajakKetetapan ?? 0,
                //        Terbayar = wp.NominalPokokBayar ?? 0,
                //        Tunggakan = (wp.PokokPajakKetetapan ?? 0) - (wp.NominalPokokBayar ?? 0),
                //        Status = (wp.NominalPokokBayar ?? 0) >= (wp.PokokPajakKetetapan ?? 0) ? "Lunas" :
                //                 (wp.NominalPokokBayar ?? 0) > 0 ? "Sebagian" : "Tunggakan",
                //        Kepatuhan = (wp.PokokPajakKetetapan ?? 0) == 0 ? "0%" :
                //            Math.Round(((wp.NominalPokokBayar ?? 0) / (wp.PokokPajakKetetapan ?? 0)) * 100, 2) + "%"
                //    })
                //    .ToList();

                ret.DetailPembayaranWPList.AddRange(DataResto);
                ret.DetailPembayaranWPList.AddRange(DataListrik);
                ret.DetailPembayaranWPList.AddRange(DataHotel);
                ret.DetailPembayaranWPList.AddRange(DataParkir);
                ret.DetailPembayaranWPList.AddRange(DataHiburan);
                ret.DetailPembayaranWPList.AddRange(DataAbt);
                ret.DetailPembayaranWPList.AddRange(DataReklame);
                //ret.DetailPembayaranWPList.AddRange(DataPbb);

                return ret;
            }
            // Di dalam public class Method
            public static Dashboard GetDashboardData()
            {
                // 1. Panggil data asli dari database SATU KALI SAJA
                List<PembayaranWP> allData = GetDataPembayaranWPList();

                // 2. Lakukan semua perhitungan di sini menggunakan LINQ
                int totalWp = allData.Count();

                decimal totalKetetapan = allData.Sum(x => x.Ketetapan);
                decimal totalPembayaran = allData.Sum(x => x.Terbayar);

                decimal totalTunggakan = totalKetetapan - totalPembayaran;

                decimal tingkatKepatuhan = (totalKetetapan > 0)
                    ? (totalPembayaran / totalKetetapan) * 100
                    : 0;

                // Asumsi: WP Menunggak adalah WP yang punya tunggakan (Ketetapan > Terbayar)
                // Ini lebih akurat daripada menghitung status "Tidak Aktif"
                int wpMenunggak = allData.Count(x => x.Ketetapan > x.Terbayar);

                // 3. Buat dan kembalikan objek Dashboard dengan data yang sudah dihitung
                var dashboardData = new Dashboard
                {
                    TotalWP = totalWp,
                    PersentaseKepatuhan = tingkatKepatuhan,
                    TotalTunggakan = totalTunggakan,
                    WPMenunggak = wpMenunggak,
                    TargetKepatuhan = 85, // Bisa diatur sesuai kebutuhan
                    PresentaseWP = 0 // Logika untuk ini bisa ditambahkan jika perlu
                };

                return dashboardData;
            }

        }

        public class PembayaranWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public decimal Ketetapan { get; set; }
            public decimal Terbayar { get; set; }
            public string Status { get; set; } = null!;
        }

        public class DetailPembayaranWP
        {
            public string NPWPD { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public int TahunBuku { get; set; }
            public decimal Ketetapan { get; set; }
            public decimal Terbayar { get; set; }
            public decimal Tunggakan { get; set; }
            public string Status { get; set; } = null!;
            public string Kepatuhan { get; set; } = null!;
        }

        public class ProfilWP
        {
            public string NPWPD { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string JenisSubjek { get; set; } = null!;
            public string NIB { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TanggalDaftar { get; set; }
            public string AlamatDom { get; set; } = null!;
            public string AlamatUsaha { get; set; } = null!;
            public string Kontak { get; set; } = null!;
            public string Email { get; set; } = null!;
            public List<DetailPembayaranWP> DetailPembayaranWPList { get; set; } = new List<DetailPembayaranWP>();

        }

        // Di dalam ProfilePembayaranWPVM.cs
        public class Dashboard
        {
            public int TotalWP { get; set; }
            public decimal PersentaseKepatuhan { get; set; }
            public decimal Tunggakan { get; set; }
            public decimal TotalTunggakan { get; set; }
            public int WPMenunggak { get; set; }
            public decimal PresentaseWP { get; set; }
            public int TargetKepatuhan { get; set; } = 85; // Contoh target
        }
    }
}
