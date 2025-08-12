using PbbWs;

IHost host = Host.CreateDefaultBuilder(args)
   .ConfigureServices((hostContext, services) =>
   {
       MonPDLib.DBClass.Monpd = hostContext.Configuration.GetSection("Conn:Monpd").Value ?? throw new InvalidOperationException("Connection string not found");
       MonPDLib.DBClass.StV1 = hostContext.Configuration.GetSection("Conn:StV1").Value ?? throw new InvalidOperationException("Connection string not found");
       MonPDLib.DBClass.Hpp = hostContext.Configuration.GetSection("Conn:Hpp").Value ?? throw new InvalidOperationException("Connection string not found");
       MonPDLib.DBClass.Phrh = hostContext.Configuration.GetSection("Conn:Phrh").Value ?? throw new InvalidOperationException("Connection string not found");
       MonPDLib.DBClass.Bima = hostContext.Configuration.GetSection("Conn:Bima").Value ?? throw new InvalidOperationException("Connection string not found");
       MonPDLib.DBClass.MonitoringDb = hostContext.Configuration.GetSection("Conn:MonitoringDb").Value ?? throw new InvalidOperationException("Connection string not found");
       services.AddHostedService<Worker>();       
   })
    .Build();
host.Run();