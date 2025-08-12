using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using SixLabors.Fonts.Tables.TrueType;
using System.Collections.Generic;
using System.Drawing;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Models.DashboardVM.ViewModel;

namespace MonPDReborn.Models.Reklame
{
    public class ReklameVM
    {
        public class Index
        {
            public DashboardData Data { get; set; } = new();
            public int SelectedJenisReklame { get; set; }
            public DateTime[] SelectedDateRange { get; set; }
            public DateTime TglAwal { get; set; }
            public DateTime TglAkhir { get; set; }
            public Index()
            {
                Data = Method.GetDashboardData();
                var tglskr = DateTime.Now;
                TglAwal = new DateTime(tglskr.Year, 1, 1);
                TglAkhir = tglskr;
            }
        }
        public class Show
        {
            public List<ReklamePerJalan> DataReklamePerJalanList { get; set; } = new();
            public Show()
            {
                DataReklamePerJalanList = Method.GetDataReklamePerJalan();
            }
        }
        public class Detail
        {
            public List<DataReklame> DataReklameDetailList { get; set; } = new();
            public string NamaJalan { get; set; } = "";

            public Detail(string namaJalan)
            {
                NamaJalan = namaJalan;
                DataReklameDetailList = Method.GetDetailDataByJalan(namaJalan);
            }
        }

        public class ShowData
        {
            public List<RekapData> DataRekap { get; set; } = new();
            public ShowData()
            {
                DataRekap = Method.GetRekapDataReklame();
            }
        }

        public class DetailReklame
        {
            public List<DetailData> DataDetailList { get; set; } = new();
            public DetailReklame(int idFlagPer, string namaJalan, string jenis)
            {
                DataDetailList = Method.GetDetailDataReklame(idFlagPer, namaJalan, jenis);
            }
            public DetailReklame(string kelasJalan, string namaJalan, string status, string jenisReklame, DateTime tglAwal, DateTime tglAkhir)
            {
                DataDetailList = Method.GetDetailDataReklame(kelasJalan, namaJalan, status, jenisReklame, tglAwal, tglAkhir);
            }

        }


        public class Method
        {
            public static DashboardData GetDashboardData()
            {
                // 1. Ambil data sumber
                var semuaReklame = GetAllDataReklame();       // Untuk total reklame
                var reklamePerJalan = GetDataReklamePerJalan(); // Untuk total jalan

                // 2. Lakukan perhitungan sesuai permintaan
                var dashboard = new DashboardData
                {
                    // Menghitung jumlah semua reklame
                    TotalReklame = semuaReklame.Count(),
                    // Menghitung jumlah baris/jalan unik yang memiliki reklame
                    JalanDenganReklame = reklamePerJalan.Count(),
                    // Mengisi data dummy untuk pelanggaran
                    PelanggaranTerdeteksi = 12
                };

                return dashboard;
            }
            // Method BARU untuk membuat data ringkasan per jalan
            public static List<ReklamePerJalan> GetDataReklamePerJalan()
            {
                return new List<ReklamePerJalan>
        {
            new ReklamePerJalan
            {
                NamaJalan = "Jl. KUTAI", KelasJalan = "Kelas I",
                InsidentilBongkar = 3, InsidentilBelumBongkar = 0, InsidentilAktif = 5,
                PermanenBongkar = 8, PermanenBelumBongkar = 2, PermanenAktif = 10,
                TerbatasBongkar = 1, TerbatasBelumBongkar = 1, TerbatasAktif = 2,
            },
            new ReklamePerJalan
            {
                NamaJalan = "Jl. AHMAD YANI", KelasJalan = "Kelas II",
                InsidentilBongkar = 5, InsidentilBelumBongkar = 2, InsidentilAktif = 10,
                PermanenBongkar = 15, PermanenBelumBongkar = 5, PermanenAktif = 20,
                TerbatasBongkar = 3, TerbatasBelumBongkar = 0, TerbatasAktif = 4,
            },
            new ReklamePerJalan
            {
                NamaJalan = "Jl. BASUKI RAHMAT", KelasJalan = "Kelas III",
                InsidentilBongkar = 2, InsidentilBelumBongkar = 1, InsidentilAktif = 8,
                PermanenBongkar = 10, PermanenBelumBongkar = 3, PermanenAktif = 15,
                TerbatasBongkar = 2, TerbatasBelumBongkar = 2, TerbatasAktif = 5,
            }
        };
            }

            // Method BARU untuk memfilter data detail berdasarkan jalan
            public static List<DataReklame> GetDetailDataByJalan(string namaJalan)
            {
                return GetAllDataReklame()
                    .Where(r => r.TitikLokasi.Contains(namaJalan))
                    .ToList();
            }

