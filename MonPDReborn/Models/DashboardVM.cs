using MonPDLib;
using MonPDLib.General;

namespace MonPDReborn.Models
{
    public class DashboardVM
    {
        public class Index
        {
            public ViewModel.Dashboard Data { get; set; } = new ViewModel.Dashboard();
            public ViewModel.DashboardChart ChartData { get; set; } = new ViewModel.DashboardChart();
            public Index()
            {
                Data = Method.GetDashboardData();
                ChartData = Method.GetDashboardChartData();
            }
        }
        public class SeriesPajakDaerah
        {
            public List<ViewModel.SeriesPajakDaerah> Data { get; set; } = new List<ViewModel.SeriesPajakDaerah>();
            public SeriesPajakDaerah()
            {
                Data = Method.GetSeriesPajakDaerahData();
            }
        }
        public class JumlahObjekPajakTahunan
        {
            public List<ViewModel.JumlahObjekPajakTahunan> Data { get; set; } = new List<ViewModel.JumlahObjekPajakTahunan>();
            public JumlahObjekPajakTahunan()
            {
                Data = Method.GetJumlahObjekPajakTahunanData();
            }
        }
        public class JumlahObjekPajakSeries
        {
            public List<ViewModel.JumlahObjekPajakSeries> Data { get; set; } = new List<ViewModel.JumlahObjekPajakSeries>();
            public JumlahObjekPajakSeries()
            {
                Data = Method.GetJumlahObjekPajakSeriesData();
            }
        }
        public class PemasanganAlatRekamDetail
        {
            public List<ViewModel.PemasanganAlatRekamDetail> Data { get; set; } = new List<ViewModel.PemasanganAlatRekamDetail>();
            public PemasanganAlatRekamDetail()
            {
                Data = Method.GetPemasanganAlatRekamDetailData();
            }
        }
        public class PemasanganAlatRekamSeries
        {
            public List<ViewModel.PemasanganAlatRekamSeries> Data { get; set; } = new List<ViewModel.PemasanganAlatRekamSeries>();
            public PemasanganAlatRekamSeries()
            {
                Data = Method.GetPemasanganAlatRekamSeriesData();
            }
        }
        public class PemeriksaanPajak
        {
            public List<ViewModel.PemeriksaanPajak> Data { get; set; } = new List<ViewModel.PemeriksaanPajak>();
            public PemeriksaanPajak()
            {
                Data = Method.GetPemeriksaanPajakData();
            }
        }
        public class PengedokanPajak
        {
            public List<ViewModel.PengedokanPajak> Data { get; set; } = new List<ViewModel.PengedokanPajak>();
            public PengedokanPajak()
            {
                Data = Method.GetPengedokanPajakData();
            }
        }
        public class DataKontrolPotensiOp
        {
            public List<ViewModel.DataKontrolPotensiOp> Data { get; set; } = new List<ViewModel.DataKontrolPotensiOp>();
            public DataKontrolPotensiOp()
            {
                Data = Method.GetDataKontrolPotensiOp();
            }
        }
        public class DataPiutang
        {
            public List<ViewModel.DataPiutang> Data { get; set; } = new List<ViewModel.DataPiutang>();
            public DataPiutang()
            {
                Data = Method.GetDataPiutangData();
            }
        }
        public class DataMutasi
        {
            public List<ViewModel.DataMutasi> Data { get; set; } = new List<ViewModel.DataMutasi>();
            public DataMutasi()
            {
                Data = Method.GetDataMutasiData();
            }
        }



