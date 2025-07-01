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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbOpHotel>().HasNoKey();
        }
    }
}
