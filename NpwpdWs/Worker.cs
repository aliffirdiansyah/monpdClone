using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Linq;
using static MonPDLib.Helper;

namespace NpwpdWs
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //var now = DateTime.Now;

                //var nextRun = now.AddDays(1); // besok jam 00:00
                //var delay = nextRun - now;

                //_logger.LogInformation("Next run scheduled at: {time}", nextRun);

                //await Task.Delay(delay, stoppingToken);

                //if (stoppingToken.IsCancellationRequested)
                //    break;

                try
                {
                    // TODO: Taruh pekerjaanmu di sini
                    _logger.LogInformation("Running daily task at: {time}", DateTimeOffset.Now);

                    // Contoh pekerjaan:
                    await DoWorkFullScanAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during daily task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR NPWPD WS",
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
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();

            if (IsGetDBWp())
            {
                using (var _contSbyTax = DBClass.GetSurabayaTaxContext())
                {
                    var sql = @"SELECT * FROM NPWPD";

                    var result = await _contSbyTax.Set<Npwpd>().FromSqlRaw(sql).ToListAsync();

                    var dataNpwpdExisting = _contMonPd.Npwpds.ToList();
                    _contMonPd.Npwpds.RemoveRange(dataNpwpdExisting);

                    foreach (var item in result)
                    {
                        Console.WriteLine($"[INSERT] DB_NPWPD_SBYTAX {item.NpwpdNo}");

                        var newRow = new Npwpd();
                        newRow.NpwpdNo = item.NpwpdNo;
                        newRow.Status = item.Status;
                        newRow.Nama = item.Nama;
                        newRow.Alamat = item.Alamat;
                        newRow.Kota = item.Kota;
                        newRow.Kontak = item.Kontak;
                        newRow.Email = item.Email;
                        newRow.RefBlnPel = item.RefBlnPel;
                        newRow.RefThnPel = item.RefThnPel;
                        newRow.RefSeqPel = item.RefSeqPel;
                        newRow.RefWf = item.RefWf;
                        newRow.InsDate = item.InsDate;
                        newRow.InsBy = item.InsBy;
                        newRow.Password = item.Password;
                        newRow.LastAct = item.LastAct;
                        newRow.ResetKey = item.ResetKey;
                        newRow.Hp = item.Hp;
                        newRow.NpwpdLama = item.NpwpdLama;
                        newRow.JenisWp = item.JenisWp;
                        newRow.AlamatDomisili = item.AlamatDomisili;
                        newRow.KodeOtp = item.KodeOtp;
                        newRow.IsValid = item.IsValid;

                        _contMonPd.Npwpds.Add(newRow);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[INSERT] DB_NPWPD_SBYTAX {item.NpwpdNo}");
                        Console.ResetColor();

                    }
                    _contMonPd.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[INSERTED] DB_NPWPD_SBYTAX");
                    Console.ResetColor();
                }

                using (var _contPhrh = DBClass.GetPhrhContext())
                {
                    var sql = @"
                    SELECT 
                        NPWPD AS NPWPD_NO,
                        5 AS JENIS_WP,
                        1 AS STATUS,
                        NVL(NAMAWP, '-') AS NAMA,
                        NVL(ALAMAT, '-') ALAMAT,
                        NVL(ALAMAT, '-') AS ALAMAT_DOMISILI,
                        '3578' AS KOTA,
                        NVL(NOTLP , '-') AS HP,
                        NVL(NOTLP , '-') AS KONTAK,
                        TRUNC(DBMS_RANDOM.VALUE(100000, 999999)) || '@dummy.com' AS EMAIL,
                        5 REF_BLN_PEL,
                        2024 REF_THN_PEL,
                        -1 REF_SEQ_PEL,
                        NPWPD AS NPWPD_LAMA,
                        -1 REF_WF,
                        'MIGRATION' INS_BY,
                        '-' RESET_KEY
                        FROM npwpd_baru";

                    var result = await _contPhrh.Set<NpwpdPhr>().FromSqlRaw(sql).ToListAsync();
                    var distinctNpwpd = _contMonPd.Npwpds.Select(x => x.NpwpdNo).ToList();

                    result = result.Where(x => (!distinctNpwpd.Contains(x.NPWPD_NO))).ToList();
                    foreach (var item in result)
                    {
                        Console.WriteLine($"[INSERT] DB_NPWPD_PRHRH {item.NPWPD_NO}");

                        var newRow = new Npwpd();
                        newRow.NpwpdNo = item.NPWPD_NO;
                        newRow.Status = item.STATUS;
                        newRow.Nama = item.NAMA;
                        newRow.Alamat = item.ALAMAT;
                        newRow.Kota = item.KOTA;
                        newRow.Kontak = item.KONTAK;
                        newRow.Email = item.EMAIL;
                        newRow.RefBlnPel = item.REF_BLN_PEL;
                        newRow.RefThnPel = item.REF_THN_PEL;
                        newRow.RefSeqPel = item.REF_SEQ_PEL;
                        newRow.RefWf = item.REF_WF;
                        newRow.InsDate = DateTime.Now;
                        newRow.InsBy = item.INS_BY;
                        newRow.Password = "-";
                        newRow.LastAct = null;
                        newRow.ResetKey = "123456";
                        newRow.Hp = item.HP;
                        newRow.NpwpdLama = item.NPWPD_LAMA;
                        newRow.JenisWp = item.JENIS_WP;
                        newRow.AlamatDomisili = item.ALAMAT_DOMISILI;
                        newRow.KodeOtp = null;
                        newRow.IsValid = 1;

                        _contMonPd.Npwpds.Add(newRow);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"[INSERT] DB_NPWPD_PRHRH {item.NPWPD_NO}");
                        Console.ResetColor();
                    }

                    _contMonPd.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[INSERTED] DB_NPWPD_PRHRH");
                    Console.ResetColor();
                }
            }


            MailHelper.SendMail(
            false,
            "DONE NPWPD WS",
            $@"NPWPD WS FINISHED",
            null
            );
        }

        private bool IsGetDBWp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBNPWPD.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBNPWPD.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }
    }
}