        public class ViewModel
        {
            public class X
            {

            }
            public class Dashboard
            {
                public decimal TotalTarget { get; set; }
                public decimal TotalRealisasi { get; set; }
                public decimal TotalPersentase { get; set; }
                public decimal TargetHotel { get; set; }
                public decimal RealisasiHotel { get; set; }
                public decimal PersentaseHotel { get; set; }
                public decimal TargetHiburan { get; set; }
                public decimal RealisasiHiburan { get; set; }
                public decimal PersentaseHiburan { get; set; }
                public decimal TargetParkir { get; set; }
                public decimal RealisasiParkir { get; set; }
                public decimal PersentaseParkir { get; set; }
                public decimal TargetMamin { get; set; }
                public decimal RealisasiMamin { get; set; }
                public decimal PersentaseMamin { get; set; }
                public decimal TargetListrik { get; set; }
                public decimal RealisasiListrik { get; set; }
                public decimal PersentaseListrik { get; set; }
                public decimal TargetAbt { get; set; }
                public decimal RealisasiAbt { get; set; }
                public decimal PersentaseAbt { get; set; }
                public decimal TargetPbb { get; set; }
                public decimal RealisasiPbb { get; set; }
                public decimal PersentasePbb { get; set; }
                public decimal TargetReklame { get; set; }
                public decimal RealisasiReklame { get; set; }
                public decimal PersentaseReklame { get; set; }
                public decimal TargetBphtb { get; set; }
                public decimal RealisasiBphtb { get; set; }
                public decimal PersentaseBphtb { get; set; }
                public decimal TargetOpsenPkb { get; set; }
                public decimal RealisasiOpsenPkb { get; set; }
                public decimal PersentaseOpsenPkb { get; set; }
                public decimal TargetOpsenBbnkb { get; set; }
                public decimal RealisasiOpsenBbnkb { get; set; }
                public decimal PersentaseOpsenBbnkb { get; set; }
            }
            public class DashboardChart
            {
                public decimal Target1 { get; set; }
                public decimal Target2 { get; set; }
                public decimal Target3 { get; set; }
                public decimal Target4 { get; set; }
                public decimal Target5 { get; set; }
                public decimal Target6 { get; set; }
                public decimal Target7 { get; set; }
                public decimal Target8 { get; set; }
                public decimal Target9 { get; set; }
                public decimal Target10 { get; set; }
                public decimal Target11 { get; set; }
                public decimal Target12 { get; set; }
                public decimal Realisasi1 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Realisasi3 { get; set; }
                public decimal Realisasi4 { get; set; }
                public decimal Realisasi5 { get; set; }
                public decimal Realisasi6 { get; set; }
                public decimal Realisasi7 { get; set; }
                public decimal Realisasi8 { get; set; }
                public decimal Realisasi9 { get; set; }
                public decimal Realisasi10 { get; set; }
                public decimal Realisasi11 { get; set; }
                public decimal Realisasi12 { get; set; }
            }
            public class SeriesPajakDaerah
            {
                public string JenisPajak { get; set; } = "";
                public decimal Target1 { get; set; }
                public decimal Target2 { get; set; }
                public decimal Target3 { get; set; }
                public decimal Target4 { get; set; }
                public decimal Target5 { get; set; }
                public decimal Realisasi1 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Realisasi3 { get; set; }
                public decimal Realisasi4 { get; set; }
                public decimal Realisasi5 { get; set; }
                public decimal Persentase1 { get; set; }
                public decimal Persentase2 { get; set; }
                public decimal Persentase3 { get; set; }
                public decimal Persentase4 { get; set; }
                public decimal Persentase5 { get; set; }
            }
            public class JumlahObjekPajakTahunan
            {
                public string JenisPajak { get; set; } = "";
                public decimal JmlOpAwal { get; set; }
                public decimal JmlOpTutupSementara { get; set; }
                public decimal JmlOpTutupPermanen { get; set; }
                public decimal JmlOpBaru { get; set; }
                public decimal JmlOpAkhir { get; set; }
            }
            public class JumlahObjekPajakSeries
            {
                public string JenisPajak { get; set; }
                public decimal Jumlah1 { get; set; }
                public decimal Jumlah2 { get; set; }
                public decimal Jumlah3 { get; set; }
                public decimal Jumlah4 { get; set; }
                public decimal Jumlah5 { get; set; }
            }
            public class PemasanganAlatRekamSeries
            {
                public string JenisPajak { get; set; }
                public decimal Jumlah1 { get; set; }
                public decimal Jumlah2 { get; set; }
                public decimal Jumlah3 { get; set; }
                public decimal Jumlah4 { get; set; }
                public decimal Jumlah5 { get; set; }
            }
            public class PemasanganAlatRekamDetail
            {
                public string JenisPajak { get; set; } = "";
                public int JmlOp { get; set; }
                public int JmlTerpasangTS { get; set; }
                public int JmlTerpasangTB { get; set; }
                public int JmlTerpasangSB { get; set; }
                public int TotalTerpasang { get; set; }
                public int TotalBelumTerpasang { get; set; }
            }
            public class PemeriksaanPajak
            {
                public string JenisPajak { get; set; } = "";
                public decimal OpPeriksa1 { get; set; }
                public decimal OpPeriksa2 { get; set; }
                public decimal OpPeriksa3 { get; set; }
                public decimal Pokok1 { get; set; }
                public decimal Pokok2 { get; set; }
                public decimal Pokok3 { get; set; }
                public decimal Sanksi1 { get; set; }
                public decimal Sanksi2 { get; set; }
                public decimal Sanksi3 { get; set; }
                public decimal Total1 { get; set; }
                public decimal Total2 { get; set; }
                public decimal Total3 { get; set; }
            }
            public class PengedokanPajak
            {
                public string JenisPajak { get; set; } = "";
                public decimal JmlOp { get; set; }
                public decimal PotensiHasilPengedokan { get; set; }
                public decimal TotalRealisasi { get; set; }
                public decimal Selisih { get; set; }
            }
            public class DataKontrolPotensiOp
            {
                public string JenisPajak { get; set; } = "";
                public decimal Target1 { get; set; }
                public decimal Realisasi1 { get; set; }
                public decimal Selisih1 { get; set; }
                public decimal Persentase1 { get; set; }
                public decimal Target2 { get; set; }
                public decimal Realisasi2 { get; set; }
                public decimal Selisih2 { get; set; }
                public decimal Persentase2 { get; set; }
            }
            public class DataPiutang
            {
                public string JenisPajak { get; set; } = "";
                public decimal NominalPiutang1 { get; set; }
                public decimal NominalPiutang2 { get; set; }
                public decimal NominalPiutang3 { get; set; }
                public decimal NominalPiutang4 { get; set; }
            }
            public class DataMutasi
            {
                public string Keterangan { get; set; } = "";
                public decimal NominalMutasi1 { get; set; }
                public decimal NominalMutasi2 { get; set; }
                public decimal NominalMutasi3 { get; set; }
                public decimal NominalMutasi4 { get; set; }
            }
        }

        public class Method
        {
            public static ViewModel.Dashboard GetDashboardData()
            {
                var context = DBClass.GetContext();

                // Target
                var dataTargetMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataTargetHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataTargetHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataTargetParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataTargetListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataTargetReklame = 0;
                var dataTargetPbb = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataTargetBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == DateTime.Now.Year).Sum(x => x.Pokok) ?? 0;
                var dataTargetAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataTargetOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == DateTime.Now.Year).Sum(x => x.JmlPokok);
                var dataTargetOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == DateTime.Now.Year).Sum(x => x.JmlPokok);

