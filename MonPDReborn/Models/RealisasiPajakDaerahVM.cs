namespace MonPDReborn.Models
{
    public class RealisasiPajakDaerahVM
    {
        public class Index
        {
            public string ErrorMessage { get; set; } = string.Empty;
            public Index()
            {
            }
            public Index(string errorMessage)
            {
                ErrorMessage = errorMessage;
            }
        }
    }
}
