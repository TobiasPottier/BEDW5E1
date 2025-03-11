namespace Sneakers.API.Middleware;


public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER = "XApiKey";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the header is present
        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided");
            return;
        }

        // Create a scope to resolve scoped services (like IUserRepository)
        using (var scope = context.RequestServices.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            // Check if a user with this API key exists
            var user = await userRepository.GetUserByApiKey(extractedApiKey);
            if (user == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client: invalid API key");
                return;
            }

            // Attach the user object (or just the discount, if you prefer) to the HttpContext.
            context.Items["User"] = user;
            // For example, you could also do:
            // context.Items["Discount"] = user.Discount;
        }

        await _next(context);
    }
}