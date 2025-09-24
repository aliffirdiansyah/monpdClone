using Microsoft.AspNetCore.Mvc;
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
    }
}
