using MonPDLib;
using static MonPDReborn.Models.DataOP.ProfileOPVM.DetailHotel;

namespace MonPDReborn.Models.DataOP
{
    public class ProfileOPVM
    {
        public class Index
        {
            public IdentitasObjekPajak IdentitasPajak { get; set; } = new();
            public DataPerizinan Perizinan { get; set; } = new();
            public DetailHotel.Pendapatan Pendapatan { get; set; } = new();
            public DetailHotel.SaranaPendukung FasilitasPendukung { get; set; } = new();
            public DetailHotel.DetailBanquet BanquetDetail { get; set; } = new();
            public List<DetailHotel.DetailFasilitas> FasilitasDetail { get; set; } = new();
            public List<DetailHotel.DetailKamar> KamarDetail { get; set; } = new();
            public Index(string nop)
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;
               
                nop = "357808000290800052"; // contoh NOP
                string kode = nop.Substring(10, 3); // ambil karakter ke-11 s.d. ke-13 (indeks mulai dari 0)

                if (kode == "908")
                {
                    var OpAbt = context.DbMonAbts.FirstOrDefault(x => x.Nop == nop);
                    if (OpAbt != null)
                    {
                        IdentitasPajak.NOP = OpAbt.Nop;
                        IdentitasPajak.NamaObjekPajak = OpAbt.NamaOp;
                        IdentitasPajak.AlamatLengkap = OpAbt.AlamatOp;
                        IdentitasPajak.Kecamatan_Kelurahan = $"{OpAbt.AlamatOpKdCamat} - {OpAbt.AlamatOpKdLurah}";
                        IdentitasPajak.Telepon = "58208788"; // contoh telepon, bisa diambil dari data lain
                        IdentitasPajak.TanggalBuka = OpAbt.TglMulaiBukaOp; // jika tidak ada tanggal tutup, gunakan tanggal sekarang
                    }
                    else
                    {
                        new ArgumentException("OP tidak ditemukan");
                    }
                }
                else if (kode == "902")
                {
                    Console.WriteLine("Pajak Restoran");
                }
                else if (kode == "901")
                {
                    Console.WriteLine("Pajak Hotel");
                }

                //IdentitasPajak = new IdentitasObjekPajak
                //{
                //    NamaObjekPajak = "The Westin Surabaya",
                //    AlamatLengkap = "Puncak Indah Lontar No. 2",
                //    Kecamatan_Kelurahan = "Sambikerep - Lontar",
                //    NOP = "35.78.011.010.901.0007",
                //    Telepon = "58208788",
                //    TanggalBuka = new DateTime(2020, 12, 1),
                //    JenisObjekPajak = "Hotel Bintang Lima"
                //};

                //Perizinan = new DataPerizinan
                //{
                //    NomorIMB = "503/IMB/0192/2015",
                //    TanggalIMB = new DateTime(2015, 3, 15),
                //    NomorSITU_NIB = "8129381273981237",
                //    NomorIzinOperasional = "503/HO/093/2016"
                //};

                //Pendapatan = new DetailHotel.Pendapatan
                //{
                //    Okupansi = "85% per bulan",
                //    RataTarifKamar = 750000m,
                //    PendapatanKotor = 1250000000m,
                //    JumlahTransaksi = "+3.500 transaksi"
                //};

                //FasilitasPendukung = new DetailHotel.SaranaPendukung
                //{
                //    JumlahKaryawan = 58,
                //    MetodePenjualan = "Offline",
                //    MetodePembayaran = "Hibrid"
                //};

                //BanquetDetail = new DetailHotel.DetailBanquet
                //{
                //    Nama = "Ruang Serbaguna A",
                //    Jumlah = 3,
                //    JenisBanquet = 1, // Misal: 1 = Kecil, 2 = Sedang, 3 = Besar
                //    Kapasitas = 100,
                //    HargaSewa = 2500000,
                //    HargaPaket = 3500000,
                //    Okupansi = 85
                //};

                //FasilitasDetail = new List<DetailHotel.DetailFasilitas>
                //{
                //    new DetailHotel.DetailFasilitas { Nama = "Ruang Rapat", Jumlah = 3, Kapasitas = 50 },
                //    new DetailHotel.DetailFasilitas { Nama = "Ballroom", Jumlah = 1, Kapasitas = 500 },
                //    new DetailHotel.DetailFasilitas { Nama = "Kolam Renang", Jumlah = 1, Kapasitas = 100 },
                //    new DetailHotel.DetailFasilitas { Nama = "Restoran", Jumlah = 2, Kapasitas = 120 },
                //    new DetailHotel.DetailFasilitas { Nama = "Gym", Jumlah = 1, Kapasitas = 40 }
                //};

