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
    }
}
