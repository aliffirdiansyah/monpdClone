using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
