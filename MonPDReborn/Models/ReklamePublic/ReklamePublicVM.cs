using ClosedXML;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using static MonPDReborn.Models.StrukPBJT.StrukPBJTVM;

namespace MonPDReborn.Models.ReklamePublic
{
    public class ReklamePublicVM
    {
        public class Index
        {
            public string selectedUpaya { get; set; }
            public string lokasiUpaya { get; set; }
            public int Number1 { get; set; }
            public int Number2 { get; set; }
            public int CaptchaAnswer { get; set; }
            public string? RecaptchaToken { get; set; }

            public Index()
            {
                var random = new Random();
                int number1 = random.Next(1, 10);
                int number2 = random.Next(1, 10);
                int captchaAnswer = number1 + number2;

                Number1 = number1;
                Number2 = number2;
                CaptchaAnswer = captchaAnswer;
            }
        }
        public class Show
        {
            public string? RecaptchaToken { get; set; }
            public string NamaJalan { get; set; }
            public List<ReklameJalan> Data { get; set; } = new List<ReklameJalan>();
            public Show(string namaJalan, string detailLokasi)
            {
                NamaJalan = namaJalan;
                Data = Method.GetReklameJalanList(namaJalan, detailLokasi);
            }
        }

        public class Detail
        {
            public List<DetailReklame> Data { get; set; } = new List<DetailReklame>();
            public Detail(string _connectionString, string noFormulir)
            {
                Data = Method.GetDetailReklame(_connectionString, noFormulir);

                if (Data.Count == 0)
                {
                    Data = Method.GetDetailNor(_connectionString, noFormulir);
                }
            }
        }
        public class Method
        {
            public static List<ReklameJalan> GetReklameJalanList(string namaJalan, string detailLokasi)
            {
                using var context = DBClass.GetContext();
                var hariIni = DateTime.Today;

                var query = context.MvReklameSummaries
                    .Where(x => x.TglMulaiBerlaku.HasValue &&
                                x.TglAkhirBerlaku.HasValue &&
                                x.TglAkhirBerlaku.Value.Date >= hariIni);

                // Validasi: minimal salah satu harus diisi
                if (string.IsNullOrWhiteSpace(namaJalan) && string.IsNullOrWhiteSpace(detailLokasi))
                {
                    throw new Exception("Nama jalan atau detail lokasi harus diisi!");
                }

                // Filter nama jalan
                if (!string.IsNullOrWhiteSpace(namaJalan))
                {
                    query = query.Where(x =>
                        x.NamaJalan != null &&
                        x.NamaJalan.ToLower().Contains(namaJalan.ToLower())
                    );
                }

                // Filter detail lokasi (pakai nama depan sebelum koma)
                if (!string.IsNullOrWhiteSpace(detailLokasi))
                {
                    query = query.Where(x =>
                        x.DetailLokasi != null &&
                        (
                            (x.DetailLokasi.Contains(",")
                                ? x.DetailLokasi.Substring(0, x.DetailLokasi.IndexOf(","))
                                : x.DetailLokasi
                            ).ToLower().Contains(detailLokasi.ToLower())
                        )
                    );
                }

                var result = query
                    .Select(x => new ReklameJalan
                    {
                        NoFormulir = x.NoFormulir ?? "-",
                        JenisReklame = x.FlagPermohonan ?? "-",
                        Kategori = x.NmJenis ?? "-",
                        Jumlah = x.Jumlah ?? 0,
                        Jalan = x.NamaJalan ?? "-",
                        Alamat = x.Alamatreklame ?? "-",
                        // tampilkan lengkap, bukan nama depan
                        DetailLokasi = x.DetailLokasi ?? "-",
                        IsiReklame = x.IsiReklame ?? "-",
                        tglMulai = x.TglMulaiBerlaku ?? DateTime.MinValue,
                        tglAkhir = x.TglAkhirBerlaku ?? DateTime.MinValue
                    })
                    .ToList();

                return result;
            }
            public static List<DetailReklame> GetDetailReklame(string _connectionString, string noFormulir)
            {
                var connection = new OracleConnection(_connectionString);
                connection.Open();
                string query = @"SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, SURVEY_KE, NO_OBYEK_REKLAME
                    FROM (
                        SELECT 
                            NO_FORMULIR,
                            FOTO,
                            STATUS_VERIFIKASI,
                            F.SURVEY_KE,
                            F.NO_OBYEK_REKLAME,
                            ROW_NUMBER() OVER (PARTITION BY NO_FORMULIR, NO_OBYEK_REKLAME ORDER BY F.SURVEY_KE DESC) AS RN
                        FROM (
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME 
                            FROM SURVEYAPP.FOTOSURVEY F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI 
                            WHERE S.STATUS_VERIFIKASI = 1 AND NO_FORMULIR = :NO_FORMULIR
                            UNION ALL  
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME  
                            FROM SURVEYAPP.FOTOSURVEY_I F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI 
                            WHERE S.STATUS_VERIFIKASI = 1 AND NO_FORMULIR = :NO_FORMULIR
                            UNION ALL  
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME  
                            FROM SURVEYAPP.FOTOSURVEY_K F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI 
                            WHERE S.STATUS_VERIFIKASI = 1 AND NO_FORMULIR = :NO_FORMULIR
                            UNION ALL  
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME  
                            FROM SURVEYAPP.FOTOSURVEY_L F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI 
                            WHERE S.STATUS_VERIFIKASI = 1 AND NO_FORMULIR = :NO_FORMULIR
                        ) F
                    )
                    WHERE RN = 1";

                return connection.Query<DetailReklame>(query, new { NO_FORMULIR = noFormulir }).ToList();
            }
            public static List<DetailReklame> GetDetailNor(string _connectionString, string noFormulir)
            {
                var connection = new OracleConnection(_connectionString);
                connection.Open();
                string query = @"SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, SURVEY_KE, NO_OBYEK_REKLAME
                    FROM (
                        SELECT 
                            NO_FORMULIR,
                            FOTO,
                            STATUS_VERIFIKASI,
                            F.SURVEY_KE,
                            F.NO_OBYEK_REKLAME,
                            ROW_NUMBER() OVER (PARTITION BY NO_OBYEK_REKLAME ORDER BY F.SURVEY_KE DESC) AS RN
                        FROM (                
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME 
                            FROM SURVEYAPP.FOTOSURVEY F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI
                            WHERE S.STATUS_VERIFIKASI = 1 AND S.NO_OBYEK_REKLAME IN (SELECT NO_OBYEK_REKLAME_MOHON FROM SURVEYAPP.DATAPERMOHONAN WHERE NO_FORMULIR= :NO_FORMULIR)
                            UNION ALL  
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME  
                            FROM SURVEYAPP.FOTOSURVEY_I F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI
                            WHERE S.STATUS_VERIFIKASI = 1 AND S.NO_OBYEK_REKLAME IN (SELECT NO_OBYEK_REKLAME_MOHON FROM SURVEYAPP.DATAPERMOHONAN WHERE NO_FORMULIR= :NO_FORMULIR)
                            UNION ALL  
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME  
                            FROM SURVEYAPP.FOTOSURVEY_K F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI
                            WHERE S.STATUS_VERIFIKASI = 1 AND S.NO_OBYEK_REKLAME IN (SELECT NO_OBYEK_REKLAME_MOHON FROM SURVEYAPP.DATAPERMOHONAN WHERE NO_FORMULIR= :NO_FORMULIR)
                            UNION ALL  
                            SELECT NO_FORMULIR, FOTO, STATUS_VERIFIKASI, F.SURVEY_KE, F.NO_OBYEK_REKLAME  
                            FROM SURVEYAPP.FOTOSURVEY_L F 
                            LEFT JOIN SURVEYAPP.DATASURVEY S ON S.NO_TRANSAKSI = F.NO_TRANSAKSI
                            WHERE S.STATUS_VERIFIKASI = 1 AND S.NO_OBYEK_REKLAME IN (SELECT NO_OBYEK_REKLAME_MOHON FROM SURVEYAPP.DATAPERMOHONAN WHERE NO_FORMULIR= :NO_FORMULIR)                            
                        ) F    ORDER BY F.SURVEY_KE DESC   
                    )
                    WHERE RN = 1";

                return connection.Query<DetailReklame>(query, new { NO_FORMULIR = noFormulir }).ToList();
            }

        }

        public class namaJalanView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }

        public class lokasiReklameView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }

        public class ReklameJalan
        {
            public string NoFormulir { get; set; } = null!;
            public string NOR { get; set; } = null!;
            public string Jalan { get; set; } = null!;
            public string Alamat { get; set; } = null!;
            public string JenisReklame { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string DetailLokasi { get; set; } = null!;
            public string Kategori { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime tglMulai { get; set; }
            public DateTime tglAkhir { get; set; }
            public string Lampiran { get; set; } = null!;
            public string TanggalTayang => string.Concat(tglMulai.ToString("dd MMM yyyy", new CultureInfo("id-ID")), " - ", tglAkhir.ToString("dd MMM yyyy", new CultureInfo("id-ID")));
            public decimal Jumlah { get; set; }
        }
        public class DetailReklame
        {
            public string NO_FORMULIR { get; set; }
            public byte[] FOTO { get; set; }
            public int STATUS_VERIFIKASI { get; set; }
            public int SURVEY_KE { get; set; }
            public string NO_OBYEK_REKLAME { get; set; }
        }

    }
}
