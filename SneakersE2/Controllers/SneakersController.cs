using Sneakers.API.Validitors;

namespace Sneakers.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SneakersController : ControllerBase
{
    private readonly IBrandRepository _brandRepository;
    private readonly IOccasionRepository _occasionRepository;
    private readonly ISneakerRepository _sneakerRepository;
    private readonly SneakerValidator _sneakerValidator;

    public SneakersController(
        IBrandRepository brandRepository,
        IOccasionRepository occasionRepository,
        ISneakerRepository sneakerRepository,
        SneakerValidator sneakerValidator)
    {
        _brandRepository = brandRepository;
        _occasionRepository = occasionRepository;
        _sneakerRepository = sneakerRepository;
        _sneakerValidator = sneakerValidator;
    }

    // GET: api/sneakers/brands
    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await _brandRepository.GetBrands();
        return Ok(brands);
    }

    // GET: api/sneakers/occasions
    [HttpGet("occasions")]
    public async Task<IActionResult> GetOccasions()
    {
        var occasions = await _occasionRepository.GetOccasions();
        return Ok(occasions);
    }

    // POST: api/sneakers
    [HttpPost]
    public async Task<IActionResult> AddSneaker([FromBody] AddSneakerDTO sneakerDto)
    {
        var validationResult = _sneakerValidator.Validate(sneakerDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        // Retrieve all brands and filter by provided BrandId
        var brands = await _brandRepository.GetBrands();
        var brand = brands.FirstOrDefault(b => b.BrandId == sneakerDto.BrandId);
        if (brand == null)
        {
            return BadRequest("Invalid BrandId provided.");
        }

        // Retrieve all occasions and filter by provided OccasionIds
        var occasions = await _occasionRepository.GetOccasions();
        var sneakerOccasions = occasions
            .Where(o => sneakerDto.OccasionIds.Contains(o.OccasionId))
            .ToList();
        if (sneakerOccasions.Count != sneakerDto.OccasionIds.Count)
        {
            return BadRequest("One or more OccasionIds are invalid.");
        }
        
        var sneaker = new Sneaker
        {
            Name = sneakerDto.Name,
            Price = sneakerDto.Price,
            Stock = sneakerDto.Stock,
            Brand = brand,
            Occasions = sneakerOccasions
        };


        await _sneakerRepository.AddSneaker(sneaker);
        return CreatedAtAction(nameof(GetSneaker), new { id = sneaker.SneakerId }, sneaker);
    }

    // GET: api/sneakers/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSneaker(string id)
    {
        var sneaker = await _sneakerRepository.GetSneaker(id);
        if (sneaker == null)
            return NotFound();
        return Ok(sneaker);
    }
}