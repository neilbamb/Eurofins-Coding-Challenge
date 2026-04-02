using BikeShop.API.Application.Bikes;
using BikeShop.API.Contracts;
using BikeShop.API.Domain.Bikes;

namespace BikeShop.API.Tests;

public class BikeAppServiceTests
{
    [Fact]
    public async Task GetAllAsync_returns_mapped_bikes()
    {
        var repo = new InMemoryBikeRepository(new[]
        {
            new Bike { Ref = "ref-1", Manufacturer = "M1", Model = "A", Category = "C", Price = "P", Colour = "Col", Weight = "W", ImgUrl = "I" },
            new Bike { Ref = "ref-2", Manufacturer = "M2", Model = "B", Category = "C", Price = "P", Colour = "Col", Weight = "W", ImgUrl = "I" }
        });

        var sut = new BikeAppService(repo);

        var result = await sut.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, b => b.Ref == "ref-1" && b.Manufacturer == "M1" && b.ImgUrl == "I");
        Assert.Contains(result, b => b.Ref == "ref-2" && b.Manufacturer == "M2");
    }

    [Fact]
    public async Task GetByRefAsync_returns_null_when_not_found()
    {
        var repo = new InMemoryBikeRepository(Array.Empty<Bike>());
        var sut = new BikeAppService(repo);

        var result = await sut.GetByRefAsync("missing");

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_throws_when_manufacturer_missing()
    {
        var repo = new InMemoryBikeRepository(Array.Empty<Bike>());
        var sut = new BikeAppService(repo);

        var request = new CreateBikeRequest(
            Manufacturer: " ",
            Model: "Model",
            Category: "Category",
            Price: "€1.00",
            Colour: "Red",
            Weight: "10kg",
            ImgUrl: "/assets/images/bikes/x.png");

        await Assert.ThrowsAsync<ArgumentException>(() => sut.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_persists_and_returns_created_bike()
    {
        var repo = new InMemoryBikeRepository(Array.Empty<Bike>());
        var sut = new BikeAppService(repo);

        var request = new CreateBikeRequest(
            Manufacturer: "Boardman",
            Model: "X1",
            Category: "Road Bike",
            Price: "€999.00",
            Colour: "Black",
            Weight: "9kg",
            ImgUrl: "/assets/images/bikes/boardman-x1.png");

        var created = await sut.CreateAsync(request);
        var fromRepo = await repo.GetByRefAsync(created.Ref);

        Assert.NotNull(fromRepo);
        Assert.Equal(created.Ref, fromRepo!.Ref);
        Assert.Equal("Boardman", created.Manufacturer);
        Assert.Equal("X1", created.Model);
    }

    private sealed class InMemoryBikeRepository : IBikeRepository
    {
        private readonly List<Bike> _bikes;

        public InMemoryBikeRepository(IEnumerable<Bike> seed)
        {
            _bikes = seed.ToList();
        }

        public Task<IReadOnlyList<Bike>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<Bike> result = _bikes.ToList();
            return Task.FromResult(result);
        }

        public Task<Bike?> GetByRefAsync(string bikeRef, CancellationToken cancellationToken = default)
        {
            var bike = _bikes.FirstOrDefault(b => b.Ref == bikeRef);
            return Task.FromResult(bike);
        }

        public Task<Bike> AddAsync(Bike bike, CancellationToken cancellationToken = default)
        {
            _bikes.Add(bike);
            return Task.FromResult(bike);
        }
    }
}
