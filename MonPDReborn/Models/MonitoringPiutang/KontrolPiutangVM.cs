using MonPDLib;
using MonPDLib.General;

namespace MonPDReborn.Models.MonitoringPiutang 
{
    public class KontrolPiutangVM
    {
        public class Index
        {
            public List<ViewModel.DataPiutang> DataPiutang { get; set; } = new List<ViewModel.DataPiutang>();
            public List<ViewModel.DataMutasi> DataMutasi { get; set; } = new List<ViewModel.DataMutasi>();
            public Index()
            {
                DataPiutang = Method.GetDataPiutangData();
                DataMutasi = Method.GetDataMutasiData();
            }
        }
        public class DataPiutang
        {
            public List<ViewModel.DataPiutang> Data { get; set; } = new List<ViewModel.DataPiutang>();
            public DataPiutang()
            {
                Data = Method.GetDataPiutangData();
            }
        }
        public class DetailPiutang
        {
            public List<ViewModel.DetailPiutang> Data { get; set; } = new List<ViewModel.DetailPiutang>();
            public DetailPiutang(EnumFactory.EPajak jenisPajak, int tahun)
            {
                Data = Method.GetDetailPiutangData(jenisPajak, tahun);
            }
        }
        public class DataMutasi
        {
            public List<ViewModel.DataMutasi> Data { get; set; } = new List<ViewModel.DataMutasi>();
            public DataMutasi()
            {
                Data = Method.GetDataMutasiData();
            }
        }

        public class Method
        {
            public static List<ViewModel.DataPiutang> GetDataPiutangData()
            {
                var ret = new List<ViewModel.DataPiutang>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;


                var dataPiutangResto = context.TPiutangRestos
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.MakananMinuman.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.MakananMinuman,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangResto);

                var dataPiutangListrik = context.TPiutangListriks
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.TenagaListrik.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.TenagaListrik,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangListrik);

                var dataPiutangHotel = context.TPiutangHotels
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.JasaPerhotelan.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaPerhotelan,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangHotel);

