using DevExpress.CodeParser;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Globalization;
using System.Numerics;
using System.Web.Mvc;
using static MonPDReborn.Models.AktivitasOP.PendataanObjekPajakVM;
using static MonPDReborn.Models.CCTVParkir.MonitoringCCTVVM.ViewModel.KapasitasChart;

namespace MonPDReborn.Models.CCTVParkir
{
    public class MonitoringCCTVVM
    {
        public class Index
        {
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
            public List<MonitoringCCTV> MonitoringCCTVList { get; set; } = new();
            public decimal TotalOp => MonitoringCCTVList.Count;
            public decimal TotalCCTV => MonitoringCCTVList.Count(x => x.StatusTerpasang == "TERPASANG");

            public decimal TotalCCTVTerpasangJasnita => MonitoringCCTVList.Count(x => x.StatusTerpasang == "TERPASANG" && x.VedorId == 1);
            public decimal TotalCCTVTerpasangTelkom => MonitoringCCTVList.Count(x => x.StatusTerpasang == "TERPASANG" && x.VedorId == 2);
            public decimal TotalCCTVTerpasangPerforma => MonitoringCCTVList.Count(x => x.StatusTerpasang == "TERPASANG" && x.VedorId == 3);
            public decimal TotalBelumTerpasang => MonitoringCCTVList.Count(x => x.StatusTerpasang == "BELUM TERPASANG");

            public Show(int uptb)
            {
                MonitoringCCTVList = Method.GetMonitoringCCTVList(uptb);
            }
        }
        public class Detail
        {
            public List<MonitoringCCTVDet> MonitoringCCTVDetList { get; set; } = new();
            public Detail(string nop, int vendor)
            {
                MonitoringCCTVDetList = Method.GetMonitoringCCTVDetList(nop, (EnumFactory.EVendorParkirCCTV)vendor);
            }
        }
        public class Kapasitas
        {
            public List<MonitoringCCTVBulanan> RekapBulanan { get; set; } = new();
            public string Nop { get; set; } = "";
            public string formattedNop { get; set; }
            public string NamaOP { get; set; }
            public string AlamatOP { get; set; }
            public string Vendor { get; set; }
            public string TanggalPasang { get; set; }
            public Kapasitas(string nop, int vendorId)
            {
                Nop = nop;
                formattedNop = Utility.GetFormattedNOP(Nop);
                var context = DBClass.GetContext();
                var query = context.MOpParkirCctvs.FirstOrDefault(m => m.Nop == nop && m.Vendor == vendorId);
                if (vendorId == (int)(EnumFactory.EVendorParkirCCTV.Jasnita))
                {
                    var jasnita = context.MOpParkirCctvJasnita.FirstOrDefault(x => x.Nop == nop);
                    if (jasnita != null)
                    {
                        Vendor = (EnumFactory.EVendorParkirCCTV.Jasnita).GetDescription();
                        TanggalPasang = jasnita.TglPasang.ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                    }
                }
                if (vendorId == (int)(EnumFactory.EVendorParkirCCTV.Telkom))
                {
                    var telkom = context.MOpParkirCctvTelkoms.FirstOrDefault(x => x.Nop == nop);
                    if (telkom != null)
                    {
                        Vendor = (EnumFactory.EVendorParkirCCTV.Telkom).GetDescription();
                        TanggalPasang = telkom.TglPasang.ToString(format: "dd MMM yyyy", new CultureInfo("id-ID"));
                    }
                }

                if (query != null)
                {
                    NamaOP = query.NamaOp;
                    AlamatOP = query.AlamatOp;
                }


                RekapBulanan = Method.GetMonitoringCCTVBulanan(nop, vendorId);

            }
        }

        public class KapasitasHarianDetail
        {
            public List<MonitoringCctvHarianDetail> DataMonitoringDetail { get; set; } = new();
            public ViewModel.KapasitasChart KapasitasChartDetail { get; set; } = new();
            public string Nop { get; set; } = "";
            public string formattedNop { get; set; }
            public string NamaOP { get; set; }
            public string AlamatOP { get; set; }
            public string Vendor { get; set; }
            public string TanggalPasang { get; set; }
            public string TanggalCctv { get; set; }
            public KapasitasHarianDetail(string nop, int vendorId, DateTime tanggal)
            {
                DataMonitoringDetail = Method.GetMonitoringHarianDetail(nop, vendorId, tanggal);
                KapasitasChartDetail = Method.GetKapasitasChart(DataMonitoringDetail, tanggal);

                var context = DBClass.GetContext();
                var query = context.MOpParkirCctvs.FirstOrDefault(m => m.Nop == nop);
                if (query != null)
                {
                    Nop = nop;
                    formattedNop = Utility.GetFormattedNOP(Nop);
                    NamaOP = query.NamaOp;
                    AlamatOP = query.AlamatOp;
                    TanggalCctv = tanggal.ToString(format: "dd MMM yyyy", new CultureInfo("id-ID"));

                    if (query.Vendor == (int)(EnumFactory.EVendorParkirCCTV.Jasnita))
                    {
                        var jasnita = context.MOpParkirCctvJasnita.FirstOrDefault(x => x.Nop == nop);
                        if (jasnita != null)
                        {
                            Vendor = (EnumFactory.EVendorParkirCCTV.Jasnita).GetDescription();
                            TanggalPasang = jasnita.TglPasang.ToString("dd MMM yyyy", new CultureInfo("id-ID"));
                        }
                    }
                    if (query.Vendor == (int)(EnumFactory.EVendorParkirCCTV.Telkom))
                    {
                        var telkom = context.MOpParkirCctvTelkoms.FirstOrDefault(x => x.Nop == nop);
                        if (telkom != null)
                        {
                            Vendor = (EnumFactory.EVendorParkirCCTV.Telkom).GetDescription();
                            TanggalPasang = telkom.TglPasang.ToString(format: "dd MMM yyyy", new CultureInfo("id-ID"));
                        }
                    }
                }
            }
        }

