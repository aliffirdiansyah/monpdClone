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
    public class KalkulatorReklameInsidentil
    {
        public class ReklameInput
        {
            // --- Properti Umum ---
            public int IdJalan { get; set; }
            public int IdJenisReklame { get; set; }
            public int IdJenisProduk { get; set; }
            public int IdLetakReklame { get; set; }
            public EnumFactory.KategoriReklame JenisReklame => (EnumFactory.KategoriReklame)IdJenisReklame;
            public EnumFactory.ProdukReklame JenisProduk => (EnumFactory.ProdukReklame)IdJenisProduk;
            public EnumFactory.LetakReklame LetakReklame => (EnumFactory.LetakReklame)IdLetakReklame;

            public DateTime TglMulaiBerlaku { get; set; }
            public DateTime TglSelesaiBerlaku { get; set; }

            //--- Untuk Mode LUAS ---
            public int JumlahSatuan { get; set; }
            public decimal Panjang { get; set; }
            public decimal Lebar { get; set; }

            //--- Untuk Mode PERULANGAN ---
            public int JumlahPerulangan { get; set; }
            public int JumlahLayar { get; set; }

            // --- Properti Turunan (otomatis dihitung) ---
            public int LamaPenyelenggaraan
            {
                get
                {
                    if (TglMulaiBerlaku != DateTime.MinValue && TglSelesaiBerlaku != DateTime.MinValue)
                        return (TglSelesaiBerlaku - TglMulaiBerlaku).Days + 1;
                    return 0;
                }
            }
        }
        public decimal Luas { get; private set; }
        public decimal TarifDasar { get; private set; }
        public decimal NjopLuas { get; private set; }
        public decimal NjopKetinggian { get; private set; }
        public decimal TotalNjop { get; private set; }
        public decimal HasilNss { get; private set; }
        public decimal TotalNilaiStrategis { get; private set; }
        public decimal PokokPajak { get; private set; }
        public decimal ProdukRokok { get; private set; }
        public decimal TotalNilaiSewa { get; private set; }
        public decimal JaminanBongkar { get; private set; }

        //---------------------------------------------------------------
        public decimal Nsr { get; private set; }
        public decimal Nss { get; private set; }
        public decimal SatuanNominal { get; private set; }
        //public EnumFactory.KawasanReklame Kawasan { get; set; }
        public string NamaJalan { get; set; }
        public string KelasJalan { get; set; }
        public int MasaPajak { get; set; } = 12;
        public decimal JambongNilai { get; private set; }
        public EnumFactory.EModeUkur ModeUkur { get; set; }

        private static ReklameContext _context = DBClass.GetReklameContext();

        public static KalkulatorReklameInsidentil HitungNilaiSewaReklame(ReklameInput input)
        {
            var ret = new KalkulatorReklameInsidentil();
            var setting = new SettingReklame();
            if (input.LamaPenyelenggaraan > 30)
            {
                throw new ArgumentException("Masa berlaku reklame tidak boleh lebih dari 30 hari");
            }
            var jenisReklame = _context.MJenisReklames
                .FirstOrDefault(x => x.Kategori == (int)EnumFactory.JenisReklame.Insidentil && x.IdJenisReklame == input.IdJenisReklame);
            if (jenisReklame == null)
                throw new ArgumentException("Jenis Reklame tidak ditemukan.");

            var kelasJalan = 0;
            var kawasan = EnumFactory.KawasanReklame.NonPenataan;

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

            kelasJalan = distinctKelasJalan.First();

            ret.NamaJalan = jalanData.NamaJalan;
            ret.KelasJalan = "Kelas " + kelasJalan;


            var modeUkur = (EnumFactory.EModeUkur)jenisReklame.ModeUkur;
            var njop = _context.MNsrIns.Where(x => x.IdJenisReklame == input.IdJenisReklame).FirstOrDefault();
            var nss = _context.MNilaiStrategisJalanIns.Where(x => x.IdJenisReklame == input.IdJenisReklame && x.KelasJalan == kelasJalan).FirstOrDefault();
            var jambong = _context.MNsrInsJambongs.Where(x => x.IdJenisReklame == input.IdJenisReklame).FirstOrDefault();
            ret.JambongNilai = jambong?.Nilai ?? 0m;

            decimal luas = 0m;
            decimal nsr = 0m;
            decimal totalSebelumPajak = 0m;

            ret.ModeUkur = modeUkur;
            decimal minimDPP = 0m;
            if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.SelebaranBrosurLeaflet))
            {
                minimDPP = setting.MINIM_DPP_9_SELEBARAN;
            }
            else if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.StikerMelekat))
            {
                minimDPP = setting.MINIM_DPP_10_STIKER;
            }
            else if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.Peragaan))
            {
                minimDPP = setting.MINIM_DPP_15_PERAGAAN;
            }

            switch (modeUkur)
            {
                case EnumFactory.EModeUkur.Luas:

                    //if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.StikerMelekat))
                    //{
                    //    luas = (input.Panjang * input.Lebar) * 100;
                    //}
                    //else
                    //{
                    //}
                    //nsr = (((njop.NilaiNjop ?? 0) * luas) + ((nss?.Nilai ?? 0) * luas));
                    //if (nsr <= minimDPP)
                    //{
                    //    nsr = minimDPP;
                    //    ret.Nsr = minimDPP;
                    //}
                    //else
                    //{
                    //nsr = (((njop.NilaiNjop ?? 0) * luas) + ((nss?.Nilai ?? 0) * luas));
                    //ret.Nsr = nsr;
                    //}
                    luas = Math.Round((Math.Round(input.Panjang, 2) * Math.Round(input.Lebar, 2)), 2);
                    ret.Luas = luas;

                    nsr = (((njop.NilaiNjop ?? 0) * luas) + ((nss?.Nilai ?? 0) * luas));
                    ret.Nsr = nsr;

                    if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.Baliho))
                    {
                        totalSebelumPajak = input.JumlahSatuan * input.LamaPenyelenggaraan * nsr;
                    }
                    else if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.KainSpandukUmbulUmbul))
                    {
                        totalSebelumPajak = input.JumlahSatuan * input.LamaPenyelenggaraan * nsr;
                    }
                    else
                    {
                        totalSebelumPajak = input.JumlahSatuan * 1 * nsr;
                    }


                    ret.PokokPajak = totalSebelumPajak * (setting.PERSEN_PAJAK);

                    if (ret.PokokPajak <= minimDPP)
                    {
                        ret.PokokPajak = minimDPP;
                    }
                    else
                    {
                        ret.PokokPajak = totalSebelumPajak * setting.PERSEN_PAJAK;
                    }

                    if (input.JenisProduk == EnumFactory.ProdukReklame.Rokok)
                    {
                        ret.ProdukRokok = ret.PokokPajak * (setting.PERSEN_ROKOK);
                    }
                    else
                    {
                        ret.ProdukRokok = 0;
                    }
                    ret.TotalNilaiSewa = Math.Ceiling((ret.PokokPajak + ret.ProdukRokok) / 100) * 100;
                    ret.JaminanBongkar = Math.Ceiling(((decimal)(jambong?.Nilai ?? 0) * luas * input.JumlahSatuan) / 100m) * 100m;

                    break;
                case EnumFactory.EModeUkur.Satuan:
                    if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.Peragaan))
                    {
                        nsr = input.JumlahSatuan * (njop?.NilaiNjop ?? 0) * input.LamaPenyelenggaraan;
                    }
                    else if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.Suara))
                    {
                        nsr = input.JumlahSatuan * (njop?.NilaiNjop ?? 0) * input.LamaPenyelenggaraan;
                    }
                    else
                    {
                        nsr = input.JumlahSatuan * (njop?.NilaiNjop ?? 0) * 1;
                    }
                    totalSebelumPajak = nsr;
                    //if (nsr <= minimDPP)
                    //{
                    //    nsr = minimDPP;
                    //    ret.Nsr = minimDPP;
                    //    totalSebelumPajak = minimDPP;
                    //}
                    //else
                    //{
                    //    ret.Nsr = nsr;
                    //    totalSebelumPajak = nsr;
                    //}
                    ret.PokokPajak = totalSebelumPajak * setting.PERSEN_PAJAK;

                    if (ret.PokokPajak <= minimDPP)
                    {
                        ret.PokokPajak = minimDPP;
                    }
                    else
                    {
                        ret.PokokPajak = totalSebelumPajak * setting.PERSEN_PAJAK;
                    }

                    if (input.JenisProduk == EnumFactory.ProdukReklame.Rokok)
                    {
                        ret.ProdukRokok = ret.PokokPajak * (setting.PERSEN_ROKOK);
                    }
                    else
                    {
                        ret.ProdukRokok = 0;
                    }
                    ret.TotalNilaiSewa = Math.Ceiling((ret.PokokPajak + ret.ProdukRokok) / 100) * 100;
                    ret.JaminanBongkar = 0m;

                    break;
                case EnumFactory.EModeUkur.Perulangan:
                    if (input.JumlahSatuan > 0)
                    {
                        input.JumlahSatuan = (int)(Math.Ceiling(input.JumlahSatuan / 10.0m) * 10);
                    }
                    //nsr = ((decimal)input.JumlahSatuan / (njop?.SatuanNominal ?? 1)) * (input.JumlahLayar == 0 ? 1 : input.JumlahLayar) * (input.JumlahPerulangan == 0 ? 1 : input.JumlahPerulangan) * (njop?.NilaiNjop ?? 0) * input.LamaPenyelenggaraan;
                    if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.Suara))
                    {
                        nsr = ((decimal)input.JumlahSatuan / (njop?.SatuanNominal ?? 1)) * (input.JumlahLayar == 0 ? 1 : input.JumlahLayar) * (njop?.NilaiNjop ?? 0) * input.LamaPenyelenggaraan;
                    }
                    else if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.FilmSlide))
                    {
                        nsr = ((decimal)input.JumlahSatuan / (njop?.SatuanNominal ?? 1)) * (input.JumlahLayar == 0 ? 1 : input.JumlahLayar) * (input.JumlahPerulangan == 0 ? 1 : input.JumlahPerulangan) * (njop?.NilaiNjop ?? 0) * input.LamaPenyelenggaraan;
                    }
                    ret.SatuanNominal = (njop?.SatuanNominal ?? 1);
                    ret.Nsr = nsr;
                    totalSebelumPajak = nsr;
                    ret.PokokPajak = totalSebelumPajak * setting.PERSEN_PAJAK;
                    if (input.JenisProduk == EnumFactory.ProdukReklame.Rokok)
                    {
                        ret.ProdukRokok = ret.PokokPajak * (setting.PERSEN_ROKOK);
                    }
                    else
                    {
                        ret.ProdukRokok = 0;
                    }
                    ret.TotalNilaiSewa = Math.Ceiling((ret.PokokPajak + ret.ProdukRokok) / 100) * 100;
                    ret.JaminanBongkar = 0m;

                    break;
                default:
                    break;
            }

            return ret;
        }
    }
}
