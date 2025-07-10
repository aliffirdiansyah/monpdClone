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
    public class HppContext : DbContext
    {
        public HppContext()
        {
        }

        public HppContext(DbContextOptions<HppContext> options)
            : base(options)
        {
        }

        public DbSet<DbOpHotel> DbOpHotels { get; set; }
        public DbSet<DbOpResto> DbOpRestos { get; set; }
        public DbSet<DbOpParkir> DbOpParkirs { get; set; }
        public DbSet<DbOpHiburan> DbOpHiburans { get; set; }
        public DbSet<DbOpListrik> DbOpListriks { get; set; }
        public DbSet<SSPDPbjt> SSPDPbjts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbOpHotel>().HasNoKey();
            modelBuilder.Entity<DbOpParkir>().HasNoKey();
            modelBuilder.Entity<DbOpHiburan>().HasNoKey();
            modelBuilder.Entity<DbOpResto>().HasNoKey();
            modelBuilder.Entity<DbOpListrik>().HasNoKey();
            modelBuilder.Entity<SSPDPbjt>().HasNoKey();
        }
    }
}
