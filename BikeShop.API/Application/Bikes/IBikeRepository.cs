using BikeShop.API.Domain.Bikes;

namespace BikeShop.API.Application.Bikes;

public interface IBikeRepository
{
    Task<IReadOnlyList<Bike>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Bike?> GetByRefAsync(string bikeRef, CancellationToken cancellationToken = default);
    Task<Bike> AddAsync(Bike bike, CancellationToken cancellationToken = default);
}

