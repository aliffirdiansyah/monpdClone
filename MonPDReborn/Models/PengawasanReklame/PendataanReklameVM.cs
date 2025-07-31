using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;

namespace MonPDReborn.Models.PengawasanReklame
{
    public class PendataanReklameVM
    {
        // Untuk halaman utama Index.cshtml
        public class Index
        {

        }

        // Untuk Partial View _Show.cshtml
        public class Show
        {
            public List<PengawasanReklame> Data { get; set; } = new();
            public int Tahun { get; set; }
            public int Bulan { get; set; }
            public Show(int tahun, int bulan)
            {
                Tahun = tahun;
                Bulan = bulan;
                Data = Method.GetPengawasanReklameData(tahun, bulan);
            }

        }

        // Untuk Partial View _Detail.cshtml (modal)
        public class Detail
        {
            public List<DetailPengawasan> Data { get; set; }
            public Detail(string namaKegiatan, int tahun, int bulan)
            {
                Data = Method.GetDetailPengawasanData(namaKegiatan, tahun, bulan);
            }
        }

        public class DetailRekap
        {
            public List<DetailReklame> Data { get; set; }
            public DetailRekap(string namaKegiatan, int tahun, int bulan, string petugas)
            {
                Data = Method.GetDetailReklameData(namaKegiatan, tahun, bulan, petugas);
            }
        }

        public static class Method
        {
            public static List<PengawasanReklame> GetPengawasanReklameData(int tahun, int bulan)
            {
                
                var context = DBClass.GetContext();

                var ret = context.DbMonReklameAktivitas
                    .Where(x => x.Tahun == tahun && x.Bulan == bulan)
                    .AsEnumerable()
                    .GroupBy(x => x.Aktifitas)
                    .Select(g => new PengawasanReklame
                    {
                        NamaKegiatan = g.Key ?? "-",
                        Tahun = tahun,              // ✅ Tambahkan ini
                        Bulan = bulan,
                        JmlPetugas = g
                                .SelectMany(x => (x.Petugas ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries))
                                .Select(n => n.Trim())
                                .Distinct()
                                .Count(),
                        Target = g.Sum(x => x.Target ?? 0),
                        Terlaksana = g.Sum(x => x.Terlaksana ?? 0),
                        Status = g
                            .Where(x => !string.IsNullOrEmpty(x.Status))
                            .GroupBy(x => x.Status)
                            .OrderByDescending(s => s.Count())
                            .Select(s => s.Key)
                            .FirstOrDefault() ?? "-"
                    }).ToList();

                return ret;
            }

            public static List<DetailPengawasan> GetDetailPengawasanData(string namaKegiatan, int tahun, int bulan)
            {
                var context = DBClass.GetContext();
                namaKegiatan = namaKegiatan.Trim().ToLowerInvariant();

                var ret = context.DbMonReklameAktivitas
                    .Where(x =>
                        x.Tahun == tahun &&
                        x.Bulan == bulan
                    )
                    .AsEnumerable()
                    .Where(x =>
                        (x.Aktifitas ?? "").Trim().ToLowerInvariant() == namaKegiatan
                    )
                    .SelectMany(x =>
                        (x.Petugas ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => new DetailPengawasan
                        {
                            NamaKegiatan = x.Aktifitas ?? "-",
                            Tahun = tahun,
                            Bulan = bulan,
                            Petugas = p.Trim(),
                            Target = x.Target ?? 0,
                            ObjekLama = x.Lama ?? 0,
                            PajakLama = x.PajakLama ?? 0,
                            ObjekBaru = x.Baru ?? 0,
                            PajakBaru = x.PajakBaru ?? 0,
                            ObjekTutup = x.Tutup ?? 0,
                            PajakTutup = x.PajakTutup ?? 0,
                            Terlaksana = x.Terlaksana ?? 0,
                            Status = x.Status ?? "-"
                        })
                    ).ToList();

                return ret;
            }

            public static List<DetailReklame> GetDetailReklameData(string namaKegiatan, int tahun, int bulan, string petugas)
            {
                var context = DBClass.GetContext();

                var ret = context.DbMonReklameAktivitasDets
                    .Where(d =>
                        (d.Aktifitas ?? "").Trim().ToLower() == namaKegiatan.Trim().ToLower() &&
                        d.Tahun == tahun &&
                        d.Bulan == bulan &&
                        (d.Petugas ?? "").Trim().ToLower() == petugas.Trim().ToLower()
                    )
                    .Select(d => new DetailReklame
                    {
                        Petugas = d.Petugas ?? "-",
                        NoFormulir = d.NoFormulir ?? "-",
                        NOR = d.Nor ?? "-",
                        NamaWP = d.Nama ?? "-",
                        NamaPerusahaan = d.NamaPerusahaan ?? "-",
                        AlamatPerusahaan = d.AlamatPerusahaan ?? "-",
                        IsiReklame = d.IsiReklame ?? "-",
                        AlamatReklame = d.Alamatreklame ?? "-",
                        Nominal = d.NominalPokokBayar ?? 0,
                        Tgl = d.Tanggal ?? DateTime.MinValue
                    })
                    .ToList();

                return ret;
            }

            //public static List<PengawasanReklame> GetPengawasanReklameList(int tahun, int bulan)
            //{
            //    var allData = GetPengawasanReklameData();

            //    return allData
            //        .Where(d => d.Tahun == tahun && d.Bulan == bulan)
            //        .ToList();
            //}

