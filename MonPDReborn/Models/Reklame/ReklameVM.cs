using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonPDLib;
using SixLabors.Fonts.Tables.TrueType;
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
            public ShowData(Index input)
            {
                DataRekap = Method.GetRekapDataReklame(input.TglAwal, input.TglAkhir);
            }
        }

        public class DetailReklame
        {
            public List<DetailData> DataDetailList { get; set; } = new();

            public DetailReklame(string kelasJalan, string namaJalan, string status, DateTime tglAwal, DateTime tglAkhir)
            {
                DataDetailList = Method.GetDetailDataReklame(kelasJalan, namaJalan, status, tglAwal, tglAkhir);
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


            public static List<RekapData> GetRekapDataReklame(DateTime tglAwal, DateTime tglAkhir)
            {
                var ret = new List<RekapData>();
                var context = DBClass.GetContext();

                var insidentilData = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "INSIDENTIL" &&
                                r.TglAkhirBerlaku.HasValue &&
                                r.TglAkhirBerlaku.Value >= tglAwal &&
                                r.TglAkhirBerlaku.Value <= tglAkhir)
                    .Select(r => new
                    {
                        NoFormulir = r.NoFormulir,
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        UpayaId = context.TUpayaReklames
                            .Where(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                            .Select(t => (int?)t.Id)
                            .FirstOrDefault() ?? 0
                    })
                    .ToList()
                    .GroupBy(r => r.Status)
                    .Select(g => new
                    {
                        KelasJalan = g.First().KelasJalan,
                        NamaJalan = g.First().NamaJalan,
                        NoFormulir = g.First().NoFormulir,
                        Status = g.Key,
                        Jml = g.Count(),
                        UpayaId = g.First().UpayaId
                    })
                    .ToList();

                var permanenData = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "PERMANEN" &&
                                r.TglAkhirBerlaku.HasValue &&
                                r.TglAkhirBerlaku.Value.Date >= tglAwal.Date &&
                                r.TglAkhirBerlaku.Value.Date <= tglAkhir.Date)
                    .Select(r => new
                    {
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        r.NoFormulir,
                        Status = r.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                        UpayaId = context.TUpayaReklames
                            .Where(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                            .Select(t => (int?)t.Id ?? 0)
                            .FirstOrDefault()
                    })
                    .ToList();

                var terbatasData = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "TERBATAS" &&
                                r.TglAkhirBerlaku.HasValue &&
                                r.TglAkhirBerlaku.Value.Date >= tglAwal.Date &&
                                r.TglAkhirBerlaku.Value.Date <= tglAkhir.Date)
                    .Select(r => new
                    {
                        KelasJalanId = r.KelasJalan,
                        KelasJalan = string.Concat("Kelas ", r.KelasJalan),
                        NamaJalan = r.NamaJalan,
                        r.NoFormulir,
                        Status = r.TglAkhirBerlaku.Value.Date < DateTime.Today ? "EXPIRED" : "AKTIF",
                        UpayaId = context.TUpayaReklames
                            .Where(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                            .Select(t => (int?)t.Id ?? 0)
                            .FirstOrDefault()
                    })
                    .ToList();

                var allKeys = insidentilData
                    .Select(x => new { x.KelasJalan, x.NamaJalan })
                    .Concat(permanenData.Select(x => new { x.KelasJalan, x.NamaJalan }))
                    .Concat(terbatasData.Select(x => new { x.KelasJalan, x.NamaJalan }))
                    .Distinct()
                    .ToList();

                foreach (var key in allKeys)
                {
                    var insidentil = insidentilData
                        .Where(x => x.KelasJalan == key.KelasJalan && x.NamaJalan == key.NamaJalan)
                        .ToList();

                    var permanen = permanenData
                        .Where(x => x.KelasJalan == key.KelasJalan && x.NamaJalan == key.NamaJalan)
                        .ToList();

                    var terbatas = terbatasData
                        .Where(x => x.KelasJalan == key.KelasJalan && x.NamaJalan == key.NamaJalan)
                        .ToList();

                    var item = new RekapData
                    {
                        KelasJalanId = key.KelasJalan,
                        KelasJalan = "Kelas " + key.KelasJalan,
                        NamaJalan = key.NamaJalan,
                        Isidentil = new KategoriReklame
                        {
                            ExpiredBongkar = insidentil.Count(x => x.Status == "EXPIRED" && x.UpayaId != 0),
                            ExpiredBlmBongkar = insidentil.Count(x => x.Status == "EXPIRED" && x.UpayaId == 0),
                            Aktif = insidentil.Count(x => x.Status == "AKTIF")
                        },
                        Permanen = new KategoriReklame
                        {
                            ExpiredBongkar = permanen.Count(x => x.Status == "EXPIRED" && x.UpayaId != 0),
                            ExpiredBlmBongkar = permanen.Count(x => x.Status == "EXPIRED" && x.UpayaId == 0),
                            Aktif = permanen.Count(x => x.Status == "AKTIF")
                        },
                        Terbatas = new KategoriReklame
                        {
                            ExpiredBongkar = terbatas.Count(x => x.Status == "EXPIRED" && x.UpayaId == 1),
                            ExpiredBlmBongkar = terbatas.Count(x => x.Status == "EXPIRED" && x.UpayaId == 0),
                            Aktif = terbatas.Count(x => x.Status == "AKTIF")
                        }
                    };

                    ret.Add(item);
                }

                return ret;
            }

            public static List<DetailData> GetDetailDataReklame(string kelasJalan, string namaJalan, string status, DateTime tglAwal, DateTime tglAkhir)
            {
                var ret = new List<DetailData>();
                var context = DBClass.GetContext();

                //insidentil
                if (status == "ExpiredBongkar")
                {
                    var insidentilDataExpBongkar = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "INSIDENTIL" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                                r.TglAkhirBerlaku.HasValue && r.TglAkhirBerlaku.Value >= tglAwal &&
                                r.TglAkhirBerlaku.Value < DateTime.Today &&
                                context.TUpayaReklames
                                    .Any(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                                )
                    .Select(r => new DetailData
                    {
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        AlamatReklame = r.Alamat,
                        IsiReklame = r.IsiReklame,
                        JenisReklame = r.Jenis,
                        Kategori = r.KategoriKetetapan,
                        Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        KategoriReklame = r.FlagPermohonan,
                        TglMulai = r.TglMulaiBerlaku.Value,
                        TglSelesai = r.TglAkhirBerlaku.Value
                    })
                    .ToList();
                    ret.AddRange(insidentilDataExpBongkar);
                }
                if (status == "ExpiredBlmBongkar")
                {
                    var insidentilDataExpBlmBongkar = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "INSIDENTIL" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                                r.TglAkhirBerlaku.HasValue && r.TglAkhirBerlaku.Value >= tglAwal &&
                                r.TglAkhirBerlaku.Value < DateTime.Today &&
                                !context.TUpayaReklames.Any(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                                )
                    .Select(r => new DetailData
                    {
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        AlamatReklame = r.Alamat,
                        IsiReklame = r.IsiReklame,
                        JenisReklame = r.Jenis,
                        Kategori = r.KategoriKetetapan,
                        Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        KategoriReklame = r.FlagPermohonan,
                        TglMulai = r.TglMulaiBerlaku.Value,
                        TglSelesai = r.TglAkhirBerlaku.Value
                    })
                    .ToList();
                    ret.AddRange(insidentilDataExpBlmBongkar);
                }
                if (status == "Aktif")
                {
                    var insidentilDataAktif = context.DbMonReklames
                        .Where(r =>
                            r.FlagPermohonan == "INSIDENTIL" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                            r.TglAkhirBerlaku.HasValue &&
                            r.TglAkhirBerlaku.Value >= tglAwal &&
                            r.TglAkhirBerlaku.Value < DateTime.Today)
                        .Select(r => new DetailData
                        {
                            KelasJalan = r.KelasJalan,
                            NamaJalan = r.NamaJalan,
                            AlamatReklame = r.Alamat,
                            IsiReklame = r.IsiReklame,
                            JenisReklame = r.Jenis,
                            Kategori = r.KategoriKetetapan,
                            Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                            KategoriReklame = r.FlagPermohonan,
                            TglMulai = r.TglMulaiBerlaku.Value,
                            TglSelesai = r.TglAkhirBerlaku.Value
                        })
                        .ToList();
                    ret.AddRange(insidentilDataAktif);
                }
                //permanen
                if (status == "ExpiredBongkar")
                {
                    var permanenDataExpBongkar = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "PERMANEN" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                                r.TglAkhirBerlaku.HasValue && r.TglAkhirBerlaku.Value >= tglAwal &&
                                r.TglAkhirBerlaku.Value < DateTime.Today &&
                                context.TUpayaReklames
                                    .Any(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                                )
                    .Select(r => new DetailData
                    {
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        AlamatReklame = r.Alamat,
                        IsiReklame = r.IsiReklame,
                        JenisReklame = r.Jenis,
                        Kategori = r.KategoriKetetapan,
                        Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        KategoriReklame = r.FlagPermohonan,
                        TglMulai = r.TglMulaiBerlaku.Value,
                        TglSelesai = r.TglAkhirBerlaku.Value
                    })
                    .ToList();
                    ret.AddRange(permanenDataExpBongkar);
                }
                if (status == "ExpiredBlmBongkar")
                {
                    var permanenDataExpBlmBongkar = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "PERMANEN" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                                r.TglAkhirBerlaku.HasValue && r.TglAkhirBerlaku.Value >= tglAwal &&
                                r.TglAkhirBerlaku.Value < DateTime.Today &&
                                !context.TUpayaReklames.Any(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                                )
                    .Select(r => new DetailData
                    {
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        AlamatReklame = r.Alamat,
                        IsiReklame = r.IsiReklame,
                        JenisReklame = r.Jenis,
                        Kategori = r.KategoriKetetapan,
                        Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        KategoriReklame = r.FlagPermohonan,
                        TglMulai = r.TglMulaiBerlaku.Value,
                        TglSelesai = r.TglAkhirBerlaku.Value
                    })
                    .ToList();
                    ret.AddRange(permanenDataExpBlmBongkar);
                }
                if (status == "Aktif")
                {
                    var permanenDataAktif = context.DbMonReklames
                        .Where(r =>
                            r.FlagPermohonan == "PERMANEN" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                            r.TglAkhirBerlaku.HasValue &&
                            r.TglAkhirBerlaku.Value >= tglAwal &&
                            r.TglAkhirBerlaku.Value < DateTime.Today)
                        .Select(r => new DetailData
                        {
                            KelasJalan = r.KelasJalan,
                            NamaJalan = r.NamaJalan,
                            AlamatReklame = r.Alamat,
                            IsiReklame = r.IsiReklame,
                            JenisReklame = r.Jenis,
                            Kategori = r.KategoriKetetapan,
                            Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                            KategoriReklame = r.FlagPermohonan,
                            TglMulai = r.TglMulaiBerlaku.Value,
                            TglSelesai = r.TglAkhirBerlaku.Value
                        })
                        .ToList();
                    ret.AddRange(permanenDataAktif);
                }

                //terbatas
                if (status == "ExpiredBongkar")
                {
                    var terbatasDataExpBongkar = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "TERBATAS" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                                r.TglAkhirBerlaku.HasValue && r.TglAkhirBerlaku.Value >= tglAwal &&
                                r.TglAkhirBerlaku.Value < DateTime.Today &&
                                context.TUpayaReklames
                                    .Any(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                                )
                    .Select(r => new DetailData
                    {
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        AlamatReklame = r.Alamat,
                        IsiReklame = r.IsiReklame,
                        JenisReklame = r.Jenis,
                        Kategori = r.KategoriKetetapan,
                        Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        KategoriReklame = r.FlagPermohonan,
                        TglMulai = r.TglMulaiBerlaku.Value,
                        TglSelesai = r.TglAkhirBerlaku.Value
                    })
                    .ToList();
                    ret.AddRange(terbatasDataExpBongkar);
                }
                if (status == "ExpiredBlmBongkar")
                {
                    var terbatasDataExpBlmBongkar = context.DbMonReklames
                    .Where(r => r.FlagPermohonan == "TERBATAS" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                                r.TglAkhirBerlaku.HasValue && r.TglAkhirBerlaku.Value >= tglAwal &&
                                r.TglAkhirBerlaku.Value < DateTime.Today &&
                                !context.TUpayaReklames.Any(t => t.NoFormulir == r.NoFormulir && t.IdUpaya == 2)
                                )
                    .Select(r => new DetailData
                    {
                        KelasJalan = r.KelasJalan,
                        NamaJalan = r.NamaJalan,
                        AlamatReklame = r.Alamat,
                        IsiReklame = r.IsiReklame,
                        JenisReklame = r.Jenis,
                        Kategori = r.KategoriKetetapan,
                        Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                        KategoriReklame = r.FlagPermohonan,
                        TglMulai = r.TglMulaiBerlaku.Value,
                        TglSelesai = r.TglAkhirBerlaku.Value
                    })
                    .ToList();
                    ret.AddRange(terbatasDataExpBlmBongkar);
                }
                if (status == "Aktif")
                {
                    var terbatasDataAktif = context.DbMonReklames
                        .Where(r =>
                            r.FlagPermohonan == "TERBATAS" &&
                                r.KelasJalan == kelasJalan &&
                                r.NamaJalan == namaJalan &&
                            r.TglAkhirBerlaku.HasValue &&
                            r.TglAkhirBerlaku.Value >= tglAwal &&
                            r.TglAkhirBerlaku.Value < DateTime.Today)
                        .Select(r => new DetailData
                        {
                            KelasJalan = r.KelasJalan,
                            NamaJalan = r.NamaJalan,
                            AlamatReklame = r.Alamat,
                            IsiReklame = r.IsiReklame,
                            JenisReklame = r.Jenis,
                            Kategori = r.KategoriKetetapan,
                            Status = r.TglAkhirBerlaku.Value < DateTime.Today ? "EXPIRED" : "AKTIF",
                            KategoriReklame = r.FlagPermohonan,
                            TglMulai = r.TglMulaiBerlaku.Value,
                            TglSelesai = r.TglAkhirBerlaku.Value
                        })
                        .ToList();
                    ret.AddRange(terbatasDataAktif);
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

        public KategoriReklame Isidentil { get; set; } = new();
        public KategoriReklame Permanen { get; set; } = new();
        public KategoriReklame Terbatas { get; set; } = new();
    }

    public class KategoriReklame
    {
        public int ExpiredBongkar { get; set; }
        public int ExpiredBlmBongkar { get; set; }
        public int Aktif { get; set; }
        public int Jumlah => ExpiredBongkar + ExpiredBlmBongkar + Aktif;
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
    }

}