        public class MonitoringCctvHarianDetail
        {
            public string Id { get; set; } = null!;
            public string Nop { get; set; } = null!;
            public string CctvId { get; set; } = null!;
            public string? NamaOp { get; set; }
            public string? AlamatOp { get; set; }
            public int? WilayahPajak { get; set; }
            public DateTime TanggalMasuk { get; set; }
            public EnumFactory.EJenisKendParkirCCTV JenisKendEnum { get; set; }
            public string JenisKend { get; set; }
            public string? PlatNo { get; set; }
            public String Direction { get; set; }
            public string? Log { get; set; }
            public string? ImageUrl { get; set; }

            public bool IsLog { get; set; }
            public bool IsOn { get; set; }

        }

        public class MonitoringCCTVBulanan
        {
            public string Nop { get; set; } = null!;
            public int Tahun { get; set; }
            public int Bulan { get; set; }

            // Untuk tampilan
            public string NamaBulan { get; set; }
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);

            // Agregat per bulan
            public int Motor { get; set; }
            public int Mobil { get; set; }
            public int Unknown { get; set; }
            public decimal Omset { get; set; }
            public decimal EstimasiPajak { get; set; }
            public decimal TahunKemarin { get; set; }
            public decimal TahunIni { get; set; }
            public string Potensi { get; set; }

        }

        public class MonitoringCCTVHarian
        {
            public string Nop { get; set; } = null!;
            public DateTime Tanggal { get; set; }
            public string TanggalLabel => Tanggal.ToString("dd");        // 01, 02, 03 ...
            public string HariLabel => Tanggal.ToString("dddd", new System.Globalization.CultureInfo("id-ID"));            // Senin, Selasa ...

            public string FormattedNOP => Utility.GetFormattedNOP(Nop);

            // Agregat per bulan
            public int Motor { get; set; }
            public int Mobil { get; set; }
            public int Unknown { get; set; }
            public decimal Omset { get; set; }
            public decimal EstimasiPajak { get; set; }
            public decimal TahunKemarin { get; set; }
            public decimal TahunIni { get; set; }
            public decimal Potensi { get; set; }
        }


        public class DataKapasitasParkir
        {
            public List<MonitoringCCTVKapasitas> MonitoringCCTVKapasitasList { get; set; } = new();
            public DataKapasitasParkir(string nop, DateTime tglAwal, DateTime tglAkhir)
            {
                MonitoringCCTVKapasitasList = Method.GetMonitoringCCTVKapasitas(nop, tglAwal, tglAkhir);
            }
        }
        public class Log
        {
            public List<MonitoringCCTVLog> MonitoringCCTVLogList { get; set; } = new();
            public Log(string nop, int jenisKend, DateTime tanggalAwal, DateTime tanggalAkhir)
            {
                MonitoringCCTVLogList = Method.GetMonitoringCCTVLog(nop, jenisKend, tanggalAwal, tanggalAkhir);
            }
        }

        public class ViewModel
        {
            public class KapasitasChart
            {
                public List<MasterKendaraan> MasterKendaraans { get; set; } = new();
                public List<KendaraanJenis> KendaraanJeniss { get; set; } = new();
                public List<KendaraanData> KendaraanDatas { get; set; } = new();
                public List<CctvEvent> CctvEvents { get; set; } = new();
                public class MasterKendaraan
                {
                    public string Name { get; set; } = "";   // misal: "mobil", "motor", "truck"
                    public string Color { get; set; } = "";  // misal: "#038edc"
                }

                // 2. Detail kendaraan per waktu
                public class KendaraanJenis
                {
                    public string Name { get; set; } = "";   // nama kendaraan
                    public int Value { get; set; }           // jumlah kendaraan
                }

                // 3. Data kendaraan per waktu
                public class KendaraanData
                {
                    public string Waktu { get; set; }               // waktu pengukuran (yyyy-MM-dd HH:mm)
                    public List<KendaraanJenis> Jenis { get; set; } = new List<KendaraanJenis>();
                }

                // 4. Event CCTV
                public class CctvEvent
                {
                    public string Status { get; set; } = "";     // "ON" atau "OFF"
                    public string Time { get; set; }           // waktu event (yyyy-MM-dd HH:mm)
                }
            }
        }

