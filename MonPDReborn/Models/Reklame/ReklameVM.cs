using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MonPDReborn.Models.Reklame
{
    public class ReklameVM
    {
        public class Index
        {
            public DashboardData Data { get; set; } = new();
            // Properti untuk menampung nilai yang dipilih dari filter
            public int SelectedJalan { get; set; }
            public int SelectedKelasJalan { get; set; }
            public int SelectedJenisReklame { get; set; }
            public DateTime[] SelectedDateRange { get; set; }

            // Properti untuk mengisi data ke dalam dropdown
            public List<SelectListItem> JalanList { get; set; } = new();
            public List<SelectListItem> KelasJalanList { get; set; } = new();
            public List<SelectListItem> JenisReklameList { get; set; } = new();

            public Index()
            {
                Data = Method.GetDashboardData();

                
                // Contoh pengisian data statis:
                JalanList.Add(new SelectListItem { Value = "1", Text = "Jl. Ahmad Yani" });
                JalanList.Add(new SelectListItem { Value = "2", Text = "Jl. Basuki Rahmat" });

                KelasJalanList.Add(new SelectListItem { Value = "1", Text = "Gayungan" });
                KelasJalanList.Add(new SelectListItem { Value = "2", Text = "Tegalsari" });

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
            public static DashboardData GetDashboardData()
            {
                // 1. Ambil data sumber
                var semuaReklame = GetAllDataReklame();       // Untuk total reklame
                var reklamePerJalan = GetDataReklamePerJalan(); // Untuk total jalan

                // 2. Lakukan perhitungan sesuai permintaan
                var dashboard = new DashboardData
                {
                    // Menghitung jumlah semua reklame
                    TotalReklame = semuaReklame.Count(),
                    // Menghitung jumlah baris/jalan unik yang memiliki reklame
                    JalanDenganReklame = reklamePerJalan.Count(),
                    // Mengisi data dummy untuk pelanggaran
                    PelanggaranTerdeteksi = 12
                };

                return dashboard;
            }
            // Method BARU untuk membuat data ringkasan per jalan
            public static List<ReklamePerJalan> GetDataReklamePerJalan()
            {
                return new List<ReklamePerJalan>
        {
            new ReklamePerJalan
            {
                NamaJalan = "Jl. KUTAI", KelasJalan = "Kelas I",
                InsidentilBongkar = 3, InsidentilBelumBongkar = 0, InsidentilAktif = 5,
                PermanenBongkar = 8, PermanenBelumBongkar = 2, PermanenAktif = 10,
                TerbatasBongkar = 1, TerbatasBelumBongkar = 1, TerbatasAktif = 2,
            },
            new ReklamePerJalan
            {
                NamaJalan = "Jl. AHMAD YANI", KelasJalan = "Kelas II",
                InsidentilBongkar = 5, InsidentilBelumBongkar = 2, InsidentilAktif = 10,
                PermanenBongkar = 15, PermanenBelumBongkar = 5, PermanenAktif = 20,
                TerbatasBongkar = 3, TerbatasBelumBongkar = 0, TerbatasAktif = 4,
            },
            new ReklamePerJalan
            {
                NamaJalan = "Jl. BASUKI RAHMAT", KelasJalan = "Kelas III",
                InsidentilBongkar = 2, InsidentilBelumBongkar = 1, InsidentilAktif = 8,
                PermanenBongkar = 10, PermanenBelumBongkar = 3, PermanenAktif = 15,
                TerbatasBongkar = 2, TerbatasBelumBongkar = 2, TerbatasAktif = 5,
            }
        };
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
        // Properti Kunci
        public string NamaJalan { get; set; } = null!;
        public string KelasJalan { get; set; } = null!; // Kelas Jalan diganti KelasJalan agar sesuai data

        // Properti untuk grup INSIDENTIL
        public int InsidentilBongkar { get; set; }
        public int InsidentilBelumBongkar { get; set; }
        public int InsidentilAktif { get; set; }
        public int InsidentilJumlah => InsidentilBongkar + InsidentilBelumBongkar + InsidentilAktif;

        // Properti untuk grup JENIS PERMANENT
        public int PermanenBongkar { get; set; }
        public int PermanenBelumBongkar { get; set; }
        public int PermanenAktif { get; set; }
        public int PermanenJumlah => PermanenBongkar + PermanenBelumBongkar + PermanenAktif;

        // Properti untuk grup TERBATAS
        public int TerbatasBongkar { get; set; }
        public int TerbatasBelumBongkar { get; set; }
        public int TerbatasAktif { get; set; }
        public int TerbatasJumlah => TerbatasBongkar + TerbatasBelumBongkar + TerbatasAktif;
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

    public class DashboardData
    {
        public int TotalReklame { get; set; }
        public int JalanDenganReklame { get; set; }
        public int PelanggaranTerdeteksi { get; set; }
    }

}
