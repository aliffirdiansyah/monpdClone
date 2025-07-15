using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MonPDReborn.Models.Reklame
{
    public class ReklameVM
    {
        public class Index
        {
            // Properti untuk menampung nilai yang dipilih dari filter
            public int SelectedJalan { get; set; }
            public int SelectedKecamatan { get; set; }
            public int SelectedJenisReklame { get; set; }

            // Properti untuk mengisi data ke dalam dropdown
            public List<SelectListItem> JalanList { get; set; } = new();
            public List<SelectListItem> KecamatanList { get; set; } = new();
            public List<SelectListItem> JenisReklameList { get; set; } = new();

            public Index()
            {
                // Anda bisa mengisi data list ini dari database atau secara statis
                // Contoh pengisian data statis:
                JalanList.Add(new SelectListItem { Value = "1", Text = "Jl. Ahmad Yani" });
                JalanList.Add(new SelectListItem { Value = "2", Text = "Jl. Basuki Rahmat" });

                KecamatanList.Add(new SelectListItem { Value = "1", Text = "Gayungan" });
                KecamatanList.Add(new SelectListItem { Value = "2", Text = "Tegalsari" });

                JenisReklameList.Add(new SelectListItem { Value = "1", Text = "Insidentil" });
                JenisReklameList.Add(new SelectListItem { Value = "2", Text = "Permanen < 8m" });
                JenisReklameList.Add(new SelectListItem { Value = "3", Text = "Permanen > 8m" });
            }
        }
        
        // Kelas untuk menampung data yang akan ditampilkan di View
        public class Show
        {
            public List<ReklamePerJalan> DataReklamePerJalanList { get; set; } = new();
            public Show()
            {
                DataReklamePerJalanList = Method.GetDataReklamePerJalan();
            }
        }

        // Model untuk partial view Detail.cshtml (tabel detail di modal)
        public class Detail
        {
            public List<DataReklame> DataReklameDetailList { get; set; } = new();
            public string NamaJalan { get; set; } = "";

            public Detail(string namaJalan)
            {
                NamaJalan = namaJalan;
                DataReklameDetailList = Method.GetDetailDataByJalan(namaJalan);
            }
        }

        public class Method
        {
            // Method BARU untuk membuat data ringkasan per jalan
            public static List<ReklamePerJalan> GetDataReklamePerJalan()
            {
                // Ambil semua data detail sebagai sumber
                var semuaReklame = GetAllDataReklame();

                // Kelompokkan berdasarkan nama jalan dan hitung
                var ringkasan = semuaReklame
                    .GroupBy(r => r.TitikLokasi.Split('(')[0].Trim()) // Mengambil nama jalan saja
                    .Select(g => new ReklamePerJalan
                    {
                        NamaJalan = g.Key,
                        Kecamatan = "Contoh Kecamatan", // Ganti dengan data asli jika ada
                        TotalReklame = g.Count(),
                        Insidentil = g.Count(r => r.Jenis == "Insidentil"),
                        PermanenKecil = g.Count(r => r.Jenis == "Permanen < 8m"),
                        PermanenBesar = g.Count(r => r.Jenis == "Permanen > 8m")
                    }).ToList();

                return ringkasan;
            }

            // Method BARU untuk memfilter data detail berdasarkan jalan
            public static List<DataReklame> GetDetailDataByJalan(string namaJalan)
            {
                return GetAllDataReklame()
                    .Where(r => r.TitikLokasi.Contains(namaJalan))
                    .ToList();
            }

            // Method ini sekarang menjadi sumber data utama
            public static List<DataReklame> GetAllDataReklame()
            {
                return new List<DataReklame>
                {
                    new DataReklame { TitikLokasi = "Jl. Ahmad Yani (Depan Graha Pena)", Jenis = "Permanen > 8m", Ukuran = "5m x 10m", Penyelenggara = "PT. Djarum", MasaBerlaku = "01 Jan s/d 31 Des 2025" },
                    new DataReklame { TitikLokasi = "Jl. Ahmad Yani (Samping Royal Plaza)", Jenis = "Permanen < 8m", Ukuran = "3m x 4m", Penyelenggara = "Unilever Indonesia", MasaBerlaku = "01 Jun 2025 s/d 31 Mei 2026" },
                    new DataReklame { TitikLokasi = "Jl. Basuki Rahmat (Depan Tunjungan Plaza)", Jenis = "Permanen < 8m", Ukuran = "4m x 6m", Penyelenggara = "Samsung Indonesia", MasaBerlaku = "15 Mar s/d 14 Mar 2026" },
                    new DataReklame { TitikLokasi = "Perempatan Jl. Kertajaya Indah", Jenis = "Insidentil", Ukuran = "1m x 2m", Penyelenggara = "Event Organizer Laris", MasaBerlaku = "10 Jul s/d 20 Jul 2025" },
                    new DataReklame { TitikLokasi = "Jl. Mayjend Sungkono (Ciputra World)", Jenis = "Permanen > 8m", Ukuran = "6m x 12m", Penyelenggara = "Gudang Garam Tbk", MasaBerlaku = "01 Feb 2025 s/d 31 Jan 2026" },
                    new DataReklame { TitikLokasi = "Area Parkir Stadion GBT", Jenis = "Insidentil", Ukuran = "3m x 5m", Penyelenggara = "Konser Musik Nasional", MasaBerlaku = "01 Agu s/d 03 Agu 2025" }
                };
            }
        }
    }

    // Model BARU untuk tabel ringkasan
    public class ReklamePerJalan
    {
        public string NamaJalan { get; set; } = null!;
        public string Kecamatan { get; set; } = null!;
        public int TotalReklame { get; set; }
        public int Insidentil { get; set; }
        public int PermanenKecil { get; set; }
        public int PermanenBesar { get; set; }
    }

    // Model untuk setiap baris data reklame detail (yang sudah ada)
    public class DataReklame
    {
        public string TitikLokasi { get; set; } = null!;
        public string Jenis { get; set; } = null!;
        public string Ukuran { get; set; } = null!;
        public string Penyelenggara { get; set; } = null!;
        public string MasaBerlaku { get; set; } = null!;
    }

}
