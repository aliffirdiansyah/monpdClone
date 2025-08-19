using MonPDLib;
using MonPDLib.General;
using static MonPDReborn.Models.AktivitasWP.PenghimbauanVM;

namespace MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM
{
    public class Index
    {
        public string Keyword { get; set; } = null!;
        public Dashboard Data { get; set; } = new Dashboard();
        public Index()
        {
            Data = Method.GetDashboardData();
        }
    }
    public class Show
    {
        public List<KontrolPrediksi> DataKontrolPrediksiList { get; set; } = new();


        public Show(DateTime? tanggalAwal, DateTime? tanggalAkhir)
        {
            if (tanggalAwal == null)
                throw new ArgumentException("Tanggal awal tidak boleh null.");
            if (tanggalAkhir == null)
                throw new ArgumentException("Tanggal akhir tidak boleh null.");
            if (tanggalAkhir < tanggalAwal)
                throw new ArgumentException("Tanggal akhir tidak boleh lebih kecil dari tanggal awal.");
            if (tanggalAwal > DateTime.Now)
                throw new ArgumentException("Tanggal awal tidak boleh melebihi tanggal hari ini.");
            if (tanggalAwal.Value.Year != tanggalAkhir.Value.Year)
                throw new ArgumentException("Tanggal awal dan akhir harus di tahun yang sama.");

            DataKontrolPrediksiList = Method.GetDataList(tanggalAwal.Value, tanggalAkhir.Value);
        }
    }

    public class Method
    {
        public static Dashboard GetDashboardData()
        {
            return new Dashboard
            {
                TotalTarget = 500000000,
                TotalRealisasi = 435750000.50,
                RataRataPencapaian = 87.15
            };
        }

        public static List<KontrolPrediksi> GetDataList(DateTime tanggalAwal, DateTime tanggalAkhir)
        {
           
            var context = DBClass.GetContext();
            var currentYear = DateTime.Now.Year;
            var bulanLaluCutoff = new DateTime(currentYear, DateTime.Now.Month, 1).AddDays(-1);
            var awalTahun = new DateTime(currentYear, 1, 1);

            var list = new List<KontrolPrediksi>();

            // ==== Ambil target semua pajak dalam 1 query ====
            var targetDict = context.DbAkunTargets
                .Where(x => x.TahunBuku == currentYear)
                .GroupBy(x => x.PajakId)
                .ToDictionary(
                    g => (int)g.Key,
                    g => g.Sum(y => y.Target)
                );

            // ==== Makanan Minuman ====
            var restoData = context.DbMonRestos
                .Where(x => x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayarPokok.Value,
                    Nominal = x.NominalPokokBayar ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.MakananMinuman, targetDict, restoData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Hotel ====
            var hotelData = context.DbMonHotels
                .Where(x => x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayarPokok.Value,
                    Nominal = x.NominalPokokBayar ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.JasaPerhotelan, targetDict, hotelData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Hiburan ====
            var hiburanData = context.DbMonHiburans
                .Where(x => x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayarPokok.Value,
                    Nominal = x.NominalPokokBayar ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.JasaKesenianHiburan, targetDict, hiburanData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Parkir ====
            var parkirData = context.DbMonParkirs
                .Where(x => x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayarPokok.Value,
                    Nominal = x.NominalPokokBayar ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.JasaParkir, targetDict, parkirData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Listrik ====
            var listrikData = context.DbMonPpjs
                .Where(x => x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayarPokok.Value,
                    Nominal = x.NominalPokokBayar ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.TenagaListrik, targetDict, listrikData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Reklame ====
            var reklameData = context.DbMonReklames
                .Where(x => x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayarPokok.Value,
                    Nominal = x.NominalPokokBayar ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.Reklame, targetDict, reklameData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== PBB ====
            var pbbData = context.DbMonPbbs
                .Where(x => x.TglBayar != null && x.TglBayar.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayar.Value,
                    Nominal = x.JumlahBayarPokok ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.PBB, targetDict, pbbData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== BPHTB ====
            var bphtbData = context.DbMonBphtbs
                .Where(x => x.TglBayar != null && x.TglBayar.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayar.Value,
                    Nominal = x.Pokok ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.BPHTB, targetDict, bphtbData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Air Tanah ====
            var abtData = context.DbMonAbts
                .Where(x => x.TglBayarPokok != null && x.TglBayarPokok.Value.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglBayarPokok.Value,
                    Nominal = x.NominalPokokBayar ?? 0
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.AirTanah, targetDict, abtData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Opsen PKB ====
            var opsenPkbData = context.DbMonOpsenPkbs
                .Where(x => x.TglSspd.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglSspd,
                    Nominal = x.JmlPokok
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.OpsenPkb, targetDict, opsenPkbData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            // ==== Opsen BBNKB ====
            var opsenBbnkbData = context.DbMonOpsenBbnkbs
                .Where(x => x.TglSspd.Year == currentYear)
                .Select(x => new PajakData
                {
                    Tanggal = x.TglSspd,
                    Nominal = x.JmlPokok
                })
                .ToList();

            list.Add(BuildPrediksi(EnumFactory.EPajak.OpsenBbnkb, targetDict, opsenBbnkbData, bulanLaluCutoff, tanggalAwal, tanggalAkhir));

            return list;
        }

        private static KontrolPrediksi BuildPrediksi(EnumFactory.EPajak jenisPajak,
            Dictionary<int, decimal> targetDict,
            List<PajakData> data,
            DateTime bulanLaluCutoff,
            DateTime tanggalAwal,
            DateTime tanggalAkhir)
        {
            var currentYear = DateTime.Now.Year;
            var awalTahun = new DateTime(currentYear, 1, 1);
            var id = (int)jenisPajak;

            // Batas hari ini
            var startOfDay = tanggalAwal.Date; // 00:00
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); // 23:59:59.9999999

            var realisasiBulanLalu = data.Where(d => d.Tanggal <= bulanLaluCutoff).Sum(d => d.Nominal);
            var realisasiBulanIni = data.Where(d => d.Tanggal >= awalTahun && d.Tanggal < startOfDay).Sum(d => d.Nominal);
            var realisasiHariIni = data.Where(d => d.Tanggal >= startOfDay && d.Tanggal <= endOfDay).Sum(d => d.Nominal);

            return new KontrolPrediksi
            {
                tgl = DateTime.Now,
                JenisPajak = jenisPajak.ToString(),
                Target = targetDict.ContainsKey(id) ? targetDict[id] : 0,
                RealisasiBulanLalu = realisasiBulanLalu,
                RealisasiBulanIni = realisasiBulanIni,
                RealisasiHari = realisasiHariIni
            };

        }

    }

    public class Dashboard
    {
        public int TotalTarget { get; set; }
        public double TotalRealisasi { get; set; }
        public double RataRataPencapaian { get; set; }

    }
    public class PajakData
    {
        public DateTime Tanggal { get; set; }
        public decimal Nominal { get; set; }
    }
    public class KontrolPrediksi
    {
        public DateTime tgl { get; set; }
        public string JenisPajak { get; set; } = null!;
        public decimal Target { get; set; }
        public decimal RealisasiBulanLalu { get; set; }
        public decimal RealisasiBulanIni { get; set; }
        public decimal RealisasiHari { get; set; }
        public decimal Jumlah => RealisasiBulanIni + RealisasiHari;
        public decimal Persentase => Target > 0
            ? Math.Round((Jumlah / Target) * 100, 2)
            : 0;
    }
}
