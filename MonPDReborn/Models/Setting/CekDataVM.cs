using Microsoft.AspNetCore.Mvc;
using static MonPDReborn.Models.PengawasanReklame.ReklameLiarVM;

namespace MonPDReborn.Models.Setting
{
    public class CekDataVM
    {
        public class Index
        {
            public int SelectedTahun { get; set; }
        }

        // Untuk Partial View _Show.cshtml
        public class Show
        {
           
            public Show()
            {
            }
        }
    }
}
