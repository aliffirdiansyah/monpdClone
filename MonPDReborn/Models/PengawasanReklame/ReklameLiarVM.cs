using MonPDLib;
using MonPDReborn.Models.Reklame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonPDReborn.Models.PengawasanReklame
{
    public class ReklameLiarVM
    {
        // Untuk halaman utama Index.cshtml
        public class Index
        {
            public int SelectedTahun { get; set; }
        }

        // Untuk Partial View _Show.cshtml
        public class Show
        {
            public List<RekapBulanan> DataRekap { get; set; } = new();
            public Show()
            {
                DataRekap = Method.GetRekapDataBulanan();
            }
        }

        // Untuk Partial View _Detail.cshtml (modal)
        public class Detail
        {
            public InfoJalan DataJalan { get; set; } = new();
            public List<DetailData> DataDetail { get; set; } = new();

            public Detail(string jalan, string kategori, string status)
            {
                DataDetail = Method.GetDetailDataReklame(jalan, kategori, status);
            }
        }

        public static class Method
        {
            // Method baru untuk data dummy bulanan
            public static List<RekapBulanan> GetRekapDataBulanan()
            {
                var ret = new List<RekapBulanan>();
                var context = DBClass.GetContext();
                var tahun = DateTime.Now.Year;



                return ret;
            }
            public static List<DetailData> GetDetailDataReklame(string jalan, string jenis, string bulan)
            {
                var ret = new List<DetailData>();
                var context = DBClass.GetContext();



                return ret;
            }
        }

        // --- MODEL DATA BARU ---
        public class RekapBulanan
        {
            public int TahunData { get; set; }
            public string NamaJalan { get; set; } = null!;
            public string KelasJalan { get; set; } = null!;
            public string Jenis { get; set; } = null!;
            public int Jan { get; set; }
            public int Feb { get; set; }
            public int Mar { get; set; }
            public int Apr { get; set; }
            public int Mei { get; set; }
            public int Jun { get; set; }
            public int Jul { get; set; }
            public int Agu { get; set; }
            public int Sep { get; set; }
            public int Okt { get; set; }
            public int Nov { get; set; }
            public int Des { get; set; }
            public int Total => Jan + Feb + Mar + Apr + Mei + Jun + Jul + Agu + Sep + Okt + Nov + Des;
        }

        public class DetailData
        {
            public InfoJalan InfoJalan { get; set; } = new();
            public string KelasJalan { get; set; } = null!;
            public string NamaJalan { get; set; } = null!;
            public string AlamatReklame { get; set; } = null!;
            public string Jenis { get; set; } = null!;
            public DateTime TanggalBongkar { get; set; }
        }

        public class InfoJalan
        {
            public string NamaJalan { get; set; } = "";
            public string Bulan { get; set; } = "";
            public string Tahun { get; set; } = "";
            public int JumlahReklame { get; set; }
        }
    }
}