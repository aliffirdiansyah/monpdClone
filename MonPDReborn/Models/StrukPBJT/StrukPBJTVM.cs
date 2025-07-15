using Microsoft.AspNetCore.Mvc;
using static MonPDReborn.Models.DataOP.ProfileTargetOPVM;

namespace MonPDReborn.Models.StrukPBJT
{
    public class StrukPBJTVM
    {
        public class Index
        {
            public string Keyword { get; set; } = string.Empty;
            public Index(string keyword)
            {
                Keyword = keyword;
            }
        }
        public class Show
        {
            public List<RekapStruk> DataRekapStrukList { get; set; } = new();
            public Show(string noStruk)
            {
                DataRekapStrukList = Method.GetRekapStrukList(noStruk);
            }
        }
        /* public class Detail
         {
             public List<RealisasiBulanan> DataRealisasiBulananList { get; set; } = new();
             public Detail()
             {

             }
             public Detail(string nop)
             {
                 DataRealisasiBulananList = Method.GetDetailByNOP(nop);
             }
         }*/
        public class Method
        {
            public static List<RekapStruk> GetRekapStrukList(string noStruk)
            {
                var allData = GetAllData();

                if (string.IsNullOrWhiteSpace(noStruk))
                    return allData;

                return allData
                    .Where(d => d.NoStruk != null && d.NoStruk.Contains(noStruk, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private static List<RekapStruk> GetAllData()
            {
                return new List<RekapStruk>
                {
                    new RekapStruk {NoStruk = "STRK-20250714001", NamaObjek = "Toko Maju Jaya",AlamatObjek = "Jl. Merdeka No. 123, Bandung", NilaiTransaksi = 1_250_000m, Status = "Terekam"},
                    new RekapStruk {NoStruk = "STRK-20250714001", NamaObjek = "Toko Maju Jalan",AlamatObjek = "Jl. Merdeka No. 123, Bandung", NilaiTransaksi = 1_300_000m, Status = "Terekam"},
                    new RekapStruk {NoStruk = "STRK-20220", NamaObjek = "Toko Maju Jalan",AlamatObjek = "Jl. Merdeka No. 123, Bandung", NilaiTransaksi = 1_300_000m, Status = "Terekam"},

                };
            }
        }

        public class RekapStruk
        {
            public string NoStruk { get; set; } = null!;
            public string NamaObjek { get; set; } = null!;
            public string AlamatObjek { get; set; } = null!;
            public decimal NilaiTransaksi { get; set; }
            public string Status {  get; set; } = null!;
        }
    }
}