        public class Method
        {
            public static List<MonitoringCCTV> GetMonitoringCCTVList(int uptbId)
            {
                var result = new List<MonitoringCCTV>();
                var context = DBClass.GetContext();
                if (uptbId == 0)
                {
                    result = (
                        from c in context.MOpParkirCctvs
                        join d1 in context.MOpParkirCctvJasnita on c.Nop equals d1.Nop into jasnitaDets
                        from d1 in jasnitaDets.DefaultIfEmpty()
                        join d2 in context.MOpParkirCctvTelkoms on c.Nop equals d2.Nop into telkomDets
                        from d2 in telkomDets.DefaultIfEmpty()
                        join l1 in context.MOpParkirCctvJasnitaLogs
                            on new { Nop = (string?)d1.Nop, CctvId = d1.CctvId }
                            equals new { Nop = (string?)l1.Nop, CctvId = l1.CctvId }
                            into logsJasnita
                        from l1 in logsJasnita.DefaultIfEmpty()
                        join l2 in context.MOpParkirCctvTelkomLogs
                            on new { Nop = (string?)d2.Nop, CctvId = d2.CctvId }
                            equals new { Nop = (string?)l2.Nop, CctvId = l2.CctvId }
                            into logsTelkom
                        from l2 in logsTelkom.DefaultIfEmpty()
                        group new { c, d1, d2, l1, l2 } by new
                        {
                            c.Nop,
                            c.NamaOp,
                            c.AlamatOp,
                            c.WilayahPajak,
                            c.Vendor,
                            c.IsPasang
                        }
                        into g
                        select new
                        {
                            g.Key.Nop,
                            g.Key.NamaOp,
                            g.Key.AlamatOp,
                            g.Key.WilayahPajak,
                            g.Key.Vendor,
                            g.Key.IsPasang,
                            D1 = g.Select(x => x.d1),
                            D2 = g.Select(x => x.d2),
                            L1 = g.Select(x => x.l1),
                            L2 = g.Select(x => x.l2)
                        }
                    )
                    .AsEnumerable()
                    .Select(g =>
                    {
                        var dets = g.D1.Where(x => x != null).Cast<object>()
                            .Concat(g.D2.Where(x => x != null));

                        var logs = g.L1.Where(x => x != null).Cast<object>()
                            .Concat(g.L2.Where(x => x != null));

                        var tglTerpasang = dets
                            .Select(d => (DateTime?)((dynamic)d).TglPasang)
                            .Min();

                        var lastLog = logs
                            .OrderByDescending(l => ((dynamic)l).TglTerakhirAktif)
                            .FirstOrDefault();

                        var statusTerpasang = tglTerpasang.HasValue ? "TERPASANG" : "BELUM TERPASANG";

                        bool isHaveDetail = dets.Any();
                        bool ishaveLogs = logs.Any();

                        var isAktif = "ACTIVE";

                        DateTime? tglTerakhirAktif = null;
                        string? tglTerakhirAktifString = "";

                        if (!ishaveLogs)
                        {
                            // tidak punya log sama sekali → INACTIVE
                            isAktif = "INACTIVE";
                        }
                        
                        if (lastLog != null && ((dynamic)lastLog).Status == "NON AKTIF")
                        {
                            isAktif = "INACTIVE";
                            tglTerakhirAktif = (DateTime?)((dynamic)lastLog).TglTerakhirAktif;
                            if (tglTerakhirAktif.HasValue)
                                tglTerakhirAktifString = tglTerakhirAktif.Value.ToString("dd MMM yyyy HH:mm:ss");

                        }           

                        if (g.IsPasang != 1)
                        {
                            isAktif = "UNASSIGNED";
                            statusTerpasang = "BELUM TERPASANG";
                            /*  PENGECEKAN MENGAKALI , JIKA DI DB ADA TGL TERPASANG, TAPI SEBENARNYA IS PASANG = 0 , KARENA DI DB KOLOM TGL TERPASANG 
                             *  CONSTRAINT NOT NULL
                             */
                            tglTerpasang = null;
                        }


                        return new MonitoringCCTV
                        {
                            Nop = g.Nop,
                            NamaOp = g.NamaOp,
                            AlamatOp = g.AlamatOp,
                            WilayahPajak = ((EnumFactory.EUPTB)g.WilayahPajak).GetDescription(),
                            UptbId = g.WilayahPajak,
                            TglTerpasang = tglTerpasang,
                            VedorId = g.Vendor,
                            Vendor = ((EnumFactory.EVendorParkirCCTV)g.Vendor).GetDescription(),
                            StatusTerpasang = statusTerpasang,
                            StatusAktif = isAktif,
                            Ishasdetail = isHaveDetail,
                            Ishaslog = ishaveLogs,
                            TglTerakhirAktif = tglTerakhirAktif,
                            TglTerakhirAktifString = tglTerakhirAktifString
                        };
                    })
                    .ToList();
                }
                else
                {
                    result = (
                        from c in context.MOpParkirCctvs
                        join d1 in context.MOpParkirCctvJasnita on c.Nop equals d1.Nop into jasnitaDets
                        from d1 in jasnitaDets.DefaultIfEmpty()
                        join d2 in context.MOpParkirCctvTelkoms on c.Nop equals d2.Nop into telkomDets
                        from d2 in telkomDets.DefaultIfEmpty()
                        join l1 in context.MOpParkirCctvJasnitaLogs
                            on new { Nop = (string?)d1.Nop, CctvId = d1.CctvId }
                            equals new { Nop = (string?)l1.Nop, CctvId = l1.CctvId }
                            into logsJasnita
                        from l1 in logsJasnita.DefaultIfEmpty()
                        join l2 in context.MOpParkirCctvTelkomLogs
                            on new { Nop = (string?)d2.Nop, CctvId = d2.CctvId }
                            equals new { Nop = (string?)l2.Nop, CctvId = l2.CctvId }
                            into logsTelkom
                        from l2 in logsTelkom.DefaultIfEmpty()
                        where c.WilayahPajak == uptbId
                        group new { c, d1, d2, l1, l2 } by new
                        {
                            c.Nop,
                            c.NamaOp,
                            c.AlamatOp,
                            c.WilayahPajak,
                            c.Vendor,
                            c.IsPasang
                        }
                        into g
                        select new
                        {
                            g.Key.Nop,
                            g.Key.NamaOp,
                            g.Key.AlamatOp,
                            g.Key.WilayahPajak,
                            g.Key.Vendor,
                            g.Key.IsPasang,
                            D1 = g.Select(x => x.d1),
                            D2 = g.Select(x => x.d2),
                            L1 = g.Select(x => x.l1),
                            L2 = g.Select(x => x.l2)
                        }
                    )
                    .AsEnumerable()
                    .Select(g =>
                    {
                        var dets = g.D1.Where(x => x != null).Cast<object>()
                            .Concat(g.D2.Where(x => x != null));

                        var logs = g.L1.Where(x => x != null).Cast<object>()
                            .Concat(g.L2.Where(x => x != null));

                        var tglTerpasang = dets
                            .Select(d => (DateTime?)((dynamic)d).TglPasang)
                            .Min();

                        var lastLog = logs
                            .OrderByDescending(l => ((dynamic)l).TglTerakhirAktif)
                            .FirstOrDefault();

                        var statusTerpasang = tglTerpasang.HasValue ? "TERPASANG" : "BELUM TERPASANG";

                        bool isHaveDetail = dets.Any();
                        bool ishaveLogs = logs.Any();

                        var isAktif = "ACTIVE";

                        DateTime? tglTerakhirAktif = null;
                        string? tglTerakhirAktifString = "";

                        if (!ishaveLogs)
                        {
                            // tidak punya log sama sekali → INACTIVE
                            isAktif = "INACTIVE";
                        }
                        
                        if (lastLog != null && ((dynamic)lastLog).Status == "NON AKTIF")
                        {
                            isAktif = "INACTIVE";
                            tglTerakhirAktif = (DateTime?)((dynamic)lastLog).TglTerakhirAktif;
                            if (tglTerakhirAktif.HasValue)
                                tglTerakhirAktifString = tglTerakhirAktif.Value.ToString("dd MMM yyyy HH:mm:ss");

                        }           

                        if (g.IsPasang != 1)
                        {
                            isAktif = "UNASSIGNED";
                            statusTerpasang = "BELUM TERPASANG";
                            /*  PENGECEKAN MENGAKALI , JIKA DI DB ADA TGL TERPASANG, TAPI SEBENARNYA IS PASANG = 0 , KARENA DI DB KOLOM TGL TERPASANG 
                             *  CONSTRAINT NOT NULL
                             */
                            tglTerpasang = null;
                        }


                        return new MonitoringCCTV
                        {
                            Nop = g.Nop,
                            NamaOp = g.NamaOp,
                            AlamatOp = g.AlamatOp,
                            WilayahPajak = ((EnumFactory.EUPTB)g.WilayahPajak).GetDescription(),
                            UptbId = g.WilayahPajak,
                            TglTerpasang = tglTerpasang,
                            VedorId = g.Vendor,
                            Vendor = ((EnumFactory.EVendorParkirCCTV)g.Vendor).GetDescription(),
                            StatusTerpasang = statusTerpasang,
                            StatusAktif = isAktif,
                            Ishasdetail = isHaveDetail,
                            Ishaslog = ishaveLogs,
                            TglTerakhirAktif = tglTerakhirAktif,
                            TglTerakhirAktifString = tglTerakhirAktifString
                        };
                    })
                    .ToList();
                }
                return result;
            }
            public static List<MonitoringCCTVDet> GetMonitoringCCTVDetList(string nop, EnumFactory.EVendorParkirCCTV vendorid)
            {
                var result = new List<MonitoringCCTVDet>();
                var context = DBClass.GetContext();

                switch (vendorid)
                {
                    case EnumFactory.EVendorParkirCCTV.Jasnita:
                        result = context.MOpParkirCctvJasnitaLogs
                        .Where(l => l.Nop == nop)
                        .Select(l => new MonitoringCCTVDet
                        {
                            Nop = l.Nop,
                            Tgl = l.TglTerakhirAktif,
                            TglAktif = l.TglTerakhirAktif,
                            TglDown = l.TglTerakhirDown.HasValue ? l.TglTerakhirDown.Value : null,
                            StatusAktif = l.Status ?? ""
                        })
                        .ToList();
                        break;
                    case EnumFactory.EVendorParkirCCTV.Telkom:
                        result = context.MOpParkirCctvTelkomLogs
                        .Where(l => l.Nop == nop)
                        .Select(l => new MonitoringCCTVDet
                        {
                            Nop = l.Nop,
                            Tgl = l.TglTerakhirAktif,
                            TglAktif = l.TglTerakhirAktif,
                            TglDown = l.TglTerakhirDown.HasValue ? l.TglTerakhirDown.Value : null,
                            StatusAktif = l.Status ?? ""
                        })
                        .ToList();
                        break;
                    default:
                        break;
                }


                return result;
            }
            public static List<MonitoringCCTVKapasitas> GetMonitoringCCTVKapasitas(string nop, int vendorId)
            {
                var result = new List<MonitoringCCTVKapasitas>();
                var context = DBClass.GetContext();

                var kapasitas = context.DbPotensiParkirs
                    .Where(p => p.Nop == nop && p.TahunBuku == DateTime.Now.Year + 1)
                    .AsQueryable();

                var data = context.TOpParkirCctvs
                    .Where(l => l.Nop == nop && l.WaktuMasuk.Date == DateTime.Now.Date)
                    .GroupBy(l => l.JenisKend)
                    .Select(g => new
                    {
                        JenisKendaraan = g.Key,
                        JumlahKendaraanTerparkir = g.Count()
                    })
                    .ToList();
                result = data.Select(d => new MonitoringCCTVKapasitas
                {
                    Nop = nop,
                    Tanggal = DateTime.Today,
                    JenisKendaraan = d.JenisKendaraan,
                    JumlahKendaraanTerparkir = d.JumlahKendaraanTerparkir,
                    Kapasitas = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMotor ?? 0 : 0) :
                                d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMobil ?? 0 : 0) :
                                d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapTrukBus ?? 0 : 0) : 0,

