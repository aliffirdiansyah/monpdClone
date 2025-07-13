using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using MonPDLib;
using static MonPDReborn.Models.AktivitasOP.PendataanObjekPajakVM;
using static MonPDReborn.Models.MonitoringWilayah.MonitoringWilayahVM;

namespace MonPDReborn.Models.DataWP
{
    public class ProfileWPVM
    {
        public class Index
        {
            public Index()
            {
            }
        }
        public class Show
        {
            public Dashboard Data { get; set; } = new Dashboard();
            public List<RekapWP> DataRekapWPList { get; set; } = new();
            public Show()
            {
                DataRekapWPList = Method.GetDataWpList();
                Data.BadanUsaha = DataRekapWPList.Count(wp => wp.JenisSubjek == "Badan Usaha");
                Data.OrangPribadi = DataRekapWPList.Count(wp => wp.JenisSubjek == "Orang Pribadi");
                Data.WPAktif = DataRekapWPList.Count(wp => wp.Status == "Aktif");
                Data.WPTahun = DataRekapWPList.Count(wp => wp.TanggalDaftar.Year == DateTime.Now.Year);
                Data.WPBulan = DataRekapWPList.Count(wp => wp.TanggalDaftar.Year == DateTime.Now.Year && wp.TanggalDaftar.Month == DateTime.Now.Month);
                Data.RataWPBulan = Data.WPTahun / 12;
            }
        }
        public class Detail
        {
            public ProfilWP DataWPRow { get; set; } = new();
            public Detail() { }
            public Detail(string NPWPD)
            {
                DataWPRow = Method.GetDataProfilWP(NPWPD);
            }
        }

