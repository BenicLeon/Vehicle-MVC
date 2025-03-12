using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ninject;
using Ninject.Extensions.DependencyInjection;
using Ninject.Web.Common;
using Vehicle.Service.Data;
using Vehicle.Service.Mappings;
using Vehicle.Service.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());


builder.Host.ConfigureContainer<IKernel>(kernel =>
{
    
    kernel.Bind<IVehicleService>().To<VehicleService>().InRequestScope();
    
});

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<VehicleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),ServiceLifetime.Scoped);


builder.Services.AddControllersWithViews();


builder.Services.AddLogging();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
