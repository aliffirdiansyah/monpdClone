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
        public EnumFactory.EModeUkur ModeUkur { get; set; }

        private static ReklameContext _context = DBClass.GetReklameContext();

        public static KalkulatorReklameInsidentil HitungNilaiSewaReklame(ReklameInput input)
        {
            var ret = new KalkulatorReklameInsidentil();
            var setting = new SettingReklame();

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

            decimal luas = 0m;
            decimal nsr = 0m;
            decimal totalSebelumPajak = 0m;

            ret.ModeUkur = modeUkur;
            decimal minimDPP = 0m;
            if (jenisReklame.IdJenisReklame == (int)(EnumFactory.KategoriReklame.Baliho))
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

                    luas = input.Panjang * input.Lebar;
                    ret.Luas = luas;
                    if (nsr <= minimDPP)
                    {
                        ret.Nsr = 0m;
                        totalSebelumPajak = 0m;
                    }
                    else
                    {
                        nsr = ((njop.NilaiNjop ?? 0 * luas) + (nss.Nilai * luas));
                        ret.Nsr = nsr;
                    }
                    
                    totalSebelumPajak = input.JumlahSatuan * input.LamaPenyelenggaraan * nsr;
                    ret.PokokPajak = totalSebelumPajak * (setting.PERSEN_PAJAK);
                    ret.ProdukRokok = ret.PokokPajak * (setting.PERSEN_ROKOK);
                    ret.TotalNilaiSewa = ret.PokokPajak + ret.ProdukRokok;
                    ret.JaminanBongkar = (jambong?.Nilai ?? 0) * luas * input.JumlahSatuan;

                    break;
                case EnumFactory.EModeUkur.Satuan:

                    nsr = input.JumlahSatuan * (njop?.NilaiNjop ?? 0);

                    if (nsr <= minimDPP)
                    {
                        ret.Nsr = 0m;
                        totalSebelumPajak = 0m;
                    }
                    else
                    {
                        ret.Nsr = nsr;
                        totalSebelumPajak = minimDPP;
                    }
                    ret.PokokPajak = totalSebelumPajak * setting.PERSEN_PAJAK;
                    ret.ProdukRokok = ret.PokokPajak * setting.PERSEN_ROKOK;
                    ret.TotalNilaiSewa = ret.PokokPajak + ret.ProdukRokok;
                    ret.JaminanBongkar = 0m;

                    break;
                case EnumFactory.EModeUkur.Perulangan:

                    nsr = (input.LamaPenyelenggaraan / njop?.SatuanNominal ?? 0) * input.JumlahLayar * input.JumlahPerulangan * (njop?.NilaiNjop ?? 0);
                    ret.SatuanNominal = njop?.SatuanNominal ?? 0;
                    ret.Nsr = nsr;
                    totalSebelumPajak = nsr;
                    ret.PokokPajak = totalSebelumPajak * setting.PERSEN_PAJAK;
                    ret.ProdukRokok = ret.PokokPajak * setting.PERSEN_ROKOK;
                    ret.TotalNilaiSewa = ret.PokokPajak + ret.ProdukRokok;
                    ret.JaminanBongkar = 0m;

                    break;
                default:
                    break;
            }

            return ret;
        }
    }
}
