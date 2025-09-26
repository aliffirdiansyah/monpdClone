namespace CCTVParkirWorker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.            
            ApplicationConfiguration.Initialize();
            MonPDLib.DBClass.Monpd = "User Id=monpd;Password=monpd2025;Data Source=10.21.39.80:1521/DEVDB;";
            Application.Run(new Form1());
        }
    }
}