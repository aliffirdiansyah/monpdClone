using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using DashBoardDevExp;

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

IFileProvider? fileProvider = builder.Environment.ContentRootFileProvider;
IConfiguration? configuration = builder.Configuration;


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDevExpressControls();
Helper.Conn = builder.Configuration.GetConnectionString("gss") ?? throw new InvalidOperationException("Connection string 'PostgreeConnection' not found.");

builder.Services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) => {
    return DashboardUtils.CreateDashboardConfigurator(configuration, fileProvider);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDevExpressControls();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapDashboardRoute("dashboardControl", "DefaultDashboard");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
