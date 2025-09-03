using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
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
        private static EnumFactory.EPajak PAJAK_ENUM = EnumFactory.EPajak.PBB;

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
                    DoWorkNewMeta(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                    MailHelper.SendMail(
                    false,
                    "ERROR HOTEL WS",
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
                GetOPProcess();
            }

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                GetRealisasi(i);
            }

            for (var i = tahunAmbil; i <= tglServer.Year; i++)
            {
                UpdateKoreksi(i);
            }

            MailHelper.SendMail(
            false,
            "DONE PBB WS",
            $@"PBB WS FINISHED",
            null
            );
        }


        public void GetOPProcess()
        {
            var tglSkr = DateTime.Now;
            Stopwatch swQuery = new Stopwatch();
            Stopwatch swProses = new Stopwatch();
            swQuery.Start();
            
            try
            {
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();               

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
JOIN TABCAMAT@LIHATGATOTKACA B ON A.T_PROP_KD=B.T_PROP_KD AND A.T_DATI2_KD = B.T_DATI2_KD AND A.T_KEC_KD=B.T_KEC_KD";
                var result = _contMonitoringDB.Set<OPPbb>().FromSqlRaw(sql).AsNoTracking().ToList();
                var jml = result.Count;
                var index = 0;                
                var nopList = result.Select(r => r.NOP).ToList();                
                var newList = new List<MonPDLib.EF.DbOpPbb>();
                var updateList = new List<MonPDLib.EF.DbOpPbb>();
                swQuery.Stop();
                swProses.Start();
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
                    Console.Write($"\r[{tglSkr.ToString("dd MMM yyyy HH:mm:ss")}] total data OP {jml.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}    [({persen:F2}%)]");
                }
                swProses.Stop();
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
                Console.Write($"Done Proses Query :{swQuery.Elapsed.TotalSeconds} Sec, Proses Data {swProses.Elapsed.TotalSeconds} Sec");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OP ERROR: {ex.Message}");
            }
        }


        private void GetRealisasi(int tahunBuku)
        {
            var tglMulai = DateTime.Now;            
            Stopwatch swQuery = new Stopwatch();
            Stopwatch swProses = new Stopwatch();
            swQuery.Start();
            try
            {
                var _contMonitoringDB = DBClass.GetMonitoringDbContext();
                var _contMonPd = DBClass.GetContext();
                var sqlRealisasi = @"
            SELECT A.T_PROP_KD || A.T_DATI2_KD || A.T_KEC_KD || A.T_KEL_KD || A.D_NOP_BLK || A.D_NOP_URUT || A.D_NOP_JNS NOP,
            TO_NUMBER(A.D_PJK_THN) TAHUN_PAJAK, TO_NUMBER(POKOK) POKOK,NVL( LUNAS,'N') LUNAS,TGL_BAYAR, TO_NUMBER(NVL(BAYAR_POKOK,0)) BAYAR_POKOK,
TO_NUMBER(NVL( BAYAR_SANKSI,0))  BAYAR_SANKSI,
  CASE
               WHEN KATEGORI IS NULL THEN 72
               WHEN KATEGORI='PASAR' THEN  57
               WHEN KATEGORI='HOTEL/WISMA' THEN 65
               WHEN KATEGORI='TANAH KOSONG' THEN 66
               WHEN KATEGORI='PERUMAHAN' THEN 67
               WHEN KATEGORI='BENGKEL/GUDANG/PERTANIAN' THEN 68
               WHEN KATEGORI='TOKO/APOTIK/PASAR/RUKO' THEN 69
               WHEN KATEGORI='POMPA BENSIN' THEN 70
               WHEN KATEGORI='PERKANTORAN SWASTA' THEN 72
               WHEN KATEGORI='LAIN-LAIN' THEN 72
               WHEN KATEGORI='RUMAH SAKIT/KLINIK' THEN 73
               WHEN KATEGORI='RUSUN' THEN 74
               WHEN KATEGORI='PABRIK' THEN 75
               WHEN KATEGORI='TANGKI MINYAK' THEN 76
               WHEN KATEGORI='GEDUNG SEKOLAH' THEN 77
               WHEN KATEGORI='BANGUNAN TIDAK KENA PAJAK' THEN 78
                WHEN KATEGORI='APARTEMEN' THEN 79
                WHEN KATEGORI='BANGUNAN PARKIR' THEN 80
                WHEN KATEGORI='OLAH RAGA/REKREASI' THEN 81
                WHEN KATEGORI='MENARA/TOWER' THEN 82
                WHEN KATEGORI='GEDUNG PEMERINTAH' THEN 83
                ELSE 72
                 END ID_PERUNTUKAN,
                   CASE
               WHEN KATEGORI IS NULL THEN 'LAIN-LAIN'
               WHEN KATEGORI='PASAR' THEN  'PASAR'
               WHEN KATEGORI='HOTEL/WISMA' THEN 'HOTEL/WISMA'
               WHEN KATEGORI='TANAH KOSONG' THEN 'TANAH KOSONG'
               WHEN KATEGORI='PERUMAHAN' THEN 'PERUMAHAN'
               WHEN KATEGORI='BENGKEL/GUDANG/PERTANIAN' THEN 'BENGKEL/GUDANG/PERTANIAN'
               WHEN KATEGORI='TOKO/APOTIK/PASAR/RUKO' THEN 'TOKO/APOTIK/PASAR/RUKO'
               WHEN KATEGORI='POMPA BENSIN' THEN 'POMPA BENSIN'
               WHEN KATEGORI='PERKANTORAN SWASTA' THEN 'PERKANTORAN SWASTA'
               WHEN KATEGORI='LAIN-LAIN' THEN 'LAIN-LAIN'
               WHEN KATEGORI='RUMAH SAKIT/KLINIK' THEN 'RUMAH SAKIT/KLINIK'
               WHEN KATEGORI='RUSUN' THEN 'RUSUN'
               WHEN KATEGORI='PABRIK' THEN 'PABRIK'
               WHEN KATEGORI='TANGKI MINYAK' THEN 'TANGKI MINYAK'
               WHEN KATEGORI='GEDUNG SEKOLAH' THEN 'GEDUNG SEKOLAH'
               WHEN KATEGORI='BANGUNAN TIDAK KENA PAJAK' THEN 'BANGUNAN TIDAK KENA PAJAK'
                WHEN KATEGORI='APARTEMEN' THEN 'APARTEMEN'
                WHEN KATEGORI='BANGUNAN PARKIR' THEN 'BANGUNAN PARKIR'
                WHEN KATEGORI='OLAH RAGA/REKREASI' THEN 'OLAH RAGA/REKREASI'
                WHEN KATEGORI='MENARA/TOWER' THEN 'MENARA/TOWER'
                WHEN KATEGORI='GEDUNG PEMERINTAH' THEN 'GEDUNG PEMERINTAH'
                ELSE 'LAIN-LAIN'
                 END PERUNTUKAN         
FROM (            
SELECT A.*,D_PJK_TAX- NVL(D_PJK_ADJ,0) POKOK,D_PJK_LUNAS LUNAS            
FROM (
SELECT A.T_PROP_KD , A.T_DATI2_KD , A.T_KEC_KD , A.T_KEL_KD , A.D_NOP_BLK ,A.D_NOP_URUT , A.D_NOP_JNS ,D_PJK_THN,
            SUM(POKOK) BAYAR_POKOK , SUM(SANKSI) BAYAR_SANKSI , MAX(D_PJK_TGBYR) TGL_BAYAR
FROM (       
SELECT     A.T_PROP_KD , A.T_DATI2_KD , A.T_KEC_KD , A.T_KEL_KD , A.D_NOP_BLK ,A.D_NOP_URUT , A.D_NOP_JNS ,D_PJK_THN,
                D_PJK_PBB POKOK,D_PJK_JMBYR-D_PJK_PBB SANKSI,D_PJK_TGBYR                
FROM        CATBAYAR@LIHATGATOTKACA A
WHERE       TO_CHAR(D_PJK_TGBYR,'YYYY') = :TAHUN
UNION ALL
SELECT      A.KD_PROPINSI , A.KD_DATI2 , A.KD_KECAMATAN , A.KD_KELURAHAN , A.KD_BLOK , A.NO_URUT , A.KD_JNS_OP ,THN_PAJAK_SPPT,
                JML_SPPT_YG_DIBAYAR-DENDA_SPPT,DENDA_SPPT,TGL_PEMBAYARAN_SPPT                 
FROM        PEMBAYARAN_SPPT@LIHATGATOTKACA A
WHERE       to_char(TGL_PEMBAYARAN_SPPT,'YYYY') = :TAHUN AND NVL(REV_FLAG,0) !=1
) A 
GROUP BY A.T_PROP_KD , A.T_DATI2_KD , A.T_KEC_KD , A.T_KEL_KD , A.D_NOP_BLK ,A.D_NOP_URUT , A.D_NOP_JNS ,D_PJK_THN
) A
JOIN DATABAYAR@LIHATGATOTKACA B ON A.T_PROP_KD=B.T_PROP_KD AND A.T_DATI2_KD = B.T_DATI2_KD AND A.T_KEC_KD=B.T_KEC_KD AND 
                                                                                                                         A.T_KEL_KD=B.T_KEL_KD AND A.D_NOP_BLK=B.D_NOP_BLK AND 
                                                                                                                         A.D_NOP_URUT=B.D_NOP_URUT AND A.D_NOP_JNS=B.D_NOP_JNS AND
                                                                                                                         A.D_PJK_THN=B.D_PJK_THN                                                                                                                         
) A
LEFT JOIN POTENSIBYR@NRC B ON  A.T_PROP_KD=SPPT_PROP AND A.T_DATI2_KD=SPPT_KOTA AND A.T_KEC_KD=SPPT_KEC AND A.T_KEL_KD=SPPT_KEL AND 
                                                     A.D_NOP_BLK=SPPT_URUTBLK AND A.D_NOP_URUT=SPPT_URUTOP AND A.D_NOP_JNS=SPPT_TANDA           
                ";

                var realisasiMonitoringDb = _contMonitoringDB.Set<SSPDPBB>()
                    .FromSqlRaw(sqlRealisasi, new[] {                                
                                new OracleParameter("TAHUN", tahunBuku.ToString())
                    }).ToList();

                swQuery.Stop();
                int jmlData = realisasiMonitoringDb.Count;
                int index = 0;
                var newList = new List<MonPDLib.EF.DbMonPbb>();
                var updateList = new List<MonPDLib.EF.DbMonPbb>();
                swProses.Start();
                foreach (var itemRealisasi in realisasiMonitoringDb)
                {
                    var realisasi = _contMonPd.DbMonPbbs.Find(itemRealisasi.NOP,(decimal)tahunBuku, itemRealisasi.TAHUN_PAJAK);
                    if (realisasi != null)
                    {
                        realisasi.KategoriId = itemRealisasi.ID_PERUNTUKAN;
                        realisasi.KategoriNama = itemRealisasi.PERUNTUKAN;
                        realisasi.IsLunas = itemRealisasi.LUNAS=="Y"?1:0;
                        realisasi.TglBayar = itemRealisasi.TGL_BAYAR;
                        realisasi.PokokPajak = itemRealisasi.POKOK;
                        realisasi.JumlahBayarPokok = itemRealisasi.BAYAR_POKOK;
                        realisasi.JumlahBayarSanksi = itemRealisasi.BAYAR_SANKSI;                        
                        updateList.Add(realisasi);
                    }
                    else
                    {
                        var OP = _contMonPd.DbOpPbbs.Find(itemRealisasi.NOP);

                        if (OP != null)
                        {
                            //var OP = OPL.First();
                            var AkunPokok = GetDbAkunPokok(tahunBuku, KDPajak, (int)OP.KategoriId);
                            var AkunSanksi = GetDbAkunSanksi(tahunBuku, KDPajak, (int)OP.KategoriId);

                            var newRow = new DbMonPbb();
                            newRow.Nop = OP.Nop;
                            newRow.TahunBuku = tahunBuku;
                            newRow.KategoriId = OP.KategoriId;
                            newRow.KategoriNama = OP.KategoriNama;
                            newRow.AlamatOp = OP.AlamatOp;
                            newRow.AlamatOpNo = OP.AlamatOpNo ;
                            newRow.AlamatOpRt = OP.AlamatOpRt;
                            newRow.AlamatOpRw = OP.AlamatOpRw;
                            newRow.AlamatKdCamat = OP.AlamatKdCamat;
                            newRow.AlamatKdLurah = OP.AlamatKdLurah;
                            newRow.Uptb = OP.Uptb;
                            newRow.AlamatWp = OP.AlamatWp;
                            newRow.AlamatWpNo = OP.AlamatWpNo;
                            newRow.AlamatWpKel = OP.AlamatWpKel;
                            newRow.AlamatWpKota = OP.AlamatWpKota;
                            newRow.WpNama = OP.WpNama;
                            newRow.WpNpwp= OP.WpNpwp;
                            newRow.TahunPajak = itemRealisasi.TAHUN_PAJAK;
                            newRow.PokokPajak = itemRealisasi.POKOK;
                            newRow.KategoriOp = OP.KategoriNama;
                            newRow.Peruntukan = itemRealisasi.PERUNTUKAN;
                            newRow.IsLunas = itemRealisasi.LUNAS=="Y"?1:0;
                            newRow.TglBayar= itemRealisasi.TGL_BAYAR;
                            newRow.JumlahBayarPokok= itemRealisasi.BAYAR_POKOK;
                            newRow.JumlahBayarSanksi = itemRealisasi.BAYAR_SANKSI;
                            newRow.InsDate = DateTime.Now;
                            newRow.InsBy = "JOB";                            
                            newList.Add(newRow);
                        }
                        else
                        {
                            var newRow = new DbMonPbb();
                            newRow.Nop = itemRealisasi.NOP;
                            newRow.TahunBuku = tahunBuku;
                            newRow.KategoriId = 72;
                            newRow.KategoriNama = "LAIN-LAIN";
                            newRow.AlamatOp = "-";
                            newRow.AlamatOpNo = "-";
                            newRow.AlamatOpRt = "-";
                            newRow.AlamatOpRw = "-";
                            newRow.AlamatKdCamat = "-";
                            newRow.AlamatKdLurah = "-";
                            newRow.Uptb = 0;
                            newRow.AlamatWp = "-";
                            newRow.AlamatWpNo = "-";
                            newRow.AlamatWpKel = "-";
                            newRow.AlamatWpKota = "-";
                            newRow.WpNama = "-";
                            newRow.WpNpwp = "-";
                            newRow.TahunPajak = itemRealisasi.TAHUN_PAJAK;
                            newRow.PokokPajak = itemRealisasi.POKOK;
                            newRow.KategoriOp = "LAIN-LAIN";
                            newRow.Peruntukan = itemRealisasi.PERUNTUKAN;
                            newRow.IsLunas = itemRealisasi.LUNAS == "Y" ? 1 : 0;
                            newRow.TglBayar = itemRealisasi.TGL_BAYAR;
                            newRow.JumlahBayarPokok = itemRealisasi.BAYAR_POKOK;
                            newRow.JumlahBayarSanksi = itemRealisasi.BAYAR_SANKSI;
                            newRow.InsDate = DateTime.Now;
                            newRow.InsBy = "JOB";                            
                            newList.Add(newRow);
                        }
                    }
                    index++;
                    double persen = ((double)index / jmlData) * 100;
                    Console.Write($"\r{tglMulai.ToString("dd MMM yyyy HH:mm:ss")} REALISASI HOTEL TAHUN {tahunBuku} JML DATA {jmlData.ToString("n0")} Baru: {newList.Count.ToString("n0")}, Update: {updateList.Count.ToString("n0")}     [({persen:F2}%)]");
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
                swProses.Stop();
                Console.Write($"Done Proses Query :{swQuery.Elapsed.TotalSeconds} Sec, Proses Data {swProses.Elapsed.TotalSeconds} Sec");
                Console.WriteLine($"");

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

        private List<string> GetKategoriOvveride(string nop)
        {
            var ret = new List<string>();
            ret.Add("17");
            ret.Add("HOTEL NON BINTANG");

            var c = DBClass.GetMonitoringDbContext();
            var connection = c.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @" SELECT *
                                        FROM T_OP_KATEGORI_STATUS
                                        WHERE REPLACE(FK_NOP,'.','')=:NOP  AND ROWNUM=1";
                var param = command.CreateParameter();
                param.ParameterName = "NOP";
                param.Value = nop;
                command.Parameters.Add(param);
                var dr = command.ExecuteReader();

                if (dr.Read())
                {
                    var katname = dr.GetString(2);
                    switch (katname)
                    {
                        //HOTEL BINTANG TIGA
                        //RUMAH KOS
                        //HOTEL BINTANG EMPAT
                        //HOTEL BINTANG DUA
                        //RESTORAN
                        //HOTEL NON BINTANG
                        //HOTEL BINTANG SATU
                        //KATERING
                        //HOTEL BINTANG LIMA
                        case "HOTEL BINTANG TIGA":
                            ret[0] = "14";
                            ret[1] = "HOTEL BINTANG TIGA";
                            break;
                        case "RUMAH KOS":
                            ret[0] = "17";
                            ret[1] = "HOTEL NON BINTANG";
                            break;
                        case "HOTEL BINTANG EMPAT":
                            ret[0] = "15";
                            ret[1] = "HOTEL BINTANG EMPAT";
                            break;
                        case "HOTEL BINTANG DUA":
                            ret[0] = "13";
                            ret[1] = "HOTEL BINTANG DUA";
                            break;
                        case "RESTORAN":
                            ret[0] = "17";
                            ret[1] = "HOTEL NON BINTANG";
                            break;
                        case "HOTEL NON BINTANG":
                            ret[0] = "17";
                            ret[1] = "HOTEL NON BINTANG";
                            break;
                        case "HOTEL BINTANG SATU":
                            ret[0] = "12";
                            ret[1] = "HOTEL BINTANG SATU";
                            break;
                        case "KATERING":
                            ret[0] = "17";
                            ret[1] = "HOTEL NON BINTANG";
                            break;
                        case "HOTEL BINTANG LIMA":
                            ret[0] = "16";
                            ret[1] = "HOTEL BINTANG LIMA";
                            break;
                        default:
                            ret[0] = "17";
                            ret[1] = "HOTEL NON BINTANG";
                            break;
                    }

                }
                dr.Close();
            }
            catch
            {

            }

            connection.Close();
            return ret;
        }
        public static List<string> GetInfoWPHPP(string npwpd)
        {
            var ret = new List<string>();
            var c = DBClass.GetMonitoringDbContext();
            var connection = c.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                var command = connection.CreateCommand();
                command.CommandText = @" SELECT  NVL(NAMAWP,'-'),NVL(ALAMAT,'-')
                                    FROM PHRH_USER.npwpd_baru@LIHATHR
                                    WHERE NPWPD=:NPWPD  AND ROWNUM=1";
                var param = command.CreateParameter();
                param.ParameterName = "NPWPD";
                param.Value = npwpd;
                command.Parameters.Add(param);
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    ret.Add(dr.GetString(0));
                    ret.Add(dr.GetString(1));
                }
                else
                {
                    ret.Add("-");
                    ret.Add("-");
                }
                dr.Close();
            }
            catch
            {

            }

            connection.Close();
            return ret;
        }
        private Helper.DbAkun GetDbAkun(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.Kategoris).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.Kategoris.Any(y => y.PajakId == idPajak && y.Id == idKategori));

            if (row != null)
            {
                return new Helper.DbAkun
                {
                    Akun = row.Akun,
                    NamaAkun = row.NamaAkun,
                    Kelompok = row.Kelompok,
                    NamaKelompok = row.NamaKelompok,
                    Jenis = row.Jenis,
                    NamaJenis = row.NamaJenis,
                    Objek = row.Objek,
                    NamaObjek = row.NamaObjek,
                    Rincian = row.Rincian,
                    NamaRincian = row.NamaRincian,
                    SubRincian = row.SubRincian,
                    NamaSubRincian = row.NamaSubRincian,
                };
            }
            else
            {
                return new Helper.DbAkun
                {
                    Akun = "-",
                    NamaAkun = "-",
                    Kelompok = "-",
                    NamaKelompok = "-",
                    Jenis = "-",
                    NamaJenis = "-",
                    Objek = "-",
                    NamaObjek = "-",
                    Rincian = "-",
                    NamaRincian = "-",
                    SubRincian = "-",
                    NamaSubRincian = "-",
                };
            }
        }
        private Helper.DbAkun GetDbAkunPokok(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.Kategoris).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.Kategoris.Any(y => y.PajakId == idPajak && y.Id == idKategori));
            if (row != null)
            {
                return new Helper.DbAkun
                {
                    Akun = row.Akun,
                    NamaAkun = row.NamaAkun,
                    Kelompok = row.Kelompok,
                    NamaKelompok = row.NamaKelompok,
                    Jenis = row.Jenis,
                    NamaJenis = row.NamaJenis,
                    Objek = row.Objek,
                    NamaObjek = row.NamaObjek,
                    Rincian = row.Rincian,
                    NamaRincian = row.NamaRincian,
                    SubRincian = row.SubRincian,
                    NamaSubRincian = row.NamaSubRincian,
                };
            }
            else
            {
                return new Helper.DbAkun
                {
                    Akun = "-",
                    NamaAkun = "-",
                    Kelompok = "-",
                    NamaKelompok = "-",
                    Jenis = "-",
                    NamaJenis = "-",
                    Objek = "-",
                    NamaObjek = "-",
                    Rincian = "-",
                    NamaRincian = "-",
                    SubRincian = "-",
                    NamaSubRincian = "-",
                };
            }

        }
        private Helper.DbAkun GetDbAkunSanksi(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.KategoriSanksis).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.KategoriSanksis.Any(y => y.PajakId == idPajak && y.Id == idKategori));
            if (row != null)
            {
                return new Helper.DbAkun
                {
                    Akun = row.Akun,
                    NamaAkun = row.NamaAkun,
                    Kelompok = row.Kelompok,
                    NamaKelompok = row.NamaKelompok,
                    Jenis = row.Jenis,
                    NamaJenis = row.NamaJenis,
                    Objek = row.Objek,
                    NamaObjek = row.NamaObjek,
                    Rincian = row.Rincian,
                    NamaRincian = row.NamaRincian,
                    SubRincian = row.SubRincian,
                    NamaSubRincian = row.NamaSubRincian,
                };
            }
            else
            {
                return new Helper.DbAkun
                {
                    Akun = "-",
                    NamaAkun = "-",
                    Kelompok = "-",
                    NamaKelompok = "-",
                    Jenis = "-",
                    NamaJenis = "-",
                    Objek = "-",
                    NamaObjek = "-",
                    Rincian = "-",
                    NamaRincian = "-",
                    SubRincian = "-",
                    NamaSubRincian = "-",
                };
            }

        }
        private void UpdateKoreksi(int tahunBuku)
        {
            Console.WriteLine($"[START] UpdateKoreksi {tahunBuku}");

            var context = DBClass.GetContext();
            var query = @"SELECT 	TAHUN, 
		                A.PAJAK_ID, 
		                A.SCONTRO, 
		                B.REALISASI, 
		                (A.SCONTRO-B.REALISASI) SELISIH
                FROM 
                (
	                SELECT 
	                       P.TAHUN_BUKU AS TAHUN,
	                       A.PAJAK_ID,
	                       SUM(P.REALISASI) AS SCONTRO
	                FROM DB_PENDAPATAN_DAERAH P
	                LEFT JOIN DB_PAJAK_MAPPING A 
	                     ON P.AKUN = A.AKUN
	                     AND P.KELOMPOK = A.KELOMPOK 
	                     AND P.JENIS = A.JENIS 
	                     AND P.OBJEK = A.OBJEK 
	                     AND P.RINCIAN = A.RINCIAN 
	                     AND P.SUB_RINCIAN = A.SUB_RINCIAN 
	                     AND P.TAHUN_BUKU = A.TAHUN_BUKU
	                WHERE P.TAHUN_BUKU = :YEAR
	                  AND P.OBJEK LIKE '4.1.01%'
	                  AND A.PAJAK_ID IS NOT NULL
	                GROUP BY P.TAHUN_BUKU, A.PAJAK_ID
                ) A 
                JOIN 
                (
	                SELECT EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
	                    1 AS PAJAK_ID, 
	                    SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
	                FROM DB_MON_RESTO
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
	                    2 AS PAJAK_ID, 
	                    SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
	                FROM DB_MON_PPJ
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
	                    3 AS PAJAK_ID, 
	                    SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
	                FROM DB_MON_HOTEL
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
	                    4 AS PAJAK_ID, 
	                    SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
	                FROM DB_MON_PARKIR
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
	                    5 AS PAJAK_ID, 
	                    SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
	                FROM DB_MON_HIBURAN
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
	                    6 AS PAJAK_ID, 
	                    SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
	                FROM DB_MON_ABT
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR_POKOK) AS TAHUN_BUKU,
	                    7 AS PAJAK_ID, 
	                    SUM(NVL(NOMINAL_POKOK_BAYAR, 0)) AS REALISASI
	                FROM DB_MON_REKLAME
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR_POKOK) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR_POKOK)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR) AS TAHUN_BUKU,
	                    9 AS PAJAK_ID, 
	                    SUM(NVL(JUMLAH_BAYAR_POKOK, 0)) AS REALISASI
	                FROM DB_MON_PBB
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_BAYAR) AS TAHUN_BUKU,
	                    12 AS PAJAK_ID, 
	                    SUM(NVL(POKOK, 0)) AS REALISASI
	                FROM DB_MON_BPHTB
	                WHERE EXTRACT(YEAR FROM TGL_BAYAR) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_BAYAR)
	                UNION ALL
	                SELECT 
	                    EXTRACT(YEAR FROM TGL_SSPD) AS TAHUN_BUKU,
	                    20 AS PAJAK_ID, 
	                    SUM(NVL(JML_POKOK, 0)) AS REALISASI
	                FROM DB_MON_OPSEN_PKB
	                WHERE EXTRACT(YEAR FROM TGL_SSPD) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_SSPD)
	                UNION ALL
	                SELECT  EXTRACT(YEAR FROM TGL_SSPD) AS TAHUN_BUKU,
		                    21 AS PAJAK_ID, 
		                    SUM(NVL(JML_POKOK, 0)) AS REALISASI
	                FROM DB_MON_OPSEN_BBNKB
	                WHERE EXTRACT(YEAR FROM TGL_SSPD) = :YEAR
	                GROUP BY EXTRACT(YEAR FROM TGL_SSPD)
                ) B ON A.TAHUN = B.TAHUN_BUKU 
	                AND A.PAJAK_ID = B.PAJAK_ID
                WHERE A.PAJAK_ID = :PAJAK";

            var db = getOracleConnection();
            var result = db.Query<MonPDLib.Helper.SCONTROSELISIH>(query, new { YEAR = tahunBuku, PAJAK = (int)PAJAK_ENUM }).ToList();

            decimal selisih = result.FirstOrDefault()?.SELISIH ?? 0;

            int pajakId = (int)PAJAK_ENUM;
            string pajakNama = PAJAK_ENUM.GetDescription();
            var kdPajakString = ((int)PAJAK_ENUM).ToString().PadLeft(2, '0');
            var nop = $"0000000000000000{kdPajakString}";
            var namaop = $"KOREKSI SCONTRO {PAJAK_ENUM.GetDescription()}";

            int kategoriId = 57;
            string kategoriNama = "PBB";
            var tanggal = DateTime.Now.Date;
            if (tahunBuku < DateTime.Now.Year)
            {
                tanggal = new DateTime(tahunBuku, 12, 31);
            }

            var source = context.DbOpPbbs.FirstOrDefault(x => x.Nop == nop);
            if (source != null)
            {
                source.WpNama = namaop;

                context.DbOpPbbs.Update(source);
                context.SaveChanges();
            }
            else
            {
                var newRow = new DbOpPbb();

                newRow.Nop = nop;
                newRow.KategoriId = kategoriId;
                newRow.KategoriNama = kategoriNama;
                newRow.AlamatOp = "-";
                newRow.AlamatOpNo = "-";
                newRow.AlamatOpRt = "-";
                newRow.AlamatOpRw = "-";
                newRow.AlamatKdCamat = "-";
                newRow.AlamatKdLurah = "-";
                newRow.Uptb = 0;
                newRow.LuasTanah = 0;
                newRow.AlamatWp = "-";
                newRow.AlamatWpNo = "-";
                newRow.AlamatWpKel = "-";
                newRow.AlamatWpKota = "-";
                newRow.WpKtp = "-";
                newRow.WpNama = namaop;
                newRow.WpNpwp = "-";
                newRow.WpRt = "-";
                newRow.WpRw = "-";
                newRow.Status = 0;
                newRow.InsDate = tanggal;
                newRow.InsBy = "-";


                context.DbOpPbbs.Add(newRow);
                context.SaveChanges();
            }

            source = context.DbOpPbbs.FirstOrDefault(x => x.Nop == nop);
            if (source == null)
            {
                throw new Exception("Gagal membuat data OP untuk koreksi scontro");
            }
            var sourceMon = context.DbMonPbbs.Where(x => x.Nop == nop && x.TahunBuku == tahunBuku).FirstOrDefault();
            if (sourceMon != null)
            {
                sourceMon.TglBayar = tanggal;
                sourceMon.JumlahBayarPokok = selisih;
                context.DbMonPbbs.Update(sourceMon);
                context.SaveChanges();
            }
            else
            {
                var newRow = new DbMonPbb();

                newRow.Nop = nop;
                newRow.TahunBuku = tahunBuku;
                newRow.KategoriId = kategoriId;
                newRow.KategoriNama = kategoriNama;
                newRow.AlamatOp = "-";
                newRow.AlamatOpNo = "-";
                newRow.AlamatOpRt = "-";
                newRow.AlamatOpRw = "-";
                newRow.AlamatKdCamat = "-";
                newRow.AlamatKdLurah = "-";
                newRow.Uptb = 0;
                newRow.AlamatWp = "-";
                newRow.AlamatWpNo = "-";
                newRow.AlamatWpKel = "-";
                newRow.AlamatWpKota = "-";
                newRow.WpNama = namaop;
                newRow.WpNpwp = nop;
                newRow.TahunPajak = tahunBuku;
                newRow.PokokPajak = selisih;
                newRow.KategoriOp = "-";
                newRow.Peruntukan = "-";
                newRow.IsLunas = 0;
                newRow.TglBayar = tanggal;
                newRow.JumlahBayarPokok = selisih;
                newRow.InsDate = tanggal;
                newRow.InsBy = "-";
                newRow.JumlahBayarSanksi = 0;
                newRow.KetetapanPokok = 0;

                context.DbMonPbbs.Add(newRow);
                context.SaveChanges();
            }

            Console.WriteLine($"[FINISHED] UpdateKoreksi {tahunBuku}");
        }
        public static OracleConnection getOracleConnection()
        {
            try
            {
                OracleConnection ret = new OracleConnection(MonPDLib.DBClass.Monpd);
                ret.Open();
                ret.Close();
                return ret;
            }
            catch (Exception ex)
            {
                return new OracleConnection();
            }
        }
    }
}
