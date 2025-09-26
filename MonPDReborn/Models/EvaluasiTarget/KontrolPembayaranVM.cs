using DevExpress.CodeParser;
using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Models.AktivitasOP.PemasanganAlatVM;
using static MonPDReborn.Models.DashboardVM.ViewModel;
using static MonPDReborn.Models.EvaluasiTarget.KontrolPembayaranVM;
using static MonPDReborn.Models.EvaluasiTarget.KontrolPembayaranVM.Method;

namespace MonPDReborn.Models.EvaluasiTarget
{
    public class KontrolPembayaranVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public List<SelectListItem> JenisUptbList { get; set; } = new();
            public int SelectedUPTB { get; set; }

            public Index()
            {
                JenisUptbList = Enum.GetValues(typeof(EnumFactory.EUPTB))
                    .Cast<EnumFactory.EUPTB>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
            }
        }


        public class Show
        {
            public int Tahun { get; set; }
            public EPajak Pajak { get; set; }
            public List<KontrolPembayaran> Data { get; set; }

            // Khusus untuk Hotel
            public List<KontrolPembayaran> DataHotelBintang { get; set; }
            public List<KontrolPembayaran> DataHotelNonBintang { get; set; }

            public Show(int tahun, EPajak pajak, EUPTB uptb)
            {
                Tahun = tahun;
                Pajak = pajak;

                if (pajak == EPajak.JasaPerhotelan)
                {
                    DataHotelBintang = Method.GetKontrolPembayaranHotelRekap(tahun, uptb);
                    DataHotelNonBintang = Method.GetKontrolPembayaranHotelRekap(tahun, uptb, true);
                }
                else
                {
                    Data = AmbilData(pajak, tahun, uptb);
                }
            }

            private List<KontrolPembayaran> AmbilData(EPajak pajak, int tahun, EnumFactory.EUPTB uptb)
            {
                return pajak switch
                {
                    EPajak.MakananMinuman => Method.GetKontrolPembayaranRestoRekap(tahun, uptb),
                    EPajak.JasaParkir => Method.GetKontrolPembayaranParkirRekap(tahun, uptb),
                    EPajak.JasaKesenianHiburan => Method.GetKontrolPembayaranHiburanRekap(tahun, uptb),
                    EPajak.TenagaListrik => Method.GetKontrolPembayaranPPJRekap(tahun, uptb),
                    EPajak.Reklame => Method.GetKontrolPembayaranReklameRekap(tahun),
                    EPajak.AirTanah => Method.GetKontrolPembayaranABTRekap(tahun, uptb),
                    EPajak.PBB => Method.GetKontrolPembayaranPBBRekap(tahun, uptb),
                    EPajak.BPHTB => Method.GetKontrolPembayaranBphtbRekap(tahun),
                    _ => new List<KontrolPembayaran>()
                };
            }
        }


        public class ShowPotensi
        {
            public int Tahun { get; set; }
            public EPajak Pajak { get; set; }
            public List<Potensi> Data { get; set; }

            public List<Potensi> DataHotelBintang { get; set; }
            public List<Potensi> DataHotelNonBintang { get; set; }
            public ShowPotensi(int tahun, EPajak pajak, EnumFactory.EUPTB uptb)
            {
                Tahun = tahun;
                Pajak = pajak;
                if (pajak == EPajak.JasaPerhotelan)
                {
                    DataHotelBintang = Method.GetPotensiPajakHotelRekap(tahun, uptb);
                    DataHotelNonBintang = Method.GetPotensiPajakHotelRekap(tahun, uptb, true);
                }
                else
                {
                    Data = AmbilData(pajak, tahun, uptb);
                }
            }
            private List<Potensi> AmbilData(EPajak pajak, int tahun, EnumFactory.EUPTB uptb)
            {
                return pajak switch
                {
                    EPajak.MakananMinuman => Method.GetPotensiPajakRestoRekap(tahun, uptb),
                    EPajak.JasaParkir => Method.GetPotensiPajakParkirRekap(tahun, uptb),
                    EPajak.JasaKesenianHiburan => Method.GetPotensiPajakHiburanRekap(tahun, uptb),
                    EPajak.Reklame => Method.GetPotensiPajakReklameRekap(tahun),
                    EPajak.TenagaListrik => Method.GetPotensiPajakPPJRekap(tahun, uptb),
                    EPajak.AirTanah => Method.GetPotensiPajakAirTanahRekap(tahun, uptb),
                    EPajak.PBB => Method.GetPotensiPajakPbbRekap(tahun, uptb),
                    EPajak.BPHTB => Method.GetPotensiPajakBphtbRekap(tahun),
                    _ => new List<Potensi>()
                };
            }
        }
        public class DetailPembayaran
        {
            public bool IsTotal { get; set; }
            public bool IsHotelNonBintang { get; set; }
            public int Bulan { get; set; }
            public List<DetailPajak> Data { get; set; }


            public DetailPembayaran(EnumFactory.EPajak jenisPajak, int kategoriId, int tahun, int bulan, int status, EnumFactory.EUPTB uptb)
            {
                Bulan = bulan;
                Data = Method.GetDetailKontrolPembayaranList(jenisPajak, kategoriId, tahun, bulan, status, uptb);
            }
            // Constructor untuk summary/total (new) - dengan parameter hotel type
            public DetailPembayaran(EnumFactory.EPajak jenisPajak, int tahun, int bulan, int status, EnumFactory.EUPTB uptb, bool isTotal = true, bool isHotelNonBintang = false)
            {

                Bulan = bulan;
                IsTotal = isTotal;
                IsHotelNonBintang = isHotelNonBintang;
                Data = Method.GetDetailKontrolPembayaranList(jenisPajak, 0, tahun, bulan, status, uptb, isTotal, isHotelNonBintang);

            }
        }

        public class DetailPotensiPajak
        {
            public bool IsTotal { get; set; }
            public bool IsHotelNonBintang { get; set; }
            public int Bulan { get; set; }
            public List<DetailPotensi> Data { get; set; }

            // Constructor untuk detail kategori (existing)
            public DetailPotensiPajak(EnumFactory.EPajak jenisPajak, int kategoriId, int tahun, int bulan, EnumFactory.EUPTB uptb)
            {
                Bulan = bulan;
                Data = Method.GetDetailPotensiPajakList(jenisPajak, kategoriId, tahun, bulan, uptb);
            }

            // Constructor untuk summary/total (new) - dengan parameter hotel type
            public DetailPotensiPajak(EnumFactory.EPajak jenisPajak, int tahun, int bulan, EnumFactory.EUPTB uptb, bool isTotal = true, bool isHotelNonBintang = false)
            {
                Bulan = bulan;
                IsTotal = isTotal;
                IsHotelNonBintang = isHotelNonBintang;
                Data = Method.GetDetailPotensiPajakList(jenisPajak, 0, tahun, bulan, uptb, isTotal, isHotelNonBintang);
            }
        }

        public class ShowUpaya
        {
            public int Tahun { get; set; }
            public EPajak Pajak { get; set; }
            public List<UpayaPajak> Data { get; set; }
            public List<UpayaPajak> DataHotelBintang { get; set; }
            public List<UpayaPajak> DataHotelNonBintang { get; set; }
            public ShowUpaya(int tahun, EPajak pajak)
            {
                Tahun = tahun;
                Pajak = pajak;
                if (pajak == EPajak.JasaPerhotelan)
                {
                    DataHotelBintang = Method.GetUpayaPajakHotelRekap(tahun);
                    DataHotelNonBintang = Method.GetUpayaPajakHotelRekap(tahun, true);
                }
                else
                {
                    Data = AmbilData(pajak, tahun);
                }
            }

            private List<UpayaPajak> AmbilData(EPajak pajak, int tahun)
            {
                return pajak switch
                {
                    EPajak.MakananMinuman => Method.GetUpayaPajakRestoRekap(tahun),
                    EPajak.JasaParkir => Method.GetUpayaPajakParkirRekap(tahun),
                    EPajak.JasaKesenianHiburan => Method.GetUpayaPajakHiburanRekap(tahun),
                    EPajak.Reklame => Method.GetUpayaPajakReklameRekap(tahun),
                    EPajak.TenagaListrik => Method.GetUpayaPajakPPJRekap(tahun),
                    EPajak.AirTanah => Method.GetUpayaPajakABTRekap(tahun),
                    EPajak.PBB => Method.GetUpayaPajakPBBRekap(tahun),
                    EPajak.BPHTB => Method.GetUpayaPajakBphtbRekap(tahun),
                    _ => new List<UpayaPajak>()
                };
            }
        }

        public class UpayaDetail
        {
            public List<DetailUpaya> Data { get; set; }
            public UpayaDetail(EnumFactory.EPajak jenisPajak, int kategoriId, int tahun, int bulan, int status)
            {
                Data = Method.GetDetailUpayaPajakList(jenisPajak, kategoriId, tahun, bulan, status);
            }
        }



        public class Detail
        {
            public Detail()
            {

            }
            public Detail(string nop)
            {
            }
        }
        public class Method
        {
            #region Get Rekap Kontrol Pembayaran
            //HOTEL
            public static List<KontrolPembayaran> GetKontrolPembayaranHotelRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                        .Where(x => x.Tahun == tahun && x.WilayahPajak == ((int)uptb).ToString())
                        .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                        .Select(g => new
                        {
                            KategoriId = g.Key.KategoriId,
                            Tahun = g.Key.Tahun,
                            Bulan = g.Key.Bulan,
                            Jml = g.Count(),
                            JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                            JmlBayar = g.Count(x => x.StatusBayar == 1),
                            JmlNTs = g.Count(x => x.StatusBayar == 2),
                            //Ketetapan = g.Sum(x => x.Ketetapan),
                            //Realisasi = g.Sum(x => x.Realisasi),
                        })
                        .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                        .Where(x => x.Tahun == tahun)
                        .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                        .Select(g => new
                        {
                            KategoriId = g.Key.KategoriId,
                            Tahun = g.Key.Tahun,
                            Bulan = g.Key.Bulan,
                            Jml = g.Count(),
                            JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                            JmlBayar = g.Count(x => x.StatusBayar == 1),
                            JmlNTs = g.Count(x => x.StatusBayar == 2),
                            //Ketetapan = g.Sum(x => x.Ketetapan),
                            //Realisasi = g.Sum(x => x.Realisasi),
                        })
                        .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                return ret;
            }
            public static List<KontrolPembayaran> GetKontrolPembayaranHotelRekap(int tahun, EnumFactory.EUPTB uptb, bool turu)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                    .Where(x => x.Tahun == tahun && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id == 17))
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                    .Where(x => x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id == 17))
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }

                return ret;
            }
            //RESTO
            public static List<KontrolPembayaran> GetKontrolPembayaranRestoRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var nopList = context.DbOpRestos
                    .Where(x => x.TahunBuku == tahun && x.PajakNama != "MAMIN" && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > tahun))
                    .Select(x => x.Nop)
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrRestos
                    .Where(x => x.Tahun == tahun && x.WilayahPajak == ((int)uptb).ToString() && nopList.Contains(x.Nop))
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar.Value == 0),
                        JmlBayar = g.Count(x => x.StatusBayar.Value == 1),
                        JmlNTs = g.Count(x => x.StatusBayar.Value == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();


                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrRestos
                    .Where(x => x.Tahun == tahun && nopList.Contains(x.Nop))
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar.Value == 0),
                        JmlBayar = g.Count(x => x.StatusBayar.Value == 1),
                        JmlNTs = g.Count(x => x.StatusBayar.Value == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();


                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }

                return ret;
            }
            //PARKIR
            public static List<KontrolPembayaran> GetKontrolPembayaranParkirRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaParkir).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrParkirs
                    .Where(x => x.Tahun == tahun && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrParkirs
                    .Where(x => x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }

                return ret;
            }
            //HIBURAN
            public static List<KontrolPembayaran> GetKontrolPembayaranHiburanRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrHiburans
                    .Where(x => x.Tahun == tahun && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrHiburans
                    .Where(x => x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }

                return ret;
            }
            //PPJ
            public static List<KontrolPembayaran> GetKontrolPembayaranPPJRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrPpjs
                    .Where(x => x.Tahun == tahun && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrPpjs
                    .Where(x => x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }

                return ret;
            }
            //ABT
            public static List<KontrolPembayaran> GetKontrolPembayaranABTRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.AirTanah).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrAbts
                    .Where(x => x.Tahun == tahun && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrAbts
                    .Where(x => x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription();

                        re.OPbuka1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);
                        re.Nts1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Jml);
                        re.Blm2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);
                        re.Nts2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Jml);
                        re.Blm3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);
                        re.Nts3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Jml);
                        re.Blm4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);
                        re.Nts4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Jml);
                        re.Blm5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);
                        re.Nts5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Jml);
                        re.Blm6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);
                        re.Nts6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Jml);
                        re.Blm7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);
                        re.Nts7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Jml);
                        re.Blm8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);
                        re.Nts8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Jml);
                        re.Blm9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);
                        re.Nts9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Jml);
                        re.Blm10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);
                        re.Nts10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Jml);
                        re.Blm11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);
                        re.Nts11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlNTs);

                        re.OPbuka12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Jml);
                        re.Blm12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);
                        re.Nts12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlNTs);

                        ret.Add(re);
                    }
                }

                return ret;
            }
            //PBB
            public static List<KontrolPembayaran> GetKontrolPembayaranPBBRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.PBB).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranAwalList = context.DbCtrlByrPbbs
                   .Where(x => x.Tahun == tahun && x.WilayahPajak == (decimal)uptb)
                   .GroupBy(x => new { x.KategoriId })
                   .Select(g => new
                   {
                       KategoriId = g.Key.KategoriId,
                       Jml = g.Count(),
                       JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                       JmlNTs = g.Count(x => x.StatusBayar == 2),
                       //Ketetapan = g.Sum(x => x.Ketetapan),
                       //Realisasi = g.Sum(x => x.Realisasi),
                   })
                   .AsQueryable();
                    var kontrolPembayaranList = context.DbCtrlByrPbbs
                        .Where(x => x.Tahun == tahun && x.WilayahPajak == (decimal)uptb)
                        .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                        .Select(g => new
                        {
                            KategoriId = g.Key.KategoriId,
                            Tahun = g.Key.Tahun,
                            Bulan = g.Key.Bulan,
                            JmlBayar = g.Count(x => x.StatusBayar == 1),
                            //Ketetapan = g.Sum(x => x.Ketetapan),
                            //Realisasi = g.Sum(x => x.Realisasi),
                        })
                        .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.PBB.GetDescription();

                        re.OPbuka1 = kontrolPembayaranAwalList
                            .Where(x => x.KategoriId == item.Id)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranAwalList
                            .Where(x => x.KategoriId == item.Id)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Nts1 = kontrolPembayaranAwalList
                            .Where(x => x.KategoriId == item.Id)
                            .Sum(x => x.JmlNTs);

                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);

                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);

                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);

                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);

                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);

                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);

                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);

                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);

                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);

                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);

                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);

                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);


                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranAwalList = context.DbCtrlByrPbbs
                   .Where(x => x.Tahun == tahun)
                   .GroupBy(x => new { x.KategoriId })
                   .Select(g => new
                   {
                       KategoriId = g.Key.KategoriId,
                       Jml = g.Count(),
                       JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                       JmlNTs = g.Count(x => x.StatusBayar == 2),
                       //Ketetapan = g.Sum(x => x.Ketetapan),
                       //Realisasi = g.Sum(x => x.Realisasi),
                   })
                   .AsQueryable();
                    var kontrolPembayaranList = context.DbCtrlByrPbbs
                        .Where(x => x.Tahun == tahun)
                        .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                        .Select(g => new
                        {
                            KategoriId = g.Key.KategoriId,
                            Tahun = g.Key.Tahun,
                            Bulan = g.Key.Bulan,
                            JmlBayar = g.Count(x => x.StatusBayar == 1),
                            //Ketetapan = g.Sum(x => x.Ketetapan),
                            //Realisasi = g.Sum(x => x.Realisasi),
                        })
                        .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new KontrolPembayaran();
                        re.Kategori = item.Nama;
                        re.Tahun = tahun;
                        re.kategoriId = (int)item.Id;
                        re.JenisPajak = EnumFactory.EPajak.PBB.GetDescription();

                        re.OPbuka1 = kontrolPembayaranAwalList
                            .Where(x => x.KategoriId == item.Id)
                            .Sum(x => x.Jml);
                        re.Blm1 = kontrolPembayaranAwalList
                            .Where(x => x.KategoriId == item.Id)
                            .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                        re.Nts1 = kontrolPembayaranAwalList
                            .Where(x => x.KategoriId == item.Id)
                            .Sum(x => x.JmlNTs);

                        re.Byr1 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.JmlBayar);

                        re.Byr2 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.JmlBayar);

                        re.Byr3 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.JmlBayar);

                        re.Byr4 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.JmlBayar);

                        re.Byr5 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.JmlBayar);

                        re.Byr6 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.JmlBayar);

                        re.Byr7 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.JmlBayar);

                        re.Byr8 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.JmlBayar);

                        re.Byr9 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.JmlBayar);

                        re.Byr10 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.JmlBayar);

                        re.Byr11 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.JmlBayar);

                        re.Byr12 = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.JmlBayar);


                        ret.Add(re);
                    }
                }

                return ret;
            }
            //REKLAME
            public static List<KontrolPembayaran> GetKontrolPembayaranReklameRekap(int tahun)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.Reklame).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var kontrolPembayaranList = context.DbCtrlByrReklames
                    .Where(x => x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new KontrolPembayaran();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.Reklame.GetDescription();

                    re.OPbuka1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.Jml);
                    re.Blm1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.JmlBayar);
                    re.Nts1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.Jml);
                    re.Blm2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.JmlBayar);
                    re.Nts2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.Jml);
                    re.Blm3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.JmlBayar);
                    re.Nts3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.Jml);
                    re.Blm4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.JmlBayar);
                    re.Nts4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.Jml);
                    re.Blm5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.JmlBayar);
                    re.Nts5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.Jml);
                    re.Blm6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.JmlBayar);
                    re.Nts6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.Jml);
                    re.Blm7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.JmlBayar);
                    re.Nts7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.Jml);
                    re.Blm8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.JmlBayar);
                    re.Nts8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.Jml);
                    re.Blm9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.JmlBayar);
                    re.Nts9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.Jml);
                    re.Blm10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.JmlBayar);
                    re.Nts10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.Jml);
                    re.Blm11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.JmlBayar);
                    re.Nts11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.Jml);
                    re.Blm12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.JmlBayar);
                    re.Nts12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.JmlNTs);

                    ret.Add(re);
                }

                return ret;
            }
            //BPHTB
            public static List<KontrolPembayaran> GetKontrolPembayaranBphtbRekap(int tahun)
            {
                var ret = new List<KontrolPembayaran>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.BPHTB)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var kontrolPembayaranList = context.DbCtrlByrBphtbs
                    .Where(x => x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Jml = g.Count(),
                        JmlBlmBayar = g.Count(x => x.StatusBayar == 0),
                        JmlBayar = g.Count(x => x.StatusBayar == 1),
                        JmlNTs = g.Count(x => x.StatusBayar == 2),
                        //Ketetapan = g.Sum(x => x.Ketetapan),
                        //Realisasi = g.Sum(x => x.Realisasi),
                    })
                    .AsQueryable();

                foreach (var item in kategoriList.OrderBy(x => x.Id))
                {
                    var re = new KontrolPembayaran();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription();

                    re.OPbuka1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.Jml);
                    re.Blm1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.JmlBayar);
                    re.Nts1 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.Jml);
                    re.Blm2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.JmlBayar);
                    re.Nts2 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.Jml);
                    re.Blm3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.JmlBayar);
                    re.Nts3 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.Jml);
                    re.Blm4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.JmlBayar);
                    re.Nts4 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.Jml);
                    re.Blm5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.JmlBayar);
                    re.Nts5 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.Jml);
                    re.Blm6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.JmlBayar);
                    re.Nts6 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.Jml);
                    re.Blm7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.JmlBayar);
                    re.Nts7 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.Jml);
                    re.Blm8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.JmlBayar);
                    re.Nts8 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.Jml);
                    re.Blm9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.JmlBayar);
                    re.Nts9 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.Jml);
                    re.Blm10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.JmlBayar);
                    re.Nts10 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.Jml);
                    re.Blm11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.JmlBayar);
                    re.Nts11 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.JmlNTs);

                    re.OPbuka12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.Jml);
                    re.Blm12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.JmlBlmBayar + x.JmlNTs);
                    re.Byr12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.JmlBayar);
                    re.Nts12 = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.JmlNTs);

                    ret.Add(re);
                }

                return ret;
            }
            #endregion

            #region Data Detail KontrolPembayaran
            public static List<DetailPajak> GetDetailKontrolPembayaranList(EnumFactory.EPajak jenisPajak, int kategoriId, int tahun, int bulan, int status, EnumFactory.EUPTB uptb, bool isTotal = false, bool isHotelNonBintang = false)
            {
                // STATUS
                // 0 = belumBayar
                // 1 = sudahBayar
                // 2 = NTs
                // 3 all
                var ret = new List<DetailPajak>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;

                    case EnumFactory.EPajak.MakananMinuman:
                        var queryResto = context.DbCtrlByrRestos
                        .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryResto = queryResto.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (status != 3) // Jika bukan "all"
                        {
                            queryResto = queryResto.Where(x => x.StatusBayar == status);
                        }

                        // Filter berdasarkan kategori
                        if (!isTotal)
                        {
                            queryResto = queryResto.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryResto.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                            .ToList();
                        break;

                    case EnumFactory.EPajak.TenagaListrik:
                        var queryTenagaListrik = context.DbCtrlByrPpjs
                            .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryTenagaListrik = queryTenagaListrik.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (status != 3) // Jika bukan "all"
                        {
                            queryTenagaListrik = queryTenagaListrik.Where(x => x.StatusBayar == status);
                        }

                        // Filter berdasarkan kategori
                        if (!isTotal)
                        {
                            queryTenagaListrik = queryTenagaListrik.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryTenagaListrik.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                            .ToList();
                        break;

                    case EnumFactory.EPajak.JasaPerhotelan:
                        var queryHotel = context.DbCtrlByrHotels
                            .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0)
                            .AsQueryable();

                        // Filter berdasarkan UPTB (skip jika SEMUA)
                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryHotel = queryHotel.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        // Filter berdasarkan status
                        if (status != 3) // Jika bukan "all"
                        {
                            queryHotel = queryHotel.Where(x => x.StatusBayar == status);
                        }

                        // Filter berdasarkan total / kategori
                        if (isTotal)
                        {
                            if (isHotelNonBintang)
                            {
                                // Jika isHotelNonBintang true, filter KategoriId = 17
                                queryHotel = queryHotel.Where(x => x.KategoriId == 17);
                            }
                            else
                            {
                                // Jika isHotelNonBintang false, filter KategoriId yang bukan 17
                                queryHotel = queryHotel.Where(x => x.KategoriId != 17);
                            }
                        }
                        else
                        {
                            // Jika isTotal false, filter berdasarkan KategoriId yang spesifik
                            queryHotel = queryHotel.Where(x => x.KategoriId == kategoriId);
                        }

                        // Select hasil
                        ret = queryHotel.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + (x.WilayahPajak ?? "-"),
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();

                        break;

                    case EnumFactory.EPajak.JasaParkir:
                        var queryJasaParkir = context.DbCtrlByrParkirs
                            .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryJasaParkir = queryJasaParkir.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (status != 3) // Jika bukan "all"
                        {
                            queryJasaParkir = queryJasaParkir.Where(x => x.StatusBayar == status);
                        }

                        // Filter berdasarkan kategori
                        if (!isTotal)
                        {
                            queryJasaParkir = queryJasaParkir.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryJasaParkir.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                            .ToList();
                        break;

                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var queryHiburan = context.DbCtrlByrHiburans
                            .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryHiburan = queryHiburan.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (status != 3)
                        {
                            queryHiburan = queryHiburan.Where(x => x.StatusBayar == status);
                        }

                        if (!isTotal)
                        {
                            queryHiburan = queryHiburan.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryHiburan.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                            .ToList();
                        break;

                    case EnumFactory.EPajak.AirTanah:
                        var queryAirTanah = context.DbCtrlByrAbts
                            .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryAirTanah = queryAirTanah.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (status != 3) // Jika bukan "all"
                        {
                            queryAirTanah = queryAirTanah.Where(x => x.StatusBayar == status);
                        }

                        // Filter berdasarkan kategori
                        if (!isTotal)
                        {
                            queryAirTanah = queryAirTanah.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryAirTanah
                        .ToList()
                        .Select(x =>
                        {
                            var ss = context.DbOpAbts
                                            .FirstOrDefault(y => y.Nop == x.Nop);

                            return new DetailPajak
                            {
                                Kategori = x.NamaKategori,
                                JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                                NOP = x.Nop,
                                Tahun = tahun,
                                NamaOP = ss?.NamaOp ?? x.NamaOp,
                                Alamat = ss?.AlamatOp ?? x.AlamatOp,
                                Ketetapan = x.Ketetapan ?? 0,
                                Realisasi = x.Realisasi ?? 0,
                                Wilayah = "SURABAYA " + (ss.WilayahPajak ?? x.WilayahPajak) ?? "-",
                                Keterangan = x.Keterangan ?? "-"
                            };
                        })
                        .ToList();
                        break;

                    case EnumFactory.EPajak.Reklame:
                        var queryReklame = context.DbCtrlByrReklames
                        .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0);

                        // Filter berdasarkan status
                        if (status != 3)
                        {
                            queryReklame = queryReklame.Where(x => x.StatusBayar == status);
                        }

                        // Filter berdasarkan kategori
                        if (!isTotal)
                        {
                            queryReklame = queryReklame.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryReklame.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                            NOP = x.NoFormulir,
                            Tahun = tahun,
                            NamaOP = x.NamaWp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                            .ToList();
                        break;

                    case EnumFactory.EPajak.PBB:

                        // Status 0 = belum bayar All, 1 = sudah bayar Bulan, 2 = NTs All
                        // Jika status 3, maka tampilkan semua
                        var queryPbb = context.DbCtrlByrPbbs.Where(x => x.Tahun == tahun && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryPbb = queryPbb.Where(x => x.WilayahPajak == (decimal)uptb);
                        }

                        // PBB logic khusus: status 1 perlu filter bulan, status lain tidak
                        if (status == 1)
                        {
                            queryPbb = queryPbb.Where(x => x.Bulan == bulan && x.StatusBayar == status);
                        }
                        else if (status != 3)
                        {
                            queryPbb = queryPbb.Where(x => x.Bulan == bulan && x.StatusBayar == status);
                        }
                        // Jika status == 3, tidak perlu filter StatusBayar

                        if (!isTotal)
                        {
                            queryPbb = queryPbb.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryPbb.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaWp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                            .ToList();
                        break;

                    case EnumFactory.EPajak.BPHTB:
                        var queryBphtb = context.DbCtrlByrBphtbs
                        .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.KategoriId > 0);

                        // Filter berdasarkan status
                        if (status != 3)
                        {
                            queryBphtb = queryBphtb.Where(x => x.StatusBayar == status);
                        }

                        // Filter berdasarkan kategori
                        if (!isTotal)
                        {
                            queryBphtb = queryBphtb.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryBphtb.Select(x => new DetailPajak
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                            NOP = x.Idsspd,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",

                        })
                            .ToList();
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
            #endregion


            #region Data Upaya Pajak
            public static List<UpayaPajak> GetUpayaPajakHotelRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakHotelRekap(int tahun, bool turu)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList.Where(x => x.Id == 17).OrderByDescending(x => x.Id).ToList())
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakRestoRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.MakananMinuman && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakParkirRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaParkir).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaParkir && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakHiburanRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakPPJRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.TenagaListrik && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakABTRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.AirTanah).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.AirTanah && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakPBBRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.PBB).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.PBB && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.PBB.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakReklameRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.Reklame).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.Reklame && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList)
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.Reklame.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            public static List<UpayaPajak> GetUpayaPajakBphtbRekap(int tahun)
            {
                var ret = new List<UpayaPajak>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.BPHTB)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                var upayaPajakList = context.MvUpayaPadKategoris
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.BPHTB && x.Tahun == tahun)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        himbauan = g.Count(x => x.IsHimbauan == 1),
                        teguran = g.Count(x => x.IsTeguran == 1),
                        silang = g.Count(x => x.IsPenyilangan == 1),
                        kejaksaan = g.Count(x => x.IsKejaksaan == 1),

                    })
                    .AsQueryable();

                foreach (var item in kategoriList.OrderBy(x => x.Id))
                {
                    var re = new UpayaPajak();
                    re.Kategori = item.Nama;
                    re.Tahun = tahun;
                    re.kategoriId = (int)item.Id;
                    re.JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription();

                    re.Himb1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.himbauan);
                    re.Tegur1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.teguran);
                    re.Sil1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.silang);
                    re.Kejak1 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.kejaksaan);

                    re.Himb2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.himbauan);
                    re.Tegur2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.teguran);
                    re.Sil2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.silang);
                    re.Kejak2 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.kejaksaan);

                    re.Himb3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.himbauan);
                    re.Tegur3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.teguran);
                    re.Sil3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.silang);
                    re.Kejak3 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.kejaksaan);

                    re.Himb4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.himbauan);
                    re.Tegur4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.teguran);
                    re.Sil4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.silang);
                    re.Kejak4 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.kejaksaan);

                    re.Himb5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.himbauan);
                    re.Tegur5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.teguran);
                    re.Sil5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.silang);
                    re.Kejak5 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.kejaksaan);

                    re.Himb6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.himbauan);
                    re.Tegur6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.teguran);
                    re.Sil6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.silang);
                    re.Kejak6 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.kejaksaan);

                    re.Himb7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.himbauan);
                    re.Tegur7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.teguran);
                    re.Sil7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.silang);
                    re.Kejak7 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.kejaksaan);

                    re.Himb8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.himbauan);
                    re.Tegur8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.teguran);
                    re.Sil8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.silang);
                    re.Kejak8 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.kejaksaan);

                    re.Himb9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.himbauan);
                    re.Tegur9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.teguran);
                    re.Sil9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.silang);
                    re.Kejak9 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.kejaksaan);

                    re.Himb10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.himbauan);
                    re.Tegur10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.teguran);
                    re.Sil10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.silang);
                    re.Kejak10 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.kejaksaan);

                    re.Himb11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.himbauan);
                    re.Tegur11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.teguran);
                    re.Sil11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.silang);
                    re.Kejak11 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.kejaksaan);

                    re.Himb12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.himbauan);
                    re.Tegur12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.teguran);
                    re.Sil12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.silang);
                    re.Kejak12 = upayaPajakList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.kejaksaan);

                    ret.Add(re);
                }
                return ret;
            }
            #endregion

            #region Detail Upaya Pajak
            public static List<DetailUpaya> GetDetailUpayaPajakList(EnumFactory.EPajak jenisPajak, int kategoriId, int tahun, int bulan, int status)
            {
                var ret = new List<DetailUpaya>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EPajak.Semua:
                        break;
                    case EPajak.MakananMinuman:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpRestos
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpRestos
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpRestos
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpRestos
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.TenagaListrik:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpListriks
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpListriks
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpListriks
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpListriks
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.JasaPerhotelan:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHotels
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHotels
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHotels
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                    .Distinct()
                                    .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHotels
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.JasaParkir:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpParkirs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpParkirs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpParkirs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpParkirs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.JasaKesenianHiburan:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHiburans
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHiburans
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHiburans
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpHiburans
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.AirTanah:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpAbts
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpAbts
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpAbts
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpAbts
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaOp : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.Reklame:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpReklames
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.Nama : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpReklames
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.Nama : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpReklames
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.Nama : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbOpReklames
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.Nama : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.PBB:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonPbbs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.WpNama : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonPbbs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.WpNama : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonPbbs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.WpNama : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonPbbs
                                       on upaya.Nop equals ctrl.Nop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.WpNama : "-",
                                       Alamat = ctrl != null ? ctrl.AlamatOp : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.BPHTB:
                        if (status == 0)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonBphtbs
                                       on upaya.Nop equals ctrl.SpptNop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsHimbauan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaWp : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "Himbauan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 1)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonBphtbs
                                       on upaya.Nop equals ctrl.SpptNop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsTeguran == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaWp : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "Teguran"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 2)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonBphtbs
                                       on upaya.Nop equals ctrl.SpptNop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsPenyilangan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaWp : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "Silang"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        else if (status == 3)
                        {
                            ret = (from upaya in context.MvUpayaPadKategoris
                                   join ctrl in context.DbMonBphtbs
                                       on upaya.Nop equals ctrl.SpptNop into gj
                                   from ctrl in gj.DefaultIfEmpty()

                                   join kat in context.MKategoriPajaks
                                       on upaya.KategoriId equals kat.Id into gk
                                   from kat in gk.DefaultIfEmpty()

                                   where upaya.Tahun == tahun
                                         && upaya.Bulan == bulan
                                         && upaya.KategoriId == kategoriId
                                         && upaya.IsKejaksaan == 1

                                   select new DetailUpaya
                                   {
                                       JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                                       NOP = upaya.Nop,
                                       // ubah dari ID ke Nama TitleCase
                                       Kategori = kat != null
                                           ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kat.Nama.ToLower())
                                           : "-",
                                       Tahun = (int)upaya.Tahun,
                                       NamaOP = ctrl != null ? ctrl.NamaWp : "-",
                                       Alamat = ctrl != null ? ctrl.Alamat : "-",
                                       JenisPenagihan = "Kejaksaan"
                                   })
                                   .Distinct()
                                   .ToList();
                        }
                        break;
                    case EPajak.OpsenPkb:
                        break;
                    case EPajak.OpsenBbnkb:
                        break;
                    default:
                        break;
                }

                return ret;
            }
            #endregion

            #region Data Rekap Potensi Pajak
            public static List<Potensi> GetPotensiPajakHotelRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();

                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id != 17).OrderByDescending(x => x.Id).ToList())
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }

                return ret;
            }
            public static List<Potensi> GetPotensiPajakHotelRekap(int tahun, EnumFactory.EUPTB uptb, bool turu)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();

                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id == 17).OrderByDescending(x => x.Id).ToList())
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrHotels
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList.Where(x => x.Id == 17).OrderByDescending(x => x.Id).ToList())
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                return ret;
            }
            public static List<Potensi> GetPotensiPajakRestoRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();

                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrRestos
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrRestos
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }

                return ret;
            }
            public static List<Potensi> GetPotensiPajakPPJRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();

                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrPpjs
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrPpjs
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();

                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }


                return ret;
            }
            public static List<Potensi> GetPotensiPajakParkirRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaParkir).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrParkirs
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrParkirs
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }

                return ret;
            }
            public static List<Potensi> GetPotensiPajakHiburanRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrHiburans
                   .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((int)uptb).ToString())
                   .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                   .Select(g => new
                   {
                       KategoriId = g.Key.KategoriId,
                       Tahun = g.Key.Tahun,
                       Bulan = g.Key.Bulan,
                       Ketetapan = g.Sum(x => x.Ketetapan),
                   })
                   .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrHiburans
                   .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                   .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                   .Select(g => new
                   {
                       KategoriId = g.Key.KategoriId,
                       Tahun = g.Key.Tahun,
                       Bulan = g.Key.Bulan,
                       Ketetapan = g.Sum(x => x.Ketetapan),
                   })
                   .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }

                }

                return ret;
            }
            public static List<Potensi> GetPotensiPajakReklameRekap(int tahun)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.Reklame).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                var kontrolPembayaranList = context.DbCtrlByrReklames
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();
                foreach (var item in kategoriList)
                {
                    var re = new Potensi();
                    re.Kategori = item.Nama;
                    re.kategoriId = (int)item.Id;
                    re.Tahun = tahun;
                    re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                    re.Jan = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Feb = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Mar = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Apr = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Mei = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Jun = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Jul = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Agt = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Sep = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Okt = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Nov = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Des = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.Ketetapan) ?? 0;

                    ret.Add(re);
                }

                return ret;
            }
            public static List<Potensi> GetPotensiPajakAirTanahRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.AirTanah).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrAbts
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((int)uptb).ToString())
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrAbts
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }

                return ret;
            }
            public static List<Potensi> GetPotensiPajakPbbRekap(int tahun, EnumFactory.EUPTB uptb)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.PBB).OrderBy(x => x.Urutan)
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();

                if (uptb != EUPTB.SEMUA)
                {
                    var kontrolPembayaranList = context.DbCtrlByrPbbs
                   .Where(x => x.Tahun == tahun && x.StatusBayar == 0 && x.WilayahPajak == ((decimal)uptb))
                   .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                   .Select(g => new
                   {
                       KategoriId = g.Key.KategoriId,
                       Tahun = g.Key.Tahun,
                       Bulan = g.Key.Bulan,
                       Ketetapan = g.Sum(x => x.Ketetapan),
                   })
                   .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.PBB.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                else
                {
                    var kontrolPembayaranList = context.DbCtrlByrPbbs
                   .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                   .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                   .Select(g => new
                   {
                       KategoriId = g.Key.KategoriId,
                       Tahun = g.Key.Tahun,
                       Bulan = g.Key.Bulan,
                       Ketetapan = g.Sum(x => x.Ketetapan),
                   })
                   .AsQueryable();
                    foreach (var item in kategoriList)
                    {
                        var re = new Potensi();
                        re.Kategori = item.Nama;
                        re.kategoriId = (int)item.Id;
                        re.Tahun = tahun;
                        re.JenisPajak = EnumFactory.EPajak.PBB.GetDescription();
                        re.Jan = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Feb = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mar = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Apr = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Mei = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jun = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Jul = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Agt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Sep = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Okt = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Nov = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                            .Sum(x => x.Ketetapan) ?? 0;
                        re.Des = kontrolPembayaranList
                            .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                            .Sum(x => x.Ketetapan) ?? 0;

                        ret.Add(re);
                    }
                }
                return ret;
            }
            public static List<Potensi> GetPotensiPajakBphtbRekap(int tahun)
            {
                var ret = new List<Potensi>();
                var context = DBClass.GetContext();
                var kategoriList = context.MKategoriPajaks
                    .Where(x => x.PajakId == (int)EnumFactory.EPajak.BPHTB)
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.Nama.ToLower())
                    })
                    .ToList();
                var kontrolPembayaranList = context.DbCtrlByrBphtbs
                    .Where(x => x.Tahun == tahun && x.StatusBayar == 0)
                    .GroupBy(x => new { x.KategoriId, x.Tahun, x.Bulan })
                    .Select(g => new
                    {
                        KategoriId = g.Key.KategoriId,
                        Tahun = g.Key.Tahun,
                        Bulan = g.Key.Bulan,
                        Ketetapan = g.Sum(x => x.Ketetapan),
                    })
                    .AsQueryable();
                foreach (var item in kategoriList.OrderBy(x => x.Id))
                {
                    var re = new Potensi();
                    re.Kategori = item.Nama;
                    re.kategoriId = (int)item.Id;
                    re.Tahun = tahun;
                    re.JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription();
                    re.Jan = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 1)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Feb = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 2)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Mar = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 3)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Apr = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 4)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Mei = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 5)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Jun = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 6)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Jul = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 7)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Agt = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 8)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Sep = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 9)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Okt = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 10)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Nov = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 11)
                        .Sum(x => x.Ketetapan) ?? 0;
                    re.Des = kontrolPembayaranList
                        .Where(x => x.KategoriId == item.Id && x.Tahun == tahun && x.Bulan == 12)
                        .Sum(x => x.Ketetapan) ?? 0;

                    ret.Add(re);
                }

                return ret;
            }
            #endregion

            #region Data Detail Potensi
            //buat method untuk ambil detail potensi 
            public static List<DetailPotensi> GetDetailPotensiPajakList(EnumFactory.EPajak jenisPajak, int kategoriId, int tahun, int bulan, EnumFactory.EUPTB uptb, bool isTotal = false, bool isHotelNonBintang = false)
            {
                var ret = new List<DetailPotensi>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.Semua:
                        break;

                    case EnumFactory.EPajak.MakananMinuman:
                        var queryResto = context.DbCtrlByrRestos
                        .Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryResto = queryResto.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        // Jika bukan total, filter berdasarkan kategoriId
                        if (!isTotal)
                        {
                            queryResto = queryResto.Where(x => x.KategoriId == kategoriId);
                        }
                        ret = queryResto.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();
                        break;

                    case EnumFactory.EPajak.TenagaListrik:
                        var queryPajakListrik = context.DbCtrlByrPpjs.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryPajakListrik = queryPajakListrik.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }
                        // Jika bukan total, filter berdasarkan kategoriId
                        if (!isTotal)
                        {
                            queryPajakListrik = queryPajakListrik.Where(x => x.KategoriId == kategoriId);
                        }
                        ret = queryPajakListrik.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                    .ToList();
                        break;

                    case EnumFactory.EPajak.JasaPerhotelan:
                        var queryHotel = context.DbCtrlByrHotels.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryHotel = queryHotel.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (isTotal)
                        {
                            // Jika isTotal true, maka :
                            if (isHotelNonBintang)
                            {
                                // Jika isHotelNonBintang true, filter KategoriId = 17
                                queryHotel = queryHotel.Where(x => x.KategoriId == 17);
                            }
                            else
                            {
                                // Jika isHotelNonBintang false, filter KategoriId yang bukan 17
                                queryHotel = queryHotel.Where(x => x.KategoriId != 17 && x.KategoriId > 0);
                            }
                        }
                        else
                        {
                            // Jika isTotal false, filter berdasarkan KategoriId yang spesifik
                            queryHotel = queryHotel.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryHotel.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                            .ToList();
                        break;

                    case EnumFactory.EPajak.JasaParkir:
                        var queryParkir = context.DbCtrlByrParkirs.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryParkir = queryParkir.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (!isTotal)
                        {
                            queryParkir = queryParkir.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryParkir.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();
                        break;

                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var queryKesenian = context.DbCtrlByrHiburans.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryKesenian = queryKesenian.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (!isTotal)
                        {
                            queryKesenian = queryKesenian.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryKesenian.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();
                        break;

                    case EnumFactory.EPajak.AirTanah:
                        var queryAirTanah = context.DbCtrlByrAbts.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryAirTanah = queryAirTanah.Where(x => x.WilayahPajak == ((int)uptb).ToString());
                        }

                        if (!isTotal)
                        {
                            queryAirTanah = queryAirTanah.Where(x => x.KategoriId == kategoriId);
                        }

                        ret = queryAirTanah.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();
                        break;

                    case EnumFactory.EPajak.Reklame:
                        var queryReklame = context.DbCtrlByrReklames.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0);
                        if (!isTotal)
                        {
                            queryReklame = queryReklame.Where(x => x.KategoriId == kategoriId);
                        }
                        ret = queryReklame.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                            NOP = x.NoFormulir,
                            Tahun = tahun,
                            NamaOP = x.NamaWp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();
                        break;

                    case EnumFactory.EPajak.PBB:
                        var queryPBB = context.DbCtrlByrPbbs.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0).AsQueryable();

                        if (uptb != EnumFactory.EUPTB.SEMUA)
                        {
                            queryPBB = queryPBB.Where(x => x.WilayahPajak == ((decimal)uptb));
                        }

                        if (!isTotal)
                        {
                            queryPBB = queryPBB.Where(x => x.KategoriId == kategoriId);
                        }
                        ret = queryPBB.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                            NOP = x.Nop,
                            Tahun = tahun,
                            NamaOP = x.NamaWp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var queryBPHTB = context.DbCtrlByrBphtbs.Where(x => x.Tahun == tahun && x.Bulan == bulan && x.StatusBayar == 0 && x.KategoriId > 0);
                        if (!isTotal)
                        {
                            queryBPHTB = queryBPHTB.Where(x => x.KategoriId == kategoriId);
                        }
                        ret = queryBPHTB.Select(x => new DetailPotensi
                        {
                            Kategori = x.NamaKategori,
                            JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                            NOP = x.Idsspd,
                            Tahun = tahun,
                            NamaOP = x.NamaOp,
                            Alamat = x.AlamatOp,
                            Ketetapan = x.Ketetapan ?? 0,
                            Realisasi = x.Realisasi ?? 0,
                            Wilayah = "SURABAYA " + x.WilayahPajak ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                        })
                        .ToList();
                        break;

                    case EnumFactory.EPajak.OpsenPkb:
                        break;

                    case EnumFactory.EPajak.OpsenBbnkb:
                        break;
                }
                return ret;
            }
            #endregion
        }

        public class KontrolPembayaran
        {
            public string Kategori { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public int kategoriId { get; set; }
            public int Tahun { get; set; }

            public decimal OPbuka1 { get; set; }
            public decimal Byr1 { get; set; }
            public decimal Nts1 { get; set; }
            public decimal Blm1 { get; set; }

            public decimal OPbuka2 { get; set; }
            public decimal Byr2 { get; set; }
            public decimal Nts2 { get; set; }
            public decimal Blm2 { get; set; }

            public decimal OPbuka3 { get; set; }
            public decimal Byr3 { get; set; }
            public decimal Nts3 { get; set; }
            public decimal Blm3 { get; set; }

            public decimal OPbuka4 { get; set; }
            public decimal Byr4 { get; set; }
            public decimal Nts4 { get; set; }
            public decimal Blm4 { get; set; }

            public decimal OPbuka5 { get; set; }
            public decimal Byr5 { get; set; }
            public decimal Nts5 { get; set; }
            public decimal Blm5 { get; set; }

            public decimal OPbuka6 { get; set; }
            public decimal Byr6 { get; set; }
            public decimal Nts6 { get; set; }
            public decimal Blm6 { get; set; }

            public decimal OPbuka7 { get; set; }
            public decimal Byr7 { get; set; }
            public decimal Nts7 { get; set; }
            public decimal Blm7 { get; set; }

            public decimal OPbuka8 { get; set; }
            public decimal Byr8 { get; set; }
            public decimal Nts8 { get; set; }
            public decimal Blm8 { get; set; }

            public decimal OPbuka9 { get; set; }
            public decimal Byr9 { get; set; }
            public decimal Nts9 { get; set; }
            public decimal Blm9 { get; set; }

            public decimal OPbuka10 { get; set; }
            public decimal Byr10 { get; set; }
            public decimal Nts10 { get; set; }
            public decimal Blm10 { get; set; }

            public decimal OPbuka11 { get; set; }
            public decimal Byr11 { get; set; }
            public decimal Nts11 { get; set; }
            public decimal Blm11 { get; set; }

            public decimal OPbuka12 { get; set; }
            public decimal Byr12 { get; set; }
            public decimal Nts12 { get; set; }
            public decimal Blm12 { get; set; }
        }
        public class UpayaPajak
        {
            public string Kategori { get; set; } = null!;
            public int kategoriId { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }

            public int Himb1 { get; set; }
            public int Tegur1 { get; set; }
            public int Sil1 { get; set; }
            public int Kejak1 { get; set; }

            public int Himb2 { get; set; }
            public int Tegur2 { get; set; }
            public int Sil2 { get; set; }
            public int Kejak2 { get; set; }

            public int Himb3 { get; set; }
            public int Tegur3 { get; set; }
            public int Sil3 { get; set; }
            public int Kejak3 { get; set; }

            public int Himb4 { get; set; }
            public int Tegur4 { get; set; }
            public int Sil4 { get; set; }
            public int Kejak4 { get; set; }

            public int Himb5 { get; set; }
            public int Tegur5 { get; set; }
            public int Sil5 { get; set; }
            public int Kejak5 { get; set; }

            public int Himb6 { get; set; }
            public int Tegur6 { get; set; }
            public int Sil6 { get; set; }
            public int Kejak6 { get; set; }

            public int Himb7 { get; set; }
            public int Tegur7 { get; set; }
            public int Sil7 { get; set; }
            public int Kejak7 { get; set; }

            public int Himb8 { get; set; }
            public int Tegur8 { get; set; }
            public int Sil8 { get; set; }
            public int Kejak8 { get; set; }

            public int Himb9 { get; set; }
            public int Tegur9 { get; set; }
            public int Sil9 { get; set; }
            public int Kejak9 { get; set; }

            public int Himb10 { get; set; }
            public int Tegur10 { get; set; }
            public int Sil10 { get; set; }
            public int Kejak10 { get; set; }

            public int Himb11 { get; set; }
            public int Tegur11 { get; set; }
            public int Sil11 { get; set; }
            public int Kejak11 { get; set; }

            public int Himb12 { get; set; }
            public int Tegur12 { get; set; }
            public int Sil12 { get; set; }
            public int Kejak12 { get; set; }
        }
        public class Potensi
        {
            public string Kategori { get; set; } = null!;
            public int kategoriId { get; set; }
            public string JenisPajak { get; set; } = null!;
            public int Tahun { get; set; }
            public decimal Jan { get; set; }
            public decimal Feb { get; set; }
            public decimal Mar { get; set; }
            public decimal Apr { get; set; }
            public decimal Mei { get; set; }
            public decimal Jun { get; set; }
            public decimal Jul { get; set; }
            public decimal Agt { get; set; }
            public decimal Sep { get; set; }
            public decimal Okt { get; set; }
            public decimal Nov { get; set; }
            public decimal Des { get; set; }
            public decimal Total => Jan + Feb + Mar + Apr + Mei + Jun + Jul + Agt + Sep + Okt + Nov + Des;
        }
        public class DetailPajak
        {
            public string Kategori { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public int Tahun { get; set; }
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public decimal Ketetapan { get; set; }
            public decimal Realisasi { get; set; }
            public string Wilayah { get; set; } = null!;
            public string Keterangan { get; set; } = null!;
            public string Status { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
        }

        public class DetailPotensi
        {
            public string Kategori { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public int Tahun { get; set; }
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public decimal Ketetapan { get; set; }
            public decimal Realisasi { get; set; }
            public string Wilayah { get; set; } = null!;
            public string Keterangan { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
        }

        public class DetailUpaya
        {
            public string Kategori { get; set; } = null!;
            public string JenisPajak { get; set; } = null!;
            public string NOP { get; set; } = null!;
            public int Tahun { get; set; }
            public string NamaOP { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisPenagihan { get; set; } = null!;
            public string Status { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(NOP);
        }


    }
}
