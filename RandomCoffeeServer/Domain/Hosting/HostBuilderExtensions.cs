namespace RandomCoffeeServer.Domain.Hosting;

public static class HostBuilderExtensions
{
    public static void DisableRedirectOnUnauthorized(this IServiceCollection builder)
    {
        const int unauthorized = 401;
        
        builder.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = unauthorized;
                return Task.CompletedTask;
            };
        });
    } 
}