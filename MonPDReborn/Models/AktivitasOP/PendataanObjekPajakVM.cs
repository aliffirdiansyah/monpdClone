using MonPDLib;
using MonPDLib.General;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MonPDReborn.Models.AktivitasOP
{
    public class PendataanObjekPajakVM
    {
        public class Index
        {
            public string Keyword { get; set; } = string.Empty;
            public Dashboard Data { get; set; } = new Dashboard();
            public Index()
            {
                Data = Method.GetDashboard();
            }
        }

        public class Show
        {
            public List<DataPendataan> DataPendataanList { get; set; } = new();
            public Show()
            {
                DataPendataanList = Method.GetDataPendataanList();
            }
        }

        public class Detail
        {
            public List<DataDetailPendataan> DataDetailList { get; set; } = new();

            public Detail() { }

            public Detail(EnumFactory.EPajak jenisPajak, int tahun)
            {
                DataDetailList = Method.GetDetailPendataanList(jenisPajak, tahun);
            }
        }

        public class SubDetail
        {
            public List<SubDetailRestoran> DataRestoranList { get; set; } = new();
            public List<SubDetailParkir> DataParkirList { get; set; } = new();

            public SubDetail() { }

            public SubDetail(EnumFactory.EPajak jenisPajak, string nop, int tahun)
            {
                if (jenisPajak == EnumFactory.EPajak.MakananMinuman)
                {
                    DataRestoranList = Method.GetSubDetailRestoran(jenisPajak, nop, tahun);
                }
                else if (jenisPajak == EnumFactory.EPajak.JasaParkir)
                {
                    DataParkirList = Method.GetSubDetailParkir(jenisPajak, nop, tahun);
                }
            }
        }


        public class DataPendataan
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = string.Empty;
            public decimal JumlahOp { get; set; }
            public decimal Potensi { get; set; }
            public decimal TotalRealisasi { get; set; }
            public decimal Selisih { get; set; }
        }

        public class DataDetailPendataan
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = string.Empty;
            public string NOP { get; set; } = string.Empty;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string ObjekPajak { get; set; } = string.Empty;
            public string Alamat { get; set; } = string.Empty;
            public decimal Omzet { get; set; } = 0;
            public decimal PajakBulanan { get; set; }
            public decimal AvgRealisasi { get; set; }
            public decimal Selisih => PajakBulanan - AvgRealisasi;
        }

        public class Dashboard
        {
            public decimal TotalPengedokan { get; set; }
            public decimal TotalRealisasi { get; set; }

            public decimal JumlahObjek { get; set; }

            public decimal Ratarata =>
                JumlahObjek > 0 ? TotalRealisasi / JumlahObjek : 0;
        }


        public static class Method
        {
            public static List<DataPendataan> GetDataPendataanList()
            {
                var ret = new List<DataPendataan>();
                var context = DBClass.GetContext();

                var restoDokNop = context.DbRekamRestorans.GroupBy(x => new { Nop = x.Nop }).Select(x => (x.Key.Nop).Replace(".", "")).ToList();
                var restoRealisasi = context.DbMonRestos.Where(x => restoDokNop.Contains(x.Nop)).AsQueryable();
                var restoDok = context.DbRekamRestorans
                    .GroupBy(x => new { PajakId = x.PajakId, Tahun = x.Tanggal.Year })
                    .AsEnumerable() // biar LINQ to Objects
                    .Select(x => new DataPendataan
                    {
                        Tahun = x.Key.Tahun,
                        EnumPajak = (int)x.Key.PajakId,
                        JenisPajak = ((EnumFactory.EPajak)x.Key.PajakId).GetDescription(),
                        JumlahOp = x.Select(d => d.Nop.Replace(".", "")).Distinct().Count(),
                        Potensi = x.GroupBy(d => d.Nop).Sum(g => g.Max(d => d.PajakBulan)) * 12,
                        TotalRealisasi = restoRealisasi
                            .Where(s => s.TglBayarPokok.Value.Year == x.Key.Tahun)
                            .Sum(c => (decimal?)c.NominalPokokBayar) ?? 0,
                        Selisih = (
                            restoRealisasi
                                .Where(s => s.TglBayarPokok.Value.Year == x.Key.Tahun)
                                .Sum(c => (decimal?)c.NominalPokokBayar) ?? 0
                        ) - x.GroupBy(d => d.Nop).Sum(g => g.Max(d => d.PajakBulan))
                    })
                    .ToList();

                ret.AddRange(restoDok);

                var parkirDokNop = context.DbRekamParkirs
                    .GroupBy(x => new { Nop = x.Nop })
                    .Select(x => (x.Key.Nop).Replace(".", ""))
                    .ToList();

                var parkirRealisasi = context.DbMonParkirs
                    .Where(x => parkirDokNop.Contains(x.Nop))
                    .AsQueryable();

                var parkirDok = context.DbRekamParkirs
                    .GroupBy(x => new { PajakId = x.PajakId, Tahun = x.Tanggal.Year })
                    .AsEnumerable()
                    .Select(x => new DataPendataan
                    {
                        Tahun = x.Key.Tahun,
                        EnumPajak = (int)x.Key.PajakId,
                        JenisPajak = ((EnumFactory.EPajak)x.Key.PajakId).GetDescription(),
                        JumlahOp = x.Select(d => d.Nop.Replace(".", "")).Distinct().Count(),
                        Potensi = x.GroupBy(d => d.Nop).Sum(g => g.Max(d => d.PajakBulan)) * 12,
                        TotalRealisasi = parkirRealisasi
                            .Where(s => s.TglBayarPokok.Value.Year == x.Key.Tahun)
                            .Sum(c => (decimal?)c.NominalPokokBayar) ?? 0,
                        Selisih = (
                            parkirRealisasi
                                .Where(s => s.TglBayarPokok.Value.Year == x.Key.Tahun)
                                .Sum(c => (decimal?)c.NominalPokokBayar) ?? 0
                        ) - x.GroupBy(d => d.Nop).Sum(g => g.Max(d => d.PajakBulan))
                    })
                    .ToList();

                ret.AddRange(parkirDok);

                return ret;
            }

            public static List<DataDetailPendataan> GetDetailPendataanList(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var ret = new List<DataDetailPendataan>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var restoDokNop = context.DbRekamRestorans.Where(x => x.Tanggal.Year == tahun).GroupBy(x => new { Nop = x.Nop }).Select(x => (x.Key.Nop).Replace(".", "")).ToList();
                        var restoRealisasiList = context.DbMonRestos
                            .Where(x => restoDokNop.Contains(x.Nop) && x.TahunBuku == tahun)
                            .ToList();

                        var restoDok = context.DbRekamRestorans
                            .Where(x => x.Tanggal.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, PajakId = x.PajakId, Tahun = x.Tanggal.Year })
                            .ToList()
                            .Select(x =>
                            {
                                var nop = (x.Key.Nop).Replace(".", "");
                                var realisasi = restoRealisasiList.FirstOrDefault(r => r.Nop == nop);
                                return new DataDetailPendataan
                                {
                                    Tahun = tahun,
                                    EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                                    JenisPajak = ((EnumFactory.EPajak.MakananMinuman).GetDescription()),
                                    NOP = nop,
                                    ObjekPajak = realisasi?.NamaOp ?? "-",
                                    Alamat = realisasi?.AlamatOp ?? "-",
                                    Omzet = x.Max(x => x.OmseBulan),
                                    PajakBulanan = x.Max(x => x.PajakBulan),
                                    AvgRealisasi = restoRealisasiList
                                        .Where(r => r.Nop == nop)
                                        .Average(r => (decimal?)r.NominalPokokBayar) ?? 0
                                };
                            })
                            .ToList();

                        ret.AddRange(restoDok);
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var parkirDokNop = context.DbRekamParkirs.Where(x => x.Tanggal.Year == tahun).GroupBy(x => new { Nop = x.Nop }).Select(x => (x.Key.Nop).Replace(".", "")).ToList();
                        var parkirRealisasiList = context.DbMonParkirs
                            .Where(x => parkirDokNop.Contains(x.Nop) && x.TahunBuku == tahun)
                            .ToList();

                        var parkirDok = context.DbRekamParkirs
                            .Where(x => x.Tanggal.Year == tahun)
                            .GroupBy(x => new { Nop = x.Nop, PajakId = x.PajakId, Tahun = x.Tanggal.Year })
                            .ToList()
                            .Select(x =>
                            {
                                var nop = (x.Key.Nop).Replace(".", "");
                                var realisasi = parkirRealisasiList.FirstOrDefault(r => r.Nop == nop);
                                return new DataDetailPendataan
                                {
                                    Tahun = tahun,
                                    EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                                    JenisPajak = ((EnumFactory.EPajak.JasaParkir).GetDescription()),
                                    NOP = nop,
                                    ObjekPajak = realisasi?.NamaOp ?? "-",
                                    Alamat = realisasi?.AlamatOp ?? "-",
                                    Omzet = x.Max(x => x.OmzetBulan),
                                    PajakBulanan = x.Max(x => x.PajakBulan),
                                    AvgRealisasi = parkirRealisasiList
                                        .Where(r => r.Nop == nop)
                                        .Average(r => (decimal?)r.NominalPokokBayar) ?? 0
                                };
                            })
                            .ToList();

                        ret.AddRange(parkirDok);
                        break;
                    default:
                        break;
                }

                return ret;
            }
            public static List<SubDetailRestoran> GetSubDetailRestoran(EnumFactory.EPajak jenisPajak, string nop, int tahun)
            {
                var context = DBClass.GetContext();
                nop = nop.Replace(".", "");

                var dbResto = context.DbOpRestos
                    .Where(x => x.Nop == nop)
                    .Select(x => new
                    {
                        x.Nop,
                        x.NamaOp,
                        x.AlamatOp
                    })
                    .FirstOrDefault();
                var restoData = context.DbRekamRestorans
                    .Where(x => (x.Nop).Replace(".", "") == nop && x.Tanggal.Year == tahun)
                    .Select(x => new SubDetailRestoran
                    {
                        Tahun = tahun,
                        EnumPajak = (int)jenisPajak,
                        JenisPajak = jenisPajak.GetDescription(),
                        NOP = x.Nop,
                        ObjekPajak = dbResto.NamaOp ?? "-",
                        Alamat = dbResto.AlamatOp ?? "-",
                        Hari = x.Tanggal,
                        Tgl = x.Tanggal,
                        JmlMeja = (int)x.JmlMeja,
                        JmlKursi = (int)x.JmlKursi,
                        JmlPengunjung = (int)x.JmlPengunjung,
                        Bill = x.Bill,
                        RataPengunjung = (int)x.RataPengunjungHari,
                        RataBill = x.RataBillPengunjung
                    })
                    .ToList();

                return restoData;
            }
            public static List<SubDetailParkir> GetSubDetailParkir(EnumFactory.EPajak jenisPajak, string nop, int tahun)
            {
                var context = DBClass.GetContext();
                nop = nop.Replace(".", "");

                var dbParkir = context.DbOpParkirs
                    .Where(x => x.Nop == nop)
                    .Select(x => new
                    {
                        x.Nop,
                        x.NamaOp,
                        x.AlamatOp
                    })
                    .FirstOrDefault();
                var parkirData = context.DbRekamParkirs
                    .Where(x => (x.Nop).Replace(".", "") == nop && x.Tanggal.Year == tahun)
                    .Select(x => new SubDetailParkir
                    {
                        Tahun = tahun,
                        EnumPajak = (int)jenisPajak,
                        JenisPajak = jenisPajak.GetDescription(),
                        NOP = x.Nop,
                        ObjekPajak = dbParkir.NamaOp ?? "-",
                        Alamat = dbParkir.AlamatOp ?? "-",
                        Hari = x.Tanggal,
                        Tgl = x.Tanggal,
                        JenisBiaya = x.JenisBiayaParkir.Length <= 1 ? ((EnumFactory.EPungutTarifParkir)Convert.ToInt32(x.JenisBiayaParkir)).GetDescription() : x.JenisBiayaParkir,
                        KapasitasMotor = (int)x.KapasitasMotor,
                        KapasitasMobil = (int)x.KapasitasMobil,
                        JmlMotor = (int)x.HasilJumlahMotor,
                        JmlMobil = (int)x.HasilJumlahMobil,
                        JmlMobilBox = (int)x.HasilJumlahMobilBox,
                        JmlTruk = (int)x.HasilJumlahTruk,
                        JmlTrailer = (int)x.HasilJumlahTrailer,
                        EstMotor = (int)x.EstMotorHarian,
                        EstMobil = (int)x.EstMobilHarian,
                        EstMobilBox = (int)x.EstMobilBoxHarian,
                        EstTruk = (int)x.EstTrukHarian,
                        EstTrailer = (int)x.EstTrailerHarian,
                        TarifMotor = x.TarifMotor,
                        TarifMobil = x.TarifMobil,
                        TarifMobilBox = x.TarifMobilBox,
                        TarifTruk = x.TarifTruk,
                        TarifTrailer = x.TarifTrailer
                    })
                    .ToList();

                return parkirData;
            }
            public static Dashboard GetDashboard()
            {
                var ret = GetDataPendataanList();

                return new Dashboard
                {
                    TotalPengedokan = ret.Sum(x => x.Potensi),
                    TotalRealisasi = ret.Sum(x => x.TotalRealisasi),
                    JumlahObjek = ret.Sum(x => x.JumlahOp)
                };
            }


        }

        public class SubDetailRestoran
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string ObjekPajak { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public DateTime Hari { get; set; }
            public DateTime Tgl { get; set; }
            public int JmlMeja { get; set; }
            public int JmlKursi { get; set; }
            public int JmlPengunjung { get; set; }
            public decimal Bill { get; set; }
            public int RataPengunjung { get; set; }
            public decimal RataBill { get; set; }
            public string? NamaHari => Hari.ToString("dddd", new CultureInfo("id-ID"));
        }

        public class SubDetailParkir
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string ObjekPajak { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public DateTime Hari { get; set; }
            public DateTime Tgl { get; set; }
            public string JenisBiaya { get; set; } = null!;
            public int KapasitasMotor { get; set; }
            public int KapasitasMobil { get; set; }
            public int JmlMotor { get; set; }
            public int JmlMobil { get; set; }
            public int JmlMobilBox { get; set; }
            public int JmlTruk { get; set; }
            public int JmlTrailer { get; set; }
            public int EstMotor { get; set; }
            public int EstMobil { get; set; }
            public int EstMobilBox { get; set; }
            public int EstTruk { get; set; }
            public int EstTrailer { get; set; }
            public decimal TarifMotor { get; set; }
            public decimal TarifMobil { get; set; }
            public decimal TarifMobilBox { get; set; }
            public decimal TarifTruk { get; set; }
            public decimal TarifTrailer { get; set; }
            public string? NamaHari => Hari.ToString("dddd", new CultureInfo("id-ID"));
        }
    }
}