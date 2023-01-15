using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer;
using RandomCoffeeServer.Controllers.ApiControllers;
using RandomCoffeeServer.Controllers.JobControllers;
using RandomCoffeeServer.Controllers.WebControllers;
using RandomCoffeeServer.Domain.Hosting;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;
using RandomCoffeeServer.Storage.YandexCloud.Lockbox;

DotEnv.Load("./.env");

var builder = WebApplication.CreateBuilder(args);
var modes = ArgsParser.GetApplicationModes();

builder.Services.AddControllers().ConfigureApplicationPartManager(manager =>
{
    manager.ApplicationParts.Clear();
    if (modes.HasFlag(ApplicationMode.ApiServer))
    {
        manager.ApplicationParts.Add(new ControllersApplicationPart(ApplicationMode.ApiServer.ToString(), new[]
        {
            typeof(AccountController),
            typeof(GroupsController),
            typeof(UsersController),
            typeof(LoginController)
        }));
    }

    if (modes.HasFlag(ApplicationMode.RoundsMakerJob))
    {
        manager.ApplicationParts.Add(new ControllersApplicationPart(ApplicationMode.RoundsMakerJob.ToString(), new[]
        {
            typeof(RoundMakerJobController)
        }));
    }

    if (modes.HasFlag(ApplicationMode.DatabaseUpdateJob))
    {
        manager.ApplicationParts.Add(new ControllersApplicationPart(ApplicationMode.DatabaseUpdateJob.ToString(), new[]
        {
            typeof(DatabaseJobsController)
        }));
    }
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<IdentityCoffeeUser, IdentityRoleModel>();
builder.Services.DisableRedirectOnUnauthorized();

var lockboxService = new LockboxFactory().Create(builder.Environment); // todo this is bad!!!
builder.Services.AddAuthentication()
    .AddGoogle(o =>
    {
        o.ClientId = lockboxService.GetCoffeeLocalOpenIdId().GetAwaiter().GetResult();
        o.ClientSecret = lockboxService.GetCoffeeLocalOpenIdSecret().GetAwaiter().GetResult();

        o.ClaimActions.MapJsonKey("image", "picture");
        o.CorrelationCookie.SameSite = SameSiteMode.Unspecified;

        o.SaveTokens = true;
    });


builder.Services.AddCoffeeHostedServices(modes);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((hostBuilder, containerBuilder) =>
{
    containerBuilder.RegisterModule(
        new RandomCoffeeModule(hostBuilder.HostingEnvironment, hostBuilder.Configuration));
});

builder.Services.AddDataProtection().PersistKeysToYdb(); // тут должен быть еще ProtectKeys...

if (!builder.Environment.IsDevelopment())
{
    if (Environment.GetEnvironmentVariable("PORT") is not { } port)
        throw new Exception("Required PORT environment variable not found");

    builder.WebHost.UseUrls($"http://*:{port}");
}
else
{
    builder.WebHost.UseUrls($"http://*:5678");
}

builder.Services.AddLogging();

var app = builder.Build();

// await app.Services.GetRequiredService<SchemeUpdateJob>().UpdateScheme(app.Lifetime.ApplicationStopping);
// await app.Services.GetRequiredService<PopulateWithMockDataJob>().Fill(app.Lifetime.ApplicationStopping);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();