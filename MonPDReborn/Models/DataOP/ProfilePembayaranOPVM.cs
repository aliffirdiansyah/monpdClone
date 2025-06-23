using System;
using System.Collections.Generic;
using System.Linq; // diperlukan untuk .Select()

namespace MonPDReborn.Models.DataOP
{
    public class ProfilePembayaranOPVM
    {
        public class Index
        {
            public IdentitasObjekPajak IdentitasPajak { get; set; }

            public MasaPajak MasaPajakData { get; set; } // PASTIKAN PUBLIC

            public MasaBayar MasaBayarData { get; set; } // PASTIKAN PUBLIC

            public Index()
            {
                IdentitasPajak = new IdentitasObjekPajak
                {
                    NOP = "35.78.120.120.0001.0",
                    NamaObjekPajak = "Hotel Adam",
                    AlamatLengkap = "Jl. Ada",
                    Kecamatan_Kelurahan = "Ada - Ada"
                };

                var dataRealisasiBulanan = new List<DataRealisasiPajak>
                {
                    new DataRealisasiPajak { Bulan = "Januari", TglSSPD = new DateTime(2025, 1, 15), Realisasi = 1000000m },
                    new DataRealisasiPajak { Bulan = "Februari", TglSSPD = new DateTime(2025, 2, 15), Realisasi = 1200000m },
                    new DataRealisasiPajak { Bulan = "Maret", TglSSPD = new DateTime(2025, 3, 15), Realisasi = 1300000m },
                    new DataRealisasiPajak { Bulan = "April", TglSSPD = new DateTime(2025, 4, 15), Realisasi = 1250000m },
                    new DataRealisasiPajak { Bulan = "Mei", TglSSPD = new DateTime(2025, 5, 15), Realisasi = 1400000m },
                    new DataRealisasiPajak { Bulan = "Juni", TglSSPD = new DateTime(2025, 6, 15), Realisasi = 1100000m },
                    new DataRealisasiPajak { Bulan = "Juli", TglSSPD = new DateTime(2025, 7, 15), Realisasi = 1500000m },
                    new DataRealisasiPajak { Bulan = "Agustus", TglSSPD = new DateTime(2025, 8, 15), Realisasi = 1600000m },
                    new DataRealisasiPajak { Bulan = "September", TglSSPD = new DateTime(2025, 9, 15), Realisasi = 1700000m },
                    new DataRealisasiPajak { Bulan = "Oktober", TglSSPD = new DateTime(2025, 10, 15), Realisasi = 1800000m },
                    new DataRealisasiPajak { Bulan = "November", TglSSPD = new DateTime(2025, 11, 15), Realisasi = 1900000m },
                    new DataRealisasiPajak { Bulan = "Desember", TglSSPD = new DateTime(2025, 12, 15), Realisasi = 2000000m }
                };


                MasaPajakData = new MasaPajak
                {
                    Tahun = 2025,
                    DataRealisasi = dataRealisasiBulanan.Select(d => new DataRealisasiPajak
                    {
                        Bulan = d.Bulan,
                        TglSSPD = d.TglSSPD,
                        Realisasi = d.Realisasi
                    }).ToList(),
                    Total = dataRealisasiBulanan.Sum(d => d.Realisasi)
                };


                var dataRealisasiBayar = new List<DataRealisasiPajak>
                {
                    new DataRealisasiPajak { Bulan = "Januari", TglSSPD = new DateTime(2025, 2, 10), Realisasi = 950000m },
                    new DataRealisasiPajak { Bulan = "Februari", TglSSPD = new DateTime(2025, 3, 12), Realisasi = 1150000m },
                    new DataRealisasiPajak { Bulan = "Maret", TglSSPD = new DateTime(2025, 4, 9), Realisasi = 1280000m },
                    new DataRealisasiPajak { Bulan = "April", TglSSPD = new DateTime(2025, 5, 15), Realisasi = 1225000m },
                    new DataRealisasiPajak { Bulan = "Mei", TglSSPD = new DateTime(2025, 6, 17), Realisasi = 1380000m },
                    new DataRealisasiPajak { Bulan = "Juni", TglSSPD = new DateTime(2025, 7, 10), Realisasi = 1050000m },
                    new DataRealisasiPajak { Bulan = "Juli", TglSSPD = new DateTime(2025, 8, 13), Realisasi = 1450000m },
                    new DataRealisasiPajak { Bulan = "Agustus", TglSSPD = new DateTime(2025, 9, 11), Realisasi = 1580000m },
                    new DataRealisasiPajak { Bulan = "September", TglSSPD = new DateTime(2025, 10, 14), Realisasi = 1650000m },
                    new DataRealisasiPajak { Bulan = "Oktober", TglSSPD = new DateTime(2025, 11, 10), Realisasi = 1750000m },
                    new DataRealisasiPajak { Bulan = "November", TglSSPD = new DateTime(2025, 12, 9), Realisasi = 1850000m },
                    new DataRealisasiPajak { Bulan = "Desember", TglSSPD = new DateTime(2026, 1, 10), Realisasi = 1950000m }
                };

                MasaBayarData = new MasaBayar
                {
                    Tahun = 2025,
                    DataRealisasi = dataRealisasiBayar.Select(d => new DataRealisasiPajak
                    {
                        Bulan = d.Bulan,
                        TglSSPD = d.TglSSPD,
                        Realisasi = d.Realisasi
                    }).ToList(),
                    Total = dataRealisasiBayar.Sum(d => d.Realisasi)
                };
            }
        }


        public class Show
        {
            // Tambahkan properti atau method jika diperlukan
        }

        public class Detail
        {
            // Tambahkan properti atau method jika diperlukan
        }

        public class Method
        {
            // Tambahkan properti atau method jika diperlukan
        }

        // Kelas identitas objek pajak
        public class IdentitasObjekPajak
        {
            public string NOP { get; set; }
            public string NamaObjekPajak { get; set; }
            public string AlamatLengkap { get; set; }
            public string Kecamatan_Kelurahan { get; set; }

            // Bisa ditambahkan jika perlu:
            // public string Telp { get; set; }
            // public DateTime TanggalBuka { get; set; }
            // public string JenisObjekPajak { get; set; }
        }

        public class MasaPajak
        {
            public int Tahun { get; set; }
            public List<DataRealisasiPajak> DataRealisasi { get; set; }
            public decimal Total { get; set; }

            public MasaPajak()
            {
                DataRealisasi = new List<DataRealisasiPajak>();
            }
        }

        public class MasaBayar
        {
            public int Tahun { get; set; }
            public List<DataRealisasiPajak> DataRealisasi { get; set; }
            public decimal Total { get; set; }

            public MasaBayar()
            {
                DataRealisasi = new List<DataRealisasiPajak>();
            }
        }

        public class DataRealisasiPajak
        {
            public string Bulan { get; set; } // "Januari", "Februari", dst
            public DateTime? TglSSPD { get; set; } // null jika belum dibayar
            public decimal Realisasi { get; set; } // nominal
        }
    }
}
