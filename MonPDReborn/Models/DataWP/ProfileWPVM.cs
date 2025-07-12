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
            public Dashboard Data { get; set; } = new Dashboard();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }
        public class Show
        {
            public List<RekapWP> DataRekapWPList { get; set; } = new();
            public Show()
            {
                DataRekapWPList = Method.GetDataWpList();
            }
        }
        public class Detail
        {
            public List<ProfilWP> DataWPList { get; set; } = new();
            public Detail() { }
            public Detail(string NPWPD)
            {
                DataWPList = Method.GetDataProfilWP(NPWPD);
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
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
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
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
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
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
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
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
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
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
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
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
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
                            Status = first?.IsTutup == 1 ? "Tidak Aktif" : "Aktif"
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

            public static List<ProfilWP> GetDataProfilWP(string npwpd)
            {
                var ret = new List<ProfilWP>();

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
