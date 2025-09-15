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
            public IFormFile Lampiran { get; set; } = null!;
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
                {
                    throw new ArgumentException("Kode Petugas tidak boleh kosong.");
                }
                if (string.IsNullOrEmpty(NewRowUpaya.NoFormulir))
                {
                    throw new ArgumentException("No Formulir tidak boleh kosong.");
                }
                if (NewRowUpaya.IdUpaya == 0)
                {
                    throw new ArgumentException("Upaya tidak boleh kosong.");
                }
                if (NewRowUpaya.IdTindakan == 0)
                {
                    throw new ArgumentException("Keterangan tidak boleh kosong.");
                }
                if (NewRowUpaya.TglUpaya == null || NewRowUpaya.TglUpaya == DateTime.MinValue)
                {
                    throw new ArgumentException("Tanggal Upaya tidak boleh kosong.");
                }
                if (string.IsNullOrEmpty(NewRowUpaya.NamaPetugas))
                {
                    throw new ArgumentException("Nama Petugas tidak boleh kosong.");
                }
                if (NewRowUpaya.Lampiran == null || NewRowUpaya.Lampiran.Length <= 0)
                {
                    throw new ArgumentException("lampiran foto tidak boleh kosong.");
                }
                var tindakan = context.MTindakanReklames.Where(x => x.Id == NewRowUpaya.IdTindakan && x.IdUpaya == NewRowUpaya.IdUpaya).SingleOrDefault().Tindakan;
                var upaya = context.MUpayaReklames.Where(x => x.Id == NewRowUpaya.IdUpaya).SingleOrDefault().Upaya;

                var seq = context.DbMonReklameUpayas
                    .Where(x => x.NoFormulir == NewRowUpaya.NoFormulir)
                    .Select(x => x.Seq)
                    .Count() + 1;


                var newUpaya = new MonPDLib.EF.DbMonReklameUpaya();
                newUpaya.DbMonReklameUpayaDok = new DbMonReklameUpayaDok();

                newUpaya.NoFormulir = NewRowUpaya.NoFormulir;
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
            public string? Value { get; set; }
            public string Text { get; set; } = null!;
        }
        public class NoFormulirCbView
        {
            public string? Value { get; set; }
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
