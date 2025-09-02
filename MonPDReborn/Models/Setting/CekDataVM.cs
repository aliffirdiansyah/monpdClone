using Microsoft.AspNetCore.Mvc;
using MonPDLib;
using MonPDLib.General;
using static MonPDReborn.Models.PengawasanReklame.ReklameLiarVM;

namespace MonPDReborn.Models.Setting
{
    public class CekDataVM
    {
        public class Index
        {
            public int SelectedTahun { get; set; }
        }

        // Untuk Partial View _Show.cshtml
        public class Show
        {
            public List<DataRealisasi> DataRealisasiList { get; set; } = new List<DataRealisasi>();
            public List<DataScontro> DataScontroList { get; set; } = new List<DataScontro>();
            public List<DataSelisih> DataSelisihList { get; set; } = new List<DataSelisih>();
            public Show(int tahun)
            {
                DataRealisasiList = Method.GetDataRealisasi(tahun);
                DataScontroList = Method.GetDataScontro(tahun);
                DataSelisihList = Method.GetDataSelisih(tahun);
            }
        }

        public class Method
        {
            public static List<DataRealisasi> GetDataRealisasi(int tahun)
            {
                var context = DBClass.GetContext();
                var year = tahun;
                var yearLast = year - 4;

                var dataRealisasiResto = context.DbMonRestos
                    .Where(x => x.TglBayarPokok.Value.Year == year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.MakananMinuman) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiListrik = context.DbMonPpjs
                    .Where(x => x.TglBayarPokok.Value.Year == year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.TenagaListrik) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiHotel = context.DbMonHotels
                    .Where(x => x.TglBayarPokok.Value.Year == year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaPerhotelan) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiParkir = context.DbMonParkirs
                    .Where(x => x.TglBayarPokok.Value.Year == year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaParkir) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiHiburan = context.DbMonHiburans
                    .Where(x => x.TglBayarPokok.Value.Year == year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.JasaKesenianHiburan) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiAbt = context.DbMonAbts
                    .Where(x => x.TglBayarPokok.Value.Year == year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.AirTanah) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiReklame = context.DbMonReklames
                    .Where(x => x.TglBayarPokok.Value.Year == year)
                    .GroupBy(x => new { x.TglBayarPokok.Value.Year, PajakId = (int)(EnumFactory.EPajak.Reklame) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.NominalPokokBayar ?? 0) })
                    .AsEnumerable();

                var dataRealisasiPbb = context.DbMonPbbs
                    .Where(x => x.TglBayar.Value.Year == year)
                    .GroupBy(x => new { x.TglBayar.Value.Year, PajakId = (int)(EnumFactory.EPajak.PBB) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.JumlahBayarPokok ?? 0) })
                    .AsEnumerable();

                var dataRealisasiBphtb = context.DbMonBphtbs
                    .Where(x => x.TglBayar.Value.Year == year)
                    .GroupBy(x => new { x.TglBayar.Value.Year, PajakId = (int)(EnumFactory.EPajak.BPHTB) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.Pokok ?? 0) })
                    .AsEnumerable();

