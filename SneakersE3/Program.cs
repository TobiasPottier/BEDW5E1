using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sneakers.API.Validitors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("MongoConnection"));
builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IOccasionRepository, OccasionRepository>();
builder.Services.AddScoped<ISneakerRepository, SneakerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<SneakerValidator, SneakerValidator>();
builder.Services.AddControllers();
builder.Services.Configure<ApiKeySettings>(builder.Configuration.GetSection("ApiKeySettings"));


var connectionString = builder.Configuration.GetConnectionString("Server");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(connectionString)));
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
       .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure token validation parameters, e.g.:
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();


var app = builder.Build();

// app.UseMiddleware<ApiKeyMiddleware>();
app.MapIdentityApi<IdentityUser>();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "|App is running|");

// Call SetupData on startup
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
    await dataService.SetupData();
}

app.MapControllers();

app.MapGet("/sneakers", async (IDataService sneakerService, HttpContext context) =>
{
    // Optionally extract user information from HttpContext.User
    // return await sneakerService.GetSneakers(/* pass user info if needed */);
}).RequireAuthorization();

app.Run("http://localhost:5002");
