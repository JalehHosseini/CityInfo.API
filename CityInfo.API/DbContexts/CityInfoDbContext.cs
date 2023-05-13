

using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoDbContext : DbContext
{

    public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options)
        : base(options)
    {

    }


    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasData
            (new City("Tehran")
            {
                Id = 1,
                Description = "This Is Tehran from Seed Data",
            },
            new City("Shiraz")
            {
                Id = 2,
                Description = "This Is Shiraz from Seed Data",
            },
            new City("Ahvaz")
            {
                Id = 3,
                Description = "This Is Ahvaz from Seed Data",
            }
            );


        modelBuilder.Entity<PointOfInterest>()
            .HasData(new PointOfInterest("Point 1")
            {
                Id = 1,
                CityId = 1,
                Description = "This Is Point  1",

            },
            new PointOfInterest("Point 2")
            {
                Id = 2,
                CityId = 1,
                Description = "This Is Point  2",

            },
            new PointOfInterest("Point 3")
            {
                Id = 3,
                CityId = 1,
                Description = "This Is Point  3",
            });

        base.OnModelCreating(modelBuilder);
    }
}
