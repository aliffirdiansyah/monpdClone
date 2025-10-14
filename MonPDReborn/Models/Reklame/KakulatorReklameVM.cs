using MonPDLib;
using MonPDLib.General;
using MonPDLib.Lib;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Web.Mvc;
using static MonPDLib.Lib.KalkullatorReklame;

namespace MonPDReborn.Models.Reklame
{
    public class KakulatorReklameVM
    {

        public class Index
        {
            public ReklameInput Inputan { get; set; } = new();
            public List<SelectListItem> JenisReklameList { get; set; } = new();
            public List<SelectListItem> LetakReklameList { get; set; } = new();
            public List<SelectListItem> ProdukList { get; set; } = new();
            public List<SelectListItem> JalanList { get; set; } = new();
            public Index()
            {
                var context = DBClass.GetReklameContext();
                JenisReklameList = context.MJenisReklames
                    .Select(j => new SelectListItem
                    {
                        Value = j.IdJenisReklame.ToString(),
                        Text = j.NamaJenis
                    })
                    .ToList();
                LetakReklameList = Enum.GetValues(typeof(EnumFactory.LetakReklame))
                    .Cast<EnumFactory.LetakReklame>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                ProdukList = Enum.GetValues(typeof(EnumFactory.ProdukReklame))
                    .Cast<EnumFactory.ProdukReklame>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                JalanList = context.MJalans
                    .AsEnumerable() // ⬅️ Pindahkan ke memory agar LINQ to Objects, bukan LINQ to Entities
                    .GroupBy(q => q.NamaJalan)
                    .Select(g => g.First())
                    .Select(q => new SelectListItem
                    {
                        Value = q.NamaJalan.Trim().ToUpper().ToString(),
                        Text = q.NamaJalan
                    })
                    .ToList();
            }
        }

        public class Show
        {
            public ReklameInput Output { get; set; } = new();
            public KalkullatorReklame KalkullatorReklameRow { get; set; } = new();
            public Show(ReklameInput input)
            {
                Output = input;
                KalkullatorReklameRow = KalkullatorReklame.HitungNilaiSewaReklame(input);
            }
        }

        public class ShowKontrak
        {
            public decimal nilaiKontrak { get; set; } = new();
            public KalkullatorReklame HitungKontrak { get; set;} = new();
            public ShowKontrak(decimal NilaiKontrak)
            {
                nilaiKontrak = NilaiKontrak; 
                HitungKontrak = KalkullatorReklame.HitungNilaiSewaReklame(NilaiKontrak);
            }
        }
    }
}
