using Microsoft.EntityFrameworkCore;
using MonPDLib.EF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static MonPDLib.General.EnumFactory;

namespace MonPDLib.General
{
    public class Utility
    {
        public const string SESSION_USER = "SESSION_USER";
        public const string SESSION_NAMA = "SESSION_NAMA";
        public const string SESSION_ROLE = "SESSION_ROLE";

        public class TahunModel
        {
            public int Tahun1 { get; set; }
            public int Tahun2 { get; set; }
            public int Tahun3 { get; set; }
            public int Tahun4 { get; set; }
            public int Tahun5 { get; set; }
        }

        // Mapping dari EPajakBlok ke EPajak
        private static readonly Dictionary<EPajakBlok, EPajak> _map = new()
        {
            { EPajakBlok.JasaPerhotelan, EPajak.JasaPerhotelan },
            { EPajakBlok.MakananMinuman, EPajak.MakananMinuman },
            { EPajakBlok.JasaKesenianHiburan, EPajak.JasaKesenianHiburan },
            { EPajakBlok.TenagaListrik, EPajak.TenagaListrik },
            { EPajakBlok.JasaParkir, EPajak.JasaParkir },
            { EPajakBlok.AirTanah, EPajak.AirTanah }
        };
        public static EPajak GetJenisPajakFromNop(string nop)
        {
            if (string.IsNullOrWhiteSpace(nop))
                return EPajak.Semua;

            // Pecah NOP berdasarkan titik
            var parts = nop.Split('.');

            if (parts.Length < 5)
                return EPajak.Semua;

            // Ambil blok (posisi ke-4, misalnya "902")
            if (int.TryParse(parts[4], out int blokCode))
            {
                if (Enum.IsDefined(typeof(EPajakBlok), blokCode))
                {
                    var blok = (EPajakBlok)blokCode;
                    if (_map.ContainsKey(blok))
                    {
                        return _map[blok];
                    }
                }
            }

            return EPajak.PBB; // fallback default
        }
        public static TahunModel Generate5TahunKebelakang()
        {
            var tahunList = Enumerable.Range(DateTime.Now.Year - 4, 5).Reverse().ToArray();

            return new TahunModel
            {
                Tahun1 = tahunList[0],
                Tahun2 = tahunList[1],
                Tahun3 = tahunList[2],
                Tahun4 = tahunList[3],
                Tahun5 = tahunList[4],
            };
        }
        public static string GetFormattedNOP(string NOP)
        {
            try
            {
                //35 78 010 001 902 00001
                var prop = NOP.Substring(0, 2);
                var dati = NOP.Substring(2, 2);
                var kec = NOP.Substring(4, 3);
                var kel = NOP.Substring(7, 3);
                var jnspajak = NOP.Substring(10, 3);
                var seq = NOP.Substring(13, 5);
                return prop + "." + dati + "." + kec + "." + kel + "." + jnspajak + "." + seq;
            }
            catch
            {
                return NOP;
            }
        }
        public static string GetFormattedNOPPBB(string NOP)
        {
            try
            {
                // Format NOP PBB : PP.KK.KEC.KEL.BLOK.URUT.JNS
                var prop = NOP.Substring(0, 2);    // Provinsi
                var kab = NOP.Substring(2, 2);     // Kabupaten/Kota
                var kec = NOP.Substring(4, 3);     // Kecamatan
                var kel = NOP.Substring(7, 3);     // Kelurahan
                var blok = NOP.Substring(10, 3);   // Blok
                var urut = NOP.Substring(13, 4);   // Nomor Urut
                var jns = NOP.Substring(17, 1);    // Jenis OP

                return $"{prop}.{kab}.{kec}.{kel}.{blok}.{urut}.{jns}";
            }
            catch
            {
                return NOP;
            }
        }

        public static int GetMaxValueSpecifyColumn<T>(DbContext ctx, Expression<Func<T, bool>>? expression, string propertyNameEntityEf = "Id")
        where T : class
        {
            var query = ctx.Set<T>().AsQueryable();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            var propertyinfo = typeof(T).GetProperty(propertyNameEntityEf);
            if (propertyinfo == null)
            {
                throw new ArgumentException($"Property '{propertyNameEntityEf}' not found in entity '{typeof(T).Name}'.");
            }
            var maxEntity = query.Select(entity => new { Value = propertyinfo.GetValue(entity) })
                     .AsEnumerable() // Switch to client-side evaluation
                     .Max(x => (int?)x.Value);
            return maxEntity ?? 0;
        }
        public class JenisFile
        {
            public ETipeFile Jenis { get; set; }
            public string ContentType { get; set; } = "";
            public string FileExtension { get; set; } = "";
        }
        public static EnumFactory.ETipeFile GetTipeFilebyContent(string content)
        {
            switch (content)
            {
                case "application/pdf":
                    return EnumFactory.ETipeFile.PDF;
                //case "image/png":
                //    return EnumFactory.ETipeFile.Image;

                //Added BIMOUW
                case "image/png":
                case "image/jpeg":
                case "image/gif":
                case "image/bmp":
                case "image/tiff":
                    return EnumFactory.ETipeFile.Image;
                //End Added BIMOUW

                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return EnumFactory.ETipeFile.Excel;
                case "application/x-msdownload":
                    return EnumFactory.ETipeFile.App;
                case "application/x-zip-compressed":
                    return EnumFactory.ETipeFile.Zip;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return EnumFactory.ETipeFile.Word;
                default:
                    return EnumFactory.ETipeFile.Unknown;
            }
            //application/pdf
            //image/png
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            //application/x-msdownload  - dll / exe
            //application/x-zip-compressed  - 
            //application/vnd.openxmlformats-officedocument.wordprocessingml.document
        }
        public static JenisFile GetJenisFile(byte[] bytes)
        {
            JenisFile jenis = new JenisFile();
            if (bytes == null)
            {
                jenis.Jenis = ETipeFile.Unknown;
                jenis.ContentType = "text/html";
                jenis.FileExtension = ".txt";
            }
            else
            {
                if (bytes.Length >= 8 &&
                    bytes[0] == 137 && bytes[1] == 80 && bytes[2] == 78 && bytes[3] == 71 &&
                    bytes[4] == 13 && bytes[5] == 10 && bytes[6] == 26 && bytes[7] == 10)
                {
                    // Set the content type and file extension for a PNG file
                    jenis.Jenis = ETipeFile.Image;
                    jenis.ContentType = "image/png";
                    jenis.FileExtension = ".png";
                }
                else if (bytes.Length > 4 && Encoding.ASCII.GetString(bytes.Take(5).ToArray()) == "%PDF-")
                {
                    // Set the content type and file extension for a PDF file
                    jenis.Jenis = ETipeFile.PDF;
                    jenis.ContentType = "application/pdf";
                    jenis.FileExtension = ".pdf";
                }
                else if (bytes.Length > 2 && bytes[0] == 0xFF && bytes[1] == 0xD8)
                {
                    // Set the content type and file extension for a JPEG image
                    jenis.Jenis = ETipeFile.Image;
                    jenis.ContentType = "image/jpeg";
                    jenis.FileExtension = ".jpg";
                }
                else
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(bytes))
                            Image.FromStream(ms);
                        jenis.Jenis = ETipeFile.Image;
                        jenis.ContentType = "image/jpeg";
                        jenis.FileExtension = ".jpg";
                    }
                    catch (ArgumentException)
                    {
                        jenis.Jenis = ETipeFile.Unknown;
                        jenis.ContentType = "text/html";
                        jenis.FileExtension = ".txt";
                    }

                }
            }

            return jenis;
        }
    }


}
