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
                Data.Temporary = DataRekapWPList.Count(wp => wp.JenisSubjek == "Temporary");
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

                var Data = context.Npwpds
                    .ToList()
                    .Select(wp =>
                    {
                        return new RekapWP
                        {
                            NPWPD = wp.NpwpdNo,
                            Nama = wp.Nama,
                            JenisSubjek = wp.JenisWp == 0 ? "Orang Pribadi" : wp.JenisWp == 1 ? "Badan Usaha" : "Temporary",
                            Kontak = wp.Kontak ?? wp.Hp,
                            Status = wp.Status == 0 ? "Tidak Aktif" : "Aktif",
                            TanggalDaftar = wp.InsDate,
                        };
                    })
                    .ToList();
               
                ret.AddRange(Data);

                return ret;
            }
            public static ProfilWP GetDataProfilWP(string npwpd)
            {
                var ret = new ProfilWP();
                var context = DBClass.GetContext();

                var Data = context.Npwpds
                    .Where(x => x.NpwpdNo == npwpd)
                    .Select(wp => new ProfilWP
                    {
                        NPWPD = wp.NpwpdNo,
                        Nama = wp.Nama,
                        JenisSubjek = wp.JenisWp == 0 ? "Orang Pribadi" : wp.JenisWp == 1 ? "Badan Usaha" : "Temporary",
                        NIB = wp.JenisWp == 0 ? "-" : wp.JenisWp == 1 ? wp.NpwpdNo : "-",
                        Status = wp.Status == 1 ? "Tidak Aktif" : "Aktif",
                        TanggalDaftar = wp.InsDate,
                        AlamatDom = wp.AlamatDomisili,
                        AlamatUsaha = wp.Alamat,
                        Kontak = wp.Kontak ?? wp.Hp,
                        Email = wp.Email
                    })
                    .OrderBy(r => r.TanggalDaftar)
                    .FirstOrDefault();

                if (Data != null)
                {
                    ret = Data;
                }

                
                return ret;
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
            public int TotalWP => OrangPribadi + BadanUsaha + Temporary;
            public int OrangPribadi { get; set; }
            public int BadanUsaha { get; set; }
            public int Temporary { get; set; }

            // Persentase otomatis
            public decimal PersentaseOrangPribadi
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return (decimal)OrangPribadi / TotalWP * 100;
                }
            }

            public decimal PersentaseBadanUsaha
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return (decimal)BadanUsaha / TotalWP * 100;
                }
            }
            
            public decimal PersentaseTemporary
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return (decimal)Temporary / TotalWP * 100;
                }
            }

            // Wajib Pajak Baru
            public int WPTahun { get; set; }       // Total tahun ini
            public int WPBulan { get; set; }       // Bulan ini
            public int RataWPBulan { get; set; }   // Rata-rata per bulan

            public decimal Presentase
            {
                get
                {
                    if (RataWPBulan == 0) return 0; // Hindari pembagi 0
                    return ((decimal)(WPBulan - RataWPBulan) / RataWPBulan) * 100;
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

            public decimal PersentaseAktif
            {
                get
                {
                    if (TotalWP == 0) return 0;
                    return ((decimal)WPAktif / TotalWP) * 100;
                }
            }
        }


    }
}
