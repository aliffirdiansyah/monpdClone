using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Globalization;

namespace MonPDReborn.Models.DataOP
{
    public class ProfilePotensiOPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }
        }

        public class ShowRekap
        {
            public List<RekapPotensi> DataRekapPotensi { get; set; } = new();
            public Dashboard DataDashboard { get; set; } = new();

            public ShowRekap() { }
            public ShowRekap(string jenisPajak)
            {
                DataRekapPotensi = Method.GetRekapPotensiList();
                DataDashboard = Method.GetDashboardData();
                // Total Potensi dari seluruh data rekap
                DataDashboard.Potensi = DataRekapPotensi.Sum(r => r.TotalPotensi1);

                // Realisasi total = total dari Realisasi1 + Realisasi2 + Realisasi3
                DataDashboard.RealisasiTotal = DataRekapPotensi.Sum(q => q.Realisasi3);

                // Capaian dalam persen: (RealisasiTotal / Potensi) * 100
                DataDashboard.Capaian = DataDashboard.Potensi == 0 ? 0 :
                    Math.Round((DataDashboard.RealisasiTotal / DataDashboard.Potensi) * 100, 2);

                // Jumlah OP
                DataDashboard.RealisasiOP = DataRekapPotensi.Sum(r => r.Realisasi3);

                // Capaian OP sebagai persen string
                //Data.CapaianOP = totalOP == 0 ? "0%" :
                //    Math.Round((decimal)realisasiOP / totalOP * 100, 2).ToString("0.##") + "%";
            }
        }


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
                var context = DBClass.GetContext();
                var Data = new Dashboard();
                var tahun = DateTime.Now.Year + 1; // tahun berjalan

                // Ambil jumlah OP unik dari masing-masing DbPotensi*
                var dataResto3 = context.DbPotensiRestos
                    .Where(x => x.TahunBuku == tahun)
                    .Select(x => x.Nop)
                    .Distinct()
                    .Count();

                var dataPpj3 = context.DbPotensiPpjs
                    .Where(x => x.TahunBuku == tahun)
                    .Select(x => x.Nop)
                    .Distinct()
                    .Count();

                var dataHotel3 = context.DbPotensiHotels
                    .Where(x => x.TahunBuku == tahun)
                    .Select(x => x.Nop)
                    .Distinct()
                    .Count();

                var dataParkir3 = context.DbPotensiParkirs
                    .Where(x => x.TahunBuku == tahun)
                    .Select(x => x.Nop)
                    .Distinct()
                    .Count();

                var dataHiburan3 = context.DbPotensiHiburans
                    .Where(x => x.TahunBuku == tahun)
                    .Select(x => x.Nop)
                    .Distinct()
                    .Count();

                var dataAbt3 = context.DbPotensiAbts
                    .Where(x => x.TahunBuku == tahun)
                    .Select(x => x.Nop)
                    .Distinct()
                    .Count();

                // Hitung total OP
                Data.TotalOP = dataResto3
                             + dataPpj3
                             + dataHotel3
                             + dataParkir3
                             + dataHiburan3
                             + dataAbt3;
                return Data;
            }

            public static List<RekapPotensi> GetRekapPotensiList()
            {
                var ret = new List<RekapPotensi>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                #region LIST OP BUKA
                var dataResto1 = context.DbOpRestos
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 2)) && x.PajakNama != "MAMIN")
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataPpj1 = context.DbOpListriks
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 2)))
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataHotel1 = context.DbOpHotels
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 2)))
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataParkir1 = context.DbOpParkirs
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 2)))
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataHiburan1 = context.DbOpHiburans
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 2)))
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataAbt1 = context.DbOpAbts
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 2)))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop })
                    .ToList();

                var dataResto2 = context.DbOpRestos
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 1)) && x.PajakNama != "MAMIN")
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataPpj2 = context.DbOpListriks
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 1)))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataHotel2 = context.DbOpHotels
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 1)))
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataParkir2 = context.DbOpParkirs
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 1)))
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataHiburan2 = context.DbOpHiburans
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 1)))
                    .GroupBy(x => new { x.Nop })
                    .Select(x => new { x.Key.Nop })
                    .ToList();
                var dataAbt2 = context.DbOpAbts
                    .Where(x => ((x.TahunBuku == DateTime.Now.Year - 1)))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop })
                    .ToList();

                var dataResto3 = context.DbOpRestos
                     .Where(x => x.TahunBuku == DateTime.Now.Year && x.PajakNama != "MAMIN" &&
                                 (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                     .GroupBy(x => new { x.Nop })
                     .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                     .ToList();

                var dataPpj3 = context.DbOpListriks
                    .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .ToList();

                var dataHotel3 = context.DbOpHotels
                    .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .ToList();

                var dataParkir3 = context.DbOpParkirs
                    .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .ToList();

                var dataHiburan3 = context.DbOpHiburans
                    .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .ToList();

                var dataHiburan3Ins = context.DbOpHiburans
                    .Where(x => x.TahunBuku == DateTime.Now.Year - 1 &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year - 1) && x.KategoriId == 64)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .ToList();

                var dataAbt3 = context.DbOpAbts
                    .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .ToList();
                #endregion

                #region Now
                var targetRestoNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoNow = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear && dataResto3.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelNow = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear && dataHotel3.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanNow = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear && dataHiburan3.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                realisasiHiburanNow += context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear && dataHiburan3Ins.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirNow = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear && dataParkir3.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikNow = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear && dataPpj3.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetReklameNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameNow = context.DbMonReklames.Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtNow = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear && dataAbt3.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;


                #endregion

                #region Mines1
                var targetRestoMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines1 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1 && dataResto2.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1 && dataHotel2.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1 && dataHiburan2.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1 && dataParkir2.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1 && dataPpj2.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetReklameMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines1 = context.DbMonReklames.Where(x => x.TahunBuku == currentYear - 1 && x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1 && dataAbt2.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                #endregion

                #region Mines2
                var targetRestoMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var realisasiRestoMines2 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2 && dataResto1.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHotelMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var realisasiHotelMines2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2 && dataHotel1.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetHiburanMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var realisasiHiburanMines2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2 && dataHiburan1.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetParkirMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var realisasiParkirMines2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2 && dataParkir1.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetListrikMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var realisasiListrikMines2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2 && dataPpj1.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetReklameMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var realisasiReklameMines2 = context.DbMonReklames.Where(x => x.TahunBuku == currentYear - 2 && x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                var targetAbtMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var realisasiAbtMines2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2 && dataAbt1.Select(x => x.Nop).ToList().Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                #endregion

                #region PotensioList();

                var potensiResto = context.DbPotensiRestos
                    .Where(x => dataResto3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                    .ToList()
                    .Select(x =>
                    {
                        var op = dataResto3.FirstOrDefault(o => o.Nop == x.Nop);

                        return new DetailPotensiPajakResto
                        {
                            NOP = x.Nop,
                            Kategori = "-",
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

                //var totalPotensiPpj = context.PotensiCtrlPpjs.Where(x => listOpPpjAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun) ?? 0;
                var totalPotensiPpj = context.DbPotensiPpjs
                    .Where(x => dataPpj3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                    .Sum(q => q.Hit1bulan * 12) ?? 0;

                var potensiHotel = context.DbPotensiHotels
                    .Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
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
                            HargaRataRataRoomKos = x.AvgRoomPriceKos ?? 0,
                            OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                            KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                            HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                            TarifPajak = 0.1m
                        };
                    })
                    .ToList();
                var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun + x.PotensiPajakPerTahunKos);

                var potensiParkir = context.DbPotensiParkirs
                    .Where(x => dataParkir3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                    .ToList()
                    .Select(x =>
                    {
                        var op = dataParkir3.FirstOrDefault(o => o.Nop == x.Nop);

                        return new DetailPotensiPajakParkir
                        {
                            NOP = x.Nop,
                            Kategori = "-",
                            TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                            Memungut = ((EnumFactory.EPungutTarifParkir)(x.JenisTarif ?? 0)).GetDescription(),
                            SistemParkir = ((EnumFactory.EPalangParkir)(x.SistemParkir ?? 0)).GetDescription(),
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

                //var totalPotensiHiburanBar = 0m;
                //var totalPotensiHiburanBioskop = 0m;
                var totalPotensiHiburan = 0m;
                var potensiHiburanNext1 = 0m;
                var potensiHiburanNext2 = 0m;
                var potensiHiburanNext3 = 0m;
                var potensiHiburanNext4 = 0m;



                var kategoriPajakHiburan = context.MKategoriPajaks.Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).ToList();
                foreach (var item in kategoriPajakHiburan.Where(x => x.Id != 0 && x.Id != 54).ToList())
                {
                    var nopList = dataHiburan3.Where(x => x.KategoriId == item.Id).AsEnumerable();

                    var potensiHiburan = context.DbPotensiHiburans
                    .Where(x => nopList.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                    .ToList()
                    .Select(x =>
                    {
                        var op = nopList.FirstOrDefault(o => o.Nop == x.Nop);

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
                            TarifPajak = op?.KategoriId == 44 ? 0.5m : op?.KategoriId == 41 ? 0.5m : op?.KategoriId == 48 ? 0.5m : op?.KategoriId == 45 ? 0.4m : 0.1m
                        };
                    })
                    .ToList();

                    if (item.Id == 41) //BAR/CAFE
                    {
                        totalPotensiHiburan += potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnyaBar);
                    }
                    else if (item.Id == 42) //BIOSKOP
                    {
                        totalPotensiHiburan += potensiHiburan.Sum(x => x.PotensiPajakPerTahunBioskop);
                    }
                    else if (item.Id == 64)
                    {
                        totalPotensiHiburan += context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 1).Sum(x => x.PotensiPajakTahun) ?? 0;
                    }
                    else // DLL
                    {
                        totalPotensiHiburan += potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnya + x.PotensiPajakPerTahunFitnes);
                    }
                    if (item.Id == 64)
                    {
                        potensiHiburanNext1 += context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                        potensiHiburanNext2 += context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                        potensiHiburanNext3 += context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                        potensiHiburanNext4 += context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;
                    }
                    else
                    {
                        potensiHiburanNext1 += context.DbPotensiHiburans.Where(x => nopList.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                        potensiHiburanNext2 += context.DbPotensiHiburans.Where(x => nopList.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                        potensiHiburanNext3 += context.DbPotensiHiburans.Where(x => nopList.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                        potensiHiburanNext4 += context.DbPotensiHiburans.Where(x => nopList.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;
                    }

                }


                //--------
                var totalPotensiAbt = context.DbPotensiAbts
                    .Where(x => dataAbt3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                    .Sum(q => q.Hit1bulan * 12) ?? 0;

                var totalPotensiReklame = context.DbPotensiReklames.Where(x => x.TahunBuku == DateTime.Now.Year + 1).Sum(q => q.Hit1bulan) ?? 0;

                //POTENSI TAHUN NEXT
                var potensiRestoNext1 = context.DbPotensiRestos.Where(x => dataResto3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiRestoNext2 = context.DbPotensiRestos.Where(x => dataResto3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiRestoNext3 = context.DbPotensiRestos.Where(x => dataResto3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiRestoNext4 = context.DbPotensiRestos.Where(x => dataResto3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                var potensiPpjNext1 = context.DbPotensiPpjs.Where(x => dataPpj3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.Hit1bulan * 12) ?? 0;
                var potensiPpjNext2 = context.DbPotensiPpjs.Where(x => dataPpj3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.Hit1bulan * 12) ?? 0;
                var potensiPpjNext3 = context.DbPotensiPpjs.Where(x => dataPpj3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.Hit1bulan * 12) ?? 0;
                var potensiPpjNext4 = context.DbPotensiPpjs.Where(x => dataPpj3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.Hit1bulan * 12) ?? 0;

                var potensiHotelNext1 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiHotelNext2 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiHotelNext3 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiHotelNext4 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                var potensiParkirNext1 = context.DbPotensiParkirs.Where(x => dataParkir3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiParkirNext2 = context.DbPotensiParkirs.Where(x => dataParkir3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiParkirNext3 = context.DbPotensiParkirs.Where(x => dataParkir3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiParkirNext4 = context.DbPotensiParkirs.Where(x => dataParkir3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;




                var potensiAbtNext1 = context.DbPotensiAbts.Where(x => dataAbt3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.Hit1bulan * 12) ?? 0;
                var potensiAbtNext2 = context.DbPotensiAbts.Where(x => dataAbt3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.Hit1bulan * 12) ?? 0;
                var potensiAbtNext3 = context.DbPotensiAbts.Where(x => dataAbt3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.Hit1bulan * 12) ?? 0;
                var potensiAbtNext4 = context.DbPotensiAbts.Where(x => dataAbt3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.Hit1bulan * 12) ?? 0;

                var potensiReklameNext1 = context.DbPotensiReklames.Where(x => x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.Hit1bulan) ?? 0;
                var potensiReklameNext2 = context.DbPotensiReklames.Where(x => x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.Hit1bulan) ?? 0;
                var potensiReklameNext3 = context.DbPotensiReklames.Where(x => x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.Hit1bulan) ?? 0;
                var potensiReklameNext4 = context.DbPotensiReklames.Where(x => x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.Hit1bulan) ?? 0;



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
                    TotalPotensi1 = totalPotensiResto,
                    TotalPotensi2 = potensiRestoNext1,
                    TotalPotensi3 = potensiRestoNext2,
                    TotalPotensi4 = potensiRestoNext3,
                    TotalPotensi5 = potensiRestoNext4
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
                    TotalPotensi1 = totalPotensiPpj,
                    TotalPotensi2 = potensiPpjNext1,
                    TotalPotensi3 = potensiPpjNext2,
                    TotalPotensi4 = potensiPpjNext3,
                    TotalPotensi5 = potensiPpjNext4
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
                    TotalPotensi1 = totalPotensiHotel,
                    TotalPotensi2 = potensiHotelNext1,
                    TotalPotensi3 = potensiHotelNext2,
                    TotalPotensi4 = potensiHotelNext3,
                    TotalPotensi5 = potensiHotelNext4
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
                    TotalPotensi1 = totalPotensiParkir,
                    TotalPotensi2 = potensiParkirNext1,
                    TotalPotensi3 = potensiParkirNext2,
                    TotalPotensi4 = potensiParkirNext3,
                    TotalPotensi5 = potensiParkirNext4
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
                    TotalPotensi1 = totalPotensiHiburan,
                    TotalPotensi2 = potensiHiburanNext1,
                    TotalPotensi3 = potensiHiburanNext2,
                    TotalPotensi4 = potensiHiburanNext3,
                    TotalPotensi5 = potensiHiburanNext4
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
                    TotalPotensi1 = totalPotensiAbt,
                    TotalPotensi2 = potensiAbtNext1,
                    TotalPotensi3 = potensiAbtNext2,
                    TotalPotensi4 = potensiAbtNext3,
                    TotalPotensi5 = potensiAbtNext4
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
                    TotalPotensi1 = totalPotensiReklame,
                    TotalPotensi2 = potensiReklameNext1,
                    TotalPotensi3 = potensiReklameNext2,
                    TotalPotensi4 = potensiReklameNext3,
                    TotalPotensi5 = potensiReklameNext4
                });

                /*ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.PBB,
                    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    Target3 = targetPbbNow,
                    Realisasi3 = realisasiPbbNow,
                    Target2 = targetPbbMines1,
                    Realisasi2 = realisasiPbbMines1,
                    Target1 = targetPbbMines2,
                    Realisasi1 = realisasiPbbMines2,
                    TotalPotensi = 0
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    Target3 = targetBphtbNow,
                    Realisasi3 = realisasiBphtbNow,
                    Target2 = targetBphtbMines1,
                    Realisasi2 = realisasiBphtbMines1,
                    Target1 = targetBphtbMines2,
                    Realisasi1 = realisasiBphtbMines2,
                    TotalPotensi = 0
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    Target3 = targetOpsenPkbNow,
                    Realisasi3 = realisasiOpsenPkbNow,
                    Target2 = targetOpsenPkbMines1,
                    Realisasi2 = realisasiOpsenPkbMines1,
                    Target1 = targetOpsenPkbMines2,
                    Realisasi1 = realisasiOpsenPkbMines2,
                    TotalPotensi = 0
                });

                ret.Add(new RekapPotensi
                {
                    EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    Target3 = targetOpsenBbnkbNow,
                    Realisasi3 = realisasiOpsenBbnkbNow,
                    Target2 = targetOpsenBbnkbMines1,
                    Realisasi2 = realisasiOpsenBbnkbMines1,
                    Target1 = targetOpsenBbnkbMines2,
                    Realisasi1 = realisasiOpsenBbnkbMines2,
                    TotalPotensi = 0
                });*/

                return ret;
            }
            public static List<DetailPotensi> GetDetailPotensiList(EnumFactory.EPajak jenisPajak)
            {
                var ret = new List<DetailPotensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak)
                    .OrderBy(x => x.Urutan)
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
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.PajakNama != "MAMIN")
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataResto2 = context.DbOpRestos
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.PajakNama != "MAMIN")
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();
                        var dataResto3 = context.DbOpRestos
                         .Where(x => x.TahunBuku == DateTime.Now.Year && x.PajakNama != "MAMIN" &&
                                     (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                         .GroupBy(x => new { x.Nop })
                         .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
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

                            var targetResto1 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetResto2 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetResto3 = context.DbAkunTargetObjekRestos.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiResto1 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpResto1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiResto2 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpResto2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiResto3 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpResto3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var potensiResto = context.DbPotensiRestos
                            .Where(x => listOpResto3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
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

                            var potensiRestoNext1 = context.DbPotensiRestos.Where(x => listOpResto3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiRestoNext2 = context.DbPotensiRestos.Where(x => listOpResto3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiRestoNext3 = context.DbPotensiRestos.Where(x => listOpResto3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiRestoNext4 = context.DbPotensiRestos.Where(x => listOpResto3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                            re.Target1 = targetResto1;
                            re.Realisasi1 = realisasiResto1;
                            re.Target2 = targetResto2;
                            re.Realisasi2 = realisasiResto2;
                            re.Target3 = targetResto3;
                            re.Realisasi3 = realisasiResto3;
                            re.TotalPotensi1 = totalPotensiResto;
                            re.TotalPotensi2 = potensiRestoNext1;
                            re.TotalPotensi3 = potensiRestoNext2;
                            re.TotalPotensi4 = potensiRestoNext3;
                            re.TotalPotensi5 = potensiRestoNext4;

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
                        .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                    (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
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

                            var targetListrik1 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetListrik2 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetListrik3 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiListrik1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpListrik1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiListrik2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpListrik2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiListrik3 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpListrik3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var totalPotensiListrik = context.DbPotensiPpjs.Where(x => listOpListrik3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1).Sum(q => q.Hit1bulan * 12) ?? 0;

                            var potensiPpjNext1 = context.DbPotensiPpjs.Where(x => listOpListrikAll.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.Hit1bulan * 12) ?? 0;
                            var potensiPpjNext2 = context.DbPotensiPpjs.Where(x => listOpListrikAll.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.Hit1bulan * 12) ?? 0;
                            var potensiPpjNext3 = context.DbPotensiPpjs.Where(x => listOpListrikAll.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.Hit1bulan * 12) ?? 0;
                            var potensiPpjNext4 = context.DbPotensiPpjs.Where(x => listOpListrikAll.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.Hit1bulan * 12) ?? 0;

                            re.Target1 = targetListrik1;
                            re.Realisasi1 = realisasiListrik1;
                            re.Target2 = targetListrik2;
                            re.Realisasi2 = realisasiListrik2;
                            re.Target3 = targetListrik3;
                            re.Realisasi3 = realisasiListrik3;
                            re.TotalPotensi1 = totalPotensiListrik;
                            re.TotalPotensi2 = potensiPpjNext1;
                            re.TotalPotensi3 = potensiPpjNext2;
                            re.TotalPotensi4 = potensiPpjNext3;
                            re.TotalPotensi5 = potensiPpjNext4;


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
                        .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                    (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                        .GroupBy(x => new { x.Nop })
                        .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                        .ToList();



                        foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpHotel1 = dataHotel1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHotel2 = dataHotel2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHotel3 = dataHotel3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetHotel1 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetHotel2 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetHotel3 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiHotel1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHotel1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHotel2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHotel2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHotel3 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHotel3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var potensiHotel = context.DbPotensiHotels
                                .Where(x => listOpHotel3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = context.DbOpHotels.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHotel
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
                                        Kategori = "-",
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        JumlahTotalRoom = x.TotalRoom ?? 0,
                                        HargaRataRataRoom = x.AvgRoomPrice ?? 0,
                                        HargaRataRataRoomKos = x.AvgRoomPriceKos ?? 0,
                                        OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                                        KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                                        HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun + x.PotensiPajakPerTahunKos);

                            var potensiHotelNext1 = context.DbPotensiHotels.Where(x => listOpHotel3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiHotelNext2 = context.DbPotensiHotels.Where(x => listOpHotel3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiHotelNext3 = context.DbPotensiHotels.Where(x => listOpHotel3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiHotelNext4 = context.DbPotensiHotels.Where(x => listOpHotel3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                            re.Target1 = targetHotel1;
                            re.Realisasi1 = realisasiHotel1;
                            re.Target2 = targetHotel2;
                            re.Realisasi2 = realisasiHotel2;
                            re.Target3 = targetHotel3;
                            re.Realisasi3 = realisasiHotel3;
                            re.TotalPotensi1 = totalPotensiHotel;
                            re.TotalPotensi2 = potensiHotelNext1;
                            re.TotalPotensi3 = potensiHotelNext2;
                            re.TotalPotensi4 = potensiHotelNext3;
                            re.TotalPotensi5 = potensiHotelNext4;

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
                        .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                    (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                        .GroupBy(x => new { x.Nop })
                        .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
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

                            var targetParkir1 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetParkir2 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetParkir3 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiParkir1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpParkir1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiParkir2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpParkir2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiParkir3 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpParkir3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var potensiParkir = context.DbPotensiParkirs
                                .Where(x => listOpParkir3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = context.DbOpParkirs.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakParkir
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
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

                            var potensiParkirNext1 = context.DbPotensiParkirs.Where(x => listOpParkir3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiParkirNext2 = context.DbPotensiParkirs.Where(x => listOpParkir3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiParkirNext3 = context.DbPotensiParkirs.Where(x => listOpParkir3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                            var potensiParkirNext4 = context.DbPotensiParkirs.Where(x => listOpParkir3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                            re.Target1 = targetParkir1;
                            re.Realisasi1 = realisasiParkir1;
                            re.Target2 = targetParkir2;
                            re.Realisasi2 = realisasiParkir2;
                            re.Target3 = targetParkir3;
                            re.Realisasi3 = realisasiParkir3;
                            re.TotalPotensi1 = totalPotensiParkir;
                            re.TotalPotensi2 = potensiParkirNext1;
                            re.TotalPotensi3 = potensiParkirNext2;
                            re.TotalPotensi4 = potensiParkirNext3;
                            re.TotalPotensi5 = potensiParkirNext4;

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
                        .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                    (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                        .GroupBy(x => new { x.Nop })
                        .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                        .ToList();
                        var dataHiburan3Ins = context.DbOpHiburans
                        .Where(x => x.TahunBuku == DateTime.Now.Year - 1 &&
                                    (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year - 1))
                        .GroupBy(x => new { x.Nop })
                        .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                        .ToList();


                        foreach (var item in kategoriList.Where(x => x.Id != 0 && x.Id != 54).ToList())
                        {

                            var re = new DetailPotensi();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.Kategori = item.Nama;
                            re.EnumPajak = (int)jenisPajak;
                            re.EnumKategori = (int)item.Id;

                            var listOpHiburan1 = dataHiburan1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHiburan2 = dataHiburan2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHiburan3 = dataHiburan3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpHiburan3Ins = dataHiburan3Ins.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var totalPotensiHiburan = 0m;
                            var realisasiHiburan1 = 0m;
                            var realisasiHiburan2 = 0m;
                            var realisasiHiburan3 = 0m;
                            if (item.Id != 64)
                            {
                                var targetHiburan1 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id && x.Insidentil == 0).Sum(q => q.TargetBulan) ?? 0;
                                var targetHiburan2 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id && x.Insidentil == 0).Sum(q => q.TargetBulan) ?? 0;
                                var targetHiburan3 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id && x.Insidentil == 0).Sum(q => q.TargetBulan) ?? 0;
                                re.Target1 = targetHiburan1;
                                re.Target2 = targetHiburan2;
                                re.Target3 = targetHiburan3;

                                var potensiHiburanNext1 = context.DbPotensiHiburans.Where(x => listOpHiburan3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                                var potensiHiburanNext2 = context.DbPotensiHiburans.Where(x => listOpHiburan3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                                var potensiHiburanNext3 = context.DbPotensiHiburans.Where(x => listOpHiburan3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                                var potensiHiburanNext4 = context.DbPotensiHiburans.Where(x => listOpHiburan3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                                re.TotalPotensi2 = potensiHiburanNext1;
                                re.TotalPotensi3 = potensiHiburanNext2;
                                re.TotalPotensi4 = potensiHiburanNext3;
                                re.TotalPotensi5 = potensiHiburanNext4;

                                realisasiHiburan1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHiburan1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                                realisasiHiburan2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHiburan2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                                realisasiHiburan3 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHiburan3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            }
                            else
                            {
                                var targetHiburan1 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id && x.Insidentil == 1).Sum(q => q.TargetBulan) ?? 0;
                                var targetHiburan2 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id && x.Insidentil == 1).Sum(q => q.TargetBulan) ?? 0;
                                var targetHiburan3 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id && x.Insidentil == 1).Sum(q => q.TargetBulan) ?? 0;
                                re.Target1 = targetHiburan1;
                                re.Target2 = targetHiburan2;
                                re.Target3 = targetHiburan3;
                                totalPotensiHiburan = context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 1).Sum(x => x.PotensiPajakTahun) ?? 0;

                                var potensiHiburanNext1 = context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                                var potensiHiburanNext2 = context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                                var potensiHiburanNext3 = context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                                var potensiHiburanNext4 = context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                                re.TotalPotensi1 = totalPotensiHiburan;
                                re.TotalPotensi2 = potensiHiburanNext1;
                                re.TotalPotensi3 = potensiHiburanNext2;
                                re.TotalPotensi4 = potensiHiburanNext3;
                                re.TotalPotensi5 = potensiHiburanNext4;

                                realisasiHiburan1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHiburan1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                                realisasiHiburan2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHiburan2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                                realisasiHiburan3 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHiburan3Ins.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            }


                            var potensiHiburan = context.DbPotensiHiburans
                                .Where(x => listOpHiburan3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = context.DbOpHiburans.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHiburan
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
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
                                        TarifPajak = op?.KategoriId == 44 ? 0.5m : op?.KategoriId == 41 ? 0.5m : op?.KategoriId == 48 ? 0.5m : op?.KategoriId == 45 ? 0.4m : 0.1m
                                    };
                                })
                                .ToList();
                            if (item.Id == 41) //BAR/CAFE
                            {
                                totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnyaBar);
                            }
                            else if (item.Id == 42) //BIOSKOP
                            {
                                totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunBioskop);
                            }
                            else if (item.Id != 64)
                            {
                                totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnya + x.PotensiPajakPerTahunFitnes);
                            }




                            re.Realisasi1 = realisasiHiburan1;
                            re.Realisasi2 = realisasiHiburan2;
                            re.Realisasi3 = realisasiHiburan3;

                            re.TotalPotensi1 = totalPotensiHiburan;


                            ret.Add(re);
                        }
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
                        .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                    (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                        .GroupBy(x => new { x.Nop, x.KategoriId })
                        .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
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

                            var targetAbt1 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetAbt2 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var targetAbt3 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id).Sum(q => q.TargetBulan) ?? 0;
                            var realisasiAbt1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpAbt1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiAbt2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpAbt2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiAbt3 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpAbt3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                            var totalPotensiAbt = context.DbPotensiAbts.Where(x => listOpAbt3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1).Sum(q => q.Hit1bulan * 12) ?? 0;

                            var potensiAbtNext1 = context.DbPotensiAbts.Where(x => listOpAbt3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.Hit1bulan * 12) ?? 0;
                            var potensiAbtNext2 = context.DbPotensiAbts.Where(x => listOpAbt3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.Hit1bulan * 12) ?? 0;
                            var potensiAbtNext3 = context.DbPotensiAbts.Where(x => listOpAbt3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.Hit1bulan * 12) ?? 0;
                            var potensiAbtNext4 = context.DbPotensiAbts.Where(x => listOpAbt3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.Hit1bulan * 12) ?? 0;

                            re.Target1 = targetAbt1;
                            re.Realisasi1 = realisasiAbt1;
                            re.Target2 = targetAbt2;
                            re.Realisasi2 = realisasiAbt2;
                            re.Target3 = targetAbt3;
                            re.Realisasi3 = realisasiAbt3;
                            re.TotalPotensi1 = totalPotensiAbt;
                            re.TotalPotensi2 = potensiAbtNext1;
                            re.TotalPotensi3 = potensiAbtNext2;
                            re.TotalPotensi4 = potensiAbtNext3;
                            re.TotalPotensi5 = potensiAbtNext4;

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

                            var targetReklame1 = context.DbAkunTargetObjekReklames.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == item.Id).Sum(x => x.TargetBulan) ?? 0;
                            var realisasiReklame1 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && x.FlagPermohonan.ToUpper() == item.Nama.ToUpper()).Sum(x => x.NominalPokokBayar) ?? 0;
                            var targetReklame2 = context.DbAkunTargetObjekReklames.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == item.Id).Sum(x => x.TargetBulan) ?? 0;
                            var realisasiReklame2 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && x.FlagPermohonan.ToUpper() == item.Nama.ToUpper()).Sum(x => x.NominalPokokBayar) ?? 0;
                            var targetReklame3 = context.DbAkunTargetObjekReklames.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == item.Id).Sum(x => x.TargetBulan) ?? 0;
                            var realisasiReklame3 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && x.FlagPermohonan.ToUpper() == item.Nama.ToUpper()).Sum(x => x.NominalPokokBayar) ?? 0;

                            var totalPotensiReklame = context.DbPotensiReklames.Where(x => x.FlagPermohonan.ToUpper() == item.Nama.ToUpper() && x.TahunBuku == DateTime.Now.Year + 1).Sum(q => q.Hit1bulan) ?? 0;

                            var potensiReklameNext1 = context.DbPotensiReklames.Where(x => x.FlagPermohonan.ToUpper() == item.Nama.ToUpper() && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.Hit1bulan) ?? 0;
                            var potensiReklameNext2 = context.DbPotensiReklames.Where(x => x.FlagPermohonan.ToUpper() == item.Nama.ToUpper() && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.Hit1bulan) ?? 0;
                            var potensiReklameNext3 = context.DbPotensiReklames.Where(x => x.FlagPermohonan.ToUpper() == item.Nama.ToUpper() && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.Hit1bulan) ?? 0;
                            var potensiReklameNext4 = context.DbPotensiReklames.Where(x => x.FlagPermohonan.ToUpper() == item.Nama.ToUpper() && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.Hit1bulan) ?? 0;

                            re.Target1 = targetReklame1;
                            re.Realisasi1 = realisasiReklame1;
                            re.Target2 = targetReklame2;
                            re.Realisasi2 = realisasiReklame2;
                            re.Target3 = targetReklame3;
                            re.Realisasi3 = realisasiReklame3;
                            re.TotalPotensi1 = totalPotensiReklame;
                            re.TotalPotensi2 = potensiReklameNext1;
                            re.TotalPotensi3 = potensiReklameNext2;
                            re.TotalPotensi4 = potensiReklameNext3;
                            re.TotalPotensi5 = potensiReklameNext4;


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
            public static List<DetailPotensi> GetDetailPotensiList(EnumFactory.EPajak jenisPajak, bool turu)
            {
                var ret = new List<DetailPotensi>();
                var context = DBClass.GetContext();

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
                .Where(x => x.TahunBuku == DateTime.Now.Year &&
                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year))
                .GroupBy(x => new { x.Nop })
                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                .ToList();




                var kategori = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak && x.Id == 17)
                    .FirstOrDefault();

                var re = new DetailPotensi();
                re.JenisPajak = jenisPajak.GetDescription();
                re.Kategori = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kategori.Nama.ToLower());
                re.EnumPajak = (int)jenisPajak;
                re.EnumKategori = (int)kategori.Id;

                var listOpHotel1 = dataHotel1.Where(x => x.KategoriId == kategori.Id).Select(x => x.Nop).ToList();
                var listOpHotel2 = dataHotel2.Where(x => x.KategoriId == kategori.Id).Select(x => x.Nop).ToList();
                var listOpHotel3 = dataHotel3.Where(x => x.KategoriId == kategori.Id).Select(x => x.Nop).ToList();

                var targetHotel1 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori.Id).Sum(q => q.TargetBulan) ?? 0;
                var targetHotel2 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori.Id).Sum(q => q.TargetBulan) ?? 0;
                var targetHotel3 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori.Id).Sum(q => q.TargetBulan) ?? 0;
                var realisasiHotel1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHotel1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHotel2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHotel2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var realisasiHotel3 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHotel3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var potensiHotel = context.DbPotensiHotels
                    .Where(x => listOpHotel3.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 1)
                    .ToList()
                    .Select(x =>
                    {
                        var op = context.DbOpHotels.FirstOrDefault(o => o.Nop == x.Nop);

                        return new DetailPotensiPajakHotel
                        {
                            NOP = x.Nop,
                            Nama = op?.NamaOp ?? "-",
                            Alamat = op?.AlamatOp ?? "-",
                            Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
                            Kategori = "-",
                            TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                            JumlahTotalRoom = x.TotalRoom ?? 0,
                            HargaRataRataRoom = x.AvgRoomPrice ?? 0,
                            HargaRataRataRoomKos = x.AvgRoomPriceKos ?? 0,
                            OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                            KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                            HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                            TarifPajak = 0.1m
                        };
                    })
                    .ToList();
                var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun + x.PotensiPajakPerTahunKos);

                var potensiHotelNext1 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 2).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiHotelNext2 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 3).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiHotelNext3 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 4).Sum(x => x.PotensiPajakTahun) ?? 0;
                var potensiHotelNext4 = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year + 5).Sum(x => x.PotensiPajakTahun) ?? 0;

                re.Target1 = targetHotel1;
                re.Realisasi1 = realisasiHotel1;
                re.Target2 = targetHotel2;
                re.Realisasi2 = realisasiHotel2;
                re.Target3 = targetHotel3;
                re.Realisasi3 = realisasiHotel3;
                re.TotalPotensi1 = totalPotensiHotel;
                re.TotalPotensi2 = potensiHotelNext1;
                re.TotalPotensi3 = potensiHotelNext2;
                re.TotalPotensi4 = potensiHotelNext3;
                re.TotalPotensi5 = potensiHotelNext4;

                ret.Add(re);

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
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.KategoriId == kategori && x.PajakNama != "MAMIN")
                            .ToList();
                        var dataResto2 = context.DbOpRestos
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.KategoriId == kategori && x.PajakNama != "MAMIN")
                            .ToList();
                        var dataResto3 = context.DbOpRestos
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year)) && x.KategoriId == kategori && x.PajakNama != "MAMIN")
                            .ToList();

                        var distinctNopResto = dataResto3.Select(x => x.Nop).Distinct().ToList();

                        string katResto = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum";
                        var dataTargetResto1 = context.DbAkunTargetObjekRestos
                            .Where(x => distinctNopResto.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop })
                            .Select(x => new
                            {
                                Nop = x.Key.Nop,
                                Nominal = x.Sum(q => q.TargetBulan)
                            }).ToList();

                        var dataRealisasiResto1 = context.DbMonRestos
                            .Where(x => distinctNopResto.Contains(x.Nop) && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nop })
                            .Select(x => new
                            {
                                Nop = x.Key.Nop,
                                Nominal = x.Sum(q => q.NominalPokokBayar)
                            }).ToList();

                        var dataTargetResto2 = context.DbAkunTargetObjekRestos
                            .Where(x => distinctNopResto.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop })
                            .Select(x => new
                            {
                                Nop = x.Key.Nop,
                                Nominal = x.Sum(q => q.TargetBulan)
                            }).ToList();

                        var dataRealisasiResto2 = context.DbMonRestos
                            .Where(x => distinctNopResto.Contains(x.Nop) && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nop })
                            .Select(x => new
                            {
                                Nop = x.Key.Nop,
                                Nominal = x.Sum(q => q.NominalPokokBayar)
                            }).ToList();

                        var dataTargetResto3 = context.DbAkunTargetObjekRestos
                            .Where(x => distinctNopResto.Contains(x.Nop) && x.TahunBuku == DateTime.Now.Year)
                            .GroupBy(x => new { x.Nop })
                            .Select(x => new
                            {
                                Nop = x.Key.Nop,
                                Nominal = x.Sum(q => q.TargetBulan)
                            }).ToList();

                        var dataRealisasiResto3 = context.DbMonRestos
                            .Where(x => distinctNopResto.Contains(x.Nop) && x.TglBayarPokok.Value.Year == DateTime.Now.Year)
                            .GroupBy(x => new { x.Nop })
                            .Select(x => new
                            {
                                Nop = x.Key.Nop,
                                Nominal = x.Sum(q => q.NominalPokokBayar)
                            }).ToList();

                        var dictTargetResto1 = dataTargetResto1.ToDictionary(x => x.Nop, x => x.Nominal);
                        var dictRealisasiResto1 = dataRealisasiResto1.ToDictionary(x => x.Nop, x => x.Nominal);
                        var dictTargetResto2 = dataTargetResto2.ToDictionary(x => x.Nop, x => x.Nominal);
                        var dictRealisasiResto2 = dataRealisasiResto2.ToDictionary(x => x.Nop, x => x.Nominal);
                        var dictTargetResto3 = dataTargetResto3.ToDictionary(x => x.Nop, x => x.Nominal);
                        var dictRealisasiResto3 = dataRealisasiResto3.ToDictionary(x => x.Nop, x => x.Nominal);

                        foreach (var item in dataResto3.Distinct())
                        {
                            var potensiResto = context.DbPotensiRestos
                            .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year + 1)
                            .ToList()
                            .Select(x =>
                            {
                                var op = dataResto3.FirstOrDefault(o => o.Nop == x.Nop);

                                return new DetailPotensiPajakResto
                                {
                                    NOP = x.Nop,
                                    Nama = op?.NamaOp ?? "-",
                                    Alamat = op?.AlamatOp ?? "-",
                                    Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
                                    Kategori = context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(op.KategoriId)).Nama ?? "Umum",
                                    KategoriId = Convert.ToInt32(op.KategoriId),
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
                                KategoriId = kategori,
                                EnumPajak = (int)jenisPajak,
                                Kategori = katResto,
                                Target1 = dictTargetResto1.TryGetValue(item.Nop, out var t1) ? t1 ?? 0 : 0,
                                Realisasi1 = dictRealisasiResto1.TryGetValue(item.Nop, out var r1) ? r1 ?? 0 : 0,
                                Target2 = dictTargetResto2.TryGetValue(item.Nop, out var t2) ? t2 ?? 0 : 0,
                                Realisasi2 = dictRealisasiResto2.TryGetValue(item.Nop, out var r2) ? r2 ?? 0 : 0,
                                Target3 = dictTargetResto3.TryGetValue(item.Nop, out var t3) ? t3 ?? 0 : 0,
                                Realisasi3 = dictRealisasiResto3.TryGetValue(item.Nop, out var r3) ? r3 ?? 0 : 0,
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
                        .Where(x => x.TahunBuku == DateTime.Now.Year &&
                                    (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > DateTime.Now.Year) && x.KategoriId == kategori)
                        .ToList();


                        foreach (var item in dataListrik3.Distinct())
                        {
                            var totalPotensiListrik = context.DbPotensiPpjs.Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year + 1).Sum(q => q.Hit1bulan * 12) ?? 0;
                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                KategoriId = kategori,
                                EnumPajak = (int)jenisPajak,
                                Kategori = context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(item.KategoriId)).Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi1 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi2 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekPpjs.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
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
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year)) && x.KategoriId == kategori)
                            .ToList();

                        foreach (var item in dataHotel3.Distinct())
                        {
                            var potensiHotel = context.DbPotensiHotels
                                .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year + 1)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = dataHotel3.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHotel
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
                                        Kategori = context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(op.KategoriId)).Nama ?? "Umum",
                                        KategoriId = Convert.ToInt32(op.KategoriId),
                                        TglOpBuka = op?.TglMulaiBukaOp ?? DateTime.MinValue,
                                        JumlahTotalRoom = x.TotalRoom ?? 0,
                                        HargaRataRataRoom = x.AvgRoomPrice ?? 0,
                                        HargaRataRataRoomKos = x.AvgRoomPriceKos ?? 0,
                                        OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                                        KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                                        HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                                        TarifPajak = 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun + x.PotensiPajakPerTahunKos);

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                KategoriId = kategori,
                                EnumPajak = (int)jenisPajak,
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi1 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi2 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekHotels.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
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
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year)) && x.KategoriId == kategori)
                            .ToList();

                        foreach (var item in dataParkir3.Distinct())
                        {
                            var potensiParkir = context.DbPotensiParkirs
                                .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year + 1)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = dataParkir3.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakParkir
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
                                        Kategori = context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(op.KategoriId)).Nama ?? "Umum",
                                        KategoriId = Convert.ToInt32(op.KategoriId),
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
                                Kategori = context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(item.KategoriId)).Nama ?? "Umum",
                                KategoriId = Convert.ToInt32(item.KategoriId),
                                EnumPajak = (int)jenisPajak,
                                Target1 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi1 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi2 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekParkirs.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
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
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year)) && x.KategoriId == kategori)
                            .ToList();
                        if (kategori == 64)
                        {
                            var potensi = new DataPotensi
                            {
                                NOP = "000000000090300000",
                                NamaOP = "INSIDENTIL",
                                Alamat = "INSIDENTIL",
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = "INSIDENTIL",
                                KategoriId = Convert.ToInt32(kategori),
                                EnumPajak = (int)jenisPajak,
                                Target1 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && dataHiburan3.Select(y => y.Nop).ToList().Contains(x.Nop) && x.Insidentil == 1).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi1 = context.DbMonHiburans.Where(x => dataHiburan3.Select(y => y.Nop).ToList().Contains(x.Nop) && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && dataHiburan3.Select(y => y.Nop).ToList().Contains(x.Nop) && x.Insidentil == 1).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi2 = context.DbMonHiburans.Where(x => dataHiburan3.Select(y => y.Nop).ToList().Contains(x.Nop) && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && dataHiburan3.Select(y => y.Nop).ToList().Contains(x.Nop) && x.Insidentil == 1).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi3 = context.DbMonHiburans.Where(x => dataHiburan3.Select(y => y.Nop).ToList().Contains(x.Nop) && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = context.DbPotensiHiburans.Where(x => x.Nop == "000000000090300000" && x.TahunBuku == DateTime.Now.Year + 1).Sum(x => x.PotensiPajakTahun) ?? 0
                            };
                            ret.Add(potensi);
                        }
                        foreach (var item in dataHiburan3.Distinct())
                        {
                            var potensiHiburan = context.DbPotensiHiburans
                                .Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year + 1)
                                .ToList()
                                .Select(x =>
                                {
                                    var op = dataHiburan3.FirstOrDefault(o => o.Nop == x.Nop);

                                    return new DetailPotensiPajakHiburan
                                    {
                                        NOP = x.Nop,
                                        Nama = op?.NamaOp ?? "-",
                                        Alamat = op?.AlamatOp ?? "-",
                                        Wilayah = "SURABAYA " + op?.WilayahPajak ?? "-",
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
                                        TarifPajak = op?.KategoriId == 44 ? 0.5m : op?.KategoriId == 41 ? 0.5m : op?.KategoriId == 48 ? 0.5m : op?.KategoriId == 45 ? 0.4m : 0.1m
                                    };
                                })
                                .ToList();
                            var totalPotensiHiburan = 0m;
                            if (kategori == 41)
                            {
                                totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnyaBar);
                            }
                            else if (kategori == 42)
                            {
                                totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunBioskop);
                            }
                            else
                            {
                                totalPotensiHiburan = potensiHiburan.Sum(x => x.PotensiPajakPerTahunLainnya + x.PotensiPajakPerTahunFitnes);
                            }
                            if (kategori != 64)
                            {
                                var potensi = new DataPotensi
                                {
                                    NOP = item.Nop,
                                    NamaOP = item.NamaOp,
                                    Alamat = item.AlamatOp,
                                    JenisPajak = jenisPajak.GetDescription(),
                                    Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                    KategoriId = Convert.ToInt32(item.KategoriId),
                                    EnumPajak = (int)jenisPajak,
                                    Target1 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                    Realisasi1 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                    Target2 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                    Realisasi2 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                    Target3 = context.DbAkunTargetObjekHiburans.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                    Realisasi3 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                    TotalPotensi = totalPotensiHiburan
                                };
                                ret.Add(potensi);
                            }
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
                            .Where(x => (x.TahunBuku == DateTime.Now.Year && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > DateTime.Now.Year)) && x.KategoriId == kategori)
                            .ToList();

                        var listOpAbt1 = dataAbt1.Select(x => x.Nop).ToList();
                        var listOpAbt2 = dataAbt2.Select(x => x.Nop).ToList();
                        var listOpAbt3 = dataAbt3.Select(x => x.Nop).ToList();

                        foreach (var item in dataAbt3.Distinct())
                        {
                            var totalPotensiAbt = context.DbPotensiAbts.Where(x => x.Nop == item.Nop && x.TahunBuku == DateTime.Now.Year + 1).Sum(q => q.Hit1bulan * 12) ?? 0;

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                EnumPajak = (int)jenisPajak,
                                Target1 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 2 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi1 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year - 1 && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi2 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.DbAkunTargetObjekAbts.Where(x => x.TahunBuku == DateTime.Now.Year && x.Nop == item.Nop).Sum(q => q.TargetBulan) ?? 0,
                                Realisasi3 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = totalPotensiAbt
                            };
                            ret.Add(potensi);
                        }

                        break;
                    case EnumFactory.EPajak.Reklame:

                        string x = "";

                        var dataReklame1 = context.DbOpReklames
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 2 && !(string.IsNullOrEmpty(x.Nor)) && x.KategoriId == kategori)
                            .ToList();
                        var dataReklame2 = context.DbOpReklames
                            .Where(x => x.TahunBuku == DateTime.Now.Year - 1 && !(string.IsNullOrEmpty(x.Nor)) && x.KategoriId == kategori)
                            .ToList();
                        var dataReklame3 = context.DbOpReklames
                            .Where(x => x.TahunBuku == DateTime.Now.Year && !(string.IsNullOrEmpty(x.Nor)) && x.KategoriId == kategori)
                            .ToList();

                        var dataReklameAll = dataReklame1
                            .Concat(dataReklame2)
                            .Concat(dataReklame3)
                            .Select(x => new { Nor = x.Nor, NamaOp = x.Nama, AlamatOp = x.Alamat })
                            .Distinct()
                            .ToList();

                        var distinctNor = dataReklameAll.Select(x => x.Nor).Distinct().ToList();

                        string kat = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum";
                        var dataTarget1 = context.DbAkunTargetObjekReklames
                            .Where(x => distinctNor.Contains(x.Nor) && x.TahunBuku == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nor })
                            .Select(x => new
                            {
                                Nor = x.Key.Nor,
                                Nominal = x.Sum(q => q.Target)
                            }).ToList();

                        var dataRealisasi1 = context.DbMonReklames
                            .Where(x => distinctNor.Contains(x.Nor) && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2)
                            .GroupBy(x => new { x.Nor })
                            .Select(x => new
                            {
                                Nor = x.Key.Nor,
                                Nominal = x.Sum(q => q.NominalPokokBayar)
                            }).ToList();

                        var dataTarget2 = context.DbAkunTargetObjekReklames
                            .Where(x => distinctNor.Contains(x.Nor) && x.TahunBuku == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nor })
                            .Select(x => new
                            {
                                Nor = x.Key.Nor,
                                Nominal = x.Sum(q => q.Target)
                            }).ToList();

                        var dataRealisasi2 = context.DbMonReklames
                            .Where(x => distinctNor.Contains(x.Nor) && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1)
                            .GroupBy(x => new { x.Nor })
                            .Select(x => new
                            {
                                Nor = x.Key.Nor,
                                Nominal = x.Sum(q => q.NominalPokokBayar)
                            }).ToList();

                        var dataTarget3 = context.DbAkunTargetObjekReklames
                            .Where(x => distinctNor.Contains(x.Nor) && x.TahunBuku == DateTime.Now.Year)
                            .GroupBy(x => new { x.Nor })
                            .Select(x => new
                            {
                                Nor = x.Key.Nor,
                                Nominal = x.Sum(q => q.Target)
                            }).ToList();

                        var dataRealisasi3 = context.DbMonReklames
                            .Where(x => distinctNor.Contains(x.Nor) && x.TglBayarPokok.Value.Year == DateTime.Now.Year)
                            .GroupBy(x => new { x.Nor })
                            .Select(x => new
                            {
                                Nor = x.Key.Nor,
                                Nominal = x.Sum(q => q.NominalPokokBayar)
                            }).ToList();

                        var dataPotensi = context.DbPotensiReklames
                            .Where(x => distinctNor.Contains(x.Nor) && x.TahunBuku == DateTime.Now.Year + 1)
                            .GroupBy(x => new { x.Nor })
                            .Select(x => new
                            {
                                Nor = x.Key.Nor,
                                Nominal = x.Sum(q => q.Hit1bulan)
                            }).ToList();

                        var dictTarget1 = dataTarget1.ToDictionary(x => x.Nor, x => x.Nominal);
                        var dictRealisasi1 = dataRealisasi1.ToDictionary(x => x.Nor, x => x.Nominal);
                        var dictTarget2 = dataTarget2.ToDictionary(x => x.Nor, x => x.Nominal);
                        var dictRealisasi2 = dataRealisasi2.ToDictionary(x => x.Nor, x => x.Nominal);
                        var dictTarget3 = dataTarget3.ToDictionary(x => x.Nor, x => x.Nominal);
                        var dictRealisasi3 = dataRealisasi3.ToDictionary(x => x.Nor, x => x.Nominal);
                        var dictPotensi = dataPotensi.ToDictionary(x => x.Nor, x => x.Nominal);


                        int total = dataReklameAll.Count;
                        int current = 0;
                        foreach (var item in dataReklameAll)
                        {
                            current++;
                            int percent = (int)((double)current / total * 100);
                            Console.Write($"\rMemproses data... {percent}%");

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nor ?? "-",
                                NamaOP = item.NamaOp ?? "-",
                                Alamat = item.AlamatOp ?? "-",
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                Kategori = kat,
                                Target1 = dictTarget1.TryGetValue(item.Nor, out var t1) ? t1 ?? 0 : 0,
                                Realisasi1 = dictRealisasi1.TryGetValue(item.Nor, out var r1) ? r1 ?? 0 : 0,
                                Target2 = dictTarget2.TryGetValue(item.Nor, out var t2) ? t2 ?? 0 : 0,
                                Realisasi2 = dictRealisasi2.TryGetValue(item.Nor, out var r2) ? r2 ?? 0 : 0,
                                Target3 = dictTarget3.TryGetValue(item.Nor, out var t3) ? t3 ?? 0 : 0,
                                Realisasi3 = dictRealisasi3.TryGetValue(item.Nor, out var r3) ? r3 ?? 0 : 0,
                                TotalPotensi = dictPotensi.TryGetValue(item.Nor, out var p) ? p ?? 0 : 0,
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
            //DETAIL POTENSI PAJAK
            public static DetailPotensiPajakHotel? GetDataPotensiHotel(string nop)
            {
                var ret = new DetailPotensiPajakHotel();
                var context = DBClass.GetContext();

                var hotel = context.DbOpHotels
                    .FirstOrDefault(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue);
                ret = context.DbPotensiHotels
                    .Where(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year + 1)
                    .Select(x => new DetailPotensiPajakHotel
                    {
                        NOP = x.Nop,
                        Nama = hotel != null ? hotel.NamaOp ?? "-" : "-",
                        Alamat = hotel != null ? hotel.AlamatOp ?? "-" : "-",
                        Wilayah = hotel != null ? "SURABAYA " + hotel.WilayahPajak ?? "-" : "-",
                        Kategori = hotel != null
                            ? (context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(hotel.KategoriId)).Nama ?? "Umum")
                            : "Umum",
                        TglOpBuka = hotel != null ? hotel.TglMulaiBukaOp : DateTime.MinValue,
                        JumlahTotalRoom = x.TotalRoom ?? 0,
                        HargaRataRataRoom = x.AvgRoomPrice ?? 0,
                        HargaRataRataRoomKos = x.AvgRoomPriceKos ?? 0,
                        OkupansiRateRoom = x.OkupansiRateRoom ?? 0,
                        KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet ?? 0,
                        HargaRataRataBanquetPerPax = x.AvgBanquetPrice ?? 0,
                        TarifPajak = 0.1m
                    })
                    .FirstOrDefault();
                return ret;
            }
            public static DetailPotensiPajakResto? GetDataPotensiResto(string nop)
            {
                var ret = new DetailPotensiPajakResto();
                var context = DBClass.GetContext();
                var resto = context.DbOpRestos
                    .FirstOrDefault(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue);
                ret = context.DbPotensiRestos
                    .Where(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year + 1)
                    .Select(x => new DetailPotensiPajakResto
                    {
                        NOP = x.Nop,
                        Nama = resto != null ? resto.NamaOp ?? "-" : "-",
                        Alamat = resto != null ? resto.AlamatOp ?? "-" : "-",
                        Wilayah = resto != null ? "SURABAYA " + resto.WilayahPajak ?? "-" : "-",
                        Kategori = resto != null
                            ? (context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(resto.KategoriId)).Nama ?? "Umum")
                            : "Umum",
                        TglOpBuka = resto != null ? resto.TglMulaiBukaOp : DateTime.MinValue,
                        JumlahKursi = x.KapKursi ?? 0,
                        KapasitasTenantCatering = x.KapTenantCatering ?? 0,
                        RataRataBillPerOrang = x.AvgBillOrg ?? 0,
                        TurnoverWeekdaysCatering = x.TurnoverWd ?? 0,
                        TurnoverWeekendCatering = x.TurnoverWe ?? 0,
                        TurnoverWeekdaysNonCatering = x.TurnoverWd ?? 0,
                        TurnoverWeekendNonCatering = x.TurnoverWe ?? 0,
                        TarifPajak = 0.1m
                    })
                    .FirstOrDefault();
                return ret;
            }
            public static DetailPotensiPajakParkir? GetDataPotensiParkir(string nop)
            {
                var ret = new DetailPotensiPajakParkir();
                var context = DBClass.GetContext();
                var parkir = context.DbOpParkirs
                    .FirstOrDefault(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue);
                ret = context.DbPotensiParkirs
                    .Where(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year + 1)
                    .Select(x => new DetailPotensiPajakParkir
                    {
                        NOP = x.Nop,
                        Nama = parkir != null ? parkir.NamaOp ?? "-" : "-",
                        Alamat = parkir != null ? parkir.AlamatOp ?? "-" : "-",
                        Wilayah = "SURABAYA " + (parkir != null ? parkir.WilayahPajak ?? "-" : "-"),
                        Kategori = parkir != null
                            ? (context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(parkir.KategoriId)).Nama ?? "Umum")
                            : "Umum",
                        Memungut = ((EnumFactory.EPungutTarifParkir)(x.JenisTarif ?? 0)).GetDescription(),
                        SistemParkir = ((EnumFactory.EPalangParkir)(x.SistemParkir ?? 0)).GetDescription(),
                        TglOpBuka = parkir != null ? parkir.TglMulaiBukaOp : DateTime.MinValue,
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
                    })
                    .FirstOrDefault();

                return ret;
            }
            public static DetailPotensiPajakHiburan? GetDataPotensiHiburan(string nop)
            {
                var ret = new DetailPotensiPajakHiburan();
                var context = DBClass.GetContext();
                var hiburan = context.DbOpHiburans
                    .FirstOrDefault(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue);
                ret = context.DbPotensiHiburans
                    .Where(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year + 1)
                    .Select(x => new DetailPotensiPajakHiburan
                    {
                        NOP = x.Nop,
                        Nama = hiburan != null ? hiburan.NamaOp ?? "-" : "-",
                        Alamat = hiburan != null ? hiburan.AlamatOp ?? "-" : "-",
                        Wilayah = hiburan != null ? "SURABAYA " + hiburan.WilayahPajak ?? "-" : "-",
                        Kategori = hiburan != null
                            ? (context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(hiburan.KategoriId)).Nama ?? "Umum")
                            : "Umum",
                        TglOpBuka = hiburan != null ? hiburan.TglMulaiBukaOp : DateTime.MinValue,
                        KategoriId = (int)hiburan.KategoriId,
                        KapasitasStudio = x.KapKursiStudio ?? 0,
                        JumlahStudio = x.JumlahStudio ?? 0,
                        Kapasitas = x.KapPengunjung ?? 0,
                        HargaMemberFitness = x.HargaMemberBulan ?? 0,
                        HTMWeekdays = x.HtmWd ?? 0,
                        HTMWeekend = x.HtmWe ?? 0,
                        TurnoverWeekdays = x.ToWd ?? 0,
                        TurnoverWeekend = x.ToWe ?? 0,
                        TarifPajak = hiburan.KategoriId == 44 ? 0.5m : hiburan.KategoriId == 41 ? 0.5m : hiburan.KategoriId == 48 ? 0.5m : hiburan.KategoriId == 45 ? 0.4m : 0.1m
                    })
                    .FirstOrDefault();
                return ret;
            }
            public static DetailPotensiPajakABT? GetDataPotensiABT(string nop)
            {
                var context = DBClass.GetContext();
                var tahun = DateTime.Now.Year;

                var abt = context.DbOpAbts
                    .FirstOrDefault(x => x.Nop == nop && x.TahunBuku == tahun && !x.TglOpTutup.HasValue);

                if (abt == null)
                    return null;

                // Ambil volume existing
                var data = context.DbPotensiAbts.Where(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year + 1).FirstOrDefault();
                if (data == null)
                {
                    return null;
                }
                var kelompok = context.MAbtKelompoks
                    .Include(x => x.MAbtKelompokHda)
                    .FirstOrDefault(x => x.Id == data.Kelompok);

                var hdaList = kelompok?.MAbtKelompokHda.ToList() ?? new List<MAbtKelompokHdum>();

                var detail = new DetailPotensiPajakABT
                {
                    NOP = abt.Nop ?? "-",
                    Nama = abt.NamaOp ?? "-",
                    Alamat = abt.AlamatOp ?? "-",
                    Wilayah = abt.WilayahPajak != null ? "SURABAYA " + abt.WilayahPajak : "-",
                    Pajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    Kategori = context.MKategoriPajaks.FirstOrDefault(k => k.Id == Convert.ToInt32(abt.KategoriId))?.Nama ?? "Umum",
                    KelompokId = kelompok?.Id ?? 0,
                    KelompokNama = kelompok?.Nama ?? "-",
                    VolumeRata = (decimal)data.VolPenggunaan,
                    TarifPajakPersen = 20m,
                    DetailPerhitungan = HitungHDAmanual((decimal)data.VolPenggunaan, hdaList)
                };

                detail.TotalNPA = detail.DetailPerhitungan.Sum(x => x.NPA);
                detail.PajakAir = data.Hit1bulan ?? 0;

                return detail;
            }
            public static List<DetailPerhitunganABT> HitungHDAmanual(decimal volume, List<MAbtKelompokHdum> hdaList)
            {
                var result = new List<DetailPerhitunganABT>();
                var sisa = volume;

                // Urutkan berdasarkan batas minimum pemakaian
                var sorted = hdaList.OrderBy(x => x.PemakaianBatasMinim).ToList();

                foreach (var item in sorted)
                {
                    if (sisa <= 0) break;

                    var batasMin = item.PemakaianBatasMinim;
                    var batasMaks = item.PemakaianBatasMaks;

                    // Volume yg bisa diambil di range ini
                    var volumeRange = Math.Min(sisa, batasMaks - batasMin + 1);

                    result.Add(new DetailPerhitunganABT
                    {
                        BatasMinim = batasMin,
                        BatasMaks = batasMaks,
                        Volume = volumeRange,
                        HDA = item.Harga
                    });

                    sisa -= volumeRange;
                }

                return result;
            }

            public static DetailPotensiPPJ? GetDataPotensiPPJ(string nop)
            {
                var context = DBClass.GetContext();

                // Cek objek PJU aktif
                var pju = context.DbOpListriks
                    .FirstOrDefault(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue);

                // Ambil data potensi pajak terakhir untuk NOP
                var data = context.DbPotensiPpjs
                    .Where(x => x.Nop == nop && x.TahunBuku == DateTime.Now.Year + 1)
                    .FirstOrDefault();

                if (data == null)
                    return null;

                // Buat list detail NJTL bulanan dari 6 bulan terakhir
                var detail = new List<DetailNJTL>();
                var now = DateTime.Now;

                detail.Add(new DetailNJTL { Bulan = now.AddMonths(-1).ToString("MMMM yyyy"), NJTL = data.NjtlBulanMinus1 ?? 0 });
                detail.Add(new DetailNJTL { Bulan = now.AddMonths(-2).ToString("MMMM yyyy"), NJTL = data.NjtlBulanMinus2 ?? 0 });
                detail.Add(new DetailNJTL { Bulan = now.AddMonths(-3).ToString("MMMM yyyy"), NJTL = data.NjtlBulanMinus3 ?? 0 });
                detail.Add(new DetailNJTL { Bulan = now.AddMonths(-4).ToString("MMMM yyyy"), NJTL = data.NjtlBulanMinus4 ?? 0 });
                detail.Add(new DetailNJTL { Bulan = now.AddMonths(-5).ToString("MMMM yyyy"), NJTL = data.NjtlBulanMinus5 ?? 0 });
                detail.Add(new DetailNJTL { Bulan = now.AddMonths(-6).ToString("MMMM yyyy"), NJTL = data.NjtlBulanMinus6 ?? 0 });

                var ret = new DetailPotensiPPJ
                {
                    NOP = nop,
                    Status = pju != null ? "Aktif" : "Tidak Aktif",
                    PeriodeData = $"{now.AddMonths(-6):MMMM} - {now.AddMonths(-1):MMMM yyyy}",
                    PeriodeTerakhir = now.AddMonths(-1).ToString("MMMM yyyy"),
                    TanggalPerhitungan = pju?.TglMulaiBukaOp ?? DateTime.Now,
                    RataRataNJTL = data.RataRataNjtl6Bulan ?? 0,
                    TarifPajak = pju == null
                        ? 0.015m
                        : (pju.Peruntukan == 58 ? 0.10m : 0.015m),
                    TotalPajak = data.Hit1bulan ?? 0,
                    DetailNJTLBulanan = detail,
                    KonsistensiNJTL = HitungKonsistensi(detail),
                };

                return ret;
            }
            private static string HitungKonsistensi(List<DetailNJTL> data)
            {
                if (data.Count < 2)
                    return "Tidak Cukup Data";

                var selisih = data.Max(x => x.NJTL) - data.Min(x => x.NJTL);
                var rata = data.Average(x => x.NJTL);

                if (selisih < 0.1m * rata)
                    return "Stabil";
                else if (selisih < 0.3m * rata)
                    return "Berfluktuasi";
                else
                    return "Tidak Stabil";
            }

            public static DetailPotensiReklame? GetDataPotensiReklame(string nop)
            {
                using var context = DBClass.GetContext();

                // Ambil data dari master OP Reklame
                var op = context.DbOpReklames.FirstOrDefault(x => x.Nor == nop);
                if (op == null) return null;

                // Ambil data potensi
                var potensi = context.DbPotensiReklames.FirstOrDefault(x => x.Nor == nop && x.TahunBuku == DateTime.Now.Year + 1);
                if (potensi == null) return null;

                // Tentukan Tarif Pajak berdasarkan jenis reklame
                var tarifPajak = 0.20m;

                // Buat model view
                var model = new DetailPotensiReklame
                {
                    NomorObjekReklame = nop,
                    JenisReklame = op.FlagPermohonan ?? "-",
                    Status = op.StatusVer == 1 ? "Aktif" : "Tidak Aktif",

                    TarifPajak = tarifPajak,

                    // NSR Tahunan dari BulanMin-6 s.d BulanMin-1
                    NSRTahun0 = potensi.Nsr0 ?? 0,
                    NSRTahun1 = potensi.Nsr1 ?? 0,
                    NSRTahun2 = potensi.Nsr2 ?? 0,
                    NSRTahun3 = potensi.Nsr3 ?? 0,
                    NSRTahun4 = potensi.Nsr4 ?? 0,

                    RataRataNSR = potensi.Rata2Nsr ?? 0,
                    RataRataPajak = potensi.Rata2Pajak ?? 0,

                    KonsistensiNSR = CekKonsistensiNSR(potensi),
                };

                return model;
            }
            private static string CekKonsistensiNSR(DbPotensiReklame potensi)
            {
                var list = new List<decimal?> {
                        potensi.Nsr0, potensi.Nsr1,
                        potensi.Nsr2, potensi.Nsr3,
                        potensi.Nsr4
                    }.Where(x => x.HasValue).Select(x => x.Value).ToList();

                if (!list.Any()) return "Data Kosong";

                var max = list.Max();
                var min = list.Min();

                var selisih = max - min;
                var stabil = selisih < 0.2m * max;

                return stabil ? "Stabil" : "Fluktuatif";
            }
        }

        public class Dashboard
        {
            public decimal Potensi { get; set; }
            public decimal RealisasiTotal { get; set; }
            public decimal Capaian { get; set; }
            public int TotalOP { get; set; }
            public decimal RealisasiOP { get; set; }
            public string CapaianOP { get; set; } = "";
        }

        public class DataPotensi
        {
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public int EnumPajak { get; set; }
            public int EnumKategori { get; set; }
            public string JenisPajak { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public int KategoriId { get; set; }
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
            public decimal TotalPotensi1 { get; set; }
            public decimal TotalPotensi2 { get; set; }
            public decimal TotalPotensi3 { get; set; }
            public decimal TotalPotensi4 { get; set; }
            public decimal TotalPotensi5 { get; set; }
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
            public decimal TotalPotensi1 { get; set; }
            public decimal TotalPotensi2 { get; set; }
            public decimal TotalPotensi3 { get; set; }
            public decimal TotalPotensi4 { get; set; }
            public decimal TotalPotensi5 { get; set; }
        }

        #region CLASS DetailPotensiFix
        public class DetailPotensiPajakHotel
        {
            // Informasi dasar
            public string NOP { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; }
            public int KategoriId { get; set; }

            // Room
            public int JumlahTotalRoom { get; set; }
            public decimal HargaRataRataRoomKos { get; set; }
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
                    var now = DateTime.Now.AddYears(1);
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
            //KOS
            public decimal PotensiOmzetRoomPerBulanKos => HargaRataRataRoomKos * Math.Ceiling(RataRataRoomTerjualPerHari);

            public decimal PotensiOmzetRoomPerBulan => HargaRataRataRoom * Math.Ceiling(RataRataRoomTerjualPerHari) * 30;

            public decimal OkupansiRateBanquet => KapasitasMaksimalPaxBanquetPerHari > 0 ? Math.Round((0.3m * OkupansiRateRoom) * 100, MidpointRounding.AwayFromZero) / 100 : 0;

            public decimal RataRataPaxBanquetTerjualPerHari => KapasitasMaksimalPaxBanquetPerHari * OkupansiRateBanquet;

            public decimal PotensiOmzetBanquetPerBulan => HargaRataRataBanquetPerPax * Math.Ceiling(RataRataPaxBanquetTerjualPerHari) * 8;

            public decimal PotensiPajakPerBulan => (PotensiOmzetRoomPerBulan + PotensiOmzetBanquetPerBulan) * TarifPajak;
            public decimal PotensiPajakPerTahun => PotensiPajakPerBulan * BulanSisa;

            //KOS
            public decimal PotensiPajakPerBulanKos => (PotensiOmzetRoomPerBulanKos) * TarifPajak;
            public decimal PotensiPajakPerTahunKos => PotensiPajakPerBulanKos * BulanSisa;
        }
        public class DetailPotensiPajakResto
        {
            // Identitas
            public string NOP { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; }
            public int KategoriId { get; set; }
            public DateTime TglOpBuka { get; set; }
            public int BulanSisa
            {
                get
                {
                    var now = DateTime.Now.AddYears(1);
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
            public decimal RataRataTerjualWeekdaysCatering => Math.Ceiling(KapasitasTenantCatering * TurnoverWeekdaysCatering);
            public decimal RataRataTerjualWeekendCatering => Math.Ceiling(KapasitasTenantCatering * TurnoverWeekendCatering);
            public decimal RataRataTerjualPerHariCatering =>
                (RataRataTerjualWeekdaysCatering * 22) + (RataRataTerjualWeekendCatering * 8);

            public decimal OmzetPerBulanCatering =>
                (RataRataBillPerOrang * Math.Ceiling(RataRataTerjualWeekdaysCatering) * 22) +
                (RataRataBillPerOrang * Math.Ceiling(RataRataTerjualWeekendCatering) * 8);

            public decimal PotensiPajakPerBulanCatering => OmzetPerBulanCatering * TarifPajak;
            public decimal PotensiPajakPerTahunCatering => PotensiPajakPerBulanCatering * BulanSisa;

            // PerhitunganNonCatering
            public decimal RataRataPengunjungWeekdaysNonCatering => Math.Ceiling(JumlahKursi * TurnoverWeekdaysNonCatering);
            public decimal RataRataPengunjungWeekendNonCatering => Math.Ceiling(JumlahKursi * TurnoverWeekendNonCatering);

            public decimal TotalPengunjungWeekdaysNonCatering => Math.Ceiling(RataRataPengunjungWeekdaysNonCatering) * 22;
            public decimal TotalPengunjungWeekendNonCatering => Math.Ceiling(RataRataPengunjungWeekendNonCatering) * 8;

            public decimal OmzetPerBulanNonCatering =>
                (RataRataBillPerOrang * Math.Ceiling(RataRataPengunjungWeekdaysNonCatering) * 22) +
                (RataRataBillPerOrang * Math.Ceiling(RataRataPengunjungWeekendNonCatering) * 8);

            public decimal PotensiPajakPerBulanNonCatering => OmzetPerBulanNonCatering * TarifPajak;
            public decimal PotensiPajakPerTahunNonCatering => PotensiPajakPerBulanNonCatering * BulanSisa;
        }
        public class DetailPotensiPajakParkir
        {
            // Identitas
            public string NOP { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; }
            public int KategoriId { get; set; }
            public string Memungut { get; set; } // "Memungut" atau "Tidak Memungut"
            public string SistemParkir { get; set; }
            public DateTime TglOpBuka { get; set; }
            public int BulanSisa
            {
                get
                {
                    var now = DateTime.Now.AddYears(1);
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
            public int TotalKapasitas =>
                (KapasitasMobil + KapasitasMotor + KapasitasSepeda + KapasitasTrailer + KapasitasTrukBus + KapasitasTrukMini);

            public decimal PersentaseKapasitasSepeda =>
                TotalKapasitas == 0 ? 0 : (decimal)KapasitasSepeda / TotalKapasitas;

            public decimal PersentaseKapasitasMotor =>
                TotalKapasitas == 0 ? 0 : (decimal)KapasitasMotor / TotalKapasitas;

            public decimal PersentaseKapasitasMobil =>
                TotalKapasitas == 0 ? 0 : (decimal)KapasitasMobil / TotalKapasitas;

            public decimal PersentaseKapasitasTrukMini =>
                TotalKapasitas == 0 ? 0 : (decimal)KapasitasTrukMini / TotalKapasitas;

            public decimal PersentaseKapasitasTrukBus =>
                TotalKapasitas == 0 ? 0 : (decimal)KapasitasTrukBus / TotalKapasitas;

            public decimal PersentaseKapasitasTrailer =>
                TotalKapasitas == 0 ? 0 : (decimal)KapasitasTrailer / TotalKapasitas;

            // Jumlah terparkir weekdays & weekend
            public decimal JumlahTerparkirSepedaWeekdays => KapasitasSepeda == 0 ? 0 : Math.Ceiling(TurnoverWeekdays * KapasitasSepeda);
            public decimal JumlahTerparkirSepedaWeekend => KapasitasSepeda == 0 ? 0 : Math.Ceiling(TurnoverWeekend * KapasitasSepeda);
            public decimal OmzetSepeda =>
                KapasitasSepeda == 0 ? 0 :
                (Math.Ceiling(JumlahTerparkirSepedaWeekdays) * TarifSepeda * 22) +
                (Math.Ceiling(JumlahTerparkirSepedaWeekend) * TarifSepeda * 8);

            public decimal JumlahTerparkirMotorWeekdays => KapasitasMotor == 0 ? 0 : Math.Ceiling(TurnoverWeekdays * KapasitasMotor);
            public decimal JumlahTerparkirMotorWeekend => KapasitasMotor == 0 ? 0 : Math.Ceiling(TurnoverWeekend * KapasitasMotor);
            public decimal OmzetMotor =>
                KapasitasMotor == 0 ? 0 :
                (Math.Ceiling(JumlahTerparkirMotorWeekdays) * TarifMotor * 22) +
                (Math.Ceiling(JumlahTerparkirMotorWeekend) * TarifMotor * 8);

            public decimal JumlahTerparkirMobilWeekdays => KapasitasMobil == 0 ? 0 : Math.Ceiling(TurnoverWeekdays * KapasitasMobil);
            public decimal JumlahTerparkirMobilWeekend => KapasitasMobil == 0 ? 0 : Math.Ceiling(TurnoverWeekend * KapasitasMobil);
            public decimal OmzetMobil =>
                KapasitasMobil == 0 ? 0 :
                (Math.Ceiling(JumlahTerparkirMobilWeekdays) * TarifMobil * 22) +
                (Math.Ceiling(JumlahTerparkirMobilWeekend) * TarifMobil * 8);

            public decimal JumlahTerparkirTrukMiniWeekdays => KapasitasTrukMini == 0 ? 0 : Math.Ceiling(TurnoverWeekdays * KapasitasTrukMini);
            public decimal JumlahTerparkirTrukMiniWeekend => KapasitasTrukMini == 0 ? 0 : Math.Ceiling(TurnoverWeekend * KapasitasTrukMini);
            public decimal OmzetTrukMini =>
                KapasitasTrukMini == 0 ? 0 :
                (Math.Ceiling(JumlahTerparkirTrukMiniWeekdays) * TarifTrukMini * 22) +
                (Math.Ceiling(JumlahTerparkirTrukMiniWeekend) * TarifTrukMini * 8);

            public decimal JumlahTerparkirTrukBusWeekdays => KapasitasTrukBus == 0 ? 0 : Math.Ceiling(TurnoverWeekdays * KapasitasTrukBus);
            public decimal JumlahTerparkirTrukBusWeekend => KapasitasTrukBus == 0 ? 0 : Math.Ceiling(TurnoverWeekend * KapasitasTrukBus);
            public decimal OmzetTrukBus =>
                KapasitasTrukBus == 0 ? 0 :
                (Math.Ceiling(JumlahTerparkirTrukBusWeekdays) * TarifTrukBus * 22) +
                (Math.Ceiling(JumlahTerparkirTrukBusWeekend) * TarifTrukBus * 8);

            public decimal JumlahTerparkirTrailerWeekdays => KapasitasTrailer == 0 ? 0 : Math.Ceiling(TurnoverWeekdays * KapasitasTrailer);
            public decimal JumlahTerparkirTrailerWeekend => KapasitasTrailer == 0 ? 0 : Math.Ceiling(TurnoverWeekend * KapasitasTrailer);
            public decimal OmzetTrailer =>
                KapasitasTrailer == 0 ? 0 :
                (Math.Ceiling(JumlahTerparkirTrailerWeekdays) * TarifTrailer * 22) +
                (Math.Ceiling(JumlahTerparkirTrailerWeekend) * TarifTrailer * 8);


            // Total Omzet dan Pajak
            public decimal TotalOmzet =>
                OmzetSepeda + OmzetMotor + OmzetMobil + OmzetTrukMini + OmzetTrukBus + OmzetTrailer;

            public decimal PotensiPajakPerBulan => TotalOmzet * TarifPajak;

            public decimal PotensiPajakPerTahun => PotensiPajakPerBulan * BulanSisa;

            public decimal TotalTerparkirWeekdays =>
                JumlahTerparkirMobilWeekdays + JumlahTerparkirMotorWeekdays + JumlahTerparkirSepedaWeekdays + JumlahTerparkirTrailerWeekdays + JumlahTerparkirTrukBusWeekdays + JumlahTerparkirTrukMiniWeekdays;
            public decimal PersentaseMotorWeekdays =>
                JumlahTerparkirMotorWeekdays / TotalKapasitas;
            public decimal PersentaseMobilWeekdays =>
                JumlahTerparkirMobilWeekdays / TotalKapasitas;
            public decimal PersentaseSepedaWeekdays =>
                JumlahTerparkirSepedaWeekdays / TotalKapasitas;
            public decimal PersentaseTrailerWeekdays =>
                JumlahTerparkirTrailerWeekdays / TotalKapasitas;
            public decimal PersentaseTrukMiniWeekdays =>
                JumlahTerparkirTrukMiniWeekdays / TotalKapasitas;
            public decimal PersentaseTrukBusWeekdays =>
                JumlahTerparkirTrukBusWeekdays / TotalKapasitas;

            public decimal TotalTerparkirWeekend =>
                JumlahTerparkirMobilWeekend + JumlahTerparkirMotorWeekend + JumlahTerparkirSepedaWeekend + JumlahTerparkirTrailerWeekend + JumlahTerparkirTrukBusWeekend + JumlahTerparkirTrukMiniWeekend;

            public decimal PersentaseMotorWeekend =>
                TotalTerparkirWeekend == 0 ? 0 : JumlahTerparkirMotorWeekend / TotalTerparkirWeekend;

            public decimal PersentaseMobilWeekend =>
                TotalTerparkirWeekend == 0 ? 0 : JumlahTerparkirMobilWeekend / TotalTerparkirWeekend;

            public decimal PersentaseSepedaWeekend =>
                TotalTerparkirWeekend == 0 ? 0 : JumlahTerparkirSepedaWeekend / TotalTerparkirWeekend;

            public decimal PersentaseTrailerWeekend =>
                TotalTerparkirWeekend == 0 ? 0 : JumlahTerparkirTrailerWeekend / TotalTerparkirWeekend;

            public decimal PersentaseTrukMiniWeekend =>
                TotalTerparkirWeekend == 0 ? 0 : JumlahTerparkirTrukMiniWeekend / TotalTerparkirWeekend;

            public decimal PersentaseTrukBusWeekend =>
                TotalTerparkirWeekend == 0 ? 0 : JumlahTerparkirTrukBusWeekend / TotalTerparkirWeekend;

        }
        public class DetailPotensiPajakHiburan
        {
            // Identitas
            public string NOP { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }
            public string Kategori { get; set; }
            public int KategoriId { get; set; }
            public DateTime TglOpBuka { get; set; }
            public int BulanSisa
            {
                get
                {
                    var now = DateTime.Now.AddYears(1);
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
            public decimal JumlahPengunjungWeekdaysLainnya => Math.Ceiling(Kapasitas * TurnoverWeekdays);
            public decimal JumlahPengunjungWeekendLainnya => Math.Ceiling(Kapasitas * TurnoverWeekend);
            public decimal JumlahPengunjungPerBulanLainnya =>
                (JumlahPengunjungWeekdaysLainnya * 22) + (JumlahPengunjungWeekendLainnya * 8);
            public decimal RataRataPengunjungLainnya =>
                ((JumlahPengunjungWeekdaysLainnya * 22) + (JumlahPengunjungWeekendLainnya * 8)) / 12;
            public decimal OmzetPerBulanLainnya =>
                (HTMWeekdays * Math.Ceiling(JumlahPengunjungWeekdaysLainnya) * 22) +
                (HTMWeekend * Math.Ceiling(JumlahPengunjungWeekendLainnya) * 8);
            public decimal OmzetWeekdaysLainnya => HTMWeekdays * JumlahPengunjungWeekdaysLainnya * 22;
            public decimal OmzetWeekendLainnya => HTMWeekend * JumlahPengunjungWeekendLainnya * 8;
            public decimal PotensiPajakPerBulanLainnya => OmzetPerBulanLainnya * TarifPajak;
            public decimal PotensiPajakPerTahunLainnya => PotensiPajakPerBulanLainnya * BulanSisa;

            // ========== Perhitungan Kategori Lainnya Bar ==========
            public decimal JumlahPengunjungWeekdaysLainnyaBar => Math.Ceiling(Kapasitas * TurnoverWeekdays);
            public decimal JumlahPengunjungWeekendLainnyaBar => Math.Ceiling(Kapasitas * TurnoverWeekend);
            public decimal JumlahPengunjungPerBulanLainnyaBar =>
                (JumlahPengunjungWeekdaysLainnyaBar * 22) + (JumlahPengunjungWeekendLainnyaBar * 8);
            public decimal RataRataPengunjungLainnyaBar =>
                ((JumlahPengunjungWeekdaysLainnyaBar * 22) + (JumlahPengunjungWeekendLainnyaBar * 8)) / 12;
            public decimal OmzetPerBulanLainnyaBar =>
                (HTMWeekdays * Math.Ceiling(JumlahPengunjungWeekdaysLainnyaBar) * 22) +
                (HTMWeekend * Math.Ceiling(JumlahPengunjungWeekendLainnyaBar) * 8);
            public decimal OmzetWeekdaysLainnyaBar => HTMWeekdays * JumlahPengunjungWeekdaysLainnyaBar * 22;
            public decimal OmzetWeekendLainnyaBar => HTMWeekend * JumlahPengunjungWeekendLainnyaBar * 8;
            public decimal PotensiPajakPerBulanLainnyaBar => OmzetPerBulanLainnyaBar * TarifPajak * 0.1m;
            public decimal PotensiPajakPerTahunLainnyaBar => PotensiPajakPerBulanLainnyaBar * BulanSisa;

            // ========== Perhitungan Kategori Bioskop ==========
            public int KapasitasBioskop => JumlahStudio * KapasitasStudio * 4;
            public decimal JumlahPengunjungWeekdaysBioskop => Math.Ceiling(KapasitasBioskop * TurnoverWeekdays);
            public decimal JumlahPengunjungWeekendBioskop => Math.Ceiling(KapasitasBioskop * TurnoverWeekend);
            public decimal OmzetPerBulanBioskop =>
                (HTMWeekdays * Math.Ceiling(JumlahPengunjungWeekdaysBioskop) * 22) +
                (HTMWeekend * Math.Ceiling(JumlahPengunjungWeekendBioskop) * 8);
            public decimal PotensiPajakPerBulanBioskop => OmzetPerBulanBioskop * TarifPajak;
            public decimal PotensiPajakPerTahunBioskop => PotensiPajakPerBulanBioskop * BulanSisa;

            // ========== Perhitungan Kategori Fitness/Pusat Kebugaran ==========
            public decimal EstimasiJumlahMemberFitnes =>
                Math.Ceiling(((Math.Ceiling(Kapasitas * TurnoverWeekdays) * 22) + (Math.Ceiling(Kapasitas * TurnoverWeekend) * 8)) / 12);
            public decimal OmzetPerBulanFitnes => HargaMemberFitness * Math.Ceiling(EstimasiJumlahMemberFitnes);
            public decimal PotensiPajakPerBulanFitnes => OmzetPerBulanFitnes * TarifPajak;
            public decimal PotensiPajakPerTahunFitnes => PotensiPajakPerBulanFitnes * BulanSisa;
        }

        public class DetailPotensiPajakABT
        {
            public string NOP { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string NPWPD { get; set; }
            public string Nama { get; set; }
            public string Alamat { get; set; }
            public string Wilayah { get; set; }

            public string Pajak { get; set; }
            public string Kategori { get; set; }

            public int KelompokId { get; set; }
            public string KelompokNama { get; set; }

            public decimal VolumeRata { get; set; }
            public decimal TotalNPA { get; set; }
            public decimal TarifPajakPersen { get; set; } = 10;
            public decimal PajakAir { get; set; }

            public List<DetailPerhitunganABT> DetailPerhitungan { get; set; } = new();
        }
        public class DetailPerhitunganABT
        {
            public decimal BatasMinim { get; set; }
            public decimal BatasMaks { get; set; }
            public decimal Volume { get; set; }
            public decimal HDA { get; set; }
            public decimal NPA => Volume * HDA;
        }

        public class DetailPotensiPPJ
        {
            public string NOP { get; set; } = string.Empty;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string Status { get; set; } = "Aktif";
            public string PeriodeData { get; set; } = string.Empty; // Contoh: "Januari - Juni 2023"
            public string PeriodeTerakhir { get; set; } = string.Empty; // Contoh: "Juli 2023"
            public DateTime TanggalPerhitungan { get; set; }

            public decimal RataRataNJTL { get; set; }
            public decimal TarifPajak { get; set; } // dalam persen, contoh: 1.5
            public decimal TotalPajak { get; set; }

            public List<DetailNJTL> DetailNJTLBulanan { get; set; } = new();
            public string KonsistensiNJTL { get; set; } = "Stabil";

            public string Catatan { get; set; } = string.Empty;
        }

        public class DetailNJTL
        {
            public string Bulan { get; set; } = string.Empty; // Contoh: "Bulan -1"
            public decimal NJTL { get; set; }
        }

        public class DetailPotensiReklame
        {

            public string NomorObjekReklame { get; set; } = string.Empty;
            //public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string JenisReklame { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public DateTime TanggalPerhitungan { get; set; }

            public decimal TarifPajak { get; set; }

            public decimal NSRTahun0 { get; set; }
            public decimal NSRTahun1 { get; set; }
            public decimal NSRTahun2 { get; set; }
            public decimal NSRTahun3 { get; set; }
            public decimal NSRTahun4 { get; set; }

            public decimal RataRataNSR { get; set; }
            public decimal RataRataPajak { get; set; }

            public string KonsistensiNSR { get; set; } = string.Empty;

            public string Catatan { get; set; }

            public string RumusPerhitungan { get; set; }
        }

        #endregion



    }
}
