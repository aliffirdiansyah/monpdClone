using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MonPDLib.EFPlanning;

public partial class PlanningContext : DbContext
{
    public PlanningContext()
    {
    }

    public PlanningContext(DbContextOptions<PlanningContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DbPendapatanDaerah> DbPendapatanDaerahs { get; set; }

    public virtual DbSet<MOpd> MOpds { get; set; }

    public virtual DbSet<MPendapatan> MPendapatans { get; set; }

    public virtual DbSet<MUserLogin> MUserLogins { get; set; }

    public virtual DbSet<MUserRole> MUserRoles { get; set; }

    public virtual DbSet<TInputManual> TInputManuals { get; set; }

    public virtual DbSet<TTransaksi> TTransaksis { get; set; }

    public virtual DbSet<TTransaksiTemp> TTransaksiTemps { get; set; }

    public virtual DbSet<TblAsal> TblAsals { get; set; }

    public virtual DbSet<TblTarget> TblTargets { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseOracle("User Id=planning;Password=Bapenda@2025;Data Source=10.21.39.80:1521/DEVDB;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PLANNING");

        modelBuilder.Entity<DbPendapatanDaerah>(entity =>
        {
            entity.Property(e => e.InsBy).IsFixedLength();
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
        });

        modelBuilder.Entity<MOpd>(entity =>
        {
            entity.HasKey(e => e.IdOpd).HasName("M_OPD_PK");

            entity.Property(e => e.IdOpd).ValueGeneratedNever();
            entity.Property(e => e.InsBy).HasDefaultValueSql("'system' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("sysdate ");
        });

        modelBuilder.Entity<MPendapatan>(entity =>
        {
            entity.Property(e => e.UpdateBy).HasDefaultValueSql("USER");
            entity.Property(e => e.UpdateDate).HasDefaultValueSql("SYSDATE");
        });

        modelBuilder.Entity<MUserLogin>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("SYS_C0035780");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");

            entity.HasOne(d => d.IdOpdNavigation).WithMany(p => p.MUserLogins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_OPD");

            entity.HasOne(d => d.Role).WithMany(p => p.MUserLogins).HasConstraintName("FK_USER_ROLE");
        });

        modelBuilder.Entity<MUserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C0035777");

            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
        });

        modelBuilder.Entity<TInputManual>(entity =>
        {
            entity.HasKey(e => new { e.Akun, e.Kelompok, e.Jenis, e.Objek, e.Rincian, e.SubRincian, e.KodeOpd, e.KodeSubOpd, e.Seq, e.Tanggal }).HasName("T_INPUT_MANUAL_PK");
        });

        modelBuilder.Entity<TTransaksi>(entity =>
        {
            entity.HasKey(e => new { e.IdOpd, e.TahunBuku, e.KodeOpd, e.SubRincian, e.Seq }).HasName("T_TRANSAKSI_PK");

            entity.Property(e => e.Seq).HasDefaultValueSql("0 ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'SYSTEM' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");

            entity.HasOne(d => d.IdOpdNavigation).WithMany(p => p.TTransaksis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_TRANSAKSI_M_OPD_FK");
        });

        modelBuilder.Entity<TTransaksiTemp>(entity =>
        {
            entity.HasKey(e => new { e.TahunBuku, e.Akun, e.Kelompok, e.Jenis, e.Objek, e.Rincian, e.SubRincian, e.KodeOpd, e.KodeSubOpd, e.Bulan }).HasName("PK_T_TRANSAKSI_10KODE");

            entity.Property(e => e.IdTransaksi).ValueGeneratedOnAdd();
            entity.Property(e => e.InsBy)
                .HasDefaultValueSql("'SYSTEM'")
                .IsFixedLength();
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");

            entity.HasOne(d => d.IdOpdNavigation).WithMany(p => p.TTransaksiTemps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_OPD_TRANSAKSI");
        });

        modelBuilder.Entity<TblAsal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C0035992");
        });
        modelBuilder.HasSequence("SEQ_M_USER_ROLE");
        modelBuilder.HasSequence("SEQ_T_TRANSAKSI");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
