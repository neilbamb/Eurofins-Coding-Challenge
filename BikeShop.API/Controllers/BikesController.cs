using Microsoft.AspNetCore.Mvc;
using BikeShop.API.Application.Bikes;
using BikeShop.API.Contracts;

namespace BikeShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BikesController : ControllerBase
{
    private readonly BikeAppService _bikeAppService;

    public BikesController(BikeAppService bikeAppService)
    {
        _bikeAppService = bikeAppService;
    }

    [HttpGet]
    public async Task<IReadOnlyList<BikeDto>> Get()
    {
        return await _bikeAppService.GetAllAsync();
    }

    [HttpGet("{bikeRef:required}")]
    public async Task<ActionResult<BikeDto>> GetByRef([FromRoute(Name = "bikeRef")] string bikeRef)
    {
        var bike = await _bikeAppService.GetByRefAsync(bikeRef);
        if (bike is null)
        {
            return NotFound();
        }

        return Ok(bike);
    }

    [HttpPost]
    public async Task<ActionResult<BikeDto>> Create([FromBody] CreateBikeRequest request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        BikeDto created;
        try
        {
            created = await _bikeAppService.CreateAsync(request);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }

        return CreatedAtAction(
            nameof(GetByRef),
            new { bikeRef = created.Ref },
            created);
    }
}