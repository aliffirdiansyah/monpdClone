namespace MonPDReborn.Models.DataOP
{
    public class ProfileOPVM
    {
        public class Index
        {
            public IdentitasObjekPajak IdentitasPajak { get; set; }
            public DataPerizinan Perizinan { get; set; }
            public Pendapatan Pendapatan { get; set; }
            public SaranaPendukung FasilitasPendukung { get; set; }
            public DetailBanquet BanquetDetail {  get; set; }
            public List<DetailFasilitas> FasilitasDetail { get; set; }
            public List<DetailKamar> KamarDetail { get; set; }
            public Index()
            {
                IdentitasPajak = new IdentitasObjekPajak
                {
                    NamaObjekPajak = "The Westin Surabaya",
                    AlamatLengkap = "Puncak Indah Lontar No. 2",
                    Kecamatan_Kelurahan = "Sambikerep - Lontar",
                    NOP = "35.78.011.010.901.0007",
                    Telepon = "58208788",
                    TanggalBuka = new DateTime(2020, 12, 1),
                    JenisObjekPajak = "Hotel Bintang Lima"
                };

                Perizinan = new DataPerizinan
                {
                    NomorIMB = "503/IMB/0192/2015",
                    TanggalIMB = new DateTime(2015, 3, 15),
                    NomorSITU_NIB = "8129381273981237",
                    NomorIzinOperasional = "503/HO/093/2016"
                };

                Pendapatan = new Pendapatan
                {
                    Okupansi = "85% per bulan",
                    RataTarifKamar = 750000m,
                    PendapatanKotor = 1250000000m,
                    JumlahTransaksi = "+3.500 transaksi"
                };

                FasilitasPendukung = new SaranaPendukung
                {
                    JumlahKaryawan = 58,
                    MetodePenjualan = "Offline",
                    MetodePembayaran = "Hibrid"
                };

                BanquetDetail = new DetailBanquet
                {
                    Nama = "Ruang Serbaguna A",
                    Jumlah = 3,
                    JenisBanquet = 1, // Misal: 1 = Kecil, 2 = Sedang, 3 = Besar
                    Kapasitas = 100,
                    HargaSewa = 2500000,
                    HargaPaket = 3500000,
                    Okupansi = 85
                };

                FasilitasDetail = new List<DetailFasilitas>
                {
                    new DetailFasilitas { Nama = "Ruang Rapat", Jumlah = 3, Kapasitas = 50 },
                    new DetailFasilitas { Nama = "Ballroom", Jumlah = 1, Kapasitas = 500 },
                    new DetailFasilitas { Nama = "Kolam Renang", Jumlah = 1, Kapasitas = 100 },
                    new DetailFasilitas { Nama = "Restoran", Jumlah = 2, Kapasitas = 120 },
                    new DetailFasilitas { Nama = "Gym", Jumlah = 1, Kapasitas = 40 }
                };

                KamarDetail = new List<DetailKamar>
                {
                    new DetailKamar { Kamar = "Standard", Jumlah = 20, Tarif = 350000 },
                    new DetailKamar { Kamar = "Deluxe", Jumlah = 15, Tarif = 500000 },
                    new DetailKamar { Kamar = "Executive", Jumlah = 10, Tarif = 750000 },
                    new DetailKamar { Kamar = "Suite", Jumlah = 5, Tarif = 1200000 }
                };

            }
        }
        public class Show
        {

        }
        public class Detail
        {

        }
        public class Method
        {

        }
        public class IdentitasObjekPajak
        {
            public string NamaObjekPajak { get; set; }
            public string AlamatLengkap { get; set; }
            public string Kecamatan_Kelurahan { get; set; } // Kecamatan - Kelurahan
            public string NOP { get; set; } // Nomor Objek Pajak
            public string Telepon { get; set; }
            public DateTime TanggalBuka { get; set; }
            public string JenisObjekPajak { get; set; }
        }

        // Class untuk bagian "Data Perizinan"
        public class DataPerizinan
        {
            public string NomorIMB { get; set; } // Nomor IMB
            public DateTime TanggalIMB { get; set; } // Tanggal IMB
            public string NomorSITU_NIB { get; set; } // Nomor SITU/NIB
            public string NomorIzinOperasional { get; set; } // Nomor Izin Operasional
        }

        // Class untuk bagian "Pendapatan"
        public class Pendapatan
        {
            public string Okupansi { get; set; } // Okupansi (misal: "85% per bulan")
            public decimal RataTarifKamar { get; set; } // Rata Tarif Kamar
            public decimal PendapatanKotor { get; set; } // Pendapatan Kotor
            public string JumlahTransaksi { get; set; } // Jumlah Transaksi (misal: "+3.500 transaksi")
        }

        // Class untuk bagian "Sarana Pendukung"
        public class SaranaPendukung
        {
            public int JumlahKaryawan { get; set; }
            public string MetodePembayaran { get; set; }
            public string MetodePenjualan { get; set; }
        }

        public class DetailBanquet
        {
            public string Nama { get; set; }
            public int Jumlah { get; set; }
            public int JenisBanquet { get; set; }
            public int Kapasitas { get; set; }
            public int HargaSewa { get; set; }
            public int HargaPaket { get; set; }
            public int Okupansi { get; set; }
        }

        public class DetailFasilitas
        {
            public string Nama { get; set; }
            public int Jumlah { get; set; }
            public int Kapasitas { get; set; }
        }

        public class DetailKamar
        {
            public string Kamar { get; set; }
            public int Jumlah { get; set; }
            public int Tarif { get; set; }
        }
    }
}
