using MonPDLib;
using MonPDLib.General;

namespace MonPDReborn.Models.MonitoringGlobal
{
    public class SeriesTahunanVM
    {
        public class Index
        {
            public Index()
            {

            }
        }
        public class Show
        {
            public List<ViewModel.SeriesPajakDaerah> Data { get; set; } = new();
            public Show()
            {
                Data = Method.GetSeriesPajakDaerahData();
            }
        }
        public class ViewModel
        {
            public class SeriesPajakDaerah
            {
                public string JenisPajak { get; set; } = "";
                public decimal Target1 { get; set; }
                public decimal Target2 { get; set; }
                public decimal Target3 { get; set; }
                public decimal Target4 { get; set; }
                public decimal Target5 { get; set; }
                public decimal Target6 { get; set; }
                public decimal Target7 { get; set; }
                public decimal Realisasi1 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Realisasi3 { get; set; }
                public decimal Realisasi4 { get; set; }
                public decimal Realisasi5 { get; set; }
                public decimal Realisasi6 { get; set; }
                public decimal Realisasi7 { get; set; }
                public decimal Persentase1 { get; set; }
                public decimal Persentase2 { get; set; }
                public decimal Persentase3 { get; set; }
                public decimal Persentase4 { get; set; }
                public decimal Persentase5 { get; set; }
                public decimal Persentase6 { get; set; }
                public decimal Persentase7 { get; set; }
            }
        }
        public class Method
        {
            public static List<ViewModel.SeriesPajakDaerah> GetSeriesPajakDaerahData()
            {
                var result = new List<ViewModel.SeriesPajakDaerah>();
                var context = DBClass.GetContext();
                var year = DateTime.Now.Year;
                var yearLast = year - 6;

                //target
                var targetData = context.DbAkunTargets
                    .Where(x => x.TahunBuku >= yearLast && x.TahunBuku <= year)
                    .GroupBy(x => new { x.PajakId, x.TahunBuku })
                    .Select(x => new { TahunBuku = x.Key.TahunBuku, PajakId = x.Key.PajakId, Target = x.Sum(q => q.Target) })
                    .AsEnumerable();

                //realisasi
                var dataRealisasiResto = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.MakananMinuman) })
                    .Select(x => new { TahunBuku = x.Key.Year, PajakId = x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiListrik = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.TenagaListrik) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiHotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaPerhotelan) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiParkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaParkir) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiHiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaKesenianHiburan) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiAbt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.AirTanah) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiReklame = context.DbMonReklames
                    .Where(x => x.TglBayarPokok.Value.Year >= yearLast && x.TglBayarPokok.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.Reklame) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiPbb = Enumerable.Range(yearLast, year - yearLast + 1)
                    .Select(t => new
                    {
                        TahunBuku = t,
                        PajakId = (int)EnumFactory.EPajak.PBB,
                        Realisasi = context.DbMonPbbs
                            .Where(x => x.TglBayar.HasValue
                                        && x.TglBayar.Value.Year == t
                                        && x.TahunBuku == t
                                        && x.JumlahBayarPokok > 0)
                            .Sum(x => x.JumlahBayarPokok ?? 0)
                    })
                    .AsEnumerable();

                var dataRealisasiBphtb = context.DbMonBphtbs
                    .Where(x => x.TglBayar.Value.Year >= yearLast && x.TglBayar.Value.Year <= year)
                    .GroupBy(x => new { x.TglBayar.Value.Year, PajakId = (int)(EnumFactory.EPajak.BPHTB) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.Pokok ?? 0) })
                    .AsEnumerable();

                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs
                    .Where(x => x.TglSspd.Year >= yearLast && x.TglSspd.Year <= year)
                    .GroupBy(x => new { x.TglSspd.Year, PajakId = (int)(EnumFactory.EPajak.OpsenPkb) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.JmlPokok) })
                    .AsEnumerable();

                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs
                    .Where(x => x.TglSspd.Year >= yearLast && x.TglSspd.Year <= year)
                    .GroupBy(x => new { x.TglSspd.Year, PajakId = (int)(EnumFactory.EPajak.OpsenBbnkb) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.JmlPokok) })
                    .AsEnumerable();

                //isi realisasi data
                var dataRealisasi = new List<(int TahunBuku, int PajakId, decimal Realisasi)>();
                dataRealisasi.AddRange(dataRealisasiResto.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiListrik.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiHotel.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiParkir.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiHiburan.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiAbt.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiReklame.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiPbb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiBphtb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiOpsenPkb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataRealisasi.AddRange(dataRealisasiOpsenBbnkb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));

                var pajakList = context.MPajaks.Where(x => x.Id > 0).Select(x => x.Id).ToList();
                foreach (var pajakId in pajakList)
                {
                    int tahun1 = year - 6;
                    int tahun2 = year - 5;
                    int tahun3 = year - 4;
                    int tahun4 = year - 3;
                    int tahun5 = year - 2;
                    int tahun6 = year - 1;
                    int tahun7 = year;

                    decimal target1 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun1).FirstOrDefault()?.Target ?? 0;
                    decimal target2 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun2).FirstOrDefault()?.Target ?? 0;
                    decimal target3 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun3).FirstOrDefault()?.Target ?? 0;
                    decimal target4 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun4).FirstOrDefault()?.Target ?? 0;
                    decimal target5 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun5).FirstOrDefault()?.Target ?? 0;
                    decimal target6 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun6).FirstOrDefault()?.Target ?? 0;
                    decimal target7 = targetData.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun7).FirstOrDefault()?.Target ?? 0;
                    decimal realisasi1 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun1).FirstOrDefault().Realisasi;
                    decimal realisasi2 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun2).FirstOrDefault().Realisasi;
                    decimal realisasi3 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun3).FirstOrDefault().Realisasi;
                    decimal realisasi4 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun4).FirstOrDefault().Realisasi;
                    decimal realisasi5 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun5).FirstOrDefault().Realisasi;
                    decimal realisasi6 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun6).FirstOrDefault().Realisasi;
                    decimal realisasi7 = dataRealisasi.Where(x => x.PajakId == pajakId && x.TahunBuku == tahun7).FirstOrDefault().Realisasi;
                    decimal persentase1 = target1 != 0 ? Math.Round(realisasi1 / target1 * 100, 2) : 0;
                    decimal persentase2 = target2 != 0 ? Math.Round(realisasi2 / target2 * 100, 2) : 0;
                    decimal persentase3 = target3 != 0 ? Math.Round(realisasi3 / target3 * 100, 2) : 0;
                    decimal persentase4 = target4 != 0 ? Math.Round(realisasi4 / target4 * 100, 2) : 0;
                    decimal persentase5 = target5 != 0 ? Math.Round(realisasi5 / target5 * 100, 2) : 0;
                    decimal persentase6 = target6 != 0 ? Math.Round(realisasi6 / target6 * 100, 2) : 0;
                    decimal persentase7 = target7 != 0 ? Math.Round(realisasi7 / target7 * 100, 2) : 0;



                    var res = new ViewModel.SeriesPajakDaerah();
                    res.JenisPajak = ((EnumFactory.EPajak)pajakId).GetDescription();
                    res.Target1 = target1;
                    res.Target2 = target2;
                    res.Target3 = target3;
                    res.Target4 = target4;
                    res.Target5 = target5;
                    res.Target6 = target6;
                    res.Target7 = target7;
                    res.Realisasi1 = realisasi1;
                    res.Realisasi2 = realisasi2;
                    res.Realisasi3 = realisasi3;
                    res.Realisasi4 = realisasi4;
                    res.Realisasi5 = realisasi5;
                    res.Realisasi6 = realisasi6;
                    res.Realisasi7 = realisasi7;
                    res.Persentase1 = persentase1;
                    res.Persentase2 = persentase2;
                    res.Persentase3 = persentase3;
                    res.Persentase4 = persentase4;
                    res.Persentase5 = persentase5;
                    res.Persentase6 = persentase6;
                    res.Persentase7 = persentase7;

                    result.Add(res);
                }


                return result;
            }
        }
    }
}
