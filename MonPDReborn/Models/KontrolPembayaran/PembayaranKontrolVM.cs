using Microsoft.EntityFrameworkCore;
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
            public int RekeningBank { get; set; }
            public List<SelectListItem> RekeningBankList { get; set; } = new();

            public Index()
            {
                RekeningBankList = Enum.GetValues(typeof(EnumFactory.EBankRekening))
                    .Cast<EnumFactory.EBankRekening>()
                    .Select(x => new SelectListItem
                    {
                        Value = "00" + ((int)x).ToString(),
                        Text = x.GetDescription()
                    }).ToList();
            }
        }

        public class Show
        {
            public List<DataPembayaranOP> Data { get; set; } = new();

            public Show(EnumFactory.EBankRekening rekening, DateTime? tanggalAwal, DateTime? tanggalAkhir)
            {
                if (tanggalAwal == null)
                    throw new ArgumentException("Tanggal awal tidak boleh null.");
                if (tanggalAkhir == null)
                    throw new ArgumentException("Tanggal akhir tidak boleh null.");
                if (tanggalAkhir < tanggalAwal)
                    throw new ArgumentException("Tanggal akhir tidak boleh lebih kecil dari tanggal awal.");
                if (tanggalAwal > DateTime.Now)
                    throw new ArgumentException("Tanggal awal tidak boleh melebihi tanggal hari ini.");
                if (tanggalAwal.Value.Year != tanggalAkhir.Value.Year)
                    throw new ArgumentException("Tanggal awal dan akhir harus di tahun yang sama.");

                Data = Method.GetDataPembayaranOP(rekening, tanggalAwal.Value, tanggalAkhir.Value);
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
            public static List<DataPembayaranOP> GetDataPembayaranOP(EnumFactory.EBankRekening rekening, DateTime? tanggalAwal, DateTime? tanggalAkhir)
            {
                using var context = DBClass.GetContext();

                // 1️⃣ Fallback tanggal default
                var start = tanggalAwal ?? new DateTime(DateTime.Now.Year, 1, 1);
                var end = (tanggalAkhir ?? new DateTime(DateTime.Now.Year, 12, 31)).AddDays(1); // include hari terakhir

                var rekeningInt = (int)rekening;

                // 2️⃣ Ambil data dengan filter aman
                var ret = context.DbMutasiRekenings
                    .AsNoTracking()
                    .Where(x => !string.IsNullOrEmpty(x.Flag) && x.Flag.ToUpper() == "C"
                                && Convert.ToInt32(x.RekeningBank) == rekeningInt // aman terhadap "1", "01", "001"
                                && x.TanggalTransaksi >= start
                                && x.TanggalTransaksi < end)
                    .Select(x => new DataPembayaranOP
                    {
                        KodeTransaksi = x.TransactionCode,
                        Deskripsi = x.Description,
                        Amount = x.Amount ?? 0,
                        TanggalTransaksi = x.TanggalTransaksi,
                        Reff = x.Reffno,
                        RekeningBank = x.RekeningBank,
                        NamaRekening = x.RekeningBankNama
                    })
                    .ToList();

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
            public string KodeTransaksi { get; set; } = null!;
            public string? Deskripsi { get; set; } = null!;
            public decimal Amount { get; set; }
            public DateTime TanggalTransaksi { get; set; }
            public string? Reff { get; set; } = null!;
            public string? RekeningBank { get; set; } = null!;
            public string? NamaRekening { get; set; } = null!;

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
