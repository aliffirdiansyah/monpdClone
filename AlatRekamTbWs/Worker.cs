using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;

namespace AlatRekamTbWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 2;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                DateTime nextRun = now.AddSeconds(1); // besok jam 00:00
                TimeSpan delay = nextRun - now;
                if (isFirst)
                {
                    nextRun = now.AddSeconds(1); // besok jam 00:00
                    delay = nextRun - now;
                    isFirst = false;
                }
                else
                {
                    nextRun = now.AddHours(1); // next jam 00:00
                    delay = nextRun - now;
                }


                _logger.LogInformation("Next run scheduled at: {time}", nextRun);

                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;

                // Eksekusi tugas
                try
                {
                    await DoWorkFullScanAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR TBSB WS",
                    $@"
                            Terjadi exception pada sistem:

                            Pesan Error       : {ex.Message}
                            Tipe Exception    : {ex.GetType().FullName}
                            Source            : {ex.Source}
                            Method            : {ex.TargetSite}
                            Stack Trace       :
                            {ex.StackTrace}

                            Inner Exception   :
                            {ex.InnerException?.Message}
                            {ex.InnerException?.StackTrace}
                            ",
                        null
                    );
                }
            }
        }

        private async Task DoWorkFullScanAsync(CancellationToken stoppingToken)
        {
            if (IsGetDBOp())
            {
                var tglServer = DateTime.Now;
                var _contMonPd = DBClass.GetContext();

                using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                {
                    var sql = @"
                        SELECT  REPLACE(NOP, '.', '') NOP, 
		                        KETERANGAN, 
		                        '-' NAMA_OBJEK,
		                        '-' ALAMAT_OBJEK,
		                        PAJAK_ID, 
		                        TGL_TERPASANG
                        FROM (
                            SELECT 
                                NOP,  
                                KETERANGAN,
                                PAJAK_ID,
                                TGL_TERPASANG,
                                ROW_NUMBER() OVER (PARTITION BY NOP ORDER BY TGL_TERPASANG DESC) AS RN
                            FROM (
                                SELECT 
                                    NOP, 
                                    CASE 
                                        WHEN JENIS_PAJAK = 'RESTORAN' THEN 1
                                        WHEN JENIS_PAJAK = 'HIBURAN' THEN 5
                                        WHEN JENIS_PAJAK = 'HOTEL' THEN 3
                                        WHEN JENIS_PAJAK = 'PARKIR' THEN 4
                                        ELSE 0
                                    END AS PAJAK_ID,
                                    TRIM(SUBSTR(NAMA_OBYEK, 1, INSTR(NAMA_OBYEK, '-') - 1)) AS NAMA_OP,
                                    TRIM(SUBSTR(NAMA_OBYEK, INSTR(NAMA_OBYEK, '-') + 1)) AS ALAMAT_OP,
                                    CREATE_DATE AS TGL_TERPASANG,
                                    KETERANGAN
                                FROM PO.USER_NOP_TBSB@LIHATSURVEILLANCE
                            ) 
                        ) WHERE RN = 1 AND NOP != '-'
                    ";

                    Console.WriteLine($@"{DateTime.Now} TBSB WS STARTED");
                    var result = await _contMonitoringDb.Set<DbRekamAlatTbsb>().FromSqlRaw(sql).ToListAsync();
                    int jmlData = result.Count;
                    int index = 0;

                    var source = _contMonPd.DbRekamAlatTbsbs.ToList();
                    _contMonPd.DbRekamAlatTbsbs.RemoveRange(source);
                    Console.WriteLine($@"{DateTime.Now} TBSB EXISTING REMOVED");

                    foreach (var item in result)
                    {

                        var newRow = new DbRekamAlatTbsb();

                        newRow.Nop = item.Nop;
                        newRow.PajakId = item.PajakId;

                        var op = OvverideOp(item.PajakId, item.Nop);

                        newRow.NamaObjek = op.NamaOp;
                        newRow.AlamatObjek = op.AlamatOp;
                        newRow.Keterangan = item.Keterangan;

                        _contMonPd.DbRekamAlatTbsbs.Add(newRow);

                        double persen = ((double)index / jmlData) * 100;
                        Console.Write($"\rTBSB MONITORINGDB JML OP {jmlData.ToString("n0")} {item.Nop} : {persen:F2}%   ");

                        index++;
                    }
                    _contMonPd.SaveChanges();
                }


                MailHelper.SendMail(
                false,
                "DONE TBSB WS",
                $@"TBSB WS FINISHED",
                null
                );
            }
        }

        public static OracleConnection getOracleConnection()
        {
            try
            {
                OracleConnection ret = new OracleConnection(MonPDLib.DBClass.MonitoringDb);
                ret.Open();
                ret.Close();
                return ret;
            }
            catch (Exception ex)
            {
                return new OracleConnection();
            }
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPALATREKAMTBSB.ToString().ToUpper());
            if (row != null)
            {
                if (row.InsDate.HasValue)
                {
                    var tglTarik = row.InsDate.Value.Date;
                    var tglServer = DateTime.Now.Date;
                    if (tglTarik >= tglServer)
                    {
                        return false;
                    }
                    else
                    {
                        row.InsDate = DateTime.Now;
                        _contMonPd.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    row.InsDate = DateTime.Now;
                    _contMonPd.SaveChanges();
                    return true;
                }
            }
            var newRow = new MonPDLib.EF.SetLastRun();
            newRow.Job = EnumFactory.EJobName.DBOPALATREKAMTBSB.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }

        public OvverideOpProp OvverideOp(int pajakId, string nop)
        {
            var result = new OvverideOpProp();
            result.NamaOp = "-";
            result.AlamatOp = "-";

            var _contMonPd = DBClass.GetContext();

            switch ((EnumFactory.EPajak)pajakId)
            {
                case EnumFactory.EPajak.MakananMinuman:
                    var resto = _contMonPd.DbOpRestos.Where(x => x.Nop == nop).FirstOrDefault();
                    if (resto != null)
                    {
                        result.NamaOp = resto.NamaOp;
                        result.AlamatOp = resto.AlamatOp;
                    }
                    break;
                case EnumFactory.EPajak.TenagaListrik:
                    var listrik = _contMonPd.DbOpListriks.Where(x => x.Nop == nop).FirstOrDefault();
                    if (listrik != null)
                    {
                        result.NamaOp = listrik.NamaOp;
                        result.AlamatOp = listrik.AlamatOp;
                    }
                    break;
                case EnumFactory.EPajak.JasaPerhotelan:
                    var hotel = _contMonPd.DbOpHotels.Where(x => x.Nop == nop).FirstOrDefault();
                    if (hotel != null)
                    {
                        result.NamaOp = hotel.NamaOp;
                        result.AlamatOp = hotel.AlamatOp;
                    }
                    break;
                case EnumFactory.EPajak.JasaParkir:
                    var parkir = _contMonPd.DbOpParkirs.Where(x => x.Nop == nop).FirstOrDefault();
                    if (parkir != null)
                    {
                        result.NamaOp = parkir.NamaOp;
                        result.AlamatOp = parkir.AlamatOp;
                    }
                    break;
                case EnumFactory.EPajak.JasaKesenianHiburan:
                    var hiburan = _contMonPd.DbOpHiburans.Where(x => x.Nop == nop).FirstOrDefault();
                    if (hiburan != null)
                    {
                        result.NamaOp = hiburan.NamaOp;
                        result.AlamatOp = hiburan.AlamatOp;
                    }
                    break;
                case EnumFactory.EPajak.AirTanah:
                    var airTanah = _contMonPd.DbOpAbts.Where(x => x.Nop == nop).FirstOrDefault();
                    if (airTanah != null)
                    {
                        result.NamaOp = airTanah.NamaOp;
                        result.AlamatOp = airTanah.AlamatOp;
                    }
                    break;
                case EnumFactory.EPajak.Reklame:
                    break;
                case EnumFactory.EPajak.PBB:
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

            return result;
        }

        public class OvverideOpProp
        {
            public string NamaOp { get; set; } = null!;
            public string AlamatOp { get; set; } = null!;
        }
    }
}
