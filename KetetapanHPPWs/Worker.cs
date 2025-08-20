using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Diagnostics;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace KetetapanHPPWs
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
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == -2);
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
                SELECT *
FROM (
SELECT MAX(ID_KETETAPAN) ID_KETETAPAN , NOP,TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,MASA_PAJAK,SEQ, MAX(TGL_SPTPD) TGL_SPTPD,
            SUM(KETETAPAN_TOTAL) KETETAPAN_TOTAL,MAX(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO, MAX(JENIS_KEETETAPAN) JENIS_KETETAPAN,
            PAJAK_ID,NAMA_PAJAK_DAERAH
FROM (
SELECT ID_KETETAPAN,B.FK_NOP NOP,TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,BULAN_PAJAK MASA_PAJAK,1 SEQ,DECODE(TGL_SPTPD_DISETOR,NULL,TGL_REALISASI,TGL_SPTPD_DISETOR) TGL_SPTPD , KETETAPAN_TOTAL,
            JATUHTEMPO TGL_JATUH_TEMPO, 'SPTPD' JENIS_KEETETAPAN,
            CASE
            WHEN A.NAMA_PAJAK_DAERAH='HIBURAN' THEN 5
            WHEN A.NAMA_PAJAK_DAERAH='PPJ' THEN 2
            WHEN A.NAMA_PAJAK_DAERAH='PARKIR' THEN 4
            ELSE
                -1
                END PAJAK_ID,
                A.NAMA_PAJAK_DAERAH
FROM VW_SIMPADA_SPTPD_MONPD@LIHATHPPSERVER A
JOIN VW_SIMPADA_OP_ALL_MON@LIHATHPPSERVER B ON A.FK_OP=B.ID_OP
JOIN JATUHTEMPO_MPS@LIHATELANG C ON A.BULAN_PAJAK=C.BULAN AND A.TAHUN_PAJAK=C.TAHUN 
WHERE A.NAMA_PAJAK_DAERAH  IN ('HIBURAN','PPJ','PARKIR') AND (TGL_SPTPD_DISETOR IS NOT NULL OR TGL_REALISASI IS NOT NULL)             
    AND TO_CHAR(DECODE(TGL_SPTPD_DISETOR,NULL,TGL_REALISASI,TGL_SPTPD_DISETOR),'YYYY')=:TAHUN
)
GROUP BY NOP,TAHUN_PAJAK,MASA_PAJAK,SEQ,PAJAK_ID,NAMA_PAJAK_DAERAH
UNION ALL
SELECT MAX(ID_KETETAPAN),B.FK_NOP NOP,TO_NUMBER(TAHUN_PAJAK),BULAN_PAJAK MASA_PAJAK,1 SEQ, MAX(TGL_KETETAPAN) TGL_SPTPD,
            SUM(TOTAL_NILAI_KETETAPAN) KETETAPAN_TOTAL,MAX(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO, JENIS_KETETAPAN,
            5 PAJAK_ID,'HIBURAN' NAMA_PAJAK_DAERAH
FROM HIBURAN_KETETAPAN@LIHATHPPSERVER A
JOIN VW_SIMPADA_OP_ALL_MON@LIHATHPPSERVER B ON A.FK_WP=B.ID_OP
WHERE  TO_CHAR(TGL_KETETAPAN,'YYYY')=:TAHUN
GROUP BY B.FK_NOP,TAHUN_PAJAK,BULAN_PAJAK,1,JENIS_KETETAPAN,5,'HIBURAN'
UNION ALL
SELECT MAX(NO_SKPD),B.FK_NOP NOP,TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,MASA_PAJAK,1 SEQ, MAX(TGL_PENETAPAN) TGL_SPTPD,
            SUM(NILAI_KETETAPAN) KETETAPAN_TOTAL,MAX(TGL_JATUH_TEMPO) TGL_JATUH_TEMPO,JENIS_DOKUMEN JENIS_KETETAPAN,
            4 PAJAK_ID,B.NAMA_PAJAK_DAERAH            
FROM SKPD@LIHATHPPSERVER A
JOIN VW_SIMPADA_OP_ALL_MON@LIHATHPPSERVER B ON A.NO_OBYEK_PAJAK=B.ID_OP
where B.NAMA_PAJAK_DAERAH='PARKIR' and JENIS_DOKUMEN='SKPDKB' AND  TO_CHAR(TGL_PENETAPAN,'YYYY')=:TAHUN
GROUP BY B.FK_NOP,TAHUN_PAJAK,MASA_PAJAK,1, JENIS_DOKUMEN,4,B.NAMA_PAJAK_DAERAH
)
WHERE NOP IS not  NULL
                ";

                var result = _contMonitoringDB.Set<KETETAPANPBJT>()
                    .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();

                var removeEx = _contMonPd.DbMonKetetapanHpps.Where(x => x.TahunBuku == tahunBuku).ToList();
                int jmlData = result.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbMonKetetapanHpp>();
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        if (!string.IsNullOrEmpty(item.NOP))
                        {
                            newList.Add(new DbMonKetetapanHpp()
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
                                TglJatuhTempo = item.TGL_JATUH_TEMPO,
                                TglSptpd = item.TGL_SPTPD
                            });
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error : {ex.Message}");
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[{tglMulai.ToString("dd MMM yyyy HH:mm:ss")}] KETETAPAN HPP TAHUN {tahunBuku} JML DATA {jmlData.ToString("n0")}      [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (removeEx.Any())
                {
                    _contMonPd.DbMonKetetapanHpps.RemoveRange(removeEx);
                    _contMonPd.SaveChanges();
                }
                if (newList.Any())
                {
                    _contMonPd.DbMonKetetapanHpps.AddRange(newList);
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
