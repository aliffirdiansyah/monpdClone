using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using static MonPDLib.Helper;

namespace OpsenWs
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
                    "ERROR OPSEN WS",
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
            int idPajak = 20;
            var tglServer = DateTime.Now;
            var _contMonPd = DBClass.GetContext();
            int tahunAmbil = tglServer.Year;
            var thnSetting = _contMonPd.SetYearJobScans.SingleOrDefault(x => x.IdPajak == idPajak);
            if (thnSetting != null)
            {
                var temp = tglServer.Year - (int)thnSetting.YearBefore;
                if (temp >= 2023)
                {
                    tahunAmbil = temp;
                }
                else
                {
                    tahunAmbil = 2023;
                }
            }


            using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
            {
                var sql = @"
                        SELECT 	ID_SSPD, 
		                        TGL_SSPD, 
		                        SSPD_TGL_ENTRY, 
		                        ID_AYAT_PAJAK, 
		                        BULAN_PAJAK_SSPD, 
		                        TAHUN_PAJAK_SSPD, 
		                        JML_POKOK, 
		                        JML_DENDA, 
		                        REFF_DASAR_SETORAN, 
		                        TEMPAT_BAYAR, 
		                        SETORAN_BERDASARKAN, 
		                        REKON_DATE, 
		                        REKON_BY, 
		                        DASAR_SETORAN, 
		                        NAMA_JENIS_PAJAK, 
		                        DESCRIPTION, 
		                        SAMSAT_ASAL, 
		                        JENIS_BAYAR
                        FROM T_SSPD_OPSEN_PKB A
                    ";

                var result = await _contMonitoringDb.Set<OpOpsenSkpdPkb>().FromSqlRaw(sql).ToListAsync();

                var dbMonOpsenPkb = _contMonPd.DbMonOpsenPkbs.ToList();
                foreach (var item in result)
                {
                    var rowMonOpsenPkb = dbMonOpsenPkb.FirstOrDefault(x => x.IdSspd == item.ID_SSPD);
                    if (rowMonOpsenPkb != null)
                    {
                        _contMonPd.DbMonOpsenPkbs.Remove(rowMonOpsenPkb);
                    }

                    _contMonPd.DbMonOpsenPkbs.Add(new DbMonOpsenPkb()
                    {
                        IdSspd = item.ID_SSPD,
                        TglSspd = item.TGL_SSPD,
                        SspdTglEntry = item.SSPD_TGL_ENTRY,
                        IdAyatPajak = item.ID_AYAT_PAJAK,
                        BulanPajakSspd = item.BULAN_PAJAK_SSPD,
                        TahunPajakSspd = item.TAHUN_PAJAK_SSPD,
                        JmlPokok = item.JML_POKOK,
                        JmlDenda = item.JML_DENDA,
                        ReffDasarSetoran = item.REFF_DASAR_SETORAN,
                        TempatBayar = item.TEMPAT_BAYAR,
                        SetoranBerdasarkan = item.SETORAN_BERDASARKAN,
                        RekonDate = item.REKON_DATE,
                        RekonBy = item.REKON_BY,
                        DasarSetoran = item.DASAR_SETORAN,
                        NamaJenisPajak = item.NAMA_JENIS_PAJAK,
                        Description = item.DESCRIPTION,
                        SamsatAsal = item.SAMSAT_ASAL,
                        JenisBayar = item.JENIS_BAYAR,

                    });

                    _contMonPd.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} DB_MON_OPSEN_PKB_MONITORINGDB {item.ID_SSPD}");
                    Console.ResetColor();
                }
            }

            using (var _contMonitoringDb = DBClass.GetMonitoringDbContext())
            {
                var sql = @"
                        SELECT 	ID_SSPD, 
		                        TGL_SSPD, 
		                        SSPD_TGL_ENTRY, 
		                        ID_AYAT_PAJAK, 
		                        BULAN_PAJAK_SSPD, 
		                        TAHUN_PAJAK_SSPD, 
		                        JML_POKOK, 
		                        JML_DENDA, 
		                        REFF_DASAR_SETORAN, 
		                        TEMPAT_BAYAR, 
		                        SETORAN_BERDASARKAN, 
		                        REKON_DATE, 
		                        REKON_BY, 
		                        DASAR_SETORAN, 
		                        NAMA_JENIS_PAJAK, 
		                        DESCRIPTION, 
		                        SAMSAT_ASAL, 
		                        JENIS_BAYAR
                        FROM T_SSPD_OPSEN_BBNKB B
                    ";

                var result = await _contMonitoringDb.Set<OpOpsenSkpdBbnkb>().FromSqlRaw(sql).ToListAsync();

                var dbMonOpsenBbnkb = _contMonPd.DbMonOpsenBbnkbs.ToList();
                foreach (var item in result)
                {
                    var rowMonOpsenBbnkb = dbMonOpsenBbnkb.FirstOrDefault(x => x.IdSspd == item.ID_SSPD);
                    if (rowMonOpsenBbnkb != null)
                    {
                        _contMonPd.DbMonOpsenBbnkbs.Remove(rowMonOpsenBbnkb);
                    }

                    _contMonPd.DbMonOpsenBbnkbs.Add(new DbMonOpsenBbnkb()
                    {
                        IdSspd = item.ID_SSPD,
                        TglSspd = item.TGL_SSPD,
                        SspdTglEntry = item.SSPD_TGL_ENTRY,
                        IdAyatPajak = item.ID_AYAT_PAJAK,
                        BulanPajakSspd = item.BULAN_PAJAK_SSPD,
                        TahunPajakSspd = item.TAHUN_PAJAK_SSPD,
                        JmlPokok = item.JML_POKOK,
                        JmlDenda = item.JML_DENDA,
                        ReffDasarSetoran = item.REFF_DASAR_SETORAN,
                        TempatBayar = item.TEMPAT_BAYAR,
                        SetoranBerdasarkan = item.SETORAN_BERDASARKAN,
                        RekonDate = item.REKON_DATE,
                        RekonBy = item.REKON_BY,
                        DasarSetoran = item.DASAR_SETORAN,
                        NamaJenisPajak = item.NAMA_JENIS_PAJAK,
                        Description = item.DESCRIPTION,
                        SamsatAsal = item.SAMSAT_ASAL,
                        JenisBayar = item.JENIS_BAYAR,

                    });

                    _contMonPd.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{DateTime.Now} DB_MON_OPSEN_BBNKB_MONITORINGDB {item.ID_SSPD}");
                    Console.ResetColor();
                }
            }

            MailHelper.SendMail(
            false,
            "DONE OPSEN WS",
            $@"OPSEN WS FINISHED",
            null
            );
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPOPSEN.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPOPSEN.ToString().ToUpper();
            newRow.InsDate = DateTime.Now;
            _contMonPd.SetLastRuns.Add(newRow);
            _contMonPd.SaveChanges();
            return true;
        }
        private Helper.DbAkun? GetDbAkun(int tahun, int idPajak, int idKategori)
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
        private Helper.DbAkun GetDbAkunKenaikan(int tahun, int idPajak, int idKategori)
        {
            var _contMonPd = DBClass.GetContext();
            var query = _contMonPd.DbAkuns.Include(x => x.KategoriKenaikans).Where(x => x.TahunBuku == tahun).ToList();
            var row = query.FirstOrDefault(x => x.KategoriKenaikans.Any(y => y.PajakId == idPajak && y.Id == idKategori));
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
    }
}
