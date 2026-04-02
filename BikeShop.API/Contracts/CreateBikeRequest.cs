using System.Text.Json.Serialization;

namespace BikeShop.API.Contracts;

public record CreateBikeRequest(
    string Manufacturer,
    string Model,
    string Category,
    string Price,
    string Colour,
    string Weight,
    [property: JsonPropertyName("img_url")] string ImgUrl
);

