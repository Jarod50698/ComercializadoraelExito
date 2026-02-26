using ComercializadoraelExito.Data;
using ComercializadoraelExito.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Licencia QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

// MVC
builder.Services.AddControllersWithViews();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    )
);

// Servicios
builder.Services.AddScoped<FacturaService>();

var app = builder.Build();

// Manejo de errores en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

app.Run();