namespace Sneakers.API.Services;

public interface IDataService
{
    Task SetupData();
}

public class DataService : IDataService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IOccasionRepository _occasionRepository;
    private readonly ISneakerRepository _sneakerRepository;
    private readonly IUserRepository _userRepository;

    public DataService(IBrandRepository brandRepository, IOccasionRepository occasionRepository, ISneakerRepository sneakerRepository, IUserRepository userRepository)
    {
        _brandRepository = brandRepository;
        _occasionRepository = occasionRepository;
        _sneakerRepository = sneakerRepository;
        _userRepository = userRepository;
    }

    public async Task SetupData()
    {
        try
        {
            // Add Brands if none exist
            if (!(await _brandRepository.GetBrands()).Any())
            {
                await _brandRepository.AddBrands(new List<Brand>()
                {
                    new Brand() { Name = "ASICS" },
                    new Brand() { Name = "CONVERSE" },
                    new Brand() { Name = "JORDAN" },
                    new Brand() { Name = "PUMA" }
                });
            }

            // Add Occasions if none exist
            if (!(await _occasionRepository.GetOccasions()).Any())
            {
                await _occasionRepository.AddOccasions(new List<Occasion>()
                {
                    new Occasion() { Description = "Sports" },
                    new Occasion() { Description = "Casual" },
                    new Occasion() { Description = "Skate" },
                    new Occasion() { Description = "Diner" }
                });
            }

            // Add Sneakers if none exist
            if (!(await _sneakerRepository.GetSneakers()).Any())
            {
                var occasions = await _occasionRepository.GetOccasions();
                var brands = await _brandRepository.GetBrands();
                var asicsBrand = brands.FirstOrDefault(b => b.Name == "ASICS");

                await _sneakerRepository.AddSneaker(new Sneaker()
                {
                    Name = "GEL-KAYANO 14",
                    Price = 150,
                    Stock = 10,
                    Brand = asicsBrand,
                    Occasions = new List<Occasion>() { occasions[0], occasions[1] }
                });
                // Repeat similarly for the other sneakers
                await _sneakerRepository.AddSneaker(new Sneaker()
                {
                    Name = "GEL-KAYANO 15",
                    Price = 160,
                    Stock = 10,
                    Brand = asicsBrand,
                    Occasions = new List<Occasion>() { occasions[0], occasions[1] }
                });
                await _sneakerRepository.AddSneaker(new Sneaker()
                {
                    Name = "GEL-KAYANO 16",
                    Price = 170,
                    Stock = 10,
                    Brand = asicsBrand,
                    Occasions = new List<Occasion>() { occasions[0], occasions[1] }
                });
                await _sneakerRepository.AddSneaker(new Sneaker()
                {
                    Name = "GEL-KAYANO 17",
                    Price = 180,
                    Stock = 10,
                    Brand = asicsBrand,
                    Occasions = new List<Occasion>() { occasions[0], occasions[1] }
                });
                await _sneakerRepository.AddSneaker(new Sneaker()
                {
                    Name = "GEL-KAYANO 18",
                    Price = 190,
                    Stock = 10,
                    Brand = asicsBrand,
                    Occasions = new List<Occasion>() { occasions[0], occasions[1] }
                });
                await _sneakerRepository.AddSneaker(new Sneaker()
                {
                    Name = "GEL-KAYANO 19",
                    Price = 200,
                    Stock = 10,
                    Brand = asicsBrand,
                    Occasions = new List<Occasion>() { occasions[0], occasions[1] }
                });
                await _sneakerRepository.AddSneaker(new Sneaker()
                {
                    Name = "GEL-KAYANO 20",
                    Price = 210,
                    Stock = 10,
                    Brand = asicsBrand,
                    Occasions = new List<Occasion>() { occasions[0], occasions[1] }
                });
            }

            // Add User if none exist
            if (!(await _userRepository.GetUsers()).Any())
            {
                User newUser = new User()
                {
                    CustomerNr = 123,
                    Discount = 10,
                    ApiKey = "HelloBias"
                };
                await _userRepository.AddUsers(new List<User> { newUser });
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex}");
            throw;
        }
    }
}