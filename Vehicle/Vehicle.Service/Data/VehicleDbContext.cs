using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Service.Models;

namespace Vehicle.Service.Data
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext() { }
        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) { }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<VehicleMake>().HasData(
                new VehicleMake { Id = 1, Name = "BMW", Abrv = "BMW" },
                new VehicleMake { Id = 2, Name = "Ford", Abrv = "FRD" },
                new VehicleMake { Id = 3, Name = "Volkswagen", Abrv = "VW" }
            );


            modelBuilder.Entity<VehicleModel>().HasData(
                new VehicleModel { Id = 1, MakeId = 1, Name = "X5", Abrv = "X5" },
                new VehicleModel { Id = 2, MakeId = 1, Name = "M3", Abrv = "M3" },
                new VehicleModel { Id = 3, MakeId = 2, Name = "Mustang", Abrv = "MST" },
                new VehicleModel { Id = 4, MakeId = 3, Name = "Golf", Abrv = "GLF" }
            );
        }
    }
}

