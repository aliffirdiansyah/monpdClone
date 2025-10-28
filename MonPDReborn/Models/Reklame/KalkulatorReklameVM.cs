using MonPDLib;
using MonPDLib.General;
using MonPDLib.Lib;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Globalization;
using System.Web.Mvc;

namespace MonPDReborn.Models.Reklame
{
    public class KalkulatorReklameVM
    {

        public class Index
        {
            public KalkulatorReklamePermanen.ReklameInput Inputan { get; set; } = new();
            public KalkulatorReklameInsidentil.ReklameInput InputanIns { get; set; } = new();
            public List<SelectListItem> JenisReklameList { get; set; } = new();
            public List<dynamic> JenisReklameInsList { get; set; } = new();
            public List<SelectListItem> LetakReklameList { get; set; } = new();
            public List<SelectListItem> ProdukList { get; set; } = new();
            public List<SelectListItem> JalanList { get; set; } = new();
            public Index()
            {
                var context = DBClass.GetReklameContext();
                JenisReklameList = context.MJenisReklames
                    .Where(x => x.Kategori == (int)EnumFactory.JenisReklame.Permanen)
                    .Select(j => new SelectListItem
                    {
                        Value = j.IdJenisReklame.ToString(),
                        Text = j.NamaJenis
                    })
                    .OrderBy(x => x.Text)
                    .ToList();
                JenisReklameInsList = context.MJenisReklames
                    .Where(x => x.Kategori == (int)EnumFactory.JenisReklame.Insidentil)
                    .Select(j => new
                    {
                        Value = j.IdJenisReklame.ToString(),
                        Text = j.NamaJenis,
                        ModeUkur = j.ModeUkur
                    })
                    .OrderBy(x => x.Text)
                    .ToList<dynamic>();
                LetakReklameList = Enum.GetValues(typeof(EnumFactory.LetakReklame))
                    .Cast<EnumFactory.LetakReklame>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                ProdukList = Enum.GetValues(typeof(EnumFactory.ProdukReklame))
                    .Cast<EnumFactory.ProdukReklame>()
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
                JalanList = context.MJalans
                    .Select(q => new SelectListItem
                    {
                        Value = q.IdJalan.ToString(),
                        Text = q.NamaJalan
                    })
                    .ToList();
            }
        }

        public class Show
        {
            public KalkulatorReklamePermanen.ReklameInput Output { get; set; } = new();
            public KalkulatorReklamePermanen KalkullatorReklameRow { get; set; } = new();
            public Show(KalkulatorReklamePermanen.ReklameInput input)
            {
                Output = input;
                KalkullatorReklameRow = KalkulatorReklamePermanen.HitungNilaiSewaReklame(input);
            }
        }

