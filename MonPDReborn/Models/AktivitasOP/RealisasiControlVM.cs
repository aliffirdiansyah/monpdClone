using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MonPDReborn.Models.AktivitasOP // Sesuaikan dengan namespace proyek Anda
{
    public class RealisasiControlVM
    {
        // Bagian untuk Halaman Utama (Index)
        public class Index
        {
            public DashboardData Data { get; set; } = new();
            public Index()
            {
                Data = Method.GetDashboardData();
            }
        }

        // Bagian untuk Tabel Pertama (Show)
        public class Show
        {
            public List<DataRealisasi> DataRealisasiList { get; set; } = new();
            public Show()
            {
                DataRealisasiList = Method.GetDataRealisasi();
            }
        }

        // Bagian untuk Tabel Kedua (Detail)
        public class Detail
        {
            public List<DataDetailRealisasi> DataDetailList { get; set; } = new();
            public Detail()
            {
                DataDetailList = Method.GetDataDetail();
            }
        }

        // Kelas untuk semua logika pengambilan data (saat ini dummy)
        public class Method
        {
            public static DashboardData GetDashboardData()
            {
                return new DashboardData
                {
                    TotalTarget = 15000000000,
                    TotalRealisasi = 12500000000,
                    PersentaseCapaian = (12500000000m / 15000000000m) * 100
                };
            }

            public static List<DataRealisasi> GetDataRealisasi()
            {
                return new List<DataRealisasi>
                {
                    new() {
                        No = 1, JenisPajak = "Pajak Hotel", Target = 3000000000,
                        PembayaranBulanIni = new() { AKP = 250000000, Realisasi = 240000000, Persen = 96 },
                        PembayaranSDBI = new() { AKP = 1500000000, Realisasi = 1450000000, PersenAkpTarget = 95, PersenAkpRealisasi = 98, PersenTarget = 96.67m }
                    },
                    new() {
                        No = 2, JenisPajak = "Pajak Restoran", Target = 5000000000,
                        PembayaranBulanIni = new() { AKP = 400000000, Realisasi = 410000000, Persen = 102.5m },
                        PembayaranSDBI = new() { AKP = 2500000000, Realisasi = 2600000000, PersenAkpTarget = 100, PersenAkpRealisasi = 101, PersenTarget = 104 }
                    },
                    new() {
                        No = 3, JenisPajak = "Pajak Hiburan", Target = 1500000000,
                        PembayaranBulanIni = new() { AKP = 120000000, Realisasi = 110000000, Persen = 91.67m },
                        PembayaranSDBI = new() { AKP = 700000000, Realisasi = 650000000, PersenAkpTarget = 90, PersenAkpRealisasi = 92, PersenTarget = 92.86m }
                    }
                };
            }

            public static List<DataDetailRealisasi> GetDataDetail()
            {
                return new List<DataDetailRealisasi>
                {
                    new() { No = 1, JenisPajak = "Pajak Hotel",
                        UPTB1 = new() { Target = 500, Realisasi = 480, Persen = 96 },
                        UPTB2 = new() { Target = 600, Realisasi = 590, Persen = 98.33m },
                        UPTB3 = new() { Target = 700, Realisasi = 680, Persen = 97.14m },
                        UPTB4 = new() { Target = 400, Realisasi = 400, Persen = 100 },
                        UPTB5 = new() { Target = 800, Realisasi = 750, Persen = 93.75m },
                        Bidang = new() { Target = 3000, Realisasi = 2900, Persen = 96.67m }
                    },
                     new() { No = 2, JenisPajak = "Pajak Restoran",
                        UPTB1 = new() { Target = 1000, Realisasi = 1050, Persen = 105 },
                        UPTB2 = new() { Target = 1200, Realisasi = 1200, Persen = 100 },
                        UPTB3 = new() { Target = 900, Realisasi = 850, Persen = 94.44m },
                        UPTB4 = new() { Target = 800, Realisasi = 810, Persen = 101.25m },
                        UPTB5 = new() { Target = 1100, Realisasi = 1150, Persen = 104.55m },
                        Bidang = new() { Target = 5000, Realisasi = 5060, Persen = 101.2m }
                    },
                };
            }
        }

        // == KELAS-KELAS UNTUK MENAMPUNG DATA ==
        public class DashboardData
        {
            public decimal TotalTarget { get; set; }
            public decimal TotalRealisasi { get; set; }
            public decimal PersentaseCapaian { get; set; }
        }

        public class DataRealisasi
        {
            public int No { get; set; }
            public string JenisPajak { get; set; } = "";
            public decimal Target { get; set; }
            public PembayaranDetail PembayaranBulanIni { get; set; } = new();
            public PembayaranDetailSDBI PembayaranSDBI { get; set; } = new();
        }

        public class PembayaranDetail
        {
            public decimal AKP { get; set; }
            public decimal Realisasi { get; set; }
            public decimal Persen { get; set; }
        }

        public class PembayaranDetailSDBI : PembayaranDetail
        {
            public decimal PersenAkpTarget { get; set; }
            public decimal PersenAkpRealisasi { get; set; }
            public decimal PersenTarget { get; set; }
        }

        public class DataDetailRealisasi
        {
            public int No { get; set; }
            public string JenisPajak { get; set; } = "";
            public RealisasiPerLokasi UPTB1 { get; set; } = new();
            public RealisasiPerLokasi UPTB2 { get; set; } = new();
            public RealisasiPerLokasi UPTB3 { get; set; } = new();
            public RealisasiPerLokasi UPTB4 { get; set; } = new();
            public RealisasiPerLokasi UPTB5 { get; set; } = new();
            public RealisasiPerLokasi Bidang { get; set; } = new();
        }

        public class RealisasiPerLokasi
        {
            public decimal Target { get; set; }
            public decimal Realisasi { get; set; }
            public decimal Persen { get; set; }
        }
    }
}
