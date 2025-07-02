using Microsoft.EntityFrameworkCore;
using MonPDLib.EF;
using System.Text;
using System.Text.Json;

namespace MonPDLib
{
    public class DBClass
    {        
        public static string Monpd = "";
        public static string StV1 = "";
        public static string Hpp = "";
        public static string Phrh = "";
        public static string Bima = "";
        public static string MonitoringDb = "";

        public static ModelContext GetContext()
        {         
            var optionsBuilder = new DbContextOptionsBuilder<ModelContext>();
            optionsBuilder.UseOracle(Monpd, opt => opt.UseOracleSQLCompatibility("11"));
            return new ModelContext(optionsBuilder.Options);
        }

        public static SurabayaTaxContext GetSurabayaTaxContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SurabayaTaxContext>();
            optionsBuilder.UseOracle(StV1, opt => opt.UseOracleSQLCompatibility("11"));
            return new SurabayaTaxContext(optionsBuilder.Options);
        }

        public static HppContext GetHppContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<HppContext>();
            optionsBuilder.UseOracle(Hpp, opt => opt.UseOracleSQLCompatibility("11"));
            return new HppContext(optionsBuilder.Options);
        }

        public static PhrhContext GetPhrhContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PhrhContext>();
            optionsBuilder.UseOracle(Phrh, opt => opt.UseOracleSQLCompatibility("11"));
            return new PhrhContext(optionsBuilder.Options);
        }

        public static BimaContext GetBimaContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BimaContext>();
            optionsBuilder.UseOracle(Bima, opt => opt.UseOracleSQLCompatibility("11"));
            return new BimaContext(optionsBuilder.Options);
        }

        public static MonitoringDbContext GetMonitoringDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MonitoringDbContext>();
            optionsBuilder.UseOracle(MonitoringDb, opt => opt.UseOracleSQLCompatibility("11"));
            return new MonitoringDbContext(optionsBuilder.Options);
        }
    }
}
