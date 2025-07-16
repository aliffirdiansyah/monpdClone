using MonPDLib;
using MonPDLib.General;
using System.Collections.Generic;
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
                Data = Method.GetDashboardData();
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

            public Detail(EnumFactory.EPajak jenisPajak)
            {
                DataDetailList = Method.GetDetailPendataanList(jenisPajak);
            }
        }

        public class SubDetail
        {
            public List<SubDetailRestoran> DataRestoranList { get; set; } = new();
            public List<SubDetailParkir> DataParkirList { get; set; } = new();

            public SubDetail() { }

            public SubDetail(EnumFactory.EPajak jenisPajak, string nop)
            {
                if (jenisPajak == EnumFactory.EPajak.MakananMinuman)
                {
                    DataRestoranList = Method.GetSubDetailRestoran(jenisPajak, nop);
                }
                else if (jenisPajak == EnumFactory.EPajak.JasaParkir)
                {
                    DataParkirList = Method.GetSubDetailParkir(jenisPajak, nop);
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
            public string ObjekPajak { get; set; } = string.Empty;
            public string Alamat { get; set; } = string.Empty;
            public decimal Omzet { get; set; }
            public decimal PajakBulanan { get; set; }
            public decimal AvgRealisasi { get; set; }
            public decimal Selisih => PajakBulanan - AvgRealisasi;
        }

        public class Dashboard
        {
            public decimal TotalPengedokan { get; set; }
            public decimal TotalRealisasi { get; set; }

            public int JumlahObjek { get; set; }

            public decimal Ratarata =>
                JumlahObjek > 0 ? TotalRealisasi / JumlahObjek : 0;
        }


        public static class Method
        {
            public static List<DataPendataan> GetDataPendataanList()
            {
                var ret = new List<DataPendataan>();
                var context = DBClass.GetContext();

                var restoDokNop = context.DbRekamRestorans.GroupBy(x => new { Nop = x.Nop }).Select(x => (x.Key.Nop).Replace(".","")).ToList();
                var restoRealisasi = context.DbMonRestos.Where(x => restoDokNop.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year).Sum(x => x.NominalPokokBayar);
                var restoDok = context.DbRekamRestorans.GroupBy(x => new { PajakId = x.PajakId })
                    .Select(x => new DataPendataan
                    {
                        Tahun = DateTime.Now.Year,
                        EnumPajak = (int)x.Key.PajakId,
                        JenisPajak = ((EnumFactory.EPajak)x.Key.PajakId).GetDescription(),
                        JumlahOp = x.Count(),
                        Potensi = x.Sum(x => x.PajakBulan),
                        TotalRealisasi = restoRealisasi ?? 0,
                        Selisih = x.Sum(x => x.PajakBulan) - (restoRealisasi ?? 0)
                    })
                    .ToList();

                ret.AddRange(restoDok);

                var parkirDokNop = context.DbRekamParkirs.GroupBy(x => new { Nop = x.Nop }).Select(x => (x.Key.Nop).Replace(".", "")).ToList();
                var parkirRealisasi = context.DbMonParkirs.Where(x => parkirDokNop.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year).Sum(x => x.NominalPokokBayar);
                var parkirDok = context.DbRekamParkirs.GroupBy(x => new { PajakId = x.PajakId })
                    .Select(x => new DataPendataan
                    {
                        Tahun = DateTime.Now.Year,
                        EnumPajak = (int)x.Key.PajakId,
                        JenisPajak = ((EnumFactory.EPajak)x.Key.PajakId).GetDescription(),
                        JumlahOp = x.Count(),
                        Potensi = x.Sum(x => x.PajakBulan),
                        TotalRealisasi = parkirRealisasi ?? 0,
                        Selisih = x.Sum(x => x.PajakBulan) - (parkirRealisasi ?? 0)
                    })
                    .ToList();

                ret.AddRange(parkirDok);

                return ret;
            }

            public static List<DataDetailPendataan> GetDetailPendataanList(EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<DataDetailPendataan>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var restoDokNop = context.DbRekamRestorans.GroupBy(x => new { Nop = x.Nop }).Select(x => x.Key.Nop).ToList();
                        var restoRealisasiList = context.DbMonRestos
                            .Where(x => restoDokNop.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year)
                            .ToList();

                        var restoDok = context.DbRekamRestorans
                            .GroupBy(x => new { x.Nop })
                            .ToList()
                            .Select(x =>
                            {
                                var nop = (x.Key.Nop).Replace(".","");
                                var realisasi = restoRealisasiList.FirstOrDefault(r => r.Nop == nop);
                                return new DataDetailPendataan
                                {
                                    Tahun = DateTime.Now.Year,
                                    EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                                    JenisPajak = ((EnumFactory.EPajak.MakananMinuman).GetDescription()),
                                    NOP = nop,
                                    ObjekPajak = realisasi?.NamaOp ?? "-",
                                    Alamat = realisasi?.AlamatOp ?? "-",
                                    Omzet = x.Sum(r => r.OmseBulan),
                                    PajakBulanan = x.Sum(r => r.PajakBulan),
                                    AvgRealisasi = restoRealisasiList
                                        .Where(r => r.Nop == nop)
                                        .Average(r => (decimal?)r.NominalPokokBayar) ?? 0
                                };
                            })
                            .ToList();

                        ret.AddRange(restoDok);
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var parkirDokNop = context.DbRekamParkirs.GroupBy(x => new { Nop = x.Nop }).Select(x => x.Key.Nop).ToList();
                        var parkirRealisasiList = context.DbMonParkirs
                            .Where(x => parkirDokNop.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year)
                            .ToList();

                        var parkirDok = context.DbRekamParkirs
                            .GroupBy(x => new { x.Nop })
                            .ToList()
                            .Select(x =>
                            {
                                var nop = (x.Key.Nop).Replace(".","");
                                var realisasi = parkirRealisasiList.FirstOrDefault(r => r.Nop == nop);
                                return new DataDetailPendataan
                                {
                                    Tahun = DateTime.Now.Year,
                                    EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                                    JenisPajak = ((EnumFactory.EPajak.JasaParkir).GetDescription()),
                                    NOP = nop,
                                    ObjekPajak = realisasi?.NamaOp ?? "-",
                                    Alamat = realisasi?.AlamatOp ?? "-",
                                    Omzet = x.Sum(r => r.OmzetBulan),
                                    PajakBulanan = x.Sum(r => r.PajakBulan),
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
            public static List<SubDetailRestoran> GetSubDetailRestoran(EnumFactory.EPajak jenisPajak, string nop)
            {
                var context = DBClass.GetContext();
                nop = nop.Replace(".", "");

                var dbResto = context.DbOpRestos
                    .Where(x => x.Nop == nop && x.PajakId == (int)jenisPajak)
                    .Select(x => new
                    {
                        x.Nop,
                        x.NamaOp,
                        x.AlamatOp
                    })
                    .FirstOrDefault();
                var restoData = context.DbRekamRestorans
                    .Where(x => (x.Nop).Replace(".","") == nop && x.PajakId == (int)jenisPajak)
                    .Select(x => new SubDetailRestoran
                    {
                        Tahun = DateTime.Now.Year,
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
            public static List<SubDetailParkir> GetSubDetailParkir(EnumFactory.EPajak jenisPajak, string nop)
            {
                var context = DBClass.GetContext();
                nop = nop.Replace(".", "");

                var dbParkir = context.DbOpParkirs
                    .Where(x => x.Nop == nop && x.PajakId == (int)jenisPajak)
                    .Select(x => new
                    {
                        x.Nop,
                        x.NamaOp,
                        x.AlamatOp
                    })
                    .FirstOrDefault();
                var parkirData = context.DbRekamParkirs
                    .Where(x => (x.Nop).Replace(".","") == nop && x.PajakId == (int)jenisPajak)
                    .Select(x => new SubDetailParkir
                    {
                        Tahun = DateTime.Now.Year,
                        EnumPajak = (int)jenisPajak,
                        JenisPajak = jenisPajak.GetDescription(),
                        NOP = x.Nop,
                        ObjekPajak = dbParkir.NamaOp ?? "-",
                        Alamat = dbParkir.AlamatOp ?? "-",
                        Hari = x.Tanggal,
                        Tgl = x.Tanggal,
                        JenisBiaya = x.JenisBiayaParkir,
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

            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    TotalPengedokan = 50,
                    JumlahObjek = 100,
                    TotalRealisasi = 800000000
                };
            }
        }

        public class SubDetailRestoran
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
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
        }

        public class SubDetailParkir
        {
            public int Tahun { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
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
        }
    }
}