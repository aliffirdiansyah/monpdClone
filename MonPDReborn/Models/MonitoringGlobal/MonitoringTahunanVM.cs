using DocumentFormat.OpenXml.InkML;
using MonPDLib;
using MonPDLib.General;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Models.MonitoringGlobal.MonitoringTahunanVM.MonitoringTahunanViewModels;

namespace MonPDReborn.Models.MonitoringGlobal
{
    public class MonitoringTahunanVM
    {
        public class Index
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
            public Show(DateTime TanggalCutOff)
            {
                TahunanPajakList = Method.GetTahunanPajak(TanggalCutOff);
                tgl = TanggalCutOff;
            }
        }
        public class Method
        {
            public static List<MonitoringTahunanViewModels.TahunanPajak> GetTahunanPajak(DateTime TanggalCutOff)
            {
                var ret = new List<MonitoringTahunanViewModels.TahunanPajak>();

                var context = DBClass.GetContext();
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

                // RealisasiSD
                var dataRealisasiMamin = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotel = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburan = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkir = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrik = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiReklame = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbb = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiBphtb = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayar.Value <= TanggalCutOff).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiAbt = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglBayarPokok.Value <= TanggalCutOff && x.TahunBuku == TanggalCutOff.Year).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);
                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd >= new DateTime(TanggalCutOff.Year, 1, 1) && x.TglSspd <= TanggalCutOff).Sum(x => x.JmlPokok);
                // RealisasiHariIni
                var dataRealisasiMaminHari = context.DbMonRestos.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value == TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHotelHari = context.DbMonHotels.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value == TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiHiburanHari = context.DbMonHiburans.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value == TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiParkirHari = context.DbMonParkirs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value == TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiListrikHari = context.DbMonPpjs.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value == TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiReklameHari = context.DbMonReklames.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value == TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiPbbHari = context.DbMonPbbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value == TanggalCutOff && x.TahunBuku == TanggalCutOff.Year && x.JumlahBayarPokok > 0).Sum(x => x.JumlahBayarPokok) ?? 0;
                var dataRealisasiBphtbHari = context.DbMonBphtbs.Where(x => x.TglBayar.Value.Year == TanggalCutOff.Year && x.TglBayar.Value == TanggalCutOff).Sum(x => x.Pokok) ?? 0;
                var dataRealisasiAbtHari = context.DbMonAbts.Where(x => x.TglBayarPokok.Value.Year == TanggalCutOff.Year && x.TglBayarPokok.Value == TanggalCutOff).Sum(x => x.NominalPokokBayar) ?? 0;
                var dataRealisasiOpsenPkbHari = context.DbMonOpsenPkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd == TanggalCutOff).Sum(x => x.JmlPokok);
                var dataRealisasiOpsenBbnkbHari = context.DbMonOpsenBbnkbs.Where(x => x.TglSspd.Year == TanggalCutOff.Year && x.TglSspd == TanggalCutOff).Sum(x => x.JmlPokok);

                var listTahunan = new List<TahunanPajak>
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
                        JenisPajak = EPajak.Reklame.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiReklameHari,
                        AkpTahun = dataTargetReklame,
                        RealisasiSDHariAccrual = dataRealisasiReklame,
                        PersenAccrual = dataTargetReklame > 0
                            ? Math.Round((decimal)dataRealisasiReklame / dataTargetReklame * 100, 2)
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
                        JenisPajak = EPajak.BPHTB.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiBphtbHari,
                        AkpTahun = dataTargetBphtb,
                        RealisasiSDHariAccrual = dataRealisasiBphtb,
                        PersenAccrual = dataTargetBphtb > 0
                            ? Math.Round((decimal)dataRealisasiBphtb / dataTargetBphtb * 100, 2)
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
                    },
                    new()
                    {
                        JenisPajak = EPajak.OpsenPkb.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiOpsenPkbHari,
                        AkpTahun = dataTargetOpsenPkb,
                        RealisasiSDHariAccrual = dataRealisasiOpsenPkb,
                        PersenAccrual = dataTargetOpsenPkb > 0
                            ? Math.Round((decimal)dataRealisasiOpsenPkb / dataTargetOpsenPkb * 100, 2)
                            : 0
                    },
                    new()
                    {
                        JenisPajak = EPajak.OpsenBbnkb.GetDescription(),
                        RealisasiHariAccrual = dataRealisasiOpsenBbnkbHari,
                        AkpTahun = dataTargetOpsenBbnkb,
                        RealisasiSDHariAccrual = dataRealisasiOpsenBbnkb,
                        PersenAccrual = dataTargetOpsenBbnkb > 0
                            ? Math.Round((decimal)dataRealisasiOpsenBbnkb / dataTargetOpsenBbnkb * 100, 2)
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
