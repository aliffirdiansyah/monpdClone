using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CctvRealtimeWs
{
    public class DataCctv
    {
        public class DataOpCctv
        {
            public string Nop { get; set; } = "";
            public string NamaOp { get; set; } = "";
            public string AlamatOp { get; set; } = "";
            public int WilayahPajak { get; set; }
            public EnumFactory.EVendorParkirCCTV Vendor { get; set; }
            public string? AccessPoint { get; set; }
            public string? CctvId { get; set; }
            public string? DisplayId { get; set; }
        }


        public static async Task<List<DataOpCctv>> GetDataOpCctvAsync()
        {
            using var context = DBClass.GetContext();

            var result = new List<DataOpCctv>();

            //// Gunakan ToListAsync agar EF Core menjalankan query secara async
            var query = await context.MOpParkirCctvs
                .Include(x => x.MOpParkirCctvTelkoms)
                .Include(x => x.MOpParkirCctvJasnita)
                .Where(x => x.IsPasang == 1)
                .ToListAsync();

            //for dev
            //var query = await context.MOpParkirCctvs
            //    .Include(x => x.MOpParkirCctvTelkoms)
            //    .Include(x => x.MOpParkirCctvJasnita)
            //    .Where(x => x.IsPasang == 1 && x.Vendor == (int)EnumFactory.EVendorParkirCCTV.Jasnita)
            //    .Take(1)
            //    .ToListAsync();

            foreach (var item in query)
            {
                if (item.Vendor == (int)EnumFactory.EVendorParkirCCTV.Telkom)
                {
                    foreach (var cctv in item.MOpParkirCctvTelkoms)
                    {
                        result.Add(new DataOpCctv
                        {
                            Nop = item.Nop,
                            NamaOp = item.NamaOp,
                            AlamatOp = item.AlamatOp,
                            WilayahPajak = item.WilayahPajak,
                            Vendor = (EnumFactory.EVendorParkirCCTV)item.Vendor,
                            AccessPoint = "",
                            CctvId = cctv.CctvId
                        });
                    }
                }
                else if (item.Vendor == (int)EnumFactory.EVendorParkirCCTV.Jasnita)
                {
                    foreach (var cctv in item.MOpParkirCctvJasnita)
                    {
                        result.Add(new DataOpCctv
                        {
                            Nop = item.Nop,
                            NamaOp = item.NamaOp,
                            AlamatOp = item.AlamatOp,
                            WilayahPajak = item.WilayahPajak,
                            Vendor = (EnumFactory.EVendorParkirCCTV)item.Vendor,
                            AccessPoint = cctv.AccessPoint,
                            CctvId = cctv.CctvId,
                            DisplayId = cctv.DisplayId
                        });
                    }
                }
            }

            return result;
        }
    }
}