                // Realisasi
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiReklame = 0m;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == DateTime.Now.Year).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == DateTime.Now.Year).Sum(x => x.JmlPokok);
                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == DateTime.Now.Year).Sum(x => x.JmlPokok);

                // Total keseluruhan
                decimal TotalTarget = dataTargetMamin + dataTargetHotel + dataTargetHiburan + dataTargetParkir + dataTargetListrik + dataTargetReklame
                                    + dataTargetPbb + dataTargetBphtb + dataTargetAbt + dataTargetOpsenPkb + dataTargetOpsenBbnkb;

                decimal TotalRealisasi = dataRealisasiMamin + dataRealisasiHotel + dataRealisasiHiburan + dataRealisasiParkir + dataRealisasiListrik + dataRealisasiReklame
                                       + dataRealisasiPbb + dataRealisasiBphtb + dataRealisasiAbt + dataRealisasiOpsenPkb + dataRealisasiOpsenBbnkb;

                decimal TotalPersentase = TotalTarget != 0 ? (TotalRealisasi / TotalTarget) * 100 : 0;

                // Hasil akhir ViewModel
                var result = new ViewModel.Dashboard
                {
                    TotalTarget = TotalTarget,
                    TotalRealisasi = TotalRealisasi,
                    TotalPersentase = Math.Round(TotalPersentase, 2),

                    TargetHotel = dataTargetHotel,
                    RealisasiHotel = dataRealisasiHotel,
                    PersentaseHotel = dataTargetHotel != 0 ? Math.Round((dataRealisasiHotel / dataTargetHotel) * 100, 2) : 0,

                    TargetHiburan = dataTargetHiburan,
                    RealisasiHiburan = dataRealisasiHiburan,
                    PersentaseHiburan = dataTargetHiburan != 0 ? Math.Round((dataRealisasiHiburan / dataTargetHiburan) * 100, 2) : 0,

                    TargetParkir = dataTargetParkir,
                    RealisasiParkir = dataRealisasiParkir,
                    PersentaseParkir = dataTargetParkir != 0 ? Math.Round((dataRealisasiParkir / dataTargetParkir) * 100, 2) : 0,

                    TargetMamin = dataTargetMamin,
                    RealisasiMamin = dataRealisasiMamin,
                    PersentaseMamin = dataTargetMamin != 0 ? Math.Round((dataRealisasiMamin / dataTargetMamin) * 100, 2) : 0,

                    TargetListrik = dataTargetListrik,
                    RealisasiListrik = dataRealisasiListrik,
                    PersentaseListrik = dataTargetListrik != 0 ? Math.Round((dataRealisasiListrik / dataTargetListrik) * 100, 2) : 0,

                    TargetAbt = dataTargetAbt,
                    RealisasiAbt = dataRealisasiAbt,
                    PersentaseAbt = dataTargetAbt != 0 ? Math.Round((dataRealisasiAbt / dataTargetAbt) * 100, 2) : 0,

                    TargetPbb = dataTargetPbb,
                    RealisasiPbb = dataRealisasiPbb,
                    PersentasePbb = dataTargetPbb != 0 ? Math.Round((dataRealisasiPbb / dataTargetPbb) * 100, 2) : 0,

                    TargetBphtb = dataTargetBphtb,
                    RealisasiBphtb = dataRealisasiBphtb,
                    PersentaseBphtb = dataTargetBphtb != 0 ? Math.Round((dataRealisasiBphtb / dataTargetBphtb) * 100, 2) : 0,

                    TargetOpsenPkb = dataTargetOpsenPkb,
                    RealisasiOpsenPkb = dataRealisasiOpsenPkb,
                    PersentaseOpsenPkb = dataTargetOpsenPkb != 0 ? Math.Round((dataRealisasiOpsenPkb / dataTargetOpsenPkb) * 100, 2) : 0,

                    TargetOpsenBbnkb = dataTargetOpsenBbnkb,
                    RealisasiOpsenBbnkb = dataRealisasiOpsenBbnkb,
                    PersentaseOpsenBbnkb = dataTargetOpsenBbnkb != 0 ? Math.Round((dataRealisasiOpsenBbnkb / dataTargetOpsenBbnkb) * 100, 2) : 0,

                    //Optional: Uncomment if you want to include Reklame later
                    TargetReklame = dataTargetReklame,
                    RealisasiReklame = dataRealisasiReklame,
                    PersentaseReklame = dataTargetReklame != 0 ? Math.Round((dataRealisasiReklame / dataTargetReklame) * 100, 2) : 0,
                };

                return result;
            }
            public static ViewModel.DashboardChart GetDashboardChartData()
            {
                var context = DBClass.GetContext();
                int currentYear = DateTime.Now.Year;
                decimal monthlyTarget = 100_000_000;

                var monthlyRealisasi = new decimal[12];

                // Mamin
                var resto = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Hotel
                var hotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Hiburan
                var hiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Parkir
                var parkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Listrik (PPJ)
                var ppj = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // PBB
                var pbb = context.DbMonPbbs
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // BPHTB
                var bphtb = context.DbMonBphtbs
                    .Where(x => x.TglBayar.HasValue && x.TglBayar.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayar.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.Pokok ?? 0) });

                // ABT
                var abt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == currentYear)
                    .GroupBy(x => x.TglBayarPokok.Value.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.NominalPokokBayar ?? 0) });

                // Opsen PKB
                var opsenPkb = context.DbMonOpsenPkbs
                    .Where(x => x.TglSspd.Year == currentYear)
                    .GroupBy(x => x.TglSspd.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.JmlPokok) });

                // Opsen BBNKB
                var opsenBbnkb = context.DbMonOpsenBbnkbs
                    .Where(x => x.TglSspd.Year == currentYear)
                    .GroupBy(x => x.TglSspd.Month)
                    .Select(g => new { Month = g.Key, Total = g.Sum(x => x.JmlPokok) });

                // Gabungkan semua data pajak ke monthlyRealisasi
                void Tambah(IEnumerable<dynamic> data)
                {
                    foreach (var d in data)
                    {
                        int idx = d.Month - 1;
                        if (idx >= 0 && idx < 12)
                            monthlyRealisasi[idx] += d.Total;
                    }
                }

                Tambah(resto);
                Tambah(hotel);
                Tambah(hiburan);
                Tambah(parkir);
                Tambah(ppj);
                Tambah(pbb);
                Tambah(bphtb);
                Tambah(abt);
                Tambah(opsenPkb);
                Tambah(opsenBbnkb);
                // Tidak ada data reklame, jadi tidak dimasukkan

                // Bangun ViewModel
                var result = new ViewModel.DashboardChart();
                for (int i = 0; i < 12; i++)
                {
                    typeof(ViewModel.DashboardChart).GetProperty($"Target{i + 1}")?.SetValue(result, monthlyTarget);
                    typeof(ViewModel.DashboardChart).GetProperty($"Realisasi{i + 1}")?.SetValue(result, monthlyRealisasi[i]);
                }

                return result;
            }
            public static List<ViewModel.SeriesPajakDaerah> GetSeriesPajakDaerahData(int g)
            {
                return new List<ViewModel.SeriesPajakDaerah>
                {
                    new ViewModel.SeriesPajakDaerah
                    {
                        JenisPajak = "Pajak Restoran",
                        Target1 = 80000000,
                        Target2 = 100000000,
                        Target3 = 110000000,
                        Target4 = 130000000,
                        Target5 = 150000000,
                        Realisasi1 = 70000000,
                        Realisasi2 = 90000000,
                        Realisasi3 = 100000000,
                        Realisasi4 = 110000000,
                        Realisasi5 = 120000000,
                        Persentase1 = Math.Round(70000000m / 80000000m * 100, 2),
                        Persentase2 = Math.Round(90000000m / 100000000m * 100, 2),
                        Persentase3 = Math.Round(100000000m / 110000000m * 100, 2),
                        Persentase4 = Math.Round(110000000m / 130000000m * 100, 2),
                        Persentase5 = Math.Round(120000000m / 150000000m * 10, 2)
                    },
                    new ViewModel.SeriesPajakDaerah
                    {
                        JenisPajak = "Pajak Hotel",
                        Target1 = 80000000,
                        Target2 = 100000000,
                        Target3 = 110000000,
                        Target4 = 130000000,
                        Target5 = 150000000,
                        Realisasi1 = 70000000,
                        Realisasi2 = 90000000,
                        Realisasi3 = 100000000,
                        Realisasi4 = 110000000,
                        Realisasi5 = 120000000,
                        Persentase1 = Math.Round(70000000m / 80000000m * 100, 2),
                        Persentase2 = Math.Round(90000000m / 100000000m * 100, 2),
                        Persentase3 = Math.Round(100000000m / 110000000m * 100, 2),
                        Persentase4 = Math.Round(110000000m / 130000000m * 100, 2),
                        Persentase5 = Math.Round(120000000m / 150000000m * 100, 2)
                    },
                };
            }
            public static List<ViewModel.SeriesPajakDaerah> GetSeriesPajakDaerahData()
            {

                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                List<ViewModel.SeriesPajakDaerah> result = new();

                #region Now
                var targetRestoNow = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiRestoNow = targetRestoNow;

                var targetHotelNow = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHotelNow = targetHotelNow;

                var targetHiburanNow = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHiburanNow = targetHiburanNow;

                var targetParkirNow = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiParkirNow = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikNow = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiListrikNow = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbNow = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiPbbNow = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbNow = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear).Sum(x => x.Pokok) ?? 0;
                var realisasiBphtbNow = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear).Sum(x => x.Pokok) ?? 0;

                var targetAbtNow = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiAbtNow = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbNow = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);
                var realisasiOpsenPkbNow = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);
                var realisasiOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);
                #endregion

                #region Mines1
                var targetRestoMines1 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiRestoMines1 = targetRestoMines1;

                var targetHotelMines1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHotelMines1 = targetHotelMines1;

                var targetHiburanMines1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHiburanMines1 = targetHiburanMines1;

                var targetParkirMines1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiParkirMines1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiListrikMines1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines1 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiPbbMines1 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines1 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 1).Sum(x => x.Pokok) ?? 0;
                var realisasiBphtbMines1 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 1).Sum(x => x.Pokok) ?? 0;

                var targetAbtMines1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiAbtMines1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines1 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);
                var realisasiOpsenPkbMines1 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines1 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);
                var realisasiOpsenBbnkbMines1 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);
                #endregion

                #region Mines2
                var targetRestoMines2 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiRestoMines2 = targetRestoMines2;

                var targetHotelMines2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHotelMines2 = targetHotelMines2;

                var targetHiburanMines2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHiburanMines2 = targetHiburanMines2;

                var targetParkirMines2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiParkirMines2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiListrikMines2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines2 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiPbbMines2 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines2 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 2).Sum(x => x.Pokok) ?? 0;
                var realisasiBphtbMines2 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 2).Sum(x => x.Pokok) ?? 0;

                var targetAbtMines2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiAbtMines2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines2 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);
                var realisasiOpsenPkbMines2 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines2 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);
                var realisasiOpsenBbnkbMines2 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);
                #endregion

                #region Mines3
                var targetRestoMines3 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiRestoMines3 = targetRestoMines3;

                var targetHotelMines3 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHotelMines3 = targetHotelMines3;

                var targetHiburanMines3 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHiburanMines3 = targetHiburanMines3;

                var targetParkirMines3 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiParkirMines3 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines3 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiListrikMines3 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines3 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiPbbMines3 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines3 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 3).Sum(x => x.Pokok) ?? 0;
                var realisasiBphtbMines3 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 3).Sum(x => x.Pokok) ?? 0;

                var targetAbtMines3 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiAbtMines3 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 3).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines3 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 3).Sum(x => x.JmlPokok);
                var realisasiOpsenPkbMines3 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 3).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines3 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 3).Sum(x => x.JmlPokok);
                var realisasiOpsenBbnkbMines3 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 3).Sum(x => x.JmlPokok);
                #endregion

                #region Mines4
                var targetRestoMines4 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiRestoMines4 = targetRestoMines4;

                var targetHotelMines4 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHotelMines4 = targetHotelMines4;

                var targetHiburanMines4 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHiburanMines4 = targetHiburanMines4;

                var targetParkirMines4 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiParkirMines4 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines4 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiListrikMines4 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetPbbMines4 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiPbbMines4 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetBphtbMines4 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 4).Sum(x => x.Pokok) ?? 0;
                var realisasiBphtbMines4 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 4).Sum(x => x.Pokok) ?? 0;

                var targetAbtMines4 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiAbtMines4 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 4).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetOpsenPkbMines4 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 4).Sum(x => x.JmlPokok);
                var realisasiOpsenPkbMines4 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 4).Sum(x => x.JmlPokok);

                var targetOpsenBbnkbMines4 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 4).Sum(x => x.JmlPokok);
                var realisasiOpsenBbnkbMines4 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 4).Sum(x => x.JmlPokok);
                #endregion


                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    Target5 = targetRestoNow,
                    Realisasi5 = realisasiRestoNow,
                    Persentase5 = targetRestoNow != 0 ? Math.Round(realisasiRestoNow / targetRestoNow * 100, 2) : 0,
                    Target4 = targetRestoMines1,
                    Realisasi4 = realisasiRestoMines1,
                    Persentase4 = targetRestoMines1 != 0 ? Math.Round(realisasiRestoMines1 / targetRestoMines1 * 100, 2) : 0,
                    Target3 = targetRestoMines2,
                    Realisasi3 = realisasiRestoMines2,
                    Persentase3 = targetRestoMines2 != 0 ? Math.Round(realisasiRestoMines2 / targetRestoMines2 * 100, 2) : 0,
                    Target2 = targetRestoMines3,
                    Realisasi2 = realisasiRestoMines3,
                    Persentase2 = targetRestoMines3 != 0 ? Math.Round(realisasiRestoMines3 / targetRestoMines3 * 100, 2) : 0,
                    Target1 = targetRestoMines4,
                    Realisasi1 = realisasiRestoMines4,
                    Persentase1 = targetRestoMines4 != 0 ? Math.Round(realisasiRestoMines4 / targetRestoMines4 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),,
                    Target5 = targetHotelNow,
                    Realisasi5 = realisasiHotelNow,
                    Persentase5 = targetHotelNow != 0 ? Math.Round(realisasiHotelNow / targetHotelNow * 100, 2) : 0,
                    Target4 = targetHotelMines1,
                    Realisasi4 = realisasiHotelMines1,
                    Persentase4 = targetHotelMines1 != 0 ? Math.Round(realisasiHotelMines1 / targetHotelMines1 * 100, 2) : 0,
                    Target3 = targetHotelMines2,
                    Realisasi3 = realisasiHotelMines2,
                    Persentase3 = targetHotelMines2 != 0 ? Math.Round(realisasiHotelMines2 / targetHotelMines2 * 100, 2) : 0,
                    Target2 = targetHotelMines3,
                    Realisasi2 = realisasiHotelMines3,
                    Persentase2 = targetHotelMines3 != 0 ? Math.Round(realisasiHotelMines3 / targetHotelMines3 * 100, 2) : 0,
                    Target1 = targetHotelMines4,
                    Realisasi1 = realisasiHotelMines4,
                    Persentase1 = targetHotelMines4 != 0 ? Math.Round(realisasiHotelMines4 / targetHotelMines4 * 100, 2) : 0,
                });


                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    Target5 = targetHiburanNow,
                    Realisasi5 = realisasiHiburanNow,
                    Persentase5 = targetHiburanNow != 0 ? Math.Round(realisasiHiburanNow / targetHiburanNow * 100, 2) : 0,
                    Target4 = targetHiburanMines1,
                    Realisasi4 = realisasiHiburanMines1,
                    Persentase4 = targetHiburanMines1 != 0 ? Math.Round(realisasiHiburanMines1 / targetHiburanMines1 * 100, 2) : 0,
                    Target3 = targetHiburanMines2,
                    Realisasi3 = realisasiHiburanMines2,
                    Persentase3 = targetHiburanMines2 != 0 ? Math.Round(realisasiHiburanMines2 / targetHiburanMines2 * 100, 2) : 0,
                    Target2 = targetHiburanMines3,
                    Realisasi2 = realisasiHiburanMines3,
                    Persentase2 = targetHiburanMines3 != 0 ? Math.Round(realisasiHiburanMines3 / targetHiburanMines3 * 100, 2) : 0,
                    Target1 = targetHiburanMines4,
                    Realisasi1 = realisasiHiburanMines4,
                    Persentase1 = targetHiburanMines4 != 0 ? Math.Round(realisasiHiburanMines4 / targetHiburanMines4 * 100, 2) : 0,
                });


                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    Target5 = targetParkirNow,
                    Realisasi5 = realisasiParkirNow,
                    Persentase5 = targetParkirNow != 0 ? Math.Round(realisasiParkirNow / targetParkirNow * 100, 2) : 0,
                    Target4 = targetParkirMines1,
                    Realisasi4 = realisasiParkirMines1,
                    Persentase4 = targetParkirMines1 != 0 ? Math.Round(realisasiParkirMines1 / targetParkirMines1 * 100, 2) : 0,
                    Target3 = targetParkirMines2,
                    Realisasi3 = realisasiParkirMines2,
                    Persentase3 = targetParkirMines2 != 0 ? Math.Round(realisasiParkirMines2 / targetParkirMines2 * 100, 2) : 0,
                    Target2 = targetParkirMines3,
                    Realisasi2 = realisasiParkirMines3,
                    Persentase2 = targetParkirMines3 != 0 ? Math.Round(realisasiParkirMines3 / targetParkirMines3 * 100, 2) : 0,
                    Target1 = targetParkirMines4,
                    Realisasi1 = realisasiParkirMines4,
                    Persentase1 = targetParkirMines4 != 0 ? Math.Round(realisasiParkirMines4 / targetParkirMines4 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    Target5 = targetListrikNow,
                    Realisasi5 = realisasiListrikNow,
                    Persentase5 = targetListrikNow != 0 ? Math.Round(realisasiListrikNow / targetListrikNow * 100, 2) : 0,
                    Target4 = targetListrikMines1,
                    Realisasi4 = realisasiListrikMines1,
                    Persentase4 = targetListrikMines1 != 0 ? Math.Round(realisasiListrikMines1 / targetListrikMines1 * 100, 2) : 0,
                    Target3 = targetListrikMines2,
                    Realisasi3 = realisasiListrikMines2,
                    Persentase3 = targetListrikMines2 != 0 ? Math.Round(realisasiListrikMines2 / targetListrikMines2 * 100, 2) : 0,
                    Target2 = targetListrikMines3,
                    Realisasi2 = realisasiListrikMines3,
                    Persentase2 = targetListrikMines3 != 0 ? Math.Round(realisasiListrikMines3 / targetListrikMines3 * 100, 2) : 0,
                    Target1 = targetListrikMines4,
                    Realisasi1 = realisasiListrikMines4,
                    Persentase1 = targetListrikMines4 != 0 ? Math.Round(realisasiListrikMines4 / targetListrikMines4 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    Target5 = targetPbbNow,
                    Realisasi5 = realisasiPbbNow,
                    Persentase5 = targetPbbNow != 0 ? Math.Round(realisasiPbbNow / targetPbbNow * 100, 2) : 0,
                    Target4 = targetPbbMines1,
                    Realisasi4 = realisasiPbbMines1,
                    Persentase4 = targetPbbMines1 != 0 ? Math.Round(realisasiPbbMines1 / targetPbbMines1 * 100, 2) : 0,
                    Target3 = targetPbbMines2,
                    Realisasi3 = realisasiPbbMines2,
                    Persentase3 = targetPbbMines2 != 0 ? Math.Round(realisasiPbbMines2 / targetPbbMines2 * 100, 2) : 0,
                    Target2 = targetPbbMines3,
                    Realisasi2 = realisasiPbbMines3,
                    Persentase2 = targetPbbMines3 != 0 ? Math.Round(realisasiPbbMines3 / targetPbbMines3 * 100, 2) : 0,
                    Target1 = targetPbbMines4,
                    Realisasi1 = realisasiPbbMines4,
                    Persentase1 = targetPbbMines4 != 0 ? Math.Round(realisasiPbbMines4 / targetPbbMines4 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    Target5 = targetBphtbNow,
                    Realisasi5 = realisasiBphtbNow,
                    Persentase5 = targetBphtbNow != 0 ? Math.Round(realisasiBphtbNow / targetBphtbNow * 100, 2) : 0,
                    Target4 = targetBphtbMines1,
                    Realisasi4 = realisasiBphtbMines1,
                    Persentase4 = targetBphtbMines1 != 0 ? Math.Round(realisasiBphtbMines1 / targetBphtbMines1 * 100, 2) : 0,
                    Target3 = targetBphtbMines2,
                    Realisasi3 = realisasiBphtbMines2,
                    Persentase3 = targetBphtbMines2 != 0 ? Math.Round(realisasiBphtbMines2 / targetBphtbMines2 * 100, 2) : 0,
                    Target2 = targetBphtbMines3,
                    Realisasi2 = realisasiBphtbMines3,
                    Persentase2 = targetBphtbMines3 != 0 ? Math.Round(realisasiBphtbMines3 / targetBphtbMines3 * 100, 2) : 0,
                    Target1 = targetBphtbMines4,
                    Realisasi1 = realisasiBphtbMines4,
                    Persentase1 = targetBphtbMines4 != 0 ? Math.Round(realisasiBphtbMines4 / targetBphtbMines4 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    Target5 = 0,
                    Realisasi5 = 0,
                    Persentase5 = 0,
                    Target4 = 0,
                    Realisasi4 = 0,
                    Persentase4 = 0,
                    Target3 = 0,
                    Realisasi3 = 0,
                    Persentase3 = 0,
                    Target2 = 0,
                    Realisasi2 = 0,
                    Persentase2 = 0,
                    Target1 = 0,
                    Realisasi1 = 0,
                    Persentase1 = 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    Target5 = targetAbtNow,
                    Realisasi5 = realisasiAbtNow,
                    Persentase5 = targetAbtNow != 0 ? Math.Round(realisasiAbtNow / targetAbtNow * 100, 2) : 0,
                    Target4 = targetAbtMines1,
                    Realisasi4 = realisasiAbtMines1,
                    Persentase4 = targetAbtMines1 != 0 ? Math.Round(realisasiAbtMines1 / targetAbtMines1 * 100, 2) : 0,
                    Target3 = targetAbtMines2,
                    Realisasi3 = realisasiAbtMines2,
                    Persentase3 = targetAbtMines2 != 0 ? Math.Round(realisasiAbtMines2 / targetAbtMines2 * 100, 2) : 0,
                    Target2 = targetAbtMines3,
                    Realisasi2 = realisasiAbtMines3,
                    Persentase2 = targetAbtMines3 != 0 ? Math.Round(realisasiAbtMines3 / targetAbtMines3 * 100, 2) : 0,
                    Target1 = targetAbtMines4,
                    Realisasi1 = realisasiAbtMines4,
                    Persentase1 = targetAbtMines4 != 0 ? Math.Round(realisasiAbtMines4 / targetAbtMines4 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    Target5 = targetOpsenPkbNow,
                    Realisasi5 = realisasiOpsenPkbNow,
                    Persentase5 = targetOpsenPkbNow != 0 ? Math.Round(realisasiOpsenPkbNow / targetOpsenPkbNow * 100, 2) : 0,
                    Target4 = targetOpsenPkbMines1,
                    Realisasi4 = realisasiOpsenPkbMines1,
                    Persentase4 = targetOpsenPkbMines1 != 0 ? Math.Round(realisasiOpsenPkbMines1 / targetOpsenPkbMines1 * 100, 2) : 0,
                    Target3 = targetOpsenPkbMines2,
                    Realisasi3 = realisasiOpsenPkbMines2,
                    Persentase3 = targetOpsenPkbMines2 != 0 ? Math.Round(realisasiOpsenPkbMines2 / targetOpsenPkbMines2 * 100, 2) : 0,
                    Target2 = targetOpsenPkbMines3,
                    Realisasi2 = realisasiOpsenPkbMines3,
                    Persentase2 = targetOpsenPkbMines3 != 0 ? Math.Round(realisasiOpsenPkbMines3 / targetOpsenPkbMines3 * 100, 2) : 0,
                    Target1 = targetOpsenPkbMines4,
                    Realisasi1 = realisasiOpsenPkbMines4,
                    Persentase1 = targetOpsenPkbMines4 != 0 ? Math.Round(realisasiOpsenPkbMines4 / targetOpsenPkbMines4 * 100, 2) : 0,
                });

                result.Add(new ViewModel.SeriesPajakDaerah
                {
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    Target5 = targetOpsenBbnkbNow,
                    Realisasi5 = realisasiOpsenBbnkbNow,
                    Persentase5 = targetOpsenBbnkbNow != 0 ? Math.Round(realisasiOpsenBbnkbNow / targetOpsenBbnkbNow * 100, 2) : 0,
                    Target4 = targetOpsenBbnkbMines1,
                    Realisasi4 = realisasiOpsenBbnkbMines1,
                    Persentase4 = targetOpsenBbnkbMines1 != 0 ? Math.Round(realisasiOpsenBbnkbMines1 / targetOpsenBbnkbMines1 * 100, 2) : 0,
                    Target3 = targetOpsenBbnkbMines2,
                    Realisasi3 = realisasiOpsenBbnkbMines2,
                    Persentase3 = targetOpsenBbnkbMines2 != 0 ? Math.Round(realisasiOpsenBbnkbMines2 / targetOpsenBbnkbMines2 * 100, 2) : 0,
                    Target2 = targetOpsenBbnkbMines3,
                    Realisasi2 = realisasiOpsenBbnkbMines3,
                    Persentase2 = targetOpsenBbnkbMines3 != 0 ? Math.Round(realisasiOpsenBbnkbMines3 / targetOpsenBbnkbMines3 * 100, 2) : 0,
                    Target1 = targetOpsenBbnkbMines4,
                    Realisasi1 = realisasiOpsenBbnkbMines4,
                    Persentase1 = targetOpsenBbnkbMines4 != 0 ? Math.Round(realisasiOpsenBbnkbMines4 / targetOpsenBbnkbMines4 * 100, 2) : 0,
                });

                return result;
            }
            public static List<ViewModel.JumlahObjekPajakTahunan> GetJumlahObjekPajakTahunanData()
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var OpRestoNow = context.DbOpRestos.Count(x => x.TahunBuku == currentYear);
                var OpRestoTutup = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpRestoAwal = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1);

                var OpHotelNow = context.DbOpHotels.Count(x => x.TahunBuku == currentYear);
                var OpHotelTutup = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpHotelAwal = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1);

                var OpHiburanNow = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear);
                var OpHiburanTutup = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpHiburanAwal = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1);

                var OpParkirNow = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear);
                var OpParkirTutup = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpParkirAwal = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1);

                var OpListrikNow = context.DbOpListriks.Count(x => x.TahunBuku == currentYear);
                var OpListrikTutup = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpListrikAwal = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1);

                var OpAbtNow = context.DbOpAbts.Count(x => x.TahunBuku == currentYear);
                var OpAbtTutup = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == currentYear);
                var OpAbtAwal = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1);

                var OpPbbNow = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear);
                var OpPbbAwal = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 1);

                var OpBphtbNow = 0;
                var OpBphtbAwal = 0;
                
                var OpReklameNow = 0;
                var OpReklameAwal = 0;

                var OpOpsenPkbNow = 0;
                var OpOpsenPkbAwal = 0;

                var OpOpsenBbnkbNow = 0;
                var OpOpsenBbnkbAwal = 0;

                return new List<ViewModel.JumlahObjekPajakTahunan>
                {
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                        JmlOpAwal = OpRestoAwal,
                        JmlOpTutupPermanen = OpRestoTutup,
                        JmlOpBaru = OpRestoNow - OpRestoAwal,
                        JmlOpAkhir = OpRestoAwal -OpRestoTutup
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),,
                        JmlOpAwal = OpHotelAwal,
                        JmlOpTutupPermanen = OpHotelTutup,
                        JmlOpBaru = OpHotelNow - OpHotelAwal,
                        JmlOpAkhir = OpHotelAwal - OpHotelTutup
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                        JmlOpAwal = OpHiburanAwal,
                        JmlOpTutupPermanen = OpHiburanTutup,
                        JmlOpBaru = OpHiburanNow - OpHiburanAwal,
                        JmlOpAkhir = OpHiburanAwal - OpHiburanTutup
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                        JmlOpAwal = OpParkirAwal,
                        JmlOpTutupPermanen = OpParkirTutup,
                        JmlOpBaru = OpParkirNow - OpParkirAwal,
                        JmlOpAkhir = OpParkirAwal - OpParkirTutup
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                        JmlOpAwal = OpListrikAwal,
                        JmlOpTutupPermanen = OpListrikTutup,
                        JmlOpBaru = OpListrikNow - OpListrikAwal,
                        JmlOpAkhir = OpListrikAwal - OpListrikTutup
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                        JmlOpAwal = OpPbbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpPbbNow - OpPbbAwal,
                        JmlOpAkhir = OpPbbAwal - 0
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                        JmlOpAwal = OpBphtbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpBphtbNow - OpBphtbAwal,
                        JmlOpAkhir = OpBphtbAwal - 0
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                        JmlOpAwal = OpReklameAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpReklameNow - OpReklameAwal,
                        JmlOpAkhir = OpReklameAwal - 0
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                        JmlOpAwal = OpAbtAwal,
                        JmlOpTutupPermanen = OpAbtTutup,
                        JmlOpBaru = OpAbtNow - OpAbtAwal,
                        JmlOpAkhir = OpAbtAwal - OpAbtTutup
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                        JmlOpAwal = OpOpsenPkbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpOpsenPkbNow - OpOpsenPkbAwal,
                        JmlOpAkhir = OpOpsenPkbAwal - 0
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                        JmlOpAwal = OpOpsenBbnkbAwal,
                        JmlOpTutupPermanen = 0,
                        JmlOpBaru = OpOpsenBbnkbNow - OpOpsenBbnkbAwal,
                        JmlOpAkhir = OpOpsenBbnkbAwal - 0
                    },
                };
            }
            public static List<ViewModel.JumlahObjekPajakSeries> GetJumlahObjekPajakSeriesData()
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var OpRestoNow = context.DbOpRestos.Count(x => x.TahunBuku == currentYear);
                var OpRestoMines1 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1);
                var OpRestoMines2 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 2);
                var OpRestoMines3 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 3);
                var OpRestoMines4 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 4);

                var OpHotelNow = context.DbOpHotels.Count(x => x.TahunBuku == currentYear);
                var OpHotelMines1 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1);
                var OpHotelMines2 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 2);
                var OpHotelMines3 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 3);
                var OpHotelMines4 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 4);

                var OpHiburanNow = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear);
                var OpHiburanMines1 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1);
                var OpHiburanMines2 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 2);
                var OpHiburanMines3 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 3);
                var OpHiburanMines4 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 4);

                var OpParkirNow = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear);
                var OpParkirMines1 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1);
                var OpParkirMines2 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 2);
                var OpParkirMines3 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 3);
                var OpParkirMines4 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 4);

                var OpListrikNow = context.DbOpListriks.Count(x => x.TahunBuku == currentYear);
                var OpListrikMines1 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1);
                var OpListrikMines2 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 2);
                var OpListrikMines3 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 3);
                var OpListrikMines4 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 4);

                var OpAbtNow = context.DbOpAbts.Count(x => x.TahunBuku == currentYear);
                var OpAbtMines1 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1);
                var OpAbtMines2 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 2);
                var OpAbtMines3 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 3);
                var OpAbtMines4 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 4);

                var OpPbbNow = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear);
                var OpPbbMines1 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 1);
                var OpPbbMines2 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 2);
                var OpPbbMines3 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 3);
                var OpPbbMines4 = context.DbOpPbbs.Count(x => x.TahunBuku == currentYear - 4);

                var OpBphtbNow = 0;
                var OpBphtbMines1 = 0;
                var OpBphtbMines2 = 0;
                var OpBphtbMines3 = 0;
                var OpBphtbMines4 = 0;
                
                var OpReklameNow = 0;
                var OpReklameMines1 = 0;
                var OpReklameMines2 = 0;
                var OpReklameMines3 = 0;
                var OpReklameMines4 = 0;

                var OpOpsenPkbNow = 0;
                var OpOpsenPkbMines1 = 0;
                var OpOpsenPkbMines2 = 0;
                var OpOpsenPkbMines3 = 0;
                var OpOpsenPkbMines4 = 0;

                var OpOpsenBbnkbNow = 0;
                var OpOpsenBbnkbMines1 = 0;
                var OpOpsenBbnkbMines2 = 0;
                var OpOpsenBbnkbMines3 = 0;
                var OpOpsenBbnkbMines4 = 0;

                var result = new List<ViewModel.JumlahObjekPajakSeries>();

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    Jumlah1 = OpRestoMines4,
                    Jumlah2 = OpRestoMines3,
                    Jumlah3 = OpRestoMines2,
                    Jumlah4 = OpRestoMines1,
                    Jumlah5 = OpRestoNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),,
                    Jumlah1 = OpHotelMines4,
                    Jumlah2 = OpHotelMines3,
                    Jumlah3 = OpHotelMines2,
                    Jumlah4 = OpHotelMines1,
                    Jumlah5 = OpHotelNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    Jumlah1 = OpHiburanMines4,
                    Jumlah2 = OpHiburanMines3,
                    Jumlah3 = OpHiburanMines2,
                    Jumlah4 = OpHiburanMines1,
                    Jumlah5 = OpHiburanNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    Jumlah1 = OpParkirMines4,
                    Jumlah2 = OpParkirMines3,
                    Jumlah3 = OpParkirMines2,
                    Jumlah4 = OpParkirMines1,
                    Jumlah5 = OpParkirNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    Jumlah1 = OpListrikMines4,
                    Jumlah2 = OpListrikMines3,
                    Jumlah3 = OpListrikMines2,
                    Jumlah4 = OpListrikMines1,
                    Jumlah5 = OpListrikNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    Jumlah1 = OpPbbMines4,
                    Jumlah2 = OpPbbMines3,
                    Jumlah3 = OpPbbMines2,
                    Jumlah4 = OpPbbMines1,
                    Jumlah5 = OpPbbNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    Jumlah1 = OpBphtbMines4,
                    Jumlah2 = OpBphtbMines3,
                    Jumlah3 = OpBphtbMines2,
                    Jumlah4 = OpBphtbMines1,
                    Jumlah5 = OpBphtbNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    Jumlah1 = OpReklameMines4,
                    Jumlah2 = OpReklameMines3,
                    Jumlah3 = OpReklameMines2,
                    Jumlah4 = OpReklameMines1,
                    Jumlah5 = OpReklameNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    Jumlah1 = OpAbtMines4,
                    Jumlah2 = OpAbtMines3,
                    Jumlah3 = OpAbtMines2,
                    Jumlah4 = OpAbtMines1,
                    Jumlah5 = OpAbtNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    Jumlah1 = OpOpsenPkbMines4,
                    Jumlah2 = OpOpsenPkbMines3,
                    Jumlah3 = OpOpsenPkbMines2,
                    Jumlah4 = OpOpsenPkbMines1,
                    Jumlah5 = OpOpsenPkbNow
                });

                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    Jumlah1 = OpOpsenBbnkbMines4,
                    Jumlah2 = OpOpsenBbnkbMines3,
                    Jumlah3 = OpOpsenBbnkbMines2,
                    Jumlah4 = OpOpsenBbnkbMines1,
                    Jumlah5 = OpOpsenBbnkbNow
                });

                return result;
            }
            public static List<ViewModel.PemasanganAlatRekamSeries> GetPemasanganAlatRekamSeriesData()
            {
                var result = new List<ViewModel.PemasanganAlatRekamSeries>();
                result.Add(new ViewModel.PemasanganAlatRekamSeries()
                {
                    JenisPajak = "Hotel",
                    Jumlah1 = 950,
                    Jumlah2 = 980,
                    Jumlah3 = 1025,
                    Jumlah4 = 1080,
                    Jumlah5 = 1150,
                });
                result.Add(new ViewModel.PemasanganAlatRekamSeries()
                {
                    JenisPajak = "Resto",
                    Jumlah1 = 951,
                    Jumlah2 = 982,
                    Jumlah3 = 1035,
                    Jumlah4 = 1040,
                    Jumlah5 = 1120,
                });

                return result;
            }
            public static List<ViewModel.PemasanganAlatRekamDetail> GetPemasanganAlatRekamDetailData()
            {
                return new List<ViewModel.PemasanganAlatRekamDetail>
                {
                    new ViewModel.PemasanganAlatRekamDetail
                    {
                        JenisPajak = "Hotel",
                        JmlOp = 100,
                        JmlTerpasangTS = 30,    // Terpasang SPT
                        JmlTerpasangTB = 20,    // Terpasang Biller
                        JmlTerpasangSB = 10,    // Terpasang SPT + Biller
                        TotalTerpasang = 60,    // Total dari atas
                        TotalBelumTerpasang = 40
                    },
                    new ViewModel.PemasanganAlatRekamDetail
                    {
                        JenisPajak = "Resto",
                        JmlOp = 150,
                        JmlTerpasangTS = 50,
                        JmlTerpasangTB = 30,
                        JmlTerpasangSB = 20,
                        TotalTerpasang = 100,
                        TotalBelumTerpasang = 50
                    },
                    new ViewModel.PemasanganAlatRekamDetail
                    {
                        JenisPajak = "Parkir",
                        JmlOp = 200,
                        JmlTerpasangTS = 70,
                        JmlTerpasangTB = 50,
                        JmlTerpasangSB = 30,
                        TotalTerpasang = 150,
                        TotalBelumTerpasang = 50
                    }
                };
            }
            public static List<ViewModel.PengedokanPajak> GetPengedokanPajakData()
            {
                return new List<ViewModel.PengedokanPajak>
                {
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Hotel",
                        JmlOp = 120,
                        PotensiHasilPengedokan = 500_000_000,
                        TotalRealisasi = 450_000_000,
                        Selisih = 50_000_000
                    },
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Restoran",
                        JmlOp = 200,
                        PotensiHasilPengedokan = 800_000_000,
                        TotalRealisasi = 750_000_000,
                        Selisih = 50_000_000
                    },
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Hiburan",
                        JmlOp = 75,
                        PotensiHasilPengedokan = 300_000_000,
                        TotalRealisasi = 250_000_000,
                        Selisih = 50_000_000
                    },
                    new ViewModel.PengedokanPajak
                    {
                        JenisPajak = "Parkir",
                        JmlOp = 50,
                        PotensiHasilPengedokan = 150_000_000,
                        TotalRealisasi = 140_000_000,
                        Selisih = 10_000_000
                    }
                };
            }
            public static List<ViewModel.PemeriksaanPajak> GetPemeriksaanPajakData()
            {
                return new List<ViewModel.PemeriksaanPajak>()
                {
                    new ViewModel.PemeriksaanPajak()
                    {
                        JenisPajak = "Restoran",
                        OpPeriksa1 = 10,
                        OpPeriksa2 = 12,
                        OpPeriksa3 = 15,
                        Pokok1 = 10000000,
                        Pokok2 = 12000000,
                        Pokok3 = 15000000,
                        Sanksi1 = 1000000,
                        Sanksi2 = 1200000,
                        Sanksi3 = 1500000,
                        Total1 = 11000000,
                        Total2 = 13200000,
                        Total3 = 16500000
                    },
                    new ViewModel.PemeriksaanPajak()
                    {
                        JenisPajak = "Hotel",
                        OpPeriksa1 = 8,
                        OpPeriksa2 = 9,
                        OpPeriksa3 = 11,
                        Pokok1 = 8000000,
                        Pokok2 = 9000000,
                        Pokok3 = 11000000,
                        Sanksi1 = 800000,
                        Sanksi2 = 900000,
                        Sanksi3 = 1100000,
                        Total1 = 8800000,
                        Total2 = 9900000,
                        Total3 = 12100000
                    },
                    new ViewModel.PemeriksaanPajak()
                    {
                        JenisPajak = "Hiburan",
                        OpPeriksa1 = 5,
                        OpPeriksa2 = 6,
                        OpPeriksa3 = 7,
                        Pokok1 = 6000000,
                        Pokok2 = 7000000,
                        Pokok3 = 7500000,
                        Sanksi1 = 600000,
                        Sanksi2 = 700000,
                        Sanksi3 = 750000,
                        Total1 = 6600000,
                        Total2 = 7700000,
                        Total3 = 8250000
                    }
                };
            }

            public static List<ViewModel.DataKontrolPotensiOp> GetDataKontrolPotensiOp()
            {
                return new List<ViewModel.DataKontrolPotensiOp>
                {
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Restoran",
                        Target1 = 100000000,
                        Realisasi1 = 85000000,
                        Selisih1 = 15000000,
                        Persentase1 = 85,
                        Target2 = 120000000,
                        Realisasi2 = 90000000,
                        Selisih2 = 30000000,
                        Persentase2 = 75
                    },
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Hotel",
                        Target1 = 200000000,
                        Realisasi1 = 180000000,
                        Selisih1 = 20000000,
                        Persentase1 = 90,
                        Target2 = 210000000,
                        Realisasi2 = 195000000,
                        Selisih2 = 15000000,
                        Persentase2 = 92.86M
                    },
                    new ViewModel.DataKontrolPotensiOp
                    {
                        JenisPajak = "Parkir",
                        Target1 = 50000000,
                        Realisasi1 = 40000000,
                        Selisih1 = 10000000,
                        Persentase1 = 80,
                        Target2 = 60000000,
                        Realisasi2 = 50000000,
                        Selisih2 = 10000000,
                        Persentase2 = 83.33M
                    }
                };
            }
            public static List<ViewModel.DataPiutang> GetDataPiutangData()
            {
                return new List<ViewModel.DataPiutang>
                {
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Restoran",
                        NominalPiutang1 = 15000000,
                        NominalPiutang2 = 12000000,
                        NominalPiutang3 = 10000000,
                        NominalPiutang4 = 8000000
                    },
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Hotel",
                        NominalPiutang1 = 25000000,
                        NominalPiutang2 = 23000000,
                        NominalPiutang3 = 18000000,
                        NominalPiutang4 = 16000000
                    },
                    new ViewModel.DataPiutang
                    {
                        JenisPajak = "Hiburan",
                        NominalPiutang1 = 10000000,
                        NominalPiutang2 = 9000000,
                        NominalPiutang3 = 7500000,
                        NominalPiutang4 = 7000000
                    }
                };
            }
            public static List<ViewModel.DataMutasi> GetDataMutasiData()
            {
                return new List<ViewModel.DataMutasi>
                {
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Penyesuaian akhir tahun",
                        NominalMutasi1 = 5000000,
                        NominalMutasi2 = 4500000,
                        NominalMutasi3 = 4000000,
                        NominalMutasi4 = 3500000
                    },
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Koreksi sistem",
                        NominalMutasi1 = 3000000,
                        NominalMutasi2 = 2800000,
                        NominalMutasi3 = 2600000,
                        NominalMutasi4 = 2400000
                    },
                    new ViewModel.DataMutasi
                    {
                        Keterangan = "Pemindahan saldo",
                        NominalMutasi1 = 7000000,
                        NominalMutasi2 = 6800000,
                        NominalMutasi3 = 6500000,
                        NominalMutasi4 = 6000000
                    }
                };
            }

        }
    }
}
