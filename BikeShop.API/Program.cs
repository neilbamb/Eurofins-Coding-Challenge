using BikeShop.API.Application.Bikes;
using BikeShop.API.Domain.Bikes;
using BikeShop.API.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddDbContext<BikesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BikesDb")!));

builder.Services.AddScoped<IBikeRepository, EfBikeRepository>();
builder.Services.AddScoped<BikeAppService>();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BikesDbContext>();
    db.Database.EnsureCreated();
    BikeSeeder.SeedIfEmpty(db);
}

app.UseAuthorization();

app.MapControllers();

app.Run();