            //// Pengawasan Reklame
            //private static List<PengawasanReklame> GetPengawasanReklameData()
            //{
            //    return new List<PengawasanReklame>()
            //    {
            //        new PengawasanReklame
            //        {
            //            NamaKegiatan = "Survey",
            //            Tahun = 2025,
            //            Bulan = 5,
            //            JmlPetugas = 5,
            //            Target = 100,
            //            Terlaksana = 85,
            //            Status = "Tercapai"
            //        },
            //        new PengawasanReklame
            //        {
            //            NamaKegiatan = "Verifikasi",
            //            Tahun = 2025,
            //            Bulan = 5,
            //            JmlPetugas = 3,
            //            Target = 60,
            //            Terlaksana = 45,
            //            Status = "Belum Tercapai"
            //        },
            //    };
            //}

            // Rekap Survey Reklame

            /*public static List<Rekap> GetRekapList(string namaKegiatan)
            {
                var allData = GetRekapData();

                if (string.IsNullOrWhiteSpace(namaKegiatan))
                    return allData;

                return allData
                    .Where(d => d.NamaKegiatan != null && d.NamaKegiatan.Contains(namaKegiatan, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<Rekap> GetRekapData()
            {
                return new List<Rekap>()
                {
                    new Rekap
                    {
                        NamaKegiatan = "Survey",
                        Surveyor = "Andi",
                        Target = 50,
                        ObjekLama = 20,
                        PajakLama = 12_000_000m,
                        ObjekBaru = 15,
                        PajakBaru = 9_500_000m,
                        ObjekTutup = 5,
                        PajakTutup = 3_000_000m,
                        Status = "Di bawah Target"
                    },
                    new Rekap
                    {
                        NamaKegiatan = "Survey",
                        Surveyor = "Budi",
                        Target = 40,
                        ObjekLama = 10,
                        PajakLama = 6_000_000m,
                        ObjekBaru = 20,
                        PajakBaru = 8_000_000m,
                        ObjekTutup = 2,
                        PajakTutup = 1_000_000m,
                        Status = "Sesuai Target"
                    },
                    new Rekap
                    {
                        NamaKegiatan = "Verifikasi",
                        Surveyor = "Rina",
                        Target = 60,
                        ObjekLama = 25,
                        PajakLama = 14_500_000m,
                        ObjekBaru = 20,
                        PajakBaru = 10_000_000m,
                        ObjekTutup = 5,
                        PajakTutup = 2_000_000m,
                        Status = "Di bawah Target"
                    },
                    new Rekap
                    {
                        NamaKegiatan = "Verifikasi",
                        Surveyor = "Fajar",
                        Target = 45,
                        ObjekLama = 15,
                        PajakLama = 8_000_000m,
                        ObjekBaru = 10,
                        PajakBaru = 5_500_000m,
                        ObjekTutup = 3,
                        PajakTutup = 1_200_000m,
                        Status = "Sesuai Target"
                    },
                };
            }*/

        }

        public class PengawasanReklame
        {
            public string NamaKegiatan { get; set; } = null!;
            public int Tahun { get; set; }
            public int Bulan { get; set; }
            public decimal JmlPetugas { get; set; }
            public decimal Target {  get; set; }
            public decimal Terlaksana { get; set; }
            public decimal Selisih => Target - Terlaksana;
            public decimal Persentase => Target == 0 ? 0 : Math.Round((decimal)Terlaksana / Target * 100, 2);
            public string Status { get; set; } = null!;
        }

        public class DetailPengawasan
        {
            public string NamaKegiatan { get; set; } = null!;
            public string Petugas { get; set; } = null!;
            public int Tahun { get; set; }
            public int Bulan { get; set; }
            public decimal Target {  get; set; }
            public decimal ObjekLama { get; set; }
            public decimal PajakLama { get; set; }
            public decimal ObjekBaru { get; set; }
            public decimal PajakBaru { get; set; }
            public decimal ObjekTutup {  get; set; }
            public decimal PajakTutup { get; set; }
            public decimal Terlaksana { get; set; }
            public decimal TotalObjek => ObjekLama + ObjekBaru + ObjekTutup;
            public decimal TotalPajak => PajakLama + PajakBaru + PajakTutup;
            public decimal Selisih => Target - TotalObjek;
            public string Status { get; set; } = null!;
        }

        public class DetailReklame
        {
            public string Petugas { get; set; } = null!;
            public string NoFormulir { get; set; } = null!;
            public string NOR { get; set; } = null!;
            public string NamaWP { get; set; } = null!;
            public string NamaPerusahaan { get; set; } = null!;
            public string AlamatPerusahaan { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string AlamatReklame { get; set; } = null!;
            public decimal Nominal { get; set; }
            public DateTime Tgl { get; set; }
        }
        /* public class RekapVerifikasi
{
    public string NamaKegiatan { get; set; } = null!;
    public string Verifikator { get; set; } = null!;
    public int Target { get; set; }
    public int ObjekLama { get; set; }
    public decimal PajakLama { get; set; }
    public int ObjekBaru { get; set; }
    public decimal PajakBaru { get; set; }
    public int ObjekTutup { get; set; }
    public decimal PajakTutup { get; set; }
    public int TotalObjek => ObjekLama + ObjekBaru + ObjekTutup;
    public decimal TotalPajak => PajakLama + PajakBaru + PajakTutup;
    public int Selisih => Target - TotalObjek;
    public string Status { get; set; } = null!;
}*/
    }
}
