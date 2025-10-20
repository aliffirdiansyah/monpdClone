using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPenyelia;

public partial class PenyeliaContext : DbContext
{
    public PenyeliaContext()
    {
    }

    public PenyeliaContext(DbContextOptions<PenyeliaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BobotNop> BobotNops { get; set; }

    public virtual DbSet<BobotPegawai> BobotPegawais { get; set; }

    public virtual DbSet<BobotPegawaiHasil> BobotPegawaiHasils { get; set; }

    public virtual DbSet<MAktifita> MAktifitas { get; set; }

    public virtual DbSet<MBidang> MBidangs { get; set; }

    public virtual DbSet<MGolongan> MGolongans { get; set; }

    public virtual DbSet<MJabatan> MJabatans { get; set; }

    public virtual DbSet<MKeterangan> MKeterangans { get; set; }

    public virtual DbSet<MObjekPajak> MObjekPajaks { get; set; }

    public virtual DbSet<MPegawai> MPegawais { get; set; }

    public virtual DbSet<MPegawaiBaru> MPegawaiBarus { get; set; }

    public virtual DbSet<MPegawaiOpDet> MPegawaiOpDets { get; set; }

    public virtual DbSet<MUnit> MUnits { get; set; }

    public virtual DbSet<MUserLogin> MUserLogins { get; set; }

    public virtual DbSet<MUserRole> MUserRoles { get; set; }

    public virtual DbSet<MWilayah> MWilayahs { get; set; }

    public virtual DbSet<TAktifitasPegawai> TAktifitasPegawais { get; set; }

    public virtual DbSet<TAktifitasPegawaiFile> TAktifitasPegawaiFiles { get; set; }

    public virtual DbSet<TMappingUnitWilayah> TMappingUnitWilayahs { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=penyelia;Password=Penyelia@2025!;Data Source=10.21.39.80:1521/DEVDB;");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PENYELIA");

        modelBuilder.Entity<MAktifita>(entity =>
        {
            entity.HasKey(e => e.IdAktifitas).HasName("SYS_C0034481");

            entity.Property(e => e.IdAktifitas).ValueGeneratedNever();
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER KEY'\r\n");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
        });

        modelBuilder.Entity<MBidang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_BIDANG_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.Cabang).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<MGolongan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_GOLONGAN_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<MJabatan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_JABATAN_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");
        });

        modelBuilder.Entity<MKeterangan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_KETERANGAN_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdAktifitasNavigation).WithMany(p => p.MKeterangans).HasConstraintName("M_KETERANGAN_M_AKTIFITAS_FK");
        });

        modelBuilder.Entity<MObjekPajak>(entity =>
        {
            entity.HasKey(e => e.Nop).HasName("M_OBJEK_PAJAK_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER KEY'");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
        });

        modelBuilder.Entity<MPegawai>(entity =>
        {
            entity.HasKey(e => e.Nip).HasName("M_PEGAWAI_PK");

            entity.Property(e => e.Aktif).HasDefaultValueSql("1                     ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'          ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate               ");

            entity.HasOne(d => d.Bidang).WithMany(p => p.MPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_PEGAWAI_BID_ID");

            entity.HasOne(d => d.Golongan).WithMany(p => p.MPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_PEGAWAI_GOL_FK");

            entity.HasOne(d => d.Jabatan).WithMany(p => p.MPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_PEGAWAI_JAB_ID");

            entity.HasOne(d => d.Ke).WithMany(p => p.MPegawais).HasConstraintName("M_PEGAWAI_M_WILAYAH_FK");
        });

        modelBuilder.Entity<MPegawaiBaru>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C0034825");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.JenisKelamin).IsFixedLength();
            entity.Property(e => e.Ket).HasDefaultValueSql("0\r\n");
        });

        modelBuilder.Entity<MPegawaiOpDet>(entity =>
        {
            entity.HasKey(e => new { e.Nip, e.Nop, e.TglAssign }).HasName("M_PEGAWAI_OP_DET_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");

            entity.HasOne(d => d.NipNavigation).WithMany(p => p.MPegawaiOpDets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEGAWAI_OP_DET_NIP");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.MPegawaiOpDets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEGAWAI_OP_DET_NOP");
        });

        modelBuilder.Entity<MUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C0034786");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<MUserLogin>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("M_USER_LOGIN_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");

            entity.HasOne(d => d.Role).WithMany(p => p.MUserLogins).HasConstraintName("FK_USER_ROLE");

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.MUserLogin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_USER_LOGIN_M_PEGAWAI_FK");
        });

        modelBuilder.Entity<MUserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C0034484");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
        });

        modelBuilder.Entity<MWilayah>(entity =>
        {
            entity.HasKey(e => new { e.KdKecamatan, e.KdKelurahan }).HasName("M_WILAYAH_PK");
        });

        modelBuilder.Entity<TAktifitasPegawai>(entity =>
        {
            entity.HasOne(d => d.IdAktifitasNavigation).WithMany(p => p.TAktifitasPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_AKTIFITAS_M_AKTIFITAS_FK");

            entity.HasOne(d => d.NipNavigation).WithMany(p => p.TAktifitasPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_AKTIFITAS_M_PEGAWAI_FK");

            entity.HasOne(d => d.NopNavigation).WithMany(p => p.TAktifitasPegawais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_AKTIFITAS_M_OBJEK_PAJAK_FK");
        });

        modelBuilder.Entity<TAktifitasPegawaiFile>(entity =>
        {
            entity.HasKey(e => new { e.Nip, e.Nop, e.IdAktifitas, e.Seq }).HasName("PK_T_AKTIFITAS_FILE");

            entity.HasOne(d => d.TAktifitasPegawai).WithOne(p => p.TAktifitasPegawaiFile)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_AKTIFITAS_FILE_PEGAWAI_FK");
        });

        modelBuilder.Entity<TMappingUnitWilayah>(entity =>
        {
            entity.Property(e => e.IdUnit).ValueGeneratedOnAdd();
            entity.Property(e => e.InsertDate).HasDefaultValueSql("SYSDATE\r\n");
            entity.Property(e => e.TglMulai).HasDefaultValueSql("SYSDATE");
        });
        modelBuilder.HasSequence("SEQ_M_PEGAWAI_BARU");
        modelBuilder.HasSequence("SEQ_MAPPING_UNIT_WILAYAH");
        modelBuilder.HasSequence("SEQ_MASTER_UNIT");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
