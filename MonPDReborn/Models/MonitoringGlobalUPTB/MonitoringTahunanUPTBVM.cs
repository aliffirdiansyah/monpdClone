using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using System.Collections.Generic;
using static MonPDLib.General.EnumFactory;

namespace MonPDReborn.Models.MonitoringGlobalUPTB
{
    public class MonitoringTahunanUPTBVM
    {
        public class  Index
        {
            public DateTime TanggalCutOff { get; set; } = DateTime.Now;
            public Index()
            {

            }
        }

        public class Show
        {
            public List<MonitoringTahunanViewModels.TahunanPajak> TahunanPajakList { get; set; } = new();
            public DateTime tgl { get; set; }
            public Show(DateTime TanggalCutOff, int wilayah)
            {
                TahunanPajakList = Method.GetTahunanPajak(TanggalCutOff, wilayah);
                tgl = TanggalCutOff;
            }
        }

        public class Method
        {
            public static List<MonitoringTahunanViewModels.TahunanPajak> GetTahunanPajak(DateTime TanggalCutOff, int wilayah)
            {
                var ret = new List<MonitoringTahunanViewModels.TahunanPajak>();

                var context = DBClass.GetContext();

                //Ambil Wilayah
                var nopListSemuaAbt = context.DbOpAbts
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop).Distinct().AsQueryable();
                var nopListSemuaResto = context.DbOpRestos
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop).Distinct().AsQueryable();
                var nopListSemuaHotel = context.DbOpHotels
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop).Distinct().AsQueryable();
                var nopListSemuaListrik = context.DbOpListriks
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString() && x.Sumber == 55)
                    .Select(x => x.Nop).Distinct().AsQueryable();
                var nopListSemuaParkir = context.DbOpParkirs
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop).Distinct().AsQueryable();
                var nopListSemuaHiburan = context.DbOpHiburans
                    .Where(x => x.WilayahPajak == ((int)wilayah).ToString())
                    .Select(x => x.Nop).Distinct().AsQueryable();
                var nopListSemuaPbb = context.DbMonPbbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year && x.Uptb == wilayah)
                    .Select(x => x.Nop)
                    .Distinct()
                    .AsQueryable();

                //Target
                var dataTargetMamin = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year 
                                && x.PajakId == (int)EnumFactory.EPajak.MakananMinuman
                                && x.Uptb == wilayah).Sum(x => x.Target);
                var dataTargetHotel = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year 
                                && x.PajakId == (int)EnumFactory.EPajak.JasaPerhotelan
                                && x.Uptb == wilayah).Sum(x => x.Target);
                var dataTargetHiburan = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year 
                                && x.PajakId == (int)EnumFactory.EPajak.JasaKesenianHiburan
                                && x.Uptb == wilayah).Sum(x => x.Target);
                var dataTargetParkir = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year 
                                && x.PajakId == (int)EnumFactory.EPajak.JasaParkir
                                && x.Uptb == wilayah).Sum(x => x.Target);
                var dataTargetListrik = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year 
                                && x.PajakId == (int)EnumFactory.EPajak.TenagaListrik
                                && x.Uptb == wilayah && x.SubRincian == "2").Sum(x => x.Target);
                var dataTargetPbb = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year 
                                && x.PajakId == (int)EnumFactory.EPajak.PBB
                                && x.Uptb == wilayah).Sum(x => x.Target);
                var dataTargetAbt = context.DbAkunTargetBulanUptbs
                    .Where(x => x.TahunBuku == TanggalCutOff.Year 
                                && x.PajakId == (int)EnumFactory.EPajak.AirTanah
                                && x.Uptb == wilayah).Sum(x => x.Target);

                // RealisasiSD
                var dataRealisasiMamin = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaResto.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) 
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaHotel.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) 
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaHiburan.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) 
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaParkir.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) 
                            && x.TglBayarPokok.Value <= TanggalCutOff
                            && nopListSemuaListrik.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs
                    .Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year 
                            && x.TglBayar.Value >= new DateTime(TanggalCutOff.Year, 1, 1) 
                            && x.TahunBuku == TanggalCutOff.Year
                            && x.TglBayar.Value <= TanggalCutOff
                            && nopListSemuaPbb.Contains(x.Nop))
                    .Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) 
                            && x.TglBayarPokok.Value <= TanggalCutOff 
                            && x.TahunBuku == TanggalCutOff.Year
                            && nopListSemuaAbt.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;

                // RealisasiHariIni
                var dataRealisasiMaminHari = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value == TanggalCutOff
                            && nopListSemuaResto.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotelHari = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value == TanggalCutOff
                            && nopListSemuaHotel.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburanHari = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value == TanggalCutOff
                            && nopListSemuaHiburan.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkirHari = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value == TanggalCutOff
                            && nopListSemuaParkir.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrikHari = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value == TanggalCutOff
                            && nopListSemuaListrik.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbbHari = context.DbMonPbbs
                    .Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year 
                            && x.TglBayar.Value == TanggalCutOff && x.TahunBuku == TanggalCutOff.Year 
                            && nopListSemuaPbb.Contains(x.Nop))
                    .Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiAbtHari = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year 
                            && x.TglBayarPokok.Value == TanggalCutOff
                            && nopListSemuaAbt.Contains(x.Nop))
                    .Sum(x => x.NominalPokokBayar) ?? 0;

                var listTahunan = new List<MonitoringTahunanViewModels.TahunanPajak>
                {
                    new()
                    {
                        JenisPajak = EPajak.MakananMinuman.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiMaminHari,
                        AkpTahun = dataTargetMamin,
                        RealisasiSDHariAccrual = dataRealisasiMamin,
                        PersenAccrual = dataTargetMamin > 0
                            ? Math.Round((decimal)dataRealisasiMamin / dataTargetMamin * 100, 2)
                            : 0
                    },
                    new()
                    {
                        JenisPajak = EPajak.TenagaListrik.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiListrikHari,
                        AkpTahun = dataTargetListrik,
                        RealisasiSDHariAccrual = dataRealisasiListrik,
                        PersenAccrual = dataTargetListrik > 0
                            ? Math.Round((decimal)dataRealisasiListrik / dataTargetListrik * 100, 2)
                            : 0
                    },
                    new()
                    {
                        JenisPajak = EPajak.JasaPerhotelan.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiHotelHari,
                        AkpTahun = dataTargetHotel,
                        RealisasiSDHariAccrual = dataRealisasiHotel,
                        PersenAccrual = dataTargetHotel > 0
                            ? Math.Round((decimal)dataRealisasiHotel / dataTargetHotel * 100, 2)
                            : 0
                    },
                    new()
                    {
                        JenisPajak = EPajak.JasaKesenianHiburan.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiHiburanHari,
                        AkpTahun = dataTargetHiburan,
                        RealisasiSDHariAccrual = dataRealisasiHiburan,
                        PersenAccrual = dataTargetHiburan > 0
                            ? Math.Round((decimal)dataRealisasiHiburan / dataTargetHiburan * 100, 2)
                            : 0
                    },
                    new()
                    {
                        JenisPajak = EPajak.JasaParkir.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiParkirHari,
                        AkpTahun = dataTargetParkir,
                        RealisasiSDHariAccrual = dataRealisasiParkir,
                        PersenAccrual = dataTargetParkir > 0
                            ? Math.Round((decimal)dataRealisasiParkir / dataTargetParkir * 100, 2)
                            : 0
                    },
                    new()
                    {
                        JenisPajak = EPajak.PBB.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiPbbHari,
                        AkpTahun = dataTargetPbb,
                        RealisasiSDHariAccrual = dataRealisasiPbb,
                        PersenAccrual = dataTargetPbb > 0
                            ? Math.Round((decimal)dataRealisasiPbb / dataTargetPbb * 100, 2)
                            : 0
                    },
                    new()
                    {
                        JenisPajak = EPajak.AirTanah.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiAbtHari,
                        AkpTahun = dataTargetAbt,
                        RealisasiSDHariAccrual = dataRealisasiAbt,
                        PersenAccrual = dataTargetAbt > 0
                            ? Math.Round((decimal)dataRealisasiAbt / dataTargetAbt * 100, 2)
                            : 0
                    }
                };


                ret.AddRange(listTahunan);
                return ret;
            }
        }

        public class MonitoringTahunanViewModels
        {
            public class TahunanPajak
            {
                public string JenisPajak { get; set; } = null!;
                public decimal AkpTahun { get; set; } = 0;
                public decimal RealisasiHariAccrual { get; set; } = 0;
                public decimal RealisasiSDHariAccrual { get; set; } = 0;
                public decimal PersenAccrual { get; set; } = 0;
            }
        }
    }
}