                    Tarif = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMotor ?? 0 : 0) :
                            d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMobil ?? 0 : 0) :
                            d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifTrukBus ?? 0 : 0) : 0,
                }).ToList();
                return result;
            }
            public static List<MonitoringCCTVKapasitas> GetMonitoringCCTVKapasitas(string nop, DateTime tglAwal, DateTime tglAkhir)
            {
                var result = new List<MonitoringCCTVKapasitas>();
                var context = DBClass.GetContext();

                var kapasitas = context.DbPotensiParkirs
                    .Where(p => p.Nop == nop && p.TahunBuku == DateTime.Now.Year + 1)
                    .AsQueryable();

                var data = context.TOpParkirCctvs
                    .Where(l => l.Nop == nop && l.WaktuMasuk >= tglAwal && l.WaktuMasuk <= tglAkhir)
                    .GroupBy(l => l.JenisKend)
                    .Select(g => new
                    {
                        JenisKendaraan = g.Key,
                        JumlahKendaraanTerparkir = g.Count()
                    })
                    .ToList();
                result = data.Select(d => new MonitoringCCTVKapasitas
                {
                    Nop = nop,
                    JenisKendaraan = d.JenisKendaraan,
                    JumlahKendaraanTerparkir = d.JumlahKendaraanTerparkir,
                    Kapasitas = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMotor ?? 0 : 0) :
                                d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapMobil ?? 0 : 0) :
                                d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().KapTrukBus ?? 0 : 0) : 0,

                    Tarif = d.JenisKendaraan == 1 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMotor ?? 0 : 0) :
                            d.JenisKendaraan == 2 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifMobil ?? 0 : 0) :
                            d.JenisKendaraan == 3 ? (kapasitas.Any() ? kapasitas.FirstOrDefault().TarifTrukBus ?? 0 : 0) : 0,
                }).ToList();
                return result;
            }
            public static List<MonitoringCCTVLog> GetMonitoringCCTVLog(string nop, int jenisKend, DateTime tglAwal, DateTime tglAkhir)
            {
                var result = new List<MonitoringCCTVLog>();
                var context = DBClass.GetContext();
                result = context.TOpParkirCctvs
                .Where(l => l.Nop == nop && l.JenisKend == jenisKend && l.WaktuMasuk >= tglAwal && l.WaktuMasuk <= tglAkhir)
                .Select(l => new MonitoringCCTVLog
                {
                    Nop = l.Nop,
                    JenisKendaraan = l.JenisKend,
                    JenisKendaraanNama = ((EnumFactory.EJenisKendParkirCCTV)l.JenisKend).GetDescription(),
                    WaktuIn = l.WaktuMasuk,
                    WaktuOut = l.WaktuKeluar,
                    PlatNo = l.PlatNo ?? "-"
                })
                .ToList();
                return result;
            }
            public static List<MonitoringCCTVBulanan> GetMonitoringCCTVBulanan(string nop, int vendorId)
            {
                var result = new List<MonitoringCCTVBulanan>();

                var context = DBClass.GetContext();
                int tahunKemarin = DateTime.Now.Year - 1;
                int tahunIni = DateTime.Now.Year;
                int tahunDepan = DateTime.Now.Year + 1;

                var parkirPotensi = context.DbPotensiParkirs
                    .Where(x => x.TahunBuku == tahunDepan && x.Nop == nop)
                    .FirstOrDefault();

                var parkirTahunKemarin = context.DbMonParkirs
                    .Where(x => x.Nop == nop && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahunKemarin)
                    .ToList();

                var parkirTahunSekarang = context.DbMonParkirs
                    .Where(x => x.Nop == nop && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahunIni)
                    .ToList();

                var kendaraanParkir = context.TOpParkirCctvs
                    .Where(x => x.WaktuMasuk.Year == tahunIni && x.Nop == nop && x.Direction == ((int)EnumFactory.CctvParkirDirection.Incoming))
                    .GroupBy(x => new { x.JenisKend, x.WaktuMasuk.Month })
                    .Select(x => new { x.Key.JenisKend, Bln = x.Key.Month, Jml = x.Count() })
                    .ToList();

                for (int bln = 1; bln <= 12; bln++)
                {
                    var res = new MonitoringCCTVBulanan();

                    int jmlMotor = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Motor)
                        && x.Bln == bln
                    ).Sum(q => q.Jml);

                    int jmlMobil = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Mobil)
                        && x.Bln == bln
                    ).Sum(q => q.Jml);

                    int jmlTruck = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Truck)
                        && x.Bln == bln
                    ).Sum(q => q.Jml);

                    int jmlUnknown = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Unknown)
                        && x.Bln == bln
                    ).Sum(q => q.Jml);

                    decimal tarifMotor = parkirPotensi?.TarifMotor ?? 2000;
                    decimal tarifMobil = parkirPotensi?.TarifMobil ?? 4000;
                    decimal tarifUnknown = 2000;

                    decimal omsetMotor = (2000 * jmlMotor);
                    decimal omsetMobil = (4000 * (jmlMobil + jmlTruck));
                    decimal omsetUnknown = (tarifUnknown * (jmlUnknown));

                    decimal estimasiPajakMotor = omsetMotor * 0.1m;
                    decimal estimasiPajakMobil = omsetMobil * 0.1m;
                    decimal estimasiPajakUnknown = omsetUnknown * 0.1m;

                    decimal omset = (omsetMotor + omsetMobil + omsetUnknown);
                    decimal estimasi = (estimasiPajakMotor + estimasiPajakMobil + estimasiPajakUnknown);

                    decimal realisasiTahunKemarin = parkirTahunKemarin.Where(x => x.TglBayarPokok.Value.Month == bln).Sum(q => q.NominalPokokBayar) ?? 0;
                    decimal realisasiTahunIni = parkirTahunSekarang.Where(x => x.TglBayarPokok.Value.Month == bln).Sum(q => q.NominalPokokBayar) ?? 0;

                    decimal potensi = realisasiTahunIni - estimasi;

                    // ** LOGIKA BARU UNTUK POTENSI **
                    string potensiString = "";

                    if (realisasiTahunIni >= estimasi)
                    {
                        // KONDISI 1: Realisasi lebih besar dari estimasi -> Potensi = 0
                        potensiString = "0"; // Atau "0 (100%)" atau "Sesuai Target"
                    }
                    else
                    {
                        // KONDISI 2: Estimasi lebih besar dari Realisasi -> Hitung Selisih dan Persentase
                        decimal selisihNominal = estimasi - realisasiTahunIni;
                        // Persentase -> selisih - estimasi x 100%
                        decimal persentaseRealisasi = (estimasi == 0) ? 0 : (selisihNominal / estimasi) * 100m;
                        string selisihFormat = selisihNominal.ToString("N0", new CultureInfo("id-ID")); // Format Rupiah tanpa simbol
                        string persenFormat = persentaseRealisasi.ToString("F2", CultureInfo.InvariantCulture); // Format Persen 2 desimal

                        potensiString = $"Rp {selisihFormat} ({persenFormat}%)";

                    }

                    res.Nop = nop;
                    res.Tahun = tahunIni;
                    res.Bulan = bln;
                    res.NamaBulan = new DateTime(tahunIni, bln, 1).ToString("MMMM", new CultureInfo("id-ID"));
                    res.Motor = jmlMotor;
                    res.Mobil = (jmlMobil + jmlTruck);
                    res.Unknown = jmlUnknown;
                    res.Omset = omset;
                    res.EstimasiPajak = estimasi;
                    res.TahunKemarin = realisasiTahunKemarin;
                    res.TahunIni = realisasiTahunIni;
                    res.Potensi = potensiString;

                    result.Add(res);
                }

                return result;
            }
            public static List<MonitoringCCTVHarian> GetMonitoringCCTVHarian(string nop, int vendorId, int tahun, int bulan)
            {
                var result = new List<MonitoringCCTVHarian>();
                var context = DBClass.GetContext();
                int tahunKemarin = DateTime.Now.Year - 1;
                int tahunIni = DateTime.Now.Year;
                int tahunDepan = DateTime.Now.Year + 1;

                var parkirPotensi = context.DbPotensiParkirs
                    .Where(x => x.TahunBuku == tahunDepan && x.Nop == nop)
                    .FirstOrDefault();

                var parkirTahunKemarin = context.DbMonParkirs
                    .Where(x => x.Nop == nop && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahunKemarin && x.TglBayarPokok.Value.Month == bulan)
                    .ToList();

                var parkirTahunSekarang = context.DbMonParkirs
                    .Where(x => x.Nop == nop && x.TglBayarPokok.HasValue && x.TglBayarPokok.Value.Year == tahunIni && x.TglBayarPokok.Value.Month == bulan)
                    .ToList();

                var kendaraanParkir = context.TOpParkirCctvs
                    .Where(x => x.WaktuMasuk.Year == tahunIni
                        && x.WaktuMasuk.Month == bulan
                        && x.Nop == nop && x.Direction == ((int)EnumFactory.CctvParkirDirection.Incoming)
                    )
                    .GroupBy(x => new { x.JenisKend, x.WaktuMasuk.Month, x.WaktuMasuk.Day })
                    .Select(x => new { x.Key.JenisKend, Bln = x.Key.Month, Hari = x.Key.Day, Jml = x.Count() })
                    .ToList();

                var totalDay = DateTime.DaysInMonth(tahun, bulan);

                for (int tgl = 1; tgl <= totalDay; tgl++)
                {
                    var res = new MonitoringCCTVHarian();


                    int jmlMotor = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Motor)
                        && x.Bln == bulan
                        && x.Hari == tgl
                    ).Sum(q => q.Jml);

                    int jmlMobil = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Mobil)
                        && x.Bln == bulan
                        && x.Hari == tgl
                    ).Sum(q => q.Jml);

                    int jmlTruck = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Truck)
                        && x.Bln == bulan
                        && x.Hari == tgl
                    ).Sum(q => q.Jml);

                    int jmlUnknown = kendaraanParkir.Where(x =>
                        x.JenisKend == ((int)EnumFactory.EJenisKendParkirCCTV.Unknown)
                        && x.Bln == bulan
                        && x.Hari == tgl
                    ).Sum(q => q.Jml);

                    decimal tarifMotor = parkirPotensi?.TarifMotor ?? 2000;
                    decimal tarifMobil = parkirPotensi?.TarifMobil ?? 4000;
                    decimal tarifUnknown = 2000;

                    decimal omsetMotor = (2000 * jmlMotor);
                    decimal omsetMobil = (4000 * (jmlMobil + jmlTruck));
                    decimal omsetUnknown = (tarifUnknown * (jmlUnknown));

                    decimal estimasiPajakMotor = omsetMotor * 0.1m;
                    decimal estimasiPajakMobil = omsetMobil * 0.1m;
                    decimal estimasiPajakUnknown = omsetUnknown * 0.1m;

                    decimal omset = (omsetMotor + omsetMobil + omsetUnknown);
                    decimal estimasi = (estimasiPajakMotor + estimasiPajakMobil + estimasiPajakUnknown);

                    decimal realisasiTahunKemarin = parkirTahunKemarin
                        .Where(x => x.TglBayarPokok.Value.Month == bulan && x.TglBayarPokok.Value.Day == tgl)
                        .Sum(q => q.NominalPokokBayar) ?? 0;
                    decimal realisasiTahunIni = parkirTahunSekarang
                        .Where(x => x.TglBayarPokok.Value.Month == bulan && x.TglBayarPokok.Value.Day == tgl)
                        .Sum(q => q.NominalPokokBayar) ?? 0;

                    decimal potensi = realisasiTahunIni - estimasi;

                    res.Nop = nop;
                    res.Tanggal = new DateTime(tahun, bulan, tgl);
                    res.Motor = jmlMotor;
                    res.Mobil = (jmlMobil + jmlTruck);
                    res.Unknown = jmlUnknown;
                    res.Omset = omset;
                    res.EstimasiPajak = estimasi;
                    res.TahunKemarin = realisasiTahunKemarin;
                    res.TahunIni = realisasiTahunIni;
                    res.Potensi = potensi;

                    result.Add(res);
                }
                return result.OrderBy(x => x.Tanggal).ToList();
            }
            public static List<MonitoringCctvHarianDetail> GetMonitoringHarianDetail(string nop, int vendorId, DateTime tgl)
            {
                var result = new List<MonitoringCctvHarianDetail>();
                var context = DBClass.GetContext();

                var data = context.TOpParkirCctvs.Where(x => x.Nop == nop && x.WaktuMasuk.Date == tgl && x.Direction == ((int)EnumFactory.CctvParkirDirection.Incoming)).ToList();

                foreach (var item in data)
                {
                    var res = new MonitoringCctvHarianDetail();
                    res.Id = item.Id;
                    res.Nop = item.Nop;
                    res.CctvId = item.CctvId;
                    res.NamaOp = item.NamaOp;
                    res.AlamatOp = item.AlamatOp;
                    res.WilayahPajak = item.WilayahPajak;
                    res.TanggalMasuk = item.WaktuMasuk;
                    res.JenisKendEnum = (EnumFactory.EJenisKendParkirCCTV)item.JenisKend;
                    res.JenisKend = ((EnumFactory.EJenisKendParkirCCTV)item.JenisKend).GetDescription();
                    res.PlatNo = item.PlatNo;
                    res.Direction = ((EnumFactory.CctvParkirDirection)item.Direction).GetDescription();
                    res.Log = item.Log;
                    res.ImageUrl = item.ImageUrl;
                    res.IsLog = false;
                    res.IsOn = false;
                    result.Add(res);
                }

                var dataLogs = context.MOpParkirCctvJasnitaLogDs.Where(x => x.Nop == nop && x.TglEvent.Date == tgl).ToList();
                foreach (var item in dataLogs)
                {
                    var res = new MonitoringCctvHarianDetail();
                    res.Id = item.Guid;
                    res.Nop = item.Nop;
                    res.CctvId = item.CctvId;
                    res.NamaOp = "-";
                    res.AlamatOp = "-";
                    res.WilayahPajak = 0;
                    res.TanggalMasuk = item.TglEvent;
                    res.JenisKendEnum = EnumFactory.EJenisKendParkirCCTV.Unknown;
                    res.JenisKend = item.Event;
                    res.PlatNo = "";
                    res.Direction = "-";
                    res.Log = "";
                    res.ImageUrl = null;
                    res.IsLog = true;
                    res.IsOn = item.IsOn == 1 ? true : false;
                    result.Add(res);
                }

                return result.OrderBy(x => x.TanggalMasuk).ToList();
            }
            public static ViewModel.KapasitasChart GetKapasitasChart(List<MonitoringCctvHarianDetail> dataDetail, DateTime tanggal)
            {
                var result = new ViewModel.KapasitasChart();

                var kendaraanData = dataDetail.Where(x => x.IsLog == false).AsQueryable();
                var cctvEventData = dataDetail.Where(x => x.IsLog == true).AsQueryable();

                //CUSTOM
                var kendaraanMotor = kendaraanData
                    .Where(x => x.JenisKendEnum == EnumFactory.EJenisKendParkirCCTV.Motor)
                    .GroupBy(x => new { x.JenisKendEnum, x.TanggalMasuk })
                    .Select(x => new { x.Key.JenisKendEnum, x.Key.TanggalMasuk })
                    .ToList();
                var kendaraanMobil = kendaraanData
                    .Where(x => new[] { EnumFactory.EJenisKendParkirCCTV.Mobil, EnumFactory.EJenisKendParkirCCTV.Truck }.Contains(x.JenisKendEnum))
                    .GroupBy(x => new { x.JenisKendEnum, x.TanggalMasuk })
                    .Select(x => new { JenisKendEnum = EnumFactory.EJenisKendParkirCCTV.Mobil, x.Key.TanggalMasuk })
                    .ToList();

                var semuaKendaraan = kendaraanMotor
                    .Concat(kendaraanMobil)
                    .ToList();
                //CUSTOM

                //MASTER KENDARAAN
                result.MasterKendaraans = semuaKendaraan
                    .GroupBy(x => new { x.JenisKendEnum })
                    .Select(x => new ViewModel.KapasitasChart.MasterKendaraan
                    {
                        Color = Extension.GetColorVehicle(x.Key.JenisKendEnum),
                        Name = x.Key.JenisKendEnum.GetDescription()
                    }).ToList();

                //NGISI WAKTU KENDARAAN
                {
                    var dataKendaraanTanggal = semuaKendaraan
                        .GroupBy(x => new { x.JenisKendEnum, x.TanggalMasuk })
                        .Select(x => new
                        {
                            Jenis = x.Key.JenisKendEnum.GetDescription(),
                            Waktu = x.Key.TanggalMasuk
                        }).ToList();

                    var timeInterval = GetTimeIntervals(tanggal);
                    foreach (var start in timeInterval)
                    {
                        var end = start.AddMinutes(15);

                        var kendaraanDiInterval = dataKendaraanTanggal
                            .Where(k => k.Waktu >= start && k.Waktu < end)
                            .GroupBy(k => k.Jenis)
                            .Select(g => new KendaraanJenis
                            {
                                Name = g.Key,
                                Value = g.Count()
                            })
                            .ToList();

                        result.KendaraanDatas.Add(new KendaraanData()
                        {
                            Waktu = start.ToString("yyyy-MM-dd HH:mm"),
                            Jenis = kendaraanDiInterval
                        });
                    }
                }

                {
                    foreach (var item in cctvEventData)
                    {
                        var res = new ViewModel.KapasitasChart.CctvEvent();

                        res.Status = item.IsOn ? "ON" : "OFF";
                        res.Time = item.TanggalMasuk.ToString("yyyy-MM-dd HH:mm");

                        result.CctvEvents.Add(res);
                    }
                }


                return result;
            }


            public static List<DateTime> GetTimeIntervals(DateTime tanggal)
            {
                List<DateTime> result = new List<DateTime>();
                DateTime start = tanggal.Date;
                DateTime end = tanggal.Date.AddDays(1).AddMinutes(-15);

                for (DateTime current = start; current <= end; current = current.AddMinutes(15))
                {
                    result.Add(current);
                }

                return result;
            }
        }


        public class MonitoringCCTV
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public string NamaOp { get; set; } = null!;
            public string AlamatOp { get; set; } = null!;
            public string WilayahPajak { get; set; } = null!;
            public int UptbId { get; set; }
            public DateTime? TglTerpasang { get; set; } = DateTime.MinValue;
            public int VedorId { get; set; }
            public string Vendor { get; set; } = null!;
            public string StatusTerpasang { get; set; } = null!;
            public string StatusAktif { get; set; } = null!;
            public DateTime? TglTerakhirAktif { get; set; } = DateTime.MinValue;
            public string? TglTerakhirAktifString { get; set; }
            public bool Ishaslog { get; set; }
            public bool Ishasdetail { get; set; }
        }
        public class MonitoringCCTVDet
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public DateTime Tgl { get; set; }
            public DateTime TglAktif { get; set; }
            public DateTime? TglDown { get; set; }
            public string StatusAktif { get; set; } = null!;

            //public int Hari => (TglDown.Value - TglAktif).Days;
            //public int Jam => (TglDown.Value - TglAktif).Hours;
            //public int Menit => (TglDown.Value - TglAktif).Minutes;
            //public string DownTime
            //{
            //    get
            //    {
            //        var parts = new List<string>();

            //        if (Hari > 0) parts.Add($"{Hari} Hari");
            //        if (Jam > 0) parts.Add($"{Jam} Jam");
            //        if (Menit > 0) parts.Add($"{Menit} Menit");

            //        // Kalau semua 0 (misal TglDown == TglAktif)
            //        if (parts.Count == 0)
            //            return "0 Menit";

            //        return string.Join(" ", parts);
            //    }
            //}
        }
        public class MonitoringCCTVKapasitas
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public DateTime Tanggal { get; set; }
            public int JenisKendaraan { get; set; }
            public string JenisKendaraanNama => ((EnumFactory.EJenisKendParkirCCTV)JenisKendaraan).GetDescription();
            public int JumlahKendaraanTerparkir { get; set; }
            public decimal Tarif { get; set; }
            public int Kapasitas { get; set; }
        }
        public class MonitoringCCTVLog
        {
            public string Nop { get; set; } = null!;
            public string FormattedNOP => Utility.GetFormattedNOP(Nop);
            public int JenisKendaraan { get; set; }
            public string JenisKendaraanNama { get; set; } = null!;
            public DateTime WaktuIn { get; set; }
            public DateTime? WaktuOut { get; set; }   // nullable
            public string? PlatNo { get; set; }

            private TimeSpan LamaTerparkir => (WaktuOut ?? DateTime.Now) - WaktuIn;

            public int Hari => LamaTerparkir.Days;
            public int Jam => LamaTerparkir.Hours;
            public int Menit => LamaTerparkir.Minutes;

            public string Terparkir
            {
                get
                {
                    var parts = new List<string>();

                    if (Hari > 0) parts.Add($"{Hari} Hari");
                    if (Jam > 0) parts.Add($"{Jam} Jam");
                    if (Menit > 0) parts.Add($"{Menit} Menit");

                    if (parts.Count == 0)
                        return "0 Menit";

                    return string.Join(" ", parts);
                }
            }

            public bool MasihTerparkir => WaktuOut == null;
        }
    }

}
