using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using System.Globalization;

namespace MonPDReborn.Models.AktivitasOP
{
    public class ReklameSummaryVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }

        }

        public class Show
        {
            public List<ReklamePermanen> ReklamePermanenList { get; set; } = new();
            public List<TerbatasReklame> TerbatasReklameList { get; set; } = new();
            public List<IsidentilReklame> IsidentilReklameList { get; set; } = new();
            public int Tahun { get; set; }
            public Show() { }

            public Show(int tahun)
            {
                Tahun = tahun;
                ReklamePermanenList = Method.GetReklamePermanen(tahun);
                TerbatasReklameList = Method.GetTerbatasReklame(tahun);
                IsidentilReklameList = Method.GetIsidentilReklame(tahun);
            }
        }

        //Detail Reklame Permanen
        public class DetailPermanenJT
        {
            public int Tahun { get; set; }
            public string? Bulan { get; set; }
            public int? SkpdBlmJT { get; set; }

            public List<PermanenJT> Data { get; set; } = new();

            public DetailPermanenJT() { }

            public DetailPermanenJT(int tahun, string? bulan = null, int? skpdBlmJT = null)
            {
                Tahun = tahun;
                Bulan = bulan;
                SkpdBlmJT = skpdBlmJT;

                // Panggil GetPermanenJT
                Data = Method.GetPermanenJT(tahun, bulan, skpdBlmJT);
            }
        }

        public class DetailPermanenBP
        {
            public int Tahun { get; set; }
            public string? Bulan { get; set; }
            public int? SkpdBlmPanjang { get; set; }

            public List<PermanenBP> Data { get; set; } = new();

            public DetailPermanenBP() { }

            public DetailPermanenBP(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
            {
                Tahun = tahun;
                Bulan = bulan;
                SkpdBlmPanjang = skpdBlmPanjang;

                // Panggil GetPermanenJT
                Data = Method.GetPermanenBP(tahun, bulan, skpdBlmPanjang);
            }
        }

        public class DetailPermanenKB
        {
            public int Tahun { get; set; }
            public string? Bulan { get; set; }
            public int? SkpdBlmKB { get; set; }

            public List<PermanenKB> Data { get; set; } = new();

            public DetailPermanenKB() { }

            public DetailPermanenKB(int tahun, string? bulan = null, int? skpdKB = null)
            {
                Tahun = tahun;
                Bulan = bulan;
                SkpdBlmKB = skpdKB;

                // Panggil GetPermanenJT
                Data = Method.GetPermanenKB(tahun, bulan, skpdKB);
            }
        }

        //Detail Reklame Terbatas
        public class DetailTerbatasJT
        {
            public int Tahun { get; set; }
            public string? Bulan { get; set; }
            public int? SkpdBlmJT { get; set; }

            public List<TerbatasJT> Data { get; set; } = new();

            public DetailTerbatasJT() { }

            public DetailTerbatasJT(int tahun, string? bulan = null, int? skpdBlmJT = null)
            {
                Tahun = tahun;
                Bulan = bulan;
                SkpdBlmJT = skpdBlmJT;

                // Panggil GetTerbatasJT
                Data = Method.GetTerbatasJT(tahun, bulan, skpdBlmJT);
            }
        }

        public class DetailTerbatasBP
        {
            public int Tahun { get; set; }
            public string? Bulan { get; set; }
            public int? SkpdBlmPanjang { get; set; }

            public List<TerbatasBP> Data { get; set; } = new();

            public DetailTerbatasBP() { }

            public DetailTerbatasBP(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
            {
                Tahun = tahun;
                Bulan = bulan;
                SkpdBlmPanjang = skpdBlmPanjang;

                // Panggil GetPermanenJT
                Data = Method.GetTerbatasBP(tahun, bulan, skpdBlmPanjang);
            }
        }

        public class DetailTerbatasKB
        {
            public int Tahun { get; set; }
            public string? Bulan { get; set; }
            public int? SkpdBlmKB { get; set; }

            public List<TerbatasKB> Data { get; set; } = new();

            public DetailTerbatasKB() { }

            public DetailTerbatasKB(int tahun, string? bulan = null, int? skpdKB = null)
            {
                Tahun = tahun;
                Bulan = bulan;
                SkpdBlmKB = skpdKB;

                // Panggil GetTerbatasJT
                Data = Method.GetTerbatasKB(tahun, bulan, skpdKB);
            }
        }

        //Detail Isidentil
        public class DetailIsidentilKB
        {
            public int Tahun { get; set; }
            public string? Bulan { get; set; }
            public int? SkpdBlmKB { get; set; }

            public List<IsidentilKB> Data { get; set; } = new();

            public DetailIsidentilKB() { }

            public DetailIsidentilKB(int tahun, string? bulan = null, int? skpdKB = null)
            {
                Tahun = tahun;
                Bulan = bulan;
                SkpdBlmKB = skpdKB;

                // Panggil GetTerbatasJT
                Data = Method.GetIsidentilKB(tahun, bulan, skpdKB);
            }
        }

        // Detail Upaya

        public class Detail
        {
            public string? NoFormulir { get; set; }

            public List<DetailUpaya> Data { get; set; } = new();

            public Detail() { }

            public Detail(string? noFormulir)
            {
                NoFormulir = noFormulir;

                // panggil GetDetailUpaya
                Data = Method.GetDetailUpaya(noFormulir ?? string.Empty);
            }
        }

        public class Method
        {
            public static List<ReklamePermanen> GetReklamePermanen(int tahun)
            {
                var ret = new List<ReklamePermanen>();
                var context = DBClass.GetContext();


                var dataReklame = context.DbMonReklames
                    .Where(x => x.FlagPermohonan == "PERMANEN")
                    .ToList();

                var dataReklamePanjang = dataReklame.Where(x => x.NoFormulirLama != null).ToList();

                for (int bulan = 1; bulan <= 12; bulan++)
                {
                    var jmlHari = DateTime.DaysInMonth(tahun, bulan);
                    var tglServer = new DateTime(tahun, bulan, jmlHari);
                    if (tglServer.Year == DateTime.Now.Year && tglServer.Month == DateTime.Now.Month)
                    {
                        tglServer = new DateTime(tahun, bulan, DateTime.Now.Day);
                    }

                    ret.Add(new ReklamePermanen
                    {
                        Bulan = new DateTime(2025, bulan, 1).ToString("MMMM", new CultureInfo("id-ID")),
                        Tahun = tahun,
                        SKPDJT = dataReklame.Where(x => x.TglKetetapan <= DateTime.Now && x.TglAkhirBerlaku.HasValue && x.TglAkhirBerlaku.Value > tglServer && x.TglAkhirBerlaku.Value.Month == tglServer.Month).Count(),
                        NilaiJT = dataReklame.Where(x => x.TglKetetapan <= DateTime.Now && x.TglAkhirBerlaku.HasValue && x.TglAkhirBerlaku.Value > tglServer && x.TglAkhirBerlaku.Value.Month == tglServer.Month).Sum(q => q.Nilaipajak) ?? 0,
                        SKPDBlmJT = dataReklame.Where(x => x.TglKetetapan <= DateTime.Now && x.TglAkhirBerlaku.HasValue && x.TglAkhirBerlaku.Value > tglServer && x.TglAkhirBerlaku.Value.Month == tglServer.Month && (!x.TglBayarPokok.HasValue)).Count(),
                        NilaiBlmJT = dataReklame.Where(x => x.TglKetetapan <= DateTime.Now && x.TglAkhirBerlaku.HasValue && x.TglAkhirBerlaku.Value > tglServer && x.TglAkhirBerlaku.Value.Month == tglServer.Month && (!x.TglBayarPokok.HasValue)).Sum(q => q.Nilaipajak) ?? 0,
                        SKPDPanjang = dataReklamePanjang.Where(x => x.TglMulaiBerlaku.Value.Month == bulan && (!x.TglJtempoSkpd.HasValue)).Count(),
                        NilaiPanjang = dataReklamePanjang.Where(x => x.TglMulaiBerlaku.Value.Month == bulan).Sum(q => q.Nilaipajak) ?? 0,
                        //SKPDBlmPanjang
                        //NilaiBlmPanjang
                        //SKPDKB
                        //NilaiKB
                        //SKPDBlmKB
                        //NilaiBlmKB  
                        //Bulan
                        //Tahun
                        //SKPDJT  //jumlah objek yang tgl akhir berlakunya ada di tahun 2025 dengan pengkategorian berdasarkan bulan tgl akhir berlaku
                        //NilaiJT  //nilai ketetapan berdasarkan bulan tgl akhir berlaku
                        //SKPDBlmJT //tidak ada tanggal bayar... dan masih belum jatuh tempo
                        //NilaiBlmJT //nilai tidak ada tanggal bayar... dan masih belum jatuh tempo
                        //SKPDPanjang //skpd baru di bulan itu dengan no formulir lama yang tanggal akhir berlakunya = yang pertama tadi
                        //NilaiPanjang //nilai ketetapan berdasarkan bulan tgl akhir berlaku
                        //SKPDBlmPanjang //tidak ada tanggal bayar... dan masih belum jatuh tempo
                        //NilaiBlmPanjang //nilai tidak ada tanggal bayar... dan masih belum jatuh tempo
                        //SKPDKB //skpd baru di bulan itu dengan no formulir lama yang tanggal akhir berlakunya = yang pertama tadi
                        //NilaiKB //nilai ketetapan berdasarkan bulan tgl akhir berlaku
                        //SKPDBlmKB //tidak ada tanggal bayar... dan masih belum jatuh tempo
                        //NilaiBlmKB  //nilai tidak ada tanggal bayar... dan masih belum jatuh tempo
                    });
                }

                return ret;

            }

            public static List<TerbatasReklame> GetTerbatasReklame(int tahun)
            {
                var ret = new List<TerbatasReklame>();
                return ret;
            }

            public static List<IsidentilReklame> GetIsidentilReklame(int tahun)
            {
                var ret = new List<IsidentilReklame>();
                var context = DBClass.GetContext();

                //var dataIns = context.reklamesum

                return ret;
            }
            // Detail Reklame Permanen
            public static List<PermanenJT> GetPermanenJT(int tahun, string? bulan = null, int? skpdBlmJT = null)
            {
                var all = GetAllPermanenJT(); // ambil semua data dummy misalnya

                var query = all.Where(x => x.Tahun == tahun);

                if (!string.IsNullOrWhiteSpace(bulan))
                {
                    query = query.Where(x => string.Equals(x.Bulan, bulan, StringComparison.OrdinalIgnoreCase));
                }

                if (skpdBlmJT.HasValue)
                {
                    query = query.Where(x => x.SKPDBlmJT == skpdBlmJT.Value);
                }

                return query.ToList();
            }

            public static List<PermanenJT> GetAllPermanenJT()
            {
                return new List<PermanenJT>
                {
                    new PermanenJT { Bulan = "Januari", Tahun = 2025, SKPDBlmJT = 5, NoFormulir = "FM-2025-0001", Nama = "PT Reklame Jaya", AlamatOP = "Jl. Merdeka No. 123", IsiReklame = "Promo Awal Tahun", Status = "Belum Lunas", TahunPajak = new DateTime(2025, 1, 1), JumlahNilai = 15000000m, Email = "contact@reklamejaya.co.id", JmlUpaya = 1 },
                };
            }

            public static List<PermanenBP> GetPermanenBP(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
            {
                var all = GetAllPermanenBP(); // ambil semua data dummy misalnya

                var query = all.Where(x => x.Tahun == tahun);

                if (!string.IsNullOrWhiteSpace(bulan))
                {
                    query = query.Where(x => string.Equals(x.Bulan, bulan, StringComparison.OrdinalIgnoreCase));
                }

                if (skpdBlmPanjang.HasValue)
                {
                    query = query.Where(x => x.SKPDBlmPanjang == skpdBlmPanjang.Value);
                }

                return query.ToList();
            }

            public static List<PermanenBP> GetAllPermanenBP()
            {
                return new List<PermanenBP>
                {
                    new PermanenBP { Bulan = "Januari", Tahun = 2025, SKPDBlmPanjang = 4, NoFormulir = "FM-2025-0001", Nama = "PT Reklame Jaya", AlamatOP = "Jl. Merdeka No. 123", IsiReklame = "Promo Awal Tahun", Status = "Belum Lunas", TahunPajak = new DateTime(2025, 1, 1), JumlahNilai = 15000000m, JmlUpaya = 1 },
                };
            }

            public static List<PermanenKB> GetPermanenKB(int tahun, string? bulan = null, int? skpdKB = null)
            {
                var all = GetAllPermanenKB(); // ambil semua data dummy misalnya

                var query = all.Where(x => x.Tahun == tahun);

                if (!string.IsNullOrWhiteSpace(bulan))
                {
                    query = query.Where(x => string.Equals(x.Bulan, bulan, StringComparison.OrdinalIgnoreCase));
                }

                if (skpdKB.HasValue)
                {
                    query = query.Where(x => x.SKPDBlmKB == skpdKB.Value);
                }

                return query.ToList();
            }

            public static List<PermanenKB> GetAllPermanenKB()
            {
                return new List<PermanenKB>
                {
                    new PermanenKB { Bulan = "Januari", Tahun = 2025, SKPDBlmKB = 3, NoFormulir = "FM-2025-0001", Nama = "PT Reklame Jaya", AlamatOP = "Jl. Merdeka No. 123", IsiReklame = "Promo Awal Tahun", Status = "Belum Lunas", TahunPajak = new DateTime(2025, 1, 1), JumlahNilai = 15000000m, JmlUpaya = 1 },
                };
            }
            // DetailTerbatas Reklame
            public static List<TerbatasJT> GetTerbatasJT(int tahun, string? bulan = null, int? skpdBlmJT = null)
            {
                var all = GetAllTerbatasJT(); // ambil semua data dummy misalnya

                var query = all.Where(x => x.Tahun == tahun);

                if (!string.IsNullOrWhiteSpace(bulan))
                {
                    query = query.Where(x => string.Equals(x.Bulan, bulan, StringComparison.OrdinalIgnoreCase));
                }

                if (skpdBlmJT.HasValue)
                {
                    query = query.Where(x => x.SKPDBlmJT == skpdBlmJT.Value);
                }

                return query.ToList();
            }

            private static List<TerbatasJT> GetAllTerbatasJT()
            {
                return new List<TerbatasJT>
                {
                    new TerbatasJT {  Bulan = "Februari", Tahun = 2025, SKPDBlmJT = 6, NoFormulir = "FM-2025-0001", Nama = "PT Reklame Jaya", AlamatOP = "Jl. Merdeka No. 123", IsiReklame = "Promo Awal Tahun", Status = "Belum Lunas", TahunPajak = new DateTime(2025, 1, 1), JumlahNilai = 15000000m, Email = "contact@reklamejaya.co.id", JmlUpaya = 1 },
                };
            }

            public static List<TerbatasBP> GetTerbatasBP(int tahun, string? bulan = null, int? skpdBlmPanjang = null)
            {
                var all = GetAllTerbatasBP(); // ambil semua data dummy misalnya

                var query = all.Where(x => x.Tahun == tahun);

                if (!string.IsNullOrWhiteSpace(bulan))
                {
                    query = query.Where(x => string.Equals(x.Bulan, bulan, StringComparison.OrdinalIgnoreCase));
                }

                if (skpdBlmPanjang.HasValue)
                {
                    query = query.Where(x => x.SKPDBlmPanjang == skpdBlmPanjang.Value);
                }

                return query.ToList();
            }

            public static List<TerbatasBP> GetAllTerbatasBP()
            {
                return new List<TerbatasBP>
                {
                    new TerbatasBP { Bulan = "Februari", Tahun = 2025, SKPDBlmPanjang = 4, NoFormulir = "FM-2025-0001", Nama = "PT Reklame Jaya", AlamatOP = "Jl. Merdeka No. 123", IsiReklame = "Promo Awal Tahun", Status = "Belum Lunas", TahunPajak = new DateTime(2025, 1, 1), JumlahNilai = 15000000m, JmlUpaya = 1 },
                };
            }

            public static List<TerbatasKB> GetTerbatasKB(int tahun, string? bulan = null, int? skpdKB = null)
            {
                var all = GetAllTerbatasKB(); // ambil semua data dummy misalnya

                var query = all.Where(x => x.Tahun == tahun);

                if (!string.IsNullOrWhiteSpace(bulan))
                {
                    query = query.Where(x => string.Equals(x.Bulan, bulan, StringComparison.OrdinalIgnoreCase));
                }

                if (skpdKB.HasValue)
                {
                    query = query.Where(x => x.SKPDBlmKB == skpdKB.Value);
                }

                return query.ToList();
            }

            public static List<TerbatasKB> GetAllTerbatasKB()
            {
                return new List<TerbatasKB>
                {
                    new TerbatasKB { Bulan = "Februari", Tahun = 2025, SKPDBlmKB = 3, NoFormulir = "FM-2025-0001", Nama = "PT Reklame Jaya", AlamatOP = "Jl. Merdeka No. 123", IsiReklame = "Promo Awal Tahun", Status = "Belum Lunas", TahunPajak = new DateTime(2025, 1, 1), JumlahNilai = 15000000m, JmlUpaya = 1 },
                };
            }

            // Isidentil Reklame
            public static List<IsidentilKB> GetIsidentilKB(int tahun, string? bulan = null, int? skpdKB = null)
            {
                var all = GetAllIsidentilKB(); // ambil semua data dummy misalnya

                var query = all.Where(x => x.Tahun == tahun);

                if (!string.IsNullOrWhiteSpace(bulan))
                {
                    query = query.Where(x => string.Equals(x.Bulan, bulan, StringComparison.OrdinalIgnoreCase));
                }

                if (skpdKB.HasValue)
                {
                    query = query.Where(x => x.SKPDBlmKB == skpdKB.Value);
                }

                return query.ToList();
            }

            public static List<IsidentilKB> GetAllIsidentilKB()
            {
                return new List<IsidentilKB>
                {
                    new IsidentilKB { Bulan = "Maret", Tahun = 2025, SKPDBlmKB = 4, NoFormulir = "FM-2025-0021", Nama = "PT Reklame Jaya", AlamatOP = "Jl. Merdeka No. 123", IsiReklame = "Promo Awal Tahun", Status = "Belum Lunas", TahunPajak = new DateTime(2025, 1, 1), JumlahNilai = 15000000m, JmlUpaya = 1 },
                };
            }

            // Detail Upaya
            public static List<DetailUpaya> GetDetailUpaya(string noFormulir)
            {
                var all = GetAllDetailUpaya(); // ambil semua data dummy misalnya

                if (string.IsNullOrWhiteSpace(noFormulir))
                    return all;

                return all
                    .Where(x => x.NoFormulir != null && x.NoFormulir.Contains(noFormulir, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<DetailUpaya> GetAllDetailUpaya()
            {
                return new List<DetailUpaya>
                {
                     new DetailUpaya {Tahun = 2025, NoFormulir = "FM-2025-0001", AlamatReklame = "Jl. Merdeka No. 10", JenisReklame = "Billboard", Panjang = 5, Lebar = 3, Luas = 15, Tinggi = 4, TglMulai = new DateTime(2025, 1, 1), TglSelesai = new DateTime(2025, 12, 31), TglUpaya = DateTime.Now, Upaya = "Peringatan Tertulis", Keterangan = "Pemasangan tanpa izin", Petugas = "Budi Santoso"},
                };
            }


        }

        public class ReklamePermanen
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDJT { get; set; }
            public decimal NilaiJT { get; set; }
            public int SKPDBlmJT { get; set; }
            public decimal NilaiBlmJT { get; set; }
            public int SKPDPanjang { get; set; }
            public decimal NilaiPanjang { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public decimal NilaiBlmPanjang { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal Potensi => NilaiBlmJT + NilaiBlmPanjang + NilaiBlmKB;

        }

        public class TerbatasReklame
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDJT { get; set; }
            public decimal NilaiJT { get; set; }
            public int SKPDBlmJT { get; set; }
            public decimal NilaiBlmJT { get; set; }
            public int SKPDPanjang { get; set; }
            public decimal NilaiPanjang { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public decimal NilaiBlmPanjang { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal Potensi => NilaiBlmJT + NilaiBlmPanjang + NilaiBlmKB;
        }

        public class IsidentilReklame
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal Potensi => NilaiBlmKB;
        }

        public class PermanenJT
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmJT { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public string Email { get; set; } = null!;
            public int JmlUpaya { get; set; }
        }
        public class PermanenBP
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public int JmlUpaya { get; set; }
        }

        public class PermanenKB
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmKB { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public string Email { get; set; } = null!;
            public int JmlUpaya { get; set; }
        }

        public class TerbatasJT
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmJT { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public string Email { get; set; } = null!;
            public int JmlUpaya { get; set; }
        }

        public class TerbatasBP
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public int JmlUpaya { get; set; }
        }

        public class TerbatasKB
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmKB { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public string Email { get; set; } = null!;
            public int JmlUpaya { get; set; }
        }

        public class IsidentilKB
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmKB { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public string Email { get; set; } = null!;
            public int JmlUpaya { get; set; }
        }

        public class DetailUpaya
        {
            public int Tahun { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string AlamatReklame { get; set; } = null!;
            public string JenisReklame { get; set; } = null!;
            public decimal Panjang { get; set; }
            public decimal Lebar { get; set; }
            public decimal Luas { get; set; }
            public decimal Tinggi { get; set; }
            public DateTime TglMulai { get; set; }
            public DateTime TglSelesai { get; set; }
            public DateTime TglUpaya { get; set; }
            public string Upaya { get; set; } = null!;
            public string Keterangan { get; set; } = null!;
            public string Petugas { get; set; } = null!;
        }
    }
}