                var dataPiutangParkir = context.TPiutangParkirs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.JasaParkir.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaParkir,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangParkir);

                var dataPiutangHiburan = context.TPiutangHiburans
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.JasaKesenianHiburan.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.JasaKesenianHiburan,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangHiburan);

                var dataPiutangAbt = context.TPiutangAbts
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.AirTanah.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.AirTanah,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangAbt);

                var dataPiutangReklame = context.TPiutangReklames
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.Reklame.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.Reklame,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangReklame);

                var dataPiutangPbb = context.TPiutangPbbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.PBB.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.PBB,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangPbb);

                var dataPiutangBphtb = context.TPiutangBphtbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.BPHTB.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.BPHTB,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangBphtb);

                var dataPiutangOpsenPkb = context.TPiutangOpsenPkbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenPkb.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.OpsenPkb,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangOpsenPkb);

                var dataPiutangOpsenBbnkb = context.TPiutangOpsenBbnkbs
                    .GroupBy(x => 1)
                    .Select(g => new ViewModel.DataPiutang
                    {
                        JenisPajak = EnumFactory.EPajak.OpsenBbnkb.GetDescription(),
                        EnumPajak = (int)EnumFactory.EPajak.OpsenBbnkb,
                        NominalPiutang1 = g.Where(x => x.TahunBuku == currentYear - 3).Sum(x => x.Piutang),
                        NominalPiutang2 = g.Where(x => x.TahunBuku == currentYear - 2).Sum(x => x.Piutang),
                        NominalPiutang3 = g.Where(x => x.TahunBuku == currentYear - 1).Sum(x => x.Piutang),
                        NominalPiutang4 = g.Where(x => x.TahunBuku == currentYear).Sum(x => x.Piutang)
                    })
                    .ToList();
                ret.AddRange(dataPiutangOpsenBbnkb);

                return ret;

            }

            public static List<ViewModel.DetailPiutang> GetDetailPiutangData(EnumFactory.EPajak jenisPajak, int tahun)
            {
                var ret = new List<ViewModel.DetailPiutang>();
                var context = DBClass.GetContext();

                switch (jenisPajak)
                {
                    case EnumFactory.EPajak.MakananMinuman:
                        var dataPiutangResto = context.TPiutangRestos
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangResto);
                        break;
                    case EnumFactory.EPajak.TenagaListrik:
                        var dataPiutangListrik = context.TPiutangListriks
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangListrik);
                        break;
                    case EnumFactory.EPajak.JasaPerhotelan:
                        var dataPiutangHotel = context.TPiutangHotels
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangHotel);
                        break;
                    case EnumFactory.EPajak.JasaParkir:
                        var dataPiutangParkir = context.TPiutangParkirs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangParkir);
                        break;
                    case EnumFactory.EPajak.JasaKesenianHiburan:
                        var dataPiutangHiburan = context.TPiutangHiburans
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangHiburan);
                        break;
                    case EnumFactory.EPajak.AirTanah:
                        var dataPiutangAbt = context.TPiutangAbts
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangAbt);
                        break;
                    case EnumFactory.EPajak.Reklame:
                        var dataPiutangReklame = context.TPiutangReklames
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangReklame);
                        break;
                    case EnumFactory.EPajak.PBB:
                        var dataPiutangPbb = context.TPiutangPbbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangPbb);
                        break;
                    case EnumFactory.EPajak.BPHTB:
                        var dataPiutangBphtb = context.TPiutangBphtbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangBphtb);
                        break;
                    case EnumFactory.EPajak.OpsenPkb:
                        var dataPiutangOpsenPkb = context.TPiutangOpsenPkbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangOpsenPkb);
                        break;
                    case EnumFactory.EPajak.OpsenBbnkb:
                        var dataPiutangOpsenBbnkb = context.TPiutangOpsenBbnkbs
                            .Where(x => x.TahunBuku == tahun)
                            .Select(x => new ViewModel.DetailPiutang
                            {
                                Nop = x.Nop,
                                NamaOp = x.NamaOp,
                                AlamatOp = x.AlamatOp,
                                WilayahPajak = x.WilayahPajak,
                                JenisPajak = jenisPajak.GetDescription(),
                                EnumPajak = (int)jenisPajak,
                                MasaPajak = x.MasaPajak,
                                TahunPajak = x.TahunPajak,
                                NominalPiutang = x.Piutang,
                            })
                            .ToList();
                        ret.AddRange(dataPiutangOpsenBbnkb);
                        break;
                    default:
                        break;
                }

                return ret;
            }
            public static List<ViewModel.DataMutasi> GetDataMutasiData()
            {
                var ret = new List<ViewModel.DataMutasi>();
                var context = DBClass.GetContext();
                var currentYear = DateTime.Now.Year;

                var MutasiData = context.TMutasiPiutangs
                    .AsEnumerable()
                    .GroupBy(x => new { x.Mutasi, x.Urutan })
                    .OrderBy(g => g.Key.Urutan)
                    .ToList();

                ret = MutasiData.Select(g => new ViewModel.DataMutasi
                {
                    Keterangan = g.Key.Mutasi,
                    NominalMutasi1 = g.Where(x => int.Parse(x.TahunBuku) <= currentYear - 4).Sum(x => x.Nilai),
                    NominalMutasi2 = g.Where(x => int.Parse(x.TahunBuku) == currentYear - 3).Sum(x => x.Nilai),
                    NominalMutasi3 = g.Where(x => int.Parse(x.TahunBuku) == currentYear - 2).Sum(x => x.Nilai),
                    NominalMutasi4 = g.Where(x => int.Parse(x.TahunBuku) == currentYear - 1).Sum(x => x.Nilai),
                    NominalMutasi5 = g.Where(x => int.Parse(x.TahunBuku) == currentYear).Sum(x => x.Nilai),
                }).ToList();

                return ret;

            }

        }

        public class ViewModel
        {
            public class DataPiutang
            {
                public string JenisPajak { get; set; } = "";
                public int EnumPajak { get; set; }
                public decimal NominalPiutang1 { get; set; }
                public decimal NominalPiutang2 { get; set; }
                public decimal NominalPiutang3 { get; set; }
                public decimal NominalPiutang4 { get; set; }
            }
            public class DetailPiutang
            {
                public string Nop { get; set; } = "";
                public string NamaOp { get; set; } = "";
                public string AlamatOp { get; set; } = "";
                public string WilayahPajak { get; set; } = "";
                public string JenisPajak { get; set; }
                public int EnumPajak { get; set; }
                public int MasaPajak { get; set; }
                public int TahunPajak { get; set; }
                public decimal NominalPiutang { get; set; }
            }
            public class DataMutasi
            {
                public string Keterangan { get; set; } = "";
                public decimal NominalMutasi1 { get; set; }
                public decimal NominalMutasi2 { get; set; }
                public decimal NominalMutasi3 { get; set; }
                public decimal NominalMutasi4 { get; set; }
                public decimal NominalMutasi5 { get; set; }
            }
        }
    }
}
