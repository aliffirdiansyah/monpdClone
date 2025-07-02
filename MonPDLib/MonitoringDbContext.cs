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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbOpHotel>().HasNoKey();

            modelBuilder.Entity<OPSkpdHotel>().HasNoKey();
        }
    }
}
