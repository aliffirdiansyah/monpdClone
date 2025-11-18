using Microsoft.EntityFrameworkCore;
using MonPDLib.EF;
using MonPDLib.EFReklame;
using MonPDLib.EFPenyelia;
using MonPDLib.EFReklameSsw;
using MonPDLib.EFPlanning;
using System.Text;
using System.Text.Json;

namespace MonPDLib
{
    public class DBClass
    {        
        public static string Monpd = "";
        public static string Reklame = "";
        public static string Penyelia = "";
        public static string ReklameSSW = "";
        public static string Planning = "";
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
        
        public static ReklameContext GetReklameContext()
        {         
            var optionsBuilder = new DbContextOptionsBuilder<ReklameContext>();
            optionsBuilder.UseOracle(Reklame, opt => opt.UseOracleSQLCompatibility("11"));
            return new ReklameContext(optionsBuilder.Options);
        }
        
        public static PenyeliaContext GetPenyeliaContext()
        {         
            var optionsBuilder = new DbContextOptionsBuilder<PenyeliaContext>();
            optionsBuilder.UseOracle(Penyelia, opt => opt.UseOracleSQLCompatibility("11"));
            return new PenyeliaContext(optionsBuilder.Options);
        }
        
        public static ReklameSswContext GetReklameSswContext()
        {         
            var optionsBuilder = new DbContextOptionsBuilder<ReklameSswContext>();
            optionsBuilder.UseOracle(ReklameSSW, opt => opt.UseOracleSQLCompatibility("11"));
            return new ReklameSswContext(optionsBuilder.Options);
        }
        
        public static PlanningContext GetEPlanningContext()
        {         
            var optionsBuilder = new DbContextOptionsBuilder<PlanningContext>();
            optionsBuilder.UseOracle(Planning, opt => opt.UseOracleSQLCompatibility("11"));
            return new PlanningContext(optionsBuilder.Options);
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
