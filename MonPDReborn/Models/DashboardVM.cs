using MonPDLib;

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
            public static List<ViewModel.SeriesPajakDaerah> GetSeriesPajakDaerahData()
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
            public static List<ViewModel.JumlahObjekPajakTahunan> GetJumlahObjekPajakTahunanData()
            {
                return new List<ViewModel.JumlahObjekPajakTahunan>
                {
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Hotel",
                        JmlOpAwal = 120,
                        JmlOpTutupSementara = 5,
                        JmlOpTutupPermanen = 3,
                        JmlOpBaru = 10
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Restoran",
                        JmlOpAwal = 250,
                        JmlOpTutupSementara = 12,
                        JmlOpTutupPermanen = 8,
                        JmlOpBaru = 25
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Parkir",
                        JmlOpAwal = 80,
                        JmlOpTutupSementara = 2,
                        JmlOpTutupPermanen = 1,
                        JmlOpBaru = 5
                    },
                    new ViewModel.JumlahObjekPajakTahunan
                    {
                        JenisPajak = "Hiburan",
                        JmlOpAwal = 60,
                        JmlOpTutupSementara = 3,
                        JmlOpTutupPermanen = 2,
                        JmlOpBaru = 7
                    }
                    // Tambahkan jenis pajak lain jika diperlukan
                };
            }
            public static List<ViewModel.JumlahObjekPajakSeries> GetJumlahObjekPajakSeriesData()
            {
                var result = new List<ViewModel.JumlahObjekPajakSeries>();
                result.Add(new ViewModel.JumlahObjekPajakSeries()
                {
                    JenisPajak = "Hotel",
                    Jumlah1 = 950,
                    Jumlah2 = 980,
                    Jumlah3 = 1025,
                    Jumlah4 = 1080,
                    Jumlah5 = 1150,
                });
                result.Add(new ViewModel.JumlahObjekPajakSeries()
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
