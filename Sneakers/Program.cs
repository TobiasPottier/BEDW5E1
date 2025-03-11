using Sneakers.API.Validitors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("MongoConnection"));
builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IOccasionRepository, OccasionRepository>();
builder.Services.AddScoped<ISneakerRepository, SneakerRepository>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<SneakerValidator, SneakerValidator>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "|App is running|");

// Call SetupData on startup
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
    await dataService.SetupData();
}

app.MapControllers();

app.Run("http://localhost:5000");
