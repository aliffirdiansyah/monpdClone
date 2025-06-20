using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
