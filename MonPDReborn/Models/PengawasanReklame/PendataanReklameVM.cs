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
            public List<Rekap> Data { get; set; }
            public Detail(string namaKegiatan)
            {
                Data = Method.GetRekapList(namaKegiatan);
            }
        }

        public static class Method
        {
            public static List<PengawasanReklame> GetPengawasanReklameData(int tahun, int bulan)
            {
                var context = DBClass.GetContext();
                var ret = context.DbMonReklames
                    .Where(x => x.TahunBuku == tahun /*&& x.BulanPajak == bulan*/)
                    .Select(x => new PengawasanReklame
                    {
                        /*NamaKegiatan = x.NamaKegiatan ?? "",
                        Tahun = x.Tahun,
                        Bulan = x.Bulan,
                        JmlPetugas = x.JmlPetugas,
                        Target = x.Target,
                        Terlaksana = x.Terlaksana,
                        Status = x.Status ?? ""*/
                    })
                    .ToList();
                return ret;
            }

            /*public static List<Rekap> GetRekaps(string namaKegiatan)
            {
                var context = DBClass.GetContext();
                var ret = context.DbMonReklames
                    .Where(x => x.NamaKegiatan != null && x.NamaKegiatan.Contains(namaKegiatan, StringComparison.OrdinalIgnoreCase))
                    .Select(x => new Rekap
                    {
                        NamaKegiatan = x.NamaKegiatan ?? "",
                        Surveyor = x.Surveyor ?? "",
                        Target = x.Target,
                        ObjekLama = x.ObjekLama,
                        PajakLama = x.PajakLama,
                        ObjekBaru = x.ObjekBaru,
                        PajakBaru = x.PajakBaru,
                        ObjekTutup = x.ObjekTutup,
                        PajakTutup = x.PajakTutup,
                        Status = x.Status ?? ""
                    })
                    .ToList();
                return ret;
            }*/
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

            public static List<Rekap> GetRekapList(string namaKegiatan)
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
            }

        }

        public class PengawasanReklame
        {
            public string NamaKegiatan { get; set; } = null!;
            public int Tahun { get; set; }
            public int Bulan { get; set; }
            public int JmlPetugas { get; set; }
            public int Target {  get; set; }
            public int Terlaksana { get; set; }
            public decimal Selisih => Target - Terlaksana;
            public decimal Persentase => Target == 0 ? 0 : Math.Round((decimal)Terlaksana / Target * 100, 2);
            public string Status { get; set; } = null!;
        }

        public class Rekap
        {
            public string NamaKegiatan { get; set; } = null!;
            public string Surveyor { get; set; } = null!;
            public int Target {  get; set; }
            public int ObjekLama { get; set; }
            public decimal PajakLama { get; set; }
            public int ObjekBaru { get; set; }
            public decimal PajakBaru { get; set; }
            public int ObjekTutup {  get; set; }
            public decimal PajakTutup { get; set; }
            public int TotalObjek => ObjekLama + ObjekBaru + ObjekTutup;
            public decimal TotalPajak => PajakLama + PajakBaru + PajakTutup;
            public int Selisih => Target - TotalObjek;
            public string Status { get; set; } = null!;
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
