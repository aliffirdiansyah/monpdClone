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
    public class KalkulatorReklamePermanen
    {
        public class ReklameInput
        {
            public int IdJalan { get; set; }
            public decimal Panjang { get; set; }
            public decimal Lebar { get; set; }
            public decimal Tinggi { get; set; }
            public int SudutPandang { get; set; }
            public int Sisi { get; set; }
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

        public static KalkulatorReklamePermanen HitungNilaiSewaReklame(ReklameInput input)
        {
            DateTime today = DateTime.Today;
            var setting = new SettingReklame();

            input.Panjang = Math.Round(input.Panjang, 2);
            input.Lebar = Math.Round(input.Lebar, 2);

            var kelasJalan = 0;
            var kawasan = EnumFactory.KawasanReklame.NonPenataan;
            var jenisReklame = input.JenisReklame;
            decimal luas = input.Panjang * input.Lebar;

            var jalanData = _context.MJalans
                .Include(x => x.MJalanKawasans)
                .Where(x => x.IdJalan == input.IdJalan)
                .FirstOrDefault();
            if (jalanData == null)
            {
                throw new ArgumentException("Data jalan tidak ditemukan.");
            }

            var distinctKelasJalan = _context.MJalanKawasans
                .Where(x => x.IdJalan == jalanData.IdJalan)
                .Select(x => x.KelasJalanId).Distinct()
                .ToList();
            if (distinctKelasJalan.Count > 1)
            {
                throw new Exception("Data jalan memiliki kelas jalan yang berbeda");
            }

            var IdJalanPenataan = new List<int>
            {
                7,
                1536,
                1177,
                110,
                374,
                23,
                1109
            };
            var IdJalanIrisan = new List<int>
            {
                853,
                854,
                6,
                1504,
                88,
                293,
                115,
                1111,
                1494,
                375,
                1560,
                1000,
                1352,
                645,
                982,
                817,
                1179,
                1129,
                244,
                253
            };


            kelasJalan = distinctKelasJalan.First();
            if (kelasJalan == 1)
            {
                if (jenisReklame == EnumFactory.KategoriReklame.Megatron)
                {
                    kawasan = EnumFactory.KawasanReklame.Penataan;
                }
                else if (IdJalanPenataan.Contains(jalanData.IdJalan))
                {
                    kawasan = EnumFactory.KawasanReklame.Penataan;
                }
                else if (IdJalanIrisan.Contains(jalanData.IdJalan))
                {
                    if (luas > 8)
                    {
                        kawasan = EnumFactory.KawasanReklame.Penataan;
                    }
                }
            }

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

            //if (nsrTinggi == null)
            //    throw new ArgumentException("NSR Tinggi tidak ditemukan atau tidak berlaku.");

            // 3️⃣ Ambil nilai satuan strategis (NSS)
            var nss = _context.MNilaiSatuanStrategis
                    .Where(x => x.IdJenisReklame == (int)input.JenisReklame
                             && x.Kawasan == (int)kawasan
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
            else
            {
                kelasJalan = 3;
            }

            // 4️⃣ Hitung NJOP dasar (luas + tinggi)
            decimal njopLuas = Math.Round(luas, 2) * (nsrLuas.NilaiSewa);
            decimal njopKetinggian = Math.Round(input.Tinggi, 2) * (nsrTinggi?.NilaiKetinggian ?? 0m);
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

            //sudut pandang = 25 => skor == 10
            if (input.SudutPandang > 4)
            {
                pandang = _context.MNilaiStrategisSpandangs
                    .Where(x => x.SudutPandang == 5
                             && x.TglAwalBerlaku <= today
                             && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                    .OrderByDescending(x => x.TglAwalBerlaku)
                    .FirstOrDefault();
            }
            var tinggiData = _context.MNilaiStrategisTinggis
                .Where(x => input.Tinggi >= x.MinKetinggian
                            && (x.MaxKetinggian == null || input.Tinggi <= x.MaxKetinggian)
                            && x.TglAwalBerlaku <= today
                            && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();
            if (letak == 0)
            {
                lokasi = _context.MNilaiStrategisLokasis
                .Where(x => x.IsDlmRuang == letak
                         && x.KelasJalan == kelasJalan
                         && x.TglAwalBerlaku <= today
                         && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                .OrderByDescending(x => x.TglAwalBerlaku)
                .FirstOrDefault();

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
                if (input.SudutPandang > 4)
                {
                    pandang = _context.MNilaiStrategisSpandangs
                        .Where(x => x.SudutPandang == 5
                                 && x.TglAwalBerlaku <= today
                                 && (x.TglAkhirBerlaku == null || x.TglAkhirBerlaku >= today))
                        .OrderByDescending(x => x.TglAwalBerlaku)
                        .FirstOrDefault();
                }

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
            else
            {
                skorLokasi = lokasi.Skor;
                skorPandang = pandang.Skor;
                skorTinggi = tinggiData.Skor;
                totalskor = skorLokasi + skorPandang + skorTinggi;
                skorLokasiBobot = lokasi.Skor * lokasi.Bobot;
                skorPandangBobot = pandang.Skor * pandang.Bobot;
                skorTinggiBobot = tinggiData.Skor * tinggiData.Bobot;
                totalStrategis = skorLokasiBobot + skorPandangBobot + skorTinggiBobot;
            }

            // 7️⃣ Total Nilai Strategis (dikalikan NSS)
            decimal totalNilaiStrategis = totalStrategis * (nss.NilaiSatuan);
            decimal totalNjopStrategis = totalNjop + totalNilaiStrategis;

            // 8️⃣ Tambahan tinggi (jika > 15m, tiap 15m = +20%)
            decimal tambahanPersen = setting.NILAI_KETINGGIAN;
            decimal penambahanKetinggian = 0;
            if (input.Tinggi > setting.TAMBAH_KETINGGIAN)
            {
                // Hitung jumlah kelipatan 15 meter (pembulatan ke bawah)
                int kelipatan = (int)(input.Tinggi / setting.TAMBAH_KETINGGIAN);

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
            decimal pokokPajak = totalSetelahKetinggian * setting.PERSEN_PAJAK;

            // 🔟 Tambahan 25% jika produk rokok
            decimal produkRokok = 0;
            if (input.JenisProduk == EnumFactory.ProdukReklame.Rokok)
            {
                produkRokok = pokokPajak * setting.PERSEN_ROKOK;
            }

            decimal nilaiJambong = 0m;
            if (luas <= 8)
            {
                nilaiJambong = 50000;
            }
            else
            {
                nilaiJambong = 200000;
            }

            decimal totalNilaiSewa = 0m;
            decimal jaminanBongkar = 0m;

            if (input.Sisi == 0)
            {
                totalNilaiSewa = pokokPajak + produkRokok;

                jaminanBongkar = Math.Round(luas, 2) * nilaiJambong;
            }
            else
            {
                totalNilaiSewa = (pokokPajak + produkRokok) * input.Sisi;

                jaminanBongkar = (Math.Round(luas, 2) * nilaiJambong) * input.Sisi;
            }

            // 🧮 Pembulatan ke atas ke kelipatan Rp 100
            totalNilaiSewa = Math.Ceiling(totalNilaiSewa / 100) * 100;
            jaminanBongkar = Math.Ceiling(jaminanBongkar / 100) * 100;

            return new KalkulatorReklamePermanen
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
                NsrKetinggian = nsrTinggi?.NilaiKetinggian ?? 0,
                Nss = nss.NilaiSatuan,
                BobotLokasiNilai = lokasi.Bobot,
                BobotPandangNilai = pandang.Bobot,
                BobotKetinggianNilai = tinggiData.Bobot,
                NamaJalan = jalanData.NamaJalan,
                Kawasan = (EnumFactory.KawasanReklame)kawasan,
                KelasJalan = "Kelas " + Convert.ToString(kelasJalan),
            };
        }
        public static KalkulatorReklamePermanen HitungNilaiSewaReklame(decimal NilaiKontrak)
        {
            decimal totalNilaiSewa = 0m;
            decimal jaminanBongkar = 0m;
            decimal pokokPajak = NilaiKontrak * 0.25m;

            totalNilaiSewa = (pokokPajak);


            // 🧮 Pembulatan ke atas ke kelipatan Rp 100
            totalNilaiSewa = Math.Ceiling(totalNilaiSewa / 100) * 100;
            jaminanBongkar = Math.Ceiling(jaminanBongkar / 100) * 100;

            return new KalkulatorReklamePermanen
            {
                PokokPajak = pokokPajak,
                TotalNilaiSewa = totalNilaiSewa,
                JaminanBongkar = jaminanBongkar,
            };
        }
    }
}
