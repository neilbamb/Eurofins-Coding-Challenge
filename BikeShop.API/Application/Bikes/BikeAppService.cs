using BikeShop.API.Contracts;
using BikeShop.API.Domain.Bikes;

namespace BikeShop.API.Application.Bikes;

public class BikeAppService
{
    private readonly IBikeRepository _bikeRepository;

    public BikeAppService(IBikeRepository bikeRepository)
    {
        _bikeRepository = bikeRepository;
    }

    public Task<IReadOnlyList<BikeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return MapToDtoAsync(_bikeRepository.GetAllAsync(cancellationToken), cancellationToken);
    }

    public async Task<BikeDto?> GetByRefAsync(string bikeRef, CancellationToken cancellationToken = default)
    {
        var bike = await _bikeRepository.GetByRefAsync(bikeRef, cancellationToken);
        return bike is null ? null : MapToDto(bike);
    }

    public async Task<BikeDto> CreateAsync(CreateBikeRequest request, CancellationToken cancellationToken = default)
    {
        Validate(request);

        var bike = new Bike
        {
            Ref = Guid.NewGuid().ToString(),
            Manufacturer = request.Manufacturer.Trim(),
            Model = request.Model.Trim(),
            Category = request.Category.Trim(),
            Price = request.Price.Trim(),
            Colour = request.Colour.Trim(),
            Weight = request.Weight.Trim(),
            ImgUrl = request.ImgUrl.Trim()
        };

        var created = await _bikeRepository.AddAsync(bike, cancellationToken);
        return MapToDto(created);
    }

    private static void Validate(CreateBikeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Manufacturer)) throw new ArgumentException("Manufacturer is required.");
        if (string.IsNullOrWhiteSpace(request.Model)) throw new ArgumentException("Model is required.");
        if (string.IsNullOrWhiteSpace(request.Category)) throw new ArgumentException("Category is required.");
        if (string.IsNullOrWhiteSpace(request.Price)) throw new ArgumentException("Price is required.");
        if (string.IsNullOrWhiteSpace(request.Colour)) throw new ArgumentException("Colour is required.");
        if (string.IsNullOrWhiteSpace(request.Weight)) throw new ArgumentException("Weight is required.");
        if (string.IsNullOrWhiteSpace(request.ImgUrl)) throw new ArgumentException("ImgUrl is required.");
    }

    private static BikeDto MapToDto(Bike bike)
    {
        return new BikeDto(
            Manufacturer: bike.Manufacturer,
            Ref: bike.Ref,
            Model: bike.Model,
            Category: bike.Category,
            Price: bike.Price,
            Colour: bike.Colour,
            Weight: bike.Weight,
            ImgUrl: bike.ImgUrl);
    }

    private static async Task<IReadOnlyList<BikeDto>> MapToDtoAsync(Task<IReadOnlyList<Bike>> bikesTask, CancellationToken _)
    {
        var bikes = await bikesTask;
        return bikes.Select(MapToDto).ToList();
    }
}

