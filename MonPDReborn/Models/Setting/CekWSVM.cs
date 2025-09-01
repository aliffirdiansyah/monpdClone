using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using static MonPDReborn.Models.PengawasanReklame.ReklameLiarVM;

namespace MonPDReborn.Models.Setting
{
    public class CekWSVM
    {
        public class Index
        {
            public Index()
            {
                
            }
        }

        // Untuk Partial View _Show.cshtml
        public class Show
        {
            public List<DataWs> DataWsList { get; set; } = new();

            public Show()
            {
                DataWsList = Method.GetDataWs();
            }
        }
        public class Method
        {
            public static List<DataWs> GetDataWs()
            {
                var ret = new List<DataWs>();
                var context = DBClass.GetContext();
                ret = context.SetLastRuns
                    .Select(x => new DataWs()
                    {
                        NamaWs = x.Job ?? "",
                        LastRunUpdate = x.InsDate
                    })
                    .OrderByDescending(x => x.LastRunUpdate)
                    .ToList();
                return ret;
            }
        }
        public class DataWs
        {
            public string NamaWs { get; set; } = null!;
            public DateTime? LastRunUpdate { get; set; }

            public string StatusWs =>
                !LastRunUpdate.HasValue
                    ? "WS Not Running"
                    : (LastRunUpdate.Value.Date < DateTime.Now.Date ? "WS Not Running" : "WS Running");
        }
    }
}
