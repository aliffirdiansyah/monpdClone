using MonPDLib;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahVM
    {
        public class Index
        {

        }

        public class Show
        {
            public List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekening.Akun> SudutPandangRekening { get; set; } = new();
            public List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangOpd.Opd> SudutPandangOpd { get; set; } = new();
            public List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis> SudutPandangRekeningJenisObjekOpd { get; set; } = new();
            public int Type { get; set; }
            public Show()
            {
                
            }
            public Show(int type)
            {
                Type = type;
                switch (type)
                {
                    case 1:
                        SudutPandangRekening = SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangRekeningData();
                        break;
                    case 2:
                        SudutPandangOpd = SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangOpdData();
                        break;
                    case 3:
                        SudutPandangRekeningJenisObjekOpd = SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangRekeningJenisObjekOpdData();
                        break;
                    default:
                        break;
                }
            }
        }
        public class Method
        {
            public static List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekening.Akun> GetSudutPandangRekening()
            {
                return SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangRekeningData();
            }
            public static List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangOpd.Opd> GetSudutPandangOpd()
            {
                return SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangOpdData();
            }
            public static List<SeriesPendapatanDaerahVMMethod.ViewModel.ShowSeriesSudutPandangRekeningJenisObjekOpd.Jenis> GetSudutPandangRekeningJenisObjekOpd()
            {
                return SeriesPendapatanDaerahVMMethod.Method.GetSudutPandangRekeningJenisObjekOpdData();
            }
        }

       
    }
}