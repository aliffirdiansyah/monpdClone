using MonPDLib;
using MonPDLib.General;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Text.Json.Serialization;
using System.Web.Mvc;

namespace MonPDReborn.Models.DataOP
{
    public class KartuDataVM
    {
        public class Index
        {
            public string SelectedNOP { get; set; } = string.Empty;
            public Index()
            {

            }

        }
        public class GetData
        {
            public ResponseKartuData KartuData { get; private set; }

            public GetData(string nop)
            {
                Load(nop).GetAwaiter().GetResult();
            }

            private async Task Load(string nop)
            {
                string username = "SBYTAX!API22024";
                string password = "!SBYTAX2024!";

                // Handler yang bypass SSL validation (DEV ONLY)
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
                };

                using (var client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // Build URL dengan credentials di query string (escape semua komponen)
                    var baseUrl = "https://10.21.31.180:7079/KARTUDATA/GetApi";
                    var q = $"nop={Uri.EscapeDataString(nop)}&username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";
                    var url = $"{baseUrl}?{q}";

                    HttpResponseMessage response;
                    try
                    {
                        response = await client.GetAsync(url);
                    }
                    catch (HttpRequestException httpEx)
                    {
                        throw new Exception("Gagal melakukan request ke API: " + httpEx.Message, httpEx);
                    }
                    catch (TaskCanceledException tcEx) when (!tcEx.CancellationToken.IsCancellationRequested)
                    {
                        throw new Exception("Request ke API timeout.", tcEx);
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        throw new Exception($"API Error: {(int)response.StatusCode} {response.ReasonPhrase}. Response: {content}");
                    }

                    var json = await response.Content.ReadAsStringAsync();

                    try
                    {
                        KartuData = System.Text.Json.JsonSerializer.Deserialize<ResponseKartuData>(json,
                            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch (System.Text.Json.JsonException jex)
                    {
                        throw new Exception("Gagal parsing JSON dari API: " + jex.Message + ". JSON: " + json, jex);
                    }
                }
            }
        }
        public class Method
        {
            public static List<SelectListItem> GetOpList(string nop)
            {
                var context = DBClass.GetContext();
                var ret = new List<SelectListItem>();
                nop = nop.Replace(".", "").Trim();
                var currentYear = DateTime.Now.Year;

                var opResto = context.DbOpRestos
                    .Where(x => x.TahunBuku == currentYear && x.PajakNama != "MAMIN" && (x.Nop == nop || nop.ToUpper().Trim().Contains(x.NamaOp.ToUpper().Trim() ) || nop.ToUpper().Trim().Contains(x.AlamatOp.ToUpper().Trim())))
                    .Select(x => new SelectListItem() { Value = x.Nop, Text = $"[{Utility.GetFormattedNOP(x.Nop)}] {x.NamaOp} [{(EnumFactory.EPajak.MakananMinuman).GetDescription()}] - [{x.AlamatOp}]" })
                    .ToList();
                var OpHotel = context.DbOpHotels
                    .Where(x => x.TahunBuku == currentYear && (x.Nop == nop || nop.ToUpper().Contains(x.NamaOp.ToUpper()) || nop.ToUpper().Trim().Contains(x.AlamatOp.ToUpper().Trim())))
                    .Select(x => new SelectListItem() { Value = x.Nop, Text = $"[{Utility.GetFormattedNOP(x.Nop)}] {x.NamaOp} [{(EnumFactory.EPajak.JasaPerhotelan).GetDescription()}] - [{x.AlamatOp}]" })
                    .ToList();
                var opParkir = context.DbOpParkirs
                    .Where(x => x.TahunBuku == currentYear && (x.Nop == nop || nop.ToUpper().Contains(x.NamaOp.ToUpper()) || nop.ToUpper().Trim().Contains(x.AlamatOp.ToUpper().Trim())))
                    .Select(x => new SelectListItem() { Value = x.Nop, Text = $"[{Utility.GetFormattedNOP(x.Nop)}] {x.NamaOp} [{(EnumFactory.EPajak.JasaParkir).GetDescription()}] - [{x.AlamatOp}]" })
                    .ToList();
                var opListrik = context.DbOpListriks
                    .Where(x => x.TahunBuku == currentYear && (x.Nop == nop || nop.ToUpper().Contains(x.NamaOp.ToUpper()) || nop.ToUpper().Trim().Contains(x.AlamatOp.ToUpper().Trim())))
                    .Select(x => new SelectListItem() { Value = x.Nop, Text = $"[{Utility.GetFormattedNOP(x.Nop)}] {x.NamaOp} [{(EnumFactory.EPajak.TenagaListrik).GetDescription()}] - [{x.AlamatOp}]" })
                    .ToList();
                var opHiburan = context.DbOpHiburans
                    .Where(x => x.TahunBuku == currentYear && (x.Nop == nop || nop.ToUpper().Contains(x.NamaOp.ToUpper()) || nop.ToUpper().Trim().Contains(x.AlamatOp.ToUpper().Trim())))
                    .Select(x => new SelectListItem() { Value = x.Nop, Text = $"[{Utility.GetFormattedNOP(x.Nop)}] {x.NamaOp} [{(EnumFactory.EPajak.JasaKesenianHiburan).GetDescription()}] - [{x.AlamatOp}]" })
                    .ToList();
                var opAirTanah = context.DbOpAbts
                    .Where(x => x.TahunBuku == currentYear && (x.Nop == nop || nop.ToUpper().Contains(x.NamaOp.ToUpper()) || nop.ToUpper().Trim().Contains(x.AlamatOp.ToUpper().Trim())))
                    .Select(x => new SelectListItem() { Value = x.Nop, Text = $"[{Utility.GetFormattedNOP(x.Nop)}] {x.NamaOp} [{(EnumFactory.EPajak.AirTanah).GetDescription()}] - [{x.AlamatOp}]" })
                    .ToList();

                ret.AddRange(opResto);
                ret.AddRange(OpHotel);
                ret.AddRange(opParkir);
                ret.AddRange(opListrik);
                ret.AddRange(opHiburan);
                ret.AddRange(opAirTanah);

                return ret;

            }
        }
        public class KartuDataList
        {
            [JsonPropertyName("Tahun")]
            public int Tahun { get; set; }

