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
        public DbSet<DbOpResto> DbOpRestos { get; set; }
        public DbSet<DbOpParkir> DbOpParkirs { get; set; }
        public DbSet<DbOpHiburan> DbOpHiburans { get; set; }
        public DbSet<DbOpListrik> DbOpListriks { get; set; }
        public DbSet<OPSkpdAbt> OPSkpdAbts { get; set; }
        public DbSet<OPSkpdHotel> OPSkpdHotels { get; set; }
        public DbSet<OPSkpdResto> OPSkpdRestos { get; set; }
        public DbSet<OPSkpdParkir> OPSkpdParkirs { get; set; }
        public DbSet<OPSkpdHiburan> OPSkpdHiburans { get; set; }
        public DbSet<OPSkpdListrik> OPSkpdListriks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DbOpAbt>().HasNoKey();
            modelBuilder.Entity<DbOpHotel>().HasNoKey();
            modelBuilder.Entity<DbOpResto>().HasNoKey();
            modelBuilder.Entity<DbOpHiburan>().HasNoKey();
            modelBuilder.Entity<DbOpParkir>().HasNoKey();
            modelBuilder.Entity<DbOpListrik>().HasNoKey();
            modelBuilder.Entity<OPSkpdAbt>().HasNoKey();
            modelBuilder.Entity<OPSkpdHotel>().HasNoKey();
            modelBuilder.Entity<OPSkpdResto>().HasNoKey();
            modelBuilder.Entity<OPSkpdParkir>().HasNoKey();
            modelBuilder.Entity<OPSkpdHiburan>().HasNoKey();
            modelBuilder.Entity<OPSkpdListrik>().HasNoKey();
        }
    }
}
