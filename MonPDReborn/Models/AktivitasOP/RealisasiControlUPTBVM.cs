using DevExpress.CodeParser;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Models.MonitoringWilayah.MonitoringWilayahVM;

namespace MonPDReborn.Models.AktivitasOP
{
    public class RealisasiControlUPTBVM
    {
        public class Index
        {
            public DashboardData Data { get; set; } = new();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }

        public class Show
        {
            public List<DataRealisasi> DataRealisasiList { get; set; } = new();
            public Show(int wilayah)
            {
                DataRealisasiList = Method.GetDataRealisasi(wilayah);
            }
        }

        //public class Detail
        //{
        //    public List<DataRealisasiUptb> DataDetailList { get; set; } = new();
        //    public Detail()
        //    {
        //        DataDetailList = Method.GetDataRealisasiUptb();
        //    }
        //}

        public class ShowPembandingA
        {
            public List<DataRealisasi> DataRealisasiList { get; set; } = new();
            public ShowPembandingA(int tahun, int wilayah)
            {
                DataRealisasiList = Method.GetDataRealisasi(tahun, wilayah);
            }
        }

        public class ShowPembandingB
        {
            public List<DataRealisasi> DataRealisasiList { get; set; } = new();
            public ShowPembandingB(int tahun, int wilayah)
            {
                DataRealisasiList = Method.GetDataRealisasi(tahun, wilayah);
            }
        }

