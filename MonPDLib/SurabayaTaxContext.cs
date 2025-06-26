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

        public DbSet<DbOpAbt> OpAbt { get; set; }
        public DbSet<OPSkpdAbt> OPSkpdAbt { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DbOpAbt>().HasNoKey();
            modelBuilder.Entity<OPSkpdAbt>().HasNoKey();
        }
    }
}
