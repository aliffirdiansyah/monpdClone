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

        public DbSet<DbOpHotel> DbOpHotels { get; set; }
        public DbSet<OPSkpdHotel> OPSkpdHotels { get; set; }
        public DbSet<OPSkpdResto> OPSkpdRestos { get; set; }
        public DbSet<OPSkpdParkir> OPSkpdParkirs { get; set; }
        public DbSet<OPSkpdHiburan> OPSkpdHiburans { get; set; }
        public DbSet<OPSkpdListrik> OPSkpdListriks { get; set; }
        public DbSet<OpSkpdBphtb> OpSkpdBphtbs { get; set; }
        public DbSet<OPSkpdPbb> OPSkpdPbbs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbOpHotel>().HasNoKey();

            modelBuilder.Entity<OPSkpdHotel>().HasNoKey();

            modelBuilder.Entity<OPSkpdResto>().HasNoKey();

            modelBuilder.Entity<OPSkpdParkir>().HasNoKey();

            modelBuilder.Entity<OPSkpdHiburan>().HasNoKey();

            modelBuilder.Entity<OPSkpdListrik>().HasNoKey();

            modelBuilder.Entity<OpSkpdBphtb>().HasNoKey();

            modelBuilder.Entity<OPSkpdPbb>().HasNoKey();
        }
    }
}
