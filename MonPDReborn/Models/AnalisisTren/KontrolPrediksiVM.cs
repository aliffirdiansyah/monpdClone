namespace MonPDReborn.Models.AnalisisTren.KontrolPrediksiVM
{
    public class Index
    {
        public string? Keyword { get; set; }
    }

    public class Show
    {
        public Show(string keyword) => Keyword = keyword;
        public string Keyword { get; set; }
        public List<DataRealisasiPrediksi> Data { get; set; } = new();
    }

    public class DataRealisasiPrediksi
    {
        public string JenisPajak { get; set; }
        public decimal Target { get; set; }
        public decimal Realisasi { get; set; }
        public decimal Prediksi { get; set; }
        public decimal Potensi { get; set; }
        public TrendStatus Tren { get; set; }
        public string Aksi { get; set; }
    }

    public enum TrendStatus
    {
        Naik,
        Turun,
        Stabil
    }
}
