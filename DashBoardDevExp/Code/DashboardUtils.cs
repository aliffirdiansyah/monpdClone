using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.EntityFramework;
using DevExpress.DataAccess.Excel;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace DashBoardDevExp
{
    public static class DashboardUtils {
        public static DashboardConfigurator CreateDashboardConfigurator(IConfiguration configuration, IFileProvider fileProvider) {
            DashboardConfigurator configurator = new DashboardConfigurator();
            DashboardConfigurator.PassCredentials = true;
            configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(configuration));
            configurator.AllowExecutingCustomSql = true;

            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(fileProvider.GetFileInfo("Data/Dashboard").PhysicalPath);
            configurator.SetDashboardStorage(dashboardFileStorage);

            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

            // Registers an SQL data source.

            var parts =Helper.Conn.Split(';')
                      .Select(part => part.Split('='))
                      .ToDictionary(kv => kv[0].Trim().ToLower(), kv => kv[1].Trim());
            OracleConnectionParameters oracleParams = new OracleConnectionParameters();
            oracleParams.ServerName = parts.ContainsKey("data source") ? parts["data source"] : "";
            oracleParams.UserName = parts.ContainsKey("user id") ? parts["user id"] : "";
            oracleParams.Password = parts.ContainsKey("password") ? parts["password"] : "";
            

            DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("Oracle Data Source", oracleParams);
            //SelectQuery selectQuery = SelectQueryFluentBuilder
            //    .AddTable("SalesPerson")
            //    .SelectColumns("CategoryName", "Extended Price")
            //    .Build("Query 1");
            //sqlDataSource.Queries.Add(selectQuery);
            //sqlDataSource.Fill();
            //dashboard.DataSources.Add(sqlDataSource);

            dataSourceStorage.RegisterDataSource("oracleEFDataSource", sqlDataSource.SaveToXml());

            //var sqlDataSource = new DevExpress.DataAccess.Sql.SqlDataSource(
            //    "OracleConnectionString"
            //    //"Data Source=localhost:1521/FREEPDB1;User ID=uoracle;Password=pwduoracle@123;"
            //    );

            //sqlDataSource.Fill();
            //dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml());
            //DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("SQL Data Source", "NWindConnectionString");
            //sqlDataSource.DataProcessingMode = DataProcessingMode.Client;
            //SelectQuery query = SelectQueryFluentBuilder
            //    .AddTable("Categories").SelectAllColumnsFromTable()
            //    .Join("Products", "CategoryID").SelectAllColumnsFromTable()
            //    .Build("Products_Categories");
            //sqlDataSource.Queries.Add(query);
            //dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml());

            //// Registers an Object data source.
            //DashboardObjectDataSource objDataSource = new DashboardObjectDataSource("Object Data Source");
            //objDataSource.DataId = "Object Data Source Data Id";
            //dataSourceStorage.RegisterDataSource("objDataSource", objDataSource.SaveToXml());

            //// Registers an Excel data source.
            //DashboardExcelDataSource excelDataSource = new DashboardExcelDataSource("Excel Data Source");
            //excelDataSource.ConnectionName = "Excel Data Source Connection Name";
            //excelDataSource.SourceOptions = new ExcelSourceOptions(new ExcelWorksheetSettings("Sheet1"));
            //dataSourceStorage.RegisterDataSource("excelDataSource", excelDataSource.SaveToXml());

            configurator.SetDataSourceStorage(dataSourceStorage);

            //configurator.DataLoading += (s, e) => {
            //    if(e.DataId == "Object Data Source Data Id") {
            //        e.Data = Invoices.CreateData();
            //    }
            //};
            //configurator.ConfigureDataConnection += (s, e) => {
            //    if(e.ConnectionName == "Excel Data Source Connection Name") {
            //        ExcelDataSourceConnectionParameters excelParameters = (ExcelDataSourceConnectionParameters)e.ConnectionParameters;
            //        excelParameters.FileName = fileProvider.GetFileInfo("Data/Sales.xlsx").PhysicalPath;
            //    }
            //};
            return configurator;
        }
    }
}