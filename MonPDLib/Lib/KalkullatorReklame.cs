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
            public decimal IdJalan { get; set; }
            public decimal Panjang { get; set; }
            public decimal Lebar { get; set; }
            public decimal Tinggi { get; set; }
            public int SudutPandang { get; set; }
            public int IdJenisReklame { get; set; }
            public int IdJenisProduk { get; set; }
            public int IdLetakReklame { get; set; }
            public EnumFactory.KategoriReklame JenisReklame => (EnumFactory.KategoriReklame)IdJenisReklame;
            public EnumFactory.ProdukReklame JenisProduk => (EnumFactory.ProdukReklame)IdJenisProduk;
            public EnumFactory.LetakReklame LetakReklame => (EnumFactory.LetakReklame)IdLetakReklame;
        }

        public decimal Luas { get; private set; }
        public decimal NjopLuas { get; private set; }
        public decimal NjopKetinggian { get; private set; }
        public decimal TotalNjop { get; private set; }
        public decimal SkorLokasi { get; private set; }
        public decimal SkorPandang { get; private set; }
        public decimal SkorKetinggian { get; private set; }
        public decimal TotalSkor { get; private set; }
        public decimal HasilNssLokasi { get; private set; }
        public decimal HasilNssPandang { get; private set; }
        public decimal HasilNssKetinggian { get; private set; }
        public decimal TotalNilaiStrategis { get; private set; }
        public decimal TotalNjopStrategis { get; private set; }
        public decimal PersenTambahKetinggian { get; private set; }
        public decimal PenambahanKetinggian { get; private set; }
        public decimal TotalAfterKetinggian { get; private set; }
        public decimal PokokPajak { get; private set; }
        public decimal ProdukRokok { get; private set; }
        public decimal TotalNilaiSewa { get; private set; }
        public decimal JaminanBongkar { get; private set; }

        //---------------------------------------------------------------
        public decimal NsrLuas { get; private set; }
        public decimal NsrKetinggian { get; private set; }
        public decimal Nss { get; private set; }
        public decimal BobotLokasiNilai { get; private set; }
        public decimal BobotPandangNilai { get; private set; }
        public decimal BobotKetinggianNilai { get; private set; }
        public EnumFactory.KawasanReklame Kawasan { get; set; }
        public string NamaJalan { get; set; }
        public string KelasJalan { get; set; }
        public int MasaPajak { get; set; } = 12;

        private static ReklameContext _context = DBClass.GetReklameContext();

        public static KalkullatorReklame HitungNilaiSewaReklame(ReklameInput input)
        {
            decimal luas = input.Panjang * input.Lebar;
            DateTime today = DateTime.Today;

            var jalan = _context.MJalans
                .Where(x => x.IdJalan == input.IdJalan)
                .FirstOrDefault();

            if (jalan == null)
                throw new ArgumentException("Data jalan tidak ditemukan.");

            // 1️⃣ Ambil NSR Luas (cek tanggal berlaku)
            var nsrLuas = _context.MNsrLuas
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                         && luas >= x.MinLuas
                         && (x.MaxLuas == null || luas <= x.MaxLuas)
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            if (nsrLuas == null)
                throw new ArgumentException("NSR Luas tidak ditemukan atau tidak berlaku.");

            // 2️⃣ Ambil NSR Tinggi
            var nsrTinggi = _context.MNsrTinggis
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            if (nsrTinggi == null)
                throw new ArgumentException("NSR Tinggi tidak ditemukan atau tidak berlaku.");

            // 3️⃣ Ambil nilai satuan strategis (NSS)
            var nss = _context.MNilaiSatuanStrategis
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                         && luas >= x.MinLuas
                         && (x.MaxLuas == null || luas <= x.MaxLuas)
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            if (nss == null)
                throw new ArgumentException("Nilai Satuan Strategis tidak ditemukan atau tidak berlaku.");

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
                .Where(x => x.IsDlmRuang == letak
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

            var pandang = _context.MNilaiStrategisSpandangs
                .Where(x => x.IsDlmRuang == letak
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
                             && x.MinKetinggian >= 0
                             && (x.MaxKetinggian == null || input.Tinggi <= x.MaxKetinggian)
                             && x.TglAwalBerlaku <= today
                             && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                    .OrderByDescending(x => x.TglAwalBerlaku)
                    .FirstOrDefault();
                pandang = _context.MNilaiStrategisSpandangs
                .Where(x => x.SudutPandang == input.SudutPandang
                         && x.IsDlmRuang == letak
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

                tinggiData = _context.MNilaiStrategisTinggis
                    .Where(x => input.Tinggi >= x.MinKetinggian
                                && (x.MaxKetinggian == null || input.Tinggi <= x.MaxKetinggian)
                                && x.TglAwalBerlaku <= today
                                && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                    .OrderByDescending(x => x.TglAwalBerlaku)
                    .FirstOrDefault();

                if (lokasi == null || pandang == null || tinggiData == null)
                    throw new ArgumentException("Nilai strategis lokasi / sudut pandang / tinggi tidak ditemukan atau tidak berlaku.");
            }

            if (lokasi == null || pandang == null || tinggiData == null)
                throw new ArgumentException("Nilai strategis lokasi / sudut pandang / tinggi tidak ditemukan atau tidak berlaku.");

            // 6️⃣ Hitung nilai strategis (skor * bobot)
            decimal skorLokasiBobot = 0m;
            decimal skorPandangBobot = 0m;
            decimal skorTinggiBobot = 0m;
            decimal totalStrategis = 0m;

            decimal skorLokasi = 0m;
            decimal skorPandang = 0m;
            decimal skorTinggi = 0m;
            decimal totalskor = 0m;

            var def = _context.MNilaiStrategisDefs
                .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                    && x.TglAwalBerlaku <= today
                    && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();
            if (def != null)
            {
                skorLokasi = def.Lokasi;
                skorPandang = def.Spandang;
                skorTinggi = def.Ketinggian;
                totalskor = skorLokasi + skorPandang + skorTinggi;

                skorLokasiBobot = def.Lokasi * lokasi.Bobot;
                skorPandangBobot = def.Spandang * pandang.Bobot;
                skorTinggiBobot = def.Ketinggian * tinggiData.Bobot;
                totalStrategis = skorLokasiBobot + skorPandangBobot + skorTinggiBobot;
            }
            skorLokasi = lokasi.Skor;
            skorPandang = pandang.Skor;
            skorTinggi = tinggiData.Skor;
            totalskor = skorLokasi + skorPandang + skorTinggi;

            skorLokasiBobot = lokasi.Skor * lokasi.Bobot;
            skorPandangBobot = pandang.Skor * pandang.Bobot;
            skorTinggiBobot = tinggiData.Skor * tinggiData.Bobot;
            totalStrategis = skorLokasiBobot + skorPandangBobot + skorTinggiBobot;

            // 7️⃣ Total Nilai Strategis (dikalikan NSS)
            decimal totalNilaiStrategis = totalStrategis * (nss.NilaiSatuan);
            decimal totalNjopStrategis = totalNjop + totalNilaiStrategis;

            // 8️⃣ Tambahan tinggi (jika > 15m, tiap 15m = +20%)
            decimal tambahanPersen = 0.20m;
            decimal penambahanKetinggian = 0;
            if (input.Tinggi > 15)
            {
                // Hitung jumlah kelipatan 15 meter (pembulatan ke bawah)
                int kelipatan = (int)(input.Tinggi / 15);

                // Setiap kelipatan menambah 20%
                tambahanPersen = tambahanPersen * kelipatan;

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
                SkorLokasi = skorLokasi,
                SkorPandang = skorPandang,
                SkorKetinggian = skorTinggi,
                TotalSkor = totalStrategis,
                HasilNssLokasi = skorLokasiBobot,
                HasilNssPandang = skorPandangBobot,
                HasilNssKetinggian = skorTinggiBobot,
                TotalNilaiStrategis = totalNilaiStrategis,
                TotalNjopStrategis = totalNjopStrategis,
                PersenTambahKetinggian = tambahanPersen,
                PenambahanKetinggian = penambahanKetinggian,
                TotalAfterKetinggian = totalSetelahKetinggian,
                PokokPajak = pokokPajak,
                ProdukRokok = produkRokok,
                TotalNilaiSewa = totalNilaiSewa,
                JaminanBongkar = jaminanBongkar,
                NsrLuas = nsrLuas.NilaiSewa,
                NsrKetinggian = nsrTinggi.NilaiKetinggian,
                Nss = nss.NilaiSatuan,
                BobotLokasiNilai = lokasi.Bobot,
                BobotPandangNilai = pandang.Bobot,
                BobotKetinggianNilai = tinggiData.Bobot,
                NamaJalan = jalan.NamaJalan,
                Kawasan = (EnumFactory.KawasanReklame)jalan.Kawasan,
                KelasJalan = "Kelas " + Convert.ToString(jalan.KelasJalan.Value),
            };
        }

    }
}
