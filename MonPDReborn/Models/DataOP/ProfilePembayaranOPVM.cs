using ClosedXML.Excel;
using MonPDLib;
using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq; // diperlukan untuk .Select()

namespace MonPDReborn.Models.DataOP
{
    public class ProfilePembayaranOPVM
    {
        public class Index
        {
            //public IdentitasObjekPajak IdentitasPajak { get; set; }

            //public MasaPajak MasaPajakData { get; set; } // PASTIKAN PUBLIC

            //public MasaBayar MasaBayarData { get; set; } // PASTIKAN PUBLIC

            //public Index()
            //{
            //    IdentitasPajak = new IdentitasObjekPajak
            //    {
            //        NOP = "35.78.120.120.0001.0",
            //        NamaObjekPajak = "Hotel Adam",
            //        AlamatLengkap = "Jl. Ada",
            //        Kecamatan_Kelurahan = "Ada - Ada"
            //    };

            //    var dataRealisasiBulanan = new List<DataRealisasiPajak>
            //    {
            //        new DataRealisasiPajak { Bulan = "Januari", TglSSPD = new DateTime(2025, 1, 15), Realisasi = 1000000m },
            //        new DataRealisasiPajak { Bulan = "Februari", TglSSPD = new DateTime(2025, 2, 15), Realisasi = 1200000m },
            //        new DataRealisasiPajak { Bulan = "Maret", TglSSPD = new DateTime(2025, 3, 15), Realisasi = 1300000m },
            //        new DataRealisasiPajak { Bulan = "April", TglSSPD = new DateTime(2025, 4, 15), Realisasi = 1250000m },
            //        new DataRealisasiPajak { Bulan = "Mei", TglSSPD = new DateTime(2025, 5, 15), Realisasi = 1400000m },
            //        new DataRealisasiPajak { Bulan = "Juni", TglSSPD = new DateTime(2025, 6, 15), Realisasi = 1100000m },
            //        new DataRealisasiPajak { Bulan = "Juli", TglSSPD = new DateTime(2025, 7, 15), Realisasi = 1500000m },
            //        new DataRealisasiPajak { Bulan = "Agustus", TglSSPD = new DateTime(2025, 8, 15), Realisasi = 1600000m },
            //        new DataRealisasiPajak { Bulan = "September", TglSSPD = new DateTime(2025, 9, 15), Realisasi = 1700000m },
            //        new DataRealisasiPajak { Bulan = "Oktober", TglSSPD = new DateTime(2025, 10, 15), Realisasi = 1800000m },
            //        new DataRealisasiPajak { Bulan = "November", TglSSPD = new DateTime(2025, 11, 15), Realisasi = 1900000m },
            //        new DataRealisasiPajak { Bulan = "Desember", TglSSPD = new DateTime(2025, 12, 15), Realisasi = 2000000m }
            //    };


            //    MasaPajakData = new MasaPajak
            //    {
            //        Tahun = 2025,
            //        DataRealisasi = dataRealisasiBulanan.Select(d => new DataRealisasiPajak
            //        {
            //            Bulan = d.Bulan,
            //            TglSSPD = d.TglSSPD,
            //            Realisasi = d.Realisasi
            //        }).ToList(),
            //        Total = dataRealisasiBulanan.Sum(d => d.Realisasi)
            //    };


            //    var dataRealisasiBayar = new List<DataRealisasiPajak>
            //    {
            //        new DataRealisasiPajak { Bulan = "Januari", TglSSPD = new DateTime(2025, 2, 10), Realisasi = 950000m },
            //        new DataRealisasiPajak { Bulan = "Februari", TglSSPD = new DateTime(2025, 3, 12), Realisasi = 1150000m },
            //        new DataRealisasiPajak { Bulan = "Maret", TglSSPD = new DateTime(2025, 4, 9), Realisasi = 1280000m },
            //        new DataRealisasiPajak { Bulan = "April", TglSSPD = new DateTime(2025, 5, 15), Realisasi = 1225000m },
            //        new DataRealisasiPajak { Bulan = "Mei", TglSSPD = new DateTime(2025, 6, 17), Realisasi = 1380000m },
            //        new DataRealisasiPajak { Bulan = "Juni", TglSSPD = new DateTime(2025, 7, 10), Realisasi = 1050000m },
            //        new DataRealisasiPajak { Bulan = "Juli", TglSSPD = new DateTime(2025, 8, 13), Realisasi = 1450000m },
            //        new DataRealisasiPajak { Bulan = "Agustus", TglSSPD = new DateTime(2025, 9, 11), Realisasi = 1580000m },
            //        new DataRealisasiPajak { Bulan = "September", TglSSPD = new DateTime(2025, 10, 14), Realisasi = 1650000m },
            //        new DataRealisasiPajak { Bulan = "Oktober", TglSSPD = new DateTime(2025, 11, 10), Realisasi = 1750000m },
            //        new DataRealisasiPajak { Bulan = "November", TglSSPD = new DateTime(2025, 12, 9), Realisasi = 1850000m },
            //        new DataRealisasiPajak { Bulan = "Desember", TglSSPD = new DateTime(2026, 1, 10), Realisasi = 1950000m }
            //    };

