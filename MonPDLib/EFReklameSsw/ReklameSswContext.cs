using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklameSsw;

public partial class ReklameSswContext : DbContext
{
    public ReklameSswContext()
    {
    }

    public ReklameSswContext(DbContextOptions<ReklameSswContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<ChatUser> ChatUsers { get; set; }

    public virtual DbSet<MGambarDepan> MGambarDepans { get; set; }

    public virtual DbSet<MJabatan> MJabatans { get; set; }

    public virtual DbSet<MJenisPerizinanRek> MJenisPerizinanReks { get; set; }

    public virtual DbSet<MKecamatanKelurahan> MKecamatanKelurahans { get; set; }

    public virtual DbSet<MPegawai> MPegawais { get; set; }

    public virtual DbSet<MPegawaiFoto> MPegawaiFotos { get; set; }

    public virtual DbSet<MUcUserLogin> MUcUserLogins { get; set; }

    public virtual DbSet<MUptb> MUptbs { get; set; }

    public virtual DbSet<MUptbEvent> MUptbEvents { get; set; }

    public virtual DbSet<MUptbEventGambar> MUptbEventGambars { get; set; }

    public virtual DbSet<MUptbFungsi> MUptbFungsis { get; set; }

    public virtual DbSet<MUptbGalery> MUptbGaleries { get; set; }

    public virtual DbSet<MUptbMisi> MUptbMisis { get; set; }

    public virtual DbSet<MUptbMitra> MUptbMitras { get; set; }

    public virtual DbSet<MUptbPeran> MUptbPerans { get; set; }

    public virtual DbSet<MUptbStruktur> MUptbStrukturs { get; set; }

    public virtual DbSet<MUptbVisi> MUptbVisis { get; set; }

    public virtual DbSet<PbbMUcRole> PbbMUcRoles { get; set; }

    public virtual DbSet<PbbMUcUserLogin> PbbMUcUserLogins { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<TBeritaGambar> TBeritaGambars { get; set; }

    public virtual DbSet<TBeritaIsi> TBeritaIsis { get; set; }

    public virtual DbSet<TBeritum> TBerita { get; set; }

    public virtual DbSet<TCheckSspd> TCheckSspds { get; set; }

    public virtual DbSet<TJadwalMobling> TJadwalMoblings { get; set; }

    public virtual DbSet<TPerizinanReklame> TPerizinanReklames { get; set; }

    public virtual DbSet<TPerizinanReklameBatal> TPerizinanReklameBatals { get; set; }

    public virtual DbSet<TPerizinanReklameDet> TPerizinanReklameDets { get; set; }

    public virtual DbSet<TPerizinanReklameDetFoto> TPerizinanReklameDetFotos { get; set; }

    public virtual DbSet<TPersetujuanTekni> TPersetujuanTeknis { get; set; }

    public virtual DbSet<TReklameSkpd> TReklameSkpds { get; set; }

    public virtual DbSet<TReklameSspd> TReklameSspds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=cms;Password=cms;Data Source=10.21.39.188:1521/DEV;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("CMS");

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => new { e.Nik, e.Seq }).HasName("CHAT_MESSAGE_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.NikNavigation).WithMany(p => p.ChatMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CHAT_MESSAGE_R02");

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.ChatMessages).HasConstraintName("CHAT_MESSAGE_R01");
        });

        modelBuilder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => e.Nik).HasName("CHAT_USERS_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.LastAct).HasDefaultValueSql("NULL");
            entity.Property(e => e.ResetKey).HasDefaultValueSql("NULL");
        });

        modelBuilder.Entity<MGambarDepan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_GAMBAR_DEPAN_PK");
        });

        modelBuilder.Entity<MJabatan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_JABATAN_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<MJenisPerizinanRek>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_JENIS_PERIZINAN_REK_PK");
        });

        modelBuilder.Entity<MKecamatanKelurahan>(entity =>
        {
            entity.HasKey(e => new { e.Uptb, e.KdCamat, e.KdLurah }).HasName("M_KECAMATAN_KELURAHAN_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.NamaKelurahan).HasDefaultValueSql("NULL");
        });

        modelBuilder.Entity<MPegawai>(entity =>
        {
            entity.HasKey(e => e.Nip).HasName("M_PEGAWAI_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");

            entity.HasOne(d => d.BidangNavigation).WithMany(p => p.MPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_PEGAWAI_R01");

            entity.HasOne(d => d.JabatanNavigation).WithMany(p => p.MPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_PEGAWAI_R02");
        });

        modelBuilder.Entity<MPegawaiFoto>(entity =>
        {
            entity.HasKey(e => e.Nip).HasName("M_PEGAWAI_FOTO_PK");

            entity.HasOne(d => d.NipNavigation).WithOne(p => p.MPegawaiFoto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_PEGAWAI_FOTO_R01");
        });

        modelBuilder.Entity<MUcUserLogin>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("M_UC_USER_LOGIN_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
            entity.Property(e => e.LastAct).HasDefaultValueSql("NULL");
            entity.Property(e => e.ResetKey).HasDefaultValueSql("NULL");
        });

        modelBuilder.Entity<MUptb>(entity =>
        {
            entity.HasKey(e => e.Uptb).HasName("M_UPTB_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<MUptbEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPTB_EVENT_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.UptbNavigation).WithMany(p => p.MUptbEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_EVENT_R01");
        });

        modelBuilder.Entity<MUptbEventGambar>(entity =>
        {
            entity.HasKey(e => e.IdEvent).HasName("M_UPTB_EVENT_GAMBAR_PK");

            entity.HasOne(d => d.IdEventNavigation).WithOne(p => p.MUptbEventGambar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_EVENT_GAMBAR_R01");
        });

        modelBuilder.Entity<MUptbFungsi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPTB_FUNGSI_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.UptbNavigation).WithMany(p => p.MUptbFungsis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_FUNGSI_R01");
        });

        modelBuilder.Entity<MUptbGalery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPTB_GALERY_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.UptbNavigation).WithMany(p => p.MUptbGaleries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_GALERY_R01");
        });

        modelBuilder.Entity<MUptbMisi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPTB_MISI_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.UptbNavigation).WithMany(p => p.MUptbMisis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_MISI_R01");
        });

        modelBuilder.Entity<MUptbMitra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPTB_MITRA_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.UptbNavigation).WithMany(p => p.MUptbMitras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_MITRA_R01");
        });

        modelBuilder.Entity<MUptbPeran>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPTB_PERAN_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.UptbNavigation).WithMany(p => p.MUptbPerans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_PERAN_R01");
        });

        modelBuilder.Entity<MUptbStruktur>(entity =>
        {
            entity.HasKey(e => e.Uptb).HasName("M_UPTB_STRUKTUR_PK");

            entity.HasOne(d => d.UptbNavigation).WithOne(p => p.MUptbStruktur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_STRUKTUR_R01");
        });

        modelBuilder.Entity<MUptbVisi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_UPTB_VISI_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.UptbNavigation).WithMany(p => p.MUptbVisis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_UPTB_VISI_R01");
        });

        modelBuilder.Entity<PbbMUcRole>(entity =>
        {
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<PbbMUcUserLogin>(entity =>
        {
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Nama).HasName("SETTING_PK");
        });

        modelBuilder.Entity<TBeritaGambar>(entity =>
        {
            entity.HasKey(e => e.IdBerita).HasName("T_BERITA_GAMBAR_PK");

            entity.HasOne(d => d.IdBeritaNavigation).WithOne(p => p.TBeritaGambar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_BERITA_GAMBAR_R01");
        });

        modelBuilder.Entity<TBeritaIsi>(entity =>
        {
            entity.HasKey(e => e.IdBerita).HasName("T_BERITA_ISI_PK");

            entity.HasOne(d => d.IdBeritaNavigation).WithOne(p => p.TBeritaIsi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_BERITA_ISI_R01");
        });

        modelBuilder.Entity<TBeritum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T_BERITA_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("'1'                   ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<TJadwalMobling>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("T_JADWAL_MOBLING_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");

            entity.HasOne(d => d.MKecamatanKelurahan).WithMany(p => p.TJadwalMoblings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_JADWAL_MOBLING_R01");
        });

        modelBuilder.Entity<TPerizinanReklame>(entity =>
        {
            entity.HasKey(e => new { e.NomorDaftar, e.Tahun }).HasName("T_PERIZINAN_REKLAME_PK");

            entity.Property(e => e.StatusKirim).HasDefaultValueSql("0");

            entity.HasOne(d => d.KdPerizinanNavigation).WithMany(p => p.TPerizinanReklames).HasConstraintName("T_JENIS_PERIZINAN_REK_FK");
        });

        modelBuilder.Entity<TPerizinanReklameBatal>(entity =>
        {
            entity.HasKey(e => new { e.NomorDaftar, e.Tahun, e.Seq }).HasName("T_PERIZINAN_REKLAME_BATAL_PK");

            entity.Property(e => e.TglBatal).HasDefaultValueSql("sysdate ");

            entity.HasOne(d => d.TPerizinanReklame).WithMany(p => p.TPerizinanReklameBatals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PERIZINAN_REKLAME_BATAL_FK");
        });

        modelBuilder.Entity<TPerizinanReklameDet>(entity =>
        {
            entity.HasKey(e => new { e.NomorDaftar, e.Seq, e.Tahun }).HasName("T_PERIZINAN_REKLAME_DET_PK");

            entity.Property(e => e.NilaiVa).HasDefaultValueSql("0 ");

            entity.HasOne(d => d.TPerizinanReklame).WithMany(p => p.TPerizinanReklameDets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PERIZINAN_REKLAME_DET__FK");
        });

        modelBuilder.Entity<TPerizinanReklameDetFoto>(entity =>
        {
            entity.HasKey(e => new { e.NomorDaftar, e.Seq, e.FotoId, e.Tahun }).HasName("T_PERIZINAN_DET_FOTO_PK");

            entity.HasOne(d => d.TPerizinanReklameDet).WithMany(p => p.TPerizinanReklameDetFotos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PERIZINAN_REKLAMEDETFOTO_FK");
        });

        modelBuilder.Entity<TPersetujuanTekni>(entity =>
        {
            entity.HasKey(e => e.NoKetetapan).HasName("T_PERSETUJUAN_TEKNIS_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'ADMIN'               ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE               ");
        });

        modelBuilder.Entity<TReklameSkpd>(entity =>
        {
            entity.HasKey(e => e.NoKetetapan).HasName("T_REKLAME_SKPD_PK");

            entity.Property(e => e.Status).HasDefaultValueSql("1");
        });

        modelBuilder.Entity<TReklameSspd>(entity =>
        {
            entity.HasKey(e => e.NoKetetapan).HasName("T_REKLAME_SSPD_PK");

            entity.Property(e => e.Status).HasDefaultValueSql("1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
