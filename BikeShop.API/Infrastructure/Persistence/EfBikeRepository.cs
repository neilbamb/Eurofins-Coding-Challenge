using BikeShop.API.Domain.Bikes;
using BikeShop.API.Application.Bikes;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.API.Infrastructure.Persistence;

public class EfBikeRepository : IBikeRepository
{
    private readonly BikesDbContext _dbContext;

    public EfBikeRepository(BikesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Bike>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bikes
            .AsNoTracking()
            .OrderBy(b => b.Manufacturer)
            .ThenBy(b => b.Model)
            .ToListAsync(cancellationToken);
    }

    public Task<Bike?> GetByRefAsync(string bikeRef, CancellationToken cancellationToken = default)
    {
        return _dbContext.Bikes
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Ref == bikeRef, cancellationToken);
    }

    public async Task<Bike> AddAsync(Bike bike, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Bikes.FirstOrDefaultAsync(b => b.Ref == bike.Ref, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException($"Bike with ref '{bike.Ref}' already exists.");
        }

        _dbContext.Bikes.Add(bike);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return bike;
    }
}

