using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Jobs;
using RandomCoffeeServer.Repositories.IdentityStorageProvider;
using RandomCoffeeServer.Services.YandexCloud.Lockbox;

DotEnv.Load("./.env");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders();

var lockboxService = new LockboxFactory().Create(builder.Environment); // todo this is bad!!!
builder.Services
    .AddAuthentication()
    .AddGoogle(o =>
    {
        o.ClientId = lockboxService.GetCoffeeLocalOpenIdId().GetAwaiter().GetResult();
        o.ClientSecret = lockboxService.GetCoffeeLocalOpenIdSecret().GetAwaiter().GetResult();
    });

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((hostBuilder, containerBuilder) =>
{
    containerBuilder.RegisterModule(new RandomCoffeeModule(hostBuilder));
});

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

await app.Services.GetRequiredService<SchemeUpdateJob>().UpdateScheme(app.Lifetime.ApplicationStopping);
await app.Services.GetRequiredService<PopulateWithMockDataJob>().Fill(app.Lifetime.ApplicationStopping);

// app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();