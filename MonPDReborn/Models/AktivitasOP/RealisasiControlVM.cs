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
    public class RealisasiControlVM
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
            public Show()
            {
                DataRealisasiList = Method.GetDataRealisasi();
            }
        }

        public class Detail
        {
            public List<DataRealisasiUptb> DataDetailList { get; set; } = new();
            public Detail()
            {
                DataDetailList = Method.GetDataRealisasiUptb();
            }
        }

        public class ShowPembandingA
        {
            public List<DataRealisasi> DataRealisasiList { get; set; } = new();
            public ShowPembandingA(int tahun)
            {
                DataRealisasiList = Method.GetDataRealisasi(tahun);
            }
        }

        public class ShowPembandingB
        {
            public List<DataRealisasi> DataRealisasiList { get; set; } = new();
            public ShowPembandingB(int tahun)
            {
                DataRealisasiList = Method.GetDataRealisasi(tahun);
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
            public static List<DataRealisasi> GetDataRealisasi()
            {
                var ret = new List<DataRealisasi>();
                var context = DBClass.GetContext();
                var TanggalCutOff = DateTime.Now;

                //Target [BIMOUW]
                var dataTarget = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year)
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetAKP [BIMOUW]
                var dataTargetAkp = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month)
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetSdBulanIni [BIMOUW]
                var dataTargetAkpSdBulanIni = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month)
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                // RealisasiSdBulanIn
                var dataRealisasiSdBulaiIniMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiSdBulaiIniBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiSdBulaiIniAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);
                var dataRealisasiSdBulaiIniOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);

                var dataRealisasiSdBulanIni = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiSdBulaiIniMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiSdBulaiIniHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiSdBulaiIniHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiSdBulaiIniParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiSdBulaiIniListrik),
                    ((int)EnumFactory.EPajak.Reklame, dataRealisasiSdBulaiIniReklame),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiSdBulaiIniPbb),
                    ((int)EnumFactory.EPajak.BPHTB, dataRealisasiSdBulaiIniBphtb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiSdBulaiIniAbt),
                    ((int)EnumFactory.EPajak.OpsenPkb, dataRealisasiSdBulaiIniOpsenPkb),
                    ((int)EnumFactory.EPajak.OpsenBbnkb, dataRealisasiSdBulaiIniOpsenBbnkb)
                };

                // RealisasiBulanIni
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value.Month == TanggalCutOff.Month).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value.Month >= TanggalCutOff.Month).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd.Month == TanggalCutOff.Month).Sum(x => x.JmlPokok);
                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd.Month == TanggalCutOff.Month).Sum(x => x.JmlPokok);

                var dataRealisasi = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiListrik),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiPbb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiAbt),
                    ((int)EnumFactory.EPajak.BPHTB, dataRealisasiBphtb),
                    ((int)EnumFactory.EPajak.Reklame, dataRealisasiReklame),
                    ((int)EnumFactory.EPajak.OpsenPkb, dataRealisasiOpsenPkb),
                    ((int)EnumFactory.EPajak.OpsenBbnkb, dataRealisasiOpsenBbnkb)
                };

                var pajakList = context.MPajaks.ToList();
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
            public static List<DataRealisasi> GetDataRealisasi(int tahun)
            {
                var ret = new List<DataRealisasi>();
                var context = DBClass.GetContext();
                var TanggalCutOff = new DateTime(tahun, DateTime.Now.Month, 1);

                //Target [BIMOUW]
                var dataTarget = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year)
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetAKP [BIMOUW]
                var dataTargetAkp = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month)
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                //TargetSdBulanIni [BIMOUW]
                var dataTargetAkpSdBulanIni = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month)
                    .GroupBy(x => new
                    {
                        x.PajakId
                    }).Select(x => new
                    {
                        PajakId = x.Key.PajakId,
                        Target = x.Sum(y => y.Target)
                    }).ToList();

                // RealisasiSdBulanIn
                var dataRealisasiSdBulaiIniMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiSdBulaiIniBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiSdBulaiIniAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);
                var dataRealisasiSdBulaiIniOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);

                var dataRealisasiSdBulanIni = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiSdBulaiIniMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiSdBulaiIniHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiSdBulaiIniHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiSdBulaiIniParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiSdBulaiIniListrik),
                    ((int)EnumFactory.EPajak.Reklame, dataRealisasiSdBulaiIniReklame),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiSdBulaiIniPbb),
                    ((int)EnumFactory.EPajak.BPHTB, dataRealisasiSdBulaiIniBphtb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiSdBulaiIniAbt),
                    ((int)EnumFactory.EPajak.OpsenPkb, dataRealisasiSdBulaiIniOpsenPkb),
                    ((int)EnumFactory.EPajak.OpsenBbnkb, dataRealisasiSdBulaiIniOpsenBbnkb)
                };

                // RealisasiBulanIni
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value.Month == TanggalCutOff.Month).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value.Month == TanggalCutOff.Month).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd.Month == TanggalCutOff.Month).Sum(x => x.JmlPokok);
                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd.Month == TanggalCutOff.Month).Sum(x => x.JmlPokok);

                var dataRealisasi = new List<(int PajakId, decimal Nominal)>()
                {
                    ((int)EnumFactory.EPajak.MakananMinuman, dataRealisasiMamin),
                    ((int)EnumFactory.EPajak.JasaPerhotelan, dataRealisasiHotel),
                    ((int)EnumFactory.EPajak.JasaKesenianHiburan, dataRealisasiHiburan),
                    ((int)EnumFactory.EPajak.JasaParkir, dataRealisasiParkir),
                    ((int)EnumFactory.EPajak.TenagaListrik, dataRealisasiListrik),
                    ((int)EnumFactory.EPajak.PBB, dataRealisasiPbb),
                    ((int)EnumFactory.EPajak.AirTanah, dataRealisasiAbt),
                    ((int)EnumFactory.EPajak.BPHTB, dataRealisasiBphtb),
                    ((int)EnumFactory.EPajak.Reklame, dataRealisasiReklame),
                    ((int)EnumFactory.EPajak.OpsenPkb, dataRealisasiOpsenPkb),
                    ((int)EnumFactory.EPajak.OpsenBbnkb, dataRealisasiOpsenBbnkb)
                };

                var pajakList = context.MPajaks.ToList();
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
            public static List<DataDetailRealisasi> GetDataDetail()
            {
                var ret = new List<DataDetailRealisasi>();

                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;

                var dataTargetWilayah = context.DbAkunTargetBulanUptbs
                           .Where(x => x.TahunBuku == currentYear && x.Bulan <= currentMonth)
                           .GroupBy(x => new { x.Uptb, x.PajakId })
                           .Select(g => new
                           {
                               Uptb = g.Key.Uptb,
                               PajakId = g.Key.PajakId,
                               TotalTarget = g.Sum(x => x.Target)
                           })
                           .ToList();

                var dataWilayahGabungan = new List<(string Nop, string Wilayah, decimal PajakId)>();

                dataWilayahGabungan.AddRange(
                    context.DbOpRestos
                        .Where(x => x.TahunBuku == currentYear)
                        .Select(x => new
                        {
                            x.Nop,
                            x.WilayahPajak,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.WilayahPajak, x.PajakId))
                        .ToList()
                );

                dataWilayahGabungan.AddRange(
                    context.DbOpHotels
                        .Where(x => x.TahunBuku == currentYear)
                        .Select(x => new
                        {
                            x.Nop,
                            x.WilayahPajak,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.WilayahPajak, x.PajakId))
                        .ToList()
                );

                dataWilayahGabungan.AddRange(
                    context.DbOpParkirs
                        .Where(x => x.TahunBuku == currentYear)
                        .Select(x => new
                        {
                            x.Nop,
                            x.WilayahPajak,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.WilayahPajak, x.PajakId))
                        .ToList()
                );

                dataWilayahGabungan.AddRange(
                    context.DbOpListriks
                        .Where(x => x.TahunBuku == currentYear)
                        .Select(x => new
                        {
                            x.Nop,
                            x.WilayahPajak,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.WilayahPajak, x.PajakId))
                        .ToList()
                );

                dataWilayahGabungan.AddRange(
                    context.DbOpHiburans
                        .Where(x => x.TahunBuku == currentYear)
                        .Select(x => new
                        {
                            x.Nop,
                            x.WilayahPajak,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.WilayahPajak, x.PajakId))
                        .ToList()
                );

                dataWilayahGabungan.AddRange(
                    context.DbOpAbts
                        .Where(x => x.TahunBuku == currentYear)
                        .Select(x => new
                        {
                            x.Nop,
                            x.WilayahPajak,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.WilayahPajak, x.PajakId))
                        .ToList()
                );

                dataWilayahGabungan.AddRange(
                    context.DbOpReklames
                        .Where(x => x.TahunBuku == currentYear)
                        .Select(x => new
                        {
                            x.Nop,
                            Wilayah = 0.ToString(),
                            PajakId = 7m // Reklame
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.Wilayah, x.PajakId))
                        .ToList()
                );

                dataWilayahGabungan.AddRange(
                    context.DbOpPbbs
                        .Select(x => new
                        {
                            x.Nop,
                            x.Uptb,
                            PajakId = 9m // PBB
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.Uptb.ToString(), x.PajakId))
                        .ToList()
                );

                // Gabungkan data realisasi
                var dataRealisasiGabungan = new List<(string Nop, DateTime? TglBayarPokok, decimal NominalPokokBayar, decimal PajakId)>();

                dataRealisasiGabungan.AddRange(
                    context.DbMonRestos
                        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayarPokok,
                            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        .ToList()
                );

                dataRealisasiGabungan.AddRange(
                    context.DbMonHotels
                        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayarPokok,
                            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        .ToList()
                );

                dataRealisasiGabungan.AddRange(
                    context.DbMonParkirs
                        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayarPokok,
                            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        .ToList()
                );

                dataRealisasiGabungan.AddRange(
                    context.DbMonPpjs
                        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayarPokok,
                            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        .ToList()
                );

                dataRealisasiGabungan.AddRange(
                    context.DbMonHiburans
                        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayarPokok,
                            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        .ToList()
                );

                dataRealisasiGabungan.AddRange(
                    context.DbMonAbts
                        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayarPokok,
                            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                            x.PajakId
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        .ToList()
                );

                dataRealisasiGabungan.AddRange(
                    context.DbMonReklames
                        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayarPokok,
                            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                            PajakId = 7m // Reklame
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                        .ToList()
                );

                dataRealisasiGabungan.AddRange(
                    context.DbMonPbbs
                        .Where(x => x.TahunBuku == currentYear && x.TglBayar.HasValue && x.TglBayar.Value.Month <= currentMonth)
                        .Select(x => new
                        {
                            x.Nop,
                            x.TglBayar,
                            NominalPokokBayar = x.JumlahBayarPokok ?? 0,
                            PajakId = 9
                        })
                        .ToList()
                        .Select(x => (x.Nop, x.TglBayar, x.NominalPokokBayar, Convert.ToDecimal(x.PajakId)))
                        .ToList()
                );

                var groupedByPajak = dataTargetWilayah
                    .GroupBy(x => x.PajakId)
                    .ToList();

                foreach (var group in groupedByPajak)
                {
                    var dataDetail = new DataDetailRealisasi
                    {
                        No = ret.Count + 1,
                        JenisPajak = ((EnumFactory.EPajak)group.Key).GetDescription(),
                        UPTB1 = new RealisasiPerLokasi(),
                        UPTB2 = new RealisasiPerLokasi(),
                        UPTB3 = new RealisasiPerLokasi(),
                        UPTB4 = new RealisasiPerLokasi(),
                        UPTB5 = new RealisasiPerLokasi(),
                        Bidang = new RealisasiPerLokasi() // jika ingin total semua UPTB
                    };

                    foreach (var item in group)
                    {
                        var nopUptb = dataWilayahGabungan
                            .Where(x => Convert.ToInt32(x.Wilayah) == item.Uptb && x.PajakId == item.PajakId)
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        var totalRealisasi = dataRealisasiGabungan
                            .Where(x => x.PajakId == item.PajakId && x.TglBayarPokok.Value.Month == currentMonth && nopUptb.Contains(x.Nop))
                            .Sum(x => x.NominalPokokBayar);

                        var lokasi = new RealisasiPerLokasi
                        {
                            Target = item.TotalTarget,
                            Realisasi = totalRealisasi,
                            Persen = item.TotalTarget > 0 ? (totalRealisasi / item.TotalTarget) * 100 : 0
                        };

                        switch (item.Uptb)
                        {
                            case 1:
                                dataDetail.UPTB1 = lokasi;
                                break;
                            case 2:
                                dataDetail.UPTB2 = lokasi;
                                break;
                            case 3:
                                dataDetail.UPTB3 = lokasi;
                                break;
                            case 4:
                                dataDetail.UPTB4 = lokasi;
                                break;
                            case 5:
                                dataDetail.UPTB5 = lokasi;
                                break;
                        }

                        // Hitung total per bidang (semua UPTB)
                        dataDetail.Bidang.Target += item.TotalTarget;
                        dataDetail.Bidang.Realisasi += totalRealisasi;
                    }

                    // Hitung persentase Bidang
                    dataDetail.Bidang.Persen = dataDetail.Bidang.Target > 0
                        ? (dataDetail.Bidang.Realisasi / dataDetail.Bidang.Target) * 100
                        : 0;

                    ret.Add(dataDetail);
                }
                return ret;
            }
            public static List<DataRealisasiUptb> GetDataRealisasiUptb()
            {
                var result = new List<DataRealisasiUptb>();

                var context = DBClass.GetContext();
                var currentDate = DateTime.Now;
                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;

                var dataOpRestos = context.DbOpRestos
                    .Where(x => x.TahunBuku == currentYear && (!string.IsNullOrEmpty(x.WilayahPajak)))
                    .Select(x => new
                    {
                        x.Nop,
                        WilayahPajak = Convert.ToInt32(x.WilayahPajak),
                        PajakId = (int)EnumFactory.EPajak.MakananMinuman
                    })
                    .Distinct()
                    .AsEnumerable();

                var dataOpListrik = context.DbOpListriks
                    .Where(x => x.TahunBuku == currentYear)
                    .Select(x => new
                    {
                        x.Nop,
                        WilayahPajak = Convert.ToInt32(x.WilayahPajak),
                        PajakId = (int)EnumFactory.EPajak.TenagaListrik
                    })
                    .Distinct()
                    .AsEnumerable();

                var dataOpHotel = context.DbOpHotels
                    .Where(x => x.TahunBuku == currentYear)
                    .Select(x => new
                    {
                        x.Nop,
                        WilayahPajak = Convert.ToInt32(x.WilayahPajak),
                        PajakId = (int)EnumFactory.EPajak.JasaPerhotelan
                    })
                    .Distinct()
                    .AsEnumerable();

                var dataOpParkir = context.DbOpParkirs
                    .Where(x => x.TahunBuku == currentYear)
                    .Select(x => new
                    {
                        x.Nop,
                        WilayahPajak = Convert.ToInt32(x.WilayahPajak),
                        PajakId = (int)EnumFactory.EPajak.JasaParkir
                    })
                    .Distinct()
                    .AsEnumerable();

                var dataOpHiburan = context.DbOpHiburans
                    .Where(x => x.TahunBuku == currentYear && (!string.IsNullOrEmpty(x.WilayahPajak)))
                    .Select(x => new
                    {
                        x.Nop,
                        WilayahPajak = Convert.ToInt32(x.WilayahPajak),
                        PajakId = (int)EnumFactory.EPajak.JasaKesenianHiburan
                    })
                    .Distinct()
                    .AsEnumerable();

                var dataOpAbt = context.DbOpAbts
                    .Where(x => x.TahunBuku == currentYear)
                    .Select(x => new
                    {
                        x.Nop,
                        WilayahPajak = Convert.ToInt32(x.WilayahPajak),
                        PajakId = (int)EnumFactory.EPajak.AirTanah
                    })
                    .Distinct()
                    .AsEnumerable();

                var dataNopWilayah = new List<(string Nop, int PajakId, int WilayahPajak)>();
                dataNopWilayah.AddRange(dataOpRestos.Select(x => (x.Nop, x.PajakId, x.WilayahPajak)));
                dataNopWilayah.AddRange(dataOpListrik.Select(x => (x.Nop, x.PajakId, x.WilayahPajak)));
                dataNopWilayah.AddRange(dataOpHotel.Select(x => (x.Nop, x.PajakId, x.WilayahPajak)));
                dataNopWilayah.AddRange(dataOpParkir.Select(x => (x.Nop, x.PajakId, x.WilayahPajak)));
                dataNopWilayah.AddRange(dataOpHiburan.Select(x => (x.Nop, x.PajakId, x.WilayahPajak)));
                dataNopWilayah.AddRange(dataOpAbt.Select(x => (x.Nop, x.PajakId, x.WilayahPajak)));

                var dataTarget = context.DbAkunTargetBulanUptbs.Where(x => x.TahunBuku == currentYear)
                   .GroupBy(x => new
                   {
                       x.PajakId,
                       x.Uptb
                   }).Select(x => new
                   {
                       PajakId = x.Key.PajakId,
                       Uptb = Convert.ToInt32(x.Key.Uptb),
                       Target = x.Sum(y => y.Target)
                   }).ToList();
                var dataRealisasiSdBulaiIniMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == currentYear && x.TglBayarPokok.Value <= currentDate)
                    .GroupBy(x => new
                    {
                        x.Nop,
                    }).Select(x => new
                    {
                        PajakId = (int)EnumFactory.EPajak.MakananMinuman,
                        Nop = x.Key.Nop,
                        NominalPokokBayar = x.Sum(y => y.NominalPokokBayar) ?? 0
                    }).ToList();
                var dataRealisasiSdBulaiIniHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == currentYear && x.TglBayarPokok.Value <= currentDate)
                    .GroupBy(x => new
                    {
                        x.Nop,
                    }).Select(x => new
                    {
                        PajakId = (int)EnumFactory.EPajak.JasaPerhotelan,
                        Nop = x.Key.Nop,
                        NominalPokokBayar = x.Sum(y => y.NominalPokokBayar) ?? 0
                    }).ToList();
                var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == currentYear && x.TglBayarPokok.Value <= currentDate)
                    .GroupBy(x => new
                    {
                        x.Nop,
                    }).Select(x => new
                    {
                        PajakId = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                        Nop = x.Key.Nop,
                        NominalPokokBayar = x.Sum(y => y.NominalPokokBayar) ?? 0
                    }).ToList();
                var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == currentYear && x.TglBayarPokok.Value <= currentDate)
                    .GroupBy(x => new
                    {
                        x.Nop,
                    }).Select(x => new
                    {
                        PajakId = (int)EnumFactory.EPajak.JasaParkir,
                        Nop = x.Key.Nop,
                        NominalPokokBayar = x.Sum(y => y.NominalPokokBayar) ?? 0
                    }).ToList();
                var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == currentYear && x.TglBayarPokok.Value <= currentDate)
                    .GroupBy(x => new
                    {
                        x.Nop,
                    }).Select(x => new
                    {
                        PajakId = (int)EnumFactory.EPajak.TenagaListrik,
                        Nop = x.Key.Nop,
                        NominalPokokBayar = x.Sum(y => y.NominalPokokBayar) ?? 0
                    }).ToList();
                var dataRealisasiSdBulaiIniAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == currentYear && x.TglBayarPokok.Value <= currentDate)
                    .GroupBy(x => new
                    {
                        x.Nop,
                    }).Select(x => new
                    {
                        PajakId = (int)EnumFactory.EPajak.AirTanah,
                        Nop = x.Key.Nop,
                        NominalPokokBayar = x.Sum(y => y.NominalPokokBayar) ?? 0
                    }).ToList();

                var dataRealisasi = new List<(int PajakId, string Nop, decimal Nominal)>();
                dataRealisasi.AddRange(dataRealisasiSdBulaiIniMamin.Select(x => (x.PajakId, x.Nop, x.NominalPokokBayar)));
                dataRealisasi.AddRange(dataRealisasiSdBulaiIniHotel.Select(x => (x.PajakId, x.Nop, x.NominalPokokBayar)));
                dataRealisasi.AddRange(dataRealisasiSdBulaiIniHiburan.Select(x => (x.PajakId, x.Nop, x.NominalPokokBayar)));
                dataRealisasi.AddRange(dataRealisasiSdBulaiIniParkir.Select(x => (x.PajakId, x.Nop, x.NominalPokokBayar)));
                dataRealisasi.AddRange(dataRealisasiSdBulaiIniListrik.Select(x => (x.PajakId, x.Nop, x.NominalPokokBayar)));
                dataRealisasi.AddRange(dataRealisasiSdBulaiIniAbt.Select(x => (x.PajakId, x.Nop, x.NominalPokokBayar)));

                var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == currentYear && x.TglBayar.Value <= currentDate)
                    .GroupBy(x => new
                    {
                        x.Uptb
                    }).Select(x => new
                    {
                        Uptb = Convert.ToInt32(x.Key.Uptb),
                        PajakId = (int)EnumFactory.EPajak.PBB,
                        NominalPokokBayar = x.Sum(y => y.JumlahBayarPokok) ?? 0
                    }).ToList();


                var pajakList = context.MPajaks.Where(x => x.Id > 0).ToList();
                var uptbList = context.MWilayahs.Select(x => Convert.ToInt32(x.Uptd)).Distinct().ToList();
                foreach (var pajak in pajakList)
                {
                    var res = new DataRealisasiUptb();
                    res.PajakId = pajak.Id;
                    res.PajakNama = pajak.Nama;
                    foreach (var uptb in uptbList)
                    {
                        var re = new DataRealisasiUptbWilayah();

                        var nopList = dataNopWilayah.Where(x => x.PajakId == pajak.Id && x.WilayahPajak == uptb)
                            .Select(x => x.Nop)
                            .Distinct()
                            .ToList();

                        decimal target = dataTarget
                            .Where(x => x.PajakId == pajak.Id && x.Uptb == uptb)
                            .Sum(x => x.Target);

                        decimal realisasi = dataRealisasi
                            .Where(x => x.PajakId == pajak.Id && nopList.Contains(x.Nop))
                            .Sum(x => x.Nominal);

                        if(pajak.Id == (int)EnumFactory.EPajak.PBB)
                        {
                            realisasi = dataRealisasiSdBulaiIniPbb.Where(x => x.Uptb == uptb).Sum(x => x.NominalPokokBayar);
                        }

                        decimal persentase = target > 0 ? (realisasi / target) * 100 : 0;

                        re.UptbId = uptb;
                        re.UptbNama = $"UPTB {uptb}";
                        re.Target = target;
                        re.Realisasi = realisasi;
                        re.Persen = persentase;

                        res.Uptbs.Add(re);
                    }
                    result.Add(res);
                }

                var dataRealisasiSdBulaiIniReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == currentYear && x.TglBayarPokok.Value <= currentDate).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == currentYear && x.TglBayar.Value <= currentDate).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiSdBulaiIniOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == currentYear && x.TglSspd <= currentDate).Sum(x => x.JmlPokok);
                var dataRealisasiSdBulaiIniOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == currentYear && x.TglSspd <= currentDate).Sum(x => x.JmlPokok);

                var dataRealisasiBidang = new List<(int PajakId, decimal Nominal)>
                {
                    ((int)EnumFactory.EPajak.BPHTB, dataRealisasiSdBulaiIniBphtb),
                    ((int)EnumFactory.EPajak.Reklame, dataRealisasiSdBulaiIniReklame),
                    ((int)EnumFactory.EPajak.OpsenPkb, dataRealisasiSdBulaiIniOpsenPkb),
                    ((int)EnumFactory.EPajak.OpsenBbnkb, dataRealisasiSdBulaiIniOpsenBbnkb)
                };

                var pajakBidangList = new List<int>
                {
                    (int)EnumFactory.EPajak.BPHTB,
                    (int)EnumFactory.EPajak.Reklame,
                    (int)EnumFactory.EPajak.OpsenPkb,
                    (int)EnumFactory.EPajak.OpsenBbnkb,
                };

                foreach (var res in result.Where(x => pajakBidangList.Contains(x.PajakId)).ToList())
                {
                    decimal target = dataTarget.Where(x => x.PajakId == res.PajakId).Sum(x => x.Target);
                    decimal realisasi = dataRealisasiBidang.Where(x => x.PajakId == res.PajakId).Sum(x => x.Nominal);
                    decimal persentase = target > 0 ? (realisasi / target) * 100 : 0;

                    var re = new DataRealisasiUptbWilayah();
                    re.UptbId = 1000;
                    re.UptbNama = "BIDANG";
                    re.Target = target;
                    re.Realisasi = realisasi;
                    re.Persen = persentase;

                    res.Uptbs.Clear();
                    res.Uptbs.Add(re);
                }

                return result;
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