        public class Method
        {
            public static List<RekapWP> GetDataWpList()
            {
                var ret = new List<RekapWP>();
                var context = DBClass.GetContext();

                var DataResto = context.DbOpRestos
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            Nama = first?.NpwpdNama,
                            JenisSubjek = "",
                            Kontak = first?.Telp,
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = first.TglMulaiBukaOp,
                        };
                    })
                    .ToList();
                var DataListrik = context.DbOpListriks
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            Nama = first?.NpwpdNama,
                            JenisSubjek = "",
                            Kontak = first?.Telp,
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = first.TglMulaiBukaOp,
                        };
                    })
                    .ToList();
                var DataHotel = context.DbOpHotels
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            Nama = first?.NpwpdNama,
                            JenisSubjek = "",
                            Kontak = first?.Telp,
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = first.TglMulaiBukaOp,
                        };
                    })
                    .ToList();
                var DataParkir = context.DbOpParkirs
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            Nama = first?.NpwpdNama,
                            JenisSubjek = "",
                            Kontak = first?.Telp,
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = first.TglMulaiBukaOp,
                        };
                    })
                    .ToList();
                var DataHiburan = context.DbOpHiburans
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            Nama = first?.NpwpdNama,
                            JenisSubjek = "",
                            Kontak = first?.Telp,
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = first.TglMulaiBukaOp,
                        };
                    })
                    .ToList();
                var DataAbt = context.DbOpAbts
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            Nama = first?.NpwpdNama,
                            JenisSubjek = "",
                            Kontak = first?.Telp,
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = first.TglMulaiBukaOp,
                        };
                    })
                    .ToList();
                var DataReklame = context.DbOpReklames
                    .GroupBy(r => new { r.Npwpd })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.Npwpd,
                            Nama = first?.NamaPerusahaan,
                            JenisSubjek = "",
                            Kontak = first?.TelpPerusahaan,
                            Status = "Aktif"
                        };
                    })
                    .ToList();

                var DataPbb = context.DbOpPbbs
                    .GroupBy(r => new { r.WpNpwp })
                    .ToList()
                    .Select(wp =>
                    {
                        var first = wp.FirstOrDefault();
                        return new RekapWP
                        {
                            NPWPD = wp.Key.WpNpwp,
                            Nama = first?.WpNama,
                            JenisSubjek = "",
                            Kontak = "-",
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = new DateTime((int)first.TahunBuku, 1, 1),
                        };
                    })
                    .ToList();

                ret.AddRange(DataResto);
                ret.AddRange(DataListrik);
                ret.AddRange(DataHotel);
                ret.AddRange(DataParkir);
                ret.AddRange(DataHiburan);
                ret.AddRange(DataAbt);
                ret.AddRange(DataReklame);
                ret.AddRange(DataPbb);


                //var DataBphtb = context.DbMonBphtbs
                //    .GroupBy(r => new { r.Npwpd })
                //    .ToList()
                //    .Select(wp =>
                //    {
                //        var first = wp.FirstOrDefault();
                //        return new RekapWP
                //        {
                //            NPWPD = wp.Key.WpNpwp,
                //            Nama = first?.WpNama,
                //            JenisSubjek = "",
                //            Kontak = "-",
                //            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
                //        };
                //    })
                //    .ToList();



                return ret;
            }
            public static ProfilWP GetDataProfilWP(string npwpd)
            {
                var ret = new ProfilWP();
                var context = DBClass.GetContext();

                var DataResto = context.DbOpRestos
                    .Where(x => x.Npwpd == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.Npwpd,
                        Nama = r.NpwpdNama,
                        JenisSubjek = "-",
                        NIB = "-",
                        Status = r.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = r.TglMulaiBukaOp,
                        AlamatDom = r.NpwpdAlamat,
                        AlamatUsaha = r.NpwpdAlamat,
                        Kontak = r.Telp,
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                var DataListrik = context.DbOpListriks
                .Where(x => x.Npwpd == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.Npwpd,
                        Nama = r.NpwpdNama,
                        JenisSubjek = "-",
                        NIB = "-",
                        Status = r.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = r.TglMulaiBukaOp,
                        AlamatDom = r.NpwpdAlamat,
                        AlamatUsaha = r.NpwpdAlamat,
                        Kontak = r.Telp,
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                var DataHotel = context.DbOpHotels
                    .Where(x => x.Npwpd == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.Npwpd,
                        Nama = r.NpwpdNama,
                        JenisSubjek = "-",
                        NIB = "-",
                        Status = r.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = r.TglMulaiBukaOp,
                        AlamatDom = r.NpwpdAlamat,
                        AlamatUsaha = r.NpwpdAlamat,
                        Kontak = r.Telp,
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                var DataParkir = context.DbOpParkirs
                    .Where(x => x.Npwpd == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.Npwpd,
                        Nama = r.NpwpdNama,
                        JenisSubjek = "-",
                        NIB = "-",
                        Status = r.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = r.TglMulaiBukaOp,
                        AlamatDom = r.NpwpdAlamat,
                        AlamatUsaha = r.NpwpdAlamat,
                        Kontak = r.Telp,
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                var DataHiburan = context.DbOpHiburans
                    .Where(x => x.Npwpd == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.Npwpd,
                        Nama = r.NpwpdNama,
                        JenisSubjek = "-",
                        NIB = "-",
                        Status = r.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = r.TglMulaiBukaOp,
                        AlamatDom = r.NpwpdAlamat,
                        AlamatUsaha = r.NpwpdAlamat,
                        Kontak = r.Telp,
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                var DataAbt = context.DbOpAbts
                    .Where(x => x.Npwpd == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.Npwpd,
                        Nama = r.NpwpdNama,
                        JenisSubjek = "-",
                        NIB = "-",
                        Status = r.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = r.TglMulaiBukaOp,
                        AlamatDom = r.NpwpdAlamat,
                        AlamatUsaha = r.NpwpdAlamat,
                        Kontak = r.Telp,
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                var DataReklame = context.DbOpReklames
                    .Where(x => x.Npwpd == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.Npwpd,
                        Nama = r.NamaPerusahaan,
                        JenisSubjek = r.JenisWp,
                        NIB = "-",
                        Status = "Aktif",
                        TanggalDaftar = r.TglPermohonan.Value,
                        AlamatDom = r.AlamatperPenanggungjawab,
                        AlamatUsaha = r.AlamatPerusahaan,
                        Kontak = "-",
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();


                var DataPbb = context.DbOpPbbs
                    .Where(x => x.WpNpwp == npwpd)
                    .Select(r => new ProfilWP
                    {
                        NPWPD = r.WpNpwp,
                        Nama = r.WpNama,
                        JenisSubjek = "-",
                        NIB = "-",
                        Status = r.IsTutup == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = r.InsDate,
                        AlamatDom = r.AlamatWp,
                        AlamatUsaha = r.AlamatWp,
                        Kontak = "-",
                        Email = "-"
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                ret = DataResto
                    ?? DataListrik
                    ?? DataHotel
                    ?? DataParkir
                    ?? DataHiburan
                    ?? DataAbt
                    ?? DataReklame
                    ?? DataPbb;

                return ret;
            }
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    OrangPribadi = 8234,
                    BadanUsaha = 4613,
                    WPTahun = 1234,
                    WPBulan = 156,
                    RataWPBulan = 103,
                    WPAktif = 11523
                };
            }
        }

        public class RekapWP
        {
            public string NPWPD { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string JenisSubjek { get; set; } = null!;
            public string Kontak { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TanggalDaftar { get; set; }

        }

        public class ProfilWP
        {
            public string NPWPD { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string JenisSubjek { get; set; } = null!;
            public string NIB { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TanggalDaftar { get; set; }
            public string AlamatDom { get; set; } = null!;
            public string AlamatUsaha { get; set; } = null!;
            public string Kontak { get; set; } = null!;
            public string Email { get; set; } = null!;

        }

        public class Dashboard
        {
            // Data dasar
            public int TotalWP => OrangPribadi + BadanUsaha;
            public int OrangPribadi { get; set; }
            public int BadanUsaha { get; set; }

            // Persentase otomatis
            public double PersentaseOrangPribadi
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return (double)OrangPribadi / TotalWP * 100;
                }
            }

            public double PersentaseBadanUsaha
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return (double)BadanUsaha / TotalWP * 100;
                }
            }

            // Wajib Pajak Baru
            public int WPTahun { get; set; }       // Total tahun ini
            public int WPBulan { get; set; }       // Bulan ini
            public int RataWPBulan { get; set; }   // Rata-rata per bulan

            public double Presentase
            {
                get
                {
                    if (RataWPBulan == 0) return 0; // Hindari pembagi 0
                    return ((double)(WPBulan - RataWPBulan) / RataWPBulan) * 100;
                }
            }

            // Wajib Pajak Aktif
            public int WPAktif { get; set; }
            public int WPNonAktif
            {
                get
                {
                    return TotalWP - WPAktif;
                }
            }

            public double PersentaseAktif
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return ((double)WPAktif / TotalWP) * 100;
                }
            }
        }


    }
}
