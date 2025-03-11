namespace Sneakers.API.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsers();
    Task AddUsers(List<User> users);
    Task<User> GetUserByApiKey(string apiKey);
}

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoContext context)
    {
        _users = context.UsersCollection;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _users.Find(_ => true).ToListAsync();
    }
    public async Task<User> GetUserByApiKey(string apiKey)
    {
        return await _users.Find(user => user.ApiKey == apiKey).FirstOrDefaultAsync();
    }

    public async Task AddUsers(List<User> users)
    {
        await _users.InsertManyAsync(users);
    }
}