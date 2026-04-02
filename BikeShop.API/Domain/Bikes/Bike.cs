namespace BikeShop.API.Domain.Bikes;

public class Bike
{
    public string Ref { get; set; } = Guid.NewGuid().ToString();

    public string Manufacturer { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Price { get; set; } = string.Empty;

    public string Colour { get; set; } = string.Empty;

    public string Weight { get; set; } = string.Empty;

    public string ImgUrl { get; set; } = string.Empty;
}

