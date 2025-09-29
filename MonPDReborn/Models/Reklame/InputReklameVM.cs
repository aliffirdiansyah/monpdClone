using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;

namespace MonPDReborn.Models.Reklame
{
    public class InputReklameVM
    {
        public class Index
        {
            public Index() 
            {
                
            }
        }

        public class Show
        {
            public DetailUpaya Data { get; set; } = new();
            public int SelectedUpaya { get; set; }
            public int SelectedTindakan { get; set; }
            public string SelectedKdAktifitas { get; set; } = null!;
            public string SelectedNoFormulir { get; set; } = null!;
            public string SelectedNOR { get; set; } = null!;
            public IFormFile Lampiran { get; set; } = null!;
            public Show() { }
            public Show(string noFormulir) 
            {
                Data.NoFormulir = noFormulir;
                Data.NewRowUpaya.NoFormulir = noFormulir;
                Data.NewRowUpaya.TglUpaya = DateTime.Now;
            }
        }

        public class RekapView
        {
            public List<Rekap> DataRekap { get; set; } = new();
            public RekapView()
            {
                DataRekap = new Method().GetDataRekap();
            }
        }

        public class  Detail
        {
            public List<RekapDet> Data { get; set; } = new();
            public Detail(string petugas, int noKegiatan)
            {
                Data = new Method().GetRekapDet(petugas, noKegiatan);
            }
        }

        public class Method
        {
            public static void SimpanUpaya(DetailUpaya.NewRow NewRowUpaya)
            {
                var context = DBClass.GetContext();

                if (string.IsNullOrEmpty(NewRowUpaya.KdKatifitas))
                    throw new ArgumentException("Kode Petugas tidak boleh kosong.");

                if (string.IsNullOrEmpty(NewRowUpaya.NoFormulir) && string.IsNullOrEmpty(NewRowUpaya.NOR))
                    throw new ArgumentException("Silakan isi salah satu: No Formulir atau NOR.");

                if (NewRowUpaya.IdUpaya == 0)
                    throw new ArgumentException("Upaya tidak boleh kosong.");

                if (NewRowUpaya.IdTindakan == 0)
                    throw new ArgumentException("Keterangan tidak boleh kosong.");

                if (NewRowUpaya.TglUpaya == null || NewRowUpaya.TglUpaya == DateTime.MinValue)
                    throw new ArgumentException("Tanggal Upaya tidak boleh kosong.");

                if (string.IsNullOrEmpty(NewRowUpaya.NamaPetugas))
                    throw new ArgumentException("Nama Petugas tidak boleh kosong.");

                if (string.IsNullOrEmpty(NewRowUpaya.NIKPetugas))
                    throw new ArgumentException("NIK Petugas tidak boleh kosong.");

                if (NewRowUpaya.Lampiran == null || NewRowUpaya.Lampiran.Length <= 0)
                    throw new ArgumentException("Lampiran foto tidak boleh kosong.");

                // Ambil nama tindakan dan upaya
                var tindakan = context.MTindakanReklames
                    .Where(x => x.Id == NewRowUpaya.IdTindakan && x.IdUpaya == NewRowUpaya.IdUpaya)
                    .Select(x => x.Tindakan)
                    .SingleOrDefault();

                var upaya = context.MUpayaReklames
                    .Where(x => x.Id == NewRowUpaya.IdUpaya)
                    .Select(x => x.Upaya)
                    .SingleOrDefault();

                var seq = 1;
                if (!string.IsNullOrEmpty(NewRowUpaya.NoFormulir))
                {
                    seq = context.DbMonReklameUpayas
                        .Where(x => x.NoFormulir == NewRowUpaya.NoFormulir)
                        .Select(x => x.Seq)
                        .Count() + 1;
                }
                else if (!string.IsNullOrEmpty(NewRowUpaya.NOR))
                {
                    seq = context.DbMonReklameUpayas
                        .Where(x => x.Nor == NewRowUpaya.NOR) 
                        .Select(x => x.Seq)
                        .Count() + 1;
                }

                // Simpan data
                var newUpaya = new MonPDLib.EF.DbMonReklameUpaya();
                newUpaya.DbMonReklameUpayaDok = new DbMonReklameUpayaDok();

                newUpaya.NoFormulir = NewRowUpaya.NoFormulir.Trim().ToUpper();
                newUpaya.Nor = NewRowUpaya.NOR.Trim().ToUpper();
                newUpaya.Seq = seq;
                newUpaya.TglUpaya = NewRowUpaya.TglUpaya;
                newUpaya.Upaya = upaya ?? "-";
                newUpaya.Keterangan = tindakan ?? "-";
                newUpaya.Petugas = NewRowUpaya.NamaPetugas;
                newUpaya.NikPetugas = NewRowUpaya.NIKPetugas;
                newUpaya.KdAktifitas = NewRowUpaya.KdKatifitas;
                newUpaya.DbMonReklameUpayaDok.Gambar = NewRowUpaya.Lampiran;

                context.DbMonReklameUpayas.Add(newUpaya);
                context.SaveChanges();
            }

