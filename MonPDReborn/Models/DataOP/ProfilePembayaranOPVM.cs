using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class ProfilePembayaranOPVM
    {
        public class Index
        {
            public string Keyword { get; set; } = string.Empty;
            public int SelectedPajak { get; set; }

            public List<SelectListItem> JenisPajakList { get; set; } = new();

            public Index(string keyword)
            {
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                   .Cast<EnumFactory.EPajak>()
                   .Select(x => new SelectListItem
                   {
                       Value = ((int)x).ToString(),
                       Text = x.GetDescription()
                   }).ToList();
                Keyword = keyword;
            }
        }



        public class Show
        {
            public IdentitasObjekPajak DataIdentitasObjekPajak { get; set; }
            public EnumFactory.EPajak JenisPajak { get; set; }

            public Show() { }

            public Show(EnumFactory.EPajak jenisPajak, string keyword)
            {
                JenisPajak = jenisPajak;

                // Ini penting: keyword WAJIB dikirim ke method
                DataIdentitasObjekPajak = Method.GetDataPembayaranOPList(jenisPajak, keyword);
            }
        }






        public class Detail
        {
            public string NOP { get; set; }
            public EnumFactory.EPajak JenisPajak { get; set; }

            public int TahunKiri { get; set; }
            public int TahunKanan { get; set; }

            public DataTahunPembayaran DataRealisasiKiri { get; set; }
            public DataTahunPembayaran DataRealisasiKanan { get; set; }

            public Detail() { }

            public Detail(EnumFactory.EPajak jenisPajak, string nop, int tahunKiri, int tahunKanan)
            {
                JenisPajak = jenisPajak;
                NOP = nop;

                TahunKiri = tahunKiri;
                TahunKanan = tahunKanan;

                DataRealisasiKiri = Method.GetDataTahunanPembayaranList(jenisPajak, nop, tahunKiri);
                DataRealisasiKanan = Method.GetDataTahunanPembayaranList(jenisPajak, nop, tahunKanan);
            }
        }


        public class Method
        {
            public static IdentitasObjekPajak GetDataPembayaranList(EnumFactory.EPajak jenisPajak, string keyword)
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    throw new ArgumentException("Keyword harus diisi");
                }
                if (keyword.Length < 3)
                {
                    throw new ArgumentException("Keyword harus diisi minimal 3");
                }

                var ret = new IdentitasObjekPajak();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var resto = context.DbOpRestos
                            .Where(x => (x.Nop == keyword) || x.NamaOp.ToUpper().Contains(keyword.ToUpper()))
                            .FirstOrDefault(x => x.TahunBuku == currentYear);
                        if (resto != null) MapIdentitas(ret, resto.Nop, resto.AlamatOp, resto.AlamatOpKdCamat, resto.AlamatOpKdLurah, resto.WilayahPajak);
                        break;

                    case EnumFactory.EPajak.TenagaListrik:
                        var listrik = context.DbOpListriks
                            .Where(x => (x.Nop == keyword) || x.NamaOp.ToUpper().Contains(keyword.ToUpper()))
                            .FirstOrDefault(x => x.TahunBuku == currentYear);
                        if (listrik != null) MapIdentitas(ret, listrik.Nop, listrik.AlamatOp, listrik.AlamatOpKdCamat, listrik.AlamatOpKdLurah, listrik.WilayahPajak);
                        break;

                    case EnumFactory.EPajak.JasaPerhotelan:
                        var hotel = context.DbOpHotels
                            .Where(x => (x.Nop == keyword) || x.NamaOp.ToUpper().Contains(keyword.ToUpper()))
                            .FirstOrDefault(x => x.TahunBuku == currentYear);
                        if (hotel != null) MapIdentitas(ret, hotel.Nop, hotel.AlamatOp, hotel.AlamatOpKdCamat, hotel.AlamatOpKdLurah, hotel.WilayahPajak);
                        break;

                    case EnumFactory.EPajak.JasaParkir:
                        var parkir = context.DbOpParkirs
                            .Where(x => (x.Nop == keyword) || x.NamaOp.ToUpper().Contains(keyword.ToUpper()))
                            .FirstOrDefault(x => x.TahunBuku == currentYear);
                        if (parkir != null) MapIdentitas(ret, parkir.Nop, parkir.AlamatOp, parkir.AlamatOpKdCamat, parkir.AlamatOpKdLurah, parkir.WilayahPajak);
                        break;

                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var hiburan = context.DbOpHiburans
                            .Where(x => (x.Nop == keyword) || x.NamaOp.ToUpper().Contains(keyword.ToUpper()))
                            .FirstOrDefault(x => x.TahunBuku == currentYear);
                        if (hiburan != null) MapIdentitas(ret, hiburan.Nop, hiburan.AlamatOp, hiburan.AlamatOpKdCamat, hiburan.AlamatOpKdLurah, hiburan.WilayahPajak);
                        break;

                    case EnumFactory.EPajak.AirTanah:
                        var abt = context.DbOpAbts
                            .Where(x => (x.Nop == keyword) || x.NamaOp.ToUpper().Contains(keyword.ToUpper()))
                            .FirstOrDefault(x => x.TahunBuku == currentYear);
                        if (abt != null) MapIdentitas(ret, abt.Nop, abt.AlamatOp, abt.AlamatOpKdCamat, abt.AlamatOpKdLurah, abt.WilayahPajak);
                        break;

                    case EnumFactory.EPajak.Reklame:
                        // Masih kosong, bisa isi kalau datanya siap
                        break;

                    case EnumFactory.EPajak.PBB:
                        //var pbb = context.DbOpPbbs
                        //    .Where(x => (x.Nop == keyword) || x.WpNama.ToUpper().Contains(keyword.ToUpper()))
                        //    .FirstOrDefault(x => x.TahunBuku == currentYear);
                        //if (pbb != null) MapIdentitas(ret, pbb.Nop, pbb.AlamatOp, pbb.AlamatKdCamat, pbb.AlamatKdLurah, pbb.WilayahPajak);
                        break;

                    case EnumFactory.EPajak.BPHTB:
                    case EnumFactory.EPajak.OpsenPkb:
                    case EnumFactory.EPajak.OpsenBbnkb:
                        // Belum ada implementasi
                        break;

                    default:
                        break;
                }

                return ret;
            }
            private static void MapIdentitas(
            IdentitasObjekPajak ret,
            string nop,
            string alamatLengkap,
            string kecamatan,
            string kelurahan,
            string wilayah)
            {
                ret.NOP = nop;
                ret.AlamatLengkap = alamatLengkap;
                ret.Kecamatan = kecamatan;
                ret.Kelurahan = kelurahan;
                ret.WilayahPajak = string.IsNullOrEmpty(wilayah) ? wilayah : wilayah;
            }


            public static IdentitasObjekPajak GetDataPembayaranOPList(EnumFactory.EPajak jenisPajak, string keyword)
            {
                var ret = new IdentitasObjekPajak();
                var currentYear = DateTime.Now.Year;
                var context = DBClass.GetContext();
                
                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataResto = context.DbOpRestos.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var RestoTahunIni = dataResto.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (RestoTahunIni != null)
                        {
                            ret.NOP = RestoTahunIni.Nop;
                            ret.NamaObjekPajak = RestoTahunIni.NamaOp;
                            ret.AlamatLengkap = RestoTahunIni.AlamatOp;
                            ret.Kecamatan = RestoTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = RestoTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = RestoTahunIni.WilayahPajak;
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrik = context.DbOpListriks.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var ListrikTahunIni = dataListrik.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (ListrikTahunIni != null)
                        {
                            ret.NOP = ListrikTahunIni.Nop;
                            ret.NamaObjekPajak = ListrikTahunIni.NamaOp;
                            ret.AlamatLengkap = ListrikTahunIni.AlamatOp;
                            ret.Kecamatan = ListrikTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = ListrikTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = ListrikTahunIni.WilayahPajak;
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotel = context.DbOpHotels.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var HotelTahunIni = dataHotel.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (HotelTahunIni != null)
                        {
                            ret.NOP = HotelTahunIni.Nop;
                            ret.NamaObjekPajak = HotelTahunIni.NamaOp;
                            ret.AlamatLengkap = HotelTahunIni.AlamatOp;
                            ret.Kecamatan = HotelTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = HotelTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = HotelTahunIni.WilayahPajak;
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkir = context.DbOpParkirs.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var ParkirTahunIni = dataParkir.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (ParkirTahunIni != null)
                        {
                            ret.NOP = ParkirTahunIni.Nop;
                            ret.NamaObjekPajak = ParkirTahunIni.NamaOp;
                            ret.AlamatLengkap = ParkirTahunIni.AlamatOp;
                            ret.Kecamatan = ParkirTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = ParkirTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = ParkirTahunIni.WilayahPajak;
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburan = context.DbOpHiburans.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var HiburanTahunIni = dataHiburan.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (HiburanTahunIni != null)
                        {
                            ret.NOP = HiburanTahunIni.Nop;
                            ret.NamaObjekPajak = HiburanTahunIni.NamaOp;
                            ret.AlamatLengkap = HiburanTahunIni.AlamatOp;
                            ret.Kecamatan = HiburanTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = HiburanTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = HiburanTahunIni.WilayahPajak;
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbt = context.DbOpAbts.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var AbtTahunIni = dataAbt.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (AbtTahunIni != null)
                        {
                            ret.NOP = AbtTahunIni.Nop;
                            ret.NamaObjekPajak = AbtTahunIni.NamaOp;
                            ret.AlamatLengkap = AbtTahunIni.AlamatOp;
                            ret.Kecamatan = AbtTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = AbtTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = AbtTahunIni.WilayahPajak;
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var dataRekalme = context.DbOpReklames.Where(x => (x.Nop == keyword) || (x.Nama.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var RekalmeTahunIni = dataRekalme.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (RekalmeTahunIni != null)
                        {
                            ret.NOP = RekalmeTahunIni.Nop;
                            ret.NamaObjekPajak = RekalmeTahunIni.Nama;
                            ret.AlamatLengkap = RekalmeTahunIni.Alamat;
                            ret.Kecamatan = RekalmeTahunIni.Kecamatan;
                            ret.Kelurahan = RekalmeTahunIni.Nmkelurahan;
                            ret.WilayahPajak = "-";
                        }
                        break;
                    case EnumFactory.EPajak.PBB:
                        //var dataPbb = context.DbOpPbbs.Where(x => (x.Nop == keyword) || (x.WpNama.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        //var PbbTahunIni = dataPbb.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        //if (PbbTahunIni != null)
                        //{
                        //    ret.NOP = PbbTahunIni.Nop;
                        //    ret.NamaObjekPajak = PbbTahunIni.WpNama;
                        //    ret.AlamatLengkap = PbbTahunIni.AlamatOp;
                        //    ret.Kecamatan = PbbTahunIni.AlamatKdCamat;
                        //    ret.Kelurahan = PbbTahunIni.AlamatKdLurah;
                        //    ret.WilayahPajak = PbbTahunIni.WilayahPajak;
                        //}
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

            public static DataTahunPembayaran GetDataTahunanPembayaranList(EnumFactory.EPajak jenisPajak, string nop, int tahun)
            {
                var ret = new DataTahunPembayaran();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataResto = context.DbMonRestos.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new 
                                {
                                    TglBayarPokok = x.TglBayarPokok.Value 
                                })
                                .Select(x => new 
                                {
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        ret.Tahun = tahun;
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var rest = dataResto.Where(x => x.TglBayarPokok.Month == bln).OrderBy(x => x.TglBayarPokok).ToList();
                            decimal realisasi = rest.Sum(q => q.Realisasi) ?? 0;
                            var tanggal = rest.LastOrDefault()?.TglBayarPokok;

                            ret.DataRealisasi.Add(new DataRealisasiPajak
                            {
                                Bulan = new DateTime(DateTime.Now.Year, bln, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                TglSSPD = tanggal,
                                Realisasi = realisasi,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrik = context.DbMonPpjs.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        ret.Tahun = tahun;
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var rest = dataListrik.Where(x => x.TglBayarPokok.Month == bln).OrderBy(x => x.TglBayarPokok).ToList();
                            decimal realisasi = rest.Sum(q => q.Realisasi) ?? 0;
                            var tanggal = rest.LastOrDefault()?.TglBayarPokok;

                            ret.DataRealisasi.Add(new DataRealisasiPajak
                            {
                                Bulan = new DateTime(DateTime.Now.Year, bln, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                TglSSPD = tanggal,
                                Realisasi = realisasi,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotel = context.DbMonHotels.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        ret.Tahun = tahun;
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var rest = dataHotel.Where(x => x.TglBayarPokok.Month == bln).OrderBy(x => x.TglBayarPokok).ToList();
                            decimal realisasi = rest.Sum(q => q.Realisasi) ?? 0;
                            var tanggal = rest.LastOrDefault()?.TglBayarPokok;

                            ret.DataRealisasi.Add(new DataRealisasiPajak
                            {
                                Bulan = new DateTime(DateTime.Now.Year, bln, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                TglSSPD = tanggal,
                                Realisasi = realisasi,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkir = context.DbMonParkirs.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        ret.Tahun = tahun;
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var rest = dataParkir.Where(x => x.TglBayarPokok.Month == bln).OrderBy(x => x.TglBayarPokok).ToList();
                            decimal realisasi = rest.Sum(q => q.Realisasi) ?? 0;
                            var tanggal = rest.LastOrDefault()?.TglBayarPokok;

                            ret.DataRealisasi.Add(new DataRealisasiPajak
                            {
                                Bulan = new DateTime(DateTime.Now.Year, bln, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                TglSSPD = tanggal,
                                Realisasi = realisasi,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburan = context.DbMonHiburans.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        ret.Tahun = tahun;
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var rest = dataHiburan.Where(x => x.TglBayarPokok.Month == bln).OrderBy(x => x.TglBayarPokok).ToList();
                            decimal realisasi = rest.Sum(q => q.Realisasi) ?? 0;
                            var tanggal = rest.LastOrDefault()?.TglBayarPokok;

                            ret.DataRealisasi.Add(new DataRealisasiPajak
                            {
                                Bulan = new DateTime(DateTime.Now.Year, bln, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                TglSSPD = tanggal,
                                Realisasi = realisasi,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbt = context.DbMonAbts.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayarPokok.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayarPokok = x.TglBayarPokok.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayarPokok,
                                    Realisasi = x.Sum(q => q.NominalPokokBayar)
                                })
                            .ToList();
                        ret.Tahun = tahun;
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var rest = dataAbt.Where(x => x.TglBayarPokok.Month == bln).OrderBy(x => x.TglBayarPokok).ToList();
                            decimal realisasi = rest.Sum(q => q.Realisasi) ?? 0;
                            var tanggal = rest.LastOrDefault()?.TglBayarPokok;

                            ret.DataRealisasi.Add(new DataRealisasiPajak
                            {
                                Bulan = new DateTime(DateTime.Now.Year, bln, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                TglSSPD = tanggal,
                                Realisasi = realisasi,
                            });
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        break;
                    case EnumFactory.EPajak.PBB:

                        var dataPbb = context.DbMonPbbs.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayar.HasValue)
                            .GroupBy(x => new
                            {
                                TglBayar = x.TglBayar.Value
                            })
                                .Select(x => new
                                {
                                    x.Key.TglBayar,
                                    Realisasi = x.Sum(q => q.JumlahBayarPokok)
                                })
                            .ToList();
                        ret.Tahun = tahun;
                        for (int bln = 1; bln <= 12; bln++)
                        {
                            var rest = dataPbb.Where(x => x.TglBayar.Month == bln).OrderBy(x => x.TglBayar).ToList();
                            decimal realisasi = rest.Sum(q => q.Realisasi) ?? 0;
                            var tanggal = rest.LastOrDefault()?.TglBayar;

                            ret.DataRealisasi.Add(new DataRealisasiPajak
                            {
                                Bulan = new DateTime(DateTime.Now.Year, bln, 1).ToString("MMMM", new CultureInfo("id-ID")),
                                TglSSPD = tanggal,
                                Realisasi = realisasi,
                            });
                        }
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
        }


        public class IdentitasObjekPajak
        {
            public string NOP { get; set; }
            public string NamaObjekPajak { get; set; }
            public string AlamatLengkap { get; set; }
            public string Kecamatan { get; set; }
            public string Kelurahan { get; set; }
            public string WilayahPajak { get; set; }
        }

        public class DataTahunPembayaran
        {
            public int Tahun { get; set; }
            public List<DataRealisasiPajak> DataRealisasi { get; set; } = new();
            public decimal Total { get; set; }
        }

        public class DataRealisasiPajak
        {
            public string Bulan { get; set; }
            public DateTime? TglSSPD { get; set; }
            public decimal Realisasi { get; set; }
        }
    }
}
