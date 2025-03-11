namespace Sneakers.API.Repositories;

public interface IOccasionRepository
{
    Task<List<Occasion>> GetOccasions();
    Task AddOccasions(List<Occasion> occasions);
}

public class OccasionRepository : IOccasionRepository
{
    private readonly IMongoCollection<Occasion> _occasions;

    public OccasionRepository(IMongoContext context)
    {
        _occasions = context.OccasionCollection;
    }

    public async Task<List<Occasion>> GetOccasions()
    {
        return await _occasions.Find(_ => true).ToListAsync();
    }

    public async Task AddOccasions(List<Occasion> occasions)
    {
        await _occasions.InsertManyAsync(occasions);
    }
}