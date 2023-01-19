using Autofac;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Hosting.Jobs;
using RandomCoffeeServer.Domain.Services.Coffee;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;
using RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;
using RandomCoffeeServer.Storage.YandexCloud.Lockbox;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.YdbFactory;

namespace RandomCoffeeServer;

public class RandomCoffeeModule : Module
{
    public RandomCoffeeModule(IHostEnvironment hostEnvironment, IConfiguration configuration)
    {
        this.hostEnvironment = hostEnvironment;
        this.configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        RegisterJobs(builder);
        RegisterAuth(builder);
        RegisterServices(builder);
        RegisterRepositories(builder);
    }

    private void RegisterJobs(ContainerBuilder builder)
    {
        builder.RegisterType<SchemeUpdateJob>().SingleInstance();
        builder.RegisterType<PopulateWithMockDataJob>()
            .SingleInstance();
        builder.RegisterType<RoundsMakerJob>()
            .SingleInstance();
    }

    private void RegisterAuth(ContainerBuilder builder)
    {
        builder.RegisterType<IdentityUserInfoStore>().AsSelf().SingleInstance();
        builder.RegisterType<IdentityUserLoginsOnlyStore>().AsSelf().SingleInstance();

        builder.RegisterType<IdentityUserStore>()
            .AsSelf()
            .As<IUserStore<IdentityCoffeeUser>>()
            .As<IUserLoginStore<IdentityCoffeeUser>>()
            .SingleInstance();
        builder.RegisterType<CoffeeRoleStore>()
            .As<IRoleStore<IdentityRoleModel>>()
            .SingleInstance();

        builder.RegisterType<DataProtectionKeysUnprotectedRepository>()
            .As<IXmlRepository>()
            .SingleInstance();
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<LockboxFactory>().SingleInstance();
        builder.Register(
                ctx => ctx.Resolve<LockboxFactory>().Create(hostEnvironment))
            .SingleInstance();

        builder.RegisterType<YdbFactory>().SingleInstance();
        builder.Register(ctx =>
                ctx.Resolve<YdbFactory>().Create(hostEnvironment, ctx.Resolve<ILoggerFactory>()))
            .SingleInstance();

        builder.RegisterType<GroupService>().SingleInstance(); // <=> .AsSelf()
        builder.RegisterType<UserService>().SingleInstance();
        builder.RegisterType<EmailService>().SingleInstance();

        builder.RegisterType<SingleRoundMakerService>().SingleInstance();

        if (hostEnvironment.IsDevelopment())
        {
            if (configuration[CheckPeriodConfigurationName] is not { } checkPeriodSeconds)
                throw new ArgumentException($"Expected {CheckPeriodConfigurationName} configuration");
            builder.RegisterType<MockRoundsService>()
                .WithParameter("checkPeriod", TimeSpan.FromSeconds(int.Parse(checkPeriodSeconds)))
                .SingleInstance();
        }
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<GroupRepository>().SingleInstance();
        builder.RegisterType<GroupUserRepository>().SingleInstance();
        builder.RegisterType<UserRepository>().SingleInstance();
    }

    private readonly IHostEnvironment hostEnvironment;
    private readonly IConfiguration configuration;
    private const string CheckPeriodConfigurationName = "RoundsSettings:CheckEverySeconds";
}