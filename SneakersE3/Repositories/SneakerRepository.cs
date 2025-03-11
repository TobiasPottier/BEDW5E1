namespace Sneakers.API.Repositories;

public interface ISneakerRepository
{
    Task<List<Sneaker>> GetSneakers();
    Task AddSneaker(Sneaker sneaker);
    Task<Sneaker?> GetSneaker(string id);
}

public class SneakerRepository : ISneakerRepository
{
    private readonly IMongoCollection<Sneaker> _sneakers;

    public SneakerRepository(IMongoContext context)
    {
        _sneakers = context.SneakerCollection;
    }

    public async Task<List<Sneaker>> GetSneakers()
    {
        return await _sneakers.Find(_ => true).ToListAsync();
    }

    public async Task AddSneaker(Sneaker sneaker)
    {
        await _sneakers.InsertOneAsync(sneaker);
    }

    public async Task<Sneaker?> GetSneaker(string id)
    {
        return await _sneakers.Find(s => s.SneakerId == id).FirstOrDefaultAsync();
    }
}