            // Method ini sekarang menjadi sumber data utama
            public static List<DataReklame> GetAllDataReklame()
            {
                return new List<DataReklame>
                {
                    new DataReklame { TitikLokasi = "Jl. Ahmad Yani (Depan Graha Pena)", Jenis = "Permanen > 8m", Ukuran = "5m x 10m", Penyelenggara = "PT. Djarum", MasaBerlaku = "01 Jan s/d 31 Des 2025" },
                    new DataReklame { TitikLokasi = "Jl. Ahmad Yani (Samping Royal Plaza)", Jenis = "Permanen < 8m", Ukuran = "3m x 4m", Penyelenggara = "Unilever Indonesia", MasaBerlaku = "01 Jun 2025 s/d 31 Mei 2026" },
                    new DataReklame { TitikLokasi = "Jl. Basuki Rahmat (Depan Tunjungan Plaza)", Jenis = "Permanen < 8m", Ukuran = "4m x 6m", Penyelenggara = "Samsung Indonesia", MasaBerlaku = "15 Mar s/d 14 Mar 2026" },
                    new DataReklame { TitikLokasi = "Perempatan Jl. Kertajaya Indah", Jenis = "Insidentil", Ukuran = "1m x 2m", Penyelenggara = "Event Organizer Laris", MasaBerlaku = "10 Jul s/d 20 Jul 2025" },
                    new DataReklame { TitikLokasi = "Jl. Mayjend Sungkono (Ciputra World)", Jenis = "Permanen > 8m", Ukuran = "6m x 12m", Penyelenggara = "Gudang Garam Tbk", MasaBerlaku = "01 Feb 2025 s/d 31 Jan 2026" },
                    new DataReklame { TitikLokasi = "Area Parkir Stadion GBT", Jenis = "Insidentil", Ukuran = "3m x 5m", Penyelenggara = "Konser Musik Nasional", MasaBerlaku = "01 Agu s/d 03 Agu 2025" }
                };
            }

            public static List<RekapData> GetRekapDataReklame()
            {
                var ret = new List<RekapData>();
                var context = DBClass.GetContext();
                var tglCutOff = new DateTime(DateTime.Now.Year, 1, 1); // Tanggal awal tahun
                var tglAkhir = DateTime.Now;

                var jalanList = context.DbMonReklames.Where(x => x.TglMulaiBerlaku.Value.Date >= tglCutOff.Date && x.TglMulaiBerlaku.Value.Date <= tglAkhir.Date).Select(x => new { x.KelasJalan, x.NamaJalan }).Distinct().ToList();

                var insidentilData = context.MvReklameRekapJalans.Where(x => x.IdFlagPermohonan == 1).ToList();
                var permanenData = context.MvReklameRekapJalans.Where(x => x.IdFlagPermohonan == 2).ToList();
                var terbatasData = context.MvReklameRekapJalans.Where(x => x.IdFlagPermohonan == 3).ToList();

                foreach (var item in jalanList)
                {
                    var row = new RekapData();
                    row.KelasJalan = "Kelas " + item.KelasJalan;
                    row.NamaJalan = item.NamaJalan;
                    row.KelasJalanId = item.KelasJalan;

                    // INSIDENTIL
                    var ins = insidentilData.FirstOrDefault(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan);
                    row.Insidentil.Aktif = ins?.Aktif ?? 0;
                    row.Insidentil.Expired = ins?.Expire ?? 0;
                    row.Insidentil.Perpanjangan = ins?.Perpanjangan ?? 0;
                    row.Insidentil.WajibBongkar = ins?.WajibBongkar ?? 0;
                    row.Insidentil.Bongkar = ins?.Bongkar ?? 0;
                    row.Insidentil.BlmBongkar = ins?.BlmBongkar ?? 0;

                    // PERMANEN
                    var per = permanenData.FirstOrDefault(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan);
                    row.Permanen.Aktif = per?.Aktif ?? 0;
                    row.Permanen.Expired = per?.Expire ?? 0;
                    row.Permanen.Perpanjangan = per?.Perpanjangan ?? 0;
                    row.Permanen.WajibBongkar = per?.WajibBongkar ?? 0;
                    row.Permanen.Bongkar = per?.Bongkar ?? 0;
                    row.Permanen.BlmBongkar = per?.BlmBongkar ?? 0;

                    // TERBATAS
                    var terb = terbatasData.FirstOrDefault(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan);
                    row.Terbatas.Aktif = terb?.Aktif ?? 0;
                    row.Terbatas.Expired = terb?.Expire ?? 0;
                    row.Terbatas.Perpanjangan = terb?.Perpanjangan ?? 0;
                    row.Terbatas.WajibBongkar = terb?.WajibBongkar ?? 0;
                    row.Terbatas.Bongkar = terb?.Bongkar ?? 0;
                    row.Terbatas.BlmBongkar = terb?.BlmBongkar ?? 0;

                    ret.Add(row);
                }

                return ret;
            }

