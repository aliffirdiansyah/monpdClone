using Microsoft.AspNetCore.Mvc;

namespace MonPDReborn.Models.AktivitasOP
{
    public class ReklameSummaryVM
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
            public List<ReklamePermanen> ReklamePermanenList { get; set; } = new();
            public List<TerbatasReklame> TerbatasReklameList { get; set; } = new();
            public List<IsidentilReklame> IsidentilReklameList { get; set; } = new();
            public int Tahun { get; set; }
            public Show() { }

            public Show(int tahun)
            {
                Tahun = tahun;
                ReklamePermanenList = Method.GetReklamePermanen(tahun);
                TerbatasReklameList = Method.GetTerbatasReklame(tahun);
                IsidentilReklameList = Method.GetIsidentilReklame(tahun);
            }
        }

        public class Detail
        {

            public Detail() { }

            public Detail(string jenisPajak)
            {
            }
        }

        public class Method
        {
            public static List<ReklamePermanen> GetReklamePermanen(int tahun)
            {
                var all = GetAllReklamePermanen();

                return all
                    .Where(x => x.Tahun == tahun)
                    .ToList();
            }

            public static List<TerbatasReklame> GetTerbatasReklame(int tahun)
            {
                var all = GetAllTerbatasReklame();

                return all
                    .Where(x => x.Tahun == tahun)
                    .ToList();
            }

            public static List<IsidentilReklame> GetIsidentilReklame(int tahun)
            {
                var all = GetAllIsidentilReklame();

                return all
                    .Where(x => x.Tahun == tahun)
                    .ToList();
            }

            public static List<ReklamePermanen> GetAllReklamePermanen()
            {
                return new List<ReklamePermanen>
                {
                    new ReklamePermanen { Bulan = "Januari", Tahun = 2025, SKPDJT = 10, NilaiJT = 1000000, SKPDBlmJT = 5, NilaiBlmJT = 500000, SKPDPanjang = 8, NilaiPanjang = 800000, SKPDBlmPanjang = 4, NilaiBlmPanjang = 400000, SKPDKB = 6, NilaiKB = 600000, SKPDBlmKB = 3, NilaiBlmKB = 300000 },
                };
            }

            public static List<TerbatasReklame> GetAllTerbatasReklame()
            {
                return new List<TerbatasReklame>
                {
                    new TerbatasReklame { Bulan = "Februari", Tahun = 2025, SKPDJT = 12, NilaiJT = 1200000, SKPDBlmJT = 6, NilaiBlmJT = 600000, SKPDPanjang = 9, NilaiPanjang = 900000, SKPDBlmPanjang = 4, NilaiBlmPanjang = 400000, SKPDKB = 7, NilaiKB = 700000, SKPDBlmKB = 3, NilaiBlmKB = 300000 },
                };
            }

            public static List<IsidentilReklame> GetAllIsidentilReklame()
            {
                return new List<IsidentilReklame>
                {
                    new IsidentilReklame { Bulan = "Maret", Tahun = 2025, SKPDKB = 8, NilaiKB = 800000, SKPDBlmKB = 4, NilaiBlmKB = 400000 },
                };
            }



        }

        public class ReklamePermanen
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDJT { get; set; } 
            public decimal NilaiJT { get; set; }
            public int SKPDBlmJT { get; set; } 
            public decimal NilaiBlmJT { get; set; } 
            public int SKPDPanjang { get; set; } 
            public decimal NilaiPanjang { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public decimal NilaiBlmPanjang { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal Potensi => NilaiBlmJT + NilaiBlmPanjang + NilaiBlmKB;

        }

        public class TerbatasReklame
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDJT { get; set; }
            public decimal NilaiJT { get; set; }
            public int SKPDBlmJT { get; set; }
            public decimal NilaiBlmJT { get; set; }
            public int SKPDPanjang { get; set; }
            public decimal NilaiPanjang { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public decimal NilaiBlmPanjang { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal Potensi => NilaiBlmJT + NilaiBlmPanjang + NilaiBlmKB;
        }

        public class IsidentilReklame
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDKB { get; set; }
            public decimal NilaiKB { get; set; }
            public int SKPDBlmKB { get; set; }
            public decimal NilaiBlmKB { get; set; }
            public decimal Potensi => NilaiBlmKB;
        }

        public class PermanenJT
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmJT { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } =null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public string Email { get; set; } = null!;
            public int JmlUpaya { get; set; }
        }
        public class PermananBP
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmPanjang { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public int JmlUpaya { get; set; }
        }

        public class PermanenKB
        {
            public string Bulan { get; set; } = null!;
            public int Tahun { get; set; }
            public int SKPDBlmKB { get; set; }
            public string NoFormulir { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string AlamatOP { get; set; } = null!;
            public string IsiReklame { get; set; } = null!;
            public string Status { get; set; } = null!;
            public DateTime TahunPajak { get; set; }
            public decimal JumlahNilai { get; set; }
            public string Email { get; set; } = null!;
            public int JmlUpaya { get; set; }
        }
    }
}
