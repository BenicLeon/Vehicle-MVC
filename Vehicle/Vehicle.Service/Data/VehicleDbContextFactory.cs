using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Vehicle.Service.Data
{
    public class VehicleDbContextFactory : IDesignTimeDbContextFactory<VehicleDbContext>
    {
        public VehicleDbContext CreateDbContext(string[] args)
        {
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Vehicle.MVC")) 
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<VehicleDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new VehicleDbContext(optionsBuilder.Options);
        }
    }
}