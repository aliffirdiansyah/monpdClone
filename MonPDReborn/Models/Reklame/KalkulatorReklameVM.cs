using MonPDLib;
using MonPDLib.General;
using MonPDLib.Lib;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Web.Mvc;

namespace MonPDReborn.Models.Reklame
{
    public class KalkulatorReklameVM
    {

        public class Index
        {
            public KalkulatorReklamePermanen.ReklameInput Inputan { get; set; } = new();
            public KalkulatorReklameInsidentil.ReklameInput InputanIns { get; set; } = new();
            public List<SelectListItem> JenisReklameList { get; set; } = new();
            public List<dynamic> JenisReklameInsList { get; set; } = new();
            public List<SelectListItem> LetakReklameList { get; set; } = new();
            public List<SelectListItem> ProdukList { get; set; } = new();
            public List<SelectListItem> JalanList { get; set; } = new();
            public Index()
            {
                var context = DBClass.GetReklameContext();
                JenisReklameList = context.MJenisReklames
                    .Where(x => x.Kategori == (int)EnumFactory.JenisReklame.Permanen)
                    .Select(j => new SelectListItem
                    {
                        Value = j.IdJenisReklame.ToString(),
                        Text = j.NamaJenis
                    })
                    .OrderBy(x => x.Text)
                    .ToList();
                JenisReklameInsList = context.MJenisReklames
                    .Where(x => x.Kategori == (int)EnumFactory.JenisReklame.Insidentil)
                    .Select(j => new 
                    {
                        Value = j.IdJenisReklame.ToString(),
                        Text = j.NamaJenis,
                        ModeUkur = j.ModeUkur
                    })
                    .OrderBy(x => x.Text)
                    .ToList<dynamic>();
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
                    .Select(q => new SelectListItem
                    {
                        Value = q.IdJalan.ToString(),
                        Text = q.NamaJalan
                    })
                    .ToList();
            }
        }

        public class Show
        {
            public KalkulatorReklamePermanen.ReklameInput Output { get; set; } = new();
            public KalkulatorReklamePermanen KalkullatorReklameRow { get; set; } = new();
            public Show(KalkulatorReklamePermanen.ReklameInput input)
            {
                Output = input;
                KalkullatorReklameRow = KalkulatorReklamePermanen.HitungNilaiSewaReklame(input);
            }
        }
        
        public class ShowIns
        {
            public KalkulatorReklameInsidentil.ReklameInput Output { get; set; } = new();
            public KalkulatorReklameInsidentil KalkullatorReklameRow { get; set; } = new();
            public ShowIns(KalkulatorReklameInsidentil.ReklameInput input)
            {
                Output = input;
                KalkullatorReklameRow = KalkulatorReklameInsidentil.HitungNilaiSewaReklame(input);
            }
        }

        public class ShowKontrak
        {
            public decimal nilaiKontrak { get; set; } = new();
            public KalkulatorReklamePermanen HitungKontrak { get; set;} = new();
            public ShowKontrak(decimal NilaiKontrak)
            {
                nilaiKontrak = NilaiKontrak; 
                HitungKontrak = KalkulatorReklamePermanen.HitungNilaiSewaReklame(NilaiKontrak);
            }
        }
    }
}
