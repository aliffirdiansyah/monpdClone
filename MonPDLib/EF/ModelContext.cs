using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EF;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DbAkun> DbAkuns { get; set; }

    public virtual DbSet<DbAkunTarget> DbAkunTargets { get; set; }

    public virtual DbSet<DbAkunTargetBulan> DbAkunTargetBulans { get; set; }

    public virtual DbSet<DbAkunTargetBulanUptb> DbAkunTargetBulanUptbs { get; set; }

    public virtual DbSet<DbMonAbt> DbMonAbts { get; set; }

    public virtual DbSet<DbMonBphtb> DbMonBphtbs { get; set; }

    public virtual DbSet<DbMonHiburan> DbMonHiburans { get; set; }

    public virtual DbSet<DbMonHotel> DbMonHotels { get; set; }

    public virtual DbSet<DbMonOpsenBbnkb> DbMonOpsenBbnkbs { get; set; }

    public virtual DbSet<DbMonOpsenPkb> DbMonOpsenPkbs { get; set; }

    public virtual DbSet<DbMonParkir> DbMonParkirs { get; set; }

    public virtual DbSet<DbMonPbb> DbMonPbbs { get; set; }

    public virtual DbSet<DbMonPpj> DbMonPpjs { get; set; }

    public virtual DbSet<DbMonReklame> DbMonReklames { get; set; }

    public virtual DbSet<DbMonReklameEmail> DbMonReklameEmails { get; set; }

    public virtual DbSet<DbMonReklameSurat> DbMonReklameSurats { get; set; }

    public virtual DbSet<DbMonReklameSuratTegur> DbMonReklameSuratTegurs { get; set; }

    public virtual DbSet<DbMonReklameSuratTegurDok> DbMonReklameSuratTegurDoks { get; set; }

    public virtual DbSet<DbMonReklameUpaya> DbMonReklameUpayas { get; set; }

    public virtual DbSet<DbMonReklameUpayaDok> DbMonReklameUpayaDoks { get; set; }

    public virtual DbSet<DbMonResto> DbMonRestos { get; set; }

    public virtual DbSet<DbOpAbt> DbOpAbts { get; set; }

    public virtual DbSet<DbOpHiburan> DbOpHiburans { get; set; }

    public virtual DbSet<DbOpHotel> DbOpHotels { get; set; }

    public virtual DbSet<DbOpListrik> DbOpListriks { get; set; }

    public virtual DbSet<DbOpParkir> DbOpParkirs { get; set; }

    public virtual DbSet<DbOpPbb> DbOpPbbs { get; set; }

    public virtual DbSet<DbOpReklame> DbOpReklames { get; set; }

    public virtual DbSet<DbOpResto> DbOpRestos { get; set; }

    public virtual DbSet<DbRekamParkir> DbRekamParkirs { get; set; }

    public virtual DbSet<DbRekamRestoran> DbRekamRestorans { get; set; }

    public virtual DbSet<DetailUpayaReklame> DetailUpayaReklames { get; set; }

    public virtual DbSet<MFasilita> MFasilitas { get; set; }

    public virtual DbSet<MJenisKendaraan> MJenisKendaraans { get; set; }

    public virtual DbSet<MKategoriPajak> MKategoriPajaks { get; set; }

    public virtual DbSet<MKategoriUpaya> MKategoriUpayas { get; set; }

    public virtual DbSet<MPajak> MPajaks { get; set; }

    public virtual DbSet<MTindakanReklame> MTindakanReklames { get; set; }

    public virtual DbSet<MTipekamarhotel> MTipekamarhotels { get; set; }

    public virtual DbSet<MUpayaReklame> MUpayaReklames { get; set; }

    public virtual DbSet<MUserLogin> MUserLogins { get; set; }

    public virtual DbSet<MWilayah> MWilayahs { get; set; }

    public virtual DbSet<MvReklameSum> MvReklameSums { get; set; }

    public virtual DbSet<MvReklameSummary> MvReklameSummaries { get; set; }

    public virtual DbSet<MvSeriesPendapatan> MvSeriesPendapatans { get; set; }

    public virtual DbSet<MvSeriesTargetP> MvSeriesTargetPs { get; set; }

    public virtual DbSet<Npwpd> Npwpds { get; set; }

    public virtual DbSet<Op> Ops { get; set; }

    public virtual DbSet<OpAbt> OpAbts { get; set; }

    public virtual DbSet<OpAbtJadwal> OpAbtJadwals { get; set; }

    public virtual DbSet<OpAbtKapasita> OpAbtKapasitas { get; set; }

    public virtual DbSet<OpAbtKetetapan> OpAbtKetetapans { get; set; }

    public virtual DbSet<OpAbtKetetapanSspd> OpAbtKetetapanSspds { get; set; }

    public virtual DbSet<OpHiburan> OpHiburans { get; set; }

    public virtual DbSet<OpHiburanJadwal> OpHiburanJadwals { get; set; }

    public virtual DbSet<OpHiburanKetetapan> OpHiburanKetetapans { get; set; }

    public virtual DbSet<OpHiburanKetetapanSspd> OpHiburanKetetapanSspds { get; set; }

    public virtual DbSet<OpHiburanKtFilm> OpHiburanKtFilms { get; set; }

    public virtual DbSet<OpHiburanKtGym> OpHiburanKtGyms { get; set; }

    public virtual DbSet<OpHiburanKtOther> OpHiburanKtOthers { get; set; }

    public virtual DbSet<OpHotel> OpHotels { get; set; }

    public virtual DbSet<OpHotelBanquet> OpHotelBanquets { get; set; }

    public virtual DbSet<OpHotelFasilita> OpHotelFasilitas { get; set; }

    public virtual DbSet<OpHotelKamar> OpHotelKamars { get; set; }

    public virtual DbSet<OpHotelKetetapan> OpHotelKetetapans { get; set; }

    public virtual DbSet<OpHotelKetetapanSspd> OpHotelKetetapanSspds { get; set; }

    public virtual DbSet<OpHotelKost> OpHotelKosts { get; set; }

    public virtual DbSet<OpListrik> OpListriks { get; set; }

    public virtual DbSet<OpListrikJadwal> OpListrikJadwals { get; set; }

    public virtual DbSet<OpListrikKetetapan> OpListrikKetetapans { get; set; }

    public virtual DbSet<OpListrikKetetapanSspd> OpListrikKetetapanSspds { get; set; }

    public virtual DbSet<OpListrikSumberLain> OpListrikSumberLains { get; set; }

    public virtual DbSet<OpListrikSumberSendiri> OpListrikSumberSendiris { get; set; }

    public virtual DbSet<OpParkir> OpParkirs { get; set; }

    public virtual DbSet<OpParkirJadwal> OpParkirJadwals { get; set; }

    public virtual DbSet<OpParkirKetetapan> OpParkirKetetapans { get; set; }

    public virtual DbSet<OpParkirKetetapanSspd> OpParkirKetetapanSspds { get; set; }

    public virtual DbSet<OpParkirTarif> OpParkirTarifs { get; set; }

    public virtual DbSet<OpPbb> OpPbbs { get; set; }

    public virtual DbSet<OpPbbKetetapan> OpPbbKetetapans { get; set; }

    public virtual DbSet<OpPbbKetetapanSspd> OpPbbKetetapanSspds { get; set; }

    public virtual DbSet<OpResto> OpRestos { get; set; }

    public virtual DbSet<OpRestoFasilita> OpRestoFasilitas { get; set; }

    public virtual DbSet<OpRestoJadwal> OpRestoJadwals { get; set; }

    public virtual DbSet<OpRestoKetetapan> OpRestoKetetapans { get; set; }

    public virtual DbSet<OpRestoKetetapanSspd> OpRestoKetetapanSspds { get; set; }

    public virtual DbSet<OpRestoMenu> OpRestoMenus { get; set; }

    public virtual DbSet<PSb> PSbs { get; set; }

    public virtual DbSet<PT> PTs { get; set; }

    public virtual DbSet<PTb> PTbs { get; set; }

    public virtual DbSet<PenHimbauanBayar> PenHimbauanBayars { get; set; }

    public virtual DbSet<PenTeguranBayar> PenTeguranBayars { get; set; }

    public virtual DbSet<PotensiCtrlAirTanah> PotensiCtrlAirTanahs { get; set; }

    public virtual DbSet<PotensiCtrlHiburan> PotensiCtrlHiburans { get; set; }

    public virtual DbSet<PotensiCtrlHotel> PotensiCtrlHotels { get; set; }

    public virtual DbSet<PotensiCtrlParkir> PotensiCtrlParkirs { get; set; }

    public virtual DbSet<PotensiCtrlPpj> PotensiCtrlPpjs { get; set; }

    public virtual DbSet<PotensiCtrlReklame> PotensiCtrlReklames { get; set; }

    public virtual DbSet<PotensiCtrlRestoran> PotensiCtrlRestorans { get; set; }

    public virtual DbSet<PotensiCtrlTarget> PotensiCtrlTargets { get; set; }

    public virtual DbSet<SetLastRun> SetLastRuns { get; set; }

    public virtual DbSet<SetYearJobScan> SetYearJobScans { get; set; }

    public virtual DbSet<THimbauanSptpd> THimbauanSptpds { get; set; }

    public virtual DbSet<TMutasiPiutang> TMutasiPiutangs { get; set; }

    public virtual DbSet<TPemeriksaan> TPemeriksaans { get; set; }

    public virtual DbSet<TPendapatanDaerah> TPendapatanDaerahs { get; set; }

    public virtual DbSet<TPenungguanSptpd> TPenungguanSptpds { get; set; }

    public virtual DbSet<TPenungguanSptpdMamin> TPenungguanSptpdMamins { get; set; }

    public virtual DbSet<TPenungguanSptpdParkir> TPenungguanSptpdParkirs { get; set; }

    public virtual DbSet<TPiutangAbt> TPiutangAbts { get; set; }

    public virtual DbSet<TPiutangBphtb> TPiutangBphtbs { get; set; }

    public virtual DbSet<TPiutangHiburan> TPiutangHiburans { get; set; }

    public virtual DbSet<TPiutangHotel> TPiutangHotels { get; set; }

    public virtual DbSet<TPiutangListrik> TPiutangListriks { get; set; }

    public virtual DbSet<TPiutangOpsenBbnkb> TPiutangOpsenBbnkbs { get; set; }

    public virtual DbSet<TPiutangOpsenPkb> TPiutangOpsenPkbs { get; set; }

    public virtual DbSet<TPiutangParkir> TPiutangParkirs { get; set; }

    public virtual DbSet<TPiutangPbb> TPiutangPbbs { get; set; }

    public virtual DbSet<TPiutangReklame> TPiutangReklames { get; set; }

    public virtual DbSet<TPiutangResto> TPiutangRestos { get; set; }

    public virtual DbSet<TSeriesPendapatan> TSeriesPendapatans { get; set; }

    public virtual DbSet<TSeriesTargetP> TSeriesTargetPs { get; set; }

    public virtual DbSet<TSuratReklameTeguranFile> TSuratReklameTeguranFiles { get; set; }

    public virtual DbSet<TTeguranSptpd> TTeguranSptpds { get; set; }

    public virtual DbSet<TempPiutang> TempPiutangs { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseOracle("User Id=monpd;Password=monpd2025;Data Source=10.21.39.80:1521/DEVDB;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("MONPD");

        modelBuilder.Entity<DbAkun>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Akun, e.Kelompok, e.Jenis, e.Objek, e.Rincian, e.SubRincian }).HasName("DB_AKUN_PK");

            entity.HasMany(d => d.KategoriKenaikans).WithMany(p => p.DbAkunsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "DbAkunKategoriKenaikan",
                    r => r.HasOne<MKategoriPajak>().WithMany()
                        .HasForeignKey("KategoriKenaikan")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DB_AKUN_KATEGORI_KENAIKAN_R02"),
                    l => l.HasOne<DbAkun>().WithMany()
                        .HasForeignKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DB_AKUN_KATEGORI_KENAIKAN_R01"),
                    j =>
                    {
                        j.HasKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "KategoriKenaikan").HasName("DB_AKUN_KATEGORI_KENAIKAN_PK");
                        j.ToTable("DB_AKUN_KATEGORI_KENAIKAN");
                        j.IndexerProperty<decimal>("TahunBuku")
                            .HasColumnType("NUMBER")
                            .HasColumnName("TAHUN_BUKU");
                        j.IndexerProperty<string>("Akun")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("AKUN");
                        j.IndexerProperty<string>("Kelompok")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("KELOMPOK");
                        j.IndexerProperty<string>("Jenis")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("JENIS");
                        j.IndexerProperty<string>("Objek")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("OBJEK");
                        j.IndexerProperty<string>("Rincian")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("RINCIAN");
                        j.IndexerProperty<string>("SubRincian")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("SUB_RINCIAN");
                        j.IndexerProperty<decimal>("KategoriKenaikan")
                            .HasColumnType("NUMBER")
                            .HasColumnName("KATEGORI_KENAIKAN");
                    });

            entity.HasMany(d => d.KategoriSanksis).WithMany(p => p.DbAkuns1)
                .UsingEntity<Dictionary<string, object>>(
                    "DbAkunKategoriSanksi",
                    r => r.HasOne<MKategoriPajak>().WithMany()
                        .HasForeignKey("KategoriSanksi")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("KATEGORI_SANKSI_FK"),
                    l => l.HasOne<DbAkun>().WithMany()
                        .HasForeignKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DB_AKUN_KATEGORI_SANKSI_R01"),
                    j =>
                    {
                        j.HasKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "KategoriSanksi").HasName("DB_AKUN_KATEGORI_SANKSI_PK");
                        j.ToTable("DB_AKUN_KATEGORI_SANKSI");
                        j.IndexerProperty<decimal>("TahunBuku")
                            .HasColumnType("NUMBER")
                            .HasColumnName("TAHUN_BUKU");
                        j.IndexerProperty<string>("Akun")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("AKUN");
                        j.IndexerProperty<string>("Kelompok")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("KELOMPOK");
                        j.IndexerProperty<string>("Jenis")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("JENIS");
                        j.IndexerProperty<string>("Objek")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("OBJEK");
                        j.IndexerProperty<string>("Rincian")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("RINCIAN");
                        j.IndexerProperty<string>("SubRincian")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("SUB_RINCIAN");
                        j.IndexerProperty<decimal>("KategoriSanksi")
                            .HasColumnType("NUMBER")
                            .HasColumnName("KATEGORI_SANKSI");
                    });

            entity.HasMany(d => d.Kategoris).WithMany(p => p.DbAkuns)
                .UsingEntity<Dictionary<string, object>>(
                    "DbAkunKategori",
                    r => r.HasOne<MKategoriPajak>().WithMany()
                        .HasForeignKey("Kategori")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("KATEGORI_FK"),
                    l => l.HasOne<DbAkun>().WithMany()
                        .HasForeignKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("DB_AKUN_KATEGORI_DB_AKUN_FK"),
                    j =>
                    {
                        j.HasKey("TahunBuku", "Akun", "Kelompok", "Jenis", "Objek", "Rincian", "SubRincian", "Kategori").HasName("DB_AKUN_KATEGORI_PK");
                        j.ToTable("DB_AKUN_KATEGORI");
                        j.IndexerProperty<decimal>("TahunBuku")
                            .HasColumnType("NUMBER")
                            .HasColumnName("TAHUN_BUKU");
                        j.IndexerProperty<string>("Akun")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("AKUN");
                        j.IndexerProperty<string>("Kelompok")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("KELOMPOK");
                        j.IndexerProperty<string>("Jenis")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("JENIS");
                        j.IndexerProperty<string>("Objek")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("OBJEK");
                        j.IndexerProperty<string>("Rincian")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("RINCIAN");
                        j.IndexerProperty<string>("SubRincian")
                            .HasMaxLength(20)
                            .IsUnicode(false)
                            .HasColumnName("SUB_RINCIAN");
                        j.IndexerProperty<decimal>("Kategori")
                            .HasColumnType("NUMBER")
                            .HasColumnName("KATEGORI");
                    });
        });

        modelBuilder.Entity<DbAkunTarget>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Akun, e.Kelompok, e.Jenis, e.Objek, e.Rincian, e.SubRincian }).HasName("DB_AKUN_TARGET_PK");

            entity.Property(e => e.Target).HasDefaultValueSql("0                     ");
        });

        modelBuilder.Entity<DbAkunTargetBulan>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Akun, e.Kelompok, e.Jenis, e.Objek, e.Rincian, e.SubRincian, e.Tgl, e.Bulan }).HasName("DB_AKUN_TARGET_BULAN_PK");

            entity.Property(e => e.Tgl).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Bulan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Target).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.DbAkunTarget).WithMany(p => p.DbAkunTargetBulans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DB_AKUN_TARGET_BULAN_R01");
        });

        modelBuilder.Entity<DbAkunTargetBulanUptb>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Akun, e.Kelompok, e.Jenis, e.Objek, e.Rincian, e.SubRincian, e.Tgl, e.Bulan, e.Uptb }).HasName("DB_AKUN_TARGET_BULAN_UPTB_PK");

            entity.Property(e => e.Tgl).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Bulan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Uptb).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Target).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.DbAkunTargetBulan).WithMany(p => p.DbAkunTargetBulanUptbs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DB_AKUN_TARGET_BULAN_UPTB_R01");
        });

        modelBuilder.Entity<DbMonAbt>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajakKetetapan, e.MasaPajakKetetapan, e.SeqPajakKetetapan }).HasName("DB_MON_ABT_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.NoKetetapan).HasDefaultValueSql("'-' ");
            entity.Property(e => e.PeruntukanId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbMonBphtb>(entity =>
        {
            entity.HasKey(e => new { e.Idsspd, e.Seq }).HasName("DB_MON_BPHTB_PK");

            entity.Property(e => e.Seq).ValueGeneratedOnAdd();
            entity.Property(e => e.RekonBy).IsFixedLength();
        });

        modelBuilder.Entity<DbMonHiburan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajakKetetapan, e.MasaPajakKetetapan, e.SeqPajakKetetapan }).HasName("DB_MON_HIBURAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbMonHotel>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajakKetetapan, e.MasaPajakKetetapan, e.SeqPajakKetetapan }).HasName("DB_MON_HOTEL_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbMonOpsenBbnkb>(entity =>
        {
            entity.HasKey(e => e.IdSspd).HasName("DB_MON_OPSEN_BBNKB_PK");

            entity.Property(e => e.IdAyatPajak).IsFixedLength();
        });

        modelBuilder.Entity<DbMonOpsenPkb>(entity =>
        {
            entity.HasKey(e => e.IdSspd).HasName("DB_MON_OPSEN_PKB_PK");

            entity.Property(e => e.IdAyatPajak).IsFixedLength();
        });

        modelBuilder.Entity<DbMonParkir>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajakKetetapan, e.MasaPajakKetetapan, e.SeqPajakKetetapan }).HasName("DB_MON_PARKIR_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbMonPbb>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajakKetetapan, e.MasaPajakKetetapan, e.SeqPajakKetetapan }).HasName("DB_MON_PBB_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbMonPpj>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajakKetetapan, e.MasaPajakKetetapan, e.SeqPajakKetetapan }).HasName("DB_MON_PPJ_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbMonReklame>(entity =>
        {
            entity.HasKey(e => new { e.NoFormulir, e.Seq }).HasName("DB_MON_REKLAME_PK");

            entity.Property(e => e.Seq).ValueGeneratedOnAdd();
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.KelasJalan).IsFixedLength();
            entity.Property(e => e.KodeJenis).IsFixedLength();
            entity.Property(e => e.KodeObyek).IsFixedLength();
            entity.Property(e => e.NoKetetapan).HasDefaultValueSql("'-' ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbMonReklameEmail>(entity =>
        {
            entity.HasKey(e => new { e.NoFormulir, e.TglKirimEmail }).HasName("DB_MON_REKLAME_EMAIL_PK");
        });

        modelBuilder.Entity<DbMonReklameSurat>(entity =>
        {
            entity.HasKey(e => new { e.Agenda, e.Bidang, e.Klasifikasi, e.KodeDokumen, e.Pajak, e.TahunSurat }).HasName("DB_MON_REKLAME_SURAT_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.Status).HasDefaultValueSql("1 ");
        });

        modelBuilder.Entity<DbMonReklameSuratTegur>(entity =>
        {
            entity.HasKey(e => new { e.Klasifikasi, e.TahunSurat, e.Pajak, e.KodeDokumen, e.Bidang, e.Agenda }).HasName("DB_MON_REKLAME_SURAT_TEGUR_PK");
        });

        modelBuilder.Entity<DbMonReklameSuratTegurDok>(entity =>
        {
            entity.HasKey(e => new { e.Klasifikasi, e.TahunSurat, e.Pajak, e.KodeDokumen, e.Bidang, e.Agenda }).HasName("REKLAME_TEGUR_DOK_PK");
        });

        modelBuilder.Entity<DbMonReklameUpaya>(entity =>
        {
            entity.HasKey(e => new { e.NoFormulir, e.TglUpaya, e.Seq }).HasName("DB_MON_REKLAME_UPAYA_PK");
        });

        modelBuilder.Entity<DbMonReklameUpayaDok>(entity =>
        {
            entity.HasKey(e => new { e.NoformS, e.TglUpaya, e.Seq }).HasName("PK_DETAIL_UPLOAD_REKLAME");

            entity.HasOne(d => d.DbMonReklameUpaya).WithOne(p => p.DbMonReklameUpayaDok)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DB_MON_UPAYA_FK");
        });

        modelBuilder.Entity<DbMonResto>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajakKetetapan, e.MasaPajakKetetapan, e.SeqPajakKetetapan }).HasName("DB_MON_RESTO_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.UpdDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<DbOpAbt>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunBuku }).HasName("DB_OP_ABT_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'JOB'                 ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.IsMeteranAir).HasDefaultValueSql("1                     ");
            entity.Property(e => e.IsTutup).HasDefaultValueSql("1                     ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.PeruntukanId).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<DbOpHiburan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunBuku }).HasName("DB_OP_HIBURAN_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'JOB'                 ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("'0'                   ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<DbOpHotel>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunBuku }).HasName("DB_OP_HOTEL_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'JOB'                 ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("'0'                   ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<DbOpListrik>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunBuku }).HasName("DB_OP_LISTRIK_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'JOB'                 ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
        });

        modelBuilder.Entity<DbOpParkir>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunBuku }).HasName("DB_OP_PARKIR_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'JOB'                 ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<DbOpPbb>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("DB_OP_PBB_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<DbOpReklame>(entity =>
        {
            entity.HasKey(e => new { e.NoFormulir, e.Seq }).HasName("DB_OP_REKLAME_PK");

            entity.Property(e => e.Seq).ValueGeneratedOnAdd();
            entity.Property(e => e.KelasJalan).IsFixedLength();
            entity.Property(e => e.KodeJenis).IsFixedLength();
            entity.Property(e => e.KodeObyek).IsFixedLength();
        });

        modelBuilder.Entity<DbOpResto>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunBuku }).HasName("DB_OP_RESTO_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'JOB' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.JumlahKursi).HasDefaultValueSql("0                     ");
            entity.Property(e => e.JumlahMeja).HasDefaultValueSql("0                     ");
            entity.Property(e => e.KapasitasRuanganOrang).HasDefaultValueSql("0                     ");
            entity.Property(e => e.KategoriId).HasDefaultValueSql("1                     ");
            entity.Property(e => e.MaksimalProduksiPorsiHari).HasDefaultValueSql("0                     ");
            entity.Property(e => e.RataTerjualPorsiHari).HasDefaultValueSql("0                     ");
        });

        modelBuilder.Entity<DbRekamParkir>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tanggal, e.Seq }).HasName("PK_REKAM_PARKIR");
        });

        modelBuilder.Entity<DbRekamRestoran>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tanggal, e.Seq }).HasName("SYS_C0032871");
        });

        modelBuilder.Entity<MFasilita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_FASILITAS_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<MJenisKendaraan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_JENIS_KENDARAAN_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.TarifPerwali).HasDefaultValueSql("0                     ");
        });

        modelBuilder.Entity<MKategoriPajak>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_KATEGORI_PAJAK_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.ModeKapTar).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.Pajak).WithMany(p => p.MKategoriPajaks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_KATEGORI_PAJAK_R01");
        });

        modelBuilder.Entity<MKategoriUpaya>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_KATEGORI_UPAYA_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<MPajak>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_PAJAK_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<MTindakanReklame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_TINDAKAN_REKLAME_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE\n");

            entity.HasOne(d => d.IdUpayaNavigation).WithMany(p => p.MTindakanReklames)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_TINDAKAN_REKLAME_R01");
        });

        modelBuilder.Entity<MTipekamarhotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_TIPEKAMARHOTEL_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<MUpayaReklame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPAYA_REKLAME_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE\n");
        });

        modelBuilder.Entity<MUserLogin>(entity =>
        {
            entity.Property(e => e.InsertBy).HasDefaultValueSql("'MASTER KEY'");
            entity.Property(e => e.InsertDate).HasDefaultValueSql("SYSDATE");
        });

        modelBuilder.Entity<MWilayah>(entity =>
        {
            entity.HasKey(e => new { e.KdKecamatan, e.KdKelurahan }).HasName("M_WILAYAH_PK");
        });

        modelBuilder.Entity<MvReklameSum>(entity =>
        {
            entity.ToView("MV_REKLAME_SUM");
        });

        modelBuilder.Entity<MvReklameSummary>(entity =>
        {
            entity.ToView("MV_REKLAME_SUMMARY");
        });

        modelBuilder.Entity<MvSeriesPendapatan>(entity =>
        {
            entity.ToView("MV_SERIES_PENDAPATAN");
        });

        modelBuilder.Entity<MvSeriesTargetP>(entity =>
        {
            entity.ToView("MV_SERIES_TARGET_P");
        });

        modelBuilder.Entity<Npwpd>(entity =>
        {
            entity.HasKey(e => e.NpwpdNo).HasName("NPWPD_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.IsValid).HasDefaultValueSql("0 ");
            entity.Property(e => e.RefWf).HasDefaultValueSql("0");
            entity.Property(e => e.Status).HasDefaultValueSql("1 ");
        });

        modelBuilder.Entity<Op>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");

            entity.HasOne(d => d.Pajak).WithMany(p => p.Ops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_R01");

            entity.HasOne(d => d.Kd).WithMany(p => p.Ops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_R02");
        });

        modelBuilder.Entity<OpAbt>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_ABT_PK");

            entity.Property(e => e.IsMeteranAir).HasDefaultValueSql("1                     ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Kategori).HasDefaultValueSql("1                     ");
            entity.Property(e => e.Peruntukan).HasDefaultValueSql("1                     ");

            entity.HasOne(d => d.KategoriNavigation).WithMany(p => p.OpAbts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_ABT_R02");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpAbt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_ABT_R01");
        });

        modelBuilder.Entity<OpAbtJadwal>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdHari }).HasName("OP_ABT_JADWAL_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpAbtJadwals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_ABT_JADWAL_R01");
        });

        modelBuilder.Entity<OpAbtKapasita>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Id }).HasName("OP_ABT_KAPASITAS_PK");

            entity.Property(e => e.DayaPompa).HasDefaultValueSql("1                     ");
            entity.Property(e => e.Jumlah).HasDefaultValueSql("1                     ");
            entity.Property(e => e.MerkJenisPompa).HasDefaultValueSql("1                     ");
            entity.Property(e => e.RataPenggunaanMenit).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpAbtKapasita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_ABT_KAPASITAS_R01");
        });

        modelBuilder.Entity<OpAbtKetetapan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq }).HasName("OP_ABT_KETETAPAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsLunas).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Peruntukan).HasDefaultValueSql("1                     ");

            entity.HasOne(d => d.Kategori).WithMany(p => p.OpAbtKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_ABT_KETETAPAN_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpAbtKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_ABT_KETETAPAN_R01");
        });

        modelBuilder.Entity<OpAbtKetetapanSspd>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq, e.IdSspd }).HasName("OP_ABT_KETETAPAN_SSPD_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.OpAbtKetetapan).WithMany(p => p.OpAbtKetetapanSspds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_ABT_KETETAPAN_SSPD_R01");
        });

        modelBuilder.Entity<OpHiburan>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_HIBURAN_PK");

            entity.Property(e => e.AlatPengawasan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("'0'                   ");
            entity.Property(e => e.Kategori).HasDefaultValueSql("0                     ");
            entity.Property(e => e.MetodePembayaran).HasDefaultValueSql("3                     ");
            entity.Property(e => e.MetodePenjualan).HasDefaultValueSql("3                     ");

            entity.HasOne(d => d.KategoriNavigation).WithMany(p => p.OpHiburans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_R02");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpHiburan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_R01");
        });

        modelBuilder.Entity<OpHiburanJadwal>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdHari }).HasName("OP_HIBURAN_JADWAL_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHiburanJadwals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_JADWAL_R01");
        });

        modelBuilder.Entity<OpHiburanKetetapan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq }).HasName("OP_HIBURAN_KETETAPAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsLunas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.Kategori).WithMany(p => p.OpHiburanKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_KETETAPAN_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHiburanKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_KETETAPAN_R01");
        });

        modelBuilder.Entity<OpHiburanKetetapanSspd>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq, e.IdSspd }).HasName("OP_HIBURAN_KETETAPAN_SSPD_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.OpHiburanKetetapan).WithMany(p => p.OpHiburanKetetapanSspds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_KETETAPAN_SSPD_R01");
        });

        modelBuilder.Entity<OpHiburanKtFilm>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Seq }).HasName("OP_HIBURAN_KT_FILM_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHiburanKtFilms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_KT_FILM_R01");
        });

        modelBuilder.Entity<OpHiburanKtGym>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_HIBURAN_KT_GYM_PK");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpHiburanKtGym)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_KT_GYM_R01");
        });

        modelBuilder.Entity<OpHiburanKtOther>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Seq }).HasName("OP_HIBURAN_KT_OTHER_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHiburanKtOthers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HIBURAN_KT_OTHER_R01");
        });

        modelBuilder.Entity<OpHotel>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_HOTEL_PK");

            entity.Property(e => e.AlatPengawasan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.MetodePembayaran).HasDefaultValueSql("1                     ");
            entity.Property(e => e.MetodePenjualan).HasDefaultValueSql("1                     ");

            entity.HasOne(d => d.KategoriNavigation).WithMany(p => p.OpHotels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_R02");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpHotel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_R01");
        });

        modelBuilder.Entity<OpHotelBanquet>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Seq }).HasName("OP_HOTEL_BANQUET_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHotelBanquets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_BANQUET_R01");
        });

        modelBuilder.Entity<OpHotelFasilita>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdFasilitas }).HasName("OP_HOTEL_FASILITAS_PK");

            entity.Property(e => e.Kapasitas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.IdFasilitasNavigation).WithMany(p => p.OpHotelFasilita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_FASILITAS_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHotelFasilita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_FASILITAS_R01");
        });

        modelBuilder.Entity<OpHotelKamar>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdTipe }).HasName("OP_HOTEL_KAMAR_PK");

            entity.HasOne(d => d.IdTipeNavigation).WithMany(p => p.OpHotelKamars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_KAMAR_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHotelKamars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_KAMAR_R01");
        });

        modelBuilder.Entity<OpHotelKetetapan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq }).HasName("OP_HOTEL_KETETAPAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsLunas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.Kategori).WithMany(p => p.OpHotelKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_KETETAPAN_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpHotelKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_KETETAPAN_R01");
        });

        modelBuilder.Entity<OpHotelKetetapanSspd>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq, e.IdSspd }).HasName("OP_HOTEL_KETETAPAN_SSPD_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.OpHotelKetetapan).WithMany(p => p.OpHotelKetetapanSspds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_KETETAPAN_SSPD_R01");
        });

        modelBuilder.Entity<OpHotelKost>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_HOTEL_KOST_PK");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpHotelKost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_HOTEL_KOST_R01");
        });

        modelBuilder.Entity<OpListrik>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_LISTRIK_PK");

            entity.Property(e => e.AlatPengawasan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpListrik)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_LISTRIK_R01");
        });

        modelBuilder.Entity<OpListrikJadwal>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdHari }).HasName("OP_LISTRIK_JADWAL_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpListrikJadwals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_LISTRIK_JADWAL_R01");
        });

        modelBuilder.Entity<OpListrikKetetapan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq }).HasName("OP_LISTRIK_KETETAPAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsLunas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.Kategori).WithMany(p => p.OpListrikKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_LISTRIK_KETETAPAN_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpListrikKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_LISTRIK_KETETAPAN_R01");
        });

        modelBuilder.Entity<OpListrikKetetapanSspd>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq, e.IdSspd }).HasName("OP_LISTRIK_KETETAPAN_SSPD_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.OpListrikKetetapan).WithMany(p => p.OpListrikKetetapanSspds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_LISTRIK_KETETAPAN_SSPD_R01");
        });

        modelBuilder.Entity<OpListrikSumberLain>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_LISTRIK_SUMBER_LAIN_PK");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpListrikSumberLain)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_LISTRIK_SUMBER_LAIN_R01");
        });

        modelBuilder.Entity<OpListrikSumberSendiri>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Seq }).HasName("OP_LISTRIK_SUMBER_SENDIRI_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpListrikSumberSendiris)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_LISTRIK_SUMBER_SENDIRI_R01");
        });

        modelBuilder.Entity<OpParkir>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_PARKIR_PK");

            entity.Property(e => e.AlatPengawasan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Dikelola).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.Kategori).HasDefaultValueSql("1                     ");
            entity.Property(e => e.PungutTarif).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.KategoriNavigation).WithMany(p => p.OpParkirs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PARKIR_R02");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpParkir)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PARKIR_R01");
        });

        modelBuilder.Entity<OpParkirJadwal>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdHari }).HasName("OP_PARKIR_JADWAL_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpParkirJadwals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PARKIR_JADWAL_R01");
        });

        modelBuilder.Entity<OpParkirKetetapan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq }).HasName("OP_PARKIR_KETETAPAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsLunas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpParkirKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PARKIR_KETETAPAN_R01");
        });

        modelBuilder.Entity<OpParkirKetetapanSspd>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq, e.IdSspd }).HasName("OP_PARKIR_KETETAPAN_SSPD_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.OpParkirKetetapan).WithMany(p => p.OpParkirKetetapanSspds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PARKIR_KETETAPAN_SSPD_R01");
        });

        modelBuilder.Entity<OpParkirTarif>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdJenisKendaraan }).HasName("OP_PARKIR_TARIF_PK");

            entity.Property(e => e.KapasitasMaks).HasDefaultValueSql("0                     ");
            entity.Property(e => e.TarifAwal).HasDefaultValueSql("0                     ");
            entity.Property(e => e.TarifMember).HasDefaultValueSql("0                     ");
            entity.Property(e => e.TarifProgresif).HasDefaultValueSql("0                     ");
            entity.Property(e => e.TarifValet).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.IdJenisKendaraanNavigation).WithMany(p => p.OpParkirTarifs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PARKIR_TARIF_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpParkirTarifs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PARKIR_TARIF_R01");
        });

        modelBuilder.Entity<OpPbb>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_PBB_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");

            entity.HasOne(d => d.KategoriNavigation).WithMany(p => p.OpPbbs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PBB_R01");

            entity.HasOne(d => d.AlamatKd).WithMany(p => p.OpPbbs).HasConstraintName("OP_PBB_R02");
        });

        modelBuilder.Entity<OpPbbKetetapan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq }).HasName("OP_PBB_KETETAPAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsLunas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.Kategori).WithMany(p => p.OpPbbKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PBB_KETETAPAN_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpPbbKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PBB_KETETAPAN_R01");
        });

        modelBuilder.Entity<OpPbbKetetapanSspd>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq, e.IdSspd }).HasName("OP_PBB_KETETAPAN_SSPD_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.OpPbbKetetapan).WithMany(p => p.OpPbbKetetapanSspds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_PBB_KETETAPAN_SSPD_R01");
        });

        modelBuilder.Entity<OpResto>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("OP_RESTO_PK");

            entity.Property(e => e.AlatPengawasan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.JumlahKaryawan).HasDefaultValueSql("0                     ");
            entity.Property(e => e.JumlahKursi).HasDefaultValueSql("0                     ");
            entity.Property(e => e.JumlahMeja).HasDefaultValueSql("0                     ");
            entity.Property(e => e.KapasitasRuanganOrang).HasDefaultValueSql("0                     ");
            entity.Property(e => e.MaksimalProduksiPorsiHari).HasDefaultValueSql("0                     ");
            entity.Property(e => e.MetodePembayaran).HasDefaultValueSql("1                     ");
            entity.Property(e => e.MetodePenjualan).HasDefaultValueSql("1                     ");
            entity.Property(e => e.RataTerjualPorsiHari).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.KategoriNavigation).WithMany(p => p.OpRestos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_R02");

            entity.HasOne(d => d.NopNavigation).WithOne(p => p.OpResto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_R01");
        });

        modelBuilder.Entity<OpRestoFasilita>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdFasilitas }).HasName("OP_RESTO_FASILITAS_PK");

            entity.Property(e => e.Kapasitas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.IdFasilitasNavigation).WithMany(p => p.OpRestoFasilita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_FASILITAS_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpRestoFasilita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_FASILITAS_R01");
        });

        modelBuilder.Entity<OpRestoJadwal>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdHari }).HasName("OP_RESTO_JADWAL_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpRestoJadwals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_JADWAL_R01");
        });

        modelBuilder.Entity<OpRestoKetetapan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq }).HasName("OP_RESTO_KETETAPAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
            entity.Property(e => e.IsLunas).HasDefaultValueSql("0                     ");

            entity.HasOne(d => d.Kategori).WithMany(p => p.OpRestoKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_KETETAPAN_R02");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpRestoKetetapans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_KETETAPAN_R01");
        });

        modelBuilder.Entity<OpRestoKetetapanSspd>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.Tahun, e.Masapajak, e.Seq, e.IdSspd }).HasName("OP_RESTO_KETETAPAN_SSPD_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.OpRestoKetetapan).WithMany(p => p.OpRestoKetetapanSspds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_KETETAPAN_SSPD_R01");
        });

        modelBuilder.Entity<OpRestoMenu>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.IdSeqMenu }).HasName("OP_RESTO_MENU_PK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.OpRestoMenus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OP_RESTO_MENU_R01");
        });

        modelBuilder.Entity<PSb>(entity =>
        {
            entity.HasKey(e => e.IdOp).HasName("P_SB_PK");

            entity.Property(e => e.Status).IsFixedLength();
        });

        modelBuilder.Entity<PT>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("P_TS_PK");
        });

        modelBuilder.Entity<PTb>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("P_TB_PK");
        });

        modelBuilder.Entity<PenHimbauanBayar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PEN_HIMBAUAN_BAYAR_PK");
        });

        modelBuilder.Entity<PenTeguranBayar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PEN_TEGURAN_BAYAR_PK");
        });

        modelBuilder.Entity<PotensiCtrlAirTanah>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.KdPajak }).HasName("PK_POTENSI_CTRL_AT");

            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<PotensiCtrlHiburan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.KdPajak }).HasName("PK_HIBURAN");

            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.Status).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<PotensiCtrlHotel>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.KdPajak }).HasName("PK_PAJAK_HOTEL");

            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.Status).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<PotensiCtrlParkir>(entity =>
        {
            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.Status).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<PotensiCtrlPpj>(entity =>
        {
            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<PotensiCtrlReklame>(entity =>
        {
            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<PotensiCtrlRestoran>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.KdPajak }).HasName("PK_RESTORAN");

            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.Status).HasDefaultValueSql("1                     ");
        });

        modelBuilder.Entity<PotensiCtrlTarget>(entity =>
        {
            entity.Property(e => e.Nop).IsFixedLength();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<SetLastRun>(entity =>
        {
            entity.HasKey(e => e.Job).HasName("LAST_RUN_PK");
        });

        modelBuilder.Entity<SetYearJobScan>(entity =>
        {
            entity.HasKey(e => e.IdPajak).HasName("SET_YEAR_JOB_SCAN_PK");

            entity.Property(e => e.IdPajak).ValueGeneratedNever();

            entity.HasOne(d => d.IdPajakNavigation).WithOne(p => p.SetYearJobScan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SET_YEAR_JOB_SCAN_R01");
        });

        modelBuilder.Entity<THimbauanSptpd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T_HIMBAUAN_SPTPD_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<TMutasiPiutang>(entity =>
        {
            entity.HasKey(e => new { e.Mutasi, e.TahunBuku }).HasName("PK_MUTASI_PIUTANG");
        });

        modelBuilder.Entity<TPemeriksaan>(entity =>
        {
            entity.HasKey(e => new { e.Nop, e.TahunPajak, e.MasaPajak, e.Seq }).HasName("T_PEMERIKSAAN_PK");
        });

        modelBuilder.Entity<TPendapatanDaerah>(entity =>
        {
            entity.ToView("T_PENDAPATAN_DAERAH");
        });

        modelBuilder.Entity<TPenungguanSptpd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T_PENUNGGUAN_SPTPD_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<TPenungguanSptpdMamin>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Nop, e.TahunPajak, e.MasaPajak, e.TglPenungguan, e.JamMulai }).HasName("T_PENUNGGUAN_SPTPD_MAMIN_PK");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.TPenungguanSptpdMamins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PENUNGGUAN_SPTPD_MAMIN_R01");
        });

        modelBuilder.Entity<TPenungguanSptpdParkir>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Nop, e.TahunPajak, e.MasaPajak, e.TglPenungguan, e.JamMulai }).HasName("T_PENUNGGUAN_SPTPD_PARKIR_PK");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.TPenungguanSptpdParkirs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PENUNGGUAN_SPTPD_PARKIR_R01");
        });

        modelBuilder.Entity<TPiutangAbt>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_ABT_PK");
        });

        modelBuilder.Entity<TPiutangBphtb>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_BPHTB_PK");
        });

        modelBuilder.Entity<TPiutangHiburan>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_HIBURAN_PK");
        });

        modelBuilder.Entity<TPiutangHotel>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_HOTEL_PK");
        });

        modelBuilder.Entity<TPiutangListrik>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_LISTRIK_PK");
        });

        modelBuilder.Entity<TPiutangOpsenBbnkb>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_OPSEN_BBNKB_PK");
        });

        modelBuilder.Entity<TPiutangOpsenPkb>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_OPSEN_PKB_PK");
        });

        modelBuilder.Entity<TPiutangParkir>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_PARKIR_PK");
        });

        modelBuilder.Entity<TPiutangPbb>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_PBB_PK");
        });

        modelBuilder.Entity<TPiutangReklame>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_REKLAME_PK");
        });

        modelBuilder.Entity<TPiutangResto>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Nop, e.MasaPajak, e.TahunPajak }).HasName("T_PIUTANG_RESTO_PK");
        });

        modelBuilder.Entity<TSeriesPendapatan>(entity =>
        {
            entity.Property(e => e.InsertDate).HasDefaultValueSql("SYSDATE");
        });

        modelBuilder.Entity<TSeriesTargetP>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.KelompokRek, e.JenisRek, e.ObyekRek, e.RincianRek, e.SubrincianRek }).HasName("PK_PAD_TARGET_REKLAME");
        });

        modelBuilder.Entity<TSuratReklameTeguranFile>(entity =>
        {
            entity.HasKey(e => new { e.Klasifikasi, e.TahunSurat, e.Pajak, e.KodeDokumen, e.Bidang, e.Agenda }).HasName("PK_TSURATREKLAMETEGURANFILE");
        });

        modelBuilder.Entity<TTeguranSptpd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T_TEGURAN_SPTPD_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });
        modelBuilder.HasSequence("SEQ_DB_MON_BPHTB");
        modelBuilder.HasSequence("SEQ_DB_MON_REKLAME");
        modelBuilder.HasSequence("SEQ_DB_OP_REKLAME");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
