﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vehicle.Service.Data;

#nullable disable

namespace Vehicle.Service.Migrations
{
    [DbContext(typeof(VehicleDbContext))]
    partial class VehicleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Vehicle.Service.Models.VehicleMake", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abrv")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("VehicleMakes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Abrv = "BMW",
                            Name = "BMW"
                        },
                        new
                        {
                            Id = 2,
                            Abrv = "FRD",
                            Name = "Ford"
                        },
                        new
                        {
                            Id = 3,
                            Abrv = "VW",
                            Name = "Volkswagen"
                        });
                });

            modelBuilder.Entity("Vehicle.Service.Models.VehicleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abrv")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("MakeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("MakeId");

                    b.ToTable("VehicleModels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Abrv = "X5",
                            MakeId = 1,
                            Name = "X5"
                        },
                        new
                        {
                            Id = 2,
                            Abrv = "M3",
                            MakeId = 1,
                            Name = "M3"
                        },
                        new
                        {
                            Id = 3,
                            Abrv = "MST",
                            MakeId = 2,
                            Name = "Mustang"
                        },
                        new
                        {
                            Id = 4,
                            Abrv = "GLF",
                            MakeId = 3,
                            Name = "Golf"
                        });
                });

            modelBuilder.Entity("Vehicle.Service.Models.VehicleModel", b =>
                {
                    b.HasOne("Vehicle.Service.Models.VehicleMake", "Make")
                        .WithMany("Vehicles")
                        .HasForeignKey("MakeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Make");
                });

            modelBuilder.Entity("Vehicle.Service.Models.VehicleMake", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
