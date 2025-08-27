using Microsoft.AspNetCore.Mvc.Rendering;
using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahVM
    {
        public class Index
        {
            public List<SelectListItem> TahunListSekarang { get; set; } = new();
            public List<SelectListItem> TahunListMinus1 { get; set; } = new();

            public Index()
            {
                var tahunSekarang = DateTime.Now.Year;
                var listTahun = Method.GetTahun()
                    .Select(x => new SelectListItem
                    {
                        Value = x.Tahun.ToString(),
                        Text = x.Tahun.ToString()
                    }).ToList();

                // untuk dropdown default tahun sekarang
                TahunListSekarang = listTahun.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = (x.Value == tahunSekarang.ToString())
                }).ToList();

                // untuk dropdown default tahun sekarang - 1
                TahunListMinus1 = listTahun.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text,
                    Selected = (x.Value == (tahunSekarang - 1).ToString())
                }).ToList();
            }
        }

        public class Show
        {
            public int Type { get; set; }
            public int Seq { get; set; }
            public int Tahun { get; set; }
            public List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekening.Akun> SudutPandangRekening { get; set; } = new();
            public List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangOpd.Opd> SudutPandangOpd { get; set; } = new();
            public List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis> SudutPandangRekeningJenisObjekOpd { get; set; } = new();
            public Show(int type, int tahun, int seq)
            {
                Type = type;
                Tahun = tahun;
                Seq = seq;
                switch (type)
                {
                    case 1:
                        SudutPandangRekening = Method.GetSudutPandangRekening(tahun);
                        break;
                    case 2:
                        SudutPandangOpd = Method.GetSudutPandangOpd(tahun);
                        break;
                    case 3:
                        SudutPandangRekeningJenisObjekOpd = Method.GetSudutPandangRekeningJenisObjekOpd(tahun);
                        break;
                    default:
                        break;
                }
            }
        }

        public class Method
        {
            public static List<SeriesPendapatanDaerahVMMethod.ViewModel.TahunRow> GetTahun()
            {
                return SeriesPendapatanDaerahVMMethod.Method.GetTahunList();
            }
            public static List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekening.Akun> GetSudutPandangRekening(int tahun)
            {
                return SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangRekeningData(tahun);
            }
            public static List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangOpd.Opd> GetSudutPandangOpd(int tahun)
            {
                return SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangOpdData(tahun);
            }
            public static List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis> GetSudutPandangRekeningJenisObjekOpd(int tahun)
            {
                return SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangRekeningJenisObjekOpdData(tahun);
            }
        }
    }
}