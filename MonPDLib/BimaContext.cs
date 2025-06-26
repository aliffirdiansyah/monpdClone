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
    public class BimaContext : DbContext
    {
        public BimaContext()
        {
        }

        public BimaContext(DbContextOptions<BimaContext> options)
            : base(options)
        {
        }
        public DbSet<SSPD> SSPD { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SSPD>().HasNoKey();
        }
    }
}