            [JsonPropertyName("Bulan")]
            public string Bulan { get; set; }

            [JsonPropertyName("Sistem")]
            public string Sistem { get; set; }

            [JsonPropertyName("Surat")]
            public string Surat { get; set; }

            [JsonPropertyName("KetetapanPokok")]
            public int KetetapanPokok { get; set; }

            [JsonPropertyName("KetetapanSanksiSK")]
            public int KetetapanSanksiSK { get; set; }

            [JsonPropertyName("KetetapanTotal")]
            public int KetetapanTotal { get; set; }

            [JsonPropertyName("SetoranPokok")]
            public int SetoranPokok { get; set; }

            [JsonPropertyName("SetoranDenda")]
            public int SetoranDenda { get; set; }

            [JsonPropertyName("SetoranTotal")]
            public int SetoranTotal { get; set; }

            [JsonPropertyName("TunggakanPokok")]
            public int TunggakanPokok { get; set; }

            [JsonPropertyName("TunggakanSanksiSK")]
            public int TunggakanSanksiSK { get; set; }

            [JsonPropertyName("TunggakanPersen")]
            public int TunggakanPersen { get; set; }

            [JsonPropertyName("TunggakanDenda")]
            public int TunggakanDenda { get; set; }

            [JsonPropertyName("TunggakanTotal")]
            public int TunggakanTotal { get; set; }

            [JsonPropertyName("TglSetoran")]
            public string TglSetoran { get; set; }

            [JsonPropertyName("LokasiSetoran")]
            public string LokasiSetoran { get; set; }

            [JsonPropertyName("OperatorSetoran")]
            public string OperatorSetoran { get; set; }

            [JsonPropertyName("Restitusi")]
            public int Restitusi { get; set; }

            [JsonPropertyName("RestitusiTotal")]
            public int RestitusiTotal { get; set; }
        }

        public class ResponseKartuData
        {
            [JsonPropertyName("KartuDataList")]
            public List<KartuDataList> KartuDataList { get; set; }

            [JsonPropertyName("RangeTahun")]
            public List<object> RangeTahun { get; set; }

            [JsonPropertyName("RangeBulan")]
            public List<DateTime> RangeBulan { get; set; }

            [JsonPropertyName("NPWPD")]
            public string NPWPD { get; set; }

            [JsonPropertyName("AlamatWP")]
            public string AlamatWP { get; set; }

            [JsonPropertyName("NamaWP")]
            public string NamaWP { get; set; }

            [JsonPropertyName("NOP")]
            public string NOP { get; set; }

            [JsonPropertyName("NamaOP")]
            public string NamaOP { get; set; }

            [JsonPropertyName("AlamatOP")]
            public string AlamatOP { get; set; }

            [JsonPropertyName("JenisPajak")]
            public string JenisPajak { get; set; }

            [JsonPropertyName("UPTB")]
            public string UPTB { get; set; }
        }

    }

}
