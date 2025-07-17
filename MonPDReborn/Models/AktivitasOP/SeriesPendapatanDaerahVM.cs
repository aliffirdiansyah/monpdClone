using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonPDReborn.Models.AktivitasOP // Pastikan namespace benar
{
    public class SeriesPendapatanDaerahVM
    {
        public class Index { }

        public class Show
        {
            public List<PenerimaanInduk> Data { get; set; } = new();
            public Show()
            {
                Data = Method.GetData();
            }
        }

        public class ShowPendapatanTransfer
        {
            public List<DataTransfer> Data { get; set; } = new();
            public ShowPendapatanTransfer()
            {
                Data = Method.GetDataPendapatanTransfer();
            }
        }

        public static class Method
        {
            public static List<PenerimaanInduk> GetData()
            {
                var detailPajakDaerah = new List<PenerimaanDetail>
                {
                    new() { Uraian = "Pajak Hotel", Target1 = 1200, Realisasi1 = 1150, Target2 = 1300, Realisasi2 = 1250, Target3 = 1400, Realisasi3 = 1350, Target4 = 1500, Realisasi4 = 1450, Target5 = 1600, Realisasi5 = 1550 },
                    new() { Uraian = "Pajak Restoran", Target1 = 2400, Realisasi1 = 2450, Target2 = 2500, Realisasi2 = 2550, Target3 = 2600, Realisasi3 = 2650, Target4 = 2700, Realisasi4 = 2750, Target5 = 2800, Realisasi5 = 2850 },
                    new() { Uraian = "Pajak Hiburan", Target1 = 600, Realisasi1 = 590, Target2 = 650, Realisasi2 = 640, Target3 = 700, Realisasi3 = 680, Target4 = 750, Realisasi4 = 780, Target5 = 800, Realisasi5 = 820 },
                    new() { Uraian = "Pajak Reklame", Target1 = 840, Realisasi1 = 840, Target2 = 850, Realisasi2 = 860, Target3 = 860, Realisasi3 = 870, Target4 = 870, Realisasi4 = 880, Target5 = 880, Realisasi5 = 890 },
                    new() { Uraian = "Pajak Penerangan Jalan", Target1 = 4000, Realisasi1 = 4100, Target2 = 4200, Realisasi2 = 4150, Target3 = 4400, Realisasi3 = 4500, Target4 = 4600, Realisasi4 = 4550, Target5 = 4800, Realisasi5 = 4900 },
                    new() { Uraian = "Pajak Parkir", Target1 = 700, Realisasi1 = 720, Target2 = 750, Realisasi2 = 760, Target3 = 800, Realisasi3 = 790, Target4 = 850, Realisasi4 = 860, Target5 = 900, Realisasi5 = 910 },
                    new() { Uraian = "Pajak Air Tanah", Target1 = 200, Realisasi1 = 180, Target2 = 210, Realisasi2 = 200, Target3 = 220, Realisasi3 = 230, Target4 = 230, Realisasi4 = 220, Target5 = 240, Realisasi5 = 250 },
                    new() { Uraian = "PBB-P2", Target1 = 2500, Realisasi1 = 2400, Target2 = 2600, Realisasi2 = 2550, Target3 = 2700, Realisasi3 = 2800, Target4 = 2800, Realisasi4 = 2750, Target5 = 2900, Realisasi5 = 3000 },
                    new() { Uraian = "BPHTB", Target1 = 800, Realisasi1 = 820, Target2 = 850, Realisasi2 = 860, Target3 = 900, Realisasi3 = 890, Target4 = 950, Realisasi4 = 960, Target5 = 1000, Realisasi5 = 1010 },
                    new() { Uraian = "Opsen PKB", Target1 = 0, Realisasi1 = 0, Target2 = 0, Realisasi2 = 0, Target3 = 0, Realisasi3 = 0, Target4 = 0, Realisasi4 = 0, Target5 = 0, Realisasi5 = 0 },
                    new() { Uraian = "Opsen BBNKB", Target1 = 0, Realisasi1 = 0, Target2 = 0, Realisasi2 = 0, Target3 = 0, Realisasi3 = 0, Target4 = 0, Realisasi4 = 0, Target5 = 0, Realisasi5 = 0 }
                };


                return new List<PenerimaanInduk>
                {
                    new() {
                        ID = 1, JenisPajak = "Pajak Daerah",
                        Target1 = detailPajakDaerah.Sum(d => d.Target1), Realisasi1 = detailPajakDaerah.Sum(d => d.Realisasi1),
                        Target2 = detailPajakDaerah.Sum(d => d.Target2), Realisasi2 = detailPajakDaerah.Sum(d => d.Realisasi2),
                        Target3 = detailPajakDaerah.Sum(d => d.Target3), Realisasi3 = detailPajakDaerah.Sum(d => d.Realisasi3),
                        Target4 = detailPajakDaerah.Sum(d => d.Target4), Realisasi4 = detailPajakDaerah.Sum(d => d.Realisasi4),
                        Target5 = detailPajakDaerah.Sum(d => d.Target5), Realisasi5 = detailPajakDaerah.Sum(d => d.Realisasi5),
                        DetailItems = detailPajakDaerah
                    },
                    new() {
                        ID = 2, JenisPajak = "Retribusi Daerah",
                        Target1 = 3000, Realisasi1 = 2800, Target2 = 3200, Realisasi2 = 3100,
                        Target3 = 3400, Realisasi3 = 3500, Target4 = 3600, Realisasi4 = 3550,
                        Target5 = 3800, Realisasi5 = 3900
                    },
                    new() {
                        ID = 3, JenisPajak = "Hasil Pengelolaan Kekayaan Daerah yang Dipisahkan",
                        Target1 = 1500, Realisasi1 = 1600, Target2 = 1550, Realisasi2 = 1580,
                        Target3 = 1600, Realisasi3 = 1610, Target4 = 1650, Realisasi4 = 1680,
                        Target5 = 1700, Realisasi5 = 1720
                    },
                    new() {
                        ID = 4, JenisPajak = "Lain-lain PAD yang Sah",
                        Target1 = 500, Realisasi1 = 550, Target2 = 520, Realisasi2 = 530,
                        Target3 = 540, Realisasi3 = 550, Target4 = 560, Realisasi4 = 555,
                        Target5 = 580, Realisasi5 = 600
                    }
                };
            }

            public static List<DataTransfer> GetDataPendapatanTransfer()
            {
                // Buat data detail untuk 'Pemerintah Pusat'
                var detailPusat = new List<DataTransfer> {
                new() { ID = 101, JenisPendapatan = "Insentif Fiskal", Target3 = 500, Realisasi3 = 480 },
                new() { ID = 102, JenisPendapatan = "Dana Bagi Hasil (DBH)", Target3 = 1200, Realisasi3 = 1250 },
                new() { ID = 103, JenisPendapatan = "Dana Alokasi Umum", Target3 = 2000, Realisasi3 = 2000 },
                new() { ID = 104, JenisPendapatan = "Dana Alokasi Khusus (DAK) Fisik", Target3 = 800, Realisasi3 = 750 },
                new() { ID = 105, JenisPendapatan = "Dana Alokasi Khusus (DAK) Non Fisik", Target3 = 900, Realisasi3 = 910 }
            };

                // Buat data detail untuk 'Antar Daerah'
                var detailAntarDaerah = new List<DataTransfer> {
                new() { ID = 201, JenisPendapatan = "Pendapatan Bagi Hasil", Target3 = 300, Realisasi3 = 310 },
                new() { ID = 202, JenisPendapatan = "Bantuan Keuangan", Target3 = 400, Realisasi3 = 390 }
            };

                return new List<DataTransfer>
            {
                // Baris Induk 1
                new() {
                    ID = 1, JenisPendapatan = "Pendapatan Transfer Pemerintah Pusat",
                    // Total adalah penjumlahan dari detailnya
                    Target3 = detailPusat.Sum(d => d.Target3),
                    Realisasi3 = detailPusat.Sum(d => d.Realisasi3),
                    DetailItems = detailPusat
                },
                // Baris Induk 2
                new() {
                    ID = 2, JenisPendapatan = "Pendapatan Transfer Antar Daerah",
                    Target3 = detailAntarDaerah.Sum(d => d.Target3),
                    Realisasi3 = detailAntarDaerah.Sum(d => d.Realisasi3),
                    DetailItems = detailAntarDaerah
                }
            };
            }
        }
        public class PenerimaanInduk
        {
            public int ID { get; set; }
            public string JenisPajak { get; set; } = "";
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Persentase1 => Target1 > 0 ? (Realisasi1 / Target1) * 100 : 0;
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Persentase2 => Target2 > 0 ? (Realisasi2 / Target2) * 100 : 0;
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Persentase3 => Target3 > 0 ? (Realisasi3 / Target3) * 100 : 0;
            public decimal Target4 { get; set; }
            public decimal Realisasi4 { get; set; }
            public decimal Persentase4 => Target4 > 0 ? (Realisasi4 / Target4) * 100 : 0;
            public decimal Target5 { get; set; }
            public decimal Realisasi5 { get; set; }
            public decimal Persentase5 => Target5 > 0 ? (Realisasi5 / Target5) * 100 : 0;
            public List<PenerimaanDetail>? DetailItems { get; set; }
        }

        public class PenerimaanDetail
        {
            public string Uraian { get; set; } = "";
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Persentase1 => Target1 > 0 ? (Realisasi1 / Target1) * 100 : 0;
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Persentase2 => Target2 > 0 ? (Realisasi2 / Target2) * 100 : 0;
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Persentase3 => Target3 > 0 ? (Realisasi3 / Target3) * 100 : 0;
            public decimal Target4 { get; set; }
            public decimal Realisasi4 { get; set; }
            public decimal Persentase4 => Target4 > 0 ? (Realisasi4 / Target4) * 100 : 0;
            public decimal Target5 { get; set; }
            public decimal Realisasi5 { get; set; }
            public decimal Persentase5 => Target5 > 0 ? (Realisasi5 / Target5) * 100 : 0;
        }

        public class DataTransfer
        {
            public int ID { get; set; }
            public string JenisPendapatan { get; set; } = "";
            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Persentase1 => Target1 > 0 ? (Realisasi1 / Target1) * 100 : 0;
            // ... properti untuk tahun 2, 3, 4, 5 sama
            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Persentase2 => Target2 > 0 ? (Realisasi2 / Target2) * 100 : 0;
            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Persentase3 => Target3 > 0 ? (Realisasi3 / Target3) * 100 : 0;
            public decimal Target4 { get; set; }
            public decimal Realisasi4 { get; set; }
            public decimal Persentase4 => Target4 > 0 ? (Realisasi4 / Target4) * 100 : 0;
            public decimal Target5 { get; set; }
            public decimal Realisasi5 { get; set; }
            public decimal Persentase5 => Target5 > 0 ? (Realisasi5 / Target5) * 100 : 0;
            public List<DataTransfer>? DetailItems { get; set; } // Properti untuk sub-baris
        }
    }
}