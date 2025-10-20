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

HttpClientHandler handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()   // izinkan semua origin
            .AllowAnyMethod()   // izinkan semua HTTP method (GET, POST, dll)
            .AllowAnyHeader();  // izinkan semua header
    });
});

//koneksi oracle
var configValue = builder.Configuration.GetSection("Conn:Monpd").Value;
DBClass.Monpd = configValue ?? throw new ArgumentNullException("Connection string 'Monpd' is not configured.");

var configValueReklame = builder.Configuration.GetSection("Conn:Reklame").Value;
DBClass.Reklame = configValue ?? throw new ArgumentNullException("Connection string 'Reklame' is not configured.");

var configValuePenyelia = builder.Configuration.GetSection("Conn:Penyelia").Value;
DBClass.Penyelia = configValue ?? throw new ArgumentNullException("Connection string 'Penyelia' is not configured.");

var supportedCultures = new[] { new CultureInfo("id-ID") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("id-ID");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddReverseProxy()
    .LoadFromMemory(
        routes: new[]
        {
            new Yarp.ReverseProxy.Configuration.RouteConfig()
            {
                RouteId = "stream",
                ClusterId = "stream_cluster",
                Match = new() { Path = "/stream/{**catch-all}" } // route prefix
            }
        },
        clusters: new[]
        {
            new Yarp.ReverseProxy.Configuration.ClusterConfig()
            {
                ClusterId = "stream_cluster",
                Destinations = new Dictionary<string, Yarp.ReverseProxy.Configuration.DestinationConfig>
                {
                    ["dest1"] = new() { Address = "http://10.0.12.48:8889/" }
                }
            }
        }
    );

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

// ✅ Aktifkan CORS sebelum Authorization
app.UseCors("AllowAll");

// ✅ Tambahkan endpoint proxy
app.MapReverseProxy();

app.UseAuthorization();

app.UseSession();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?message='Error'&?statusCode={404}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

public class RecaptchaSettings
{
    public string SiteKey { get; set; }
    public string SecretKey { get; set; }
}