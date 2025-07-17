using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Models.AktivitasOP
{
    public class SeriesPendapatanDaerahVM
    {
        public class Index
        {
            public string Keyword { get; set; } = null!;
            public Index()
            {

            }

        }

        public class Show
        {
            public Show() { }

        }



        public class Detail
        {
            public Detail()
            {

            }
        }

        public class Method
        {


        }

        public class DataPendapatan
        {
            public string JenisPajak { get; set; } = null!;
            public decimal Target1 { get; set; }
            public decimal Target2 { get; set; }
            public decimal Target3 { get; set; }
            public decimal Target4 { get; set; }
            public decimal Target5 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Realisasi4 { get; set; }
            public decimal Realisasi5 { get; set; }
            public decimal Persentase1 { get; set; }
            public decimal Persentase2 { get; set; }
            public decimal Persentase3 { get; set; }
            public decimal Persentase4 { get; set; }
            public decimal Persentase5 { get; set; }
        }
        
    }
}
