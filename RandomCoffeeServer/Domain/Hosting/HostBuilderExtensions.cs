using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Options;
using RandomCoffeeServer.Domain.Hosting.Jobs;

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

    public static IDataProtectionBuilder PersistKeysToYdb(this IDataProtectionBuilder builder)
    {
        builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
        {
            return new ConfigureOptions<KeyManagementOptions>(options =>
            {
                options.XmlRepository = services.GetRequiredService<IXmlRepository>();
            });
        });

        return builder;
    }

    public static void AddCoffeeHostedServices(this IServiceCollection services, ApplicationMode applicationModes)
    {
        if (applicationModes.HasFlag(ApplicationMode.DatabaseUpdateHostedService))
        {
            services.AddHostedService<JobStarter<SchemeUpdateJob>>();
            services.AddHostedService<JobStarter<PopulateWithMockDataJob>>();
        }

        if (applicationModes.HasFlag(ApplicationMode.RoundsMakerHostedService))
        {
            services.AddHostedService<RoundsMakerBackgroundService>();
        }
    }
}