        public class ShowIns
        {
            public KalkulatorReklameInsidentil.ReklameInput Output { get; set; } = new();
            public KalkulatorReklameInsidentil KalkullatorReklameRow { get; set; } = new();
            public Reports.KalkulatorInsPublic KalkulatorInsPublic { get; set; } = new();
            public ShowIns(KalkulatorReklameInsidentil.ReklameInput input)
            {
                Output = input;
                KalkullatorReklameRow = KalkulatorReklameInsidentil.HitungNilaiSewaReklame(input);

                //Binding Reports
                KalkulatorInsPublic.jenis.Text = Output.JenisReklame.GetDescription();
                KalkulatorInsPublic.produk.Text = Output.JenisProduk.GetDescription();
                KalkulatorInsPublic.lamaHari.Text = Output.LamaPenyelenggaraan.ToString() + " Hari";
                KalkulatorInsPublic.jalan.Text = KalkullatorReklameRow.NamaJalan.ToString();
                KalkulatorInsPublic.kelas.Text = KalkullatorReklameRow.KelasJalan.ToString();
                KalkulatorInsPublic.jumlahSatuan.Text = Output.JumlahSatuan.ToString("N0");
                KalkulatorInsPublic.tglBerlaku.Text = $"{Output.TglMulaiBerlaku.ToString("dd MMM yyyy", new CultureInfo("id-ID"))} s/d {Output.TglSelesaiBerlaku.ToString("dd MMM yyyy", new CultureInfo("id-ID"))}";
                KalkulatorInsPublic.letak.Text = Output.LetakReklame.GetDescription();
                KalkulatorInsPublic.totalNSR.Text = $"Rp. {KalkullatorReklameRow.TotalNilaiSewa.ToString("N0")}";
                KalkulatorInsPublic.totalJambong.Text = $"Rp. {KalkullatorReklameRow.JambongNilai.ToString("N0")}";

                if (KalkullatorReklameRow.ModeUkur == EnumFactory.EModeUkur.Luas)
                {
                    KalkulatorInsPublic.jumlahSatuan.Text = $"{Output.JumlahSatuan} Unit";
                    if (Output.JenisReklame == EnumFactory.KategoriReklame.StikerMelekat)
                    {
                        KalkulatorInsPublic.itungan1.Text = $"Panjang";
                        KalkulatorInsPublic.hasilItung1.Text = $"{Output.Panjang.ToString("N0")} cm";
                        KalkulatorInsPublic.itungan2.Text = $"Lebar";
                        KalkulatorInsPublic.hasilItung2.Text = $"{Output.Lebar.ToString("N0")} cm";
                        KalkulatorInsPublic.itungan3.Text = $"Luas";
                        KalkulatorInsPublic.hasilItung3.Text = $"{KalkullatorReklameRow.Luas.ToString("N0")} cm²";
                    }
                    else
                    {
                        KalkulatorInsPublic.itungan1.Text = $"Panjang";
                        KalkulatorInsPublic.hasilItung1.Text = $"{Output.Panjang.ToString("N0")} m";
                        KalkulatorInsPublic.itungan2.Text = $"Lebar";
                        KalkulatorInsPublic.hasilItung2.Text = $"{Output.Lebar.ToString("N0")} m";
                        KalkulatorInsPublic.itungan3.Text = $"Luas";
                        KalkulatorInsPublic.hasilItung3.Text = $"{KalkullatorReklameRow.Luas.ToString("N0")} m²";
                    }
                    KalkulatorInsPublic.itungan4.Visible = false;
                    KalkulatorInsPublic.hasilItung4.Visible = false;
                }
                else if (KalkullatorReklameRow.ModeUkur == EnumFactory.EModeUkur.Satuan)
                {
                    KalkulatorInsPublic.jumlahSatuan.Text = $"{Output.JumlahSatuan} Unit";
                    KalkulatorInsPublic.itungan1.Visible = false;
                    KalkulatorInsPublic.hasilItung1.Visible = false;
                    KalkulatorInsPublic.itungan2.Visible = false;
                    KalkulatorInsPublic.hasilItung2.Visible = false;
                    KalkulatorInsPublic.itungan3.Visible = false;
                    KalkulatorInsPublic.hasilItung3.Visible = false;
                    KalkulatorInsPublic.itungan4.Visible = false;
                    KalkulatorInsPublic.hasilItung4.Visible = false;
                }
                else if (KalkullatorReklameRow.ModeUkur == EnumFactory.EModeUkur.Perulangan)
                {
                    if (Output.JenisReklame == EnumFactory.KategoriReklame.Suara)
                    {
                        KalkulatorInsPublic.jumlahSatuan.Text = $"{Output.JumlahSatuan} Menit";
                    }
                    if (Output.JenisReklame == EnumFactory.KategoriReklame.FilmSlide)
                    {
                        KalkulatorInsPublic.jumlahSatuan.Text = $"{Output.JumlahSatuan} Detik";
                    }
                    KalkulatorInsPublic.itungan1.Text = $"Jumlah Layar";
                    KalkulatorInsPublic.hasilItung1.Text = $"{Output.JumlahLayar} Layar";
                    KalkulatorInsPublic.itungan2.Text = $"Jumlah Show";
                    KalkulatorInsPublic.hasilItung2.Text = $"{Output.JumlahPerulangan} Show";
                    KalkulatorInsPublic.itungan3.Visible = false;
                    KalkulatorInsPublic.hasilItung3.Visible = false;
                    KalkulatorInsPublic.itungan4.Visible = false;
                    KalkulatorInsPublic.hasilItung4.Visible = false;
                }
                KalkulatorInsPublic.ExportOptions.PrintPreview.DefaultFileName = "Est NSR " + Output.JenisReklame.GetDescription();
                KalkulatorInsPublic.CreateDocument();
            }
        }

        public class ShowKontrak
        {
            public decimal nilaiKontrak { get; set; } = new();
            public KalkulatorReklamePermanen HitungKontrak { get; set; } = new();
            public ShowKontrak(decimal NilaiKontrak)
            {
                nilaiKontrak = NilaiKontrak;
                HitungKontrak = KalkulatorReklamePermanen.HitungNilaiSewaReklame(NilaiKontrak);
            }
        }
    }
}