                var dataRealisasiOpsenPkb = context.DbMonOpsenPkbs
                    .Where(x => x.TglSspd.Year == year)
                    .GroupBy(x => new { x.TglSspd.Year, PajakId = (int)(EnumFactory.EPajak.OpsenPkb) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.JmlPokok) })
                    .AsEnumerable();

                var dataRealisasiOpsenBbnkb = context.DbMonOpsenBbnkbs
                    .Where(x => x.TglSspd.Year == year)
                    .GroupBy(x => new { x.TglSspd.Year, PajakId = (int)(EnumFactory.EPajak.OpsenBbnkb) })
                    .Select(x => new { TahunBuku = x.Key.Year, x.Key.PajakId, Realisasi = x.Sum(q => q.JmlPokok) })
                    .AsEnumerable();

                // gabungkan semua data
                var dataGabungan = new List<(int TahunBuku, int PajakId, decimal Realisasi)>();
                dataGabungan.AddRange(dataRealisasiResto.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiListrik.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiHotel.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiParkir.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiHiburan.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiAbt.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiReklame.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiPbb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiBphtb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiOpsenPkb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));
                dataGabungan.AddRange(dataRealisasiOpsenBbnkb.Select(x => (x.TahunBuku, x.PajakId, x.Realisasi)));

                // Group lagi berdasarkan Tahun + PajakId, lalu sum Realisasi
                var ret = dataGabungan
                    .GroupBy(x => new { x.TahunBuku, x.PajakId })
                    .Select(g => new DataRealisasi
                    {
                        tahun = g.Key.TahunBuku,
                        PajakId = g.Key.PajakId,
                        realisasi = g.Sum(q => q.Realisasi)
                    })
                    .OrderBy(x => x.PajakId)
                    .ToList();

                return ret;
            }


            public static List<DataScontro> GetDataScontro(int tahun)
            {
                var ret = new List<DataScontro>();
                var context = DBClass.GetContext();
                var year = tahun;

                var dataPendapatan = (from p in context.DbPendapatanDaerahs
                                      join a in context.DbPajakMappings
                                          on new
                                          {
                                              p.TahunBuku,
                                              p.Akun,
                                              p.Kelompok,
                                              p.Jenis,
                                              p.Objek,
                                              p.Rincian,
                                              p.SubRincian
                                          }
                                          equals new
                                          {
                                              a.TahunBuku,
                                              a.Akun,
                                              a.Kelompok,
                                              a.Jenis,
                                              a.Objek,
                                              a.Rincian,
                                              a.SubRincian
                                          }
                                      where p.TahunBuku == tahun
                                            && p.Objek.StartsWith("4.1.01")
                                            && a.PajakId != null
                                      group p by new { p.TahunBuku, a.PajakId } into g
                                      select new DataScontro
                                      {
                                          tahun = (int)g.Key.TahunBuku,
                                          PajakId = (int)g.Key.PajakId,
                                          scontro = g.Sum(x => x.Realisasi)
                                      })
                                      .OrderBy(x => x.PajakId)
                                      .ToList();

                ret.AddRange(dataPendapatan);

                return ret;
            }

            public static List<DataSelisih> GetDataSelisih(int tahun)
            {
                // Ambil data realisasi dan scontro
                var dataRealisasi = GetDataRealisasi(tahun);
                var dataScontro = GetDataScontro(tahun);

                // Gabungkan keduanya berdasarkan Tahun + PajakId
                var dataGabungan = (from r in dataRealisasi
                                    join s in dataScontro
                                        on new { r.tahun, r.PajakId }
                                        equals new { s.tahun, s.PajakId }
                                        into gj
                                    from s in gj.DefaultIfEmpty() // left join, biar tidak hilang kalau data kosong
                                    select new DataSelisih
                                    {
                                        tahun = r.tahun,
                                        PajakId = r.PajakId,
                                        realisasi = r.realisasi,
                                        scontro = s?.scontro ?? 0
                                    })
                                    .Union( // Tambahkan yang hanya ada di scontro tapi tidak ada di realisasi
                                        from s in dataScontro
                                        where !dataRealisasi.Any(r => r.tahun == s.tahun && r.PajakId == s.PajakId)
                                        select new DataSelisih
                                        {
                                            tahun = s.tahun,
                                            PajakId = s.PajakId,
                                            realisasi = 0,
                                            scontro = s.scontro
                                        }
                                    )
                                    .OrderBy(x => x.PajakId)
                                    .ToList();

                return dataGabungan;
            }


        }

        public class DataRealisasi
        {
            public int tahun { get; set; }
            public int PajakId { get; set; }
            public string jenisPajak => ((EnumFactory.EPajak)PajakId).GetDescription().Replace("_", " ");
            public decimal realisasi { get; set; }
        }

        public class  DataScontro
        {
            public int tahun { get; set; }
            public int PajakId { get; set; }
            public string jenisPajak => ((EnumFactory.EPajak)PajakId).GetDescription().Replace("_", " ");
            public decimal scontro { get; set; }

        }

        public class DataSelisih
        {
            public int tahun { get; set; }
            public int PajakId { get; set; }
            public string jenisPajak => ((EnumFactory.EPajak)PajakId).GetDescription().Replace("_", " ");
            public decimal realisasi { get; set; }
            public decimal scontro { get; set; }
            public decimal selisih => realisasi - scontro;
        }
    }
    
}
