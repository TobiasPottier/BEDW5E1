namespace Sneakers.API.Repositories;

public interface IBrandRepository
{
    Task<List<Brand>> GetBrands();
    Task AddBrands(List<Brand> brands);
}

public class BrandRepository : IBrandRepository
{
    private readonly IMongoCollection<Brand> _brands;

    public BrandRepository(IMongoContext context)
    {
        _brands = context.BrandsCollection;
    }

    public async Task<List<Brand>> GetBrands()
    {
        return await _brands.Find(_ => true).ToListAsync();
    }

    public async Task AddBrands(List<Brand> brands)
    {
        await _brands.InsertManyAsync(brands);
    }
}