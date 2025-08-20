using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using static MonPDLib.Helper;

namespace KetetapanHRWs
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
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == -3);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);


            for (int i = tahunAmbil; i <= tglServer.Year; i++)
            {
                HPPKetetapanProcess(i);
            }


            MailHelper.SendMail(
            false,
            "DONE HIBURAN  WS",
            $@"HIBURAN WS FINISHED",
            null
            );
        }

        private void HPPKetetapanProcess(int tahunBuku)
        {
            try
            {
                var tglMulai = DateTime.Now;
                var sw = new Stopwatch();
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                sw.Start();
                var sqlKetetapan = @"
                SELECT MAX(ID_KETETAPAN) ID_KETETAPAN , NOP,TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,MASA_PAJAK,SEQ, MAX(TGL_SPTPD) TGL_SPTPD,
            SUM(KETETAPAN_TOTAL) KETETAPAN_TOTAL,MAX(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO, MAX(JENIS_KEETETAPAN) JENIS_KETETAPAN,
            PAJAK_ID,NAMA_PAJAK_DAERAH
FROM (
SELECT ID_KETETAPAN,B.FK_NOP NOP,TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,BULAN_PAJAK MASA_PAJAK,1 SEQ,TGL_SPTPD_DISETOR TGL_SPTPD , KETETAPAN_TOTAL,
            JATUHTEMPO TGL_JATUH_TEMPO, 'SPTPD' JENIS_KEETETAPAN,
            CASE
            WHEN A.NAMA_PAJAK_DAERAH='RESTORAN' THEN 1
            WHEN A.NAMA_PAJAK_DAERAH='HOTEL' THEN 3
            ELSE
                -1
                END PAJAK_ID,
                A.NAMA_PAJAK_DAERAH
FROM VW_SIMPADAHPP_SPTPD_PHR@LIHATELANG A
JOIN VW_SIMPADA_OP_ALL_MON@LIHATHPPSERVER B ON A.FK_OP=B.ID_OP
JOIN JATUHTEMPO_MPS@LIHATELANG C ON A.BULAN_PAJAK=C.BULAN AND A.TAHUN_PAJAK=C.TAHUN 
WHERE A.NAMA_PAJAK_DAERAH  IN ('RESTORAN','HOTEL') AND TGL_SPTPD_DISETOR IS NOT NULL      
        AND TO_CHAR(TGL_SPTPD_DISETOR,'YYYY')=:TAHUN AND JATUHTEMPO IS NOT NULL
)
GROUP BY NOP,TAHUN_PAJAK,MASA_PAJAK,SEQ,PAJAK_ID,NAMA_PAJAK_DAERAH
UNION ALL
SELECT MAX(ID_KETETAPAN) ID_KETETAPAN , NOP,TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,MASA_PAJAK,SEQ, MAX(TGL_SPTPD) TGL_SPTPD,
            SUM(KETETAPAN_TOTAL) KETETAPAN_TOTAL,MAX(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO, MAX(JENIS_KEETETAPAN) JENIS_KETETAPAN,
            PAJAK_ID,NAMA_PAJAK_DAERAH
FROM (
SELECT NOMORSKPD ID_KETETAPAN,NOP,TO_NUMBER(TAHUN) TAHUN_PAJAK,MASAPAJAK MASA_PAJAK,1 SEQ,TGLDIBUAT TGL_SPTPD , JUMLAHKESELURUHAN KETETAPAN_TOTAL,
            TANGGALJATUHTEMPO TGL_JATUH_TEMPO, JENISSKPD JENIS_KEETETAPAN,            
             CASE 
         WHEN SUBSTR(NOP, 15, 3) = '902' THEN 3
         WHEN SUBSTR(NOP, 15, 3) = '901' THEN 1
         ELSE -1                            
                END PAJAK_ID,                
             CASE 
         WHEN SUBSTR(NOP, 15, 3) = '902' THEN 'HOTEL'
         WHEN SUBSTR(NOP, 15, 3) = '901' THEN 'RESTORAN'
         ELSE '-'                            
                END NAMA_PAJAK_DAERAH
FROM SKPD@LIHATELANG
WHERE NOP IS NOT NULL AND JENISSKPD='SKPDKB' AND NOMORSKPD IS NOT NULL and   TGLDIBUAT IS NOT NULL AND  TO_CHAR(TGLDIBUAT,'YYYY')=:TAHUN
) 
GROUP BY NOP,TAHUN_PAJAK,MASA_PAJAK,SEQ,PAJAK_ID,NAMA_PAJAK_DAERAH
                ";

                var result = _contMonitoringDB.Set<KETETAPANHR>()
                    .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();

                var removeEx = _contMonPd.DbMonKetetapanHrs.Where(x => x.TahunBuku == tahunBuku).ToList();
                int jmlData = result.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbMonKetetapanHr>();
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        if (!string.IsNullOrEmpty(item.NOP))
                        {
                            newList.Add(new DbMonKetetapanHr()
                            {
                                IdKetetapan = item.ID_KETETAPAN,
                                JenisKetetapan = item.JENIS_KETETAPAN,
                                KetetapanTotal = item.KETETAPAN_TOTAL,
                                MasaPajak = item.MASA_PAJAK,
                                NamaPajakDaerah = item.NAMA_PAJAK_DAERAH,
                                Nop = item.NOP.Replace(".", ""),
                                PajakId = item.PAJAK_ID,
                                SeqPajak = item.SEQ,
                                TahunBuku = tahunBuku,
                                TahunPajak = item.TAHUN_PAJAK,
                                TglJatuhTempo = item.TGL_JATUH_TEMPO.Value,
                                TglSptpd = item.TGL_SPTPD.Value
                            });
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error : {ex.Message}");
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] KETETAPAN HR TAHUN {tahunBuku} JML DATA {jmlData.ToString("n0")}      [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (removeEx.Any())
                {
                    _contMonPd.DbMonKetetapanHrs.RemoveRange(removeEx);
                    _contMonPd.SaveChanges();
                }
                if (newList.Any())
                {
                    _contMonPd.DbMonKetetapanHrs.AddRange(newList);
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