            public List<Rekap> GetDataRekap()
            {
                var context = DBClass.GetContext();
                var result = context.DbMonReklameUpayas
                    .Where(x => x.KdAktifitas != null && x.KdAktifitas != "-" && x.NikPetugas != null && x.NikPetugas != "-")
                    .GroupBy(x => x.Petugas)
                    .Select(g => new Rekap
                    {
                        namaPetugas = g.Key!,
                        JmlBantib = g.Count(x => x.Upaya == "BANTIP REKLAME"), 
                        JmlSilang = g.Count(x => x.Upaya == "PENYILANGAN"),    
                        JmlSurvey = g.Count(x => x.Upaya == "PENDATAAN SURVEY"),
                        JmlBongkar = g.Count(x => x.Upaya == "PEMBONGKARAN"),  
                        JmlLain = g.Count(x => x.Upaya == "LAINNYA"),
                        JmlBatal = g.Count(x => x.Upaya == "PEMBATALAN KETETAPAN"),
                    })
                    .OrderBy(x => x.namaPetugas)
                    .ToList();
                return result;
            }

            public List<RekapDet> GetRekapDet(string petugas, int noKegiatan)
            {
                var ret = new List<RekapDet>();
                var context = DBClass.GetContext();
                
                if (noKegiatan == 1)
                {
                    ret = context.DbMonReklameUpayas
                        .Where(x => x.Petugas == petugas
                                    && x.KdAktifitas != null && x.KdAktifitas != "-"
                                    && x.NikPetugas != null && x.NikPetugas != "-"
                                    && x.Upaya == "PENDATAAN SURVEY")
                        .OrderByDescending(x => x.TglUpaya)
                        .Select(x => new RekapDet
                        {
                            NoFormulir = x.NoFormulir ?? "-",
                            NOR = x.Nor ?? "-",
                            TglUpaya = x.TglUpaya.ToString("dd-MM-yyyy"),
                            NamaUpaya = x.Upaya ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            Petugas = x.Petugas ?? "-",
                        })
                        .ToList();
                }
                else if (noKegiatan == 2)
                {
                    ret = context.DbMonReklameUpayas
                        .Where(x => x.Petugas == petugas
                                    && x.KdAktifitas != null && x.KdAktifitas != "-"
                                    && x.NikPetugas != null && x.NikPetugas != "-"
                                    && x.Upaya == "PENYILANGAN")
                        .OrderByDescending(x => x.TglUpaya)
                        .Select(x => new RekapDet
                        {
                            NoFormulir = x.NoFormulir ?? "-",
                            NOR = x.Nor ?? "-",
                            TglUpaya = x.TglUpaya.ToString("dd-MM-yyyy"),
                            NamaUpaya = x.Upaya ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            Petugas = x.Petugas ?? "-",
                            Seq = x.Seq,
                        })
                        .ToList();
                }
                else if (noKegiatan == 3)
                {
                    ret = context.DbMonReklameUpayas
                        .Where(x => x.Petugas == petugas
                                    && x.KdAktifitas != null && x.KdAktifitas != "-"
                                    && x.NikPetugas != null && x.NikPetugas != "-"
                                    && x.Upaya == "PEMBONGKARAN")
                        .OrderByDescending(x => x.TglUpaya)
                        .Select(x => new RekapDet
                        {
                            NoFormulir = x.NoFormulir ?? "-",
                            NOR = x.Nor ?? "-",
                            TglUpaya = x.TglUpaya.ToString("dd-MM-yyyy"),
                            NamaUpaya = x.Upaya ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            Petugas = x.Petugas ?? "-",
                        })
                        .ToList();
                }
                else if (noKegiatan == 4)
                {
                    ret = context.DbMonReklameUpayas
                        .Where(x => x.Petugas == petugas
                                    && x.KdAktifitas != null && x.KdAktifitas != "-"
                                    && x.NikPetugas != null && x.NikPetugas != "-"
                                    && x.Upaya == "BANTIP REKLAME")
                        .OrderByDescending(x => x.TglUpaya)
                        .Select(x => new RekapDet
                        {
                            NoFormulir = x.NoFormulir ?? "-",
                            NOR = x.Nor ?? "-",
                            TglUpaya = x.TglUpaya.ToString("dd-MM-yyyy"),
                            NamaUpaya = x.Upaya ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            Petugas = x.Petugas ?? "-",
                        })
                        .ToList();
                }
                else if (noKegiatan == 5)
                {
                    ret = context.DbMonReklameUpayas
                        .Where(x => x.Petugas == petugas
                                    && x.KdAktifitas != null && x.KdAktifitas != "-"
                                    && x.NikPetugas != null && x.NikPetugas != "-"
                                    && x.Upaya == "LAINNYA")
                        .OrderByDescending(x => x.TglUpaya)
                        .Select(x => new RekapDet
                        {
                            NoFormulir = x.NoFormulir ?? "-",
                            NOR = x.Nor ?? "-",
                            TglUpaya = x.TglUpaya.ToString("dd-MM-yyyy"),
                            NamaUpaya = x.Upaya ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            Petugas = x.Petugas ?? "-",
                        })
                        .ToList();
                }
                else if (noKegiatan == 6)
                {
                    ret = context.DbMonReklameUpayas
                        .Where(x => x.Petugas == petugas
                                    && x.KdAktifitas != null && x.KdAktifitas != "-"
                                    && x.NikPetugas != null && x.NikPetugas != "-"
                                    && x.Upaya == "PEMBATALAN KETETAPAN")
                        .OrderByDescending(x => x.TglUpaya)
                        .Select(x => new RekapDet
                        {
                            NoFormulir = x.NoFormulir ?? "-",
                            NOR = x.Nor ?? "-",
                            TglUpaya = x.TglUpaya.ToString("dd-MM-yyyy"),
                            NamaUpaya = x.Upaya ?? "-",
                            Keterangan = x.Keterangan ?? "-",
                            Petugas = x.Petugas ?? "-",
                        })
                        .ToList();

                }
                return ret;
            }
        }
        public class UpayaCbView
        {
            public int Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class TindakanCbView
        {
            public int Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class KdAktifitasCbView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class NoFormulirCbView
        {
            public string Value { get; set; }
            public string Text { get; set; } = null!;
        }

        public class DetailUpaya
        {
            public int Lokasi { get; set; }
            public int Bulan { get; set; }
            public int Tahun { get; set; }
            public string NoFormulir { get; set; } = null!;
            public NewRow NewRowUpaya { get; set; } = new NewRow();
            public class NewRow
            {
                public string NoFormulir { get; set; } = null!;
                public string NOR { get; set; } = null!;
                public int IdUpaya { get; set; }
                public int IdTindakan { get; set; }
                public string NamaPetugas { get; set; } = null!;
                public string KdKatifitas { get; set; } = null!;
                public string NIKPetugas { get; set; } = null!;
                public DateTime TglUpaya { get; set; }
                public byte[] Lampiran { get; set; } = null!;
            }
        }
        public class Rekap
        {
            public string namaPetugas { get; set; } = null!;
            public int JmlSurvey { get; set; }
            public int JmlSilang { get; set; }
            public int JmlBantib { get; set; }
            public int JmlBongkar { get; set; }
            public int JmlLain { get; set; }
            public int JmlBatal { get; set; }
        }

        public class RekapDet
        {
            public string NoFormulir { get; set; } = null!;
            public string NOR { get; set; } = null!;
            public string TglUpaya { get; set; } = null!;
            public string NamaUpaya { get; set; } = null!;
            public string Keterangan { get; set; } = null!;
            public string Petugas { get; set; } = null!;
            public string Lampiran { get; set; }
            public decimal UpayaId { get; set; }
            public decimal Seq { get; set; }
        }
    }
}
