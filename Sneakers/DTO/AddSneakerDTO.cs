namespace Sneakers.API.DTOs;

public class AddSneakerDTO
{
    public string? Name { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? BrandId { get; set; }

    public List<string> OccasionIds { get; set; } = new();
}