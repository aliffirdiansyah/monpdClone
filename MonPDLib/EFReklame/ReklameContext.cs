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

    public virtual DbSet<DrftMJalan> DrftMJalans { get; set; }

    public virtual DbSet<MDokuman> MDokumen { get; set; }

    public virtual DbSet<MDokumenIn> MDokumenIns { get; set; }

    public virtual DbSet<MJalan> MJalans { get; set; }

    public virtual DbSet<MJalanKawasan> MJalanKawasans { get; set; }

    public virtual DbSet<MJenisReklame> MJenisReklames { get; set; }

    public virtual DbSet<MKawasan> MKawasans { get; set; }

    public virtual DbSet<MKelasJalan> MKelasJalans { get; set; }

    public virtual DbSet<MNilaiSatuanStrategi> MNilaiSatuanStrategis { get; set; }

    public virtual DbSet<MNilaiStrategisDef> MNilaiStrategisDefs { get; set; }

    public virtual DbSet<MNilaiStrategisJalanIn> MNilaiStrategisJalanIns { get; set; }

    public virtual DbSet<MNilaiStrategisLokasi> MNilaiStrategisLokasis { get; set; }

    public virtual DbSet<MNilaiStrategisSpandang> MNilaiStrategisSpandangs { get; set; }

    public virtual DbSet<MNilaiStrategisTinggi> MNilaiStrategisTinggis { get; set; }

    public virtual DbSet<MNsrIn> MNsrIns { get; set; }

    public virtual DbSet<MNsrInsJambong> MNsrInsJambongs { get; set; }

    public virtual DbSet<MNsrLua> MNsrLuas { get; set; }

    public virtual DbSet<MNsrTinggi> MNsrTinggis { get; set; }

    public virtual DbSet<MWfActivity> MWfActivities { get; set; }

    public virtual DbSet<MWfWorkflow> MWfWorkflows { get; set; }

    public virtual DbSet<MWfWorkflowActivity> MWfWorkflowActivities { get; set; }

    public virtual DbSet<MWfWorkflowFinal> MWfWorkflowFinals { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<TPermohonan> TPermohonans { get; set; }

    public virtual DbSet<TPermohonanFile> TPermohonanFiles { get; set; }

    public virtual DbSet<TPermohonanIn> TPermohonanIns { get; set; }

    public virtual DbSet<TPermohonanInsNilai> TPermohonanInsNilais { get; set; }

    public virtual DbSet<TPermohonanInsNilaiAct> TPermohonanInsNilaiActs { get; set; }

    public virtual DbSet<TPermohonanInsNilaiHist> TPermohonanInsNilaiHists { get; set; }

    public virtual DbSet<TPermohonanPrmn> TPermohonanPrmns { get; set; }

    public virtual DbSet<TPermohonanPrmnNilai> TPermohonanPrmnNilais { get; set; }

    public virtual DbSet<TSkpdIn> TSkpdIns { get; set; }

    public virtual DbSet<TSkpdInsPenetapan> TSkpdInsPenetapans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=reklame;Password=Reklame@2025;Data Source=10.21.39.80:1521/DEVDB;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("REKLAME");

        modelBuilder.Entity<DrftMJalan>(entity =>
        {
            entity.HasKey(e => e.IdJalan).HasName("DRFT_M_JALAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
        });

        modelBuilder.Entity<MDokuman>(entity =>
        {
            entity.HasKey(e => e.IdDokumen).HasName("M_DOKUMEN_PK");

            entity.Property(e => e.IdDokumen).ValueGeneratedNever();
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'\r\n ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
        });

        modelBuilder.Entity<MDokumenIn>(entity =>
        {
            entity.HasKey(e => e.IdDokumen).HasName("M_DOKUMEN_INS_PK");

            entity.Property(e => e.IdDokumen).ValueGeneratedNever();

            entity.HasOne(d => d.IdDokumenNavigation).WithOne(p => p.MDokumenIn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_DOKUMEN_INS_FK");
        });

        modelBuilder.Entity<MJalan>(entity =>
        {
            entity.Property(e => e.IdJalan).ValueGeneratedNever();
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.KoridorJalan).HasDefaultValueSql("0 ");
            entity.Property(e => e.KoridorNilai).HasDefaultValueSql("0 ");
        });

        modelBuilder.Entity<MJalanKawasan>(entity =>
        {
            entity.HasKey(e => new { e.IdJalan, e.KawasanId, e.KelasJalanId }).HasName("DRFT_M_JALAN_KELAS_PK");

            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");

            entity.HasOne(d => d.IdJalanNavigation).WithMany(p => p.MJalanKawasans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_JALAN");

            entity.HasOne(d => d.K).WithMany(p => p.MJalanKawasans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_JALAN_KELAS");
        });

        modelBuilder.Entity<MJenisReklame>(entity =>
        {
            entity.HasKey(e => e.IdJenisReklame).HasName("SYS_C0034682");

            entity.Property(e => e.IdJenisReklame).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1 ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY'\r\n ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.IsBerjalan).HasDefaultValueSql("0");
            entity.Property(e => e.Kategori).HasDefaultValueSql("1 ");
        });

        modelBuilder.Entity<MKawasan>(entity =>
        {
            entity.HasKey(e => e.KawasanId).HasName("DRFT_M_KAWASAN_PK");
        });

        modelBuilder.Entity<MKelasJalan>(entity =>
        {
            entity.HasKey(e => new { e.KawasanId, e.KelasJalanId }).HasName("DRFT_M_KELAS_JALAN_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
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

        modelBuilder.Entity<MNilaiStrategisJalanIn>(entity =>
        {
            entity.HasKey(e => new { e.IdJenisReklame, e.KelasJalan, e.TglAwalBerlaku }).HasName("PK_M_STGRS_INS");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.IsDlmRuang).HasDefaultValueSql("0 ");
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

        modelBuilder.Entity<MNsrIn>(entity =>
        {
            entity.HasKey(e => new { e.IdJenisReklame, e.TglAwalBerlaku }).HasName("M_NSR_INS_PK");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.IsEvent).HasDefaultValueSql("0 ");
            entity.Property(e => e.MasaPajak).HasDefaultValueSql("0 ");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.MNsrIns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_NSR_INS_M_JENIS_REKLAME_FK");
        });

        modelBuilder.Entity<MNsrInsJambong>(entity =>
        {
            entity.HasKey(e => new { e.IdJenisReklame, e.TglAwalBerlaku }).HasName("PK_NILAI_JAMBONG");

            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE");

            entity.HasOne(d => d.MNsrIn).WithOne(p => p.MNsrInsJambong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_INS_JMB_M_NSR_FK");
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

        modelBuilder.Entity<MWfActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_WF_ACTIVITY_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1 ");
            entity.Property(e => e.AllowEdit).HasDefaultValueSql("0 ");
            entity.Property(e => e.AllowReject).HasDefaultValueSql("0 ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.KdSurvey).HasDefaultValueSql("-1 ");
        });

        modelBuilder.Entity<MWfWorkflow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("M_WF_WORKFLOW_PK");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Aktif).HasDefaultValueSql("1 ");
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
        });

        modelBuilder.Entity<MWfWorkflowActivity>(entity =>
        {
            entity.HasKey(e => new { e.WorkflowId, e.ActivityId }).HasName("M_WF_WORKFLOW_ACTIVITY_PK");

            entity.HasOne(d => d.Activity).WithMany(p => p.MWfWorkflowActivities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_WF_DETAIL_FK_ACT");

            entity.HasOne(d => d.Workflow).WithMany(p => p.MWfWorkflowActivities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_WF_DETAIL_FK_WF");
        });

        modelBuilder.Entity<MWfWorkflowFinal>(entity =>
        {
            entity.HasKey(e => new { e.WorkflowId, e.Nip, e.TglMulai }).HasName("M_WF_WORKFLOW_FINAL_PK");

            entity.Property(e => e.IsPlt).HasDefaultValueSql("0 ");
            entity.Property(e => e.ModeKeputusan).HasDefaultValueSql("0 ");
            entity.Property(e => e.StatusBlock).HasDefaultValueSql("0 ");
            entity.Property(e => e.Terminate).HasDefaultValueSql("0 ");

            entity.HasOne(d => d.Workflow).WithMany(p => p.MWfWorkflowFinals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("M_WF_WORKFLOW_FINAL_R01");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Properti).HasName("SYS_C0034835");
        });

        modelBuilder.Entity<TPermohonan>(entity =>
        {
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.StatusPermohonan).HasDefaultValueSql("1 ");
            entity.Property(e => e.StatusProses).HasDefaultValueSql("0 ");
        });

        modelBuilder.Entity<TPermohonanFile>(entity =>
        {
            entity.HasKey(e => new { e.IdFile, e.TahunPel, e.BulanPel, e.SeqPel }).HasName("T_PERMOHONAN_FILE_PK");

            entity.HasOne(d => d.TPermohonan).WithMany(p => p.TPermohonanFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PERMOHONAN_FILE_FK");
        });

        modelBuilder.Entity<TPermohonanIn>(entity =>
        {
            entity.HasKey(e => new { e.TahunPel, e.BulanPel, e.SeqPel, e.Seq }).HasName("T_PERMOHONAN_INS_PK");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.TPermohonanIns).HasConstraintName("T_PERMOHONAN_INS_JENIS_FK");

            entity.HasOne(d => d.TPermohonan).WithMany(p => p.TPermohonanIns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PERMOHONAN_INS_FK");
        });

        modelBuilder.Entity<TPermohonanInsNilai>(entity =>
        {
            entity.HasKey(e => new { e.TahunPel, e.BulanPel, e.SeqPel, e.Seq }).HasName("T_PERMOHONAN_INS_NILAI_PK");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.TPermohonanInsNilais).HasConstraintName("T_PER_INS_NILAI_JENIS_FK");

            entity.HasOne(d => d.TPermohonanIn).WithOne(p => p.TPermohonanInsNilai).HasConstraintName("T_PERMOHONAN_INS_NILAI_FK");
        });

        modelBuilder.Entity<TPermohonanInsNilaiAct>(entity =>
        {
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.Status).HasDefaultValueSql("0 ");

            entity.HasOne(d => d.Act).WithMany(p => p.TPermohonanInsNilaiActs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PER_INS_ACT_WF_ACTIVITY_FK");

            entity.HasOne(d => d.Wf).WithMany(p => p.TPermohonanInsNilaiActs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PER_ACT_M_WF_WORKFLOW_FK");

            entity.HasOne(d => d.TPermohonanInsNilai).WithMany(p => p.TPermohonanInsNilaiActs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_PERMOHONAN_INS_NILAI_ACT");
        });

        modelBuilder.Entity<TPermohonanInsNilaiHist>(entity =>
        {
            entity.HasKey(e => new { e.TahunPel, e.BulanPel, e.SeqPel, e.Seq, e.ActId, e.WfId, e.ActSeq, e.SeqHistory }).HasName("T_PERMOHONAN_INS_NILAI_HIST_PK");
        });

        modelBuilder.Entity<TPermohonanPrmn>(entity =>
        {
            entity.HasKey(e => new { e.TahunPel, e.BulanPel, e.SeqPel, e.Seq }).HasName("T_PERMOHONAN_PRMN_PK");

            entity.HasOne(d => d.IdJenisReklameNavigation).WithMany(p => p.TPermohonanPrmns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("T_PERMOHONAN_PRMN_JENIS_FK");
        });

        modelBuilder.Entity<TPermohonanPrmnNilai>(entity =>
        {
            entity.HasKey(e => new { e.TahunPel, e.BulanPel, e.SeqPel, e.Seq }).HasName("T_PERM_PRMN_NILAI_PK");
        });

        modelBuilder.Entity<TSkpdIn>(entity =>
        {
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");
            entity.Property(e => e.TarifPajak).HasDefaultValueSql("25");
            entity.Property(e => e.TarifRokok).HasDefaultValueSql("25");
        });

        modelBuilder.Entity<TSkpdInsPenetapan>(entity =>
        {
            entity.Property(e => e.InsBy).HasDefaultValueSql("'MASTER_KEY' ");
            entity.Property(e => e.InsDate).HasDefaultValueSql("SYSDATE ");

            entity.HasOne(d => d.Surat).WithOne(p => p.TSkpdInsPenetapan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_SKPD_INS_PENETAPAN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
