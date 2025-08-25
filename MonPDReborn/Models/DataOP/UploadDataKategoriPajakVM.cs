using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using OfficeOpenXml;
using static MonPDLib.General.EnumFactory;
using static MonPDReborn.Models.DataOP.UploadDataKategoriPajakVM.ViewModel;

namespace MonPDReborn.Models.DataOP
{
    public class UploadDataKategoriPajakVM
    {
        public class Index
        {
            public IFormFile? FileExcel { get; set; }
            public List<PajakViewModel> PajakList { get; set; } = new();
            public Index()
            {
                var context = DBClass.GetContext();
                PajakList = context.MPajaks
                    .Include(x => x.MKategoriPajaks)
                    .Where(x => x.Id > 0)
                    .Select(x => new PajakViewModel() 
                    { 
                        Id = x.Id, 
                        Nama = x.Nama, 
                        KategoriPajakList = x.MKategoriPajaks
                            .Where(k => k.Id > 0)
                            .Select(k => new KategoriPajakViewModel() 
                            { 
                                Id = Convert.ToInt32(k.Id), 
                                Nama = k.Nama 
                            })
                            .ToList()
                    })
                    .ToList();
            }
        }

        public class Show
        {
            public List<ObjekPajakViewModel> Data { get; set; } = new();
            public Show()
            {
                
            }
            public Show(IFormFile? fileExcel)
            {
                if (fileExcel == null)
                {
                    throw new ArgumentException("Excel Harap Diisi");
                }

                Data = Method.GetUploadKategoriOp(fileExcel);
            }
        }

