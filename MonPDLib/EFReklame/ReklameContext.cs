using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFReklame;

public partial class ReklameContext : DbContext
{
    public ReklameContext()
    {
    }

    public ReklameContext(DbContextOptions<ReklameContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MJalan> MJalans { get; set; }

    public virtual DbSet<MJenisReklame> MJenisReklames { get; set; }

    public virtual DbSet<MNilaiSatuanStrategi> MNilaiSatuanStrategis { get; set; }

    public virtual DbSet<MNilaiStrategisDef> MNilaiStrategisDefs { get; set; }

    public virtual DbSet<MNilaiStrategisLokasi> MNilaiStrategisLokasis { get; set; }

    public virtual DbSet<MNilaiStrategisSpandang> MNilaiStrategisSpandangs { get; set; }

    public virtual DbSet<MNilaiStrategisTinggi> MNilaiStrategisTinggis { get; set; }

    public virtual DbSet<MNsrLua> MNsrLuas { get; set; }

    public virtual DbSet<MNsrTinggi> MNsrTinggis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=reklame;Password=Reklame@2025;Data Source=10.21.39.80:1521/DEVDB;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("REKLAME");

        modelBuilder.Entity<MJalan>(entity =>
        {
            entity.Property(e => e.IdJalan).ValueGeneratedNever();
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.KoridorJalan).HasDefaultValueSql("0 ");
            entity.Property(e => e.KoridorNilai).HasDefaultValueSql("0 ");
        });

        modelBuilder.Entity<MJenisReklame>(entity =>
        {
            entity.HasKey(e => e.IdJenisReklame).HasName("SYS_C0034682");

            entity.Property(e => e.IdJenisReklame).ValueGeneratedNever();
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'\r\n");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.IsBerjalan).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<MNilaiSatuanStrategi>(entity =>
        {
            entity.HasKey(e => new { e.IdJenisReklame, e.MinLuas, e.MaxLuas, e.TglAwalBerlaku, e.Kawasan }).HasName("M_NILAI_SATUAN_STRATEGIS_PK");

            entity.Property(e => e.Kawasan).HasDefaultValueSql("0 ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.MNilaiSatuanStrategis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MNSR_JENIS_STGRS");
        });

        modelBuilder.Entity<MNilaiStrategisDef>(entity =>
        {
            entity.HasKey(e => new { e.IdJenisReklame, e.TglAwalBerlaku }).HasName("PK_M_STGRS_DEF");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.MNilaiStrategisDefs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_STRGS_DEF");
        });

        modelBuilder.Entity<MNilaiStrategisLokasi>(entity =>
        {
            entity.HasKey(e => new { e.KelasJalan, e.TglAwalBerlaku }).HasName("PK_M_STGRS_LOKASI");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.IsDlmRuang).HasDefaultValueSql("0 ");
        });

        modelBuilder.Entity<MNilaiStrategisSpandang>(entity =>
        {
            entity.HasKey(e => new { e.SudutPandang, e.TglAwalBerlaku }).HasName("PK_M_STGRS_SPNDNG");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.IsDlmRuang).HasDefaultValueSql("0 ");
        });

        modelBuilder.Entity<MNilaiStrategisTinggi>(entity =>
        {
            entity.HasKey(e => new { e.MinKetinggian, e.MaxKetinggian, e.TglAwalBerlaku }).HasName("PK_M_STGRS_TINGGI");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
        });

        modelBuilder.Entity<MNsrLua>(entity =>
        {
            entity.HasKey(e => new { e.IdJenisReklame, e.MinLuas, e.MaxLuas, e.TglAwalBerlaku }).HasName("M_NSR_LUAS_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.MNsrLuas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MNSR_JENIS_LUAS");
        });

        modelBuilder.Entity<MNsrTinggi>(entity =>
        {
            entity.HasKey(e => new { e.IdJenisReklame, e.NilaiKetinggian, e.TglAwalBerlaku }).HasName("M_NSR_TINGGI_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.MNsrTinggis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MNSR_JENIS_TGG");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
