using Microsoft.EntityFrameworkCore;
using MonPDLib;

namespace AbtWs
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
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                // Hitung waktu untuk 00:00 esok hari
                var nextRunTime = now.Date.AddHours(1); // Tambah 1 hari dan set jam 00:00
                var delay = nextRunTime - now;

                _logger.LogInformation("Next run scheduled at: {time}", nextRunTime);
                _logger.LogInformation("Next run scheduled : {lama}", delay.Hours + ":" + delay.Minutes);

                // Tunggu hingga waktu eksekusi
                await Task.Delay(delay, stoppingToken);

                // Eksekusi tugas
                try
                {
                    await DoWorkAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                }
            }
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {

            //if (IsRunOP)
            //{

            //}

            



            using (var context = DBClass.GetSurabayaTaxContext())
            {
                var sql = @"SELECT A.NOP,C.NPWPD_NO,C.NAMA NPWPD_NAMA,C.ALAMAT NPWPD_ALAMAT,6 PAJAK_ID,'PAJAK AIR TANAH' PAJAK_NAMA,
             A.NAMA NAMA_OP, A.ALAMAT ALAMAT_OP,A.ALAMAT_NO ALAMAT_OP_NO, A.RT ALAMAT_OP_RT,A.RW ALAMAT_OP_RW, A.TELP,A.KD_LURAH ALAMAT_OP_KD_LURAH, A.KD_CAMAT ALAMAT_OP_KD_CAMAT,
             TGL_OP_TUTUP,TGL_MULAI_BUKA_OP,B.PERUNTUKAN PERUNTUKAN_ID,
     case
     when B.peruntukan=1 then 'NIAGA'
     when B.peruntukan=2 then 'NON NIAGA'
     when B.peruntukan=3 then 'BAHAN BAKU AIR' 
     END PERUNTUKAN_NAMA,
     A.KATEGORI KATEGORI_ID,
     D.NAMA KATEGORI_NAMA,
     1 IS_METERAN_AIR, 0 JUMLAH_KARYAWAN,
      DECODE(TGL_OP_TUTUP,NULL,0,1) IS_TUTUP,
      sysdate INS_dATE, 'JOB' INS_BY,
      TO_CHAR(SYSDATE,'YYYY') TAHUN_BUKU,
       '-'  AKUN  ,
  '-'  NAMA_AKUN           ,
  '-'  JENIS             ,
  '-'  NAMA_JENIS        ,
  '-'  OBJEK            ,
  '-'  NAMA_OBJEK       ,
  '-'  RINCIAN         ,
  '-'  NAMA_RINCIAN     ,
  '-'  SUB_RINCIAN      ,
  '-'  NAMA_SUB_RINCIAN        
FROM OBJEK_PAJAK A
JOIN OBJEK_PAJAK_ABT B ON A.NOP=B.NOP
JOIN NPWPD  C ON A.NPWPD=C.NPWPD_no
JOIN M_KATEGORI_PAJAK D ON D.ID=A.KATEGORI";
                var _contMonPd = DBClass.GetContext();
                var result =await context.Database.SqlQueryRaw<MonPDLib.EF.OpAbt>(sql).ToListAsync();
                
                var source=await _contMonPd.DbOpAbts.ToListAsync();  


                

            }            
            
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.LastRuns.FirstOrDefault(x => x.Job == EnumFactory.EJobName.DBOPABT.ToString());
            if (row!=null)
            {
                if (row.InsDate.HasValue)
                {
                    var tglTarik = row.InsDate.Value.Date;
                    var tglServer= DateTime.Now.Date;
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
                    return true;
                }
            }
            return true;
        }
    }
}
