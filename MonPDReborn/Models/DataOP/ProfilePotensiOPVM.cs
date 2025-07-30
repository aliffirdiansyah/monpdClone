using MonPDLib;
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

                //var targetReklameNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                //var realisasiReklameNow = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetAbtNow = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                //var realisasiAbtNow = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear).Sum(x => x.NominalPokokBayar) ?? 0;

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

                //var targetReklameMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                //var realisasiReklameMines1 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetAbtMines1 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 1 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                //var realisasiAbtMines1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 1).Sum(x => x.NominalPokokBayar) ?? 0;

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

                //var targetReklameMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                //var realisasiReklameMines2 = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetAbtMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                //var realisasiAbtMines2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear - 2).Sum(x => x.NominalPokokBayar) ?? 0;

                //var targetOpsenPkbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                //var realisasiOpsenPkbMines2 = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);

                //var targetOpsenBbnkbMines2 = context.DbAkunTargets.Where(x => x.TahunBuku == currentYear - 2 && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);
                //var realisasiOpsenBbnkbMines2 = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear - 2).Sum(x => x.JmlPokok);
                #endregion

                #region Potensi
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
                    .Where(x => x.TahunBuku == DateTime.Now.Year)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                var dataRestoAll = dataResto1
                    .Concat(dataResto2)
                    .Concat(dataResto3)
                    .Select(x => (x.Nop, x.KategoriId))
                    .Distinct()
                    .ToList();
                var listOpRestoAll = dataRestoAll.Select(x => x.Nop).Distinct().ToList();
                var potensiResto = new object();

                var dataPpj1 = context.DbOpListriks
                    .Where(x => x.TahunBuku == DateTime.Now.Year - 2)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();
                var dataPpj2 = context.DbOpListriks
                    .Where(x => x.TahunBuku == DateTime.Now.Year - 1)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();
                var dataPpj3 = context.DbOpListriks
                    .Where(x => x.TahunBuku == DateTime.Now.Year)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                var dataPpjAll = dataPpj1
                    .Concat(dataPpj2)
                    .Concat(dataPpj3)
                    .Select(x => (x.Nop, x.KategoriId))
                    .Distinct()
                    .ToList();
                var listOpPpjAll = dataPpjAll.Select(x => x.Nop).Distinct().ToList();
                var totalPotensiPpj = context.PotensiCtrlPpjs.Where(x => listOpPpjAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun) ?? 0;

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
                    .Where(x => (x.TahunBuku == DateTime.Now.Year && !x.TglOpTutup.HasValue) || (x.TglOpTutup.HasValue && x.TglOpTutup.Value < DateTime.Now))
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                var dataHotelAll = dataHotel1
                    .Concat(dataHotel2)
                    .Concat(dataHotel3)
                    .Select(x => (x.Nop, x.KategoriId))
                    .Distinct()
                    .ToList();
                var listOpHotelAll = dataHotelAll.Select(x => x.Nop).Distinct().ToList();
                var potensiHotel = context.DbPotensiHotels.Where(x => dataHotel3.Select(v => v.Nop).ToList().Contains(x.Nop))
                    .Select(x => new DetailPotensiPajakHotel()
                    {
                        NOP = x.Nop,
                        Nama = "",
                        Alamat = "",
                        Wilayah = "",
                        Kategori = "",
                        JumlahTotalRoom = x.TotalRoom.Value,
                        HargaRataRataRoom = x.AvgRoomPrice.Value,
                        OkupansiRateRoom = x.OkupansiRateRoom.Value,
                        KapasitasMaksimalPaxBanquetPerHari = x.MaxPaxBanquet.Value,
                        HargaRataRataBanquetPerPax = x.AvgBanquetPrice.Value,
                        TarifPajak = 0.1m
                    })
                    .ToList();
                var totalPotensiHotel = potensiHotel.Sum(x => x.PotensiPajakPerTahun);

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
                    .Where(x => x.TahunBuku == DateTime.Now.Year)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                var dataParkirAll = dataParkir1
                    .Concat(dataParkir2)
                    .Concat(dataParkir3)
                    .Select(x => (x.Nop, x.KategoriId))
                    .Distinct()
                    .ToList();
                var listOpParkirAll = dataParkirAll.Select(x => x.Nop).Distinct().ToList();
                var totalPotensiParkir = context.PotensiCtrlParkirs.Where(x => listOpParkirAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun);

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
                    .Where(x => x.TahunBuku == DateTime.Now.Year)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                var dataHiburanAll = dataHiburan1
                    .Concat(dataHiburan2)
                    .Concat(dataHiburan3)
                    .Select(x => (x.Nop, x.KategoriId))
                    .Distinct()
                    .ToList();
                var listOpHiburanAll = dataHiburanAll.Select(x => x.Nop).Distinct().ToList();
                var totalPotensiHiburan = context.PotensiCtrlHiburans.Where(x => listOpHiburanAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun);

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
                    .Where(x => x.TahunBuku == DateTime.Now.Year)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                    .ToList();

                var dataAbtAll = dataAbt1
                    .Concat(dataAbt2)
                    .Concat(dataAbt3)
                    .Select(x => (x.Nop, x.KategoriId))
                    .Distinct()
                    .ToList();
                var listOpAbtAll = dataAbtAll.Select(x => x.Nop).Distinct().ToList();
                //var totalPotensiAbt = context.PotensiCtrlAirTanahs.Where(x => listOpAbtAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun) ?? 0;

                var totalPotensiReklame = context.PotensiCtrlReklames.Sum(q => q.PotensiPajakTahun) ?? 0;

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
                    //TotalPotensi = totalPotensiResto,
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

                //ret.Add(new RekapPotensi
                //{
                //    EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                //    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                //    Target3 = targetAbtNow,
                //    Realisasi3 = realisasiAbtNow,
                //    Target2 = targetAbtMines1,
                //    Realisasi2 = realisasiAbtMines1,
                //    Target1 = targetAbtMines2,
                //    Realisasi1 = realisasiAbtMines2,
                //    TotalPotensi = totalPotensiAbt
                //});

                //ret.Add(new RekapPotensi
                //{
                //    EnumPajak = (int)EnumFactory.EPajak.Reklame,
                //    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                //    Target3 = targetReklameNow,
                //    Realisasi3 = realisasiReklameNow,
                //    Target2 = targetReklameMines1,
                //    Realisasi2 = realisasiReklameMines1,
                //    Target1 = targetReklameMines2,
                //    Realisasi1 = realisasiReklameMines2,
                //    TotalPotensi = totalPotensiReklame
                //});

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
                            .Where(x => x.TahunBuku == DateTime.Now.Year)
                            .GroupBy(x => new { x.Nop, x.KategoriId })
                            .Select(x => new { x.Key.Nop, x.Key.KategoriId })
                            .ToList();

                        var dataRestoAll = dataResto1
                            .Concat(dataResto2)
                            .Concat(dataResto3)
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

                            var listOpResto1 = dataResto1.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpResto2 = dataResto2.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpResto3 = dataResto3.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();
                            var listOpRestoAll = dataRestoAll.Where(x => x.KategoriId == item.Id).Select(x => x.Nop).ToList();

                            var targetResto1 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 2 && listOpResto1.Contains(x.Nop)).Sum(q => q.Target);
                            var targetResto2 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 1 && listOpResto2.Contains(x.Nop)).Sum(q => q.Target);
                            var targetResto3 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year && listOpResto3.Contains(x.Nop)).Sum(q => q.Target);
                            var realisasiResto1 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpResto1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiResto2 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpResto2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiResto3 = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpResto3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var totalPotensiResto = context.PotensiCtrlRestorans.Where(x => listOpRestoAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun);

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
                            .Where(x => x.TahunBuku == DateTime.Now.Year)
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

                            var targetListrik1 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 2 && listOpListrik1.Contains(x.Nop)).Sum(q => q.Target);
                            var targetListrik2 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 1 && listOpListrik2.Contains(x.Nop)).Sum(q => q.Target);
                            var targetListrik3 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year && listOpListrik3.Contains(x.Nop)).Sum(q => q.Target);
                            var realisasiListrik1 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpListrik1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiListrik2 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpListrik2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiListrik3 = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpListrik3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var totalPotensiListrik = context.PotensiCtrlPpjs.Where(x => listOpListrikAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun) ?? 0;

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
                            .Where(x => x.TahunBuku == DateTime.Now.Year)
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

                            var targetHotel1 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 2 && listOpHotel1.Contains(x.Nop)).Sum(q => q.Target);
                            var targetHotel2 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 1 && listOpHotel2.Contains(x.Nop)).Sum(q => q.Target);
                            var targetHotel3 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year && listOpHotel3.Contains(x.Nop)).Sum(q => q.Target);
                            var realisasiHotel1 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHotel1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHotel2 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHotel2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHotel3 = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHotel3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var totalPotensiHotel = context.PotensiCtrlHotels.Where(x => listOpHotelAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun);

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
                            .Where(x => x.TahunBuku == DateTime.Now.Year)
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

                            var targetParkir1 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 2 && listOpParkir1.Contains(x.Nop)).Sum(q => q.Target);
                            var targetParkir2 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 1 && listOpParkir2.Contains(x.Nop)).Sum(q => q.Target);
                            var targetParkir3 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year && listOpParkir3.Contains(x.Nop)).Sum(q => q.Target);
                            var realisasiParkir1 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpParkir1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiParkir2 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpParkir2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiParkir3 = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpParkir3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var totalPotensiParkir = context.PotensiCtrlParkirs.Where(x => listOpParkirAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun);

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
                            .Where(x => x.TahunBuku == DateTime.Now.Year)
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

                            var targetHiburan1 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 2 && listOpHiburan1.Contains(x.Nop)).Sum(q => q.Target);
                            var targetHiburan2 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 1 && listOpHiburan2.Contains(x.Nop)).Sum(q => q.Target);
                            var targetHiburan3 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year && listOpHiburan3.Contains(x.Nop)).Sum(q => q.Target);
                            var realisasiHiburan1 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpHiburan1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHiburan2 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpHiburan2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiHiburan3 = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpHiburan3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var totalPotensiHiburan = context.PotensiCtrlHiburans.Where(x => listOpHiburanAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun);

                            re.Target1 = targetHiburan1;
                            re.Realisasi1 = realisasiHiburan1;
                            re.Target2 = targetHiburan2;
                            re.Realisasi2 = realisasiHiburan2;
                            re.Target3 = targetHiburan3;
                            re.Realisasi3 = realisasiHiburan3;
                            re.TotalPotensi = totalPotensiHiburan;


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
                            .Where(x => x.TahunBuku == DateTime.Now.Year)
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

                            var targetAbt1 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 2 && listOpAbt1.Contains(x.Nop)).Sum(q => q.Target);
                            var targetAbt2 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year - 1 && listOpAbt2.Contains(x.Nop)).Sum(q => q.Target);
                            var targetAbt3 = context.PotensiCtrlTargets.Where(x => x.KdPajak == (int)jenisPajak && x.Tahun == DateTime.Now.Year && listOpAbt3.Contains(x.Nop)).Sum(q => q.Target);
                            var realisasiAbt1 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2 && listOpAbt1.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiAbt2 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1 && listOpAbt2.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            var realisasiAbt3 = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == DateTime.Now.Year && listOpAbt3.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                            //var totalPotensiAbt = context.PotensiCtrlAirTanahs.Where(x => listOpAbtAll.Contains(x.Nop)).Sum(q => q.PotensiPajakTahun) ?? 0;

                            re.Target1 = targetAbt1;
                            re.Realisasi1 = realisasiAbt1;
                            re.Target2 = targetAbt2;
                            re.Realisasi2 = realisasiAbt2;
                            re.Target3 = targetAbt3;
                            re.Realisasi3 = realisasiAbt3;
                            //re.TotalPotensi = totalPotensiAbt;


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
                            var totalPotensiReklame = context.PotensiCtrlReklames.Sum(q => q.PotensiPajakTahun) ?? 0;

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
                            .Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori)
                            .ToList();

                        var dataRestoAll = dataResto1
                            .Concat(dataResto2)
                            .Concat(dataResto3)
                            .Select(x => new { Nop = x.Nop, NamaOp = x.NamaOp, AlamatOp = x.AlamatOp })
                            .Distinct()
                            .ToList();

                        foreach (var item in dataRestoAll.Distinct())
                        {
                            var checkPotensi = context.PotensiCtrlRestorans.Any(x => x.Nop == item.Nop);

                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 2).Sum(q => q.Target),
                                Realisasi1 = context.DbMonRestos.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 1).Sum(q => q.Target),
                                Realisasi2 = context.DbMonRestos.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year).Sum(q => q.Target),
                                Realisasi3 = context.DbMonRestos.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = context.PotensiCtrlRestorans.Where(x => x.Nop == item.Nop).Sum(q => q.PotensiPajakTahun)
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
                            .Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori)
                            .ToList();

                        var dataListrikAll = dataListrik1
                            .Concat(dataListrik2)
                            .Concat(dataListrik3)
                            .Select(x => new { Nop = x.Nop, NamaOp = x.NamaOp, AlamatOp = x.AlamatOp })
                            .Distinct()
                            .ToList();

                        foreach (var item in dataListrikAll)
                        {
                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 2).Sum(q => q.Target),
                                Realisasi1 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 1).Sum(q => q.Target),
                                Realisasi2 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year).Sum(q => q.Target),
                                Realisasi3 = context.DbMonPpjs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = context.PotensiCtrlPpjs.Where(x => x.Nop == item.Nop).Sum(q => q.PotensiPajakTahun) ?? 0
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
                            .Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori)
                            .ToList();

                        var dataHotelAll = dataHotel1
                            .Concat(dataHotel2)
                            .Concat(dataHotel3)
                            .Select(x => new { Nop = x.Nop, NamaOp = x.NamaOp, AlamatOp = x.AlamatOp })
                            .Distinct()
                            .ToList();

                        foreach (var item in dataHotelAll)
                        {
                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 2).Sum(q => q.Target),
                                Realisasi1 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 1).Sum(q => q.Target),
                                Realisasi2 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year).Sum(q => q.Target),
                                Realisasi3 = context.DbMonHotels.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = context.PotensiCtrlHotels.Where(x => x.Nop == item.Nop).Sum(q => q.PotensiPajakTahun)
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
                            .Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori)
                            .ToList();

                        var dataParkirAll = dataParkir1
                            .Concat(dataParkir2)
                            .Concat(dataParkir3)
                            .Select(x => new { Nop = x.Nop, NamaOp = x.NamaOp, AlamatOp = x.AlamatOp })
                            .Distinct()
                            .ToList();

                        foreach (var item in dataParkirAll)
                        {
                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 2).Sum(q => q.Target),
                                Realisasi1 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 1).Sum(q => q.Target),
                                Realisasi2 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year).Sum(q => q.Target),
                                Realisasi3 = context.DbMonParkirs.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = context.PotensiCtrlParkirs.Where(x => x.Nop == item.Nop).Sum(q => q.PotensiPajakTahun)
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
                            .Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori)
                            .ToList();

                        var dataHiburanAll = dataHiburan1
                            .Concat(dataHiburan2)
                            .Concat(dataHiburan3)
                            .Select(x => new { Nop = x.Nop, NamaOp = x.NamaOp, AlamatOp = x.AlamatOp })
                            .Distinct()
                            .ToList();

                        foreach (var item in dataHiburanAll)
                        {
                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 2).Sum(q => q.Target),
                                Realisasi1 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 1).Sum(q => q.Target),
                                Realisasi2 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year).Sum(q => q.Target),
                                Realisasi3 = context.DbMonHiburans.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                TotalPotensi = context.PotensiCtrlHiburans.Where(x => x.Nop == item.Nop).Sum(q => q.PotensiPajakTahun)
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
                            .Where(x => x.TahunBuku == DateTime.Now.Year && x.KategoriId == kategori)
                            .ToList();

                        var dataAbtAll = dataAbt1
                            .Concat(dataAbt2)
                            .Concat(dataAbt3)
                            .Select(x => new { Nop = x.Nop, NamaOp = x.NamaOp, AlamatOp = x.AlamatOp })
                            .Distinct()
                            .ToList();

                        var listOpAbt1 = dataAbt1.Select(x => x.Nop).ToList();
                        var listOpAbt2 = dataAbt2.Select(x => x.Nop).ToList();
                        var listOpAbt3 = dataAbt3.Select(x => x.Nop).ToList();
                        var listOpAbtAll = dataAbtAll.Select(x => x.Nop).ToList();

                        foreach (var item in dataAbtAll)
                        {
                            var potensi = new DataPotensi
                            {
                                NOP = item.Nop,
                                NamaOP = item.NamaOp,
                                Alamat = item.AlamatOp,
                                JenisPajak = jenisPajak.GetDescription(),
                                Kategori = context.MKategoriPajaks.FirstOrDefault(x => x.Id == kategori)?.Nama ?? "Umum",
                                Target1 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 2).Sum(q => q.Target),
                                Realisasi1 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 2).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target2 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year - 1).Sum(q => q.Target),
                                Realisasi2 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year - 1).Sum(x => x.NominalPokokBayar) ?? 0,
                                Target3 = context.PotensiCtrlTargets.Where(x => x.Nop == item.Nop && x.Tahun == DateTime.Now.Year).Sum(q => q.Target),
                                Realisasi3 = context.DbMonAbts.Where(x => x.Nop == item.Nop && x.TglBayarPokok.Value.Year == DateTime.Now.Year).Sum(x => x.NominalPokokBayar) ?? 0,
                                //TotalPotensi = context.PotensiCtrlAirTanahs.Where(x => x.Nop == item.Nop).Sum(q => q.PotensiPajakTahun) ?? 0
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

            // Perhitungan otomatis

            public decimal RataRataRoomTerjualPerHari => JumlahTotalRoom * OkupansiRateRoom;

            public decimal PotensiOmzetRoomPerBulan => HargaRataRataRoom * RataRataRoomTerjualPerHari * 30;

            public decimal OkupansiRateBanquet => 0.3m * OkupansiRateRoom;

            public decimal RataRataPaxBanquetTerjualPerHari => KapasitasMaksimalPaxBanquetPerHari * OkupansiRateBanquet;

            public decimal PotensiOmzetBanquetPerBulan => HargaRataRataBanquetPerPax * RataRataPaxBanquetTerjualPerHari * 8;

            public decimal PotensiPajakPerBulan => (PotensiOmzetRoomPerBulan + PotensiOmzetBanquetPerBulan) * TarifPajak;

            public decimal PotensiPajakPerTahun => PotensiPajakPerBulan * 12;
        }

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
