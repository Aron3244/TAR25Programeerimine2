using ShopTARpe25.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ShopTARpe25Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        }));
//void ConfigureServices(IServiceCollection services)
//{
//    services.AddDbContext<DatabaseTaskDbContext>(options =>
//        options.UseSqlServer(Microsoft.Extensions.Configuration.GetConnectionString("databasename")));
//}

builder.Services.AddScoped<IKindergartenServices, KindergartenServices>();

// Lisa enne rida: var app = builder.Build();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
