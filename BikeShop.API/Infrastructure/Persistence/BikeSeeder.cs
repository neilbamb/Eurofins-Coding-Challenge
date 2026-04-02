using BikeShop.API.Domain.Bikes;
using Microsoft.EntityFrameworkCore;

namespace BikeShop.API.Infrastructure.Persistence;

public static class BikeSeeder
{
    public static void SeedIfEmpty(BikesDbContext dbContext)
    {
        if (dbContext.Bikes.Any())
        {
            return;
        }

        var seededBikes = new List<Bike>
        {
            new Bike { Manufacturer = "Carrera", Ref = "07e9548b-f35e-4e00-99d7-e49b5fb08907", Model = "Karkinos", Category = "Mountain Bike", Price = "€415.00", Colour = "Red", Weight = "14kg", ImgUrl = "/assets/images/bikes/Carrera-Karkinos-Mountain-Bike.png" },
            new Bike { Manufacturer = "Indi", Ref = "62343d00-1503-43fb-9db4-80cd06d24345", Model = "ATB 1", Category = "Mountain Bike", Price = "€156.60", Colour = "Black", Weight = "15.5kg", ImgUrl = "/assets/images/bikes/Indi-ATB-1-Mountain-Bike.png" },
            new Bike { Manufacturer = "Boardman", Ref = "79204036-aa35-405b-8030-f1562d7a4f18", Model = "HYB 8.8", Category = "Hybrid Electric Bike", Price = "€1,020.00", Colour = "Blue", Weight = "17.5kg", ImgUrl = "/assets/images/bikes/Boardman-HYB-8.png" },
            new Bike { Manufacturer = "Carrera", Ref = "6eddfc34-0fbd-42da-afd8-6d52566ab5a1", Model = "Vengeance E", Category = "Hybrid Electric Bike", Price = "€1,319.00", Colour = "Grey", Weight = "18kg", ImgUrl = "/assets/images/bikes/Carrera-Vengeance-E-Electric-Bike.png" },
            new Bike { Manufacturer = "Pendleton", Ref = "8c3c304b-b7ca-4155-af05-e6e29bfe89e0", Model = "Somerby Electric Bike", Category = "Hybrid Electric Bike", Price = "€1,450.00", Colour = "Dark Blue", Weight = "22kg", ImgUrl = "/assets/images/bikes/Pendleton-Somerby-Electric-Bike.png" },
            new Bike { Manufacturer = "X-Rated", Ref = "b53806b2-1774-45e6-b273-68cee9571399", Model = "Shockwave DBS Superleggera", Category = "BMX Bike", Price = "€185.00", Colour = "Orange", Weight = "11kg", ImgUrl = "/assets/images/bikes/X-Rated-Shockwave-BMX-Bike.png" },
            new Bike { Manufacturer = "Voodoo", Ref = "de65dac3-bc15-413c-b9f6-93079c9eba8f", Model = "Zaka", Category = "BMX Bike", Price = "€225.00", Colour = "Blue", Weight = "12.5kg", ImgUrl = "/assets/images/bikes/Voodoo-Zaka-BMX-Bike.png" },
            new Bike { Manufacturer = "Assist", Ref = "501d25e3-6cc0-4d85-a816-b8d25275c4b9", Model = "2021", Category = "Hybrid Electric Bike", Price = "€594.00", Colour = "White", Weight = "18kg", ImgUrl = "/assets/images/bikes/Assist-Hybrid-Electric-Bike.png" },
            new Bike { Manufacturer = "Boardman", Ref = "91d0a29b-f352-47a1-a2b7-1b21f4e8d187", Model = "MHT 8.6", Category = "Mountain Bike", Price = "€760.00", Colour = "Red", Weight = "13.5kg", ImgUrl = "/assets/images/bikes/Boardman-MHT-8.png" },
            new Bike { Manufacturer = "Pendleton", Ref = "863618e2-60fa-4742-8824-9cdd4b16a294", Model = "Somerby", Category = "Hybrid Electric Bike", Price = "€385.00", Colour = "Maroon Red", Weight = "14.6kg", ImgUrl = "/assets/images/bikes/Pendleton-Somerby-Hybrid-Bike.png" },
            new Bike { Manufacturer = "Carrera", Ref = "f69caf5d-ee90-4b62-b5da-bea3f3646949", Model = "Virtuoso", Category = "Road Bike", Price = "€520.00", Colour = "White", Weight = "11.6kg", ImgUrl = "/assets/images/bikes/Carrera-Virtuoso-Road-Bike.png" },
            new Bike { Manufacturer = "Boardman", Ref = "b61bea5b-4eda-4401-bfeb-aafad3f24e8f", Model = "SLR 8.9", Category = "Road Bike", Price = "€2,100.00", Colour = "Black", Weight = "11kg", ImgUrl = "/assets/images/bikes/Boardman-SLR-8.png" }
        };

        dbContext.Bikes.AddRange(seededBikes);
        dbContext.SaveChanges();
    }
}

