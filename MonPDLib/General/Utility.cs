using Microsoft.EntityFrameworkCore;
using MonPDLib.EF;
using System;
using System.Collections.Generic;
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
    }


}
