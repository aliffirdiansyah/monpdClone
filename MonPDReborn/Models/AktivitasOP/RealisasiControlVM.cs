using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            public List<DataDetailRealisasi> DataDetailList { get; set; } = new();
            public Detail()
            {
                DataDetailList = Method.GetDataDetail();
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
                //var currentYear = DateTime.Now.Year;

                // Target
                var dataTargetMamin = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var dataTargetHotel = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var dataTargetHiburan = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var dataTargetParkir = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var dataTargetListrik = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var dataTargetReklame = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var dataTargetPbb = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                var dataTargetBphtb = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var dataTargetAbt = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var dataTargetOpsenPkb = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var dataTargetOpsenBbnkb = context.DbAkunTargets.Where(x => x.TahunBuku == TanggalCutOff.Year && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);

                // TargetAKP
                var dataTargetAKPMamin = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var dataTargetAKPHotel = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var dataTargetAKPHiburan = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var dataTargetAKPParkir = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var dataTargetAKPListrik = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var dataTargetAKPReklame = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var dataTargetAKPPbb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                var dataTargetAKPBphtb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var dataTargetAKPAbt = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var dataTargetAKPOpsenPkb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var dataTargetAKPOpsenBbnkb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan == TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);

                // TargetSdBulanIni
                var dataTargetAKPSdBulanIniMamin = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniHotel = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniHiburan = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniParkir = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.JasaParkir).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniListrik = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniReklame = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.Reklame).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniPbb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.PBB).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniBphtb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.BPHTB).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniAbt = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.AirTanah).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniOpsenPkb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.OpsenPkb).Sum(x => x.Target);
                var dataTargetAKPSdBulanIniOpsenBbnkb = context.DbAkunTargetBulans.Where(x => x.TahunBuku == TanggalCutOff.Year && x.Bulan <= TanggalCutOff.Month && x.PajakId == (int)EnumFactory.EPajak.OpsenBbnkb).Sum(x => x.Target);

                // Realisasi
                var dataRealisasiSdBulaiIniMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiSdBulaiIniPbb = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiSdBulaiIniAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiSdBulaiIniOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);
                var dataRealisasiSdBulaiIniOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);

                // RealisasiSdBulaiIni
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                //var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value.Month >= TanggalCutOff.Month).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value.Month == TanggalCutOff.Month).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd.Month == TanggalCutOff.Month).Sum(x => x.JmlPokok);
                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd.Month == TanggalCutOff.Month).Sum(x => x.JmlPokok);

                ret.Add(new DataRealisasi
                {
                    No = 1,
                    JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                    Target = dataTargetMamin,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiMamin,
                        Persen = dataTargetAKPMamin > 0 ? (dataRealisasiMamin / dataTargetAKPMamin) * 100 : 0,
                        AKP = dataTargetAKPMamin

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniMamin,
                        PersenAkpTarget = dataTargetAKPSdBulanIniMamin > 0 ? (dataTargetAKPSdBulanIniMamin / dataTargetMamin) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniMamin > 0 ? (dataRealisasiSdBulaiIniMamin / dataTargetAKPSdBulanIniMamin) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniMamin > 0 ? (dataRealisasiSdBulaiIniMamin / dataTargetMamin) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniMamin,
                        Persen = dataTargetAKPSdBulanIniMamin > 0 ? (dataRealisasiSdBulaiIniMamin / dataTargetAKPSdBulanIniMamin) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 2,
                    JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                    Target = dataTargetListrik,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiListrik,
                        Persen = dataTargetAKPListrik > 0 ? (dataRealisasiListrik / dataTargetAKPListrik) * 100 : 0,
                        AKP = dataTargetAKPListrik

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniListrik,
                        PersenAkpTarget = dataTargetAKPSdBulanIniListrik > 0 ? (dataTargetAKPSdBulanIniListrik / dataTargetListrik) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniListrik > 0 ? (dataRealisasiSdBulaiIniListrik / dataTargetAKPSdBulanIniListrik) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniListrik > 0 ? (dataRealisasiSdBulaiIniListrik / dataTargetListrik) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniListrik,
                        Persen = dataTargetAKPSdBulanIniListrik > 0 ? (dataRealisasiSdBulaiIniListrik / dataTargetAKPSdBulanIniListrik) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 3,
                    JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                    Target = dataTargetHotel,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiHotel,
                        Persen = dataTargetAKPHotel > 0 ? (dataRealisasiHotel / dataTargetAKPHotel) * 100 : 0,
                        AKP = dataTargetAKPHotel

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniHotel,
                        PersenAkpTarget = dataTargetAKPSdBulanIniHotel > 0 ? (dataTargetAKPSdBulanIniHotel / dataTargetHotel) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniHotel > 0 ? (dataRealisasiSdBulaiIniHotel / dataTargetAKPSdBulanIniHotel) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniHotel > 0 ? (dataRealisasiSdBulaiIniHotel / dataTargetHotel) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniHotel,
                        Persen = dataTargetAKPSdBulanIniHotel > 0 ? (dataRealisasiSdBulaiIniHotel / dataTargetAKPSdBulanIniHotel) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 4,
                    JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                    Target = dataTargetParkir,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiParkir,
                        Persen = dataTargetAKPParkir > 0 ? (dataRealisasiParkir / dataTargetAKPParkir) * 100 : 0,
                        AKP = dataTargetAKPParkir

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniParkir,
                        PersenAkpTarget = dataTargetAKPSdBulanIniParkir > 0 ? (dataTargetAKPSdBulanIniParkir / dataTargetParkir) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniParkir > 0 ? (dataRealisasiSdBulaiIniParkir / dataTargetAKPSdBulanIniParkir) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniParkir > 0 ? (dataRealisasiSdBulaiIniParkir / dataTargetParkir) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniParkir,
                        Persen = dataTargetAKPSdBulanIniParkir > 0 ? (dataRealisasiSdBulaiIniParkir / dataTargetAKPSdBulanIniParkir) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 5,
                    JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                    Target = dataTargetHiburan,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiHiburan,
                        Persen = dataTargetAKPHiburan > 0 ? (dataRealisasiHiburan / dataTargetAKPHiburan) * 100 : 0,
                        AKP = dataTargetAKPHiburan

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniHiburan,
                        PersenAkpTarget = dataTargetAKPSdBulanIniHiburan > 0 ? (dataTargetAKPSdBulanIniHiburan / dataTargetHiburan) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniHiburan > 0 ? (dataRealisasiSdBulaiIniHiburan / dataTargetAKPSdBulanIniHiburan) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniHiburan > 0 ? (dataRealisasiSdBulaiIniHiburan / dataTargetHiburan) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniHiburan,
                        Persen = dataTargetAKPSdBulanIniHiburan > 0 ? (dataRealisasiSdBulaiIniHiburan / dataTargetAKPSdBulanIniHiburan) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 6,
                    JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                    Target = dataTargetAbt,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiAbt,
                        Persen = dataTargetAKPAbt > 0 ? (dataRealisasiAbt / dataTargetAKPAbt) * 100 : 0,
                        AKP = dataTargetAKPAbt

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniAbt,
                        PersenAkpTarget = dataTargetAKPSdBulanIniAbt > 0 ? (dataTargetAKPSdBulanIniAbt / dataTargetAbt) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniAbt > 0 ? (dataRealisasiSdBulaiIniAbt / dataTargetAKPSdBulanIniAbt) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniAbt > 0 ? (dataRealisasiSdBulaiIniAbt / dataTargetAbt) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniAbt,
                        Persen = dataTargetAKPSdBulanIniAbt > 0 ? (dataRealisasiSdBulaiIniAbt / dataTargetAKPSdBulanIniAbt) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 7,
                    JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                    Target = dataTargetReklame,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiReklame,
                        Persen = dataTargetAKPReklame > 0 ? (dataRealisasiReklame / dataTargetAKPReklame) * 100 : 0,
                        AKP = dataTargetAKPReklame

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniReklame,
                        PersenAkpTarget = dataTargetAKPSdBulanIniReklame > 0 ? (dataTargetAKPSdBulanIniReklame / dataTargetReklame) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniReklame > 0 ? (dataRealisasiSdBulaiIniReklame / dataTargetAKPSdBulanIniReklame) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniReklame > 0 ? (dataRealisasiSdBulaiIniReklame / dataTargetReklame) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniReklame,
                        Persen = dataTargetAKPSdBulanIniReklame > 0 ? (dataRealisasiSdBulaiIniReklame / dataTargetAKPSdBulanIniReklame) * 100 : 0,
                    }
                });

                //ret.Add(new DataRealisasi
                //{
                //    No = 8,
                //    JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                //    Target = dataTargetPbb,
                //    PembayaranBulanIni = new PembayaranDetail
                //    {
                //        Realisasi = dataRealisasiPbb,
                //        Persen = dataTargetAKPPbb > 0 ? (dataRealisasiPbb / dataTargetAKPPbb) * 100 : 0,
                //        AKP = dataTargetAKPPbb

                //    },
                //    PembayaranSDBI = new PembayaranDetailSDBI
                //    {
                //        AKP = dataTargetAKPSdBulanIniPbb,
                //        PersenAkpTarget = dataTargetAKPSdBulanIniPbb > 0 ? (dataTargetAKPSdBulanIniPbb / dataTargetPbb) * 100 : 0,
                //        PersenAkpRealisasi = dataTargetAKPSdBulanIniPbb > 0 ? (dataRealisasiSdBulaiIniPbb / dataTargetAKPSdBulanIniPbb) * 100 : 0,
                //        PersenTarget = dataRealisasiSdBulaiIniPbb > 0 ? (dataRealisasiSdBulaiIniPbb / dataTargetPbb) * 100 : 0,
                //        Realisasi = dataRealisasiSdBulaiIniPbb,
                //        Persen = dataTargetAKPSdBulanIniPbb > 0 ? (dataRealisasiSdBulaiIniPbb / dataTargetAKPSdBulanIniPbb) * 100 : 0,
                //    }
                //});

                ret.Add(new DataRealisasi
                {
                    No = 9,
                    JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                    Target = dataTargetBphtb,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiBphtb,
                        Persen = dataTargetAKPBphtb > 0 ? (dataRealisasiBphtb / dataTargetAKPBphtb) * 100 : 0,
                        AKP = dataTargetAKPBphtb

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniBphtb,
                        PersenAkpTarget = dataTargetAKPSdBulanIniBphtb > 0 ? (dataTargetAKPSdBulanIniBphtb / dataTargetBphtb) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniBphtb > 0 ? (dataRealisasiSdBulaiIniBphtb / dataTargetAKPSdBulanIniBphtb) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniBphtb > 0 ? (dataRealisasiSdBulaiIniBphtb / dataTargetBphtb) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniBphtb,
                        Persen = dataTargetAKPSdBulanIniBphtb > 0 ? (dataRealisasiSdBulaiIniBphtb / dataTargetAKPSdBulanIniBphtb) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 10,
                    JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                    Target = dataTargetOpsenPkb,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiOpsenPkb,
                        Persen = dataTargetAKPOpsenPkb > 0 ? (dataRealisasiOpsenPkb / dataTargetAKPOpsenPkb) * 100 : 0,
                        AKP = dataTargetAKPOpsenPkb

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniOpsenPkb,
                        PersenAkpTarget = dataTargetAKPSdBulanIniOpsenPkb > 0 ? (dataTargetAKPSdBulanIniOpsenPkb / dataTargetOpsenPkb) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniOpsenPkb > 0 ? (dataRealisasiSdBulaiIniOpsenPkb / dataTargetAKPSdBulanIniOpsenPkb) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniOpsenPkb > 0 ? (dataRealisasiSdBulaiIniOpsenPkb / dataTargetOpsenPkb) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniOpsenPkb,
                        Persen = dataTargetAKPSdBulanIniOpsenPkb > 0 ? (dataRealisasiSdBulaiIniOpsenPkb / dataTargetAKPSdBulanIniOpsenPkb) * 100 : 0,
                    }
                });

                ret.Add(new DataRealisasi
                {
                    No = 11,
                    JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                    Target = dataTargetOpsenBbnkb,
                    PembayaranBulanIni = new PembayaranDetail
                    {
                        Realisasi = dataRealisasiOpsenBbnkb,
                        Persen = dataTargetAKPOpsenBbnkb > 0 ? (dataRealisasiOpsenBbnkb / dataTargetAKPOpsenBbnkb) * 100 : 0,
                        AKP = dataTargetAKPOpsenBbnkb

                    },
                    PembayaranSDBI = new PembayaranDetailSDBI
                    {
                        AKP = dataTargetAKPSdBulanIniOpsenBbnkb,
                        PersenAkpTarget = dataTargetAKPSdBulanIniOpsenBbnkb > 0 ? (dataTargetAKPSdBulanIniOpsenBbnkb / dataTargetOpsenBbnkb) * 100 : 0,
                        PersenAkpRealisasi = dataTargetAKPSdBulanIniOpsenBbnkb > 0 ? (dataRealisasiSdBulaiIniOpsenBbnkb / dataTargetAKPSdBulanIniOpsenBbnkb) * 100 : 0,
                        PersenTarget = dataRealisasiSdBulaiIniOpsenBbnkb > 0 ? (dataRealisasiSdBulaiIniOpsenBbnkb / dataTargetOpsenBbnkb) * 100 : 0,
                        Realisasi = dataRealisasiSdBulaiIniOpsenBbnkb,
                        Persen = dataTargetAKPSdBulanIniOpsenBbnkb > 0 ? (dataRealisasiSdBulaiIniOpsenBbnkb / dataTargetAKPSdBulanIniOpsenBbnkb) * 100 : 0,
                    }
                });

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

                //dataWilayahGabungan.AddRange(
                //    context.DbOpPbbs
                //        .Where(x => x.TahunBuku == currentYear)
                //        .Select(x => new
                //        {
                //            x.Nop,
                //            x.WilayahPajak,
                //            PajakId = 9m // PBB
                //        })
                //        .ToList()
                //        .Select(x => (x.Nop, x.WilayahPajak, x.PajakId))
                //        .ToList()
                //);
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

                //dataRealisasiGabungan.AddRange(
                //    context.DbMonPbbs
                //        .Where(x => x.TahunBuku == currentYear && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Month <= currentMonth)
                //        .Select(x => new
                //        {
                //            x.Nop,
                //            x.TglBayarPokok,
                //            NominalPokokBayar = x.NominalPokokBayar ?? 0,
                //            x.PajakId
                //        })
                //        .ToList()
                //        .Select(x => (x.Nop, x.TglBayarPokok, x.NominalPokokBayar, x.PajakId))
                //        .ToList()
                //);

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

                return new List<DataDetailRealisasi>
                {
                    new() { No = 1, JenisPajak = "Pajak Hotel",
                        UPTB1 = new() { Target = 500, Realisasi = 480, Persen = 96 },
                        UPTB2 = new() { Target = 600, Realisasi = 590, Persen = 98.33m },
                        UPTB3 = new() { Target = 700, Realisasi = 680, Persen = 97.14m },
                        UPTB4 = new() { Target = 400, Realisasi = 400, Persen = 100 },
                        UPTB5 = new() { Target = 800, Realisasi = 750, Persen = 93.75m },
                        Bidang = new() { Target = 3000, Realisasi = 2900, Persen = 96.67m }
                    },
                     new() { No = 2, JenisPajak = "Pajak Restoran",
                        UPTB1 = new() { Target = 1000, Realisasi = 1050, Persen = 105 },
                        UPTB2 = new() { Target = 1200, Realisasi = 1200, Persen = 100 },
                        UPTB3 = new() { Target = 900, Realisasi = 850, Persen = 94.44m },
                        UPTB4 = new() { Target = 800, Realisasi = 810, Persen = 101.25m },
                        UPTB5 = new() { Target = 1100, Realisasi = 1150, Persen = 104.55m },
                        Bidang = new() { Target = 5000, Realisasi = 5060, Persen = 101.2m }
                    },
                };
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
    }
}
