using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static MonPDReborn.Models.DashboardVM;

namespace MonPDReborn.Models.DataOP
{
    public class ProfilePotensiOPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Dashboard Data { get; set; } = new();
            public Index()
            {
                Data = Method.GetDashboardData();

            }
        }

        public class ShowRekap
        {
            public List<RekapPotensi> DataRekapPotensi { get; set; } = new();
            public ShowRekap() { }
            public ShowRekap(string jenisPajak)
            {

                DataRekapPotensi = Method.GetRekapPotensiList();
            }
        }

        //public class ShowDetail
        //{
        //    public List<DetailPotensi> DataDetailPotensi { get; set; } = new();
        //    public string JenisPajak { get; set; } = string.Empty;

        //    public ShowDetail() { }
        //    public ShowDetail(EnumFactory.EPajak jenisPajak)
        //    {
        //        JenisPajak = jenisPajak.GetDescription();
        //        DataDetailPotensi = Method.GetDetailPotensiList(jenisPajak);
        //    }
        //}

        public class ShowData
        {
            public string JenisPajak { get; set; } = string.Empty;
            public string Kategori { get; set; } = string.Empty;
            public int EnumPajak { get; set; }
            public int EnumKategori { get; set; }
            public ShowData() { }
            public ShowData(EnumFactory.EPajak jenisPajak, int kategori)
            {
                var context = DBClass.GetContext();
                JenisPajak = jenisPajak.GetDescription();
                Kategori = context.MKategoriPajaks.Where(x => x.Id == kategori).FirstOrDefault().Nama ?? "-";
                EnumPajak = (int)jenisPajak;
                EnumKategori = kategori;
            }
        }
        public class Detail
        {
            public string NOP { get; set; } = string.Empty;
            public string JenisPajak { get; set; } = string.Empty;
            public string? Kategori { get; set; }



            public Detail() { }

            public Detail(string nop, string jenisPajak, string? kategori = null)
            {
                NOP = nop;
                JenisPajak = jenisPajak;
                Kategori = kategori;

                //DataRealisasiBulananList = Method.GetDetailByNOP(nop, jenisPajak, kategori);
            }
        }


        public class Method
        {
            public static Dashboard GetDashboardData()
            {
                decimal potensi = 125000000;
                decimal realisasi = 110000000;
                int totalOp = 500;
                int realisasiOp = 450;

                var dashboardData = new Dashboard
                {
                    Potensi = potensi,
                    RealisasiTotal = realisasi,
                    Capaian = (potensi > 0) ? Math.Round((realisasi / potensi) * 100, 0) : 0,
                    TotalOP = totalOp,
                    RealisasiOP = realisasiOp,
                    CapaianOP = $"{realisasiOp} dari {totalOp} OP"
                };

                return dashboardData;
            }

            public static List<RekapPotensi> GetRekapPotensiList()
            {
                var ret = new List<RekapPotensi>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                #region Now
                var targetRestoNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoNow = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelNow = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanNow = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirNow = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikNow = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetPbbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                //var realisasiPbbNow = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetBphtbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                //var realisasiBphtbNow = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear).Sum(x => x.Pokok) ?? 0;

                var targetReklameNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameNow = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtNow = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetOpsenPkbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                //var realisasiOpsenPkbNow = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);

                //var targetOpsenBbnkbNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                //var realisasiOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear).Sum(x => x.JmlPokok);
                #endregion

                #region Mines1
                var targetRestoMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines1 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetPbbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                //var realisasiPbbMines1 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetBphtbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                //var realisasiBphtbMines1 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 1).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines1 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetOpsenPkbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                //var realisasiOpsenPkbMines1 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);

                //var targetOpsenBbnkbMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                //var realisasiOpsenBbnkbMines1 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 1).Sum(x => x.JmlPokok);
                #endregion

                #region Mines2
                var targetRestoMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines2 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetPbbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                //var realisasiPbbMines2 = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetBphtbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                //var realisasiBphtbMines2 = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear - 2).Sum(x => x.Pokok) ?? 0;

                var targetReklameMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines2 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetOpsenPkbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                //var realisasiOpsenPkbMines2 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);

                //var targetOpsenBbnkbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                //var realisasiOpsenBbnkbMines2 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);
                #endregion

                #region PotensioList();
                var dataResto3 = context.DbOpRestos
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                    .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                    .ToList();

                var potensiResto = context.DbPotensiRestos
                    .Where(x => dataResto3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year)
                    .ToList()
                    .Select(x =>
                    {
                        var op = dataResto3.FirstOrDefault(o => o.Nop == x.Nop);

                        return new DetailPotensiPajakResto
                        {
                            NOP = x.Nop,
                            Kategori = "-",
                            TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                            JumlahKursi = x.KapKursi ?? 0,
                            KapasitasTenantCatering = x.KapTenantCatering ?? 0,
                            RataRataBillPerOrang = x.AvgBillOrg ?? 0,
                            TurnoverWeekdaysCatering = x.TurnoverWd ?? 0,
                            TurnoverWeekendCatering = x.TurnoverWe ?? 0,
                            TurnoverWeekdaysNonCatering = x.TurnoverWd ?? 0,
                            TurnoverWeekendNonCatering = x.TurnoverWe ?? 0,
                            TarifPajak = 0.1m
                        };
                    })
                    .ToList();
                var totalPotensiResto = potensiResto.Sum(x => x.PotensiPajakPerTahunNonCatering + x.PotensiPajakPerTahunCatering);

                var dataPpj3 = context.DbOpListriks
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                //var totalPotensiPpj = context.PotensiCtrlPpjs.Where(x => listOpPpjAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun) ?? 0;
                var totalPotensiPpj = context.DbPotensiPpjs
                    .Where(x => dataPpj3.Select(v => v.Nop).ToList().Contains(x.Nop))
                    .Sum(q => q.JumlahPajak) ?? 0;

                var dataHotel3 = context.DbOpHotels
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                    .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                    .ToList();

                var potensiHotel = context.DbPotensiHotels
                    .Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year)
                    .ToList()
                    .Select(x =>
                    {
                        var op = dataHotel3.FirstOrDefault(o => o.Nop == x.Nop);

                        return new DetailPotensiPajakHotel
                        {
                            NOP = x.Nop,
                            Kategori = "-",
                            TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                            JumlahTotalRoom = x.TotalRoom ?? 0,
                            HargaRataRataRoom = x.AvgRoomPrice ?? 0,
                            OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                            KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                            HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                            TarifPajak = 0.1m
                        };
                    })
                    .ToList();
                var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun);

                var dataParkir3 = context.DbOpParkirs
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                    .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                    .ToList();

                var potensiParkir = context.DbPotensiParkirs
                    .Where(x => dataParkir3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year)
                    .ToList()
                    .Select(x =>
                    {
                        var op = dataParkir3.FirstOrDefault(o => o.Nop == x.Nop);

                        return new DetailPotensiPajakParkir
                        {
                            NOP = x.Nop,
                            Kategori = "-",
                            Memungut = ((EnumFactory.EPungutTarifParkir)(x.JenisTarif ?? 0)).GetDescription(),
                            SistemParkir = ((EnumFactory.EPalangParkir)(x.SistemParkir ?? 0)).GetDescription(),
                            TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                            TurnoverWeekdays = x.ToWd ?? 0,
                            TurnoverWeekend = x.ToWe ?? 0,
                            KapasitasSepeda = x.KapSepeda ?? 0,
                            TarifSepeda = x.TarifSepeda ?? 0,
                            KapasitasMotor = x.KapMotor ?? 0,
                            TarifMotor = x.TarifMotor ?? 0,
                            KapasitasMobil = x.KapMobil ?? 0,
                            TarifMobil = x.TarifMobil ?? 0,
                            KapasitasTrukMini = x.KapTrukMini ?? 0,
                            TarifTrukMini = x.TarifTrukMini ?? 0,
                            KapasitasTrukBus = x.KapTrukBus ?? 0,
                            TarifTrukBus = x.TarifTrukBus ?? 0,
                            KapasitasTrailer = x.KapTrailer ?? 0,
                            TarifTrailer = x.TarifTrailer ?? 0,
                            TarifPajak = 0.1m
                        };
                    })
                    .ToList();
                var totalPotensiParkir = potensiParkir.Sum(x => x.PotensiPajakPerTahun);

                var dataHiburan3 = context.DbOpHiburans
                    .Where(x => x.TahunBuku == DateTime.Now.Year)
                    .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                    .ToList();

                var potensiHiburan = context.DbPotensiHiburans
                    .Where(x => dataHiburan3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year)
                    .ToList()
                    .Select(x =>
                    {
                        var op = dataHiburan3.FirstOrDefault(o => o.Nop == x.Nop);

                        return new DetailPotensiPajakHiburan
                        {
                            NOP = x.Nop,
                            Kategori = "-",
                            TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                            KapasitasStudio = x.KapKursiStudio ?? 0,
                            JumlahStudio = x.JumlahStudio ?? 0,
                            Kapasitas = x.KapPengunjung ?? 0,
                            HargaMemberFitness = x.HargaMemberBulan ?? 0,
                            HTMWeekdays = x.HtmWd ?? 0,
                            HTMWeekend = x.HtmWe ?? 0,
                            TurnoverWeekdays = x.ToWd ?? 0,
                            TurnoverWeekend = x.ToWe ?? 0,
                            TarifPajak = 0.1m
                        };
                    })
                    .ToList();
                var totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnya + x.PotensiPajakPerTahunBioskop + x.PotensiPajakPerTahunBioskop);

                var dataAbt3 = context.DbOpAbts
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                var totalPotensiAbt = context.DbPotensiAbts
                    .Where(x => dataAbt3.Select(v => v.Nop).ToList().Contains(x.Nop))
                    .Sum(q => q.PajakAirTanah) ?? 0;

                var totalPotensiReklame = context.DbPotensiReklames.Sum(q => q.Rata2Pajak) ?? 0;

                #endregion

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    Target3 = targetRestoNow,
                    Realisasi3 = realisasiRestoNow,
                    Target2 = targetRestoMines1,
                    Realisasi2 = realisasiRestoMines1,
                    Target1 = targetRestoMines2,
                    Realisasi1 = realisasiRestoMines2,
                    TotalPotensi = totalPotensiResto,
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    Target3 = targetListrikNow,
                    Realisasi3 = realisasiListrikNow,
                    Target2 = targetListrikMines1,
                    Realisasi2 = realisasiListrikMines1,
                    Target1 = targetListrikMines2,
                    Realisasi1 = realisasiListrikMines2,
                    TotalPotensi = totalPotensiPpj,
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    Target3 = targetHotelNow,
                    Realisasi3 = realisasiHotelNow,
                    Target2 = targetHotelMines1,
                    Realisasi2 = realisasiHotelMines1,
                    Target1 = targetHotelMines2,
                    Realisasi1 = realisasiHotelMines2,
                    TotalPotensi = totalPotensiHotel
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    Target3 = targetParkirNow,
                    Realisasi3 = realisasiParkirNow,
                    Target2 = targetParkirMines1,
                    Realisasi2 = realisasiParkirMines1,
                    Target1 = targetParkirMines2,
                    Realisasi1 = realisasiParkirMines2,
                    TotalPotensi = totalPotensiParkir
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    Target3 = targetHiburanNow,
                    Realisasi3 = realisasiHiburanNow,
                    Target2 = targetHiburanMines1,
                    Realisasi2 = realisasiHiburanMines1,
                    Target1 = targetHiburanMines2,
                    Realisasi1 = realisasiHiburanMines2,
                    TotalPotensi = totalPotensiHiburan
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    Target3 = targetAbtNow,
                    Realisasi3 = realisasiAbtNow,
                    Target2 = targetAbtMines1,
                    Realisasi2 = realisasiAbtMines1,
                    Target1 = targetAbtMines2,
                    Realisasi1 = realisasiAbtMines2,
                    TotalPotensi = totalPotensiAbt
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.Reklame,
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    Target3 = targetReklameNow,
                    Realisasi3 = realisasiReklameNow,
                    Target2 = targetReklameMines1,
                    Realisasi2 = realisasiReklameMines1,
                    Target1 = targetReklameMines2,
                    Realisasi1 = realisasiReklameMines2,
                    TotalPotensi = totalPotensiReklame
                });

                //ret.Add(new RekapPotensi
                //{
                //    EnumPajak = (int)EnumFactory.EPajak.PBB,
                //    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                //    Target3 = targetPbbNow,
                //    Realisasi3 = realisasiPbbNow,
                //    Target2 = targetPbbMines1,
                //    Realisasi2 = realisasiPbbMines1,
                //    Target1 = targetPbbMines2,
                //    Realisasi1 = realisasiPbbMines2,
                //    TotalPotensi = 0
                //});

                //ret.Add(new RekapPotensi
                //{
                //    EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                //    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                //    Target3 = targetBphtbNow,
                //    Realisasi3 = realisasiBphtbNow,
                //    Target2 = targetBphtbMines1,
                //    Realisasi2 = realisasiBphtbMines1,
                //    Target1 = targetBphtbMines2,
                //    Realisasi1 = realisasiBphtbMines2,
                //    TotalPotensi = 0
                //});

                //ret.Add(new RekapPotensi
                //{
                //    EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                //    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                //    Target3 = targetOpsenPkbNow,
                //    Realisasi3 = realisasiOpsenPkbNow,
                //    Target2 = targetOpsenPkbMines1,
                //    Realisasi2 = realisasiOpsenPkbMines1,
                //    Target1 = targetOpsenPkbMines2,
                //    Realisasi1 = realisasiOpsenPkbMines2,
                //    TotalPotensi = 0
                //});

                //ret.Add(new RekapPotensi
                //{
                //    EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                //    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                //    Target3 = targetOpsenBbnkbNow,
                //    Realisasi3 = realisasiOpsenBbnkbNow,
                //    Target2 = targetOpsenBbnkbMines1,
                //    Realisasi2 = realisasiOpsenBbnkbMines1,
                //    Target1 = targetOpsenBbnkbMines2,
                //    Realisasi1 = realisasiOpsenBbnkbMines2,
                //    TotalPotensi = 0
                //});

                return ret;
            }

            public static List<DetailPotensi> GetDetailPotensiList(EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<DetailPotensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();


                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataResto1 = context.DbOpRestos
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataResto2 = context.DbOpRestos
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataResto3 = context.DbOpRestos
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                            .GroupBy(x => new { x.Nop, x.KategoriId, x.TglMulaiBukaOp })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId, x.Key.TglMulaiBukaOp })
                            .ToList();

                        foreach (var item in kategoriList)
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpResto1 = dataResto1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpResto2 = dataResto2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpResto3 = dataResto3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetResto1 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && listOpResto1.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetResto2 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && listOpResto2.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetResto3 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && listOpResto3.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var realisasiResto1 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpResto1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiResto2 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpResto2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiResto3 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpResto3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var potensiResto = context.DbPotensiRestos
                            .Where(x => listOpResto3.Contains(x.Nop))
                            .ToList()
                            .Select(x =>
                            {
                                var op = dataResto3.FirstOrDefault(o => o.Nop == x.Nop);

                                return new DetailPotensiPajakResto
                                {
                                    NOP = x.Nop,
                                    Kategori = "-",
                                    TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                    JumlahKursi = x.KapKursi ?? 0,
                                    KapasitasTenantCatering = x.KapTenantCatering ?? 0,
                                    RataRataBillPerOrang = x.AvgBillOrg ?? 0,
                                    TurnoverWeekdaysCatering = x.TurnoverWd ?? 0,
                                    TurnoverWeekendCatering = x.TurnoverWe ?? 0,
                                    TurnoverWeekdaysNonCatering = x.TurnoverWd ?? 0,
                                    TurnoverWeekendNonCatering = x.TurnoverWe ?? 0,
                                    TarifPajak = 0.1m
                                };
                            })
                            .ToList();
                            var totalPotensiResto = potensiResto.Sum(x => x.PotensiPajakPerTahunCatering + x.PotensiPajakPerTahunNonCatering);

                            re.Target1 = targetResto1;
                            re.Realisasi1 = realisasiResto1;
                            re.Target2 = targetResto2;
                            re.Realisasi2 = realisasiResto2;
                            re.Target3 = targetResto3;
                            re.Realisasi3 = realisasiResto3;
                            re.TotalPotensi = totalPotensiResto;


                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrik1 = context.DbOpListriks
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataListrik2 = context.DbOpListriks
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataListrik3 = context.DbOpListriks
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();

                        var dataListrikAll = dataListrik1
                            .Concat(dataListrik2)
                            .Concat(dataListrik3)
                            .Select(x => (x.Nop, x.KategoriId))
                            .Distinct()
                            .ToList();

                        foreach (var item in kategoriList)
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpListrik1 = dataListrik1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpListrik2 = dataListrik2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpListrik3 = dataListrik3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpListrikAll = dataListrikAll.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetListrik1 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && listOpListrik1.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetListrik2 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && listOpListrik2.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetListrik3 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year && listOpListrik3.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var realisasiListrik1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpListrik1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiListrik2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpListrik2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiListrik3 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpListrik3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var totalPotensiListrik = context.DbPotensiPpjs.Where(x => listOpListrikAll.Contains(x.Nop)).Sum(q => q.JumlahPajak) ?? 0;

                            re.Target1 = targetListrik1;
                            re.Realisasi1 = realisasiListrik1;
                            re.Target2 = targetListrik2;
                            re.Realisasi2 = realisasiListrik2;
                            re.Target3 = targetListrik3;
                            re.Realisasi3 = realisasiListrik3;
                            re.TotalPotensi = totalPotensiListrik;


                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotel1 = context.DbOpHotels
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataHotel2 = context.DbOpHotels
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataHotel3 = context.DbOpHotels
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();

                        var dataHotelAll = dataHotel1
                            .Concat(dataHotel2)
                            .Concat(dataHotel3)
                            .Select(x => (x.Nop, x.KategoriId))
                            .Distinct()
                            .ToList();

                        foreach (var item in kategoriList.OrderBy(x => x.Id).ToList())
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpHotel1 = dataHotel1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHotel2 = dataHotel2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHotel3 = dataHotel3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHotelAll = dataHotelAll.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetHotel1 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && listOpHotel1.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetHotel2 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && listOpHotel2.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetHotel3 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year && listOpHotel3.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var realisasiHotel1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHotel1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHotel2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHotel2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHotel3 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHotel3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var potensiHotel = context.DbPotensiHotels
                                .Where(x => listOpHotel3.Contains(x.Nop))
                                .ToList()
                                .Select(x =>
                                {
                                    var op = context.DbOpHotels.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHotel
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        JumlahTotalRoom = x.TotalRoom ?? 0,
                                        HargaRataRataRoom = x.AvgRoomPrice ?? 0,
                                        OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                                        KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                                        HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun);

                            re.Target1 = targetHotel1;
                            re.Realisasi1 = realisasiHotel1;
                            re.Target2 = targetHotel2;
                            re.Realisasi2 = realisasiHotel2;
                            re.Target3 = targetHotel3;
                            re.Realisasi3 = realisasiHotel3;
                            re.TotalPotensi = totalPotensiHotel;


                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkir1 = context.DbOpParkirs
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataParkir2 = context.DbOpParkirs
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataParkir3 = context.DbOpParkirs
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();

                        var dataParkirAll = dataParkir1
                            .Concat(dataParkir2)
                            .Concat(dataParkir3)
                            .Select(x => (x.Nop, x.KategoriId))
                            .Distinct()
                            .ToList();

                        foreach (var item in kategoriList)
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpParkir1 = dataParkir1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpParkir2 = dataParkir2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpParkir3 = dataParkir3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpParkirAll = dataParkirAll.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetParkir1 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && listOpParkir1.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetParkir2 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && listOpParkir2.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetParkir3 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year && listOpParkir3.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var realisasiParkir1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpParkir1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiParkir2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpParkir2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiParkir3 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpParkir3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var potensiParkir = context.DbPotensiParkirs
                                .Where(x => listOpParkir3.Contains(x.Nop))
                                .ToList()
                                .Select(x =>
                                {
                                    var op = context.DbOpParkirs.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakParkir
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        Memungut = ((EnumFactory.EPungutTarifParkir)(x.JenisTarif ?? 0)).GetDescription(),
                                        SistemParkir = ((EnumFactory.EPalangParkir)(x.SistemParkir ?? 0)).GetDescription(),
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        TurnoverWeekdays = x.ToWd ?? 0,
                                        TurnoverWeekend = x.ToWe ?? 0,
                                        KapasitasSepeda = x.KapSepeda ?? 0,
                                        TarifSepeda = x.TarifSepeda ?? 0,
                                        KapasitasMotor = x.KapMotor ?? 0,
                                        TarifMotor = x.TarifMotor ?? 0,
                                        KapasitasMobil = x.KapMobil ?? 0,
                                        TarifMobil = x.TarifMobil ?? 0,
                                        KapasitasTrukMini = x.KapTrukMini ?? 0,
                                        TarifTrukMini = x.TarifTrukMini ?? 0,
                                        KapasitasTrukBus = x.KapTrukBus ?? 0,
                                        TarifTrukBus = x.TarifTrukBus ?? 0,
                                        KapasitasTrailer = x.KapTrailer ?? 0,
                                        TarifTrailer = x.TarifTrailer ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiParkir = potensiParkir.Sum(x => x.PotensiPajakPerTahun);

                            re.Target1 = targetParkir1;
                            re.Realisasi1 = realisasiParkir1;
                            re.Target2 = targetParkir2;
                            re.Realisasi2 = realisasiParkir2;
                            re.Target3 = targetParkir3;
                            re.Realisasi3 = realisasiParkir3;
                            re.TotalPotensi = totalPotensiParkir;


                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburan1 = context.DbOpHiburans
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataHiburan2 = context.DbOpHiburans
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataHiburan3 = context.DbOpHiburans
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();

                        var dataHiburanAll = dataHiburan1
                            .Concat(dataHiburan2)
                            .Concat(dataHiburan3)
                            .Select(x => (x.Nop, x.KategoriId))
                            .Distinct()
                            .ToList();

                        foreach (var item in kategoriList)
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpHiburan1 = dataHiburan1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHiburan2 = dataHiburan2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHiburan3 = dataHiburan3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHiburanAll = dataHiburanAll.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetHiburan1 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && listOpHiburan1.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetHiburan2 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && listOpHiburan2.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetHiburan3 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && listOpHiburan3.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var realisasiHiburan1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHiburan1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHiburan2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHiburan2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHiburan3 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHiburan3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var potensiHiburan = context.DbPotensiHiburans
                                .Where(x => listOpHiburan3.Contains(x.Nop))
                                .ToList()
                                .Select(x =>
                                {
                                    var op = context.DbOpHiburans.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHiburan
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        KapasitasStudio = x.KapKursiStudio ?? 0,
                                        JumlahStudio = x.JumlahStudio ?? 0,
                                        Kapasitas = x.KapPengunjung ?? 0,
                                        HargaMemberFitness = x.HargaMemberBulan ?? 0,
                                        HTMWeekdays = x.HtmWd ?? 0,
                                        HTMWeekend = x.HtmWe ?? 0,
                                        TurnoverWeekdays = x.ToWd ?? 0,
                                        TurnoverWeekend = x.ToWe ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnya + x.PotensiPajakPerTahunBioskop + x.PotensiPajakPerTahunBioskop);

                            re.Target1 = targetHiburan1;
                            re.Realisasi1 = realisasiHiburan1;
                            re.Target2 = targetHiburan2;
                            re.Realisasi2 = realisasiHiburan2;
                            re.Target3 = targetHiburan3;
                            re.Realisasi3 = realisasiHiburan3;
                            re.TotalPotensi = totalPotensiHiburan;


                            ret.Add(re);
                        }

                        var hiburanIns1 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan && x.Insidentil == 1).AsQueryable();
                        var hiburanIns2 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan && x.Insidentil == 1).AsQueryable();
                        var hiburanIns3 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan && x.Insidentil == 1).AsQueryable();

                        var hiburanInsTarget1 = hiburanIns1.Sum(x => x.Target) ?? 0;
                        var hiburanInsTarget2 = hiburanIns1.Sum(x => x.Target) ?? 0;
                        var hiburanInsTarget3 = hiburanIns1.Sum(x => x.Target) ?? 0;
                        var hiburanInsRealisasi1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && hiburanIns1.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var hiburanInsRealisasi2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && hiburanIns2.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                        var hiburanInsRealisasi3 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && hiburanIns3.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                        var potensiHiburanIns = context.DbPotensiHiburans
                                .Where(x => hiburanIns3.Select(x => x.Nop).ToList().Contains(x.Nop))
                                .ToList()
                                .Select(x =>
                                {
                                    var op = context.DbOpHiburans.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHiburan
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        KapasitasStudio = x.KapKursiStudio ?? 0,
                                        JumlahStudio = x.JumlahStudio ?? 0,
                                        Kapasitas = x.KapPengunjung ?? 0,
                                        HargaMemberFitness = x.HargaMemberBulan ?? 0,
                                        HTMWeekdays = x.HtmWd ?? 0,
                                        HTMWeekend = x.HtmWe ?? 0,
                                        TurnoverWeekdays = x.ToWd ?? 0,
                                        TurnoverWeekend = x.ToWe ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                        var totalPotensiHiburanIns = potensiHiburanIns.Sum(x => x.PotensiPajakPerTahunLainnya + x.PotensiPajakPerTahunBioskop + x.PotensiPajakPerTahunBioskop);

                        ret.Add(new DetailPotensi()
                        {
                            Target1 = hiburanInsTarget1,
                            Realisasi1 = hiburanInsRealisasi1,
                            Target2 = hiburanInsTarget2,
                            Realisasi2 = hiburanInsRealisasi2,
                            Target3 = hiburanInsTarget3,
                            Realisasi3 = hiburanInsRealisasi3,
                            TotalPotensi = totalPotensiHiburanIns,
                        });
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbt1 = context.DbOpAbts
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataAbt2 = context.DbOpAbts
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataAbt3 = context.DbOpAbts
                            .Where(x => ((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now)))
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();

                        var dataAbtAll = dataAbt1
                            .Concat(dataAbt2)
                            .Concat(dataAbt3)
                            .Select(x => (x.Nop, x.KategoriId))
                            .Distinct()
                            .ToList();

                        foreach (var item in kategoriList)
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpAbt1 = dataAbt1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpAbt2 = dataAbt2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpAbt3 = dataAbt3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpAbtAll = dataAbtAll.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetAbt1 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && listOpAbt1.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetAbt2 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && listOpAbt2.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var targetAbt3 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year && listOpAbt3.Contains(x.Nop)).Sum(q => q.Target) ?? 0;
                            var realisasiAbt1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpAbt1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiAbt2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpAbt2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiAbt3 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpAbt3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var totalPotensiAbt = context.DbPotensiAbts.Where(x => listOpAbt3.Contains(x.Nop)).Sum(q => q.PajakAirTanah) ?? 0;

                            re.Target1 = targetAbt1;
                            re.Realisasi1 = realisasiAbt1;
                            re.Target2 = targetAbt2;
                            re.Realisasi2 = realisasiAbt2;
                            re.Target3 = targetAbt3;
                            re.Realisasi3 = realisasiAbt3;
                            re.TotalPotensi = totalPotensiAbt;


                            ret.Add(re);
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:

                        foreach (var item in kategoriList)
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var targetReklame1 = context.DbAkunTargets.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                            var realisasiReklame1 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0;
                            var targetReklame2 = context.DbAkunTargets.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                            var realisasiReklame2 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0;
                            var targetReklame3 = context.DbAkunTargets.Where(x => x.TahunBuku == DateTime.Now.Year && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                            var realisasiReklame3 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0;

                            var totalPotensiReklame = context.DbPotensiReklames.Sum(q => q.Rata2Pajak) ?? 0;

                            re.Target1 = targetReklame1;
                            re.Realisasi1 = realisasiReklame1;
                            re.Target2 = targetReklame2;
                            re.Realisasi2 = realisasiReklame2;
                            re.Target3 = targetReklame3;
                            re.Realisasi3 = realisasiReklame3;
                            re.TotalPotensi = totalPotensiReklame;


                            ret.Add(re);
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        break;
                    case EnumFactory.EPajak.BPHTB:
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

            public static List<DataPotensi> GetDataPotensiList(EnumFactory.EPajak jenisPajak, int kategori)
            {
                var ret = new List<DataPotensi>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataResto1 = context.DbOpRestos
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori)
                            .ToList();
                        var dataResto2 = context.DbOpRestos
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori)
                            .ToList();
                        var dataResto3 = context.DbOpRestos
                            .Where(x => (((x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now))) && x.KategoriId == kategori)
                            .ToList();

                        foreach (var item in dataResto3.Distinct())
                        {
                            var potensiResto = context.DbPotensiRestos
                            .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year)
                            .ToList()
                            .Select(x =>
                            {
                                var op = dataResto3.FirstOrDefault(o => o.Nop == x.Nop);

                                return new DetailPotensiPajakResto
                                {
                                    NOP = x.Nop,
                                    Nama = op?.NamaOp ?? "-",
                                    Alamat = op?.AlamatOp ?? "-",
                                    Wilayah = op?.WilayahPajak ?? "-",
                                    Kategori = "-",
                                    TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                    JumlahKursi = x.KapKursi ?? 0,
                                    KapasitasTenantCatering = x.KapTenantCatering ?? 0,
                                    RataRataBillPerOrang = x.AvgBillOrg ?? 0,
                                    TurnoverWeekdaysCatering = x.TurnoverWd ?? 0,
                                    TurnoverWeekendCatering = x.TurnoverWe ?? 0,
                                    TurnoverWeekdaysNonCatering = x.TurnoverWd ?? 0,
                                    TurnoverWeekendNonCatering = x.TurnoverWe ?? 0,
                                    TarifPajak = 0.1m
                                };
                            })
                            .ToList();
                            var totalPotensiResto = potensiResto.Sum(x => x.PotensiPajakPerTahunCatering + x.PotensiPajakPerTahunNonCatering);

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi1 = context.DbMonRestos.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi2 = context.DbMonRestos.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi3 = context.DbMonRestos.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = totalPotensiResto
                            };
                            ret.Add(potensi);
                        }

                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrik1 = context.DbOpListriks
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori)
                            .ToList();
                        var dataListrik2 = context.DbOpListriks
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori)
                            .ToList();
                        var dataListrik3 = context.DbOpListriks
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now) && x.KategoriId == kategori)
                            .ToList();

                        foreach (var item in dataListrik3.Distinct())
                        {
                            var totalPotensiListrik = context.DbPotensiPpjs.Where(x => x.Nop == item.Nop).Sum(q => q.JumlahPajak) ?? 0;

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi1 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi2 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi3 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = totalPotensiListrik
                            };
                            ret.Add(potensi);
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotel1 = context.DbOpHotels
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori)
                            .ToList();
                        var dataHotel2 = context.DbOpHotels
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori)
                            .ToList();
                        var dataHotel3 = context.DbOpHotels
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now) && x.KategoriId == kategori)
                            .ToList();

                        foreach (var item in dataHotel3.Distinct())
                        {
                            var potensiHotel = context.DbPotensiHotels
                                .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = dataHotel3.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHotel
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        JumlahTotalRoom = x.TotalRoom ?? 0,
                                        HargaRataRataRoom = x.AvgRoomPrice ?? 0,
                                        OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                                        KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                                        HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun);

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi1 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi2 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi3 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = totalPotensiHotel
                            };
                            ret.Add(potensi);
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkir1 = context.DbOpParkirs
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori)
                            .ToList();
                        var dataParkir2 = context.DbOpParkirs
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori)
                            .ToList();
                        var dataParkir3 = context.DbOpParkirs
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now) && x.KategoriId == kategori)
                            .ToList();

                        foreach (var item in dataParkir3.Distinct())
                        {
                            var potensiParkir = context.DbPotensiParkirs
                                .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = dataParkir3.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakParkir
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        Memungut = ((EnumFactory.EPungutTarifParkir)(x.JenisTarif ?? 0)).GetDescription(),
                                        SistemParkir = ((EnumFactory.EPalangParkir)(x.SistemParkir ?? 0)).GetDescription(),
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        TurnoverWeekdays = x.ToWd ?? 0,
                                        TurnoverWeekend = x.ToWe ?? 0,
                                        KapasitasSepeda = x.KapSepeda ?? 0,
                                        TarifSepeda = x.TarifSepeda ?? 0,
                                        KapasitasMotor = x.KapMotor ?? 0,
                                        TarifMotor = x.TarifMotor ?? 0,
                                        KapasitasMobil = x.KapMobil ?? 0,
                                        TarifMobil = x.TarifMobil ?? 0,
                                        KapasitasTrukMini = x.KapTrukMini ?? 0,
                                        TarifTrukMini = x.TarifTrukMini ?? 0,
                                        KapasitasTrukBus = x.KapTrukBus ?? 0,
                                        TarifTrukBus = x.TarifTrukBus ?? 0,
                                        KapasitasTrailer = x.KapTrailer ?? 0,
                                        TarifTrailer = x.TarifTrailer ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiParkir = potensiParkir.Sum(x => x.PotensiPajakPerTahun);

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi1 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi2 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi3 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = totalPotensiParkir
                            };
                            ret.Add(potensi);
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburan1 = context.DbOpHiburans
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori)
                            .ToList();
                        var dataHiburan2 = context.DbOpHiburans
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori)
                            .ToList();
                        var dataHiburan3 = context.DbOpHiburans
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now) && x.KategoriId == kategori)
                            .ToList();

                        foreach (var item in dataHiburan3.Distinct())
                        {
                            var potensiHiburan = context.DbPotensiHiburans
                                .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = dataHiburan3.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHiburan
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        KapasitasStudio = x.KapKursiStudio ?? 0,
                                        JumlahStudio = x.JumlahStudio ?? 0,
                                        Kapasitas = x.KapPengunjung ?? 0,
                                        HargaMemberFitness = x.HargaMemberBulan ?? 0,
                                        HTMWeekdays = x.HtmWd ?? 0,
                                        HTMWeekend = x.HtmWe ?? 0,
                                        TurnoverWeekdays = x.ToWd ?? 0,
                                        TurnoverWeekend = x.ToWe ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnya + x.PotensiPajakPerTahunBioskop + x.PotensiPajakPerTahunBioskop);

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi1 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi2 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi3 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = totalPotensiHiburan
                            };
                            ret.Add(potensi);
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbt1 = context.DbOpAbts
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori)
                            .ToList();
                        var dataAbt2 = context.DbOpAbts
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori)
                            .ToList();
                        var dataAbt3 = context.DbOpAbts
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now) && x.KategoriId == kategori)
                            .ToList();

                        var listOpAbt1 = dataAbt1.Select(x => x.Nop).ToList();
                        var listOpAbt2 = dataAbt2.Select(x => x.Nop).ToList();
                        var listOpAbt3 = dataAbt3.Select(x => x.Nop).ToList();

                        foreach (var item in dataAbt3.Distinct())
                        {
                            var totalPotensiAbt = context.DbPotensiAbts.Where(x => x.Nop == item.Nop).Sum(q => q.PajakAirTanah) ?? 0;

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi1 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi2 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.Target) ?? 0,
                                Realisasi3 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = totalPotensiAbt
                            };
                            ret.Add(potensi);
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:
                        var dataReklame1 = context.DbOpReklames
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori)
                            .ToList();
                        var dataReklame2 = context.DbOpReklames
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori)
                            .ToList();
                        var dataReklame3 = context.DbOpReklames
                            .Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori)
                            .ToList();

                        var dataReklameAll = dataReklame1
                            .Concat(dataReklame2)
                            .Concat(dataReklame3)
                            .Select(x => new { Nop = x.Nop, NamaOp = x.Nama, AlamatOp = x.Alamat })
                            .Distinct()
                            .ToList();

                        foreach (var item in dataReklameAll)
                        {
                            //var totalPotensiReklame = context.DbPotensiReklames.Sum(q => q.Rata2Pajak) ?? 0;

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Alamat = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 2).Sum(q => q.Target),
                                Realisasi1 = context.DbMonReklames.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 1).Sum(q => q.Target),
                                Realisasi2 = context.DbMonReklames.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year).Sum(q => q.Target),
                                Realisasi3 = context.DbMonReklames.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = context.PotensiCtrlReklames.Where(x => x.Nop == item.Nop).Sum(q => q.PotensiPajakTahun) ?? 0
                            };
                            ret.Add(potensi);
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        break;
                    case EnumFactory.EPajak.BPHTB:
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


            public static InfoDasar GetInfoDasar(string nop)
            {
                // Simulasi mengambil data dari DB berdasarkan NOP
                return new InfoDasar
                {
                    NOP = nop,
                    NamaWP = "GALAXY MALL PARKIR",
                    Alamat = "JL. DHARMAHUSADA INDAH TIMUR",
                    KapasitasMotor = 1500,
                    KapasitasMobil = 1000
                };
            }

            // Method untuk data dummy Kapasitas & Tarif
            public static KapasitasTarif GetKapasitasTarif(string nop)
            {
                return new KapasitasTarif
                {
                    KapasitasMotor = 1500,
                    TerpakaiMotorHariKerja = 900,
                    TerpakaiMotorAkhirPekan = 1350,
                    TarifMotor = 3000,
                    KapasitasMobil = 1000,
                    TerpakaiMobilHariKerja = 800,
                    TerpakaiMobilAkhirPekan = 950,
                    TarifMobil = 10000
                };
            }

            public static InfoDasar GetInfoDasarHotel(string nop)
            {
                // Simulasi ambil data dari DB
                return new InfoDasar
                {
                    NOP = nop,
                    NamaWP = "HOTEL JW MARRIOTT",
                    Alamat = "Jl. Embong Malang 85-89, Surabaya",
                };
            }

            public static InfoKamar GetInfoKamar(string nop)
            {
                return new InfoKamar
                {
                    JumlahKamar = 400,
                    TarifRataRata = 1500000,
                    TingkatHunian = 85 // artinya 85%
                };
            }

            public static InfoBanquet GetInfoBanquet(string nop)
            {
                return new InfoBanquet
                {
                    KapasitasMaksimum = 1500,
                    TingkatOkupansi = 60, // artinya 60%
                    TarifRataRata = 250000,
                    HariEventPerBulan = 10
                };
            }

            public static InfoDasar GetInfoDasarBioskop(string nop)
            {
                return new InfoDasar
                {
                    NOP = nop,
                    NamaWP = "XXI CITO",
                    Alamat = "Jl. A. Yani No. 288, Surabaya",
                    KapasitasKursiStudio = 850 // Properti baru untuk InfoDasar
                };
            }

            public static InfoBioskop GetInfoBioskop(string nop)
            {
                return new InfoBioskop
                {
                    KapasitasKursi = 850,
                    KursiTerjualPerHari = 600,
                    TurnoverHariKerja = 2.5m,
                    TurnoverAkhirPekan = 4.0m,
                    HargaTiketHariKerja = 40000,
                    HargaTiketAkhirPekan = 55000
                };
            }
        }

        public class Dashboard
        {
            public decimal Potensi { get; set; }
            public decimal RealisasiTotal { get; set; }
            public decimal Capaian { get; set; }
            public int TotalOP { get; set; }
            public int RealisasiOP { get; set; }
            public string CapaianOP { get; set; } = "";
        }

        public class DataPotensi
        {
            public string NOP { get; set; } = null!;
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public int EnumPajak { get; set; }
            public int EnumKategori { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public decimal Target1 { get; set; } = 0;
            public decimal Realisasi1 { get; set; } = 0;
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Selisih1 => Realisasi1 - Target1;
            public decimal Target2 { get; set; } = 0;
            public decimal Realisasi2 { get; set; } = 0;
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Selisih2 => Realisasi2 - Target2;
            public decimal Target3 { get; set; } = 0;
            public decimal Realisasi3 { get; set; } = 0;
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal Selisih3 => Realisasi3 - Target3;
            public decimal TotalPotensi { get; set; } = 0;
        }
        public class RekapPotensi
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }

        public class DetailPotensi
        {
            public int EnumPajak { get; set; }
            public int EnumKategori { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Capaian1 => Target1 == 0 ? 0 : Math.Round((Realisasi1 / Target1) * 100, 2);
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Capaian2 => Target2 == 0 ? 0 : Math.Round((Realisasi2 / Target2) * 100, 2);
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Capaian3 => Target3 == 0 ? 0 : Math.Round((Realisasi3 / Target3) * 100, 2);
            public decimal TotalPotensi { get; set; }
        }
        #region CLASS DetailPotensiFix
        public class DetailPotensiPajakHotel
        {
            // Informasi dasar
            public string NOP { get; set; }
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; }

            // Room
            public int JumlahTotalRoom { get; set; }
            public decimal HargaRataRataRoom { get; set; }
            public decimal OkupansiRateRoom { get; set; } // dalam 0.0 - 1.0

            // Banquet
            public int KapasitasMaksimalPaxBanquetPerHari { get; set; }
            public decimal HargaRataRataBanquetPerPax { get; set; }

            // Tarif Pajak
            public decimal TarifPajak { get; set; } // dalam 0.0 - 1.0
            public DateTime TglOpBuka { get; set; }
            public int BulanSisa
            {
                get
                {
                    var now = DateTime.Now;
                    var akhirTahun = new DateTime(now.Year, 12, 31);

                    if (TglOpBuka.Year < now.Year)
                        return 12;

                    if (TglOpBuka.Year > now.Year)
                        return 0;

                    int totalBulan = (12 - TglOpBuka.Month) + 1;

                    return totalBulan;
                }
            }

            // Perhitungan otomatis

            public decimal RataRataRoomTerjualPerHari => JumlahTotalRoom * OkupansiRateRoom;

            public decimal PotensiOmzetRoomPerBulan => HargaRataRataRoom * RataRataRoomTerjualPerHari * 30;

            public decimal OkupansiRateBanquet => 0.3m * OkupansiRateRoom;

            public decimal RataRataPaxBanquetTerjualPerHari => KapasitasMaksimalPaxBanquetPerHari * OkupansiRateBanquet;

            public decimal PotensiOmzetBanquetPerBulan => HargaRataRataBanquetPerPax * RataRataPaxBanquetTerjualPerHari * 8;

            public decimal PotensiPajakPerBulan => (PotensiOmzetRoomPerBulan + PotensiOmzetBanquetPerBulan) * TarifPajak;
            public decimal PotensiPajakPerTahun => PotensiPajakPerBulan * BulanSisa;
        }
        public class DetailPotensiPajakResto
        {
            // Identitas
            public string NOP { get; set; }
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; }
            public DateTime TglOpBuka { get; set; }
            public int BulanSisa
            {
                get
                {
                    var now = DateTime.Now;
                    var akhirTahun = new DateTime(now.Year, 12, 31);

                    if (TglOpBuka.Year < now.Year)
                        return 12;

                    if (TglOpBuka.Year > now.Year)
                        return 0;

                    int totalBulan = (12 - TglOpBuka.Month) + 1;

                    return totalBulan;
                }
            }

            // Data utama
            //NonCatering
            public int JumlahKursi { get; set; }

            //Catering
            public int KapasitasTenantCatering { get; set; }
            public decimal RataRataBillPerOrang { get; set; }
            public decimal TurnoverWeekdaysCatering { get; set; } // antara 0.0 - 1.0
            public decimal TurnoverWeekendCatering { get; set; }  // antara 0.0 - 1.0
            public decimal TurnoverWeekdaysNonCatering { get; set; } // antara 0.0 - 1.0
            public decimal TurnoverWeekendNonCatering { get; set; }  // antara 0.0 - 1.0
            public decimal TarifPajak { get; set; }       // misal 0.10 untuk 10%


            // PerhitunganCatering
            public decimal RataRataTerjualWeekdaysCatering => KapasitasTenantCatering * TurnoverWeekdaysCatering;
            public decimal RataRataTerjualWeekendCatering => KapasitasTenantCatering * TurnoverWeekendCatering;

            public decimal OmzetPerBulanCatering =>
                (RataRataBillPerOrang * RataRataTerjualWeekdaysCatering * 22) +
                (RataRataBillPerOrang * RataRataTerjualWeekendCatering * 8);

            public decimal PotensiPajakPerBulanCatering => OmzetPerBulanCatering * TarifPajak;
            public decimal PotensiPajakPerTahunCatering => PotensiPajakPerBulanCatering * BulanSisa;

            // PerhitunganNonCatering
            public decimal RataRataPengunjungWeekdaysNonCatering => JumlahKursi * TurnoverWeekdaysNonCatering;
            public decimal RataRataPengunjungWeekendNonCatering => JumlahKursi * TurnoverWeekendNonCatering;

            public decimal OmzetPerBulanNonCatering =>
                (RataRataBillPerOrang * RataRataPengunjungWeekdaysNonCatering * 22) +
                (RataRataBillPerOrang * RataRataPengunjungWeekendNonCatering * 8);

            public decimal PotensiPajakPerBulanNonCatering => OmzetPerBulanNonCatering * TarifPajak;
            public decimal PotensiPajakPerTahunNonCatering => PotensiPajakPerBulanNonCatering * BulanSisa;
        }
        public class DetailPotensiPajakParkir
        {
            // Identitas
            public string NOP { get; set; }
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; }
            public string Memungut { get; set; } // "Memungut" atau "Tidak Memungut"
            public string SistemParkir { get; set; }
            public DateTime TglOpBuka { get; set; }
            public int BulanSisa
            {
                get
                {
                    var now = DateTime.Now;
                    var akhirTahun = new DateTime(now.Year, 12, 31);

                    if (TglOpBuka.Year < now.Year)
                        return 12;

                    if (TglOpBuka.Year > now.Year)
                        return 0;

                    int totalBulan = (12 - TglOpBuka.Month) + 1;

                    return totalBulan;
                }
            }

            // Parameter umum
            public decimal TurnoverWeekdays { get; set; } // 0.0 - 1.0
            public decimal TurnoverWeekend { get; set; }  // 0.0 - 1.0
            public decimal TarifPajak { get; set; }       // 0.0 - 1.0

            // Data kendaraan & tarif
            public int KapasitasSepeda { get; set; }
            public decimal TarifSepeda { get; set; }

            public int KapasitasMotor { get; set; }
            public decimal TarifMotor { get; set; }

            public int KapasitasMobil { get; set; }
            public decimal TarifMobil { get; set; }

            public int KapasitasTrukMini { get; set; }
            public decimal TarifTrukMini { get; set; }

            public int KapasitasTrukBus { get; set; }
            public decimal TarifTrukBus { get; set; }

            public int KapasitasTrailer { get; set; }
            public decimal TarifTrailer { get; set; }

            // Jumlah terparkir weekdays & weekend
            public decimal JumlahTerparkirSepedaWeekdays => TurnoverWeekdays * KapasitasSepeda;
            public decimal JumlahTerparkirSepedaWeekend => TurnoverWeekend * KapasitasSepeda;
            public decimal OmzetSepeda =>
                (JumlahTerparkirSepedaWeekdays * TarifSepeda * 22) +
                (JumlahTerparkirSepedaWeekend * TarifSepeda * 8);

            public decimal JumlahTerparkirMotorWeekdays => TurnoverWeekdays * KapasitasMotor;
            public decimal JumlahTerparkirMotorWeekend => TurnoverWeekend * KapasitasMotor;
            public decimal OmzetMotor =>
                (JumlahTerparkirMotorWeekdays * TarifMotor * 22) +
                (JumlahTerparkirMotorWeekend * TarifMotor * 8);

            public decimal JumlahTerparkirMobilWeekdays => TurnoverWeekdays * KapasitasMobil;
            public decimal JumlahTerparkirMobilWeekend => TurnoverWeekend * KapasitasMobil;
            public decimal OmzetMobil =>
                (JumlahTerparkirMobilWeekdays * TarifMobil * 22) +
                (JumlahTerparkirMobilWeekend * TarifMobil * 8);

            public decimal JumlahTerparkirTrukMiniWeekdays => TurnoverWeekdays * KapasitasTrukMini;
            public decimal JumlahTerparkirTrukMiniWeekend => TurnoverWeekend * KapasitasTrukMini;
            public decimal OmzetTrukMini =>
                (JumlahTerparkirTrukMiniWeekdays * TarifTrukMini * 22) +
                (JumlahTerparkirTrukMiniWeekend * TarifTrukMini * 8);

            public decimal JumlahTerparkirTrukBusWeekdays => TurnoverWeekdays * KapasitasTrukBus;
            public decimal JumlahTerparkirTrukBusWeekend => TurnoverWeekend * KapasitasTrukBus;
            public decimal OmzetTrukBus =>
                (JumlahTerparkirTrukBusWeekdays * TarifTrukBus * 22) +
                (JumlahTerparkirTrukBusWeekend * TarifTrukBus * 8);

            public decimal JumlahTerparkirTrailerWeekdays => TurnoverWeekdays * KapasitasTrailer;
            public decimal JumlahTerparkirTrailerWeekend => TurnoverWeekend * KapasitasTrailer;
            public decimal OmzetTrailer =>
                (JumlahTerparkirTrailerWeekdays * TarifTrailer * 22) +
                (JumlahTerparkirTrailerWeekend * TarifTrailer * 8);

            // Total Omzet dan Pajak
            public decimal TotalOmzet =>
                OmzetSepeda + OmzetMotor + OmzetMobil + OmzetTrukMini + OmzetTrukBus + OmzetTrailer;

            public decimal PotensiPajakPerBulan => TotalOmzet * TarifPajak;

            public decimal PotensiPajakPerTahun => PotensiPajakPerBulan * BulanSisa;
        }
        public class DetailPotensiPajakHiburan
        {
            // Identitas
            public string NOP { get; set; }
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; } // "Bioskop", "Fitness/Pusat Kebugaran", atau lainnya
            public DateTime TglOpBuka { get; set; }
            public int BulanSisa
            {
                get
                {
                    var now = DateTime.Now;
                    var akhirTahun = new DateTime(now.Year, 12, 31);

                    if (TglOpBuka.Year < now.Year)
                        return 12;

                    if (TglOpBuka.Year > now.Year)
                        return 0;

                    int totalBulan = (12 - TglOpBuka.Month) + 1;

                    return totalBulan;
                }
            }

            // Umum
            public int Kapasitas { get; set; }
            public decimal HTMWeekdays { get; set; }
            public decimal HTMWeekend { get; set; }
            public decimal TurnoverWeekdays { get; set; } // 0.0 - 1.0
            public decimal TurnoverWeekend { get; set; }  // 0.0 - 1.0
            public decimal TarifPajak { get; set; }       // 0.0 - 1.0

            // Fitness
            public decimal HargaMemberFitness { get; set; }

            // Bioskop
            public int JumlahStudio { get; set; }
            public int KapasitasStudio { get; set; }

            // ========== Perhitungan Kategori Lainnya ==========
            public decimal JumlahPengunjungWeekdaysLainnya => Kapasitas * TurnoverWeekdays;
            public decimal JumlahPengunjungWeekendLainnya => Kapasitas * TurnoverWeekend;
            public decimal OmzetPerBulanLainnya =>
                (HTMWeekdays * JumlahPengunjungWeekdaysLainnya * 22) +
                (HTMWeekend * JumlahPengunjungWeekendLainnya * 8);
            public decimal PotensiPajakPerBulanLainnya => OmzetPerBulanLainnya * TarifPajak;
            public decimal PotensiPajakPerTahunLainnya => PotensiPajakPerBulanLainnya * BulanSisa;

            // ========== Perhitungan Kategori Bioskop ==========
            public int KapasitasBioskop => JumlahStudio * KapasitasStudio * 4;
            public decimal JumlahPengunjungWeekdaysBioskop => KapasitasBioskop * TurnoverWeekdays;
            public decimal JumlahPengunjungWeekendBioskop => KapasitasBioskop * TurnoverWeekend;
            public decimal OmzetPerBulanBioskop =>
                (HTMWeekdays * JumlahPengunjungWeekdaysBioskop * 22) +
                (HTMWeekend * JumlahPengunjungWeekendBioskop * 8);
            public decimal PotensiPajakPerBulanBioskop => OmzetPerBulanBioskop * TarifPajak;
            public decimal PotensiPajakPerTahunBioskop => PotensiPajakPerBulanBioskop * BulanSisa;

            // ========== Perhitungan Kategori Fitness/Pusat Kebugaran ==========
            public decimal EstimasiJumlahMemberFitnes =>
                ((Kapasitas * TurnoverWeekdays * 22) + (Kapasitas * TurnoverWeekend * 8)) / 12;
            public decimal OmzetPerBulanFitnes => HargaMemberFitness * EstimasiJumlahMemberFitnes;
            public decimal PotensiPajakPerBulanFitnes => OmzetPerBulanFitnes * TarifPajak;
            public decimal PotensiPajakPerTahunFitnes => PotensiPajakPerBulanFitnes * BulanSisa;
        }
        #endregion

        public class DetailParkir
        {
            public InfoDasar DataDasar { get; set; } = new();
            public KapasitasTarif DataKapasitas { get; set; } = new();
            public PotensiPajak DataPotensi { get; set; } = new();

            public DetailParkir() { }

            // Konstruktor untuk mengambil semua data berdasarkan NOP
            public DetailParkir(string nop)
            {
                // Panggil method untuk mengambil semua data dummy
                DataDasar = Method.GetInfoDasar(nop);
                DataKapasitas = Method.GetKapasitasTarif(nop);
                DataPotensi = new PotensiPajak(DataKapasitas); // Potensi dihitung dari kapasitas
            }
        }

        // Class untuk data di kartu paling atas
        public class InfoDasar
        {
            public string NamaWP { get; set; } = "";
            public string Alamat { get; set; } = "";
            public string NOP { get; set; } = "";
            public decimal TotalKapasitas => KapasitasMotor + KapasitasMobil;
            public decimal KapasitasMotor { get; set; }
            public decimal KapasitasMobil { get; set; }

            public decimal PersenKapasitasMotor => TotalKapasitas > 0 ? (KapasitasMotor / TotalKapasitas) * 100 : 0;

            // Menghitung persentase kapasitas mobil dari total
            public decimal PersenKapasitasMobil => TotalKapasitas > 0 ? (KapasitasMobil / TotalKapasitas) * 100 : 0;

            // Hotel
            public int? JumlahKamar { get; set; }
            public int? KapasitasBanquet { get; set; }

            // Bioskop
            public int? KapasitasKursiStudio { get; set; }
        }

        // Class untuk data di kartu kapasitas dan tarif
        public class KapasitasTarif
        {
            public int KapasitasMotor { get; set; }
            public int TerpakaiMotorHariKerja { get; set; }
            public int TerpakaiMotorAkhirPekan { get; set; }
            public decimal TarifMotor { get; set; }

            public int KapasitasMobil { get; set; }
            public int TerpakaiMobilHariKerja { get; set; }
            public int TerpakaiMobilAkhirPekan { get; set; }
            public decimal TarifMobil { get; set; }

            // Properti kalkulasi untuk persentase pemakaian
            public decimal PersenMotorHariKerja => KapasitasMotor > 0 ? ((decimal)TerpakaiMotorHariKerja / KapasitasMotor) * 100 : 0;
            public decimal PersenMotorAkhirPekan => KapasitasMotor > 0 ? ((decimal)TerpakaiMotorAkhirPekan / KapasitasMotor) * 100 : 0;
            public decimal PersenMobilHariKerja => KapasitasMobil > 0 ? ((decimal)TerpakaiMobilHariKerja / KapasitasMobil) * 100 : 0;
            public decimal PersenMobilAkhirPekan => KapasitasMobil > 0 ? ((decimal)TerpakaiMobilAkhirPekan / KapasitasMobil) * 100 : 0;

            public int TotalTerparkirHariKerja => TerpakaiMotorHariKerja + TerpakaiMobilHariKerja;
            public int TotalTerparkirAkhirPekan => TerpakaiMotorAkhirPekan + TerpakaiMobilAkhirPekan;
        }

        // Class untuk data di kartu perhitungan potensi
        public class PotensiPajak
        {
            public decimal OmzetMotor { get; }
            public decimal OmzetMobil { get; }
            public decimal TotalOmzetBulanan { get; }
            public decimal PotensiBulanan { get; }
            public decimal PotensiTahunan { get; }
            public const decimal TarifPajak = 0.10m; // 10%

            // Konstruktor untuk menghitung semua potensi
            public PotensiPajak() { }
            public PotensiPajak(KapasitasTarif dataKapasitas)
            {
                decimal omzetMotorHariKerja = dataKapasitas.TerpakaiMotorHariKerja * dataKapasitas.TarifMotor * 22;
                decimal omzetMotorAkhirPekan = dataKapasitas.TerpakaiMotorAkhirPekan * dataKapasitas.TarifMotor * 8;
                OmzetMotor = omzetMotorHariKerja + omzetMotorAkhirPekan;

                decimal omzetMobilHariKerja = dataKapasitas.TerpakaiMobilHariKerja * dataKapasitas.TarifMobil * 22;
                decimal omzetMobilAkhirPekan = dataKapasitas.TerpakaiMobilAkhirPekan * dataKapasitas.TarifMobil * 8;
                OmzetMobil = omzetMobilHariKerja + omzetMobilAkhirPekan;

                TotalOmzetBulanan = OmzetMotor + OmzetMobil;
                PotensiBulanan = TotalOmzetBulanan * TarifPajak;
                PotensiTahunan = PotensiBulanan * 12;
            }
        }

        // Class ini akan menjadi @model untuk halaman DetailHotel.cshtml
        public class DetailHotel
        {
            public InfoDasar DataDasar { get; set; } = new();
            public InfoKamar DataKamar { get; set; } = new();
            public InfoBanquet DataBanquet { get; set; } = new();
            public PotensiPajakHotel DataPotensi { get; set; } = new();

            public DetailHotel() { }
            public DetailHotel(string nop)
            {
                DataDasar = Method.GetInfoDasarHotel(nop);
                DataKamar = Method.GetInfoKamar(nop);
                DataBanquet = Method.GetInfoBanquet(nop);
                DataPotensi = new PotensiPajakHotel(DataKamar, DataBanquet);
            }
        }

        // Class untuk info kamar
        public class InfoKamar
        {
            public int JumlahKamar { get; set; }
            public decimal TarifRataRata { get; set; }
            public decimal TingkatHunian { get; set; } // Dalam persen (misal: 85 untuk 85%)
            public int RataKamarTerjual => (int)(JumlahKamar * (TingkatHunian / 100));
        }

        // Class untuk info banquet
        public class InfoBanquet
        {
            public int KapasitasMaksimum { get; set; }
            public decimal TingkatOkupansi { get; set; } // Dalam persen
            public decimal TarifRataRata { get; set; }
            public int HariEventPerBulan { get; set; }
            public int RataTamuBanquet => (int)(KapasitasMaksimum * (TingkatOkupansi / 100));
        }

        // Class untuk kalkulasi potensi pajak hotel
        public class PotensiPajakHotel
        {
            public decimal OmzetKamarBulanan { get; }
            public decimal PajakKamarBulanan { get; }
            public decimal OmzetBanquetBulanan { get; }
            public decimal PajakBanquetBulanan { get; }
            public decimal TotalPotensiBulanan { get; }
            public decimal TotalPotensiTahunan { get; }
            public const decimal TarifPajak = 0.10m; // 10%

            public decimal PersenPotensiKamar => TotalPotensiBulanan > 0 ? (PajakKamarBulanan / TotalPotensiBulanan) * 100 : 0;
            public decimal PersenPotensiBanquet => TotalPotensiBulanan > 0 ? (PajakBanquetBulanan / TotalPotensiBulanan) * 100 : 0;

            public PotensiPajakHotel() { }
            public PotensiPajakHotel(InfoKamar kamar, InfoBanquet banquet)
            {
                OmzetKamarBulanan = kamar.TarifRataRata * kamar.RataKamarTerjual * 30;
                PajakKamarBulanan = OmzetKamarBulanan * TarifPajak;

                OmzetBanquetBulanan = banquet.TarifRataRata * banquet.RataTamuBanquet * banquet.HariEventPerBulan;
                PajakBanquetBulanan = OmzetBanquetBulanan * TarifPajak;

                TotalPotensiBulanan = PajakKamarBulanan + PajakBanquetBulanan;
                TotalPotensiTahunan = TotalPotensiBulanan * 12;
            }
        }

        // Class ini akan menjadi @model untuk halaman DetailBioskop.cshtml
        public class DetailBioskop
        {
            public InfoDasar DataDasar { get; set; } = new();
            public InfoBioskop DataBioskop { get; set; } = new();
            public PotensiPajakBioskop DataPotensi { get; set; } = new();

            public DetailBioskop() { }
            public DetailBioskop(string nop)
            {
                DataDasar = Method.GetInfoDasarBioskop(nop);
                DataBioskop = Method.GetInfoBioskop(nop);
                DataPotensi = new PotensiPajakBioskop(DataBioskop);
            }
        }

        // Class untuk info spesifik bioskop
        public class InfoBioskop
        {
            public int KapasitasKursi { get; set; }
            public int KursiTerjualPerHari { get; set; }
            public decimal TurnoverHariKerja { get; set; }
            public decimal TurnoverAkhirPekan { get; set; }
            public decimal HargaTiketHariKerja { get; set; }
            public decimal HargaTiketAkhirPekan { get; set; }

            // Properti kalkulasi
            public int RataKunjunganHariKerja => (int)(KursiTerjualPerHari * TurnoverHariKerja);
            public int RataKunjunganAkhirPekan => (int)(KursiTerjualPerHari * TurnoverAkhirPekan);
        }

        // Class untuk kalkulasi potensi pajak bioskop
        public class PotensiPajakBioskop
        {
            public decimal OmzetHariKerja { get; }
            public decimal OmzetAkhirPekan { get; }
            public decimal TotalOmzetBulanan { get; }
            public decimal PotensiBulanan { get; }
            public decimal PotensiTahunan { get; }
            public const decimal TarifPajak = 0.10m; // 10%

            public PotensiPajakBioskop() { }
            public PotensiPajakBioskop(InfoBioskop dataBioskop)
            {
                OmzetHariKerja = dataBioskop.HargaTiketHariKerja * dataBioskop.RataKunjunganHariKerja * 22;
                OmzetAkhirPekan = dataBioskop.HargaTiketAkhirPekan * dataBioskop.RataKunjunganAkhirPekan * 8;
                TotalOmzetBulanan = OmzetHariKerja + OmzetAkhirPekan;
                PotensiBulanan = TotalOmzetBulanan * TarifPajak;
                PotensiTahunan = PotensiBulanan * 12;
            }
        }


    }
}
