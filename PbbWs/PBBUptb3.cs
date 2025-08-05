using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
    public class PBBUptb3 : BackgroundService
    {
        private bool isFirst = true;
        private readonly ILogger<PBBUptb3> _logger;
        private static int KDPajak = 9;
        private static int Uptb = 3;

        public PBBUptb3(ILogger<PBBUptb3> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PBBUptb3 running at: {time}", DateTimeOffset.Now);
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
                     DoWorkNewMeta(stoppingToken);
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

        private void DoWorkNewMeta(CancellationToken stoppingToken)
        {
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == KDPajak);
            tahunAmbil = tglServer.Year - Convert.ToInt32(thnSetting?.YearBefore ?? DateTime.Now.Year);

            if (IsGetDBOp())
            {
                //await Task.WhenAll(GetOPProcessAsync());
                GetOPProcess();
            }
            //Task.WaitAll();

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                //await Task.WhenAll(GetKetetapanAsync(i));
                GetKetetapan(i);
            }
            //for (var i = tahunAmbil; i <= tglServer.Year; i++)
            //{
            //    GetRealisasi(i);
            //}

            MailHelper.SendMail(
            false,
            "DONE PBB WS",
            $@"PBB WS FINISHED",
            null
            );
        }

        public async Task GetOPProcessAsync()
        {
            try
            {
                Console.WriteLine($"Get OP UPTB :{Uptb}");
                using var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                using var _contMonPd = DBClass.GetContext();

                string sql = $@"
                SELECT A.T_PROP_KD || A.T_DATI2_KD || A.T_KEC_KD || A.T_KEL_KD || A.D_NOP_BLK || A.D_NOP_URUT || A.D_NOP_JNS NOP,
                    57 KATEGORI_ID, 'PBB' KATEGORI_NAMA,
                    NVL(D_OP_JLN,'-') ALAMAT_OP,
                    NVL(D_OP_JLNO ,'-') ALAMAT_OP_NO,
                    NVL(D_OP_RT ,'-')   ALAMAT_OP_RT,
                    NVL(D_OP_RW ,'-')   ALAMAT_OP_RW,
                    NVL(KD_CAMAT ,'-')  ALAMAT_KD_CAMAT,
                    NVL(KD_LURAH ,'-')  ALAMAT_KD_LURAH,
                    TO_NUMBER(NVL(UPTD,0)) UPTB,
                    TO_NUMBER(NVL(D_TNH_LUAS,0)) LUAS_TANAH,
                    NVL(D_WP_JLN ,'-')  ALAMAT_WP,
                    NVL(D_WP_JLNO ,'-') ALAMAT_WP_NO,
                    NVL(D_WP_KEL ,'-')  ALAMAT_WP_KEL,
                    NVL(D_WP_KOTA ,'-') ALAMAT_WP_KOTA,
                    NVL(D_WP_KTP ,'-')  WP_KTP,
                    NVL(D_WP_NAMA ,'-') WP_NAMA,
                    NVL(D_WP_NPWP ,'-') WP_NPWP,
                    NVL(D_WP_RT ,'-')   WP_RT,
                    NVL(D_WP_RW ,'-')   WP_RW,
                    TO_NUMBER(NVL(STATUSAKTIF,0)) STATUS,
                    SYSDATE INS_DATE,
                    'JOB' INS_BY
                FROM DATAOP@LIHATGATOTKACA A
                JOIN M_WILAYAHV1 B ON KD_CAMAT=KD_KECAMATAN AND KD_LURAH=KD_KELURAHAN
                WHERE TO_NUMBER(NVL(UPTD,0)) = {Uptb}";

                var result = _contMonitoringDB.Set<OPPbb>().FromSqlRaw(sql).AsNoTracking().ToList();
                var jml = result.Count;
                var index = 0;
                if (jml == 0)
                {
                    Console.WriteLine($"[UPTB {Uptb}] No data found.");
                    return;
                }

                var nopList = result.Select(r => r.NOP).ToList();
                //var existingDb = _contMonPd.DbOpPbbs
                //    .Where(x => nopList.Contains(x.Nop))
                //    .AsNoTracking()
                //    .ToList();

                //var existingMap = existingDb.ToDictionary(x => x.Nop, x => x);
                var newList = new List<MonPDLib.EF.DbOpPbb>();
                var updateList = new List<MonPDLib.EF.DbOpPbb>();

                foreach (var item in result)
                {
                    var row = _contMonPd.DbOpPbbs.Find(item.NOP);
                    if (row != null)
                    {
                        row.Status = item.STATUS;
                        row.LuasTanah = item.LUAS_TANAH;
                        row.AlamatKdLurah = item.ALAMAT_KD_LURAH;
                        row.AlamatKdCamat = item.ALAMAT_KD_CAMAT;
                        updateList.Add(row);
                    }
                    else
                    {
                        newList.Add(new MonPDLib.EF.DbOpPbb
                        {
                            Nop = item.NOP,
                            KategoriId = item.KATEGORI_ID,
                            KategoriNama = item.KATEGORI_NAMA,
                            AlamatOp = item.ALAMAT_OP,
                            AlamatOpNo = item.ALAMAT_OP_NO,
                            AlamatOpRt = item.ALAMAT_OP_RT,
                            AlamatOpRw = item.ALAMAT_OP_RW,
                            AlamatKdCamat = item.ALAMAT_KD_CAMAT,
                            AlamatKdLurah = item.ALAMAT_KD_LURAH,
                            Uptb = item.UPTB,
                            LuasTanah = item.LUAS_TANAH,
                            AlamatWp = item.ALAMAT_WP,
                            AlamatWpNo = item.ALAMAT_WP_NO,
                            AlamatWpKel = item.ALAMAT_WP_KEL,
                            AlamatWpKota = item.ALAMAT_WP_KOTA,
                            WpKtp = item.WP_KTP,
                            WpNama = item.WP_NAMA,
                            WpNpwp = item.WP_NPWP,
                            WpRt = item.WP_RT,
                            WpRw = item.WP_RW,
                            Status = item.STATUS,
                            InsDate = item.INS_DATE,
                            InsBy = item.INS_BY
                        });
                    }
                    index++;
                    double persen = ((double)index / jml) * 100;
                    Console.Write($"\r[UPTB {Uptb}] total data OP {jml.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}       [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbOpPbbs.AddRange(newList);
                    await _contMonPd.SaveChangesAsync();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbOpPbbs.UpdateRange(updateList);
                    await _contMonPd.SaveChangesAsync();
                }
                Console.Write($"Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UPTB {Uptb}] ERROR: {ex.Message}");
            }
        }

        public void GetOPProcess()
        {
            try
            {
                using var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                using var _contMonPd = DBClass.GetContext();

                string sql = $@"
                SELECT A.T_PROP_KD || A.T_DATI2_KD || A.T_KEC_KD || A.T_KEL_KD || A.D_NOP_BLK || A.D_NOP_URUT || A.D_NOP_JNS NOP,
                    57 KATEGORI_ID, 'PBB' KATEGORI_NAMA,
                    NVL(D_OP_JLN,'-') ALAMAT_OP,
                    NVL(D_OP_JLNO ,'-') ALAMAT_OP_NO,
                    NVL(D_OP_RT ,'-')   ALAMAT_OP_RT,
                    NVL(D_OP_RW ,'-')   ALAMAT_OP_RW,
                    NVL(KD_CAMAT ,'-')  ALAMAT_KD_CAMAT,
                    NVL(KD_LURAH ,'-')  ALAMAT_KD_LURAH,
                    TO_NUMBER(NVL(UPTD,0)) UPTB,
                    TO_NUMBER(NVL(D_TNH_LUAS,0)) LUAS_TANAH,
                    NVL(D_WP_JLN ,'-')  ALAMAT_WP,
                    NVL(D_WP_JLNO ,'-') ALAMAT_WP_NO,
                    NVL(D_WP_KEL ,'-')  ALAMAT_WP_KEL,
                    NVL(D_WP_KOTA ,'-') ALAMAT_WP_KOTA,
                    NVL(D_WP_KTP ,'-')  WP_KTP,
                    NVL(D_WP_NAMA ,'-') WP_NAMA,
                    NVL(D_WP_NPWP ,'-') WP_NPWP,
                    NVL(D_WP_RT ,'-')   WP_RT,
                    NVL(D_WP_RW ,'-')   WP_RW,
                    TO_NUMBER(NVL(STATUSAKTIF,0)) STATUS,
                    SYSDATE INS_DATE,
                    'JOB' INS_BY
                FROM DATAOP@LIHATGATOTKACA A
                JOIN M_WILAYAHV1 B ON KD_CAMAT=KD_KECAMATAN AND KD_LURAH=KD_KELURAHAN
                WHERE TO_NUMBER(NVL(UPTD,0)) = {Uptb}";

                var result = _contMonitoringDB.Set<OPPbb>().FromSqlRaw(sql).AsNoTracking().ToList();
                var jml = result.Count;
                var index = 0;
                if (jml == 0)
                {
                    Console.WriteLine($"[UPTB {Uptb}] No data found.");
                    return;
                }

                var nopList = result.Select(r => r.NOP).ToList();
                //var existingDb = _contMonPd.DbOpPbbs
                //    .Where(x => nopList.Contains(x.Nop))
                //    .AsNoTracking()
                //    .ToList();

                //var existingMap = existingDb.ToDictionary(x => x.Nop, x => x);
                var newList = new List<MonPDLib.EF.DbOpPbb>();
                var updateList = new List<MonPDLib.EF.DbOpPbb>();

                foreach (var item in result)
                {
                    var row = _contMonPd.DbOpPbbs.Find(item.NOP);
                    if (row != null)
                    {
                        row.Status = item.STATUS;
                        row.LuasTanah = item.LUAS_TANAH;
                        row.AlamatKdLurah = item.ALAMAT_KD_LURAH;
                        row.AlamatKdCamat = item.ALAMAT_KD_CAMAT;
                        updateList.Add(row);
                    }
                    else
                    {
                        newList.Add(new MonPDLib.EF.DbOpPbb
                        {
                            Nop = item.NOP,
                            KategoriId = item.KATEGORI_ID,
                            KategoriNama = item.KATEGORI_NAMA,
                            AlamatOp = item.ALAMAT_OP,
                            AlamatOpNo = item.ALAMAT_OP_NO,
                            AlamatOpRt = item.ALAMAT_OP_RT,
                            AlamatOpRw = item.ALAMAT_OP_RW,
                            AlamatKdCamat = item.ALAMAT_KD_CAMAT,
                            AlamatKdLurah = item.ALAMAT_KD_LURAH,
                            Uptb = item.UPTB,
                            LuasTanah = item.LUAS_TANAH,
                            AlamatWp = item.ALAMAT_WP,
                            AlamatWpNo = item.ALAMAT_WP_NO,
                            AlamatWpKel = item.ALAMAT_WP_KEL,
                            AlamatWpKota = item.ALAMAT_WP_KOTA,
                            WpKtp = item.WP_KTP,
                            WpNama = item.WP_NAMA,
                            WpNpwp = item.WP_NPWP,
                            WpRt = item.WP_RT,
                            WpRw = item.WP_RW,
                            Status = item.STATUS,
                            InsDate = item.INS_DATE,
                            InsBy = item.INS_BY
                        });
                    }
                    index++;
                    double persen = ((double)index / jml) * 100;
                    Console.Write($"\r[UPTB {Uptb}] total data OP {jml.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}       [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbOpPbbs.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbOpPbbs.UpdateRange(updateList);
                    _contMonPd.SaveChanges();
                }
                Console.Write($"Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UPTB {Uptb}] ERROR: {ex.Message}");
            }
        }

        private async Task GetKetetapanAsync(int tahunBuku)
        {
            try
            {
                Console.WriteLine($"Get Ketetapan Tahun {tahunBuku} UPTB {Uptb}");
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlKetetapan = @"
                SELECT  A.NOP,TO_NUMBER(TAHUN_BUKU) TAHUN_BUKU, TO_NUMBER(KATEGORI_ID) KATEGORI_ID,
                            NVL(KATEGORI_NAMA,'-') KATEGORI_NAMA, NVL(ALAMAT_OP,'-') ALAMAT_OP,NVL(ALAMAT_OP_NO,'-') ALAMAT_OP_NO,
                            NVL(ALAMAT_OP_RT,'-') ALAMAT_OP_RT ,NVL(ALAMAT_OP_RW,'-') ALAMAT_OP_RW ,NVL(ALAMAT_KD_CAMAT,'-') ALAMAT_KD_CAMAT  ,
                            NVL(ALAMAT_KD_LURAH,'-') ALAMAT_KD_LURAH ,TO_NUMBER(nvl(UPTB,0)) UPTB,NVL(ALAMAT_WP,'-') ALAMAT_WP,
                            NVL(ALAMAT_WP_NO,'-') ALAMAT_WP_NO, NVL(ALAMAT_WP_KEL,'-') ALAMAT_WP_KEL,NVL(ALAMAT_WP_KOTA,'-') ALAMAT_WP_KOTA,
                            NVL(WP_NAMA,'-') WP_NAMA, NVL(WP_NPWP,'-') WP_NPWP,
                            TO_NUMBER(nvl( TAHUN_PAJAK,0)) TAHUN_PAJAK,
                            TO_NUMBER(nvl(POKOK_PAJAK,0)) POKOK_PAJAK,
                            NVL(KATEGORI_OP,'-') KATEGORI_OP,NVL(B.KATEGORI,'BELUM DIKETAHUI') PERUNTUKAN,                                       
                            TO_NUMBER(nvl( IS_LUNAS,1)) IS_LUNAS ,TGL_BAYAR ,NVL(D_PJK_PBB,0)  JUMLAH_BAYAR_POKOK,
                            NVL(D_PJK_JMBYR,0)- NVL(D_PJK_PBB,0) JUMLAH_BAYAR_SANKSI,NVL(JML_SPPT_YG_DIBAYAR,0)- NVL(DENDA_SPPT,0) POKOK_BANK,NVL(DENDA_SPPT,0) DENDA_BANK,
                            INS_DATE ,INS_BY    
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
WHERE TO_CHAR(A.D_CREA_DATE,'YYYY')= :TAHUN AND P.UPTD=:UPTB
) A
LEFT JOIN POTENSIBYR@NRC B ON  A.T_PROP_KD=SPPT_PROP AND A.T_DATI2_KD=SPPT_KOTA AND A.T_KEC_KD=SPPT_KEC AND A.T_KEL_KD=SPPT_KEL AND 
                                                     A.D_NOP_BLK=SPPT_URUTBLK AND A.D_NOP_URUT=SPPT_URUTOP AND A.D_NOP_JNS=SPPT_TANDA
LEFT JOIN  CATBAYAR@LIHATGATOTKACA C ON  C.T_PROP_KD=A.T_PROP_KD AND C.T_DATI2_KD=A.T_DATI2_KD AND C.T_KEC_KD=A.T_KEC_KD AND 
                                                                        C.T_KEL_KD=A.T_KEL_KD AND C.D_NOP_BLK=A.D_NOP_BLK AND C.D_NOP_URUT=A.D_NOP_URUT AND C.D_NOP_JNS=A.D_NOP_JNS AND A.TAHUN_PAJAK=C.D_PJK_THN
LEFT JOIN PEMBAYARAN_SPPT@LIHATGATOTKACA D ON D.KD_PROPINSI=A.T_PROP_KD AND  D.KD_DATI2=A.T_DATI2_KD  AND  D.KD_KECAMATAN=A.T_KEC_KD  AND  D.KD_KELURAHAN = A.T_KEL_KD AND
                                                                                   D.KD_BLOK = A.D_NOP_BLK AND   D.NO_URUT= A.D_NOP_URUT  AND  D.KD_JNS_OP= A.D_NOP_JNS AND D.THN_PAJAK_SPPT=A.TAHUN_PAJAK
                ";

                var ketetapanMonitoringDb = _contMonitoringDB.Set<KetetapanPbbAsync>()
                    .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("TAHUN", tahunBuku.ToString()),
                                new OracleParameter("UPTB", Uptb)
                    }).AsNoTracking().ToList();

                var grouped = ketetapanMonitoringDb
    .GroupBy(x => new
    {
        x.NOP,
        x.TAHUN_BUKU,
        x.KATEGORI_ID,
        x.KATEGORI_NAMA,
        x.ALAMAT_OP,
        x.ALAMAT_OP_NO,
        x.ALAMAT_OP_RT,
        x.ALAMAT_OP_RW,
        x.ALAMAT_KD_CAMAT,
        x.ALAMAT_KD_LURAH,
        x.UPTB,
        x.ALAMAT_WP,
        x.ALAMAT_WP_NO,
        x.ALAMAT_WP_KEL,
        x.ALAMAT_WP_KOTA,
        x.WP_NAMA,
        x.WP_NPWP,
        x.TAHUN_PAJAK,
        x.POKOK_PAJAK,
        x.KATEGORI_OP,
        x.PERUNTUKAN,
        x.IS_LUNAS,
        x.TGL_BAYAR,
        x.INS_DATE,
        x.INS_BY
    })
    .Select(g => new
    {
        NOP = g.Key.NOP,
        TAHUN_BUKU = g.Key.TAHUN_BUKU,
        KATEGORI_ID = g.Key.KATEGORI_ID,
        KATEGORI_NAMA = g.Key.KATEGORI_NAMA,
        ALAMAT_OP = g.Key.ALAMAT_OP,
        ALAMAT_OP_NO = g.Key.ALAMAT_OP_NO,
        ALAMAT_OP_RT = g.Key.ALAMAT_OP_RT,
        ALAMAT_OP_RW = g.Key.ALAMAT_OP_RW,
        ALAMAT_KD_CAMAT = g.Key.ALAMAT_KD_CAMAT,
        ALAMAT_KD_LURAH = g.Key.ALAMAT_KD_LURAH,
        UPTB = g.Key.UPTB,
        ALAMAT_WP = g.Key.ALAMAT_WP,
        ALAMAT_WP_NO = g.Key.ALAMAT_WP_NO,
        ALAMAT_WP_KEL = g.Key.ALAMAT_WP_KEL,
        ALAMAT_WP_KOTA = g.Key.ALAMAT_WP_KOTA,
        WP_NAMA = g.Key.WP_NAMA,
        WP_NPWP = g.Key.WP_NPWP,
        TAHUN_PAJAK = g.Key.TAHUN_PAJAK,
        POKOK_PAJAK = g.Key.POKOK_PAJAK,
        KATEGORI_OP = g.Key.KATEGORI_OP,
        PERUNTUKAN = g.Key.PERUNTUKAN,
        IS_LUNAS = g.Key.IS_LUNAS,
        TGL_BAYAR = g.Key.TGL_BAYAR,
        INS_DATE = g.Key.INS_DATE,
        INS_BY = g.Key.INS_BY,
        JUMLAH_BAYAR_POKOK = g.Sum(x => x.JUMLAH_BAYAR_POKOK + x.POKOK_BANK),
        JUMLAH_BAYAR_SANKSI = g.Sum(x => x.JUMLAH_BAYAR_SANKSI + x.DENDA_BANK)
    })
    .ToList();

                int index = 0;
                double jmlData = grouped.Count;
                var newList = new List<MonPDLib.EF.DbMonPbb>();
                var updateList = new List<MonPDLib.EF.DbMonPbb>();
                foreach (var itemKetetapan in grouped)
                {
                    var row = _contMonPd.DbMonPbbs.Find(itemKetetapan.NOP, itemKetetapan.TAHUN_BUKU, itemKetetapan.TAHUN_PAJAK);
                    if (row != null)
                    {
                        row.PokokPajak = itemKetetapan.POKOK_PAJAK;
                        row.IsLunas = itemKetetapan.IS_LUNAS;
                        row.TglBayar = itemKetetapan.TGL_BAYAR;
                        row.JumlahBayarPokok = itemKetetapan.JUMLAH_BAYAR_POKOK;
                        row.JumlahBayarSanksi = itemKetetapan.JUMLAH_BAYAR_SANKSI;
                        updateList.Add(row);
                    }
                    else
                    {
                        newList.Add(new MonPDLib.EF.DbMonPbb
                        {
                            Nop = itemKetetapan.NOP,
                            TahunBuku = itemKetetapan.TAHUN_BUKU,
                            KategoriId = itemKetetapan.KATEGORI_ID,
                            KategoriNama = itemKetetapan.KATEGORI_NAMA,
                            AlamatOp = itemKetetapan.ALAMAT_OP,
                            AlamatOpNo = itemKetetapan.ALAMAT_OP_NO,
                            AlamatOpRt = itemKetetapan.ALAMAT_OP_RT,
                            AlamatOpRw = itemKetetapan.ALAMAT_OP_RW,
                            AlamatKdCamat = itemKetetapan.ALAMAT_KD_CAMAT,
                            AlamatKdLurah = itemKetetapan.ALAMAT_KD_LURAH,
                            Uptb = itemKetetapan.UPTB,
                            AlamatWp = itemKetetapan.ALAMAT_WP,
                            AlamatWpNo = itemKetetapan.ALAMAT_WP_NO,
                            AlamatWpKel = itemKetetapan.ALAMAT_WP_KEL,
                            AlamatWpKota = itemKetetapan.ALAMAT_WP_KOTA,
                            WpNama = itemKetetapan.WP_NAMA,
                            WpNpwp = itemKetetapan.WP_NPWP,
                            TahunPajak = itemKetetapan.TAHUN_PAJAK,
                            PokokPajak = itemKetetapan.POKOK_PAJAK,
                            KategoriOp = itemKetetapan.KATEGORI_OP,
                            Peruntukan = itemKetetapan.PERUNTUKAN,
                            IsLunas = itemKetetapan.IS_LUNAS,
                            TglBayar = itemKetetapan.TGL_BAYAR,
                            JumlahBayarPokok = itemKetetapan.JUMLAH_BAYAR_POKOK,
                            JumlahBayarSanksi = itemKetetapan.JUMLAH_BAYAR_SANKSI,
                            InsDate = itemKetetapan.INS_DATE,
                            InsBy = itemKetetapan.INS_BY
                        });
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[UPTB {Uptb}] total data Ketetapan {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}       [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbMonPbbs.AddRange(newList);
                    await _contMonPd.SaveChangesAsync();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbMonPbbs.UpdateRange(updateList);
                    await _contMonPd.SaveChangesAsync();
                }
                Console.Write($"Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing KETETAPAN {ex.Message}");
            }
        }

        private void GetKetetapan(int tahunBuku)
        {
            try
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");

                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlKetetapan = @"
                SELECT  A.NOP,TO_NUMBER(TAHUN_BUKU) TAHUN_BUKU, TO_NUMBER(KATEGORI_ID) KATEGORI_ID,
                            NVL(KATEGORI_NAMA,'-') KATEGORI_NAMA, NVL(ALAMAT_OP,'-') ALAMAT_OP,NVL(ALAMAT_OP_NO,'-') ALAMAT_OP_NO,
                            NVL(ALAMAT_OP_RT,'-') ALAMAT_OP_RT ,NVL(ALAMAT_OP_RW,'-') ALAMAT_OP_RW ,NVL(ALAMAT_KD_CAMAT,'-') ALAMAT_KD_CAMAT  ,
                            NVL(ALAMAT_KD_LURAH,'-') ALAMAT_KD_LURAH ,TO_NUMBER(nvl(UPTB,0)) UPTB,NVL(ALAMAT_WP,'-') ALAMAT_WP,
                            NVL(ALAMAT_WP_NO,'-') ALAMAT_WP_NO, NVL(ALAMAT_WP_KEL,'-') ALAMAT_WP_KEL,NVL(ALAMAT_WP_KOTA,'-') ALAMAT_WP_KOTA,
                            NVL(WP_NAMA,'-') WP_NAMA, NVL(WP_NPWP,'-') WP_NPWP,
                            TO_NUMBER(nvl( TAHUN_PAJAK,0)) TAHUN_PAJAK,
                            TO_NUMBER(nvl(POKOK_PAJAK,0)) POKOK_PAJAK,
                            NVL(KATEGORI_OP,'-') KATEGORI_OP,NVL(B.KATEGORI,'BELUM DIKETAHUI') PERUNTUKAN,                                       
                            TO_NUMBER(nvl( IS_LUNAS,1)) IS_LUNAS ,TGL_BAYAR ,NVL(D_PJK_PBB,0)  JUMLAH_BAYAR_POKOK,
                            NVL(D_PJK_JMBYR,0)- NVL(D_PJK_PBB,0) JUMLAH_BAYAR_SANKSI,NVL(JML_SPPT_YG_DIBAYAR,0)- NVL(DENDA_SPPT,0) POKOK_BANK,NVL(DENDA_SPPT,0) DENDA_BANK,
                            INS_DATE ,INS_BY    
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
WHERE TO_CHAR(A.D_CREA_DATE,'YYYY')= :TAHUN AND P.UPTD=:UPTB
) A
LEFT JOIN POTENSIBYR@NRC B ON  A.T_PROP_KD=SPPT_PROP AND A.T_DATI2_KD=SPPT_KOTA AND A.T_KEC_KD=SPPT_KEC AND A.T_KEL_KD=SPPT_KEL AND 
                                                     A.D_NOP_BLK=SPPT_URUTBLK AND A.D_NOP_URUT=SPPT_URUTOP AND A.D_NOP_JNS=SPPT_TANDA
LEFT JOIN  CATBAYAR@LIHATGATOTKACA C ON  C.T_PROP_KD=A.T_PROP_KD AND C.T_DATI2_KD=A.T_DATI2_KD AND C.T_KEC_KD=A.T_KEC_KD AND 
                                                                        C.T_KEL_KD=A.T_KEL_KD AND C.D_NOP_BLK=A.D_NOP_BLK AND C.D_NOP_URUT=A.D_NOP_URUT AND C.D_NOP_JNS=A.D_NOP_JNS AND A.TAHUN_PAJAK=C.D_PJK_THN
LEFT JOIN PEMBAYARAN_SPPT@LIHATGATOTKACA D ON D.KD_PROPINSI=A.T_PROP_KD AND  D.KD_DATI2=A.T_DATI2_KD  AND  D.KD_KECAMATAN=A.T_KEC_KD  AND  D.KD_KELURAHAN = A.T_KEL_KD AND
                                                                                   D.KD_BLOK = A.D_NOP_BLK AND   D.NO_URUT= A.D_NOP_URUT  AND  D.KD_JNS_OP= A.D_NOP_JNS AND D.THN_PAJAK_SPPT=A.TAHUN_PAJAK
                ";

                var ketetapanMonitoringDb = _contMonitoringDB.Set<KetetapanPbbAsync>()
                    .FromSqlRaw(sqlKetetapan, new[] {
                                new OracleParameter("TAHUN", tahunBuku.ToString()),
                                new OracleParameter("UPTB", Uptb)
                    }).AsNoTracking().ToList();

                var grouped = ketetapanMonitoringDb
    .GroupBy(x => new
    {
        x.NOP,
        x.TAHUN_BUKU,
        x.KATEGORI_ID,
        x.KATEGORI_NAMA,
        x.ALAMAT_OP,
        x.ALAMAT_OP_NO,
        x.ALAMAT_OP_RT,
        x.ALAMAT_OP_RW,
        x.ALAMAT_KD_CAMAT,
        x.ALAMAT_KD_LURAH,
        x.UPTB,
        x.ALAMAT_WP,
        x.ALAMAT_WP_NO,
        x.ALAMAT_WP_KEL,
        x.ALAMAT_WP_KOTA,
        x.WP_NAMA,
        x.WP_NPWP,
        x.TAHUN_PAJAK,
        x.POKOK_PAJAK,
        x.KATEGORI_OP,
        x.PERUNTUKAN,
        x.IS_LUNAS,
        x.TGL_BAYAR,
        x.INS_DATE,
        x.INS_BY
    })
    .Select(g => new
    {
        NOP = g.Key.NOP,
        TAHUN_BUKU = g.Key.TAHUN_BUKU,
        KATEGORI_ID = g.Key.KATEGORI_ID,
        KATEGORI_NAMA = g.Key.KATEGORI_NAMA,
        ALAMAT_OP = g.Key.ALAMAT_OP,
        ALAMAT_OP_NO = g.Key.ALAMAT_OP_NO,
        ALAMAT_OP_RT = g.Key.ALAMAT_OP_RT,
        ALAMAT_OP_RW = g.Key.ALAMAT_OP_RW,
        ALAMAT_KD_CAMAT = g.Key.ALAMAT_KD_CAMAT,
        ALAMAT_KD_LURAH = g.Key.ALAMAT_KD_LURAH,
        UPTB = g.Key.UPTB,
        ALAMAT_WP = g.Key.ALAMAT_WP,
        ALAMAT_WP_NO = g.Key.ALAMAT_WP_NO,
        ALAMAT_WP_KEL = g.Key.ALAMAT_WP_KEL,
        ALAMAT_WP_KOTA = g.Key.ALAMAT_WP_KOTA,
        WP_NAMA = g.Key.WP_NAMA,
        WP_NPWP = g.Key.WP_NPWP,
        TAHUN_PAJAK = g.Key.TAHUN_PAJAK,
        POKOK_PAJAK = g.Key.POKOK_PAJAK,
        KATEGORI_OP = g.Key.KATEGORI_OP,
        PERUNTUKAN = g.Key.PERUNTUKAN,
        IS_LUNAS = g.Key.IS_LUNAS,
        TGL_BAYAR = g.Key.TGL_BAYAR,
        INS_DATE = g.Key.INS_DATE,
        INS_BY = g.Key.INS_BY,
        JUMLAH_BAYAR_POKOK = g.Sum(x => x.JUMLAH_BAYAR_POKOK + x.POKOK_BANK),
        JUMLAH_BAYAR_SANKSI = g.Sum(x => x.JUMLAH_BAYAR_SANKSI + x.DENDA_BANK)
    })
    .ToList();

                int index = 0;
                double jmlData = grouped.Count;
                var newList = new List<MonPDLib.EF.DbMonPbb>();
                var updateList = new List<MonPDLib.EF.DbMonPbb>();
                foreach (var itemKetetapan in grouped)
                {
                    var row = _contMonPd.DbMonPbbs.Find(itemKetetapan.NOP, itemKetetapan.TAHUN_BUKU, itemKetetapan.TAHUN_PAJAK);
                    if (row != null)
                    {
                        row.PokokPajak = itemKetetapan.POKOK_PAJAK;
                        row.IsLunas = itemKetetapan.IS_LUNAS;
                        row.TglBayar = itemKetetapan.TGL_BAYAR;
                        row.JumlahBayarPokok = itemKetetapan.JUMLAH_BAYAR_POKOK;
                        row.JumlahBayarSanksi = itemKetetapan.JUMLAH_BAYAR_SANKSI;
                        updateList.Add(row);
                    }
                    else
                    {
                        newList.Add(new MonPDLib.EF.DbMonPbb
                        {
                            Nop = itemKetetapan.NOP,
                            TahunBuku = itemKetetapan.TAHUN_BUKU,
                            KategoriId = itemKetetapan.KATEGORI_ID,
                            KategoriNama = itemKetetapan.KATEGORI_NAMA,
                            AlamatOp = itemKetetapan.ALAMAT_OP,
                            AlamatOpNo = itemKetetapan.ALAMAT_OP_NO,
                            AlamatOpRt = itemKetetapan.ALAMAT_OP_RT,
                            AlamatOpRw = itemKetetapan.ALAMAT_OP_RW,
                            AlamatKdCamat = itemKetetapan.ALAMAT_KD_CAMAT,
                            AlamatKdLurah = itemKetetapan.ALAMAT_KD_LURAH,
                            Uptb = itemKetetapan.UPTB,
                            AlamatWp = itemKetetapan.ALAMAT_WP,
                            AlamatWpNo = itemKetetapan.ALAMAT_WP_NO,
                            AlamatWpKel = itemKetetapan.ALAMAT_WP_KEL,
                            AlamatWpKota = itemKetetapan.ALAMAT_WP_KOTA,
                            WpNama = itemKetetapan.WP_NAMA,
                            WpNpwp = itemKetetapan.WP_NPWP,
                            TahunPajak = itemKetetapan.TAHUN_PAJAK,
                            PokokPajak = itemKetetapan.POKOK_PAJAK,
                            KategoriOp = itemKetetapan.KATEGORI_OP,
                            Peruntukan = itemKetetapan.PERUNTUKAN,
                            IsLunas = itemKetetapan.IS_LUNAS,
                            TglBayar = itemKetetapan.TGL_BAYAR,
                            JumlahBayarPokok = itemKetetapan.JUMLAH_BAYAR_POKOK,
                            JumlahBayarSanksi = itemKetetapan.JUMLAH_BAYAR_SANKSI,
                            InsDate = itemKetetapan.INS_DATE,
                            InsBy = itemKetetapan.INS_BY
                        });
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r[UPTB {Uptb}] tahun buku {tahunBuku} total data Ketetapan {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}       [({persen:F2}%)]");
                }

                Console.WriteLine("Updating DB!");
                if (newList.Any())
                {
                    _contMonPd.DbMonPbbs.AddRange(newList);
                    _contMonPd.SaveChanges();
                }


                if (updateList.Any())
                {
                    _contMonPd.DbMonPbbs.UpdateRange(updateList);
                    _contMonPd.SaveChanges();
                }
                Console.Write($"Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing KETETAPAN {ex.Message}");
            }
        }
        private void GetOPProcess1()
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

                var result = _contMonitoringDB.Set<OPPbb>().FromSqlRaw(sql).AsNoTracking().ToList();
                var _contMonPd = DBClass.GetContext();
                int jmlData = result.Count;

                var nopList = result.Select(r => r.NOP).ToList();
                var existingDb = _contMonPd.DbOpPbbs
                    .Where(x => nopList.Contains(x.Nop))
                    .AsNoTracking()
                    .ToList();

                var existingMap = existingDb.ToDictionary(x => x.Nop, x => x);

                var newList = new List<MonPDLib.EF.DbOpPbb>();
                var updateList = new List<MonPDLib.EF.DbOpPbb>();

                int index = 0;

                foreach (var item in result)
                {
                    try
                    {
                        if (existingMap.TryGetValue(item.NOP, out var existing))
                        {
                            existing.Status = item.STATUS;
                            existing.LuasTanah = item.LUAS_TANAH;
                            existing.AlamatKdLurah = item.ALAMAT_KD_LURAH;
                            existing.AlamatKdCamat = item.ALAMAT_KD_CAMAT;
                            updateList.Add(existing);
                        }
                        else
                        {
                            var newRow = new MonPDLib.EF.DbOpPbb
                            {
                                Nop = item.NOP,
                                KategoriId = item.KATEGORI_ID,
                                KategoriNama = item.KATEGORI_NAMA,
                                AlamatOp = item.ALAMAT_OP,
                                AlamatOpNo = item.ALAMAT_OP_NO,
                                AlamatOpRt = item.ALAMAT_OP_RT,
                                AlamatOpRw = item.ALAMAT_OP_RW,
                                AlamatKdCamat = item.ALAMAT_KD_CAMAT,
                                AlamatKdLurah = item.ALAMAT_KD_LURAH,
                                Uptb = item.UPTB,
                                LuasTanah = item.LUAS_TANAH,
                                AlamatWp = item.ALAMAT_WP,
                                AlamatWpNo = item.ALAMAT_WP_NO,
                                AlamatWpKel = item.ALAMAT_WP_KEL,
                                AlamatWpKota = item.ALAMAT_WP_KOTA,
                                WpKtp = item.WP_KTP,
                                WpNama = item.WP_NAMA,
                                WpNpwp = item.WP_NPWP,
                                WpRt = item.WP_RT,
                                WpRw = item.WP_RW,
                                Status = item.STATUS,
                                InsDate = item.INS_DATE,
                                InsBy = item.INS_BY
                            };
                            newList.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing NOP {item.NOP}: {ex.Message}");
                    }

                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    if (index % 1000 == 0 || index == jmlData)
                    {
                        Console.Write($"\rDB_OP_PBB {index}/{jmlData} ({persen:F2}%) diproses...");
                    }
                }

                // Step 3: Commit ke database (batch insert dan update)
                if (newList.Any())
                    _contMonPd.DbOpPbbs.AddRange(newList);

                foreach (var updated in updateList)
                    _contMonPd.DbOpPbbs.Update(updated);

                _contMonPd.SaveChanges();

                Console.WriteLine("\nSelesai proses insert/update semua data.");


                //int index = 0;
                //foreach (var item in result)
                //{
                //    // DATA OP
                //    try
                //    {
                //        var sourceRow = _contMonPd.DbOpPbbs.SingleOrDefault(x => x.Nop == item.NOP);
                //        if (sourceRow != null)
                //        {
                //            sourceRow.Status = item.STATUS;
                //            sourceRow.LuasTanah = item.LUAS_TANAH;
                //            sourceRow.AlamatKdLurah = item.ALAMAT_KD_LURAH;
                //            sourceRow.AlamatKdCamat = item.ALAMAT_KD_CAMAT;
                //            _contMonPd.SaveChanges();
                //        }
                //        else
                //        {
                //            var newRow = new MonPDLib.EF.DbOpPbb();

                //            newRow.Nop = item.NOP;
                //            newRow.KategoriId = item.KATEGORI_ID;
                //            newRow.KategoriNama = item.KATEGORI_NAMA;
                //            newRow.AlamatOp = item.ALAMAT_OP;
                //            newRow.AlamatOpNo = item.ALAMAT_OP_NO;
                //            newRow.AlamatOpRt = item.ALAMAT_OP_RT;
                //            newRow.AlamatOpRw = item.ALAMAT_OP_RW;
                //            newRow.AlamatKdCamat = item.ALAMAT_KD_CAMAT;
                //            newRow.AlamatKdLurah = item.ALAMAT_KD_LURAH;
                //            newRow.Uptb = item.UPTB;
                //            newRow.LuasTanah = item.LUAS_TANAH;
                //            newRow.AlamatWp = item.ALAMAT_WP;
                //            newRow.AlamatWpNo = item.ALAMAT_WP_NO;
                //            newRow.AlamatWpKel = item.ALAMAT_WP_KEL;
                //            newRow.AlamatWpKota = item.ALAMAT_WP_KOTA;
                //            newRow.WpKtp = item.WP_KTP;
                //            newRow.WpNama = item.WP_NAMA;
                //            newRow.WpNpwp = item.WP_NPWP;
                //            newRow.WpRt = item.WP_RT;
                //            newRow.WpRw = item.WP_RW;
                //            newRow.Status = item.STATUS;
                //            newRow.InsDate = item.INS_DATE;
                //            newRow.InsBy = item.INS_BY;
                //            _contMonPd.DbOpPbbs.Add(newRow);
                //            _contMonPd.SaveChanges();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"Error processing NOP {ex.Message}");
                //        var kkk = item.NOP;
                //    }
                //    _contMonPd.SaveChanges();
                //    index++;
                //    double persen = ((double)index / jmlData) * 100;
                //    Console.Write($"\rDB_OP_PBB JML OP {jmlData} : {item.NOP}  {persen:F2}%   ");
                //    Thread.Sleep(50);
                //}
            }
        }

        private void GetKetetapan1(int tahunBuku)
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
                    newRow.IsLunas = itemKetetapan.IS_LUNAS;
                    newRow.TglBayar = itemKetetapan.TGL_BAYAR;
                    newRow.JumlahBayarPokok = itemKetetapan.JUMLAH_BAYAR_POKOK;
                    newRow.JumlahBayarSanksi = itemKetetapan.JUMLAH_BAYAR_SANKSI;
                    newRow.InsDate = itemKetetapan.INS_DATE;
                    newRow.InsBy = itemKetetapan.INS_BY;
                    _contMonPd.DbMonPbbs.Add(newRow);
                    _contMonPd.SaveChanges();
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\rDB_KETETAPAN_PBB HPP TAHUN {tahunBuku} JML OP {jmlData} : {jml.ToString("n0")}  {persen:F2}%   ");
                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing KETETAPAN {ex.Message}");
            }
        }

        private void GetRealisasi(int tahunBuku)
        {
            try
            {
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlRealisasi = @"
             SELECT NOP,TAHUN_PAJAK,SUM(POKOK) POKOK , SUM(SANKSI) SANKSI
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
                    if (realisasi != null)
                    {
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
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPPBB1.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPPBB1.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }
    }
}
