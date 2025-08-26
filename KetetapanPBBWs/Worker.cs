using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;
using static MonPDLib.Helper;

namespace KetetapanPBBWs
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private bool isFirst = true;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
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

                //// GUNAKAN KETIKA EKSEKUSI TUGAS MANUAL
                try
                {
                    await DoWorkNewMeta(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR HIBURAN WS",
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

        private async Task DoWorkNewMeta(CancellationToken stoppingToken)
        {
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == -4);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);


            for (int i = tahunAmbil; i <= tglServer.Year; i++)
            {
                PBBKetetapanProcess(i);
            }


            MailHelper.SendMail(
            false,
            "DONE HIBURAN  WS",
            $@"HIBURAN WS FINISHED",
            null
            );
        }

        private void PBBKetetapanProcess(int tahunBuku)
        {
            try
            {
                var tglMulai = DateTime.Now;
                var sw = new Stopwatch();
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                sw.Start();
                var sqlKetetapan = @"
                           SELECT T_PROP_KD||T_DATI2_KD||T_KEC_KD ||T_KEL_KD || D_NOP_BLK || D_NOP_URUT || D_NOP_JNS || D_PJK_THN ID_KETETAPAN,
             T_PROP_KD||T_DATI2_KD||T_KEC_KD ||T_KEL_KD || D_NOP_BLK || D_NOP_URUT || D_NOP_JNS NOP, 
             to_number(D_PJK_THN) TAHUN_PAJAK,-1 MASA_PAJAK,1 SEQ,D_CREA_DATE TGL_SKPD,to_number(D_PJK_TAX -NVL(D_PJK_ADJ,0)) KETETAPAN_TOTAL,D_PJK_TJTT TGL_JATUH_TEMPO,'SKPD' JENIS_KETETAPAN,
             9 PAJAK_ID,'PBB' NAMA_PAJAK_DAERAH
FROM DATABAYAR@LIHATGATOTKACA
WHERE TO_CHAR(D_CREA_DATE,'YYYY') =:TAHUN
                ";

                var result = _contMonitoringDB.Set<KETETAPANPBB>()
                    .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();

                var removeEx = _contMonPd.DbMonKetetapanPbbs.Where(x => x.TahunBuku == tahunBuku).ToList();
                int jmlData = result.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbMonKetetapanPbb>();
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        if (!string.IsNullOrEmpty(item.NOP))
                        {
                            if (!item.NOP.Contains("-"))
                            {
                                newList.Add(new DbMonKetetapanPbb()
                                {
                                    IdKetetapan = item.ID_KETETAPAN ?? "-",
                                    JenisKetetapan = item.JENIS_KETETAPAN,
                                    KetetapanTotal = item.KETETAPAN_TOTAL ?? 0,
                                    MasaPajak = item.MASA_PAJAK ?? 0,
                                    NamaPajakDaerah = item.NAMA_PAJAK_DAERAH,
                                    Nop = item.NOP.Replace(".", ""),
                                    PajakId = item.PAJAK_ID.Value,
                                    SeqPajak = item.SEQ,
                                    TahunBuku = tahunBuku,
                                    TahunPajak = item.TAHUN_PAJAK ?? 0,
                                    TglJatuhTempo = item.TGL_JATUH_TEMPO.Value,
                                    TglSkpd = item.TGL_SKPD.Value
                                });
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error : {ex.Message}");
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] KETETAPAN PBB TAHUN {tahunBuku} JML DATA {jmlData.ToString("n0")}      [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (removeEx.Any())
                {
                    _contMonPd.DbMonKetetapanPbbs.RemoveRange(removeEx);
                    _contMonPd.SaveChanges();
                }
                if (newList.Any())
                {
                    _contMonPd.DbMonKetetapanPbbs.AddRange(newList);
                    _contMonPd.SaveChanges();
                }
                sw.Stop();
                Console.Write($"Done {sw.Elapsed.Minutes} Menit {sw.Elapsed.Seconds} Detik");
                Console.WriteLine($"");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing NOP {ex.Message}");
            }
        }
    }
}
