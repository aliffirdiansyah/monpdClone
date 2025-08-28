using MonPDLib;
using MonPDLib.EF;
using MonPDLib.General;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static MonPDLib.General.EnumFactory;

namespace MonPDReborn.Models.KontrolPembayaran
{
    public class PembayaranKontrolVM
    {
        public class Index
        {
            public int SelectedPajak { get; set; }
            public List<SelectListItem> JenisPajakList { get; set; } = new();

            public Index()
            {
                JenisPajakList = Enum.GetValues(typeof(EnumFactory.EPajak))
                    .Cast<EnumFactory.EPajak>()
                    .Where(x => x != EnumFactory.EPajak.Reklame && x != EnumFactory.EPajak.BPHTB && x != EnumFactory.EPajak.OpsenPkb && x != EnumFactory.EPajak.OpsenBbnkb)
                    .Select(x => new SelectListItem
                    {
                        Value = ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
            }
        }

        public class Show
        {
            public List<DataPembayaranOP> Data { get; set; } = new();
            public Show(EnumFactory.EPajak jenisPajak, DateTime? tanggal)
            {
                Data = Method.GetDataPembayaranOP(jenisPajak, tanggal.Value);
            }
        }

        public class  Detail
        {
            public List<DetailPembayaranOP> Data { get; set; } = new();
            public Detail(EnumFactory.EPajak enumPajak)
            {
                Data = Method.GetDetailPembayaranOP(enumPajak);
            }
        }

        public class Method
        {
            public static List<DataPembayaranOP> GetDataPembayaranOP(EnumFactory.EPajak jenisPajak, DateTime Tanggal)
            {
                var ret = new List<DataPembayaranOP>();

                return ret;
            }

            public static List<DetailPembayaranOP> GetDetailPembayaranOP(EnumFactory.EPajak enumPajak)
            {
                var ret = new List<DetailPembayaranOP>();
                return ret;
            }
        }

        public class DataPembayaranOP
        {
            public string NOP { get; set; } = null!;
            public string Nama { get; set; } = null!;
            public string ObjekPajak => $"{NOP} - {Nama}";
            public int enumPajak { get; set; }
            public string JenisPajak => ((EnumFactory.EPajak)enumPajak).GetDescription().Replace("_", " ");
            public int MasaPajak { get; set; }
            public int TahunPajak { get; set; }
            public decimal Ketetapan { get; set; }
            public decimal Realisasi { get; set; }
            public decimal PembayaranBendahara { get; set; }
            public string? Reff { get; set; }
            public string Status { get; set; } = "Belum Valid";
            public string? NTPD { get; set; }

        }

        public class DetailPembayaranOP
        {
            public string NOP { get; set; } = null!;
            public string? Reff { get; set; }
            public int enumRekening { get;set; }
            public string Rekening => ((EnumFactory.EBankRekening)enumRekening).GetDescription().Replace("_", " ");
            public DateTime TanggalBayar { get; set; }
            public decimal Kredit { get; set; }
            public string Status { get; set; } = "Belum Valid";

        }   
    }
}
