using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.EF;

namespace MonPDReborn.Models.DataOP
{
    public class PotensiUploadVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public IFormFile Lampiran { get; set; } = null!;
            public Index()
            {

            }

        }

        public class  Method
        {
            public static void SimpanLampiranExcel(IFormFile fileExcel)
            {
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel tidak boleh kosong.");

                var context = DBClass.GetContext();

                // Konversi file ke byte[]
                byte[] excelData;
                using (var ms = new MemoryStream())
                {
                    fileExcel.CopyTo(ms);
                    excelData = ms.ToArray();
                }

                // Simpan ke entitas (gunakan nilai default atau placeholder jika perlu)
                var newUpaya = new DbMonReklameUpaya
                {
                    Upaya = "UPLOAD EXCEL",
                    Keterangan = "Lampiran file Excel",
                  
                    DbMonReklameUpayaDok = new DbMonReklameUpayaDok
                    {
                        Gambar = excelData
                    }
                };

                context.DbMonReklameUpayas.Add(newUpaya);
                context.SaveChanges();
            }

        }

        public class Upload
        {
            public byte[] lampiran { get; set; } = null!;
        }
    }
}
