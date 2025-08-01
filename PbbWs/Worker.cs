using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace PbbWs
{
    public class Worker : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<Worker> _logger;
        private static int KDPajak = 9;

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
                    "ERROR PBB WS",
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
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);
            
            if (IsGetDBOp())
            {
                GetOPProcess();
            }

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                GetKetetapan(i);                
            }
            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                GetRealisasi(i);
            }
            
            MailHelper.SendMail(
            false,
            "DONE PBB WS",
            $@"PBB WS FINISHED",
            null
            );
        }

        private void GetOPProcess()
        {
            using (var _contMonitoringDB = DBClass.GetMonitoringDbContext())
            {
                var sql = @"
         select A.T_PROP_KD ||A.T_DATI2_KD ||A.T_KEC_KD || A.T_KEL_KD || A.D_NOP_BLK || A.D_NOP_URUT  || A.D_NOP_JNS  NOP,
         57 KATEGORI_ID,'PBB' KATEGORI_NAMA,
         NVL(D_OP_JLN,'-') ALAMAT_OP,
          NVL(D_OP_JLNO ,'-') ALAMAT_OP_NO,
          NVL(D_OP_RT ,'-')   ALAMAT_OP_RT  ,
          NVL(D_OP_RW ,'-')     ALAMAT_OP_RW ,
         NVL(KD_CAMAT ,'-')   ALAMAT_KD_CAMAT  ,
 NVL(KD_LURAH ,'-') ALAMAT_KD_LURAH  ,
 TO_NUMBER( NVL(UPTD,0)) UPTB     ,
TO_NUMBER(NVL(D_TNH_LUAS,0)) LUAS_TANAH,
 NVL(D_WP_JLN ,'-')  ALAMAT_WP ,
 NVL(D_WP_JLNO ,'-') ALAMAT_WP_NO,
NVL(D_WP_KEL ,'-')  ALAMAT_WP_KEL,
 NVL(D_WP_KOTA ,'-') ALAMAT_WP_KOTA ,
 NVL(D_WP_KTP ,'-') WP_KTP        ,
 NVL(D_WP_NAMA ,'-') WP_NAMA       ,
 NVL(D_WP_NPWP ,'-') WP_NPWP       ,
 NVL(D_WP_RT ,'-') WP_RT         ,
 NVL(D_WP_RW ,'-')  WP_RW         ,
 TO_NUMBER(NVL(STATUSAKTIF,0)) STATUS        ,
SYSDATE INS_DATE      ,
 'JOB 'INS_BY        
 FROM DATAOP@LIHATGATOTKACA A
   JOIN M_WILAYAHV1 B ON KD_CAMAT=KD_KECAMATAN AND KD_LURAH=KD_KELURAHAN   
                    ";

                var result = _contMonitoringDB.Set<OPPbb>().FromSqlRaw(sql).ToList();
                var _contMonPd = DBClass.GetContext();
                int jmlData = result.Count;
                int index = 0;
                foreach (var item in result)
                {
                    // DATA OP
                    try
                    {
                        var sourceRow = _contMonPd.DbOpPbbs.SingleOrDefault(x => x.Nop == item.NOP);
                        if (sourceRow != null)
                        {
                            sourceRow.Status = item.STATUS;
                            sourceRow.LuasTanah = item.LUAS_TANAH;
                            sourceRow.AlamatKdLurah = item.ALAMAT_KD_LURAH;
                            sourceRow.AlamatKdCamat = item.ALAMAT_KD_CAMAT;
                            _contMonPd.SaveChanges();
                        }
                        else
                        {
                            var newRow = new MonPDLib.EF.DbOpPbb();

                            newRow.Nop = item.NOP;
                            newRow.KategoriId = item.KATEGORI_ID;
                            newRow.KategoriNama = item.KATEGORI_NAMA;
                            newRow.AlamatOp = item.ALAMAT_OP;
                            newRow.AlamatOpNo = item.ALAMAT_OP_NO;
                            newRow.AlamatOpRt = item.ALAMAT_OP_RT;
                            newRow.AlamatOpRw = item.ALAMAT_OP_RW;
                            newRow.AlamatKdCamat = item.ALAMAT_KD_CAMAT;
                            newRow.AlamatKdLurah = item.ALAMAT_KD_LURAH;
                            newRow.Uptb = item.UPTB;
                            newRow.LuasTanah = item.LUAS_TANAH;
                            newRow.AlamatWp = item.ALAMAT_WP;
                            newRow.AlamatWpNo = item.ALAMAT_WP_NO;
                            newRow.AlamatWpKel = item.ALAMAT_WP_KEL;
                            newRow.AlamatWpKota = item.ALAMAT_WP_KOTA;
                            newRow.WpKtp = item.WP_KTP;
                            newRow.WpNama = item.WP_NAMA;
                            newRow.WpNpwp = item.WP_NPWP;
                            newRow.WpRt = item.WP_RT;
                            newRow.WpRw = item.WP_RW;
                            newRow.Status = item.STATUS;
                            newRow.InsDate = item.INS_DATE;
                            newRow.InsBy = item.INS_BY;
                            _contMonPd.DbOpPbbs.Add(newRow);
                            _contMonPd.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing NOP {ex.Message}");
                        var kkk = item.NOP;
                    }
                    _contMonPd.SaveChanges();
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_OP_PBB JML OP {jmlData} : {item.NOP}  {persen:F2}%   ");
                    Thread.Sleep(50);
                }
            }
        }

        private void GetKetetapan(int tahunBuku)
        {            
            try
            {
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlKetetapan = @"
               SELECT  NOP,TO_NUMBER(TAHUN_BUKU) TAHUN_BUKU, TO_NUMBER(KATEGORI_ID) KATEGORI_ID,
                            NVL(KATEGORI_NAMA,'-') KATEGORI_NAMA, NVL(ALAMAT_OP,'-') ALAMAT_OP,NVL(ALAMAT_OP_NO,'-') ALAMAT_OP_NO,
                            NVL(ALAMAT_OP_RT,'-') ALAMAT_OP_RT ,NVL(ALAMAT_OP_RW,'-') ALAMAT_OP_RW ,NVL(ALAMAT_KD_CAMAT,'-') ALAMAT_KD_CAMAT  ,
                            NVL(ALAMAT_KD_LURAH,'-') ALAMAT_KD_LURAH ,TO_NUMBER(nvl(UPTB,0)) UPTB,NVL(ALAMAT_WP,'-') ALAMAT_WP,
                            NVL(ALAMAT_WP_NO,'-') ALAMAT_WP_NO, NVL(ALAMAT_WP_KEL,'-') ALAMAT_WP_KEL,NVL(ALAMAT_WP_KOTA,'-') ALAMAT_WP_KOTA,
                            NVL(WP_NAMA,'-') WP_NAMA, NVL(WP_NPWP,'-') WP_NPWP,
                            TO_NUMBER(TAHUN_PAJAK) TAHUN_PAJAK,
                            TO_NUMBER(nvl(POKOK_PAJAK,0)) POKOK_PAJAK,KATEGORI_OP,NVL(B.KATEGORI,'BELUM DIKETAHUI') PERUNTUKAN,                                       
                            TO_NUMBER(nvl( IS_LUNAS,1)) IS_LUNAS ,TGL_BAYAR ,nvl(JUMLAH_BAYAR_POKOK,0)  JUMLAH_BAYAR_POKOK,
                            nvl(JUMLAH_BAYAR_SANKSI,0) JUMLAH_BAYAR_SANKSI,INS_DATE ,INS_BY    
FROM (
SELECT  K.T_PROP_KD ||K.T_DATI2_KD ||K.T_KEC_KD || K.T_KEL_KD || K.D_NOP_BLK || K.D_NOP_URUT  || K.D_NOP_JNS NOP,
 :TAHUN TAHUN_BUKU      ,
  57 KATEGORI_ID   ,
  'PBB' KATEGORI_NAMA,
         K.D_OP_JLN ALAMAT_OP,
           K.D_OP_JLNO  ALAMAT_OP_NO,
           K.D_OP_RT   ALAMAT_OP_RT  ,
           K.D_OP_RW     ALAMAT_OP_RW ,
          K.KD_CAMAT   ALAMAT_KD_CAMAT  ,
  K.KD_LURAH ALAMAT_KD_LURAH  ,
  P.UPTD UPTB,  
  D_WP_JLN  ALAMAT_WP ,
  D_WP_JLNO ALAMAT_WP_NO,
 D_WP_KEL  ALAMAT_WP_KEL,
  D_WP_KOTA ALAMAT_WP_KOTA , 
    D_WP_NAMA WP_NAMA       ,
  D_WP_NPWP WP_NPWP       ,  
  D_PJK_THN TAHUN_PAJAK       ,  
  D_PJK_TAX-NVL(D_PJK_ADJ,0) POKOK_PAJAK        ,  
  CASE
                WHEN D_PJK_LUNAS='Y' THEN 1                
                ELSE 0
                END   IS_LUNAS ,
                CASE
                WHEN LUAS_BANGUNANB>0 THEN 'BANGUNAN BERSAMA'
                WHEN LUAS_BANGUNAN=0 THEN 'TANAH KOSONG'
                ELSE 'TANAH DAN BANGUNAN'
                END  KATEGORI_OP ,
  D_PJK_TGBYR TGL_BAYAR ,
  0 JUMLAH_BAYAR_POKOK ,
   0 JUMLAH_BAYAR_SANKSI ,
  SYSDATE INS_DATE ,
    'JOB' INS_BY,        
    A.T_PROP_KD,A.T_DATI2_KD,A.T_KEC_KD,A.T_KEL_KD,A.D_NOP_BLK,A.D_NOP_URUT,A.D_NOP_JNS      
FROM DATABAYAR@LIHATGATOTKACA A
JOIN    DATAOP@LIHATGATOTKACA K ON A.T_KEC_KD=K.T_KEC_KD AND A.T_KEL_KD=K.T_KEL_KD AND A.D_NOP_BLK=K.D_NOP_BLK AND A.D_NOP_URUT=K.D_NOP_URUT AND A.D_NOP_JNS=K.D_NOP_JNS
JOIN  M_WILAYAHV1 P ON K.KD_CAMAT=KD_KECAMATAN AND K.KD_LURAH=KD_KELURAHAN
JOIN    sppt_new@LIHATGATOTKACA c ON A.T_KEC_KD=c.sppt_kec AND A.T_KEL_KD=c.sppt_kel AND A.D_NOP_BLK=c.sppt_urutblk AND A.D_NOP_URUT=c.sppt_urutop AND A.D_NOP_JNS=c.sppt_tanda
WHERE TO_CHAR(A.D_CREA_DATE,'YYYY')= :TAHUN
) A
LEFT JOIN POTENSIBYR@NRC B ON  A.T_PROP_KD=SPPT_PROP AND A.T_DATI2_KD=SPPT_KOTA AND A.T_KEC_KD=SPPT_KEC AND A.T_KEL_KD=SPPT_KEL AND A.D_NOP_BLK=SPPT_URUTBLK AND A.D_NOP_URUT=SPPT_URUTOP AND A.D_NOP_JNS=SPPT_TANDA
                ";

                var ketetapanMonitoringDb = _contMonitoringDB.Set<KetetapanPbb>()
                    .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();

                int jmlData = ketetapanMonitoringDb.Count;
                int index = 0;
                decimal jml = 0;

                foreach (var itemKetetapan in ketetapanMonitoringDb)
                {
                    var src = _contMonPd.DbMonPbbs.SingleOrDefault(x => x.Nop == itemKetetapan.NOP && x.TahunBuku == itemKetetapan.TAHUN_BUKU && x.TahunPajak == itemKetetapan.TAHUN_PAJAK);
                    if(src != null)
                    {
                        _contMonPd.DbMonPbbs.Remove(src);
                        _contMonPd.SaveChanges();
                    }

                    var newRow = new DbMonPbb();
                    newRow.Nop = itemKetetapan.NOP;
                    newRow.TahunBuku = itemKetetapan.TAHUN_BUKU;
                    newRow.KategoriId = itemKetetapan.KATEGORI_ID;
                    newRow.KategoriNama = itemKetetapan.KATEGORI_NAMA;
                    newRow.AlamatOp = itemKetetapan.ALAMAT_OP;
                    newRow.AlamatOpNo = itemKetetapan.ALAMAT_OP_NO;
                    newRow.AlamatOpRt = itemKetetapan.ALAMAT_OP_RT;
                    newRow.AlamatOpRw = itemKetetapan.ALAMAT_OP_RW;
                    newRow.AlamatKdCamat = itemKetetapan.ALAMAT_KD_CAMAT;
                    newRow.AlamatKdLurah = itemKetetapan.ALAMAT_KD_LURAH;
                    newRow.Uptb = itemKetetapan.UPTB;
                    newRow.AlamatWp = itemKetetapan.ALAMAT_WP;
                    newRow.AlamatWpNo = itemKetetapan.ALAMAT_WP_NO;
                    newRow.AlamatWpKel = itemKetetapan.ALAMAT_WP_KEL;
                    newRow.AlamatWpKota = itemKetetapan.ALAMAT_WP_KOTA;
                    newRow.WpNama = itemKetetapan.WP_NAMA;
                    newRow.WpNpwp = itemKetetapan.WP_NPWP;
                    newRow.TahunPajak = itemKetetapan.TAHUN_PAJAK;
                    newRow.PokokPajak = itemKetetapan.POKOK_PAJAK;
                    newRow.KategoriOp = itemKetetapan.KATEGORI_OP;
                    newRow.Peruntukan = itemKetetapan.PERUNTUKAN;
                    newRow.TglBayar = itemKetetapan.TGL_BAYAR;
                    newRow.InsDate = itemKetetapan.INS_DATE;
                    newRow.InsBy = itemKetetapan.INS_BY;
                    newRow.IsLunas = itemKetetapan.IS_LUNAS;

                    var realisasi = GetRealisasi(itemKetetapan.TAHUN_PAJAK, itemKetetapan.NOP);

                    newRow.JumlahBayarPokok = realisasi.NominalRealisasiPokok;
                    newRow.JumlahBayarSanksi = realisasi.NominalSanksi;

                    _contMonPd.DbMonPbbs.Add(newRow);
                    _contMonPd.SaveChanges();

                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_KETETAPAN_PBB HPP TAHUN {tahunBuku} JML OP {jmlData} {index} : {jml.ToString("n0")}  {persen:F2}%   ");
                    Thread.Sleep(50);

                    index++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing KETETAPAN {ex.Message}");
            }
        }

        private PbbRealisasi GetRealisasi(int tahunPajak, string nop)
        {
            var res = new PbbRealisasi();
            try
            {
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlRealisasi = @"
             SELECT NOP,TAHUN_PAJAK,SUM(NVL(POKOK, 0)) POKOK , SUM(NVL(SANKSI, 0)) SANKSI
FROM (
	SELECT      A.T_PROP_KD ||A.T_DATI2_KD ||A.T_KEC_KD || A.T_KEL_KD || A.D_NOP_BLK || A.D_NOP_URUT  || A.D_NOP_JNS  NOP , 
				TO_NUMBER(A.d_pjk_thn) TAHUN_PAJAK,    
				D_PJK_PBB POKOK,
				D_PJK_JMBYR-D_PJK_PBB SANKSI
    FROM        CATBAYAR@LIHATGATOTKACA A 
    WHERE       TO_NUMBER(A.d_pjk_thn) =:TAHUN AND (A.T_PROP_KD ||A.T_DATI2_KD ||A.T_KEC_KD || A.T_KEL_KD || A.D_NOP_BLK || A.D_NOP_URUT  || A.D_NOP_JNS) = :NOP
    UNION ALL
    SELECT      A.KD_PROPINSI || A.KD_DATI2  ||  A.KD_KECAMATAN  ||  A.KD_KELURAHAN  ||  A.KD_BLOK  ||  A.NO_URUT  ||  A.KD_JNS_OP NOP,
                TO_NUMBER(THN_PAJAK_SPPT) THN_PAJAK_SPPT,JML_SPPT_YG_DIBAYAR-DENDA_SPPT,DENDA_SPPT                                                    
    FROM        PEMBAYARAN_SPPT@LIHATGATOTKACA A
    WHERE       TO_NUMBER(THN_PAJAK_SPPT) = :TAHUN  AND NVL(REV_FLAG,0) !=1 AND (A.KD_PROPINSI || A.KD_DATI2  ||  A.KD_KECAMATAN  ||  A.KD_KELURAHAN  ||  A.KD_BLOK  ||  A.NO_URUT  ||  A.KD_JNS_OP) = :NOP
)
GROUP BY NOP,TAHUN_PAJAK                                        
                ";

                var realisasiMonitoringDb = _contMonitoringDB.Set<RealisasiPbb>()
                    .FromSqlRaw(sqlRealisasi, new[] {
                                new OracleParameter("NOP", nop),
                                new OracleParameter("TAHUN", tahunPajak)
                    }).ToList();

                res.NominalRealisasiPokok = realisasiMonitoringDb.Sum(x => x.POKOK);
                res.NominalSanksi = realisasiMonitoringDb.Sum(x => x.SANKSI);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing REALISASI {ex.Message}");
            }

            return res;
        }

        private void GetRealisasi(int tahunBuku)
        {
            try
            {
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlRealisasi = @"
             SELECT NOP,TAHUN_PAJAK,SUM(NVL(POKOK, 0)) POKOK , SUM(NVL(SANKSI, 0)) SANKSI
FROM (
SELECT      A.T_PROP_KD ||A.T_DATI2_KD ||A.T_KEC_KD || A.T_KEL_KD || A.D_NOP_BLK || A.D_NOP_URUT  || A.D_NOP_JNS  NOP , A.d_pjk_thn TAHUN_PAJAK,                                        
                                        D_PJK_PBB POKOK,D_PJK_JMBYR-D_PJK_PBB SANKSI
                                        FROM        CATBAYAR@LIHATGATOTKACA A 
                                        WHERE       to_char(D_PJK_TGBYR,'YYYY') =:TAHUN
                                        UNION ALL
                                        SELECT      A.KD_PROPINSI || A.KD_DATI2  ||  A.KD_KECAMATAN  ||  A.KD_KELURAHAN  ||  A.KD_BLOK  ||  A.NO_URUT  ||  A.KD_JNS_OP NOP,
                                                    THN_PAJAK_SPPT,JML_SPPT_YG_DIBAYAR-DENDA_SPPT,DENDA_SPPT                                                    
                                        FROM        PEMBAYARAN_SPPT@LIHATGATOTKACA A
                                        WHERE       to_char(TGL_PEMBAYARAN_SPPT,'YYYY') = :TAHUN  AND NVL(REV_FLAG,0) !=1
)
GROUP BY NOP,TAHUN_PAJAK                                        
                ";

                var realisasiMonitoringDb = _contMonitoringDB.Set<RealisasiPbb>()
                    .FromSqlRaw(sqlRealisasi, new[] {
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();

                int jmlData = realisasiMonitoringDb.Count;
                int index = 0;                

                foreach (var itemRealisasi in realisasiMonitoringDb)
                {
                    var realisasi = _contMonPd.DbMonPbbs.SingleOrDefault(x => x.Nop == itemRealisasi.NOP && x.TahunBuku == tahunBuku && x.TahunPajak == itemRealisasi.TAHUN_PAJAK);
                    if (realisasi != null) {
                        realisasi.JumlahBayarPokok = itemRealisasi.POKOK;
                        realisasi.JumlahBayarSanksi = itemRealisasi.SANKSI;
                        _contMonPd.SaveChanges();
                    }                    
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_REALISASI_PBB  TAHUN {tahunBuku} JML OP {jmlData} : {index.ToString("n0")}  {persen:F2}%   ");
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing REALISASI {ex.Message}");
            }
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPPBB.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPPBB.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }

        public class PbbRealisasi
        {
            public decimal NominalRealisasiPokok { get; set; } = 0;
            public decimal NominalSanksi { get; set; } = 0;
        }
    }
}
