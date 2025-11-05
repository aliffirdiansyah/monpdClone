using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class RealisasiPendapatanDaerahVM
    {
        public class Index
        {
            public Index()
            {

            }
        }
        public class Show
        {
            public List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Kelompok> Data { get; set; } = new();
            public DateTime tgl { get; set; }
            public Show(DateTime TglCutOff)
            {
                Data = Methods.GetSudutPandangRekeningJenisObjekOpdData(TglCutOff);
                tgl = TglCutOff;
            }
        }
        public class ViewModels
        {
            public class ShowSeriesSudutPandangRekeningJenisObjekOpd
            {
                public class Jenis
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Obyek> RekObyeks { get; set; } = new List<Obyek>();
                }
                public class Kelompok
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Jenis> RekJenis { get; set; } = new();
                }
                public class Obyek
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<Opd> RekOpds { get; set; } = new List<Opd>();
                }
                public class Opd
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                    public List<SubRincian> RekSubRincians { get; set; } = new List<SubRincian>();
                }
                public class SubRincian
                {
                    public FormatColumn.ColumnA Col { get; set; } = new();
                }
            }
            public class FormatColumn
            {
                public class ColumnA
                {
                    public string Kode { get; set; } = "";
                    public string Nama { get; set; } = "";
                    public decimal Target { get; set; } = 0;
                    public decimal Realisasi { get; set; } = 0;
                    public decimal Persentase { get; set; } = 0;
                    public decimal AkpTahun { get; set; } = 0;
                    public decimal RealisasiHariAccrual { get; set; } = 0;
                    public decimal RealisasiSDHariAccrual { get; set; } = 0;
                    public decimal PersenAccrual { get; set; } = 0;
                }

                public class ReportColumnA
                {
                    public string Nama { get; set; } = "";
                    public decimal TargetTotal { get; set; } = 0;
                    public decimal TargetSampaiDengan { get; set; } = 0;
                    public decimal RealisasiSampaiDengan { get; set; } = 0;
                    public decimal SelisihSampaiDengan => RealisasiSampaiDengan - TargetSampaiDengan;
                    public decimal PersentaseSampaiDengan => TargetSampaiDengan > 0
                        ? Math.Round((RealisasiSampaiDengan / TargetSampaiDengan) * 100, 2)
                        : 0;
                    public decimal TargetBulanIni { get; set; } = 0;
                    public decimal RealisasiBulanIni { get; set; } = 0;
                    public decimal SelisihBulanIni => RealisasiBulanIni - TargetBulanIni;
                    public decimal PersentaseBulanIni => TargetBulanIni > 0
                       ? Math.Round((RealisasiBulanIni / TargetBulanIni) * 100, 2)
                       : 0;
                    public decimal PersentaseTotal => TargetTotal > 0
                       ? Math.Round((RealisasiSampaiDengan / TargetTotal) * 100, 2)
                       : 0;
                }

                public class ReportColumnB
                {
                    public string Nama { get; set; } = "";
                    public decimal TargetTotal { get; set; } = 0;
                    public decimal TargetSampaiDengan { get; set; } = 0;
                    public decimal RealisasiSampaiDengan { get; set; } = 0;
                    public decimal SelisihSampaiDengan => RealisasiSampaiDengan - TargetSampaiDengan;
                    public decimal PersentaseSampaiDengan => TargetSampaiDengan > 0
                        ? Math.Round((RealisasiSampaiDengan / TargetSampaiDengan) * 100, 2)
                        : 0;
                    public decimal TargetBulanIni { get; set; } = 0;
                    public decimal RealisasiBulanIni { get; set; } = 0;
                    public decimal SelisihBulanIni => RealisasiBulanIni - TargetBulanIni;
                    public decimal PersentaseBulanIni => TargetBulanIni > 0
                       ? Math.Round((RealisasiBulanIni / TargetBulanIni) * 100, 2)
                       : 0;
                    public decimal PersentaseTotal => TargetTotal > 0
                       ? Math.Round((RealisasiSampaiDengan / TargetTotal) * 100, 2)
                       : 0;
                    public int Status { get; set; } = 0;
                }
            }
        }
        public class Methods
        {
            public static List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Kelompok> GetSudutPandangRekeningJenisObjekOpdData(DateTime TglCutOff)
            {
                var result = new List<ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Kelompok>();
                var context = DBClass.GetContext();

                // === 1️⃣ Ambil target tahunan (AkpTahun) ===
                var akpTahunQuery = context.DbPendapatanDaerahHarians
                    .GroupBy(x => new
                    {
                        x.Kelompok,
                        x.NamaKelompok,
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek
                    })
                    .Select(x => new
                    {
                        x.Key.Kelompok,
                        x.Key.NamaKelompok,
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        x.Key.Objek,
                        x.Key.NamaObjek,
                        AkpTahun = x.Sum(y => y.Target)
                    })
                    .ToList();

                // === 2️⃣ Ambil realisasi per hari (tgl cutoff) ===
                var realisasiHariQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.Tanggal.Date == TglCutOff.Date)
                    .GroupBy(x => new
                    {
                        x.Kelompok,
                        x.NamaKelompok,
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek
                    })
                    .Select(x => new
                    {
                        x.Key.Kelompok,
                        x.Key.NamaKelompok,
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        x.Key.Objek,
                        x.Key.NamaObjek,
                        RealisasiHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // === 3️⃣ Ambil realisasi sampai tgl cutoff ===
                var realisasiSdQuery = context.DbPendapatanDaerahHarians
                    .Where(x => x.Tanggal.Date <= TglCutOff.Date)
                    .GroupBy(x => new
                    {
                        x.Kelompok,
                        x.NamaKelompok,
                        x.Jenis,
                        x.NamaJenis,
                        x.Objek,
                        x.NamaObjek
                    })
                    .Select(x => new
                    {
                        x.Key.Kelompok,
                        x.Key.NamaKelompok,
                        x.Key.Jenis,
                        x.Key.NamaJenis,
                        x.Key.Objek,
                        x.Key.NamaObjek,
                        RealisasiSDHariAccrual = x.Sum(y => y.Realisasi)
                    })
                    .ToList();

                // === 4️⃣ Gabungkan semua data ===
                var merged = (from a in akpTahunQuery
                              join b in realisasiHariQuery
                                  on new { a.Kelompok, a.Jenis, a.Objek }
                                  equals new { b.Kelompok, b.Jenis, b.Objek } into gj1
                              from b in gj1.DefaultIfEmpty()
                              join c in realisasiSdQuery
                                  on new { a.Kelompok, a.Jenis, a.Objek }
                                  equals new { c.Kelompok, c.Jenis, c.Objek } into gj2
                              from c in gj2.DefaultIfEmpty()
                              select new
                              {
                                  a.Kelompok,
                                  a.NamaKelompok,
                                  a.Jenis,
                                  a.NamaJenis,
                                  a.Objek,
                                  a.NamaObjek,
                                  AkpTahun = a.AkpTahun,
                                  RealisasiHariAccrual = b?.RealisasiHariAccrual ?? 0,
                                  RealisasiSDHariAccrual = c?.RealisasiSDHariAccrual ?? 0
                              })
                              .ToList();

                // === 5️⃣ Grouping sesuai hierarki ===
                var groupByKelompok = merged
                    .GroupBy(k => new { k.Kelompok, k.NamaKelompok })
                    .Select(kel => new
                    {
                        kel.Key.Kelompok,
                        kel.Key.NamaKelompok,
                        JenisList = kel
                            .GroupBy(j => new { j.Jenis, j.NamaJenis })
                            .Select(jen => new
                            {
                                jen.Key.Jenis,
                                jen.Key.NamaJenis,
                                ObjekList = jen
                                    .GroupBy(o => new { o.Objek, o.NamaObjek })
                                    .Select(obj => new
                                    {
                                        obj.Key.Objek,
                                        obj.Key.NamaObjek,
                                        AkpTahun = obj.Sum(z => z.AkpTahun),
                                        RealisasiHariAccrual = obj.Sum(z => z.RealisasiHariAccrual),
                                        RealisasiSDHariAccrual = obj.Sum(z => z.RealisasiSDHariAccrual)
                                    })
                                    .OrderBy(x => x.Objek)
                                    .ToList(),
                                AkpTahun = jen.Sum(z => z.AkpTahun),
                                RealisasiHariAccrual = jen.Sum(z => z.RealisasiHariAccrual),
                                RealisasiSDHariAccrual = jen.Sum(z => z.RealisasiSDHariAccrual)
                            })
                            .OrderBy(x => x.Jenis)
                            .ToList(),
                        AkpTahun = kel.Sum(z => z.AkpTahun),
                        RealisasiHariAccrual = kel.Sum(z => z.RealisasiHariAccrual),
                        RealisasiSDHariAccrual = kel.Sum(z => z.RealisasiSDHariAccrual)
                    })
                    .OrderBy(x => x.Kelompok)
                    .ToList();

                // === 6️⃣ Mapping ke ViewModel ===
                decimal totalAkp = 0, totalHari = 0, totalSd = 0;

                foreach (var kel in groupByKelompok)
                {
                    var resKel = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Kelompok();
                    resKel.Col.Kode = kel.Kelompok;
                    resKel.Col.Nama = kel.NamaKelompok;
                    resKel.Col.AkpTahun = kel.AkpTahun;
                    resKel.Col.RealisasiHariAccrual = kel.RealisasiHariAccrual;
                    resKel.Col.RealisasiSDHariAccrual = kel.RealisasiSDHariAccrual;
                    resKel.Col.PersenAccrual = kel.AkpTahun > 0 ? Math.Round((kel.RealisasiSDHariAccrual / kel.AkpTahun) * 100, 2) : 0;

                    totalAkp += kel.AkpTahun;
                    totalHari += kel.RealisasiHariAccrual;
                    totalSd += kel.RealisasiSDHariAccrual;

                    foreach (var jen in kel.JenisList)
                    {
                        var resJen = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis();
                        resJen.Col.Kode = jen.Jenis;
                        resJen.Col.Nama = jen.NamaJenis;
                        resJen.Col.AkpTahun = jen.AkpTahun;
                        resJen.Col.RealisasiHariAccrual = jen.RealisasiHariAccrual;
                        resJen.Col.RealisasiSDHariAccrual = jen.RealisasiSDHariAccrual;
                        resJen.Col.PersenAccrual = jen.AkpTahun > 0 ? Math.Round((jen.RealisasiSDHariAccrual / jen.AkpTahun) * 100, 2) : 0;

                        foreach (var obj in jen.ObjekList)
                        {
                            var resObj = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Obyek();
                            resObj.Col.Kode = obj.Objek;
                            resObj.Col.Nama = obj.NamaObjek;
                            resObj.Col.AkpTahun = obj.AkpTahun;
                            resObj.Col.RealisasiHariAccrual = obj.RealisasiHariAccrual;
                            resObj.Col.RealisasiSDHariAccrual = obj.RealisasiSDHariAccrual;
                            resObj.Col.PersenAccrual = obj.AkpTahun > 0 ? Math.Round((obj.RealisasiSDHariAccrual / obj.AkpTahun) * 100, 2) : 0;

                            resJen.RekObyeks.Add(resObj);
                        }

                        resKel.RekJenis.Add(resJen);
                    }

                    result.Add(resKel);
                }

                // === 7️⃣ Total keseluruhan ===
                var totalKel = new ViewModels.ShowSeriesSudutPandangRekeningJenisObjekOpd.Kelompok();
                totalKel.Col.Kode = "";
                totalKel.Col.Nama = "TOTAL SEMUA KELOMPOK";
                totalKel.Col.AkpTahun = totalAkp;
                totalKel.Col.RealisasiHariAccrual = totalHari;
                totalKel.Col.RealisasiSDHariAccrual = totalSd;
                totalKel.Col.PersenAccrual = totalAkp > 0 ? Math.Round((totalSd / totalAkp) * 100, 2) : 0;

                result.Insert(0, totalKel);

                return result;
            }


        }
    }
}
