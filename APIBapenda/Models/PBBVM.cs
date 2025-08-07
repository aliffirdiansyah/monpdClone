namespace APIBapenda.Models
{
    public class PBBVM
    {
        public class PBBReq
        {
            public string PBB { get; set; } = "";     
        }

        public class IdPersilReq
        {
            public string Persil { get; set; } = "";
        }        

        public class KecamatanReq
        {
            public string Kecamatan { get; set; } = "";
        }        

        public class KecamatanKelurahanReq
        {
            public string Kecamatan { get; set; } = "";
            public string Kelurahan { get; set; } = "";
        }        
    }
}
