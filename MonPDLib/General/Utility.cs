using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonPDLib.General
{
    public class Utility
    {
        public const string SESSION_USER = "SESSION_USER";
        public const string SESSION_ROLE = "SESSION_ROLE";

        public class TahunModel
        {
            public int Tahun1 { get; set; }
            public int Tahun2 { get; set; }
            public int Tahun3 { get; set; }
            public int Tahun4 { get; set; }
            public int Tahun5 { get; set; }
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
