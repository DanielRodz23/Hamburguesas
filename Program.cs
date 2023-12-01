using Hamburguesas.Models.Entities;
using Hamburguesas.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MenuRepository>();
builder.Services.AddTransient<ClasificacionRepository>();

var json = Directory.GetFiles(Directory.GetCurrentDirectory(), "Conexion.json");
if (json.Length>0)
{
    var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("Conexion.json")
               .Build();

    builder.Services.AddDbContext<NeatContext>
        (x =>
        x.UseMySql(configuration.GetConnectionString("NeatConnection"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql"))
    );
}
else
{
    builder.Services.AddDbContext<NeatContext>
    (x =>
        x.UseMySql("server=localhost;user=root;database=neat;password=root",
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.1.0-mysql"))
    );
}

builder.Services.AddMvc();

var app = builder.Build();

app.UseFileServer();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapDefaultControllerRoute();

app.Run();