            //    MasaBayarData = new MasaBayar
            //    {
            //        Tahun = 2025,
            //        DataRealisasi = dataRealisasiBayar.Select(d => new DataRealisasiPajak
            //        {
            //            Bulan = d.Bulan,
            //            TglSSPD = d.TglSSPD,
            //            Realisasi = d.Realisasi
            //        }).ToList(),
            //        Total = dataRealisasiBayar.Sum(d => d.Realisasi)
            //    };
            //}
        }


        public class Show
        {
            // Tambahkan properti atau method jika diperlukan
        }

        public class Detail
        {
            // Tambahkan properti atau method jika diperlukan
        }

        public class Method
        {
            public static IdentitasObjekPajak GetDataPembayaranList(EnumFactory.EPajak jenisPajak, string keyword)
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
                            ret.AlamatLengkap = RestoTahunIni.AlamatOp;
                            ret.Kecamatan = RestoTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = RestoTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = RestoTahunIni.AlamatOp;
                        }
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataListrik = context.DbOpListriks.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var ListrikTahunIni = dataListrik.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (ListrikTahunIni != null)
                        {
                            ret.NOP = ListrikTahunIni.Nop;
                            ret.AlamatLengkap = ListrikTahunIni.AlamatOp;
                            ret.Kecamatan = ListrikTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = ListrikTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = ListrikTahunIni.AlamatOp;
                        }
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataHotel = context.DbOpHotels.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var HotelTahunIni = dataHotel.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (HotelTahunIni != null)
                        {
                            ret.NOP = HotelTahunIni.Nop;
                            ret.AlamatLengkap = HotelTahunIni.AlamatOp;
                            ret.Kecamatan = HotelTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = HotelTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = HotelTahunIni.AlamatOp;
                        }
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataParkir = context.DbOpParkirs.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var ParkirTahunIni = dataParkir.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (ParkirTahunIni != null)
                        {
                            ret.NOP = ParkirTahunIni.Nop;
                            ret.AlamatLengkap = ParkirTahunIni.AlamatOp;
                            ret.Kecamatan = ParkirTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = ParkirTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = ParkirTahunIni.AlamatOp;
                        }
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataHiburan = context.DbOpHiburans.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var HiburanTahunIni = dataHiburan.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (HiburanTahunIni != null)
                        {
                            ret.NOP = HiburanTahunIni.Nop;
                            ret.AlamatLengkap = HiburanTahunIni.AlamatOp;
                            ret.Kecamatan = HiburanTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = HiburanTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = HiburanTahunIni.AlamatOp;
                        }
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataAbt = context.DbOpAbts.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var AbtTahunIni = dataAbt.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (AbtTahunIni != null)
                        {
                            ret.NOP = AbtTahunIni.Nop;
                            ret.AlamatLengkap = AbtTahunIni.AlamatOp;
                            ret.Kecamatan = AbtTahunIni.AlamatOpKdCamat;
                            ret.Kelurahan = AbtTahunIni.AlamatOpKdLurah;
                            ret.WilayahPajak = AbtTahunIni.AlamatOp;
                        }
                        break;
                    case EnumFactory.EPajak.Reklame:
                        //var dataRekalme = context.DbOpRekalmes.Where(x => (x.Nop == keyword) || (x.NamaOp.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        //var RekalmeTahunIni = dataRekalme.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        //if (RekalmeTahunIni != null)
                        //{
                        //    ret.NOP = RekalmeTahunIni.Nop;
                        //    ret.AlamatLengkap = RekalmeTahunIni.AlamatOp;
                        //    ret.Kecamatan = RekalmeTahunIni.AlamatOpKdCamat;
                        //    ret.Kelurahan = RekalmeTahunIni.AlamatOpKdLurah;
                        //    ret.WilayahPajak = RekalmeTahunIni.AlamatOp;
                        //}
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataPbb = context.DbOpPbbs.Where(x => (x.Nop == keyword) || (x.WpNama.ToUpper().Contains(keyword.ToUpper()))).ToList();
                        var PbbTahunIni = dataPbb.Where(x => x.TahunBuku == currentYear).FirstOrDefault();
                        if (PbbTahunIni != null)
                        {
                            ret.NOP = PbbTahunIni.Nop;
                            ret.AlamatLengkap = PbbTahunIni.AlamatOp;
                            ret.Kecamatan = PbbTahunIni.AlamatKdCamat;
                            ret.Kelurahan = PbbTahunIni.AlamatKdLurah;
                            ret.WilayahPajak = PbbTahunIni.AlamatOp;
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
                        for (int bln = 1; bln < 12; bln++)
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
                        for (int bln = 1; bln < 12; bln++)
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
                        for (int bln = 1; bln < 12; bln++)
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
                        for (int bln = 1; bln < 12; bln++)
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
                        for (int bln = 1; bln < 12; bln++)
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
                        for (int bln = 1; bln < 12; bln++)
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

                        var dataPbb = context.DbMonPbbs.Where(x => x.Nop == nop && x.TahunBuku == tahun && x.TglBayarPokok.HasValue)
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
                        for (int bln = 1; bln < 12; bln++)
                        {
                            var rest = dataPbb.Where(x => x.TglBayarPokok.Month == bln).OrderBy(x => x.TglBayarPokok).ToList();
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