                //KamarDetail = new List<DetailHotel.DetailKamar>
                //{
                //    new DetailHotel.DetailKamar { Kamar = "Standard", Jumlah = 20, Tarif = 350000 },
                //    new DetailHotel.DetailKamar { Kamar = "Deluxe", Jumlah = 15, Tarif = 500000 },
                //    new DetailHotel.DetailKamar { Kamar = "Executive", Jumlah = 10, Tarif = 750000 },
                //    new DetailHotel.DetailKamar { Kamar = "Suite", Jumlah = 5, Tarif = 1200000 }
                //};

            }
        }
        public class Show
        {
            public IdentitasObjekPajak IdentitasPajak { get; set; } = new();
            public DataPerizinan Perizinan { get; set; } = new();
            public DetailHotel.Pendapatan Pendapatan { get; set; } = new();
            public DetailHotel.SaranaPendukung FasilitasHotelPendukung { get; set; } = new();
            public DetailHotel.DetailBanquet BanquetHotelDetail { get; set; } = new();
            public List<DetailHotel.DetailFasilitas> FasilitasHotelDetail { get; set; } = new();
            public List<DetailHotel.DetailKamar> KamarHotelDetail { get; set; } = new();
            public Show()
            {
                
            }
            public Show(string nop)
            {
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                nop = "357808000290800052"; // contoh NOP
                string kode = nop.Substring(10, 3); // ambil karakter ke-11 s.d. ke-13 (indeks mulai dari 0)

                if (kode == "908")
                {
                    var OpAbt = context.DbMonAbts.FirstOrDefault(x => x.Nop == nop);
                    if (OpAbt != null)
                    {
                        IdentitasPajak.NOP = OpAbt.Nop;
                        IdentitasPajak.NamaObjekPajak = OpAbt.NamaOp;
                        IdentitasPajak.AlamatLengkap = OpAbt.AlamatOp;
                        IdentitasPajak.Kecamatan_Kelurahan = $"{OpAbt.AlamatOpKdCamat} - {OpAbt.AlamatOpKdLurah}";
                        IdentitasPajak.Telepon = "58208788"; // contoh telepon, bisa diambil dari data lain
                        IdentitasPajak.TanggalBuka = OpAbt.TglMulaiBukaOp; // jika tidak ada tanggal tutup, gunakan tanggal sekarang
                    }
                    else
                    {
                        new ArgumentException("OP tidak ditemukan");
                    }
                }
                else if (kode == "902")
                {
                    Console.WriteLine("Pajak Restoran");
                }
                else if (kode == "901")
                {
                    Console.WriteLine("Pajak Hotel");
                }

                //IdentitasPajak = new IdentitasObjekPajak
                //{
                //    NamaObjekPajak = "The Westin Surabaya",
                //    AlamatLengkap = "Puncak Indah Lontar No. 2",
                //    Kecamatan_Kelurahan = "Sambikerep - Lontar",
                //    NOP = "35.78.011.010.901.0007",
                //    Telepon = "58208788",
                //    TanggalBuka = new DateTime(2020, 12, 1),
                //    JenisObjekPajak = "Hotel Bintang Lima"
                //};

                //Perizinan = new DataPerizinan
                //{
                //    NomorIMB = "503/IMB/0192/2015",
                //    TanggalIMB = new DateTime(2015, 3, 15),
                //    NomorSITU_NIB = "8129381273981237",
                //    NomorIzinOperasional = "503/HO/093/2016"
                //};

                //Pendapatan = new DetailHotel.Pendapatan
                //{
                //    Okupansi = "85% per bulan",
                //    RataTarifKamar = 750000m,
                //    PendapatanKotor = 1250000000m,
                //    JumlahTransaksi = "+3.500 transaksi"
                //};

                //FasilitasPendukung = new DetailHotel.SaranaPendukung
                //{
                //    JumlahKaryawan = 58,
                //    MetodePenjualan = "Offline",
                //    MetodePembayaran = "Hibrid"
                //};

                //BanquetDetail = new DetailHotel.DetailBanquet
                //{
                //    Nama = "Ruang Serbaguna A",
                //    Jumlah = 3,
                //    JenisBanquet = 1, // Misal: 1 = Kecil, 2 = Sedang, 3 = Besar
                //    Kapasitas = 100,
                //    HargaSewa = 2500000,
                //    HargaPaket = 3500000,
                //    Okupansi = 85
                //};

                //FasilitasDetail = new List<DetailHotel.DetailFasilitas>
                //{
                //    new DetailHotel.DetailFasilitas { Nama = "Ruang Rapat", Jumlah = 3, Kapasitas = 50 },
                //    new DetailHotel.DetailFasilitas { Nama = "Ballroom", Jumlah = 1, Kapasitas = 500 },
                //    new DetailHotel.DetailFasilitas { Nama = "Kolam Renang", Jumlah = 1, Kapasitas = 100 },
                //    new DetailHotel.DetailFasilitas { Nama = "Restoran", Jumlah = 2, Kapasitas = 120 },
                //    new DetailHotel.DetailFasilitas { Nama = "Gym", Jumlah = 1, Kapasitas = 40 }
                //};

                //KamarDetail = new List<DetailHotel.DetailKamar>
                //{
                //    new DetailHotel.DetailKamar { Kamar = "Standard", Jumlah = 20, Tarif = 350000 },
                //    new DetailHotel.DetailKamar { Kamar = "Deluxe", Jumlah = 15, Tarif = 500000 },
                //    new DetailHotel.DetailKamar { Kamar = "Executive", Jumlah = 10, Tarif = 750000 },
                //    new DetailHotel.DetailKamar { Kamar = "Suite", Jumlah = 5, Tarif = 1200000 }
                //};

            }
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
        public class DetailHotel
        {
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
}
