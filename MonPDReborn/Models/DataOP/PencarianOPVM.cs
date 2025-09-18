using DocumentFormat.OpenXml.Presentation;
using MonPDLib;
using MonPDLib.General;
using System.Globalization;
using static MonPDReborn.Models.DashboardUPTBVM.ViewModel;
using static MonPDReborn.Models.DashboardVM.ViewModel;

namespace MonPDReborn.Models.DataOP
{
    public class PencarianOPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }
            public Index(string keyword)
            {
                Keyword = keyword;
            }
        }
        public class Show
        {
            public List<DataPencarianOp> DataPencarianOpList { get; set; } = new();
            public Show()
            {

            }
            public Show(string keyword)
            {
                DataPencarianOpList = Method.GetDataPencarianOpList(keyword);
            }
        }
        public class Detail
        {
            public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();
            public List<RealisasiBulanan> DataRealisasiBulananMines1List { get; set; } = new();
            public Detail()
            {

            }
            public Detail(string nop, EnumFactory.EPajak pajak)
            {
                DataRealisasiBulananList = Method.GetRealisasiDetail(nop, pajak);
                DataRealisasiBulananMines1List = Method.GetRealisasiDetailMines1(nop, pajak);
            }
        }
        public class Method
        {
            public static List<DataPencarianOp> GetDataPencarianOpList(string keyword)
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    throw new ArgumentException("Keyword harus diisi");
                }
                if (keyword.Length < 3)
                {
                    throw new ArgumentException("Keyword harus diisi minimal 3");
                }
                if (keyword.Length == 24 || keyword.Contains("."))
                {
                    keyword = keyword.Replace(".", "");
                }
                var context = DBClass.GetContext();
                var ret = new List<DataPencarianOp>();
                var alatRekam = context.DbMonAlatRekams
                    .Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper())) || (x.AlamatOp.ToUpper().Contains(keyword.ToUpper())))
                    .Select(x => new
                    {
                        Nop = x.Nop,
                        JenisPajak = x.PajakId,
                        jenisAlat = x.JenisAlat,
                        tgl = x.Tgl,
                        bln = x.Bln,
                        thn = x.Tahun,
                        jam = x.Jam,
                        online = x.StatusOnline,
                        kunci = x.StatusKunci
                    })
                    .ToList();

                /*var dbrekapTs = context.DbRekamAlatGabungs
                    .Select(x => new
                    {
                        Nop = x.Nop,
                        JenisPajak = x.PajakId,
                        IsTs = x.IsTs,
                        IsTb = x.IsTb,
                        IsSb = x.IsSb
                    })
                    .ToList();*/

                var dataResto = context.DbOpRestos
                    .Where(x => (x.Npwpd == keyword) || (x.NpwpdNama == keyword) || (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper())) || (x.AlamatOp.ToUpper().Contains(keyword.ToUpper())))
                    .OrderByDescending(x => x.TahunBuku)
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .Select(g =>
                    {
                        var bukaNow = g.FirstOrDefault(x => x.TahunBuku == DateTime.Now.Year && x.TglOpTutup == null);
                        if (bukaNow != null)
                            return bukaNow;

                        return g.FirstOrDefault(x => x.TglOpTutup != null);
                    })
                    .Where(x => x != null)
                    .Select(x =>
                    {
                        // Ambil alat rekam terbaru untuk NOP ini
                        var alat = alatRekam
                            .Where(y => y.Nop == x.Nop)
                            .OrderByDescending(y => y.thn)
                            .ThenByDescending(y => y.bln)
                            .ThenByDescending(y => y.tgl)
                            .ThenByDescending(y => y.jam)
                            .FirstOrDefault();

                        // Hitung TerakhirAktif
                        string terakhirAktif;
                        if (alat != null)
                        {
                            var jam = string.IsNullOrWhiteSpace(alat.jam) ? "00:00:00" : alat.jam;
                            if (!TimeSpan.TryParse(jam, out var ts)) ts = TimeSpan.Zero;

                            if (alat.thn > 0 && alat.bln > 0 && alat.tgl > 0)
                            {
                                terakhirAktif = new DateTime((int)alat.thn, (int)alat.bln, (int)alat.tgl)
                                    .Add(ts)
                                    .ToString("dd-MM-yyyy HH:mm:ss");
                            }
                            else
                            {
                                terakhirAktif = "Tidak Ada Data";
                            }
                        }
                        else
                        {
                            terakhirAktif = "Offline";
                        }

                        return new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = alat != null
                                ? (alat.jenisAlat == "TS" ? "Tax Surveillance"
                                    : alat.jenisAlat == "TB" ? "Tapping Box"
                                    : alat.jenisAlat == "SB" ? "Sinkron Box"
                                    : "Belum Terpasang")
                                : "Tidak Terpasang",
                            TerakhirAktif = terakhirAktif,
                            StatusKunci = alat != null ? (alat.kunci == 1 ? "Terkunci" : "Tidak Terkunci") : "Tidak Ada Data",
                            StatusOnline = alat != null ? (alat.online == 1 ? "Online" : "Offline") : "Tidak Ada Data",
                            StatusNOP = x.TglOpTutup != null ? "Tutup" : "Buka",
                            Wilayah = "SURABAYA " + (x.WilayahPajak ?? ""),
                            EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                            Tahun = (int)x.TahunBuku
                        };
                    }).ToList();

                if (dataResto.Count > 0)
                {
                    ret.AddRange(dataResto);
                }

                var dataHotel = context.DbOpHotels
                    .Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper())) || (x.AlamatOp.ToUpper().Contains(keyword.ToUpper())))
                    .OrderByDescending(x => x.TahunBuku)
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .Select(g =>
                    {
                        var bukaNow = g.FirstOrDefault(x => x.TahunBuku == DateTime.Now.Year && x.TglOpTutup == null);
                        if (bukaNow != null)
                            return bukaNow;

                        return g.FirstOrDefault(x => x.TglOpTutup != null);
                    })
                    .Where(x => x != null)
                    .Select(x =>
                    {
                        // Ambil alat rekam terbaru untuk NOP ini
                        var alat = alatRekam
                            .Where(y => y.Nop == x.Nop)
                            .OrderByDescending(y => y.thn)
                            .ThenByDescending(y => y.bln)
                            .ThenByDescending(y => y.tgl)
                            .ThenByDescending(y => y.jam)
                            .FirstOrDefault();

                        // Hitung TerakhirAktif
                        string terakhirAktif;
                        if (alat != null)
                        {
                            var jam = string.IsNullOrWhiteSpace(alat.jam) ? "00:00:00" : alat.jam;
                            if (!TimeSpan.TryParse(jam, out var ts)) ts = TimeSpan.Zero;

                            if (alat.thn > 0 && alat.bln > 0 && alat.tgl > 0)
                            {
                                terakhirAktif = new DateTime((int)alat.thn, (int)alat.bln, (int)alat.tgl)
                                    .Add(ts)
                                    .ToString("dd-MM-yyyy HH:mm:ss");
                            }
                            else
                            {
                                terakhirAktif = "Tidak Ada Data";
                            }
                        }
                        else
                        {
                            terakhirAktif = "Offline";
                        }

                        return new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = alat != null
                                ? (alat.jenisAlat == "TS" ? "Tax Surveillance"
                                    : alat.jenisAlat == "TB" ? "Tapping Box"
                                    : alat.jenisAlat == "SB" ? "Sinkron Box"
                                    : "Belum Terpasang")
                                : "Tidak Terpasang",
                            TerakhirAktif = terakhirAktif,
                            StatusNOP = x.TglOpTutup != null ? "Tutup" : "Buka",
                            StatusKunci = alat != null ? (alat.kunci == 1 ? "Terkunci" : "Tidak Terkunci") : "Tidak Ada Data",
                            StatusOnline = alat != null ? (alat.online == 1 ? "Online" : "Offline") : "Tidak Ada Data",
                            Wilayah = "SURABAYA " + (x.WilayahPajak ?? ""),
                            EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                            Tahun = (int)x.TahunBuku
                        };
                    }).ToList();

                if (dataHotel.Count > 0)
                {
                    ret.AddRange(dataHotel);
                }

                var dataHiburan = context.DbOpHiburans
                    .Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper())) || (x.AlamatOp.ToUpper().Contains(keyword.ToUpper())))
                    .OrderByDescending(x => x.TahunBuku)
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .Select(g =>
                    {
                        var bukaNow = g.FirstOrDefault(x => x.TahunBuku == DateTime.Now.Year && x.TglOpTutup == null);
                        if (bukaNow != null)
                            return bukaNow;

                        return g.FirstOrDefault(x => x.TglOpTutup != null);
                    })
                    .Where(x => x != null)
                    .Select(x =>
                    {
                        // Ambil alat rekam terbaru untuk NOP ini
                        var alat = alatRekam
                            .Where(y => y.Nop == x.Nop)
                            .OrderByDescending(y => y.thn)
                            .ThenByDescending(y => y.bln)
                            .ThenByDescending(y => y.tgl)
                            .ThenByDescending(y => y.jam)
                            .FirstOrDefault();

                        // Hitung TerakhirAktif
                        string terakhirAktif;
                        if (alat != null)
                        {
                            var jam = string.IsNullOrWhiteSpace(alat.jam) ? "00:00:00" : alat.jam;
                            if (!TimeSpan.TryParse(jam, out var ts)) ts = TimeSpan.Zero;

                            if (alat.thn > 0 && alat.bln > 0 && alat.tgl > 0)
                            {
                                terakhirAktif = new DateTime((int)alat.thn, (int)alat.bln, (int)alat.tgl)
                                    .Add(ts)
                                    .ToString("dd-MM-yyyy HH:mm:ss");
                            }
                            else
                            {
                                terakhirAktif = "Tidak Ada Data";
                            }
                        }
                        else
                        {
                            terakhirAktif = "Offline";
                        }

                        return new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = alat != null
                                ? (alat.jenisAlat == "TS" ? "Tax Surveillance"
                                    : alat.jenisAlat == "TB" ? "Tapping Box"
                                    : alat.jenisAlat == "SB" ? "Sinkron Box"
                                    : "Belum Terpasang")
                                : "Tidak Terpasang",
                            TerakhirAktif = terakhirAktif,
                            StatusKunci = alat != null ? (alat.kunci == 1 ? "Terkunci" : "Tidak Terkunci") : "Tidak Ada Data",
                            StatusOnline = alat != null ? (alat.online == 1 ? "Online" : "Offline") : "Tidak Ada Data",
                            StatusNOP = x.TglOpTutup != null ? "Tutup" : "Buka",
                            Wilayah = "SURABAYA " + (x.WilayahPajak ?? ""),
                            EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                            Tahun = (int)x.TahunBuku
                        };
                    }).ToList();

                if (dataHiburan.Count > 0)
                {
                    ret.AddRange(dataHiburan);
                }


                var dataParkir = context.DbOpParkirs
                    .Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper())) || (x.AlamatOp.ToUpper().Contains(keyword.ToUpper())))
                    .OrderByDescending(x => x.TahunBuku)
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .Select(g =>
                    {
                        var bukaNow = g.FirstOrDefault(x => x.TahunBuku == DateTime.Now.Year && x.TglOpTutup == null);
                        if (bukaNow != null)
                            return bukaNow;

                        return g.FirstOrDefault(x => x.TglOpTutup != null);
                    })
                    .Where(x => x != null)
                    .Select(x =>
                    {
                        // Ambil alat rekam terbaru untuk NOP ini
                        var alat = alatRekam
                            .Where(y => y.Nop == x.Nop)
                            .OrderByDescending(y => y.thn)
                            .ThenByDescending(y => y.bln)
                            .ThenByDescending(y => y.tgl)
                            .ThenByDescending(y => y.jam)
                            .FirstOrDefault();

                        // Hitung TerakhirAktif
                        string terakhirAktif;
                        if (alat != null)
                        {
                            var jam = string.IsNullOrWhiteSpace(alat.jam) ? "00:00:00" : alat.jam;
                            if (!TimeSpan.TryParse(jam, out var ts)) ts = TimeSpan.Zero;

                            if (alat.thn > 0 && alat.bln > 0 && alat.tgl > 0)
                            {
                                terakhirAktif = new DateTime((int)alat.thn, (int)alat.bln, (int)alat.tgl)
                                    .Add(ts)
                                    .ToString("dd-MM-yyyy HH:mm:ss");
                            }
                            else
                            {
                                terakhirAktif = "Tidak Ada Data";
                            }
                        }
                        else
                        {
                            terakhirAktif = "Offline";
                        }

                        return new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = EnumFactory.EPajak.JasaParkir.GetDescription(),
                            KategoriOp = x.KategoriNama,
                            JenisPenarikan = alat != null
                                ? (alat.jenisAlat == "TS" ? "Tax Surveillance"
                                    : alat.jenisAlat == "TB" ? "Tapping Box"
                                    : alat.jenisAlat == "SB" ? "Sinkron Box"
                                    : "Belum Terpasang")
                                : "Tidak Terpasang",
                            TerakhirAktif = terakhirAktif,
                            StatusKunci = alat != null ? (alat.kunci == 1 ? "Terkunci" : "Tidak Terkunci") : "Tidak Ada Data",
                            StatusOnline = alat != null ? (alat.online == 1 ? "Online" : "Offline") : "Tidak Ada Data",
                            StatusNOP = x.TglOpTutup != null ? "Tutup" : "Buka",
                            Wilayah = "SURABAYA " + (x.WilayahPajak ?? ""),
                            EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                            Tahun = (int)x.TahunBuku
                        };
                    }).ToList();

                if (dataParkir.Count > 0)
                {
                    ret.AddRange(dataParkir);
                }

                var dataListrik = context.DbOpListriks
                    .Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper())) || (x.AlamatOp.ToUpper().Contains(keyword.ToUpper())))
                    .OrderByDescending(x => x.TahunBuku)
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .Select(g =>
                    {
                        var bukaNow = g.FirstOrDefault(x => x.TahunBuku == DateTime.Now.Year && x.TglOpTutup == null);
                        if (bukaNow != null)
                            return bukaNow;

                        return g.FirstOrDefault(x => x.TglOpTutup != null);
                    })
                    .Where(x => x != null)
                    .Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                            KategoriOp = x.KategoriNama ?? "",
                            JenisPenarikan = "Tidak Ada",
                            StatusKunci = "Tidak Ada",
                            StatusOnline = "Tidak Ada",
                            StatusNOP = x.TglOpTutup != null ? "Tutup" : "Buka",
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "",
                            EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                            Tahun = (int)x.TahunBuku
                        }
                    ).ToList();

                if (dataListrik.Count > 0)
                {
                    ret.AddRange(dataListrik);
                }

                var dataReklame = context.DbOpReklames.Where(x => x.Nop == keyword || x.Nama.Contains(keyword))
                    .OrderByDescending(x => x.TahunBuku)
                    .Select(
                        x => new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.Nama,
                            Alamat = x.Alamat,
                            JenisOp = EnumFactory.EPajak.Reklame.GetDescription(),
                            KategoriOp = "Reklame",
                            JenisPenarikan = "Tidak Ada",
                            StatusKunci = "Tidak Ada",
                            StatusOnline = "Tidak Ada",
                            StatusNOP = "Buka",
                            Wilayah = "-",
                            EnumPajak = (int)EnumFactory.EPajak.Reklame,
                            Tahun = (int)DateTime.Now.Year
                        }
                    ).ToList();

                if (dataReklame.Count > 0)
                {
                    ret.AddRange(dataReklame);
                }

                var dataAbt = context.DbOpAbts
                    .Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper())) || (x.AlamatOp.ToUpper().Contains(keyword.ToUpper())))
                    .OrderByDescending(x => x.TahunBuku)
                    .AsEnumerable()
                    .GroupBy(x => x.Nop)
                    .Select(g =>
                    {
                        var bukaNow = g.FirstOrDefault(x => x.TahunBuku == DateTime.Now.Year && x.TglOpTutup == null);
                        if (bukaNow != null)
                            return bukaNow;

                        return g.FirstOrDefault(x => x.TglOpTutup != null);
                    })
                    .Where(x => x != null)
                    .Select(
                        x => 
                        new DataPencarianOp
                        {
                            NOP = x.Nop,
                            Nama = x.NamaOp,
                            Alamat = x.AlamatOp,
                            JenisOp = EnumFactory.EPajak.AirTanah.GetDescription(),
                            KategoriOp = x.KategoriNama ?? "",
                            JenisPenarikan = "Tidak Ada",
                            StatusKunci = "Tidak Ada",
                            StatusOnline = "Tidak Ada",
                            StatusNOP = "Buka",
                            Wilayah = "-",
                            EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                            Tahun = (int)x.TahunBuku
                        }
                    ).ToList();

                if (dataAbt.Count > 0)
                {
                    ret.AddRange(dataAbt);
                }

                return ret;
            }

            public static List<RealisasiBulanan> GetRealisasiDetail(string nop, EnumFactory.EPajak pajak)
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;
                var ret = new List<RealisasiBulanan>();

                if (string.IsNullOrWhiteSpace(nop))
                {
                    throw new ArgumentException("NOP harus diisi");
                }
                switch (pajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        ret = context.DbMonRestos
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        ret = context.DbMonPpjs
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            }).OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        ret = context.DbMonHotels
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        ret = context.DbMonParkirs
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        ret = context.DbMonHiburans
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        ret = context.DbMonAbts
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.Reklame:

                        break;
                    case EnumFactory.EPajak.PBB:
                        //ret = context.DbMonPbbs
                        //    .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                        //    .GroupBy(x => x.TglBayarPokok.Value.Date)
                        //    .Select(g => new RealisasiBulanan
                        //    {
                        //        NOP = nop,
                        //        Month = g.Key.Month,
                        //        Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                        //        Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                        //    })
                        //    .OrderBy(x => x.Month)
                        //    .ToList();
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        ret = context.DbMonBphtbs
                            .Where(x => x.SpptNop == nop && x.TglBayar != null && x.TglBayar.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayar.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.Pokok ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.OpsenPkb:

                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:

                        break;
                    default:
                        break;
                }


                return ret;
            }
            public static List<RealisasiBulanan> GetRealisasiDetailMines1(string nop, EnumFactory.EPajak pajak)
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year - 1;
                var ret = new List<RealisasiBulanan>();

                if (string.IsNullOrWhiteSpace(nop))
                {
                    throw new ArgumentException("NOP harus diisi");
                }
                switch (pajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        ret = context.DbMonRestos
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        ret = context.DbMonPpjs
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            }).OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        ret = context.DbMonHotels
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        ret = context.DbMonParkirs
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        ret = context.DbMonHiburans
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        ret = context.DbMonAbts
                            .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayarPokok.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.Reklame:

                        break;
                    case EnumFactory.EPajak.PBB:
                        //ret = context.DbMonPbbs
                        //    .Where(x => x.Nop == nop && x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                        //    .GroupBy(x => x.TglBayarPokok.Value.Date)
                        //    .Select(g => new RealisasiBulanan
                        //    {
                        //        NOP = nop,
                        //        Month = g.Key.Month,
                        //        Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                        //        Nominal = g.Sum(x => x.NominalPokokBayar ?? 0)
                        //    })
                        //    .OrderBy(x => x.Month)
                        //    .ToList();
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        ret = context.DbMonBphtbs
                            .Where(x => x.SpptNop == nop && x.TglBayar != null && x.TglBayar.Value.Year == currentYear)
                            .GroupBy(x => x.TglBayar.Value.Date)
                            .Select(g => new RealisasiBulanan
                            {
                                NOP = nop,
                                Month = g.Key.Month,
                                Bulan = g.Key.ToString("MMMM", new CultureInfo("id-ID")),
                                Nominal = g.Sum(x => x.Pokok ?? 0)
                            })
                            .OrderBy(x => x.Month)
                            .ToList();
                        break;
                    case EnumFactory.EPajak.OpsenPkb:

                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:

                        break;
                    default:
                        break;
                }


                return ret;
            }
        }
        public class DataPencarianOp
        {
            //public int No { get; set; }
            public string Wilayah { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string StatusNOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOp { get; set; } = null!;
            public string KategoriOp { get; set; } = null!;
            public string JenisPenarikan { get; set; } = "Tidak Terapasang";
            public string TerakhirAktif { get; set; } = null!;
            public string StatusOnline { get; set; } = null!;
            public string StatusKunci { get; set; } = null!;
            public int EnumPajak { get; set; }
            public int Tahun { get; set; }
        }

        public class RealisasiBulanan
        {
            public string NOP { get; set; } = null!;
            public int Month { get; set; }
            public string Bulan { get; set; } = null!;
            public decimal Nominal { get; set; }
        }
    }
}
