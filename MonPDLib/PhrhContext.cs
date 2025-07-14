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
    public class PhrhContext : DbContext
    {
        public PhrhContext()
        {
        }

        public PhrhContext(DbContextOptions<PhrhContext> options)
            : base(options)
        {
        }

        public DbSet<Npwpd> Npwpds { get; set; }
        public DbSet<SSPDPbjt> SSPD { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SSPDPbjt>().HasNoKey();

            modelBuilder.Entity<Npwpd>().HasNoKey();
        }
    }
}
