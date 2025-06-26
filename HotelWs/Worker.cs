using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;

namespace HotelWs
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
                    await DoWorkFullScanAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing task.");
                }
            }
        }

        private async Task DoWorkFullScanAsync(CancellationToken stoppingToken)
        {
            int idPajak = 3;
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


            //FILL DB OP
            if (IsGetDBOp())
            {
                using (var _contSbyTax = DBClass.GetSurabayaTaxContext())
                {
                    var sql = @"
                        SELECT  A.NOP,
                                C.NPWPD_NO NPWPD,
                                C.NAMA NPWPD_NAMA,
                                C.ALAMAT NPWPD_ALAMAT,
                                A.PAJAK_ID ,
                                'PAJAK JASA PERHOTELAN' PAJAK_NAMA,
                                A.NAMA NAMA_OP,
                                A.ALAMAT ALAMAT_OP,
                                A.ALAMAT_NO ALAMAT_OP_NO,
                                A.RT ALAMAT_OP_RT,
                                A.RW ALAMAT_OP_RW,
                                A.TELP,
                                A.KD_LURAH ALAMAT_OP_KD_LURAH,
                                A.KD_CAMAT ALAMAT_OP_KD_CAMAT,
                                TGL_OP_TUTUP,
                                TGL_MULAI_BUKA_OP,
                                D.ID  KATEGORI_ID,
                                D.NAMA KATEGORI_NAMA,
                                sysdate INS_dATE, 
                                'JOB' INS_BY,
                                TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')) TAHUN_BUKU,
                                '-'  AKUN  ,
                                '-'  NAMA_AKUN         ,
                                '-'  KELOMPOK      ,
                                '-'  NAMA_KELOMPOK     ,
                                '-'  JENIS             ,
                                '-'  NAMA_JENIS        ,
                                '-'  OBJEK            ,
                                '-'  NAMA_OBJEK       ,
                                '-'  RINCIAN         ,
                                '-'  NAMA_RINCIAN     ,
                                '-'  SUB_RINCIAN      ,
                                '-'  NAMA_SUB_RINCIAN    
                        FROM OBJEK_PAJAK A
                        JOIN OBJEK_PAJAK_HOTEL B ON A.NOP = B.NOP
                        JOIN NPWPD C ON A.NPWPD = C.NPWPD_no
                        JOIN M_KATEGORI_PAJAK D ON D.ID = A.KATEGORI";

                    var result = await _contSbyTax.Set<DbOpHotel>().FromSqlRaw(sql).ToListAsync(); //822
                    for (var i = tahunAmbil; i <= tglServer.Year; i++)
                    {
                        var source = await _contMonPd.DbOpHotels.Where(x => x.TahunBuku == i).ToListAsync();
                        foreach (var item in result)
                        {
                            if (item.TglMulaiBukaOp.Year <= i)
                            {
                                var sourceRow = source.SingleOrDefault(x => x.Nop == item.Nop);
                                if (sourceRow != null)
                                {

                                    sourceRow.TglOpTutup = item.TglOpTutup;
                                    sourceRow.TglMulaiBukaOp = item.TglMulaiBukaOp;

                                    var dbakun = GetDbAkun(i, idPajak, (int)item.KategoriId);
                                    if (dbakun != null)
                                    {
                                        sourceRow.Akun = dbakun.Akun;
                                        sourceRow.NamaAkun = dbakun.NamaAkun;
                                        sourceRow.Kelompok = dbakun.Kelompok;
                                        sourceRow.NamaKelompok = dbakun.NamaKelompok;
                                        sourceRow.Jenis = dbakun.Jenis;
                                        sourceRow.NamaJenis = dbakun.NamaJenis;
                                        sourceRow.Objek = dbakun.Objek;
                                        sourceRow.NamaObjek = dbakun.NamaObjek;
                                        sourceRow.Rincian = dbakun.Rincian;
                                        sourceRow.NamaRincian = dbakun.NamaRincian;
                                        sourceRow.SubRincian = dbakun.SubRincian;
                                        sourceRow.NamaSubRincian = dbakun.NamaSubRincian;
                                    }
                                    else
                                    {
                                        sourceRow.Akun = item.Akun;
                                        sourceRow.NamaAkun = item.NamaAkun;
                                        sourceRow.Kelompok = item.Kelompok;
                                        sourceRow.NamaKelompok = item.NamaKelompok;
                                        sourceRow.Jenis = item.Jenis;
                                        sourceRow.NamaJenis = item.NamaJenis;
                                        sourceRow.Objek = item.Objek;
                                        sourceRow.NamaObjek = item.NamaObjek;
                                        sourceRow.Rincian = item.Rincian;
                                        sourceRow.NamaRincian = item.NamaRincian;
                                        sourceRow.SubRincian = item.SubRincian;
                                        sourceRow.NamaSubRincian = item.NamaSubRincian;
                                    }
                                }
                                else
                                {
                                    var newRow = new MonPDLib.EF.DbOpAbt();
                                    newRow.Nop = item.Nop;
                                    newRow.Npwpd = item.Npwpd;
                                    newRow.NpwpdNama = item.NpwpdNama;
                                    newRow.NpwpdAlamat = item.NpwpdAlamat;
                                    newRow.PajakId = item.PajakId;
                                    newRow.PajakNama = item.PajakNama;
                                    newRow.NamaOp = item.NamaOp;
                                    newRow.AlamatOp = item.AlamatOp;
                                    newRow.AlamatOpNo = item.AlamatOpNo;
                                    newRow.AlamatOpRt = item.AlamatOpRt;
                                    newRow.AlamatOpRw = item.AlamatOpRw;
                                    newRow.Telp = item.Telp;
                                    newRow.AlamatOpKdLurah = item.AlamatOpKdLurah;
                                    newRow.AlamatOpKdCamat = item.AlamatOpKdCamat;
                                    newRow.TglOpTutup = item.TglOpTutup;
                                    newRow.TglMulaiBukaOp = item.TglMulaiBukaOp;
                                    newRow.KategoriId = item.KategoriId;
                                    newRow.KategoriNama = item.KategoriNama;
                                    newRow.JumlahKaryawan = item.JumlahKaryawan;
                                    newRow.InsDate = item.InsDate;
                                    newRow.InsBy = item.InsBy;

                                    newRow.TahunBuku = i;
                                    var dbakun = GetDbAkun(i, idPajak, (int)item.KategoriId);
                                    if (dbakun != null)
                                    {
                                        newRow.Akun = dbakun.Akun;
                                        newRow.NamaAkun = dbakun.NamaAkun;
                                        newRow.Kelompok = dbakun.Kelompok;
                                        newRow.NamaKelompok = dbakun.NamaKelompok;
                                        newRow.Jenis = dbakun.Jenis;
                                        newRow.NamaJenis = dbakun.NamaJenis;
                                        newRow.Objek = dbakun.Objek;
                                        newRow.NamaObjek = dbakun.NamaObjek;
                                        newRow.Rincian = dbakun.Rincian;
                                        newRow.NamaRincian = dbakun.NamaRincian;
                                        newRow.SubRincian = dbakun.SubRincian;
                                        newRow.NamaSubRincian = dbakun.NamaSubRincian;
                                    }
                                    else
                                    {
                                        newRow.Akun = item.Akun;
                                        newRow.NamaAkun = item.NamaAkun;
                                        newRow.Kelompok = item.Kelompok;
                                        newRow.NamaKelompok = item.NamaKelompok;
                                        newRow.Jenis = item.Jenis;
                                        newRow.NamaJenis = item.NamaJenis;
                                        newRow.Objek = item.Objek;
                                        newRow.NamaObjek = item.NamaObjek;
                                        newRow.Rincian = item.Rincian;
                                        newRow.NamaRincian = item.NamaRincian;
                                        newRow.SubRincian = item.SubRincian;
                                        newRow.NamaSubRincian = item.NamaSubRincian;
                                    }
                                    _contMonPd.DbOpAbts.Add(newRow);
                                }

                                Console.WriteLine($"DB_OP {item.Nop}");
                                _contMonPd.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        private bool IsGetDBOp()
        {
            var _contMonPd = DBClass.GetContext();
            var row = _contMonPd.SetLastRuns.FirstOrDefault(x => x.Job.ToUpper() == EnumFactory.EJobName.DBOPHOTEL.ToString().ToUpper());
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
            newRow.Job = EnumFactory.EJobName.DBOPHOTEL.ToString().ToUpper();
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
