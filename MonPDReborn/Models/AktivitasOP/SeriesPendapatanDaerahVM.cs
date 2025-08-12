using Microsoft.AspNetCore.Components.Web;
using MonPDLib;
using MonPDLib.EF;
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
            public List<PendapatanAsliDaerah> Data { get; set; } = new();
            public Show()
            {
                Data = Method.GetDataPendapatanAsliDaerahList();
            }
        }

        public class ShowPendapatanTransfer
        {
            public List<PendapatanTransfer> Data { get; set; } = new();
            public ShowPendapatanTransfer()
            {
                Data = Method.GetDataPendapatanTransferList();
            }
        }

        public class ShowLainLainPendapatan
        {
            public List<LainLainPendapatan> Data { get; set; } = new();
            public ShowLainLainPendapatan()
            {
                Data = Method.GetDataLainLainPendapatanList();
            }
        }

        public class ShowPenerimaanPembiayaan
        {
            public List<PenerimaanPembiayaan> Data { get; set; } = new();
            public ShowPenerimaanPembiayaan()
            {
                Data = Method.GetDataPenerimaanPembiayaanList();
            }
        }

        public class ShowTotal
        {
            public List<RingkasanPendapatan> Data { get; set; } = new();
            public ShowTotal()
            {
                Data = Method.GetDataRingkasanList();
            }
        }

        public static class Method
        {
            public static List<PendapatanAsliDaerah> GetDataPendapatanAsliDaerahList()
            {
                var ret = new List<PendapatanAsliDaerah>();
                var context = DBClass.GetContext();

                var data = context.TPendapatanDaerahs
                    .Where(x => x.Seq >= 1 && x.Seq <= 17)
                    .ToList();

                foreach (var item in data)
                {
                    var existing = ret.FirstOrDefault(x => x.Uraian == item.UraianRealisasi);
                    if (existing == null)
                    {
                        existing = new PendapatanAsliDaerah
                        {
                            ID = ret.Count + 1,
                            Uraian = item.UraianRealisasi ?? "",
                        };
                        ret.Add(existing);
                    }
                    switch (item.Tahun)
                    {
                        case 2019:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2020:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2021:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target6 = item.JumlahTarget ?? 0;
                            existing.Realisasi6 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target7 = item.JumlahTarget ?? 0;
                            existing.Realisasi7 = item.JumlahRealisasi ?? 0;
                            break;
                    }
                }
                return ret;
            }

            public static List<PendapatanTransfer> GetDataPendapatanTransferList()
            {
                var ret = new List<PendapatanTransfer>();
                var context = DBClass.GetContext();

                var data = context.TPendapatanDaerahs
                    .Where(x => x.Seq >= 18 && x.Seq <= 29)
                    .ToList();

                foreach (var item in data)
                {
                    var existing = ret.FirstOrDefault(x => x.Uraian == item.UraianRealisasi);
                    if (existing == null)
                    {
                        existing = new PendapatanTransfer
                        {
                            ID = ret.Count + 1,
                            Uraian = item.UraianRealisasi ?? "",
                        };
                        ret.Add(existing);
                    }
                    switch (item.Tahun)
                    {
                        case 2019:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2020:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2021:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target6 = item.JumlahTarget ?? 0;
                            existing.Realisasi6 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target7 = item.JumlahTarget ?? 0;
                            existing.Realisasi7 = item.JumlahRealisasi ?? 0;
                            break;
                    }
                }

                return ret;
            }

            public static List<LainLainPendapatan> GetDataLainLainPendapatanList()
            {
                var ret = new List<LainLainPendapatan>();
                var context = DBClass.GetContext();

                var data = context.TPendapatanDaerahs
                    .Where(x => x.Seq >= 30 && x.Seq <= 31)
                    .ToList();

                foreach (var item in data)
                {
                    var existing = ret.FirstOrDefault(x => x.Uraian == item.UraianRealisasi);
                    if (existing == null)
                    {
                        existing = new LainLainPendapatan
                        {
                            ID = ret.Count + 1,
                            Uraian = item.UraianRealisasi ?? "",
                        };
                        ret.Add(existing);
                    }
                    switch (item.Tahun)
                    {
                        case 2019:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2020:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2021:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target6 = item.JumlahTarget ?? 0;
                            existing.Realisasi6 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target7 = item.JumlahTarget ?? 0;
                            existing.Realisasi7 = item.JumlahRealisasi ?? 0;
                            break;
                    }
                }

                return ret;
            }

            public static List<PenerimaanPembiayaan> GetDataPenerimaanPembiayaanList()
            {
                var ret = new List<PenerimaanPembiayaan>();
                var context = DBClass.GetContext();

                var data = context.TPendapatanDaerahs
                    .Where(x => x.Seq >= 32 && x.Seq <= 35)
                    .ToList();

                foreach (var item in data)
                {
                    var existing = ret.FirstOrDefault(x => x.Uraian == item.UraianRealisasi);
                    if (existing == null)
                    {
                        existing = new PenerimaanPembiayaan
                        {
                            ID = ret.Count + 1,
                            Uraian = item.UraianRealisasi ?? "",
                        };
                        ret.Add(existing);
                    }
                    switch (item.Tahun)
                    {
                        case 2019:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2020:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2021:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target6 = item.JumlahTarget ?? 0;
                            existing.Realisasi6 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target7 = item.JumlahTarget ?? 0;
                            existing.Realisasi7 = item.JumlahRealisasi ?? 0;
                            break;
                    }
                }

                return ret;
            }

            public static List<RingkasanPendapatan> GetDataRingkasanList()
            {
                var result = new List<RingkasanPendapatan>();
                var context = DBClass.GetContext();

                // Ambil PENDAPATAN
                var dataPAD = context.TPendapatanDaerahs
                    .Where(x => x.UraianRealisasi == "PENDAPATAN")
                    .ToList();

                var ringkasanPAD = new RingkasanPendapatan
                {
                    Uraian = "PENDAPATAN",
                    Target1 = dataPAD.FirstOrDefault(x => x.Tahun == 2019)?.JumlahTarget ?? 0,
                    Realisasi1 = dataPAD.FirstOrDefault(x => x.Tahun == 2019)?.JumlahRealisasi ?? 0,
                    Target2 = dataPAD.FirstOrDefault(x => x.Tahun == 2020)?.JumlahTarget ?? 0,
                    Realisasi2 = dataPAD.FirstOrDefault(x => x.Tahun == 2020)?.JumlahRealisasi ?? 0,
                    Target3 = dataPAD.FirstOrDefault(x => x.Tahun == 2021)?.JumlahTarget ?? 0,
                    Realisasi3 = dataPAD.FirstOrDefault(x => x.Tahun == 2021)?.JumlahRealisasi ?? 0,
                    Target4 = dataPAD.FirstOrDefault(x => x.Tahun == 2022)?.JumlahTarget ?? 0,
                    Realisasi4 = dataPAD.FirstOrDefault(x => x.Tahun == 2022)?.JumlahRealisasi ?? 0,
                    Target5 = dataPAD.FirstOrDefault(x => x.Tahun == 2023)?.JumlahTarget ?? 0,
                    Realisasi5 = dataPAD.FirstOrDefault(x => x.Tahun == 2023)?.JumlahRealisasi ?? 0,
                    Target6 = dataPAD.FirstOrDefault(x => x.Tahun == 2024)?.JumlahTarget ?? 0,
                    Realisasi6 = dataPAD.FirstOrDefault(x => x.Tahun == 2024)?.JumlahRealisasi ?? 0,
                    Target7 = dataPAD.FirstOrDefault(x => x.Tahun == 2025)?.JumlahTarget ?? 0,
                    Realisasi7 = dataPAD.FirstOrDefault(x => x.Tahun == 2025)?.JumlahRealisasi ?? 0
                };

                result.Add(ringkasanPAD);

                // Ambil PENDAPATAN TRANSFER
                var dataTransfer = context.TPendapatanDaerahs
                    .Where(x => x.Seq >= 18 && x.Seq <= 29)
                    .ToList();

                var ringkasanTransfer = new RingkasanPendapatan
                {
                    Uraian = "PENDAPATAN TRANSFER",
                    Target1 = dataTransfer.FirstOrDefault(x => x.Tahun == 2019)?.JumlahTarget ?? 0,
                    Realisasi1 = dataTransfer.FirstOrDefault(x => x.Tahun == 2019)?.JumlahRealisasi ?? 0,
                    Target2 = dataTransfer.FirstOrDefault(x => x.Tahun == 2020)?.JumlahTarget ?? 0,
                    Realisasi2 = dataTransfer.FirstOrDefault(x => x.Tahun == 2020)?.JumlahRealisasi ?? 0,
                    Target3 = dataTransfer.FirstOrDefault(x => x.Tahun == 2021)?.JumlahTarget ?? 0,
                    Realisasi3 = dataTransfer.FirstOrDefault(x => x.Tahun == 2021)?.JumlahRealisasi ?? 0,
                    Target4 = dataTransfer.FirstOrDefault(x => x.Tahun == 2022)?.JumlahTarget ?? 0,
                    Realisasi4 = dataTransfer.FirstOrDefault(x => x.Tahun == 2022)?.JumlahRealisasi ?? 0,
                    Target5 = dataTransfer.FirstOrDefault(x => x.Tahun == 2023)?.JumlahTarget ?? 0,
                    Realisasi5 = dataTransfer.FirstOrDefault(x => x.Tahun == 2023)?.JumlahRealisasi ?? 0,
                    Target6 = dataTransfer.FirstOrDefault(x => x.Tahun == 2024)?.JumlahTarget ?? 0,
                    Realisasi6 = dataTransfer.FirstOrDefault(x => x.Tahun == 2024)?.JumlahRealisasi ?? 0,
                    Target7 = dataTransfer.FirstOrDefault(x => x.Tahun == 2025)?.JumlahTarget ?? 0,
                    Realisasi7 = dataTransfer.FirstOrDefault(x => x.Tahun == 2025)?.JumlahRealisasi ?? 0
                };

                result.Add(ringkasanTransfer);

                // Ambil LAIN-LAIN PENDAPATAN
                var dataLain = context.TPendapatanDaerahs
                    .Where(x => x.Seq >= 30 && x.Seq <= 31)
                    .ToList();

                var ringkasanLain = new RingkasanPendapatan
                {
                    Uraian = "Lain - lain Pendapatan yang Sah",
                    Target1 = dataLain.FirstOrDefault(x => x.Tahun == 2019)?.JumlahTarget ?? 0,
                    Realisasi1 = dataLain.FirstOrDefault(x => x.Tahun == 2019)?.JumlahRealisasi ?? 0,
                    Target2 = dataLain.FirstOrDefault(x => x.Tahun == 2020)?.JumlahTarget ?? 0,
                    Realisasi2 = dataLain.FirstOrDefault(x => x.Tahun == 2020)?.JumlahRealisasi ?? 0,
                    Target3 = dataLain.FirstOrDefault(x => x.Tahun == 2021)?.JumlahTarget ?? 0,
                    Realisasi3 = dataLain.FirstOrDefault(x => x.Tahun == 2021)?.JumlahRealisasi ?? 0,
                    Target4 = dataLain.FirstOrDefault(x => x.Tahun == 2022)?.JumlahTarget ?? 0,
                    Realisasi4 = dataLain.FirstOrDefault(x => x.Tahun == 2022)?.JumlahRealisasi ?? 0,
                    Target5 = dataLain.FirstOrDefault(x => x.Tahun == 2023)?.JumlahTarget ?? 0,
                    Realisasi5 = dataLain.FirstOrDefault(x => x.Tahun == 2023)?.JumlahRealisasi ?? 0,
                    Target6 = dataLain.FirstOrDefault(x => x.Tahun == 2024)?.JumlahTarget ?? 0,
                    Realisasi6 = dataLain.FirstOrDefault(x => x.Tahun == 2024)?.JumlahRealisasi ?? 0,
                    Target7 = dataLain.FirstOrDefault(x => x.Tahun == 2025)?.JumlahTarget ?? 0,
                    Realisasi7 = dataLain.FirstOrDefault(x => x.Tahun == 2025)?.JumlahRealisasi ?? 0
                };

                result.Add(ringkasanLain);

                // Ambil PENERIMAAN PEMBIAYAAN
                var dataPenerimaan = context.TPendapatanDaerahs
                    .Where(x => x.Seq >= 32 && x.Seq <= 35)
                    .ToList();

                var ringkasanPenerimaan = new RingkasanPendapatan
                {
                    Uraian = "Lain - lain Pendapatan yang Sah",
                    Target1 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2019)?.JumlahTarget ?? 0,
                    Realisasi1 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2019)?.JumlahRealisasi ?? 0,
                    Target2 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2020)?.JumlahTarget ?? 0,
                    Realisasi2 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2020)?.JumlahRealisasi ?? 0,
                    Target3 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2021)?.JumlahTarget ?? 0,
                    Realisasi3 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2021)?.JumlahRealisasi ?? 0,
                    Target4 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2022)?.JumlahTarget ?? 0,
                    Realisasi4 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2022)?.JumlahRealisasi ?? 0,
                    Target5 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2023)?.JumlahTarget ?? 0,
                    Realisasi5 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2023)?.JumlahRealisasi ?? 0,
                    Target6 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2024)?.JumlahTarget ?? 0,
                    Realisasi6 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2024)?.JumlahRealisasi ?? 0,
                    Target7 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2025)?.JumlahTarget ?? 0,
                    Realisasi7 = dataPenerimaan.FirstOrDefault(x => x.Tahun == 2025)?.JumlahRealisasi ?? 0
                };

                result.Add(ringkasanPenerimaan);

                return result;
            }

        }
        public class PendapatanAsliDaerah
        {
            public int ID { get; set; }
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
            public decimal Target6 { get; set; }
            public decimal Realisasi6 { get; set; }
            public decimal Persentase6 => Target6 > 0 ? (Realisasi6 / Target6) * 100 : 0;
            public decimal Target7 { get; set; }
            public decimal Realisasi7 { get; set; }
            public decimal Persentase7 => Target7 > 0 ? (Realisasi7 / Target7) * 100 : 0;
        }

        public class PendapatanTransfer
        {
            public int ID { get; set; }
            public string Uraian { get; set; } = "";
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
            public decimal Target6 { get; set; }
            public decimal Realisasi6 { get; set; }
            public decimal Persentase6 => Target6 > 0 ? (Realisasi6 / Target6) * 100 : 0;
            public decimal Target7 { get; set; }
            public decimal Realisasi7 { get; set; }
            public decimal Persentase7 => Target7 > 0 ? (Realisasi7 / Target7) * 100 : 0;
        }

        public class LainLainPendapatan
        {
            public int ID { get; set; }
            public string Uraian { get; set; } = "";
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
            public decimal Target6 { get; set; }
            public decimal Realisasi6 { get; set; }
            public decimal Persentase6 => Target6 > 0 ? (Realisasi6 / Target6) * 100 : 0;
            public decimal Target7 { get; set; }
            public decimal Realisasi7 { get; set; }
            public decimal Persentase7 => Target7 > 0 ? (Realisasi7 / Target7) * 100 : 0;
        }

        public class PenerimaanPembiayaan
        {
            public int ID { get; set; }
            public string Uraian { get; set; } = "";
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
            public decimal Target6 { get; set; }
            public decimal Realisasi6 { get; set; }
            public decimal Persentase6 => Target6 > 0 ? (Realisasi6 / Target6) * 100 : 0;
            public decimal Target7 { get; set; }
            public decimal Realisasi7 { get; set; }
            public decimal Persentase7 => Target7 > 0 ? (Realisasi7 / Target7) * 100 : 0;
        }

        public class PendapatanTotal
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
            public decimal Target6 { get; set; }
            public decimal Realisasi6 { get; set; }
            public decimal Persentase6 => Target6 > 0 ? (Realisasi6 / Target6) * 100 : 0;
            public decimal Target7 { get; set; }
            public decimal Realisasi7 { get; set; }
            public decimal Persentase7 => Target7 > 0 ? (Realisasi7 / Target7) * 100 : 0;
        }
        public class RingkasanPendapatan
        {
            public string Uraian { get; set; } = "";
            public int ID { get; set; }
            public string Kategori { get; set; } = "";

            public decimal Target1 { get; set; }
            public decimal Realisasi1 { get; set; }
            public decimal Persentase1 => Target1 == 0 ? 0 : Realisasi1 / Target1 * 100;

            public decimal Target2 { get; set; }
            public decimal Realisasi2 { get; set; }
            public decimal Persentase2 => Target2 == 0 ? 0 : Realisasi2 / Target2 * 100;

            public decimal Target3 { get; set; }
            public decimal Realisasi3 { get; set; }
            public decimal Persentase3 => Target3 == 0 ? 0 : Realisasi3 / Target3 * 100;

            public decimal Target4 { get; set; }
            public decimal Realisasi4 { get; set; }
            public decimal Persentase4 => Target4 == 0 ? 0 : Realisasi4 / Target4 * 100;

            public decimal Target5 { get; set; }
            public decimal Realisasi5 { get; set; }
            public decimal Persentase5 => Target5 == 0 ? 0 : Realisasi5 / Target5 * 100;

            public decimal Target6 { get; set; }
            public decimal Realisasi6 { get; set; }
            public decimal Persentase6 => Target6 > 0 ? (Realisasi6 / Target6) * 100 : 0;

            public decimal Target7 { get; set; }
            public decimal Realisasi7 { get; set; }
            public decimal Persentase7 => Target7 > 0 ? (Realisasi7 / Target7) * 100 : 0;
        }


    }
}