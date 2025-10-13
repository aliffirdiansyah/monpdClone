using Microsoft.EntityFrameworkCore;
using MonPDLib.EFReklame;
using MonPDLib.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonPDLib.Lib
{
    public class KalkullatorReklame
    {
        public class ReklameInput
        {
            public decimal Panjang { get; set; }
            public decimal Lebar { get; set; }
            public decimal Tinggi { get; set; }
            public int KelasJalan { get; set; }
            public int SudutPandang { get; set; }
            public EnumFactory.KategoriReklame JenisReklame { get; set; }
            public int MasaPajak { get; set; }
            public EnumFactory.ProdukReklame JenisProduk { get; set; }
            public EnumFactory.LetakReklame LetakReklame { get; set; }
        }

        public decimal Luas { get; private set; }
        public decimal NjopLuas { get; private set; }
        public decimal NjopKetinggian { get; private set; }
        public decimal TotalNjop { get; private set; }
        public decimal SkorLokasi { get; private set; }
        public decimal SkorPandang { get; private set; }
        public decimal SkorKetinggian { get; private set; }
        public decimal TotalNilaiStrategis { get; private set; }
        public decimal TotalNjopStrategis { get; private set; }
        public decimal PenambahanKetinggian { get; private set; }
        public decimal PokokPajak { get; private set; }
        public decimal ProdukRokok { get; private set; }
        public decimal TotalNilaiSewa { get; private set; }
        public decimal JaminanBongkar { get; private set; }

        private static ReklameContext _context = DBClass.GetReklameContext();

        public static KalkullatorReklame HitungNilaiSewaReklame(ReklameInput input)
        {
            decimal luas = input.Panjang * input.Lebar;
            DateTime today = DateTime.Today;

            // 1️⃣ Ambil NSR Luas (cek tanggal berlaku)
            var nsrLuas = _context.MNsrLuas
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                         && x.MasaPajak == input.MasaPajak
                         && luas >= x.MinLuas
                         && (x.MaxLuas == null || luas <= x.MaxLuas)
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            if (nsrLuas == null)
                throw new Exception("NSR Luas tidak ditemukan atau tidak berlaku.");

            // 2️⃣ Ambil NSR Tinggi
            var nsrTinggi = _context.MNsrTinggis
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                         && x.MasaPajak == input.MasaPajak
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            if (nsrTinggi == null)
                throw new Exception("NSR Tinggi tidak ditemukan atau tidak berlaku.");

            // 3️⃣ Ambil nilai satuan strategis (NSS)
            var nss = _context.MNilaiSatuanStrategis
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                         && x.MasaPajak == input.MasaPajak
                         && luas >= x.MinLuas
                         && (x.MaxLuas == null || luas <= x.MaxLuas)
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            if (nss == null)
                throw new Exception("Nilai Satuan Strategis tidak ditemukan atau tidak berlaku.");

            int letak = 1; // Default indoor
            if (input.LetakReklame == EnumFactory.LetakReklame.Outdoor)
            {
                letak = 0;
            }

            // 4️⃣ Hitung NJOP dasar (luas + tinggi)
            decimal njopLuas = luas * (nsrLuas.NilaiSewa);
            decimal njopKetinggian = input.Tinggi * (nsrTinggi.NilaiKetinggian);
            decimal totalNjop = njopLuas + njopKetinggian;

            // 5️⃣ Ambil skor & bobot dari masing-masing tabel strategis (dengan tanggal berlaku)
            var lokasi = _context.MNilaiStrategisLokasis
                .Where(x => x.KelasJalan == input.KelasJalan
                         && x.IsDlmRuang == letak
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            var pandang = _context.MNilaiStrategisSpandangs
                .Where(x => x.SudutPandang == input.SudutPandang
                         && x.IsDlmRuang == letak
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            var tinggiData = _context.MNilaiStrategisTinggis
                .Where(x => input.Tinggi >= x.MinKetinggian
                            && (x.MaxKetinggian == null || input.Tinggi <= x.MaxKetinggian)
                            && x.TglAwalBerlaku <= today
                            && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();
            if (letak == 0)
            {
                tinggiData = _context.MNilaiStrategisTinggis
                    .Where(x => input.Tinggi >= x.MinKetinggian
                             && x.MinKetinggian > 0
                             && (x.MaxKetinggian == null || input.Tinggi <= x.MaxKetinggian)
                             && x.TglAwalBerlaku <= today
                             && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                    .OrderByDescending(x => x.TglAwalBerlaku)
                    .FirstOrDefault();
                if (tinggiData == null)
                    throw new Exception("Nilai strategis tinggi tidak ditemukan atau tidak berlaku.");
            }

            if (lokasi == null || pandang == null || tinggiData == null)
                throw new Exception("Nilai strategis lokasi / sudut pandang / tinggi tidak ditemukan atau tidak berlaku.");

            // 6️⃣ Hitung nilai strategis (skor * bobot)
            decimal skorLokasiBobot = 0m;
            decimal skorPandangBobot = 0m;
            decimal skorTinggiBobot = 0m;
            decimal totalStrategis = 0m;

            var def = _context.MNilaiStrategisDefs
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                    && x.TglAwalBerlaku <= today
                    && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();
            if (def != null)
            {
                skorLokasiBobot = def.Lokasi * lokasi.Bobot;
                skorPandangBobot = def.Spandang * pandang.Bobot;
                skorTinggiBobot = def.Ketinggian * tinggiData.Bobot;
                totalStrategis = skorLokasiBobot + skorPandangBobot + skorTinggiBobot;
            }
            skorLokasiBobot = lokasi.Skor * lokasi.Bobot;
            skorPandangBobot = pandang.Skor * pandang.Bobot;
            skorTinggiBobot = tinggiData.Skor * tinggiData.Bobot;
            totalStrategis = skorLokasiBobot + skorPandangBobot + skorTinggiBobot;

            // 7️⃣ Total Nilai Strategis (dikalikan NSS)
            decimal totalNilaiStrategis = totalStrategis * (nss.NilaiSatuan);
            decimal totalNjopStrategis = totalNjop + totalNilaiStrategis;

            // 8️⃣ Tambahan tinggi (jika > 15m, tiap 15m = +20%)
            decimal penambahanKetinggian = 0;
            if (input.Tinggi > 15)
            {
                // Hitung jumlah kelipatan 15 meter (pembulatan ke bawah)
                int kelipatan = (int)(input.Tinggi / 15);

                // Setiap kelipatan menambah 20%
                decimal tambahanPersen = 0.20m * kelipatan;

                if (tambahanPersen == 0)
                {
                    penambahanKetinggian = totalNjopStrategis;
                }

                penambahanKetinggian = totalNjopStrategis * tambahanPersen;
            }

            decimal totalSetelahKetinggian = totalNjopStrategis + penambahanKetinggian;

            // 9️⃣ Pokok Pajak 25%
            decimal pokokPajak = totalSetelahKetinggian * 0.25m;

            // 🔟 Tambahan 25% jika produk rokok
            decimal produkRokok = 0;
            if (input.JenisProduk == EnumFactory.ProdukReklame.Rokok)
            {
                produkRokok = pokokPajak * 0.25m;
            }

            decimal totalNilaiSewa = pokokPajak + produkRokok;

            // ⓫ Jaminan bongkar
            decimal jaminanBongkar = luas * 50000;

            return new KalkullatorReklame
            {
                Luas = luas,
                NjopLuas = njopLuas,
                NjopKetinggian = njopKetinggian,
                TotalNjop = totalNjop,
                SkorLokasi = skorLokasiBobot,
                SkorPandang = skorPandangBobot,
                SkorKetinggian = skorTinggiBobot,
                TotalNilaiStrategis = totalNilaiStrategis,
                TotalNjopStrategis = totalNjopStrategis,
                PenambahanKetinggian = penambahanKetinggian,
                PokokPajak = pokokPajak,
                ProdukRokok = produkRokok,
                TotalNilaiSewa = totalNilaiSewa,
                JaminanBongkar = jaminanBongkar
            };
        }

    }
}
