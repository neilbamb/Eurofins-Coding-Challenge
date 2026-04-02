using BikeShop.API.Domain.Bikes;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.API.Infrastructure.Persistence;

public class BikesDbContext : DbContext
{
    public BikesDbContext(DbContextOptions<BikesDbContext> options) : base(options)
    {
    }

    public DbSet<Bike> Bikes => Set<Bike>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bike>().HasKey(b => b.Ref);
        modelBuilder.Entity<Bike>().Property(b => b.Ref).ValueGeneratedNever();
    }
}

