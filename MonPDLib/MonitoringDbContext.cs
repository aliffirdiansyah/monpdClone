using Microsoft.EntityFrameworkCore;
using MonPDLib.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonPDLib.Helper;

namespace MonPDLib
{
    public class MonitoringDbContext : DbContext
    {
        public MonitoringDbContext()
        {
        }

        public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options)
            : base(options)
        {
        }

        public DbSet<DbMonReklamePerpanjangan> DbMonReklamePerpanjangans { get; set; }
        public DbSet<DbMonReklameInsJumlah> DbMonReklameInsJumlahs { get; set; }
        public DbSet<DbRekamAlatT> DbRekamAlatTs { get; set; }
        public DbSet<KetetapanPbb> KetetapanPbbs { get; set; }
        public DbSet<RealisasiPbb> RealisasiPbbs { get; set; }
        public DbSet<SSPD> SSPDs { get; set; }
        public DbSet<OPPbb> OPPbbs { get; set; }
        public DbSet<DbOpHotel> DbOpHotels { get; set; }
        public DbSet<DbOpPbb> DbOpPbbs { get; set; }
        public DbSet<DbOpHiburan> DbOpHiburans { get; set; }
        public DbSet<DbOpReklame> DbOpReklames { get; set; }
        public DbSet<DbOpResto> DbOpRestos { get; set; }
        public DbSet<DbOpListrik> DbOpListriks { get; set; }
        public DbSet<DbOpParkir> DbOpParkirs { get; set; }
        public DbSet<OPSkpdHotel> OPSkpdHotels { get; set; }
        public DbSet<OPSkpdResto> OPSkpdRestos { get; set; }
        public DbSet<OPSkpdParkir> OPSkpdParkirs { get; set; }
        public DbSet<OPSkpdHiburan> OPSkpdHiburans { get; set; }
        public DbSet<OPSkpdListrik> OPSkpdListriks { get; set; }
        public DbSet<OpSkpdBphtb> OpSkpdBphtbs { get; set; }
        public DbSet<OPSkpdPbb> OPSkpdPbbs { get; set; }
        public DbSet<OpOpsenSkpdPkb> OpOpsenSkpdPkbs { get; set; }
        public DbSet<OpOpsenSkpdBbnkb> OpOpsenSkpdBbnkbs { get; set; }
        public DbSet<OpSkpdSspdReklame> OpSkpdSspdReklames { get; set; }

        public DbSet<DbMonReklameEmail> DbMonReklameEmails { get; set; }
        public DbSet<DbMonReklameSurat> DbMonReklameSurats { get; set; }
        public DbSet<DbMonReklameSuratTegur> DbMonReklameSuratTegurs { get; set; }
        public DbSet<DbMonReklameSuratTegurDok> DbMonReklameSuratTegurDoks { get; set; }
        public DbSet<DbMonReklameSurvey> DbMonReklameSurveys { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbMonReklamePerpanjangan>().HasNoKey();
            modelBuilder.Entity<DbMonReklameInsJumlah>().HasNoKey();
            modelBuilder.Entity<DbRekamAlatT>().HasNoKey();
            modelBuilder.Entity<RealisasiPbb>().HasNoKey();
            modelBuilder.Entity<KetetapanPbb>().HasNoKey();
            modelBuilder.Entity<OPPbb>().HasNoKey();
            modelBuilder.Entity<SSPD>().HasNoKey();
            modelBuilder.Entity<DbOpHotel>().HasNoKey();

            modelBuilder.Entity<DbOpReklame>().HasNoKey();

            modelBuilder.Entity<DbOpParkir>().HasNoKey();

            modelBuilder.Entity<DbOpPbb>().HasNoKey();

            modelBuilder.Entity<DbOpHiburan>().HasNoKey();

            modelBuilder.Entity<DbOpListrik>().HasNoKey();

            modelBuilder.Entity<OPSkpdHotel>().HasNoKey();

            modelBuilder.Entity<OPSkpdResto>().HasNoKey();

            modelBuilder.Entity<OPSkpdParkir>().HasNoKey();

            modelBuilder.Entity<OPSkpdHiburan>().HasNoKey();

            modelBuilder.Entity<OPSkpdListrik>().HasNoKey();

            modelBuilder.Entity<OpSkpdBphtb>().HasNoKey();

            modelBuilder.Entity<OPSkpdPbb>().HasNoKey();

            modelBuilder.Entity<OpOpsenSkpdPkb>().HasNoKey();

            modelBuilder.Entity<OpOpsenSkpdBbnkb>().HasNoKey();

            modelBuilder.Entity<OpSkpdSspdReklame>().HasNoKey();

            modelBuilder.Entity<SSPDPbjt>().HasNoKey();

            modelBuilder.Entity<DbMonReklameEmail>().HasNoKey();
            modelBuilder.Entity<DbMonReklameSurat>().HasNoKey();
            modelBuilder.Entity<DbMonReklameSuratTegur>().HasNoKey();
            modelBuilder.Entity<DbMonReklameSuratTegurDok>().HasNoKey();
            modelBuilder.Entity<DbMonReklameSurvey>().HasNoKey();
        }
    }
}
