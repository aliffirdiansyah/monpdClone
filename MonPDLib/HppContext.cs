using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
