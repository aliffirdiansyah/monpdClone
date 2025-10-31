using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Models.DashboardUPTBVM.ViewModel;
using static MonPDReborn.Models.DashboardVM.ViewModel;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileOPVM
    {
        public class Index
        {
            public Index()
            {

            }
        }

        public class ShowRekap
        {
            public List<RekapOP> DataRekapOPList { get; set; } = new();
            public List<OkupansiHotel> DataOkupansiList { get; set; } = new();
            public Dashboard Data { get; set; } = new Dashboard();
            public RekapOPTotal SummaryData { get; set; } = new();
            public RekapOkupansiTotal SummaryDataOkupansi { get; set; } = new();
            public ShowRekap() { }

            public ShowRekap(int tahun)
            {
                DataRekapOPList = Method.GetDataRekapOPList(tahun);
                DataOkupansiList = Method.GetOkupansiHotel(tahun);
                Data = Method.GetDashboardData();

                foreach (var rekap in DataRekapOPList)
                {
                    if (rekap.EnumPajak == 3)
                    {
                        rekap.OkunpasiHotel = DataOkupansiList;
                    }
                }

                SummaryData.TotalOpAwal = DataRekapOPList.Sum(x => x.JmlOpAwal);
                SummaryData.TotalOpTutup = DataRekapOPList.Sum(x => x.JmlOpTutupPermanen);
                SummaryData.TotalOpBaru = DataRekapOPList.Sum(x => x.JmlOpBaru);
                SummaryData.TotalOpAkhir = DataRekapOPList.Sum(x => x.JmlOpAkhir);
                SummaryDataOkupansi.TotalRoom = DataOkupansiList.Sum(x => x.TotalKamar);
                SummaryDataOkupansi.AvgRoomSold = DataOkupansiList.Sum(x => x.AvgRate);
            }
        }


        public class ShowSeries
        {
            public List<SeriesOP> DataSeriesOPList { get; set; } = new();
            public SeriesOPStatistik StatistikData { get; set; } = new();

            public ShowSeries() { }

            public ShowSeries(string keyword)
            {
                DataSeriesOPList = Method.GetDataSeriesOPList();

                // Hitung total untuk setiap tahun menggunakan LINQ
                int total2021 = DataSeriesOPList.Sum(x => x.Tahun2021);
                int total2022 = DataSeriesOPList.Sum(x => x.Tahun2022);
                int total2023 = DataSeriesOPList.Sum(x => x.Tahun2023);
                int total2024 = DataSeriesOPList.Sum(x => x.Tahun2024);
                int total2025 = DataSeriesOPList.Sum(x => x.Tahun2025);

                // Hitung selisih pertumbuhan tahunan
                var selisihPerTahun = new Dictionary<string, int>
                {
                    { "2021-2022", total2022 - total2021 },
                    { "2022-2023", total2023 - total2022 },
                    { "2023-2024", total2024 - total2023 },
                    { "2024-2025", total2025 - total2024 }
                };

                // Cari kenaikan tertinggi
                int kenaikanTertinggi = 0;
                string periodeTertinggi = "-";
                if (selisihPerTahun.Any())
                {
                    kenaikanTertinggi = selisihPerTahun.Values.Max();
                    periodeTertinggi = selisihPerTahun.FirstOrDefault(kv => kv.Value == kenaikanTertinggi).Key ?? "-";
                }

                // Masukkan semua hasil perhitungan ke dalam model StatistikData
                StatistikData = new SeriesOPStatistik
                {
                    TotalOpTahunIni = total2025,
                    PertumbuhanOp = total2025 - total2024,
                    KenaikanTertinggiOp = kenaikanTertinggi,
                    PeriodeKenaikanTertinggi = periodeTertinggi
                };
            }
        }

        public class RekapJml
        {
            public List<JmlObjekPajak> Data { get; set; } = new();
            public SeriesOPStatistik StatistikData { get; set; } = new();

            public RekapJml()
            {
                // Ambil data objek pajak
                Data = Method.GetJmlObjekPajakData();
                var tahunList = Enumerable.Range(DateTime.Now.Year - 4, 5).ToArray(); // contoh: 2021-2025

                // Total akhir semua pajak per tahun
                var totalPerTahun = new Dictionary<int, decimal>();

                foreach (var tahun in tahunList)
                {
                    decimal totalAkhir = 0;

                    foreach (var item in Data)
                    {
                        totalAkhir += tahun switch
                        {
                            var y when y == tahunList[0] => item.Tahun5_Akhir,
                            var y when y == tahunList[1] => item.Tahun4_Akhir,
                            var y when y == tahunList[2] => item.Tahun3_Akhir,
                            var y when y == tahunList[3] => item.Tahun2_Akhir,
                            var y when y == tahunList[4] => item.Tahun1_Akhir,
                            _ => 0
                        };
                    }

                    totalPerTahun[tahun] = totalAkhir;
                }

                // Hitung pertumbuhan
                var tahunIni = tahunList[4];
                var tahunSebelumnya = tahunList[3];

                decimal totalOpTahunIni = totalPerTahun.GetValueOrDefault(tahunIni);
                decimal totalOpTahunSebelumnya = totalPerTahun.GetValueOrDefault(tahunSebelumnya);

                decimal pertumbuhanOp = (totalOpTahunSebelumnya == 0) ? 0 :
                    (totalOpTahunIni - totalOpTahunSebelumnya) / totalOpTahunSebelumnya * 100;

                // Cari kenaikan tertinggi antar tahun
                decimal kenaikanTertinggi = decimal.MinValue;
                string periodeTertinggi = "-";

                for (int i = 1; i < tahunList.Length; i++)
                {
                    var tahunA = tahunList[i - 1];
                    var tahunB = tahunList[i];

                    if (!totalPerTahun.ContainsKey(tahunA) || !totalPerTahun.ContainsKey(tahunB)) continue;

                    var selisih = totalPerTahun[tahunB] - totalPerTahun[tahunA];

                    if (selisih > kenaikanTertinggi)
                    {
                        kenaikanTertinggi = selisih;
                        periodeTertinggi = $"{tahunA}-{tahunB}";
                    }
                }

                // Set data statistik
                StatistikData = new SeriesOPStatistik
                {
                    TotalOpTahunIni = totalOpTahunIni,
                    PertumbuhanOp = Math.Round(pertumbuhanOp, 2),
                    KenaikanTertinggiOp = kenaikanTertinggi,
                    PeriodeKenaikanTertinggi = periodeTertinggi
                };
            }
        }

        public class RekapPerWilayah
        {
            public List<RekapOP> RekapOpWilayahList { get; set; } = new();
            public RekapPerWilayah(string uptb, string kec, string kel)
            {
                RekapOpWilayahList = Method.GetDataRekapPerWilayahList(uptb, kec, kel);
            }
        }

        public class Detail
        {
            public DataDetailOP DataDetail { get; set; } = new();
            public EnumFactory.EPajak Pajak { get; set; }
            public Detail()
            {

            }
            public Detail(string nop, EnumFactory.EPajak pajak)
            {
                Pajak = pajak;
                DataDetail = Method.GetDetailObjekPajak(nop, pajak);
            }
        }

        public class DetailOPSeries
        {
            public List<DetailSeries> Data { get; set; } = new();
            public DetailOPSeries(EnumFactory.EPajak jenisPajak, int tahun)
            {
                Data = Method.GetDetailObjekPajak(jenisPajak, tahun);
            }
        }

        public class ShowTPK
        {
            public List<TPKHotel> Kiri { get; set; }
            public List<TPKHotel> Kanan { get; set; }
            public DashboardTPK Data { get; set; }
            public int TahunKiri { get; set; }
            public int TahunKanan { get; set; }
            public ShowTPK()
            {
                Data = Method.GetDashboardTPK2025();
            }
            public ShowTPK(int tahunKiri, int tahunKanan)
            {


                TahunKiri = tahunKiri;
                TahunKanan = tahunKanan;

                Kiri = Method.GetTPKHotelData(tahunKiri);
                Kanan = Method.GetTPKHotelData(tahunKanan);
            }
        }

        public class RekapDetailHotel
        {
            public List<DetailOkupansiHotel> DataOkupansiHotel { get; set; } = new();
            public RekapDetailHotel(int enumPajak, int kategori, int tahun)
            {
                DataOkupansiHotel = ProfileOPVM.Method.GetRekapDetailHotel(EnumFactory.EPajak.JasaPerhotelan, kategori, tahun);
            }
        }

        public class Method
        {
            public static Dashboard GetDashboardData()
            {
                return new Dashboard
                {
                    TotalPenghimbauan = 1274,
                    PenghimbauanAktif = 156,
                    PenghimbauanSelesai = 103,
                    TingkatKepatuhan = 11.7,
                };
            }
            public static List<RekapOP> GetDataRekapPerWilayahList(string uptb, string kec, string kel)
            {
                var ret = new List<RekapOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var dataResto = context.DbOpRestos
                     .Where(x => x.TahunBuku == currentYear && x.PajakNama != "MAMIN" &&
                                 (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                     .GroupBy(x => new { x.Nop })
                     .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                     .Count();

                var dataPpj = context.DbOpListriks
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataHotel = context.DbOpHotels
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataParkir = context.DbOpParkirs
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataHiburan = context.DbOpHiburans
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();

                var dataAbt = context.DbOpAbts
                    .Where(x => x.TahunBuku == currentYear &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                    .GroupBy(x => new { x.Nop, x.KategoriId })
                    .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                    .Count();


                var OpPbbAkhir = context.DbMonPbbs
                    .Where(x => x.TahunBuku == currentYear && x.Uptb == Convert.ToDecimal(uptb) && x.AlamatKdCamat == kec && x.AlamatKdLurah == kel)
                    .GroupBy(x => new { x.Nop })
                    .Select(g => new { g.Key.Nop })
                    .Count();



                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                    JmlOpAkhir = dataResto,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                    JmlOpAkhir = dataPpj,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                    JmlOpAkhir = dataHotel,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                    JmlOpAkhir = dataParkir,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                    JmlOpAkhir = dataHiburan,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                    JmlOpAkhir = dataAbt,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });
                ret.Add(new RekapOP
                {
                    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.PBB,
                    JmlOpAkhir = OpPbbAkhir,
                    Uptb = uptb,
                    KdKecamatan = kec,
                    KdKelurahan = kel,
                });


                return ret;
            }
            public static List<RekapDetailOP> GetDataRekapPerWilayahDetailList(EnumFactory.EPajak jenisPajak, string uptb, string kec, string kel, bool turu)
            {
                var ret = new List<RekapDetailOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var kategori = context.MKategoriPajaks
                .Where(x => x.PajakId == (int)jenisPajak && x.Id == 17)
                .FirstOrDefault();

                var dataHotel = context.DbOpHotels
                            .Where(x => x.KategoriId == kategori.Id && x.TahunBuku == currentYear &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                            .GroupBy(x => new { x.Nop })
                            .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                            .Count();
                var re = new RekapDetailOP();
                re.JenisPajak = jenisPajak.GetDescription();
                re.EnumPajak = (int)jenisPajak;
                re.KategoriId = (int)kategori.Id;
                re.KategoriNama = kategori.Nama;
                re.JmlOpAkhir = dataHotel;
                re.Uptb = uptb;
                re.KdKecamatan = kec;
                re.KdKelurahan = kel;
                ret.Add(re);

                return ret;
            }
            public static List<RekapDetailOP> GetDataRekapPerWilayahDetailList(EnumFactory.EPajak jenisPajak, string uptb, string kec, string kel)
            {
                var ret = new List<RekapDetailOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

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
                    case EPajak.MakananMinuman:
                        foreach (var item in kategoriList)
                        {
                            var dataResto = context.DbOpRestos
                             .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear && x.PajakNama != "MAMIN" &&
                                         (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                             .GroupBy(x => new { x.Nop })
                             .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                             .Count();

                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataResto;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.TenagaListrik:
                        foreach (var item in kategoriList)
                        {
                            var dataPpj = context.DbOpListriks
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop, x.KategoriId })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataPpj;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.JasaPerhotelan:
                        foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                        {
                            var dataHotel = context.DbOpHotels
                            .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                        (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                            .GroupBy(x => new { x.Nop })
                            .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                            .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataHotel;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);

                        }
                        break;
                    case EPajak.JasaParkir:
                        foreach (var item in kategoriList)
                        {
                            var dataParkir = context.DbOpParkirs
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataParkir;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.JasaKesenianHiburan:
                        foreach (var item in kategoriList)
                        {
                            var dataHiburan = context.DbOpHiburans
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataHiburan;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.AirTanah:
                        foreach (var item in kategoriList)
                        {
                            var dataAbt = context.DbOpAbts
                                .Where(x => x.KategoriId == item.Id && x.TahunBuku == currentYear &&
                                            (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear) && x.WilayahPajak == uptb && x.AlamatOpKdCamat == kec && x.AlamatOpKdLurah == kel)
                                .GroupBy(x => new { x.Nop, x.KategoriId })
                                .Select(g => new { g.Key.Nop, TglMulaiBukaOp = g.Min(y => y.TglMulaiBukaOp), KategoriId = g.First().KategoriId })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = dataAbt;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    case EPajak.PBB:
                        foreach (var item in kategoriList)
                        {
                            var OpPbbAkhir = context.DbMonPbbs
                                .Where(x => x.TahunBuku == currentYear && x.Uptb == Convert.ToDecimal(uptb) && x.AlamatKdCamat == kec && x.AlamatKdLurah == kel && x.KategoriId == item.Id)
                                .GroupBy(x => new { x.Nop })
                                .Select(g => new { g.Key.Nop })
                                .Count();
                            var re = new RekapDetailOP();
                            re.JenisPajak = jenisPajak.GetDescription();
                            re.EnumPajak = (int)jenisPajak;
                            re.KategoriId = (int)item.Id;
                            re.KategoriNama = item.Nama;
                            re.JmlOpAkhir = OpPbbAkhir;
                            re.Uptb = uptb;
                            re.KdKecamatan = kec;
                            re.KdKelurahan = kel;
                            ret.Add(re);
                        }
                        break;
                    default:
                        break;
                }
                return ret;
            }
            public static List<DetailOP> GetDataRekapPerWilayahDetailOPList(EnumFactory.EPajak jenisPajak, int kategori, string uptb, string kec, string kel)
            {
                var ret = new List<DetailOP>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                switch (jenisPajak)
                {
                    case EPajak.MakananMinuman:
                        var dataResto = context.DbOpRestos
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && x.PajakNama != "MAMIN"
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();


                        ret = dataResto;
                        break;
                    case EPajak.TenagaListrik:
                        var dataListrik = context.DbOpListriks
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataListrik;
                        break;
                    case EPajak.JasaPerhotelan:
                        var dataHotel = context.DbOpHotels
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataHotel;
                        break;
                    case EPajak.JasaParkir:
                        var dataParkir = context.DbOpParkirs
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataParkir;
                        break;
                    case EPajak.JasaKesenianHiburan:
                        var dataHiburan = context.DbOpHiburans
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataHiburan;
                        break;
                    case EPajak.AirTanah:
                        var dataAbt = context.DbOpAbts
                            .Where(x => x.KategoriId == kategori
                                        && x.TahunBuku == currentYear
                                        && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > currentYear)
                                        && x.WilayahPajak == uptb
                                        && x.AlamatOpKdCamat == kec
                                        && x.AlamatOpKdLurah == kel)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                                Kecamatan = x.AlamatOpKdCamat,
                                Kelurahan = x.AlamatOpKdLurah
                            })
                            .ToList();

                        ret = dataAbt;
                        break;
                    case EPajak.PBB:
                        var dataPbb = context.DbMonPbbs
                            .Where(x => x.TahunBuku == currentYear
                                        && x.Uptb == Convert.ToDecimal(uptb)
                                        && x.AlamatKdCamat == kec
                                        && x.AlamatKdLurah == kel
                                        && x.KategoriId == kategori)
                            .Select(x => new DetailOP
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.Uptb ?? "-",
                                Kecamatan = x.AlamatKdCamat,
                                Kelurahan = x.AlamatKdLurah
                            })
                            .ToList();

                        ret = dataPbb;
                        break;
                    default:
                        break;
                }

                return ret;
            }
            public static List<RekapOP> GetDataRekapOPList(int tahun)
            {
                var startOfYear = new DateTime(tahun, 1, 1);
                var sysDate = DateTime.Now;
                var result = new List<RekapOP>();
                var context = DBClass.GetContext();

                var pajakList = context.MPajaks
                    .Include(x => x.MKategoriPajaks.OrderBy(x => x.Urutan))
                    .Where(x => x.Id > 0)
                    .ToList();

                var dbOpResto = context.DbOpRestos
                        .Where(x => x.TahunBuku >= tahun - 1)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TahunBuku,
                            x.TglOpTutup,
                            x.TglMulaiBukaOp,
                            x.KategoriId,
                            x.PajakNama
                        }).AsQueryable();

                var dbOpListrik = context.DbOpListriks
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .Select(x => new
                    {
                        x.Nop,
                        x.TahunBuku,
                        x.TglOpTutup,
                        x.TglMulaiBukaOp,
                        x.KategoriId
                    }).AsQueryable();

                var dbOpHotel = context.DbOpHotels
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .Select(x => new
                    {
                        x.Nop,
                        x.TahunBuku,
                        x.TglOpTutup,
                        x.TglMulaiBukaOp,
                        x.KategoriId
                    }).AsQueryable();

                var dbOpParkir = context.DbOpParkirs
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .Select(x => new
                    {
                        x.Nop,
                        x.TahunBuku,
                        x.TglOpTutup,
                        x.TglMulaiBukaOp,
                        x.KategoriId
                    }).AsQueryable();


                var dbOpHiburan = context.DbOpHiburans
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .Select(x => new
                    {
                        x.Nop,
                        x.TahunBuku,
                        x.TglOpTutup,
                        x.TglMulaiBukaOp,
                        x.KategoriId
                    }).AsQueryable();


                var dbOpAbt = context.DbOpAbts
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .Select(x => new
                    {
                        x.Nop,
                        x.TahunBuku,
                        x.TglOpTutup,
                        x.TglMulaiBukaOp,
                        x.KategoriId
                    }).AsQueryable();

                var dbOPProfilePAD = context.DbOpProfils
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .Select(x => new 
                    {
                        x.TahunBuku,
                        x.TglOpTutup,
                        x.TglOpBuka,
                        x.Nop,
                        x.PajakId,
                        x.Kategori,
                        x.Status
                    }).AsQueryable();

                var dbOpReklameProfile = context.DbOpReklameProfils
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .Select(x => new 
                    {
                        x.TahunBuku,
                        x.TglAkhir,
                        x.TglMulai,
                        x.NoFormulir,
                        x.Kategori,
                        x.Status
                    }).AsQueryable();

                //var dbOpPbb = context.DbMonPbbs
                //    .Where(x => x.TahunBuku >= tahun - 1)
                //    .Select(x => new
                //    {
                //        x.TahunBuku,
                //        x.Nop,
                //        x.KategoriId
                //    }).Distinct().AsQueryable();

                var dbOpPbb = context.DbMonPbbs
                    .Where(x => x.TahunBuku >= tahun - 1)
                    .GroupBy(x => new { x.TahunBuku, x.KategoriId })
                    .Select(x => new { 
                        x.Key.TahunBuku, 
                        x.Key.KategoriId, 
                        Jml = x.Count() })
                    .Distinct()
                    .ToList();

                var dbOpBphtb = context.DbMonBphtbs
                    .Where(x => x.Tahun >= tahun - 1)
                    .Select(x => new { x.Tahun, x.Seq })
                    .ToList();

                 var dbOpOpsenPkb = context.DbMonOpsenPkbs
                    .Where(x => x.TahunPajakSspd >= tahun - 1)
                    .AsQueryable();

                var dbOpOpsenBbnkb = context.DbMonOpsenBbnkbs
                    .Where(x => x.TahunPajakSspd >= tahun - 1)
                    .AsQueryable();

                var totalSw = new Stopwatch();
                totalSw.Start();

                foreach (var pajak in pajakList)
                {
                    var sw = new Stopwatch();
                    sw.Start();

                    int totalData = pajak.MKategoriPajaks.Count();
                    int index = 0;

                    var res = new RekapOP();

                    int pajakId = (int)pajak.Id;
                    string pajakName = pajak.Nama;
                    int tutup = 0;
                    int awal = 0;
                    int baru = 0;
                    int akhir = 0;

                    IEnumerable<string> opAwal;
                    IEnumerable<string> opBaru;
                    IEnumerable<string> opAkhir;
                    IEnumerable<string> opTutup;

                    switch ((EnumFactory.EPajak)pajak.Id)
                    {
                        case EnumFactory.EPajak.MakananMinuman:
                            //awal
                            opAwal = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                .Select(x => x.Nop)
                                .Distinct();

                            //Baru
                            opBaru = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Tutup
                            opTutup = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Akhir
                            opAkhir = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                .Select(x => x.Nop)
                                .Distinct();

                            awal = opAwal.Count();
                            baru = opBaru.Count();
                            tutup = opTutup.Count();
                            akhir = opAkhir.Count();

                            break;
                        case EnumFactory.EPajak.TenagaListrik:

                            //awal
                            opAwal = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                .Select(x => x.Nop)
                                .Distinct();

                            //Baru
                            opBaru = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Tutup
                            opTutup = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Akhir
                            opAkhir = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                .Select(x => x.Nop)
                                .Distinct();

                            awal = opAwal.Count();
                            baru = opBaru.Count();
                            tutup = opTutup.Count();
                            akhir = opAkhir.Count();

                            break;
                        case EnumFactory.EPajak.JasaPerhotelan:

                            //awal
                            opAwal = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                .Select(x => x.Nop)
                                .Distinct();

                            //Baru
                            opBaru = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Tutup
                            opTutup = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Akhir
                            opAkhir = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                .Select(x => x.Nop)
                                .Distinct();

                            awal = opAwal.Count();
                            baru = opBaru.Count();
                            tutup = opTutup.Count();
                            akhir = opAkhir.Count();

                            break;
                        case EnumFactory.EPajak.JasaParkir:

                            //awal
                            opAwal = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                .Select(x => x.Nop)
                                .Distinct();

                            //Baru
                            opBaru = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Tutup
                            opTutup = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Akhir
                            opAkhir = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                .Select(x => x.Nop)
                                .Distinct();

                            awal = opAwal.Count();
                            baru = opBaru.Count();
                            tutup = opTutup.Count();
                            akhir = opAkhir.Count();

                            break;
                        case EnumFactory.EPajak.JasaKesenianHiburan:

                            //awal
                            opAwal = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                .Select(x => x.Nop)
                                .Distinct();

                            //Baru
                            opBaru = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Tutup
                            opTutup = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Akhir
                            opAkhir = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                .Select(x => x.Nop)
                                .Distinct();

                            awal = opAwal.Count();
                            baru = opBaru.Count();
                            tutup = opTutup.Count();
                            akhir = opAkhir.Count();

                            break;
                        case EnumFactory.EPajak.AirTanah:

                            //awal
                            opAwal = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                .Select(x => x.Nop)
                                .Distinct();

                            //Baru
                            opBaru = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Tutup
                            opTutup = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                .Select(x => x.Nop)
                                .Distinct();

                            // Akhir
                            opAkhir = dbOPProfilePAD
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                .Select(x => x.Nop)
                                .Distinct();

                            awal = opAwal.Count();
                            baru = opBaru.Count();
                            tutup = opTutup.Count();
                            akhir = opAkhir.Count();

                            break;
                        case EnumFactory.EPajak.Reklame:                            
                            // 1.Reklame Awal
                            opAwal = dbOpReklameProfile
                                .Where( x => x.TahunBuku == tahun &&
                                            x.Status == 1)
                                .Select(x => x.NoFormulir)
                                .Distinct();

                            // 2️. Reklame Baru
                            opBaru = dbOpReklameProfile
                                .Where(x => x.TahunBuku == tahun &&
                                            x.Status == 2)
                                .Select(x => x.NoFormulir)
                                .Distinct();

                            // 3️⃣ Reklame Tutup
                            opTutup = dbOpReklameProfile
                                .Where( x => x.TahunBuku == tahun &&
                                            x.Status == 0)
                                .Select(x => x.NoFormulir)
                                .Distinct();

                            // 4️⃣ Reklame Akhir (Masih Aktif)
                            opAkhir = dbOpReklameProfile
                                .Where( x => x.TahunBuku == tahun &&
                                            x.Status == 3)
                                .Select(x => x.NoFormulir)
                                .Distinct();

                            awal = opAwal.Count();
                            baru = opBaru.Count();
                            tutup = opTutup.Count();
                            akhir = opAkhir.Count();

                            break;
                        case EnumFactory.EPajak.PBB:

                            tutup = 0;
                            awal = context.DbMonPbbs.Where(x => x.TahunBuku == tahun - 1).Select(x => x.Nop).Distinct().Count();
                            baru = 0;
                            akhir = context.DbMonPbbs.Where(x => x.TahunBuku == tahun).Select(x => x.Nop).Distinct().Count();

                            break;
                        case EnumFactory.EPajak.BPHTB:
                            awal = context.DbMonBphtbs.Count(x => x.Tahun == tahun - 1 && x.TglBayar.HasValue && x.TglBayar.Value.Year == tahun - 1);
                            tutup = 0;
                            baru = context.DbMonBphtbs.Count(x => x.Tahun == tahun && x.TglBayar.HasValue && x.TglBayar.Value.Year == tahun);
                            akhir = awal + baru;
                            break;
                        case EnumFactory.EPajak.OpsenPkb:

                            akhir = dbOpOpsenPkb.Count(x => x.TahunPajakSspd == tahun);
                            awal = dbOpOpsenPkb.Count(x => x.TahunPajakSspd == tahun - 1);
                            break;
                        case EnumFactory.EPajak.OpsenBbnkb:
                            akhir = dbOpOpsenBbnkb.Count(x => x.TahunPajakSspd == tahun);
                            awal = dbOpOpsenBbnkb.Count(x => x.TahunPajakSspd == tahun - 1);

                            break;
                        default:
                            break;
                    }

                    res.JenisPajak = pajakName;
                    res.EnumPajak = pajakId;
                    res.JmlOpAwal = awal;
                    res.JmlOpTutupPermanen = tutup;
                    res.JmlOpBaru = baru;
                    res.JmlOpAkhir = akhir;
                    res.Tahun = tahun;

                    foreach (var pajakKategori in pajak.MKategoriPajaks)
                    {
                        var re = new RekapDetailOP();

                        int awalKategori = 0;
                        int baruKategori = 0;
                        int akhirKategori = 0;
                        int tutupKategori = 0;

                        IEnumerable<string> opAwalKategori; 
                        IEnumerable<string> opBaruKategori; 
                        IEnumerable<string> opAkhirKategori;
                        IEnumerable<string> opTutupKategori;

                        switch ((EnumFactory.EPajak)pajak.Id)
                        {
                            case EnumFactory.EPajak.MakananMinuman:
                                // 1. Awal
                                opAwalKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 2️.  Baru
                                opBaruKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 3️. Tutup
                                opTutupKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 4️. Akhir 
                                opAkhirKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                awalKategori = opAwalKategori.Count();
                                baruKategori = opBaruKategori.Count();
                                tutupKategori = opTutupKategori.Count();
                                akhirKategori = opAkhirKategori.Count();

                                break;
                            case EnumFactory.EPajak.TenagaListrik:
                                // 1. Awal
                                opAwalKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 2️.  Baru
                                opBaruKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 3️. Tutup
                                opTutupKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 4️. Akhir 
                                opAkhirKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                awalKategori = opAwalKategori.Count();
                                baruKategori = opBaruKategori.Count();
                                tutupKategori = opTutupKategori.Count();
                                akhirKategori = opAkhirKategori.Count();

                                break;
                            case EnumFactory.EPajak.JasaPerhotelan:

                                // 1. Awal
                                opAwalKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 2️.  Baru
                                opBaruKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 3️. Tutup
                                opTutupKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 4️. Akhir 
                                opAkhirKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                awalKategori = opAwalKategori.Count();
                                baruKategori = opBaruKategori.Count();
                                tutupKategori = opTutupKategori.Count();
                                akhirKategori = opAkhirKategori.Count();

                                break;
                            case EnumFactory.EPajak.JasaParkir:
                                // 1. Awal
                                opAwalKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 2️.  Baru
                                opBaruKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 3️. Tutup
                                opTutupKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 4️. Akhir 
                                opAkhirKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.JasaParkir)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                awalKategori = opAwalKategori.Count();
                                baruKategori = opBaruKategori.Count();
                                tutupKategori = opTutupKategori.Count();
                                akhirKategori = opAkhirKategori.Count();

                                break;
                            case EnumFactory.EPajak.JasaKesenianHiburan:
                                // 1. Awal
                                opAwalKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 2️.  Baru
                                opBaruKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 3️. Tutup
                                opTutupKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 4️. Akhir 
                                opAkhirKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                awalKategori = opAwalKategori.Count();
                                baruKategori = opBaruKategori.Count();
                                tutupKategori = opTutupKategori.Count();
                                akhirKategori = opAkhirKategori.Count();

                                break;
                            case EnumFactory.EPajak.AirTanah:

                                // 1. Awal
                                opAwalKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 1 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 2️.  Baru
                                opBaruKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 2 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 3️. Tutup
                                opTutupKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 0 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                // 4️. Akhir 
                                opAkhirKategori = dbOPProfilePAD
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 3 && x.PajakId == (int)EnumFactory.EPajak.AirTanah)
                                    .Select(x => x.Nop)
                                    .Distinct();

                                awalKategori = opAwalKategori.Count();
                                baruKategori = opBaruKategori.Count();
                                tutupKategori = opTutupKategori.Count();
                                akhirKategori = opAkhirKategori.Count();

                                break;
                            case EnumFactory.EPajak.Reklame:
                                // --- FILTER BERDASARKAN JENIS REKLAME ---

                                // 1.Reklame Awal
                                opAwalKategori = dbOpReklameProfile
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 1)
                                    .Select(x => x.NoFormulir)
                                    .Distinct();

                                // 2️. Reklame Baru
                                opBaruKategori = dbOpReklameProfile
                                    .Where(x => x.Kategori == pajakKategori.Id && x.TahunBuku == tahun &&
                                                x.Status == 2)
                                    .Select(x => x.NoFormulir)
                                    .Distinct();

                                // 3️⃣ Reklame Tutup
                                opTutupKategori = dbOpReklameProfile
                                    .Where(x => x.Kategori == pajakKategori.Id &&  x.TahunBuku == tahun &&
                                                x.Status == 0)
                                    .Select(x => x.NoFormulir)
                                    .Distinct();

                                // 4️⃣ Reklame Akhir (Masih Aktif)
                                opAkhirKategori = dbOpReklameProfile
                                    .Where(x => x.Kategori == pajakKategori.Id &&  x.TahunBuku == tahun &&
                                                x.Status == 3)
                                    .Select(x => x.NoFormulir)
                                    .Distinct();

                                awalKategori = opAwalKategori.Count();
                                baruKategori = opBaruKategori.Count();
                                tutupKategori = opTutupKategori.Count();
                                akhirKategori = opAkhirKategori.Count();

                                break;
                            case EnumFactory.EPajak.PBB:

                                tutupKategori = 0;
                                awalKategori = dbOpPbb.Where(x => x.KategoriId == pajakKategori.Id && x.TahunBuku == tahun - 1).Sum(q => q.Jml);
                                baruKategori = 0;
                                akhirKategori = dbOpPbb.Where(x => x.KategoriId == pajakKategori.Id && x.TahunBuku == tahun).Sum(q => q.Jml);

                                break;
                            case EnumFactory.EPajak.BPHTB:
                                akhirKategori = akhir;
                                awalKategori = awal;
                                break;
                            case EnumFactory.EPajak.OpsenPkb:

                                akhirKategori = dbOpOpsenPkb.Count(x => x.TahunPajakSspd == tahun);
                                awalKategori = dbOpOpsenPkb.Count(x => x.TahunPajakSspd == tahun - 1);
                                break;
                            case EnumFactory.EPajak.OpsenBbnkb:
                                akhirKategori = dbOpOpsenBbnkb.Count(x => x.TahunPajakSspd == tahun);
                                awalKategori = dbOpOpsenBbnkb.Count(x => x.TahunPajakSspd == tahun - 1);

                                break;
                            default:

                                break;
                        }

                        re.KategoriId = Convert.ToInt32(pajakKategori.Id);
                        re.KategoriNama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pajakKategori.Nama.ToLower());
                        re.EnumPajak = pajak.Id;
                        re.JenisPajak = pajak.Nama;
                        re.Tahun = tahun;
                        re.JmlOpAwal = awalKategori;
                        re.JmlOpTutupPermanen = tutupKategori;
                        re.JmlOpBaru = baruKategori;
                        re.JmlOpAkhir = akhirKategori;
                        res.RekapDetail.Add(re);

                        double persen = ((double)(index + 1) / totalData) * 100;
                        Console.Write($"\r[ DATA PAJAK {pajak.Nama} [({persen:F2}%)]");
                        index++;
                    }

                    sw.Stop();
                    Console.WriteLine($"");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($" DATA PAJAK {pajak.Nama} Finished [{sw.Elapsed.Minutes} Menit {sw.Elapsed.Seconds} Detik]");
                    Console.ResetColor();
                    Console.WriteLine($"");
                    result.Add(res);
                }

                totalSw.Stop();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($" SEMUA PAJAK Finished [{totalSw.Elapsed.Hours} Jam {totalSw.Elapsed.Minutes} Menit {totalSw.Elapsed.Seconds} Detik]");
                Console.ResetColor();

                return result;
                //return new List<RekapOP>
                //{
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                //        JmlOpAwal = OpRestoAwal,
                //        JmlOpTutupPermanen = OpRestoTutup,
                //        JmlOpBaru = OpRestoBaru,
                //        JmlOpAkhir = OpRestoAkhir,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                //        JmlOpAwal = OpHotelAwal,
                //        JmlOpTutupPermanen = OpHotelTutup,
                //        JmlOpBaru = OpHotelBaru,
                //        JmlOpAkhir = OpHotelAkhir,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                //        JmlOpAwal = OpHiburanAwal,
                //        JmlOpTutupPermanen = OpHiburanTutup,
                //        JmlOpBaru = OpHiburanBaru,
                //        JmlOpAkhir = OpHiburanAkhir,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                //        JmlOpAwal = OpParkirAwal,
                //        JmlOpTutupPermanen = OpParkirTutup,
                //        JmlOpBaru = OpParkirBaru,
                //        JmlOpAkhir = OpParkirAkhir,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                //        JmlOpAwal = OpListrikAwal,
                //        JmlOpTutupPermanen = OpListrikTutup,
                //        JmlOpBaru = OpListrikBaru,
                //        JmlOpAkhir = OpListrikAkhir,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.PBB,
                //        JmlOpAwal = OpPbbAwal,
                //        JmlOpTutupPermanen = 0,
                //        JmlOpBaru = OpPbbBaru,
                //        JmlOpAkhir = OpPbbAkhir,
                //        Tahun = tahun,
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                //        JmlOpAwal = OpBphtbAwal,
                //        JmlOpTutupPermanen = 0,
                //        JmlOpBaru = OpBphtbNow - 0,
                //        JmlOpAkhir = OpBphtbNow ,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.Reklame,
                //        JmlOpAwal = OpReklameAwal,
                //        JmlOpTutupPermanen = 0,
                //        JmlOpBaru = OpReklameBaru,
                //        JmlOpAkhir = OpReklameAkhir,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                //        JmlOpAwal = OpAbtAwal,
                //        JmlOpTutupPermanen = OpAbtTutup,
                //        JmlOpBaru = OpAbtBaru,
                //        JmlOpAkhir = OpAbtAkhir,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                //        JmlOpAwal = OpOpsenPkbAwal,
                //        JmlOpTutupPermanen = 0,
                //        JmlOpBaru = 0,
                //        JmlOpAkhir = OpOpsenPkbNow,
                //        Tahun = tahun
                //    },
                //    new RekapOP
                //    {
                //        JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                //        EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                //        JmlOpAwal = OpOpsenBbnkbAwal,
                //        JmlOpTutupPermanen = 0,
                //        JmlOpBaru = 0,
                //        JmlOpAkhir = OpOpsenBbnkbNow,
                //        Tahun = tahun
                //    },
                //};

            }
            public static List<SeriesOP> GetDataSeriesOPList()
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var OpRestoNow = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpRestoMines1 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpRestoMines2 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpRestoMines3 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpRestoMines4 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpHotelNow = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpHotelMines1 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpHotelMines2 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpHotelMines3 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpHotelMines4 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpHiburanNow = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpHiburanMines1 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpHiburanMines2 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpHiburanMines3 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpHiburanMines4 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpParkirNow = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpParkirMines1 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpParkirMines2 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpParkirMines3 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpParkirMines4 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpListrikNow = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpListrikMines1 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpListrikMines2 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpListrikMines3 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpListrikMines4 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpAbtNow = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear));
                var OpAbtMines1 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1));
                var OpAbtMines2 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2));
                var OpAbtMines3 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3));
                var OpAbtMines4 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4));

                var OpPbbNow = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear).Select(x => x.Nop).Distinct().Count();
                var OpPbbMines1 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 1).Select(x => x.Nop).Distinct().Count();
                var OpPbbMines2 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 2).Select(x => x.Nop).Distinct().Count();
                var OpPbbMines3 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 3).Select(x => x.Nop).Distinct().Count();
                var OpPbbMines4 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 4).Select(x => x.Nop).Distinct().Count();

                var OpBphtbNow = context.DbMonBphtbs.Count(x => x.Tahun == currentYear);
                var OpBphtbMines1 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 1);
                var OpBphtbMines2 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 2);
                var OpBphtbMines3 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 3);
                var OpBphtbMines4 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 4);

                var OpReklameNow = context.DbOpReklames.Count(x => x.TahunBuku == currentYear);
                var OpReklameMines1 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 1);
                var OpReklameMines2 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 2);
                var OpReklameMines3 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 3);
                var OpReklameMines4 = context.DbOpReklames.Count(x => x.TahunBuku == currentYear - 4);

                var OpOpsenPkbNow = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear);
                var OpOpsenPkbMines1 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 1);
                var OpOpsenPkbMines2 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 2);
                var OpOpsenPkbMines3 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 3);
                var OpOpsenPkbMines4 = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == currentYear - 4);

                var OpOpsenBbnkbNow = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear);
                var OpOpsenBbnkbMines1 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 1);
                var OpOpsenBbnkbMines2 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 2);
                var OpOpsenBbnkbMines3 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 3);
                var OpOpsenBbnkbMines4 = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == currentYear - 4);

                var result = new List<SeriesOP>();

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                    Tahun2021 = OpRestoMines4,
                    Tahun2022 = OpRestoMines3,
                    Tahun2023 = OpRestoMines2,
                    Tahun2024 = OpRestoMines1,
                    Tahun2025 = OpRestoNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                    Tahun2021 = OpHotelMines4,
                    Tahun2022 = OpHotelMines3,
                    Tahun2023 = OpHotelMines2,
                    Tahun2024 = OpHotelMines1,
                    Tahun2025 = OpHotelNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                    Tahun2021 = OpHiburanMines4,
                    Tahun2022 = OpHiburanMines3,
                    Tahun2023 = OpHiburanMines2,
                    Tahun2024 = OpHiburanMines1,
                    Tahun2025 = OpHiburanNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                    Tahun2021 = OpParkirMines4,
                    Tahun2022 = OpParkirMines3,
                    Tahun2023 = OpParkirMines2,
                    Tahun2024 = OpParkirMines1,
                    Tahun2025 = OpParkirNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                    Tahun2021 = OpListrikMines4,
                    Tahun2022 = OpListrikMines3,
                    Tahun2023 = OpListrikMines2,
                    Tahun2024 = OpListrikMines1,
                    Tahun2025 = OpListrikNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.PBB,
                    Tahun2021 = OpPbbMines4,
                    Tahun2022 = OpPbbMines3,
                    Tahun2023 = OpPbbMines2,
                    Tahun2024 = OpPbbMines1,
                    Tahun2025 = OpPbbNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                    Tahun2021 = OpBphtbMines4,
                    Tahun2022 = OpBphtbMines3,
                    Tahun2023 = OpBphtbMines2,
                    Tahun2024 = OpBphtbMines1,
                    Tahun2025 = OpBphtbNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.Reklame,
                    Tahun2021 = OpReklameMines4,
                    Tahun2022 = OpReklameMines3,
                    Tahun2023 = OpReklameMines2,
                    Tahun2024 = OpReklameMines1,
                    Tahun2025 = OpReklameNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                    Tahun2021 = OpAbtMines4,
                    Tahun2022 = OpAbtMines3,
                    Tahun2023 = OpAbtMines2,
                    Tahun2024 = OpAbtMines1,
                    Tahun2025 = OpAbtNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                    Tahun2021 = OpOpsenPkbMines4,
                    Tahun2022 = OpOpsenPkbMines3,
                    Tahun2023 = OpOpsenPkbMines2,
                    Tahun2024 = OpOpsenPkbMines1,
                    Tahun2025 = OpOpsenPkbNow
                });

                result.Add(new SeriesOP()
                {
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                    Tahun2021 = OpOpsenBbnkbMines4,
                    Tahun2022 = OpOpsenBbnkbMines3,
                    Tahun2023 = OpOpsenBbnkbMines2,
                    Tahun2024 = OpOpsenBbnkbMines1,
                    Tahun2025 = OpOpsenBbnkbNow
                });

                return result;
            }

            #region REKAP DATA JUMLAH OP
            public static List<RekapDetail> GetRekapDetailData(EnumFactory.EPajak JenisPajak, int tahun)
            {
                var context = DBClass.GetContext();
                var ret = new List<RekapDetail>();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)JenisPajak).OrderBy(x => x.Urutan)
                    .ToList() // pindah ke memory agar bisa pakai ToTitleCase
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        foreach (var kat in kategoriList)
                        {
                            //var OpRestoTutup = context.DbOpRestos.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpRestoAwal = context.DbOpRestos.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id && x.PajakNama != "MAMIN");
                            var OpRestoBaru = context.DbOpRestos.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id && x.PajakNama != "MAMIN");
                            var OpRestoAkhir = context.DbOpRestos.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id && x.PajakNama != "MAMIN");

                            var OpRestoTutup = (OpRestoAwal + OpRestoBaru) - OpRestoAkhir;

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpRestoAwal,
                                JmlOpTutupPermanen = OpRestoTutup,
                                JmlOpBaru = OpRestoBaru,
                                JmlOpAkhir = OpRestoAkhir,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        foreach (var kat in kategoriList)
                        {
                            var OpListrikTutup = context.DbOpListriks.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpListrikAwal = context.DbOpListriks.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpListrikBaru = context.DbOpListriks.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpListrikAkhir = context.DbOpListriks.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpListrikAwal,
                                JmlOpTutupPermanen = OpListrikTutup,
                                JmlOpBaru = OpListrikBaru,
                                JmlOpAkhir = OpListrikAkhir
                            });
                        }

                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        foreach (var kat in kategoriList)
                        {
                            var OpHotelTutup = context.DbOpHotels.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpHotelAwal = context.DbOpHotels.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpHotelBaru = context.DbOpHotels.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpHotelAkhir = context.DbOpHotels.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            //// Determine KategoriGroup based on nama
                            //string kategoriGroup;
                            //if (kat.Nama.ToUpper().Contains("BINTANG") && !kat.Nama.ToUpper().Contains("NON"))
                            //{
                            //    kategoriGroup = "Hotel Berbintang";
                            //}
                            //else if (kat.Nama.ToUpper().Contains("NON"))
                            //{
                            //    kategoriGroup = "Hotel Non Bintang";
                            //}
                            //else
                            //{
                            //    kategoriGroup = "Hotel Lainnya"; // fallback
                            //}

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                //KategoriGroup = kategoriGroup,
                                JmlOpAwal = OpHotelAwal,
                                JmlOpTutupPermanen = OpHotelTutup,
                                JmlOpBaru = OpHotelBaru,
                                JmlOpAkhir = OpHotelAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        foreach (var kat in kategoriList)
                        {
                            var OpParkirTutup = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpParkirAwal = context.DbOpParkirs.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpParkirBaru = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpParkirAkhir = context.DbOpParkirs.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpParkirAwal,
                                JmlOpTutupPermanen = OpParkirTutup,
                                JmlOpBaru = OpParkirBaru,
                                JmlOpAkhir = OpParkirAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        foreach (var kat in kategoriList)
                        {
                            var OpHiburanTutup = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpHiburanAwal = context.DbOpHiburans.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpHiburanBaru = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpHiburanAkhir = context.DbOpHiburans.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpHiburanAwal,
                                JmlOpTutupPermanen = OpHiburanTutup,
                                JmlOpBaru = OpHiburanBaru,
                                JmlOpAkhir = OpHiburanAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        foreach (var kat in kategoriList)
                        {
                            var OpAbtTutup = context.DbOpAbts.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpAbtAwal = context.DbOpAbts.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpAbtBaru = context.DbOpAbts.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Year == tahun && x.KategoriId == kat.Id);
                            var OpAbtAkhir = context.DbOpAbts.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpAbtAwal,
                                JmlOpTutupPermanen = OpAbtTutup,
                                JmlOpBaru = OpAbtBaru,
                                JmlOpAkhir = OpAbtAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        foreach (var kat in kategoriList)
                        {
                            var OpReklameTutup = context.DbOpReklames.Count(x => x.TahunBuku == tahun && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpReklameAwal = context.DbOpReklames.Count(x => x.TahunBuku == tahun - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun - 1) && x.KategoriId == kat.Id);
                            var OpReklameBaru = context.DbOpReklames.Count(x => x.TahunBuku == tahun && x.TglMulaiBukaOp.Value.Year == tahun && x.KategoriId == kat.Id);
                            var OpReklameAkhir = context.DbOpReklames.Count(x => x.TahunBuku == tahun && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun) && x.KategoriId == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpReklameAwal,
                                JmlOpTutupPermanen = OpReklameTutup,
                                JmlOpBaru = OpReklameBaru,
                                JmlOpAkhir = OpReklameAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        foreach (var kat in kategoriList)
                        {
                            var OpPbbTutup = context.DbMonPbbs.Where(x => x.TahunBuku == tahun && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();
                            var OpPbbAwal = context.DbMonPbbs.Where(x => x.TahunBuku == tahun - 1 && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();
                            var OpPbbBaru = context.DbMonPbbs.Where(x => x.TahunBuku == tahun && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();
                            var OpPbbAkhir = context.DbMonPbbs.Where(x => x.TahunBuku == tahun && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpPbbAwal,
                                JmlOpTutupPermanen = 0,
                                JmlOpBaru = OpPbbBaru,
                                JmlOpAkhir = OpPbbAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        foreach (var kat in kategoriList)
                        {
                            var OpBphtbTutup = context.DbMonBphtbs.Count(x => x.Tahun == tahun && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kat.Id);
                            var OpBphtbAwal = context.DbMonBphtbs.Count(x => x.Tahun == tahun - 1 && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kat.Id);
                            var OpBphtbBaru = context.DbMonBphtbs.Count(x => x.Tahun == tahun && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kat.Id);
                            var OpBphtbAkhir = context.DbMonBphtbs.Count(x => x.Tahun == tahun && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kat.Id);

                            ret.Add(new RekapDetail
                            {
                                JenisPajak = JenisPajak.GetDescription(),
                                EnumPajak = (int)JenisPajak,
                                Tahun = tahun,
                                KategoriId = (int)kat.Id,
                                Kategori = kat.Nama,
                                JmlOpAwal = OpBphtbAwal,
                                JmlOpTutupPermanen = 0,
                                JmlOpBaru = OpBphtbBaru,
                                JmlOpAkhir = OpBphtbAkhir
                            });
                        }
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;

                return ret;

            }
            public static List<RekapMaster> GetRekapMasterData(EnumFactory.EPajak JenisPajak, int kategori, string status, int tahun)
            {
                var context = DBClass.GetContext();
                var ret = new List<RekapMaster>();

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        //  STATUS 0 - TUTUP
                        var OpRestoTutup = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 0
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 1 - AWAL
                        var OpRestoAwal = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 1
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 2 - BARU
                        var OpRestoBaru = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 2
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 3 - AKHIR
                        var OpRestoAkhir = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 3
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpRestoAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpRestoTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpRestoBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpRestoAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        //  STATUS 0 - TUTUP
                        var OpListrikTutup = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 0
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 1 - AWAL
                        var OpListrikAwal = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 1
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 2 - BARU
                        var OpListrikBaru = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 2
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 3 - AKHIR
                        var OpListrikAkhir = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 3
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpListrikAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpListrikTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpListrikBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpListrikAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:

                        //  STATUS 0 - TUTUP
                        var OpHotelTutup = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 0
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 1 - AWAL
                        var OpHotelAwal = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 1
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 2 - BARU
                        var OpHotelBaru = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 2
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 3 - AKHIR
                        var OpHotelAkhir = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 3
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpHotelAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpHotelTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpHotelBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpHotelAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        //  STATUS 0 - TUTUP
                        var OpParkirTutup = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 0
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 1 - AWAL
                        var OpParkirAwal = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 1
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 2 - BARU
                        var OpParkirBaru = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 2
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 3 - AKHIR
                        var OpParkirAkhir = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 3
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpParkirAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpParkirTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpParkirBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpParkirAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        //  STATUS 0 - TUTUP
                        var OpHiburanTutup = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 0
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 1 - AWAL
                        var OpHiburanAwal = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 1
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 2 - BARU
                        var OpHiburanBaru = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 2
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 3 - AKHIR
                        var OpHiburanAkhir = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 3
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();
                        if (status == "JmlOpAwal")
                        {
                            ret = OpHiburanAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpHiburanTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpHiburanBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpHiburanAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        //  STATUS 0 - TUTUP
                        var OpAbtTutup = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 0
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 1 - AWAL
                        var OpAbtAwal = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 1
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 2 - BARU
                        var OpAbtBaru = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 2
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();

                        //  STATUS 3 - AKHIR
                        var OpAbtAkhir = context.DbOpProfils
                            .Where(p => p.TahunBuku == (decimal)tahun
                                      && p.Status == 3
                                      && p.PajakId == (decimal)JenisPajak
                                      && p.Kategori == (decimal)kategori)
                            .Select(p => new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.Nop,
                                p.TglOpBuka,
                                p.TglOpTutup,
                                p.NamaOp,
                                p.AlamatOp,
                                p.KategoriNama,
                                p.WilayahPajak
                            })
                            .ToList();
                        if (status == "JmlOpAwal")
                        {
                            ret = OpAbtAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpAbtTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpAbtBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpAbtAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        // Untuk Reklame Tahunbuku = tahun ( tidak perlu -1 )karena sudah dihandel dari tabel  DbOpReklameProfils

                        //  STATUS 0 - TUTUP
                        var OpReklameTutup =
                             (from p in context.DbOpReklameProfils
                              join r in context.DbOpReklames
                                  on new { p.NoFormulir, Kategori = (int)p.Kategori } equals new { r.NoFormulir, Kategori = (int)r.KategoriId }
                              where p.TahunBuku == tahun && p.Status == 0 && p.Kategori == kategori
                              select new
                              {
                                  p.TahunBuku,
                                  p.Kategori,
                                  p.Status,
                                  p.NoFormulir,
                                  r.Nama,
                                  r.Alamat,
                                  r.KategoriNama,
                                  r.TglMulaiBerlaku,
                                  r.TglAkhirBerlaku,
                                  r.FlagPermohonan
                              })
                             .Distinct()
                             .ToList();


                        //  STATUS 1 - AWAL
                        var OpReklameAwal =
                           (from p in context.DbOpReklameProfils
                            join r in context.DbOpReklames
                                on new { p.NoFormulir, Kategori = (int)p.Kategori } equals new { r.NoFormulir, Kategori = (int)r.KategoriId }
                            where p.TahunBuku == tahun && p.Status == 1 && p.Kategori == kategori
                            select new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.NoFormulir,
                                r.Nama,
                                r.Alamat,
                                r.KategoriNama,
                                r.TglMulaiBerlaku,
                                r.TglAkhirBerlaku,
                                r.FlagPermohonan
                            })
                             .Distinct()
                             .ToList();

                        //  STATUS 2 - BARU
                        var OpReklameBaru =
                           (from p in context.DbOpReklameProfils
                            join r in context.DbOpReklames
                                on new { p.NoFormulir, Kategori = (int)p.Kategori } equals new { r.NoFormulir, Kategori = (int)r.KategoriId }
                            where p.TahunBuku == tahun && p.Status == 2 && p.Kategori == kategori
                            select new
                            {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.NoFormulir,
                                r.Nama,
                                r.KategoriNama,
                                r.Alamat,
                                r.TglMulaiBerlaku,
                                r.TglAkhirBerlaku,
                            })
                            .Distinct()
                            .ToList();

                        //  STATUS 3 - AKHIR
                        var OpReklameAkhir =
                            (from p in context.DbOpReklameProfils
                             join r in context.DbOpReklames
                                 on new { p.NoFormulir, Kategori = (int)p.Kategori } equals new { r.NoFormulir, Kategori = (int)r.KategoriId }
                             where p.TahunBuku == tahun && p.Status == 3 && p.Kategori == kategori
                             select new
                             {
                                p.TahunBuku,
                                p.Kategori,
                                p.Status,
                                p.NoFormulir,
                                r.Nama,
                                r.KategoriNama,
                                r.Alamat,
                                r.TglMulaiBerlaku,
                                r.TglAkhirBerlaku,
                            })
                            .Distinct()
                            .ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpReklameAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.NoFormulir,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            ret = OpReklameTutup.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.NoFormulir,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpReklameBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.NoFormulir,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpReklameAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.Kategori,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.NoFormulir,
                                NamaOP = x.Nama,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.PBB:

                        var OpPbbTutup = context.DbMonPbbs.Where(x => x.TahunBuku == tahun)
                            .Select(x => new
                            {
                                x.KategoriId,
                                x.KategoriNama,
                                x.Nop,
                                x.WpNama,
                                x.AlamatOp,
                                x.Uptb,
                            }).Distinct()
                            .ToList();
                        var OpPbbAwal = context.DbMonPbbs.Where(x => x.TahunBuku == tahun - 1)
                            .Select(x => new
                            {
                                x.KategoriId,
                                x.KategoriNama,
                                x.Nop,
                                x.WpNama,
                                x.AlamatOp,
                                x.Uptb,
                            }).Distinct()
                            .ToList();
                        var OpPbbBaru = context.DbMonPbbs.Where(x => x.TahunBuku == tahun)
                            .Select(x => new
                            {
                                x.KategoriId,
                                x.KategoriNama,
                                x.Nop,
                                x.WpNama,
                                x.AlamatOp,
                                x.Uptb,
                            }).Distinct()
                            .ToList();
                        var OpPbbAkhir = context.DbMonPbbs.Where(x => x.TahunBuku == tahun)
                            .Select(x => new
                            {
                                x.KategoriId,
                                x.KategoriNama,
                                x.Nop,
                                x.WpNama,
                                x.AlamatOp,
                                x.Uptb,
                            }).Distinct()
                            .ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpPbbAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            //
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpPbbBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpPbbAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var OpBphtbTutup = context.DbMonBphtbs.Where(x => x.Tahun == tahun && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kategori).ToList();
                        var OpBphtbAwal = context.DbMonBphtbs.Where(x => x.Tahun == tahun - 1 && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kategori).ToList();
                        var OpBphtbBaru = context.DbMonBphtbs.Where(x => x.Tahun == tahun && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kategori).ToList();
                        var OpBphtbAkhir = context.DbMonBphtbs.Where(x => x.Tahun == tahun && Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kategori).ToList();

                        if (status == "JmlOpAwal")
                        {
                            ret = OpBphtbAwal.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()),
                                Kategori_Nama = x.Perolehan,
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpTutupPermanen")
                        {
                            //
                        }
                        else if (status == "JmlOpBaru")
                        {
                            ret = OpBphtbBaru.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()),
                                Kategori_Nama = x.Perolehan,
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (status == "JmlOpAkhir")
                        {
                            ret = OpBphtbAkhir.Select(x => new RekapMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()),
                                Kategori_Nama = x.Perolehan,
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;
                return ret;
            }
            #endregion

            #region SERIES DATA JUMLAH OP
            public static List<SeriesDetail> GetSeriesDetailData(EnumFactory.EPajak JenisPajak)
            {
                var ret = new List<SeriesDetail>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)JenisPajak)
                    .ToList() // pindah ke memory agar bisa pakai ToTitleCase
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                var currentYear = DateTime.Now.Year;

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        foreach (var kat in kategoriList)
                        {
                            var OpRestoNow = context.DbOpRestos.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpRestoMines1 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpRestoMines2 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpRestoMines3 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpRestoMines4 = context.DbOpRestos.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpRestoMines4,
                                TahunMines3 = OpRestoMines3,
                                TahunMines2 = OpRestoMines2,
                                TahunMines1 = OpRestoMines1,
                                TahunNow = OpRestoNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        foreach (var kat in kategoriList)
                        {
                            var OpListrikNow = context.DbOpListriks.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpListrikMines1 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpListrikMines2 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpListrikMines3 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpListrikMines4 = context.DbOpListriks.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpListrikMines4,
                                TahunMines3 = OpListrikMines3,
                                TahunMines2 = OpListrikMines2,
                                TahunMines1 = OpListrikMines1,
                                TahunNow = OpListrikNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        foreach (var kat in kategoriList.OrderBy(x => x.Id).ToList())
                        {
                            var OpHotelNow = context.DbOpHotels.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpHotelMines1 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpHotelMines2 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpHotelMines3 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpHotelMines4 = context.DbOpHotels.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpHotelMines4,
                                TahunMines3 = OpHotelMines3,
                                TahunMines2 = OpHotelMines2,
                                TahunMines1 = OpHotelMines1,
                                TahunNow = OpHotelNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        foreach (var kat in kategoriList)
                        {
                            var OpParkirNow = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpParkirMines1 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpParkirMines2 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpParkirMines3 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpParkirMines4 = context.DbOpParkirs.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpParkirMines4,
                                TahunMines3 = OpParkirMines3,
                                TahunMines2 = OpParkirMines2,
                                TahunMines1 = OpParkirMines1,
                                TahunNow = OpParkirNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        foreach (var kat in kategoriList)
                        {
                            var OpHiburanNow = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpHiburanMines1 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpHiburanMines2 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpHiburanMines3 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpHiburanMines4 = context.DbOpHiburans.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpHiburanMines4,
                                TahunMines3 = OpHiburanMines3,
                                TahunMines2 = OpHiburanMines2,
                                TahunMines1 = OpHiburanMines1,
                                TahunNow = OpHiburanNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        foreach (var kat in kategoriList)
                        {
                            var OpAbtNow = context.DbOpAbts.Count(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kat.Id);
                            var OpAbtMines1 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kat.Id);
                            var OpAbtMines2 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kat.Id);
                            var OpAbtMines3 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kat.Id);
                            var OpAbtMines4 = context.DbOpAbts.Count(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kat.Id);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpAbtMines4,
                                TahunMines3 = OpAbtMines3,
                                TahunMines2 = OpAbtMines2,
                                TahunMines1 = OpAbtMines1,
                                TahunNow = OpAbtNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:

                        break;
                    case EnumFactory.EPajak.PBB:
                        foreach (var kat in kategoriList)
                        {
                            var OpPbbNow = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();
                            var OpPbbMines1 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 1 && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();
                            var OpPbbMines2 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 2 && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();
                            var OpPbbMines3 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 3 && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();
                            var OpPbbMines4 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 4 && x.KategoriId == kat.Id).Select(x => x.Nop).Distinct().Count();


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpPbbMines4,
                                TahunMines3 = OpPbbMines3,
                                TahunMines2 = OpPbbMines2,
                                TahunMines1 = OpPbbMines1,
                                TahunNow = OpPbbNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        foreach (var kat in kategoriList)
                        {
                            var OpBphtbNow = context.DbMonBphtbs.Count(x => x.Tahun == currentYear);
                            var OpBphtbMines1 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 1);
                            var OpBphtbMines2 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 2);
                            var OpBphtbMines3 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 3);
                            var OpBphtbMines4 = context.DbMonBphtbs.Count(x => x.Tahun == currentYear - 4);


                            ret.Add(new SeriesDetail
                            {
                                EnumPajak = (int)JenisPajak,
                                JenisPajak = JenisPajak.GetDescription(),
                                Kategori = kat.Nama,
                                KategoriId = (int)kat.Id,
                                TahunMines4 = OpBphtbMines4,
                                TahunMines3 = OpBphtbMines3,
                                TahunMines2 = OpBphtbMines2,
                                TahunMines1 = OpBphtbMines1,
                                TahunNow = OpBphtbNow,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;

                return ret;

            }
            public static List<SeriesMaster> GetSeriesMasterData(EnumFactory.EPajak JenisPajak, int kategori, string tahunHuruf)
            {
                var ret = new List<SeriesMaster>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                switch (JenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var OpRestoNow = context.DbOpRestos.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpRestoMines1 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpRestoMines2 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpRestoMines3 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpRestoMines4 = context.DbOpRestos.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpRestoMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpRestoMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpRestoMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpRestoMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpRestoNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var OpListrikNow = context.DbOpListriks.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpListrikMines1 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpListrikMines2 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpListrikMines3 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpListrikMines4 = context.DbOpListriks.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpListrikMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpListrikMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpListrikMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpListrikMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpListrikNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var OpHotelNow = context.DbOpHotels.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpHotelMines1 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpHotelMines2 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpHotelMines3 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpHotelMines4 = context.DbOpHotels.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpHotelMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpHotelMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpHotelMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpHotelMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpHotelNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var OpParkirNow = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpParkirMines1 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpParkirMines2 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpParkirMines3 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpParkirMines4 = context.DbOpParkirs.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpParkirMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpParkirMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpParkirMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpParkirMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpParkirNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var OpHiburanNow = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines1 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines2 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines3 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpHiburanMines4 = context.DbOpHiburans.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpHiburanMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpHiburanMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpHiburanMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpHiburanMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpHiburanNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var OpAbtNow = context.DbOpAbts.Where(x => x.TahunBuku == currentYear && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear) && x.KategoriId == kategori).ToList();
                        var OpAbtMines1 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 1 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 1) && x.KategoriId == kategori).ToList();
                        var OpAbtMines2 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 2 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 2) && x.KategoriId == kategori).ToList();
                        var OpAbtMines3 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 3 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 3) && x.KategoriId == kategori).ToList();
                        var OpAbtMines4 = context.DbOpAbts.Where(x => x.TahunBuku == currentYear - 4 && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > currentYear - 4) && x.KategoriId == kategori).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpAbtMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpAbtMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpAbtMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpAbtMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpAbtNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:
                        var OpPbbNow = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear && x.KategoriId == kategori).Select(x => new
                        {
                            x.KategoriId,
                            x.KategoriNama,
                            x.Nop,
                            x.WpNama,
                            x.AlamatOp,
                            x.Uptb,
                        }).Distinct()
.ToList();
                        var OpPbbMines1 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 1 && x.KategoriId == kategori).Select(x => new
                        {
                            x.KategoriId,
                            x.KategoriNama,
                            x.Nop,
                            x.WpNama,
                            x.AlamatOp,
                            x.Uptb,
                        }).Distinct()
.ToList();
                        var OpPbbMines2 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 2 && x.KategoriId == kategori).Select(x => new
                        {
                            x.KategoriId,
                            x.KategoriNama,
                            x.Nop,
                            x.WpNama,
                            x.AlamatOp,
                            x.Uptb,
                        }).Distinct()
.ToList();
                        var OpPbbMines3 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 3 && x.KategoriId == kategori).Select(x => new
                        {
                            x.KategoriId,
                            x.KategoriNama,
                            x.Nop,
                            x.WpNama,
                            x.AlamatOp,
                            x.Uptb,
                        }).Distinct()
