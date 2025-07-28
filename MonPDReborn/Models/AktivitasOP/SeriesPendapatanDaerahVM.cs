using Microsoft.AspNetCore.Components.Web;
using MonPDLib;
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
                        
                        case 2021:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
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
                        case 2021:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
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
                        case 2021:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
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
                        case 2021:
                            existing.Target1 = item.JumlahTarget ?? 0;
                            existing.Realisasi1 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2022:
                            existing.Target2 = item.JumlahTarget ?? 0;
                            existing.Realisasi2 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2023:
                            existing.Target3 = item.JumlahTarget ?? 0;
                            existing.Realisasi3 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2024:
                            existing.Target4 = item.JumlahTarget ?? 0;
                            existing.Realisasi4 = item.JumlahRealisasi ?? 0;
                            break;
                        case 2025:
                            existing.Target5 = item.JumlahTarget ?? 0;
                            existing.Realisasi5 = item.JumlahRealisasi ?? 0;
                            break;
                    }
                }

                return ret;
            }
            public static List<RingkasanPendapatan> GetDataRingkasanList()
            {
                var result = new List<RingkasanPendapatan>();

                // 1. Pendapatan Asli Daerah
                var pendapatanList = GetDataPendapatanAsliDaerahList();

                var ringkasan = new RingkasanPendapatan
                {
                    Uraian = "Pendapatan Asli Daerah",
                    Target1 = pendapatanList.Sum(x => x.Target1),
                    Realisasi1 = pendapatanList.Sum(x => x.Realisasi1),
                    Target2 = pendapatanList.Sum(x => x.Target2),
                    Realisasi2 = pendapatanList.Sum(x => x.Realisasi2),
                    Target3 = pendapatanList.Sum(x => x.Target3),
                    Realisasi3 = pendapatanList.Sum(x => x.Realisasi3),
                    Target4 = pendapatanList.Sum(x => x.Target4),
                    Realisasi4 = pendapatanList.Sum(x => x.Realisasi4),
                    Target5 = pendapatanList.Sum(x => x.Target5),
                    Realisasi5 = pendapatanList.Sum(x => x.Realisasi5)
                };

                return new List<RingkasanPendapatan> { ringkasan };

                // 2. Pendapatan Transfer
                var listTransfer = GetDataPendapatanTransferList();
                var totalTransfer = new RingkasanPendapatan
                {
                    Uraian = "Pendapatan Transfer",
                    Target1 = listTransfer.Sum(x => x.Target1),
                    Realisasi1 = listTransfer.Sum(x => x.Realisasi1),
                    Target2 = listTransfer.Sum(x => x.Target2),
                    Realisasi2 = listTransfer.Sum(x => x.Realisasi2),
                    Target3 = listTransfer.Sum(x => x.Target3),
                    Realisasi3 = listTransfer.Sum(x => x.Realisasi3),
                    Target4 = listTransfer.Sum(x => x.Target4),
                    Realisasi4 = listTransfer.Sum(x => x.Realisasi4),
                    Target5 = listTransfer.Sum(x => x.Target5),
                    Realisasi5 = listTransfer.Sum(x => x.Realisasi5),
                };
                result.Add(totalTransfer);

                // 3. Lain-lain Pendapatan Daerah yang Sah
                var listLain = GetDataLainLainPendapatanList();
                var totalLain = new RingkasanPendapatan
                {
                    Uraian = "Lain-lain Pendapatan Daerah yang Sah",
                    Target1 = listLain.Sum(x => x.Target1),
                    Realisasi1 = listLain.Sum(x => x.Realisasi1),
                    Target2 = listLain.Sum(x => x.Target2),
                    Realisasi2 = listLain.Sum(x => x.Realisasi2),
                    Target3 = listLain.Sum(x => x.Target3),
                    Realisasi3 = listLain.Sum(x => x.Realisasi3),
                    Target4 = listLain.Sum(x => x.Target4),
                    Realisasi4 = listLain.Sum(x => x.Realisasi4),
                    Target5 = listLain.Sum(x => x.Target5),
                    Realisasi5 = listLain.Sum(x => x.Realisasi5),
                };
                result.Add(totalLain);

                // 4. Penerimaan Pembiayaan
                var listPembiayaan = GetDataPenerimaanPembiayaanList();
                var totalPembiayaan = new RingkasanPendapatan
                {
                    Uraian = "Penerimaan Pembiayaan",
                    Target1 = listPembiayaan.Sum(x => x.Target1),
                    Realisasi1 = listPembiayaan.Sum(x => x.Realisasi1),
                    Target2 = listPembiayaan.Sum(x => x.Target2),
                    Realisasi2 = listPembiayaan.Sum(x => x.Realisasi2),
                    Target3 = listPembiayaan.Sum(x => x.Target3),
                    Realisasi3 = listPembiayaan.Sum(x => x.Realisasi3),
                    Target4 = listPembiayaan.Sum(x => x.Target4),
                    Realisasi4 = listPembiayaan.Sum(x => x.Realisasi4),
                    Target5 = listPembiayaan.Sum(x => x.Target5),
                    Realisasi5 = listPembiayaan.Sum(x => x.Realisasi5),
                };
                result.Add(totalPembiayaan);

                // 5. TOTAL
                var totalAll = new RingkasanPendapatan
                {
                    Uraian = "TOTAL",
                    Target1 = result.Sum(x => x.Target1),
                    Realisasi1 = result.Sum(x => x.Realisasi1),
                    Target2 = result.Sum(x => x.Target2),
                    Realisasi2 = result.Sum(x => x.Realisasi2),
                    Target3 = result.Sum(x => x.Target3),
                    Realisasi3 = result.Sum(x => x.Realisasi3),
                    Target4 = result.Sum(x => x.Target4),
                    Realisasi4 = result.Sum(x => x.Realisasi4),
                    Target5 = result.Sum(x => x.Target5),
                    Realisasi5 = result.Sum(x => x.Realisasi5),
                };
                result.Add(totalAll);

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
           /* public decimal Target6 { get; set; }
            public decimal Realisasi6 { get; set; }
            public decimal Persentase6 => Target6 > 0 ? (Realisasi6 / Target6) * 100 : 0;*/
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
        }
        public class RingkasanPendapatan
        {
            public string Uraian { get; set; } = "";

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
        }


    }
}