        public class Method
        {
            public static List<ObjekPajakViewModel> GetUploadKategoriOp(IFormFile fileExcel)
            {
                var result = new List<ObjekPajakViewModel>();
                if (fileExcel == null || fileExcel.Length == 0)
                    throw new ArgumentException("File Excel kosong.");

                using var stream = new MemoryStream();
                fileExcel.CopyTo(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null)
                    throw new ArgumentException("Tidak ada sheet di file Excel.");
                if (sheet == null)
                    throw new Exception("Sheet1 tidak ditemukan.");

                using (var context = DBClass.GetContext())
                {
                    var pajakList = context.MPajaks
                        .Where(x => x.Id > 0)
                        .Include(x => x.MKategoriPajaks)
                        .AsQueryable();

                    int rowNum = 1;
                    for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                    {
                        int pajakId = (sheet.Cells[row, 2].GetValue<int?>() ?? 0);
                        int pajakKategoriId = (sheet.Cells[row, 3].GetValue<int?>() ?? 0);
                        string nop = (sheet.Cells[row, 1].GetValue<string?>() ?? "").Replace(".", "").Trim();

                        bool isValid = false;
                        string isValidDescription = "";
                        string namaop = "";
                        int idpajak = 0;
                        string namapajak = "";
                        int idkategoripajak = 0;
                        string namakategoripajak = "";


                        bool isPajakValid = false;
                        bool isKategoriPajakValid = false;
                        bool isNopValid = false;

                        var pajak = pajakList.FirstOrDefault(q => q.Id == pajakId);
                        if(pajak != null)
                        {
                            isPajakValid = true;
                            idpajak = pajak.Id;
                            namapajak = pajak.Nama;
                        }

                        var katPajak = pajakList
                            .Where(x => x.Id == pajakId)
                            .SelectMany(x => x.MKategoriPajaks)
                            .FirstOrDefault(q => q.Id == pajakKategoriId);
                        if(katPajak != null)
                        {
                            isKategoriPajakValid = true;
                            idkategoripajak = Convert.ToInt32(katPajak.Id);
                            namakategoripajak = katPajak.Nama;
                        }

                        if (isPajakValid && isKategoriPajakValid)
                        {
                            switch ((EnumFactory.EPajak)pajakId)
                            {
                                case EnumFactory.EPajak.MakananMinuman:
                                    var a = context.DbOpRestos.FirstOrDefault(q => q.Nop == nop);
                                    if(a != null)
                                    {
                                        isNopValid = true;
                                        namaop = a.NamaOp;
                                    }
                                    break;
                                case EnumFactory.EPajak.TenagaListrik:
                                    var b = context.DbOpListriks.FirstOrDefault(q => q.Nop == nop);
                                    if (b != null)
                                    {
                                        isNopValid = true;
                                        namaop = b.NamaOp;
                                    }
                                    break;
                                case EnumFactory.EPajak.JasaPerhotelan:
                                    var c = context.DbOpHotels.FirstOrDefault(q => q.Nop == nop);
                                    if (c != null)
                                    {
                                        isNopValid = true;
                                        namaop = c.NamaOp;
                                    }
                                    break;
                                case EnumFactory.EPajak.JasaParkir:
                                    var d = context.DbOpParkirs.FirstOrDefault(q => q.Nop == nop);
                                    if (d != null)
                                    {
                                        isNopValid = true;
                                        namaop = d.NamaOp;
                                    }
                                    break;
                                case EnumFactory.EPajak.JasaKesenianHiburan:
                                    var e = context.DbOpHiburans.FirstOrDefault(q => q.Nop == nop);
                                    if (e != null)
                                    {
                                        isNopValid = true;
                                        namaop = e.NamaOp;
                                    }
                                    break;
                                case EnumFactory.EPajak.AirTanah:
                                    var f = context.DbOpAbts.FirstOrDefault(q => q.Nop == nop);
                                    if (f != null)
                                    {
                                        isNopValid = true;
                                        namaop = f.NamaOp;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (!isPajakValid)
                        {
                            isValidDescription += " [JENIS PAJAK TIDAK ADA PADA MASTER] ";
                        }
                        if(!isKategoriPajakValid)
                        {
                            isValidDescription += " [KATEGORI PAJAK TIDAK ADA PADA MASTER] ";
                        }
                        if (!isNopValid)
                        {
                            isValidDescription += $" [NOP {nop} TIDAK ADA PADA MASTER] ";
                        }

                        if(isPajakValid && isKategoriPajakValid && isNopValid)
                        {
                            isValid = true;
                        }

                        var res = new ObjekPajakViewModel();

                        res.rowNumber = rowNum;
                        res.IsValid = isValid;
                        res.IsValidDescription = isValidDescription;
                        res.Nop = nop;
                        res.NamaOp = namaop;
                        res.IdPajak = idpajak;
                        res.NamaPajak = namapajak;
                        res.IdKategoriPajak = idkategoripajak;
                        res.NamaKategoriPajak = namakategoripajak;

                        result.Add(res);

                        rowNum++;
                    }
                }

                return result;
            }
            public static void UpdateKategoriOp(List<ObjekPajakViewModel> input)
            {
                var context = DBClass.GetContext();

                foreach (var item in input)
                {
                    if (item.IsValid)
                    {
                        switch ((EnumFactory.EPajak)item.IdPajak)
                        {
                            case EPajak.MakananMinuman:
                                var Resto = context.DbOpRestos
                                    .Where(x => x.Nop == item.Nop)
                                    .ToList();

                                foreach (var r in Resto)
                                {
                                    r.KategoriId = item.IdKategoriPajak;
                                    r.KategoriNama = item.NamaKategoriPajak;
                                }

                                if (Resto.Any())
                                {
                                    context.DbOpRestos.UpdateRange(Resto);
                                }
                                break;

                            case EPajak.TenagaListrik:
                                var Listrik = context.DbOpListriks
                                    .Where(x => x.Nop == item.Nop)
                                    .ToList();

                                foreach (var r in Listrik)
                                {
                                    r.KategoriId = item.IdKategoriPajak;
                                    r.KategoriNama = item.NamaKategoriPajak;
                                }

                                if (Listrik.Any())
                                {
                                    context.DbOpListriks.UpdateRange(Listrik);
                                }
                                break;

                            case EPajak.JasaPerhotelan:
                                var Hotel = context.DbOpHotels
                                    .Where(x => x.Nop == item.Nop)
                                    .ToList();

                                foreach (var r in Hotel)
                                {
                                    r.KategoriId = item.IdKategoriPajak;
                                    r.KategoriNama = item.NamaKategoriPajak;
                                }

                                if (Hotel.Any())
                                {
                                    context.DbOpHotels.UpdateRange(Hotel);
                                }
                                break;

                            case EPajak.JasaParkir:
                                var Parkir = context.DbOpParkirs
                                    .Where(x => x.Nop == item.Nop)
                                    .ToList();

                                foreach (var r in Parkir)
                                {
                                    r.KategoriId = item.IdKategoriPajak;
                                    r.KategoriNama = item.NamaKategoriPajak;
                                }

                                if (Parkir.Any())
                                {
                                    context.DbOpParkirs.UpdateRange(Parkir);
                                }
                                break;

                            case EPajak.JasaKesenianHiburan:
                                var Hiburan = context.DbOpHiburans
                                    .Where(x => x.Nop == item.Nop)
                                    .ToList();

                                foreach (var r in Hiburan)
                                {
                                    r.KategoriId = item.IdKategoriPajak;
                                    r.KategoriNama = item.NamaKategoriPajak;
                                }

                                if (Hiburan.Any())
                                {
                                    context.DbOpHiburans.UpdateRange(Hiburan);
                                }
                                break;

                            case EPajak.AirTanah:
                                var Abt = context.DbOpAbts
                                    .Where(x => x.Nop == item.Nop)
                                    .ToList();

                                foreach (var r in Abt)
                                {
                                    r.KategoriId = item.IdKategoriPajak;
                                    r.KategoriNama = item.NamaKategoriPajak;
                                }

                                if (Abt.Any())
                                {
                                    context.DbOpAbts.UpdateRange(Abt);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }

                context.SaveChanges();
            }

        }
        public class ViewModel
        {
            public class PajakViewModel
            {
                public int Id { get; set; }
                public string Nama { get; set; }
                public List<KategoriPajakViewModel> KategoriPajakList { get; set; } = new ();
            }
            public class KategoriPajakViewModel
            {
                public int Id { get; set; }
                public string Nama { get; set; }
            }

            public class ObjekPajakViewModel
            {
                public int rowNumber { get; set; }
                public bool IsValid { get; set; } = false;
                public string IsValidDescription { get; set; } = "";
                public string Nop { get; set; } = string.Empty;
                public string NamaOp { get; set; } = string.Empty;
                public int IdPajak { get; set; }
                public string NamaPajak { get; set; } = string.Empty;
                public int IdKategoriPajak { get; set; }
                public string NamaKategoriPajak { get; set; } = string.Empty;
            }
        }
    }
}