.ToList();
                        var OpPbbMines4 = context.DbMonPbbs.Where(x => x.TahunBuku == currentYear - 4 && x.KategoriId == kategori).Select(x => new
                        {
                            x.KategoriId,
                            x.KategoriNama,
                            x.Nop,
                            x.WpNama,
                            x.AlamatOp,
                            x.Uptb,
                        }).Distinct()
.ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpPbbMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",

                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpPbbMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",

                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpPbbMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",

                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpPbbMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",

                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpPbbNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama,
                                Alamat = x.AlamatOp,
                                JenisOP = "-",

                                Wilayah = x.Uptb.ToString() ?? "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var OpBphtbNow = context.DbMonBphtbs.Where(x => x.Tahun == currentYear).ToList();
                        var OpBphtbMines1 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 1).ToList();
                        var OpBphtbMines2 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 2).ToList();
                        var OpBphtbMines3 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 3).ToList();
                        var OpBphtbMines4 = context.DbMonBphtbs.Where(x => x.Tahun == currentYear - 4).ToList();

                        if (tahunHuruf == "TahunMines4")
                        {
                            ret = OpBphtbMines4.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines3")
                        {
                            ret = OpBphtbMines3.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines2")
                        {
                            ret = OpBphtbMines2.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunMines1")
                        {
                            ret = OpBphtbMines1.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        else if (tahunHuruf == "TahunNow")
                        {
                            ret = OpBphtbNow.Select(x => new SeriesMaster()
                            {
                                EnumPajak = (int)JenisPajak,
                                Kategori_Id = (int)112,
                                Kategori_Nama = "BPHTB",
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp,
                                Alamat = x.Alamat,
                                JenisOP = "-",
                                Wilayah = "-"
                            }).ToList();
                        }
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }
                ;

                return ret;

            }


            public static List<JmlObjekPajak> GetJmlObjekPajakData()
            {
                var context = DBClass.GetContext();

                var result = new List<JmlObjekPajak>();

                var tahunList = Enumerable.Range(DateTime.Now.Year - 4, 5).Reverse().ToArray();
                var pajakList = MonPDLib.General.Extension.ToEnumList<EnumFactory.EPajak>();

                foreach (var pajak in pajakList)
                {
                    /* var item = new JmlObjekPajak
                     {
                         EnumPajak = (int)pajak.Value,
                         JenisPajak = pajak.Description,
                     };*/
                    if ((EnumFactory.EPajak)pajak.Value == EnumFactory.EPajak.Semua)
                        continue;

                    var item = new JmlObjekPajak
                    {
                        EnumPajak = (int)pajak.Value,
                        JenisPajak = pajak.Description,
                    };

                    for (int i = 0; i < tahunList.Length; i++)
                    {
                        var year = tahunList[i];
                        var yearBefore = year - 1;

                        int awal = 0, tutup = 0, baru = 0, akhir = 0;

                        switch ((EnumFactory.EPajak)pajak.Value)
                        {
                            case EnumFactory.EPajak.MakananMinuman:
                                awal = context.DbOpRestos.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpRestos.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpRestos.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpRestos.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year) && x.PajakNama != "MAMIN");
                                break;

                            case EnumFactory.EPajak.JasaPerhotelan:
                                awal = context.DbOpHotels.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpHotels.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpHotels.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpHotels.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.JasaKesenianHiburan:
                                awal = context.DbOpHiburans.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpHiburans.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpHiburans.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpHiburans.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.JasaParkir:
                                awal = context.DbOpParkirs.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpParkirs.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpParkirs.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpParkirs.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.TenagaListrik:
                                awal = context.DbOpListriks.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpListriks.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpListriks.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpListriks.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.PBB:
                                awal = context.DbMonPbbs.Where(x => x.TahunBuku == yearBefore).Select(x => x.Nop).Distinct().Count();
                                tutup = context.DbMonPbbs.Where(x => x.TahunBuku == year).Select(x => x.Nop).Distinct().Count();
                                baru = context.DbMonPbbs.Where(x => x.TahunBuku == year).Select(x => x.Nop).Distinct().Count();
                                akhir = context.DbMonPbbs.Where(x => x.TahunBuku == year).Select(x => x.Nop).Distinct().Count();
                                break;

                            case EnumFactory.EPajak.AirTanah:
                                awal = context.DbOpAbts.Count(x => x.TahunBuku == yearBefore && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > yearBefore));
                                tutup = context.DbOpAbts.Count(x => x.TahunBuku == year && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year);
                                baru = context.DbOpAbts.Count(x => x.TahunBuku == year && x.TglMulaiBukaOp.Year == year);
                                akhir = context.DbOpAbts.Count(x => x.TahunBuku == year && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year));
                                break;

                            case EnumFactory.EPajak.BPHTB:
                                awal = context.DbMonBphtbs.Count(x => x.Tahun == yearBefore && x.TglBayar.HasValue && x.TglBayar.Value.Year == yearBefore);
                                tutup = 0;
                                baru = context.DbMonBphtbs.Count(x => x.Tahun == year && x.TglBayar.HasValue && x.TglBayar.Value.Year == year);
                                akhir = awal + baru;
                                break;

                            case EnumFactory.EPajak.Reklame:
                                awal = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year - 1/* && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > (year - 1))*/
                                );

                                tutup = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year/* && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == year*/
                                );

                                baru = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year /*&& x.TglMulaiBukaOp.HasValue && x.TglMulaiBukaOp.Value.Year == year*/
                                );

                                akhir = context.DbOpReklames.Count(
                                    x => x.TahunBuku == year /*&& (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > year)*/
                                );
                                break;

                            case EnumFactory.EPajak.OpsenPkb:
                                awal = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == yearBefore);
                                tutup = 0; // Tidak ada data `TglOpTutup` di contoh pola sebelumnya
                                baru = 0; // Tidak ada data `TglMulaiBukaOp` di contoh pola sebelumnya
                                akhir = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == year);
                                break;

                            case EnumFactory.EPajak.OpsenBbnkb:
                                awal = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == yearBefore);
                                tutup = 0; // tidak ada kolom TglOpTutup di pola
                                baru = 0;  // tidak ada kolom TglMulaiBukaOp di pola
                                akhir = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == year);
                                break;
                        }

                        switch (i)
                        {
                            case 0:
                                item.Tahun1_Awal = awal;
                                item.Tahun1_Tutup = tutup;
                                item.Tahun1_Baru = baru;
                                item.Tahun1_Akhir = akhir;
                                break;
                            case 1:
                                item.Tahun2_Awal = awal;
                                item.Tahun2_Tutup = tutup;
                                item.Tahun2_Baru = baru;
                                item.Tahun2_Akhir = akhir;
                                break;
                            case 2:
                                item.Tahun3_Awal = awal;
                                item.Tahun3_Tutup = tutup;
                                item.Tahun3_Baru = baru;
                                item.Tahun3_Akhir = akhir;
                                break;
                            case 3:
                                item.Tahun4_Awal = awal;
                                item.Tahun4_Tutup = tutup;
                                item.Tahun4_Baru = baru;
                                item.Tahun4_Akhir = akhir;
                                break;
                            case 4:
                                item.Tahun5_Awal = awal;
                                item.Tahun5_Tutup = tutup;
                                item.Tahun5_Baru = baru;
                                item.Tahun5_Akhir = akhir;
                                break;
                        }
                    }

                    result.Add(item);
                }

                return result;
            }
            public static List<DetailJmlOP> GetDetailJmlOPData(EnumFactory.EPajak jenisPajak)
            {
                var context = DBClass.GetContext();
                var ret = new List<DetailJmlOP>();

                var tahunList = Enumerable.Range(DateTime.Now.Year - 4, 5).Reverse().ToArray();

                // Ambil semua kategori untuk pajak ini
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)jenisPajak).OrderBy(x => x.Urutan)
                    .Select(x => new { x.Id, x.Nama })
                    .ToList();

                foreach (var kat in kategoriList)
                {
                    var detail = new DetailJmlOP
                    {
                        EnumPajak = (int)jenisPajak,
                        JenisPajak = jenisPajak.GetDescription(),
                        KategoriId = (int)kat.Id,
                        Kategori = kat.Nama
                    };

                    int i = 1;

                    if (jenisPajak == EnumFactory.EPajak.MakananMinuman)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbOpRestos.Where(x => x.KategoriId == kat.Id);

                            //tutup = query.Count(x =>
                            //    x.TahunBuku == tahun &&
                            //    x.TglOpTutup.HasValue &&
                            //    x.TglOpTutup.Value.Year == tahun);

                            awal = query.Count(x =>
                                x.TahunBuku == tahun - 1 &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun - 1));

                            baru = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglMulaiBukaOp.Year == tahun);

                            akhir = query.Count(x =>
                                x.TahunBuku == tahun &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun) && x.PajakNama != "MAMIN");

                            tutup = (awal + baru) - akhir;

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.JasaPerhotelan)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbOpHotels.Where(x => x.KategoriId == kat.Id);

                            tutup = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglOpTutup.HasValue &&
                                x.TglOpTutup.Value.Year == tahun);

                            awal = query.Count(x =>
                                x.TahunBuku == tahun - 1 &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun - 1));

                            baru = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglMulaiBukaOp.Year == tahun);

                            akhir = query.Count(x =>
                                x.TahunBuku == tahun &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun));

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.JasaKesenianHiburan)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbOpHiburans.Where(x => x.KategoriId == kat.Id);

                            tutup = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglOpTutup.HasValue &&
                                x.TglOpTutup.Value.Year == tahun);

                            awal = query.Count(x =>
                                x.TahunBuku == tahun - 1 &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun - 1));

                            baru = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglMulaiBukaOp.Year == tahun);

                            akhir = query.Count(x =>
                                x.TahunBuku == tahun &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun));

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.JasaParkir)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbOpParkirs.Where(x => x.KategoriId == kat.Id);

                            tutup = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglOpTutup.HasValue &&
                                x.TglOpTutup.Value.Year == tahun);

                            awal = query.Count(x =>
                                x.TahunBuku == tahun - 1 &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun - 1));

                            baru = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglMulaiBukaOp.Year == tahun);

                            akhir = query.Count(x =>
                                x.TahunBuku == tahun &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun));

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.TenagaListrik)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbOpListriks.Where(x => x.KategoriId == kat.Id);

                            tutup = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglOpTutup.HasValue &&
                                x.TglOpTutup.Value.Year == tahun);

                            awal = query.Count(x =>
                                x.TahunBuku == tahun - 1 &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun - 1));

                            baru = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglMulaiBukaOp.Year == tahun);

                            akhir = query.Count(x =>
                                x.TahunBuku == tahun &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun));

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.PBB)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbMonPbbs.Where(x => x.KategoriId == kat.Id);

                            tutup = query.Where(x =>
                                x.TahunBuku == tahun).Select(x => x.Nop).Distinct().Count();

                            awal = query.Where(x =>
                                x.TahunBuku == tahun).Select(x => x.Nop).Distinct().Count();

                            baru = query.Where(x =>
                                x.TahunBuku == tahun).Select(x => x.Nop).Distinct().Count();

                            akhir = query.Where(x =>
                                x.TahunBuku == tahun).Select(x => x.Nop).Distinct().Count();

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.AirTanah)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbOpAbts.Where(x => x.KategoriId == kat.Id);

                            tutup = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglOpTutup.HasValue &&
                                x.TglOpTutup.Value.Year == tahun);

                            awal = query.Count(x =>
                                x.TahunBuku == tahun - 1 &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun - 1));

                            baru = query.Count(x =>
                                x.TahunBuku == tahun &&
                                x.TglMulaiBukaOp.Year == tahun);

                            akhir = query.Count(x =>
                                x.TahunBuku == tahun &&
                                (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun));

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.BPHTB)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbMonBphtbs.Where(x => Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()) == kat.Id);

                            var yearBefore = tahun - 1;
                            awal = query.Count(x => x.Tahun == yearBefore && x.TglBayar.HasValue && x.TglBayar.Value.Year == yearBefore);
                            tutup = 0;
                            baru = query.Count(x => x.Tahun == tahun && x.TglBayar.HasValue && x.TglBayar.Value.Year == tahun);
                            akhir = awal + baru;

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.Reklame)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            var query = context.DbOpReklames.Where(x => x.KategoriId == kat.Id);

                            tutup = query.Count(x =>
                                x.TahunBuku == tahun/* && x.TglOpTutup.HasValue && x.TglOpTutup.Value.Year == tahun*/);

                            awal = query.Count(x =>
                                x.TahunBuku == tahun - 1 /*&& (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun - 1)*/);

                            baru = query.Count(x =>
                                x.TahunBuku == tahun/* && x.TglMulaiBukaOp.Value.Year == tahun*/);

                            akhir = query.Count(x =>
                                x.TahunBuku == tahun/* && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun)*/);

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.OpsenPkb)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            awal = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == 2024);
                            tutup = 0; // Tidak ada data `TglOpTutup` di contoh pola sebelumnya
                            baru = 0; // Tidak ada data `TglMulaiBukaOp` di contoh pola sebelumnya
                            akhir = context.DbMonOpsenPkbs.Count(x => x.TahunPajakSspd == 2025);

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }
                    else if (jenisPajak == EnumFactory.EPajak.OpsenBbnkb)
                    {
                        foreach (var tahun in tahunList)
                        {
                            int awal = 0, tutup = 0, baru = 0, akhir = 0;

                            awal = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == 2024);
                            tutup = 0; // tidak ada kolom TglOpTutup di pola
                            baru = 0;  // tidak ada kolom TglMulaiBukaOp di pola
                            akhir = context.DbMonOpsenBbnkbs.Count(x => x.TahunPajakSspd == 2025);

                            switch (i)
                            {
                                case 1:
                                    detail.Tahun1_Awal = awal;
                                    detail.Tahun1_Tutup = tutup;
                                    detail.Tahun1_Baru = baru;
                                    detail.Tahun1_Akhir = akhir;
                                    break;
                                case 2:
                                    detail.Tahun2_Awal = awal;
                                    detail.Tahun2_Tutup = tutup;
                                    detail.Tahun2_Baru = baru;
                                    detail.Tahun2_Akhir = akhir;
                                    break;
                                case 3:
                                    detail.Tahun3_Awal = awal;
                                    detail.Tahun3_Tutup = tutup;
                                    detail.Tahun3_Baru = baru;
                                    detail.Tahun3_Akhir = akhir;
                                    break;
                                case 4:
                                    detail.Tahun4_Awal = awal;
                                    detail.Tahun4_Tutup = tutup;
                                    detail.Tahun4_Baru = baru;
                                    detail.Tahun4_Akhir = akhir;
                                    break;
                                case 5:
                                    detail.Tahun5_Awal = awal;
                                    detail.Tahun5_Tutup = tutup;
                                    detail.Tahun5_Baru = baru;
                                    detail.Tahun5_Akhir = akhir;
                                    break;
                            }

                            i++;
                        }
                    }

                    // else if untuk pajak lain bisa Anda tambahkan dengan pola yang sama…

                    ret.Add(detail);
                }


                return ret;
            }

            public static List<DetailOP> GetDetailOPAllYears(EnumFactory.EPajak jenisPajak, int kategori, string status, int tahun)
            {
                var context = DBClass.GetContext();
                var result = new List<DetailOP>();

                // hanya untuk tahun tertentu
                var rekap = GetRekapMasterData(jenisPajak, kategori, status, tahun);

                result = rekap.Select(x => new DetailOP
                {
                    EnumPajak = x.EnumPajak,
                    Kategori_Id = x.Kategori_Id,
                    Kategori_Nama = x.Kategori_Nama,
                    NOP = x.NOP,
                    NamaOP = x.NamaOP,
                    Alamat = x.Alamat,
                    JenisOP = x.JenisOP,
                    Wilayah = "SURABAYA " + x.Wilayah
                }).ToList();

                return result;
            }

            #endregion
            public static List<DetailSeries> GetDetailObjekPajak(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var context = DBClass.GetContext();
                var result = new List<DetailSeries>();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        return context.DbOpRestos
                            .Where(x => x.TahunBuku == tahun && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama ?? "-",
                                NOP = x.Nop,
                                NamaOP = x.NamaOp ?? "-",
                                Alamat = x.AlamatOp ?? "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak
                            })
                            .ToList();

                    case EnumFactory.EPajak.JasaPerhotelan:
                        return context.DbOpHotels
                            .Where(x => x.TahunBuku == tahun && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama ?? "-",
                                NOP = x.Nop,
                                NamaOP = x.NamaOp ?? "-",
                                Alamat = x.AlamatOp ?? "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak
                            })
                            .ToList();

                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        return context.DbOpHiburans
                            .Where(x => x.TahunBuku == tahun && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama ?? "-",
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak
                            })
                            .ToList();

                    case EnumFactory.EPajak.JasaParkir:
                        return context.DbOpParkirs
                            .Where(x => x.TahunBuku == tahun && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama ?? "-",
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                JenisOP = "Parkir",
                                Wilayah = x.WilayahPajak
                            })
                            .ToList();

                    case EnumFactory.EPajak.TenagaListrik:
                        return context.DbOpListriks
                            .Where(x => x.TahunBuku == tahun && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama ?? "-",
                                NOP = x.Nop,
                                NamaOP = x.NamaOp ?? "-",
                                Alamat = x.AlamatOp ?? "-",
                                Wilayah = "SURABAYA " + x.WilayahPajak
                            })
                            .ToList();

                    case EnumFactory.EPajak.PBB:
                        return context.DbMonPbbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.WpNama ?? "-",
                                Alamat = x.AlamatWp ?? "-",
                                Wilayah = x.Uptb.ToString() ?? "-"
                            })
                            .Distinct()
                            .ToList();

                    case EnumFactory.EPajak.AirTanah:
                        return context.DbOpAbts
                            .Where(x => x.TahunBuku == tahun && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama,
                                NOP = x.Nop,
                                NamaOP = x.NamaOp,
                                Alamat = x.AlamatOp,
                                Wilayah = "SURABAYA " + x.WilayahPajak
                            })
                            .ToList();

                    case EnumFactory.EPajak.BPHTB:
                        return context.DbMonBphtbs
                            .Where(x => x.Tahun == tahun)
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)Convert.ToInt32("10" + Convert.ToInt32((string.IsNullOrEmpty(x.KdPerolehan) || x.KdPerolehan == "-") ? "0" : x.KdPerolehan).ToString()),
                                Kategori_Nama = x.Perolehan,
                                NOP = x.SpptNop,
                                NamaOP = x.NamaWp ?? "-",
                                Alamat = x.Alamat ?? "-",
                                Wilayah = "-"
                            })
                            .ToList();

                    case EnumFactory.EPajak.Reklame:
                        return context.DbOpReklames
                            .Where(x => x.TahunBuku == tahun && (!x.TglOpTutup.HasValue || x.TglOpTutup.Value.Year > tahun))
                            .Select(x => new DetailSeries
                            {
                                EnumPajak = (int)jenisPajak,
                                Kategori_Id = (int)x.KategoriId,
                                Kategori_Nama = x.KategoriNama ?? "-",
                                NOP = x.Nop,
                                NamaOP = x.Nama ?? "-",
                                Alamat = x.Alamatwp ?? "-",
                                Wilayah = "-"
                            })
                            .ToList();

                        /*case EnumFactory.EPajak.OpsenPkb:
                            return context.DbMonOpsenPkbs
                                .Where(x => x.TahunPajakSspd == tahun)
                                .Select(x => new DetailSeries
                                {
                                    EnumPajak = (int)jenisPajak,
                                    Kategori_Id = 0,
                                    Kategori_Nama = "-",
                                    NOP = x.NoSspd,
                                    NamaOP = x.NamaWp,
                                    Alamat = "-",
                                    JenisOP = "Opsen PKB",
                                    Wilayah = "-"
                                })
                                .ToList();

                        case EnumFactory.EPajak.OpsenBbnkb:
                            return context.DbMonOpsenBbnkbs
                                .Where(x => x.TahunPajakSspd == tahun)
                                .Select(x => new DetailSeries
                                {
                                    EnumPajak = (int)jenisPajak,
                                    Kategori_Id = 0,
                                    Kategori_Nama = "-",
                                    NOP = x.NoSspd,
                                    NamaOP = x.NamaWp,
                                    Alamat = "-",
                                    JenisOP = "Opsen BBNKB",
                                    Wilayah = "-"
                                })
                                .ToList();*/
                }

                return result;
            }

            public static DataDetailOP GetDetailObjekPajak(string nop, EnumFactory.EPajak pajak)
            {
                var context = DBClass.GetContext();
                var ret = new DataDetailOP();
                var tahun = DateTime.Now.Year;
                switch (pajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var realisasiResto = context.DbMonRestos
                            .Where(x => x.Nop == nop && x.TahunPajakKetetapan == tahun && x.TglBayarPokok.HasValue)
                            .Sum(x => x.NominalPokokBayar) ?? 0;

                        var opResto = context.DbOpRestos.FirstOrDefault(x => x.Nop == nop);
                        if (opResto != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opResto.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opResto.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opResto.NpwpdNama;
                            ret.IdentitasPajak.NOP = opResto.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opResto.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opResto.AlamatOp;
                            ret.IdentitasPajak.Telepon = opResto.Telp;
                            ret.IdentitasPajak.TanggalBuka = opResto.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opResto.KategoriNama;
                            ret.IdentitasPajak.kategoriID = (int)opResto.KategoriId;
                            ret.IdentitasPajak.RealisasiTahun = realisasiResto;
                            var wil = context.MWilayahs.Where(x => x.KdKelurahan == opResto.AlamatOpKdLurah && x.KdKecamatan == opResto.AlamatOpKdCamat).FirstOrDefault();
                            ret.IdentitasPajak.Kelurahan = wil.NmKelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = wil.NmKecamatan ?? "";
                            //isi data resto
                            ret.RestoRow.PendapatanRow = new DetailResto.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };
                            ret.RestoRow.SaranaRestoPendukungRow = new DetailResto.SaranaPendukung
                            {
                                JumlahKaryawan = (int)opResto.JumlahKaryawan,
                                MetodePembayaran = opResto.MetodePembayaran,
                                MetodePenjualan = opResto.MetodePenjualan
                            };
                        }
                        break;

                    case EnumFactory.EPajak.TenagaListrik:
                        var realisasiPpj = context.DbMonPpjs
                            .Where(x => x.Nop == nop && x.TahunPajakKetetapan == tahun && x.TglBayarPokok.HasValue)
                            .Sum(x => x.NominalPokokBayar) ?? 0;

                        var opListrik = context.DbOpListriks.FirstOrDefault(x => x.Nop == nop);
                        if (opListrik != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opListrik.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opListrik.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opListrik.NpwpdNama;
                            ret.IdentitasPajak.NOP = opListrik.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opListrik.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opListrik.AlamatOp;
                            ret.IdentitasPajak.Telepon = opListrik.Telp;
                            ret.IdentitasPajak.TanggalBuka = opListrik.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opListrik.KategoriNama;
                            ret.IdentitasPajak.kategoriID = (int)opListrik.KategoriId;
                            ret.IdentitasPajak.RealisasiTahun = realisasiPpj;
                            var wil = context.MWilayahs.Where(x => x.KdKelurahan == opListrik.AlamatOpKdLurah && x.KdKecamatan == opListrik.AlamatOpKdCamat).FirstOrDefault();
                            ret.IdentitasPajak.Kelurahan = wil.NmKelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = wil.NmKecamatan ?? "";
                        }
                        break;

                    case EnumFactory.EPajak.JasaPerhotelan:
                        var realisasiHotel = context.DbMonHotels
                            .Where(x => x.Nop == nop && x.TahunPajakKetetapan == tahun && x.TglBayarPokok.HasValue)
                            .Sum(x => x.NominalPokokBayar) ?? 0;

                        var opHotel = context.DbOpHotels.FirstOrDefault(x => x.Nop == nop);
                        if (opHotel != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opHotel.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opHotel.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opHotel.NpwpdNama;
                            ret.IdentitasPajak.NOP = opHotel.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opHotel.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opHotel.AlamatOp;
                            ret.IdentitasPajak.Telepon = opHotel.Telp;
                            ret.IdentitasPajak.TanggalBuka = opHotel.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opHotel.KategoriNama;
                            ret.IdentitasPajak.kategoriID = (int)opHotel.KategoriId;
                            ret.IdentitasPajak.RealisasiTahun = realisasiHotel;
                            var wil = context.MWilayahs.Where(x => x.KdKelurahan == opHotel.AlamatOpKdLurah && x.KdKecamatan == opHotel.AlamatOpKdCamat).FirstOrDefault();
                            ret.IdentitasPajak.Kelurahan = wil.NmKelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = wil.NmKecamatan ?? "";
                            //isi data hotel
                            ret.HotelRow.PendapatanRow = new DetailHotel.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };
                            ret.HotelRow.SaranaHotelPendukungRow = new DetailHotel.SaranaPendukung
                            {
                                JumlahKaryawan = (int)opHotel.JumlahKaryawan,
                                MetodePembayaran = opHotel.MetodePembayaran,
                                MetodePenjualan = opHotel.MetodePenjualan
                            };

                            var bulanSekarang = DateTime.Now.Month;

                            var semuaBulan = Enumerable.Range(1, 12)
                                .Select(m => new
                                {
                                    Bulan = m,
                                    BulanNama = CultureInfo.GetCultureInfo("id-ID").DateTimeFormat.GetMonthName(m)
                                }).ToList();

                            var accDb = context.DbOpAccHotels
                                .Where(x => x.Nop == nop && x.Bulan.HasValue)
                                .Select(x => new
                                {
                                    Bulan = (int)x.Bulan.Value,
                                    TahunMines1 = x.TahunMin1 ?? 0,
                                    TahunNow = x.TahunIni ?? 0
                                }).ToList();

                            ret.HotelRow.AccHotelDetailList = (from b in semuaBulan
                                                               join d in accDb on b.Bulan equals d.Bulan into gj
                                                               from sub in gj.DefaultIfEmpty()
                                                               select new DetailHotel.AccHotel
                                                               {
                                                                   Bulan = b.Bulan,
                                                                   BulanNama = b.BulanNama,
                                                                   TahunMines1 = sub?.TahunMines1 ?? 0,
                                                                   TahunNow = (b.Bulan <= bulanSekarang) ? (sub?.TahunNow ?? 0) : 0
                                                               })
                                                              .GroupBy(x => x.Bulan)
                                                              .Select(g => g.First())
                                                              .OrderBy(x => x.Bulan)
                                                              .ToList();



                            //ret.HotelRow.BanquetHotelDetailList = context.DbOpBanquets
                            //    .Where(x => x.Nop == nop)
                            //    .Select(x => new DetailHotel.DetailBanquet
                            //    {
                            //        Nama = x.NamaBanquet,
                            //        Jumlah = (int)x.JumlahBanquet,
                            //        JenisBanquet = (int)x.JenisBanquet,
                            //        Kapasitas = (int)x.KapasitasBanquet,
                            //        HargaSewa = (int)x.HargaSewaBanquet,
                            //        HargaPaket = (int)x.HargaPaketBanquet,
                            //        Okupansi = (int)x.OkupansiBanquet
                            //    }).ToList();
                        }
                        break;

                    case EnumFactory.EPajak.JasaParkir:
                        var realisasiParkir = context.DbMonParkirs
                            .Where(x => x.Nop == nop && x.TahunPajakKetetapan == tahun && x.TglBayarPokok.HasValue)
                            .Sum(x => x.NominalPokokBayar) ?? 0;
                        var opParkir = context.DbOpParkirs.FirstOrDefault(x => x.Nop == nop);
                        if (opParkir != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opParkir.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opParkir.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opParkir.NpwpdNama;
                            ret.IdentitasPajak.NOP = opParkir.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opParkir.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opParkir.AlamatOp;
                            ret.IdentitasPajak.Telepon = opParkir.Telp;
                            ret.IdentitasPajak.TanggalBuka = opParkir.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opParkir.KategoriNama;
                            ret.IdentitasPajak.kategoriID = (int)opParkir.KategoriId;
                            ret.IdentitasPajak.RealisasiTahun = realisasiParkir;
                            var wil = context.MWilayahs.Where(x => x.KdKelurahan == opParkir.AlamatOpKdLurah && x.KdKecamatan == opParkir.AlamatOpKdCamat).FirstOrDefault();
                            ret.IdentitasPajak.Kelurahan = wil.NmKelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = wil.NmKecamatan ?? "";
                        }
                        break;

                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var realisasiHiburan = context.DbMonHiburans
                            .Where(x => x.Nop == nop && x.TahunPajakKetetapan == tahun && x.TglBayarPokok.HasValue)
                            .Sum(x => x.NominalPokokBayar) ?? 0;
                        var opHiburan = context.DbOpHiburans.FirstOrDefault(x => x.Nop == nop);
                        if (opHiburan != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opHiburan.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opHiburan.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opHiburan.NpwpdNama;
                            ret.IdentitasPajak.NOP = opHiburan.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opHiburan.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opHiburan.AlamatOp;
                            ret.IdentitasPajak.Telepon = opHiburan.Telp;
                            ret.IdentitasPajak.TanggalBuka = opHiburan.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opHiburan.KategoriNama;
                            ret.IdentitasPajak.kategoriID = (int)opHiburan.KategoriId;
                            ret.IdentitasPajak.RealisasiTahun = realisasiHiburan;
                            var wil = context.MWilayahs.Where(x => x.KdKelurahan == opHiburan.AlamatOpKdLurah && x.KdKecamatan == opHiburan.AlamatOpKdCamat).FirstOrDefault();
                            ret.IdentitasPajak.Kelurahan = wil.NmKelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = wil.NmKecamatan ?? "";
                        }
                        break;

                    case EnumFactory.EPajak.AirTanah:
                        var realisasiAbt = context.DbMonAbts
                            .Where(x => x.Nop == nop && x.TahunPajakKetetapan == tahun && x.TglBayarPokok.HasValue)
                            .Sum(x => x.NominalPokokBayar) ?? 0;
                        var opAbt = context.DbOpAbts.FirstOrDefault(x => x.Nop == nop);
                        if (opAbt != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = opAbt.WilayahPajak ?? "";
                            ret.IdentitasPajak.NpwpdNo = opAbt.Npwpd;
                            ret.IdentitasPajak.NamaNpwpd = opAbt.NpwpdNama;
                            ret.IdentitasPajak.NOP = opAbt.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = opAbt.NamaOp;
                            ret.IdentitasPajak.AlamatLengkap = opAbt.AlamatOp;
                            ret.IdentitasPajak.Telepon = opAbt.Telp;
                            ret.IdentitasPajak.TanggalBuka = opAbt.TglMulaiBukaOp;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opAbt.KategoriNama;
                            ret.IdentitasPajak.kategoriID = opAbt.KategoriId;
                            ret.IdentitasPajak.RealisasiTahun = realisasiAbt;
                            var wil = context.MWilayahs.Where(x => x.KdKelurahan == opAbt.AlamatOpKdLurah && x.KdKecamatan == opAbt.AlamatOpKdCamat).FirstOrDefault();
                            ret.IdentitasPajak.Kelurahan = wil.NmKelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = wil.NmKecamatan ?? "";

                            ret.AbtRow.PendapatanRow = new DetailAbt.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };

                            ret.AbtRow.SaranaAbtPendukungRow = new DetailAbt.SaranaPendukung
                            {
                                KelompokNama = opAbt.NamaKelompok,
                            };

                        }
                        break;

                    case EnumFactory.EPajak.Reklame:
                        var opReklame = context.DbOpReklames.FirstOrDefault(x => x.Nop == nop);
                        if (opReklame != null)
                        {
                            ret.IdentitasPajak.WilayahPajak = "-";
                            ret.IdentitasPajak.NpwpdNo = opReklame.NoWp;
                            ret.IdentitasPajak.NamaNpwpd = opReklame.Nama;
                            ret.IdentitasPajak.NOP = opReklame.Nor;
                            ret.IdentitasPajak.NamaObjekPajak = opReklame.NamaPerusahaan;
                            ret.IdentitasPajak.AlamatLengkap = opReklame.AlamatPerusahaan;
                            ret.IdentitasPajak.Telepon = opReklame.TelpPerusahaan;
                            ret.IdentitasPajak.TglBerlaku = opReklame.TglMulaiBerlaku;
                            ret.IdentitasPajak.TglBerakhir = opReklame.TglAkhirBerlaku;
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opReklame.KategoriNama;
                            ret.IdentitasPajak.kategoriID = (int)opReklame.KategoriId;
                            ret.IdentitasPajak.Kelurahan = opReklame.Nmkelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = opReklame.Kecamatan ?? "";
                            //isi data reklame
                            /*ret.ReklameRow.PendapatanRow = new DetailReklame.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };
                            ret.ReklameRow.SaranaReklamePendukungRow = new DetailReklame.SaranaPendukung
                            {
                                JumlahKaryawan = (int)opReklame.JumlahKaryawan,
                                MetodePembayaran = opReklame.MetodePembayaran,
                                MetodePenjualan = opReklame.MetodePenjualan
                            };*/
                        }
                        break;

                    case EnumFactory.EPajak.PBB:
                        var realisasiPbb = context.DbMonPbbs
                            .Where(x => x.TahunPajak == tahun && x.Nop == nop)
                            .Select(g => g.PokokPajak)
                            .FirstOrDefault();
                        var opPbb = context.DbOpPbbs.FirstOrDefault(x => x.Nop == nop);
                        if (opPbb != null)
                        {
                            ret.IdentitasPajak.WilayahPajak =
                                (opPbb.Uptb != null ? opPbb.Uptb.ToString() : "");
                            ret.IdentitasPajak.NpwpdNo = opPbb.WpNpwp ?? "-";
                            ret.IdentitasPajak.NamaNpwpd = opPbb.WpNama ?? "-";
                            ret.IdentitasPajak.NOP = opPbb.Nop;
                            ret.IdentitasPajak.NamaObjekPajak = "-";
                            ret.IdentitasPajak.AlamatLengkap = opPbb.AlamatOp;
                            ret.IdentitasPajak.Telepon = "-";
                            ret.IdentitasPajak.EnumPajak = pajak;
                            ret.IdentitasPajak.JenisPajak = pajak.GetDescription();
                            ret.IdentitasPajak.KategoriPajak = opPbb.KategoriNama;
                            ret.IdentitasPajak.kategoriID = (int)opPbb.KategoriId;
                            ret.IdentitasPajak.RealisasiTahun = realisasiPbb ?? 0;
                            ret.IdentitasPajak.LuasTanah = opPbb.LuasTanah;
                            var wil = context.MWilayahs.Where(x => x.KdKelurahan == opPbb.AlamatKdLurah && x.KdKecamatan == opPbb.AlamatKdCamat).FirstOrDefault();
                            ret.IdentitasPajak.Kelurahan = wil.NmKelurahan ?? "";
                            ret.IdentitasPajak.Kecamatan = wil.NmKecamatan ?? "";

                            /*ret.AbtRow.PendapatanRow = new DetailAbt.Pendapatan
                            {
                                //isi data pendapatan jika ada
                            };

                            ret.AbtRow.SaranaAbtPendukungRow = new DetailAbt.SaranaPendukung
                            {
                                KelompokNama = opPbb.NamaKelompok,
                            };*/

                        }
                        break;

                    case EnumFactory.EPajak.BPHTB:
                        // var opBphtb = context.DbOpBphtbs.FirstOrDefault(x => x.Nop == nop);
                        break;

                    case EnumFactory.EPajak.OpsenPkb:
                        // var opOpsenPkb = context.DbOpOpsenPkb.FirstOrDefault(x => x.Nop == nop);
                        break;

                    case EnumFactory.EPajak.OpsenBbnkb:
                        // var opOpsenBbnkb = context.DbOpOpsenBbnkb.FirstOrDefault(x => x.Nop == nop);
                        break;

                    default:
                        break;
                }

                return ret;
            }

            public static List<TPKHotel> GetTPKHotelData(int tahun)
            {
                var context = DBClass.GetContext();

                var data = context.DataTpkHotels
                    .Where(x => x.Tahun == tahun)
                    .Select(g => new TPKHotel
                    {
                        Tahun = g.Tahun ?? 0,
                        Bulan = g.Bulan ?? 0,
                        BulanNama = CultureInfo
                            .GetCultureInfo("id-ID")
                            .DateTimeFormat
                            .GetMonthName(g.Bulan ?? 0),
                        HotelBintang = g.HotelBintang ?? 0,
                        HotelNonBintang = g.HotelNonBintang ?? 0
                    })
                    .ToList();

                return data; // langsung kembalikan hasil query
            }
            public static DashboardTPK GetDashboardTPK2025()
            {
                var data2025 = GetTPKHotelData(2025);

                var dashboard = new DashboardTPK
                {
                    rataBintang = data2025.Any() ? Math.Round(data2025.Average(x => x.HotelBintang), 2) : 0,
                    rataNonBintang = data2025.Any() ? Math.Round(data2025.Average(x => x.HotelNonBintang), 2) : 0
                };

                return dashboard;
            }

            public static List<TPKHotel> GenerateDummyTPKHotelData(int tahun)
            {
                var dummyData = new List<TPKHotel>
                {
                    new TPKHotel
                    {
                        EnumPajak = 1,
                        JenisPajak = "Hotel",
                        Tahun = 2025,
                        Bulan = 1,
                        BulanNama = "Januari",
                        HotelBintang = 12000000m,
                        HotelNonBintang = 5000000m
                    },
                    new TPKHotel
                    {
                        EnumPajak = 1,
                        JenisPajak = "Hotel",
                        Tahun = 2025,
                        Bulan = 2,
                        BulanNama = "Februari",
                        HotelBintang = 11500000m,
                        HotelNonBintang = 4800000m
                    },
                    new TPKHotel
                    {
                        EnumPajak = 1,
                        JenisPajak = "Hotel",
                        Tahun = 2025,
                        Bulan = 3,
                        BulanNama = "Maret",
                        HotelBintang = 13000000m,
                        HotelNonBintang = 5300000m
                    },
                    // Tambahkan data bulan lainnya jika perlu
                };

                return dummyData;
            }

            public static List<OkupansiHotel> GetOkupansiHotel(int tahunBuku)
            {
                var result = new List<OkupansiHotel>();
                var context = DBClass.GetContext();

                var query = context.DbOpHotelFixes
                    .Where(x => x.TahunBuku == tahunBuku)
                    .Select(x => new { x.Nop, x.KategoriId })
                    .Distinct();

                var potensiHotel = context.DbPotensiHotels
                    .Where(x => x.TahunBuku == tahunBuku)
                    .Select(x => new { x.Nop, x.TotalRoom, x.AvgRoomSold })
                    .ToList();

                var kategoriHotels = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).OrderBy(x => x.Urutan)
                    .ToList();

                foreach (var kategori in kategoriHotels)
                {
                    var nopPerKategori = query.Where(x => x.KategoriId == kategori.Id).Select(x => x.Nop).ToList();
                    var getPotensi = potensiHotel.Where(x => nopPerKategori.Contains(x.Nop)).ToList();

                    int totalKamar = getPotensi.Sum(q => (int?)q.TotalRoom) ?? 0;
                    decimal roomSold = getPotensi.Sum(q => q.AvgRoomSold) ?? 0;
                    decimal avgRate = totalKamar > 0 ? Math.Round((roomSold / totalKamar) * 100m, 2, MidpointRounding.AwayFromZero) : 0m;
                    //decimal avgRate = Math.Round((roomSold / totalKamar), 2);

                    var res = new OkupansiHotel();
                    res.KategoriId = Convert.ToInt32(kategori.Id);
                    res.KategoriNama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kategori.Nama.ToLower());
                    res.TotalKamar = totalKamar;
                    res.AvgRate = avgRate;
                    res.Tahun = tahunBuku;
                    res.EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan;

                    result.Add(res);
                }

                return result;
            }
            public static List<DetailOkupansiHotel> GetRekapDetailHotel(EnumFactory.EPajak jenisPajak, int kategori, int tahun)
            {
                var context = DBClass.GetContext();
                var result = new List<DetailOkupansiHotel>();

                // ambil OP hotel per tahun & kategori
                var dataOpHotels = context.DbOpHotelFixes
                    .Where(x => x.TahunBuku == tahun && x.KategoriId == kategori && x.KategoriId > 0)
                    .Select(x => new
                    {
                        x.Nop,
                        x.NamaOp,
                        x.AlamatOp,
                        x.WilayahPajak,
                        x.KategoriId,
                        x.KategoriNama
                    })
                    .Distinct();

                // ambil potensi hotel (room, room sold) per tahun
                var potensiHotels = context.DbPotensiHotels
                    .Where(x => x.TahunBuku == tahun)
                    .Select(x => new
                    {
                        x.Nop,
                        x.TotalRoom,
                        x.AvgRoomSold
                    })
                    .ToList();

                foreach (var op in dataOpHotels.OrderByDescending(o => o.KategoriId).ThenByDescending(o => o.KategoriNama))
                {
                    var potensi = potensiHotels.FirstOrDefault(p => p.Nop == op.Nop);

                    var totalRoom = potensi?.TotalRoom ?? 0;
                    var roomSold = potensi?.AvgRoomSold ?? 0m;
                    var rateOkupansi = totalRoom > 0 ? Math.Round((roomSold / totalRoom) * 100m, 2, MidpointRounding.AwayFromZero) : 0m;

                    result.Add(new DetailOkupansiHotel
                    {
                        EnumPajak = (int)jenisPajak,
                        NOP = op.Nop,
                        NamaOP = op.NamaOp,
                        AlamatOP = op.AlamatOp,
                        Wilayah = "SURABAYA " + op.WilayahPajak ?? "-",
                        KategoriId = op.KategoriId.HasValue ? Convert.ToInt32(op.KategoriId.Value) : 0,
                        KategoriNama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(op.KategoriNama.ToLower()),
                        TotalRoom = totalRoom.ToString(),
                        RoomTerjual = roomSold.ToString(),
                        RateOkupansi = rateOkupansi.ToString("F2")
                    });
                }
                return result;
            }
        }

        public class OkupansiHotel
        {
            public int EnumPajak { get; set; }
            public int Tahun { get; set; }
            public int KategoriId { get; set; }
            public string KategoriNama { get; set; }
            public int TotalKamar { get; set; }
            public decimal AvgRate { get; set; }
        }

        public class DetailOkupansiHotel
        {
            public int EnumPajak { get; set; }
            public int KategoriId { get; set; }
            public string KategoriNama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
            public string NamaOP { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string TotalRoom { get; set; } = null!;
            public string RoomTerjual { get; set; } = null;
            public string RateOkupansi { get; set; } = null;
            public string Wilayah { get; set; } = null!;
        }
        public class RekapOP
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }
            public int JmlOpAwal { get; set; }
            public int JmlOpTutupPermanen { get; set; }
            public int JmlOpBaru { get; set; }
            public int JmlOpAkhir { get; set; }
            public List<RekapDetailOP> RekapDetail { get; set; } = new();
            public List<OkupansiHotel> OkunpasiHotel { get; set; } = new();
            public string Uptb { get; set; } = null!;
            public string KdKecamatan { get; set; }
            public string NmKecamatan { get; set; }
            public string KdKelurahan { get; set; }
            public string NmKelurahan { get; set; }
        }

        public class RekapDetailOP
        {
            public int KategoriId { get; set; }
            public string KategoriNama { get; set; }
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }
            public int JmlOpAwal { get; set; }
            public int JmlOpTutupPermanen { get; set; }
            public int JmlOpBaru { get; set; }
            public int JmlOpAkhir { get; set; }
            public string Uptb { get; set; } = null!;
            public string KdKecamatan { get; set; }
            public string KdKelurahan { get; set; }
        }
        public class RekapOPTotal
        {
            public int TotalOpAwal { get; set; }
            public int TotalOpTutup { get; set; }
            public int TotalOpBaru { get; set; }
            public int TotalOpAkhir { get; set; }
        }

        public class RekapOkupansiTotal
        {
            public int TotalRoom { get; set; }
            public decimal AvgRoomSold { get; set; }
        }

        public class SeriesOP
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun2021 { get; set; }
            public int Tahun2022 { get; set; }
            public int Tahun2023 { get; set; }
            public int Tahun2024 { get; set; }
            public int Tahun2025 { get; set; }
        }

        // Di dalam ProfileOPVM.cs
        public class SeriesOPStatistik
        {
            public decimal TotalOpTahunIni { get; set; }
            public decimal PertumbuhanOp { get; set; }
            public decimal KenaikanTertinggiOp { get; set; }
            public string PeriodeKenaikanTertinggi { get; set; } = "-";
        }

        public class Dashboard
        {
            public int TotalPenghimbauan { get; set; }
            public double PenghimbauanAktif { get; set; }
            public double PenghimbauanSelesai { get; set; }
            public double TingkatKepatuhan { get; set; }

        }

        public class DataDetailOP
        {
            public IdentitasObjekPajak IdentitasPajak { get; set; } = new();
            public DataPerizinan Perizinan { get; set; } = new();
            public DetailHotel HotelRow { get; set; } = new();
            public DetailResto RestoRow { get; set; } = new();
            public DetailAbt AbtRow { get; set; } = new();
            public DetailPbb PbbRow { get; set; } = new();

        }

        public class HotelDetail
        {

        }
        public class RekapDetail
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int KategoriId { get; set; }
            public int Urutan { get; set; }
            public string Kategori { get; set; } = null!;
            public int Tahun { get; set; }
            public int JmlOpAwal { get; set; }
            public int JmlOpTutupPermanen { get; set; }
            public int JmlOpBaru { get; set; }
            public int JmlOpAkhir { get; set; }
        }

        public class JmlObjekPajak
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; }
            public decimal Tahun1_Awal { get; set; }
            public decimal Tahun1_Tutup { get; set; }
            public decimal Tahun1_Baru { get; set; }
            public decimal Tahun1_Akhir { get; set; }

            public decimal Tahun2_Awal { get; set; }
            public decimal Tahun2_Tutup { get; set; }
            public decimal Tahun2_Baru { get; set; }
            public decimal Tahun2_Akhir { get; set; }

            public decimal Tahun3_Awal { get; set; }
            public decimal Tahun3_Tutup { get; set; }
            public decimal Tahun3_Baru { get; set; }
            public decimal Tahun3_Akhir { get; set; }

            public decimal Tahun4_Awal { get; set; }
            public decimal Tahun4_Tutup { get; set; }
            public decimal Tahun4_Baru { get; set; }
            public decimal Tahun4_Akhir { get; set; }

            public decimal Tahun5_Awal { get; set; }
            public decimal Tahun5_Tutup { get; set; }
            public decimal Tahun5_Baru { get; set; }
            public decimal Tahun5_Akhir { get; set; }
        }

        public class DetailJmlOP
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int KategoriId { get; set; }
            public string Kategori { get; set; } = null!;
            public decimal Tahun1_Awal { get; set; }
            public decimal Tahun1_Tutup { get; set; }
            public decimal Tahun1_Baru { get; set; }
            public decimal Tahun1_Akhir { get; set; }

            public decimal Tahun2_Awal { get; set; }
            public decimal Tahun2_Tutup { get; set; }
            public decimal Tahun2_Baru { get; set; }
            public decimal Tahun2_Akhir { get; set; }

            public decimal Tahun3_Awal { get; set; }
            public decimal Tahun3_Tutup { get; set; }
            public decimal Tahun3_Baru { get; set; }
            public decimal Tahun3_Akhir { get; set; }

            public decimal Tahun4_Awal { get; set; }
            public decimal Tahun4_Tutup { get; set; }
            public decimal Tahun4_Baru { get; set; }
            public decimal Tahun4_Akhir { get; set; }

            public decimal Tahun5_Awal { get; set; }
            public decimal Tahun5_Tutup { get; set; }
            public decimal Tahun5_Baru { get; set; }
            public decimal Tahun5_Akhir { get; set; }
        }
        public class DetailOP
        {
            public int EnumPajak { get; set; }
            public int Kategori_Id { get; set; }
            public string Kategori_Nama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP
            {
                get
                {
                    return EnumPajak == 7 || EnumPajak == 9 || EnumPajak == 12
                        ? NOP
                        : Utility.GetFormattedNOP(NOP);
                }
            }
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOP { get; set; } = null!;
            public string Wilayah { get; set; } = "-";
            public string Kecamatan { get; set; } = "-";
            public string Kelurahan { get; set; } = "-";
            public string Status { get; set; } = null!;

        }


        public class RekapMaster
        {
            public int EnumPajak { get; set; }
            public int Kategori_Id { get; set; }
            public string Kategori_Nama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP
            {
                get
                {
                    return EnumPajak == 7 || EnumPajak == 9 || EnumPajak == 12
                        ? NOP
                        : Utility.GetFormattedNOP(NOP);
                }
            }
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOP { get; set; } = null!;
            public string Wilayah { get; set; } = null!;

        }

        public class SeriesDetail
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int KategoriId { get; set; }
            public string Kategori { get; set; } = null!;
            public int TahunMines4 { get; set; }
            public int TahunMines3 { get; set; }
            public int TahunMines2 { get; set; }
            public int TahunMines1 { get; set; }
            public int TahunNow { get; set; }
        }

        public class SeriesMaster
        {
            public int EnumPajak { get; set; }
            public int Kategori_Id { get; set; }
            public string Kategori_Nama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP
            {
                get
                {
                    return EnumPajak == 7 || EnumPajak == 9 || EnumPajak == 12
                        ? NOP
                        : Utility.GetFormattedNOP(NOP);
                }
            }
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOP { get; set; } = null!;
            public string Wilayah { get; set; } = null!;

        }

        public class DetailSeries
        {
            public int EnumPajak { get; set; }
            public int Kategori_Id { get; set; }
            public string Kategori_Nama { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public string FormattedNOP
            {
                get
                {
                    return EnumPajak == 7 || EnumPajak == 9 || EnumPajak == 12
                        ? NOP
                        : Utility.GetFormattedNOP(NOP);
                }
            }
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisOP { get; set; } = null!;
            public string Wilayah { get; set; } = null!;
        }

        public class IdentitasObjekPajak
        {
            public string NpwpdNo { get; set; }
            public string NamaNpwpd { get; set; }
            public string NamaObjekPajak { get; set; }
            public string AlamatLengkap { get; set; }
            public string Kelurahan { get; set; }
            public string Kecamatan { get; set; }
            public string WilayahPajak { get; set; }
            public string NOP { get; set; }
            public string FormattedNOP
            {
                get
                {
                    return (int)EnumPajak == 7 || (int)EnumPajak == 12
                        ? NOP : (int)EnumPajak == 9 ? Utility.GetFormattedNOPPBB(NOP)
                        : Utility.GetFormattedNOP(NOP);
                }
            }
            public string Telepon { get; set; }
            public DateTime TanggalBuka { get; set; }
            public EnumFactory.EPajak EnumPajak { get; set; }
            public string JenisPajak { get; set; }
            public string KategoriPajak { get; set; }
            public DateTime? TglBerlaku { get; set; }
            public DateTime? TglBerakhir { get; set; }
            public string NoPerusahaan { get; set; }
            public decimal kategoriID { get; set; }
            public decimal RealisasiTahun { get; set; }
            public decimal LuasTanah { get; set; }

        }
        public class DataPerizinan
        {
            public string NomorIMB { get; set; }
            public DateTime TanggalIMB { get; set; }
            public string NomorSITU_NIB { get; set; }
            public string NomorIzinOperasional { get; set; }
        }
        //DETAIL OP HOTEL
        public class DetailHotel
        {
            public Pendapatan PendapatanRow { get; set; } = new();
            public SaranaPendukung SaranaHotelPendukungRow { get; set; } = new();
            public List<DetailBanquet> BanquetHotelDetailList { get; set; } = new();
            public List<DetailFasilitas> FasilitasHotelDetailList { get; set; } = new();
            public List<DetailKamar> KamarHotelDetailList { get; set; } = new();
            public List<AccHotel> AccHotelDetailList { get; set; } = new();

            public class Pendapatan
            {
                public string Okupansi { get; set; }
                public decimal RataTarifKamar { get; set; }
                public decimal PendapatanKotor { get; set; }
                public string JumlahTransaksi { get; set; }
            }
            public class SaranaPendukung
            {
                public int JumlahKaryawan { get; set; }
                public string MetodePembayaran { get; set; }
                public string MetodePenjualan { get; set; }
            }
            public class DetailBanquet
            {
                public string Nama { get; set; }
                public int Jumlah { get; set; }
                public int JenisBanquet { get; set; }
                public int Kapasitas { get; set; }
                public int HargaSewa { get; set; }
                public int HargaPaket { get; set; }
                public int Okupansi { get; set; }
            }
            public class DetailFasilitas
            {
                public string Nama { get; set; }
                public int Jumlah { get; set; }
                public int Kapasitas { get; set; }
            }
            public class DetailKamar
            {
                public string Kamar { get; set; }
                public int Jumlah { get; set; }
                public int Tarif { get; set; }
            }

            public class AccHotel
            {
                public string BulanNama { get; set; } = null!;
                public decimal Bulan { get; set; }
                public decimal TahunMines1 { get; set; }
                public decimal TahunNow { get; set; }
            }
        }
        //DETAIL OP RESTO
        public class DetailResto
        {
            public Pendapatan PendapatanRow { get; set; } = new();
            public SaranaPendukung SaranaRestoPendukungRow { get; set; } = new();
            public List<DetailOperasional> OperasionalRestoDetailList { get; set; } = new();
            public List<DetailFasilitas> FasilitasRestoDetailList { get; set; } = new();
            public DetailKapasitas KapasitasRestoDetailRow { get; set; } = new();

            public class Pendapatan
            {

            }
            public class SaranaPendukung
            {
                public int JumlahKaryawan { get; set; }
                public string MetodePembayaran { get; set; }
                public string MetodePenjualan { get; set; }
            }
            public class DetailOperasional
            {
                public string Hari { get; set; }
                public DateTime JamBuka { get; set; }
                public DateTime JamTutup { get; set; }

            }
            public class DetailFasilitas
            {
                public string Nama { get; set; }
                public int Jumlah { get; set; }
                public int Kapasitas { get; set; }
            }
            public class DetailKapasitas
            {
                public int JumlahKursi { get; set; }
                public int JumlahMeja { get; set; }
                public int KapasitasRuangan { get; set; }
            }
        }
        //DETAIL ABT
        public class DetailAbt
        {
            public Pendapatan PendapatanRow { get; set; } = new();
            public SaranaPendukung SaranaAbtPendukungRow { get; set; } = new();

            public class Pendapatan
            {

            }
            public class SaranaPendukung
            {
                public string KelompokNama { get; set; }
            }
        }

        public class DetailPbb
        {
            public SaranaPendukung SaranaPbbPendukungRow { get; set; } = new();
            public class SaranaPendukung
            {
                public string KelompokNama { get; set; }
            }
        }

        public class TPKHotel
        {
            public int EnumPajak { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }
            public int Bulan { get; set; }
            public string BulanNama { get; set; } = null!;
            public decimal HotelBintang { get; set; }
            public decimal HotelNonBintang { get; set; }
            public decimal RataRata => (HotelBintang + HotelNonBintang) / 2m;
        }

        public class DashboardTPK
        {
            public decimal rataBintang { get; set; }
            public decimal rataNonBintang { get; set; }
        }
        public class uptbView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class kecamatanView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class kelurahanView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
    }
}