        public class Method
        {
            public static DashboardData GetDashboardData()
            {
                return new DashboardData
                {
                    TotalTarget = 15000000000,
                    TotalRealisasi = 12500000000,
                    PersentaseCapaian = (12500000000m / 15000000000m) * 100
                };
            }
            public static List<DataRealisasi> GetDataRealisasi(int wilayah)
            {
                var ret = new List<DataRealisasi>();
                var context = DBClass.GetContext();
                var TanggalCutOff = DateTime.Now;

                //Target [BIMOUW]
                var dataTarget = context.DbAkunTargetBulanUptbs
                    .Where(x =>
                        x.TahunBuku == TanggalCutOff.Year &&
                        x.Uptb == wilayah &&
                        x.PajakId != 7 &&
                        x.PajakId != 12 &&
                        x.PajakId != 20 &&
                        x.PajakId != 21 &&
                        (
                            (x.PajakId == 2 && x.SubRincian == "2") || 
                            (x.PajakId != 2)                           
                        )
                    )
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetAKP [BIMOUW]
                var dataTargetAkp = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && 
                                x.Bulan == TanggalCutOff.Month && 
                                x.Uptb == wilayah && 
                                x.PajakId != 7 && 
                                x.PajakId != 12 && 
                                x.PajakId != 20 && 
                                x.PajakId != 21 &&
                                (
                                    (x.PajakId == 2 && x.SubRincian == "2") ||
                                    (x.PajakId != 2)
                                )
                    )
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetSdBulanIni [BIMOUW]
                var dataTargetAkpSdBulanIni = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && 
                                x.Bulan <= TanggalCutOff.Month && 
                                x.Uptb == wilayah && 
                                x.PajakId != 7 && 
                                x.PajakId != 12 && 
                                x.PajakId != 20 && 
                                x.PajakId != 21 &&
                                (
                                    (x.PajakId == 2 && x.SubRincian == "2") ||
                                    (x.PajakId != 2)
                                )
                    )
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //Ambil Wilayah
                var nopListSemuaAbt = context.DbOpAbts
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaResto = context.DbOpRestos
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaHotel = context.DbOpHotels
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaListrik = context.DbOpListriks
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year) && x.Sumber == 55)
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaParkir = context.DbOpParkirs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaHiburan = context.DbOpHiburans
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();

                //// RealisasiSdBulanIn
                //var dataRealisasiSdBulaiIniMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff && x.TahunBuku == TanggalCutOff.Year && x.JumlahBayarPokok > 0).Sum(x => x.JumlahBayarPokok) ?? 0;
                //var dataRealisasiSdBulaiIniBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.Pokok) ?? 0;
                //var dataRealisasiSdBulaiIniAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);
                //var dataRealisasiSdBulaiIniOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);

                // RealisasiSD
                var dataRealisasiSdBulaiIniMamin = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaResto.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaHotel.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaHiburan.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaParkir.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaListrik.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs
                    .Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year
                            && x.TglBayar.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayar.Value <= TanggalCutOff
                            && x.TahunBuku == TanggalCutOff.Year
                            && x.JumlahBayarPokok > 0
                            && x.Uptb == Convert.ToInt32(wilayah))
                    .Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiSdBulaiIniAbt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && x.TahunBuku == TanggalCutOff.Year
                            && nopListSemuaAbt.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;

                var dataRealisasiSdBulanIni = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiSdBulaiIniMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiSdBulaiIniHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiSdBulaiIniHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiSdBulaiIniParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiSdBulaiIniListrik),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiSdBulaiIniPbb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiSdBulaiIniAbt),
                };

                // RealisasiBulanIni
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaResto.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaHotel.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaHiburan.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaParkir.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaListrik.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value.Month == TanggalCutOff.Month && x.TahunBuku == TanggalCutOff.Year && x.JumlahBayarPokok > 0 && x.Uptb == Convert.ToInt32(wilayah)).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaAbt.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var dataRealisasi = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiListrik),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiPbb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiAbt),
                };

                var pajakList = context.MPajaks.Where(x => x.Id != 7 && x.Id != 12 && x.Id != 20 && x.Id != 21).ToList();
                foreach (var item in pajakList)
                {
                    decimal target = dataTarget.Where(x => x.PajakId == item.Id).Sum(q => q.Target);
                    decimal targetAkp = dataTargetAkp.Where(x => x.PajakId == item.Id).Sum(q => q.Target);
                    decimal targetAkpSdBulanIni = dataTargetAkpSdBulanIni.Where(x => x.PajakId == item.Id).Sum(q => q.Target);

                    decimal realisasi = dataRealisasi.Where(x => x.PajakId == item.Id).Sum(q => q.Nominal);
                    decimal realisasiSdBulanIni = dataRealisasiSdBulanIni.Where(x => x.PajakId == item.Id).Sum(q => q.Nominal);

                    ret.Add(new DataRealisasi
                    {
                        No = item.Id,
                        JenisPajak = item.Nama,
                        Target = target,
                        PembayaranBulanIni = new PembayaranDetail
                        {
                            AKP = targetAkp,
                            Realisasi = realisasi,
                            Persen = targetAkp > 0 ? (realisasi / targetAkp) * 100 : 0
                        },
                        PembayaranSDBI = new PembayaranDetailSDBI
                        {
                            AKP = targetAkpSdBulanIni,
                            Realisasi = realisasiSdBulanIni,
                            PersenAkpTarget = target > 0 ? (targetAkpSdBulanIni / target) * 100 : 0,
                            PersenAkpRealisasi = targetAkpSdBulanIni > 0 ? (realisasiSdBulanIni / targetAkpSdBulanIni) * 100 : 0,
                            PersenTarget = target > 0 ? (realisasiSdBulanIni / target) * 100 : 0,
                        }
                    });
                }
                return ret;
            }
            public static List<DataRealisasi> GetDataRealisasi(int tahun, int wilayah)
            {
                var ret = new List<DataRealisasi>();
                var context = DBClass.GetContext();
                var TanggalCutOff = new DateTime(tahun, DateTime.Now.Month, 1);

                //Target [BIMOUW]
                var dataTarget = context.DbAkunTargetBulanUptbs
                    .Where(x =>
                        x.TahunBuku == TanggalCutOff.Year &&
                        x.Uptb == wilayah &&
                        x.PajakId != 7 &&
                        x.PajakId != 12 &&
                        x.PajakId != 20 &&
                        x.PajakId != 21 &&
                        (
                            (x.PajakId == 2 && x.SubRincian == "2") ||
                            (x.PajakId != 2)
                        )
                    )
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetAKP [BIMOUW]
                var dataTargetAkp = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year &&
                                x.Bulan == TanggalCutOff.Month &&
                                x.Uptb == wilayah &&
                                x.PajakId != 7 &&
                                x.PajakId != 12 &&
                                x.PajakId != 20 &&
                                x.PajakId != 21 &&
                                (
                                    (x.PajakId == 2 && x.SubRincian == "2") ||
                                    (x.PajakId != 2)
                                )
                    )
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetSdBulanIni [BIMOUW]
                var dataTargetAkpSdBulanIni = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year &&
                                x.Bulan <= TanggalCutOff.Month &&
                                x.Uptb == wilayah &&
                                x.PajakId != 7 &&
                                x.PajakId != 12 &&
                                x.PajakId != 20 &&
                                x.PajakId != 21 &&
                                (
                                    (x.PajakId == 2 && x.SubRincian == "2") ||
                                    (x.PajakId != 2)
                                )
                    )
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //Ambil Wilayah
                var nopListSemuaAbt = context.DbOpAbts
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaResto = context.DbOpRestos
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaHotel = context.DbOpHotels
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaListrik = context.DbOpListriks
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year) && x.Sumber == 55)
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaParkir = context.DbOpParkirs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();
                var nopListSemuaHiburan = context.DbOpHiburans
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.WilayahPajak == ((int)wilayah).ToString() && (x.TglOpTutup.HasValue == false || x.TglOpTutup.Value.Year > TanggalCutOff.Year))
                    .Select(x => x.Nop).Distinct().ToList();

                //// RealisasiSdBulanIn
                //var dataRealisasiSdBulaiIniMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff && x.TahunBuku == TanggalCutOff.Year && x.JumlahBayarPokok > 0).Sum(x => x.JumlahBayarPokok) ?? 0;
                //var dataRealisasiSdBulaiIniBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.Pokok) ?? 0;
                //var dataRealisasiSdBulaiIniAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);
                //var dataRealisasiSdBulaiIniOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);

                // RealisasiSD
                var dataRealisasiSdBulaiIniMamin = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaResto.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaHotel.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaHiburan.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaParkir.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaListrik.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs
                    .Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year
                            && x.TglBayar.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayar.Value <= TanggalCutOff
                            && x.TahunBuku == TanggalCutOff.Year
                            && x.JumlahBayarPokok > 0
                            && x.Uptb == Convert.ToInt32(wilayah))
                    .Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiSdBulaiIniAbt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1)
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && x.TahunBuku == TanggalCutOff.Year
                            && nopListSemuaAbt.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;

                var dataRealisasiSdBulanIni = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiSdBulaiIniMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiSdBulaiIniHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiSdBulaiIniHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiSdBulaiIniParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiSdBulaiIniListrik),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiSdBulaiIniPbb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiSdBulaiIniAbt),
                };

                // RealisasiBulanIni
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaResto.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaHotel.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaHiburan.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaParkir.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaListrik.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value.Month == TanggalCutOff.Month && x.TahunBuku == TanggalCutOff.Year && x.JumlahBayarPokok > 0 && x.Uptb == Convert.ToInt32(wilayah)).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month && nopListSemuaAbt.Contains(x.Nop)).Sum(x => x.NominalPokokBayar) ?? 0;

                var dataRealisasi = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiListrik),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiPbb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiAbt),
                };

                var pajakList = context.MPajaks.Where(x => x.Id != 7 && x.Id != 12 && x.Id != 20 && x.Id != 21).ToList();
                foreach (var item in pajakList)
                {
                    decimal target = dataTarget.Where(x => x.PajakId == item.Id).Sum(q => q.Target);
                    decimal targetAkp = dataTargetAkp.Where(x => x.PajakId == item.Id).Sum(q => q.Target);
                    decimal targetAkpSdBulanIni = dataTargetAkpSdBulanIni.Where(x => x.PajakId == item.Id).Sum(q => q.Target);

                    decimal realisasi = dataRealisasi.Where(x => x.PajakId == item.Id).Sum(q => q.Nominal);
                    decimal realisasiSdBulanIni = dataRealisasiSdBulanIni.Where(x => x.PajakId == item.Id).Sum(q => q.Nominal);

                    ret.Add(new DataRealisasi
                    {
                        No = item.Id,
                        JenisPajak = item.Nama,
                        Target = target,
                        PembayaranBulanIni = new PembayaranDetail
                        {
                            AKP = targetAkp,
                            Realisasi = realisasi,
                            Persen = targetAkp > 0 ? (realisasi / targetAkp) * 100 : 0
                        },
                        PembayaranSDBI = new PembayaranDetailSDBI
                        {
                            AKP = targetAkpSdBulanIni,
                            Realisasi = realisasiSdBulanIni,
                            PersenAkpTarget = target > 0 ? (targetAkpSdBulanIni / target) * 100 : 0,
                            PersenAkpRealisasi = targetAkpSdBulanIni > 0 ? (realisasiSdBulanIni / targetAkpSdBulanIni) * 100 : 0,
                            PersenTarget = target > 0 ? (realisasiSdBulanIni / target) * 100 : 0,
                        }
                    });
                }
                return ret;
            }
        }

        public class DashboardData
        {
            public decimal TotalTarget { get; set; }
            public decimal TotalRealisasi { get; set; }
            public decimal PersentaseCapaian { get; set; }
        }
        public class DataRealisasi
        {
            public int No { get; set; }
            public string JenisPajak { get; set; } = "";
            public decimal Target { get; set; }
            public PembayaranDetail PembayaranBulanIni { get; set; } = new();
            public PembayaranDetailSDBI PembayaranSDBI { get; set; } = new();
        }
        public class PembayaranDetail
        {
            public decimal AKP { get; set; }
            public decimal Realisasi { get; set; }
            public decimal Persen { get; set; }
        }
        public class PembayaranDetailSDBI : PembayaranDetail
        {
            public decimal PersenAkpTarget { get; set; }
            public decimal PersenAkpRealisasi { get; set; }
            public decimal PersenTarget { get; set; }
        }
        public class DataDetailRealisasi
        {
            public int No { get; set; }
            public string JenisPajak { get; set; } = "";
            public RealisasiPerLokasi UPTB1 { get; set; } = new();
            public RealisasiPerLokasi UPTB2 { get; set; } = new();
            public RealisasiPerLokasi UPTB3 { get; set; } = new();
            public RealisasiPerLokasi UPTB4 { get; set; } = new();
            public RealisasiPerLokasi UPTB5 { get; set; } = new();
            public RealisasiPerLokasi Bidang { get; set; } = new();
        }
        public class RealisasiPerLokasi
        {
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public decimal Persen { get; set; }
        }

        public class DataRealisasiUptb
        {
            public int PajakId { get; set; }
            public string PajakNama { get; set; } = string.Empty;
            public List<DataRealisasiUptbWilayah> Uptbs { get; set; } = new List<DataRealisasiUptbWilayah>();
        }

        public class DataRealisasiUptbWilayah
        {
            public int UptbId { get; set; }
            public string UptbNama { get; set; }
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public decimal Persen { get; set; }
        }
    }
}
