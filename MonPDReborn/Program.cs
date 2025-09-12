using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MonPDLib;
using MonPDLib.EF;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//koneksi oracle
var configValue = builder.Configuration.GetSection("Conn:Monpd").Value;
DBClass.Monpd = configValue ?? throw new ArgumentNullException("Connection string 'Monpd' is not configured.");

var supportedCultures = new[] { new CultureInfo("id-ID") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("id-ID");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?message='Error'&?statusCode={0}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

public class RecaptchaSettings
{
    public string SiteKey { get; set; }
    public string SecretKey { get; set; }
}