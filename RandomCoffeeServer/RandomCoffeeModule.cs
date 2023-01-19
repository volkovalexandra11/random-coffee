using Autofac;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Hosting.Jobs;
using RandomCoffeeServer.Domain.Services.Coffee;
using RandomCoffeeServer.Domain.Services.Coffee.Email;
using RandomCoffeeServer.Domain.Services.Coffee.Rounds;
using RandomCoffeeServer.Domain.Services.Coffee.UserMatching;
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

        RegisterConfiguration<EmailSettings>(builder);
        builder.RegisterType<RandomUserMatcher>().As<IUserMatcher>();
        builder.RegisterType<EmailMatchNotifier>().As<IMatchNotifier>();
        builder.RegisterType<MailMessageProvider>().SingleInstance();
        builder.RegisterType<SmtpClientFactory>().SingleInstance();
        builder.Register(ctx =>
                ctx.Resolve<SmtpClientFactory>().Create().GetAwaiter().GetResult())
            .SingleInstance();
        builder.RegisterType<EmailSender>().SingleInstance();

        builder.RegisterType<ScheduledRoundsMakerService>().SingleInstance();
        builder.RegisterType<GroupRoundMakerService>().SingleInstance();

        if (hostEnvironment.IsDevelopment())
        {
            RegisterConfiguration<DevelopmentPeriodicRoundsSettings>(builder);
            builder.RegisterType<MockRoundsService>().SingleInstance();
        }
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<GroupRepository>().SingleInstance();
        builder.RegisterType<GroupUserRepository>().SingleInstance();
        builder.RegisterType<UserRepository>().SingleInstance();
    }

    private void RegisterConfiguration<TSettings>(ContainerBuilder builder)
        where TSettings : class, new()
    {
        var settings = new TSettings();
        configuration.GetSection(typeof(TSettings).Name).Bind(settings);

        builder.RegisterInstance(settings);
    }

    private readonly IHostEnvironment hostEnvironment;
    private readonly IConfiguration configuration;
}