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
    public class SurabayaTaxContext : DbContext
    {
        public SurabayaTaxContext()
        {
        }

        public SurabayaTaxContext(DbContextOptions<SurabayaTaxContext> options)
            : base(options)
        {
        }

        public DbSet<DbOpAbt> DbOpAbts { get; set; }
        public DbSet<DbOpHotel> DbOpHotels { get; set; }
        public DbSet<OPSkpdAbt> OPSkpdAbts { get; set; }
        public DbSet<OPSkpdHotel> OPSkpdHotels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DbOpAbt>().HasNoKey();
            modelBuilder.Entity<DbOpHotel>().HasNoKey();
            modelBuilder.Entity<OPSkpdAbt>().HasNoKey();
            modelBuilder.Entity<OPSkpdHotel>().HasNoKey();
        }
    }
}
