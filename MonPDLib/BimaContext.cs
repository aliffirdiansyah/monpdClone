using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
