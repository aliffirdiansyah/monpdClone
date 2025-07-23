using Dapper;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using Oracle.ManagedDataAccess.Client;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MonPDLib.General.EnumFactory;
using static MonPDLib.Helper;

namespace ReklameSuratWs
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
                    "ERROR REKLAME_SURAT WS",
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
                        SELECT NOFORM_S NO_FORMULIR, TGL_KIRIM_EMAIL, STATUS_EMAIL, EMAIL, KET_EMAIL
                        FROM DETAIL_TEGURAN_ELEKTRONIK
                    ";

                    var result = await _contMonitoringDb.Set<DbMonReklameEmail>().FromSqlRaw(sql).ToListAsync();

                    var source = _contMonPd.DbMonReklameEmails.ToList();
                    foreach (var item in result)
                    {
                        var rowMonReklameEmail = source.FirstOrDefault(x => x.NoFormulir == item.NoFormulir && x.TglKirimEmail == item.TglKirimEmail);
                        if (rowMonReklameEmail != null)
                        {
                            _contMonPd.DbMonReklameEmails.Remove(rowMonReklameEmail);
                        }

                        _contMonPd.DbMonReklameEmails.Add(new DbMonReklameEmail()
                        {
                            NoFormulir = item.NoFormulir,
                            TglKirimEmail = item.TglKirimEmail,
                            StatusEmail = item.StatusEmail,
                            Email = item.Email,
                            KetEmail = item.KetEmail,
                        });

                        _contMonPd.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now} DB_MON_REKLAME_EMAILS {item.NoFormulir} - {item.TglKirimEmail}");
                        Console.ResetColor();
                    }
                }

                using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                {
                    var sql = @"
                        SELECT KLASIFIKASI, TAHUN_SURAT, PAJAK, KODE_DOKUMEN, BIDANG, AGENDA, NO_SURAT, TGL_SURAT, NAMA, ALAMAT, STATUS, REFF_BATAL, KETERANGAN, NIP, NAMA_PEJABAT, GOLONGAN, JABATAN, TAG_PENCARIAN, INS_DATE, INS_BY
                        FROM T_SURAT_REKLAME
                    ";

                    var result = await _contMonitoringDb.Set<DbMonReklameSurat>().FromSqlRaw(sql).ToListAsync();

                    var source = _contMonPd.DbMonReklameSurats.ToList();
                    foreach (var item in result)
                    {
                        if(item.Agenda == 717)
                        {
                            string x = "";
                        }

                        var row = source.FirstOrDefault(x =>
                               x.Agenda == item.Agenda
                            && x.Bidang == item.Bidang
                            && x.Klasifikasi == item.Klasifikasi
                            && x.KodeDokumen == item.KodeDokumen
                            && x.Pajak == item.Pajak
                            && x.TahunSurat == item.TahunSurat
                            );
                        if (row != null)
                        {
                            _contMonPd.DbMonReklameSurats.Remove(row);
                        }

                        _contMonPd.DbMonReklameSurats.Add(new DbMonReklameSurat()
                        {
                            Klasifikasi = item.Klasifikasi,
                            TahunSurat = item.TahunSurat,
                            Pajak = item.Pajak,
                            KodeDokumen = item.KodeDokumen,
                            Bidang = item.Bidang,
                            Agenda = item.Agenda,
                            NoSurat = item.NoSurat,
                            TglSurat = item.TglSurat,
                            Nama = item.Nama,
                            Alamat = item.Alamat,
                            Status = item.Status,
                            ReffBatal = item.ReffBatal,
                            Keterangan = item.Keterangan,
                            Nip = item.Nip,
                            NamaPejabat = item.NamaPejabat,
                            Golongan = item.Golongan,
                            Jabatan = item.Jabatan,
                            TagPencarian = item.TagPencarian,
                            InsDate = item.InsDate,
                            InsBy = item.InsBy,
                        });

                        _contMonPd.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now} DB_MON_REKLAME_SURAT {item.Agenda} - {item.Bidang} - {item.Klasifikasi} - {item.KodeDokumen} - {item.Pajak} - {item.TahunSurat}");
                        Console.ResetColor();
                    }
                }

                using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                {
                    var sql = @"
                        SELECT KLASIFIKASI, TAHUN_SURAT, PAJAK, KODE_DOKUMEN, BIDANG, AGENDA, NO_FORMULIR, NAMA, NAMA_PERUSAHAAN, TGL_AKHIR_BERLAKU, TGL_JT_TEMPO, FLAG_PERMOHONAN, EMAIL, JUMLAH_NILAI, ISI_REKLAME, ALAMATREKLAME, PANJANG, LEBAR, LS, KETINGGIAN, NAMA_JENIS, TAHUN_PAJAK, MASA1, MASA2, PAJAKLB
FROM T_SURAT_REKLAME_TEGURAN
                    ";

                    var result = await _contMonitoringDb.Set<DbMonReklameSuratTegur>().FromSqlRaw(sql).ToListAsync();

                    var source = _contMonPd.DbMonReklameSuratTegurs.ToList();
                    foreach (var item in result)
                    {
                        var row = source.FirstOrDefault(x =>
                               x.Agenda == item.Agenda
                            && x.Bidang == item.Bidang
                            && x.Klasifikasi == item.Klasifikasi
                            && x.KodeDokumen == item.KodeDokumen
                            && x.Pajak == item.Pajak
                            && x.TahunSurat == item.TahunSurat
                            );
                        if (row != null)
                        {
                            _contMonPd.DbMonReklameSuratTegurs.Remove(row);
                        }

                        _contMonPd.DbMonReklameSuratTegurs.Add(new DbMonReklameSuratTegur()
                        {
                            Klasifikasi = item.Klasifikasi,
                            TahunSurat = item.TahunSurat,
                            Pajak = item.Pajak,
                            KodeDokumen = item.KodeDokumen,
                            Bidang = item.Bidang,
                            Agenda = item.Agenda,
                            NoFormulir = item.NoFormulir,
                            Nama = item.Nama,
                            NamaPerusahaan = item.NamaPerusahaan,
                            TglAkhirBerlaku = item.TglAkhirBerlaku,
                            TglJtTempo = item.TglJtTempo,
                            FlagPermohonan = item.FlagPermohonan,
                            Email = item.Email,
                            JumlahNilai = item.JumlahNilai,
                            IsiReklame = item.IsiReklame,
                            Alamatreklame = item.Alamatreklame,
                            Panjang = item.Panjang,
                            Lebar = item.Lebar,
                            Ls = item.Ls,
                            Ketinggian = item.Ketinggian,
                            NamaJenis = item.NamaJenis,
                            TahunPajak = item.TahunPajak,
                            Masa1 = item.Masa1,
                            Masa2 = item.Masa2,
                            Pajaklb = item.Pajaklb,
                        });

                        _contMonPd.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now} DB_MON_REKLAME_SURAT_TEGUR {item.Agenda} - {item.Bidang} - {item.Klasifikasi} - {item.KodeDokumen} - {item.Pajak} - {item.TahunSurat}");
                        Console.ResetColor();
                    }
                }

                using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
                {
                    var db = getOracleConnection();
                    var sql = @"
                        SELECT KLASIFIKASI, TAHUN_SURAT, PAJAK, KODE_DOKUMEN, BIDANG, AGENDA, ISI_FILE
                        FROM T_SURAT_REKLAME_TEGURAN_FILE
                    ";
                    

                    var result = db.Query<TeguranFile>(sql).ToList();

                    var source = _contMonPd.DbMonReklameSuratTegurDoks.ToList();
                    foreach (var item in result)
                    {
                        var row = source.FirstOrDefault(x =>
                               x.Agenda == item.AGENDA
                            && x.Bidang == item.BIDANG
                            && x.Klasifikasi == item.KLASIFIKASI
                            && x.KodeDokumen == item.KODE_DOKUMEN
                            && x.Pajak == item.PAJAK
                            && x.TahunSurat == item.TAHUN_SURAT
                            );
                        if (row != null)
                        {
                            _contMonPd.DbMonReklameSuratTegurDoks.Remove(row);
                        }

                        _contMonPd.DbMonReklameSuratTegurDoks.Add(new DbMonReklameSuratTegurDok()
                        {
                            Klasifikasi = item.KLASIFIKASI,
                            TahunSurat = item.TAHUN_SURAT,
                            Pajak = item.PAJAK,
                            KodeDokumen = item.KODE_DOKUMEN,
                            Bidang = item.BIDANG,
                            Agenda = item.AGENDA,
                            IsiFile = item.ISI_FILE,
                        });

                        _contMonPd.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now} DB_MON_REKLAME_SURAT_TEGUR_DOK {item.AGENDA} - {item.BIDANG} - {item.KLASIFIKASI} - {item.KODE_DOKUMEN} - {item.PAJAK} - {item.TAHUN_SURAT}");
                        Console.ResetColor();
                    }
                }



                MailHelper.SendMail(
                false,
                "DONE REKLAME_SURAT WS",
                $@"REKLAME_SURAT WS FINISHED",
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
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPREKLAMESURAT.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPREKLAMESURAT.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }

        public class TeguranFile
        {
            public string KLASIFIKASI { get; set; } = null!;
            public int TAHUN_SURAT { get; set; }
            public int PAJAK { get; set; }
            public string KODE_DOKUMEN { get; set; } = null!;
            public string BIDANG { get; set; } = null!;
            public int AGENDA { get; set; }
            public byte[] ISI_FILE { get; set; } = null!;

        }
    }
}