            public static List<DetailData> GetDetailDataReklame(int idFlagPer, string namaJalan, string jenis)
            {
                var ret = new List<DetailData>();
                var context = DBClass.GetContext();
                var tglCutOff = new DateTime(DateTime.Now.Year, 1, 1); // Tanggal awal tahun
                var tglAkhir = DateTime.Now;

                //Total
                //Aktif
                //Expired
                //Perpanjangan
                //WajibBongkar
                //Bongkar
                //BlmBongkar

                if (jenis == "Total")
                {
                    ret = context.MvReklameSummaries
                        .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();
                }
                else if (jenis == "Aktif")
                {
                    ret = context.MvReklameSummaries
                        .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan
                             && x.TglAkhirBerlaku >= tglAkhir)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();

                }
                else if (jenis == "Expired")
                {
                    ret = context.MvReklameSummaries
                        .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan
                             && x.TglAkhirBerlaku < tglAkhir)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();
                }
                else if (jenis == "Perpanjangan")
                {
                    ret = context.MvReklameSummaries
                        .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan
                             && x.TglAkhirBerlaku >= tglCutOff
                             && x.TglAkhirBerlaku <= tglAkhir
                             && x.NoFormulirA != null)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();
                }
                else if (jenis == "WajibBongkar")
                {
                    ret = context.MvReklameSummaries
                        .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan
                             && x.TglAkhirBerlaku >= tglCutOff
                            && x.TglAkhirBerlaku <= tglAkhir
                            && x.NoFormulirA == null)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();
                }
                else if (idFlagPer == 1 && jenis == "Bongkar")
                {
                    ret = context.MvReklameSummaries
                        .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan
                             && x.TglAkhirBerlaku >= tglCutOff
                            && x.TglAkhirBerlaku <= tglAkhir
                            && x.NoFormulirA == null
                            && x.Bongkar == 0)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();
                }
                else if (idFlagPer == 1 && jenis == "BlmBongkar")
                {
                    ret = context.MvReklameSummaries
                         .Where(x => x.Tahun == tglCutOff.Year
                            && x.IdFlagPermohonan == idFlagPer
                            && x.NamaJalan == namaJalan
                            && x.TglAkhirBerlaku >= tglCutOff
                            && x.TglAkhirBerlaku <= tglAkhir
                            && x.NoFormulirA == null
                            && x.Bongkar != 0)
                       .Select(x => new DetailData
                       {
                           AlamatReklame = x.Alamatreklame,
                           Kategori = "-",
                           JenisReklame = x.FlagPermohonan,
                           IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                           KategoriReklame = x.NmJenis,
                           KelasJalan = x.KelasJalan,
                           NamaJalan = x.NamaJalan,
                           Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                           TglMulai = x.TglMulaiBerlaku.Value,
                           TglSelesai = x.TglAkhirBerlaku.Value,
                           Pajak = x.PajakPokok.Value,
                           Jumlah = x.Jumlah ?? 0
                       })
                       .ToList();
                }
                else if (jenis == "Bongkar")
                {
                    ret = context.MvReklameSummaries
                        .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan
                             && x.TglAkhirBerlaku >= tglCutOff
                            && x.TglAkhirBerlaku <= tglAkhir
                            && x.NoFormulirA == null
                            && x.Bongkar != 0)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();
                }
                else if (jenis == "BlmBongkar")
                {
                    ret = context.MvReklameSummaries
                          .Where(x => x.Tahun == tglCutOff.Year
                             && x.IdFlagPermohonan == idFlagPer
                             && x.NamaJalan == namaJalan
                             && x.TglAkhirBerlaku >= tglCutOff
                             && x.TglAkhirBerlaku <= tglAkhir
                             && x.NoFormulirA == null
                             && x.Bongkar == 0)
                        .Select(x => new DetailData
                        {
                            AlamatReklame = x.Alamatreklame,
                            Kategori = "-",
                            JenisReklame = x.FlagPermohonan,
                            IsiReklame = x.IsiReklame ?? x.IsiReklameA,
                            KategoriReklame = x.NmJenis,
                            KelasJalan = x.KelasJalan,
                            NamaJalan = x.NamaJalan,
                            Status = x.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            TglMulai = x.TglMulaiBerlaku.Value,
                            TglSelesai = x.TglAkhirBerlaku.Value,
                            Pajak = x.PajakPokok.Value,
                            Jumlah = x.Jumlah ?? 0
                        })
                        .ToList();
                }

                return ret;

            }

            public static List<RekapData> GetRekapDataReklame(DateTime tglAwal, DateTime tglAkhir)
            {
                var ret = new List<RekapData>();
                var context = DBClass.GetContext();

                var insidentilData = context.DbMonReklames
                    .Where(r =>
                        r.FlagPermohonan == "INSIDENTIL" &&
                        r.TglAkhirBerlaku.HasValue &&
                        r.TglAkhirBerlaku.Value.Date >= tglAwal.Date &&
                        r.TglAkhirBerlaku.Value.Date <= tglAkhir.Date)
                    .Select(r => new
                    {
                        r.KelasJalan,
                        r.NamaJalan,
                        Status = r.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF"
                    })
                    .GroupBy(r => new { r.KelasJalan, r.NamaJalan, r.Status })
                    .Select(g => new
                    {
                        KelasJalan = g.Key.KelasJalan,
                        NamaJalan = g.Key.NamaJalan,
                        Status = g.Key.Status,
                        Jml = g.Count()
                    })
                    .ToList();

                var permanenData = (
                from a in context.DbMonReklames
                where a.FlagPermohonan == "PERMANEN"
                      && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
                      && a.TglAkhirBerlaku.Value.Date <= tglAkhir.Date
                join b in context.DbMonReklameUpayas.Where(u => u.Upaya == "PEMBONGKARAN")
                    on a.NoFormulir equals b.NoFormulir into gj
                from b in gj.DefaultIfEmpty()
                select new
                {
                    a.NoFormulir,
                    a.KelasJalan,
                    a.NamaJalan,
                    Status = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date
                             ? "EXPIRED"
                             : "AKTIF",
                    Id = b != null ? b.Seq : (decimal?)null
                }
            ).ToList();

                var terbatasData = (
    from a in context.DbMonReklames
    where a.FlagPermohonan == "TERBATAS"
          && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
          && a.TglAkhirBerlaku.Value.Date <= tglAkhir.Date
    join b in context.DbMonReklameUpayas.Where(u => u.Upaya == "PEMBONGKARAN")
        on a.NoFormulir equals b.NoFormulir into gj
    from b in gj.DefaultIfEmpty()
    select new
    {
        a.NoFormulir,
        a.KelasJalan,
        a.NamaJalan,
        Status = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date
                 ? "EXPIRED"
                 : "AKTIF",
        Id = b != null ? b.Seq : (decimal?)null
    }
).ToList();

                var jalanList = context.DbMonReklames.Where(x => x.TglMulaiBerlaku.Value.Date >= tglAwal.Date && x.TglMulaiBerlaku.Value.Date <= tglAkhir.Date).Select(x => new { x.KelasJalan, x.NamaJalan }).Distinct().ToList();


                foreach (var item in jalanList)
                {
                    var row = new RekapData();
                    row.KelasJalan = "Kelas " + item.KelasJalan;
                    row.NamaJalan = item.NamaJalan;
                    row.KelasJalanId = item.KelasJalan;

                    if (row.NamaJalan == "EMBONG MALANG")
                    {
                        var AA = 1;
                    }
                    var rowInsidentilExp = insidentilData.SingleOrDefault(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "EXPIRED");
                    if (rowInsidentilExp != null)
                    {
                        row.Insidentil.Bongkar = rowInsidentilExp.Jml;
                        row.Insidentil.BlmBongkar = 0;
                    }
                    var rowInsidentilAktif = insidentilData.SingleOrDefault(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "AKTIF");
                    if (rowInsidentilAktif != null)
                    {
                        row.Insidentil.Aktif = rowInsidentilAktif.Jml;
                    }

                    row.Permanen.BlmBongkar = permanenData.Where(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "EXPIRED" && x.Id.HasValue == false).Count();
                    row.Permanen.Bongkar = permanenData.Where(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "EXPIRED" && x.Id.HasValue).Count();
                    row.Permanen.Aktif = permanenData.Where(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "AKTIF").Count();


                    row.Terbatas.BlmBongkar = terbatasData.Where(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "EXPIRED" && x.Id.HasValue == false).Count();
                    row.Terbatas.Bongkar = terbatasData.Where(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "EXPIRED" && x.Id.HasValue).Count();
                    row.Terbatas.Aktif = terbatasData.Where(x => x.KelasJalan == item.KelasJalan && x.NamaJalan == item.NamaJalan && x.Status == "AKTIF").Count();

                    ret.Add(row);
                }


                //var allKeys = insidentilData
                //    .Select(x => new { x.KelasJalan, x.NamaJalan })
                //    .Concat(permanenData.Select(x => new { x.KelasJalan, x.NamaJalan }))
                //    .Concat(terbatasData.Select(x => new { x.KelasJalan, x.NamaJalan }))
                //    .Distinct()
                //    .ToList();

                //foreach (var key in allKeys)
                //{
                //    var insidentil = insidentilData
                //        .Where(x => x.KelasJalan == key.KelasJalan && x.NamaJalan == key.NamaJalan)
                //        .ToList();

                //    var permanen = permanenData
                //        .Where(x => x.KelasJalan == key.KelasJalan && x.NamaJalan == key.NamaJalan)
                //        .ToList();

                //    var terbatas = terbatasData
                //        .Where(x => x.KelasJalan == key.KelasJalan && x.NamaJalan == key.NamaJalan)
                //        .ToList();

                //    var item = new RekapData
                //    {
                //        KelasJalanId = key.KelasJalan,
                //        KelasJalan = "Kelas " + key.KelasJalan,
                //        NamaJalan = key.NamaJalan,
                //        Isidentil = new KategoriReklame
                //        {
                //            JenisReklame = "INSIDENTIL",
                //            ExpiredBongkar = insidentil.Count(x => x.Status == "EXPIRED"),
                //            ExpiredBlmBongkar = insidentil.Count(x => x.Status == "AKTIF"),
                //            Aktif = insidentil.Count(x => x.Status == "AKTIF")
                //        },
                //        Permanen = new KategoriReklame
                //        {
                //            JenisReklame = "PERMANEN",
                //            ExpiredBongkar = permanen.Count(x => x.Status == "EXPIRED" && x.UpayaId != 0),
                //            ExpiredBlmBongkar = permanen.Count(x => x.Status == "EXPIRED" && x.UpayaId == 0),
                //            Aktif = permanen.Count(x => x.Status == "AKTIF")
                //        },
                //        Terbatas = new KategoriReklame
                //        {
                //            JenisReklame = "TERBATAS",
                //            ExpiredBongkar = terbatas.Count(x => x.Status == "EXPIRED" && x.UpayaId == 1),
                //            ExpiredBlmBongkar = terbatas.Count(x => x.Status == "EXPIRED" && x.UpayaId == 0),
                //            Aktif = terbatas.Count(x => x.Status == "AKTIF")
                //        }
                //    };

                //    ret.Add(item);
                //}

                return ret;
            }

            public static List<DetailData> GetDetailDataReklame(string kelasJalan, string namaJalan, string status, string jenisReklame, DateTime tglAwal, DateTime tglAkhir)
            {
                var ret = new List<DetailData>();
                var context = DBClass.GetContext();

                //insidentil
                if (jenisReklame == "INSIDENTIL")
                {
                    if (status == "ExpiredBongkar")
                    {

                        var insidentilDataExpBongkar = context.DbMonReklames
                        .Where(r =>
                            r.FlagPermohonan == "INSIDENTIL" &&
                            r.TglAkhirBerlaku.HasValue &&
                            r.TglAkhirBerlaku.Value.Date >= tglAwal.Date &&
                            r.TglAkhirBerlaku.Value.Date <= tglAkhir.Date &&
                            r.TglAkhirBerlaku.Value.Date < DateTime.Today &&
                            r.KelasJalan == kelasJalan &&
                            r.NamaJalan == namaJalan
                        )
                        .Select(r => new
                        {
                            r.NoFormulir,
                            r.KelasJalan,
                            r.NamaJalan,
                            r.PokokPajakKetetapan,
                            r.Alamatreklame,
                            r.IsiReklame,
                            r.NmJenis,
                            r.JenisProduk,
                            Status = r.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            r.FlagPermohonan,
                            TglMulai = r.TglMulaiBerlaku,
                            TglAkhir = r.TglAkhirBerlaku
                        })
                        .ToList();
                        foreach (var item in insidentilDataExpBongkar)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.Status,
                                TglMulai = item.TglMulai.Value,
                                TglSelesai = item.TglAkhir.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }
                    }
                    if (status == "ExpiredBlmBongkar")
                    {
                        //var insidentilDataExpBlmBongkar = context.DbMonReklames
                        //.Where(r => r.FlagPermohonan == "INSIDENTIL" &&
                        //            r.KelasJalan == kelasJalan &&
                        //            r.NamaJalan == namaJalan &&
                        //            r.TglAkhirBerlaku.HasValue && r.TglAkhirBerlaku.Value >= tglAwal &&
                        //            !(r.TglAkhirBerlaku.Value < DateTime.Today)
                        //            )
                        //.Select(r => new DetailData
                        //{
                        //    KelasJalan = r.KelasJalan,
                        //    NamaJalan = r.NamaJalan,
                        //    AlamatReklame = r.Alamat,
                        //    IsiReklame = r.IsiReklame,
                        //    JenisReklame = r.Jenis,
                        //    Kategori = r.KategoriKetetapan,
                        //    Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        //    KategoriReklame = r.FlagPermohonan,
                        //    TglMulai = r.TglMulaiBerlaku.Value,
                        //    TglSelesai = r.TglAkhirBerlaku.Value
                        //})
                        //.ToList();
                        //ret.AddRange(insidentilDataExpBlmBongkar);
                    }
                    if (status == "Aktif")
                    {
                        var insidentilDataAktif = context.DbMonReklames
                        .Where(r =>
                            r.FlagPermohonan == "INSIDENTIL" &&
                            r.TglAkhirBerlaku.HasValue &&
                            r.TglAkhirBerlaku.Value.Date >= tglAwal.Date &&
                            r.TglAkhirBerlaku.Value.Date <= tglAkhir.Date &&
                            r.TglAkhirBerlaku.Value.Date >= DateTime.Today &&
                            r.KelasJalan == kelasJalan &&
                            r.NamaJalan == namaJalan
                        )
                        .Select(r => new
                        {
                            r.NoFormulir,
                            r.KelasJalan,
                            r.NamaJalan,
                            r.PokokPajakKetetapan,
                            r.Alamatreklame, // Jika nama properti di model berbeda
                            r.IsiReklame,
                            r.NmJenis,
                            r.JenisProduk,
                            Status = r.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                            r.FlagPermohonan,
                            TglMulai = r.TglMulaiBerlaku,
                            TglAkhir = r.TglAkhirBerlaku
                        })
                        .ToList();
                        foreach (var item in insidentilDataAktif)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.Status,
                                TglMulai = item.TglMulai.Value,
                                TglSelesai = item.TglAkhir.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }

                    }
                }
                //permanen
                else if (jenisReklame == "PERMANEN")
                {
                    if (status == "ExpiredBongkar")
                    {
                        var permanenDataExpBongkar = (
                             from a in context.DbMonReklames
                             join b in context.DbMonReklameUpayas.Where(b => b.Upaya == "PEMBONGKARAN")
                                 on a.NoFormulir equals b.NoFormulir into gj
                             from b in gj.DefaultIfEmpty()
                             where a.FlagPermohonan == "PERMANEN"
                                 && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
                                 && a.TglAkhirBerlaku <= tglAkhir.Date
                                 && a.KelasJalan == kelasJalan
                                 && a.NamaJalan == namaJalan
                                 && b != null
                                 && a.TglAkhirBerlaku < DateTime.Now.Date
                             select new
                             {
                                 a.NoFormulir,
                                 a.KelasJalan,
                                 a.NamaJalan,
                                 a.PokokPajakKetetapan,
                                 a.Alamatreklame,
                                 a.IsiReklame,
                                 a.NmJenis,
                                 a.JenisProduk,
                                 STATUS = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date ? "EXPIRED" : "AKTIF",
                                 a.FlagPermohonan,
                                 a.TglMulaiBerlaku,
                                 a.TglAkhirBerlaku
                             }
                         ).ToList();
                        foreach (var item in permanenDataExpBongkar)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.STATUS,
                                TglMulai = item.TglMulaiBerlaku.Value,
                                TglSelesai = item.TglAkhirBerlaku.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }
                    }
                    if (status == "ExpiredBlmBongkar")
                    {
                        var permanenDataExpBlmBongkar = (
                           from a in context.DbMonReklames
                           join b in context.DbMonReklameUpayas.Where(b => b.Upaya == "PEMBONGKARAN")
                               on a.NoFormulir equals b.NoFormulir into gj
                           from b in gj.DefaultIfEmpty()
                           where a.FlagPermohonan == "PERMANEN"
                               && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
                               && a.TglAkhirBerlaku <= tglAkhir.Date
                               && a.KelasJalan == kelasJalan
                               && a.NamaJalan == namaJalan
                               && b == null
                               && a.TglAkhirBerlaku < DateTime.Now.Date
                           select new
                           {
                               a.NoFormulir,
                               a.KelasJalan,
                               a.NamaJalan,
                               a.Alamatreklame,
                               a.IsiReklame,
                               a.NmJenis,
                               a.JenisProduk,
                               a.PokokPajakKetetapan,
                               STATUS = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date ? "EXPIRED" : "AKTIF",
                               a.FlagPermohonan,
                               a.TglMulaiBerlaku,
                               a.TglAkhirBerlaku
                           }
                       ).ToList();
                        foreach (var item in permanenDataExpBlmBongkar)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.STATUS,
                                TglMulai = item.TglMulaiBerlaku.Value,
                                TglSelesai = item.TglAkhirBerlaku.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }
                    }
                    if (status == "Aktif")
                    {
                        var permanenDataAktif = (
                           from a in context.DbMonReklames
                           join b in context.DbMonReklameUpayas.Where(b => b.Upaya == "PEMBONGKARAN")
                               on a.NoFormulir equals b.NoFormulir into gj
                           from b in gj.DefaultIfEmpty()
                           where a.FlagPermohonan == "PERMANEN"
                               && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
                               && a.TglAkhirBerlaku <= tglAkhir.Date
                               && a.KelasJalan == kelasJalan
                               && a.NamaJalan == namaJalan
                               && b == null
                               && a.TglAkhirBerlaku >= DateTime.Now.Date
                           select new
                           {
                               a.NoFormulir,
                               a.KelasJalan,
                               a.NamaJalan,
                               a.Alamatreklame,
                               a.IsiReklame,
                               a.PokokPajakKetetapan,
                               a.NmJenis,
                               a.JenisProduk,
                               STATUS = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date ? "EXPIRED" : "AKTIF",
                               a.FlagPermohonan,
                               a.TglMulaiBerlaku,
                               a.TglAkhirBerlaku
                           }
                       ).ToList();
                        foreach (var item in permanenDataAktif)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.STATUS,
                                TglMulai = item.TglMulaiBerlaku.Value,
                                TglSelesai = item.TglAkhirBerlaku.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }
                    }
                }
                else if (jenisReklame == "TERBATAS")
                {
                    //terbatas
                    if (status == "ExpiredBongkar")
                    {
                        var permanenDataExpBongkar = (
                             from a in context.DbMonReklames
                             join b in context.DbMonReklameUpayas.Where(b => b.Upaya == "PEMBONGKARAN")
                                 on a.NoFormulir equals b.NoFormulir into gj
                             from b in gj.DefaultIfEmpty()
                             where a.FlagPermohonan == "TERBATAS"
                                 && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
                                 && a.TglAkhirBerlaku <= tglAkhir.Date
                                 && a.KelasJalan == kelasJalan
                                 && a.NamaJalan == namaJalan
                                 && b != null
                                 && a.TglAkhirBerlaku < DateTime.Now.Date
                             select new
                             {
                                 a.NoFormulir,
                                 a.KelasJalan,
                                 a.NamaJalan,
                                 a.Alamatreklame,
                                 a.IsiReklame,
                                 a.NmJenis,
                                 a.PokokPajakKetetapan,
                                 a.JenisProduk,
                                 STATUS = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date ? "EXPIRED" : "AKTIF",
                                 a.FlagPermohonan,
                                 a.TglMulaiBerlaku,
                                 a.TglAkhirBerlaku
                             }
                         ).ToList();
                        foreach (var item in permanenDataExpBongkar)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.STATUS,
                                TglMulai = item.TglMulaiBerlaku.Value,
                                TglSelesai = item.TglAkhirBerlaku.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }
                    }
                    if (status == "ExpiredBlmBongkar")
                    {
                        var permanenDataExpBlmBongkar = (
                           from a in context.DbMonReklames
                           join b in context.DbMonReklameUpayas.Where(b => b.Upaya == "PEMBONGKARAN")
                               on a.NoFormulir equals b.NoFormulir into gj
                           from b in gj.DefaultIfEmpty()
                           where a.FlagPermohonan == "TERBATAS"
                               && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
                               && a.TglAkhirBerlaku <= tglAkhir.Date
                               && a.KelasJalan == kelasJalan
                               && a.NamaJalan == namaJalan
                               && b == null
                               && a.TglAkhirBerlaku < DateTime.Now.Date
                           select new
                           {
                               a.NoFormulir,
                               a.KelasJalan,
                               a.NamaJalan,
                               a.Alamatreklame,
                               a.IsiReklame,
                               a.PokokPajakKetetapan,
                               a.NmJenis,
                               a.JenisProduk,
                               STATUS = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date ? "EXPIRED" : "AKTIF",
                               a.FlagPermohonan,
                               a.TglMulaiBerlaku,
                               a.TglAkhirBerlaku
                           }
                        ).ToList();
                        foreach (var item in permanenDataExpBlmBongkar)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.STATUS,
                                TglMulai = item.TglMulaiBerlaku.Value,
                                TglSelesai = item.TglAkhirBerlaku.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }
                    }
                    if (status == "Aktif")
                    {
                        var permanenDataAktif = (
                           from a in context.DbMonReklames
                           join b in context.DbMonReklameUpayas.Where(b => b.Upaya == "PEMBONGKARAN")
                               on a.NoFormulir equals b.NoFormulir into gj
                           from b in gj.DefaultIfEmpty()
                           where a.FlagPermohonan == "TERBATAS"
                               && a.TglAkhirBerlaku.Value.Date >= tglAwal.Date
                               && a.TglAkhirBerlaku <= tglAkhir.Date
                               && a.KelasJalan == kelasJalan
                               && a.NamaJalan == namaJalan
                               && b == null
                               && a.TglAkhirBerlaku >= DateTime.Now.Date
                           select new
                           {
                               a.NoFormulir,
                               a.KelasJalan,
                               a.NamaJalan,
                               a.Alamatreklame,
                               a.IsiReklame,
                               a.PokokPajakKetetapan,
                               a.NmJenis,
                               a.JenisProduk,
                               STATUS = a.TglAkhirBerlaku.Value.Date < DateTime.Now.Date ? "EXPIRED" : "AKTIF",
                               a.FlagPermohonan,
                               a.TglMulaiBerlaku,
                               a.TglAkhirBerlaku
                           }
                        ).ToList();
                        foreach (var item in permanenDataAktif)
                        {
                            ret.Add(new DetailData()
                            {
                                AlamatReklame = item.Alamatreklame,
                                Kategori = "-",
                                JenisReklame = item.NmJenis,
                                IsiReklame = item.IsiReklame,
                                KategoriReklame = item.JenisProduk,
                                KelasJalan = item.KelasJalan,
                                NamaJalan = item.NamaJalan,
                                Status = item.STATUS,
                                TglMulai = item.TglMulaiBerlaku.Value,
                                TglSelesai = item.TglAkhirBerlaku.Value,
                                Pajak = item.PokokPajakKetetapan.Value
                            });
                        }
                    }
                }

                return ret;
            }
        }
    }
    // Model BARU untuk tabel ringkasan
    public class ReklamePerJalan
    {
        // Properti Kunci
        public string NamaJalan { get; set; } = null!;
        public string KelasJalan { get; set; } = null!; // Kelas Jalan diganti KelasJalan agar sesuai data

        // Properti untuk grup INSIDENTIL
        public int InsidentilBongkar { get; set; }
        public int InsidentilBelumBongkar { get; set; }
        public int InsidentilAktif { get; set; }
        public int InsidentilJumlah => InsidentilBongkar + InsidentilBelumBongkar + InsidentilAktif;

        // Properti untuk grup JENIS PERMANENT
        public int PermanenBongkar { get; set; }
        public int PermanenBelumBongkar { get; set; }
        public int PermanenAktif { get; set; }
        public int PermanenJumlah => PermanenBongkar + PermanenBelumBongkar + PermanenAktif;

        // Properti untuk grup TERBATAS
        public int TerbatasBongkar { get; set; }
        public int TerbatasBelumBongkar { get; set; }
        public int TerbatasAktif { get; set; }
        public int TerbatasJumlah => TerbatasBongkar + TerbatasBelumBongkar + TerbatasAktif;
    }

    // Model untuk setiap baris data reklame detail (yang sudah ada)
    public class DataReklame
    {
        public string TitikLokasi { get; set; } = null!;
        public string Jenis { get; set; } = null!;
        public string Ukuran { get; set; } = null!;
        public string Penyelenggara { get; set; } = null!;
        public string MasaBerlaku { get; set; } = null!;
    }

    public class DashboardData
    {
        public int TotalReklame { get; set; }
        public int JalanDenganReklame { get; set; }
        public int PelanggaranTerdeteksi { get; set; }
    }

    public class RekapData
    {
        public string KelasJalanId { get; set; } = null!;
        public string KelasJalan { get; set; } = null!;
        public string NamaJalan { get; set; } = null!;

        public KategoriReklame Insidentil { get; set; } = new();
        public KategoriReklame Permanen { get; set; } = new();
        public KategoriReklame Terbatas { get; set; } = new();
    }

    public class KategoriReklame
    {
        public string JenisReklame { get; set; }
        public decimal Aktif { get; set; }
        public decimal Expired { get; set; }
        public decimal Perpanjangan { get; set; }
        public decimal WajibBongkar { get; set; }
        public decimal Bongkar { get; set; }
        public decimal BlmBongkar { get; set; }
        public decimal Jumlah => Aktif + Expired + Perpanjangan + WajibBongkar + Bongkar + BlmBongkar;
    }

    public class DetailData
    {
        public string KelasJalan { get; set; } = null!;
        public string NamaJalan { get; set; } = null!;
        public string AlamatReklame { get; set; } = null!;
        public string IsiReklame { get; set; } = null!;
        public string JenisReklame { get; set; } = null!;
        public string Kategori { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string KategoriReklame { get; set; } = null!;
        public string SkSilang { get; set; } = null!;
        public string SkBongkar { get; set; } = null!;
        public string SkBantip { get; set; } = null!;
        public DateTime TglMulai { get; set; }
        public DateTime TglSelesai { get; set; }
        public int SisaHari =>
        (TglSelesai.Date - DateTime.Now.Date).Days >= 0
            ? (TglSelesai.Date - DateTime.Now.Date).Days
            : 0;
        public string Ukuran { get; set; } = null!;
        public decimal Pajak { get; set; }
        public decimal Jumlah { get; set; }
    }

}
