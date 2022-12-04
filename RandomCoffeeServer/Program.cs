using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using RandomCoffee;
using RandomCoffee.Controllers;
using RandomCoffee.CredentialProviders;
using RandomCoffee.Services;

DotEnv.Load("./.env");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddApplicationPart(typeof(TestController).Assembly);
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((hostBuilder, containerBuilder) =>
{
    containerBuilder.RegisterModule(new RandomCoffeeModule(hostBuilder));
});

var cred = await CredentialProviderFactory.GetSa();
builder.Services
    .AddHostedService<SchemeUpdaterJob>();
    // .AddHostedService<PopulateWithMockDataJob>();

new LockboxService();

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

// app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
